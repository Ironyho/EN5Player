using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace EN5Player
{
    /// <summary>
    /// Interaction logic for AboutDilog.xaml
    /// </summary>
    public partial class AboutDilog
    {
        public AboutDilog()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(WebsitRun.Text);
        }
    }
}