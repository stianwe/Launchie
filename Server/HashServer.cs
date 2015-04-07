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

		private readonly string _rootDir;

	    private Logger _logger;

		public HashServer (string rootDir)
		{
			_rootDir = rootDir;
            _logger = new Logger("HashServer");
		}

		public void Start() {
            _logger.Log("Starting..", Logger.LogLevel.Medium);
            _logger.Log("Port: " + Port, Logger.LogLevel.Medium);
            _logger.Log("Root directory: " + _rootDir, Logger.LogLevel.Medium);
            _logger.Log("Client limit: " + ClientLimit, Logger.LogLevel.Medium);
            _logger.Log("Computing file hashes..", Logger.LogLevel.Medium);
			Hasher.GetDirectoryHash (_rootDir, true);
            _logger.Log("Done.", Logger.LogLevel.Medium);
			_logger.Log("Starting TCP listener on port " + Port + "..", Logger.LogLevel.Medium);
			_listener = new TcpListener (Port);
			_listener.Start ();
            _logger.Log("Done.", Logger.LogLevel.Medium);
            _logger.Log("Starting " + ClientLimit + " client hash service threads..", Logger.LogLevel.Medium);
			for (var i = 0; i < ClientLimit; i++) 
			{
                _logger.Log("Starting client hash service thread (" + i + ")..", Logger.LogLevel.Verbose);
                new Thread(Service).Start();
                _logger.Log("Done.", Logger.LogLevel.Verbose);
			}
            _logger.Log("Done.", Logger.LogLevel.Medium);
            _logger.Log("Successfully started.", Logger.LogLevel.Medium);
		}

		public void Service()
		{
			while (true) {
                _logger.Log("Client hash service thread accepting incoming connections.", Logger.LogLevel.Verbose);
				using (var sock = _listener.AcceptSocket ()) {
                    _logger.Log("Client connected: " + sock.RemoteEndPoint, Logger.LogLevel.Verbose);
					using (var s = new NetworkStream (sock)) {
                        _logger.Log("Sending hashes to client..", Logger.LogLevel.Verbose);
						new HashesContainer (Hasher.Hashes).Serialize (s);
						_logger.Log("Done.", Logger.LogLevel.Verbose);
					}
				}
			}
		}
	}
}

