using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ThingSpeakWinRT;

namespace BeerProcessingManager.ThingspeakManagement
{
    public class DataStorage
    {
        private const string s_writeAPIKey = "92XHIDTJ9UPNL727";
        private const string s_readAPIKey = "96038QW649O5FFNH";
        private const int i_channelID = 366148;
        private const bool b_requiredSSL = true;
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
            List<Origin> l_dataStoringList = new List<Origin>();
            foreach (var element in feeds.Feeds)
            {
                l_dataStoringList = AddToList(l_dataStoringList, 
                                              element.EntryId ?? 0,
                                              (element.Field5 == null) ? 0 : Double.Parse(element.Field5, CultureInfo.InvariantCulture),
                                              (element.Field1 == null) ? 0 : Double.Parse(element.Field1, CultureInfo.InvariantCulture),
                                              (element.Field2 == null) ? 0 : Double.Parse(element.Field2, CultureInfo.InvariantCulture),
                                              (element.Field3 == null) ? 0 : Double.Parse(element.Field3, CultureInfo.InvariantCulture));
            }
            return l_dataStoringList;
        }
        private static List<Origin> AddToList(List<Origin> l_dataStoringList, 
                                              int i_element0_measNo,
                                              double d_element1_time, 
                                              double d_element2_temp, 
                                              double d_element3_temp, 
                                              double d_element4_temp)
        {
            Origin item = new Origin { i_MeasureNo = i_element0_measNo,
                                       d_Time = d_element1_time,
                                       d_FirSensorTemp = d_element2_temp,
                                       d_SecSensorTemp = d_element3_temp,
                                       d_ThiSensorTemp = d_element4_temp
            };
            item.d_AvgSensorTemp = Math.Round((item.d_FirSensorTemp + item.d_SecSensorTemp + 
                                    item.d_ThiSensorTemp) / 3, 2, MidpointRounding.AwayFromZero);

            l_dataStoringList.Add(item);
            return l_dataStoringList;
        }
        public static async Task<ThingSpeakData> ReadThingspeak()
        {
            var client = new ThingSpeakClient(sslRequired: b_requiredSSL);
            ThingSpeakData feeds = await client.ReadAllFeedsAsync(s_readAPIKey, i_channelID);

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