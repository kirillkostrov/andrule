using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.Widget;
using Android.OS;
using Andrule.Network;
using Java.Lang;

namespace Andrule
{
	[Activity (Label = "Andrule", MainLauncher = true, Icon = "@mipmap/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
	public class MainActivity : Activity, ISensorEventListener
	{
		int count = 1;
        static readonly object _syncLock = new object();
        TextView _sensorTextView;
        SensorManager _sensorManager;
	    private NetWorkHelper netWorkHelper;

        protected override void OnCreate (Bundle bundle)
		{
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            _sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            _sensorTextView = FindViewById<TextView>(Resource.Id.CoordinateText);
            netWorkHelper = new NetWorkHelper();
            Connect("123");
		}

        protected override void OnResume()
        {
            base.OnResume();
            _sensorManager.RegisterListener(this,
                                            _sensorManager.GetDefaultSensor(SensorType.Accelerometer),
                                            SensorDelay.Ui);
        }
        protected override void OnPause()
        {
            base.OnPause();
            _sensorManager.UnregisterListener(this);
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
		{
			// We don't want to do anything here.
		}

		public void OnSensorChanged(SensorEvent e)
		{
			lock (_syncLock)
			{
                SendData(new [] { e.Values[0], e.Values[1], e.Values[2] }); ;
				_sensorTextView.Text = string.Format("x={0:f}, y={1:f}, y={2:f}", e.Values[0], e.Values[1], e.Values[2]);
			}
		}
        
	    private void Connect(string ip)
	    {
            netWorkHelper.Connect("192.168.137.1");
	    }

	    private void SendData(IReadOnlyList<float> sensorData)
	    {
            netWorkHelper.Send(string.Format("^{0}|{1}|{2}|1|1|1$", sensorData[0], sensorData[1], sensorData[2]));
	    }
	}
}


