using System.Windows;

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
        }

        private void FilePanel_OnDragEnter(object sender, DragEventArgs e)
        {
            SaceFileImage(1.2);
        }

        private void FilePanel_OnDragLeave(object sender, DragEventArgs e)
        {
            SaceFileImage();
        }

        private void FilePanel_OnDrop(object sender, DragEventArgs e)
        {
            SaceFileImage();
        }

        private void SaceFileImage(double scale = 1)
        {
            ImageTransform.ScaleX = scale;
            ImageTransform.ScaleY = scale;
        }
    }
}