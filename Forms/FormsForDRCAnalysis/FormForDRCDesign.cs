using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibPlateAnalysis;
using HCSAnalyzer.Classes;

namespace HCSAnalyzer.Forms
{
    public partial class FormForDRCDesign : Form
    {
        public FormForDRCDesign()
        {
            InitializeComponent();
            // UpDateDisplay();
        }


        public List<cWell> ListWells = null;

        private cDRC_Region TemplateRegion = null;

        int SizeWell = 8;
        SolidBrush CurrBrush;
        Graphics g;

        int PosXMin;
        int PosYMin;
        int PosXMax;
        int PosYMax;




        private void buttonApply_Click(object sender, EventArgs e)
        {
            this.Visible = false;

            if (this.radioButtonConcentrationsManual.Checked)
            {
                System.Windows.Forms.DialogResult ResWin = MessageBox.Show("By applying this process, concentration values of the concerned wells will be modified ! Proceed ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (ResWin == System.Windows.Forms.DialogResult.No) return;
            }

            cScreening CurrentScreen = this.ListWells[0].AssociatedPlate.ParentScreening;

            //this.ListWells[0].AssociatedPlate.ListDRCRegions = new cListDRCRegion();

            foreach (cPlate TmpPlate in CurrentScreen.ListPlatesActive)
            {
                TmpPlate.ListDRCRegions = new cListDRCRegion();

                int SizeX, SizeY;
                if (TemplateRegion.IsConcentrationHorizontal)
                {
                    SizeX = TemplateRegion.NumConcentrations;
                    SizeY = TemplateRegion.NumReplicate;
                }
                else
                {
                    SizeX = TemplateRegion.NumReplicate;
                    SizeY = TemplateRegion.NumConcentrations;
                }

                int NumRepeatX = (this.ListWells[0].AssociatedPlate.ParentScreening.Columns - (TemplateRegion.PosXMin - 1)) / SizeX;
                int NumRepeatY = (this.ListWells[0].AssociatedPlate.ParentScreening.Rows - (TemplateRegion.PosYMin - 1)) / SizeY;

                if (!this.radioButtonOrientationColumn.Checked)
                {
                    for (int j = 0; j < NumRepeatY; j++)
                        for (int i = 0; i < NumRepeatX; i++)
                        {
                            cDRC_Region TempRegion = new cDRC_Region(TmpPlate, (int)this.numericUpDownConcentrationNumber.Value, (int)this.numericUpDownReplication.Value, i * SizeX + (TemplateRegion.PosXMin - 1), j * SizeY + (TemplateRegion.PosYMin - 1), true);
                            TmpPlate.ListDRCRegions.AddNewRegion(TempRegion);

                            // Update the concentration from the manual entry
                            if (this.radioButtonConcentrationsManual.Checked)
                            {
                                for (int Replicate = 0; Replicate < TempRegion.NumReplicate; Replicate++)
                                {
                                    cWell[] TmpList = TempRegion.GetlistReplicate(Replicate);
                                    for (int IdxConc = 0; IdxConc < TmpList.Length; IdxConc++)
                                        if (TmpList[IdxConc] != null) TmpList[IdxConc].Concentration = Convert.ToDouble(dataGridViewForConcentration.Rows[IdxConc].Cells[1].Value.ToString());
                                }
                            }
                        }
                }
                else
                {
                    for (int j = 0; j < NumRepeatY; j++)
                        for (int i = 0; i < NumRepeatX; i++)
                        {
                            cDRC_Region TempRegion = new cDRC_Region(TmpPlate, (int)this.numericUpDownConcentrationNumber.Value, (int)this.numericUpDownReplication.Value, i * SizeX + (TemplateRegion.PosXMin - 1), j * SizeY + (TemplateRegion.PosYMin - 1), false);
                            TmpPlate.ListDRCRegions.AddNewRegion(TempRegion);

                            // Update the concentration from the manual entry
                            if (this.radioButtonConcentrationsManual.Checked)
                            {
                                for (int Replicate = 0; Replicate < TempRegion.NumReplicate; Replicate++)
                                {
                                    cWell[] TmpList = TempRegion.GetlistReplicate(Replicate);
                                    for (int IdxConc = 0; IdxConc < TmpList.Length; IdxConc++)
                                        if (TmpList[IdxConc] != null) TmpList[IdxConc].Concentration = Convert.ToDouble(dataGridViewForConcentration.Rows[IdxConc].Cells[1].Value.ToString());
                                }
                            }
                        }
                }
            }


            CurrentScreen.GetCurrentDisplayPlate().Refresh3D(CurrentScreen.ListDescriptors.CurrentSelectedDescriptor);

        }



        private void UpDateDisplayConcentration()
        {
            if (TemplateRegion == null) return;

            dataGridViewForConcentration.Columns.Clear();

            dataGridViewForConcentration.Columns.Add("Idx", "Idx");
            dataGridViewForConcentration.Columns.Add("Concentration", "Concentration");

            for (int i = 1; i <= (int)numericUpDownConcentrationNumber.Value; i++)
            {
                int IdxPosCol = 0;
                dataGridViewForConcentration.Rows.Add();
                dataGridViewForConcentration.Rows[i - 1].Cells[IdxPosCol++].Value = i;


                if (radioButtonConcentrationFromThePlate.Checked)
                {
                    if (ListWells[i - 1].Concentration >= 0)
                        dataGridViewForConcentration.Rows[i - 1].Cells[IdxPosCol++].Value = ListWells[i - 1].Concentration;
                    else
                        dataGridViewForConcentration.Rows[i - 1].Cells[IdxPosCol++].Value = 0;
                }
                else
                {
                    if (i == 1)
                        dataGridViewForConcentration.Rows[i - 1].Cells[IdxPosCol++].Value = (int)this.numericUpDownConcentrationStartingValue.Value;
                    else
                        dataGridViewForConcentration.Rows[i - 1].Cells[IdxPosCol].Value = Convert.ToDouble(dataGridViewForConcentration.Rows[i - 2].Cells[IdxPosCol++].Value.ToString()) / (double)this.numericUpDownDilutionFactor.Value;

                }
            }

            dataGridViewForConcentration.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewForConcentration.Update();
        }

        private void DrawSingleRegion(int Idx, int StartX, int StartY)
        {
            int ColorStep = 256 / (int)this.numericUpDownConcentrationNumber.Value;
            int Max = this.panelForDesignDisplay.Width / this.ListWells[0].AssociatedPlate.ParentScreening.Columns;

            if (this.radioButtonOrientationLine.Checked)
            {
                for (int i = PosXMin; i <= PosXMax; i++)
                {
                    Rectangle CurrentRect = new Rectangle((i + StartX) * SizeWell, (PosYMin + StartY) * SizeWell, SizeWell, (int)this.numericUpDownReplication.Value * SizeWell);
                    CurrBrush.Color = Color.FromArgb((i - PosXMin) * ColorStep, 0, Idx);
                    g.FillRectangle(CurrBrush, CurrentRect);
                }
            }
            else
            {
                for (int i = PosYMin; i <= PosYMax; i++)
                {
                    Rectangle CurrentRect = new Rectangle((StartX + PosXMin) * SizeWell, (i + StartY) * SizeWell, (int)this.numericUpDownReplication.Value * SizeWell, SizeWell);
                    CurrBrush.Color = Color.FromArgb((i - PosYMin) * ColorStep, 0, Idx);
                    g.FillRectangle(CurrBrush, CurrentRect);
                }
            }
        }

        public void DrawSignature()
        {
            if (ListWells == null) return;

            g = this.panelForDesignDisplay.CreateGraphics();

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.White);

            int SizeWell = 8;
            CurrBrush = new SolidBrush(Color.Fuchsia);

            PosXMin = int.MaxValue;
            PosYMin = int.MaxValue;
            PosXMax = int.MinValue;
            PosYMax = int.MinValue;


            foreach (cWell TmpWell in ListWells)
            {
                if (TmpWell.GetPosX() > PosXMax) PosXMax = TmpWell.GetPosX();
                if (TmpWell.GetPosY() > PosYMax) PosYMax = TmpWell.GetPosY();

                if (TmpWell.GetPosX() < PosXMin) PosXMin = TmpWell.GetPosX();
                if (TmpWell.GetPosY() < PosYMin) PosYMin = TmpWell.GetPosY();
            }

            int SizeX = PosXMax - PosXMin + 1;
            int SizeY = PosYMax - PosYMin + 1;

            if (this.radioButtonOrientationLine.Checked)
            {
                this.numericUpDownConcentrationNumber.Value = (decimal)SizeX;
                this.numericUpDownReplication.Value = (decimal)SizeY;
                if (PosXMin > PosXMax) return;
                TemplateRegion = new cDRC_Region(ListWells[0].AssociatedPlate, (int)this.numericUpDownConcentrationNumber.Value, (int)this.numericUpDownReplication.Value, PosXMin, PosYMin, true);
            }
            else
            {
                this.numericUpDownConcentrationNumber.Value = (decimal)SizeY;
                this.numericUpDownReplication.Value = (decimal)SizeX;
                if (PosXMin > PosXMax) return;
                TemplateRegion = new cDRC_Region(ListWells[0].AssociatedPlate, (int)this.numericUpDownConcentrationNumber.Value, (int)this.numericUpDownReplication.Value, PosXMin, PosYMin, false);
            }
            DrawSingleRegion(0, 0, 0);

            for (int j = 1; j <= ListWells[0].AssociatedPlate.ParentScreening.Columns + 1; j++)
                g.DrawLine(new Pen(Color.Black), new Point(j * SizeWell, SizeWell), new Point(j * SizeWell, (ListWells[0].AssociatedPlate.ParentScreening.Rows + 1) * SizeWell));
            for (int i = 1; i <= ListWells[0].AssociatedPlate.ParentScreening.Rows + 1; i++)
                g.DrawLine(new Pen(Color.Black), new Point(SizeWell, i * SizeWell), new Point((ListWells[0].AssociatedPlate.ParentScreening.Columns + 1) * SizeWell, i * SizeWell));
        }

