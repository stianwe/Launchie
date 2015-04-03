using System.Collections.Generic;
using Launchie;

namespace Server
{
    public class Server
    {
        private string _rootDir;

		public Dictionary<string, byte[]> Hashes 
		{
			get 
			{
				return Hasher.Hashes;
			}
		}

        public Server(string rootDir)
        {
            _rootDir = rootDir;
        }

        public void ComputeFileHashes()
        {
            Hasher.GetDirectoryHash(_rootDir, true);
        }
    }
}
