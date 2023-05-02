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

    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { Intent.ActionBatteryChanged })]

    public class BroadcastBattery : BroadcastReceiver
    {
        public TextView tv;
        public int batteryLevel { get; set; }

        public BroadcastBattery()
        {
        }
        public BroadcastBattery(TextView tv)
        {
            this.tv = tv;
        }
        public override void OnReceive(Context context, Intent intent)
        {



            int bl = intent.GetIntExtra("level", 0);
            
            this.batteryLevel = bl;
            
            if (tv != null)
            {
                tv.Text = tv.Text+ bl +"%";
            }





        }
      
       


    }
}