using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    class Program
    {
		public const int Port = 13337;

		public const int ClientLimit = 5;

		private static TcpListener _listener;

		private static Server _server;

        static void Main(string[] args)
        {
			_server = new Server ("~/test/server");
			Console.WriteLine ("Computing file hashes..");
			_server.ComputeFileHashes ();
			Console.WriteLine ("Done.");
			_listener = new TcpListener (Port);
			_listener.Start ();
			Console.WriteLine ("Server started on port " + Port);
			for (var i = 0; i < ClientLimit; i++) 
			{
				Console.WriteLine ("Starting client service..");
				new Thread (Service).Start ();
			}
        }

		public static void Service()
		{
			using (var sock = _listener.AcceptSocket ())
			{
				Console.WriteLine ("Client connected: " + sock.RemoteEndPoint);
				using (var s = new NetworkStream (sock)) 
				{
					using (var writer = new StreamWriter (s)) 
					{
						writer.AutoFlush = true;
						Console.WriteLine ("Sending message to client");
						writer.WriteLine ("Hei");
						Console.WriteLine ("Closing connection");
					}
				}
			}
		}
    }
}
