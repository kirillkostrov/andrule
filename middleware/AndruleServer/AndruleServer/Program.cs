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
            var server = new UdpListener("127.0.0.1", 51515);

            while (true)
            {
                var received = server.Receive();

                if (received.Status != TaskStatus.Faulted && received.Result.Message != null)
                {
                    server.Reply("copy " + received.Result.Message, received.Result.Sender);
                    Console.WriteLine($"Message{received.Result.Message}");
                    Console.WriteLine($"Sender {received.Result.Sender}");

                    var message = received.Result.Message;
                    var start = message.IndexOf('^');
                    var end = message.IndexOf('$');

                    var data = message
                        .Remove(end)
                        .Substring(start+1)
                        .Split('|');

                    if (data.Length == 7)
                    {
                        var phoneData = new PhoneData
                        {
                            AxisX = int.Parse(data[0]),
                            AxisY = int.Parse(data[1]),
                            AxisZ = int.Parse(data[2]),
                            Button1 = int.Parse(data[3]) != 0,
                            Button2 = int.Parse(data[4]) != 0,
                            Button3 = int.Parse(data[5]) != 0,
                            Button4 = int.Parse(data[6]) != 0,
                        };
                    }

                    if (received.Result.Message.Contains("quit"))
                        break;

                }
                
            }

        }
    }
}
