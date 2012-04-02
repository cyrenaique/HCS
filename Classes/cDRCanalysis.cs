using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibPlateAnalysis;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using HCSAnalyzer.Forms.FormsForOptions;
using System.Windows.Forms;
using System.IO;
using HCSAnalyzer.Forms.FormsForDRCAnalysis;

namespace HCSAnalyzer.Classes
{
    /// <summary>
    /// Class to display a list of DRC in a single control
    /// </summary>
    public class cDRCDisplay
    {
        public Chart CurrentChart = new Chart();
        public RichTextBox CurrentRichTextBox = new RichTextBox();

        public int ChartSizeX = 500;
        public int ChartSizeY = 300;


        //NewDRC.CurrentRichTextBox.Location = new Point(NewDRC.CurrentChart.Location.X + NewDRC.CurrentChart.Width + 50, NewDRC.CurrentChart.Location.Y);
        //NewDRC.CurrentRichTextBox.Width = 300;
        //NewDRC.CurrentRichTextBox.Height = NewDRC.CurrentChart.Height;

        //WindowforDRCsDisplay.LRichTextBox.Add(NewDRC.CurrentRichTextBox);





        public cDRCDisplay(List<cDRC> DRCstoDisplay, cGlobalInfo GlobalInfo)
        {

            Legend legend1 = new Legend();
            legend1.Name = "Legend1";


            ChartArea TmpChartArea = new ChartArea();

         //   TmpChartArea.AxisX.IsLogarithmic = true;
            TmpChartArea.AxisX.MajorGrid.Enabled = false;
            TmpChartArea.AxisY.MajorGrid.Enabled = false;



            int IdxCurve = 0;

            foreach (cDRC CurrentDRC in DRCstoDisplay)
            {

                if (CurrentDRC.ResultFit == null) continue;

                Series CurrentSeriesForSpline = new Series("[" + CurrentDRC.AssociatedDRCRegion.PosXMin + ":" + CurrentDRC.AssociatedDRCRegion.PosYMin + "]" + CurrentDRC.AssociatedDescriptorType.GetName() + " Interpolated");
                CurrentSeriesForSpline.ChartType = SeriesChartType.Spline;
                CurrentSeriesForSpline.BorderWidth = 2;
                CurrentSeriesForSpline.Color = GlobalInfo.ColorForDRCCurves[IdxCurve % 9];
                Series CurrentSeriesForRealValues = new Series("[" + CurrentDRC.AssociatedDRCRegion.PosXMin + ":" + CurrentDRC.AssociatedDRCRegion.PosYMin + "]" + CurrentDRC.AssociatedDescriptorType.GetName());
                CurrentSeriesForRealValues.ChartType = SeriesChartType.Point;

                CurrentSeriesForRealValues.Color = GlobalInfo.ColorForDRCCurves[IdxCurve % 9];

                int IdxValue_ = 0;

                for (int IdxConc = 0; IdxConc < CurrentDRC.AssociatedDRCRegion.NumConcentrations; IdxConc++)
                {
                    if ((IdxConc >= CurrentDRC.ResultFit.ConcentrationValues.Count) || (IdxConc >= CurrentDRC.ResultFit.Y_Estimated.Count)) continue;

                    CurrentSeriesForSpline.Points.AddXY(CurrentDRC.ResultFit.GetLogConcentrations()[IdxConc], CurrentDRC.ResultFit.Y_Estimated[IdxConc]);
                    for (int IdxRep = 0; IdxRep < CurrentDRC.AssociatedDRCRegion.NumReplicate; IdxRep++)
                    {
                        CurrentSeriesForRealValues.Points.AddXY(CurrentDRC.ResultFit.GetLogConcentrations()[IdxConc],
                        CurrentDRC.ResultFit.Y_RawData[IdxRep][IdxConc]);

                        CurrentSeriesForRealValues.Points[IdxValue_].MarkerStyle = (MarkerStyle)(IdxCurve % 9);
                        // CurrentSeriesForRealValues.Points[IdxValue_].ToolTip = CurrentWell.GetPosX() + " x " + CurrentWell.GetPosY() + " : " + Values[IdxValue_];
                        CurrentSeriesForRealValues.Points[IdxValue_++].MarkerSize = 8;
                    }
                }

                IdxCurve++;
                CurrentSeriesForRealValues.Legend = "Legend1";
                this.CurrentChart.Series.Add(CurrentSeriesForRealValues);
                this.CurrentChart.Series.Add(CurrentSeriesForSpline);

            }
            CurrentChart.Legends.Add(legend1);
            CurrentChart.ChartAreas.Add(TmpChartArea);
            CurrentChart.Height = CurrentRichTextBox.Height = ChartSizeY;
            CurrentChart.Width = ChartSizeX;
            CurrentRichTextBox.Width = CurrentChart.Width;
            CurrentRichTextBox.Height = 150;
            for (int i = 0; i < DRCstoDisplay.Count; i++)
            {
                this.CurrentRichTextBox.AppendText("Descriptor: " + DRCstoDisplay[i].AssociatedDescriptorType.GetName() + "\n\n");
                this.CurrentRichTextBox.AppendText("Error Relative: " + DRCstoDisplay[i].RelativeError + "\n");
                this.CurrentRichTextBox.AppendText("Bottom: " + DRCstoDisplay[i].Bottom + "\n"); this.CurrentRichTextBox.AppendText("Top: " + DRCstoDisplay[i].Top + "\n");
                this.CurrentRichTextBox.AppendText("EC50: " + DRCstoDisplay[i].EC50.ToString("E2") + "\n"); this.CurrentRichTextBox.AppendText("Slope: " + DRCstoDisplay[i].Slope + "\n");
                this.CurrentRichTextBox.AppendText("--------------------------------------\n");
            }



        }

