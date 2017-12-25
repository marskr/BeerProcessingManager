using Android.App;
using Android.Widget;
using System.Collections.Generic;
using Android.Views;

namespace BeerProcessingManager.ThingspeakManagement
{
    /// <summary>
    /// The class that stores fields that will be used in list panel generated in application.
    /// </summary>
    public class VwHolder : Java.Lang.Object
    {
        public TextView tv_TxtNo { get; set; }
        public TextView tv_TxtTemperature { get; set; }
        public TextView tv_TxtTime { get; set; }
    }

    /// <summary>
    /// The class that processes sending data from thingspeak to list panel generated in application.
    /// </summary>
    public class VwAdapter : BaseAdapter
    {
        private Activity activity;
        private List<Origin> l_measures;
        private int i_choice;
        private const int i_FirSensorChoice = 1;
        private const int i_SecSensorChoice = 2;
        private const int i_ThiSensorChoice = 3;
        private const int i_AvgSensorChoice = 4;
        private const int i_DataExtractChoice = 5;

        public VwAdapter(Activity activity, List<Origin> l_listview, int i_choice)
        {
            this.activity = activity;
            this.l_measures = l_listview;
            this.i_choice = i_choice;
        }

        public override int Count
        {
            get
            {
                return l_measures.Count;
            }
        }

        public override Java.Lang.Object GetItem(int i_position)
        {
            return null;
        }

        public override long GetItemId(int i_position)
        {
            return l_measures[i_position].i_Id;
        }

        /// <summary>
        /// Method that processes sending data from thingspeak to list panel in application.
        /// </summary>
        /// <param name="i_position"></param>
        /// <param name="v_convertView"></param>
        /// <param name="vg_parent"></param>
        /// <returns>Returns view with data sent into list panel.</returns>
        public override View GetView(int i_position, View v_convertView, ViewGroup vg_parent)
        {
            var view = v_convertView ?? activity.LayoutInflater.Inflate
                (Resource.Layout.ListVwShowData, vg_parent, false);

            TextView tv_TxtNo = view.FindViewById<TextView>(Resource.Id.id_txtView1);
            TextView tv_TxtTemperature = view.FindViewById<TextView>(Resource.Id.id_txtView2);
            TextView tv_TxtTime = view.FindViewById<TextView>(Resource.Id.id_txtView3);

            if (i_choice != i_DataExtractChoice)
                tv_TxtNo.Text = "The measure number " + l_measures[i_position].i_MeasureNo; // int to string
            else
                tv_TxtNo.Text = "";

            switch (i_choice)
            {
                case i_FirSensorChoice:
                    tv_TxtTemperature.Text = "Temperature: " + l_measures[i_position].d_FirSensorTemp + "\u00B0C";
                    break;
                case i_SecSensorChoice:
                    tv_TxtTemperature.Text = "Temperature: " + l_measures[i_position].d_SecSensorTemp + "\u00B0C";
                    break;
                case i_ThiSensorChoice:
                    tv_TxtTemperature.Text = "Temperature: " + l_measures[i_position].d_ThiSensorTemp + "\u00B0C";
                    break;
                case i_AvgSensorChoice:
                    tv_TxtTemperature.Text = "Temperature: " + l_measures[i_position].d_AvgSensorTemp + "\u00B0C";
                    break;
                case i_DataExtractChoice:
                    tv_TxtTemperature.Text = "";
                    break;
            }
            if (i_choice != i_DataExtractChoice)
                tv_TxtTime.Text = "Time: " + l_measures[i_position].d_Time;
            else
                tv_TxtTime.Text = "";

            return view;
        }
    }
}