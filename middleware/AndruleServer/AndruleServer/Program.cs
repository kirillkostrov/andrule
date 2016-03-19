using System;
using System.Threading.Tasks;
using AndruleServer.Server;

namespace AndruleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //create a new server
            var server = new UdpListener("127.0.0.1", 32323);

            while (true)
            {
                var received = server.Receive();

                if (received.Status != TaskStatus.Faulted && received.Result.Message != null)
                {
                    server.Reply("copy " + received.Result.Message, received.Result.Sender);
                    Console.WriteLine($"Message{received.Result.Message}");
                    Console.WriteLine($"Sender {received.Result.Sender}");
                    if (received.Result.Message.Contains("quit"))
                        break;

                }
                
            }

        }
    }
}
