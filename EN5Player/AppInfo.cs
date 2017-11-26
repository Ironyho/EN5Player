using System;
using System.Linq;
using System.Reflection;

namespace EN5Player
{
    /// <summary>
    /// Informaton of current application
    /// </summary>
    public class AppInfo
    {
        /// <summary>
        /// Get the <seealso cref="AppInfo"/> of current executing assembly.
        /// </summary>
        public static AppInfo Current { get; }

        /// <summary>
        /// Static constructor
        /// </summary>
        static AppInfo()
        {
            Current = new AppInfo(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assembly"></param>
        public AppInfo(Assembly assembly)
        {
            _assembly = assembly;
            _assemblyName = assembly.GetName();
        }

        /// <summary>
        /// Get the full name of the application.
        /// </summary>
        public string FileName => _assembly.Location;

        /// <summary>
        /// Get the name of current application.
        /// </summary>
        public string Name => _assemblyName.Name;

        /// <summary>
        /// Get the version of current application.
        /// </summary>
        public string Version => _assemblyName.Version.ToString();

        /// <summary>
        /// Get the description of current application.
        /// </summary>
        public string Description => GetAttribute<AssemblyDescriptionAttribute>()?.Description;

        /// <summary>
        /// Get the copyright.
        /// </summary>
        public string Copyright => GetAttribute<AssemblyCopyrightAttribute>()?.Copyright;

        private T GetAttribute<T>() where T : Attribute
        {
            return _assembly.GetCustomAttributes(typeof(T)).OfType<T>().FirstOrDefault();
        }

        private readonly Assembly _assembly;
        private readonly AssemblyName _assemblyName;
    }
}