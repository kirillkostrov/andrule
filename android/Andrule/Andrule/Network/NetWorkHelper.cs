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
        private UdpClient _client;
        public Context UIcontext;

        public bool Connect(string ip)
        {
            try
            {
                _client = new UdpClient();
                //_client.Connect(ip, 51515);
                _client.Connect("192.168.34.146", 51515);
            }
            catch (Exception e)
            {
                ShowErrorMessage("Connection error!");
                Log.Debug("Connection error: ", e.ToString());
                return false;
            }
            return true;
        }

        public void Send(string message)
        {
            var messageInByte = Encoding.ASCII.GetBytes(message);
            try
            {
                _client?.Send(messageInByte, messageInByte.Length);
            }
            catch (Exception e)
            {
                ShowErrorMessage("Sending error!");
                Log.Debug("Sending error: ", e.ToString());
                //throw;
            }
        }

        public void CloseConnection()
        {
            _client?.Close();
        }

        private void ShowErrorMessage(string message)
        {
            UIHelper.ShowMessage(message, UIcontext);
        }

        public void Dispose()
        {
            _client?.Close();
        }
    }
}