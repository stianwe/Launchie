
using System;
using System.Configuration;
using Launchie;
using Version = Launchie.Version;

namespace Server
{
    class Program
    {
        public static readonly Version Version = new Version("0.0.1");

        private const string RootDirKey = "RootDir";
        private const string HashServerPortKey = "HashServerPort";
        private const string FileServerPortKey = "FileServerPort";
        private const string HashServerClientLimitKey = "HashServerClientLimit";
        private const string FileServerClientLimitKey = "FileServerClientLimit";

        private static readonly Logger _logger = new Logger();

        static void Main(string[] args)
        {
            var rootDir = ConfigurationManager.AppSettings[RootDirKey];
            _logger.Log("Launchie server v" + Version + ".", Logger.LogLevel.Medium);
            var hashServer = new HashServer(rootDir, 
                Int32.Parse(ConfigurationManager.AppSettings[HashServerPortKey]),
                Int32.Parse(ConfigurationManager.AppSettings[HashServerClientLimitKey]));
			hashServer.Start ();

			var fileServer = new FileServer(rootDir,
                Int32.Parse(ConfigurationManager.AppSettings[FileServerPortKey]),
                Int32.Parse(ConfigurationManager.AppSettings[FileServerClientLimitKey]));
			fileServer.Start ();
        }
    }
}
