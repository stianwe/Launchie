
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Launchie
{
    public class Logger
    {
        private readonly string _componentName;

        private readonly string _logFilePath;

        public LogLevel ConsoleLogLevel = LogLevel.Medium;

        public LogLevel FileLogLevel = LogLevel.Verbose;

        private static readonly object FileLock = new object();

        public Logger(string componentName=null, string logFilePath="LAUNCHIE_LOG.txt")
        {
            _componentName = componentName;
            _logFilePath = logFilePath;
            lock (FileLock)
            {
                if (!File.Exists(_logFilePath))
                {
                    File.Create(_logFilePath).Dispose();
                }
            }
        }

        public void LogNoLineShift(string msg, LogLevel logLevel)
        {
            LogNoComponentName((_componentName != null ? ("[" + _componentName + "]: ") : "") + msg, logLevel);
        }

        public void LogNoComponentName(string msg, LogLevel logLevel)
        {
            if (ShouldBeLogged(logLevel, ConsoleLogLevel))
            {
                Console.Write(msg);
            }
            if (_logFilePath != null && ShouldBeLogged(logLevel, FileLogLevel))
            {
                WriteToFile(msg, _logFilePath);
            }
        }

        public void Log(string msg, LogLevel logLevel)
        {
            LogNoLineShift(msg + "\n", logLevel);
        }

        private static void WriteToFile(string msg, string path)
        {
            // TODO Can be improved by checking which file is being written to..
            lock (FileLock)
            {
                using (var writer = File.AppendText(path))
                {
                    writer.Write(msg);
                }
            }
        }

        public enum LogLevel
        {
            Verbose,
            Medium,
            High,
        }

        private static bool ShouldBeLogged(LogLevel logLevel, LogLevel lowestLoggedLogLevel)
        {
            if (lowestLoggedLogLevel == LogLevel.Verbose)
            {
                return true;
            }
            if (lowestLoggedLogLevel == LogLevel.Medium && logLevel != LogLevel.Verbose)
            {
                return true;
            }
            if (lowestLoggedLogLevel == LogLevel.High && logLevel == LogLevel.High)
            {
                return true;
            }
            return false;
        }
    }
}
