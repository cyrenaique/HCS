using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using HCSAnalyzer;
using HCSAnalyzer.Classes;
using weka.core;
using System.Windows.Threading;
using HCSAnalyzer.Classes._3D;

namespace LibPlateAnalysis
{
    public class cExtendWellList : List<cWell>
    {
        public cWell GetWell(int PosX, int PosY)
        {
            return null;
        }

        public cWell GetWell(int Idx)
        {
            if (Idx < 0) return null;
            if (this.Count == 0) return null;
            // if (Idx > this.Count) return null;
            return this[Idx];
        }
    }

    public class cPlate
    {
        string PlateType;
        cWell[,] ListWell = null;
        public string Name;
        public cScreening ParentScreening;
        List<double[]> ListMinMax = null;
        public cExtendWellList ListActiveWells = new cExtendWellList();

        public cListDRCRegion ListDRCRegions;


        int NumberOfActiveWells = 0;
        int[] ListNumObjectPerClasse;
        cInfoClassif InfoClassif = new cInfoClassif();



        #region Weka based clustering and classification

        /// <summary>
        /// Create an instances structure without classes for unsupervised methods
        /// </summary>
        /// <returns>a weka Instances object</returns>
        public Instances CreateInstancesWithoutClass()
        {
            weka.core.FastVector atts = new FastVector();
            int columnNo = 0;

            // Descriptors loop
            for (int i = 0; i < ParentScreening.ListDescriptors.Count; i++)
            {
                if (ParentScreening.ListDescriptors[i].IsActive() == false) continue;
                atts.addElement(new weka.core.Attribute(ParentScreening.ListDescriptors[i].GetName()));
                columnNo++;
            }
            weka.core.FastVector attVals = new FastVector();
            Instances data1 = new Instances("MyRelation", atts, 0);

            foreach (cWell CurrentWell in this.ListActiveWells)
            {
                double[] vals = new double[data1.numAttributes()];
                for (int Col = 0; Col < columnNo; Col++)
                {
                    if (ParentScreening.ListDescriptors[Col].IsActive() == false) continue;
                    vals[Col] = CurrentWell.ListDescriptors[Col].GetValue();
                }
                data1.add(new DenseInstance(1.0, vals));
            }

            return data1;
        }


        /// <summary>
        /// Create an instances structure with classes for supervised methods
        /// </summary>
        /// <param name="NumClass"></param>
        /// <returns></returns>
        public Instances CreateInstancesWithClasses(cInfoClass InfoClass, int NeutralClass)
        {
            weka.core.FastVector atts = new FastVector();

            int columnNo = 0;

            for (int i = 0; i < ParentScreening.ListDescriptors.Count; i++)
            {
                if (ParentScreening.ListDescriptors[i].IsActive() == false) continue;
                atts.addElement(new weka.core.Attribute(ParentScreening.ListDescriptors[i].GetName()));
                columnNo++;
            }

            weka.core.FastVector attVals = new FastVector();

            for (int i = 0; i < InfoClass.NumberOfClass; i++)
                attVals.addElement("Class__" + (i).ToString());


            atts.addElement(new weka.core.Attribute("Class__", attVals));

            Instances data1 = new Instances("MyRelation", atts, 0);
            int IdxWell = 0;
            foreach (cWell CurrentWell in this.ListActiveWells)
            {
                if (CurrentWell.GetClass() == NeutralClass) continue;
                double[] vals = new double[data1.numAttributes()];
                for (int Col = 0; Col < columnNo; Col++)
                {
                    if (ParentScreening.ListDescriptors[Col].IsActive() == false) continue;
                    vals[Col] = CurrentWell.ListDescriptors[Col].GetValue();
                }
                vals[columnNo] = InfoClass.CorrespondanceTable[CurrentWell.GetClass()];
                data1.add(new DenseInstance(1.0, vals));
                IdxWell++;
            }
            data1.setClassIndex((data1.numAttributes() - 1));

            return data1;
        }


