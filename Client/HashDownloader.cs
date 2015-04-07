using System;
using Launchie;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Client
{
	public class HashDownloader
	{
		public const string Host = "localhost";
		public const int Port = 13337;

		private string _rootDir;

	    private static Logger _logger = new Logger("HashDownloader");

		public HashDownloader (string rootDir)
		{
			_rootDir = rootDir;
            _logger.Log("Host: " + Host, Logger.LogLevel.Verbose);
            _logger.Log("Port: " + Port, Logger.LogLevel.Verbose);
            _logger.Log("Root directory: " + rootDir, Logger.LogLevel.Verbose);
		}

		public List<string> GetMissingOrDifferentFileNames() {
            _logger.Log("Finding missing or different files..", Logger.LogLevel.Verbose);
			Hasher.GetDirectoryHash (_rootDir, true);
			var serverHashes = GetServerHashes ();
			return GetMissingOrDifferentFileNames (Hasher.Hashes, serverHashes);
		}

		public static List<string> GetMissingOrDifferentFileNames(Dictionary<string, byte[]> localHashes, Dictionary<string, byte[]> serverHashes)
		{
			var missingOrDifferent = new List<string> ();
            _logger.Log("Comparing hashes..", Logger.LogLevel.Verbose);
			foreach (var serverFileName in serverHashes.Keys) 
			{
                _logger.Log("Checking file: " + serverFileName + "..", Logger.LogLevel.Medium);
				if (!localHashes.ContainsKey (serverFileName)) {
					_logger.Log("File is missing.", Logger.LogLevel.Medium);
					missingOrDifferent.Add (serverFileName);
				} else if (!Hasher.CompareHashes (localHashes [serverFileName], serverHashes [serverFileName])) {
					_logger.Log("File is different.", Logger.LogLevel.Medium);
					missingOrDifferent.Add (serverFileName);
				} else {
					_logger.Log("File OK", Logger.LogLevel.Verbose);
				}
			}
			return missingOrDifferent;
		}

		public static Dictionary<string, byte[]> GetServerHashes()
		{
            _logger.Log("Connecting to " + Host + " on " + Port + "..", Logger.LogLevel.Verbose);
			using (var client = new TcpClient (Host, Port)) 
			{
				using (var stream = client.GetStream ()) 
				{
					var serverHashes = HashesContainer.Deserialize (stream);
					_logger.Log("Received hashes from server:", Logger.LogLevel.Verbose);
					foreach (var file in serverHashes.Keys) 
					{
						_logger.Log(file, Logger.LogLevel.Verbose);
					}
					return serverHashes;
				}
			}
		}
	}
}

