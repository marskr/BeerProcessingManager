﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V4.App;
using Android.Support.V4.View;

namespace BeerProcessingManager
{
    /// <summary>
    /// In this class is stored generic part of application - tab bar (addition of fragment lists).
    /// </summary>
    public class GenericFragmentPagerAdaptor : FragmentPagerAdapter
    {
        private List<Android.Support.V4.App.Fragment> _fragmentList = new List<Android.Support.V4.App.Fragment>();

        public GenericFragmentPagerAdaptor(Android.Support.V4.App.FragmentManager fm) : base(fm){}

        public void AddFragmentView(Func<LayoutInflater, ViewGroup, Bundle, View> view)
        {
            _fragmentList.Add(new GenericViewPagerFragment(view));
        }

        public void AddFragment(GenericViewPagerFragment fragment)
        {
            _fragmentList.Add(fragment);
        }

        public override int Count
        {
            get
            {
                return _fragmentList.Count;
            }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return _fragmentList[position];
        }
    }

    /// <summary>
    /// In this class is stored generic part of application - tab bar. 
    /// </summary>
    public class ViewPageListenerForActionBar : ViewPager.SimpleOnPageChangeListener
    {
        private ActionBar _bar;

        public ViewPageListenerForActionBar(ActionBar bar)
        {
            _bar = bar;
        }

        public override void OnPageSelected(int position)
        {
            _bar.SetSelectedNavigationItem(position);
        }
    }

    /// <summary>
    /// In this class is stored generic part of application - tab bar.
    /// </summary>
    public static class ViewPagerExtensions
    {
        public static ActionBar.Tab GetViewPageTab (this ViewPager viewPager, ActionBar actionBar, string name)
        {
            var tab = actionBar.NewTab();
            tab.SetText(name);
            tab.TabSelected += (o, e) =>
            {
                viewPager.SetCurrentItem(actionBar.SelectedNavigationIndex, false);
            };
            return tab;
        }
    }
}