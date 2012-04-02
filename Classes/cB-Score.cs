// BScore (extract)
// Written by Plamen Dragiev
// Created: Oct 24, 2011
// Last updated: Oct 26, 2011 - compatibility with HTS Corrector

using System;
class BScoreWrapper
{

    public static double Median(double[] X)
    {
        double M = 0.0;
        if (X != null && X.Length > 0)
        {
            int Len = X.Length;
            double[] Y = new double[Len];
            Array.Copy(X, Y, Len);
            Array.Sort(Y);
            if ((Len & 1) == 1)
                M = Y[Len / 2];
            else
            {
                Len /= 2;
                M = (Y[Len] + Y[Len + 1]) / 2.0;
            }
        }
        return M;
    }

    public static double[] Row(double[,] wells, int N)
    {
        int Cols = wells.GetLength(1);
        double[] R = new double[Cols];
        for (int j = 0; j < Cols; j++) R[j] = wells[N, j];
        return R;
    }

    public static double[] Column(double[,] wells, int N)
    {
        int Rows = wells.GetLength(0);
        double[] C = new double[Rows];
        for (int i = 0; i < Rows; i++) C[i] = wells[i, N];
        return C;
    }

    public void BScore(double[,] wells)
    {
        int Rows = wells.GetLength(0);
        int Cols = wells.GetLength(1);

        double[] MRow = new double[Rows];
        double[] MCol = new double[Cols];

        double[] R = new double[Rows];
        double[] C = new double[Cols];

        Array.Clear(R, 0, Rows);
        Array.Clear(C, 0, Cols);

        double Epsilon = 0.00005, EpsPerc = 0.0001;
        int MaxIterations = 20;

        int i, j, k, iter = 1;
        double OldSum = 0.0;
        bool converge = false;

        do
        {
            // Rows
            for (i = 0; i < Rows; i++)
                R[i] += MRow[i] = Median(Row(wells, i));

            for (i = 0; i < Rows; i++)
                for (j = 0; j < Cols; j++)
                    wells[i, j] -= MRow[i];

            double RMed = Median(MRow);
            for (i = 0; i < Rows; i++) R[i] -= RMed;

            // Columns
            for (j = 0; j < Cols; j++)
                C[j] += MCol[j] = Median(Column(wells, j));

            double WellSum = 0.0;
            for (i = 0; i < Rows; i++)
                for (j = 0; j < Cols; j++)
                {
                    wells[i, j] -= MCol[j];
                    WellSum += Math.Abs(wells[i, j]);
                }
            double CMed = Median(MCol);
            for (j = 0; j < Cols; j++) C[j] -= CMed;

            converge = WellSum < Epsilon || Math.Abs(WellSum - OldSum) < EpsPerc * WellSum;
            OldSum = WellSum;
        } while (--MaxIterations > 0 && !converge);

        double[] Resid = new double[Rows * Cols];
        for (k = i = 0; i < Rows; i++)
            for (j = 0; j < Cols; j++)
                Resid[k++] = wells[i, j];

        double ResMed = Median(Resid);
        for (i = 0; i < Resid.Length; i++) Resid[i] = Math.Abs(Resid[i] - ResMed);

        double MAD = Median(Resid);

        // the following line's been added for compatibility with HTS Corrector
        // see Makarenkov V, Zentilli P, Kevorkov D, Gagarin A, Malo N and Nadon R: 
        //     An efficient method for the detection and elimination of systematic error 
        //     in high-throughput screening. Bioinformatics 2007, 23:1648-1657
        MAD *= 1.4826;

        if (MAD > 0.0001)
            for (i = 0; i < Rows; i++)
                for (j = 0; j < Cols; j++)
                    wells[i, j] /= MAD;

    }

}