using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace canproj.Resources.layout
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        EditText txtusernamer;
        EditText txtpasswordr;
        Button btncreater;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RegisterXML);
            btncreater = FindViewById<Button>(Resource.Id.btncreater);
            txtusernamer = FindViewById<EditText>(Resource.Id.usernamer);
            txtpasswordr = FindViewById<EditText>(Resource.Id.passwordr);
            btncreater.Click += Btncreate_Click;
            // Create your application here
        }

        private void Btncreate_Click(object sender, EventArgs e)
        {
            try
            {
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
                var db = new SQLiteConnection(dpPath);
                db.CreateTable<LoginTable>();
                LoginTable tbl = new LoginTable();
                tbl.username = txtusernamer.Text;
                tbl.password = txtpasswordr.Text;
                tbl.score = 0;
                tbl.CurrentScore = 0;
                db.Insert(tbl);
                Toast.MakeText(this, "Record Added Successfully...,", ToastLength.Short).Show();
                Finish();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
        }
    }
}