        public Chart GetChart()
        {

            return this.CurrentChart;
        }

        public RichTextBox GetRichBox()
        {

            return this.CurrentRichTextBox;
        }

    }




    public class cListDRCRegion : List<cDRC_Region>
    {
        public void AddNewRegion(cDRC_Region NewRegion)
        {
            this.Add(NewRegion);

        }


    }


    /// <summary>
    /// Class for a single Region of connected wells defining a DRC
    /// </summary>
    public class cDRC_Region
    {
        public cPlate AssociatedPlate = null;

        private cWell[][] ListWells = null;

        public cWell[][] GetListWells()
        {
            return this.ListWells;
        }

        public cWell[] GetlistReplicate(int IdxReplicate)
        {
            if (IdxReplicate >= this.ListWells.Length) return null;
            return this.ListWells[IdxReplicate];
        }

        public int PosXMin = -1;
        public int PosYMin = -1;
        /// <summary>
        /// Specify the mode of the DRC region: True: row, False: column
        /// </summary>
        public bool IsConcentrationHorizontal = true;
        public int NumConcentrations = 0;
        public int NumReplicate = 0;


        public List<int> GetListRespondingDescritpors(cScreening CurrentScreen, FormForDRCSelection WindowDRCSelection)
        {
            List<int> ListRespondingDescritpors = new List<int>();

            for (int IdxDesc = 0; IdxDesc < CurrentScreen.ListDescriptors.Count; IdxDesc++)
            {
                cDescriptorsType Desc = CurrentScreen.ListDescriptors[IdxDesc];
                if (!Desc.IsActive())
                {
                    ListRespondingDescritpors.Add(-1);
                    continue;
                }

                cDRC CurrentDRC = this.GetDRC(Desc);
                ListRespondingDescritpors.Add(CurrentDRC.IsResponding(WindowDRCSelection));
                
                //    ListRespondingDescritpors.Add(1);
                //else
                //    ListRespondingDescritpors.Add(0);
            }
            return ListRespondingDescritpors;
        }