        /// <summary>
        /// Create an instances structure with classes for supervised methods
        /// </summary>
        /// <param name="NumClass"></param>
        /// <returns></returns>
        public Instances CreateInstancesWithClassesWithPlateBasedDescriptor(int NumberOfClass)
        {
            weka.core.FastVector atts = new FastVector();

            int columnNo = 0;

            for (int i = 0; i < ParentScreening.ListPlateBaseddescriptorNames.Count; i++)
            {
                atts.addElement(new weka.core.Attribute(ParentScreening.ListPlateBaseddescriptorNames[i]));
                columnNo++;
            }

            weka.core.FastVector attVals = new FastVector();

            for (int i = 0; i < NumberOfClass; i++)
                attVals.addElement("Class" + (i).ToString());

            atts.addElement(new weka.core.Attribute("Class", attVals));

            Instances data1 = new Instances("MyRelation", atts, 0);
            int IdxWell = 0;
            foreach (cWell CurrentWell in this.ListActiveWells)
            {
                if (CurrentWell.GetClass() == -1) continue;
                double[] vals = new double[data1.numAttributes()];
                for (int Col = 0; Col < columnNo; Col++)
                {
                    vals[Col] = CurrentWell.ListPlateBasedDescriptors[Col].GetValue();
                }
                vals[columnNo] = CurrentWell.GetClass();
                data1.add(new DenseInstance(1.0, vals));
                IdxWell++;
            }
            data1.setClassIndex((data1.numAttributes() - 1));

            return data1;
        }


        public cInfoForHierarchical CreateInstancesWithUniqueClasse()
        {
            cInfoForHierarchical InfoForHierarchical = new cInfoForHierarchical();
            weka.core.FastVector atts = new FastVector();

            int columnNo = 0;

            for (int i = 0; i < this.ParentScreening.ListDescriptors.Count; i++)
            {
                if (this.ParentScreening.ListDescriptors[i].IsActive() == false) continue;
                atts.addElement(new weka.core.Attribute(this.ParentScreening.ListDescriptors[i].GetName()));
                columnNo++;
            }

            weka.core.FastVector attVals = new FastVector();
            atts.addElement(new weka.core.Attribute("Class_____", attVals));

            InfoForHierarchical.Ninsts = new Instances("MyRelation", atts, 0);
            int IdxWell = 0;
            foreach (cWell CurrentWell in this.ListActiveWells)
            {
                if (CurrentWell.GetClass() == -1) continue;
                attVals.addElement("Class_____" + (IdxWell).ToString());

                InfoForHierarchical.ListIndexedWells.Add(CurrentWell);

                double[] vals = new double[InfoForHierarchical.Ninsts.numAttributes()];
                for (int Col = 0; Col < columnNo; Col++)
                {
                    if (this.ParentScreening.ListDescriptors[Col].IsActive() == false) continue;
                    vals[Col] = CurrentWell.ListDescriptors[Col].GetValue();
                }
                vals[columnNo] = IdxWell;
                InfoForHierarchical.Ninsts.add(new DenseInstance(1.0, vals));
                IdxWell++;
            }

            InfoForHierarchical.Ninsts.setClassIndex((InfoForHierarchical.Ninsts.numAttributes() - 1));
            return InfoForHierarchical;
        }

        /// <summary>
        /// Assign a class to each well based on table
        /// </summary>
        /// <param name="ListClasses">Table containing the classes</param>
        public void AssignClass(double[] ListClasses)
        {
            int i = 0;
            foreach (cWell CurrentWell in this.ListActiveWells)
                CurrentWell.SetClass((int)ListClasses[i++]);
        }
        #endregion


        public int[] UpdateNumberOfClass()
        {
            ListNumObjectPerClasse = new int[11];

            for (int j = 0; j < ParentScreening.Rows; j++)
                for (int i = 0; i < ParentScreening.Columns; i++)
                {
                    cWell TempWell = GetWell(i, j, true);
                    if (TempWell == null) continue;
                    //if (TempWell.GetClass() == -1) continue;
                    ListNumObjectPerClasse[TempWell.GetClass() + 1]++;
                }

            if (ParentScreening.GetSelectionType() >= -1)
            {
                ParentScreening.LabelForClass.Text = ListNumObjectPerClasse[ParentScreening.GetSelectionType() + 1].ToString();
                ParentScreening.LabelForClass.Update();
            }
            return ListNumObjectPerClasse;
        }

        public int GetNumberOfClasses()
        {
            int NumberOfClasses = 0;
            int[] ListClasses = UpdateNumberOfClass();

            for (int i = 0; i < ListClasses.Length; i++)
            {
                if (ListClasses[i] > 0) NumberOfClasses++;
            }

            return NumberOfClasses;
        }

        public int GetNumberOfWellOfClass(int Class)
        {
            int[] ListClasses = UpdateNumberOfClass();
            return ListClasses[Class + 1];
        }

        public cInfoClassif GetInfoClassif()
        {
            return this.InfoClassif;
        }

