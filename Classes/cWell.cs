using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Windows.Forms;
using HCSAnalyzer;
using HCSAnalyzer.jp.genome.soap;
using HCSAnalyzer.Forms;
using HCSAnalyzer.Classes;
using weka.core;

namespace LibPlateAnalysis
{
    public class cWell
    {
        private int PosX = -1;
        private int PosY = -1;
        public List<cDescriptor> ListDescriptors;
        public List<cDescriptor> ListPlateBasedDescriptors;

        private PlateChart AssociatedChart;
        public string StateForClassif = "Class2";

        private int CurrentDescriptorToDisplay;
        private int ClassForClassif = 2;
        cScreening Parent;
        public cPlate AssociatedPlate;
        private Color CurrentColor;
        // private string CurrentSelectedPathway = "";
        public double LocusID = -1;

        public string Name = "";
        public string Info = "";
        public double Concentration = 0;

        FormForPathway ListP = new FormForPathway();



        // protected Size SizeHisto = new System.Drawing.Size(10, 5);


        public cWell(cWell NewWell)
        {
            this.PosX = NewWell.PosX;
            this.PosY = NewWell.PosY;
            this.ListDescriptors = NewWell.ListDescriptors;
            this.Parent = NewWell.Parent;
            this.AssociatedChart = NewWell.AssociatedChart;
            this.AssociatedPlate = NewWell.AssociatedPlate;
            this.CurrentColor = this.Parent.GlobalInfo.GetColor(ClassForClassif);
        }

        public cWell(cDescriptor Desc, int Col, int Row, cScreening screenParent, cPlate CurrentPlate)
        {
            this.Parent = screenParent;
            this.AssociatedPlate = CurrentPlate;
            this.ListDescriptors = new List<cDescriptor>();

            this.ListDescriptors.Add(Desc);

            this.PosX = Col;
            this.PosY = Row;

            this.CurrentColor = this.Parent.GlobalInfo.GetColor(ClassForClassif);
        }

        public cWell(List<cDescriptor> ListDesc, int Col, int Row, cScreening screenParent, cPlate CurrentPlate)
        {
            this.Parent = screenParent;
            this.AssociatedPlate = CurrentPlate;
            this.ListDescriptors = new List<cDescriptor>();

            this.ListDescriptors = ListDesc;

            this.PosX = Col;
            this.PosY = Row;

            this.CurrentColor = this.Parent.GlobalInfo.GetColor(ClassForClassif);
        }

