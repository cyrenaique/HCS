using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibPlateAnalysis;

namespace HCSAnalyzer.Classes
{
    public class cEdgeEffect
    {
        private List<double[,]> DiffusionMaps;
        cScreening CurrentScreening;
        private double CoeffDiff = 0.125;
        private double[,] Mask;
        private List<double> DiffusionMapsMeans;
        private List<double> DiffusionMapsStdev;
        //private List<double> List

        public cEdgeEffect(cScreening CurrentScreening, int MaxIteration)
        {
            this.CurrentScreening = CurrentScreening;
            GenerateMask();
            ComputeDiffusionMaps(MaxIteration);
        }

        public double[,] GetDiffusion(int Iteration)
        {
            return DiffusionMaps[Iteration];
        }

        private void GenerateMask()
        {
            Mask = new double[CurrentScreening.Columns + 2, CurrentScreening.Rows + 2];

            for (int X = 0; X < CurrentScreening.Columns + 2; X++)
            {
                Mask[X, 0] = 1;
                Mask[X, CurrentScreening.Rows + 1] = 1;
            }

            for (int Y = 0; Y < CurrentScreening.Rows + 2; Y++)
            {
                Mask[0,Y] = 1;
                Mask[CurrentScreening.Columns + 1,Y] = 1;
            }
        }

        private void DiffusionLaplacianFunction(double[,] input, double[,] output, int Width, int Height)
        {
            for (int j = 0; j < Height; j++)
                for (int i = 0; i < Width; i++)
                {
                    
                    if (Mask[i,j] == 0)
                    {
                        output[i,j] = input[i, j] + (input[i + 1,j] + input[i - 1,j] + input[i,j+1] + input[i,j-1] - 4 * input[i,j]) * CoeffDiff;
                    }
                    else
                        output[i,j] = Mask[i,j];
                }

            // normalize the plate
             cExtendedList LextPlate = new cExtendedList();

            // compute average 
            for (int Y = 0; Y < CurrentScreening.Rows; Y++)
                for (int X = 0; X < CurrentScreening.Columns; X++)
                {
                    LextPlate.Add(output[X+1, Y+1]);
                }

            double Average = LextPlate.Mean();
            double Stdev = LextPlate.Std();

            for (int Y = 0; Y < CurrentScreening.Rows; Y++)
                for (int X = 0; X < CurrentScreening.Columns; X++)
                {
                    output[X+1, Y+1] = (output[X+1, Y+1] - Average) / Stdev;
                }           



            return;
        }

        public int FindIterationForBestMatch(double[,] Plate)
        {
            int BestIter = -1;
            double Dist = double.MaxValue;

            cExtendedList LextPlate = new cExtendedList();
            double[,] TmpPlate = new double[CurrentScreening.Columns,CurrentScreening.Rows];


            // compute average 
            for (int Y = 0; Y < CurrentScreening.Rows; Y++)
                for (int X = 0; X < CurrentScreening.Columns; X++)
                {
                    LextPlate.Add(Plate[X, Y]);
                }

            double Average = LextPlate.Mean();
            double Stdev = LextPlate.Std();

            for (int Y = 0; Y < CurrentScreening.Rows; Y++)
                for (int X = 0; X < CurrentScreening.Columns; X++)
                {
                    TmpPlate[X, Y] = (Plate[X, Y] - Average) / Stdev;
                }


            for (int Iter = 0; Iter < this.DiffusionMaps.Count; Iter++)
            {
                double CurrentDist = 0;

                for(int Y=0;Y<CurrentScreening.Rows;Y++)
                    for (int X = 0; X < CurrentScreening.Columns; X++)
                    {
                        CurrentDist += Math.Sqrt( (this.DiffusionMaps[Iter][X, Y] - TmpPlate[X, Y]) * (this.DiffusionMaps[Iter][X, Y] - TmpPlate[X, Y]));
                    }

                if (CurrentDist < Dist)
                {
                    BestIter = Iter;
                    Dist = CurrentDist;
                }
            }
            return BestIter;
        }