        public cInfoClass GetNumberOfClassesBut(int NeutralClass)
        {

            NeutralClass++;
            int[] ListClasses = UpdateNumberOfClass();

            cInfoClass InfoClass = new cInfoClass();
            InfoClass.CorrespondanceTable = new int[ParentScreening.GlobalInfo.GetNumberofDefinedClass()];

            for (int i = 1; i < ListClasses.Length; i++)
            {
                if ((ListClasses[i] > 0) && (i != NeutralClass))
                {
                    InfoClass.CorrespondanceTable[i - 1] = InfoClass.NumberOfClass++;
                    InfoClass.ListBackAssociation.Add(i - 1);
                }
                else
                    InfoClass.CorrespondanceTable[i - 1] = -1;
            }

            return InfoClass;
        }

        public int GetNumberOfActiveWellsButClass(int NeutralClass)
        {
            int NumberOfActive = 0;

            for (int row = 0; row < ParentScreening.Rows; row++)
                for (int col = 0; col < ParentScreening.Columns; col++)
                    if ((GetWell(col, row, true) != null) && (GetWell(col, row, true).GetClass() != NeutralClass)) NumberOfActive++;
            return NumberOfActive;
        }

        public void ComputePlateBasedDescriptors()
        {

            cDescriptorsType TypeRow = new cDescriptorsType("Row_Pos", true, 1);
            cDescriptorsType TypeCol = new cDescriptorsType("Col_Pos", true, 1);
            cDescriptorsType TypeDistBorder = new cDescriptorsType("Dist_To_Border", true, 1);
            cDescriptorsType TypeDistCenter = new cDescriptorsType("Dist_To_Center", true, 1);


            for (int j = 0; j < ParentScreening.Rows; j++)
                for (int i = 0; i < ParentScreening.Columns; i++)
                {
                    cWell TempWell = GetWell(i, j, false);
                    if (TempWell == null) continue;

                    TempWell.ListPlateBasedDescriptors = new List<cDescriptor>();

                    cDescriptor Descriptor0 = new cDescriptor(j, TypeRow, this.ParentScreening);
                    TempWell.ListPlateBasedDescriptors.Add(Descriptor0);

                    cDescriptor Descriptor1 = new cDescriptor(i, TypeCol, this.ParentScreening);
                    TempWell.ListPlateBasedDescriptors.Add(Descriptor1);

                    double MinDist = i + 1;
                    double DistToRight = ParentScreening.Columns - i;
                    if (DistToRight < MinDist) MinDist = DistToRight;
                    double DistToTop = j + 1;
                    if (DistToTop < MinDist) MinDist = DistToTop;
                    double DistToBottom = ParentScreening.Rows - j;
                    if (DistToBottom < MinDist) MinDist = DistToBottom;

                    cDescriptor Descriptor2 = new cDescriptor(MinDist, TypeDistBorder, this.ParentScreening);
                    TempWell.ListPlateBasedDescriptors.Add(Descriptor2);


                    double X_Center = ParentScreening.Columns / 2;
                    double Y_Center = ParentScreening.Rows / 2;

                    double DistToCenter = Math.Sqrt((i - X_Center) * (i - X_Center) + (j - Y_Center) * (j - Y_Center));

                    cDescriptor Descriptor3 = new cDescriptor(DistToCenter, TypeDistCenter, this.ParentScreening);
                    TempWell.ListPlateBasedDescriptors.Add(Descriptor3);

                }

            return;
        }

        public void AddWell(cWell NewWell)
        {
            ListWell[NewWell.GetPosX() - 1, NewWell.GetPosY() - 1] = NewWell;
            ListActiveWells.Add(NewWell);
        }

