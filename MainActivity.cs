using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using System;
using OxyPlot.Xamarin.Android;
using BeerProcessingManager.ThingspeakManagement;
using BeerProcessingManager.PlotManagement;
using ThingSpeakWinRT;
using Android.Media;
using BeerProcessingManager.MainActivityResorces;
using Android.Content;
using BeerProcessingManager.ServiceManagement;
using System.Net.Sockets;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Globalization;

namespace BeerProcessingManager
{
    [Activity(Label = "BeerProcessingManager", MainLauncher = true)]
    public class MainActivity : FragmentActivity, SeekBar.IOnSeekBarChangeListener
    {
        Button btn_PopupMenu;
        Button btn_PopupShowCharts;
        Button btn_RetrieveFromTS;
        Button btn_RetrieveFromTS2;
        Button btn_ShowCharts;
        Button btn_TempShowData1;
        Button btn_TempShowData2;
        Button btn_TempShowData3;
        Button btn_TempShowData4;
        Button btn_GetSBSettings;
        Button btn_LaunchWatchdog;
        Button btn_StopWatchdog;
        
        ImageButton ibtn_beerImage;
        MediaPlayer mp_player;
        TextView tvw_HigherTempBD, tvw_LowerTempBD;
        SeekBar skb_HigherTempBD, skb_LowerTempBD;
        string[] s_arrangementTab = new string[] { "BASIC", "SHOW DATA", "SHOW CHARTS", "PROCESSING", "VALVE"};

        // MQTT connection
        private TcpClient client;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private readonly BackgroundWorker worker2 = new BackgroundWorker();
        public StreamReader STR;
        public StreamWriter STW;
        public string receive;
        public String text_to_send;
        EditText edt_EdIP1, edt_EdPort1, edt_EdOutput1, edt_EdIP2, edt_EdPort2, edt_EdOutput2;
        Button btn_StartServer;
        Button btn_Connect;
        Button btn_Send;
        // MQTT end

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
            SingletonOxy.Instance.i_Plotchoice = 0;

            // BASIC
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.Basic, v, false);

