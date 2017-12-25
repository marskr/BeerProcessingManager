using System;
using Android.OS;
using Android.Views;

namespace BeerProcessingManager
{
    /// <summary>
    /// In this class is stored generic part of application - tab bar.
    /// </summary>
    public class GenericViewPagerFragment : Android.Support.V4.App.Fragment
    {
        private Func<LayoutInflater, ViewGroup, Bundle, View> _view;

        public GenericViewPagerFragment (Func<LayoutInflater, ViewGroup, Bundle, View> view)
        {
            _view = view;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return _view(inflater, container, savedInstanceState);
        }
    }
}