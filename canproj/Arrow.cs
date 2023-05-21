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
    internal class Arrow
    {
        public int x { get; set; }// מיקום
        public int y { get; set; }// מיקום
        public Bitmap arrow { get; set; }// יצוג חץ
        public bool isRight { get; set; }// האם החץ פונה ימינה או לא

        public Arrow(int x, int y, Bitmap arrow, bool isRight)// פונקציה בונה
        {
            this.x = x;
            this.y = y;
            this.arrow = arrow;
            this.isRight = isRight;
        }


    }
}