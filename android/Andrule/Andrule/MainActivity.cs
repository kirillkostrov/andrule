using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Andrule.Views;

namespace Andrule
{
    [Activity(Label = "Andrule", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class MainActivity : TabActivity
    {
        public static TabHost Tabs { get; private set;}

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            Tabs = TabHost;

            CreateTab(typeof(SetupActivity), "setup", "SETUP");
            CreateTab(typeof(WheelActivity), "wheel", "WHEEL");
        }

        private void CreateTab(Type activityType, string tag, string label)
        {
            var intent = new Intent(this, activityType);
            intent.AddFlags(ActivityFlags.NewTask);

            var spec = Tabs.NewTabSpec(tag);
			spec.SetIndicator(label);
            spec.SetContent(intent);

            Tabs.AddTab(spec);
        }
    }
}


