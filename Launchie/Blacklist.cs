using System;
using System.Collections.Generic;
using System.IO;

namespace Launchie
{
    public static class Blacklist
    {
        public static string Path = "LAUNCHIE_BLACKLIST.txt";

        private static readonly HashSet<string> Files = new HashSet<string>();

        private static readonly List<string> Endings = new List<string>();

        private static readonly Logger Logger = new Logger("Blacklist");

        static Blacklist()
        {
            if (!File.Exists(Path))
            {
                Logger.Log("Blacklist (" + Path + ") does not exist!", Logger.LogLevel.Medium);
                return;
            }
            Logger.Log("Reading blacklist: " + Path, Logger.LogLevel.Medium);
            foreach (var file in File.ReadLines(Path))
            {
                Logger.Log(file, Logger.LogLevel.Verbose);
                if (file[0] == '*')
                {
                    Logger.Log("Adding ending: " + file.Substring(1), Logger.LogLevel.Verbose);
                    Endings.Add(file.Substring(1));
                }
                else
                {
                    Logger.Log("Adding file: " + file, Logger.LogLevel.Verbose);
                    Files.Add(file);
                }
            }
            Logger.Log("Done reading blacklist.", Logger.LogLevel.Verbose);
        }

        public static bool IsBlackListed(string file)
        {
            var isBlackListed = Files.Contains(file);
            if (!isBlackListed)
            {
                foreach (var ending in Endings)
                {
                    if (file.EndsWith(ending))
                    {
                        isBlackListed = true;
                        break;
                    }
                }
            }
            Logger.Log("Checking if " + file + " is blacklisted: " + isBlackListed, Logger.LogLevel.Verbose);
            return isBlackListed;
        }
    }
}
