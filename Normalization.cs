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
        #region User interface
        private void comboBoxMethodForNormalization_SelectedIndexChanged(object sender, EventArgs e)
        {

            richTextBoxInfoForNormalization.Clear();

            switch (comboBoxMethodForNormalization.SelectedIndex)
            {
                case 0:
                    richTextBoxInfoForNormalization.AppendText("Single Ctrl based.\nData are all normalized to the average value of the selected class.");
                    richTextBoxInfoForNormalization.AppendText("\nFor more information, go to: http://www.nature.com/nbt/journal/v24/n2/abs/nbt1186.html");
                    comboBoxNormalizationPositiveCtrl.Enabled = false;
                    break;
                case 1:
                    richTextBoxInfoForNormalization.AppendText("Double Control Based.\nData are shifted to the average value of the negative class. Then, the values are normalized by the absolute difference between the negative and the positive means.");
                     richTextBoxInfoForNormalization.AppendText("\nFor more information, go to: http://www.nature.com/nbt/journal/v24/n2/abs/nbt1186.html");
                     comboBoxNormalizationPositiveCtrl.Enabled = true;
                    break;
                case 2:
                    richTextBoxInfoForNormalization.AppendText("Standardization.\nData are shifted to the average value of the negative class, then divided by the standard deviation of the negative class data distribution.");
                    richTextBoxInfoForNormalization.AppendText("\nFor more information, go to: http://en.wikipedia.org/wiki/Standard_score");
                    comboBoxNormalizationPositiveCtrl.Enabled = false;
                    break;

            }
        }

        private void richTextBoxInfoForNormalization_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            ClickOnLink(e.LinkText);
        }

        private void comboBoxNormalizationNegativeCtrl_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            SolidBrush BrushForColor = new SolidBrush(GlobalInfo.GetColor(e.Index));
            e.Graphics.FillRectangle(BrushForColor, e.Bounds.X + 1, e.Bounds.Y + 1, 10, 10);
            e.Graphics.DrawString(comboBoxDimReductionNeutralClass.Items[e.Index].ToString(), comboBoxDimReductionNeutralClass.Font,
                System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X + 15, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            e.DrawFocusRectangle();
        }

        private void comboBoxNormalizationPositiveCtrl_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            SolidBrush BrushForColor = new SolidBrush(GlobalInfo.GetColor(e.Index));
            e.Graphics.FillRectangle(BrushForColor, e.Bounds.X + 1, e.Bounds.Y + 1, 10, 10);
            e.Graphics.DrawString(comboBoxDimReductionNeutralClass.Items[e.Index].ToString(), comboBoxDimReductionNeutralClass.Font,
                System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X + 15, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            e.DrawFocusRectangle();
        }

        private void buttonNormalize_Click(object sender, EventArgs e)
        {
            switch (comboBoxMethodForNormalization.SelectedIndex)
            {
                case 0:
                    NegativeBasedNormalization();
                    break;
                case 1:
                    NegativePositiveBasedNormalization();
                    break;
                case 2:
                    StandardNormalization();
                    break;
                default:
                    break;
            }

            MessageBox.Show("Process finished !", "Operation sucessfull", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        #endregion

        #region Main Functions
        private void StandardNormalization()
        {
            System.Windows.Forms.DialogResult Res = MessageBox.Show("By applying this process, data will be definitively modified ! Proceed ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (Res == System.Windows.Forms.DialogResult.No) return;

            richTextBoxInfoForNormalization.Clear();

            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;
            int NumberOfProcessedPlates = 0;
            // loop on all the plate
            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(PlateIdx);

                if (CurrentPlateToProcess.GetNumberOfWellOfClass(comboBoxNormalizationNegativeCtrl.SelectedIndex) < 2)
                {
                    richTextBoxInfoForNormalization.AppendText("\n" + CurrentPlateToProcess.Name + " does not contain enough well of the selected class. Plate skipped.");
                    continue;
                }


               // GlobalInfo.ConsoleWriteLine("Plate: " + CurrentPlateToProcess.Name);
                cExtendedList Neg = new cExtendedList();
                int NumDesc = CompleteScreening.ListDescriptors.Count;

                cWell TempWell;
                // loop on all the desciptors
                for (int Desc = 0; Desc < NumDesc; Desc++)
                {
                    Neg.Clear();

                    if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;

                    for (int row = 0; row < CompleteScreening.Rows; row++)
                        for (int col = 0; col < CompleteScreening.Columns; col++)
                        {
                            TempWell = CurrentPlateToProcess.GetWell(col, row, true);
                            if (TempWell == null) continue;
                            if (TempWell.GetClass() == comboBoxNormalizationNegativeCtrl.SelectedIndex) Neg.Add(TempWell.ListDescriptors[Desc].GetValue());
                        }

                    double CurrentMean = Neg.Mean();
                    double CurrentStd = Neg.Std();
                    if (CurrentStd == 0.0)
                    {
                        richTextBoxInfoForNormalization.AppendText("\n" + CurrentPlateToProcess.Name + " - " + CompleteScreening.ListDescriptors[Desc].GetName() + ", Standard deviation = 0, process cancelled");
                        goto NEXTPLATE;

                    }
                    GlobalInfo.ConsoleWriteLine(CompleteScreening.ListDescriptors[Desc].GetName() + ", average = " + CurrentMean);

                    for (int row = 0; row < CompleteScreening.Rows; row++)
                        for (int col = 0; col < CompleteScreening.Columns; col++)
                        {
                            TempWell = CurrentPlateToProcess.GetWell(col, row, true);
                            if (TempWell == null) continue;
                            for (int i = 0; i < TempWell.ListDescriptors[Desc].GetAssociatedType().GetBinNumber(); i++)
                            {
                                double Value = TempWell.ListDescriptors[Desc].Getvalue(i) - CurrentMean;
                                TempWell.ListDescriptors[Desc].SetHistoValues(Value / CurrentStd);
                            }

                            TempWell.ListDescriptors[Desc].UpDateDescriptorStatistics();
                        }

                    CurrentPlateToProcess.UpDataMinMax();
                 
                    
                }  
                richTextBoxInfoForNormalization.AppendText("\n" + CurrentPlateToProcess.Name + " successfully normalized.");
                NumberOfProcessedPlates++;
            NEXTPLATE: ;
            }
            richTextBoxInfoForNormalization.AppendText("\n" + NumberOfProcessedPlates + " / " + NumberOfPlates + " successfully normalized.");
        }

        private void NegativeBasedNormalization()
        {
            System.Windows.Forms.DialogResult Res = MessageBox.Show("By applying this process, data will be definitively modified ! Proceed ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (Res == System.Windows.Forms.DialogResult.No) return;

            richTextBoxInfoForNormalization.Clear();

            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;


            int NumberOfProcessedPlates = 0;
            // loop on all the plate
            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);
             //   GlobalInfo.ConsoleWriteLine("Plate: " + CurrentPlateToProcess.Name);

                if (CurrentPlateToProcess.GetNumberOfWellOfClass(comboBoxNormalizationNegativeCtrl.SelectedIndex) == 0)
                {
                    richTextBoxInfoForNormalization.AppendText("\n"+ CurrentPlateToProcess.Name + " does not contain well of the selected class. Plate skipped.");
                    continue;
                }


                cExtendedList Neg = new cExtendedList();

                int NumDesc = CompleteScreening.ListDescriptors.Count;

                cWell TempWell;
                // loop on all the desciptors
                for (int Desc = 0; Desc < NumDesc; Desc++)
                {
                    Neg.Clear();

                    if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;

                    for (int row = 0; row < CompleteScreening.Rows; row++)
                        for (int col = 0; col < CompleteScreening.Columns; col++)
                        {
                            TempWell = CurrentPlateToProcess.GetWell(col, row, true);
                            if (TempWell == null) continue;
                            if (TempWell.GetClass() == comboBoxNormalizationNegativeCtrl.SelectedIndex) Neg.Add(TempWell.ListDescriptors[Desc].GetValue());

                           
                        }

                    double CurrentMean = Neg.Mean();
                    GlobalInfo.ConsoleWriteLine(CompleteScreening.ListDescriptors[Desc].GetName() + ", average = " + CurrentMean);

                    if (CurrentMean == 0)
                    {
                        richTextBoxInfoForNormalization.AppendText("\n" + CurrentPlateToProcess.Name + " / " + CompleteScreening.ListDescriptors[Desc].GetName() + " average is null!\n");
                        richTextBoxInfoForNormalization.AppendText("\nNormalization skipped.");
                        continue;
                    }

                    for (int row = 0; row < CompleteScreening.Rows; row++)
                        for (int col = 0; col < CompleteScreening.Columns; col++)
                        {
                            TempWell = CurrentPlateToProcess.GetWell(col, row, true);
                            if (TempWell == null) continue;
                            for (int i = 0; i < TempWell.ListDescriptors[Desc].GetAssociatedType().GetBinNumber(); i++)
                            {
                                double Val = TempWell.ListDescriptors[Desc].Getvalue(i);
                                Val /= CurrentMean;
                                TempWell.ListDescriptors[Desc].SetHistoValues(i,Val*100);
                            }

                            TempWell.ListDescriptors[Desc].UpDateDescriptorStatistics();
                        }

                    CurrentPlateToProcess.UpDataMinMax();
                  
                }
                richTextBoxInfoForNormalization.AppendText("\n" + CurrentPlateToProcess.Name + " successfully normalized.");
                NumberOfProcessedPlates++;
            }
            richTextBoxInfoForNormalization.AppendText("\n" + NumberOfProcessedPlates + " / " + NumberOfPlates + " successfully normalized.");
        }

        private void NegativePositiveBasedNormalization()
        {
            System.Windows.Forms.DialogResult Res = MessageBox.Show("By applying this process, data will be definitively modified ! Proceed ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (Res == System.Windows.Forms.DialogResult.No) return;

            richTextBoxInfoForNormalization.Clear();

            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;
            int NumberOfProcessedPlates = 0;
            // loop on all the plate
            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);
               // GlobalInfo.ConsoleWriteLine("Plate: " + CurrentPlateToProcess.Name);

                if ((CurrentPlateToProcess.GetNumberOfWellOfClass(comboBoxNormalizationNegativeCtrl.SelectedIndex)==0)||((CurrentPlateToProcess.GetNumberOfWellOfClass(comboBoxNormalizationPositiveCtrl.SelectedIndex)==0)))
                {
                    richTextBoxInfoForNormalization.AppendText("\n"+ CurrentPlateToProcess.Name + " does not contain well of the selected classes. Plate skipped.");
                    continue;
                }

                cExtendedList Neg = new cExtendedList();
                cExtendedList Pos = new cExtendedList();
                int NumDesc = CompleteScreening.ListDescriptors.Count;

                cWell TempWell;
                // loop on all the desciptors
                for (int Desc = 0; Desc < NumDesc; Desc++)
                {
                    Neg.Clear();
                    if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;

                    for (int row = 0; row < CompleteScreening.Rows; row++)
                        for (int col = 0; col < CompleteScreening.Columns; col++)
                        {
                            TempWell = CurrentPlateToProcess.GetWell(col, row, true);
                            if (TempWell == null) continue;
                            if (TempWell.GetClass() == comboBoxNormalizationPositiveCtrl.SelectedIndex) Pos.Add(TempWell.ListDescriptors[Desc].GetValue());
                            if (TempWell.GetClass() == comboBoxNormalizationNegativeCtrl.SelectedIndex) Neg.Add(TempWell.ListDescriptors[Desc].GetValue());
                        }

                    double CurrentMeanNeg = Neg.Mean();
                    double CurrentMeanPos = Pos.Mean();

                    if (CurrentMeanNeg == CurrentMeanPos)
                    {
                        GlobalInfo.ConsoleWriteLine("Negative and positive are similar, no normalization operated on " + CompleteScreening.ListDescriptors[Desc].GetName());
                        continue;
                    }
                    double Denominator = (Math.Abs(CurrentMeanPos - CurrentMeanNeg));

                    for (int row = 0; row < CompleteScreening.Rows; row++)
                        for (int col = 0; col < CompleteScreening.Columns; col++)
                        {
                            TempWell = CurrentPlateToProcess.GetWell(col, row, true);
                            if (TempWell == null) continue;
                            for (int i = 0; i < TempWell.ListDescriptors[Desc].GetAssociatedType().GetBinNumber(); i++)
                            {
                                double CurrValue = TempWell.ListDescriptors[Desc].Getvalue(i);
                                TempWell.ListDescriptors[Desc].SetHistoValues(i, (CurrValue - CurrentMeanNeg) / Denominator);
                            }
                            TempWell.ListDescriptors[Desc].UpDateDescriptorStatistics();
                        }

                    CurrentPlateToProcess.UpDataMinMax();
                   
                } 
                richTextBoxInfoForNormalization.AppendText("\n" + CurrentPlateToProcess.Name + " successfully normalized.");
                NumberOfProcessedPlates++;
            }
            richTextBoxInfoForNormalization.AppendText("\n" + NumberOfProcessedPlates + " / " + NumberOfPlates + " successfully normalized.");

        }
        #endregion
    }
}
