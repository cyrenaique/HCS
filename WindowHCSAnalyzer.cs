#region Version Header
/************************************************************************
 *		$Id: Window.cs, 177+1 2010/10/20 08:28:39 HongKee $
 *		$Description: Plugin Template for IM 3.0 $
 *		$Author: HongKee $
 *		$Date: 2010/10/20 08:28:39 $
 *		$Revision: 177+1 $
 /************************************************************************/
#endregion

using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using LibPlateAnalysis;
using weka.core;
using System.IO;
using HCSAnalyzer.jp.genome.soap;
using HCSAnalyzer.Forms;
using HCSAnalyzer.Controls;
using System.Reflection;
using System.Collections;
using HCSAnalyzer.Classes;
using System.Resources;
using HCSPlugin;
using HCSAnalyzer.Forms.FormsForOptions;
using HCSAnalyzer.Forms.FormsForDRCAnalysis;


//////////////////////////////////////////////////////////////////////////
// If you want to change Menu & Name of plugin
// Go to "Properties->Resources" in Solution Explorer
// Change Menu & Name
//
// You can also use your own Painter & Mouse event handler
// 
//////////////////////////////////////////////////////////////////////////

namespace HCSAnalyzer
{
    public partial class HCSAnalyzer : Form
    {
        Boolean bHaveMouse;
        static cScreening CompleteScreening;
        FormConsole MyConsole;
        PlatesListForm PlateListWindow;
        cGlobalInfo GlobalInfo;

        public HCSAnalyzer()
        {
            InitializeComponent();
            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;

            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.radioButtonDimRedUnsupervised, "Unsupervised feature selection.\nThese approaches use all the active wells as data for the dimensionality reduction.");
            toolTip1.SetToolTip(this.radioButtonDimRedSupervised, "Supervised feature selection.\nThese approaches have learning porcess based on the well classes except for the neutral class.");

            comboBoxMethodForCorrection.SelectedIndex = 0;

            comboBoxClusteringMethod.SelectedIndex = 0;
            comboBoxCLassificationMethod.SelectedIndex = 0;
            comboBoxNeutralClassForClassif.SelectedIndex = 2;
            comboBoxReduceDimSingleClass.SelectedIndex = 0;
            comboBoxReduceDimMultiClass.SelectedIndex = 0;
            comboBoxDimReductionNeutralClass.SelectedIndex = 2;

            comboBoxMethodForNormalization.SelectedIndex = 1;
            comboBoxNormalizationNegativeCtrl.SelectedIndex = 1;
            comboBoxNormalizationPositiveCtrl.SelectedIndex = 0;

            comboBoxRejectionNegativeCtrl.SelectedIndex = 1;
            comboBoxRejectionPositiveCtrl.SelectedIndex = 0;

            comboBoxRejection.SelectedIndex = 0;

            buttonReduceDim.Focus();
            buttonReduceDim.Select();
        }

        private void HCSAnalyzer_Load(object sender, EventArgs e)
        {
            GlobalInfo = new cGlobalInfo(CompleteScreening);
            GlobalInfo.OptionsWindow.Visible = false;
            GlobalInfo.ComboForSelectedDesc = this.comboBoxDescriptorToDisplay;
            GlobalInfo.CheckedListBoxForDescActive = this.checkedListBoxActiveDescriptors;

            //new Kitware.VTK.RenderWindowControl novelRender = new RenderWindowControl

            GlobalInfo.renderWindowControlForVTK = null;//renderWindowControlForVTK;


            MyConsole = new FormConsole();
            MyConsole.Visible = false;

            PlateListWindow = new PlatesListForm();
            GlobalInfo.PlateListWindow = PlateListWindow;

            GlobalInfo.panelForPlate = this.panelForPlate;

            comboBoxClass.SelectedIndex = 1;
        }

        #region Math Tools
        public double std(double[] input)
        {
            double var = 0f, mean = Mean(input);
            foreach (double f in input) var += (f - mean) * (f - mean);
            return Math.Sqrt(var / (double)(input.Length - 1));
        }

        public double Variance(double[] input)
        {
            double var = 0f, mean = Mean(input);
            foreach (double f in input) var += (f - mean) * (f - mean);
            return var / (double)(input.Length - 1);
        }

        public double Mean(double[] input)
        {
            double mean = 0f;
            foreach (double f in input) mean += f;
            return mean / (double)input.Length;
        }

        private double[] CreateGauss(double p, double p_2, int p_3)
        {
            double[] Gauss = new double[p_3];

            for (int x = 0; x < p_3; x++)
            {
                Gauss[x] = Math.Exp(-((x - p) * (x - p)) / (2 * p_2 * p_2));
            }

            return Gauss;
        }

        private List<double[]> CreateHistogram(double[] data, double Bin)
        {
            List<double[]> ToReturn = new List<double[]>();

            //float max = math.Max(data);
            if (data.Length == 0) return ToReturn;
            double Max = data[0];
            double Min = data[0];

            for (int Idx = 1; Idx < data.Length; Idx++)
            {
                if (data[Idx] > Max) Max = data[Idx];
                if (data[Idx] < Min) Min = data[Idx];
            }

            double step = (Max - Min) / Bin;

            int HistoSize = (int)((Max - Min) / step) + 1;

            double[] axeX = new double[HistoSize];
            for (int i = 0; i < HistoSize; i++)
            {
                axeX[i] = Min + i * step;
            }
            ToReturn.Add(axeX);

            double[] histogram = new double[HistoSize];
            //double RealPos = Min;

            int PosHisto;
            foreach (double f in data)
            {
                PosHisto = (int)((f - Min) / step);
                if ((PosHisto >= 0) && (PosHisto < HistoSize))
                    histogram[PosHisto]++;
            }
            ToReturn.Add(histogram);

            return ToReturn;
        }

        public double[] MeanCenteringStdStandarization(double[] input)
        {
            double mean = Mean(input);
            double Stdev = std(input);

            double[] OutPut = new double[input.Length];

            for (int i = 0; i < input.Length; i++)
                OutPut[i] = input[i] - mean;

            for (int i = 0; i < input.Length; i++)
                OutPut[i] = OutPut[i] / Stdev;

            return OutPut;
        }

        public double Anderson_Darling(double[] tab)
        {
            double A = 0;
            double Mean1 = Mean(tab);
            double STD = std(tab);
            double[] norm = new double[tab.Length];

            for (int i = 0; i < tab.Length; i++)
                norm[i] = (tab[i] - Mean1) / STD;
            return A = Asquare(norm);
        }

        public double Asquare(double[] data)
        {
            double A = 0;
            double Mean1 = Mean(data);
            double varianceb = Math.Sqrt(2 * Variance(data));
            double err = 0;
            int cpt = 0;
            for (int i = 0; i < data.Length; i++)
            {
                cpt++;
                err += ((2 * cpt - 1) * (Math.Log(CDF(data[i], Mean1, varianceb)) + Math.Log(1 - CDF(data[data.Length - 1 - i], Mean1, varianceb))));
            }
            A = -data.Length - err / data.Length;

            return A;
        }

        public double CDF(double Y, double mu, double varb)
        {
            double Res = 0;
            Res = 0.5 * (1 + alglib.errorfunction((Y - mu) / (varb)));
            return Res;
        }


        #endregion