        public int SizeX;
        public int SizeY;
        //  public cListDRC ListDRC = new cListDRC();

        public cDRC GetDRC(cDescriptorsType DescriptorType)
        {
            cDRC DRCtoReturn = new cDRC(this, DescriptorType);



            return DRCtoReturn;
        }


        public cDRC_Region(cPlate AssociatedPlate, int NumConcentrations, int NumReplicate, int PosXMin, int PosYMin, bool IsConcentrationHorizontal)
        {
            this.AssociatedPlate = AssociatedPlate;

            this.NumConcentrations = NumConcentrations;
            this.NumReplicate = NumReplicate;
            this.PosXMin = PosXMin;
            this.PosYMin = PosYMin;
            this.IsConcentrationHorizontal = IsConcentrationHorizontal;




            if (IsConcentrationHorizontal)
            {
                SizeX = NumConcentrations;
                SizeY = NumReplicate;

                this.ListWells = new cWell[this.NumReplicate][];
                for (int i = 0; i < this.NumReplicate; i++)
                    this.ListWells[i] = new cWell[this.NumConcentrations];


                for (int j = PosYMin; j < PosYMin + SizeY; j++)
                    for (int i = PosXMin; i < PosXMin + SizeX; i++)
                        this.ListWells[j - PosYMin][i - PosXMin] = this.AssociatedPlate.GetWell(i, j, false);
            }
            else
            {
                SizeX = NumReplicate;
                SizeY = NumConcentrations;

                this.ListWells = new cWell[this.NumReplicate][];
                for (int i = 0; i < this.NumReplicate; i++)
                    this.ListWells[i] = new cWell[this.NumConcentrations];


                for (int j = PosXMin; j < PosXMin + SizeX; j++)
                    for (int i = PosYMin; i < PosYMin + SizeY; i++)
                        this.ListWells[j - PosXMin][i - PosYMin] = this.AssociatedPlate.GetWell(j, i, false);
            }

            //int NumDesc = AssociatedPlate.ParentScreening.GetNumberOfActiveDescriptor();
            //foreach (cDescriptorsType DescType in AssociatedPlate.ParentScreening.ListDescriptors)
            //{
            //    ListDRC.Add(new cDRC(this, DescType));
            //}
        }

    }

    /// <summary>
    /// Define a List of DRC
    /// </summary>
    public class cListDRC : List<cDRC>
    {



    }


    public class cResultFit
    {
        public cExtendedList ConcentrationValues = new cExtendedList();
        public cExtendedList Y_Estimated = new cExtendedList();
        public List<double[]> Y_RawData = new List<double[]>();


        public List<double> GetLogConcentrations()
        {
            List<double> LogConcentration = new List<double>();

            foreach (double Val in this.ConcentrationValues)
            {
                LogConcentration.Add(Math.Log10(Val));
            }
            return LogConcentration;
        }


        public cExtendedList GetNormalizedY_Estimated()
        {
            cExtendedList NormalizedY_Estimated = new cExtendedList();

            double Max = double.MinValue;
            double Min = double.MaxValue;

            foreach (double[] listVal in Y_RawData)
            {
                for(int i=0;i<listVal.Length;i++)
                {
                    if (listVal[i] >= Max) Max = listVal[i];
                    if (listVal[i] <= Min) Min = listVal[i];
                }
            }

            if (Min == Max) return Y_Estimated;

            foreach (double Val in Y_Estimated)
            {
                NormalizedY_Estimated.Add((Val - Min)/(Max-Min));
            }

            return NormalizedY_Estimated;
        }



    }


    /// <summary>
    /// Class for a single DRC curve
    /// </summary>
    public class cDRC
    {
        public cDescriptorsType AssociatedDescriptorType = null;
        public cDRC_Region AssociatedDRCRegion = null;

        public cResultFit ResultFit;

        int DescIdx = -1;