                TextView textSample = view.FindViewById<TextView>(Resource.Id.id_txtBasic);
                textSample.Text = AdditionalResource.IntroText();
                ibtn_beerImage = view.FindViewById<ImageButton>(Resource.Id.id_imgBeer);
                ibtn_beerImage.Click += ShowCheers;

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
                        SingletonTSList.Instance.l_dataStoringList.Clear();
                        ThingSpeakData feeds = await DataStorage.ReadThingspeak();
                        SingletonTSList.Instance.l_dataStoringList = DataStorage.ThingspeakConverter(feeds); //DataStorage.ArtificialList();
                        Toast.MakeText(this, string.Format("DATA OBTAINED!"), ToastLength.Long).Show();
                    }
                    catch /*(Exception ex)*/
                    {
                        Toast.MakeText(this, string.Format("CANNOT OBTAIN DATA FROM THINGSPEAK!"), ToastLength.Long).Show();
                        //Toast.MakeText(this, string.Format(ex.ToString()), ToastLength.Long).Show();
                    }

                    var adapter = new VwAdapter(this, SingletonTSList.Instance.l_dataStoringList, 5);
                    viewList.Adapter = adapter;
                };

                btn_TempShowData1 = view.FindViewById<Button>(Resource.Id.id_btnShowData1);
                btn_TempShowData1.Click += (s, arg) =>
                {
                    ListView viewList = view.FindViewById<ListView>(Resource.Id.id_vwListShowData);

                    var adapter = new VwAdapter(this, SingletonTSList.Instance.l_dataStoringList, 1);
                    viewList.Adapter = adapter;
                };

                btn_TempShowData2 = view.FindViewById<Button>(Resource.Id.id_btnShowData2);
                btn_TempShowData2.Click += (s, arg) =>
                {
                    ListView viewList = view.FindViewById<ListView>(Resource.Id.id_vwListShowData);

                    var adapter = new VwAdapter(this, SingletonTSList.Instance.l_dataStoringList, 2);
                    viewList.Adapter = adapter;
                };

                btn_TempShowData3 = view.FindViewById<Button>(Resource.Id.id_btnShowData3);
                btn_TempShowData3.Click += (s, arg) =>
                {
                    ListView viewList = view.FindViewById<ListView>(Resource.Id.id_vwListShowData);

                    var adapter = new VwAdapter(this, SingletonTSList.Instance.l_dataStoringList, 3);
                    viewList.Adapter = adapter;
                };

                btn_TempShowData4 = view.FindViewById<Button>(Resource.Id.id_btnShowData4);
                btn_TempShowData4.Click += (s, arg) =>
                {
                    ListView viewList = view.FindViewById<ListView>(Resource.Id.id_vwListShowData);

                    var adapter = new VwAdapter(this, SingletonTSList.Instance.l_dataStoringList, 4);
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
                    try
                    {
                        SingletonTSList.Instance.l_dataStoringList.Clear();
                        ThingSpeakData feeds = await DataStorage.ReadThingspeak();
                        SingletonTSList.Instance.l_dataStoringList = DataStorage.ThingspeakConverter(feeds);
                        Toast.MakeText(this, string.Format("DATA OBTAINED!"), ToastLength.Long).Show();
                    }
                    catch
                    {
                        Toast.MakeText(this, string.Format("CANNOT OBTAIN DATA FROM THINGSPEAK!"), ToastLength.Long).Show();
                    }
                };

                btn_PopupShowCharts = view.FindViewById<Button>(Resource.Id.id_btnChangeGraph);
                btn_PopupShowCharts.Click += ShowPopupCharts;

                btn_ShowCharts = view.FindViewById<Button>(Resource.Id.id_btnShowGraph);
                btn_ShowCharts.Click += (s, arg) =>
                {
                    SingletonOxy.Instance.viewPlot = view.FindViewById<PlotView>(Resource.Id.id_plotView);
                    SingletonOxy.Instance.viewPlot.Model = PlotManager.CreatePlotModel(SingletonTSList.Instance.l_dataStoringList);
                };

                return view;
            });

            // MODIFY PROCESSING
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.ModifyProcessing, v, false);

                skb_HigherTempBD = view.FindViewById<SeekBar>(Resource.Id.id_skbHigherBoundary);
                tvw_HigherTempBD = view.FindViewById<TextView>(Resource.Id.id_txtSeekBarHighB);
                tvw_HigherTempBD.Text = "0";

                skb_LowerTempBD = view.FindViewById<SeekBar>(Resource.Id.id_skbLowerBoundary);
                tvw_LowerTempBD = view.FindViewById<TextView>(Resource.Id.id_txtSeekBarLowB);
                tvw_LowerTempBD.Text = "0";

                skb_HigherTempBD.SetOnSeekBarChangeListener(this);
                skb_LowerTempBD.SetOnSeekBarChangeListener(this);

                btn_GetSBSettings = view.FindViewById<Button>(Resource.Id.id_btnGetSBSettings);
                btn_GetSBSettings.Click += (s, arg) =>
                {
                    double d_vwTxt1 = 0, d_vwTxt2 = 0;
                    if (!Double.TryParse(tvw_HigherTempBD.Text, out d_vwTxt1))
                        Toast.MakeText(this, string.Format("Problem with parsing text value."), ToastLength.Long).Show();

                    if (!Double.TryParse(tvw_LowerTempBD.Text, out d_vwTxt2))
                        Toast.MakeText(this, string.Format("Problem with parsing text value."), ToastLength.Long).Show();

                    SingletonTSList.Instance.st_WatchdogStorage.i_FirstskBar = d_vwTxt1;
                    SingletonTSList.Instance.st_WatchdogStorage.i_SecondskBar = d_vwTxt2;
                    Toast.MakeText(this, "Settings obtained!", ToastLength.Long).Show();
                };

                btn_LaunchWatchdog = view.FindViewById<Button>(Resource.Id.id_btnLaunchWG);
                btn_StopWatchdog = view.FindViewById<Button>(Resource.Id.id_btnStopWG);

                btn_LaunchWatchdog.Click += StartStartedService;
                btn_StopWatchdog.Click += StopStartedService;  

                return view;
            });

            // MODIFY VALVE
            adaptor.AddFragmentView((i, v, b) =>
            {
                var view = i.Inflate(Resource.Layout.ModifyValve, v, false);

                edt_EdIP1 = view.FindViewById<EditText>(Resource.Id.id_txtEdIP1);
                edt_EdPort1 = view.FindViewById<EditText>(Resource.Id.id_txtEdPort1);
                edt_EdOutput1 = view.FindViewById<EditText>(Resource.Id.id_txtEdOutput1);
                edt_EdIP2 = view.FindViewById<EditText>(Resource.Id.id_txtEdIP2);
                edt_EdPort2 = view.FindViewById<EditText>(Resource.Id.id_txtEdPort2);
                edt_EdOutput2 = view.FindViewById<EditText>(Resource.Id.id_txtEdOutput2);

                worker.DoWork += (s, arg) =>
                {
                    while (client.Connected)
                    {
                        try
                        {
                            receive = STR.ReadLine();
                            //this.txtEdOutput1.Text = receive;//Dispatcher.Invoke(new Action(delegate () { txtEdOutput1.AppendText("You : " + receive + "\n"); }));
                            receive = "";
                        }
                        catch (Exception ex)
                        {
                            Toast.MakeText(this, ex.Message.ToString(), ToastLength.Long).Show();
                        }
                    }
                };

                worker2.DoWork += (s, arg) =>
                {
                    if (client.Connected)
                    {
                        STW.WriteLine(text_to_send);
                        
                        //this.txtEdOutput1.Text = text_to_send; //Dispatcher.Invoke(new Action(delegate () { TextBox2.AppendText("Me : " + text_to_send + "\n"); }));
                    }
                    else
                    {
                        Toast.MakeText(this, "Send failed!", ToastLength.Long).Show();
                    }
                    worker2.CancelAsync();
                };

                btn_StartServer = view.FindViewById<Button>(Resource.Id.id_btnStartServer);
                btn_StartServer.Click += (s, arg) =>
                {
                    TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(edt_EdPort1.Text));
                    listener.Start();
                    client = listener.AcceptTcpClient();
                    STR = new StreamReader(client.GetStream());
                    STW = new StreamWriter(client.GetStream());
                    STW.AutoFlush = true;

                    worker.RunWorkerAsync(); // Start receiving data from background
                    worker2.WorkerSupportsCancellation = true;  // Ability to cancel thread
                };

                btn_Connect = view.FindViewById<Button>(Resource.Id.id_btnConnect);
                btn_Connect.Click += (s, arg) =>
                {
                    client = new TcpClient();
                    IPEndPoint IP_End = new IPEndPoint(IPAddress.Parse(edt_EdIP2.Text), int.Parse(edt_EdPort2.Text, CultureInfo.InvariantCulture));

                    try
                    {
                        client.Connect(IP_End);
                        if (client.Connected)
                        {
                            edt_EdOutput1.Append("Connected to server");
                            STW = new StreamWriter(client.GetStream());
                            STR = new StreamReader(client.GetStream());
                            STW.AutoFlush = true;

                            worker.RunWorkerAsync(); // Start receiving data from background
                            worker2.WorkerSupportsCancellation = true;  // Ability to cancel thread
                        }
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(this, ex.Message.ToString(), ToastLength.Long).Show();
                    }
                };

                btn_Send = view.FindViewById<Button>(Resource.Id.id_btnSend);
                btn_Send.Click += (s, arg) =>
                {
                    if (edt_EdOutput2.Text != "")
                    {
                        text_to_send = edt_EdOutput2.Text;
                        worker2.RunWorkerAsync();
                    }
                    edt_EdOutput2.Text = "";
                };

                IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (IPAddress address in localIP)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        edt_EdIP1.Text = address.ToString();
                        edt_EdIP2.Text = "10.0.0.72";
                    }
                }

                return view;
            });

            pager.Adapter = adaptor;
