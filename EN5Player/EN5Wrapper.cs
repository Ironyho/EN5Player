using System;
using System.IO;
using Ionic.Zip;

namespace EN5Player
{
    // ReSharper disable once InconsistentNaming
    internal static class EN5Wrapper
    {
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

            // file icon
            var fileIcon = string.Empty;
            var directoryInfo = new DirectoryInfo(directory);

            if (directoryInfo.Parent != null)
            {
                fileIcon = Path.Combine(directoryInfo.Parent.FullName, Configuration.EN5FileIconName);
            }

            // 3. zip the Working Directory and the *.enbx file
            using (var zip = new ZipFile())
            {
                //zip.Comment = AppInfo.Current.Description;
                //zip.Password = Configuration.Password;

                zip.AddFile(enbxFileName, "");
                zip.AddDirectory(directory, version);

                var options = new SelfExtractorSaveOptions
                {
                    Flavor = SelfExtractorFlavor.ConsoleApplication,
                    RemoveUnpackedFilesAfterExecute = true
                };

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
                var extractDirectory = $"%APPDATA%\\{AppInfo.Current.Name}";
                options.DefaultExtractDirectory = extractDirectory;
                options.PostExtractCommandLine = $"{extractDirectory}\\{version}\\{Configuration.EN5LauncherName}";
                options.ExtractExistingFile = ExtractExistingFileAction.DoNotOverwrite;

                // zip
                zip.SaveSelfExtractor(outputFileName, options);
            }
        }
    }
}