using System;

namespace EN5Player
{
    public class NotInstalledException : Exception
    {
        public string AppName { get; }

        public NotInstalledException()
        {
        }

        public NotInstalledException(string appName)
        {
            AppName = appName;
        }

        public override string Message => $"{AppName} not installed.";
    }
}