using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using canproj.Resources;
using Java.Util;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using Context = Android.Content.Context;

namespace canproj
{
    [Activity(Label = "GameOverActivity", Name = "android.permission.SCHEDULE_EXACT_ALARM")]
    public class GameOverActivity : Activity
    {
        Button restart;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GameOverXML);

            restart = FindViewById<Button>(Resource.Id.btnRestart);
            restart.Click += Restart_Click;

            string name = Intent.GetStringExtra("UNAME");

            string dpPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<LoginTable>(); //Call Table  
            var data1 = data.Where(x => x.username == name).FirstOrDefault();

   
            Intent intent = new Intent(this, typeof(NotificationReceiver));
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            Intent intent2 = new Intent(this, typeof(NotificationReceiver));
            PendingIntent pendingIntent2 = PendingIntent.GetBroadcast(this, 0, intent2, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            AlarmManager alarmManager = (AlarmManager)this.GetSystemService(AlarmService);
            AlarmManager alarmManager2 = (AlarmManager)this.GetSystemService(AlarmService);

            //alarmManager.SetRepeating(AlarmType.RtcWakeup, SystemClock.ElapsedRealtime() , 1000, pendingIntent);
            var calendar = Calendar.Instance;
            calendar.TimeInMillis = Java.Lang.JavaSystem.CurrentTimeMillis();
            calendar.Add(CalendarField.Minute, 1);
            long firstAlarmTime = calendar.TimeInMillis;

            var currentTimeMillis = Java.Lang.JavaSystem.CurrentTimeMillis();

            var calendar2 = Calendar.Instance;
                calendar2.Add(CalendarField.HourOfDay, 1);
            long secondAlarmTime = calendar2.TimeInMillis;



            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, currentTimeMillis, pendingIntent);

            //alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, firstAlarmTime, pendingIntent);
            //alarmManager2.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, secondAlarmTime, pendingIntent2);


















            data1.CurrentScore = 0;
            db.Update(data1);


            // Create your application here
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            string name = Intent.GetStringExtra("UNAME");
            Intent move = new Intent();
            move.PutExtra("UNAME", name);
            move.SetClass(this, typeof(MainActivity));
            this.StartActivity(move);
        }
    }
}