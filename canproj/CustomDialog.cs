using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Org.Apache.Commons.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace canproj
{
    [Activity(Label = "FirstDialogActivity")]
    public class CustomDialog : Dialog // הורשה מדיאלוג
    {
        Button Enter;
        private Context context;

        public CustomDialog(Context context) : base(context)
        {
            this.context = context;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FirstDialogXML);
            Enter = FindViewById<Button>(Resource.Id.btnEnter);
            Enter.Click += Enter_Click;      

            // Create your application here
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            Cancel();
        }
    }
}