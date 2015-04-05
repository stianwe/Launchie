using System;
using System.Net.Sockets;
using System.IO;
using Launchie;
using System.Collections.Generic;

namespace Client
{
	class MainClass
	{

		public static void Main (string[] args)
		{
            var rootDir = "C:/test/client";
			//var rootDir = "/home/stian/test/client";

			var hashDownloader = new HashDownloader (rootDir);
			var filesToDownload = hashDownloader.GetMissingOrDifferentFileNames ();
			foreach (var file in filesToDownload) {
				FileDownloader.DownloadFile (rootDir, file);
			}
		}
	}
}
