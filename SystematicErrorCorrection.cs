using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using weka.core;
using System.Data;
using weka.clusterers;
using LibPlateAnalysis;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Windows.Forms;
using weka.classifiers;
using HCSAnalyzer.Classes;

namespace HCSAnalyzer
{
    public partial class HCSAnalyzer
    {
        #region Systematic Error Correction
        private void comboBoxMethodForCorrection_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            richTextBoxInformationForPlateCorrection.Clear();

            switch (comboBoxMethodForCorrection.SelectedIndex)
            {
                case 0:
                    richTextBoxInformationForPlateCorrection.AppendText("B-Score.\n");
                    break;
                case 1:
                    richTextBoxInformationForPlateCorrection.AppendText("Diffusion Model.\nThis approach focus exclusively on correcting edge effects.\n\nFor more information, go to: http://bioinformatics.oxfordjournals.org/content/early/2011/11/26/bioinformatics.btr648.short?rss=1");
                    break;
            }
        }

        private void richTextBoxInformationForPlateCorrection_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            if (GlobalInfo.OptionsWindow.radioButtonIE.Checked)
                proc.StartInfo.FileName = "iexplore";
            else
                proc.StartInfo.FileName = "chrome";
            proc.StartInfo.Arguments = e.LinkText;
            proc.Start();
        }

        private void buttonCorrectionPlateByPlate_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;
            System.Windows.Forms.DialogResult Res = MessageBox.Show("By applying this process, data will be definitively modified ! Proceed ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            this.Cursor = Cursors.WaitCursor;


            if (comboBoxMethodForCorrection.SelectedIndex == 0)             // B-score correction
            {
                BScoreWrapper BSCore = new BScoreWrapper();

                int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;
                int NumDesc = CompleteScreening.ListDescriptors.Count;

                string MissingWellError = "";

                // loop on every plate
                for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
                {
                    cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);

                    if (CompleteScreening.ListDescriptors[0].IsActive() == false) continue;

                    bool IsMissingWell;
                    double[,] Table = CurrentPlateToProcess.GetAverageValueDescTable(0, out IsMissingWell);
                    if (IsMissingWell) MissingWellError += CurrentPlateToProcess.Name + "\n";

                    BSCore.BScore(Table);
                    CurrentPlateToProcess.SetAverageValueDescTable(0, Table);

                    if (NumDesc >= 1)
                    {
                        for (int Desc = 1; Desc < NumDesc; Desc++)
                        {
                            if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;

                            Table = CurrentPlateToProcess.GetAverageValueDescTable(Desc, out IsMissingWell);
                            // correct the plate
                            BSCore.BScore(Table);
                            CurrentPlateToProcess.SetAverageValueDescTable(Desc, Table);
                        }
                    }
                }
                if (MissingWellError.Length > 0)
                    MessageBox.Show("Well(s) were missing on the following plate: \n" + MissingWellError + "A null value has been use instead for the correction purpose !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Process finished !", "Operation sucessfull", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (comboBoxMethodForCorrection.SelectedIndex == 1)        // Diffusion Correction
            {
                cEdgeEffect EdgeEffect = new cEdgeEffect(CompleteScreening, (int)GlobalInfo.OptionsWindow.numericUpDownEdgeEffectMaximumIteration.Value);

                int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;
                int NumDesc = CompleteScreening.ListDescriptors.Count;
                // loop on every plate
                for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
                {
                    cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);

                    richTextBoxInformationForPlateCorrection.AppendText("\nPlate: " + CurrentPlateToProcess.Name);

                    for (int Desc = 0; Desc < NumDesc; Desc++)
                    {
                        if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;

                        bool IsMissingWell;
                        double[,] Table = CurrentPlateToProcess.GetAverageValueDescTable(Desc, out IsMissingWell);
                        // identify the background
                        int BestIteration = EdgeEffect.FindIterationForBestMatch(Table);
                        double[] ShiftMult = EdgeEffect.FindBestShiftMultCoeff(Table, BestIteration);


                        // correct the plate
                        if (BestIteration != 0)
                        {
                            double[,] CorrectedTable = EdgeEffect.CorrectThePlate(Table, BestIteration, ShiftMult[0], ShiftMult[1]);
                            CurrentPlateToProcess.SetAverageValueDescTable(Desc, CorrectedTable);
                        }
                        richTextBoxInformationForPlateCorrection.AppendText("\n" + CompleteScreening.ListDescriptors[Desc].GetName() + " :");
                        richTextBoxInformationForPlateCorrection.AppendText("\n\tDiffusion iteration: " + BestIteration);
                        richTextBoxInformationForPlateCorrection.AppendText("\n\tDiffusion shift: " + ShiftMult[0]);
                        richTextBoxInformationForPlateCorrection.AppendText("\n\tDiffusion multiplicative coeff.: " + ShiftMult[0]);

                    }
                }
                
                richTextBoxInformationForPlateCorrection.AppendText("\n--------------------------------\nProcess over !\n--------------------------------\n");
            }
            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);
            this.Cursor = Cursors.Default;

        }



        #endregion

        #region Rejection functions
        private void comboBoxRejection_SelectedIndexChanged(object sender, EventArgs e)
        {

            richTextBoxInformationRejection.Clear();

            switch (comboBoxMethodForCorrection.SelectedIndex)
            {
                case 0:
                    richTextBoxInformationRejection.AppendText("Z-Score based rejection.\n\nRemove plates with a lower Z-factor value than defined by the thresold.\nFor more information, go to: http://en.wikipedia.org/wiki/Z-factor");
                    break;
                case 1:
                    // richTextBoxSupervisedDimRec.AppendText("Chi Square.\n");
                    break;
            }
        }

        private void richTextBoxInformationRejection_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            if (GlobalInfo.OptionsWindow.radioButtonIE.Checked)
                proc.StartInfo.FileName = "iexplore";
            else
                proc.StartInfo.FileName = "chrome";
            proc.StartInfo.Arguments = e.LinkText;
            proc.Start();
        }

        private void buttonRejectPlates_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            List<double> Pos = new List<double>();
            List<double> Neg = new List<double>();
            List<cSimpleSignature> ZFactorList = new List<cSimpleSignature>();

            cWell TempWell;
            int Desc = this.comboBoxDescriptorToDisplay.SelectedIndex;

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
                            if (TempWell.GetClass() == comboBoxRejectionPositiveCtrl.SelectedIndex)
                                Pos.Add(TempWell.ListDescriptors[Desc].GetValue());
                            if (TempWell.GetClass() == comboBoxRejectionNegativeCtrl.SelectedIndex)
                                Neg.Add(TempWell.ListDescriptors[Desc].GetValue());
                        }
                    }

                if (Pos.Count < 3)
                {
                    richTextBoxInformationRejection.AppendText("\nPlate: " + CurrentPlateToProcess.Name + ". No or not enough positive controls !");
                    continue;
                }
                if (Neg.Count < 3)
                {
                    richTextBoxInformationRejection.AppendText("\nPlate: " + CurrentPlateToProcess.Name + ". No or not enough negative controls !");
                    continue;
                }

                double ZScore = 1 - 3 * (std(Pos.ToArray()) + std(Neg.ToArray())) / (Math.Abs(Mean(Pos.ToArray()) - Mean(Neg.ToArray())));
                if (double.IsNaN(ZScore))
                {
                    richTextBoxInformationRejection.AppendText("\nFailed to estimate Z' factor on plate " + CurrentPlateToProcess.Name + " => rejected");
                }
                cSimpleSignature TmpDesc = new cSimpleSignature(CurrentPlateToProcess.Name, ZScore);
                ZFactorList.Add(TmpDesc);
            }

            int NumDesc = CompleteScreening.ListDescriptors.Count;
            int PlateRejected = 0;

            richTextBoxInformationRejection.Clear();
            richTextBoxInformationRejection.AppendText("\nDescriptor: "+CompleteScreening.ListDescriptors[Desc].GetName()+"\n");

            foreach (cSimpleSignature TmpZFactor in ZFactorList)
            {
                if (TmpZFactor.AverageValue <= (float)numericUpDownRejectionThreshold.Value) PlateRejected++;
            }
            if (PlateRejected == ZFactorList.Count)
            {
                MessageBox.Show("By applying such rejection scheme, all the plates would be removed.\n", "Process cancelled !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PlateRejected = 0;
            foreach (cSimpleSignature TmpZFactor in ZFactorList)
            {

                richTextBoxInformationRejection.AppendText("\n" + TmpZFactor.Name + ": Z' factor = " + TmpZFactor.AverageValue.ToString("N3"));
                if (TmpZFactor.AverageValue <= (float)numericUpDownRejectionThreshold.Value)
                {
                    richTextBoxInformationRejection.AppendText(" <= " + (float)numericUpDownRejectionThreshold.Value + " => rejected!");
                    CompleteScreening.ListPlatesActive.Remove(CompleteScreening.ListPlatesAvailable.GetPlate(ZFactorList[PlateRejected].Name));
                    toolStripcomboBoxPlateList.Items.Remove(ZFactorList[PlateRejected].Name);
                }
                else
                {
                    richTextBoxInformationRejection.AppendText(" > " + (float)numericUpDownRejectionThreshold.Value + " => preserved!");
                }

                PlateRejected++;
            }

            RefreshInfoScreeningRichBox();
            CompleteScreening.CurrentDisplayPlateIdx = 0;
            if(toolStripcomboBoxPlateList.Items.Count>0)
                toolStripcomboBoxPlateList.SelectedIndex = 0;

            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);

        }

        private void comboBoxRejectionNegativeCtrl_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            SolidBrush BrushForColor = new SolidBrush(GlobalInfo.GetColor(e.Index));
            e.Graphics.FillRectangle(BrushForColor, e.Bounds.X + 1, e.Bounds.Y + 1, 10, 10);
            e.Graphics.DrawString(comboBoxDimReductionNeutralClass.Items[e.Index].ToString(), comboBoxDimReductionNeutralClass.Font,
                System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X + 15, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            e.DrawFocusRectangle();
        }

        private void comboBoxRejectionPositiveCtrl_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            SolidBrush BrushForColor = new SolidBrush(GlobalInfo.GetColor(e.Index));
            e.Graphics.FillRectangle(BrushForColor, e.Bounds.X + 1, e.Bounds.Y + 1, 10, 10);
            e.Graphics.DrawString(comboBoxDimReductionNeutralClass.Items[e.Index].ToString(), comboBoxDimReductionNeutralClass.Font,
                System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X + 15, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            e.DrawFocusRectangle();
        }

        #endregion
    }
}