#pragma warning disable CS0618 // Type or member is obsolete
            pager.SetOnPageChangeListener(listener: new ViewPageListenerForActionBar(ActionBar));
#pragma warning restore CS0618 // Type or member is obsolete

            foreach(var element in s_arrangementTab)
            {
                ActionBar.AddTab(pager.GetViewPageTab(ActionBar, element));
            }
        }

        private void ShowPopupMenu(object sender, EventArgs e)
        {
            PopupMenu menu = new PopupMenu(this, btn_PopupMenu);
            menu.MenuInflater.Inflate(Resource.Menu.MainPopup, menu.Menu);

            menu.MenuItemClick += (s, arg) =>
            {
                switch (arg.Item.ItemId)
                {
                    case Resource.Id.id_choiceBasic:
                        Toast.MakeText(this, string.Format("Contains basic informations about the application."), ToastLength.Long).Show();
                        break;
                    case Resource.Id.id_choiceShowData:
                        Toast.MakeText(this, string.Format("Is about the data gathered from the Thingspeak."), ToastLength.Long).Show();
                        break;
                    case Resource.Id.id_choiceShowCharts:
                        Toast.MakeText(this, string.Format("Provides charts, based on the DB data."), ToastLength.Long).Show();
                        break;
                    case Resource.Id.id_choiceModifyProcessing:
                        Toast.MakeText(this, string.Format("Delivers controll of process (danger boundaries)."), ToastLength.Long).Show();
                        break;
                    case Resource.Id.id_choiceModifyValve:
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

        private void ShowCheers(object sender, EventArgs e)
        {
            Toast.MakeText(this, string.Format("CHEERS! ;)"), ToastLength.Long).Show();
            mp_player.Start();
        }

        private void ShowPopupCharts(object sender, EventArgs e)
        {
            PopupMenu menu2 = new PopupMenu(this, btn_PopupShowCharts);
            menu2.MenuInflater.Inflate(Resource.Menu.ShowChartsPopup, menu2.Menu);

            menu2.MenuItemClick += (s, arg) =>
            {
                switch (arg.Item.ItemId)
                {
                    case Resource.Id.id_choiceSensor1:
                        SingletonOxy.Instance.i_Plotchoice = 1;
                        Toast.MakeText(this, string.Format("Graph 1!"), ToastLength.Short).Show();
                        break;
                    case Resource.Id.id_choiceSensor2:
                        SingletonOxy.Instance.i_Plotchoice = 2;
                        Toast.MakeText(this, string.Format("Graph 2!"), ToastLength.Short).Show();
                        break;
                    case Resource.Id.id_choiceSensor3:
                        SingletonOxy.Instance.i_Plotchoice = 3;
                        Toast.MakeText(this, string.Format("Graph 3!"), ToastLength.Short).Show();
                        break;
                    case Resource.Id.id_choiceSensorAvg:
                        SingletonOxy.Instance.i_Plotchoice = 4;
                        Toast.MakeText(this, string.Format("Graph Avg!"), ToastLength.Short).Show();
                        break;
                    case Resource.Id.id_choiceAll:
                        SingletonOxy.Instance.i_Plotchoice = 0;
                        Toast.MakeText(this, string.Format("All graphs!"), ToastLength.Short).Show();
                        break;
                    case Resource.Id.id_clearAll:
                        SingletonOxy.Instance.i_Plotchoice = -1;
                        Toast.MakeText(this, string.Format("Remove graphs!"), ToastLength.Short).Show();
                        break;
                    default:
                        Toast.MakeText(this, string.Format("UNKNOWN SELECTION", arg.Item.TitleFormatted), ToastLength.Short).Show();
                        break;
                }
            };

            menu2.Show();
        }

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            if (fromUser)
            {
                if(seekBar.Equals(skb_HigherTempBD))
                {
                    tvw_HigherTempBD.Text = string.Format(progress.ToString());
                }
                else if (seekBar.Equals(skb_LowerTempBD))
                {
                    tvw_LowerTempBD.Text = string.Format(progress.ToString());
                }
            }
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
            //
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            //
        }

        private void StopStartedService(object sender, System.EventArgs e)
        {
            StopService(new Intent(this, typeof(MyServices)));
        }

        private void StartStartedService(object sender, System.EventArgs e)
        {
            StartService(new Intent(this, typeof(MyServices)));
        }
    }
}

