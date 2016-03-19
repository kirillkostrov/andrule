using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Andrule.Network
{
    public class NetWorkHelper
    {
        private UdpClient client = new UdpClient();

        public string Connect(string ip)
        {
            client.Connect(ip, 32323);
            return client.EnableBroadcast ? "Connected" : "Error";
        }

        public string Send(string message)
        {
            var messageInByte = Encoding.ASCII.GetBytes(message);
            try
            {
                client.Send(messageInByte, messageInByte.Length);
            }
            catch (Exception e)
            {
                return e.ToString();
                throw;
            }
            return "ok";
        }

        public void CloseConnection()
        {
            client.Close();
        }
    }
}