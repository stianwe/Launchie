using Launchie;

namespace Server
{
    public class Server
    {
        private string _rootDir;

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
