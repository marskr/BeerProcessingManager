using System.Collections.Generic;

namespace BeerProcessingManager.ThingspeakManagement
{
    class DatabaseManager
    {
    }
    public class DataStorage
    {
        public static List<Origin> ArtificialList()
        {
            List<Origin> l_dataStoringList = new List<Origin>();
            l_dataStoringList = AddToList(l_dataStoringList, 0.1, 1.1);
            l_dataStoringList = AddToList(l_dataStoringList, 0.4, 6.0);
            l_dataStoringList = AddToList(l_dataStoringList, 1.4, 2.1);
            l_dataStoringList = AddToList(l_dataStoringList, 2.0, 4.2);
            l_dataStoringList = AddToList(l_dataStoringList, 3.3, 2.3);
            l_dataStoringList = AddToList(l_dataStoringList, 4.7, 7.4);
            l_dataStoringList = AddToList(l_dataStoringList, 6.0, 6.2);
            l_dataStoringList = AddToList(l_dataStoringList, 8.9, 8.9);
            l_dataStoringList = AddToList(l_dataStoringList, 9.7, 7.4);

            return l_dataStoringList;
        }
        private static List<Origin> AddToList(List<Origin> l_dataStoringList, double element1, double element2)
        {
            Origin item = new Origin { d_Time = element1, d_Temperature = element2 };
            l_dataStoringList.Add(item);
            return l_dataStoringList;
        }
    }
    public class Origin
    {
        private double d_time;
        private double d_temperature;
        public double d_Time
        {
            get { return d_time; }
            set { d_time = value; }
        }
        public double d_Temperature
        {
            get { return d_temperature; }
            set { d_temperature = value; }
        }
    }
}