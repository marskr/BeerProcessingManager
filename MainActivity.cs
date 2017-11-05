﻿using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using System;
using OxyPlot.Xamarin.Android;
using System.Collections.Generic;
using BeerProcessingManager.ThingspeakManagement;
using BeerProcessingManager.PlotManagement;

namespace BeerProcessingManager
{
    [Activity(Label = "BeerProcessingManager", MainLauncher = true)]
    public class MainActivity : FragmentActivity
    {
        Button b_btnPopupMenu;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            b_btnPopupMenu = FindViewById<Button>(Resource.Id.id_btnPopupMenu);
            b_btnPopupMenu.Click += ShowPopupMenu;

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            ViewPager pager = FindViewById<ViewPager>(Resource.Id.id_pager);
            GenericFragmentPagerAdaptor adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.Basic, v, false);
                var textSample = view.FindViewById<TextView>(Resource.Id.id_txtBasic);
                textSample.Text = IntroText();
                return view;
            });
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.ShowData, v, false);
                return view;
            });
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.ShowCharts, v, false);
                PlotView viewPlot = view.FindViewById<PlotView>(Resource.Id.id_plotView);

                List<Origin> l_dataStoringList = new List<Origin>();
                l_dataStoringList = DataStorage.ArtificialList();
     
                viewPlot.Model = PlotManager.CreatePlotModel(l_dataStoringList);
                return view;
            });
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.ModifyProcessing, v, false);
                return view;
            });
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.ModifyValve, v, false);
                return view;
            });

            pager.Adapter = adaptor;
            pager.SetOnPageChangeListener(new ViewPageListenerForActionBar(ActionBar));

            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "BASIC"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "SHOW DATA"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "SHOW CHARTS"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "PROCESSING"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "VALVE"));
        }
        private string IntroText()
        {
            return " This application, so - called 'BeerProcessingManager' " +
                   "was designed by Marcin Skryśkiewicz.\n " +
                   "The source code is stored in GitHub:\n " +
                   "https://github.com/marskr/BeerProcessingManager \n";
        }
        private void ShowPopupMenu(object sender, EventArgs e)
        {
            PopupMenu menu = new PopupMenu(this, b_btnPopupMenu);
            menu.MenuInflater.Inflate(Resource.Menu.MainPopup, menu.Menu);

            menu.MenuItemClick += (s, arg) =>
            {
                switch (arg.Item.ItemId)
                {
                    case Resource.Id.choiceBasic:
                        Toast.MakeText(this, string.Format("Contains basic informations about the application." /*arg.Item.TitleFormatted*/), ToastLength.Long).Show();
                        break;
                    case Resource.Id.choiceShowData:
                        Toast.MakeText(this, string.Format("Is about the data gathered from the Thingspeak."), ToastLength.Long).Show();
                        break;
                    case Resource.Id.choiceShowCharts:
                        Toast.MakeText(this, string.Format("Provides charts created based on the DB data."), ToastLength.Long).Show();
                        break;
                    case Resource.Id.choiceModifyProcessing:
                        Toast.MakeText(this, string.Format("Delivers controll of process (danger boundaries)."), ToastLength.Long).Show();
                        break;
                    case Resource.Id.choiceModifyValve:
                        Toast.MakeText(this, string.Format("Delivers controll of process (valve controls)."), ToastLength.Long).Show();
                        break;
                    default:
                        Toast.MakeText(this, string.Format("UNKNOWN SELECTION", arg.Item.TitleFormatted), ToastLength.Short).Show();
                        break;
                }
            };

            //menu.DismissEvent += (s, arg) =>
            //{
            //    Toast.MakeText(this, string.Format("Menu dissmissed"), ToastLength.Short).Show();
            //};

            menu.Show();
        }
        
    }
}

