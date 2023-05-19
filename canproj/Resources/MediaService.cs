using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace canproj.Resources
{
    [Service]
    internal class MediaService : Service
    {
        private MediaPlayer player;
        public override IBinder OnBind(Intent intent) // חובה להחזיר
        {
            return null;
        }
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(
Intent intent,
[GeneratedEnum] StartCommandFlags flags,
int startId)
        {
            base.OnStartCommand(intent, flags, startId);

            Toast.MakeText(this, "Loading",
ToastLength.Short).Show();

            // Thread הפעלת 
            Task.Run(() =>
            {
                // טעינת הקובץ
                player = MediaPlayer.Create(this,Resource.Raw.OST);

                // הגדרה שהמנגינה תחזור על עצמה
                player.Looping = true;

                // הפעלת הנגן
                player.Start();
            });

            // ראה הסבר בהמשך
            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            player.Stop();
        }
    }
}