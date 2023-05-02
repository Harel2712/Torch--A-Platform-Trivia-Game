using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using canproj.Resources;
using canproj.Resources.layout;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace canproj
{
    [Activity(Label = "LoginActivity", MainLauncher = true)]
    public class LoginActivity : Activity
    {
        EditText txtusername;
        EditText txtPassword;
        Button btncreate;
        Button btnsign;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.LoginXML);
            btnsign = FindViewById<Button>(Resource.Id.btnsign);
            btncreate = FindViewById<Button>(Resource.Id.btncreate);
            txtusername = FindViewById<EditText>(Resource.Id.txtusername);
            txtPassword = FindViewById<EditText>(Resource.Id.txtpassword);
            btnsign.Click += Btnsign_Click;
            btncreate.Click += Btncreate_Click;
        }

        private void Btncreate_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
        }

        private void Btnsign_Click(object sender, System.EventArgs e)
        {
            try
            {
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Call Database  
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<LoginTable>(); //Call Table  
                var data1 = data.Where(x => x.username == txtusername.Text && x.password == txtPassword.Text).FirstOrDefault(); //Linq Query  
                if (data1 != null)
                {
                   
                    Toast.MakeText(this, "Login Success", ToastLength.Short).Show();

                    Intent intent = new Intent();
                    intent.PutExtra("NAME",data1.username);
                    intent.PutExtra("PASS",data1.password);
                    intent.SetClass(this,typeof(StartActivity));
                    this.StartActivity(intent);

                   
                }
                else
                {
                    Toast.MakeText(this, "Username or Password invalid", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public string CreateDB()
        {
            var output = "";
            output += "Creating Databse if it doesnt exists";
            string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Create New Database  
            var db = new SQLiteConnection(dpPath);
            output += "\n Database Created....";
            return output;
        }
    }
}
