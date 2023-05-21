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
using static Android.Graphics.Paint;

namespace canproj
{
    internal class Pit // מחלקה ליצירת אובייקט מכשול מסוג בור
    {
        Bitmap pit;
        int x;
        int y;
        public Pit(Bitmap pit, int x, int y)
        {
            this.pit = pit;
            this.x = x;
            this.y = y;
        }
        public int Getx() { return x; }
        public int Gety() { return y; }
        public Bitmap GetPit() { return pit; }
        public void SetPit(Bitmap pit) { this.pit = pit; }
        public void setx(int x) { this.x = x; }
    }
}