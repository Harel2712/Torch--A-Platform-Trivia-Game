using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace canproj.Resources
{
    internal class LoginTable
    {
        [PrimaryKey, AutoIncrement, Column("_Id")]
        public int Id { get; set; }
        [MaxLength(25)]
        public string username { get; set; }
        [MaxLength(15)]
        public string password { get; set; }
        public int score { get; set; }  
        public int CurrentScore { get; set; }   
    }
}