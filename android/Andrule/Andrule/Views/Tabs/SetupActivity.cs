using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Andrule.Network;

namespace Andrule.View
{
    [Activity]
    public class SetupActivity : Activity
    {
        private NetWorkHelper netWorkHelper;
        private TextView textview;
        private EditText editIpText;
        private Button connectButton;
        private bool _isConnected;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            netWorkHelper = new NetWorkHelper() {UIcontext = this};
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SetupLayout);

            editIpText = FindViewById<EditText>(Resource.Id.editIpText);
            connectButton = FindViewById<Button>(Resource.Id.connectButton);
            connectButton.Click += GetIpAndConnect;

        }

        private void GetIpAndConnect(object sender, EventArgs e)
        {
            var _isConnected = netWorkHelper.Connect(editIpText.Text);
            if (_isConnected)
            {
                connectButton.Click -= GetIpAndConnect;
                connectButton.Text = "Stop";
                connectButton.Click += CloseConnection;
            }
        }

        private void CloseConnection(object sender, EventArgs e)
        {
            netWorkHelper.CloseConnection();
            connectButton.Click += GetIpAndConnect;
            connectButton.Click -= CloseConnection;
            connectButton.Text = "Connect";
            _isConnected = false;
        }
    }
}