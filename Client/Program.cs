using System;
using System.Diagnostics;
using Launchie;
using Version = System.Version;

namespace Client
{
	class MainClass
    {
        public static readonly Version Version = new Version("0.0.1");

        public const string ProgramToRun = "PedalTanks.exe";

	    public const string RootPath = "";

        private static readonly Logger _logger = new Logger();

		public static void Main (string[] args)
		{
            _logger.Log("Launchie v" + Version + ".", Logger.LogLevel.Medium);
            _logger.Log("Creating HashDownloader..", Logger.LogLevel.Verbose);
            var hashDownloader = new HashDownloader (RootPath);
            _logger.Log("Done.", Logger.LogLevel.Verbose);
            _logger.Log("Checking files..", Logger.LogLevel.Medium);
            var filesToDownload = hashDownloader.GetMissingOrDifferentFileNames ();
            foreach (var file in filesToDownload) {
                _logger.Log("Downloading file: " + file + "..", Logger.LogLevel.Medium);
                FileDownloader.DownloadFile (RootPath, file);
                _logger.Log("Done.", Logger.LogLevel.Medium);
            }

            _logger.Log("Done checking files.", Logger.LogLevel.Medium);
            _logger.Log("Launching program: " + ProgramToRun + "..", Logger.LogLevel.Verbose);
		    var start = new ProcessStartInfo
		    {
		        FileName = ProgramToRun,
		    };
		    Process.Start(start);
            _logger.Log("Done.", Logger.LogLevel.Verbose);
		}
	}
}
