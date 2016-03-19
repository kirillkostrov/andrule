using System.Net;

namespace AndruleServer.Server
{
    public struct Received
    {
        public IPEndPoint Sender;
        public string Message;
    }
}