using System.Net;

namespace Projekat
{
    class Program
    {
        static void Main(string[] args)
        {

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 5050;

            TcpServer mojServer = new TcpServer(ipAddress, port);
            mojServer.StartServer();
        }

    }
    
}
