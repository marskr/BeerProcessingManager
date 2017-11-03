using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using System;

namespace BeerProcessingManager
{
    [Activity(Label = "BeerProcessingManager", MainLauncher = true)]
    public class MainActivity : FragmentActivity
    {
        Button btnShow1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            btnShow1 = FindViewById<Button>(Resource.Id.btnPopupMenu);
            btnShow1.Click += ShowPopupMenu;

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            var pager = FindViewById<ViewPager>(Resource.Id.pager);
            var adaptor = new GenericFragmentPagerAdaptor(SupportFragmentManager);
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.Basic, v, false);
                var textSample = view.FindViewById<TextView>(Resource.Id.txtBasic);
                textSample.Text = " This application, so - called 'BeerProcessingManager' " +
                                  "was designed by Marcin Skryśkiewicz.\n " +
                                  "The source code is stored in GitHub:\n " +
                                  "https://github.com/marskr/BeerProcessingManager \n";
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
        private void ShowPopupMenu(object sender, EventArgs e)
        {
            PopupMenu menu = new PopupMenu(this, btnShow1);
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

