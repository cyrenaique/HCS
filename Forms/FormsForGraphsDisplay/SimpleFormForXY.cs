using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace LibPlateAnalysis
{
    public partial class SimpleFormForXY : Form
    {
        public cScreening CompleteScreening = null;

        private bool IsFullScreen;

        public SimpleFormForXY(bool IsFullScreen)
        {
            InitializeComponent();
            this.IsFullScreen = IsFullScreen;
        }

        private void saveGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog CurrSaveFileDialog = new SaveFileDialog();
            CurrSaveFileDialog.Filter = "PNG files (*.png)|*.png";
            System.Windows.Forms.DialogResult Res = CurrSaveFileDialog.ShowDialog();
            if (Res != System.Windows.Forms.DialogResult.OK) return;

            string CurrentPath = CurrSaveFileDialog.FileName;
            this.chartForSimpleFormXY.SaveImage(CurrentPath, ChartImageFormat.Png);

            MessageBox.Show("File saved !");
        }

        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            this.chartForSimpleFormXY.SaveImage(ms, ChartImageFormat.Bmp);
            Bitmap bm = new Bitmap(ms);
            Clipboard.SetImage(bm);
        }

        public string GetValues()
        {

            StringBuilder sb = new StringBuilder();

            if (this.chartForSimpleFormXY.Titles.Count != 0)
                sb.Append(this.chartForSimpleFormXY.Titles[0].Text + "\n");
            sb.Append("Axis X: " + this.chartForSimpleFormXY.ChartAreas[0].AxisX.Title + "\n");
            sb.Append("Axis Y: " + this.chartForSimpleFormXY.ChartAreas[0].AxisY.Title + "\n");


            if (this.chartForSimpleFormXY.Series[0].Name.ToString() == "Matrix")
            {
                sb.Append("\t");

                for (int X = 0; X < this.chartForSimpleFormXY.ChartAreas[0].AxisX.CustomLabels.Count; X++)
                    sb.Append(this.chartForSimpleFormXY.ChartAreas[0].AxisX.CustomLabels[X].Text + "\t");

                sb.Append("\n");
                for (int Y = 0; Y < this.chartForSimpleFormXY.ChartAreas[0].AxisY.CustomLabels.Count; Y++)
                {
                    sb.Append(this.chartForSimpleFormXY.ChartAreas[0].AxisY.CustomLabels[Y].Text + "\t");

                    for (int X = 0; X < this.chartForSimpleFormXY.ChartAreas[0].AxisX.CustomLabels.Count; X++)
                        sb.Append(this.chartForSimpleFormXY.Series[0].Points[X + Y * this.chartForSimpleFormXY.ChartAreas[0].AxisX.CustomLabels.Count].ToolTip + "\t");

                    sb.Append("\n");
                }
            }
            else
            {
                //sb.Append("\t");
                for (int Serie = 0; Serie < this.chartForSimpleFormXY.Series.Count; Serie++)
                {
                    sb.Append(this.chartForSimpleFormXY.Series[Serie].Label + " X values\t");
                    if (this.chartForSimpleFormXY.ChartAreas[0].AxisX.CustomLabels.Count > 0)
                    {
                        for (int X = 0; X < this.chartForSimpleFormXY.ChartAreas[0].AxisX.CustomLabels.Count; X++)
                            sb.Append(this.chartForSimpleFormXY.ChartAreas[0].AxisX.CustomLabels[X].Text.Replace("\n", " ") + "\t");

                    }
                    else
                    {

                        for (int i = 0; i < this.chartForSimpleFormXY.Series[Serie].Points.Count; i++)
                            sb.Append(String.Format("{0}\t", this.chartForSimpleFormXY.Series[Serie].Points[i].XValue));
                    }
                    sb.Append("\n");

                    sb.Append(this.chartForSimpleFormXY.Series[Serie].Label + " Y Values\t");
                    for (int i = 0; i < this.chartForSimpleFormXY.Series[Serie].Points.Count; i++)
                        sb.Append(String.Format("{0}\t", this.chartForSimpleFormXY.Series[Serie].Points[i].YValues[0]));
                }
            }
            return sb.ToString();
        }

        private void dataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(GetValues());
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.chartForSimpleFormXY.Printing.PageSetup();
            this.chartForSimpleFormXY.Printing.PrintPreview();
            this.chartForSimpleFormXY.Printing.Print(false);
        }

        private void comboBoxDescriptorX_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayXY();
        }

        private void comboBoxDescriptorY_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayXY();
        }

        Series CurrentSeries;
        ChartArea CurrentChartArea = new ChartArea();

        public void DisplayXY()
        {
            if (CompleteScreening == null) return;

            int DescX = this.comboBoxDescriptorX.SelectedIndex;
            int DescY = this.comboBoxDescriptorY.SelectedIndex;

            if (DescX < 0) DescX = 0;
            if (DescY < 0) DescY = 0;

            CurrentSeries = new Series("ScatterPoints");
            CurrentSeries.ShadowOffset = 1;

            double MinX = double.MaxValue;
            double MinY = double.MaxValue;
            double MaxX = double.MinValue;
            double MaxY = double.MinValue;

            double TempX, TempY;
            int Idx = 0;

            cExtendPlateList ListPlate = new cExtendPlateList();
            if (!IsFullScreen)
                ListPlate.Add(CompleteScreening.GetCurrentDisplayPlate());
            else
                ListPlate = CompleteScreening.ListPlatesActive;

            for (int i = 0; i < ListPlate.Count; i++)
            {
                cPlate CurrentPlate = ListPlate[i];
                for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                    for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                    {
                        cWell TmpWell = CurrentPlate.GetWell(IdxValue, IdxValue0, true);
                        if (TmpWell != null)
                        {

                            TempX = TmpWell.ListDescriptors[DescX].GetValue();
                            if (TempX < MinX) MinX = TempX;
                            if (TempX > MaxX) MaxX = TempX;


                            TempY = TmpWell.ListDescriptors[DescY].GetValue();
                            if (TempY < MinY) MinY = TempY;
                            if (TempY > MaxY) MaxY = TempY;


                            CurrentSeries.Points.AddXY(TempX, TempY);
                            CurrentSeries.Points[Idx].Color = TmpWell.GetColor();

                            if(IsFullScreen)
                                CurrentSeries.Points[Idx].ToolTip = TmpWell.AssociatedPlate.Name + "\n" + TmpWell.GetPosX() + "x" + TmpWell.GetPosY() + " :" + TmpWell.Name;
                            else
                            CurrentSeries.Points[Idx].ToolTip = TmpWell.GetPosX() + "x" + TmpWell.GetPosY() + " :" + TmpWell.Name;

                            CurrentSeries.Points[Idx].MarkerStyle = MarkerStyle.Circle;
                            CurrentSeries.Points[Idx].MarkerSize = 8;
                            Idx++;
                        }
                    }
            }
            CurrentChartArea.CursorX.IsUserSelectionEnabled = true;
            CurrentChartArea.BorderColor = Color.Black;
            this.chartForSimpleFormXY.ChartAreas.Clear();
            this.chartForSimpleFormXY.ChartAreas.Add(CurrentChartArea);

            this.chartForSimpleFormXY.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackColor = Color.FromArgb(164, 164, 164);


            CurrentChartArea.Axes[0].Title = CompleteScreening.ListDescriptors[DescX].GetName();
            CurrentChartArea.Axes[0].Minimum = MinX;
            CurrentChartArea.Axes[0].Maximum = MaxX;

            CurrentChartArea.Axes[1].Title = CompleteScreening.ListDescriptors[DescY].GetName();
            CurrentChartArea.Axes[1].Minimum = MinY;
            CurrentChartArea.Axes[1].Maximum = MaxY;

            CurrentChartArea.AxisX.LabelStyle.Format = "N2";
            CurrentChartArea.AxisY.LabelStyle.Format = "N2";
            CurrentSeries.ChartType = SeriesChartType.Point;

            this.chartForSimpleFormXY.Series.Clear();
            this.chartForSimpleFormXY.Series.Add(CurrentSeries);

            this.Text = "Scatter Point / " + Idx+ " points";
            this.chartForSimpleFormXY.Update();

        }
    }
}
