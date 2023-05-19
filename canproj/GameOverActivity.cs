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

            Intent mintent = new Intent(this, typeof(MediaService));
            StopService(mintent);


            restart = FindViewById<Button>(Resource.Id.btnRestart);
            restart.Click += Restart_Click;

            string name = Intent.GetStringExtra("UNAME");

            string dpPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<LoginTable>(); //Call Table  
            var data1 = data.Where(x => x.username == name).FirstOrDefault();


            
            NotificationChannel channel = new NotificationChannel("channel_id", "Channel Name", NotificationImportance.Default);
            NotificationManager notificationManager = (NotificationManager)this.GetSystemService(Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, "channel_id")
            .SetSmallIcon(Resource.Drawable.torch)
            .SetContentTitle("Hi!")
            .SetContentText("Come back to play")
            .SetAutoCancel(true);

            Notification notification = builder.Build();
            notificationManager.Notify(1, notification);

            //alarm manager use- יצירת הודעה שהמתשמש קרוב לשבור את השיא שלו אחרי שעה שהוא משחק
            Intent intent = new Intent(this, typeof(NotificationReceiver));
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
            AlarmManager alarmManager = (AlarmManager)this.GetSystemService(AlarmService);
            var calendar = Calendar.Instance;
            calendar.TimeInMillis = Java.Lang.JavaSystem.CurrentTimeMillis();
            calendar.Add(CalendarField.Hour, 1);
            if (data1.score - data1.CurrentScore < 5)
            {
                alarmManager.SetExact(AlarmType.RtcWakeup, calendar.TimeInMillis, pendingIntent);
            }

            data1.CurrentScore = 0;
            db.Update(data1);


           
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