        private void buttonFill_Click(object sender, EventArgs e)
        {
            int SizeX, SizeY;
            if (TemplateRegion.IsConcentrationHorizontal)
            {
                SizeX = TemplateRegion.NumConcentrations;
                SizeY = TemplateRegion.NumReplicate;
            }
            else
            {
                SizeX =  TemplateRegion.NumReplicate;
                SizeY =TemplateRegion.NumConcentrations;
            }

            int NumRepeatX = (this.ListWells[0].AssociatedPlate.ParentScreening.Columns - (TemplateRegion.PosXMin - 1)) / SizeX;
            int NumRepeatY = (this.ListWells[0].AssociatedPlate.ParentScreening.Rows - (TemplateRegion.PosYMin - 1)) / SizeY;

            int SecondColorStep = 256 / (NumRepeatX * NumRepeatY);

            if (!this.radioButtonOrientationColumn.Checked)
            {
                for (int j = 0; j < NumRepeatY; j++)
                    for (int i = 0; i < NumRepeatX; i++)
                        DrawSingleRegion((i + j * NumRepeatX) * SecondColorStep, i * SizeX, j * SizeY);
            }
            else
            {
                for (int j = 0; j < NumRepeatY; j++)
                    for (int i = 0; i < NumRepeatX; i++)
                        DrawSingleRegion((j + i * NumRepeatY) * SecondColorStep, i * SizeX, j * SizeY);
            }

            for (int j = 1; j <= ListWells[0].AssociatedPlate.ParentScreening.Columns + 1; j++)
                g.DrawLine(new Pen(Color.Black), new Point(j * SizeWell, SizeWell), new Point(j * SizeWell, (ListWells[0].AssociatedPlate.ParentScreening.Rows + 1) * SizeWell));
            for (int i = 1; i <= ListWells[0].AssociatedPlate.ParentScreening.Rows + 1; i++)
                g.DrawLine(new Pen(Color.Black), new Point(SizeWell, i * SizeWell), new Point((ListWells[0].AssociatedPlate.ParentScreening.Columns + 1) * SizeWell, i * SizeWell));
        }

        #region User Interface
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name.ToString() == "tabPageConcentrations")
                UpDateDisplayConcentration();
        }

        private void numericUpDownConcentrationNumber_ValueChanged(object sender, EventArgs e)
        {
            UpDateDisplayConcentration();
        }

        private void panelForDesignDisplay_Paint(object sender, PaintEventArgs e)
        {
            DrawSignature();
        }

        private void radioButtonOrientationLine_CheckedChanged(object sender, EventArgs e)
        {
            DrawSignature();
        }

        private void radioButtonOrientationColumn_CheckedChanged(object sender, EventArgs e)
        {
            DrawSignature();
        }

        private void radioButtonConcentrationsManual_CheckedChanged(object sender, EventArgs e)
        {
            UpDateDisplayConcentration();
        }

        private void radioButtonConcentrationFromThePlate_CheckedChanged(object sender, EventArgs e)
        {
            UpDateDisplayConcentration();
        }

        private void numericUpDownConcentrationStartingValue_ValueChanged(object sender, EventArgs e)
        {
            UpDateDisplayConcentration();
        }

        private void numericUpDownDilutionFactor_ValueChanged(object sender, EventArgs e)
        {
            UpDateDisplayConcentration();
        }
        #endregion


    }
}
