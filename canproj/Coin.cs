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
    internal class Coin
    {
        Bitmap coin { get; set; }// ייצוג מטבע
        bool IsBig { get; set; }// האם המטבע גדול או לא
        int x { get; set; }// מיקום
        int y {  get; set; }//מיקום

        public Coin(Bitmap coin, bool isBig, int x, int y)// פונקציה בונה
        {
            this.coin = coin;
            IsBig = isBig;
            this.x = x;
            this.y = y;
        }
        public int Getx() { return x; }
        public int Gety() { return y; }
        public Bitmap Getcoin() { return coin; }
        public void Setcoin(Bitmap coin) { this.coin = coin; }
        public void setx(int x) { this.x = x; } 
        
    }
}