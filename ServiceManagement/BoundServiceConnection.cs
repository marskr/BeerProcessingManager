using System;
using Android.Content;
using Android.OS;

namespace BeerProcessingManager.ServiceManagement
{
    class BoundServiceConnection : Java.Lang.Object, IServiceConnection
    {
        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            Console.WriteLine("OnServiceConnected() Method called");
        }
        public void OnServiceDisconnected(ComponentName name)
        {
            Console.WriteLine("OnServiceDisConnected() Method called");
        }
    }
}