        public cWell(string FileName, cScreening screenParent, cPlate CurrentPlate)
        {
            this.Parent = screenParent;
            this.AssociatedPlate = CurrentPlate;

            StreamReader sr = new StreamReader(FileName);
            int Idx;
            string NewLine;
            string TmpLine;
            string line;

            // we have to build the descriptor list
            if (screenParent.ListDescriptors.Count == 0)
            {
                Idx = FileName.LastIndexOf("\\");
                NewLine = FileName.Remove(0, Idx + 1);
                TmpLine = NewLine;

                Idx = TmpLine.IndexOf("x");
                NewLine = TmpLine.Remove(Idx);

                if (!int.TryParse(NewLine, out this.PosX))
                {
                    MessageBox.Show("Error in load the current file.\n", "Loading error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sr.Close();
                    return;
                }




                NewLine = TmpLine.Remove(0, Idx + 1);
                Idx = NewLine.IndexOf(".");
                TmpLine = NewLine.Remove(Idx);

                this.PosY = Convert.ToInt16(TmpLine);

                line = sr.ReadLine();
                while (line != null)
                {
                    if (line != null)
                    {
                        Idx = line.IndexOf("\t");
                        string DescName = line.Remove(Idx);

                        List<double> readData = new List<double>();

                        NewLine = line.Remove(0, Idx + 1);
                        line = NewLine;

                        Idx = line.IndexOf("\t");
                        int NumValue = 0;
                        while (Idx > 0)
                        {
                            string DescValue = line.Remove(Idx);
                            double CurrentValue = Convert.ToDouble(DescValue);

                            readData.Add(CurrentValue);
                            NewLine = line.Remove(0, Idx + 1);
                            line = NewLine;
                            Idx = line.IndexOf("\t");
                            NumValue++;
                        }
                        if (line.Length > 0)
                        {
                            double Value = Convert.ToDouble(line);
                            readData.Add(Value);
                        }
                        // first check if the descriptor exist
                        screenParent.ListDescriptors.AddNew(new cDescriptorsType(DescName, true, NumValue));

                    }
                    line = sr.ReadLine();
                }
                sr.Close();
            }

            this.ListDescriptors = new List<cDescriptor>();
            sr = new StreamReader(FileName);

            Idx = FileName.LastIndexOf("\\");
            NewLine = FileName.Remove(0, Idx + 1);
            TmpLine = NewLine;

            Idx = TmpLine.IndexOf("x");
            NewLine = TmpLine.Remove(Idx);
            this.PosX = Convert.ToInt16(NewLine);

            NewLine = TmpLine.Remove(0, Idx + 1);
            Idx = NewLine.IndexOf(".");
            TmpLine = NewLine.Remove(Idx);

            this.PosY = Convert.ToInt16(TmpLine);

            line = sr.ReadLine();
            int IDxLine = 0;
            while (line != null)
            {
                if (line != null)
                {
                    Idx = line.IndexOf("\t");
                    string DescName = line.Remove(Idx);
                    List<double> readData = new List<double>();

                    NewLine = line.Remove(0, Idx + 1);
                    line = NewLine;

                    Idx = line.IndexOf("\t");

                    while (Idx > 0)
                    {
                        string DescValue = line.Remove(Idx);
                        double CurrentValue = Convert.ToDouble(DescValue);

                        readData.Add(CurrentValue);
                        NewLine = line.Remove(0, Idx + 1);
                        line = NewLine;
                        Idx = line.IndexOf("\t");
                    }
                    if (line.Length > 0)
                    {
                        double Value = Convert.ToDouble(line);
                        readData.Add(Value);
                    }
                    cDescriptor CurrentDesc = new cDescriptor(readData.ToArray(), 0, screenParent.ListDescriptors[IDxLine].GetBinNumber() - 1, screenParent.ListDescriptors[IDxLine], this.Parent/* DescName*/);
                    this.ListDescriptors.Add(CurrentDesc);
                }
                line = sr.ReadLine();
                IDxLine++;
            }
            sr.Close();
            this.CurrentColor = this.Parent.GlobalInfo.GetColor(ClassForClassif);
            return;
        }

        public int GetClass()
        {
            return ClassForClassif;
        }
        
        public Color GetColor()
        {
            return this.CurrentColor;
        }

        public List<double> GetAverageValuesList()
        {
            List<double> ValuesToReturn = new List<double>();
            for (int i = 0; i < ListDescriptors.Count; i++)
                ValuesToReturn.Add(ListDescriptors[i].GetValue());
            return ValuesToReturn;
        }

        public void SetClass(int Class)
        {
            ClassForClassif = Class;
            if (Class == 0)
                StateForClassif = "Positive (0)";
            else if (Class == 1)
                StateForClassif = "Negative (1)";
            else
                StateForClassif = "Class" + Class;

            CurrentColor = Parent.GlobalInfo.GetColor(Class);
            if (AssociatedChart == null) return;
            AssociatedChart.BackColor = CurrentColor;
            AssociatedChart.Update();
        }

        public void SetAsNoneSelected()
        {
            ClassForClassif = -1;
            StateForClassif = "Unselected (-1)";
            CurrentColor = Parent.GlobalInfo.panelForPlate.BackColor;
            if (AssociatedChart == null) return;
            AssociatedChart.BackColor = CurrentColor;
            AssociatedChart.Update();
        }

        public int GetPosX()
        {
            return this.PosX;
        }

        public int GetPosY()
        {
            return this.PosY;
        }

        public PlateChart BuildChartForClass()
        {

            if (AssociatedChart != null) AssociatedChart.Dispose();
            AssociatedChart = new PlateChart();
            Series CurrentSeries = new Series("ChartSeries" + PosX + "x" + PosY);
            ChartArea CurrentChartArea = new ChartArea("ChartArea" + PosX + "x" + PosY);

            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
            CurrentChartArea.Axes[0].LabelStyle.Enabled = false;

            CurrentChartArea.Axes[1].LabelStyle.Enabled = false;
            CurrentChartArea.Axes[1].MajorGrid.Enabled = false;
            CurrentChartArea.Axes[0].Enabled = AxisEnabled.False;
            CurrentChartArea.Axes[1].Enabled = AxisEnabled.False;

            CurrentChartArea.BackColor = CurrentColor; //Color.FromArgb(LUT[0][ConvertedValue], LUT[1][ConvertedValue], LUT[2][ConvertedValue]);
            AssociatedChart.ChartAreas.Add(CurrentChartArea);
            //AssociatedChart.Location = new System.Drawing.Point((PosX - 1) * (Parent.SizeHistoWidth + Parent.GutterSize), (PosY - 1) * (Parent.SizeHistoHeight + Parent.GutterSize));
            //AssociatedChart.Series.Add(CurrentSeries);

            int GutterSize = (int)Parent.GlobalInfo.OptionsWindow.numericUpDownGutter.Value;

            AssociatedChart.Location = new System.Drawing.Point((int)((PosX - 1) * (Parent.GlobalInfo.SizeHistoWidth + GutterSize) + Parent.GlobalInfo.ShiftX), (int)((PosY - 1) * (Parent.GlobalInfo.SizeHistoHeight + GutterSize) + Parent.GlobalInfo.ShiftY));
            AssociatedChart.Series.Add(CurrentSeries);

            AssociatedChart.BackColor = CurrentColor;
            AssociatedChart.Width = (int)Parent.GlobalInfo.SizeHistoWidth;
            AssociatedChart.Height = (int)Parent.GlobalInfo.SizeHistoHeight;

            if (Parent.GlobalInfo.OptionsWindow.checkBoxDisplayWellInformation.Checked)
            {
                Title MainLegend = new Title();

                if (Parent.GlobalInfo.OptionsWindow.radioButtonWellInfoName.Checked)
                    MainLegend.Text = Name;
                if (Parent.GlobalInfo.OptionsWindow.radioButtonWellInfoInfo.Checked)
                    MainLegend.Text = Info;
                if (Parent.GlobalInfo.OptionsWindow.radioButtonWellInfoLocusID.Checked)
                    MainLegend.Text = ((int)(LocusID)).ToString();
                if (Parent.GlobalInfo.OptionsWindow.radioButtonWellInfoConcentration.Checked)
                {
                    if(Concentration>=0)
                    MainLegend.Text = Concentration.ToString("e4");
                }

                MainLegend.Docking = Docking.Bottom;
                MainLegend.Font = new System.Drawing.Font("Arial", Parent.GlobalInfo.SizeHistoWidth / 10 + 1, FontStyle.Regular);
                MainLegend.BackColor = MainLegend.BackImageTransparentColor;

                AssociatedChart.Titles.Add(MainLegend);
            }
            AssociatedChart.Update();
            AssociatedChart.Show();
            AssociatedChart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AssociatedChart_MouseClick);
            AssociatedChart.GetToolTipText += new System.EventHandler<ToolTipEventArgs>(this.AssociatedChart_GetToolTipText);
            return this.AssociatedChart;
        }

