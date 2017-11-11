using System;
using System.Collections.Generic;

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
        private static List<Origin> AddToList(List<Origin> l_dataStoringList, int element0,
                                              double element1, double element2, 
                                              double element3, double element4)
        {
            Origin item = new Origin { i_MeasureNo = element0,
                                       d_Time = element1, d_FirSensorTemp = element2,
                                       d_SecSensorTemp = element3, d_ThiSensorTemp = element4 };
            item.d_AvgSensorTemp = Math.Round((item.d_FirSensorTemp + item.d_SecSensorTemp + 
                                    item.d_ThiSensorTemp) / 3, 2, MidpointRounding.AwayFromZero);

            l_dataStoringList.Add(item);
            return l_dataStoringList;
        }
    }
    public class Origin
    {
        public int Id { get; set; }
        private int i_measureNo;
        private double d_time;
        private double d_firSensorTemp;
        private double d_secSensorTemp;
        private double d_thiSensorTemp;
        private double d_avgSensorTemp;
        public int i_MeasureNo
        {
            get { return i_measureNo; }
            set { i_measureNo = value; }
        }
        public double d_Time
        {
            get { return d_time; }
            set { d_time = value; }
        }
        public double d_FirSensorTemp
        {
            get { return d_firSensorTemp; }
            set { d_firSensorTemp = value; }
        }
        public double d_SecSensorTemp
        {
            get { return d_secSensorTemp; }
            set { d_secSensorTemp = value; }
        }
        public double d_ThiSensorTemp
        {
            get { return d_thiSensorTemp; }
            set { d_thiSensorTemp = value; }
        }
        public double d_AvgSensorTemp
        {
            get { return d_avgSensorTemp; }
            set { d_avgSensorTemp = value; }
        }
    }
}