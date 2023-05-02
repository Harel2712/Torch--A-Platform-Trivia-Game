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
        Pit pit1,pit2; 
        Coin coin1, InfoTreasure1, Quest1;
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

      
      
        Dialog d;

     //   Handler handler;

        Button btnEnter;
        string dpPath;

        string username;
        MediaPlayer bgMusic;

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
            del = 20;
            deltay = 25;
            monster_delta = 10;
            WouldJump = false;
            randx = rand.Next(200, 800);
            coined = MediaPlayer.Create(context, Resource.Raw.videoplayback);
            jumped = MediaPlayer.Create(context, Resource.Raw.jumpsfx);
            gover = MediaPlayer.Create(context, Resource.Raw.gameoverSFX);
            bgMusic = MediaPlayer.Create(context, Resource.Raw.OST);

            settings = BitmapFactory.DecodeResource(Resources, Resource.Drawable.settings);
            rightarrw = BitmapFactory.DecodeResource(Resources, Resource.Drawable.rightarrow);
            leftarrw = BitmapFactory.DecodeResource(Resources, Resource.Drawable.realeft);
            m1 = new Monster(randx, 610, BitmapFactory.DecodeResource(Resources, Resource.Drawable.marman),0,false);
            //m2 = new Monster(randx + 100, 610, BitmapFactory.DecodeResource(Resources, Resource.Drawable.marman), 0, false);
            monster2 = BitmapFactory.DecodeResource(Resources, Resource.Drawable.marman);
            platform = BitmapFactory.DecodeResource(Resources, Resource.Drawable.snip);
            jumparw = BitmapFactory.DecodeResource(Resources, Resource.Drawable.newuparrow);
            coin1 = new Coin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.coinew3), false, 300, 500);
            hero = new Hero(50, 500, BitmapFactory.DecodeResource(Resources, Resource.Drawable.torch));
            pit1 = new Pit( BitmapFactory.DecodeResource(Resources, Resource.Drawable.WhiteRec), 1700,700);
            pit2 = new Pit(BitmapFactory.DecodeResource(Resources, Resource.Drawable.WhiteRec), 2200, 700);


            InfoTreasure1 = new Coin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.treasureMap),false,600,550);
            Quest1 = new Coin(BitmapFactory.DecodeResource(Resources, Resource.Drawable.treasureMap), false, 1000, 550);

            User_Name = intent.GetStringExtra("UNAME");
             di = new CustomDialog(context);



            //intent = new Intent(context, typeof(LVL2Activity));



        }



        protected override void OnDraw(Canvas canvas)
        {

   

            if (!bgMusic.IsPlaying)
            {
                bgMusic.Start();

            }
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
            
            


            if ((m1.GetX() > hero.GetX() + hero.GetHero().Width && m1.GetX() - 40 < hero.GetX() + hero.GetHero().Width) && (hero.GetY() == 500) && m1.getsquash() == false)
            {
                Thread th = new Thread(collision);
                monster_delta = -monster_delta;
                th.Start();

            }
            else if ((m1.GetX() > hero.GetX() + 40 && m1.GetX() - 40 < hero.GetX() + 60) && (hero.GetY() < 500 && hero.GetY() > 450))
            {
                //if (m1.getsquash() == false)
                //{
                //    score = score + 2;

                //    data1.CurrentScore = score;
                //    db.Update(data1);

                //    if (score > data1.score)
                //    {
                //        data1.score =  score;
                //        db.Update(data1);

                //    }

                //}

                
                m1.setSquash(true);
                Thread th = new Thread(yo);
                 th.Start();



                m1.SetMonster(BitmapFactory.DecodeResource(Resources, Resource.Drawable.SqGoomba));
                monster_delta = 0;
            }


            if (hero.GetX() + 50 > coin1.Getx() && hero.GetX() + 50 < coin1.Getx() + 50 && coin1.Gety() == hero.GetY()) //מטבע
            {
                coined.Start();
                score = score + 1;
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

            

            base.OnDraw(canvas);
            int perx = randx + 500;
            int delta2 = 10;
            draw(canvas, platform,-counter,700,canvas.Width,500);
            //draw(canvas, m2.GetMonster(),mc, 610, 100, 100);
            

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
           

            canvas.DrawBitmap(settings, 0, 0, null) ;
            canvas.DrawBitmap(m1.GetMonster(), m1.GetX(), m1.GetY(), null);
            //canvas.DrawBitmap(m2.GetMonster(), m2.GetX(), m2.GetY(), null);
            canvas.DrawBitmap(leftarrw, xr - 500, yr, null);
            canvas.DrawBitmap(rightarrw, xr, yr, null);
            //canvas.DrawBitmap(platform, 0, 700, null);
            canvas.DrawBitmap(pit1.GetPit(), pit1.Getx(), pit1.Gety(), null);
            canvas.DrawBitmap(pit2.GetPit(), pit2.Getx(), pit2.Gety(), null);

            canvas.DrawBitmap(hero.GetHero(), hero.GetX(), hero.GetY(), null);

            canvas.DrawBitmap(jumparw, jx, yr + 140, null);
            canvas.DrawBitmap(coin1.Getcoin(), coin1.Getx(), coin1.Gety(), null);
            canvas.DrawBitmap(InfoTreasure1.Getcoin(), InfoTreasure1.Getx(), InfoTreasure1.Gety(), null);

            canvas.DrawBitmap(Quest1.Getcoin(), Quest1.Getx(), Quest1.Gety(), null);




            Paint paint = new Paint();
            paint.SetARGB(50, 20, 50, 100);
            paint.TextAlign = paint.TextAlign;
            paint.TextSize = 70;
            m1.setx(m1.GetX() - monster_delta);
            //m2.setx(m2.GetX() + monster_delta);

          
                       
           
          
            

            canvas.DrawText("Score: " + score, canvas.Width / 2, 75, paint);
            canvas.DrawText("User: " + User_Name, canvas.Width / 2-500, 75, paint);

            if (m1.GetX() < 400 || m1.GetX() > canvas.Width - 1200)
                monster_delta = -monster_delta;
            // if (m2.GetX() < 900 || m2.GetY() > canvas.Width - 700)
            //   delta2 = -delta2;

           
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

           

            if (hero.GetY() == 500 &&( hero.GetX() + 50 > pit1.Getx() && hero.GetX() + 50 < pit1.Getx() + 200)|| hero.GetY() == 500 && (hero.GetX() + 50 > pit2.Getx() && hero.GetX() + 50 < pit2.Getx() + 200))
            {
                while(hero.GetY()<860)
                {
                    hero.SetY(hero.GetY() + 1);

                }
                if (hero.GetY() ==860)
                {
                    collision();
                }

            }




            if (hero.GetX()==1900 || hero.GetX()==1901)
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

                if((usx >= Quest1.Getx() - 60 && usx <= Quest1.Getx() + 60) && (usy <= Quest1.Gety() + 70 && usy >= Quest1.Gety() - 70) && (hero.GetX() >= Quest1.Getx() - 400 && hero.GetX() <= Quest1.Getx() + 400) && (hero.GetY() <= Quest1.Gety() + 70 && hero.GetY() >= Quest1.Gety() - 70))
                {
                    string name = intent.GetStringExtra("UNAME");
                    Intent move = new Intent();
                    move.PutExtra("UNAME", name);
                    move.SetClass(context, typeof(ActivityQuestion));
                    context.StartActivity(move);


                }

                if((usx >= InfoTreasure1.Getx()-60  && usx <= InfoTreasure1.Getx() +60) && ( usy <= InfoTreasure1.Gety()+70 && usy>= InfoTreasure1.Gety()-70) && (hero.GetX() >= InfoTreasure1.Getx() - 400 && hero.GetX() <= InfoTreasure1.Getx() + 400) && (hero.GetY() <= InfoTreasure1.Gety() + 70 && hero.GetY() >= InfoTreasure1.Gety() - 70))
                {

                    CustomDialog di = new CustomDialog(context);
                    btnEnter = FindViewById<Button>(Resource.Id.btnEnter);
                    btnEnter.Click += BtnEnter_Click1;
                    di.Show();




                }
                if ((usx - 70 >= xr || usx + 70 >= xr) && (usy - 50 >= yr || usy - 10 >= yr))
                {
                    hero.SetX(hero.GetX()+25);
                    hero.SetHero(BitmapFactory.DecodeResource(Resources, Resource.Drawable.torch));
                    counter = counter % Width;
                    counter = counter + 1;

                    pit1.setx(pit1.Getx()-10);
                    pit2.setx(pit2.Getx() - 10);

                    coin1.setx(coin1.Getx() - 10);

                    rBound -= 10;
                    lBound -= 10;

                    if (monster_delta < 0)
                    {
                        m1.setx(m1.GetX() + 5);

                    }
                    else
                    {
                        m1.setx(m1.GetX() - 5);

                    }







                }
                else if ((usx >= xr - 500 || usx >= xr - 500) && (usy - 50 >= yr || usy - 10 >= yr))
                {
                    hero.SetX(hero.GetX() - 25);
                    hero.SetHero(BitmapFactory.DecodeResource(Resources, Resource.Drawable.lefttorch));
                    counter = counter % Width;
                    counter = counter - 1;

                    pit1.setx(pit1.Getx() + 10);
                    pit1.setx(pit2.Getx() + 10);

                    coin1.setx(coin1.Getx() + 10);


                    rBound += 10;
                    lBound += 10;

                    if (monster_delta < 0)
                    {
                        m1.setx(m1.GetX() + 5);

                    }
                    else
                    {
                        m1.setx(m1.GetX() - 5);

                    }




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
                    hero.SetHero(BitmapFactory.DecodeResource(Resources, Resource.Drawable.big));

                }
            }
            Invalidate();

            return true;
        }

        private void BtnEnter_Click(object sender, EventArgs e)
        {
            d.Dismiss();
           
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

        public void AskDialog()
        {
            CustomDialog di = new CustomDialog(context);
            btnEnter = FindViewById<Button>(Resource.Id.btnEnter);
            btnEnter.Click += BtnEnter_Click1;
            d.SetContentView(Resource.Layout.FirstDialogXML);




            d.Show();

        }

        private void BtnEnter_Click1(object sender, EventArgs e)
        {
            di.Dismiss();

        }

       

        public void yo()
        {
            Thread.Sleep(500);
            m1.SetMonster(BitmapFactory.DecodeResource(Resources, Resource.Drawable.ppnng));
            m1.setx(-999);

            score = score + 2;
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

        public void collision()
        {
            string name = intent.GetStringExtra("UNAME");
            Intent move = new Intent();
            move.PutExtra("UNAME", name);
            move.SetClass(context, typeof(GameOverActivity));
            context.StartActivity(move);

            hero.SetHero(BitmapFactory.DecodeResource(Resources, Resource.Drawable.ppnng));
            hero.SetX(-999);

            monster_delta = 2;





        }
    }
}