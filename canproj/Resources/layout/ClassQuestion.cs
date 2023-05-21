using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace canproj.Resources.layout
{
    public class ClassQuestion
    {
        public string question { get; set; }
        public string option1 { get; set; }
        public string option2 { get; set; }
        public string option3 { get; set; }
        public string option4 { get; set; }
        public int RightAnswer { get; set; } //מספר התשובה הנכונה

        public ClassQuestion(string question, string option1, string option2, string option3, string option4, int rightAnswer)// קונסטרקטור
        {
            this.question = question;
            this.option1 = option1;
            this.option2 = option2;
            this.option3 = option3;
            this.option4 = option4;
            RightAnswer = rightAnswer;
        }
    }
}