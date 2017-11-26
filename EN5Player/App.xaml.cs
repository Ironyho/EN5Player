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

#if DEBUG // just for test

            //if (Environment.MachineName == "IRON-PC" || Environment.MachineName == "SURFACE-YEHONG")
            //{
            //    var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //    var enbxFileName = $@"{desktop}\1234.enbx";
            //    var outputFileName = $@"{desktop}\enbx1.exe";

            //    if (File.Exists(enbxFileName))
            //    {
            //        EN5Wrapper.WrapToExe(enbxFileName, outputFileName);
            //    }
            //}

#endif
        }
    }
}