        public cDRC(cDRC_Region DRC_Region, cDescriptorsType DescriptorType)
        {
            this.AssociatedDescriptorType = DescriptorType;
            this.AssociatedDRCRegion = DRC_Region;
            DescIdx = this.AssociatedDRCRegion.AssociatedPlate.ParentScreening.ListDescriptors.GetDescriptorIndex(AssociatedDescriptorType);
            if (DescIdx == -1) return;


            FitToSigmoid();
        }

        public double Top;
        public double Bottom;
        public double Slope;
        public double EC50;
        public double RelativeError;

        #region Fitting functions
        private T MinA<T>(T[] rest) where T : IComparable
        {
            T min = rest[0];
            foreach (T f in rest) if (f.CompareTo(min) < 0)
                    min = f;
            return min;
        }

        private T MaxA<T>(T[] rest) where T : IComparable
        {
            T max = rest[0];
            foreach (T f in rest) if (f.CompareTo(max) > 0)
                    max = f;
            return max;
        }


        private static void function_SigmoidInhibition(double[] c, double[] x, ref double func, object obj)
        {
            func = c[0] + (c[1] - c[0]) / (1 + Math.Pow(((Math.Pow(10, c[2]) / Math.Pow(10,x[0]))), c[3]));
        }

        #endregion