        public void UpDateWellsSelection()
        {
            int SelectionType = ParentScreening.GetSelectionType();
            if (SelectionType == -2) return;

            int PosMouseXMax = ParentScreening.ptLast.X;
            int PosMouseXMin = ParentScreening.ptOriginal.X;
            if (ParentScreening.ptOriginal.X > PosMouseXMax)
            {
                PosMouseXMax = ParentScreening.ptOriginal.X;
                PosMouseXMin = ParentScreening.ptLast.X;
            }

            int PosMouseYMax = ParentScreening.ptLast.Y;
            int PosMouseYMin = ParentScreening.ptOriginal.Y;
            if (ParentScreening.ptOriginal.Y > PosMouseYMax)
            {
                PosMouseYMax = ParentScreening.ptOriginal.Y;
                PosMouseYMin = ParentScreening.ptLast.Y;
            }





            int GutterSize = (int)ParentScreening.GlobalInfo.OptionsWindow.numericUpDownGutter.Value;

            if (ParentScreening.IsSelectionApplyToAllPlates == true)
            {
                int NumberOfPlates = ParentScreening.GetNumberOfActivePlates();

                for (int j = 0; j < ParentScreening.Rows; j++)
                    for (int i = 0; i < ParentScreening.Columns; i++)
                    {
                        cWell TempWell = GetWell(i, j, false);
                        if (TempWell == null) continue;
                        int PWellX = (int)((TempWell.GetPosX() + 1) * (ParentScreening.GlobalInfo.SizeHistoWidth + GutterSize));// - 2*ParentScreening.GlobalInfo.ShiftX);
                        int PWellY = (int)((TempWell.GetPosY() + 1) * (ParentScreening.GlobalInfo.SizeHistoHeight + GutterSize) + (int)(GutterSize * 2.5) + 60);// + (int)ParentScreening.GlobalInfo.OptionsWindow.numericUpDownShiftY.Value);

                        if ((PWellX > PosMouseXMin) && (PWellX < PosMouseXMax) && (PWellY > PosMouseYMin) && (PWellY < PosMouseYMax))
                        {
                            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
                            {
                                cPlate CurrentPlateToProcess = ParentScreening.ListPlatesActive.GetPlate(PlateIdx);
                                TempWell = CurrentPlateToProcess.GetWell(i, j, false);
                                if (TempWell == null) continue;
                                if (SelectionType == -1)
                                    TempWell.SetAsNoneSelected();
                                else
                                    TempWell.SetClass(SelectionType);
                            }
                        }
                    }
            }
            else
            {
                for (int j = 0; j < ParentScreening.Rows; j++)
                    for (int i = 0; i < ParentScreening.Columns; i++)
                    {
                        cWell TempWell = GetWell(i, j, false);
                        if (TempWell == null) continue;
                        int PWellX = (int)((TempWell.GetPosX() + 1) * (ParentScreening.GlobalInfo.SizeHistoWidth + GutterSize));// - 2*ParentScreening.GlobalInfo.ShiftX);
                        int PWellY = (int)((TempWell.GetPosY() + 1) * (ParentScreening.GlobalInfo.SizeHistoHeight + GutterSize) + +(int)(GutterSize * 2.5) + 60);// (int)ParentScreening.GlobalInfo.OptionsWindow.numericUpDownShiftY.Value);

                        if ((PWellX > PosMouseXMin) && (PWellX < PosMouseXMax) && (PWellY > PosMouseYMin) && (PWellY < PosMouseYMax))
                        {
                            if (SelectionType == -1)
                                TempWell.SetAsNoneSelected();
                            else
                                TempWell.SetClass(SelectionType);
                        }
                    }

            }
            ParentScreening.GetCurrentDisplayPlate().UpdateNumberOfClass();
            ParentScreening.UpdateListActiveWell();

        }


