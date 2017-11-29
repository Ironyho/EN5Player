using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using EN5Player;
using Ionic.Zip;

// ReSharper disable InconsistentNaming

namespace EN5PlayerSetup
{
    class Program
    {
        static void Main()
        {
            PackageEN5Player();
        }

        private static void PackageEN5Player()
        {
            // find EN5Player.exe and dependencies
            var filesToPackage = new List<string>();
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var thisName = $"{Assembly.GetExecutingAssembly().GetName().Name}.exe";
            foreach (var file in Directory.EnumerateFiles(baseDirectory))
            {
                if (file.EndsWith($"{thisName}"))
                {
                    continue;
                }

                if (file.EndsWith(".exe") || file.EndsWith(".dll") || file.EndsWith(".ico"))
                {
                    filesToPackage.Add(file);
                }
            }

            // package them together into a single executable file
            using (var zip = new ZipFile())
            {
                foreach (var file in filesToPackage)
                {
                    zip.AddFile(file, "");
                }

                var options = new SelfExtractorSaveOptions
                {
                    Flavor = SelfExtractorFlavor.ConsoleApplication,
                    Quiet = true
                };

                // exe file info
                var appInfo = new AppInfo(typeof(AppInfo).Assembly);
                var launcherName = $"{appInfo.Name}.exe";

                options.IconFile = $"{baseDirectory}\\icon.ico";
                options.ProductName = appInfo.Name;
                options.Description = appInfo.Description;
                options.Copyright = appInfo.Copyright;
                options.ProductVersion = appInfo.Version;
                options.FileVersion = Version.Parse(appInfo.Version);

                // extract
                options.DefaultExtractDirectory = $"%APPDATA%\\{appInfo.Name}";
                options.PostExtractCommandLine = $"%APPDATA%\\{appInfo.Name}\\{launcherName}";
                options.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;

                // zip
                var package = $"{baseDirectory}\\Package";
                if (!Directory.Exists(package))
                {
                    Directory.CreateDirectory(package);
                }

                zip.SaveSelfExtractor($"{package}\\{launcherName}", options);
            }
        }
    }
}