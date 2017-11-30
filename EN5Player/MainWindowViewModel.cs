using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EN5Player.Annotations;
using Microsoft.Win32;

namespace EN5Player
{
    /// <summary>
    /// ViewModel of the <see cref="MainWindow"/>.
    /// </summary>
    internal sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        public static MainWindowViewModel Instance { get; } = new MainWindowViewModel();

        public ICommand SelectEnbxFileCommand { get; }

        public ICommand SaveToExeFileCommand { get; }

        public ICommand ExitCommand { get; }

        public ICommand FeedbackCommand { get; }

        public ICommand HelpCommand { get; }

        public ICommand AboutCommand { get; }

        public Action<State> StateUpdated;

        /// <summary>
        /// Subscribe this event to get wrapping progrocess.
        /// </summary>
        public EventHandler<WrapProgressEventArgs> Progressing;

        public event PropertyChangedEventHandler PropertyChanged;

        private MainWindowViewModel()
        {
            SelectEnbxFileCommand = new RelayCommand(o => SelectEnbxFile(), o => !_isWrapping);
            SaveToExeFileCommand = new RelayCommand(o => SaveToExeFile(), o => HasEnbxFile() && !_isWrapping);
            ExitCommand = new RelayCommand(o => Application.Current.Shutdown(0), o => !_isWrapping);

            HelpCommand = new RelayCommand(o => Help(), o => true);
            FeedbackCommand = new RelayCommand(o => Feedback(), o => true);
            AboutCommand = new RelayCommand(o => About(), o => true);

            EN5Wrapper.Progressing += (s, e) =>
            {
                _isWrapping = e.Percent != 0 && e.Percent != 100;
                Progressing?.Invoke(this, e);
            };
        }

        public string EnbxFileName
        {
            get => _enbxFileName;
            set
            {
                var state = State.DragEnbxFile;
                if (!string.IsNullOrEmpty(value))
                {
                    if (!value.EndsWith(Configuration.EN5FileExtension))
                    {
                        value = string.Empty;
                        state = State.EnbxExpected;
                    }
                    else if (!File.Exists(value))
                    {
                        value = string.Empty;
                        state = State.FileNotFound;
                    }
                    else
                    {
                        state = State.FileSelected;
                    }
                }

                _enbxFileName = value;

                OnStateUpdated(state);
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void GenerateExeFileAsync(string outputFileName)
        {
            Task.Run(() =>
            {
                try
                {
                    EN5Wrapper.WrapToExe(EnbxFileName, outputFileName);
                }
                catch (NotInstalledException)
                {
                    OnStateUpdated(State.En5NotInstall);
                }
                catch (Exception)
                {
                    OnStateUpdated(State.Exception);
                }
            });
        }

        private bool HasEnbxFile()
        {
            return !string.IsNullOrEmpty(EnbxFileName);
        }

        private void SelectEnbxFile()
        {
            var enbx = Configuration.ExeFileExtension;
            var openFileDialog = new OpenFileDialog
            {
                Filter = $"*{enbx} (EasiNote5 File)|*{enbx}"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                EnbxFileName = openFileDialog.FileName;
            }
        }

        private void SaveToExeFile()
        {
            var enbx = Configuration.EN5FileExtension;
            var exe = Configuration.ExeFileExtension;

            var fileInfo = new FileInfo(EnbxFileName);
            if (!fileInfo.Exists || fileInfo.Extension != enbx)
            {
                return;
            }

            var directory = fileInfo.Directory?.FullName;
            if (string.IsNullOrEmpty(directory))
            {
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = $"*{exe} (Executable File)|*{exe}",
                InitialDirectory = directory,
                FileName = fileInfo.Name.Replace(enbx, exe)
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    GenerateExeFileAsync(saveFileDialog.FileName);
                }
                catch (FileNotFoundException)
                {
                    OnStateUpdated(State.FileNotFound);
                }
                catch (FileFormatException)
                {
                    OnStateUpdated(State.EnbxExpected);
                }
            }
        }

        private void Help()
        {
            Process.Start(Configuration.HelpUrl);
        }

        private void Feedback()
        {
            Process.Start(Configuration.HelpUrl);
        }

        private void About()
        {
            new AboutDilog() {Owner = Application.Current.MainWindow}.ShowDialog();
        }

        private void OnStateUpdated(State state)
        {
            StateUpdated?.Invoke(state);
        }

        private string _enbxFileName;
        private bool _isWrapping;
    }

    public enum State
    {
        [Description("需要安装 EasiNote5 才能使用此工具")] En5NotInstall,

        [Description("文件格式不正确，只能处理 enbx 格式的文件")] EnbxExpected,
        
        [Description("文件不存在")] FileNotFound,

        [Description("发生未知异常")] Exception,

        [Description("请拖放或选择 *.enbx 文件")] DragEnbxFile,

        [Description("已选择文件")] FileSelected,

        [Description("正在生成 exe 文件")] Wrapping,

        [Description("成功生成 exe 文件")] Wrapped
    }
}