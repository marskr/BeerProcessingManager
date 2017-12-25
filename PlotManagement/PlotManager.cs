using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using BeerProcessingManager.ThingspeakManagement;
using OxyPlot.Xamarin.Android;

namespace BeerProcessingManager.PlotManagement
{
    /// <summary>
    /// Singleton provided to store settings bounded with plot designing and creation.
    /// </summary>
    public sealed class SingletonOxy
    {
        private static SingletonOxy SingletonInstance = null;
        private static readonly object Lock = new object();

        // basic plot settings
        public PlotModel plotModel = new PlotModel { Title = "Temperature (Time)" };
        public PlotView viewPlot;
        public OxyColor ChartColor1 = OxyColors.LightGreen;
        public OxyColor ChartColor2 = OxyColors.LightPink;
        public OxyColor ChartColor3 = OxyColors.LightGray;
        public OxyColor ChartColor4 = OxyColors.Violet;
        public OxyColor ChartSettingsColor = OxyColors.WhiteSmoke;
        public int i_Plotchoice { get; set; }
        public bool b_axesCreate = false;

        /// <summary>
        /// Here are stored operations to lock an instance (to provide singleton).
        /// </summary>
        public static SingletonOxy Instance
        {
            get
            {
                lock (Lock)
                {
                    if (SingletonInstance == null)
                    {
                        SingletonInstance = new SingletonOxy();
                    }
                    return SingletonInstance;
                }
            }
        }
    }

    /// <summary>
    /// Class operating on creation of plot model (using list gained from thingspeak).
    /// </summary>
    public class PlotManager
    {
        /// <summary>
        /// Creation of plot. Uses list gained from thingspeak.
        /// </summary>
        /// <param name="l_dataStoringList">The list gained from thingspeak.</param>
        /// <returns>Returns a plot created in method.</returns>
        public static PlotModel CreatePlotModel(List<Origin> l_dataStoringList)
        {
            SingletonOxy.Instance.plotModel.InvalidatePlot(true);
            if (!SingletonOxy.Instance.b_axesCreate)
            {
                SingletonOxy.Instance.plotModel = PlotOps(SingletonOxy.Instance.plotModel, 
                                                          SingletonOxy.Instance.ChartSettingsColor);
                SingletonOxy.Instance.b_axesCreate = true;
            }

            SingletonOxy.Instance.plotModel = SeriesOps(l_dataStoringList, 
                                                        SingletonOxy.Instance.ChartColor1,
                                                        SingletonOxy.Instance.ChartColor2,
                                                        SingletonOxy.Instance.ChartColor3,
                                                        SingletonOxy.Instance.ChartColor4);

            return SingletonOxy.Instance.plotModel;
        }

