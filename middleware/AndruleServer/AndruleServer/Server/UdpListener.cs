using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AndruleServer.Server
{
    class UdpListener : UdpBase
    {
        private IPEndPoint _listenOn;

        public UdpListener(string ip, int port) : this(new IPEndPoint(IPAddress.Parse(ip), port))
        {
        }

        public UdpListener() : this(new IPEndPoint(IPAddress.Any, 51515))
        {
        }

        public UdpListener(IPEndPoint endpoint)
        {
            _listenOn = endpoint;
            Client = new UdpClient(_listenOn);
        }

        public void Reply(string message, IPEndPoint endpoint)
        {
            var datagram = Encoding.ASCII.GetBytes(message);
            Client.Send(datagram, datagram.Length, endpoint);
        }

    }
}