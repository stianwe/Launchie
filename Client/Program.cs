using System;
using System.Diagnostics;
using Launchie;
using Version = System.Version;

namespace Client
{
	class MainClass
    {
        public static readonly Version Version = new Version("0.0.1");

        public const string ProgramToRun = "PedalTanks_0.0.7/PedalTanks_0.0.7.exe";

        public const string RootPath = "C:/Users/Stian/Desktop/TEST/client";

        private static readonly Logger _logger = new Logger();

		public static void Main (string[] args)
		{
            _logger.Log("Launchie v" + Version + ".", Logger.LogLevel.Medium);
            _logger.Log("Creating HashDownloader..", Logger.LogLevel.Verbose);
            var hashDownloader = new HashDownloader (RootPath);
            _logger.Log("Done.", Logger.LogLevel.Verbose);
            _logger.Log("Checking files..", Logger.LogLevel.Medium);
            var filesToDownload = hashDownloader.GetMissingOrDifferentFileNames ();
            _logger.Log("Downloading missing or outdated files..", Logger.LogLevel.Medium);
		    var nFilesToDownload = filesToDownload.Count;
		    for (var i = 0; i < nFilesToDownload; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                string progressText = "(" + (i + 1) + "/" + nFilesToDownload + ")";
		        string logLine = filesToDownload[i];
		        if (logLine.Length + progressText.Length >= Console.BufferWidth)
		        {
		            logLine = logLine.Substring(0, Console.BufferWidth - progressText.Length - 3) + "...";
		        }
                Console.Write(logLine);
                Console.SetCursorPosition(Console.BufferWidth - progressText.Length, Console.CursorTop);
                Console.Write(progressText);
		        FileDownloader.DownloadFile(RootPath, filesToDownload[i]);
		    }
            _logger.Log("Done checking files.", Logger.LogLevel.Medium);
		    var fullPath = RootPath + "/" + ProgramToRun;
            _logger.Log("Launching program: " + fullPath + "..", Logger.LogLevel.Verbose);
		    var start = new ProcessStartInfo
		    {
		        FileName = fullPath,
		    };
		    Process.Start(start);
            _logger.Log("Done.", Logger.LogLevel.Verbose);
		}
	}
}
