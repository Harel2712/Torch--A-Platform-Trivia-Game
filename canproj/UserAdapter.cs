using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using canproj.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace canproj
{
    internal class UserAdapter:BaseAdapter<LoginTable>
    {
        private List<LoginTable> userList;
        private LayoutInflater layoutInflater;

        public UserAdapter(Context context, List<LoginTable> userList)
        {
            this.userList = userList;
            this.layoutInflater = LayoutInflater.From(context);
        }

        public override int Count
        {
            get { return userList.Count; }
        }

        public override LoginTable this[int position]
        {
            get { return userList[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder viewHolder;
            if (convertView == null)
            {
                convertView = layoutInflater.Inflate(Resource.Layout.XMLUserAdapter, parent, false);
                viewHolder = new ViewHolder();
                viewHolder.NameTextView = convertView.FindViewById<TextView>(Resource.Id.name_text_view);
                viewHolder.EmailTextView = convertView.FindViewById<TextView>(Resource.Id.score_text_view);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (ViewHolder)convertView.Tag;
            }

            LoginTable user = userList[position];
            viewHolder.NameTextView.Text = position+1+ ". " +user.username;
            viewHolder.EmailTextView.Text = "score: " + user.score;


            return convertView;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public TextView NameTextView { get; set; }
            public TextView EmailTextView { get; set; }
        }
    }
}