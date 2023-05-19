﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Java.Util.Jar.Attributes;

namespace canproj
{
    [BroadcastReceiver(Enabled = true, Exported = true)]

    internal class NotificationReceiver: BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            NotificationChannel channel = new NotificationChannel("channel_id", "Channel Name", NotificationImportance.Default);
            NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context, "channel_id")
            .SetSmallIcon(Resource.Drawable.torch)
            .SetContentTitle("Hello!")
            .SetContentText("You are close to beat your all time best score!")
            .SetAutoCancel(true);

            Notification notification = builder.Build();
            notificationManager.Notify(1, notification);

        }
    }
}