        public void Display3Dplate(int IdxDescriptor, cPoint3D MinimumPosition)
        {
            ParentScreening._3DWorldForPlateDisplay.ListMetaObjectList = new List<cMetaBiologicalObjectList>();
            cMetaBiologicalObjectList ListMetacells = new cMetaBiologicalObjectList("List Meta Objects");
            ParentScreening._3DWorldForPlateDisplay.ListMetaObjectList.Add(ListMetacells);

            c3DWell NewPlate3D = new c3DWell(new cPoint3D(0, 0, 0), new cPoint3D(ParentScreening.Columns, ParentScreening.Rows, 1), Color.Black, null);
            NewPlate3D.SetOpacity(0.0);

            cMetaBiologicalObject Plate3D = new cMetaBiologicalObject(this.Name, ListMetacells, NewPlate3D);


            
            #region  display the well list

            foreach (cWell TmpWell in this.ListWell)
            {
                if ((TmpWell == null)||(TmpWell.GetClass()==-1)) continue;

                double PosZ = 8 - ((TmpWell.ListDescriptors[IdxDescriptor].GetValue() - this.ListMinMax[IdxDescriptor][0]) * 8) / (this.ListMinMax[IdxDescriptor][1] - this.ListMinMax[IdxDescriptor][0]);
  
                
                double WellSize = (double)ParentScreening.GlobalInfo.OptionsWindow.numericUpDownWellSize.Value;
                double WellBorder = (1 - WellSize) / 2.0;

                cPoint3D CurrentPos = new cPoint3D(TmpWell.GetPosX() - WellBorder + MinimumPosition.X, TmpWell.GetPosY() + WellBorder + MinimumPosition.Y - WellSize/2, PosZ + MinimumPosition.Z - WellBorder);
                Color WellColor = Color.Black;

                if (ParentScreening.GlobalInfo.IsDisplayClassOnly)
                    WellColor = TmpWell.GetColor();
                else
                {
                    int ConvertedValue;
                    byte[][] LUT = ParentScreening.GlobalInfo.LUT_JET;

                    if (this.ListMinMax[IdxDescriptor][0] == this.ListMinMax[IdxDescriptor][1])
                        ConvertedValue = 0;
                    else
                        ConvertedValue = (int)(((TmpWell.ListDescriptors[IdxDescriptor].GetValue() - this.ListMinMax[IdxDescriptor][0]) * (LUT[0].Length - 1)) / (this.ListMinMax[IdxDescriptor][1] - this.ListMinMax[IdxDescriptor][0]));
                    if ((ConvertedValue >= 0) && (ConvertedValue < LUT[0].Length))
                        WellColor = Color.FromArgb(LUT[0][ConvertedValue], LUT[1][ConvertedValue], LUT[2][ConvertedValue]);
                }

              

                c3DWell New3DWell = new c3DWell(CurrentPos, new cPoint3D(CurrentPos.X + WellSize, CurrentPos.Y + WellSize, CurrentPos.Z + WellSize/2.0), WellColor, TmpWell);

              
                New3DWell.SetType("[" + TmpWell.GetPosX() + " x " + TmpWell.GetPosY() + "]");

                if (ParentScreening.GlobalInfo.OptionsWindow.checkBoxDisplayWellInformation.Checked)
                {
                    string ToDisp = "";
                    if (ParentScreening.GlobalInfo.OptionsWindow.radioButtonWellInfoName.Checked)
                        ToDisp = TmpWell.Name;
                    if (ParentScreening.GlobalInfo.OptionsWindow.radioButtonWellInfoInfo.Checked)
                        ToDisp = TmpWell.Info;
                    if (ParentScreening.GlobalInfo.OptionsWindow.radioButtonWellInfoLocusID.Checked)
                        ToDisp = ((int)(TmpWell.LocusID)).ToString();
                    if (ParentScreening.GlobalInfo.OptionsWindow.radioButtonWellInfoConcentration.Checked)
                        if (TmpWell.Concentration >= 0) ToDisp = TmpWell.Concentration.ToString("e4");

                    New3DWell.AddText(ToDisp, ParentScreening._3DWorldForPlateDisplay, 0.1);
                }

                New3DWell.SetOpacity((double)ParentScreening.GlobalInfo.OptionsWindow.numericUpDownWellOpacity.Value);
                ParentScreening._3DWorldForPlateDisplay.AddBiological3DObject(New3DWell);
                Plate3D.AddObject(New3DWell);
            }

            #endregion

            #region Well numbering
            if (ParentScreening.GlobalInfo.OptionsWindow.checkBox3DPlateInformation.Checked)
            {
                for (int i = 1; i <= ParentScreening.Columns; i++)
                {
                    c3DText CurrentText = new c3DText(ParentScreening._3DWorldForPlateDisplay, i.ToString(), new cPoint3D(i - 1 + MinimumPosition.X, -0.5 + MinimumPosition.Y, 0 + MinimumPosition.Z), Color.White, 0.35);
                    ParentScreening._3DWorldForPlateDisplay.AddGeometric3DObject(CurrentText);
                }
                for (int j = 1; j <= ParentScreening.Rows; j++)
                {
                    c3DText CurrentText1;

                    if (ParentScreening.GlobalInfo.OptionsWindow.radioButtonWellPosModeDouble.Checked)
                        CurrentText1 = new c3DText(ParentScreening._3DWorldForPlateDisplay, j.ToString(), new cPoint3D(-1 + MinimumPosition.X, j - 0.5 + MinimumPosition.Y, 0 + MinimumPosition.Z), Color.White, 0.35);
                    else
                        CurrentText1 = new c3DText(ParentScreening._3DWorldForPlateDisplay, ParentScreening.GlobalInfo.ConvertIntPosToStringPos(j), new cPoint3D(-1 + MinimumPosition.X, j - 0.5 + MinimumPosition.Y, 0 + MinimumPosition.Z), Color.White, 0.35);

                    ParentScreening._3DWorldForPlateDisplay.AddGeometric3DObject(CurrentText1);
                }
            }
            #endregion

            if (this.ListDRCRegions != null)
            {
                foreach (cDRC_Region Region in this.ListDRCRegions)
                {

                    if (ParentScreening.GlobalInfo.OptionsWindow.checkBox1.Checked)
                    {

                        cDRC CurrentDRC = Region.GetDRC(this.ParentScreening.ListDescriptors[IdxDescriptor]);
                        if ((CurrentDRC == null) || (CurrentDRC.ResultFit == null) || (CurrentDRC.ResultFit.Y_Estimated.Count <= 1)) continue;


                        c3DDRC New3DDRC = new c3DDRC(CurrentDRC, Region, Color.White, this.ListMinMax[IdxDescriptor][0], this.ListMinMax[IdxDescriptor][1]);
                        New3DDRC.SetOpacity((double)ParentScreening.GlobalInfo.OptionsWindow.numericUpDownDRCOpacity.Value);
                        //New3DDRC.SetType("[" + TmpWell.GetPosX() + " x " + TmpWell.GetPosY() + "]");
                        ParentScreening._3DWorldForPlateDisplay.AddGeometric3DObject(New3DDRC);
                        //Plate3D.AddObject(New3DDRC);
                    }

                    if (ParentScreening.GlobalInfo.OptionsWindow.checkBox3DComputeThinPlate.Checked)
                    {
                        c3DThinPlate NewThinPlate = new c3DThinPlate(Region, (double)ParentScreening.GlobalInfo.OptionsWindow.numericUpDown3DThinPlateRegularization.Value);

                        if (ParentScreening.GlobalInfo.OptionsWindow.checkBox3DDisplayThinPlate.Checked)
                        {
                            NewThinPlate.SetOpacity(0.5);
                            NewThinPlate.SetToWireFrame();
                            ParentScreening._3DWorldForPlateDisplay.AddGeometric3DObject(NewThinPlate);
                        }
                        if (ParentScreening.GlobalInfo.OptionsWindow.checkBox3DDisplayIsoboles.Checked)
                        {
                            for (double PosZContour = 0; PosZContour <= 10.0; PosZContour += 1.5)
                            {
                                c3DIsoContours NewContour = new c3DIsoContours(NewThinPlate,Region, Color.Red, PosZContour, true);
                                ParentScreening._3DWorldForPlateDisplay.AddBiological3DObject(NewContour);
                            }
                        }
                        if (ParentScreening.GlobalInfo.OptionsWindow.checkBox3DDisplayIsoRatioCurves.Checked)
                        {
                            //   for (double PosZContour = 0; PosZContour <= 15.0; PosZContour += 3)
                            {
                                c3DIsoContours NewContour = new c3DIsoContours(NewThinPlate,Region, Color.Blue, 0/*PosZContour*/, false);
                                ParentScreening._3DWorldForPlateDisplay.AddBiological3DObject(NewContour);
                            }
                        }
                    }


                }

            }


 

        }

