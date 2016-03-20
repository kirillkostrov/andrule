using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Hardware;
using Android.OS;
using Andrule.Accelerometer;
using Andrule.Network;
using Andrule.UIDetails;

namespace Andrule.Views
{
    [Activity]
    public class WheelActivity : Activity
    {
        private AccelerometerListener _accelerometerListener;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WheelLayout);
            var sensorManager = (SensorManager)GetSystemService(SensorService);
            _accelerometerListener = new AccelerometerListener(this, sensorManager, OnUpdateAccelerometer);
        }

        public bool OnUpdateAccelerometer(Accelerometer.Accelerometer accelerometer)
        {
            try
            {
                NetWorkHelper.Send(string.Format("^{0}|{1}|{2}|0|0|0|0$", accelerometer.Rotation, accelerometer.Throttle, accelerometer.ZAxis));
            }
            catch (Exception ex)
            {
                UIHelper.ShowMessage("Sending error:" + ex.Message, this);
            }
            
            return NetWorkHelper.IsConnected;
        }

        protected override void OnResume()
        {
            base.OnResume();
            _accelerometerListener.Start();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _accelerometerListener.Stop();
        }


    }
}