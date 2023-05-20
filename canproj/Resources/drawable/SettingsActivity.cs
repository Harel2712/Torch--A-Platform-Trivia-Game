using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using canproj.Resources.layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace canproj.Resources.drawable
{
    [Activity(Label = "SettingsActivity")]
    public class SettingsActivity : Activity
    {
        SeekBar seekbar;
        AudioManager audioManager;
        Button retrn;
        TextView btryTv;
        BroadcastBattery bdBattery;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SettingsXML);


            seekbar = FindViewById<SeekBar>(Resource.Id.sb);

            btryTv = FindViewById<TextView>(Resource.Id.textViewBtry);
            bdBattery=new BroadcastBattery(btryTv);

            audioManager =(AudioManager)GetSystemService(Context.AudioService);
            int max = audioManager.GetStreamMaxVolume(Stream.Music);
            seekbar.Max = max;
            audioManager.SetStreamVolume(Stream.Music, 5,0);
            seekbar.ProgressChanged += Seekbar_ProgressChanged;
            retrn = FindViewById<Button>(Resource.Id.rtrn);
            retrn.Click += Retrn_Click;


        }

        private void Retrn_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
          
            intent.SetFlags(ActivityFlags.ReorderToFront); // אינטנט בלי לאתחל את המסך אליו הוא מגיע
            StartActivity(intent);
        }

        private void Seekbar_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            audioManager.SetStreamVolume(Stream.Music, e.Progress, VolumeNotificationFlags.PlaySound);
        }

        protected override void OnResume()
        {
            base.OnResume();
           
            RegisterReceiver(bdBattery, new IntentFilter(Intent.ActionBatteryChanged));
        }
        protected override void OnPause()
        {
            UnregisterReceiver(bdBattery);
            base.OnPause();
        }
    }
}