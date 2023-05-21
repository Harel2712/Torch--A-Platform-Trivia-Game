using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace canproj.Resources.layout
{

    [BroadcastReceiver(Enabled = true, Exported = true)]// הוספה למניפסט
    [IntentFilter(new[] { Intent.ActionBatteryChanged })]// מעביר באינטנט את אחוז הבטריה כאשר היא משתנה

    public class BroadcastBattery : BroadcastReceiver// ירושה מברודקאסטרסיבר
    {
        public TextView tv;

       

        public BroadcastBattery()// empty constuctor
        {
        }
        public BroadcastBattery(TextView tv)// constuctor, gets a textview to show the battery level
        {
            this.tv = tv;
        }
        public override void OnReceive(Context context, Intent intent)
        {



            int bl = intent.GetIntExtra("level", 0);// מקבל אחוזי בטריה

            if (tv != null)
            {
                tv.Text = "battery level: " + bl.ToString() + "%";// מציג בטקסט ויו
            }

        }
      
       


    }
}