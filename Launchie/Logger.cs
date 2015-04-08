
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Launchie
{
    public class Logger
    {
        public const string LogFilePath = "./LAUNCHIE_LOG.txt";

        private readonly string _componentName;

        public LogLevel ConsoleLogLevel = LogLevel.Medium;

        public LogLevel FileLogLevel = LogLevel.Verbose;

        private static readonly object FileLock = new object();

        private static StreamWriter Writer;

        public Logger(string componentName=null)
        {
            _componentName = componentName;
            lock (FileLock)
            {
                if (!File.Exists(LogFilePath))
                {
                    var stream = File.Create(LogFilePath);
                    stream.Close();
                    stream.Dispose();
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
            if (ShouldBeLogged(logLevel, FileLogLevel))
            {
                WriteToFile(msg, LogFilePath);
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
                if (Writer == null)
                {
                    Writer = File.AppendText(path);
                }
                Writer.Write(msg);
                //using (var writer = File.AppendText(path))
                //{
                //    writer.Write(msg);
                //}
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
