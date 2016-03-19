using System;
using System.Collections.Generic;
using Android.App;
using Android.Hardware;
using Android.OS;
using Android.Widget;
using Andrule.Network;

namespace Andrule.View
{
    [Activity(Label = "Andrule", MainLauncher = true, Icon = "@mipmap/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class SetupActivity : Activity, ISensorEventListener
    {
        private NetWorkHelper netWorkHelper;
        private EditText editIpText;
        private Button connectButton;
        private bool _isConnected;

        private Sensor _sensor;
        private SensorManager _sensorManager;

        private const float SmoothingFactor = 0.005f;

        private float _previousReading = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            netWorkHelper = new NetWorkHelper() {UIcontext = this};
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SetupLayout);

            editIpText = FindViewById<EditText>(Resource.Id.editIpText);
            connectButton = FindViewById<Button>(Resource.Id.connectButton);
            connectButton.Click += GetIpAndConnect;

            _sensorManager = (SensorManager)GetSystemService(SensorService);
            _sensor = _sensorManager.GetDefaultSensor(SensorType.Accelerometer);

        }

        private void GetIpAndConnect(object sender, EventArgs e)
        {
            _isConnected = netWorkHelper.Connect(editIpText.Text);
            if (_isConnected)
            {
                connectButton.Click -= GetIpAndConnect;
                connectButton.Text = "Stop";
                connectButton.Click += CloseConnection;
            }
        }

        protected override void OnResume()
        {
            _sensorManager.RegisterListener(this, _sensor, SensorDelay.Ui);
            base.OnResume();

        }
        protected override void OnPause()
        {
            base.OnPause();
            _sensorManager.UnregisterListener(this);
        }

        private void CloseConnection(object sender, EventArgs e)
        {
            netWorkHelper.CloseConnection();
            connectButton.Click += GetIpAndConnect;
            connectButton.Click -= CloseConnection;
            connectButton.Text = "Connect";
            _isConnected = false;
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {

        }

        private void SendData(IReadOnlyList<int> sensorData)
        {
            netWorkHelper.Send(string.Format("^{0}|{1}|{2}|0|0|0|0$", sensorData[0], sensorData[1], sensorData[2]));
        }

        private float LowPassFilter(float newReading)
        {
            var filteredValue = 0.0f;

            filteredValue = SmoothingFactor * newReading + (1 - SmoothingFactor) * _previousReading;
            _previousReading = filteredValue;

            return filteredValue;


        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (!_isConnected) return;

            var rotation = (int)(LowPassFilter(e.Values[1]) * 1638 + 16383);
            var throttle = (int)(e.Values[0] * 1638 + 16383);
            var zAxis = (int)(e.Values[2] * 1638 + 16383);
            var data = new List<int> { rotation, throttle, zAxis };
            SendData(data);
        }

        public new void Dispose()
        {
            base.Dispose();
            netWorkHelper.Dispose();
        }
    }
}