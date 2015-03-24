using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Launchie
{
    public class Hasher
    {
        public static byte[] GetDirectoryHash(string rootDir)
        {
            var hash = new List<byte>();
            foreach (var subDir in Directory.GetDirectories(rootDir))
            {
                hash.AddRange(GetDirectoryHash(subDir));
            }
            foreach (var file in Directory.GetFiles(rootDir))
            {
                hash.AddRange(GetFileHash(file));
            }
            return hash.ToArray();
        }

        public static byte[] GetFileHash(string filename)
        {
            Log("Computing hash for file: " + filename);
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return md5.ComputeHash(stream);
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
