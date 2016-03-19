using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.Widget;
using Android.OS;
using Android.Views;
using Andrule.Network;
using Andrule.View;

namespace Andrule
{
    [Activity(Label = "Andrule", MainLauncher = true, Icon = "@mipmap/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class MainActivity : TabActivity
    {

        private string _connectionIp;
        private bool _isConnected;
        private Button connectBtn;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            CreateTab(typeof(SetupActivity), "setup", "Setup");
            CreateTab(typeof(WheelActivity), "wheel", "Wheel");
        }

        private void CreateTab(Type activityType, string tag, string label)
        {
            var intent = new Intent(this, activityType);
            intent.AddFlags(ActivityFlags.NewTask);

            var spec = TabHost.NewTabSpec(tag);
            spec.SetIndicator(label);
            spec.SetContent(intent);

            TabHost.AddTab(spec);
        }
    }
}


