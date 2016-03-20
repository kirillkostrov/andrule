using System;
using System.Net.Sockets;
using System.Text;
using Android.Content;
using Android.Util;
using Andrule.UIDetails;

namespace Andrule.Network
{
    public class NetWorkHelper : IDisposable
    {
        private static UdpClient _client;
        public static bool IsConnected { get; private set; }

        public void Connect(string ip)
        {
            try
            {
                _client = new UdpClient();
                //_client.Connect(ip, 51515);
                //_client.Connect("192.168.137.1", 51515);
                // KK's address
                _client.Connect("192.168.34.146", 51515);
                IsConnected = true;
            }
            catch (Exception e)
            {
                IsConnected = false;
                Log.Debug("Connection error: ", e.Message);
                throw;
            }
        }

        public static void Send(string message)
        {
            var messageInByte = Encoding.ASCII.GetBytes(message);
            try
            {
                _client?.Send(messageInByte, messageInByte.Length);
            }
            catch (Exception e)
            {
                IsConnected = false;
                Log.Debug("Sending error: ", e.Message);
                throw;
            }
        }

        public void CloseConnection()
        {
            IsConnected = false;
            _client?.Close();
        }

        public void Dispose()
        {
            IsConnected = false;
            _client?.Close();
        }
    }
}