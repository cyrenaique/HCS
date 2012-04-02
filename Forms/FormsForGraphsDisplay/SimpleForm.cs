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
using HCSAnalyzer.Forms.FormsForGraphsDisplay;

namespace LibPlateAnalysis
{
    public partial class SimpleForm : Form
    {
        FormForMaxMinRequest RequestWindow = new FormForMaxMinRequest();


        public SimpleForm()
        {
            InitializeComponent();
        }



        private void saveGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog CurrSaveFileDialog = new SaveFileDialog();
            CurrSaveFileDialog.Filter = "PNG files (*.png)|*.png";
            System.Windows.Forms.DialogResult Res = CurrSaveFileDialog.ShowDialog();
            if (Res != System.Windows.Forms.DialogResult.OK) return;

            string CurrentPath = CurrSaveFileDialog.FileName;
            this.chartForSimpleForm.SaveImage(CurrentPath, ChartImageFormat.Png);

            MessageBox.Show("File saved !");
        }

        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            this.chartForSimpleForm.SaveImage(ms, ChartImageFormat.Bmp);
            Bitmap bm = new Bitmap(ms);
            Clipboard.SetImage(bm);
        }

        public string GetValues()
        {
            StringBuilder sb = new StringBuilder();

            if (this.chartForSimpleForm.Titles.Count != 0)
                sb.Append(this.chartForSimpleForm.Titles[0].Text + "\n");
            sb.Append("Axis X: " + this.chartForSimpleForm.ChartAreas[0].AxisX.Title + "\n");
            sb.Append("Axis Y: " + this.chartForSimpleForm.ChartAreas[0].AxisY.Title + "\n");


            if (this.chartForSimpleForm.Series[0].Name.ToString() == "Matrix")
            {
                sb.Append("\t");

                for (int X = 0; X < this.chartForSimpleForm.ChartAreas[0].AxisX.CustomLabels.Count; X++)
                    sb.Append(this.chartForSimpleForm.ChartAreas[0].AxisX.CustomLabels[X].Text + "\t");

                sb.Append("\n");
                for (int Y = 0; Y < this.chartForSimpleForm.ChartAreas[0].AxisY.CustomLabels.Count; Y++)
                {
                    sb.Append(this.chartForSimpleForm.ChartAreas[0].AxisY.CustomLabels[Y].Text + "\t");

                    for (int X = 0; X < this.chartForSimpleForm.ChartAreas[0].AxisX.CustomLabels.Count; X++)
                        sb.Append(this.chartForSimpleForm.Series[0].Points[X + Y * this.chartForSimpleForm.ChartAreas[0].AxisX.CustomLabels.Count].ToolTip + "\t");

                    sb.Append("\n");
                }
            }
            else
            {
                //sb.Append("\t");
                for (int Serie = 0; Serie < this.chartForSimpleForm.Series.Count; Serie++)
                {
                    sb.Append(this.chartForSimpleForm.Series[Serie].Label + " X values\t");
                    if (this.chartForSimpleForm.ChartAreas[0].AxisX.CustomLabels.Count > 0)
                    {
                        for (int X = 0; X < this.chartForSimpleForm.ChartAreas[0].AxisX.CustomLabels.Count; X++)
                            sb.Append(this.chartForSimpleForm.ChartAreas[0].AxisX.CustomLabels[X].Text.Replace("\n", " ") + "\t");
                    }
                    else
                    {
                        for (int i = 0; i < this.chartForSimpleForm.Series[Serie].Points.Count; i++)
                            sb.Append(String.Format("{0}\t", this.chartForSimpleForm.Series[Serie].Points[i].XValue));
                    }
                    sb.Append("\n");
                    sb.Append(this.chartForSimpleForm.Series[Serie].Label + " Y Values\t");
                    for (int i = 0; i < this.chartForSimpleForm.Series[Serie].Points.Count; i++)
                        sb.Append(String.Format("{0}\t", this.chartForSimpleForm.Series[Serie].Points[i].YValues[0]));
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
            this.chartForSimpleForm.Printing.PageSetup();
            this.chartForSimpleForm.Printing.PrintPreview();
            this.chartForSimpleForm.Printing.Print(false);
        }

        private void DisplayParamaters_Click(object sender, EventArgs e)
        {
            if (this.chartForSimpleForm.Series[0].Points.Count >= 1)
                RequestWindow.numericUpDownMarkerSize.Value = (decimal)this.chartForSimpleForm.Series[0].Points[0].MarkerSize;


            RequestWindow.numericUpDownMax.Value = (decimal)this.chartForSimpleForm.ChartAreas[0].AxisY.Maximum;
            RequestWindow.numericUpDownMin.Value = (decimal)this.chartForSimpleForm.ChartAreas[0].AxisY.Minimum;

            if (RequestWindow.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            if (RequestWindow.numericUpDownMax.Value <= RequestWindow.numericUpDownMin.Value) return;

            this.chartForSimpleForm.ChartAreas[0].AxisY.Maximum = (double)RequestWindow.numericUpDownMax.Value;
            this.chartForSimpleForm.ChartAreas[0].AxisY.Minimum = (double)RequestWindow.numericUpDownMin.Value;
            foreach (DataPoint Pt in this.chartForSimpleForm.Series[0].Points)
            {
                Pt.MarkerSize = (int)RequestWindow.numericUpDownMarkerSize.Value;

            }
        }


    }
}
