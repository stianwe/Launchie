using System;
using System.Net.Sockets;
using System.IO;

namespace Client
{
	class MainClass
	{
		public const string Host = "localhost";
		public const int Port = 13337;

		public static void Main (string[] args)
		{
			Console.WriteLine ("Computing hashes of local files..");
			var hash = Hasher
			Console.WriteLine ("Connecting to " + Host + " on " + Port);
			using (var client = new TcpClient (Host, Port)) 
			{
				using (var stream = client.GetStream ()) 
				{
					using (var reader = new StreamReader (stream)) 
					{
						Console.WriteLine ("Reading from server..");
						Console.WriteLine (reader.ReadLine ());
						Console.WriteLine ("Closing connection to server.");
					}
				}
			}
		}
	}
}
