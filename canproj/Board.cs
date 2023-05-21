using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using canproj.Resources;
using canproj.Resources.drawable;
using SQLite;
using System;
using System.Threading;
using static Java.Util.Jar.Attributes;
using Intent = Android.Content.Intent;

namespace canproj
{
    internal class Board : View
    {
        Context context;
        Intent intent;
       

        Arrow rightarrow, leftarrow,jumparrow;// חצים
        Bitmap platform, settings, spacebg; // ציור פלטפורמה והגדרות
        float del;//מהירות קפיצה
    
        bool WouldJump;// משתנה האם קופץ
        Pit pit1,pit2,pit3; //בורות
        Coin coin1, InfoTreasure1, Quest1, Bigcoin,Quest2, coin3;// מטבעות ושאלות (דברים שנותנים ניקוד
        int score;// ניקוד
        Hero hero;// דמות ראשית
        Monster m1,m2;// מפלצות
    
        MediaPlayer coined,jumped;// סאונדים
        int counter;// מהירות התזוזה של הפלטפורמה
      
        int rBound;//קצה אחד של הפלטפרומה
        int lBound;// קצה שני של הלפטפורמה
        string dpPath;// מחרוזת לקריאה למסד נתונים
        string User_Name;

        public Board(Context context, Intent intent) : base(context)
        {

            // הזנת ערכים במשתנים/ בניית אובייקטים
            
            rBound = 0;
            lBound = 900;
            score=0;
            this.context = context;
            this.intent = intent;
            counter = 0;
            del = 6;
            WouldJump = false;
            
            coined = MediaPlayer.Create(context, Resource.Raw.videoplayback);
            jumped = MediaPlayer.Create(context, Resource.Raw.jumpsfx);
          

            settings = BitmapFactory.DecodeResource(Resources, Resource.Drawable.settingsr);
            spacebg = BitmapFactory.DecodeResource(Resources, Resource.Drawable.spacebg);


            m1 = new Monster(400, 580, BitmapFactory.DecodeResource(Resources, Resource.Drawable.mons),7,false);
            m2 = new Monster(1900, 580, BitmapFactory.DecodeResource(Resources, Resource.Drawable.mons), 7, false);     
            platform = BitmapFactory.DecodeResource(Resources, Resource.Drawable.spaceplatform);
            coin1 = new Coin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.coinew3), false, 300, 500);
            Bigcoin = new Coin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.coinew3), false, 2000, 500);
            coin3 = new Coin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.coinew3), true, 4000, 500);
            hero = new Hero(50, 500, BitmapFactory.DecodeResource(Resources, Resource.Drawable.torch));
            pit1 = new Pit( BitmapFactory.DecodeResource(Resources, Resource.Drawable.spacerec), 1700,650);
            pit2 = new Pit(BitmapFactory.DecodeResource(Resources, Resource.Drawable.spacerec), 2200, 650);
            pit3 = new Pit(BitmapFactory.DecodeResource(Resources, Resource.Drawable.spacerec), 3700, 650);
            InfoTreasure1 = new Coin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.treasureMap),false,600,550);
            Quest1 = new Coin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.treasure), false, 1000, 570);
            Quest2 = new Coin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.treasure), false, 3200, 570);
            rightarrow = new Arrow(2000, 740, BitmapFactory.DecodeResource(Resources, Resource.Drawable.rightt), true);
            leftarrow = new Arrow(1500, 740, BitmapFactory.DecodeResource(Resources, Resource.Drawable.leftt), true);
            jumparrow = new Arrow(20, 720, BitmapFactory.DecodeResource(Resources, Resource.Drawable.jumpp), true);

            // קבלת שם משתמש מאינטנט
            User_Name = intent.GetStringExtra("UNAME");
          






        }



        protected override void OnDraw(Canvas canvas) //ציור על הקנבס
        {


            //ציור האובייקטים על הקנבס
            canvas.DrawBitmap(spacebg, 0, 0, null);
            draw(canvas, platform, -counter, 580, canvas.Width, 500);

            canvas.DrawBitmap(settings, 0, 0, null);
            canvas.DrawBitmap(m1.GetMonster(), m1.GetX(), m1.GetY(), null);
            canvas.DrawBitmap(m2.GetMonster(), m2.GetX(), m2.GetY(), null);
           
            canvas.DrawBitmap(pit1.GetPit(), pit1.Getx(), pit1.Gety(), null);
            canvas.DrawBitmap(pit2.GetPit(), pit2.Getx(), pit2.Gety(), null);
            canvas.DrawBitmap(pit3.GetPit(), pit3.Getx(), pit3.Gety(), null);

            canvas.DrawBitmap(hero.GetHero(), hero.GetX(), hero.GetY(), null);

            canvas.DrawBitmap(coin1.Getcoin(), coin1.Getx(), coin1.Gety(), null);
            canvas.DrawBitmap(InfoTreasure1.Getcoin(), InfoTreasure1.Getx(), InfoTreasure1.Gety(), null);

            canvas.DrawBitmap(Quest1.Getcoin(), Quest1.Getx(), Quest1.Gety(), null);
            canvas.DrawBitmap(Bigcoin.Getcoin(), Bigcoin.Getx(), Bigcoin.Gety(), null);
            canvas.DrawBitmap(Quest2.Getcoin(),Quest2.Getx(), Quest2.Gety(), null);
            canvas.DrawBitmap(coin3.Getcoin(), coin3.Getx(), coin3.Gety(), null);
            canvas.DrawBitmap(leftarrow.arrow, leftarrow.x, leftarrow.y, null);
            canvas.DrawBitmap(rightarrow.arrow, rightarrow.x, rightarrow.y, null);

            canvas.DrawBitmap(jumparrow.arrow, jumparrow.x, jumparrow.y, null);

            //תיאום המשתמש מהדאטהבייס
            dpPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<LoginTable>(); //Call Table  
            User_Name = intent.GetStringExtra("UNAME");
            var data1 = data.Where(x=>x.username==User_Name).FirstOrDefault();
            if (data1 != null)
            {
                score = data1.CurrentScore;

            }

           

            // בדיקה האם מתנגש עם הבורות
            pitcollison(pit1);
            pitcollison(pit2);
            pitcollison(pit3);



            // בדיקה האם מתנגש עם מפלצת אחת והפעלת תהליכון עם פונקציית התנגשות
            if ((m1.GetX() > hero.GetX() + hero.GetHero().Width && m1.GetX() - 40 < hero.GetX() + hero.GetHero().Width) && (hero.GetY() == 500) && m1.getsquash() == false)
            {
                Thread th = new Thread(collision);
                th.Start();

            }
            // בדיקה האם המשתמש קופץ על המפלצת במידה וכן- הוספת ניקוד והפעלת תהליכון למחיקת הדמות
            else if ((m1.GetX() > hero.GetX() + 40 && m1.GetX()  < hero.GetX() + 60) && (hero.GetY() < 500 && hero.GetY() > 450))
            {
                if (m1.getsquash() == false)
                {
                    score = score + 2;

                    data1.CurrentScore = score;
                    db.Update(data1);

                    if (score > data1.score)
                    {
                        data1.score = score;
                        db.Update(data1);

                    }

                }


                m1.setSquash(true);
                Thread th = new Thread(squash1);
                 th.Start();



                m1.SetMonster(BitmapFactory.DecodeResource(Resources, Resource.Drawable.sqmons));
                m1.speed = 0;
            }
            // בדיקה האם מתנגש עם מפלצת שנייה והפעלת תהליכון עם פונקציית התנגשות
            if ((m2.GetX() > hero.GetX() + hero.GetHero().Width && m2.GetX() - 40 < hero.GetX() + hero.GetHero().Width) && (hero.GetY() == 500) && m2.getsquash() == false)
            {
                Thread th = new Thread(collision);
                th.Start();

            }
            // בדיקה האם המשתמש קופץ על המפלצת במידה וכן- הוספת ניקוד והפעלת תהליכון למחיקת הדמות

            else if ((m2.GetX() > hero.GetX() + 40 && m2.GetX() < hero.GetX() + 60) && (hero.GetY() < 500 && hero.GetY() > 450))
            {
                if (m2.getsquash() == false)
                {
                    score = score + 2;

                    data1.CurrentScore = score;
                    db.Update(data1);

                    if (score > data1.score)
                    {
                        data1.score = score;
                        db.Update(data1);

                    }

                }


                m2.setSquash(true);
                Thread th = new Thread(squash2);
                th.Start();



                m2.SetMonster(BitmapFactory.DecodeResource(Resources, Resource.Drawable.sqmons));
                m2.speed = 0;
            }

            // בדיקה האם מתנגש עם מטבע והוספת ניקוד אם כן
            if (hero.GetX() + 50 > coin1.Getx() && hero.GetX() + 50 < coin1.Getx() + 50 && coin1.Gety() == hero.GetY()) 
            {
                coined.Start();
               
                score = score +coin1.HowMuchScore();
                data1.CurrentScore = score;
                db.Update(data1);
              
                if (score > data1.score)
                {
                    data1.score =  score;
                    db.Update(data1);
                }
                coin1.Setcoin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.ppnng));

                coin1.setx(-999);
            }
            // בדיקה האם מתנגש עם מטבע והוספת ניקוד אם כן

            if (hero.GetX() + 50 > Bigcoin.Getx() && hero.GetX() + 50 < Bigcoin.Getx() + 50 && Bigcoin.Gety() == hero.GetY()) //מטבע
            {
                coined.Start();
                score = score +Bigcoin.HowMuchScore();
                data1.CurrentScore = score;
                db.Update(data1);

                if (score > data1.score)
                {
                    data1.score = score;
                    db.Update(data1);
                }
                Bigcoin.Setcoin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.ppnng));

                Bigcoin.setx(-999);
            }
            // בדיקה האם מתנגש עם מטבע והוספת ניקוד אם כן

            if (hero.GetX() + 50 > coin3.Getx() && hero.GetX() + 50 < coin3.Getx() + 50 && coin3.Gety() == hero.GetY()) //מטבע
            {
                coined.Start();
                score = score + coin3.HowMuchScore();
                data1.CurrentScore = score;
                db.Update(data1);

                if (score > data1.score)
                {
                    data1.score = score;
                    db.Update(data1);
                }
                coin3.Setcoin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.ppnng));

                coin3.setx(-999);
            }


            // ציור הקנבס
            base.OnDraw(canvas);
        
   
            //יצירת צבע
            Paint paint = new Paint();
            paint.SetARGB(255,255, 255, 255);
            paint.TextAlign = paint.TextAlign;
            paint.TextSize = 70;

            // יצירת תזוזה למפלצות
            m1.setx(m1.GetX() - m1.speed);
            m2.setx(m2.GetX() -m2.speed);

   

            // ציור טקסט על הקנבס
            canvas.DrawText("Score: " + score, canvas.Width / 2+5, 75, paint);
            canvas.DrawText("User: " + User_Name, canvas.Width / 2-500, 75, paint);
            canvas.DrawText("All Time Best Score: " + data1.score, canvas.Width / 2, 200, paint);



            // החלפת כיוון התנועה של המפלצות בכל פעם שמגיעים לקצה טווח התנועה
            if (m1.GetX() < rBound+400 && m1.speed >0 || (m1.GetX() > lBound && m1.speed<0 ))
                m1.speed = -m1.speed;
            if (m2.GetX() < rBound + 2500 && m2.speed > 0 || (m1.GetX() > lBound && m2.speed < 0))
                m2.speed = -m2.speed;



            // כאשר המשתנה שווה לנכון מפעיל קפיצה- עלייה עד לנקודה מסויימת וחזרה לנקודת ההתחלה
            if (WouldJump)
            {
                hero.SetHero(BitmapFactory.DecodeResource(Resources, Resource.Drawable.jumpingtorch));

                hero.SetY(hero.GetY() - del);
                if (hero.GetY() == 200)
                    del = -del;
                if (hero.GetY() == 500)
                {
                    WouldJump = false;
                    hero.SetHero(BitmapFactory.DecodeResource(Resources, Resource.Drawable.torch));

                    del = -del;
                }
            }

           

           



            // מעבר לאקטיבטי טופ 10 כשמגיעים לקצה המסך
            if (hero.GetX()==1910 || hero.GetX()==1911)
            {
                string name = intent.GetStringExtra("UNAME");
                Intent move = new Intent();
                move.PutExtra("UNAME", name);
                move.SetClass(context, typeof(ActivityTopTen));
                context.StartActivity(move);


            }



            Invalidate();
         

        }
        public override bool OnTouchEvent(MotionEvent e)// כאשר לוחצים
        {
           
            if (MotionEventActions.Move == e.Action)
            {
                int usx = (int)e.GetX();// הגדרת משתנה של מיקום איקס של האצבע
                int usy = (int)e.GetY();// הגדרת משתנה של מיקום ווי של האצבע


                // הפעלת פונקצייה שבודקת האם נוגע בשאלות
                touchquset(usx, usy, Quest1, 1);
                touchquset(usx, usy, Quest2, 2);


                // הפעלת הדיאלוג כאשר לוחצים במיקום המתאים
                if((usx >= InfoTreasure1.Getx()-60  && usx <= InfoTreasure1.Getx() +60) && ( usy <= InfoTreasure1.Gety()+70 && usy>= InfoTreasure1.Gety()-70) && (hero.GetX() >= InfoTreasure1.Getx() - 400 && hero.GetX() <= InfoTreasure1.Getx() + 400) && (hero.GetY() <= InfoTreasure1.Gety() + 70 && hero.GetY() >= InfoTreasure1.Gety() - 70))
                {

                    CustomDialog di = new CustomDialog(context);    
                    if (di.IsShowing==false) { di.Show(); }
                  




                }

                // בדיקה האם לוחצים על חץ ימינה והזזה של המסך בהתאם
                if ((usx - 70 >= rightarrow.x || usx + 70 >= rightarrow.x) && (usy - 50 >= rightarrow.y || usy - 10 >= rightarrow.y))
                {
                    hero.SetX(hero.GetX()+5);
                    hero.SetHero(BitmapFactory.DecodeResource(Resources, Resource.Drawable.torch));
                    counter = counter % Width;
                    counter = counter + 1;

                    pit1.setx(pit1.Getx()-10);
                    pit2.setx(pit2.Getx() - 10);
                    pit3.setx(pit3.Getx() - 10);

                    coin1.setx(coin1.Getx() - 10);
                    Bigcoin.setx(Bigcoin.Getx() - 10);
                    coin3.setx(coin3.Getx() - 10);

                    InfoTreasure1.setx(InfoTreasure1.Getx() - 10);
                    Quest1.setx(Quest1.Getx() - 10);
                    Quest2.setx(Quest2.Getx() - 10);

                    rBound -= 10;
                    lBound -= 10;

                   







                }
                // בדיקה האם לוחצים על חץ שמאלה והזזה של המסך בהתאם

                else if ((usx >= leftarrow.x - 500 || usx >= leftarrow.x - 500) && (usy - 50 >= leftarrow.y || usy - 10 >= leftarrow.y))
                {
                    hero.SetX(hero.GetX() - 5);


                    hero.SetHero(BitmapFactory.DecodeResource(Resources, Resource.Drawable.lefttorch));
                    counter = counter % Width;
                    counter = counter - 1;

                    pit1.setx(pit1.Getx() + 10);
                    pit2.setx(pit2.Getx() + 10);
                    pit3.setx(pit3.Getx() + 10);

                    coin1.setx(coin1.Getx() + 10);
                    Bigcoin.setx(Bigcoin.Getx() + 10);
                    coin3.setx(coin3.Getx() + 10);

                    InfoTreasure1.setx(InfoTreasure1.Getx() + 10);
                    Quest1.setx(Quest1.Getx() + 10);
                    Quest2.setx(Quest2.Getx() + 10);





                    rBound += 10;
                    lBound += 10;

                
                    




                }
                // בדיקה האם לוצים על כפתור לקפיצה והפיכת המשתנה לנכון כדי להפעיל את הקפיצה
                if ((usx - 100 <= jumparrow.x || usx + 100 <= jumparrow.x) && (usy - 80 >= jumparrow.y  || usy + 80 >= jumparrow.y))
                {
                    jumped.Start();

                    WouldJump = true;
                    


                }
                // מעבר למסך הגדרות כאשר לוחצים במקום של אייקון ההגדרות
                if((usx  <= 100 && usx  >= 0)&&(usy<=100 && usy>=0))
                {
                    Intent intent= new Intent(context, typeof(SettingsActivity));
                    context.StartActivity(intent);

                }
            }
            Invalidate();

            return true;
        }

       

        public static  void draw(Canvas canvas, Bitmap bitmap, int x, int y, int width, int height)//ציור אובייקט שזז על הקנבס
        {
            
            Rect source = new Rect(0, 0, bitmap.Width, bitmap.Height);
            Rect target = new Rect(x, y, x + width, y + height);
            canvas.DrawBitmap(bitmap, source, target, null);
        }
 
        public void touchquset(int usx, int usy, Coin quest, int number)// בדיקה האם נוגע בכפתור שאלה ואם כן מעבר לאקטיבי שאלה
        {
            if ((usx >= quest.Getx() - 60 && usx <= quest.Getx() + 60) && (usy <= quest.Gety() + 70 && usy >= quest.Gety() - 70) && (hero.GetX() >= quest.Getx() - 400 && hero.GetX() <= quest.Getx() + 400) && (hero.GetY() <= quest.Gety() + 70 && hero.GetY() >= quest.Gety() - 70))
            {
                string name = intent.GetStringExtra("UNAME");
                Intent move = new Intent();
                move.PutExtra("UNAME", name);
                move.PutExtra("number", number);
                move.SetClass(context, typeof(ActivityQuestion));
                context.StartActivity(move);


            }
        }

        

        public void squash1()// מעיכה של המפלצת הראשונה אחרי שקופצים עלייה הוספת נקודות
        {
            Thread.Sleep(500);
            m1.SetMonster(BitmapFactory.DecodeResource(Resources, Resource.Drawable.ppnng));
            m1.setx(-999);

            dpPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<LoginTable>();
            var data1 = data.Where(x => x.username == User_Name).FirstOrDefault();

            data1.CurrentScore = score;
            db.Update(data1);

            if (score > data1.score)
            {
                data1.score = score;
                db.Update(data1);

            }


        }

        public void squash2()// מעיכה של המפלצת השנייה אחרי שקופצים עלייה הוספת נקודות
        {
            Thread.Sleep(500);
            m2.SetMonster(BitmapFactory.DecodeResource(Resources, Resource.Drawable.ppnng));
            m2.setx(-999);

            dpPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<LoginTable>();
            var data1 = data.Where(x => x.username == User_Name).FirstOrDefault();

            data1.CurrentScore = score;
            db.Update(data1);

            if (score > data1.score)
            {
                data1.score = score;
                db.Update(data1);

            }


        }

        


        public void pitcollison(Pit pit)// פונקציה הבודקת האם התנגש עם בור
        {
            if (hero.GetY() == 500 && (hero.GetX() + 50 > pit.Getx() && hero.GetX() + 50 < pit.Getx() + 200) || hero.GetY() == 500 && (hero.GetX() + 50 > pit.Getx() && hero.GetX() + 50 < pit.Getx() + 200))
            {
                while (hero.GetY() < 860)
                {
                    hero.SetY(hero.GetY() + 1);

                }
                if (hero.GetY() == 860)
                {
                    collision();
                }

            }
        }
        public void collision()// פונקציה הקורית אחרי התנגשות, מעבר למסך סוף משחק
        {
            string name = intent.GetStringExtra("UNAME");
            Intent move = new Intent();
            move.PutExtra("UNAME", name);
            move.SetClass(context, typeof(GameOverActivity));
            context.StartActivity(move);

            hero.SetHero(BitmapFactory.DecodeResource(Resources, Resource.Drawable.ppnng));
            hero.SetX(-999);







        }
    }
}