using System;
using System.Net.Sockets;
using System.IO;

namespace Client
{
	public class FileDownloader
	{
		public const int BufferSize = 1024;

		public const string Host = "localhost";

		public const int Port = 13338;

		public static void DownloadFile(string rootDir, string fileName) {
			var fullPath = rootDir + "/" + fileName;
			CreateDirs (fullPath);
			Console.WriteLine ("Connecting to file server..");
			using (var client = new TcpClient(Host, Port))
			//using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
			using (var output = File.Create(fullPath))
			using (var networkStream = client.GetStream()) 
			{
				//socket.Connect (Host, Port);
				using (var writer = new StreamWriter (networkStream)) {
					writer.AutoFlush = true;
					Console.WriteLine ("Requesting file: " + fileName);
                    writer.WriteLine(fileName);
                    var buffer = new byte[BufferSize];
                    int bytesRead = 0;
                    Console.Write("Receiving file..");
                    //while ((bytesRead = socket.Receive(buffer)) > 0) {
                    while ((bytesRead = networkStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        Console.Write(".");
                        Console.WriteLine("Read " + bytesRead + " bytes");
                        output.Write(buffer, 0, bytesRead);
                    }
                    Console.WriteLine();
                    Console.WriteLine("Done.");
				}
			}
		}

		public static void CreateDirs(string path) {
            var dirPathLength = Math.Max(path.LastIndexOf('/'), path.LastIndexOf('\\'));
			var dir = path.Substring (0, dirPathLength);
            Console.WriteLine("Checking directory: " + dir + " (full path: " + path + ")");
			if (!Directory.Exists (dir)) {
				Console.WriteLine ("Directory does not exist - creating (" + dir + ")");
				Directory.CreateDirectory (dir);
				Console.WriteLine ("Done.");
            }
            else
            {
                Console.WriteLine("Directory already exists.");
            }
		}
	}
}

