using System;
using System.Net;
using System.Threading.Tasks;
using AndruleServer.Server;

namespace AndruleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //create a new server
            var server = new UdpListener();

            var vJoyFeeder = new VJoyFeeder();

            while (true)
            {
                var received = server.Receive();

                if (received.Status != TaskStatus.Faulted && received.Result.Message != null)
                {
                    //Console.WriteLine($"Message{received.Result.Message}");
                    //Console.WriteLine($"Sender {received.Result.Sender}");

                    if (received.Result.Message.Contains("quit"))
                        break;

                    var message = received.Result.Message;
                    var start = message.IndexOf('^');
                    var end = message.IndexOf('$');
                    
                    var data = message
                        .Remove(end)
                        .Substring(start+1)
                        .Split('|');
                   
                    if (data.Length == 7)
                    {
                        var phoneData = DataProcessor.Process(
                            float.Parse(data[0]),
                            float.Parse(data[1]),
                            float.Parse(data[2]),
                            int.Parse(data[3]),
                            int.Parse(data[4]),
                            int.Parse(data[5]),
                            int.Parse(data[6]));

                        vJoyFeeder.Feed(phoneData);
                    }
                }
            }
        }
    }
}