        public double[] FindBestShiftMultCoeff(double[,] inputPlate, int IdxDiff)
        {
            double[,] TmpPlate = new double[CurrentScreening.Columns,CurrentScreening.Rows];
            double[] ShiftMult = new double[2];

            double MaxDist = double.MaxValue;
            double CurrentDist = 0;

            for (double DiffusionInitTemp = (double)CurrentScreening.GlobalInfo.OptionsWindow.numericUpDownEdgeEffectMinMult.Value; DiffusionInitTemp < (double)CurrentScreening.GlobalInfo.OptionsWindow.numericUpDownEdgeEffectMaxMult.Value; DiffusionInitTemp += (double)CurrentScreening.GlobalInfo.OptionsWindow.numericUpDownEdgeEffectDeltaMult.Value)
            {
                for (double PlateInitTemp = (double)CurrentScreening.GlobalInfo.OptionsWindow.numericUpDownEdgeEffectMinShift.Value; PlateInitTemp < (double)CurrentScreening.GlobalInfo.OptionsWindow.numericUpDownEdgeEffectMaxShift.Value; PlateInitTemp += (double)CurrentScreening.GlobalInfo.OptionsWindow.numericUpDownEdgeEffectDeltaShift.Value)
                {

                    CurrentDist = 0;

                    for (int Y = 0; Y < CurrentScreening.Rows; Y++)
                        for (int X = 0; X < CurrentScreening.Columns; X++)
                        {
                            TmpPlate[X, Y] = this.DiffusionMaps[IdxDiff][X, Y] * DiffusionInitTemp + PlateInitTemp;
                            CurrentDist += Math.Sqrt((TmpPlate[X, Y] - inputPlate[X, Y]) * (TmpPlate[X, Y] - inputPlate[X, Y]));
                        }

                    if (CurrentDist < MaxDist)
                    {
                        MaxDist = CurrentDist;
                        ShiftMult[0] = PlateInitTemp;
                        ShiftMult[1] = DiffusionInitTemp;
                    }
                }
            }
            return ShiftMult;
        }


        public double[,] CorrectThePlate(double[,] inputPlate, int IdxDiff, double Shift, double MultCoeff)
        {
            double[,] CorrectedPlate = new double[CurrentScreening.Columns, CurrentScreening.Rows];
            
            for (int Y = 0; Y < CurrentScreening.Rows; Y++)
                for (int X = 0; X < CurrentScreening.Columns; X++)
                {
                    CorrectedPlate[X, Y] = inputPlate[X, Y] / (this.DiffusionMaps[IdxDiff][X, Y] * MultCoeff + Shift);
                }


            return CorrectedPlate;
        
        }


        /// <summary>
        /// compute the diffusion maps from Iteration = 0 to Iteration = NumIterations
        /// </summary>
        /// <param name="NumIterations">Maximum number of iterations</param>
        private void ComputeDiffusionMaps(int NumIterations)
        {
            DiffusionMaps = new List<double[,]>();
            DiffusionMapsMeans = new List<double>();
            DiffusionMapsStdev = new List<double>();


            double[,] CurrentMap = new double[CurrentScreening.Columns + 2, CurrentScreening.Rows + 2];
            double[,] NextMap = new double[CurrentScreening.Columns + 2, CurrentScreening.Rows + 2];

            Array.Copy(Mask, CurrentMap, Mask.Length);

            double[,] CurrentMapWithoutBorders0 = new double[CurrentScreening.Columns, CurrentScreening.Rows];

            for (int X = 0; X < CurrentScreening.Columns; X++)
                for (int Y = 0; Y < CurrentScreening.Rows; Y++)
                    CurrentMapWithoutBorders0[X, Y] = CurrentMap[X + 1, Y + 1];
            DiffusionMaps.Add(CurrentMapWithoutBorders0);

            cExtendedList ValueList = new cExtendedList();

            for (int it = 0; it < NumIterations; it++)
            {
                DiffusionLaplacianFunction(CurrentMap, NextMap, CurrentScreening.Columns + 2, CurrentScreening.Rows + 2);
                ValueList.Clear();


                double[,] CurrentMapWithoutBorders = new double[CurrentScreening.Columns, CurrentScreening.Rows];
                for (int X = 0; X < CurrentScreening.Columns; X++)
                    for (int Y = 0; Y < CurrentScreening.Rows; Y++)
                    {
                        CurrentMapWithoutBorders[X, Y] = NextMap[X + 1, Y + 1];
                        ValueList.Add(CurrentMapWithoutBorders[X, Y]);
                    }
                DiffusionMaps.Add(CurrentMapWithoutBorders);
                DiffusionMapsMeans.Add(ValueList.Mean());
                DiffusionMapsStdev.Add(ValueList.Std());

                Array.Copy(NextMap, CurrentMap, CurrentMap.Length);
            }
            return;
        }
    }
}
