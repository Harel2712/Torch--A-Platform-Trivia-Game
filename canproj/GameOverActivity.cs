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
    [Activity(Label = "GameOverActivity", Name = "android.permission.SCHEDULE_EXACT_ALARM")]// הוספה למניפסט הרשאה ליצירת הודעה
    public class GameOverActivity : Activity
    {
        Button restart;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GameOverXML);

            // עצירת סרוויס מוזיקה
            Intent mintent = new Intent(this, typeof(MediaService));
            StopService(mintent);


            restart = FindViewById<Button>(Resource.Id.btnRestart);
            restart.Click += Restart_Click;

            string name = Intent.GetStringExtra("UNAME");

            string dpPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<LoginTable>(); //Call Table  
            var data1 = data.Where(x => x.username == name).FirstOrDefault();


            // יצירת הודעה
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
            Intent intent = new Intent(this, typeof(NotificationReceiver));// שימוש במחלקת הודעות
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);// שימוש באינטנט המחכה להפעלת הברודקאסט של הודעה שיצרתי
            AlarmManager alarmManager = (AlarmManager)this.GetSystemService(AlarmService);// יצירת אלרם מנג'ר
            var calendar = Calendar.Instance;// יצירת משתנה של זמן
            calendar.TimeInMillis = Java.Lang.JavaSystem.CurrentTimeMillis();
            calendar.Add(CalendarField.Hour, 1);// קביעה מתי להפעיל את ההודעה- כעבור שעה
            if (data1.score - data1.CurrentScore < 5)// התנאי להפעלוץ ההודעה- אם המשתמש קרוב לשבור שיא
            {
                alarmManager.SetExact(AlarmType.RtcWakeup, calendar.TimeInMillis, pendingIntent);// הפעלת הודעה עם אלארם מנג'ר
            }

            data1.CurrentScore = 0;
            db.Update(data1);


           
        }

        private void Restart_Click(object sender, EventArgs e)// מעבר חזרה לתחילת המשחק
        {
            string name = Intent.GetStringExtra("UNAME");
            Intent move = new Intent();
            move.PutExtra("UNAME", name);
            move.SetClass(this, typeof(MainActivity));
            this.StartActivity(move);
        }
    }
}