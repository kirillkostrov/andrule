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
using Android.Util;
using Android.Views;
using Android.Widget;
using Andrule.UIDetails;

namespace Andrule.Network
{
    public class NetWorkHelper
    {
        private UdpClient client = new UdpClient();
        public Context UIcontext;
        private string ip;

        public bool Connect(string ip)
        {
            this.ip = ip;
            try
            {
                client.Connect(ip, 51515);
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
                client.Send(messageInByte, messageInByte.Length);
            }
            catch (Exception e)
            {
                ShowErrorMessage("Sending error!");
                Log.Debug("Sending error: ", e.ToString());
                throw;
            }
        }

        public void CloseConnection()
        {
            client.Close();
        }

        private void ShowErrorMessage(string message)
        {
            UIHelper.ShowMessage(message, UIcontext);
        }
    }
}