        public void Refresh3D(int IdxDescriptor)
        {
            if (ParentScreening.GlobalInfo.Is3DVisu())
            {
                if (ParentScreening._3DWorldForPlateDisplay == null)
                {
                    ParentScreening._3DWorldForPlateDisplay = new c3DWorld(new cPoint3D(ParentScreening.Columns, ParentScreening.Rows, 1), new cPoint3D(1, 1, 1), ParentScreening.GlobalInfo.renderWindowControlForVTK, null);


                    Display3Dplate(IdxDescriptor, new cPoint3D(0, 0, 0));

                    ParentScreening._3DWorldForPlateDisplay.DisplayBottom(Color.FromArgb(255, 255, 255));
                    ParentScreening._3DWorldForPlateDisplay.SetBackgroundColor(Color.Black);


                    ParentScreening._3DWorldForPlateDisplay.Render();
                    ParentScreening._3DWorldForPlateDisplay.ren1.GetActiveCamera().Zoom(1.8);

                    double[] p = ParentScreening._3DWorldForPlateDisplay.ren1.GetActiveCamera().GetPosition();
                    ParentScreening._3DWorldForPlateDisplay.ren1.GetActiveCamera().SetPosition(p[0], p[1], p[2] - 4);
                }
                else
                {
                    double[] View = ParentScreening._3DWorldForPlateDisplay.ren1.GetViewPoint();

                    //this.ren1.ResetCamera();
                    double[] fp = ParentScreening._3DWorldForPlateDisplay.ren1.GetActiveCamera().GetFocalPoint();
                    double[] p = ParentScreening._3DWorldForPlateDisplay.ren1.GetActiveCamera().GetPosition();
                    double[] ViewUp = ParentScreening._3DWorldForPlateDisplay.ren1.GetActiveCamera().GetViewUp();



                    double dist = Math.Sqrt((p[0] - fp[0]) * (p[0] - fp[0]) + (p[1] - fp[1]) * (p[1] - fp[1]) + (p[2] - fp[2]) * (p[2] - fp[2]));
                    int[] WinPos = new int[2];
                    WinPos[0] = 100;
                    WinPos[1] = 100;

                    ParentScreening._3DWorldForPlateDisplay.Terminate();
                    ParentScreening._3DWorldForPlateDisplay = null;
                    ParentScreening._3DWorldForPlateDisplay = new c3DWorld(new cPoint3D(ParentScreening.Columns, ParentScreening.Rows, 1), new cPoint3D(1, 1, 1), ParentScreening.GlobalInfo.renderWindowControlForVTK, WinPos);

                    ParentScreening._3DWorldForPlateDisplay.ren1.GetActiveCamera().Roll(180);
                    ParentScreening._3DWorldForPlateDisplay.ren1.GetActiveCamera().Azimuth(180);


                    ParentScreening._3DWorldForPlateDisplay.ren1.GetActiveCamera().SetFocalPoint(fp[0], fp[1], fp[2]);
                    ParentScreening._3DWorldForPlateDisplay.ren1.GetActiveCamera().SetPosition(p[0], p[1], p[2]);
                    ParentScreening._3DWorldForPlateDisplay.ren1.GetActiveCamera().SetViewUp(ViewUp[0], ViewUp[1], ViewUp[2]);

                    ParentScreening._3DWorldForPlateDisplay.ren1.GetActiveCamera().Zoom(1.8);

                }



                Display3Dplate(IdxDescriptor, new cPoint3D(0, 0, 0));

                if (ParentScreening.GlobalInfo.OptionsWindow.checkBox3DPlateInformation.Checked)  ParentScreening._3DWorldForPlateDisplay.DisplayBottom(Color.FromArgb(255, 255, 255));
                ParentScreening._3DWorldForPlateDisplay.SetBackgroundColor(Color.Black);
                ParentScreening._3DWorldForPlateDisplay.SimpleRender();
            }

            #region Close the 3D view
            else
            {
                if (ParentScreening._3DWorldForPlateDisplay != null)
                {
                    ParentScreening._3DWorldForPlateDisplay.Terminate();
                    ParentScreening._3DWorldForPlateDisplay = null;
                }
            }
            #endregion
          
        
        
        }




