using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibPlateAnalysis;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace HCSAnalyzer.Classes
{
    class CDisplayGraph : cScreening
    {

        public string Title = "";

        public SimpleForm NewWindow = new SimpleForm();


        public CDisplayGraph(double[] Values)
        {
            SimpleForm NewWindow = new SimpleForm();
            Series SeriesPos = new Series();
            SeriesPos.ShadowOffset = 1;

            for (int IdxValue = 0; IdxValue < Values.Length; IdxValue++)
            {
                SeriesPos.Points.AddY(Values[IdxValue]);
                SeriesPos.Points[IdxValue].Color = Color.Black;
            }

            ChartArea CurrentChartArea = new ChartArea();
            CurrentChartArea.BorderColor = Color.Black;

            NewWindow.chartForSimpleForm.ChartAreas.Add(CurrentChartArea);
            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
            CurrentChartArea.AxisX.LabelStyle.Format = "N2";

            NewWindow.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackGradientStyle = GradientStyle.TopBottom;
            CurrentChartArea.BackSecondaryColor = Color.White;

            SeriesPos.ChartType = SeriesChartType.Column;
            NewWindow.chartForSimpleForm.Series.Add(SeriesPos);

            NewWindow.chartForSimpleForm.ChartAreas[0].CursorX.IsUserEnabled = true;
            NewWindow.chartForSimpleForm.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            NewWindow.chartForSimpleForm.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            NewWindow.chartForSimpleForm.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

          //  if (GlobalInfo.OptionsWindow.checkBoxDisplayHistoStats.Checked)
            {
                //StripLine AverageLine = new StripLine();
                //AverageLine.BackColor = Color.Black;
                //AverageLine.IntervalOffset = Pos.Mean();
                //AverageLine.StripWidth = double.Epsilon;
                //CurrentChartArea.AxisX.StripLines.Add(AverageLine);
                //AverageLine.Text = String.Format("{0:0.###}", AverageLine.IntervalOffset);

                //StripLine StdLine = new StripLine();
                //StdLine.BackColor = Color.FromArgb(64, Color.Black);
                //double Std = Pos.Std();
                //StdLine.IntervalOffset = AverageLine.IntervalOffset - 0.5 * Std;
                //StdLine.StripWidth = Std;
                //CurrentChartArea.AxisX.StripLines.Add(StdLine);
                //AverageLine.StripWidth = 0.0001;
            }

            NewWindow.Show();
            NewWindow.chartForSimpleForm.Update();
            NewWindow.chartForSimpleForm.Show();
            NewWindow.Controls.AddRange(new System.Windows.Forms.Control[] { NewWindow.chartForSimpleForm });
            NewWindow.Show();
        }


    }
}
