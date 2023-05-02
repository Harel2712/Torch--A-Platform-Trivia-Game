using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using canproj.Resources;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Xamarin.Essentials.Platform;

namespace canproj
{
    [Activity(Label = "ActivityTopTen")]
    public class ActivityTopTen : Activity
    {
        ListView listnames;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.XMLTopTen);

            string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
            var db = new SQLiteConnection(dpPath);
            db.CreateTable<User>();
            string name = Intent.GetStringExtra("UNAME");
            User thisUser = new User(name);
            User existingUser = db.Table<User>().Where(u => u.User_Name == thisUser.User_Name).FirstOrDefault();
            if (existingUser == null)
            {
                db.Insert(thisUser);
            }
              

            List<User> users = db.Table<User>().ToList();
            users.Sort((a, b) => b.Best_Score.CompareTo(a.Best_Score));

            listnames = FindViewById<ListView>(Resource.Id.listview);
            
            UserAdapter adapter = new UserAdapter(this, users);

            listnames.Adapter = adapter;
         












        }

       
    }
}