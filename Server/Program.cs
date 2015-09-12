
using System;
using System.Configuration;
using Launchie;
using Version = Launchie.Version;

namespace Server
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    internal class Program
    {
        public static readonly Version Version = new Version("0.0.1");

        private const string RootDirKey = "RootDir";
        private const string HashServerPortKey = "HashServerPort";
        private const string FileServerPortKey = "FileServerPort";
        private const string HashServerClientLimitKey = "HashServerClientLimit";
        private const string FileServerClientLimitKey = "FileServerClientLimit";

        private static readonly Logger _logger = new Logger();

        private static HashServer _hashServer;

        private static void Main(string[] args)
        {
            var rootDir = ConfigurationManager.AppSettings[RootDirKey];
            _logger.Log("Launchie server v" + Version + ".", Logger.LogLevel.Medium);
            _hashServer = new HashServer(rootDir,
                Int32.Parse(ConfigurationManager.AppSettings[HashServerPortKey]),
                Int32.Parse(ConfigurationManager.AppSettings[HashServerClientLimitKey]));
            _hashServer.Start();

            var fileServer = new FileServer(rootDir,
                Int32.Parse(ConfigurationManager.AppSettings[FileServerPortKey]),
                Int32.Parse(ConfigurationManager.AppSettings[FileServerClientLimitKey]));
            fileServer.Start();

            new Thread(ListenForReHashStarter).Start();
        }

        private static void ListenForReHashStarter()
        {
            var logger = new Logger("ReHashStarterListener");
            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 13342);
            listener.Start();
            while (true)
            {
                logger.Log("Listening for ReHashStarter..", Logger.LogLevel.Medium);
                //var listener = new TcpListener(13342); // TODO Use config
                listener.AcceptSocket();
                // Connecting is all it takes to start rehashing
                logger.Log("Triggering re-hashing!", Logger.LogLevel.Medium);
                _hashServer.ReHash();
            }
        }
    }
}
