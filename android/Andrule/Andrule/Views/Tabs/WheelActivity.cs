using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Andrule.View
{
    [Activity]
    public class WheelActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            TextView textview = new TextView(this) { Text = "This is the WheelActivity tab" };
            SetContentView(textview);
        }
    }
}