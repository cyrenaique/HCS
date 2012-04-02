using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCSAnalyzer.Classes
{
    public class cInfoClassif
    {
        public String StringForTree;
        public String StringForQuality;
        public String ConfusionMatrix;

    }


    public class cInfoClass
    {
        public int[] CorrespondanceTable;
        public List<int> ListBackAssociation = new List<int>();
        public int NumberOfClass = 0;

    }


    public class cScoreAndClass
    {
        public double Score;
        public int Class;

        public cScoreAndClass(int Class, double Score)
        {
            this.Class = Class;
            this.Score = Score;
        }
    }


    public class cInfoDescriptors
    {
        public int[] CorrespondanceTable;
        public List<int> ListBackAssociation = new List<int>();
    }


    public class cExtendedList : List<double>
    {

        public double Mean()
        {
            double Mean = 0;
            for (int i = 0; i < this.Count; i++)
                Mean += this[i];
            return Mean / (double)this.Count;
        }

        public double Std()
        {
            double var = 0f, mean = this.Mean();
            foreach (float f in this) var += (f - mean) * (f - mean);
            return Math.Sqrt(var / (float)(this.Count - 1));
        }

        public List<double[]> CreateHistogram(double Min, double Max, double Bin)
        {
            List<double[]> ToReturn = new List<double[]>();

            //float max = math.Max(data);
            if (this.Count == 0) return ToReturn;

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
            foreach (double f in this)
            {
                PosHisto = (int)((f - Min) / step);
                if ((PosHisto >= 0) && (PosHisto < HistoSize))
                    histogram[PosHisto]++;
            }
            ToReturn.Add(histogram);

            return ToReturn;
        }


        public List<double[]> CreateHistogram(double Bin)
        {
            List<double[]> ToReturn = new List<double[]>();

            //float max = math.Max(data);
            if (this.Count == 0) return ToReturn;
            double Max = this[0];
            double Min = this[0];

            for (int Idx = 1; Idx < this.Count; Idx++)
            {
                if (this[Idx] > Max) Max = this[Idx];
                if (this[Idx] < Min) Min = this[Idx];
            }

            double step = (Max - Min) / Bin;

            int HistoSize = (int)((Max - Min) / step) + 1;
            if (Max == Min) return null;
            double[] axeX = new double[HistoSize];
            for (int i = 0; i < HistoSize; i++)
            {
                axeX[i] = Min + i * step;
            }
            ToReturn.Add(axeX);

            double[] histogram = new double[HistoSize];
            //double RealPos = Min;

            int PosHisto;
            foreach (double f in this)
            {
                PosHisto = (int)((f - Min) / step);
                if ((PosHisto >= 0) && (PosHisto < HistoSize))
                    histogram[PosHisto]++;
            }
            ToReturn.Add(histogram);

            return ToReturn;
        }


        public double Max()
        {
            double Max = double.MinValue;

            foreach (double val in this)
                if (val >= Max) Max = val;
            return Max;

        }

        public double Min()
        {
            double Min = double.MaxValue;

            foreach (double val in this)
                if (val <= Min) Min = val;
            return Min;

        }


        public double Dist_Euclidean(cExtendedList CompareTo)
        {
            double Res = 0;
            if (CompareTo.Count != this.Count) return -1;


            for (int i = 0; i < this.Count; i++)
            {
                Res += ((this[i] - CompareTo[i]) * (this[i] - CompareTo[i]));
            }


            return Math.Sqrt(Res);
        }


    }

}
