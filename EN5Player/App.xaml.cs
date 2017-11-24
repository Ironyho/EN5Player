using System;
using System.IO;
using System.Windows;

namespace EN5Player
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // just for test
            if (Environment.MachineName == "IRON-PC")
            {
                const string directory = @"C:\Users\Iron\Desktop";

                var enbxFileName = $@"{directory}\1234.enbx";
                var outputFileName = $@"{directory}\Test.exe";

                if (File.Exists(enbxFileName))
                {
                    EN5Wrapper.WrapToExe(enbxFileName, outputFileName);
                }
            }
        }
    }
}