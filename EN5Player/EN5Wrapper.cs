using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace EN5Player
{
    // ReSharper disable once InconsistentNaming
    internal static class EN5Wrapper
    {
        /// <summary>
        /// Subscribe this event to get wrapping progrocess.
        /// </summary>
        public static EventHandler<WrapProgressEventArgs> Progressing;

        /// <summary>
        /// Wrap the specified *.enbx file together with the working directory of EasiNote5 into a single executable file.
        /// </summary>
        /// <param name="enbxFileName">The specified *.enbx file that to be wrapped.</param>
        /// <param name="outputFileName">The file name of the single executable file.</param>
        public static void WrapToExe(string enbxFileName, string outputFileName)
        {
            // 1. verify parameters
            if (!File.Exists(enbxFileName))
            {
                throw new FileNotFoundException(enbxFileName);
            }
            if (Path.GetExtension(enbxFileName) != Configuration.EN5FileExtension)
            {
                throw new FormatException($"*.{Configuration.EN5FileExtension} was expected.");
            }

            if (Path.GetExtension(outputFileName) != Configuration.ExeFileExtension)
            {
                outputFileName += Configuration.ExeFileExtension;
            }

            // 2. get the information about EasiNote5
            var version = EN5Locator.GetVersion();
            var directory = EN5Locator.GetWorkingDirectory();

            if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory) || string.IsNullOrEmpty(version))
            {
                throw new NotInstalledException(Configuration.EN5AppName);
            }

            // launcher
            var extractDirectory = $"%APPDATA%\\{AppInfo.Current.Name}";

            var entryFileNameInExtractDirectory = $"{extractDirectory}\\{version}\\{Configuration.EN5Entry}";
            var enbxFileNameInExtractDirectory = $"{extractDirectory}\\{Path.GetFileName(enbxFileName)}";

            var launcher = EN5Launcher.GenerateLauncher(entryFileNameInExtractDirectory);
            var dotNetInstaller = $"{extractDirectory}\\{version}\\{Configuration.DotNetInstaller}";

            // 3. zip the Working Directory and the *.enbx file
            using (var zip = new ZipFile())
            {
                //zip.Comment = AppInfo.Current.Description;
                //zip.Password = Configuration.Password;
                zip.SaveProgress += Zip_SaveProgress;

                zip.AlternateEncoding = Encoding.UTF8;
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;

                zip.AddFile(launcher, $"{AppInfo.Current.Version}");
                zip.AddFile(enbxFileName, "");
                zip.AddDirectory(directory, version);

                var options = new SelfExtractorSaveOptions
                {
                    Flavor = SelfExtractorFlavor.ConsoleApplication,
                    //RemoveUnpackedFilesAfterExecute = true
                };

                var fileIcon = $"{AppDomain.CurrentDomain.BaseDirectory}\\{Configuration.EN5FileIconName}";
                if (!string.IsNullOrEmpty(fileIcon) && File.Exists(fileIcon))
                {
                    options.IconFile = fileIcon;
                }

                // exe file info
                options.ProductName = AppInfo.Current.Name;
                options.Description = AppInfo.Current.Description;
                options.Copyright = AppInfo.Current.Copyright;
                options.ProductVersion = AppInfo.Current.Version; // player version
                options.FileVersion = new Version(version); // EasiNote5 verion

                // extract
                var launcherInExtract = $"{extractDirectory}\\{AppInfo.Current.Version}\\{Path.GetFileName(launcher)}";
                var parameters = $"\"{enbxFileNameInExtractDirectory}\" \"{dotNetInstaller}\"";

                options.DefaultExtractDirectory = extractDirectory;
                options.PostExtractCommandLine = $"{launcherInExtract} {parameters}";
                options.ExtractExistingFile = ExtractExistingFileAction.DoNotOverwrite;

                // zip
                zip.SaveSelfExtractor(outputFileName, options);
            }
        }

        private static void Zip_SaveProgress(object sender, SaveProgressEventArgs e)
        {
            if (e.EventType == ZipProgressEventType.Saving_AfterWriteEntry)
            {
                OnProgress(e.EntriesSaved * 100 / e.EntriesTotal);
            }
        }

        private static void OnProgress(int percent)
        {
            Progressing?.Invoke(null, new WrapProgressEventArgs(percent));
        }
    }

    /// <summary>
    /// Provide data for the <seealso cref="EN5Wrapper.Progressing"/> event.
    /// </summary>
    internal class WrapProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Get the percent of the wrapping progress.
        /// </summary>
        public int Percent { get; }

        /// <summary>
        /// WrapProgressEventArgs constructor
        /// </summary>
        /// <param name="percent"></param>
        public WrapProgressEventArgs(int percent)
        {
            Percent = percent;
        }
    }
}