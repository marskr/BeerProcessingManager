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
        private int i_choice;
        public VwAdapter(Activity activity, List<Origin> listview, int i_choice)
        {
            this.activity = activity;
            this.measures = listview;
            this.i_choice = i_choice;
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
            switch(i_choice)
            {
                case 1:
                    txtTemperature.Text = "Temperature: " + measures[i_position].d_FirSensorTemp + "\u00B0C";
                    break;
                case 2:
                    txtTemperature.Text = "Temperature: " + measures[i_position].d_SecSensorTemp + "\u00B0C";
                    break;
                case 3:
                    txtTemperature.Text = "Temperature: " + measures[i_position].d_ThiSensorTemp + "\u00B0C";
                    break;
                case 4:
                    txtTemperature.Text = "Temperature: " + measures[i_position].d_AvgSensorTemp + "\u00B0C";
                    break;
            }
            txtTime.Text = "Time: " + measures[i_position].d_Time;

            return view;
        }
    }
}