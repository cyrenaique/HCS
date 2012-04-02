using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HCSAnalyzer.Classes;
using LibPlateAnalysis;
using System.IO;

namespace HCSAnalyzer.Forms
{
    public partial class FormDendogram : Form
    {
        public cDendoGram CurrentDendo;
        Graphics g;
        public cGlobalInfo GlobalInfo;

        public FormDendogram()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Draw the dendogram signature for a specific well
        /// </summary>
        /// <param name="Well"></param>
        /// <param name="Size"></param>
        /// <param name="PosX"></param>
        /// <param name="PosY"></param>
        /// <returns>Lenght of the signature</returns>
        private int DrawSignature(cWell Well, int Size, int PosX, int PosY)
        {
            int RealIdx = 0;
            int SizeFont = Size / 4;
            int ScrollShiftY = this.VerticalScroll.Value;

            double Min, Max;
            int ConvertedValue;

            g.DrawString("[" + Well.GetPosX() + "x" + Well.GetPosY() + "]", new Font("Arial", 8), Brushes.Black, 15, PosY - ScrollShiftY);

            Rectangle CurrentRect = new Rectangle(5, PosY, 8, Size);

        
            SolidBrush CurrBrush = new SolidBrush(Well.GetColor());

            // draw the rectangle
            g.FillRectangle(CurrBrush, CurrentRect);

            for (int iDesc = 0; iDesc < GlobalInfo.CurrentScreen.ListDescriptors.Count; iDesc++)
            {
                if (!GlobalInfo.CurrentScreen.ListDescriptors[iDesc].IsActive()) continue;

                // specify the rect shape
                CurrentRect = new Rectangle(PosX + RealIdx * Size, PosY, Size, Size);

                // specify the color
                cDescriptor CurrentDesc = Well.ListDescriptors[iDesc];

                byte[][] LUT = GlobalInfo.LUT_JET;

                Min = CurrentDendo.InfoForHierarchical.ListMin[RealIdx];
                Max = CurrentDendo.InfoForHierarchical.ListMax[RealIdx];

                if (Min == Max)
                    ConvertedValue = 0;
                else
                    ConvertedValue = (int)(((CurrentDesc.GetValue() - Min) * (LUT[0].Length - 1)) / (Max - Min));
                if (ConvertedValue >= LUT[0].Length) ConvertedValue = LUT[0].Length - 1;

                CurrBrush = new SolidBrush(Color.FromArgb(LUT[0][ConvertedValue], LUT[1][ConvertedValue], LUT[2][ConvertedValue]));

                // draw the rectangle
                g.FillRectangle(CurrBrush, CurrentRect);

                RealIdx++;
            }
            return PosX + RealIdx * Size;
        }

        private void ReDrawDendo()
        {
            if (CurrentDendo == null) return;

            g = this.panelForDendogram.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.White);
            int ScrollShiftY = this.VerticalScroll.Value;
            int ScrollShiftX = this.HorizontalScroll.Value;

            Pen PenForConnections = new Pen(Color.Black, 1);
            Pen PenForBranch = new Pen(Color.Black, 1);

            int RealNumToDisp = 0;
            for (int i = 0; i < CurrentDendo.GetTree().Count; i++)
            {
                if (CurrentDendo.GetTree()[i].Name.IndexOf("Input") == -1) RealNumToDisp++;
            }

           

            double MaxSize = CurrentDendo.GetTree().GetMaxLenght();
            int RealIdx = 0;
            int MaxHeight = 0;
            int InitialShiftXForName = 60;
            
            int SizeSquareForSignature = this.panelForDendogram.Height / RealNumToDisp;
            int SizeSquareForSignatureX = (this.panelForDendogram.Width-60) / GlobalInfo.CurrentScreen.GetNumberOfActiveDescriptor();
            
            if (SizeSquareForSignatureX < 0) SizeSquareForSignatureX = 0;
            if (SizeSquareForSignatureX < SizeSquareForSignature) SizeSquareForSignature = SizeSquareForSignatureX;

            int DistBetweenLines = SizeSquareForSignature;


            int GlobalShiftX = GlobalInfo.CurrentScreen.GetNumberOfActiveDescriptor() * SizeSquareForSignature + InitialShiftXForName;
            int ShiftY = 4;
            int MultiplicativeRatio = (int)((this.panelForDendogram.Width - GlobalShiftX) / 2);

            for (int i = 0; i < CurrentDendo.GetTree().Count; i++)
            {

                int PosY = (int)(CurrentDendo.GetTree()[i].PosY * DistBetweenLines) + ShiftY;
                if (PosY > MaxHeight) MaxHeight = PosY;
 

                cWell AssociatedWell = CurrentDendo.GetTree()[i].AssociatedWell;

               if (AssociatedWell != null)
                    DrawSignature(AssociatedWell, SizeSquareForSignature, InitialShiftXForName, PosY - ScrollShiftY - (ShiftY / 2));

                if (CurrentDendo.GetTree()[i].ConnectedWith == null) continue;

                int PosX = (int)((CurrentDendo.GetTree()[i].PosX * MultiplicativeRatio) / MaxSize);
                int PosXcorner = (int)((CurrentDendo.GetTree()[i].PosXCorner * MultiplicativeRatio) / MaxSize);
                Point Start = new Point(GlobalShiftX + PosX, PosY - ScrollShiftY + SizeSquareForSignature / 2 - ShiftY / 2 );
                Point End = new Point(GlobalShiftX + PosXcorner, PosY - ScrollShiftY + SizeSquareForSignature / 2 - ShiftY / 2 );

                g.DrawLine(PenForBranch, Start, End);

                int NextPosX = (int)((CurrentDendo.GetTree()[i].ConnectedWith.PosXCorner * MultiplicativeRatio) / MaxSize);
                int NextPosY = (int)(CurrentDendo.GetTree()[i].ConnectedWith.PosY * DistBetweenLines) + ShiftY;
                Point StartNew = new Point(GlobalShiftX + NextPosX, NextPosY - ScrollShiftY + SizeSquareForSignature/2- ShiftY / 2 );

                g.DrawLine(PenForConnections, End, StartNew);
                RealIdx++;

            }
        }

        private void panelForDendogram_Paint(object sender, PaintEventArgs e)
        {
            ReDrawDendo();
        }

        private void panelForDendogram_Resize(object sender, EventArgs e)
        {
            ReDrawDendo();
        }

    }
}
