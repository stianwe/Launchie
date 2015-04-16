using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Launchie;
using Version = System.Version;

namespace Client
{
	class MainClass
    {
        public static readonly Version Version = new Version("0.0.1");

	    public const string LauncherFileName = "Launcher.exe";
	    public const string OldLuancherFileName = "Launcher_old.exe";
	    public const string DllFileName = "Utils.dll";
        public const string OldDllFileName = "Utils_old.dll";

        public const string ProgramToRun = "PedalTanks.exe";

        public const string RootPath = ".";

        private static readonly Logger Logger = new Logger();

		public static void Main (string[] args)
		{
            Logger.Log("Launchie v" + Version + ".", Logger.LogLevel.Medium);
            DeleteOldLauncherFile();
            DeleteOldDllFile();
            Logger.Log("Creating HashDownloader..", Logger.LogLevel.Verbose);
            var hashDownloader = new HashDownloader (RootPath);
            Logger.Log("Done.", Logger.LogLevel.Verbose);
            Logger.Log("Checking files..", Logger.LogLevel.Medium);
            var filesToDownload = hashDownloader.GetMissingOrDifferentFileNames ();
            Logger.Log("Downloading missing or outdated files..", Logger.LogLevel.Medium);
		    var nFilesToDownload = filesToDownload.Count;
		    for (var i = 0; i < nFilesToDownload; i++)
            {
                if (filesToDownload[i] == LauncherFileName)
                {
                    Logger.Log("New version of Launchie detected!", Logger.LogLevel.High);
                    MoveLauncherFile();
                }
                else if (filesToDownload[i] == DllFileName)
                {
                    Logger.Log("New version of DLL detected!", Logger.LogLevel.High);
                    MoveDllFile();
                }
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
            Logger.Log("Done checking files.", Logger.LogLevel.Medium);
		    if (!File.Exists(ProgramToRun))
		    {
		        Logger.Log("Program to run (" + ProgramToRun + ") not found!", Logger.LogLevel.High);
		        Console.Read();
		    }
		    else
		    {
		        Logger.Log("Launching program: " + ProgramToRun + "..", Logger.LogLevel.Verbose);
		        var start = new ProcessStartInfo
		        {
		            FileName = ProgramToRun,
		        };
		        Process.Start(start);
		    }
		    Logger.Log("Done.", Logger.LogLevel.Verbose);
		}

	    private static void MoveLauncherFile()
	    {
	        File.Move(LauncherFileName, OldLuancherFileName);
	    }

	    private static void MoveDllFile()
	    {
	        File.Move(DllFileName, OldDllFileName);
	    }

	    private static void DeleteOldLauncherFile()
	    {
	        if (File.Exists(OldLuancherFileName))
	        {
	            Logger.Log("Old launcher detected - deleting", Logger.LogLevel.High);
                File.Delete(OldLuancherFileName);
                Logger.Log("Done.", Logger.LogLevel.Verbose);
	        }
	    }

	    private static void DeleteOldDllFile()
	    {
            if (File.Exists(OldDllFileName))
            {
                Logger.Log("Old DLL detected - deleting", Logger.LogLevel.High);
                File.Delete(OldDllFileName);
                Logger.Log("Done.", Logger.LogLevel.Verbose);
            }
	    }
	}
}
