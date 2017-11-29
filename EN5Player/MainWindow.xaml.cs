using System.Linq;
using System.Windows;
using System.ComponentModel;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace EN5Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            Title = $"{AppInfo.Current.Name} {AppInfo.Current.Version}";

            MainWindowViewModel.Instance.StateUpdated += state =>
            {
                UpdateState(state);
            };

            MainWindowViewModel.Instance.Progressing += (s, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (e.Percent == 100)
                    {
                        UpdateState(State.Wrapped);
                    }
                    else
                    {
                        UpdateState(State.Wrapping, $"{e.Percent}%");
                    }
                });
            };
        }

        private void FilePanel_OnDragEnter(object sender, DragEventArgs e)
        {
            SaceFileImage(1.2);
            SetEnbxFileNameFromDragDrop(e.Data);
        }

        private void FilePanel_OnDragLeave(object sender, DragEventArgs e)
        {
            SaceFileImage();
            SetEnbxFileName(string.Empty);
        }

        private void FilePanel_OnDrop(object sender, DragEventArgs e)
        {
            SaceFileImage();
            SetEnbxFileNameFromDragDrop(e.Data);
        }

        private void SaceFileImage(double scale = 1)
        {
            ImageTransform.ScaleX = scale;
            ImageTransform.ScaleY = scale;
        }

        private void UpdateState(State state, string detail = "")
        {
            var isWarn = state <= State.Exception;
            if (string.IsNullOrEmpty(detail) && state == State.FileSelected)
            {
                detail = MainWindowViewModel.Instance.EnbxFileName;
            }

            var message = GetEnumDescription(state);
            var text = string.IsNullOrEmpty(detail) ? message : $"{message} - {detail}";

            Dispatcher.Invoke(() =>
            {
                StateText.Text = text;
                StateText.Foreground = isWarn ? Brushes.OrangeRed : Brushes.DarkGray;
            });
        }

        private static void SetEnbxFileNameFromDragDrop(IDataObject data)
        {
            var files = data.GetData(DataFormats.FileDrop) as string[];
            if (files != null && files.Any())
            {
                SetEnbxFileName(files.First());
            }
        }

        private static void SetEnbxFileName(string fileName)
        {
            MainWindowViewModel.Instance.EnbxFileName = fileName;
            CommandManager.InvalidateRequerySuggested();
        }

        private static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[]) field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }

    public class StringToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value?.ToString()) ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToInvisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value?.ToString()) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}