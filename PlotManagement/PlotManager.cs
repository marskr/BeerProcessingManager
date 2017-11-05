using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using BeerProcessingManager.ThingspeakManagement;

namespace BeerProcessingManager.PlotManagement
{
    public class PlotManager
    {
        public static PlotModel CreatePlotModel(List<Origin> l_dataStoringList)
        {
            PlotModel plotModel = new PlotModel { Title = "Temperature (Time)" };
            var ChartColor1 = OxyColors.LightGreen;
            var ChartSettingsColor = OxyColors.WhiteSmoke;
            plotModel.DefaultColors = new List<OxyColor>
            {
                ChartColor1
            };
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
            var series1 = new LineSeries
            {
                MarkerType = MarkerType.Plus,
                MarkerSize = 5,
                MarkerStroke = ChartColor1
            };

            foreach (var element in l_dataStoringList)
            {
                series1.Points.Add(new DataPoint(element.d_Time, element.d_Temperature));
            }
            plotModel.Series.Add(series1);
            return plotModel;
        }
    }
}