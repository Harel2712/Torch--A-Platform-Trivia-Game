﻿using Android.App;
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
        Button move,top;
        Animation animFadeIn;
        ImageView ImageView;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.StartXML);
            move = FindViewById<Button>(Resource.Id.btnRight);
            top = FindViewById<Button>(Resource.Id.btntop);
            animFadeIn = AnimationUtils.LoadAnimation(this, Resource.Animation.anim1);// תמונה לאנימציה
            ImageView = FindViewById<ImageView>(Resource.Id.imageView1);
            ImageView.StartAnimation(animFadeIn);// התחלת אנימציה פייד אין של לוגו המשחק
            move.Click += Move_Click;
            top.Click += Top_Click;


            BatteryManager batteryManager = (BatteryManager)GetSystemService(BatteryService);// שימוש בבטריה
            int batteryLevel = batteryManager.GetIntProperty((int)BatteryProperty.Capacity);// קבלת אחוזי הבטריה



          
            // אם אחוזי הבטריה נמוכים הופעת דיאלוג
            if (batteryLevel<21) {
                alertbtry(batteryLevel);
                
            }

            string name = Intent.GetStringExtra("NAME");
            string pass = Intent.GetStringExtra("PASS");
           
            string dpPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<LoginTable>(); //Call Table  
            var data1 = data.Where(x => x.username == name ).FirstOrDefault();
            data1.CurrentScore = 0;// החזרת הניקוד העכשווי ל0
            db.Update(data1);
        }

        private void Top_Click(object sender, EventArgs e)// כפתור מעבר לטופ 10
        {
           Intent intent = new Intent(this,typeof(ActivityTopTen));
            this.StartActivity(intent);
           
        }

        private void Move_Click(object sender, EventArgs e)// אינטנט מעבר למשחק
        {
            string name = Intent.GetStringExtra("NAME");
            Intent move = new Intent();
            move.PutExtra("UNAME", name);
            move.SetClass(this, typeof(MainActivity));
            this.StartActivity(move);
        }

        public void alertbtry(int batterylvl)// פונקציה היוצרת דיאלוג
        {
           
                AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);// בניית דיאלוג אלרט
            alertDiag.SetTitle("Low battery");// כותרת
            alertDiag.SetMessage("your phone's battery is low("+batterylvl + ") are you sure you wants to play?"); // הודעה האם רוצים לשחק למרות הבטריה נמוכה
       
            alertDiag.SetCancelable(true);

            alertDiag.SetPositiveButton("yep!", (senderAlert, args)// כאשר לוחצים על כן מעבר למשחק
            => {
                string name = Intent.GetStringExtra("NAME");
                Intent move = new Intent();
                move.PutExtra("UNAME", name);
                move.SetClass(this, typeof(MainActivity));
                this.StartActivity(move);
            });

            alertDiag.SetNegativeButton("no", (senderAlert, args)// כאשר לוחצים על לא מעבר חזרה למסך הרשמה
            => {
                alertDiag.Dispose();
                Intent move = new Intent(this, typeof(LoginActivity));
                StartActivity(move);
            });

            Dialog diag = alertDiag.Create();
           
                diag.Show();// הופעת הדיאלוכ
            

        }
       

    }
}