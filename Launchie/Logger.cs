
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

        public Logger(string logFilePath="LAUNCHIE_LOG.txt") : this(null, logFilePath)
        {
        }

        public Logger(string componentName, string logFilePath="LAUNCHIE_LOG.txt")
        {
            _componentName = componentName;
            _logFilePath = logFilePath;
            if (!File.Exists(_logFilePath))
            {
                File.Create(_logFilePath);
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
                using (var writer = File.AppendText(_logFilePath))
                {
                    writer.Write(msg);
                }
            }
        }

        public void Log(string msg, LogLevel logLevel)
        {
            LogNoLineShift(msg + "\n", logLevel);
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
