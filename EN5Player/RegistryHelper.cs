using Microsoft.Win32;

namespace EN5Player
{
    /// <summary>
    /// Helper class for reading or writting OS <seealso cref="Registry"/>.
    /// </summary>
    internal static class RegistryHelper
    {
        /// <summary>
        /// Get the value of the specified key and item in the HKEY_LOCAL_MACHINE\SOFTWARE.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns>
        /// The data in HKEY_LOCAL_MACHINE\SOFTWARE\{key}\{item} of 32bit OS,
        /// or in HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\{key}\{item} of 64bit OS.
        /// If no such a key exists in the registry, <seealso cref="string.Empty"/> will be returned.
        /// </returns>
        public static string GetValueFromLocalMachineSoftware(string key, string item)
        {
            // 32bit
            var name = $"SOFTWARE\\{key}";
            var subKey = Registry.LocalMachine.OpenSubKey(name, false);

            // 64bit
            if (subKey == null)
            {
                name = $"SOFTWARE\\WOW6432Node\\{key}";
                subKey = Registry.LocalMachine.OpenSubKey(name, false);
            }

            return subKey?.GetValue(item).ToString() ?? string.Empty;
        }
    }
}