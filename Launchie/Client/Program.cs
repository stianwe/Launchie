using System;
using System.Net.Sockets;
using System.IO;
using Launchie;
using System.Collections.Generic;

namespace Client
{
	class MainClass
	{
		public const string Host = "localhost";
		public const int Port = 13337;

		public static void Main (string[] args)
		{
			Console.WriteLine ("Computing hashes of local files..");
			Hasher.GetDirectoryHash ("/home/stian/test/client", true);
			var localHashes = Hasher.Hashes;
			var serverHashes = GetServerHashes ();
			var missingOrDifferent = GetMissingOrDifferentFileNames (localHashes, serverHashes);
		}

		public static List<string> GetMissingOrDifferentFileNames(Dictionary<string, byte[]> localHashes, Dictionary<string, byte[]> serverHashes)
		{
			var missingOrDifferent = new List<string> ();
			Console.WriteLine ("Comparing hashes");
			foreach (var serverFileName in serverHashes.Keys) 
			{
				Console.WriteLine ("Checking file: " + serverFileName);
				if (!localHashes.ContainsKey (serverFileName)) {
					Console.WriteLine ("File missing.");
					missingOrDifferent.Add (serverFileName);
				} else if (!Hasher.CompareHashes (localHashes [serverFileName], serverHashes [serverFileName])) {
					Console.WriteLine ("File different");
					missingOrDifferent.Add (serverFileName);
				} else {
					Console.WriteLine ("File OK");
				}
			}
			return missingOrDifferent;
		}

		public static Dictionary<string, byte[]> GetServerHashes()
		{
			Console.WriteLine ("Connecting to " + Host + " on " + Port);
			using (var client = new TcpClient (Host, Port)) 
			{
				using (var stream = client.GetStream ()) 
				{
					var serverHashes = HashesContainer.Deserialize (stream);
					Console.WriteLine ("Received hashes for files:");
					foreach (var file in serverHashes.Keys) 
					{
						Console.WriteLine (file);
					}
					return serverHashes;
				}
			}
		}
	}
}