        public void DisplayDistribution(int IdxDescriptor, bool IsFirstTime)
        {
              


            if (ListMinMax == null) this.UpDataMinMax();

            if (IdxDescriptor >= ListMinMax.Count) return;

            double[] MinMax = this.ListMinMax[IdxDescriptor];

            Refresh3D(IdxDescriptor);

            #region 2D display  
         //   if (!this.ParentScreening.GlobalInfo.IsDisplayClassOnly)
            {
          //  ParentScreening.PanelForPlate.Controls.Clear();

            //double[] MinMax = ListMinMax[IdxDescriptor];
           // List<PlateChart> LChart = new List<PlateChart>();

                ParentScreening.GlobalInfo.panelForPlate.Controls.Clear();
                List<PlateChart> LChart = new List<PlateChart>();
                if (ParentScreening.GlobalInfo.IsDisplayClassOnly)
                {

                    for (int j = 0; j < ParentScreening.Rows; j++)
                        for (int i = 0; i < ParentScreening.Columns; i++)
                        {
                            cWell TempWell = GetWell(i, j, false);
                            if (TempWell == null) continue;

                            // Add chart control to the form
                            LChart.Add(TempWell.BuildChartForClass());
                        }

                    // return;
                }
                else
                {

                    // Display Axes
                    //int Gutter = (int)ParentScreening.GlobalInfo.OptionsWindow.numericUpDownGutter.Value;
                    for (int j = 0; j < ParentScreening.Rows; j++)
                        for (int i = 0; i < ParentScreening.Columns; i++)
                        {
                            cWell TempWell = GetWell(i, j, false);
                            if (TempWell == null) continue;
                            LChart.Add(TempWell.BuildChart(IdxDescriptor, MinMax));
                        }
                }
                //System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate()    {  ParentScreening.PanelForPlate.BeginInvoke(new Action(delegate() {            })); }));
                //thread.Start();

                ParentScreening.GlobalInfo.panelForPlate.Controls.AddRange(LChart.ToArray());
            #endregion

                if (MinMax[0] != MinMax[1]) DisplayLUT(IdxDescriptor);
            }
            return;
        }

        public void DisplayLUT(int IdxDescriptor)
        {
            if (ParentScreening.LabelForMin == null) return;

            ParentScreening.LabelForMin.Text = String.Format("{0:0.######}", ListMinMax[IdxDescriptor][0]);
            ParentScreening.LabelForMax.Text = String.Format("{0:0.######}", ListMinMax[IdxDescriptor][1]);
        }


