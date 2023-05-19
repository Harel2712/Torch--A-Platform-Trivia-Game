using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using static Android.Graphics.Paint;
using Bitmap = Android.Graphics.Bitmap;

namespace canproj
{
    internal class Hero : character// ירושה ממחלקה
    {
        float x { get; set; }//מיקום
        float y { get; set; }//מיקום
        Bitmap character { get; set; }// ייצוג על ידי תמונת הדמות
        public Hero(float x, float y, Bitmap character) : base(x, y, character)// פונקציה בונה
        {
            this.x = x;
            this.y = y;
            this.character = character;
        }
        //public bool CheckCollisons(Monster monster)// בדיקה המחזירה אמת או שקר האם הדמות התנגשה עם מפלצת
        //{
        //    if(x+60==monster.GetX()-40 && y-60==monster.GetY()+40)
        //        return true;
        //    else
        //        return false;
        //}
        public float GetX()// קבלת מיקום Y
        {
            return x;
        }
        public float GetY()// קבלת מיקום X
        {
            return y;
        }
        public void SetX(float x) { this.x = x; }
        public void SetY(float y) { this.y = y; }
        public Bitmap GetHero() { return character; }
        public void SetHero(Bitmap bitm) { this.character = bitm; }

      
        
    }
}