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





            List<LoginTable> users = db.Table<LoginTable>().ToList();
            users.Sort((a, b) => b.score.CompareTo(a.score));

            listnames = FindViewById<ListView>(Resource.Id.listview);
            
            UserAdapter adapter = new UserAdapter(this, users);

            listnames.Adapter = adapter;
         












        }

       
    }
}