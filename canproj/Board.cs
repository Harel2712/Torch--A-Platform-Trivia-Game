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
        Bitmap rightarrw, leftarrw, jumparw;

      
       
        float xr, yr;
        Bitmap monster1, monster2, monster3, platform, settings;
        float del, deltay;
        Random rand = new Random();
        int randx, monster_delta;
        float jx;
        bool WouldJump;
        Pit pit1,pit2,pit3; 
        Coin coin1, InfoTreasure1, Quest1, Bigcoin,Quest2;
        int score;
        Hero hero;
        Monster m1,m2;
        Intent move;
        MediaPlayer coined,jumped,gover;
        int counter;
        int mc;
        int wr;
        int rBound;
        int lBound;
        int monster_delta2;
        Button btnEnter;
        string dpPath;

        string username;

        string User_Name;

        CustomDialog di;
        public Board(Context context, Intent intent) : base(context)
        {
            




         //   handler = new Handler();

            rBound = 0;
            lBound = 900;
            wr = 10;
            mc = 0;
            score=0;
            this.context = context;
            this.intent = intent;
            counter = 0;
            xr = 2000;
            yr = 550;
            jx = 20;
            del = 6;
            deltay = 25;
            monster_delta = 7;
            monster_delta2 = 7;
            WouldJump = false;
            randx = rand.Next(200, 800);
            coined = MediaPlayer.Create(context, Resource.Raw.videoplayback);
            jumped = MediaPlayer.Create(context, Resource.Raw.jumpsfx);
            gover = MediaPlayer.Create(context, Resource.Raw.gameoverSFX);

            settings = BitmapFactory.DecodeResource(Resources, Resource.Drawable.settings);
            rightarrw = BitmapFactory.DecodeResource(Resources, Resource.Drawable.rightarrow);
            leftarrw = BitmapFactory.DecodeResource(Resources, Resource.Drawable.realeft);
            m1 = new Monster(randx, 610, BitmapFactory.DecodeResource(Resources, Resource.Drawable.marman),0,false);
            m2 = new Monster(randx + 1500, 610, BitmapFactory.DecodeResource(Resources, Resource.Drawable.marman), 0, false);
            monster2 = BitmapFactory.DecodeResource(Resources, Resource.Drawable.marman);
            platform = BitmapFactory.DecodeResource(Resources, Resource.Drawable.snip);
            jumparw = BitmapFactory.DecodeResource(Resources, Resource.Drawable.newuparrow);
            coin1 = new Coin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.coinew3), false, 300, 500);
            Bigcoin = new Coin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.coinew3), false, 2000, 500);

            hero = new Hero(50, 500, BitmapFactory.DecodeResource(Resources, Resource.Drawable.torch));
            pit1 = new Pit( BitmapFactory.DecodeResource(Resources, Resource.Drawable.WhiteRec), 1700,700);
            pit2 = new Pit(BitmapFactory.DecodeResource(Resources, Resource.Drawable.WhiteRec), 2200, 700);
            pit3 = new Pit(BitmapFactory.DecodeResource(Resources, Resource.Drawable.WhiteRec), 3700, 700);


            InfoTreasure1 = new Coin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.treasureMap),false,600,550);
            Quest1 = new Coin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.treasureMap), false, 1000, 550);
            Quest2 = new Coin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.treasureMap), false, 3200, 550);

            User_Name = intent.GetStringExtra("UNAME");
             di = new CustomDialog(context);



            //intent = new Intent(context, typeof(LVL2Activity));



        }



        protected override void OnDraw(Canvas canvas)
        {


            draw(canvas, platform, -counter, 700, canvas.Width, 500);

            canvas.DrawBitmap(settings, 0, 0, null);
            canvas.DrawBitmap(m1.GetMonster(), m1.GetX(), m1.GetY(), null);
            canvas.DrawBitmap(m2.GetMonster(), m2.GetX(), m2.GetY(), null);
            canvas.DrawBitmap(leftarrw, xr - 500, yr, null);
            canvas.DrawBitmap(rightarrw, xr, yr, null);
            canvas.DrawBitmap(pit1.GetPit(), pit1.Getx(), pit1.Gety(), null);
            canvas.DrawBitmap(pit2.GetPit(), pit2.Getx(), pit2.Gety(), null);
            canvas.DrawBitmap(pit3.GetPit(), pit3.Getx(), pit3.Gety(), null);

            canvas.DrawBitmap(hero.GetHero(), hero.GetX(), hero.GetY(), null);

            canvas.DrawBitmap(jumparw, jx, yr + 140, null);
            canvas.DrawBitmap(coin1.Getcoin(), coin1.Getx(), coin1.Gety(), null);
            canvas.DrawBitmap(InfoTreasure1.Getcoin(), InfoTreasure1.Getx(), InfoTreasure1.Gety(), null);

            canvas.DrawBitmap(Quest1.Getcoin(), Quest1.Getx(), Quest1.Gety(), null);
            canvas.DrawBitmap(Bigcoin.Getcoin(), Bigcoin.Getx(), Bigcoin.Gety(), null);
            canvas.DrawBitmap(Quest2.Getcoin(),Quest2.Getx(), Quest2.Gety(), null);

            dpPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<LoginTable>(); //Call Table  
            User_Name = intent.GetStringExtra("UNAME");
            var data1 = data.Where(x=>x.username==User_Name).FirstOrDefault();
            if (data1 != null)
            {
                score = data1.CurrentScore;

                username = data1.username;
            }


            pitcollison(pit1);
            pitcollison(pit2);
            pitcollison(pit3);


            if ((m1.GetX() > hero.GetX() + hero.GetHero().Width && m1.GetX() - 40 < hero.GetX() + hero.GetHero().Width) && (hero.GetY() == 500) && m1.getsquash() == false)
            {
                Thread th = new Thread(collision);
                th.Start();

            }
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



                m1.SetMonster(BitmapFactory.DecodeResource(Resources, Resource.Drawable.SqGoomba));
                monster_delta = 0;
            }

            if ((m2.GetX() > hero.GetX() + hero.GetHero().Width && m2.GetX() - 40 < hero.GetX() + hero.GetHero().Width) && (hero.GetY() == 500) && m2.getsquash() == false)
            {
                Thread th = new Thread(collision);
                th.Start();

            }
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



                m2.SetMonster(BitmapFactory.DecodeResource(Resources, Resource.Drawable.SqGoomba));
                monster_delta2 = 0;
            }


            if (hero.GetX() + 50 > coin1.Getx() && hero.GetX() + 50 < coin1.Getx() + 50 && coin1.Gety() == hero.GetY()) //מטבע
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



            base.OnDraw(canvas);
            int perx = randx + 500;
            int delta2 = 10;
           
            

            if (mc > lBound ||mc<rBound )
            {
                wr = -wr;
                mc = mc + wr;
            }
            else
            {
                mc = mc % Width;
                mc = mc + wr;
            }

          




            Paint paint = new Paint();
            paint.SetARGB(50, 20, 50, 100);
            paint.TextAlign = paint.TextAlign;
            paint.TextSize = 70;
            m1.setx(m1.GetX() - monster_delta);
            m2.setx(m2.GetX() - monster_delta2);

          






            canvas.DrawText("Score: " + score, canvas.Width / 2, 75, paint);
            canvas.DrawText("User: " + User_Name, canvas.Width / 2-500, 75, paint);

            if (m1.GetX() < rBound+400 && monster_delta >0 || (m1.GetX() > lBound && monster_delta<0 ))
                monster_delta = -monster_delta;
            if (m2.GetX() < rBound + 2500 && monster_delta2 > 0 || (m1.GetX() > lBound+100 && monster_delta2 < 0))
                monster_delta2 = -monster_delta2;





            canvas.DrawText("All Time Best Score: " + data1.score, canvas.Width / 2, 400, paint);
            


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
        public override bool OnTouchEvent(MotionEvent e)
        {
            int wr = 10;
            float tempy = hero.GetY();
            if (MotionEventActions.Move == e.Action)
            {
                int usx = (int)e.GetX();
                int usy = (int)e.GetY();

                touchquset(usx, usy, Quest1, 1);
                touchquset(usx, usy, Quest2, 2);

                if((usx >= InfoTreasure1.Getx()-60  && usx <= InfoTreasure1.Getx() +60) && ( usy <= InfoTreasure1.Gety()+70 && usy>= InfoTreasure1.Gety()-70) && (hero.GetX() >= InfoTreasure1.Getx() - 400 && hero.GetX() <= InfoTreasure1.Getx() + 400) && (hero.GetY() <= InfoTreasure1.Gety() + 70 && hero.GetY() >= InfoTreasure1.Gety() - 70))
                {

                    CustomDialog di = new CustomDialog(context);
                    btnEnter = FindViewById<Button>(Resource.Id.btnEnter);
                    if (di.IsShowing==false) { di.Show(); }
                  




                }
                if ((usx - 70 >= xr || usx + 70 >= xr) && (usy - 50 >= yr || usy - 10 >= yr))
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
                    InfoTreasure1.setx(InfoTreasure1.Getx() - 10);
                    Quest1.setx(Quest1.Getx() - 10);
                    Quest2.setx(Quest2.Getx() - 10);

                    rBound -= 10;
                    lBound -= 10;

                   







                }
                else if ((usx >= xr - 500 || usx >= xr - 500) && (usy - 50 >= yr || usy - 10 >= yr))
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
                    InfoTreasure1.setx(InfoTreasure1.Getx() + 10);
                    Quest1.setx(Quest1.Getx() + 10);
                    Quest2.setx(Quest2.Getx() + 10);





                    rBound += 10;
                    lBound += 10;

                
                    




                }
                if ((usx - 100 <= jx || usx + 100 <= jx) && (usy - 80 >= yr + 140 || usy + 80 >= yr + 140))
                {
                    jumped.Start();

                    WouldJump = true;
                    


                }
                if((usx  <= 45 && usx  >= 0)&&(usy<=45))
                {
                    Intent intent= new Intent(context, typeof(SettingsActivity));
                    context.StartActivity(intent);

                }
            }
            Invalidate();

            return true;
        }

       

        public static  void draw(Canvas canvas, Bitmap bitmap, int x, int y, int width, int height)
        {
            
            Rect source = new Rect(0, 0, bitmap.Width, bitmap.Height);
            Rect target = new Rect(x, y, x + width, y + height);
            canvas.DrawBitmap(bitmap, source, target, null);
        }
       
        public static void InsertScore(int score,string username) {
            string dpPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); //Call Database  
            var db = new SQLiteConnection(dpPath);
            var data = db.Table<LoginTable>(); //Call Table  
            var data1 = data.Where(x => x.username == username && x.score==0).FirstOrDefault(); //Linq Query  
            data1.score = score;
            db.Update(data1);

        }

       
        public void touchquset(int usx, int usy, Coin quest, int number)
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

        

        public void squash1()
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

        public void squash2()
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
        public void pitcollison(Pit pit)
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
        public void collision()
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