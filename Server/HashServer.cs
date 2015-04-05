using System;
using System.Threading;
using System.Net.Sockets;
using Launchie;

namespace Server
{
	public class HashServer
	{
		public const int Port = 13337;

		public const int ClientLimit = 5;

		private TcpListener _listener;

		private string _rootDir;

		public HashServer (string rootDir)
		{
			_rootDir = rootDir;
		}

		public void Start() {
			Console.WriteLine ("Computing file hashes..");
			Hasher.GetDirectoryHash (_rootDir, true);
			Console.WriteLine ("Done.");
			Console.WriteLine ("Starting hash server..");
			_listener = new TcpListener (Port);
			_listener.Start ();
			Console.WriteLine ("Hash server started on port " + Port);
			for (var i = 0; i < ClientLimit; i++) 
			{
				Console.WriteLine ("Starting client hash service thread..");
				new Thread (Service).Start ();
			}
		}

		public void Service()
		{
			while (true) {
				using (var sock = _listener.AcceptSocket ()) {
					Console.WriteLine ("Client connected to hash server: " + sock.RemoteEndPoint);
					using (var s = new NetworkStream (sock)) {
						Console.WriteLine ("Sending hashes to client.");
						new HashesContainer (Hasher.Hashes).Serialize (s);
						Console.WriteLine ("Closing connection.");
					}
				}
			}
		}
	}
}

