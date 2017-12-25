using Android.OS;

namespace BeerProcessingManager.ServiceManagement
{
    class MyBoundServiceBinder : Binder
    {
        BoundService service;
        public MyBoundServiceBinder(BoundService service)
        {
            this.service = service;
        }
    }
}