        public cWell GetWell(int Col, int Row, bool OnlyIfSelected)
        {
            if ((Col >= this.ParentScreening.Columns) || (Row >= this.ParentScreening.Rows)) return null;
            if (ListWell[Col, Row] == null) return null;
            if ((OnlyIfSelected) && (ListWell[Col, Row].GetClass() == -1))
                return null;
            else return ListWell[Col, Row];
        }

        public cPlate(string Type, string Name, cScreening ParentScreening)
        {
            this.ParentScreening = ParentScreening;
            this.Name = Name;
            this.PlateType = Type;
            ListWell = new cWell[ParentScreening.Columns, ParentScreening.Rows];
            return;
        }

        double[] GetMinMax(int IdxDescriptor)
        {
            double[] Boundaries = new double[2];

            double Min = double.MaxValue;
            double Max = double.MinValue;
            double CurrentVal;

            

            for (int x = 0; x < ParentScreening.Columns; x++)
                for (int y = 0; y < ParentScreening.Rows; y++)
                {
                    
                    if (ListWell[x, y] == null) continue;

                    
                    cWell TWell = GetWell(x, y, false);

                    if (IdxDescriptor >= TWell.ListDescriptors.Count) return null;


                    if (TWell == null) continue;
                    CurrentVal = TWell.ListDescriptors[IdxDescriptor].GetValue();// ListWell[x, y].ListDescriptors[IdxDescriptor].AverageValue;
                    if (CurrentVal < Min) Min = CurrentVal;
                    if (CurrentVal > Max) Max = CurrentVal;
                }
            Boundaries[0] = Min;
            Boundaries[1] = Max;

            return Boundaries;
        }

        public void LoadFromDisk(string Path)
        {
            if (ListWell == null)
            {
                ParentScreening.GlobalInfo.ConsoleWriteLine("ListWell NULL");
                return;
            }
            IEnumerable<string> ListFile = Directory.EnumerateFiles(Path, "*.txt", SearchOption.TopDirectoryOnly);
            int ProcessedWell = 0;
            foreach (string FileName in ListFile)
            {
                cWell NewWell = new cWell(FileName, this.ParentScreening, this);
                if (NewWell.GetPosX() != -1) ProcessedWell++;
                this.AddWell(NewWell);
                //ListWell[NewWell.GetPosX() - 1, NewWell.GetPosY() - 1] = NewWell;
            }

            ListMinMax = new List<double[]>();
            for (int i = 0; i < ParentScreening.ListDescriptors.Count; i++)
            {
                double[] TmpMinMax = GetMinMax(i);
                ListMinMax.Add(TmpMinMax);
            }

            this.NumberOfActiveWells = ProcessedWell;
            ParentScreening.GlobalInfo.ConsoleWriteLine(ProcessedWell + " well(s) succesfully processed");
        }

        public void UpDataMinMax()
        {
            ListMinMax = new List<double[]>();
            for (int i = 0; i < ParentScreening.ListDescriptors.Count; i++)
            {
                double[] TmpMinMax = GetMinMax(i);
                if(ListMinMax!=null)    ListMinMax.Add(TmpMinMax);
            }
            return;
        }

        public int GetNumberOfActiveWells()
        {
            int NumberOfActive = 0;

            for (int row = 0; row < ParentScreening.Rows; row++)
                for (int col = 0; col < ParentScreening.Columns; col++)
                    if (GetWell(col, row, true) != null) NumberOfActive++;
            return NumberOfActive;
        }

        public double[,] GetAverageValueDescTable(int Desc, out bool IsMissingWell)
        {
            IsMissingWell = false;
            double[,] Table = new double[ParentScreening.Columns, ParentScreening.Rows];

            for (int j = 0; j < ParentScreening.Rows; j++)
                for (int i = 0; i < ParentScreening.Columns; i++)
                {
                    cWell currentWell = this.GetWell(i, j, true);
                    if (currentWell == null)
                        IsMissingWell = true;
                    else
                        Table[i, j] = currentWell.GetAverageValuesList()[Desc];
                }

            return Table;
        }

        public void SetAverageValueDescTable(int Desc, double[,] Table)
        {

            for (int j = 0; j < ParentScreening.Rows; j++)
                for (int i = 0; i < ParentScreening.Columns; i++)
                {
                    cWell currentWell = this.GetWell(i, j, true);
                    if (currentWell == null)
                        continue;

                    currentWell.ListDescriptors[Desc].SetHistoValues(Table[i, j]);
                    currentWell.ListDescriptors[Desc].UpDateDescriptorStatistics();
                }

            UpDataMinMax();
        }


    }
}
