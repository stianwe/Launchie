using System;
using System.Threading;
using System.Net.Sockets;
using Launchie;

namespace Server
{
    class Program
    {
		public const int Port = 13337;

		public const int ClientLimit = 5;

		public const string rootDir = "/home/stian/test/server";

        static void Main(string[] args)
        {
			var hashServer = new HashServer (rootDir);
			hashServer.Start ();

			var fileServer = new FileServer (rootDir);
			fileServer.Start ();
        }
    }
}
