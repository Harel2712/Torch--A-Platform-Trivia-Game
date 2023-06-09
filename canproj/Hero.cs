﻿using Android.App;
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
       
        public Hero(float x, float y, Bitmap character) : base(x, y, character)// פונקציה בונה
        {
            this.x = x;
            this.y = y;
            this.Character = character;
        }
      
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
        public Bitmap GetHero() { return Character; }
        public void SetHero(Bitmap bitm) { this.Character = bitm; }

      
        
    }
}