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

namespace Andrule.Network
{
    public class NetWorkHelper
    {
        private UdpClient client = new UdpClient();
        private Context context;
        private string ip;

        public NetWorkHelper(Context context)
        {
            this.context = context;
        }

        public bool Connect(string ip)
        {
            this.ip = ip;
            try
            {
                client.Connect(ip, 51515);
            }
            catch (Exception e)
            {
                ShowMessage("Connection error!");
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
                ShowMessage("Sending error!");
                Log.Debug("Sending error: ", e.ToString());
                throw;
            }
        }

        public void CloseConnection()
        {
            client.Close();
        }

        private void ShowMessage(string message)
        {
            var dialog = new AlertDialog.Builder(context);
            dialog.SetMessage(message);
            dialog.SetNegativeButton("Cancel", (s, e) => { });
            dialog.Create().Show();
        }
    }
}