        public PlateChart BuildChart(int IdxDescriptor, double[] MinMax)
        {

            if (Parent.GlobalInfo.IsDisplayClassOnly) return BuildChartForClass();

            int borderSize = 8;
            int GutterSize = (int)Parent.GlobalInfo.OptionsWindow.numericUpDownGutter.Value;

            if (AssociatedChart != null) AssociatedChart.Dispose();
            if (IdxDescriptor >= ListDescriptors.Count) return null;
            CurrentDescriptorToDisplay = IdxDescriptor;
            AssociatedChart = new PlateChart();
            Series CurrentSeries = new Series("ChartSeries" + PosX + "x" + PosY);
            ChartArea CurrentChartArea = new ChartArea("ChartArea" + PosX + "x" + PosY);

            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
            CurrentChartArea.Axes[0].LabelStyle.Enabled = false;

            if ((ListDescriptors[IdxDescriptor].GetAssociatedType().GetBinNumber() == 1) || (Parent.GlobalInfo.OptionsWindow.radioButtonDisplayAverage.Checked))
            {
                CurrentChartArea.Axes[1].LabelStyle.Enabled = false;
                CurrentChartArea.Axes[1].MajorGrid.Enabled = false;
                CurrentChartArea.Axes[0].Enabled = AxisEnabled.False;
                CurrentChartArea.Axes[1].Enabled = AxisEnabled.False;

                int ConvertedValue;

                byte[][] LUT = Parent.GlobalInfo.LUT_JET;

                if (MinMax[0] == MinMax[1])
                    ConvertedValue = 0;
                else
                    ConvertedValue = (int)(((ListDescriptors[IdxDescriptor].GetValue() - MinMax[0]) * (LUT[0].Length - 1)) / (MinMax[1] - MinMax[0]));
                if ((ConvertedValue >= 0) && (ConvertedValue < LUT[0].Length))
                    CurrentChartArea.BackColor = Color.FromArgb(LUT[0][ConvertedValue], LUT[1][ConvertedValue], LUT[2][ConvertedValue]);
                AssociatedChart.ChartAreas.Add(CurrentChartArea);
            }
            else
            {
                CurrentSeries.ChartType = SeriesChartType.Line;
                for (int IdxValue = 0; IdxValue < ListDescriptors[IdxDescriptor].GetAssociatedType().GetBinNumber(); IdxValue++)
                    CurrentSeries.Points.Add(ListDescriptors[IdxDescriptor].Getvalue(IdxValue));

                CurrentChartArea.Axes[1].MajorGrid.Enabled = false;
                CurrentChartArea.Axes[1].MajorGrid.LineColor = Color.FromArgb(127, 127, 127);
                CurrentChartArea.Axes[1].LineColor = Color.FromArgb(127, 127, 127);
                CurrentChartArea.Axes[1].MajorTickMark.LineColor = Color.FromArgb(127, 127, 127);
                CurrentChartArea.Axes[1].LabelStyle.Enabled = false;
                CurrentChartArea.Axes[0].LineColor = Color.FromArgb(127, 127, 127);
                CurrentChartArea.Axes[0].MajorTickMark.LineColor = Color.FromArgb(127, 127, 127);
                CurrentSeries.Color = Color.White;
                CurrentSeries.BorderWidth = 1;
                CurrentChartArea.BackColor = Color.FromArgb(16, 37, 63);

                CurrentChartArea.BorderWidth = borderSize;
                AssociatedChart.ChartAreas.Add(CurrentChartArea);
            }


            AssociatedChart.Location = new System.Drawing.Point((int)((PosX - 1) * (Parent.GlobalInfo.SizeHistoWidth + GutterSize) + Parent.GlobalInfo.ShiftX), (int)((PosY - 1) * (Parent.GlobalInfo.SizeHistoHeight + GutterSize) + Parent.GlobalInfo.ShiftY));
            AssociatedChart.Series.Add(CurrentSeries);

            AssociatedChart.BackColor = CurrentColor;
            AssociatedChart.Width = (int)Parent.GlobalInfo.SizeHistoWidth;
            AssociatedChart.Height = (int)Parent.GlobalInfo.SizeHistoHeight;

            if (Parent.GlobalInfo.OptionsWindow.checkBoxDisplayWellInformation.Checked)
            {
                Title MainLegend = new Title();

                if (Parent.GlobalInfo.OptionsWindow.radioButtonWellInfoName.Checked)
                    MainLegend.Text = Name;
                if (Parent.GlobalInfo.OptionsWindow.radioButtonWellInfoInfo.Checked)
                    MainLegend.Text = Info;
                if (Parent.GlobalInfo.OptionsWindow.radioButtonWellInfoLocusID.Checked)
                    MainLegend.Text = ((int)(LocusID)).ToString();
                if (Parent.GlobalInfo.OptionsWindow.radioButtonWellInfoConcentration.Checked)
                    if(Concentration>=0) MainLegend.Text = Concentration.ToString("e4");

                MainLegend.Docking = Docking.Bottom;
                MainLegend.Font = new System.Drawing.Font("Arial", Parent.GlobalInfo.SizeHistoWidth / 10 + 1, FontStyle.Regular);
                MainLegend.BackColor = MainLegend.BackImageTransparentColor;

                AssociatedChart.Titles.Add(MainLegend);
            }
            AssociatedChart.Update();
            AssociatedChart.Show();
            AssociatedChart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AssociatedChart_MouseClick);
            AssociatedChart.GetToolTipText += new System.EventHandler<ToolTipEventArgs>(this.AssociatedChart_GetToolTipText);
            return this.AssociatedChart;
        }

