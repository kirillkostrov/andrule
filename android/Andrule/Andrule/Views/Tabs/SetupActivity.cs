using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Widget;
using Andrule.Network;
using Andrule.UIDetails;

namespace Andrule.Views
{
    [Activity]
    public class SetupActivity : Activity
    {
        private ISharedPreferences preferences;

        private NetWorkHelper netWorkHelper;
        private EditText editIpText;
        private Button connectButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            preferences = GetPreferences(FileCreationMode.Private);
            netWorkHelper = new NetWorkHelper();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SetupLayout);

            editIpText = FindViewById<EditText>(Resource.Id.editIpText);
            connectButton = FindViewById<Button>(Resource.Id.connectButton);
            connectButton.Click += GetIpAndConnect;

            var ipAddress = preferences.GetString("ipAddress", string.Empty);
            editIpText.Text = ipAddress;
        }

        private void GetIpAndConnect(object sender, EventArgs e)
        {
            try
            {
                var ipAddress = editIpText.Text;
                var editor = preferences.Edit();
                editor.PutString("ipAddress", ipAddress);
                editor.Commit();

                NetWorkHelper.Connect(ipAddress);
            }
            catch (Exception ex)
            {
                UIHelper.ShowMessage("Connection error:" + ex.Message, this);
                return;
            }

            if (NetWorkHelper.IsConnected)
            {
                connectButton.Click -= GetIpAndConnect;
                connectButton.Text = "Stop";
                connectButton.Click += CloseConnection;
                MainActivity.Tabs.SetCurrentTabByTag("wheel");
            }
        }

        private void CloseConnection(object sender, EventArgs e)
        {
            netWorkHelper.CloseConnection();
            connectButton.Click += GetIpAndConnect;
            connectButton.Click -= CloseConnection;
            connectButton.Text = "Connect";
        }

        public new void Dispose()
        {
            base.Dispose();
            netWorkHelper.Dispose();
        }
    }
}