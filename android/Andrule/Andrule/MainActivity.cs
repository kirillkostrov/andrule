using System;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.Widget;
using Android.OS;

namespace Andrule
{
	[Activity (Label = "Andrule", MainLauncher = true, Icon = "@mipmap/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
	public class MainActivity : Activity
	{
        private SimulationListener _simulationListener;
	    private TextView _sensorTextView;
	    private object _syncLock = new object();

        protected override void OnCreate (Bundle bundle)
		{
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            
            _sensorTextView = FindViewById<TextView>(Resource.Id.CoordinateText);
            var sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            _simulationListener = new SimulationListener(sensorManager);
        }

        protected override void OnResume()
        {
            base.OnResume();
            _simulationListener.StartSimulation();
        }
        protected override void OnPause()
        {
            base.OnPause();
            _simulationListener.StopSimulation();
        }
	}
}


