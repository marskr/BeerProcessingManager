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
            var ChartColor2 = OxyColors.LightPink;
            var ChartColor3 = OxyColors.LightGray;
            var ChartColor4 = OxyColors.Violet;
            var ChartSettingsColor = OxyColors.WhiteSmoke;
            plotModel.DefaultColors = new List<OxyColor>
            {
                ChartColor1,
                ChartColor2,
                ChartColor3, 
                ChartColor4
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
            var series2 = new LineSeries
            {
                MarkerType = MarkerType.Cross,
                MarkerSize = 5,
                MarkerStroke = ChartColor2
            };
            var series3 = new LineSeries
            {
                MarkerType = MarkerType.Star,
                MarkerSize = 5,
                MarkerStroke = ChartColor3
            };
            var series4 = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerStroke = ChartColor4
            };
            foreach (var element in l_dataStoringList)
            {
                series1.Points.Add(new DataPoint(element.d_Time, element.d_FirSensorTemp));
                series2.Points.Add(new DataPoint(element.d_Time, element.d_SecSensorTemp));
                series3.Points.Add(new DataPoint(element.d_Time, element.d_ThiSensorTemp));
                series4.Points.Add(new DataPoint(element.d_Time, element.d_AvgSensorTemp));
            }
            plotModel.Series.Add(series1);
            plotModel.Series.Add(series2);
            plotModel.Series.Add(series3);
            plotModel.Series.Add(series4);
            return plotModel;
        }
    }
}