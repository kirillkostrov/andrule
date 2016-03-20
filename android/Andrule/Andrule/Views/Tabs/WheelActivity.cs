using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andrule.Views;
using Android.App;
using Android.Hardware;
using Android.OS;
using Andrule.Accelerometer;
using Andrule.Network;
using Andrule.UIDetails;
using Android.Content;
using Android.Widget;

namespace Andrule.Views
{
    [Activity]
	public class WheelActivity : Activity
    {
        private AccelerometerListener _accelerometerListener;
		private int progressBrake;
		private int progressRun;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WheelLayout);
			progressBrake = 0;
			progressRun = 0;

            var sensorManager = (SensorManager)GetSystemService(SensorService);
            _accelerometerListener = new AccelerometerListener(this, sensorManager, OnUpdateAccelerometer);

			var _seekBarBrake = FindViewById<SeekBar> (Resource.Id.seekBarBrake);
			_seekBarBrake.ProgressChanged += seekBarBrakeProgressChanged;
			var _seekBarRun = FindViewById<SeekBar> (Resource.Id.seekBarRun);
			_seekBarRun.ProgressChanged += seekBarRunProgressChanged;
        }

        public bool OnUpdateAccelerometer(Accelerometer.Accelerometer accelerometer)
        {
            try
            {
				NetWorkHelper.Send(string.Format("^{0}|{1}|{2}|0|0|0|0$", accelerometer.Rotation, progressBrake, progressRun));
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

		private void seekBarBrakeProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e){
			if (e.FromUser)
			{
				progressBrake = e.Progress * 1000;
			}
		}

		private void seekBarRunProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e){
			if (e.FromUser)
			{
				progressRun = e.Progress * 1000;
			}
		}
    }
}