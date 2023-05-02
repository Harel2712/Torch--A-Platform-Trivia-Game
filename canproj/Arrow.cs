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
        int x { get; set; }// מיקום
        int y { get; set; }// מיקום
        Bitmap arrow { get; set; }// יצוג חץ
        bool isRight { get; set; }// האם החץ פונה ימינה או לא

        public Arrow(int x, int y, Bitmap arrow, bool isRight)// פונקציה בונה
        {
            this.x = x;
            this.y = y;
            this.arrow = arrow;
            this.isRight = isRight;
        }


    }
}