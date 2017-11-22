// ReSharper disable InconsistentNaming

using System;

namespace EN5Player
{
    /// <summary>
    /// Configuration information of this application.
    /// </summary>
    internal static class Configuration
    {
        /// <summary>
        /// EN5Player
        /// </summary>
        public static string PlayerName = "EN5Player";

        /// <summary>
        /// Description of EN5Player
        /// </summary>
        public static string PlayerDescription =
            "EN5Player is designed to show *.enbx file directly without the installation of EasiNote."
            + Environment.NewLine +
            "More about EasiNote5: http://easinote.seewo.com.";

        /// <summary>
        /// Simple description of EN5Player.
        /// </summary>
        public static string PlayerSimpleDescription = "EN5Player for EasiNote5";

        /// <summary>
        /// Copyright of EN5Player
        /// </summary>
        public static string PlayerCopyright = "All Copyright (c) 2017 Iron.";

        /// <summary>
        /// Password used to zip or extract.
        /// </summary>
        public static string PlayerPassword = "iron.yehong";

        /// <summary>
        /// App name of EasiNote5
        /// </summary>
        public static string EN5AppName = "EasiNote5";

        /// <summary>
        /// Seewo\\EasiNote5
        /// </summary>
        public static string EN5RegistryKey = "Seewo\\EasiNote5";

        /// <summary>
        /// VersionPath
        /// </summary>
        public static string EN5RegisteryPathKey = "VersionPath";

        /// <summary>
        /// version
        /// </summary>
        public static string EN5VersionKey = "version";

        /// <summary>
        /// The extension of an EasiNote5 file
        /// </summary>
        public static string EN5FileExtension = ".enbx";

        /// <summary>
        /// The extension of an executable file.
        /// </summary>
        public static string ExeFileExtension = ".exe";

        /// <summary>
        /// File name of EasiNote5 file icon.
        /// </summary>
        public static string EN5FileIconName = "enb_file.ico";

        /// <summary>
        /// The launcher of EasiNote
        /// </summary>
        public static string EN5LauncherName = "\\Main\\EasiNote.Cloud.exe";
    }
}