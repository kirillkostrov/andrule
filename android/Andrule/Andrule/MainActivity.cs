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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            _sensorTextView = FindViewById<TextView>(Resource.Id.CoordinateText);

            _sensorManager = (SensorManager)GetSystemService(SensorService);
            _sensor = _sensorManager.GetDefaultSensor(SensorType.Accelerometer);

            netWorkHelper = new NetWorkHelper { UIcontext = this };
            Connect("192.168.137.1");
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
            netWorkHelper.Connect(ip);
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
            var rotation = (int)(e.Values[1] * 100);
            var data = new List<int> { rotation, 100, 200 };
            _sensorTextView.Text = rotation.ToString();
            SendData(data);
        }
    }
}


