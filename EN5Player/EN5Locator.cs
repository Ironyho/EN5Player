namespace EN5Player
{
    /// <summary>
    /// Locator that helps to find the installation information about EasiNote5.
    /// More about EasiNote5: http://easinote.seewo.com.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    internal class EN5Locator
    {
        /// <summary>
        /// Get the working directory of EasiNote5 according to the installation information from the OS regitry.
        /// </summary>
        /// <returns>
        /// The working directory of EasiNote5, <seealso cref="string.Empty"/> will be returned if failed to find.
        /// </returns>
        public static string GetWorkingDirectory()
        {
            var position = Configuration.EN5RegistryKey;
            var name = Configuration.EN5RegisteryPathKey;

            return RegistryHelper.GetValueFromLocalMachineSoftware(position, name);
        }

        /// <summary>
        /// Get the working directory of EasiNote5 according to the installation information from the OS regitry.
        /// </summary>
        /// <returns>
        /// The version of EasiNote5, <seealso cref="string.Empty"/> will be returned if failed to find.
        /// </returns>
        public static string GetVersion()
        {
            var position = Configuration.EN5RegistryKey;
            var name = Configuration.EN5VersionKey;

            return RegistryHelper.GetValueFromLocalMachineSoftware(position, name);
        }
    }
}