using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Launchie
{
    public class Hasher
    {
        public static readonly Dictionary<string, byte[]> Hashes = new Dictionary<string, byte[]>();

        public static byte[] GetDirectoryHash(string rootDir, bool cacheHashes=true)
        {
            var hash = new List<byte>();
            foreach (var subDir in Directory.GetDirectories(rootDir))
            {
                hash.AddRange(GetDirectoryHash(subDir, cacheHashes));
            }
            foreach (var file in Directory.GetFiles(rootDir))
            {
                hash.AddRange(GetFileHash(file, cacheHashes));
            }
            return hash.ToArray();
        }

        public static byte[] GetFileHash(string filename, bool cacheHashes=true)
        {
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
                        Hashes[filename] = hash;
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

        private static void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