        /// <summary>
        /// Method that is responsible of axes creation & text processing (i.e. font).
        /// </summary>
        /// <param name="plotModel">Takes as an argument plot created in previous method.</param>
        /// <param name="ChartSettingsColor">That's color of axes and all font related items.</param>
        /// <returns>Returns a plot processed in method.</returns>
        public static PlotModel PlotOps(PlotModel plotModel, OxyColor ChartSettingsColor)
        {
            plotModel.PlotAreaBorderThickness.Equals(0);
            plotModel.PlotAreaBorderColor = ChartSettingsColor;
            plotModel.TitleColor = ChartSettingsColor;
            plotModel.TitleFontSize = 20;
            plotModel.TitleFont = "Arial Narrow";
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Maximum = 10,
                Minimum = 0,
                AxislineColor = ChartSettingsColor,
                MajorGridlineColor = ChartSettingsColor,
                MinorGridlineColor = ChartSettingsColor,
                TicklineColor = ChartSettingsColor,
                TextColor = ChartSettingsColor,
                MinorGridlineThickness = 2,
                MajorGridlineThickness = 2
            });
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Maximum = 10,
                Minimum = 0,
                AxislineColor = ChartSettingsColor,
                MajorGridlineColor = ChartSettingsColor,
                MinorGridlineColor = ChartSettingsColor,
                TicklineColor = ChartSettingsColor,
                TextColor = ChartSettingsColor,
                MinorGridlineThickness = 2,
                MajorGridlineThickness = 2
            });
            return plotModel;
        }

        /// <summary>
        /// Method responsible for individual plot colors.
        /// </summary>
        /// <param name="l_dataStoringList">The list provided from thingspeak.</param>
        /// <param name="ChartColor1">The color of first graph.</param>
        /// <param name="ChartColor2">The color of second graph.</param>
        /// <param name="ChartColor3">The color of third graph.</param>
        /// <param name="ChartColor4">The color of fourth graph.</param>
        /// <returns>Returns a plot processed in method.</returns>
        public static PlotModel SeriesOps(List<Origin> l_dataStoringList,
                                          OxyColor ChartColor1, 
                                          OxyColor ChartColor2, 
                                          OxyColor ChartColor3, 
                                          OxyColor ChartColor4)
        {
            SingletonOxy.Instance.plotModel.DefaultColors = new List<OxyColor>
            {
                ChartColor1,
                ChartColor2,
                ChartColor3,
                ChartColor4
            };
            LineSeries series1 = MakeSeries(MarkerType.Plus, 2, ChartColor1);
            LineSeries series2 = MakeSeries(MarkerType.Cross, 2, ChartColor2);
            LineSeries series3 = MakeSeries(MarkerType.Star, 2, ChartColor3);
            LineSeries series4 = MakeSeries(MarkerType.Circle, 2, ChartColor4);

            switch(SingletonOxy.Instance.i_Plotchoice)
            {
                case 0:
                    foreach (var element in l_dataStoringList)
                    {
                        series1.Points.Add(new DataPoint(element.d_Time, element.d_FirSensorTemp));
                        series2.Points.Add(new DataPoint(element.d_Time, element.d_SecSensorTemp));
                        series3.Points.Add(new DataPoint(element.d_Time, element.d_ThiSensorTemp));
                        series4.Points.Add(new DataPoint(element.d_Time, element.d_AvgSensorTemp));
                    }
                    SingletonOxy.Instance.plotModel.Series.Add(series1);
                    SingletonOxy.Instance.plotModel.Series.Add(series2);
                    SingletonOxy.Instance.plotModel.Series.Add(series3);
                    SingletonOxy.Instance.plotModel.Series.Add(series4);
                    break;
                case 1:
                    foreach (var element in l_dataStoringList)
                    {
                        series1.Points.Add(new DataPoint(element.d_Time, element.d_FirSensorTemp));
                    }
                    SingletonOxy.Instance.plotModel.Series.Add(series1);
                    break;
                case 2:
                    foreach (var element in l_dataStoringList)
                    {
                        series2.Points.Add(new DataPoint(element.d_Time, element.d_SecSensorTemp));
                    }
                    SingletonOxy.Instance.plotModel.Series.Add(series2);
                    break;
                case 3:
                    foreach (var element in l_dataStoringList)
                    {
                        series3.Points.Add(new DataPoint(element.d_Time, element.d_ThiSensorTemp));
                    }
                    SingletonOxy.Instance.plotModel.Series.Add(series3);
                    break;
                case 4:
                    foreach (var element in l_dataStoringList)
                    {
                        series4.Points.Add(new DataPoint(element.d_Time, element.d_AvgSensorTemp));
                    }
                    SingletonOxy.Instance.plotModel.Series.Add(series4);
                    break;
                case -1:
                    SingletonOxy.Instance.plotModel.Series.Clear();
                    break;
            }

            return SingletonOxy.Instance.plotModel;
        }

        /// <summary>
        /// The class responsible for series operations simplification.
        /// </summary>
        /// <param name="o_type"></param>
        /// <param name="d_size"></param>
        /// <param name="color"></param>
        /// <returns>Returns line series.</returns>
        public static LineSeries MakeSeries(OxyPlot.MarkerType o_type, double d_size, OxyColor color)
        {
            LineSeries series = new LineSeries
            {
                MarkerType = o_type,
                MarkerSize = d_size,
                MarkerStroke = color
            };
            return series;
        }
    }
}