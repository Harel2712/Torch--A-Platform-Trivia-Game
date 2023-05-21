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
        TextView questText;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.QuestionXML);
            opt1 = FindViewById<Button>(Resource.Id.option1);
            opt2 = FindViewById<Button>(Resource.Id.option2);
            opt3 = FindViewById<Button>(Resource.Id.option3);
            opt4 = FindViewById<Button>(Resource.Id.option4);
            questText = FindViewById<TextView>(Resource.Id.questText);
            // הגדרת שאלות
            ClassQuestion firstquestion = new ClassQuestion("you're on a astroeid and don't have water where can you get some?", "You Can't!", "Go to the nearest planet", "The Astroied is made of water", "pray", 3);
            ClassQuestion secondquestion = new ClassQuestion("is?", "yes", "no", "no", "no", 1);


            // קבלת מספר השאלה מהאינטנט
            int num = Intent.GetIntExtra("number",-1);
            if (num == 1)
            {
                question1 = firstquestion;
            } else if (num == 2)
            {
                question1 = secondquestion;

            }

            questText.Text = question1.question;
            opt1.Text = question1.option1;
            opt2.Text = question1.option2;
            opt3.Text = question1.option3;
            opt4.Text = question1.option4;
            
            opt1.Click += Opt1_Click;
            opt2.Click += Opt2_Click;
            opt3.Click += Opt3_Click;
            opt4.Click += Opt4_Click;
        }

        private void Opt4_Click(object sender, EventArgs e)
        {
            if (question1.RightAnswer == 4)
            {
                AddScore(3);

            }
            else
            {
                AddScore(0);
            }
        }

        private void Opt3_Click(object sender, EventArgs e)
        {
            if (question1.RightAnswer == 3)
            {
               
                AddScore(2);
            

            }
            else
            {
                AddScore(0);
            }
        }

        private void Opt2_Click(object sender, EventArgs e)
        {
            if (question1.RightAnswer == 2)
            {
                AddScore(3);

            }
            else
            {
                AddScore(0);
            }
        }

        private void Opt1_Click(object sender, EventArgs e)
        {
            if (question1.RightAnswer == 1)
            {
                AddScore(3);

            }
            else
            {
                AddScore(0);
            }
        }
        public void AddScore(int score)// פונקציה להוספת ניקוד ואינטנט חזרה למשחק
        {
            Intent intent = new Intent(this, typeof(MainActivity));

            string User_Name = Intent.GetStringExtra("UNAME");

            string dpPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");// קריאה לדאטהבייס
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<LoginTable>();// קריאה לטבלה
            var data1 = data.Where(x => x.username == User_Name).FirstOrDefault();// התאמת המשתמש המחובר

            data1.CurrentScore +=score;// הוספת ניקוד למשתמש
            if (data1.CurrentScore > data1.score)
            {
                data1.score = data1.CurrentScore;
                db.Update(data1);// עדכון בדאטהבייס

            }
            db.Update(data1);
            intent.SetFlags(ActivityFlags.ReorderToFront); // אינטנט בלי לאתחל את המסך אליו הוא מגיע
            StartActivity(intent);
        }
    }
}