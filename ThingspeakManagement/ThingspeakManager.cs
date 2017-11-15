using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThingSpeakWinRT;

namespace BeerProcessingManager.ThingspeakManagement
{
    public class DataStorage
    {
        public static List<Origin> ArtificialList()
        {
            List<Origin> l_dataStoringList = new List<Origin>();
            l_dataStoringList = AddToList(l_dataStoringList, 1, 0.1, 1.1, 2.2, 3.3);
            l_dataStoringList = AddToList(l_dataStoringList, 2, 0.4, 6.0, 4.4, 5.5);
            l_dataStoringList = AddToList(l_dataStoringList, 3, 1.4, 2.1, 1.1, 5.5);
            l_dataStoringList = AddToList(l_dataStoringList, 4, 2.0, 4.2, 8.4, 9.6);
            l_dataStoringList = AddToList(l_dataStoringList, 5, 3.3, 2.3, 4.6, 9.2);
            l_dataStoringList = AddToList(l_dataStoringList, 6, 4.7, 7.4, 6.6, 7.7);
            l_dataStoringList = AddToList(l_dataStoringList, 7, 6.0, 6.2, 5.5, 7.8);
            l_dataStoringList = AddToList(l_dataStoringList, 8, 8.9, 8.9, 6.6, 5.3);
            l_dataStoringList = AddToList(l_dataStoringList, 9, 9.7, 7.4, 4.4, 3.2);

            return l_dataStoringList;
        }
        public static List<Origin> ThingspeakConverter(ThingSpeakData feeds)
        {
            int i_iterator = 1;
            double d_timechange = 0;
            List<Origin> l_dataStoringList = new List<Origin>();
            foreach (var element in feeds.Feeds)
            {
                l_dataStoringList = AddToList(l_dataStoringList, i_iterator, d_timechange, 
                                              (element.Field1 == null) ? 0 : Convert.ToDouble(element.Field1),
                                              (element.Field2 == null) ? 0 : Convert.ToDouble(element.Field2),
                                              (element.Field2 == null) ? 0 : Convert.ToDouble(element.Field2));
                i_iterator++; d_timechange += 0.5;
            }

            return l_dataStoringList;
        }
        private static List<Origin> AddToList(List<Origin> l_dataStoringList, int i_element0_measNo,
                                              double d_element1_time, double d_element2_temp, 
                                              double d_element3_temp, double d_element4_temp)
        {
            Origin item = new Origin { i_MeasureNo = i_element0_measNo,
                                       d_Time = d_element1_time, d_FirSensorTemp = d_element2_temp,
                                       d_SecSensorTemp = d_element3_temp, d_ThiSensorTemp = d_element4_temp };
            item.d_AvgSensorTemp = Math.Round((item.d_FirSensorTemp + item.d_SecSensorTemp + 
                                    item.d_ThiSensorTemp) / 3, 2, MidpointRounding.AwayFromZero);

            l_dataStoringList.Add(item);
            return l_dataStoringList;
        }

        //public async static void ReadAsync()
        //{
        //    List<Origin> l_dataStoringList = new List<Origin>();
        //    int i_MeasureNo = 0;
        //    double d_Time = 0;

        //    var client = new ThingSpeakClient(sslRequired: true);
        //    var feeds = await client.ReadAllFeedsAsync("SIFTZ39QIJUL48WL", 171349);

        //    System.Console.WriteLine(feeds);
        //    System.Console.WriteLine("The description of the channel: {0}", feeds.Channel.Description);
        //    foreach (var item in feeds.Feeds)
        //    {
        //        i_MeasureNo++;
        //        d_Time++;
        //        l_dataStoringList = AddToList(l_dataStoringList, i_MeasureNo, d_Time, 
        //                                      Convert.ToDouble(item.Field1.ToString()),
        //                                      Convert.ToDouble(item.Field2.ToString()),
        //                                      Convert.ToDouble(item.Field3.ToString()));
        //    }
        //}
        static async void ScreenAsync()
        {
            ThingSpeakData feeds = await ReadThingspeak();
            foreach (var element in feeds.Feeds)
            {
                System.Console.WriteLine("Field1 is {0}", element.Field1);
                System.Console.WriteLine("Field2 is {0}", element.Field2);
                System.Console.WriteLine("Field3 is {0}", element.Field3);
                System.Console.WriteLine("Field4 is {0}", element.Field4);
            }
        }
        public static async Task<ThingSpeakData> ReadThingspeak()
        {
            var client = new ThingSpeakClient(sslRequired: true);
            ThingSpeakData feeds = await client.ReadAllFeedsAsync("SIFTZ39QIJUL48WL", 171349);

            return feeds;
        }
    }
    public class Origin
    {
        public int i_Id { get; set; }
        public int i_MeasureNo { get; set; }
        public double d_Time { get; set; }
        public double d_FirSensorTemp { get; set; }
        public double d_SecSensorTemp { get; set; }
        public double d_ThiSensorTemp { get; set; }
        public double d_AvgSensorTemp { get; set; }
        //public int i_MeasureNo
        //{
        //    get { return i_measureNo; }
        //    set { i_measureNo = value; }
        //}
    }
}