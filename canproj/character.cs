using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace canproj
{
    internal abstract class character
    {
        float x { get; set; }//מיקום
        float y { get; set; }//מיקום
        Bitmap Character { get; set; }// תצוגה על ידי תמונה

        protected character(float x, float y, Bitmap character)// פונקציה בונה
        {
            this.x = x;
            this.y = y;
            this.Character = character;



        }
    }
}
