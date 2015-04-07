using System;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Launchie;

namespace Server
{
	public class FileServer
	{
		public const int Port = 13338;

		public const int ClientLimit = 5;

		private string _rootDir;

		private TcpListener _listener;

	    private Logger _logger;

		public FileServer (string rootDir)
		{
			_rootDir = rootDir;
            _logger = new Logger("FileServer");
		}

		public void Start() {
            _logger.Log("Starting..", Logger.LogLevel.Medium);
            _logger.Log("Port: " + Port, Logger.LogLevel.Medium);
            _logger.Log("Root directory: " + _rootDir, Logger.LogLevel.Medium);
            _logger.Log("Client limit: " + ClientLimit, Logger.LogLevel.Medium);
            _logger.Log("Starting TCP listener on port " + Port + "..", Logger.LogLevel.Medium);
			_listener = new TcpListener (Port);
			_listener.Start ();
            _logger.Log("Done.", Logger.LogLevel.Medium);
            _logger.Log("Starting " + ClientLimit + " client file service threads..", Logger.LogLevel.Medium);
			for (var i = 0; i < ClientLimit; i++) {
                _logger.Log("Starting client file service thread (" + i + ")..", Logger.LogLevel.Verbose);
				new Thread (Service).Start ();
				_logger.Log("Done.", Logger.LogLevel.Verbose);
			}
            _logger.Log("Done.", Logger.LogLevel.Medium);
            _logger.Log("Successfully started.", Logger.LogLevel.Medium);
		}

		public void Service() {
			while (true) {
                _logger.Log("Client file service thread accepting incoming connections.", Logger.LogLevel.Verbose);
				using (var sock = _listener.AcceptSocket ()) {
                    _logger.Log("Client connected: " + sock.RemoteEndPoint, Logger.LogLevel.Verbose);
					using (var stream = new NetworkStream (sock)) {
						using (var reader = new StreamReader (stream)) {
                            _logger.Log("Waiting for file name..", Logger.LogLevel.Verbose);
							var fileName = reader.ReadLine ();
                            _logger.Log("Received request for file: " + fileName, Logger.LogLevel.Verbose);
							var filePath = _rootDir + "/" + fileName;
                            _logger.Log("Sending file: " + filePath, Logger.LogLevel.Verbose);
							sock.SendFile (filePath);
							sock.Shutdown (SocketShutdown.Both);
                            sock.Close();
                            _logger.Log("Done.", Logger.LogLevel.Verbose);
						}
					}
				}
			}
		}
	}
}

