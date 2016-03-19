using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.Widget;
using Android.OS;
using Android.Views;
using Andrule.Network;

namespace Andrule
{
    [Activity(Label = "Andrule", MainLauncher = true, Icon = "@mipmap/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class MainActivity : Activity, ISensorEventListener
    {
        private TextView _sensorTextView;
        private object _syncLock = new object();
        private NetWorkHelper netWorkHelper;
        private SensorManager _sensorManager;
        private Sensor _sensor;
        private string _connectionIp;
        private bool _isConnected;
        private Button connectBtn;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            _sensorTextView = FindViewById<TextView>(Resource.Id.CoordinateText);
            connectBtn = FindViewById<Button>(Resource.Id.ConnectBtn);
            connectBtn.Click += GetIpAndConnect;

            _sensorManager = (SensorManager)GetSystemService(SensorService);
            _sensor = _sensorManager.GetDefaultSensor(SensorType.Accelerometer);

            netWorkHelper = new NetWorkHelper { UIcontext = this };
        }

        public void GetIpAndConnect(object sender, EventArgs e)
        {
            _connectionIp = FindViewById<EditText>(Resource.Id.ConnectIpText).Text;

            Connect(string.IsNullOrEmpty(_connectionIp) ? "169.254.237.222" : _connectionIp);
        }

        private void CloseConnection(object sender, EventArgs e)
        {
            netWorkHelper.CloseConnection();
            connectBtn.Click += GetIpAndConnect;
            connectBtn.Click -= CloseConnection;
            connectBtn.Text = "Connect";
            _isConnected = false;
        }

        protected override void OnResume()
        {
            _sensorManager.RegisterListener(this, _sensor, SensorDelay.Ui);
            base.OnResume();
            
        }
        protected override void OnPause()
        {
            base.OnPause();
            _sensorManager.UnregisterListener(this);}

        private void Connect(string ip)
        {
            _isConnected = netWorkHelper.Connect(ip);
            if (_isConnected)
            {
                connectBtn.Click -= GetIpAndConnect;
                connectBtn.Text = "Stop";
                connectBtn.Click += CloseConnection;
            }
        }

        private void SendData(IReadOnlyList<int> sensorData)
        {
            netWorkHelper.Send(string.Format("^{0}|{1}|{2}|1|1|1$", sensorData[0], sensorData[1], sensorData[2]));
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (_isConnected)
            {

                var rotation = (int) (e.Values[1]*100);
                var data = new List<int> {rotation, 100, 200};
                _sensorTextView.Text = rotation.ToString();
                SendData(data);
            }
        }
    }
}


