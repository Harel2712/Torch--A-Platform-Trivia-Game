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
        Button back;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.XMLTopTen);
            
            string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
            var db = new SQLiteConnection(dpPath);// קריאה למסד הנתונים


            back = FindViewById<Button>(Resource.Id.btnstart);

            back.Click += Back_Click;


            List<LoginTable> users = db.Table<LoginTable>().ToList();// יצירת לליסט ממסד הנתונים
            users.Sort((a, b) => b.score.CompareTo(a.score)); // סידור הליסט מהגדול לקטן

            users = users.Take(10).ToList();// קיצור הליסט רק לעשרת השחקנים עם הניקוד הכי גבוה (TOPTEN)

            listnames = FindViewById<ListView>(Resource.Id.listview);
            
            UserAdapter adapter = new UserAdapter(this, users);// יצירת אובייקט של האדפטר להצגה בליסט ויו

            listnames.Adapter = adapter;// השמה של האדפטר בליסט ויו
         












        }

        private void Back_Click(object sender, EventArgs e)
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());// סגירת האפליקציה


        }
    }
}