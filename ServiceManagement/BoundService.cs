using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace BeerProcessingManager.ServiceManagement
{
    [Service]
    class BoundService : Service  
    {  
        private MyBoundServiceBinder binder;

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override IBinder OnBind(Intent intent)
        {
            binder = new MyBoundServiceBinder(this);
            Toast.MakeText(this, "OnBind() method start from BoundService", ToastLength.Long).Show();
            Toast.MakeText(this, "Bound Service is started", ToastLength.Long).Show();
            return binder;
        }

        public override bool OnUnbind(Intent intent)
        {
            Toast.MakeText(this, "OnUnBind() Method Called from BoundService.cs", ToastLength.Long).Show();
            return base.OnUnbind(intent);
        }

        public override void OnDestroy()
        {
            Toast.MakeText(this, "Bound Service Destroyed", ToastLength.Long).Show();
            base.OnDestroy();
        }
    }  
}