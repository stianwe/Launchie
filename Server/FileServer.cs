using System;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Server
{
	public class FileServer
	{
		public const int Port = 13338;

		public const int ClientLimit = 5;

		private string _rootDir;

		private TcpListener _listener;

		public FileServer (string rootDir)
		{
			_rootDir = rootDir;
		}

		public void Start() {
			Console.WriteLine ("Starting file server..");
			_listener = new TcpListener (Port);
			_listener.Start ();
			Console.WriteLine ("File server started on port " + Port);
			for (var i = 0; i < ClientLimit; i++) {
				new Thread (Service).Start ();
				Console.WriteLine ("Started client file service thread.");
			}
		}

		public void Service() {
			while (true) {
				using (var sock = _listener.AcceptSocket ()) {
					Console.WriteLine ("Client connected: " + sock.RemoteEndPoint);
					/*
					using (var stream = new NetworkStream (sock)) {
						using (var reader = new StreamReader (stream)) {
							Console.WriteLine ("Waiting for file name..");
							var fileName = reader.ReadLine ();
							Console.WriteLine ("Received request for file: " + fileName);
							var filePath = _rootDir + "/" + fileName;
							Console.WriteLine ("Sending file " + filePath);
							sock.SendFile (filePath);
							Console.WriteLine ("Done.");
							sock.Shutdown (SocketShutdown.Both);
							sock.Close ();
						}
					}
					*/
					Console.WriteLine ("Sending file..");
					sock.SendFile ("/home/stian/test/server/file1.txt");
					Console.WriteLine ("Done sending file.");
				}
			}
		}
	}
}

