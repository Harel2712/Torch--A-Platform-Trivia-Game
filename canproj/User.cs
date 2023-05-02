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
using System.Linq;
using System.Text;
using static Java.Util.Jar.Attributes;

namespace canproj
{
    internal class User
    {
        public User()
        {
        }

        public User(string user_Name)
        {
            User_Name = user_Name;

            string dpPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<LoginTable>(); //Call Table  
            var data1 = data.Where(x => x.username == user_Name).FirstOrDefault();
            this.Best_Score = data1.score;
        }

        public string User_Name { get; set; }
        public int Best_Score { get; set; }

    }
}