        private void FitToSigmoid()
        {
            ResultFit = new cResultFit();

            // fill out the concentration
            for (int IdxConc = 0; IdxConc < AssociatedDRCRegion.NumConcentrations; IdxConc++)
            {
                if (AssociatedDRCRegion.GetlistReplicate(0)[IdxConc] == null) continue;

                if (AssociatedDRCRegion.GetlistReplicate(0)[IdxConc].GetClass() != -1)
                    ResultFit.ConcentrationValues.Add(AssociatedDRCRegion.GetlistReplicate(0)[IdxConc].Concentration);
            }

            ResultFit.Y_RawData = new List<double[]>();
            for (int idxRep = 0; idxRep < AssociatedDRCRegion.NumReplicate; idxRep++)
            {
                ResultFit.Y_RawData.Add(new double[AssociatedDRCRegion.NumConcentrations]);

                int NumWellToProcess = 0;
                for (int IdxConc = 0; IdxConc < AssociatedDRCRegion.NumConcentrations; IdxConc++)
                {
                    if (AssociatedDRCRegion.GetlistReplicate(idxRep)[IdxConc] == null) continue;
                    if (AssociatedDRCRegion.GetlistReplicate(idxRep)[IdxConc].GetClass() != -1)
                        ResultFit.Y_RawData[idxRep][IdxConc] = AssociatedDRCRegion.GetlistReplicate(idxRep)[IdxConc].ListDescriptors[DescIdx].GetValue();
                    NumWellToProcess++;
                }
            }

            if (ResultFit.ConcentrationValues.Count <= 0)
            {
                ResultFit = null;
                return;
            }

            double MinConcentration = MinA(ResultFit.GetLogConcentrations().ToArray());
            double MaxConcentration = MaxA(ResultFit.GetLogConcentrations().ToArray());
            if (MinConcentration == MaxConcentration) return;

            double GlobalMax = double.MinValue;
            for (int IdxRepl = 0; IdxRepl < AssociatedDRCRegion.NumReplicate; IdxRepl++)
            {
                double MaxValues = MaxA(ResultFit.Y_RawData[IdxRepl]);
                if (MaxValues > GlobalMax) GlobalMax = MaxValues;
            }

            double GlobalMin = double.MaxValue;
            for (int IdxRepl = 0; IdxRepl < AssociatedDRCRegion.NumReplicate; IdxRepl++)
            {
                double MinValues = MinA(ResultFit.Y_RawData[IdxRepl]);
                if (MinValues < GlobalMin) GlobalMin = MinValues;
            }

            if (GlobalMax == GlobalMin) return;

            double BaseEC50 = MaxConcentration- Math.Abs(MaxConcentration - MinConcentration) / 2.0;

            double[] c = new double[] { GlobalMin, GlobalMax, BaseEC50, 1 };
            double epsf = 0;
            double epsx = 0;
            int maxits = 0;
            int info;

            // boundaries
            // bottom / top / EC50 / Slope
            double[] bndl = null;
            double[] bndu = null;

            // boundaries
            bndu = new double[] { GlobalMax, GlobalMax,MaxConcentration, 5 };
            bndl = new double[] { GlobalMin, GlobalMin,MinConcentration, -5 };

            // c = new double[] { 0, 0, 0, 1 };
            // s = new double[] { 1, 1, 1.0e-9, 1 };

            // bndl = new double[] { 0, 100, 0.1e-12, 0 };
            //bndu = new double[] { 300, 1000, 40e-6, 5 };
            alglib.lsfitstate state;
            alglib.lsfitreport rep;
            double diffstep = 1e-12;

            // Fitting without weights
            //alglib.lsfitcreatefg(Concentrations, Values.ToArray(), c, false, out state);

            int NumDimension = 1;
            double[,] ConcentrationAlglib = new double[AssociatedDRCRegion.NumConcentrations * AssociatedDRCRegion.NumReplicate, NumDimension];
            double[] RawValuesForAlglib = new double[AssociatedDRCRegion.NumConcentrations * AssociatedDRCRegion.NumReplicate];

            for (int IdxRep = 0; IdxRep < AssociatedDRCRegion.NumReplicate; IdxRep++)
            {
                for (int IdxConc = 0; IdxConc < AssociatedDRCRegion.NumConcentrations; IdxConc++)
                {
                    if (IdxConc >= ResultFit.ConcentrationValues.Count) continue;
                    ConcentrationAlglib[IdxConc + IdxRep * AssociatedDRCRegion.NumConcentrations, 0] = ResultFit.GetLogConcentrations()[IdxConc];// AssociatedDRCRegion.GetlistReplicate(IdxRep)[IdxConc].Concentration;
                    RawValuesForAlglib[IdxConc + IdxRep * AssociatedDRCRegion.NumConcentrations] = ResultFit.Y_RawData[IdxRep][IdxConc];
                }
            }

            alglib.lsfitcreatef(ConcentrationAlglib, RawValuesForAlglib, c, diffstep, out state);
            alglib.lsfitsetcond(state, epsf, epsx, maxits);
            alglib.lsfitsetbc(state, bndl, bndu);
            // alglib.lsfitsetscale(state, s);

            alglib.lsfitfit(state, function_SigmoidInhibition, null, null);
            alglib.lsfitresults(state, out info, out c, out rep);
            this.RelativeError = rep.avgrelerror;
            if (c[0] >= c[1])
            {
                this.Top = c[0];
                this.Bottom = c[1];
            }
            else
            {
                this.Top = c[1];
                this.Bottom = c[0];
            }
            this.EC50 = c[2];
            this.Slope = c[3];

            ResultFit.Y_Estimated = new cExtendedList();
            for (int IdxConc = 0; IdxConc < ResultFit.ConcentrationValues.Count; IdxConc++)
            {
                ResultFit.Y_Estimated.Add(c[0] + (c[1] - c[0]) / (1 + Math.Pow((Math.Pow(10, c[2]) / Math.Pow(10,ResultFit.GetLogConcentrations()[IdxConc])), c[3])));
             //   ResultFit.Y_Estimated.Add(c[0] + (c[1] - c[0]) / (1 + Math.Pow(((c[2]) /  ResultFit.GetLogConcentrations()[IdxConc]), c[3])));
            }
            return;
        }

        public int IsResponding(FormForDRCSelection WindowDRCSelection)
        {
            if (((this.Top / this.Bottom) > (double)WindowDRCSelection.numericUpDownWindowMinValue.Value) && (this.RelativeError < (double)WindowDRCSelection.numericUpDownRelativeErrorMaxValue.Value)) return 1;
            else
                return 0;
        }
    }