        private void AssociatedChart_GetToolTipText(object sender, System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs e)
        {
            byte[] strArray = new byte[1];
            strArray[0] = (byte)(this.PosY + 64);

            string Chara = Encoding.UTF7.GetString(strArray);
            Chara += this.PosX + ": " + Name + "\n";
            for (int i = 0; i < Parent.ListDescriptors.Count; i++)
            {
                if (Parent.ListDescriptors[i].IsActive() == false) continue;
                if(i==Parent.ListDescriptors.CurrentSelectedDescriptor)
                    Chara += "\t-> " + Parent.ListDescriptors[i].GetName() + ": " + string.Format("{0:0.######}", ListDescriptors[i].GetValue()) + "\n";
                else
                    Chara += Parent.ListDescriptors[i].GetName() + ": " + string.Format("{0:0.######}", ListDescriptors[i].GetValue()) + "\n";
            }
            Chara += this.StateForClassif;
            e.Text = Chara;
        }

        private void AssociatedChart_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks != 1) return;

            if (e.Button == MouseButtons.Left)
            {
                int ClassSelected = Parent.GetSelectionType();
                if (ClassSelected == -2) return;

                if (!Parent.IsSelectionApplyToAllPlates)
                {
                    if (ClassSelected == -1)
                    {
                        SetAsNoneSelected();
                        return;
                    }
                    else
                        SetClass(ClassSelected);

                    int[] a = Parent.GetCurrentDisplayPlate().UpdateNumberOfClass();
                }
                else
                {
                    cWell TempWell;
                    int NumberOfPlates = Parent.ListPlatesActive.Count;

                    for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
                    {
                        cPlate CurrentPlateToProcess = Parent.ListPlatesActive.GetPlate(PlateIdx);
                        TempWell = CurrentPlateToProcess.GetWell(this.PosX - 1, this.PosY - 1, false);
                        if (TempWell == null) continue;
                        if (ClassSelected == -1)
                            TempWell.SetAsNoneSelected();
                        else
                            TempWell.SetClass(ClassSelected);

                        CurrentPlateToProcess.UpdateNumberOfClass();
                    }
                }

                if (ClassSelected == -1)
                {
                    Parent.GetCurrentDisplayPlate().UpDataMinMax();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                string TextFor3D2D;
                if (this.AssociatedPlate.ParentScreening.GlobalInfo.Is3DVisu())
                    TextFor3D2D = "Turn Off 3D vizualization";
                else
                    TextFor3D2D = "Turn On 3D vizualization";

                ToolStripMenuItem ToolStripMenuItem_SwitchVizuMode = new ToolStripMenuItem(TextFor3D2D);
                ToolStripMenuItem ToolStripMenuItem_Info = new ToolStripMenuItem("Info");
                ToolStripMenuItem ToolStripMenuItem_Histo = new ToolStripMenuItem("Histogram");
                ToolStripSeparator ToolStripSep = new ToolStripSeparator();

                ToolStripMenuItem ToolStripMenuItem_Kegg = new ToolStripMenuItem("Kegg");

                ToolStripSeparator ToolStripSep1 = new ToolStripSeparator();
                ToolStripMenuItem ToolStripMenuItem_Copy = new ToolStripMenuItem("Copy Visu.");

                contextMenuStrip.Items.AddRange(new ToolStripItem[] { ToolStripMenuItem_SwitchVizuMode, ToolStripMenuItem_Info, ToolStripMenuItem_Histo, ToolStripSep, ToolStripMenuItem_Kegg, ToolStripSep1, ToolStripMenuItem_Copy });

                //ToolStripSeparator SepratorStrip = new ToolStripSeparator();
                contextMenuStrip.Show(Control.MousePosition);

                ToolStripMenuItem_SwitchVizuMode.Click += new System.EventHandler(this.SwitchVizuMode);
                ToolStripMenuItem_Info.Click += new System.EventHandler(this.DisplayInfo);
                ToolStripMenuItem_Histo.Click += new System.EventHandler(this.DisplayHisto);
                ToolStripMenuItem_Kegg.Click += new System.EventHandler(this.DisplayPathways);
                ToolStripMenuItem_Copy.Click += new System.EventHandler(this.CopyVisu);
            }

        }

        private void SwitchVizuMode(object sender, EventArgs e)
        {
            this.Parent.GlobalInfo.SwitchVisuMode();



        }

        private void DisplayHisto(object sender, EventArgs e)
        {

            if ((Parent.ListDescriptors == null) || (Parent.ListDescriptors.Count == 0)) return;

            cExtendedList Pos = new cExtendedList();


            cWell TempWell;

            int NumberOfPlates = Parent.GlobalInfo.PlateListWindow.listBoxPlateNameToProcess.Items.Count;

            // loop on all the plate
            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = Parent.ListPlatesActive.GetPlate((string)Parent.GlobalInfo.PlateListWindow.listBoxPlateNameToProcess.Items[PlateIdx]);

                for (int row = 0; row < Parent.Rows; row++)
                    for (int col = 0; col < Parent.Columns; col++)
                    {
                        TempWell = CurrentPlateToProcess.GetWell(col, row, false);
                        if (TempWell == null) continue;
                        else
                        {
                            if (TempWell.GetClass() == this.ClassForClassif)
                                Pos.Add(TempWell.ListDescriptors[Parent.ListDescriptors.CurrentSelectedDescriptor].GetValue());
                        }
                    }
            }


            if (Pos.Count == 0)
            {
                MessageBox.Show("No well of class " + Parent.SelectedClass + " selected !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<double[]> HistoPos = Pos.CreateHistogram((int)Parent.GlobalInfo.OptionsWindow.numericUpDownHistoBin.Value);
            if (HistoPos == null) return;
            SimpleForm NewWindow = new SimpleForm();

            Series SeriesPos = new Series();
            SeriesPos.ShadowOffset = 1;

            if (HistoPos.Count == 0) return;

            for (int IdxValue = 0; IdxValue < HistoPos[0].Length; IdxValue++)
            {
                SeriesPos.Points.AddXY(HistoPos[0][IdxValue], HistoPos[1][IdxValue]);
                SeriesPos.Points[IdxValue].ToolTip = HistoPos[1][IdxValue].ToString();

                if (this.ClassForClassif == -1)
                    SeriesPos.Points[IdxValue].Color = Color.Black;
                else
                    SeriesPos.Points[IdxValue].Color = Parent.GlobalInfo.GetColor(this.ClassForClassif);
            }

            ChartArea CurrentChartArea = new ChartArea();
            CurrentChartArea.BorderColor = Color.Black;

            NewWindow.chartForSimpleForm.ChartAreas.Add(CurrentChartArea);
            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
            CurrentChartArea.Axes[0].Title = Parent.ListDescriptors[Parent.ListDescriptors.CurrentSelectedDescriptor].GetName();
            CurrentChartArea.Axes[1].Title = "Sum";
            CurrentChartArea.AxisX.LabelStyle.Format = "N2";

            NewWindow.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackGradientStyle = GradientStyle.TopBottom;
            CurrentChartArea.BackColor = Parent.GlobalInfo.OptionsWindow.panel1.BackColor;
            CurrentChartArea.BackSecondaryColor = Color.White;

            SeriesPos.ChartType = SeriesChartType.Column;
            // SeriesPos.Color = Parent.GetColor(1);
            NewWindow.chartForSimpleForm.Series.Add(SeriesPos);



            NewWindow.chartForSimpleForm.ChartAreas[0].CursorX.IsUserEnabled = true;
            NewWindow.chartForSimpleForm.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            NewWindow.chartForSimpleForm.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            NewWindow.chartForSimpleForm.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;


            StripLine AverageLine = new StripLine();
            AverageLine.BackColor = Color.Red;
            AverageLine.IntervalOffset = this.ListDescriptors[Parent.ListDescriptors.CurrentSelectedDescriptor].GetValue();
            AverageLine.StripWidth = 0.0001;
            AverageLine.Text = String.Format("{0:0.###}", this.ListDescriptors[Parent.ListDescriptors.CurrentSelectedDescriptor].GetValue());
            CurrentChartArea.AxisX.StripLines.Add(AverageLine);

            if (Parent.GlobalInfo.OptionsWindow.checkBoxDisplayHistoStats.Checked)
            {
                StripLine NAverageLine = new StripLine();
                NAverageLine.BackColor = Color.Black;
                NAverageLine.IntervalOffset = Pos.Mean();
                NAverageLine.StripWidth = 0.0001;// double.Epsilon;
                CurrentChartArea.AxisX.StripLines.Add(NAverageLine);
                NAverageLine.Text = String.Format("{0:0.###}", NAverageLine.IntervalOffset);

                StripLine StdLine = new StripLine();
                StdLine.BackColor = Color.FromArgb(64, Color.Black);
                double Std = Pos.Std();
                StdLine.IntervalOffset = NAverageLine.IntervalOffset - 0.5 * Std;
                StdLine.StripWidth = Std;
                CurrentChartArea.AxisX.StripLines.Add(StdLine);
                //NAverageLine.StripWidth = 0.01;
            }




            Title CurrentTitle = new Title(this.StateForClassif + " - " + Parent.ListDescriptors[Parent.ListDescriptors.CurrentSelectedDescriptor].GetName() + " histogram.");
            CurrentTitle.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            NewWindow.chartForSimpleForm.Titles.Add(CurrentTitle);

            NewWindow.Text = CurrentTitle.Text;
            NewWindow.Show();
            NewWindow.chartForSimpleForm.Update();
            NewWindow.chartForSimpleForm.Show();
            NewWindow.Controls.AddRange(new System.Windows.Forms.Control[] { NewWindow.chartForSimpleForm });

            return;
        }

        void DisplayPathways(object sender, EventArgs e)
        {
            if (LocusID == -1) return;
            FormForKeggGene KeggWin = new FormForKeggGene();
            KEGG ServKegg = new KEGG();
            string[] intersection_gene_pathways = new string[1];

            intersection_gene_pathways[0] = "hsa:" + LocusID;
            string[] Pathways = ServKegg.get_pathways_by_genes(intersection_gene_pathways);
            if ((Pathways == null) || (Pathways.Length == 0))
            {
                MessageBox.Show("No pathway founded !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string GenInfo = ServKegg.bget(intersection_gene_pathways[0]);



            // FormForPathway PathwaysGenes = new FormForPathway();

            KeggWin.richTextBox.Text = GenInfo;
            KeggWin.Text = "Gene Infos";
            //PathwaysGenes.Show();
            ListP = new FormForPathway();

            ListP.listBoxPathways.DataSource = Pathways;

            ListP.Text = this.Name;
            ListP.Show();
            //foreach (string item in Pathways)
            //{
            //     string PathwayInfo = ServKegg.bget(item);
            //     FormPathwaysGenes PathwaysGenes = new FormPathwaysGenes();

            //     PathwaysGenes.richTextBox1.Text = PathwayInfo;
            //     PathwaysGenes.Text = "Pathways Infos";
            //     PathwaysGenes.Show();

            //}



            string[] fg_list = { "black" };
            string[] bg_list = { "orange" };
            // string[] intersection_gene_pathways = new string[1];

            // intersection_gene_pathways[0] = "hsa:" + LocusID;


            string pathway_map_html = "";
            //  KEGG ServKegg = new KEGG();

            pathway_map_html = ServKegg.get_html_of_colored_pathway_by_objects((string)(ListP.listBoxPathways.SelectedItem), intersection_gene_pathways, fg_list, bg_list);

            // FormForKegg KeggWin = new FormForKegg();
            if (pathway_map_html.Length == 0) return;

            //
            //KeggWin.Show();
            ListP.listBoxPathways.MouseDoubleClick += new MouseEventHandler(listBox1_MouseDoubleClick);
            KeggWin.webBrowser.Navigate(pathway_map_html);

            KeggWin.Show();

        }

        void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string[] fg_list = { "black" };
            string[] bg_list = { "orange" };
            string[] intersection_gene_pathways = new string[1];

            intersection_gene_pathways[0] = "hsa:" + LocusID;

            string pathway_map_html = "";
            KEGG ServKegg = new KEGG();
            foreach (string item in ListP.listBoxPathways.SelectedItems)
            {
                pathway_map_html = ServKegg.get_html_of_colored_pathway_by_objects(item, intersection_gene_pathways, fg_list, bg_list);
            }

            string GenInfo = ServKegg.bget((string)ListP.listBoxPathways.SelectedItem);
            string[] Genes = GenInfo.Split(new char[] { '\n' });
            string Res = "";
            foreach (string item in Genes)
            {
                string[] fre = item.Split(' ');
                string[] STRsection = fre[0].Split('_');

                if (STRsection[0] != "NAME") continue;

                for (int i = 1; i < fre.Length; i++)
                {
                    if (fre[i] == "") continue;
                    Res += fre[i] + " ";
                }
            }

            FormForKegg KeggWin = new FormForKegg();

            if (pathway_map_html.Length == 0) return;

            KeggWin.Text = Res;
            KeggWin.Show();

            KeggWin.webBrowser.Navigate(pathway_map_html);

        }

        public Chart GetChart()
        {
            if (ListDescriptors[CurrentDescriptorToDisplay].GetAssociatedType().GetBinNumber() == 1) return null;

            Series CurrentSeries = new Series("ChartSeries" + PosX + "x" + PosY);
            //CurrentSeries.ShadowOffset = 2;

            for (int IdxValue = 0; IdxValue < ListDescriptors[CurrentDescriptorToDisplay].GetAssociatedType().GetBinNumber(); IdxValue++)
                CurrentSeries.Points.Add(ListDescriptors[CurrentDescriptorToDisplay].Getvalue(IdxValue));

            ChartArea CurrentChartArea = new ChartArea("ChartArea" + PosX + "x" + PosY);
            CurrentChartArea.BorderColor = Color.White;

            Chart ChartToReturn = new Chart();
            ChartToReturn.ChartAreas.Add(CurrentChartArea);
           // ChartToReturn.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackColor = Color.White;

            CurrentChartArea.Axes[1].LabelStyle.Enabled = false;
            CurrentChartArea.Axes[1].MajorGrid.Enabled = false;
            CurrentChartArea.Axes[0].Enabled = AxisEnabled.False;
            CurrentChartArea.Axes[1].Enabled = AxisEnabled.False;


            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
          //  CurrentChartArea.Axes[0].Title = ListDescriptors[CurrentDescriptorToDisplay].GetName();
            CurrentSeries.ChartType = SeriesChartType.Line;
            CurrentSeries.Color = Color.Black;
           // CurrentSeries.BorderWidth = 3;
            CurrentSeries.ChartArea = "ChartArea" + PosX + "x" + PosY;

            CurrentSeries.Name = "Series" + PosX + "x" + PosY;
            ChartToReturn.Series.Add(CurrentSeries);

            Title CurrentTitle = new Title(PosX + "x" + PosY);
           // ChartToReturn.Titles.Add(CurrentTitle);

            ChartToReturn.Width = 100;
            ChartToReturn.Height = 48;

            ChartToReturn.Update();
          //  ChartToReturn.Show();

            return ChartToReturn;

        }

        public void DisplayInfoWindow()
        {
            FormForWellInformation NewWindow = new FormForWellInformation();

            NewWindow.textBoxName.Text = Name;
            NewWindow.textBoxInfo.Text = Info;

            if (Concentration >= 0)
                NewWindow.textBoxConcentration.Text = Concentration.ToString("e4");

            if (LocusID != -1)
                NewWindow.textBoxLocusID.Text = ((int)(LocusID)).ToString();

            Series CurrentSeries = new Series("ChartSeries" + PosX + "x" + PosY);
            CurrentSeries.ShadowOffset = 2;

            for (int IdxValue = 0; IdxValue < ListDescriptors[CurrentDescriptorToDisplay].GetAssociatedType().GetBinNumber(); IdxValue++)
                CurrentSeries.Points.Add(ListDescriptors[CurrentDescriptorToDisplay].Getvalue(IdxValue));

            ChartArea CurrentChartArea = new ChartArea("ChartArea" + PosX + "x" + PosY);
            CurrentChartArea.BorderColor = Color.Black;

            NewWindow.chartForFormWell.ChartAreas.Add(CurrentChartArea);
            NewWindow.chartForFormWell.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackColor = Color.FromArgb(64, 64, 64);

            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
            CurrentChartArea.Axes[0].Title = ListDescriptors[CurrentDescriptorToDisplay].GetName();
            CurrentSeries.ChartType = SeriesChartType.Line;
            CurrentSeries.Color = Color.White;
            CurrentSeries.BorderWidth = 3;
            CurrentSeries.ChartArea = "ChartArea" + PosX + "x" + PosY;

            CurrentSeries.Name = "Series" + PosX + "x" + PosY;
            NewWindow.chartForFormWell.Series.Add(CurrentSeries);

            Title CurrentTitle = new Title(PosX + "x" + PosY);
            NewWindow.chartForFormWell.Titles.Add(CurrentTitle);

            NewWindow.Text = PosX + "x" + PosY + " / " + StateForClassif;

            if (NewWindow.ShowDialog() == DialogResult.OK)
                this.Info = NewWindow.textBoxInfo.Text;

            NewWindow.chartForFormWell.Update();
            NewWindow.chartForFormWell.Show();
            return;
        }

        private void DisplayInfo(object sender, EventArgs e)
        {
            DisplayInfoWindow();
        }

        internal void AddDescriptors(List<cDescriptor> LDesc)
        {
            this.ListDescriptors.AddRange(LDesc);
        }

        /// <summary>
        /// copy the plate visualization to the clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyVisu(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();

            Graphics g = Parent.GlobalInfo.panelForPlate.CreateGraphics();
            Bitmap bmp = new Bitmap(Parent.GlobalInfo.panelForPlate.Width, Parent.GlobalInfo.panelForPlate.Height);

            Parent.GlobalInfo.panelForPlate.DrawToBitmap(bmp, new Rectangle(0, 0, Parent.GlobalInfo.panelForPlate.Width, Parent.GlobalInfo.panelForPlate.Height));
            Clipboard.SetImage(bmp);

            return;
        }

        /// <summary>
        /// Create a single instance for WEKA
        /// </summary>
        /// <param name="NClasses">Number of classes</param>
        /// <returns>the weka instances</returns>
        public Instances CreateInstanceForNClasses(cInfoClass InfoClass)
        {

            List<double> AverageList = new List<double>();

            for (int i = 0; i < Parent.ListDescriptors.Count; i++)
                if (Parent.ListDescriptors[i].IsActive()) AverageList.Add(GetAverageValuesList()[i]);

            weka.core.FastVector atts = new FastVector();

            List<string> NameList = Parent.ListDescriptors.GetListNameActives();

            for (int i = 0; i < NameList.Count; i++)
                atts.addElement(new weka.core.Attribute(NameList[i]));

            weka.core.FastVector attVals = new FastVector();
            for (int i = 0; i < InfoClass.NumberOfClass; i++)
                attVals.addElement("Class" + i);

            atts.addElement(new weka.core.Attribute("Class__", attVals));

            Instances data1 = new Instances("SingleInstance", atts, 0);

            double[] newTable = new double[AverageList.Count + 1];
            Array.Copy(AverageList.ToArray(), 0, newTable, 0, AverageList.Count);
            //newTable[AverageList.Count] = 1;

            data1.add(new DenseInstance(1.0, newTable));
            data1.setClassIndex((data1.numAttributes() - 1));
            return data1;
        }


    }
}
