using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Launchie
{
    public class Hasher
    {
        public static readonly Dictionary<string, byte[]> Hashes = new Dictionary<string, byte[]>();

        private static Logger _logger =  new Logger("Hasher");

		public static byte[] GetDirectoryHash(string rootDir, bool cacheHashes=true)
		{
			if (rootDir [rootDir.Length - 1] != '/') {
				rootDir += "/";
			}
			return GetDirectoryHashHelper (rootDir, rootDir.Length, cacheHashes);
		}

        private static byte[] GetDirectoryHashHelper(string rootDir, int fileNameStartIndex, bool cacheHashes=true)
        {
            var hash = new List<byte>();
            foreach (var subDir in Directory.GetDirectories(rootDir))
            {
                hash.AddRange(GetDirectoryHashHelper(subDir, fileNameStartIndex, cacheHashes));
            }
            foreach (var file in Directory.GetFiles(rootDir))
            {
				hash.AddRange(GetFileHash(file, fileNameStartIndex, cacheHashes));
            }
            return hash.ToArray();
        }

		public static byte[] GetFileHash(string filename, int fileNameStartIndex, bool cacheHashes=true)
        {
		    if (Blacklist.IsBlackListed(filename))
		    {
		        Log("File is blacklisted: " + filename + " - Skipping", Logger.LogLevel.Verbose);
		        return new byte[0];
		    }
            if (Hashes.ContainsKey(filename))
            {
                return Hashes[filename];
            }
            Log("Computing hash for file: " + filename);
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    if (cacheHashes)
                    {
						var name = filename.Substring (fileNameStartIndex);
						Log ("Saving hash for " + name);
						Hashes[name] = hash;
                    }
                    return hash;
                }
            }
        }

        public static bool CompareHashes(byte[] h1, byte[] h2)
        {
            if (h1.Length != h2.Length)
            {
                Log("Different hash lengths!");
                return false;
            }
            for (var i = 0; i < h1.Length; i++)
            {
                if (h1[i] != h2[i])
                {
                    Log("Different hashes (i=" + i + ")");
                    return false;
                }
            }
            return true;
        }

        private static void Log(string msg, Logger.LogLevel logLevel = Logger.LogLevel.Verbose)
        {
            _logger.Log(msg, logLevel);
        }
    }
}
