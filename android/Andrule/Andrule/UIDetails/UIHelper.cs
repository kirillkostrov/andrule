using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Andrule.UIDetails
{
    static public class UIHelper
    {
        static public void ShowMessage(string message, Context context)
        {
            var dialog = new AlertDialog.Builder(context);
            dialog.SetMessage(message);
            dialog.SetNegativeButton("Cancel", (s, e) => { });
            dialog.Create().Show();
        }
    }
}