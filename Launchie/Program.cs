using System;
using System.IO;

namespace Launchie
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = new DirectoryInfo("F:/Documents");
            //Console.WriteLine(dir.GetHashCode());
            //dir.
            //Directory.EnumerateDirectories("F:/Documents")
            //var hash1 = Hasher.GetDirectoryHash("F:/Documents/hashme");
            //var hash2 = Hasher.GetDirectoryHash("F:/Documents/hashme2");
            //Console.WriteLine(Hasher.CompareHashes(hash1, hash2));


            var v1 = new Version("0.2.3");
            var v2 = new Version("1.1.2");
            Console.WriteLine(v1 < v2);
        }
    }
}
