// ReSharper disable All

using System;
using System.IO;
using System.Text;

namespace EN5Player
{
    /// <summary>
    /// Providing methods to launch EasiNote5.
    /// </summary>
    internal static class EN5Launcher
    {
        /// <summary>
        /// Generate a launcher that can start the EasiNote5 wiht a *.enbx file.
        /// </summary>
        /// <returns>The filename of the launcher.</returns>
        public static string GenerateLauncher(string en5EntryFileName)
        {
            // ensure directory
            var appdataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var playerDirectory = Path.Combine(appdataDirectory, AppInfo.Current.Name);

            if (!Directory.Exists(playerDirectory))
            {
                Directory.CreateDirectory(playerDirectory);
            }

            // write the launcher to file
            var fileName = Path.Combine(playerDirectory, Configuration.EN5LauncherName);
            var content = GetLauncherContent(en5EntryFileName);

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            File.AppendAllText(fileName, content);

            return fileName;
        }

        private static string GetLauncherContent(string entryFileName)
        {
            var builder = new StringBuilder();

            builder.AppendLine("@echo off");
            builder.Append(Environment.NewLine);

            builder.AppendLine("echo Launch EasiNote5...");
            builder.AppendLine($"echo Entry: {entryFileName}");
            builder.AppendLine($"echo FileName: %1");

            builder.AppendLine($"start {entryFileName} %1 -m Display");
            builder.Append(Environment.NewLine);

            builder.AppendLine("echo exit");
            builder.AppendLine("exit");

            return builder.ToString();
        }
    }
}