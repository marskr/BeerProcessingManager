 using Android.App;
using Android.Widget;
using System.Collections.Generic;
using Android.Views;

namespace BeerProcessingManager.ThingspeakManagement
{
    public class VwHolder : Java.Lang.Object
    {
        public TextView txtNo { get; set; }
        public TextView txtTemperature { get; set; }
        public TextView txtTime { get; set; }
    }
    public class VwAdapter : BaseAdapter
    {
        private Activity activity;
        private List<Origin> measures;
        public VwAdapter(Activity activity, List<Origin> listview)
        {
            this.activity = activity;
            this.measures = listview;
        }
        public override int Count
        {
            get
            {
                return measures.Count;
            }
        }
        public override Java.Lang.Object GetItem(int i_position)
        {
            return null;
        }
        public override long GetItemId(int i_position)
        {
            return measures[i_position].Id;
        }
        public override View GetView(int i_position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate
                (Resource.Layout.ListVwShowData, parent, false);

            TextView txtNo = view.FindViewById<TextView>(Resource.Id.id_txtView1);
            TextView txtTemperature = view.FindViewById<TextView>(Resource.Id.id_txtView2);
            TextView txtTime = view.FindViewById<TextView>(Resource.Id.id_txtView3);

            txtNo.Text = "The measure number " + measures[i_position].i_MeasureNo; // int to string
            txtTemperature.Text = "Temperature: " + measures[i_position].d_FirSensorTemp + "\u00B0C"; 
            txtTime.Text = "Time: " + measures[i_position].d_Time;

            return view;
        }
    }
}