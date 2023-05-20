using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using canproj.Resources;
using canproj.Resources.layout;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using static Xamarin.Essentials.Platform;
using Context = Android.Content.Context;
using Intent = Android.Content.Intent;


namespace canproj
{
    [Activity(Label = "StartActivity")]
    public class StartActivity : Activity
    {
        Button move;
        Animation animFadeIn;
        ImageView ImageView;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.StartXML);
            move = FindViewById<Button>(Resource.Id.btnRight);
            animFadeIn = AnimationUtils.LoadAnimation(this, Resource.Animation.anim1);
            ImageView = FindViewById<ImageView>(Resource.Id.imageView1);
            ImageView.StartAnimation(animFadeIn);
            move.Click += Move_Click;


            BatteryManager batteryManager = (BatteryManager)GetSystemService(BatteryService);
            int batteryLevel = batteryManager.GetIntProperty((int)BatteryProperty.Capacity);



            Toast.MakeText(this, "Battery Level: " + 0, ToastLength.Short).Show();

            if (batteryLevel<21) {
                alertbtry(batteryLevel);
                Toast.MakeText(this, "yes", ToastLength.Short);

            }

            string name = Intent.GetStringExtra("NAME");
            string pass = Intent.GetStringExtra("PASS");
           
            string dpPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<LoginTable>(); //Call Table  
            var data1 = data.Where(x => x.username == name ).FirstOrDefault();
            data1.CurrentScore = 0;
            db.Update(data1);
        }

        private void Move_Click(object sender, EventArgs e)
        {
            string name = Intent.GetStringExtra("NAME");
            Intent move = new Intent();
            move.PutExtra("UNAME", name);
            move.SetClass(this, typeof(MainActivity));
            this.StartActivity(move);
        }

        public void alertbtry(int batterylvl)
        {
           
                AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
            alertDiag.SetTitle("Low battery");
            alertDiag.SetMessage("your phone's battery is low("+batterylvl + ") are you sure you wants to play?");
       
            alertDiag.SetCancelable(true);

            alertDiag.SetPositiveButton("yep!", (senderAlert, args)
            => {
                string name = Intent.GetStringExtra("NAME");
                Intent move = new Intent();
                move.PutExtra("UNAME", name);
                move.SetClass(this, typeof(MainActivity));
                this.StartActivity(move);
            });

            alertDiag.SetNegativeButton("no", (senderAlert, args)
            => {
                alertDiag.Dispose();
                Intent move = new Intent(this, typeof(LoginActivity));
                StartActivity(move);
            });

            Dialog diag = alertDiag.Create();
           
                diag.Show();
            

        }
       

    }
}