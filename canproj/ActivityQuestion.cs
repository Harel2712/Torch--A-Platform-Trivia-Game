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
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;

namespace canproj
{
    [Activity(Label = "ActivityQuestion")]
    public class ActivityQuestion : Activity
    {
        Button opt1,opt2,opt3,opt4;
        ClassQuestion question1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.QuestionXML);
            opt1 = FindViewById<Button>(Resource.Id.option1);
            opt2 = FindViewById<Button>(Resource.Id.option2);
            opt3 = FindViewById<Button>(Resource.Id.option3);
            opt4 = FindViewById<Button>(Resource.Id.option4);
            ClassQuestion firstquestion = new ClassQuestion("is?", "no", "no", "yes", "no", 3);
            ClassQuestion secondquestion = new ClassQuestion("is?", "yes", "no", "no", "no", 1);

            int num = Intent.GetIntExtra("number",-1);
            if (num == 1)
            {
                question1 = firstquestion;
            } else if (num == 2)
            {
                question1 = secondquestion;

            }

            opt1.Text = question1.option1;
            opt2.Text = question1.option2;
            opt3.Text = question1.option3;
            opt4.Text = question1.option4;
            
            opt1.Click += Opt1_Click;
            opt2.Click += Opt2_Click;
            opt3.Click += Opt3_Click;
            opt4.Click += Opt4_Click;
            // Create your application here
        }

        private void Opt4_Click(object sender, EventArgs e)
        {
            if (question1.RightAnswer == 4)
            {
                AddScore(3);

            }
        }

        private void Opt3_Click(object sender, EventArgs e)
        {
            if (question1.RightAnswer == 3)
            {
               
                AddScore(2);
            

            }
        }

        private void Opt2_Click(object sender, EventArgs e)
        {
            if (question1.RightAnswer == 2)
            {
                AddScore(3);

            }
        }

        private void Opt1_Click(object sender, EventArgs e)
        {
            if (question1.RightAnswer == 1)
            {
                AddScore(3);

            }
        }
        public void AddScore(int score)
        {
            Intent intent = new Intent(this, typeof(MainActivity));

            string User_Name = Intent.GetStringExtra("UNAME");

            string dpPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<LoginTable>();
            var data1 = data.Where(x => x.username == User_Name).FirstOrDefault();

            data1.CurrentScore +=score;
            if (data1.CurrentScore > data1.score)
            {
                data1.score = data1.CurrentScore;
                db.Update(data1);

            }
            db.Update(data1);
            intent.SetFlags(ActivityFlags.ReorderToFront); // אינטנט בלי לאתחל את המסך אליו הוא מגיע
            StartActivity(intent);
        }
    }
}