
using Launchie;

namespace Server
{
    class Program
    {
        public static readonly Version Version = new Version("0.0.1");

		public const string RootDir = "C:/Users/Stian/Desktop/TEST";

        private static readonly Logger _logger = new Logger();

        static void Main(string[] args)
        {
            _logger.Log("Launchie server v" + Version + ".", Logger.LogLevel.Medium);
			var hashServer = new HashServer (RootDir);
			hashServer.Start ();

			var fileServer = new FileServer (RootDir);
			fileServer.Start ();
        }
    }
}