    //            {
    //                Series CurrentSeriesForRealValues1 = new Series(GlobalInfo.CurrentScreen.ListDescriptors[ListIdxDesc[i]].GetName() + "_Average");
    //                CurrentSeriesForRealValues1.Color = GlobalInfo.ColorForDRCCurves[i];
    //                CurrentSeriesForRealValues1.ChartType = SeriesChartType.Point;

    //                CurrentSeriesForRealValues.ChartType = SeriesChartType.ErrorBar;
    //                CurrentSeriesForRealValues["BoxPlotWhiskerPercentile"] = "false";
    //                CurrentSeriesForRealValues["BoxPlotShowMedian"] = "false";
    //                CurrentSeriesForRealValues["BoxPlotWhiskerPercentile"] = "false";
    //                CurrentSeriesForRealValues["BoxPlotShowAverage"] = "false";
    //                CurrentSeriesForRealValues["BoxPlotPercentile"] = "false";

    //                bool IsLine = (bool)GlobalInfo.WindowForDRCDesign.radioButtonOrientationLine.Checked;
    //                int NumConcentration = (int)GlobalInfo.WindowForDRCDesign.numericUpDownConcentrationNumber.Value;
    //                int NumReplication = (int)GlobalInfo.WindowForDRCDesign.numericUpDownReplication.Value;
    //                #region comment1
    //                /*
    //                    for (int IdxC = 0; IdxC < NumConcentration; IdxC++)
    //                    { 
    //                        cExtendedList ListValuePerWell = new cExtendedList();
    //                        for (int NumRep = 0; NumRep < NumReplication; NumRep++)
    //                            ListValuePerWell.Add(Values[IdxC * NumReplication + NumRep]);

    //                        DataPoint CurrentPt = new DataPoint();
    //                        CurrentPt.XValue = AssociatedConcentration[IdxC * NumReplication];

    //                        double[] StatValues = new double[3];
    //                        StatValues[0] = ListValuePerWell.Mean();
    //                        double Std = ListValuePerWell.Std();
    //                        StatValues[1] = StatValues[0] - Std;
    //                        StatValues[2] = StatValues[0] + Std;
    //                        CurrentPt.YValues = StatValues;
    //                        CurrentSeriesForRealValues.Points.Add(CurrentPt);

    //                        CurrentSeriesForRealValues1.Points.AddXY(CurrentPt.XValue, StatValues[0]);

    //                        CurrentSeriesForRealValues1.Points[IdxValue_].MarkerStyle = (MarkerStyle)i;
    //                        CurrentSeriesForRealValues1.Points[IdxValue_].MarkerSize = 8;

    //                        CurrentSeriesForRealValues.Points[IdxValue_].MarkerStyle = 0;// (MarkerStyle)i;
    //                        //CurrentSeriesForRealValues.Points[IdxValue_].ToolTip = CurrentWell.GetPosX() + " x " + CurrentWell.GetPosY() + " : " + Values[IdxValue_];
    //                        CurrentSeriesForRealValues.Points[IdxValue_++].MarkerSize = 8;



    //                    }


    //                currentDRC.CurrentChart.Series.Add(CurrentSeriesForRealValues1);
    //                #region comment2


    //                //DataPoint CurrentPt = new DataPoint();
    //                //CurrentPt.XValue = RealPlateIdx;

    //                //double[] Values = new double[3];
    //                //Values[0] = ListValuePerWell.Mean();
    //                //double Std = ListValuePerWell.Std();
    //                //Values[1] = Values[0] - Std;
    //                //Values[2] = Values[0] + Std;
    //                //CurrentPt.YValues = Values;//ListValuePerWell.ToArray();
    //                //CurrentSeries.Points.Add(CurrentPt);
    //                #endregion

    //            }


}

