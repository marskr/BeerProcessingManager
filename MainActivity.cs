using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using System;
using OxyPlot.Xamarin.Android;
using System.Collections.Generic;
using BeerProcessingManager.ThingspeakManagement;
using BeerProcessingManager.PlotManagement;
using BeerProcessingManager.LogManagement;
using ThingSpeakWinRT;
using Android.Media;

namespace BeerProcessingManager
{
    [Activity(Label = "BeerProcessingManager", MainLauncher = true)]
    public class MainActivity : FragmentActivity
    {
        Button btn_PopupMenu;
        Button btn_RetrieveFromTS;
        Button btn_RetrieveFromTS2;
        Button btn_TempShowData1;
        Button btn_TempShowData2;
        Button btn_TempShowData3;
        Button btn_TempShowData4;
        ImageButton ibtn_beerImage;
        MediaPlayer mp_player;
        List<Origin> l_dataStoringList = new List<Origin>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            btn_PopupMenu = FindViewById<Button>(Resource.Id.id_btnPopupMenu);
            btn_PopupMenu.Click += ShowPopupMenu;

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            ViewPager pager = FindViewById<ViewPager>(Resource.Id.id_pager);
            GenericFragmentPagerAdaptor adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);

            mp_player = MediaPlayer.Create(this, Resource.Raw.beer_pour);

            // BASIC
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.Basic, v, false);

                TextView textSample = view.FindViewById<TextView>(Resource.Id.id_txtBasic);
                textSample.Text = IntroText();
                ibtn_beerImage = view.FindViewById<ImageButton>(Resource.Id.id_imgBeer);
                ibtn_beerImage.Click += (s, arg) =>
                {
                    Toast.MakeText(this, string.Format("CHEERS! ;)"), ToastLength.Long).Show();
                    mp_player.Start();
                };

                return view;
            });

            // SHOW DATA
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.ShowData, v, false);

                btn_RetrieveFromTS = view.FindViewById<Button>(Resource.Id.id_btnRetrieveFromTS);
                btn_RetrieveFromTS.Click += async (s, arg) =>
                {
                    ListView viewList = view.FindViewById<ListView>(Resource.Id.id_vwListShowData);
                    try
                    {
                        ThingSpeakData feeds = await DataStorage.ReadThingspeak();
                        l_dataStoringList = DataStorage.ThingspeakConverter(feeds); //DataStorage.ArtificialList();
                        Toast.MakeText(this, string.Format("DATA OBTAINED!"), ToastLength.Long).Show();
                    }
                    catch /*(Exception ex)*/
                    {
                        Toast.MakeText(this, string.Format("CANNOT OBTAIN DATA FROM THINGSPEAK!"), ToastLength.Long).Show();
                        //Toast.MakeText(this, string.Format("The exception occured: {0}", ex.ToString()), ToastLength.Long).Show();
                    }

                    var adapter = new VwAdapter(this, l_dataStoringList, 5);
                    viewList.Adapter = adapter;
                };
                btn_TempShowData1 = view.FindViewById<Button>(Resource.Id.id_btnShowData1);
                btn_TempShowData1.Click += (s, arg) =>
                {
                    ListView viewList = view.FindViewById<ListView>(Resource.Id.id_vwListShowData);
                    //List<Origin> l_dataStoringList = new List<Origin>();
                    //l_dataStoringList = DataStorage.ArtificialList();

                    var adapter = new VwAdapter(this, l_dataStoringList, 1);
                    viewList.Adapter = adapter;
                };
                btn_TempShowData2 = view.FindViewById<Button>(Resource.Id.id_btnShowData2);
                btn_TempShowData2.Click += (s, arg) =>
                {
                    ListView viewList = view.FindViewById<ListView>(Resource.Id.id_vwListShowData);

                    var adapter = new VwAdapter(this, l_dataStoringList, 2);
                    viewList.Adapter = adapter;
                };
                btn_TempShowData3 = view.FindViewById<Button>(Resource.Id.id_btnShowData3);
                btn_TempShowData3.Click += (s, arg) =>
                {
                    ListView viewList = view.FindViewById<ListView>(Resource.Id.id_vwListShowData);

                    var adapter = new VwAdapter(this, l_dataStoringList, 3);
                    viewList.Adapter = adapter;
                };
                btn_TempShowData4 = view.FindViewById<Button>(Resource.Id.id_btnShowData4);
                btn_TempShowData4.Click += (s, arg) =>
                {
                    ListView viewList = view.FindViewById<ListView>(Resource.Id.id_vwListShowData);

                    var adapter = new VwAdapter(this, l_dataStoringList, 4);
                    viewList.Adapter = adapter;
                };

                return view;
            });

            // SHOW CHARTS
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.ShowCharts, v, false);

                btn_RetrieveFromTS2 = view.FindViewById<Button>(Resource.Id.id_btnRetrieveFromTS2);
                btn_RetrieveFromTS2.Click += async (s, arg) =>
                {
                    PlotView viewPlot = view.FindViewById<PlotView>(Resource.Id.id_plotView);
                    try
                    {
                        ThingSpeakData feeds = await DataStorage.ReadThingspeak();
                        l_dataStoringList = DataStorage.ThingspeakConverter(feeds); //DataStorage.ArtificialList();
                        Toast.MakeText(this, string.Format("DATA OBTAINED!"), ToastLength.Long).Show();
                    }
                    catch /*(Exception ex)*/
                    {
                        Toast.MakeText(this, string.Format("CANNOT OBTAIN DATA FROM THINGSPEAK!"), ToastLength.Long).Show();
                    }

                    viewPlot.Model = PlotManager.CreatePlotModel(l_dataStoringList);
                };

                return view;
            });

            // MODIFY PROCESSING
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.ModifyProcessing, v, false);
                return view;
            });

            // MODIFY VALVE
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.ModifyValve, v, false);
                return view;
            });

            pager.Adapter = adaptor;
            pager.SetOnPageChangeListener(listener: new ViewPageListenerForActionBar(ActionBar));

            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "BASIC"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "SHOW DATA"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "SHOW CHARTS"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "PROCESSING"));
            ActionBar.AddTab(pager.GetViewPageTab(ActionBar, "VALVE"));
        }
        private string IntroText()
        {
            return " This application so - called 'BeerProcessingManager'  " +
                   "was designed by Marcin Skryśkiewicz.\n " +
                   "The source code is stored in GitHub:\n " +
                   "https://github.com/marskr/BeerProcessingManager \n" +
                   "Please use 'CLICK ME' button to get more info!";
        }
        private void ShowPopupMenu(object sender, EventArgs e)
        {
            PopupMenu menu = new PopupMenu(this, btn_PopupMenu);
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
                        Toast.MakeText(this, string.Format("Provides charts, based on the DB data."), ToastLength.Long).Show();
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

