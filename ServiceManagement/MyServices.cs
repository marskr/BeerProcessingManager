using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Widget;
using BeerProcessingManager.ThingspeakManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ThingSpeakWinRT;

namespace BeerProcessingManager.ServiceManagement
{
    /// <summary>
    /// Service prepared to gather data by timer cyclic (interval 10 s).
    /// </summary>
    [Service]
    class GuardService : Service
    {
        static readonly int TimerWait = 10000;
        Timer timer;
        DateTime startTime;
        MediaPlayer alarm_player;
        bool isStarted = false;

        public override void OnCreate()
        {
            base.OnCreate();
            alarm_player = MediaPlayer.Create(this, Resource.Raw.alarm);
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            //Toast.MakeText(this, "Service called!", ToastLength.Short).Show();
            if (isStarted)
            {
                TimeSpan runtime = DateTime.UtcNow.Subtract(startTime);
                Toast.MakeText(this, "This service was already started!", ToastLength.Short).Show();
            }
            else
            {
                startTime = DateTime.UtcNow;
                Toast.MakeText(this, "Starting the service!", ToastLength.Short).Show();
                timer = new Timer(HandleTimerCallback, startTime, 0, TimerWait);
                isStarted = true;
            }
            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            // This is a started service, not a bound service, so we just return null.
            return null;
        }

        public override void OnDestroy()
        {
            timer.Dispose();
            timer = null;
            isStarted = false;
            Toast.MakeText(this, "The service finished work!", ToastLength.Short).Show();
            base.OnDestroy();
        }

        public async void HandleTimerCallback(object state)
        {
            //TimeSpan runTime = DateTime.UtcNow.Subtract(startTime);
            SingletonTSList.Instance.l_dataStoringList.Clear();
            ThingSpeakData feeds = await DataStorage.ReadThingspeak();
            if (CheckIfListExceeded(DataStorage.ThingspeakConverter(feeds),
                                    SingletonTSList.Instance.st_WatchdogStorage.i_FirstskBar,
                                    SingletonTSList.Instance.st_WatchdogStorage.i_SecondskBar))
            {
                alarm_player.Start();
            }
        }

        public bool CheckIfListExceeded(List <Origin> l_dataStoringList, 
                                        double d_higherBoundary, double d_lowerBoundary)
        {
            double d_maxSensor1 = l_dataStoringList.Max(s => s.d_FirSensorTemp);
            double d_maxSensor2 = l_dataStoringList.Max(s => s.d_SecSensorTemp);
            double d_maxSensor3 = l_dataStoringList.Max(s => s.d_ThiSensorTemp);
            double d_maxAll = Math.Max(d_maxSensor1, Math.Max(d_maxSensor2, d_maxSensor3));

            double d_minSensor1 = l_dataStoringList.Min(s => s.d_FirSensorTemp);
            double d_minSensor2 = l_dataStoringList.Min(s => s.d_SecSensorTemp);
            double d_minSensor3 = l_dataStoringList.Min(s => s.d_ThiSensorTemp);
            double d_minAll = Math.Min(d_minSensor1, Math.Min(d_minSensor2, d_minSensor3));

            if (d_higherBoundary < d_maxAll)
                return true;
            else if (d_lowerBoundary > d_minAll)
                return true;

            return false;
        }
    }
}