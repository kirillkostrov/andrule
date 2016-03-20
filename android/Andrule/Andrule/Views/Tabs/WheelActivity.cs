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

		private int _button01Clicked;
		private int _button02Clicked;
		private int _button03Clicked;
		private int _button04Clicked;

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

			Button button01 = FindViewById<Button>(Resource.Id.Button01);
			button01.Click += delegate { _button01Clicked = 1; };

			Button button02 = FindViewById<Button>(Resource.Id.Button02);
			button02.Click += delegate { _button02Clicked = 1; };

			Button button03 = FindViewById<Button>(Resource.Id.Button03);
			button03.Click += delegate { _button03Clicked = 1; };

			Button button04 = FindViewById<Button>(Resource.Id.Button04);
			button04.Click += delegate { _button04Clicked = 1; };
        }

        public bool OnUpdateAccelerometer(Accelerometer.Accelerometer accelerometer)
        {
            try
            {
				NetWorkHelper.Send(string.Format("^{0}|{1}|{2}|{3}|{4}|{5}|{6}$", accelerometer.Rotation, progressBrake, progressRun, _button01Clicked, _button02Clicked, _button03Clicked, _button04Clicked));
				_button01Clicked = 0;
				_button02Clicked = 0;
				_button03Clicked = 0;
				_button04Clicked = 0;
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