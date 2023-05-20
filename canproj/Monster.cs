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
    internal class Monster : character// ירושה ממחלקה
    {
       
        int speed { get; set; }// מהירות
        bool IsSquashed;
        public Monster(int x, int y, Bitmap character, int speed,bool IsSquash) : base(x, y, character)// פונקציה בונה
        {
            this.x = x;
            this.y = y;
            this.Character = character;
            this.speed = speed;
            this.IsSquashed = IsSquash;
        }
        public int GetX()// קבלת ערך X
        {
            return (int)x;
        }
        public int GetY()// קבלת ערך Y
        {
            return (int)y;
        }
        
        public Bitmap GetMonster() { return this.Character; }
        public void SetMonster(Bitmap character) { this.Character = character; }
        public void setx(int x) { this.x = x; }
        public void sety(int y) { this.y = y; }
        public void setSquash(bool sq)
        {
            sq=this.IsSquashed;
        }
        public bool getsquash()
        {
            return this.IsSquashed;
        }

    }
}