﻿using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.Widget;
using Android.OS;
using Andrule.Network;

namespace Andrule
{
	[Activity (Label = "Andrule", MainLauncher = true, Icon = "@mipmap/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
	public class MainActivity : Activity
	{

        private SimulationListener _simulationListener;
	    private TextView _sensorTextView;
	    private object _syncLock = new object();
	    private NetWorkHelper netWorkHelper;


        protected override void OnCreate (Bundle bundle)
		{
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            
            _sensorTextView = FindViewById<TextView>(Resource.Id.CoordinateText);

            var sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            _simulationListener = new SimulationListener(sensorManager);

            netWorkHelper = new NetWorkHelper();
            Connect("123");
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


