using System;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using Launchie;
using Version = System.Version;

namespace Client
{
	class MainClass
    {
        public static readonly Version Version = new Version("0.0.1");

	    public const string DllFileName = "Utils.dll";

        private static string _rootPath;
	    private static string _launcherFileName;
	    private static string _programToRun;
	    private static string _server;
	    private static int _hashServerPort;
	    private static int _fileServerPort;

	    private const string RootPathKey = "RootPath";
	    private const string LauncherFileNameKey = "LauncherFileName";
	    private const string ProgramToRunKey = "ProgramToRun";
	    private const string ServerKey = "Server";
	    private const string HashServerPortKey = "HashServerPort";
	    private const string FileServerPortKey = "FileServerPort";

	    private static bool _configSuccessfullyLoaded = true;

        private static readonly Logger Logger = new Logger();

	    private static void LoadConfig()
	    {
	        _rootPath = LoadConfig(RootPathKey);
	        _launcherFileName = LoadConfig(LauncherFileNameKey);
	        _programToRun = LoadConfig(ProgramToRunKey);
	        _server = LoadConfig(ServerKey);
	        _hashServerPort = Int32.Parse(LoadConfig(HashServerPortKey));
	        _fileServerPort = Int32.Parse(LoadConfig(FileServerPortKey));
	    }

	    private static string LoadConfig(string key)
	    {
	        var value = ConfigurationManager.AppSettings[key];
	        if (value == null)
	        {
	            Logger.Log("Key " + key + " missing in config. Please check the App.config file.", Logger.LogLevel.High);
	            _configSuccessfullyLoaded = false;
	        }
	        return value;
	    }

	    private static string GetOldName(string originalName)
	    {
	        var dotIndex = originalName.LastIndexOf('.');
	        return originalName.Substring(0, dotIndex) + "_old" + originalName.Substring(dotIndex);
	    }

		public static void Main (string[] args)
		{
            Logger.Log("Launchie v" + Version + ".", Logger.LogLevel.Medium);

            // Load config
            Logger.Log("Loading config..", Logger.LogLevel.Verbose);
		    LoadConfig();
		    if (!_configSuccessfullyLoaded)
		    {
                Logger.Log("Failed loading config. Exiting.", Logger.LogLevel.High);
                Console.Read();
		        return;
		    }
            Logger.Log("Done loading config.", Logger.LogLevel.Verbose);

            // Delete old files
            DeleteOldLauncherFile();
            DeleteOldDllFile();

            Logger.Log("Creating HashDownloader..", Logger.LogLevel.Verbose);
            var hashDownloader = new HashDownloader (_rootPath, _server, _hashServerPort);
            Logger.Log("Done.", Logger.LogLevel.Verbose);
            Logger.Log("Checking files..", Logger.LogLevel.Medium);
            var filesToDownload = hashDownloader.GetMissingOrDifferentFileNames ();
            Logger.Log("Downloading missing or outdated files..", Logger.LogLevel.Medium);
		    var nFilesToDownload = filesToDownload.Count;
		    for (var i = 0; i < nFilesToDownload; i++)
            {
                if (filesToDownload[i] == _launcherFileName)
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
		        FileDownloader.DownloadFile(_rootPath, filesToDownload[i], _server, _fileServerPort);
		    }
            Logger.Log("Done checking files.", Logger.LogLevel.Medium);
		    if (!File.Exists(_programToRun))
		    {
		        Logger.Log("Program to run (" + _programToRun + ") not found!", Logger.LogLevel.High);
		        Console.Read();
		    }
		    else
		    {
		        Logger.Log("Launching program: " + _programToRun + "..", Logger.LogLevel.Verbose);
		        var start = new ProcessStartInfo
		        {
		            FileName = _programToRun,
		        };
		        Process.Start(start);
		    }
		    Logger.Log("Done.", Logger.LogLevel.Verbose);
		}

	    private static void MoveLauncherFile()
	    {
	        File.Move(_launcherFileName, GetOldName(_launcherFileName));
	    }

	    private static void MoveDllFile()
	    {
	        File.Move(DllFileName, GetOldName(DllFileName));
	    }

	    private static void DeleteOldLauncherFile()
	    {
	        if (File.Exists(GetOldName(_launcherFileName)))
	        {
	            Logger.Log("Old launcher detected - deleting", Logger.LogLevel.High);
                File.Delete(GetOldName(_launcherFileName));
                Logger.Log("Done.", Logger.LogLevel.Verbose);
	        }
	    }

	    private static void DeleteOldDllFile()
	    {
            if (File.Exists(GetOldName(DllFileName)))
            {
                Logger.Log("Old DLL detected - deleting", Logger.LogLevel.High);
                File.Delete(GetOldName(DllFileName));
                Logger.Log("Done.", Logger.LogLevel.Verbose);
            }
	    }
	}
}
