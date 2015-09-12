namespace ReHashStarter
{
    using System.Net.Sockets;

    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new TcpClient("localhost", 13342))
            {
                // Simply connecting should be enough to trigger rehashing
            }
        }
    }
}