        #region User Interface Functions
        private void tabControlMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void tabControlMain_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files[0].Remove(0, files[0].Length - 4) == ".csv")
                {
                    LoadCSVAssay(files, false);
                    UpdateUIAfterLoading();
                }
            }
            return;
        }

        #region Descriptor List UI management
        private void checkedListBoxActiveDescriptors_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button != System.Windows.Forms.MouseButtons.Right) || (CompleteScreening == null)) return;

            ContextMenuStrip contextMenuStripActorPicker = new ContextMenuStrip();

            ToolStripMenuItem UnselectItem = new ToolStripMenuItem("Unselect all");
            UnselectItem.Click += new System.EventHandler(this.UnselectItem);

            ToolStripMenuItem SelectAllItem = new ToolStripMenuItem("Select all");
            SelectAllItem.Click += new System.EventHandler(this.SelectAllItem);

            ToolStripMenuItem ConcentrationToDescriptorItem = new ToolStripMenuItem("Concentration to descriptor");
            ConcentrationToDescriptorItem.Click += new System.EventHandler(this.ConcentrationToDescriptorItem);

            ToolStripMenuItem ColumnToDescriptorItem = new ToolStripMenuItem("Column to descriptor");
            ColumnToDescriptorItem.Click += new System.EventHandler(this.ColumnToDescriptorItem);

            ToolStripMenuItem RowToDescriptorItem = new ToolStripMenuItem("Row to descriptor");
            RowToDescriptorItem.Click += new System.EventHandler(this.RowToDescriptorItem);

            ToolStripMenuItem ToolStripConvertMenuItems = new ToolStripMenuItem("Convert");
            ToolStripConvertMenuItems.DropDownItems.Add(ConcentrationToDescriptorItem);
            ToolStripConvertMenuItems.DropDownItems.Add(ColumnToDescriptorItem);
            ToolStripConvertMenuItems.DropDownItems.Add(RowToDescriptorItem);


            ToolStripSeparator SepratorStrip = new ToolStripSeparator();

            Point locationOnForm = checkedListBoxActiveDescriptors.FindForm().PointToClient(Control.MousePosition);

            int VertPos = locationOnForm.Y - 163;
            int ItemHeight = checkedListBoxActiveDescriptors.GetItemHeight(0);
            int IdxItem = VertPos / ItemHeight;
            if ((IdxItem < CompleteScreening.ListDescriptors.Count) && ((IdxItem >= 0)))
            {
                ToolStripMenuItem ToolStripMenuItems = new ToolStripMenuItem(CompleteScreening.ListDescriptors[IdxItem].GetName());

                ToolStripMenuItem InfoDescItem = new ToolStripMenuItem("Info");
                IntToTransfer = IdxItem;
                InfoDescItem.Click += new System.EventHandler(this.InfoDescItem);
                ToolStripMenuItems.DropDownItems.Add(InfoDescItem);

                if (CompleteScreening.ListDescriptors.Count >= 2)
                {
                    ToolStripMenuItem RemoveDescItem = new ToolStripMenuItem("Remove");
                    RemoveDescItem.Click += new System.EventHandler(this.RemoveDescItem);
                    ToolStripMenuItems.DropDownItems.Add(RemoveDescItem);
                }

                if (CompleteScreening.ListDescriptors[IntToTransfer].GetBinNumber() > 1)
                {
                    ToolStripMenuItem SplitDescItem = new ToolStripMenuItem("Split");
                    SplitDescItem.Click += new System.EventHandler(this.SplitDescItem);
                    ToolStripMenuItems.DropDownItems.Add(SplitDescItem);
                }
                contextMenuStripActorPicker.Items.AddRange(new ToolStripItem[] { UnselectItem, SelectAllItem, ToolStripConvertMenuItems, ToolStripMenuItems });
            }
            else
            {

                contextMenuStripActorPicker.Items.AddRange(new ToolStripItem[] { UnselectItem, SelectAllItem, ToolStripConvertMenuItems });
            }
            contextMenuStripActorPicker.Show(Control.MousePosition);


        }

        static int IntToTransfer;
        void RemoveDescItem(object sender, EventArgs e)
        {
            System.Windows.Forms.DialogResult ResWin = MessageBox.Show("By applying this process, the selected descriptor will be definitively removed from this analysis ! Proceed ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ResWin == System.Windows.Forms.DialogResult.No) return;
            CompleteScreening.ListDescriptors.RemoveDesc(CompleteScreening.ListDescriptors[IntToTransfer], CompleteScreening);


            //CompleteScreening.UpDatePlateListWithFullAvailablePlate();
            for (int idxP = 0; idxP < CompleteScreening.ListPlatesActive.Count; idxP++)
                CompleteScreening.ListPlatesActive[idxP].UpDataMinMax();
            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);


        }

        void InfoDescItem(object sender, EventArgs e)
        {
            CompleteScreening.ListDescriptors[IntToTransfer].WindowDescriptorInfo.ShowDialog();
            CompleteScreening.ListDescriptors.UpDateDisplay();
        }

        void UnselectItem(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxActiveDescriptors.Items.Count; i++)
                CompleteScreening.ListDescriptors.SetItemState(i, false);

            RefreshInfoScreeningRichBox();
        }

        void SelectAllItem(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxActiveDescriptors.Items.Count; i++)
                CompleteScreening.ListDescriptors.SetItemState(i, true);

            RefreshInfoScreeningRichBox();
        }

        private void ConcentrationToDescriptorItem(object sender, EventArgs e)
        {
            cDescriptorsType ConcentrationType = new cDescriptorsType("Concentration", true, 1);

            CompleteScreening.ListDescriptors.AddNew(ConcentrationType);

            foreach (cPlate TmpPlate in CompleteScreening.ListPlatesAvailable)
            {
                foreach (cWell Tmpwell in TmpPlate.ListActiveWells)
                {
                    List<cDescriptor> LDesc = new List<cDescriptor>();

                    cDescriptor NewDesc = new cDescriptor(Tmpwell.Concentration, ConcentrationType,CompleteScreening);
                    LDesc.Add(NewDesc);

                    Tmpwell.AddDescriptors(LDesc);
                }
            }

            CompleteScreening.ListDescriptors.UpDateDisplay();
            CompleteScreening.UpDatePlateListWithFullAvailablePlate();

            for (int idxP = 0; idxP < CompleteScreening.ListPlatesActive.Count; idxP++)
                CompleteScreening.ListPlatesActive[idxP].UpDataMinMax();

            StartingUpDateUI();
        }

        private void ColumnToDescriptorItem(object sender, EventArgs e)
        {
            cDescriptorsType ColumnType = new cDescriptorsType("Column", true, 1);

            CompleteScreening.ListDescriptors.AddNew(ColumnType);

            foreach (cPlate TmpPlate in CompleteScreening.ListPlatesAvailable)
            {
                foreach (cWell Tmpwell in TmpPlate.ListActiveWells)
                {
                    List<cDescriptor> LDesc = new List<cDescriptor>();

                    cDescriptor NewDesc = new cDescriptor(Tmpwell.GetPosX(), ColumnType, CompleteScreening);
                    LDesc.Add(NewDesc);

                    Tmpwell.AddDescriptors(LDesc);
                }
            }

            CompleteScreening.ListDescriptors.UpDateDisplay();
            CompleteScreening.UpDatePlateListWithFullAvailablePlate();

            for (int idxP = 0; idxP < CompleteScreening.ListPlatesActive.Count; idxP++)
                CompleteScreening.ListPlatesActive[idxP].UpDataMinMax();

            StartingUpDateUI();
        }

        private void RowToDescriptorItem(object sender, EventArgs e)
        {
            cDescriptorsType RowType = new cDescriptorsType("Row", true, 1);

            CompleteScreening.ListDescriptors.AddNew(RowType);

            foreach (cPlate TmpPlate in CompleteScreening.ListPlatesAvailable)
            {
                foreach (cWell Tmpwell in TmpPlate.ListActiveWells)
                {
                    List<cDescriptor> LDesc = new List<cDescriptor>();

                    cDescriptor NewDesc = new cDescriptor(Tmpwell.GetPosY(), RowType, CompleteScreening);
                    LDesc.Add(NewDesc);

                    Tmpwell.AddDescriptors(LDesc);
                }
            }

            CompleteScreening.ListDescriptors.UpDateDisplay();
            CompleteScreening.UpDatePlateListWithFullAvailablePlate();

            for (int idxP = 0; idxP < CompleteScreening.ListPlatesActive.Count; idxP++)
                CompleteScreening.ListPlatesActive[idxP].UpDataMinMax();

            StartingUpDateUI();
        }

        private void SplitDescItem(object sender, EventArgs e)
        {
            int NumBin = CompleteScreening.ListDescriptors[IntToTransfer].GetBinNumber();

            // first we update the descriptor
            for (int i = 0; i < NumBin; i++)
                CompleteScreening.ListDescriptors.AddNew(new cDescriptorsType(CompleteScreening.ListDescriptors[IntToTransfer].GetName() + "_" + i, true, 1));

            foreach (cPlate TmpPlate in CompleteScreening.ListPlatesAvailable)
            {
                foreach (cWell Tmpwell in TmpPlate.ListActiveWells)
                {
                    List<cDescriptor> LDesc = new List<cDescriptor>();
                    for (int i = 0; i < NumBin; i++)
                    {
                        cDescriptor NewDesc = new cDescriptor(Tmpwell.ListDescriptors[IntToTransfer].Getvalue(i), CompleteScreening.ListDescriptors[i + IntToTransfer + 1],CompleteScreening);
                        LDesc.Add(NewDesc);
                    }
                    Tmpwell.AddDescriptors(LDesc);
                }
            }

            CompleteScreening.ListDescriptors.UpDateDisplay();
            CompleteScreening.UpDatePlateListWithFullAvailablePlate();

            for (int idxP = 0; idxP < CompleteScreening.ListPlatesActive.Count; idxP++)
                CompleteScreening.ListPlatesActive[idxP].UpDataMinMax();

            StartingUpDateUI();
        }
        #endregion

        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GlobalInfo.CurrentRichTextBox.Clear();
        }

        private void buttonClearConsole_Click(object sender, EventArgs e)
        {
            GlobalInfo.CurrentRichTextBox.Clear();
        }


        private void distributionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayHistogram(false);
        }

        private void checkBoxApplyToAllPlates_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxApplyToAllPlates.Checked)
                this.Cursor = Cursors.Default;
            else
                this.Cursor = Cursors.Cross;
            if (CompleteScreening == null) return;
            CompleteScreening.IsSelectionApplyToAllPlates = checkBoxApplyToAllPlates.Checked;
        }

        private void comboBoxClass_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index > 0)
            {
                SolidBrush BrushForColor = new SolidBrush(GlobalInfo.GetColor(e.Index - 1));
                e.Graphics.FillRectangle(BrushForColor, e.Bounds.X + 1, e.Bounds.Y + 1, 10, 10);
            }
            e.Graphics.DrawString(comboBoxClass.Items[e.Index].ToString(), comboBoxClass.Font,
                System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X + 15, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            e.DrawFocusRectangle();
        }

        /// <summary>
        /// Zoom Out function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSizeDecrease_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;
            CompleteScreening.GlobalInfo.ChangeSize(0.8f);
            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);
        }

        /// <summary>
        /// Zoom In function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSizeIncrease_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;
            CompleteScreening.GlobalInfo.ChangeSize(1.2f);
            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);
        }

        /// <summary>
        /// Change the slection mode (global or local)
        /// </summary>
        /// <param name="OnlyOnSelected">True: single plate selection; False: entiere screen selection</param>
        private void GlobalSelection(bool OnlyOnSelected)
        {
            if (CompleteScreening == null) return;

            if (CompleteScreening.GetSelectionType() == -2) return;

            for (int col = 0; col < CompleteScreening.Columns; col++)
                for (int row = 0; row < CompleteScreening.Rows; row++)
                {

                    if (CompleteScreening.IsSelectionApplyToAllPlates)
                    {
                        int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;

                        for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
                        {
                            cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(PlateIdx);
                            cWell TmpWell = CurrentPlateToProcess.GetWell(col, row, OnlyOnSelected);
                            if (TmpWell == null) continue;

                            if (CompleteScreening.GetSelectionType() == -1)
                                TmpWell.SetAsNoneSelected();
                            else
                                TmpWell.SetClass(CompleteScreening.GetSelectionType());
                        }
                    }
                    else
                    {
                        cWell TmpWell = CompleteScreening.GetCurrentDisplayPlate().GetWell(col, row, OnlyOnSelected);
                        if (TmpWell != null)
                        {
                            if (CompleteScreening.GetSelectionType() == -1) TmpWell.SetAsNoneSelected();
                            else
                                TmpWell.SetClass(CompleteScreening.GetSelectionType());


                        }
                    }
                }

            CompleteScreening.GetCurrentDisplayPlate().UpdateNumberOfClass();
        }

        /// <summary>
        /// Switch to global selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGlobalOnlySelected_Click(object sender, EventArgs e)
        {
            GlobalSelection(true);
        }

        /// <summary>
        /// Manage the event related to the active plate selection combo list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripcomboBoxPlateList_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompleteScreening.CurrentDisplayPlateIdx = this.toolStripcomboBoxPlateList.SelectedIndex;

            if (CompleteScreening.CurrentDisplayPlateIdx == -1) return;
            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);
        }

        /// <summary>
        /// Manage the event related to Class selection control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;
            CompleteScreening.SetSelectionType(comboBoxClass.SelectedIndex - 1);
            CompleteScreening.GetCurrentDisplayPlate().UpdateNumberOfClass();
        }

        /// <summary>
        /// set all the well of the current plate to "unselected" mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int col = 0; col < CompleteScreening.Columns; col++)
                for (int row = 0; row < CompleteScreening.Rows; row++)
                {
                    cWell TmpWell = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.CurrentDisplayPlateIdx).GetWell(col, row, false);
                    if (TmpWell != null) TmpWell.SetAsNoneSelected();
                }
        }

        private void checkedListBoxDescriptorActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CompleteScreening.CurrentDisplayPlateIdx == -1) return;

            for (int idxDesc = 0; idxDesc < CompleteScreening.ListDescriptors.Count; idxDesc++)
                CompleteScreening.ListDescriptors[idxDesc].SetActiveState(false);

            for (int IdxDesc = 0; IdxDesc < checkedListBoxActiveDescriptors.CheckedItems.Count; IdxDesc++)
                CompleteScreening.ListDescriptors[checkedListBoxActiveDescriptors.CheckedIndices[IdxDesc]].SetActiveState(true);

            RefreshInfoScreeningRichBox();
            return;
        }

        private void comboBoxDescriptorToDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompleteScreening.ListDescriptors.CurrentSelectedDescriptor = (int)comboBoxDescriptorToDisplay.SelectedIndex;

            if (!checkBoxDisplayClasses.Checked)
                CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);

        }

        private void StartingUpDateUI()
        {
            MyConsole = new FormConsole();

            GlobalInfo.CurrentRichTextBox = this.MyConsole.richTextBoxConsole;
            CompleteScreening.ListDescriptors.CurrentSelectedDescriptor = 0;
            CompleteScreening.CurrentDisplayPlateIdx = 0;
            CompleteScreening.LabelForClass = this.labelNumClasses;
            CompleteScreening.LabelForMin = this.labelMin;
            CompleteScreening.LabelForMax = this.labelMax;
            CompleteScreening.PanelForLUT = this.panelForLUT;
            CompleteScreening.PanelForPlate = this.panelForPlate;

            // CompleteScreening.ListDescriptors = new cListDescriptors(this.checkedListBoxActiveDescriptors, comboBoxDescriptorToDisplay);

            PlateListWindow.listBoxPlateNameToProcess.Items.Clear();
            PlateListWindow.listBoxAvaliableListPlates.Items.Clear();

            // CompleteScreening.ListBoxSelectedPlates = PlateListWindow.listBoxPlateNameToProcess;
            this.toolStripcomboBoxPlateList.Items.Clear();

            CompleteScreening.IsSelectionApplyToAllPlates = checkBoxApplyToAllPlates.Checked;
            GlobalInfo.CurrentScreen = CompleteScreening;
        }

        private void panelForLUT_Paint(object sender, PaintEventArgs e)
        {
            if ((CompleteScreening == null) || (CompleteScreening.ListPlatesAvailable.Count == 0) || (CompleteScreening.ISLoading))
                return;

            CompleteScreening.GetCurrentDisplayPlate().DisplayLUT(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor);
        }

        private void dataGridViewForQualityControl_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == -1) || (e.RowIndex == -1)) return;
            String PlateName = (string)dataGridViewForQualityControl.Rows[e.RowIndex].Cells[0].Value;
            String DescName = (string)dataGridViewForQualityControl.Rows[e.RowIndex].Cells[1].Value;
            tabControlMain.SelectedTab = tabPageDistribution;

            int PosPlate = this.toolStripcomboBoxPlateList.FindStringExact(PlateName);
            this.toolStripcomboBoxPlateList.SelectedIndex = PosPlate;
            CompleteScreening.CurrentDisplayPlateIdx = this.toolStripcomboBoxPlateList.SelectedIndex;

            int PosDesc = this.comboBoxDescriptorToDisplay.FindStringExact(DescName);
            comboBoxDescriptorToDisplay.SelectedIndex = PosDesc;
            CompleteScreening.ListDescriptors.CurrentSelectedDescriptor = (int)comboBoxDescriptorToDisplay.SelectedIndex;

            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);
        }

        private void comboBoxDimReductionNeutralClass_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            SolidBrush BrushForColor = new SolidBrush(GlobalInfo.GetColor(e.Index));
            e.Graphics.FillRectangle(BrushForColor, e.Bounds.X + 1, e.Bounds.Y + 1, 10, 10);
            e.Graphics.DrawString(comboBoxDimReductionNeutralClass.Items[e.Index].ToString(), comboBoxDimReductionNeutralClass.Font,
                System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X + 15, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            e.DrawFocusRectangle();
        }

        private void comboBoxNeutralClassForClassif_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            SolidBrush BrushForColor = new SolidBrush(GlobalInfo.GetColor(e.Index));
            e.Graphics.FillRectangle(BrushForColor, e.Bounds.X + 1, e.Bounds.Y + 1, 10, 10);
            e.Graphics.DrawString(comboBoxNeutralClassForClassif.Items[e.Index].ToString(), comboBoxNeutralClassForClassif.Font,
                System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X + 15, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            e.DrawFocusRectangle();
        }

        private void panelForPlate_Paint(object sender, PaintEventArgs e)
        {
            if ((CompleteScreening == null) || (CompleteScreening.ListPlatesAvailable.Count == 0) || (CompleteScreening.ISLoading))
                return;

            float SizeFont = GlobalInfo.SizeHistoHeight / 2;
            int Gutter = (int)GlobalInfo.OptionsWindow.numericUpDownGutter.Value;
            Graphics g = CompleteScreening.PanelForPlate.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(CompleteScreening.PanelForPlate.BackColor);
            int ScrollShiftY = panelForPlate.VerticalScroll.Value;
            int ScrollShiftX = panelForPlate.HorizontalScroll.Value;

            for (int i = 1; i <= CompleteScreening.Columns; i++)
                g.DrawString(i.ToString(), new Font("Arial", SizeFont), Brushes.White, new PointF((GlobalInfo.SizeHistoWidth + Gutter) * (i - 1) + (GlobalInfo.SizeHistoWidth + Gutter) / 4
                    - (i.ToString().Length - 1) * (GlobalInfo.SizeHistoWidth + Gutter) / 8 + GlobalInfo.ShiftX - ScrollShiftX, -ScrollShiftY));

            for (int j = 1; j <= CompleteScreening.Rows; j++)
            {
                if (GlobalInfo.OptionsWindow.radioButtonWellPosModeDouble.Checked)
                    g.DrawString(j.ToString(), new Font("Arial", SizeFont), Brushes.White, new PointF(-ScrollShiftX, (GlobalInfo.SizeHistoHeight + Gutter) * (j - 1) + GlobalInfo.ShiftY - ScrollShiftY));
                else
                    g.DrawString(GlobalInfo.ConvertIntPosToStringPos(j), new Font("Arial", SizeFont), Brushes.White, new PointF(-ScrollShiftX, (GlobalInfo.SizeHistoHeight + Gutter) * (j - 1) + GlobalInfo.ShiftY - ScrollShiftY));
            }
        }

        private void UpdateUIAfterLoading()
        {
            if (comboBoxDescriptorToDisplay.Items.Count >= 1)
            {
                pluginsToolStripMenuItem.Enabled = true;
                exportToolStripMenuItem.Enabled = true;
                appendDescriptorsToolStripMenuItem.Enabled = true;
                linkToolStripMenuItem.Enabled = true;
                copyAverageValuesToolStripMenuItem1.Enabled = true;
                copyClassesToolStripMenuItem.Enabled = true;
                swapClassesToolStripMenuItem.Enabled = true;
                applySelectionToScreenToolStripMenuItem.Enabled = true;
                visualizationToolStripMenuItem.Enabled = true;
                qualityControlsToolStripMenuItem1.Enabled = true;
                buttonReduceDim.Enabled = true;
                visualizationToolStripMenuItemPCA.Enabled = true;
                qualityControlToolStripMenuItem.Enabled = true;
                buttonQualityControl.Enabled = true;
                buttonCorrectionPlateByPlate.Enabled = true;
                buttonRejectPlates.Enabled = true;
                buttonNormalize.Enabled = true;
                buttonCluster.Enabled = true;
                buttonStartClassification.Enabled = true;
                buttonExport.Enabled = true;
                platesManagerToolStripMenuItem.Enabled = true;
                doseResponseManagerToolStripMenuItem.Enabled = true;
                toolStripMenuItemGeneAnalysis.Enabled = true;
                dRCAnalysisToolStripMenuItem.Enabled = true;
                dRCAnalysisToolStripMenuItem1.Enabled = true;
                CompleteScreening.ISLoading = false;
                comboBoxDescriptorToDisplay.SelectedIndex = 0;
                string NamePlate = PlateListWindow.listBoxAvaliableListPlates.Items[0].ToString();
                toolStripcomboBoxPlateList.Text = NamePlate + " ";
            }
        }

        public void RefreshInfoScreeningRichBox()
        {
            if (tabControlMain.SelectedTab.Name != "tabPageExport") return;
            richTextBoxForScreeningInformation.Clear();

            if (CompleteScreening == null) return;

            string Tmp = "Plate dimensions: " + CompleteScreening.Columns + " x " + CompleteScreening.Rows + "\n\n\n";
            richTextBoxForScreeningInformation.AppendText(Tmp);

            Tmp = "Number of plates: " + CompleteScreening.ListPlatesActive.Count + " (/ " + CompleteScreening.ListPlatesAvailable.Count + ")\n\n";
            int TotalWells = 0;
            for (int PlateIdx = 1; PlateIdx <= CompleteScreening.ListPlatesActive.Count; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(PlateIdx - 1);
                Tmp += "Plate " + PlateIdx + " :\t" + CurrentPlateToProcess.Name + "\n";
                Tmp += "\t" + CurrentPlateToProcess.GetNumberOfActiveWells() + " active wells / " + CurrentPlateToProcess.GetNumberOfClasses() + " classes.\n";
                TotalWells += CurrentPlateToProcess.GetNumberOfActiveWells();
            }
            richTextBoxForScreeningInformation.AppendText(Tmp + "\n");

            Tmp = "Number of active wells: " + TotalWells;
            richTextBoxForScreeningInformation.AppendText(Tmp + "\n\n");

            Tmp = "Number of active descriptors: " + CompleteScreening.GetNumberOfActiveDescriptor() + " (/ " + CompleteScreening.ListDescriptors.Count + ")\n\n";
            for (int Desc = 1; Desc <= CompleteScreening.ListDescriptors.Count; Desc++)
            {
                if (CompleteScreening.ListDescriptors[Desc - 1].IsActive() == false) continue;
                Tmp += "Descriptor " + Desc + " :\t" + CompleteScreening.ListDescriptors[Desc - 1].GetName() + "\n";
            }
            richTextBoxForScreeningInformation.AppendText(Tmp + "\n");
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshInfoScreeningRichBox();
        }

        private void checkBoxDisplayClasses_CheckedChanged(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;
            CompleteScreening.GlobalInfo.IsDisplayClassOnly = checkBoxDisplayClasses.Checked;
            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);
        }

        // Convert and normalize the points and draw the reversible frame.
        private void MyDrawReversibleRectangle(Point p1, Point p2)
        {
            Rectangle rc = new Rectangle();

            // Convert the points to screen coordinates.
            p1 = PointToScreen(p1);
            //p1.X += tabControlMain.Location.X;
            //p1.Y += tabControlMain.Location.Y;

            p2 = PointToScreen(p2);
            //p2.X += tabControlMain.Location.X;
            //p2.Y += tabControlMain.Location.Y;

            // Normalize the rectangle.
            if (p1.X < p2.X)
            {
                rc.X = p1.X;
                rc.Width = p2.X - p1.X;
            }
            else
            {
                rc.X = p2.X;
                rc.Width = p1.X - p2.X;
            }
            if (p1.Y < p2.Y)
            {
                rc.Y = p1.Y;
                rc.Height = p2.Y - p1.Y;
            }
            else
            {
                rc.Y = p2.Y;
                rc.Height = p1.Y - p2.Y;
            }
            // Draw the reversible frame.

            ControlPaint.DrawReversibleFrame(rc, Color.Red, FrameStyle.Dashed);
        }

        private void panelForPlate_MouseWheel(object sender, MouseEventArgs e)
        {
            // Update the drawing based upon the mouse wheel scrolling.
            if (CompleteScreening == null) return;

            int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
            if (numberOfTextLinesToMove > 0)
                CompleteScreening.GlobalInfo.ChangeSize(numberOfTextLinesToMove);
            else
                CompleteScreening.GlobalInfo.ChangeSize(1.0f / (-1 * numberOfTextLinesToMove));

            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);
        }

        private void panelForPlate_MouseDown(object sender, MouseEventArgs e)
        {
            if (CompleteScreening == null) return;


            //     if (GlobalInfo.WindowForDRCDesign.Visible) return;

            // Make a note that we "have the mouse".
            bHaveMouse = true;

            // Store the "starting point" for this rubber-band rectangle.
            CompleteScreening.ptOriginal.X = e.X + panelForPlate.Location.X + 10;
            CompleteScreening.ptOriginal.Y = e.Y + panelForPlate.Location.Y + 76;
            // Special value lets us know that no previous
            // rectangle needs to be erased.
            CompleteScreening.ptLast.X = -1;
            CompleteScreening.ptLast.Y = -1;
        }

        private void panelForPlate_MouseMove(object sender, MouseEventArgs e)
        {
            if (CompleteScreening == null) return;

            //  if (GlobalInfo.WindowForDRCDesign.Visible) return;

            Point ptCurrent = new Point(e.X + panelForPlate.Location.X + 10, e.Y + panelForPlate.Location.Y + 76);
            // If we "have the mouse", then we draw our lines.
            if (bHaveMouse)
            {
                // If we have drawn previously, draw again in
                // that spot to remove the lines.
                if (CompleteScreening.ptLast.X != -1)
                {
                    MyDrawReversibleRectangle(CompleteScreening.ptOriginal, CompleteScreening.ptLast);
                }
                // Update last point.
                CompleteScreening.ptLast = ptCurrent;
                // Draw new lines.
                MyDrawReversibleRectangle(CompleteScreening.ptOriginal, ptCurrent);
            }
        }

        private void panelForPlate_MouseUp(object sender, MouseEventArgs e)
        {
            if (CompleteScreening == null) return;

            //  if (GlobalInfo.WindowForDRCDesign.Visible) return;

            // Set internal flag to know we no longer "have the mouse".
            bHaveMouse = false;
            // If we have drawn previously, draw again in that spot
            // to remove the lines.
            if (CompleteScreening.ptLast.X != -1)
            {
                Point ptCurrent = new Point(e.X + panelForPlate.Location.X + 10, e.Y + panelForPlate.Location.Y + 76);
                MyDrawReversibleRectangle(CompleteScreening.ptOriginal, CompleteScreening.ptLast);

                if (!GlobalInfo.WindowForDRCDesign.Visible)
                    CompleteScreening.GetCurrentDisplayPlate().UpDateWellsSelection();
                else
                {
                    int PosMouseXMax = CompleteScreening.ptLast.X;
                    int PosMouseXMin = CompleteScreening.ptOriginal.X;
                    if (CompleteScreening.ptOriginal.X > PosMouseXMax)
                    {
                        PosMouseXMax = CompleteScreening.ptOriginal.X;
                        PosMouseXMin = CompleteScreening.ptLast.X;
                    }

                    int PosMouseYMax = CompleteScreening.ptLast.Y;
                    int PosMouseYMin = CompleteScreening.ptOriginal.Y;
                    if (CompleteScreening.ptOriginal.Y > PosMouseYMax)
                    {
                        PosMouseYMax = CompleteScreening.ptOriginal.Y;
                        PosMouseYMin = CompleteScreening.ptLast.Y;
                    }

                    //List<cWell> ListWellSelected = new List<cWell>();
                    GlobalInfo.WindowForDRCDesign.ListWells = new List<cWell>();


                    for (int j = 0; j < CompleteScreening.Rows; j++)
                        for (int i = 0; i < CompleteScreening.Columns; i++)
                        {
                            cWell TempWell = CompleteScreening.GetCurrentDisplayPlate().GetWell(i, j, false);
                            if (TempWell == null) continue;
                            int PWellX = (int)((TempWell.GetPosX() + 1) * (CompleteScreening.GlobalInfo.SizeHistoWidth + (int)GlobalInfo.OptionsWindow.numericUpDownGutter.Value));// - 2*ParentScreening.GlobalInfo.ShiftX);
                            int PWellY = (int)((TempWell.GetPosY() + 1) * (CompleteScreening.GlobalInfo.SizeHistoHeight + (int)GlobalInfo.OptionsWindow.numericUpDownGutter.Value) + +(int)((int)GlobalInfo.OptionsWindow.numericUpDownGutter.Value * 2.5) + 60);// (int)ParentScreening.GlobalInfo.OptionsWindow.numericUpDownShiftY.Value);

                            if ((PWellX > PosMouseXMin) && (PWellX < PosMouseXMax) && (PWellY > PosMouseYMin) && (PWellY < PosMouseYMax))
                            {
                                GlobalInfo.WindowForDRCDesign.ListWells.Add(TempWell);
                            }
                        }

                    GlobalInfo.WindowForDRCDesign.DrawSignature();
                }


                //    if (CompleteScreening.GlobalInfo.IsDisplayClassOnly)
                CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);
            }
            // Set flags to know that there is no "previous" line to reverse.
            CompleteScreening.ptLast.X = -1;
            CompleteScreening.ptLast.Y = -1;
            CompleteScreening.ptOriginal.X = -1;
            CompleteScreening.ptOriginal.Y = -1;
        }
        #endregion

        #region Selection management
        private void buttonGlobalSelection_Click(object sender, EventArgs e)
        {
            GlobalSelection(false);
            // CompleteScreening.GlobalInfo.IsDisplayClassOnly = checkBoxDisplayClasses.Checked;

            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);
        }

        private void applySelectionToScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;
            CompleteScreening.ApplyCurrentClassesToAllPlates();
        }

        private void swapClassesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            FormForSwapClasses WindowSwapClasses = new FormForSwapClasses(GlobalInfo);
            if (WindowSwapClasses.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            int Idx = 0;
            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;


            int OriginalIdx = WindowSwapClasses.comboBoxOriginalClass.SelectedIndex - 1;
            int DestinatonIdx = WindowSwapClasses.comboBoxDestinationClass.SelectedIndex - 1;

            // loop on all the plate
            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(PlateIdx);

                for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                    for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                    {
                        cWell TmpWell = CurrentPlateToProcess.GetWell(IdxValue, IdxValue0, false);
                        if (TmpWell == null) continue;

                        if (TmpWell.GetClass() == OriginalIdx)
                        {
                            if (DestinatonIdx == -1)
                                TmpWell.SetAsNoneSelected();
                            else
                                TmpWell.SetClass(DestinatonIdx);

                            Idx++;
                        }

                    }
            }
            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);
            MessageBox.Show(Idx + " wells have been swapped !", "Process over !", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        #endregion

        #region Scatter point graphs section
        private void scatterPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;
            Series CurrentSeries = new Series("ScatterPoints");
            CurrentSeries.ShadowOffset = 1;

            int Idx = 0;
            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;

            // loop on all the plate
            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);

                for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                    for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                    {
                        cWell TmpWell = CurrentPlateToProcess.GetWell(IdxValue, IdxValue0, true);
                        if (TmpWell == null) continue;
                        CurrentSeries.Points.Add(TmpWell.ListDescriptors[comboBoxDescriptorToDisplay.SelectedIndex].GetValue());
                        CurrentSeries.Points[Idx].Color = TmpWell.GetColor();
                        CurrentSeries.Points[Idx].MarkerStyle = MarkerStyle.Circle;
                        CurrentSeries.Points[Idx].MarkerSize = 6;
                        if (!GlobalInfo.OptionsWindow.checkBoxDisplayFastPerformance.Checked) CurrentSeries.Points[Idx].ToolTip = TmpWell.AssociatedPlate.Name + "\n" + TmpWell.GetPosX() + "x" + TmpWell.GetPosY() + " :" + TmpWell.Name;
                        Idx++;
                    }
            }

            SimpleForm NewWindow = new SimpleForm();

            if (Idx > (int)GlobalInfo.OptionsWindow.numericUpDownMaximumWidth.Value)
                NewWindow.Width = (int)GlobalInfo.OptionsWindow.numericUpDownMaximumWidth.Value;
            else
                NewWindow.Width = Idx;
            NewWindow.Height = 400;

            ChartArea CurrentChartArea = new ChartArea();
            CurrentChartArea.BorderColor = Color.Black;
            CurrentChartArea.CursorX.IsUserSelectionEnabled = true;
            NewWindow.chartForSimpleForm.ChartAreas.Add(CurrentChartArea);

            NewWindow.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackColor = Color.FromArgb(164, 164, 164);

            CurrentChartArea.Axes[1].Title = CompleteScreening.ListDescriptors[comboBoxDescriptorToDisplay.SelectedIndex].GetName();
            CurrentChartArea.Axes[0].Title = "Index";

            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;


            CurrentSeries.ChartType = SeriesChartType.Point;
            if (GlobalInfo.OptionsWindow.checkBoxDisplayFastPerformance.Checked) CurrentSeries.ChartType = SeriesChartType.FastPoint;

            NewWindow.chartForSimpleForm.Series.Add(CurrentSeries);

            double Av = NewWindow.chartForSimpleForm.DataManipulator.Statistics.Mean("ScatterPoints");
            double Std = Math.Sqrt(NewWindow.chartForSimpleForm.DataManipulator.Statistics.Variance("ScatterPoints", true));





            StripLine StdLine = new StripLine();
            StdLine.BackColor = Color.FromArgb(64, Color.BlanchedAlmond);
            StdLine.IntervalOffset = Av - 1.5 * Std;
            StdLine.StripWidth = 3 * Std;

            CurrentChartArea.AxisY.StripLines.Add(StdLine);

            StripLine AverageLine = new StripLine();
            AverageLine.BackColor = Color.Red;
            AverageLine.IntervalOffset = Av;
            AverageLine.StripWidth = 0.0001;
            AverageLine.Text = String.Format("{0:0.###}", Av);
            CurrentChartArea.AxisY.StripLines.Add(AverageLine);

            NewWindow.Text = "Scatter Point / " + Idx + " points";
            NewWindow.Show();
            NewWindow.chartForSimpleForm.Update();
            NewWindow.chartForSimpleForm.Show();
            NewWindow.Controls.AddRange(new System.Windows.Forms.Control[] { NewWindow.chartForSimpleForm });
            return;
        }

        private void scatterPointsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;
            SimpleForm NewWindow = new SimpleForm();
            Series CurrentSeries = new Series("ScatterPoints");

            CurrentSeries.ShadowOffset = 1;

            int Idx = 0;
            for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                {
                    cWell TmpWell = CompleteScreening.GetCurrentDisplayPlate().GetWell(IdxValue, IdxValue0, true);
                    if (TmpWell != null)
                    {
                        CurrentSeries.Points.Add(TmpWell.ListDescriptors[comboBoxDescriptorToDisplay.SelectedIndex].GetValue());
                        CurrentSeries.Points[Idx].Color = CompleteScreening.GetCurrentDisplayPlate().GetWell(IdxValue, IdxValue0, true).GetColor();
                        CurrentSeries.Points[Idx].ToolTip = TmpWell.GetPosX() + "x" + TmpWell.GetPosY() + " :" + TmpWell.Name;

                        CurrentSeries.Points[Idx].MarkerStyle = MarkerStyle.Circle;
                        CurrentSeries.Points[Idx].MarkerSize = 8;
                        Idx++;
                    }
                }
            ChartArea CurrentChartArea = new ChartArea();
            CurrentChartArea.CursorX.IsUserSelectionEnabled = true;
            CurrentChartArea.BorderColor = Color.Black;

            NewWindow.chartForSimpleForm.ChartAreas.Add(CurrentChartArea);

            NewWindow.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackColor = Color.FromArgb(164, 164, 164);

            CurrentChartArea.Axes[1].Title = CompleteScreening.ListDescriptors[CompleteScreening.ListDescriptors.CurrentSelectedDescriptor].GetName();
            CurrentChartArea.Axes[0].Title = "Index";
            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
            CurrentSeries.ChartType = SeriesChartType.Point;

            NewWindow.chartForSimpleForm.Series.Add(CurrentSeries);

            double Av = NewWindow.chartForSimpleForm.DataManipulator.Statistics.Mean("ScatterPoints");
            double Std = Math.Sqrt(NewWindow.chartForSimpleForm.DataManipulator.Statistics.Variance("ScatterPoints", true));

            StripLine StdLine = new StripLine();

            StdLine.BackColor = Color.FromArgb(64, Color.BlanchedAlmond);

            StdLine.IntervalOffset = Av - 1.5 * Std;
            StdLine.StripWidth = 3 * Std;
            CurrentChartArea.AxisY.StripLines.Add(StdLine);

            StripLine AverageLine = new StripLine();
            AverageLine.BackColor = Color.Red;
            AverageLine.IntervalOffset = Av;
            AverageLine.StripWidth = 0.01;
            AverageLine.Text = String.Format("{0:0.###}", Av);
            CurrentChartArea.AxisY.StripLines.Add(AverageLine);

            NewWindow.Text = "Scatter Point / " + CompleteScreening.GetCurrentDisplayPlate().GetNumberOfActiveWells() + " points";
            NewWindow.Show();
            NewWindow.chartForSimpleForm.Update();
            NewWindow.chartForSimpleForm.Show();
            NewWindow.Controls.AddRange(new System.Windows.Forms.Control[] { NewWindow.chartForSimpleForm });
            return;
        }
        #endregion

        #region Options window
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalInfo.OptionsWindow.CurrentScreen = CompleteScreening;
            GlobalInfo.OptionsWindow.Visible = !GlobalInfo.OptionsWindow.Visible;
            GlobalInfo.OptionsWindow.Update();
        }

        #endregion

        #region Misc menus (console, plates manager, exit, about)
        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyConsole.Visible = !MyConsole.Visible;
            MyConsole.Update();
        }

        private void platesManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            PlateListWindow.listBoxPlateNameToProcess.Items.Clear();
            for (int i = 0; i < CompleteScreening.ListPlatesActive.Count; i++)
                PlateListWindow.listBoxPlateNameToProcess.Items.Add(CompleteScreening.ListPlatesActive[i].Name);


            PlateListWindow.ShowDialog();// != System.Windows.Forms.DialogResult.OK) return; 

            while (PlateListWindow.listBoxPlateNameToProcess.Items.Count == 0)
            {
                MessageBox.Show("At least one plate has to bo selected !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PlateListWindow.ShowDialog();// != System.Windows.Forms.DialogResult.OK) return; 
            }
            toolStripcomboBoxPlateList.Items.Clear();
            CompleteScreening.ListPlatesActive.Clear();
            RefreshInfoScreeningRichBox();

            for (int i = 0; i < PlateListWindow.listBoxPlateNameToProcess.Items.Count; i++)
            {
                CompleteScreening.ListPlatesActive.Add(CompleteScreening.ListPlatesAvailable.GetPlate((string)PlateListWindow.listBoxPlateNameToProcess.Items[i]));
                toolStripcomboBoxPlateList.Items.Add(CompleteScreening.ListPlatesActive[i].Name);
            }
            CompleteScreening.CurrentDisplayPlateIdx = 0;
            toolStripcomboBoxPlateList.SelectedIndex = 0;

            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(GlobalInfo.CurrentScreen!=null)
            GlobalInfo.CurrentScreen.Close3DView();
            this.Dispose();
        }

        private void aboutHCSAnalyzerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox About = new AboutBox();
            About.Text = "HCS Analyzer";
            //About.Opacity = 0.9;
            About.ShowDialog();
        }
        #endregion

        #region Quality controls - Zfactor, SSMD, Coeff. of variation, descriptor evolution

        /// <summary>
        /// This function displays the evolution of the average value of a certain descriptor through the plates, for a specified class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void descriptorEvolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            FormClassification WindowClassification = new FormClassification(CompleteScreening);
            WindowClassification.label1.Text = "Class";
            WindowClassification.Text = CompleteScreening.ListDescriptors[CompleteScreening.ListDescriptors.CurrentSelectedDescriptor].GetName() + " evolution";
            WindowClassification.buttonClassification.Text = "Display";

            if (WindowClassification.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            int SelectedClass = WindowClassification.comboBoxForNeutralClass.SelectedIndex;
            cExtendedList ListValuePerWell = new cExtendedList();
            List<cDescriptor> List_Averages = new List<cDescriptor>();

            cWell TempWell;
            int Desc = this.comboBoxDescriptorToDisplay.SelectedIndex;

            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;
            Series CurrentSeries = new Series();
            CurrentSeries.Name = "Series1";

            CurrentSeries.ChartType = SeriesChartType.ErrorBar;
            CurrentSeries.ShadowOffset = 1;

            Series SeriesLine = new Series();
            SeriesLine.Name = "SeriesLine";
            SeriesLine.ShadowOffset = 1;
            SeriesLine.ChartType = SeriesChartType.Line;

            int RealPlateIdx = 0;

            // loop on all the plates
            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);
                ListValuePerWell.Clear();

                for (int row = 0; row < CompleteScreening.Rows; row++)
                    for (int col = 0; col < CompleteScreening.Columns; col++)
                    {
                        TempWell = CurrentPlateToProcess.GetWell(col, row, true);
                        if (TempWell == null) continue;
                        else
                        {
                            if (TempWell.GetClass() == SelectedClass)
                            {
                                double Val = TempWell.ListDescriptors[Desc].GetValue();
                                if (double.IsNaN(Val)) continue;

                                ListValuePerWell.Add(Val);
                            }
                        }
                    }

                if (ListValuePerWell.Count >= 1)
                {
                    DataPoint CurrentPt = new DataPoint();
                    CurrentPt.XValue = RealPlateIdx;

                    double[] Values = new double[3];
                    Values[0] = ListValuePerWell.Mean();
                    double Std = ListValuePerWell.Std();
                    Values[1] = Values[0] - Std;
                    Values[2] = Values[0] + Std;
                    CurrentPt.YValues = Values;//ListValuePerWell.ToArray();
                    CurrentSeries.Points.Add(CurrentPt);

                    CurrentSeries.Points[RealPlateIdx].AxisLabel = CurrentPlateToProcess.Name;
                    CurrentSeries.Points[RealPlateIdx].Font = new Font("Arial", 8);
                    CurrentSeries.Points[RealPlateIdx].Color = CompleteScreening.GlobalInfo.GetColor(SelectedClass);

                    SeriesLine.Points.AddXY(RealPlateIdx, Values[0]);

                    SeriesLine.Points[RealPlateIdx].ToolTip = "Plate name: " + CurrentPlateToProcess.Name + "\nAverage: " + string.Format("{0:0.###}", Values[0]) + "\nStdev: " + string.Format("{0:0.###}", Std);
                    SeriesLine.Points[RealPlateIdx].Font = new Font("Arial", 8);
                    SeriesLine.Points[RealPlateIdx].BorderColor = Color.Black;
                    SeriesLine.Points[RealPlateIdx].MarkerStyle = MarkerStyle.Circle;
                    SeriesLine.Points[RealPlateIdx].MarkerSize = 8;

                    RealPlateIdx++;
                }
            }

            SimpleForm NewWindow = new SimpleForm();
            int thisWidth = 200 * SeriesLine.Points.Count;
            if (thisWidth > 1500) thisWidth = 1500;
            NewWindow.Width = thisWidth;
            NewWindow.Height = 400;

            ChartArea CurrentChartArea = new ChartArea();
            CurrentChartArea.BorderColor = Color.Black;
            NewWindow.chartForSimpleForm.ChartAreas.Add(CurrentChartArea);

            CurrentChartArea.AxisX.Interval = 1;
            CurrentChartArea.Axes[1].IsMarksNextToAxis = true;
            CurrentChartArea.Axes[1].Title = CompleteScreening.ListDescriptors[CompleteScreening.ListDescriptors.CurrentSelectedDescriptor].GetName();
            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
            CurrentChartArea.Axes[1].MajorGrid.Enabled = false;

            NewWindow.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackGradientStyle = GradientStyle.TopBottom;
            CurrentChartArea.BackColor = CompleteScreening.GlobalInfo.OptionsWindow.panel1.BackColor;
            CurrentChartArea.BackSecondaryColor = Color.White;

            CurrentSeries["BoxPlotWhiskerPercentile"] = "false";
            CurrentSeries["BoxPlotShowMedian"] = "false";
            CurrentSeries["BoxPlotWhiskerPercentile"] = "false";
            CurrentSeries["BoxPlotShowAverage"] = "false";
            CurrentSeries["BoxPlotPercentile"] = "false";


            CurrentChartArea.AxisX.IsLabelAutoFit = true;
            NewWindow.chartForSimpleForm.Series.Add(CurrentSeries);
            NewWindow.chartForSimpleForm.Series.Add(SeriesLine);

            Title CurrentTitle = new Title("Class " + SelectedClass + " " + CurrentChartArea.Axes[1].Title + " evolution");

            NewWindow.chartForSimpleForm.Titles.Add(CurrentTitle);
            NewWindow.chartForSimpleForm.Titles[0].Font = new Font("Arial", 9);
            NewWindow.Show();
            NewWindow.chartForSimpleForm.Update();
            NewWindow.chartForSimpleForm.Show();
            NewWindow.Controls.AddRange(new System.Windows.Forms.Control[] { NewWindow.chartForSimpleForm });
            return;
        }

        private void zscoreToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            BuildZFactor(this.comboBoxDescriptorToDisplay.SelectedIndex).Show();
        }


        private void zscoreSinglePlateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<double> Pos = new List<double>();
            List<double> Neg = new List<double>();
            List<cSimpleSignature> ZFactorList = new List<cSimpleSignature>();

            int NumDesc = CompleteScreening.ListDescriptors.Count;

            cWell TempWell;
            // loop on all the desciptors
            for (int Desc = 0; Desc < NumDesc; Desc++)
            {
                Pos.Clear();
                Neg.Clear();

                if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;

                for (int row = 0; row < CompleteScreening.Rows; row++)
                    for (int col = 0; col < CompleteScreening.Columns; col++)
                    {
                        TempWell = CompleteScreening.GetCurrentDisplayPlate().GetWell(col, row, true);
                        if (TempWell == null) continue;
                        else
                        {
                            if (TempWell.GetClass() == 0)
                                Pos.Add(TempWell.ListDescriptors[Desc].GetValue());
                            if (TempWell.GetClass() == 1)
                                Neg.Add(TempWell.ListDescriptors[Desc].GetValue());
                        }
                    }
                if (Pos.Count < 3)
                {
                    MessageBox.Show("No or not enough positive controls !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (Neg.Count < 3)
                {
                    MessageBox.Show("No or not enough negative controls !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                double ZScore = 1 - 3 * (std(Pos.ToArray()) + std(Neg.ToArray())) / (Math.Abs(Mean(Pos.ToArray()) - Mean(Neg.ToArray())));
                GlobalInfo.ConsoleWriteLine(CompleteScreening.ListDescriptors[Desc].GetName() + ", Z-Score = " + ZScore);
                cSimpleSignature TmpDesc = new cSimpleSignature(CompleteScreening.ListDescriptors[Desc].GetName(), ZScore);
                ZFactorList.Add(TmpDesc);
            }

            ZFactorList.Sort(delegate(cSimpleSignature p1, cSimpleSignature p2) { return p1.AverageValue.CompareTo(p2.AverageValue); });

            Series CurrentSeries = new Series();
            CurrentSeries.ChartType = SeriesChartType.Column;
            CurrentSeries.ShadowOffset = 1;

            Series SeriesLine = new Series();
            SeriesLine.Name = "SeriesLine";
            SeriesLine.ShadowOffset = 1;
            SeriesLine.ChartType = SeriesChartType.Line;

            int RealIdx = 0;
            for (int IdxValue = 0; IdxValue < ZFactorList.Count; IdxValue++)
            {
                if (ZFactorList[IdxValue].AverageValue.ToString() == "NaN") continue;

                CurrentSeries.Points.Add(ZFactorList[IdxValue].AverageValue);
                CurrentSeries.Points[RealIdx].Label = string.Format("{0:0.###}", ZFactorList[IdxValue].AverageValue);
                CurrentSeries.Points[RealIdx].Font = new Font("Arial", 10);
                CurrentSeries.Points[RealIdx].ToolTip = ZFactorList[IdxValue].Name;
                CurrentSeries.Points[RealIdx].AxisLabel = ZFactorList[IdxValue].Name;

                SeriesLine.Points.Add(ZFactorList[IdxValue].AverageValue);
                SeriesLine.Points[RealIdx].BorderColor = Color.Black;
                SeriesLine.Points[RealIdx].MarkerStyle = MarkerStyle.Circle;
                SeriesLine.Points[RealIdx].MarkerSize = 4;
                RealIdx++;
            }

            SimpleForm NewWindow = new SimpleForm();
            int thisWidth = 200 * RealIdx;
            if (thisWidth > (int)GlobalInfo.OptionsWindow.numericUpDownMaximumWidth.Value) thisWidth = (int)GlobalInfo.OptionsWindow.numericUpDownMaximumWidth.Value;
            NewWindow.Width = thisWidth;
            NewWindow.Height = 400;
            NewWindow.Text = "Z-factors";

            ChartArea CurrentChartArea = new ChartArea();
            CurrentChartArea.BorderColor = Color.Black;
            CurrentChartArea.AxisX.Interval = 1;
            NewWindow.chartForSimpleForm.Series.Add(CurrentSeries);
            NewWindow.chartForSimpleForm.Series.Add(SeriesLine);

            CurrentChartArea.AxisX.IsLabelAutoFit = true;
            NewWindow.chartForSimpleForm.ChartAreas.Add(CurrentChartArea);

            CurrentChartArea.Axes[1].Maximum = 2;
            CurrentChartArea.Axes[1].IsMarksNextToAxis = true;
            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
            CurrentChartArea.Axes[1].MajorGrid.Enabled = false;

            NewWindow.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackGradientStyle = GradientStyle.TopBottom;
            CurrentChartArea.BackColor = CompleteScreening.GlobalInfo.OptionsWindow.panel1.BackColor;
            CurrentChartArea.BackSecondaryColor = Color.White;

            Title CurrentTitle = new Title(CompleteScreening.GetCurrentDisplayPlate().Name + " Z-factors");
            CurrentTitle.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            NewWindow.chartForSimpleForm.Titles.Add(CurrentTitle);

            NewWindow.Show();
            NewWindow.chartForSimpleForm.Update();
            NewWindow.chartForSimpleForm.Show();
            NewWindow.Controls.AddRange(new System.Windows.Forms.Control[] { NewWindow.chartForSimpleForm });
        }

        private SimpleForm BuildZFactor(int Desc)
        {
            List<double> Pos = new List<double>();
            List<double> Neg = new List<double>();
            List<cSimpleSignature> ZFactorList = new List<cSimpleSignature>();

            cWell TempWell;
            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;

            // loop on all the plate
            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);

                Pos.Clear();
                Neg.Clear();


                for (int row = 0; row < CompleteScreening.Rows; row++)
                    for (int col = 0; col < CompleteScreening.Columns; col++)
                    {
                        TempWell = CurrentPlateToProcess.GetWell(col, row, true);
                        if (TempWell == null) continue;
                        else
                        {
                            if (TempWell.GetClass() == 0)
                                Pos.Add(TempWell.ListDescriptors[Desc].GetValue());
                            if (TempWell.GetClass() == 1)
                                Neg.Add(TempWell.ListDescriptors[Desc].GetValue());
                        }
                    }

                double ZScore = 1 - 3 * (std(Pos.ToArray()) + std(Neg.ToArray())) / (Math.Abs(Mean(Pos.ToArray()) - Mean(Neg.ToArray())));
                GlobalInfo.ConsoleWriteLine(CurrentPlateToProcess.Name + ", Z-Score = " + ZScore);
                cSimpleSignature TmpDesc = new cSimpleSignature(CurrentPlateToProcess.Name, ZScore);
                ZFactorList.Add(TmpDesc);
            }

            Series CurrentSeries = new Series();
            CurrentSeries.ChartType = SeriesChartType.Column;
            CurrentSeries.ShadowOffset = 1;

            Series SeriesLine = new Series();
            SeriesLine.Name = "SeriesLine";
            SeriesLine.ShadowOffset = 1;
            SeriesLine.ChartType = SeriesChartType.Line;

            int RealIdx = 0;
            for (int IdxValue = 0; IdxValue < ZFactorList.Count; IdxValue++)
            {
                if (ZFactorList[IdxValue].AverageValue.ToString() == "NaN") continue;

                CurrentSeries.Points.Add(ZFactorList[IdxValue].AverageValue);
                CurrentSeries.Points[RealIdx].Label = string.Format("{0:0.###}", ZFactorList[IdxValue].AverageValue);
                CurrentSeries.Points[RealIdx].Font = new Font("Arial", 10);
                CurrentSeries.Points[RealIdx].ToolTip = ZFactorList[IdxValue].Name;
                CurrentSeries.Points[RealIdx].AxisLabel = ZFactorList[IdxValue].Name;

                SeriesLine.Points.Add(ZFactorList[IdxValue].AverageValue);
                SeriesLine.Points[RealIdx].BorderColor = Color.Black;
                SeriesLine.Points[RealIdx].MarkerStyle = MarkerStyle.Circle;
                SeriesLine.Points[RealIdx].MarkerSize = 4;
                RealIdx++;
            }

            SimpleForm NewWindow = new SimpleForm();
            int thisWidth = 200 * RealIdx;
            if (thisWidth > (int)GlobalInfo.OptionsWindow.numericUpDownMaximumWidth.Value)
                thisWidth = (int)GlobalInfo.OptionsWindow.numericUpDownMaximumWidth.Value;
            NewWindow.Width = thisWidth;
            NewWindow.Height = 400;
            NewWindow.Text = "Z-factors";

            ChartArea CurrentChartArea = new ChartArea();
            CurrentChartArea.BorderColor = Color.Black;
            CurrentChartArea.AxisX.Interval = 1;
            NewWindow.chartForSimpleForm.Series.Add(CurrentSeries);
            NewWindow.chartForSimpleForm.Series.Add(SeriesLine);

            CurrentChartArea.AxisX.IsLabelAutoFit = true;
            NewWindow.chartForSimpleForm.ChartAreas.Add(CurrentChartArea);

            CurrentChartArea.Axes[1].Maximum = 1.1;
            CurrentChartArea.Axes[1].IsMarksNextToAxis = true;
            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
            CurrentChartArea.Axes[1].MajorGrid.Enabled = false;

            NewWindow.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackGradientStyle = GradientStyle.TopBottom;
            CurrentChartArea.BackColor = CompleteScreening.GlobalInfo.OptionsWindow.panel1.BackColor;
            CurrentChartArea.BackSecondaryColor = Color.White;

            Title CurrentTitle = new Title(CompleteScreening.ListDescriptors[Desc].GetName() + " Z-factors");
            CurrentTitle.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            NewWindow.chartForSimpleForm.Titles.Add(CurrentTitle);

            return NewWindow;
        }

        private void sSMDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;
            BuildSSMD(this.comboBoxDescriptorToDisplay.SelectedIndex).Show();
        }


        private class cSimpleSignature
        {
            public cSimpleSignature(string Name, double Value)
            {
                this.Name = Name;
                this.AverageValue = Value;

            }


            public string Name;
            public double AverageValue;

        }

        private SimpleForm BuildSSMD(int Desc)
        {
            List<double> Pos = new List<double>();
            List<double> Neg = new List<double>();
            List<cSimpleSignature> ZFactorList = new List<cSimpleSignature>();

            cWell TempWell;
            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;

            // loop on all the plate
            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);

                Pos.Clear();
                Neg.Clear();

                for (int row = 0; row < CompleteScreening.Rows; row++)
                    for (int col = 0; col < CompleteScreening.Columns; col++)
                    {
                        TempWell = CurrentPlateToProcess.GetWell(col, row, true);
                        if (TempWell == null) continue;
                        else
                        {
                            if (TempWell.GetClass() == 0)
                                Pos.Add(TempWell.ListDescriptors[Desc].GetValue());
                            if (TempWell.GetClass() == 1)
                                Neg.Add(TempWell.ListDescriptors[Desc].GetValue());
                        }
                    }

                double SSMDScore = (Mean(Pos.ToArray()) - Mean(Neg.ToArray())) / Math.Sqrt(std(Pos.ToArray()) * std(Pos.ToArray()) + std(Neg.ToArray()) * std(Neg.ToArray()));
                GlobalInfo.ConsoleWriteLine(CurrentPlateToProcess.Name + ", SSMD = " + SSMDScore);

                //cDescriptor TmpDesc = new cDescriptor(SSMDScore, CurrentPlateToProcess.Name);
                cSimpleSignature TmpDesc = new cSimpleSignature(CurrentPlateToProcess.Name, SSMDScore);
                ZFactorList.Add(TmpDesc);
            }

            Series CurrentSeries = new Series();
            CurrentSeries.ChartType = SeriesChartType.Column;
            CurrentSeries.ShadowOffset = 1;

            Series SeriesLine = new Series();
            SeriesLine.Name = "SeriesLine";
            SeriesLine.ShadowOffset = 1;
            SeriesLine.ChartType = SeriesChartType.Line;

            int RealIdx = 0;
            for (int IdxValue = 0; IdxValue < ZFactorList.Count; IdxValue++)
            {
                if (ZFactorList[IdxValue].AverageValue.ToString() == "NaN") continue;

                CurrentSeries.Points.Add(ZFactorList[IdxValue].AverageValue);
                CurrentSeries.Points[RealIdx].Label = string.Format("{0:0.###}", ZFactorList[IdxValue].AverageValue);
                CurrentSeries.Points[RealIdx].Font = new Font("Arial", 10);
                CurrentSeries.Points[RealIdx].ToolTip = ZFactorList[IdxValue].Name;
                CurrentSeries.Points[RealIdx].AxisLabel = ZFactorList[IdxValue].Name;

                SeriesLine.Points.Add(ZFactorList[IdxValue].AverageValue);
                SeriesLine.Points[RealIdx].BorderColor = Color.Black;
                SeriesLine.Points[RealIdx].MarkerStyle = MarkerStyle.Circle;
                SeriesLine.Points[RealIdx].MarkerSize = 4;
                RealIdx++;
            }

            SimpleForm NewWindow = new SimpleForm();
            int thisWidth = 200 * RealIdx;
            if (thisWidth > (int)GlobalInfo.OptionsWindow.numericUpDownMaximumWidth.Value)
                thisWidth = (int)GlobalInfo.OptionsWindow.numericUpDownMaximumWidth.Value;
            NewWindow.Width = thisWidth;
            NewWindow.Height = 400;
            NewWindow.Text = "SSMD";

            ChartArea CurrentChartArea = new ChartArea();
            CurrentChartArea.BorderColor = Color.Black;
            CurrentChartArea.AxisX.Interval = 1;
            NewWindow.chartForSimpleForm.Series.Add(CurrentSeries);
            NewWindow.chartForSimpleForm.Series.Add(SeriesLine);

            CurrentChartArea.AxisX.IsLabelAutoFit = true;
            NewWindow.chartForSimpleForm.ChartAreas.Add(CurrentChartArea);

            // CurrentChartArea.Axes[1].Maximum = 2;
            CurrentChartArea.Axes[1].IsMarksNextToAxis = true;
            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
            CurrentChartArea.Axes[1].MajorGrid.Enabled = false;

            NewWindow.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackGradientStyle = GradientStyle.TopBottom;
            CurrentChartArea.BackColor = CompleteScreening.GlobalInfo.OptionsWindow.panel1.BackColor;
            CurrentChartArea.BackSecondaryColor = Color.White;

            Title CurrentTitle = new Title(CompleteScreening.ListDescriptors[Desc].GetName() + " SSMD");
            CurrentTitle.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            NewWindow.chartForSimpleForm.Titles.Add(CurrentTitle);

            return NewWindow;
        }

        private void coeffOfVariationEvolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            FormClassification WinForClass = new FormClassification(CompleteScreening);
            WinForClass.buttonClassification.Text = "Display";
            WinForClass.Text = "Select the class of interest";
            WinForClass.label1.Text = "Class";

            if (WinForClass.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            BuildCV(this.comboBoxDescriptorToDisplay.SelectedIndex, WinForClass.comboBoxForNeutralClass.SelectedIndex).Show();
        }

        private SimpleForm BuildCV(int Desc, int Class)
        {
            cExtendedList ListValue = new cExtendedList();
            //  List<double> Neg = new List<double>();
            List<cSimpleSignature> CVFactorList = new List<cSimpleSignature>();

            cWell TempWell;
            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;

            // loop on all the plate
            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);

                ListValue.Clear();


                for (int row = 0; row < CompleteScreening.Rows; row++)
                    for (int col = 0; col < CompleteScreening.Columns; col++)
                    {
                        TempWell = CurrentPlateToProcess.GetWell(col, row, true);
                        if (TempWell == null) continue;
                        else
                        {
                            if (TempWell.GetClass() == Class)
                                ListValue.Add(TempWell.ListDescriptors[Desc].GetValue());
                        }
                    }

                double CVScore = ListValue.Std() / ListValue.Mean();
                GlobalInfo.ConsoleWriteLine(CurrentPlateToProcess.Name + ", Coeff. of Variation = " + CVScore);
                cSimpleSignature TmpDesc = new cSimpleSignature(CurrentPlateToProcess.Name, CVScore);
                CVFactorList.Add(TmpDesc);
            }

            Series CurrentSeries = new Series();
            CurrentSeries.ChartType = SeriesChartType.Column;
            CurrentSeries.ShadowOffset = 1;

            Series SeriesLine = new Series();
            SeriesLine.Name = "SeriesLine";
            SeriesLine.ShadowOffset = 1;
            SeriesLine.ChartType = SeriesChartType.Line;

            int RealIdx = 0;
            for (int IdxValue = 0; IdxValue < CVFactorList.Count; IdxValue++)
            {
                if (CVFactorList[IdxValue].AverageValue.ToString() == "NaN") continue;

                CurrentSeries.Points.Add(CVFactorList[IdxValue].AverageValue);
                CurrentSeries.Points[RealIdx].Label = string.Format("{0:0.###}", CVFactorList[IdxValue].AverageValue);
                CurrentSeries.Points[RealIdx].Font = new Font("Arial", 10);
                CurrentSeries.Points[RealIdx].ToolTip = CVFactorList[IdxValue].Name;
                CurrentSeries.Points[RealIdx].AxisLabel = CVFactorList[IdxValue].Name;
                CurrentSeries.Points[RealIdx].Color = CompleteScreening.GlobalInfo.GetColor(Class);

                SeriesLine.Points.Add(CVFactorList[IdxValue].AverageValue);
                SeriesLine.Points[RealIdx].BorderColor = Color.Black;
                SeriesLine.Points[RealIdx].MarkerStyle = MarkerStyle.Circle;
                SeriesLine.Points[RealIdx].MarkerSize = 4;
                RealIdx++;
            }

            SimpleForm NewWindow = new SimpleForm();
            int thisWidth = 200 * RealIdx;
            if (thisWidth > (int)GlobalInfo.OptionsWindow.numericUpDownMaximumWidth.Value)
                thisWidth = (int)GlobalInfo.OptionsWindow.numericUpDownMaximumWidth.Value;
            NewWindow.Width = thisWidth;
            NewWindow.Height = 400;
            NewWindow.Text = "Coeff. of Variation";

            ChartArea CurrentChartArea = new ChartArea();
            CurrentChartArea.BorderColor = Color.Black;
            CurrentChartArea.AxisX.Interval = 1;
            NewWindow.chartForSimpleForm.Series.Add(CurrentSeries);
            NewWindow.chartForSimpleForm.Series.Add(SeriesLine);

            CurrentChartArea.AxisX.IsLabelAutoFit = true;
            NewWindow.chartForSimpleForm.ChartAreas.Add(CurrentChartArea);

            // CurrentChartArea.Axes[1].Maximum = 2;
            CurrentChartArea.Axes[1].IsMarksNextToAxis = true;
            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
            CurrentChartArea.Axes[1].MajorGrid.Enabled = false;

            NewWindow.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackGradientStyle = GradientStyle.TopBottom;
            CurrentChartArea.BackColor = CompleteScreening.GlobalInfo.OptionsWindow.panel1.BackColor;
            CurrentChartArea.BackSecondaryColor = Color.White;

            Title CurrentTitle = new Title(CompleteScreening.ListDescriptors[Desc].GetName() + " Coeff. of Variation");
            CurrentTitle.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            NewWindow.chartForSimpleForm.Titles.Add(CurrentTitle);

            return NewWindow;
        }
        #endregion

        #region Histograms section
        private void DisplayHistogram(bool IsFullScreen)
        {
            if (CompleteScreening == null) return;
            if ((CompleteScreening.ListDescriptors == null) || (CompleteScreening.ListDescriptors.Count == 0)) return;

            cExtendedList Pos = new cExtendedList();
            cWell TempWell;

            if (IsFullScreen == false)
            {
                for (int row = 0; row < CompleteScreening.Rows; row++)
                    for (int col = 0; col < CompleteScreening.Columns; col++)
                    {
                        TempWell = CompleteScreening.GetCurrentDisplayPlate().GetWell(col, row, false);
                        if (TempWell == null) continue;
                        else
                        {
                            if (TempWell.GetClass() == CompleteScreening.SelectedClass)
                                Pos.Add(TempWell.ListDescriptors[CompleteScreening.ListDescriptors.CurrentSelectedDescriptor].GetValue());
                        }
                    }
            }
            else
            {
                int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;

                // loop on all the plate
                for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
                {
                    cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);

                    for (int row = 0; row < CompleteScreening.Rows; row++)
                        for (int col = 0; col < CompleteScreening.Columns; col++)
                        {
                            TempWell = CurrentPlateToProcess.GetWell(col, row, false);
                            if (TempWell == null) continue;
                            else
                            {
                                if (TempWell.GetClass() == CompleteScreening.SelectedClass)
                                    Pos.Add(TempWell.ListDescriptors[CompleteScreening.ListDescriptors.CurrentSelectedDescriptor].GetValue());
                            }
                        }
                }
            }


            if (Pos.Count == 0)
            {
                MessageBox.Show("No well of class " + CompleteScreening.SelectedClass + " selected !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<double[]> HistoPos = CreateHistogram(Pos.ToArray(), (int)GlobalInfo.OptionsWindow.numericUpDownHistoBin.Value);
            SimpleForm NewWindow = new SimpleForm();

            Series SeriesPos = new Series();
            SeriesPos.ShadowOffset = 1;

            if (HistoPos.Count == 0) return;

            for (int IdxValue = 0; IdxValue < HistoPos[0].Length; IdxValue++)
            {
                SeriesPos.Points.AddXY(HistoPos[0][IdxValue], HistoPos[1][IdxValue]);
                SeriesPos.Points[IdxValue].ToolTip = HistoPos[1][IdxValue].ToString();
                if (CompleteScreening.SelectedClass == -1)
                    SeriesPos.Points[IdxValue].Color = Color.Black;
                else
                    SeriesPos.Points[IdxValue].Color = CompleteScreening.GlobalInfo.GetColor(CompleteScreening.SelectedClass);

            }

            ChartArea CurrentChartArea = new ChartArea();
            CurrentChartArea.BorderColor = Color.Black;

            NewWindow.chartForSimpleForm.ChartAreas.Add(CurrentChartArea);
            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
            CurrentChartArea.Axes[0].Title = CompleteScreening.ListDescriptors[CompleteScreening.ListDescriptors.CurrentSelectedDescriptor].GetName();
            CurrentChartArea.Axes[1].Title = "Sum";
            CurrentChartArea.AxisX.LabelStyle.Format = "N2";

            NewWindow.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackGradientStyle = GradientStyle.TopBottom;
            CurrentChartArea.BackColor = CompleteScreening.GlobalInfo.OptionsWindow.panel1.BackColor;
            CurrentChartArea.BackSecondaryColor = Color.White;

            SeriesPos.ChartType = SeriesChartType.Column;
            SeriesPos.Color = CompleteScreening.GlobalInfo.GetColor(1);
            NewWindow.chartForSimpleForm.Series.Add(SeriesPos);

            Series SeriesGaussNeg = new Series();
            SeriesGaussNeg.ChartType = SeriesChartType.Spline;

            Series SeriesGaussPos = new Series();
            SeriesGaussPos.ChartType = SeriesChartType.Spline;

            if (HistoPos.Count != 0)
            {
                double[] HistoGaussPos = CreateGauss(Mean(Pos.ToArray()), std(Pos.ToArray()), HistoPos[0].Length);

                SeriesGaussPos.Color = Color.Black;
                SeriesGaussPos.BorderWidth = 2;
            }
            SeriesGaussNeg.Color = Color.Black;
            SeriesGaussNeg.BorderWidth = 2;

            NewWindow.chartForSimpleForm.Series.Add(SeriesGaussNeg);
            NewWindow.chartForSimpleForm.Series.Add(SeriesGaussPos);
            NewWindow.chartForSimpleForm.ChartAreas[0].CursorX.IsUserEnabled = true;
            NewWindow.chartForSimpleForm.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            NewWindow.chartForSimpleForm.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            NewWindow.chartForSimpleForm.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            if (GlobalInfo.OptionsWindow.checkBoxDisplayHistoStats.Checked)
            {
                StripLine AverageLine = new StripLine();
                AverageLine.BackColor = Color.Black;
                AverageLine.IntervalOffset = Pos.Mean();
                AverageLine.StripWidth = double.Epsilon;
                CurrentChartArea.AxisX.StripLines.Add(AverageLine);
                AverageLine.Text = String.Format("{0:0.###}", AverageLine.IntervalOffset);

                StripLine StdLine = new StripLine();
                StdLine.BackColor = Color.FromArgb(64, Color.Black);
                double Std = Pos.Std();
                StdLine.IntervalOffset = AverageLine.IntervalOffset - 0.5 * Std;
                StdLine.StripWidth = Std;
                CurrentChartArea.AxisX.StripLines.Add(StdLine);
                AverageLine.StripWidth = 0.0001;
            }

            Title CurrentTitle = null;

            if (IsFullScreen)
                CurrentTitle = new Title("Class " + CompleteScreening.SelectedClass + " - " + CompleteScreening.ListDescriptors[CompleteScreening.ListDescriptors.CurrentSelectedDescriptor].GetName() + " histogram.");
            else
                CurrentTitle = new Title("Class " + CompleteScreening.SelectedClass + " - " + CompleteScreening.GetCurrentDisplayPlate().Name + " - " + CompleteScreening.ListDescriptors[CompleteScreening.ListDescriptors.CurrentSelectedDescriptor].GetName() + " histogram.");

            CurrentTitle.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            NewWindow.chartForSimpleForm.Titles.Add(CurrentTitle);
            NewWindow.Text = CurrentTitle.Text;
            NewWindow.Show();
            NewWindow.chartForSimpleForm.Update();
            NewWindow.chartForSimpleForm.Show();
            NewWindow.Controls.AddRange(new System.Windows.Forms.Control[] { NewWindow.chartForSimpleForm });
            return;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayHistogram(true);
        }


        private void stackedHistogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;
            if ((CompleteScreening.ListDescriptors == null) || (CompleteScreening.ListDescriptors.Count == 0)) return;

            cExtendedList[] ListValuesForHisto = new cExtendedList[GlobalInfo.GetNumberofDefinedClass()];
            for (int i = 0; i < ListValuesForHisto.Length; i++)
                ListValuesForHisto[i] = new cExtendedList();

            cWell TempWell;

            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;

            double MinValue = double.MaxValue;
            double MaxValue = double.MinValue;
            double CurrentValue;

            // loop on all the plate
            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);

                for (int row = 0; row < CompleteScreening.Rows; row++)
                    for (int col = 0; col < CompleteScreening.Columns; col++)
                    {
                        TempWell = CurrentPlateToProcess.GetWell(col, row, false);
                        if (TempWell == null) continue;
                        else
                        {
                            if (TempWell.GetClass() >= 0)
                            {
                                CurrentValue = TempWell.ListDescriptors[CompleteScreening.ListDescriptors.CurrentSelectedDescriptor].GetValue();
                                ListValuesForHisto[TempWell.GetClass()].Add(CurrentValue);
                                if (CurrentValue < MinValue) MinValue = CurrentValue;
                                if (CurrentValue > MaxValue) MaxValue = CurrentValue;
                            }
                        }
                    }
            }
            SimpleForm NewWindow = new SimpleForm();
            List<double[]>[] HistoPos = new List<double[]>[ListValuesForHisto.Length];
            Series[] SeriesPos = new Series[GlobalInfo.GetNumberofDefinedClass()];


            for (int i = 0; i < ListValuesForHisto.Length; i++)
            {
                HistoPos[i] = new List<double[]>();
                HistoPos[i] = ListValuesForHisto[i].CreateHistogram(MinValue, MaxValue, (int)GlobalInfo.OptionsWindow.numericUpDownHistoBin.Value);

                SeriesPos[i] = new Series();
            }

            for (int i = 0; i < SeriesPos.Length; i++)
            {
                int Max = 0;
                if (HistoPos[i].Count > 0)
                    Max = HistoPos[i][0].Length;

                for (int IdxValue = 0; IdxValue < Max; IdxValue++)
                {
                    SeriesPos[i].Points.AddXY(MinValue + ((MaxValue - MinValue) * IdxValue) / Max, HistoPos[i][1][IdxValue]);
                    SeriesPos[i].Points[IdxValue].ToolTip = HistoPos[i][1][IdxValue].ToString();
                    if (CompleteScreening.SelectedClass == -1)
                        SeriesPos[i].Points[IdxValue].Color = Color.Black;
                    else
                        SeriesPos[i].Points[IdxValue].Color = CompleteScreening.GlobalInfo.GetColor(i);

                }
            }
            ChartArea CurrentChartArea = new ChartArea();
            CurrentChartArea.BorderColor = Color.Black;

            NewWindow.chartForSimpleForm.ChartAreas.Add(CurrentChartArea);
            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
            CurrentChartArea.Axes[0].Title = CompleteScreening.ListDescriptors[CompleteScreening.ListDescriptors.CurrentSelectedDescriptor].GetName();
            CurrentChartArea.Axes[1].Title = "Sum";
            CurrentChartArea.AxisX.LabelStyle.Format = "N2";

            NewWindow.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackGradientStyle = GradientStyle.TopBottom;
            CurrentChartArea.BackColor = CompleteScreening.GlobalInfo.OptionsWindow.panel1.BackColor;
            CurrentChartArea.BackSecondaryColor = Color.White;


            for (int i = 0; i < SeriesPos.Length; i++)
            {
                SeriesPos[i].ChartType = SeriesChartType.StackedColumn;
                // SeriesPos[i].Color = CompleteScreening.GlobalInfo.GetColor(1);
                NewWindow.chartForSimpleForm.Series.Add(SeriesPos[i]);
            }
            //Series SeriesGaussNeg = new Series();
            //SeriesGaussNeg.ChartType = SeriesChartType.Spline;

            //Series SeriesGaussPos = new Series();
            //SeriesGaussPos.ChartType = SeriesChartType.Spline;

            //if (HistoPos.Count != 0)
            //{
            //    double[] HistoGaussPos = CreateGauss(Mean(Pos.ToArray()), std(Pos.ToArray()), HistoPos[0].Length);

            //    SeriesGaussPos.Color = Color.Black;
            //    SeriesGaussPos.BorderWidth = 2;
            //}
            //SeriesGaussNeg.Color = Color.Black;
            //SeriesGaussNeg.BorderWidth = 2;

            //NewWindow.chartForSimpleForm.Series.Add(SeriesGaussNeg);
            //NewWindow.chartForSimpleForm.Series.Add(SeriesGaussPos);
            NewWindow.chartForSimpleForm.ChartAreas[0].CursorX.IsUserEnabled = true;
            NewWindow.chartForSimpleForm.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            NewWindow.chartForSimpleForm.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            NewWindow.chartForSimpleForm.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            Title CurrentTitle = null;

            CurrentTitle = new Title(CompleteScreening.GetCurrentDisplayPlate().Name + " - " + CompleteScreening.ListDescriptors[CompleteScreening.ListDescriptors.CurrentSelectedDescriptor].GetName() + " Stacked histogram.");

            CurrentTitle.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            NewWindow.chartForSimpleForm.Titles.Add(CurrentTitle);
            NewWindow.Text = CurrentTitle.Text;
            NewWindow.Show();
            NewWindow.chartForSimpleForm.Update();
            NewWindow.chartForSimpleForm.Show();
            NewWindow.Controls.AddRange(new System.Windows.Forms.Control[] { NewWindow.chartForSimpleForm });
            return;
        }
        #endregion

        #region PCA
        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cExtendPlateList ListToProcess = new cExtendPlateList();
            ListToProcess.Add(CompleteScreening.GetCurrentDisplayPlate());
            ComputeAndDisplayPCA(ListToProcess);
        }

        private void pCAToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ComputeAndDisplayPCA(CompleteScreening.ListPlatesActive);
        }

        private void ComputeAndDisplayPCA(cExtendPlateList PlatesToProcess)
        {
            if (CompleteScreening == null) return;
            FormClassification WindowClassification = new FormClassification(CompleteScreening);
            WindowClassification.label1.Text = "Class of interest";
            WindowClassification.Text = "PCA";
            WindowClassification.buttonClassification.Text = "Process";

            if (WindowClassification.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            int NeutralClass = WindowClassification.comboBoxForNeutralClass.SelectedIndex;

            int NumWell = 0;
            int NumWellForLearning = 0;
            foreach (cPlate CurrentPlate in PlatesToProcess)
            {
                foreach (cWell CurrentWell in CurrentPlate.ListActiveWells)
                {
                    if (CurrentWell.GetClass() == NeutralClass)
                        NumWellForLearning++;
                }
                NumWell += CompleteScreening.GetCurrentDisplayPlate().GetNumberOfActiveWells();
            }

            if (NumWellForLearning == 0)
            {
                MessageBox.Show("No well of the selected class identified", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            int NumDesc = CompleteScreening.GetNumberOfActiveDescriptor();

            if (NumDesc <= 1)
            {
                MessageBox.Show("More than one descriptor are required for this operation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double[,] DataForPCA = new double[NumWellForLearning, CompleteScreening.GetNumberOfActiveDescriptor() + 1];

            //   return;
            Matrix EigenVectors = PCAComputation(DataForPCA, NumWellForLearning, NumWell, NumDesc, NeutralClass, PlatesToProcess);
            if (EigenVectors == null) return;

            SimpleForm NewWindow = new SimpleForm();
            Series CurrentSeries = new Series();
            CurrentSeries.ShadowOffset = 1;

            Matrix CurrentPt = new Matrix(NumWell, NumDesc);
            DataForPCA = new double[NumWell, NumDesc + 1];

            for (int desc = 0; desc < NumDesc; desc++)
            {
                if (CompleteScreening.ListDescriptors[desc].IsActive() == false) continue;
                List<double> CurrentDesc = new List<double>();
                foreach (cPlate CurrentPlate in PlatesToProcess)
                {
                    for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                        for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                        {
                            cWell TmpWell = CurrentPlate.GetWell(IdxValue, IdxValue0, true);
                            if (TmpWell == null) continue;
                            CurrentDesc.Add(TmpWell.ListDescriptors[desc].GetValue());
                        }
                }
                for (int i = 0; i < NumWell; i++)
                    DataForPCA[i, desc] = CurrentDesc[i];
            }

            int IDx = 0;
            foreach (cPlate CurrentPlate in PlatesToProcess)
            {
                for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                    for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                    {
                        cWell TmpWell = CurrentPlate.GetWell(IdxValue, IdxValue0, true);
                        if (TmpWell == null) continue;
                        DataForPCA[IDx++, NumDesc] = TmpWell.GetClass();
                    }
            }

            for (int i = 0; i < NumWell; i++)
                for (int j = 0; j < NumDesc; j++) CurrentPt.addElement(i, j, DataForPCA[i, j]);

            Matrix NewPt = new Matrix(NumWell, NumDesc);

            NewPt = CurrentPt.multiply(EigenVectors);

            double MinY = double.MaxValue, MaxY = double.MinValue;

            for (int IdxValue0 = 0; IdxValue0 < NumWell; IdxValue0++)
            {
                double CurrentY = NewPt.getElement(IdxValue0, 1);

                if (CurrentY < MinY) MinY = CurrentY;
                if (CurrentY > MaxY) MaxY = CurrentY;

                CurrentSeries.Points.AddXY(NewPt.getElement(IdxValue0, 0), CurrentY);

                CurrentSeries.Points[IdxValue0].Color = CompleteScreening.GlobalInfo.GetColor((int)DataForPCA[IdxValue0, NumDesc]);
                CurrentSeries.Points[IdxValue0].MarkerStyle = MarkerStyle.Circle;
                CurrentSeries.Points[IdxValue0].MarkerSize = 8;
            }

            ChartArea CurrentChartArea = new ChartArea();
            CurrentChartArea.BorderColor = Color.Black;

            NewWindow.chartForSimpleForm.ChartAreas.Add(CurrentChartArea);
            NewWindow.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackColor = Color.FromArgb(164, 164, 164);

            string AxeName = "";
            int IDxDesc = 0;
            for (int Desc = 0; Desc < CompleteScreening.ListDescriptors.Count; Desc++)
            {
                if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;
                AxeName += String.Format("{0:0.###}", EigenVectors.getElement(IDxDesc++, 0)) + "x" + CompleteScreening.ListDescriptors[Desc].GetName() + " + ";
                //   AxeName += String.Format("{0:0.##}", EigenVectors.getElement(CompleteScreening.ListDescriptors.Count - 1, 0)) + "x" + CompleteScreening.ListDescriptorName[CompleteScreening.ListDescriptors.Count - 1];
            }
            CurrentChartArea.Axes[0].Title = AxeName.Remove(AxeName.Length - 3);
            CurrentChartArea.Axes[0].MajorGrid.Enabled = true;

            AxeName = "";
            IDxDesc = 0;
            for (int Desc = 0; Desc < CompleteScreening.ListDescriptors.Count; Desc++)
            {
                if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;
                AxeName += String.Format("{0:0.###}", EigenVectors.getElement(IDxDesc++, 1)) + "x" + CompleteScreening.ListDescriptors[Desc].GetName() + " + ";
            }
            //AxeName += String.Format("{0:0.##}", EigenVectors.getElement(CompleteScreening.ListDescriptors.Count - 1, 0)) + "x" + CompleteScreening.ListDescriptorName[CompleteScreening.ListDescriptors.Count - 1];

            CurrentChartArea.Axes[1].Title = AxeName.Remove(AxeName.Length - 3);
            CurrentChartArea.Axes[1].MajorGrid.Enabled = true;
            CurrentChartArea.Axes[1].Minimum = MinY;
            CurrentChartArea.Axes[1].Maximum = MaxY;
            CurrentChartArea.AxisX.LabelStyle.Format = "N2";
            CurrentChartArea.AxisY.LabelStyle.Format = "N2";


            CurrentSeries.ChartType = SeriesChartType.Point;
            if (GlobalInfo.OptionsWindow.checkBoxDisplayFastPerformance.Checked) CurrentSeries.ChartType = SeriesChartType.FastPoint;
            NewWindow.chartForSimpleForm.Series.Add(CurrentSeries);


            NewWindow.Text = "PCA";
            NewWindow.Show();
            NewWindow.chartForSimpleForm.Update();
            NewWindow.chartForSimpleForm.Show();
            NewWindow.Controls.AddRange(new System.Windows.Forms.Control[] { NewWindow.chartForSimpleForm });


        }

        private Matrix PCAComputation(double[,] DataForPCA, int NumWellForLearning, int NumWell, int NumDesc, int NeutralClass, cExtendPlateList PlatesToProcess)
        {

            for (int desc = 0; desc < NumDesc; desc++)
            {
                if (CompleteScreening.ListDescriptors[desc].IsActive() == false) continue;
                List<double> CurrentDesc = new List<double>();

                foreach (cPlate CurrentPlate in PlatesToProcess)
                {
                    for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                        for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                        {
                            cWell TmpWell = CurrentPlate.GetWell(IdxValue, IdxValue0, true);
                            if ((TmpWell == null) || (TmpWell.GetClass() != NeutralClass)) continue;
                            CurrentDesc.Add(TmpWell.ListDescriptors[desc].GetValue());
                        }
                }
                for (int i = 0; i < NumWellForLearning; i++)
                {
                    DataForPCA[i, desc] = CurrentDesc[i];
                }
            }
            int IDx = 0;
            foreach (cPlate CurrentPlate in PlatesToProcess)
            {
                for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                    for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                    {
                        cWell TmpWell = CurrentPlate.GetWell(IdxValue, IdxValue0, true);
                        if ((TmpWell == null) || (TmpWell.GetClass() == NeutralClass)) continue;
                        DataForPCA[IDx++, NumDesc] = TmpWell.GetClass();
                    }
            }

            double[,] Basis;
            double[] s2;
            int Info;

            alglib.pcabuildbasis(DataForPCA, NumWellForLearning, NumDesc, out Info, out s2, out Basis);

            Matrix EigenVectors = null;
            if (Info > 0)
            {
                EigenVectors = new Matrix(NumDesc, NumDesc);
                for (int row = 0; row < NumDesc; row++)
                    for (int col = 0; col < NumDesc; col++)
                        EigenVectors.addElement(row, col, Basis[row, col]);
            }
            return EigenVectors;
        }
        #endregion

        #region LDA
        /// <summary>
        /// LDA for the current plate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lDAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cExtendPlateList ListToProcess = new cExtendPlateList();
            ListToProcess.Add(CompleteScreening.GetCurrentDisplayPlate());
            ComputeAndDisplayLDA(ListToProcess);
        }

        /// <summary>
        /// start entire screen LDA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lDAToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ComputeAndDisplayLDA(CompleteScreening.ListPlatesActive);
        }

        private Matrix LDAComputation(double[,] DataForLDA, int NumWellForLearning, int NumWell, int NumDesc, int NeutralClass, cExtendPlateList PlatesToProcess)
        {
            int Info;
            for (int desc = 0; desc < NumDesc; desc++)
            {
                if (CompleteScreening.ListDescriptors[desc].IsActive() == false) continue;
                List<double> CurrentDesc = new List<double>();

                foreach (cPlate CurrentPlate in PlatesToProcess)
                {
                    for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                        for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                        {
                            cWell TmpWell = CurrentPlate.GetWell(IdxValue, IdxValue0, true);
                            if ((TmpWell == null) || (TmpWell.GetClass() == NeutralClass)) continue;
                            CurrentDesc.Add(TmpWell.ListDescriptors[desc].GetValue());
                        }
                }
                for (int i = 0; i < NumWellForLearning; i++)
                {
                    DataForLDA[i, desc] = CurrentDesc[i];
                }
            }
            int IDx = 0;
            foreach (cPlate CurrentPlate in PlatesToProcess)
            {
                for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                    for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                    {
                        cWell TmpWell = CurrentPlate.GetWell(IdxValue, IdxValue0, true);
                        if ((TmpWell == null) || (TmpWell.GetClass() == NeutralClass)) continue;
                        DataForLDA[IDx++, NumDesc] = TmpWell.GetClass();
                    }
            }
            double[,] Basis;

            //alglib.pcabuildbasis(DataForLDA, NumWellForLearning, NumWellForLearning, out Info, out Basis);
            alglib.fisherldan(DataForLDA, NumWellForLearning, NumDesc, NumWellForLearning, out Info, out Basis);
            Matrix EigenVectors = null;
            if (Info > 0)
            {
                EigenVectors = new Matrix(NumDesc, NumDesc);
                for (int row = 0; row < NumDesc; row++)
                    for (int col = 0; col < NumDesc; col++)
                        EigenVectors.addElement(row, col, Basis[row, col]);
            }
            return EigenVectors;
        }

        private void ComputeAndDisplayLDA(cExtendPlateList PlatesToProcess)
        {
            if (CompleteScreening == null) return;
            FormClassification WindowClassification = new FormClassification(CompleteScreening);
            WindowClassification.buttonClassification.Text = "Process";
            WindowClassification.Text = "LDA";
            if (WindowClassification.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            int NeutralClass = WindowClassification.comboBoxForNeutralClass.SelectedIndex;

            int NumWell = 0;
            int NumWellForLearning = 0;
            foreach (cPlate CurrentPlate in PlatesToProcess)
            {
                NumWellForLearning += CurrentPlate.GetNumberOfActiveWellsButClass(NeutralClass);
                NumWell += CompleteScreening.GetCurrentDisplayPlate().GetNumberOfActiveWells();
            }

            if (NumWellForLearning == 0)
            {
                MessageBox.Show("No well identified !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int NumDesc = CompleteScreening.GetNumberOfActiveDescriptor();

            if (NumDesc <= 1)
            {
                MessageBox.Show("More than one descriptor are required for this operation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double[,] DataForLDA = new double[NumWellForLearning, CompleteScreening.GetNumberOfActiveDescriptor() + 1];

            //   return;
            Matrix EigenVectors = LDAComputation(DataForLDA, NumWellForLearning, NumWell, NumDesc, NeutralClass, PlatesToProcess);
            if (EigenVectors == null) return;

            SimpleForm NewWindow = new SimpleForm();
            Series CurrentSeries = new Series();
            CurrentSeries.ShadowOffset = 1;

            Matrix CurrentPt = new Matrix(NumWell, NumDesc);
            DataForLDA = new double[NumWell, NumDesc + 1];

            for (int desc = 0; desc < NumDesc; desc++)
            {
                if (CompleteScreening.ListDescriptors[desc].IsActive() == false) continue;
                List<double> CurrentDesc = new List<double>();
                foreach (cPlate CurrentPlate in PlatesToProcess)
                {
                    for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                        for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                        {
                            cWell TmpWell = CurrentPlate.GetWell(IdxValue, IdxValue0, true);
                            if (TmpWell == null) continue;
                            CurrentDesc.Add(TmpWell.ListDescriptors[desc].GetValue());
                        }
                }
                for (int i = 0; i < NumWell; i++)
                    DataForLDA[i, desc] = CurrentDesc[i];
            }

            int IDx = 0;
            foreach (cPlate CurrentPlate in PlatesToProcess)
            {
                for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                    for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                    {
                        cWell TmpWell = CurrentPlate.GetWell(IdxValue, IdxValue0, true);
                        if (TmpWell == null) continue;
                        DataForLDA[IDx++, NumDesc] = TmpWell.GetClass();
                    }
            }

            for (int i = 0; i < NumWell; i++)
                for (int j = 0; j < NumDesc; j++) CurrentPt.addElement(i, j, DataForLDA[i, j]);

            Matrix NewPt = new Matrix(NumWell, NumDesc);

            NewPt = CurrentPt.multiply(EigenVectors);

            double MinY = double.MaxValue, MaxY = double.MinValue;

            for (int IdxValue0 = 0; IdxValue0 < NumWell; IdxValue0++)
            {
                double CurrentY = NewPt.getElement(IdxValue0, 1);

                if (CurrentY < MinY) MinY = CurrentY;
                if (CurrentY > MaxY) MaxY = CurrentY;

                CurrentSeries.Points.AddXY(NewPt.getElement(IdxValue0, 0), CurrentY);

                CurrentSeries.Points[IdxValue0].Color = CompleteScreening.GlobalInfo.GetColor((int)DataForLDA[IdxValue0, NumDesc]);
                CurrentSeries.Points[IdxValue0].MarkerStyle = MarkerStyle.Circle;
                CurrentSeries.Points[IdxValue0].MarkerSize = 8;
            }

            ChartArea CurrentChartArea = new ChartArea();
            CurrentChartArea.BorderColor = Color.Black;

            NewWindow.chartForSimpleForm.ChartAreas.Add(CurrentChartArea);
            NewWindow.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackColor = Color.FromArgb(164, 164, 164);

            string AxeName = "";
            int IDxDesc = 0;
            for (int Desc = 0; Desc < CompleteScreening.ListDescriptors.Count; Desc++)
            {
                if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;
                AxeName += String.Format("{0:0.###}", EigenVectors.getElement(IDxDesc++, 0)) + "x" + CompleteScreening.ListDescriptors[Desc].GetName() + " + ";
                //   AxeName += String.Format("{0:0.##}", EigenVectors.getElement(CompleteScreening.ListDescriptors.Count - 1, 0)) + "x" + CompleteScreening.ListDescriptorName[CompleteScreening.ListDescriptors.Count - 1];
            }
            CurrentChartArea.Axes[0].Title = AxeName.Remove(AxeName.Length - 3);
            CurrentChartArea.Axes[0].MajorGrid.Enabled = true;

            AxeName = "";
            IDxDesc = 0;
            for (int Desc = 0; Desc < CompleteScreening.ListDescriptors.Count; Desc++)
            {
                if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;
                AxeName += String.Format("{0:0.###}", EigenVectors.getElement(IDxDesc++, 1)) + "x" + CompleteScreening.ListDescriptors[Desc].GetName() + " + ";
            }
            //AxeName += String.Format("{0:0.##}", EigenVectors.getElement(CompleteScreening.ListDescriptors.Count - 1, 0)) + "x" + CompleteScreening.ListDescriptorName[CompleteScreening.ListDescriptors.Count - 1];

            CurrentChartArea.Axes[1].Title = AxeName.Remove(AxeName.Length - 3);
            CurrentChartArea.Axes[1].MajorGrid.Enabled = true;
            CurrentChartArea.Axes[1].Minimum = MinY;
            CurrentChartArea.Axes[1].Maximum = MaxY;
            CurrentChartArea.AxisX.LabelStyle.Format = "N2";
            CurrentChartArea.AxisY.LabelStyle.Format = "N2";


            CurrentSeries.ChartType = SeriesChartType.Point;
            if (GlobalInfo.OptionsWindow.checkBoxDisplayFastPerformance.Checked) CurrentSeries.ChartType = SeriesChartType.FastPoint;

            NewWindow.chartForSimpleForm.Series.Add(CurrentSeries);


            NewWindow.Text = "LDA";
            NewWindow.Show();
            NewWindow.chartForSimpleForm.Update();
            NewWindow.chartForSimpleForm.Show();
            NewWindow.Controls.AddRange(new System.Windows.Forms.Control[] { NewWindow.chartForSimpleForm });


        }
        #endregion

        #region Genes Analysis
        public class cPathWay
        {
            public string Name;
            public int Occurence = 0;

        }

        private void findGeneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;
            FormForNameRequest FormForRequest = new FormForNameRequest();
            if (FormForRequest.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;

            // loop on all the plate
            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);

                for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                    for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                    {
                        cWell TmpWell = CurrentPlateToProcess.GetWell(IdxValue, IdxValue0, true);
                        if (TmpWell == null) continue;

                        if (TmpWell.Name == FormForRequest.textBoxForName.Text)
                        {

                            CurrentPlateToProcess.DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);
                            int Col = IdxValue + 1;
                            int row = IdxValue0 + 1;
                            MessageBox.Show("Column " + Col + " x Row " + row, TmpWell.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
            }

            MessageBox.Show("Gene not found !", FormForRequest.textBoxForName.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private FormForPie PathWayAnalysis(int Class)
        {
            //int Idx = 0;
            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;

            KEGG ServKegg = new KEGG();

            List<cPathWay> ListPathway = new List<cPathWay>();

            // loop on all the plate
            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);

                for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                    for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                    {
                        cWell TmpWell = CurrentPlateToProcess.GetWell(IdxValue, IdxValue0, true);
                        if ((TmpWell == null) || (TmpWell.GetClass() != Class) || (TmpWell.LocusID == -1)) continue;


                        string[] intersection_gene_pathways = new string[1];
                        intersection_gene_pathways[0] = "hsa:" + TmpWell.LocusID;
                        string[] Pathways = ServKegg.get_pathways_by_genes(intersection_gene_pathways);
                        if ((Pathways == null) || (Pathways.Length == 0)) continue;

                        for (int Idx = 0; Idx < Pathways.Length; Idx++)
                        {
                            //  string PathName = Pathways[Idx].Remove(0, 8);
                            string GenInfo = ServKegg.bget(Pathways[Idx]);
                            string[] Genes = GenInfo.Split(new char[] { '\n' });
                            string PathName = "";
                            foreach (string item in Genes)
                            {
                                string[] fre = item.Split(' ');
                                string[] STRsection = fre[0].Split('_');

                                if (STRsection[0] == "NAME")
                                {
                                    for (int i = 1; i < fre.Length; i++)
                                    {
                                        if (fre[i] == "") continue;
                                        PathName += fre[i] + " ";
                                    }
                                    break;
                                }
                            }

                            if (ListPathway.Count == 0)
                            {
                                cPathWay CurrPath = new cPathWay();
                                CurrPath.Name = PathName;
                                CurrPath.Occurence = 1;
                                ListPathway.Add(CurrPath);
                                continue;
                            }

                            bool DidIt = false;
                            for (int i = 0; i < ListPathway.Count; i++)
                            {
                                if (PathName == ListPathway[i].Name)
                                {
                                    ListPathway[i].Occurence++;
                                    DidIt = true;
                                    break;
                                }
                            }

                            if (DidIt == false)
                            {
                                cPathWay CurrPath1 = new cPathWay();
                                CurrPath1.Name = PathName;
                                CurrPath1.Occurence = 1;
                                ListPathway.Add(CurrPath1);
                            }
                        }
                    }
            }

            // now draw the pie
            if (ListPathway.Count == 0)
            {
                MessageBox.Show("No pathway identified !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;

            }
            FormForPie Pie = new FormForPie();

            Series CurrentSeries = Pie.chartForPie.Series[0];

            // loop on all the plate
            int MaxOccurence = int.MinValue;
            int MaxIdx = 0;
            int TotalOcurrence = 0;
            for (int Idx = 0; Idx < ListPathway.Count; Idx++)
            {
                if (ListPathway[Idx].Occurence > MaxOccurence)
                {
                    MaxOccurence = ListPathway[Idx].Occurence;
                    MaxIdx = Idx;
                }
                TotalOcurrence += ListPathway[Idx].Occurence;
            }



            //CurrentSeries.CustomProperties = "PieLabelStyle=Outside";
            for (int Idx = 0; Idx < ListPathway.Count; Idx++)
            {
                CurrentSeries.Points.Add(ListPathway[Idx].Occurence);
                CurrentSeries.Points[Idx].Label = String.Format("{0:0.###}", ((100.0 * ListPathway[Idx].Occurence) / TotalOcurrence)) + " %";

                CurrentSeries.Points[Idx].LegendText = ListPathway[Idx].Name;
                CurrentSeries.Points[Idx].ToolTip = ListPathway[Idx].Name;
                if (Idx == MaxIdx)
                    CurrentSeries.Points[Idx].SetCustomProperty("Exploded", "True");
            }

            return Pie;
        }

        private void pahtwaysAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            this.Cursor = Cursors.WaitCursor;
            FormForPie CurrentFormForPie = PathWayAnalysis(CompleteScreening.SelectedClass);
            this.Cursor = Cursors.Default;
            if (CurrentFormForPie != null) CurrentFormForPie.Show();
        }
        #endregion

        #region Systematic Error Identification
        private List<string> ComputePlateBasedClassification(int Classes, int MinObjectsNumber, cPlate CurrentPlateToProcess)
        {
            CurrentPlateToProcess.ComputePlateBasedDescriptors();
            weka.core.Instances insts = CurrentPlateToProcess.CreateInstancesWithClassesWithPlateBasedDescriptor(Classes);
            weka.classifiers.trees.J48 ClassificationModel = new weka.classifiers.trees.J48();
            ClassificationModel.setMinNumObj(MinObjectsNumber);

            weka.core.Instances train = new weka.core.Instances(insts, 0, insts.numInstances());
            ClassificationModel.buildClassifier(train);

            // display the tree
            string DotString = ClassificationModel.graph().Remove(0, ClassificationModel.graph().IndexOf("{") + 2);
            int DotLenght = DotString.Length;

            string NewDotString = DotString.Remove(DotLenght - 3, 3);
            ComputeAndDisplayGraph(NewDotString);
            return ComputeGraph(NewDotString, Classes);
        }

        private List<string> ComputeGraph(string DotString, int Classes)
        {
            List<string> ToReturn = new List<string>();

            int CurrentPos = 0;
            int NextReturnPos = CurrentPos;
            List<int> ListNodeId = new List<int>();
            string TmpDotString = DotString;

            int TmpClass = 0;
            string ErrorString = "";
            int ErrorMessage = 0;

            ToReturn.Add(ErrorString);

            ToReturn.Add("");   // edge
            ToReturn.Add("");   // col
            ToReturn.Add("");   // row
            ToReturn.Add("");   // bowl


            while (NextReturnPos != -1)
            {
                int NextBracket = DotString.IndexOf("[");
                string StringToProcess = DotString.Remove(NextBracket - 1);
                string StringToProcess1 = StringToProcess.Remove(0, 1);

                if (StringToProcess1.Contains("N") == false)
                {
                    int Id = Convert.ToInt32(StringToProcess1);


                    int LabelPos = DotString.IndexOf("label=\"");
                    string LabelString = DotString.Remove(0, LabelPos + 7);
                    LabelPos = LabelString.IndexOf("\"");
                    string FinalLabel = LabelString.Remove(LabelPos);

                    if (TmpClass < Classes)
                    {
                        if ((FinalLabel == "Dist_To_Border") || (FinalLabel == "Col_Pos") || (FinalLabel == "Row_Pos") || (FinalLabel == "Dist_To_Center"))
                        {
                            if ((FinalLabel == "Dist_To_Border") && (!ErrorString.Contains(" an edge effect")) && (!ErrorString.Contains(" a bowl effect")) && (ErrorMessage < 2))
                            {
                                if (TmpClass > 0) ErrorString += " combined with";
                                ErrorString += " an " + CompleteScreening.GlobalInfo.ListArtifacts[0];
                                ErrorMessage++;
                                ToReturn[1] = "X";
                            }
                            else if ((FinalLabel == "Col_Pos") && (!ErrorString.Contains(" a column artifact")) && (ErrorMessage < 2))
                            {
                                if (TmpClass > 0) ErrorString += " combined with";
                                ErrorString += " a " + CompleteScreening.GlobalInfo.ListArtifacts[1];
                                ErrorMessage++;
                                ToReturn[2] = "X";

                            }
                            else if ((FinalLabel == "Row_Pos") && (!ErrorString.Contains(" a row artifact")) && (ErrorMessage < 2))
                            {
                                if (TmpClass > 0) ErrorString += " combined with";
                                ErrorString += " a " + CompleteScreening.GlobalInfo.ListArtifacts[2];
                                ErrorMessage++;
                                ToReturn[3] = "X";

                            }
                            else if ((FinalLabel == "Dist_To_Center") && (!ErrorString.Contains(" a bowl effect")) && (!ErrorString.Contains(" an edge effect")) && (ErrorMessage < 2))
                            {
                                if (TmpClass > 0) ErrorString += " combined with";
                                ErrorString += " a " + CompleteScreening.GlobalInfo.ListArtifacts[3];
                                ErrorMessage++;
                                ToReturn[4] = "X";

                            }
                            TmpClass++;
                        }
                    }
                }

                NextReturnPos = DotString.IndexOf("\n");
                if (NextReturnPos != -1)
                {
                    string TmpString = DotString.Remove(0, NextReturnPos + 1);
                    DotString = TmpString;
                }
            }

            if (TmpClass == 0)
            {
                string NoError = "No systematic error detected !";
                ToReturn.Add(NoError);
                return ToReturn;
            }

            string FinalString = "You have a systematic error !\nThis is " + ErrorString;

            DotString = TmpDotString;
            NextReturnPos = 0;
            while (NextReturnPos != -1)
            {
                int NextBracket = DotString.IndexOf("[");
                string StringToProcess = DotString.Remove(NextBracket - 1);
                string StringToProcess1 = StringToProcess.Remove(0, 1);

                if (StringToProcess1.Contains("N"))
                {
                    //// this is an edge
                    string stringNodeIdxStart = StringToProcess1.Remove(StringToProcess1.IndexOf("-"));
                    int NodeIdxStart = Convert.ToInt32(stringNodeIdxStart);

                    string stringNodeIdxEnd = StringToProcess1.Remove(0, StringToProcess1.IndexOf("N") + 1);
                    int NodeIdxSEnd = Convert.ToInt32(stringNodeIdxEnd);

                    int LabelPos = DotString.IndexOf("label=");
                    LabelPos += 7;

                    string CurrLabelString = DotString.Remove(0, LabelPos);
                }
                NextReturnPos = DotString.IndexOf("\n");

                if (NextReturnPos != -1)
                {
                    string TmpString = DotString.Remove(0, NextReturnPos + 1);
                    DotString = TmpString;
                }
            }

            ToReturn[0] = FinalString + ".";

            return ToReturn;


        }

        private List<string> GenerateArtifactMessage(cPlate PlateToProcess, int CurrentDescSel)
        {
            int NumWell = PlateToProcess.GetNumberOfActiveWells();
            List<string> Messages = new List<string>();

            // Normality Test
            List<double> CurrentDesc = new List<double>();
            for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                {
                    cWell TmpWell = PlateToProcess.GetWell(IdxValue, IdxValue0, true);
                    if (TmpWell != null)
                        CurrentDesc.Add(TmpWell.ListDescriptors[CurrentDescSel].GetValue());
                }
            CurrentDesc.Sort();

            if ((std(CurrentDesc.ToArray()) == 0))
            {
                //Messages.Add(/*PlateToProcess.Name + "\n \n*/"No systematic error detected !");
                return null;
            }

            double Anderson_DarlingValue = Anderson_Darling(CurrentDesc.ToArray());

            Messages.Add(string.Format("{0:0.###}", Anderson_DarlingValue));

            // now clustering
            if (!KMeans((int)GlobalInfo.OptionsWindow.numericUpDownSystErrorIdentKMeansClasses.Value, PlateToProcess, CurrentDescSel))
            {
                List<string> ListMessageError = new List<string>();
                ListMessageError.Add("K-Means Error");
                return ListMessageError;
            }

            // and finally classification
            int MinObjectsNumber = (NumWell * (int)GlobalInfo.OptionsWindow.numericUpDownSystemMinWellRatio.Value) / 100;

            List<string> ListMessage = ComputePlateBasedClassification((int)GlobalInfo.OptionsWindow.numericUpDownSystErrorIdentKMeansClasses.Value, MinObjectsNumber, PlateToProcess);

            for (int i = 0; i < ListMessage.Count; i++)
                Messages.Add(ListMessage[i]);

            return Messages;
        }

        private void systematicErrorsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            System.Windows.Forms.DialogResult Res = MessageBox.Show("By applying this process, classes will be definitively modified ! Proceed ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (Res == System.Windows.Forms.DialogResult.No) return;

            List<string> Result = GenerateArtifactMessage(CompleteScreening.GetCurrentDisplayPlate(), comboBoxDescriptorToDisplay.SelectedIndex);
            MessageBox.Show(Result[1], "Systematic Error Identification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private DataTable ComputeSystematicErrorsTable()
        {
            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;
            int NumDesc = CompleteScreening.ListDescriptors.Count;

            DataTable TableForQltControl = new DataTable();
            dataGridViewForQualityControl.Columns.Clear();
            dataGridViewForQualityControl.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;

            TableForQltControl.Columns.Add(new DataColumn("Plate", typeof(string)));
            TableForQltControl.Columns.Add(new DataColumn("Descriptor", typeof(string)));
            TableForQltControl.Columns.Add(new DataColumn("Anderson-Darling test", typeof(string)));

            for (int iDesc = 0; iDesc < CompleteScreening.GlobalInfo.ListArtifacts.Length; iDesc++)
            {
                TableForQltControl.Columns.Add(new DataColumn(CompleteScreening.GlobalInfo.ListArtifacts[iDesc], typeof(string)));
            }

            int IdxTable = 0;
            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(PlateIdx);

                // loop on all the desciptors
                for (int Desc = 0; Desc < NumDesc; Desc++)
                {
                    if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;
                    GlobalInfo.CurrentRichTextBox.AppendText(CurrentPlateToProcess.Name + " \\ " + CompleteScreening.ListDescriptors[Desc].GetName() + "\n");
                    List<string> Messages = GenerateArtifactMessage(CurrentPlateToProcess, Desc);

                    GlobalInfo.CurrentRichTextBox.AppendText("\n-------------------------------------------------------------------\n");
                    if (Messages == null) continue;
                    if (Convert.ToDouble(Messages[0]) < (double)GlobalInfo.OptionsWindow.numericUpDownSystErrorAndersonDarlingThreshold.Value) continue;

                    TableForQltControl.Rows.Add();

                    TableForQltControl.Rows[IdxTable][0] = CurrentPlateToProcess.Name;
                    TableForQltControl.Rows[IdxTable][1] = CompleteScreening.ListDescriptors[Desc].GetName();

                    if (Messages == null)
                    {
                        GlobalInfo.CurrentRichTextBox.AppendText("No variation !");
                        TableForQltControl.Rows[IdxTable][2] = "0";
                    }
                    else
                    {
                        if (Messages.Count == 1)
                        {
                            GlobalInfo.CurrentRichTextBox.AppendText(Messages[0]);
                        }
                        else
                        {
                            GlobalInfo.CurrentRichTextBox.AppendText(Messages[1]);
                            TableForQltControl.Rows[IdxTable][2] = Messages[0];
                        }

                        if (Messages.Count > 2)
                        {
                            for (int i = 0; i < CompleteScreening.GlobalInfo.ListArtifacts.Length; i++)
                                TableForQltControl.Rows[IdxTable][3 + i] = Messages[2 + i];
                        }
                    }
                    IdxTable++;
                }
                GlobalInfo.CurrentRichTextBox.AppendText("*******************************************************************\n");
            }

            return TableForQltControl;


        }

        private void buttonQualityControl_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.DialogResult Res = MessageBox.Show("By applying this process, classes will be definitively modified ! Proceed ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (Res == System.Windows.Forms.DialogResult.No) return;
            this.Cursor = Cursors.WaitCursor;
            DataTable ResultSystematicError = ComputeSystematicErrorsTable();
            dataGridViewForQualityControl.DataSource = ResultSystematicError;
            dataGridViewForQualityControl.Update();
            this.Cursor = Cursors.Default;
            return;
        }
        #endregion

        #region Compute and display Correlation matrix
        private void correlationMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComputeAndDisplayCorrelationMatrix(true, true, null);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ComputeAndDisplayCorrelationMatrix(false, true, null);
        }


        private void ComputeAndDisplayCorrelationMatrix(bool IsFullScreen, bool IsToBeDisplayed, string PathForImage)
        {
            if (CompleteScreening == null) return;

            bool IsDisplayRanking = CompleteScreening.GlobalInfo.OptionsWindow.checkBoxCorrelationMatrixDisplayRanking.Checked;
            bool IsPearson = CompleteScreening.GlobalInfo.OptionsWindow.radioButtonPearson.Checked;
            Boolean IsDisplayValues = false;

            List<double>[] ListValueDesc = ExtractDesciptorAverageValuesList(IsFullScreen);
            double[,] CorrelationMatrix = ComputeCorrelationMatrix(ListValueDesc, IsPearson);

            if (CorrelationMatrix == null)
            {
                MessageBox.Show("Data error, correlation computation impossible !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //   return;
            List<string> NameX = CompleteScreening.ListDescriptors.GetListNameActives();
            List<string> NameY = CompleteScreening.ListDescriptors.GetListNameActives();

            string TitleForGraph = "";
            if (IsPearson) TitleForGraph = "Pearson ";
            else
                TitleForGraph = "Spearman's rank ";
            TitleForGraph += " correlation matrix.";

            int SquareSize;

            if (NameX.Count > 20)
                SquareSize = 5;
            else
                SquareSize = 100 - ((10 * NameX.Count) / 3);
            DisplayMatrix(CorrelationMatrix, NameX, NameY, IsDisplayValues, TitleForGraph, SquareSize, IsToBeDisplayed, PathForImage);
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (IsDisplayRanking == false) return;

            Series CurrentSeries1 = new Series("Data1");
            CurrentSeries1.ShadowOffset = 1;
            CurrentSeries1.ChartType = SeriesChartType.Column;


            int RealPosSelectedDesc = -1;

            int realPos = 0;
            for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
            {
                if (CompleteScreening.ListDescriptors[i].IsActive()) realPos++;
                if (i == CompleteScreening.ListDescriptors.CurrentSelectedDescriptor)
                {
                    RealPosSelectedDesc = i - 2;
                    break;
                }
            }

            // loop on all the desciptors
            int IdxValue = 0;
            for (int iDesc = 0; iDesc < ListValueDesc.Length; iDesc++)
                for (int jDesc = 0; jDesc < ListValueDesc.Length; jDesc++)
                {
                    if (iDesc <= jDesc) continue;
                    CurrentSeries1.Points.Add(Math.Abs(CorrelationMatrix[iDesc, jDesc]));

                    if (GlobalInfo.OptionsWindow.checkBoxCorrelationRankChangeColorForActiveDesc.Checked)
                    {
                        if (CompleteScreening.ListDescriptors.CurrentSelectedDescriptor < CompleteScreening.ListDescriptors.GetListNameActives().Count)
                        {
                            if ((CompleteScreening.ListDescriptors.GetListNameActives()[iDesc] == CompleteScreening.ListDescriptors.GetListNameActives()[CompleteScreening.ListDescriptors.CurrentSelectedDescriptor]) ||
                                (CompleteScreening.ListDescriptors.GetListNameActives()[jDesc] == CompleteScreening.ListDescriptors.GetListNameActives()[CompleteScreening.ListDescriptors.CurrentSelectedDescriptor]))
                                CurrentSeries1.Points[IdxValue].Color = Color.LightGreen;
                        }
                    }
                    CurrentSeries1.Points[IdxValue].Label = string.Format("{0:0.###}", CorrelationMatrix[iDesc, jDesc]);
                    CurrentSeries1.Points[IdxValue].ToolTip = CompleteScreening.ListDescriptors.GetListNameActives()[iDesc] + "\n vs. \n" + CompleteScreening.ListDescriptors.GetListNameActives()[jDesc];
                    CurrentSeries1.Points[IdxValue].AxisLabel = CurrentSeries1.Points[IdxValue++].ToolTip;
                }

            SimpleForm NewWindow1 = new SimpleForm();
            int thisWidth = CurrentSeries1.Points.Count * 100 + 200;
            if (thisWidth > (int)GlobalInfo.OptionsWindow.numericUpDownMaximumWidth.Value)
                thisWidth = (int)GlobalInfo.OptionsWindow.numericUpDownMaximumWidth.Value;
            NewWindow1.Width = thisWidth;
            //NewWindow1.Width = CurrentSeries1.Points.Count * 100 + 200;

            ChartArea CurrentChartArea1 = new ChartArea("Default1");
            CurrentChartArea1.BackGradientStyle = GradientStyle.TopBottom;
            CurrentChartArea1.BackColor = CompleteScreening.GlobalInfo.OptionsWindow.panel1.BackColor;
            CurrentChartArea1.BackSecondaryColor = Color.White;
            CurrentChartArea1.BorderColor = Color.Black;

            NewWindow1.chartForSimpleForm.ChartAreas.Add(CurrentChartArea1);
            CurrentSeries1.SmartLabelStyle.Enabled = true;
            CurrentChartArea1.AxisY.Title = "Absolute Correlation Coeff.";

            NewWindow1.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            NewWindow1.chartForSimpleForm.Series.Add(CurrentSeries1);

            CurrentChartArea1.Axes[0].MajorGrid.Enabled = false;
            CurrentChartArea1.Axes[1].MajorGrid.Enabled = false;
            CurrentChartArea1.Axes[1].Minimum = 0;
            CurrentChartArea1.Axes[1].Maximum = 1.2;

            CurrentChartArea1.AxisX.Interval = 1;
            NewWindow1.chartForSimpleForm.Series[0].Sort(PointSortOrder.Ascending, "Y");

            if (IsPearson) TitleForGraph = "Pearson ";
            else
                TitleForGraph = "Spearman's ";
            TitleForGraph += "correlation ranking";

            Title CurrentTitle1 = new Title(TitleForGraph);

            if (IsToBeDisplayed) NewWindow1.Show();
            else
                NewWindow1.chartForSimpleForm.SaveImage(PathForImage + "_Ranking.png", ChartImageFormat.Png);

            NewWindow1.chartForSimpleForm.Titles.Add(CurrentTitle1);
            NewWindow1.Text = "Quality Control: Corr. ranking";
            NewWindow1.chartForSimpleForm.Update();
            NewWindow1.Controls.AddRange(new System.Windows.Forms.Control[] { NewWindow1.chartForSimpleForm });

            return;
        }



        private double[,] ComputeCorrelationMatrix(List<double>[] ListValueDesc, bool IsPearson)
        {
            int NumDesc = ListValueDesc.Length;
            double[,] CorrelationMatrix = new double[NumDesc, NumDesc];
            //return null;
            for (int iDesc = 0; iDesc < NumDesc; iDesc++)
                for (int jDesc = 0; jDesc < NumDesc; jDesc++)
                {
                    try
                    {
                        if (IsPearson)
                            CorrelationMatrix[iDesc, jDesc] = (alglib.pearsoncorr2(ListValueDesc[iDesc].ToArray(), ListValueDesc[jDesc].ToArray()));
                        else
                            CorrelationMatrix[iDesc, jDesc] = (alglib.spearmancorr2(ListValueDesc[iDesc].ToArray(), ListValueDesc[jDesc].ToArray()));
                    }
                    catch
                    {
                        //Console.WriteLine("Input string is not a sequence of digits.");
                        return null;
                    }

                }
            return CorrelationMatrix;
        }

        private List<double>[] ExtractDesciptorAverageValuesList(bool IsFullScreen)
        {
            int NumDesc = CompleteScreening.ListDescriptors.GetListNameActives().Count;
            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;
            List<double>[] ListValueDesc = new List<double>[NumDesc];

            for (int i = 0; i < NumDesc; i++) ListValueDesc[i] = new List<double>();

            List<cPlate> PlatesToProcess = new List<cPlate>();
            if (IsFullScreen)
            {
                for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
                {
                    cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);
                    PlatesToProcess.Add(CurrentPlateToProcess);
                }
            }
            else
            {
                PlatesToProcess.Add(CompleteScreening.GetCurrentDisplayPlate());
            }

            int ActiveDesc;

            // loop on all the plate
            for (int PlateIdx = 0; PlateIdx < PlatesToProcess.Count; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = PlatesToProcess[PlateIdx];

                int NumActiveWells = CurrentPlateToProcess.GetNumberOfActiveWells();

                for (int Col = 0; Col < CompleteScreening.Columns; Col++)
                    for (int Row = 0; Row < CompleteScreening.Rows; Row++)
                    {
                        cWell TmpWell = CurrentPlateToProcess.GetWell(Col, Row, true);
                        if (TmpWell == null) continue;
                        ActiveDesc = 0;
                        for (int Desc = 0; Desc < CompleteScreening.ListDescriptors.Count; Desc++)
                        {
                            if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;
                            ListValueDesc[ActiveDesc++].Add(TmpWell.ListDescriptors[Desc].GetValue());
                        }
                    }
            }
            return ListValueDesc;
        }

        private void DisplayMatrix(double[,] Matrix, List<string> ListLabelX, List<string> ListLabelY, bool IsDisplayValues, string TitleForGraph, int SquareSize, bool IsToBeDisplayed, string PathName)
        {
            int IdxValue = 0;

            Series CurrentSeries = new Series("Matrix");
            CurrentSeries.ChartType = SeriesChartType.Point;
            // loop on all the desciptors
            for (int iDesc = 0; iDesc < ListLabelX.Count; iDesc++)
            {
                for (int jDesc = 0; jDesc < ListLabelY.Count; jDesc++)
                {
                    CurrentSeries.Points.AddXY(iDesc + 1, jDesc + 1);
                    CurrentSeries.Points[IdxValue].MarkerStyle = MarkerStyle.Square;
                    CurrentSeries.Points[IdxValue].MarkerSize = SquareSize;
                    CurrentSeries.Points[IdxValue].BorderColor = Color.Black;
                    CurrentSeries.Points[IdxValue].BorderWidth = 1;
                    double Value = Matrix[iDesc, jDesc];

                    if (IsDisplayValues) CurrentSeries.Points[IdxValue].Label = string.Format("{0:0.###}", Math.Abs(Value));

                    CurrentSeries.Points[IdxValue].ToolTip = Math.Abs(Value) + " <=> | " + Matrix[iDesc, jDesc].ToString() + " |";

                    int ConvertedValue = (int)(Math.Abs(Value) * (CompleteScreening.GlobalInfo.LUT_JET[0].Length - 1));

                    CurrentSeries.Points[IdxValue++].Color = Color.FromArgb(CompleteScreening.GlobalInfo.LUT_JET[0][ConvertedValue], CompleteScreening.GlobalInfo.LUT_JET[1][ConvertedValue], CompleteScreening.GlobalInfo.LUT_JET[2][ConvertedValue]);
                }
            }

            for (int iDesc = 0; iDesc < ListLabelX.Count * ListLabelX.Count; iDesc++)
                CurrentSeries.Points[iDesc].AxisLabel = CompleteScreening.ListDescriptors.GetListNameActives()[iDesc / ListLabelX.Count];

            SmartLabelStyle SStyle = new SmartLabelStyle();

            SimpleForm NewWindow = new SimpleForm();
            NewWindow.Height = SquareSize * ListLabelY.Count + 220;
            NewWindow.Width = SquareSize * ListLabelX.Count + 245;

            ChartArea CurrentChartArea = new ChartArea("Default");
            for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
            {
                CustomLabel lblY = new CustomLabel();
                lblY.ToPosition = i * 2 + 2;
                lblY.Text = ListLabelY[i];
                CurrentChartArea.AxisY.CustomLabels.Add(lblY);
            }

            CurrentChartArea.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep30;
            CurrentChartArea.BorderColor = Color.Black;
            NewWindow.chartForSimpleForm.ChartAreas.Add(CurrentChartArea);
            CurrentSeries.SmartLabelStyle.Enabled = true;

            NewWindow.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            NewWindow.chartForSimpleForm.Series.Add(CurrentSeries);

            CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
            CurrentChartArea.Axes[0].Minimum = 0;
            CurrentChartArea.Axes[0].Maximum = ListLabelX.Count + 1;
            CurrentChartArea.Axes[1].MajorGrid.Enabled = false;
            CurrentChartArea.Axes[1].Minimum = 0;
            CurrentChartArea.Axes[1].Maximum = ListLabelY.Count + 1;
            CurrentChartArea.AxisX.Interval = 1;
            CurrentChartArea.AxisY.Interval = 1;

            Title CurrentTitle = new Title(TitleForGraph);
            NewWindow.chartForSimpleForm.Titles.Add(CurrentTitle);
            NewWindow.chartForSimpleForm.Titles[0].Font = new Font("Arial", 9);

            if (IsToBeDisplayed) NewWindow.Show();
            else
                NewWindow.chartForSimpleForm.SaveImage(PathName + "_Matrix.emf", ChartImageFormat.Emf);
            NewWindow.Text = TitleForGraph;
            NewWindow.chartForSimpleForm.Update();
            NewWindow.chartForSimpleForm.Show();
            NewWindow.Controls.AddRange(new System.Windows.Forms.Control[] { NewWindow.chartForSimpleForm });
        }



        #endregion

        #region Edit
        private void copyAverageValuesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;
            StringBuilder sb = new StringBuilder();

            if (tabControlMain.SelectedTab.Name == tabPageQualityQtrl.Name)
            {



            }
            else
            {
                sb.Append(String.Format(CompleteScreening.ListDescriptors[comboBoxDescriptorToDisplay.SelectedIndex].GetName() + "\t"));
                //sb.Append("\t");
                for (int i = 0; i < CompleteScreening.Columns - 1; i++)
                {
                    int IdxCol = i + 1;
                    sb.Append(String.Format("{0}\t", IdxCol));
                }
                sb.Append(String.Format("{0}", CompleteScreening.Columns - 1));
                sb.AppendLine();

                for (int j = 0; j < CompleteScreening.Rows; j++)
                {
                    byte[] strArray = new byte[1];
                    strArray[0] = (byte)(j + 65);

                    string Chara = Encoding.UTF7.GetString(strArray);
                    sb.Append(String.Format("{0}\t", Chara));

                    for (int i = 0; i < CompleteScreening.Columns - 1; i++)
                    {
                        cWell CurrentWell = CompleteScreening.GetCurrentDisplayPlate().GetWell(i, j, false);
                        if (CurrentWell == null)
                            sb.Append("\t");
                        else
                            sb.Append(String.Format("{0}\t", CurrentWell.ListDescriptors[comboBoxDescriptorToDisplay.SelectedIndex].GetValue()));
                    }


                    cWell CurrentWellFinal = CompleteScreening.GetCurrentDisplayPlate().GetWell(CompleteScreening.Columns - 1, j, false);
                    if (CurrentWellFinal == null)
                        sb.Append("\t");
                    else
                        sb.Append(String.Format("{0}\t", CurrentWellFinal.ListDescriptors[comboBoxDescriptorToDisplay.SelectedIndex].GetValue()));


                    //  sb.Append(String.Format("{0}", CompleteScreening.GetPlate(0).GetWell(CompleteScreening.Columns-1, j).ListDescriptors[(int)numericUpDownDescriptorIndex.Value].AverageValue));
                    sb.AppendLine();
                }
            }
            Clipboard.SetText(sb.ToString());

        }




        private void copyClassesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;
            StringBuilder sb = new StringBuilder();

            sb.Append(String.Format("Class\t"));
            //sb.Append("\t");
            for (int i = 0; i < CompleteScreening.Columns - 1; i++)
            {
                int IdxCol = i + 1;
                sb.Append(String.Format("{0}\t", IdxCol));
            }
            sb.Append(String.Format("{0}", CompleteScreening.Columns - 1));
            sb.AppendLine();

            for (int j = 0; j < CompleteScreening.Rows; j++)
            {
                byte[] strArray = new byte[1];
                strArray[0] = (byte)(j + 65);

                string Chara = Encoding.UTF7.GetString(strArray);
                sb.Append(String.Format("{0}\t", Chara));

                for (int i = 0; i < CompleteScreening.Columns - 1; i++)
                {
                    cWell CurrentWell = CompleteScreening.GetCurrentDisplayPlate().GetWell(i, j, false);
                    if (CurrentWell == null)
                        sb.Append("\t");
                    else
                        sb.Append(String.Format("{0}\t", CurrentWell.GetClass()));
                }


                cWell CurrentWellFinal = CompleteScreening.GetCurrentDisplayPlate().GetWell(CompleteScreening.Columns - 1, j, false);
                if (CurrentWellFinal == null)
                    sb.Append("\t");
                else
                    sb.Append(String.Format("{0}\t", CurrentWellFinal.GetClass()));


                //  sb.Append(String.Format("{0}", CompleteScreening.GetPlate(0).GetWell(CompleteScreening.Columns-1, j).ListDescriptors[(int)numericUpDownDescriptorIndex.Value].AverageValue));
                sb.AppendLine();
            }
            Clipboard.SetText(sb.ToString());
        }

        #endregion

        #region Normal Probability Plot
        /// <summary>
        /// Draw normal probability plot a complete screening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void normalProbabilityPlotToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ComputeAndDisplayMormalProbabilityPlot(true);
        }


        /// <summary>
        /// Draw normal probability plot a single plate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void normalProbabilityPlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComputeAndDisplayMormalProbabilityPlot(false);
        }


        private void ComputeAndDisplayMormalProbabilityPlot(bool IsFullScreen)
        {
            if (CompleteScreening == null) return;


            int CurrentDescSel = comboBoxDescriptorToDisplay.SelectedIndex;
            if (CompleteScreening.SelectedClass < 0) return;

            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;
            List<cPlate> PlatesToProcess = new List<cPlate>();
            if (IsFullScreen)
            {
                for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
                {
                    cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);
                    PlatesToProcess.Add(CurrentPlateToProcess);
                }
            }
            else
            {
                PlatesToProcess.Add(CompleteScreening.GetCurrentDisplayPlate());
            }

            List<double> CurrentDesc = new List<double>();

            // loop on all the plate
            for (int PlateIdx = 0; PlateIdx < PlatesToProcess.Count; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = PlatesToProcess[PlateIdx];

                for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                    for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                    {
                        cWell TmpWell = CompleteScreening.GetCurrentDisplayPlate().GetWell(IdxValue, IdxValue0, true);
                        if (TmpWell != null)
                        {
                            if (TmpWell.GetClass() == CompleteScreening.SelectedClass)
                                CurrentDesc.Add(TmpWell.ListDescriptors[CurrentDescSel].GetValue());
                        }
                    }
            }

            if (CurrentDesc.Count < 3)
            {
                MessageBox.Show("Not enough data of class " + CompleteScreening.SelectedClass, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CurrentDesc.Sort();
            double[] CenterNormDesc = new double[CurrentDesc.Count];
            CenterNormDesc = MeanCenteringStdStandarization(CurrentDesc.ToArray());

            int N = CurrentDesc.Count;
            double[] CumulativeProba = new double[CurrentDesc.Count];
            for (int i = 1; i < N - 1; i++)
                CumulativeProba[i] = (i - 0.3175) / (N + 0.365);

            CumulativeProba[N - 1] = Math.Pow(0.5, 1.0 / N);
            CumulativeProba[0] = 1 - CumulativeProba[N - 1];

            double[] PercentPointFunction = new double[CurrentDesc.Count];

            for (int i = 0; i < N; i++)
                PercentPointFunction[i] = alglib.normaldistr.invnormaldistribution(CumulativeProba[i]);

            SimpleForm NewWindow = new SimpleForm();
            NewWindow.Width = 600;
            NewWindow.Height = 600;

            NewWindow.Name = CompleteScreening.ListDescriptors[CurrentDescSel].GetName() + " normality plot : " + CompleteScreening.GetCurrentDisplayPlate().GetNumberOfActiveWells() + " points";
            Series CurrentSeries = new Series();
            CurrentSeries.ShadowOffset = 1;
            for (int Pt = 0; Pt < CurrentDesc.Count; Pt++)
            {
                CurrentSeries.Points.AddXY(PercentPointFunction[Pt], CenterNormDesc[Pt]);
                CurrentSeries.Points[Pt].Color = CompleteScreening.GlobalInfo.GetColor(CompleteScreening.SelectedClass);
                CurrentSeries.Points[Pt].MarkerStyle = MarkerStyle.Circle;
                CurrentSeries.Points[Pt].MarkerSize = 6;
            }

            ChartArea CurrentChartArea = new ChartArea();
            CurrentChartArea.BorderColor = Color.Black;
            NewWindow.chartForSimpleForm.ChartAreas.Add(CurrentChartArea);

            NewWindow.chartForSimpleForm.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            CurrentChartArea.BackColor = Color.FromArgb(164, 164, 164);
            CurrentChartArea.Axes[0].MajorGrid.Enabled = true;
            CurrentChartArea.Axes[1].MajorGrid.Enabled = true;
            CurrentChartArea.AxisY.Minimum = CenterNormDesc[0];
            CurrentChartArea.AxisY.Maximum = CenterNormDesc[CurrentDesc.Count - 1];
            CurrentChartArea.AxisX.Minimum = -3;
            CurrentChartArea.AxisX.Maximum = 3;
            CurrentChartArea.AxisY.LabelStyle.Format = "N1";
            CurrentChartArea.AxisX.LabelStyle.Format = "N1";

            CurrentSeries.ChartType = SeriesChartType.Point;
            NewWindow.chartForSimpleForm.Series.Add(CurrentSeries);

            double Anderson_DarlingValue = Anderson_Darling(CurrentDesc.ToArray());
            GlobalInfo.ConsoleWriteLine("Anderson-Darling Test: " + Anderson_DarlingValue);

            Title AndersonLegend = new Title();
            if (CurrentDesc.Count >= 5)
            {
                double Jarque_BeraValue;
                alglib.jarqueberatest(CurrentDesc.ToArray(), CurrentDesc.Count, out Jarque_BeraValue);
                GlobalInfo.ConsoleWriteLine("Jarque-Bera Test: " + Jarque_BeraValue);
                //  AndersonLegend.Text = "Jarque-Bera: " + String.Format("{0:0.####}", Jarque_BeraValue);
            }

            AndersonLegend.Text += "Anderson-Darling: " + String.Format("{0:0.##}", Anderson_DarlingValue);
            AndersonLegend.Alignment = ContentAlignment.MiddleCenter;
            AndersonLegend.Docking = Docking.Bottom;
            AndersonLegend.TextOrientation = TextOrientation.Horizontal;

            NewWindow.chartForSimpleForm.Titles.Add(AndersonLegend);

            Title MainLegend = new Title();
            MainLegend.Text = CompleteScreening.ListDescriptors[CurrentDescSel].GetName();
            MainLegend.Docking = Docking.Top;
            MainLegend.Font = new System.Drawing.Font("Arial", 11, FontStyle.Bold);
            NewWindow.chartForSimpleForm.Titles.Add(MainLegend);

            NewWindow.chartForSimpleForm.Series.Add("TrendLine");
            NewWindow.chartForSimpleForm.Series["TrendLine"].ChartType = SeriesChartType.Line;
            NewWindow.chartForSimpleForm.Series["TrendLine"].BorderWidth = 1;
            NewWindow.chartForSimpleForm.Series["TrendLine"].Color = Color.Red;
            // Line of best fit is linear
            string typeRegression = "Linear";//"Exponential";//
            // The number of days for Forecasting
            string forecasting = "1";
            // Show Error as a range chart.
            string error = "false";
            // Show Forecasting Error as a range chart.
            string forecastingError = "false";
            // Formula parameters
            string parameters = typeRegression + ',' + forecasting + ',' + error + ',' + forecastingError;
            NewWindow.chartForSimpleForm.Series[0].Sort(PointSortOrder.Ascending, "X");
            // Create Forecasting Series.
            NewWindow.chartForSimpleForm.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, parameters, NewWindow.chartForSimpleForm.Series[0], NewWindow.chartForSimpleForm.Series["TrendLine"]);

            //  NewWindow.Text = "Normal Probability Plot / " +;
            NewWindow.Show();

            if (IsFullScreen)
            {
                NewWindow.Text = CurrentDesc.Count + " points";
            }
            else
            {
                NewWindow.Text = CompleteScreening.GetCurrentDisplayPlate().Name + " : " + CurrentDesc.Count + " points";
            }
            NewWindow.chartForSimpleForm.Update();
            NewWindow.chartForSimpleForm.Show();
            NewWindow.Controls.AddRange(new System.Windows.Forms.Control[] { NewWindow.chartForSimpleForm });
        }
        #endregion

        #region XY scatter points
        private void xYScatterPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            SimpleFormForXY FormToDisplayXY = new SimpleFormForXY(false);
            FormToDisplayXY.CompleteScreening = CompleteScreening;

            for (int i = 0; i < (int)CompleteScreening.ListDescriptors.Count; i++)
            {
                FormToDisplayXY.comboBoxDescriptorX.Items.Add(CompleteScreening.ListDescriptors[i].GetName());
                FormToDisplayXY.comboBoxDescriptorY.Items.Add(CompleteScreening.ListDescriptors[i].GetName());
            }

            FormToDisplayXY.comboBoxDescriptorX.SelectedIndex = 0;
            FormToDisplayXY.comboBoxDescriptorY.SelectedIndex = 0;


            FormToDisplayXY.DisplayXY();
            FormToDisplayXY.ShowDialog();

            return;
        }

        private void xYScatterPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            SimpleFormForXY FormToDisplayXY = new SimpleFormForXY(true);
            FormToDisplayXY.CompleteScreening = CompleteScreening;

            for (int i = 0; i < (int)CompleteScreening.ListDescriptors.Count; i++)
            {
                FormToDisplayXY.comboBoxDescriptorX.Items.Add(CompleteScreening.ListDescriptors[i].GetName());
                FormToDisplayXY.comboBoxDescriptorY.Items.Add(CompleteScreening.ListDescriptors[i].GetName());
            }

            FormToDisplayXY.comboBoxDescriptorX.SelectedIndex = 0;
            FormToDisplayXY.comboBoxDescriptorY.SelectedIndex = 0;


            FormToDisplayXY.DisplayXY();
            FormToDisplayXY.ShowDialog();

            return;
        }
        #endregion

        #region Plugins Management
        private void HCSAnalyzer_Shown(object sender, EventArgs e)
        {
            BuildPluginMenu();
        }

        private void BuildPluginMenu()
        {
            List<PluginDescriptor> paList = null;
            try
            {
                paList = PluginDescriptor.GetList(Application.StartupPath + @"\Plugins");
            }
            catch (DirectoryNotFoundException e)
            {
                Directory.CreateDirectory("Plugins");
                paList = PluginDescriptor.GetList(Application.StartupPath + @"\Plugins");
                //MessageBox.Show("Error: " + e.Message, "Plugin's directory not Found" + "\n No Plugin will be loaded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            ToolStripMenuItem currentMenu = null;

            foreach (PluginDescriptor pluginDescriptor in paList)
            {
                currentMenu = pluginsToolStripMenuItem;

                string[] subMenu = pluginDescriptor.MenuPath.Split('|');
                if (pluginDescriptor.MenuPath.Length != 0)
                {
                    foreach (string sm in subMenu)
                    {
                        string menuName = sm.Trim();

                        //if submenu exist , get in
                        if (currentMenu.DropDownItems.ContainsKey(menuName))
                        {
                            currentMenu = (ToolStripMenuItem)currentMenu.DropDownItems[menuName];
                        }
                        else//if not, create it first.
                        {
                            ToolStripMenuItem tsmMenu = new ToolStripMenuItem(menuName);
                            currentMenu.DropDownItems.Add(tsmMenu);
                            tsmMenu.Name = menuName;
                            currentMenu = tsmMenu;
                        }
                    }
                }

                ToolStripMenuItem tsmiName =
                    new ToolStripMenuItem(pluginDescriptor.Name + @" - " + pluginDescriptor.Author);
                currentMenu.DropDownItems.Add(tsmiName);
                tsmiName.Tag = pluginDescriptor;
                tsmiName.Name = pluginDescriptor.Name;
                currentMenu = tsmiName;
                currentMenu.Click += new EventHandler(toolMenuItem_Click);
            }
        }

        private void toolMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem s = (ToolStripMenuItem)sender;

                PluginDescriptor p = (PluginDescriptor)s.Tag;
                Plugin.CurrentScreen = CompleteScreening;
                p.Instanciate();
            }
            catch (PluginException ex)
            {
                MessageBox.Show(ex.Message, "Plugin information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region Hierachical Clustering Display
        private void hierarchicalClusteringToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.DialogResult Res = MessageBox.Show("Hierarchical tree is not adpated for large number of experiments !\n It can rapidly generate out-of-memory exception!\n Proceed anyway ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (Res == System.Windows.Forms.DialogResult.No) return;
            cDendoGram DendoGram = new cDendoGram(GlobalInfo, true);
            return;
        }

        private void hierarchicalTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cDendoGram DendoGram = new cDendoGram(GlobalInfo, false);
        }
        #endregion

        private void HCSAnalyzer_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (GlobalInfo.CurrentScreen != null)
                GlobalInfo.CurrentScreen.Close3DView();
            this.Dispose();
        }

        #region DRC management
        private void doseResponseManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalInfo.WindowForDRCDesign.IsDisposed) GlobalInfo.WindowForDRCDesign = new FormForDRCDesign();
            GlobalInfo.WindowForDRCDesign.Visible = true;
        }

        private void convertDRCToWellToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            
            System.Windows.Forms.DialogResult ResWin = MessageBox.Show("By applying this process, the current screening will be entirely updated ! Proceed ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ResWin == System.Windows.Forms.DialogResult.No) return;


            //foreach (cDescriptorsType DescType in CompleteScreening.ListDescriptors)
            //{
            //    CompleteScreening.ListDescriptors.RemoveDescUnSafe(DescType, CompleteScreening);
            //}


            if (CompleteScreening != null) CompleteScreening.Close3DView();

         //   CompleteScreening.ListDescriptors.RemoveDesc(CompleteScreening.ListDescriptors[IntToTransfer], CompleteScreening);
            cScreening MergedScreening = new cScreening("Merged Screen", GlobalInfo);
            MergedScreening.PanelForPlate = this.panelForPlate;

            MergedScreening.Rows = CompleteScreening.Rows;
            MergedScreening.Columns = CompleteScreening.Columns;

            MergedScreening.ListPlatesAvailable = new cExtendPlateList();

            // create the descriptor
            MergedScreening.ListDescriptors.Clean();



            int Idesc = 0;

            List<cDescriptorsType> ListDescType = new List<cDescriptorsType>();

            for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
            {
                if (!CompleteScreening.ListDescriptors[i].IsActive()) continue;

                cDescriptorsType DescEC50 = new cDescriptorsType("EC50_" + CompleteScreening.ListDescriptors[i].GetName(), true, 1);
                ListDescType.Add(DescEC50);
                MergedScreening.ListDescriptors.AddNew(DescEC50);

                cDescriptorsType DescTop = new cDescriptorsType("Top_" + CompleteScreening.ListDescriptors[i].GetName(), true, 1);
                ListDescType.Add(DescTop);
                MergedScreening.ListDescriptors.AddNew(DescTop);

                cDescriptorsType DescBottom = new cDescriptorsType("Bottom_" + CompleteScreening.ListDescriptors[i].GetName(), true, 1);
                ListDescType.Add(DescBottom);
                MergedScreening.ListDescriptors.AddNew(DescBottom);

                cDescriptorsType DescSlope = new cDescriptorsType("Slope_" + CompleteScreening.ListDescriptors[i].GetName(), true, 1);
                ListDescType.Add(DescSlope);
                MergedScreening.ListDescriptors.AddNew(DescSlope);

                Idesc++;
            }

                    MergedScreening.ListDescriptors.CurrentSelectedDescriptor = 0;
            foreach (cPlate CurrentPlate in CompleteScreening.ListPlatesAvailable)
            {

                cPlate NewPlate = new cPlate("Cpds", CurrentPlate.Name + " Merged", MergedScreening);
                // check if the plate exist already
                MergedScreening.AddPlate(NewPlate);

                foreach (cDRC_Region CurrentRegion in CurrentPlate.ListDRCRegions)
                {

                    List<cDescriptor> LDesc = new List<cDescriptor>();

                    Idesc = 0;
                    int IDESCBase = 0;

                    for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                    {
                        if (!CompleteScreening.ListDescriptors[i].IsActive()) continue;

                        cDRC CurrentDRC = CurrentRegion.GetDRC(CompleteScreening.ListDescriptors[IDESCBase++]);

                        cDescriptor Desc_EC50 = new cDescriptor(CurrentDRC.EC50, ListDescType[Idesc++],CompleteScreening);
                            LDesc.Add(Desc_EC50);

                            cDescriptor Desc_Top = new cDescriptor(CurrentDRC.Top, ListDescType[Idesc++],CompleteScreening);
                            LDesc.Add(Desc_Top);

                            cDescriptor Desc_Bottom = new cDescriptor(CurrentDRC.Bottom, ListDescType[Idesc++],CompleteScreening);
                            LDesc.Add(Desc_Bottom);

                            cDescriptor Desc_Slope = new cDescriptor(CurrentDRC.Slope, ListDescType[Idesc++],CompleteScreening);
                            LDesc.Add(Desc_Slope);

                            

                    }
                    cWell NewWell = new cWell(LDesc, CurrentRegion.PosXMin+1, CurrentRegion.PosYMin+1, MergedScreening, NewPlate);
                    NewWell.Name = "DRC [" + CurrentRegion.PosXMin + ":" + CurrentRegion.PosYMin + "]";
                    NewPlate.AddWell(NewWell);




                }
            }


            CompleteScreening.ListDescriptors = MergedScreening.ListDescriptors;
            CompleteScreening.ListPlatesAvailable = MergedScreening.ListPlatesAvailable;
            CompleteScreening.ListPlatesActive = MergedScreening.ListPlatesActive;

            //CompleteScreening.


            CompleteScreening.UpDatePlateListWithFullAvailablePlate();
            for (int idxP = 0; idxP < CompleteScreening.ListPlatesActive.Count; idxP++)
                CompleteScreening.ListPlatesActive[idxP].UpDataMinMax();
            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, true);


        }

        private void displayRespondingDRCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening.GetCurrentDisplayPlate().ListDRCRegions == null) return;

            FormForDRCSelection WindowSelectionDRC = new FormForDRCSelection();
            if (WindowSelectionDRC.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            if (WindowSelectionDRC.checkBoxMOAClassification.Checked == false)
            {
                int h = 0;
                FormToDisplayDRC WindowforDRCsDisplay = new FormToDisplayDRC();

                foreach (cDRC_Region TmpRegion in CompleteScreening.GetCurrentDisplayPlate().ListDRCRegions)
                {
                    int cpt = 0;
                    List<cDRC> ListDRC = new List<cDRC>();
                    for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                    {
                        if (CompleteScreening.ListDescriptors[i].IsActive())
                        {
                            cDRC CurrentDRC = new cDRC(TmpRegion, CompleteScreening.ListDescriptors[i]);
                            if (CurrentDRC.IsResponding(WindowSelectionDRC)==1)
                            {
                                ListDRC.Add(CurrentDRC);
                                cpt++;
                            }
                        }
                    }

                    cDRCDisplay DRCDisplay = new cDRCDisplay(ListDRC, GlobalInfo);
                    if (DRCDisplay.CurrentChart.Series.Count == 0) continue;

                    DRCDisplay.CurrentChart.Location = new Point((DRCDisplay.CurrentChart.Width + 50) * 0, (DRCDisplay.CurrentChart.Height + 10 + DRCDisplay.CurrentRichTextBox.Height) * h++);
                    DRCDisplay.CurrentRichTextBox.Location = new Point(DRCDisplay.CurrentChart.Location.X, DRCDisplay.CurrentChart.Location.Y + DRCDisplay.CurrentChart.Height + 5);

                    WindowforDRCsDisplay.LChart.Add(DRCDisplay.CurrentChart);
                    WindowforDRCsDisplay.LRichTextBox.Add(DRCDisplay.CurrentRichTextBox);
                }

                WindowforDRCsDisplay.panelForDRC.Controls.AddRange(WindowforDRCsDisplay.LChart.ToArray());
                WindowforDRCsDisplay.panelForDRC.Controls.AddRange(WindowforDRCsDisplay.LRichTextBox.ToArray());
                WindowforDRCsDisplay.Show();
                return;
            }


            System.Windows.Forms.DialogResult ResWin = MessageBox.Show("By applying this process, the current screening will be entirely updated ! Proceed ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ResWin == System.Windows.Forms.DialogResult.No) return;



            foreach (cPlate CurrentPlate in CompleteScreening.ListPlatesActive)
            {
                foreach (cDRC_Region TmpRegion in CurrentPlate.ListDRCRegions)
                {
                    int cpt = 0;
                    //List<cDRC> ListDRC = new List<cDRC>();
                    //for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                    //{
                    //  if (CompleteScreening.ListDescriptors[i].IsActive())
                    //    {
                    //        cDRC CurrentDRC = new cDRC(TmpRegion, CompleteScreening.ListDescriptors[i]);
                    //        if (CurrentDRC.IsResponding(WindowSelectionDRC))
                    //        {
                    //            ListDRC.Add(CurrentDRC);
                    //            cpt++;
                    //        }
                    //    }


                    //}
                    List<int> ResDescActive = TmpRegion.GetListRespondingDescritpors(CompleteScreening, WindowSelectionDRC);
                    
                    for(int j=0;j<TmpRegion.NumReplicate;j++)
                        for (int i = 0; i < TmpRegion.NumConcentrations; i++)
                        {
                            
                            cWell CurrentWell = TmpRegion.GetListWells()[j][i];
                            if (CurrentWell == null) continue;

                            for(int IdxDesc=0;IdxDesc<ResDescActive.Count;IdxDesc++)
                            {
                                if(ResDescActive[IdxDesc]==-1) continue;

                                //CurrentWell.ListDescriptors[IdxDesc].HistoValues = new double[1];
                                CurrentWell.ListDescriptors[IdxDesc].SetHistoValues((double)ResDescActive[IdxDesc]);
                                if ((i == 0) && (j == 0))
                                    CurrentWell.SetClass(0);
                                else
                                    CurrentWell.SetAsNoneSelected();
                                    //[0] = ResDescActive[IdxDesc];   
                                CurrentWell.ListDescriptors[IdxDesc].UpDateDescriptorStatistics();

                            }

                        }
                }
                CurrentPlate.UpDataMinMax();
            }
        }

        private void displayDRCToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (CompleteScreening.GetCurrentDisplayPlate().ListDRCRegions == null) return;

            int h = 0;
            FormToDisplayDRC WindowforDRCsDisplay = new FormToDisplayDRC();

            foreach (cDRC_Region TmpRegion in CompleteScreening.GetCurrentDisplayPlate().ListDRCRegions)
            {
                int cpt = 0;
                List<cDRC> ListDRC = new List<cDRC>();
                for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                {
                    if (CompleteScreening.ListDescriptors[i].IsActive())
                    {
                        cDRC CurrentDRC = new cDRC(TmpRegion, CompleteScreening.ListDescriptors[i]);

                        ListDRC.Add(CurrentDRC);
                        cpt++;
                    }

                }

                cDRCDisplay DRCDisplay = new cDRCDisplay(ListDRC, GlobalInfo);

                if (DRCDisplay.CurrentChart.Series.Count == 0) continue;

                DRCDisplay.CurrentChart.Location = new Point((DRCDisplay.CurrentChart.Width + 50) * 0, (DRCDisplay.CurrentChart.Height + 10 + DRCDisplay.CurrentRichTextBox.Height) * h++);
                DRCDisplay.CurrentRichTextBox.Location = new Point(DRCDisplay.CurrentChart.Location.X, DRCDisplay.CurrentChart.Location.Y + DRCDisplay.CurrentChart.Height + 5);

                WindowforDRCsDisplay.LChart.Add(DRCDisplay.CurrentChart);
                WindowforDRCsDisplay.LRichTextBox.Add(DRCDisplay.CurrentRichTextBox);
            }

            WindowforDRCsDisplay.panelForDRC.Controls.AddRange(WindowforDRCsDisplay.LChart.ToArray());
            WindowforDRCsDisplay.panelForDRC.Controls.AddRange(WindowforDRCsDisplay.LRichTextBox.ToArray());
            WindowforDRCsDisplay.Show();
        }
        #endregion

        private void switchToDistributionModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalInfo.SwitchDistributionMode();
                
        }



    }

    /// <summary>
    /// If you want keep your version information,
    /// Put your version information in the AssemblyInfo.cs file
    /// [assembly: AssemblyVersion("1.0.*")]
    /// [assembly: AssemblyFileVersion("1.0.0.0")]
    /// </summary>
    public static class PluginVersion
    {
        public static string Info
        {
            get
            {
                System.Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                bool isDaylightSavingsTime = TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now);
                DateTime MyTime = new DateTime(2000, 1, 1).AddDays(v.Build).AddSeconds(v.Revision * 2).AddHours(isDaylightSavingsTime ? 1 : 0);

                return string.Format("Version:{0}.{1} - Compiled:{2:s}", v.Major, v.MajorRevision, MyTime);
            }
        }
    }
}