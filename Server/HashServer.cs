using System;
using System.Threading;
using System.Net.Sockets;
using Launchie;

namespace Server
{
	public class HashServer
	{
		private TcpListener _listener;

		private readonly string _rootDir;

	    private readonly Logger _logger;

	    private readonly int _port;

	    private readonly int _clientLimit;

		public HashServer (string rootDir, int port, int clientLimit)
		{
			_rootDir = rootDir;
		    _clientLimit = clientLimit;
		    _port = port;
            _logger = new Logger("HashServer");
		}

		public void Start() {
            _logger.Log("Starting..", Logger.LogLevel.Medium);
            _logger.Log("Port: " + _port, Logger.LogLevel.Medium);
            _logger.Log("Root directory: " + _rootDir, Logger.LogLevel.Medium);
            _logger.Log("Client limit: " + _clientLimit, Logger.LogLevel.Medium);
            _logger.Log("Computing file hashes..", Logger.LogLevel.Medium);
			Hasher.GetDirectoryHash (_rootDir, true);
            _logger.Log("Done.", Logger.LogLevel.Medium);
			_logger.Log("Starting TCP listener on port " + _port + "..", Logger.LogLevel.Medium);
			_listener = new TcpListener (_port);
			_listener.Start ();
            _logger.Log("Done.", Logger.LogLevel.Medium);
            _logger.Log("Starting " + _clientLimit + " client hash service threads..", Logger.LogLevel.Medium);
			for (var i = 0; i < _clientLimit; i++) 
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

