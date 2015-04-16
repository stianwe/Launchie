using System;
using System.Net.Sockets;
using System.IO;
using Launchie;

namespace Client
{
	public class FileDownloader
	{
		public const int BufferSize = 1024;

		public const string Host = "localhost";

		public const int Port = 13338;

        private static Logger _logger = new Logger("FileDownloader");

		public static void DownloadFile(string rootDir, string fileName) {
            _logger.Log("Downloading file: " + fileName, Logger.LogLevel.Verbose);
			var fullPath = rootDir + "/" + fileName;
            _logger.Log("Full path: " + fullPath, Logger.LogLevel.Verbose);
			CreateDirs (fullPath);
            _logger.Log("Connecting to file eserver..", Logger.LogLevel.Verbose);
			using (var client = new TcpClient(Host, Port))
			//using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
			using (var output = File.Create(fullPath))
			using (var networkStream = client.GetStream()) 
			{
				//socket.Connect (Host, Port);
				using (var writer = new StreamWriter (networkStream)) {
					writer.AutoFlush = true;
                    _logger.Log("Requesting file: " + fileName + "..", Logger.LogLevel.Verbose);
                    writer.WriteLine(fileName);
                    var buffer = new byte[BufferSize];
                    int bytesRead = 0;
				    int totalBytesRead = 0;
                    _logger.LogNoLineShift("Receiving file..", Logger.LogLevel.Verbose);
                    //while ((bytesRead = socket.Receive(buffer)) > 0) {
                    while ((bytesRead = networkStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        _logger.LogNoComponentName(".", Logger.LogLevel.Verbose);
                        totalBytesRead += bytesRead;
                        output.Write(buffer, 0, bytesRead);
                    }
                    _logger.LogNoComponentName("\n", Logger.LogLevel.Verbose);
                    _logger.Log("Total bytes read: " + totalBytesRead, Logger.LogLevel.Verbose);
				}
			}
		}

		public static void CreateDirs(string path) {
            _logger.Log("Creating directory: " + path, Logger.LogLevel.Verbose);
            var dirPathLength = Math.Max(path.LastIndexOf('/'), path.LastIndexOf('\\'));
			var dir = path.Substring (0, dirPathLength);
            _logger.Log("Checking directory: " + dir + " (full path: " + path + ")", Logger.LogLevel.Verbose);
			if (!Directory.Exists (dir)) {
				_logger.Log("Directory does not exist - creating (" + dir + ")", Logger.LogLevel.Verbose);
				Directory.CreateDirectory (dir);
				_logger.Log("Done", Logger.LogLevel.Verbose);
            }
            else
            {
                _logger.Log("Directory alread exist.", Logger.LogLevel.Verbose);
            }
		}
	}
}

