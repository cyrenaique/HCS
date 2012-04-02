using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Kitware.VTK;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using LibPlateAnalysis;

namespace HCSAnalyzer.Classes._3D
{
    public partial class c3DWorld
    {
        public List<vtkPolyDataMapper> ListPolyDataMapper;
        public List<cObject3D> ListObject;


        public List<cMetaBiologicalObjectList> ListMetaObjectList;

        //public List<cBiological3DObject> ListBiologicalObjects;

        double Xres;
        double Yres;
        double Zres;


        public double[] GetResolution()
        {
            double[] res = new double[3];
            res[0] = Xres;
            res[1] = Yres;
            res[2] = Zres;

            return res;
        }


        int SizeX;
        int SizeY;
        int SizeZ;

        c3DPlane BottomPlane;
        vtkLegendScaleActor legendScaleActor = null;
        vtkCamera Vtk_CameraViewOrientation;

        public vtkRenderer ren1;
        public vtkRenderWindow renWin;
        public vtkRenderWindowInteractor iren;

        private DataGridView pDataGridView = null;
        private ContextMenuStrip contextMenuStripActorPicker = null;

        DataTable CurrentTable;

        private List<string> ListNameObjectType = new List<string>();
        private List<bool> SelectableObjectName = new List<bool>();

        public List<string> GetListObjectType()
        {
            return this.ListNameObjectType;
        }

        public List<cInteractive3DObject> GetListBiologicalObjectsOfType(string Type)
        {
            List<cInteractive3DObject> ToReturn = new List<cInteractive3DObject>();

            foreach (cObject3D Obj3D in ListObject)
            {
                if (((Obj3D.GetType().Name == "cBiological3DVolume") || (Obj3D.GetType().Name == "cBiologicalSpot")) && (Obj3D.ObjectType == Type))
                    ToReturn.Add((cInteractive3DObject)Obj3D);
            }

            return ToReturn;
        }


        public void SetLinkToDataGridView(DataGridView pDataGridView)
        {
            this.pDataGridView = pDataGridView;
        }

        public int RemoveNonAssociatedObjects()
        {
            int RemovedObjects = 0;

            for (int ObjIdx = 0; ObjIdx < ListObject.Count; ObjIdx++)
            {
                cObject3D Obj3D = ListObject[ObjIdx];
                if ((Obj3D.GetType().Name == "cBiological3DVolume") || (Obj3D.GetType().Name == "cBiologicalSpot"))
                {
                    cInteractive3DObject BioObj = (cInteractive3DObject)Obj3D;
                    if (BioObj.GetMetaObjectContainer() == null)
                    {
                        BioObj.Hide();
                        ListObject.Remove(Obj3D);
                        RemovedObjects++;
                        ObjIdx--;
                    }
                }
            }
            return RemovedObjects;
        }


        #region 3D world creation
        //public c3DWorld(Kitware.VTK.RenderWindowControl CurrentrenderWindowControl, Sequence Seq)
        //{
        //    this.ren1 = CurrentrenderWindowControl.RenderWindow.GetRenderers().GetFirstRenderer();
        //    this.renWin = CurrentrenderWindowControl.RenderWindow;

        //    if (Seq == null)
        //    {
        //        Xres = Yres = Zres = 0;
        //    }
        //    else
        //    {
        //        Xres = Seq.XResolution;
        //        Yres = Seq.YResolution;
        //        Zres = Seq.ZResolution;
        //    }
        //    ListPolyDataMapper = new List<vtkPolyDataMapper>();
        //    ListObject = new List<cObject3D>();
        //    ListVolume = new List<cVolume3D>();
        //}

        /// <summary>
        /// Create a 3D world
        /// </summary>
        /// <param name="CurrentrenderWindowControl">vtk Control</param>
        /// <param name="Dimensions">in pixels</param>
        /// <param name="Resolution">spatial resolutions</param>
        public c3DWorld(cPoint3D Dimensions, cPoint3D Resolution, RenderWindowControl CurrentrenderWindowControl, int[] WinPos)
        {


            // int[] Pos =  renWin.GetPosition();

            //,
            if (CurrentrenderWindowControl == null)
            {
                renWin = vtkRenderWindow.New();
                renWin.LineSmoothingOn();
                renWin.PointSmoothingOn();
                renWin.SetWindowName("3D World");
                renWin.BordersOn();
                renWin.DoubleBufferOn();

                renWin.SetSize(750, 500);


                //   if(WinPos!=null)            renWin.SetPosition(WinPos[0], WinPos[1]);
                // this.ren1 = CurrentrenderWindowControl.RenderWindow.GetRenderers().GetFirstRenderer();
                //CurrentrenderWindowControl.RenderWindow;
            }

            //// Menu Strip Construction
            //this.contextMenuStripActorPicker = new ContextMenuStrip();
            //ToolStripMenuItem StripMenuItemDisplay = new ToolStripMenuItem("Display");
            //contextMenuStripActorPicker.Items.Add(StripMenuItemDisplay);

            this.ren1 = vtkRenderer.New();
            //renWin = CurrentrenderWindowControl.RenderWindow;//vtkRenderWindow.New();

            renWin.AddRenderer(ren1);


            iren = new vtkRenderWindowInteractor();
            iren.SetRenderWindow(renWin);

            //iren.SetInteractorStyle(vtkInteractorStyleJoystickCamera.New());
            iren.SetInteractorStyle(vtkInteractorStyleTrackballCamera.New());
            //   iren.SetInteractorStyle(vtkInteractorStyleTerrain.New());


            // iren.LeftButtonPressEvt += new vtkObject.vtkObjectEventHandler(RenderWindow_LeftButtonPressEvt);
            iren.KeyPressEvt += new vtkObject.vtkObjectEventHandler(RenderWindow_KeyPressEvt);
            iren.RightButtonPressEvt += new vtkObject.vtkObjectEventHandler(RenderWindow_RightButtonPressEvt);

            //Render();
            //this.ren1 = 

            Xres = Resolution.X;
            Yres = Resolution.Y;
            Zres = Resolution.Z;

            SizeX = (int)Dimensions.X;
            SizeY = (int)Dimensions.Y;
            SizeZ = (int)Dimensions.Z;

            //  double[] fp = ren1.GetActiveCamera().GetFocalPoint();
            //   double[] p = ren1.GetActiveCamera().GetPosition();


            //   ren1.GetActiveCamera().ParallelProjectionOn();

            //   double dist = Math.Sqrt((p[0] - fp[0]) * (p[0] - fp[0]) + (p[1] - fp[1]) * (p[1] - fp[1]) + (p[2] - fp[2]) * (p[2] - fp[2]));
            //    ren1.GetActiveCamera().SetPosition(fp[0], fp[1], fp[2] + dist*1000);
            //    ren1.GetActiveCamera().Zoom(2);
            //ren1.Render();
            //  this.Render();
            ListPolyDataMapper = new List<vtkPolyDataMapper>();
            ListObject = new List<cObject3D>();

            //     Vtk_CameraViewOrientation = ren1.GetActiveCamera();
        }
        #endregion

        ToolStripMenuItem ToolStripMenuItem_DescriptorDisplay;
        cWell CurrentlySelectedWell;


        void RenderWindow_RightButtonPressEvt(vtkObject sender, vtkObjectEventArgs e)
        {
            this.contextMenuStripActorPicker = new ContextMenuStrip();
            int[] Pos;
            Pos = this.iren.GetLastEventPosition();

            ToolStripMenuItem ToolStripMenuItem_World = new ToolStripMenuItem("World");
            ToolStripMenuItem ScaleItem = new ToolStripMenuItem("Scale");
            ToolStripMenuItem_World.DropDownItems.Add(ScaleItem);
            ScaleItem.Click += new System.EventHandler(this.ScaleClicking);

            ToolStripMenuItem ColorItem = new ToolStripMenuItem("Background Color");
            ToolStripMenuItem_World.DropDownItems.Add(ColorItem);
            ColorItem.Click += new System.EventHandler(this.ColorClicking);

            ToolStripMenuItem ExportViewItem = new ToolStripMenuItem("Export View");
            ToolStripMenuItem_World.DropDownItems.Add(ExportViewItem);
            ExportViewItem.Click += new System.EventHandler(this.ExportViewClicking);

            if (this.ListNameObjectType.Count >= 1)
            {
                ToolStripMenuItem SelectionItem = new ToolStripMenuItem("Selection Type");
                ToolStripMenuItem_World.DropDownItems.Add(SelectionItem);
                SelectableObjectName = new List<bool>();

                for (int idxName = 0; idxName < this.ListNameObjectType.Count; idxName++)
                {
                    ToolStripMenuItem TypeName = new ToolStripMenuItem(ListNameObjectType[idxName]);
                    TypeName.CheckOnClick = true;
                    TypeName.Checked = true;
                    TypeName.Click += new System.EventHandler(this.TypeNameClicking);

                    if (TypeName.Checked) SelectableObjectName.Add(true);
                    else SelectableObjectName.Add(false);
                }
            }

            string MetaObjectName = "";
            vtkPicker Picker = vtkPicker.New();
            int numActors = Picker.Pick(Pos[0], Pos[1], 0, this.ren1);
            if (numActors == 1)
            {
                vtkActor PickedActor = Picker.GetActor();

                bool selectable = false;
                for (int i = 0; i < ListObject.Count; i++)
                    if (ListObject[i].GetActor() == PickedActor)
                    {
                        cInteractive3DObject Obj = (cInteractive3DObject)ListObject[i];
                        if (Obj.GetMetaObjectContainer() != null) MetaObjectName = Obj.GetMetaObjectContainer().Information.GetName() + " -> ";

                        for (int idx = 0; idx < this.SelectableObjectName.Count; idx++)
                        {
                            // the selected actor has been identified, let's get its type
                            cInteractive3DObject TmpBioObj = (cInteractive3DObject)ListObject[i];

                            int IdxType = -1;
                            for (int TmpIdx = 0; TmpIdx < this.ListNameObjectType.Count; TmpIdx++)
                                if (TmpBioObj.GetType() == this.ListNameObjectType[TmpIdx]) IdxType = TmpIdx;

                            // Is the type selectable (i.e. checked by the user in the contextual menu ?
                            if (this.SelectableObjectName[IdxType])
                            {
                                selectable = true;
                                break;
                            }
                        }
                    }

                if (selectable == true)
                {
                    ToolStripMenuItem TypeItem = null;
                    ToolStripMenuItem ToolStripMenuItem_ObjectDisplay = new ToolStripMenuItem("Object display");
                    // ToolStripMenuItem CentroidItem = new ToolStripMenuItem("Centroid");
                    ToolStripMenuItem ToolStripMenuItem_MetaDescriptorDisplay = null;

                    ToolStripMenuItem_DescriptorDisplay = new ToolStripMenuItem("Object profil");
                    {
                        for (int i = 0; i < ListObject.Count; i++)
                            if (ListObject[i].GetActor() == PickedActor)
                            {
                                Type T = ListObject[i].GetType();
                                List<double> ListDesc = null;
                                List<string> ListProfilName = null;

                                if (T.Name == "cBiological3DVolume")
                                {
                                    cBiological3DVolume TmpVol = (cBiological3DVolume)ListObject[i];
                                    ListProfilName = TmpVol.Information.GetDescriptorNames();
                                    ListDesc = TmpVol.Information.GetInformation();
                                    TypeItem = new ToolStripMenuItem(MetaObjectName + TmpVol.GetType());
                                }
                                else if (T.Name == "cBiologicalSpot")
                                {
                                    cBiologicalSpot TmpSpot = (cBiologicalSpot)ListObject[i];
                                    ListProfilName = TmpSpot.Information.GetDescriptorNames();
                                    ListDesc = TmpSpot.Information.GetInformation();
                                    TypeItem = new ToolStripMenuItem(MetaObjectName + TmpSpot.GetType());
                                }
                                else if (T.Name == "c3DWell")
                                {
                                    c3DWell TmpWell = (c3DWell)ListObject[i];
                                    ListProfilName = TmpWell.Information.GetDescriptorNames();
                                    ListDesc = TmpWell.Information.GetInformation();
                                    TypeItem = new ToolStripMenuItem(MetaObjectName + TmpWell.GetType());

                                    Chart ThumbnailChart = TmpWell.AssociatedWell.GetChart();
                                    if (ThumbnailChart != null)
                                    {
                                        MemoryStream ms = new MemoryStream();
                                        TmpWell.AssociatedWell.GetChart().SaveImage(ms, ChartImageFormat.Bmp);
                                        TypeItem.Image = new Bitmap(ms);
                                        TypeItem.ImageScaling = ToolStripItemImageScaling.None;
                                        TypeItem.Size = new System.Drawing.Size(48, 48);
                                        
                                        // New3DWell.ThumbnailnewImage = new Bitmap(ms);
                                    }

                                    CurrentlySelectedWell = TmpWell.AssociatedWell;

                                    TypeItem.Click += new System.EventHandler(this.WellInfoClick);


                                }
                                else if (T.Name == "c3DIsoContours")
                                {
                                    c3DIsoContours TmpContours = (c3DIsoContours)ListObject[i];
                                    

                                   // ListProfilName = TmpWell.Information.GetDescriptorNames();
                                  //  ListDesc = TmpWell.Information.GetInformation();
                                    TypeItem = new ToolStripMenuItem();
                                    Chart TmpChart = new Chart();
                                    Series SeriesPos = new Series();
                                    //SeriesPos.ShadowOffset = 1;


                                    ChartArea CurrentChartArea = new ChartArea();

                                    List<double> DoubleList = new List<double>();
                                    int Idx = 0;
                                    double[] Colour =TmpContours.GetActor().GetProperty().GetColor();

                                    foreach (cPoint3D CurrentPt in TmpContours.ListPtContour)
	                                {
                                        SeriesPos.Points.AddY(CurrentPt.Z);
                                        SeriesPos.Points[Idx].MarkerSize = 2;
                                        SeriesPos.Points[Idx].MarkerStyle = MarkerStyle.Circle;

                                        SeriesPos.Points[Idx++].Color = Color.FromArgb((int)Colour[0], (int)Colour[1], (int)Colour[2]);
                                        //DoubleList.Add(CurrentPt.Z);
	                                }

                                    //CDisplayGraph DispGraph = new CDisplayGraph(DoubleList.ToArray());
                                    SeriesPos.ChartType = SeriesChartType.Point;
                                    TmpChart.ChartAreas.Add(CurrentChartArea);

                                    CurrentChartArea.Axes[1].LabelStyle.Enabled = false;
                                    CurrentChartArea.Axes[1].MajorGrid.Enabled = false;
                                    CurrentChartArea.Axes[0].Enabled = AxisEnabled.False;
                                    CurrentChartArea.Axes[1].Enabled = AxisEnabled.False;
                                    CurrentChartArea.Axes[0].MajorGrid.Enabled = false;
                                    TmpChart.Width = 120;
                                    TmpChart.Height = 60;

                                    TmpChart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;


                                    TmpChart.Series.Add(SeriesPos);

                                    //Chart ThumbnailChart = TmpChart.NewWindow.chartForSimpleForm;
                                   // if (ThumbnailChart != null)
                                    {
                                        MemoryStream ms = new MemoryStream();
                                        TmpChart.SaveImage(ms, ChartImageFormat.Bmp);
                                        TypeItem.Image = new Bitmap(ms);
                                        TypeItem.ImageScaling = ToolStripItemImageScaling.None;
                                        TypeItem.Size = new System.Drawing.Size(48, 48);

                                        // New3DWell.ThumbnailnewImage = new Bitmap(ms);
                                    }

                                  //  CurrentlySelectedWell = TmpWell.AssociatedWell;

                                  //  TypeItem.Click += new System.EventHandler(this.WellInfoClick);


                                }



                                if (((cInteractive3DObject)ListObject[i]).ThumbnailnewImage != null)
                                {


                                    //    TypeItem.Image = ((cBiological3DObject)ListObject[i]).ThumbnailnewImage;
                                    //    TypeItem.ImageScaling = ToolStripItemImageScaling.None;
                                    //    TypeItem.Size = new System.Drawing.Size(48, 48);
                                    //    TypeItem.Click += new System.EventHandler(this.WellInfoClick);

                                }

                                if (ListProfilName != null)
                                    for (int idxName = 0; idxName < ListProfilName.Count; idxName++)
                                    {
                                        ToolStripMenuItem descName = new ToolStripMenuItem(ListProfilName[idxName] + " : " + ListDesc[idxName]);
                                        ToolStripMenuItem_DescriptorDisplay.DropDownItems.Add(descName);
                                    }
                                else
                                {
                                    ToolStripMenuItem descName = new ToolStripMenuItem("Null");
                                    ToolStripMenuItem_DescriptorDisplay.DropDownItems.Add(descName);
                                }
                                                        

                              
                            }
                    }

                    ToolStripSeparator SepratorStrip = new ToolStripSeparator();
                    if (ToolStripMenuItem_MetaDescriptorDisplay != null)
                        contextMenuStripActorPicker.Items.AddRange(new ToolStripItem[] { TypeItem, SepratorStrip, ToolStripMenuItem_World, ToolStripMenuItem_ObjectDisplay, ToolStripMenuItem_DescriptorDisplay, ToolStripMenuItem_MetaDescriptorDisplay });
                    else
                        contextMenuStripActorPicker.Items.AddRange(new ToolStripItem[] { TypeItem, SepratorStrip, ToolStripMenuItem_World, ToolStripMenuItem_ObjectDisplay, ToolStripMenuItem_DescriptorDisplay });
                }
                else
                    contextMenuStripActorPicker.Items.AddRange(new ToolStripItem[] { ToolStripMenuItem_World });
            }
            else
                contextMenuStripActorPicker.Items.AddRange(new ToolStripItem[] { ToolStripMenuItem_World });

            int[] PosWindow = this.renWin.GetSize();
            contextMenuStripActorPicker.Show(Control.MousePosition);
            return;
        }

        public void CopyMetaObjectSignatureToTable(cMetaBiologicalObject MetaObject, int ObjectClass)
        {
            List<string> Descriptors = MetaObject.Information.GetGlobalDescriptorNames();
            cInteractive3DObject MasterObj = MetaObject.GetMasterObject();
            List<double> ListDescMaster = null;

            List<string> ListProfilName = null;


            cObject3D Master3d = (cObject3D)(MasterObj);

            if (Master3d.GetType().ToString().IndexOf("cBiological3DVolume") != -1)
            {
                cBiological3DVolume TmpVol = (cBiological3DVolume)Master3d;
                ListProfilName = TmpVol.Information.GetDescriptorNames();
                ListDescMaster = TmpVol.Information.GetInformation();
            }
            else if (Master3d.GetType().ToString().IndexOf("cBiologicalSpot") != -1)
            {
                cBiologicalSpot TmpSpot = (cBiologicalSpot)Master3d;
                ListProfilName = TmpSpot.Information.GetDescriptorNames();
                ListDescMaster = TmpSpot.Information.GetInformation();
            }



            if ((CurrentTable == null) || (CurrentTable.Columns.Count == 0) || (CurrentTable.Rows.Count == 0))
            {
                CurrentTable = new DataTable();

                //cBiological3DObject MasterObj = MetaObject.GetMasterObject();

                CurrentTable.Columns.Add(new DataColumn("Meta Object Name", typeof(string)));


                foreach (string DescName in ListProfilName)
                    CurrentTable.Columns.Add(new DataColumn(DescName, typeof(double)));

                for (int iDesc = 0; iDesc < Descriptors.Count; iDesc++)
                    CurrentTable.Columns.Add(new DataColumn(Descriptors[iDesc], typeof(double)));

                //for (int iDesc = 0; iDesc < Descriptors.Count; iDesc++)
                //    CurrentTable.Columns.Add(new DataColumn(Descriptors[iDesc], typeof(double)));
                CurrentTable.Columns.Add(new DataColumn("Class", typeof(double)));

                if (pDataGridView == null) Console.WriteLine("c3Dworld does not contains the link to the datagridview control");
            }

            List<double> Res = MetaObject.Information.GetInformation(Descriptors);
            List<object> objectNumbers = new List<object>();

            if (Res == null) return;

            foreach (double number in Res)
                objectNumbers.Add((object)number);
            objectNumbers.Add(ObjectClass);

            CurrentTable.Rows.Add();


            CurrentTable.Rows[CurrentTable.Rows.Count - 1][0] = MetaObject.Name;

            for (int Idx = 0; Idx < ListDescMaster.Count; Idx++)
                CurrentTable.Rows[CurrentTable.Rows.Count - 1][Idx + 1] = ListDescMaster[Idx];

            for (int Idx = 0; Idx <= Res.Count; Idx++)
                CurrentTable.Rows[CurrentTable.Rows.Count - 1][Idx + ListDescMaster.Count + 1] = objectNumbers[Idx];

            if (pDataGridView != null)
            {
                pDataGridView.DataSource = CurrentTable;
                pDataGridView.Update();
            }



        }


        void WellInfoClick(object sender, EventArgs e)
        {
            CurrentlySelectedWell.DisplayInfoWindow();

        }

        private void CopyMetaObjectSignature(int Class)
        {
            int[] Pos = this.iren.GetLastEventPosition();

            vtkPicker Picker = vtkPicker.New();
            int numActors = Picker.Pick(Pos[0], Pos[1], 0, this.ren1);
            if (numActors != 1) return;
            vtkActor PickedActor = Picker.GetActor();

            for (int i = 0; i < ListObject.Count; i++)
            {
                if (ListObject[i].GetActor() == PickedActor)
                {
                    // now get the corresponding Meta Object
                    cMetaBiologicalObject MetaObject = ((cInteractive3DObject)ListObject[i]).GetMetaObjectContainer();

                    if (MetaObject == null) return;

                    CopyMetaObjectSignatureToTable(MetaObject, Class);


                    // cGeometric3DObject TmpActor;
                    if (Class == 0)
                    {
                        MetaObject.GenerateAndDisplayBoundingBox(1.0f, Color.Red, false, this);
                        //TmpActor = new cGeometric3DObject(PickedActor, Color.Red);
                    }
                    else
                    {
                        MetaObject.GenerateAndDisplayBoundingBox(1.0f, Color.Green, false, this);
                        //TmpActor = new cGeometric3DObject(PickedActor, Color.LightGreen);
                    }
                    renWin.Render();
                }
            }

        }
         
        void ScaleClicking(object sender, EventArgs e)
        {
            if (this.legendScaleActor == null)
            {
                this.DisplayScale();
                this.legendScaleActor.SetVisibility(1);
            }


            if (this.legendScaleActor.GetVisibility() == 0)
                this.legendScaleActor.SetVisibility(1);
            else
                this.legendScaleActor.SetVisibility(0);


            this.renWin.Render();
        }

        void TypeNameClicking(object sender, EventArgs e)
        {


            return;


        }

        void ExportViewClicking(object sender, EventArgs e)
        {
            // save Histogram
            SaveFileDialog SaveFileDialogCurrScene = new SaveFileDialog();
            SaveFileDialogCurrScene.OverwritePrompt = true;
            SaveFileDialogCurrScene.AutoUpgradeEnabled = true;
            SaveFileDialogCurrScene.Filter = "VRML files (*.vrml)|*.vrml| OBJ files (*.obj)|*.obj";
            DialogResult result = SaveFileDialogCurrScene.ShowDialog();
            if (result == DialogResult.OK)
            {
                string FileName = SaveFileDialogCurrScene.FileName;
                if (FileName.EndsWith(".vrml"))
                {
                    vtkVRMLExporter Exporter = vtkVRMLExporter.New();
                    Exporter.SetRenderWindow(this.renWin);
                    Exporter.SetFileName(FileName);
                    Exporter.Write();
                }
                else if (FileName.EndsWith(".obj"))
                {
                    vtkOBJExporter obj = vtkOBJExporter.New();
                    obj.SetInput(this.renWin);
                    obj.SetFilePrefix(FileName.Remove(FileName.Length - 4));
                    obj.Write();
                }
            }
            else
            {
                return;
            }
        }


        void DisplayBoundingBoxMetaObject(object sender, EventArgs e)
        {
            int[] Pos = new int[2];
            Pos = this.iren.GetLastEventPosition();
            // Console.WriteLine("Pos:" + Pos[0] + "x" + Pos[1]);

            vtkPicker Picker = vtkPicker.New();
            int numActors = Picker.Pick(Pos[0], Pos[1], 0, this.ren1);
            if (numActors != 1) return;
            vtkActor PickedActor = Picker.GetActor();

            for (int i = 0; i < ListObject.Count; i++)
            {
                if (ListObject[i].GetActor() == PickedActor)
                {

                    cInteractive3DObject Obj = (cInteractive3DObject)ListObject[i];
                    Obj.GetMetaObjectContainer().GenerateAndDisplayBoundingBox(1.0f, Color.Turquoise, false, this);


                    //cBiological3DVolume TmpVol = (cBiological3DVolume)ListObject[i];
                    //TmpVol.DisplayHull(this.ren1);


                }
            }
            renWin.Render();


        }

        void CentroidClicking(object sender, EventArgs e)
        {
            int[] Pos = new int[2];
            Pos = this.iren.GetLastEventPosition();
            // Console.WriteLine("Pos:" + Pos[0] + "x" + Pos[1]);

            vtkPicker Picker = vtkPicker.New();
            int numActors = Picker.Pick(Pos[0], Pos[1], 0, this.ren1);
            if (numActors != 1) return;
            vtkActor PickedActor = Picker.GetActor();

            for (int i = 0; i < ListObject.Count; i++)
            {
                if (ListObject[i].GetActor() == PickedActor)
                {
                    if (ListObject[i].GetType().Name == "cBiological3DVolume")
                    {
                        cBiological3DVolume TmpVol = (cBiological3DVolume)ListObject[i];
                        TmpVol.DisplayCentroid(this, 1);
                    }
                    else if (ListObject[i].GetType().Name == "cBiologicalSpot")
                    {
                        cBiologicalSpot TmpVol = (cBiologicalSpot)ListObject[i];
                        TmpVol.DisplayCentroid(this, 1);
                    }


                }
            }
            renWin.Render();
        }

        void ColorClicking(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() != DialogResult.OK) return;
            Color backgroundColor = colorDialog.Color;
            this.SetBackgroundColor(backgroundColor);
            this.renWin.Render();

        }

        void RenderWindow_KeyPressEvt(vtkObject sender, vtkObjectEventArgs e)
        {
            sbyte KeyCode = this.iren.GetKeyCode();

            // R : Reset view
            //if (KeyCode == 114)
            //{
            //    //ren1.GetActiveCamera().SetPosition(0,0,1);
            //    ren1.SetActiveCamera(Vtk_CameraViewOrientation);
            //    Render();
            //}


            if (KeyCode == 114)
            {
                this.ren1.ResetCamera();
                double[] fp = ren1.GetActiveCamera().GetFocalPoint();
                double[] p = ren1.GetActiveCamera().GetPosition();
                double dist = Math.Sqrt((p[0] - fp[0]) * (p[0] - fp[0]) + (p[1] - fp[1]) * (p[1] - fp[1]) + (p[2] - fp[2]) * (p[2] - fp[2]));
                ren1.GetActiveCamera().SetPosition(fp[0], fp[1], fp[2] + dist);
                ren1.GetActiveCamera().SetViewUp(0.0, 1.0, 0.0);

                this.Render();

                //renWin.Render();

            }
            // e : export file
            if (KeyCode == 101)
            {
                // save Histogram
                SaveFileDialog SaveFileDialogCurrScene = new SaveFileDialog();
                SaveFileDialogCurrScene.OverwritePrompt = true;
                SaveFileDialogCurrScene.AutoUpgradeEnabled = true;
                SaveFileDialogCurrScene.Filter = "VRML files (*.vrml)|*.vrml| OBJ files (*.obj)|*.obj";
                DialogResult result = SaveFileDialogCurrScene.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string FileName = SaveFileDialogCurrScene.FileName;
                    if (FileName.EndsWith(".vrml"))
                    {
                        vtkVRMLExporter Exporter = vtkVRMLExporter.New();
                        Exporter.SetRenderWindow(this.renWin);
                        Exporter.SetFileName(FileName);
                        Exporter.Write();
                    }
                    else if (FileName.EndsWith(".obj"))
                    {
                        vtkOBJExporter obj = vtkOBJExporter.New();
                        obj.SetInput(this.renWin);
                        obj.SetFilePrefix(FileName.Remove(FileName.Length - 4));
                        obj.Write();
                    }
                }
                else
                {
                    return;
                }
            }

            int[] Pos = new int[2];
            Pos = this.iren.GetLastEventPosition();

            vtkPicker Picker = vtkPicker.New();
            Picker.SetTolerance(1e-6);
            int numActors = Picker.Pick(Pos[0], Pos[1], 0, this.ren1);
            if (numActors != 1) return;
            vtkActor PickedActor = Picker.GetActor();

            //Console.WriteLine(KeyCode.ToString());

            if (KeyCode == 119)
            {
                PickedActor.GetProperty().SetRepresentationToWireframe();
                renWin.Render();
            }

            if (KeyCode == 115)
            {
                PickedActor.GetProperty().SetRepresentationToSurface();
                renWin.Render();
            }

            if (KeyCode == 45)
            {
                double CurrentOpacity = PickedActor.GetProperty().GetOpacity() - 0.01;

                if (CurrentOpacity < 0.01) CurrentOpacity = 0.01;

                PickedActor.GetProperty().SetOpacity(CurrentOpacity);
                renWin.Render();
            }

            if (KeyCode == 43)
            {
                PickedActor.GetProperty().SetOpacity(PickedActor.GetProperty().GetOpacity() + 0.01);
                renWin.Render();
            }

            // P
            //if ((KeyCode == 112) || (KeyCode == 110))
            //{
            //    int classe = 1;
            //    if (KeyCode == 110) classe = 0;

            //    for (int i = 0; i < ListObject.Count; i++)
            //    {
            //        if (ListObject[i].GetActor() == PickedActor)
            //        {
            //            cBiological3DVolume TmpVol = (cBiological3DVolume)ListObject[i];

            //            if (CurrentTable == null)
            //            {
            //                CurrentTable = new DataTable();
            //                List<string> Descriptors = TmpVol.Information.GetDescriptorNames();

            //                for (int iDesc = 0; iDesc < Descriptors.Count; iDesc++)
            //                    CurrentTable.Columns.Add(new DataColumn(Descriptors[iDesc], typeof(double)));
            //                CurrentTable.Columns.Add(new DataColumn("Class", typeof(double)));

            //                if (pDataGridView == null) Console.WriteLine("c3Dworld does not contains the link to the datagridview control");
            //            }

            //            List<double> Res = TmpVol.Information.GetInformation();

            //            List<object> objectNumbers = new List<object>();
            //            foreach (double number in Res)
            //                objectNumbers.Add((object)number);
            //            objectNumbers.Add(classe);
            //            CurrentTable.Rows.Add();

            //            for (int Idx = 0; Idx <= Res.Count; Idx++)
            //                CurrentTable.Rows[CurrentTable.Rows.Count - 1][Idx] = objectNumbers[Idx];

            //            //  pDataGridView.DataSource = CurrentTable;
            //            //  pDataGridView.Update();

            //            cGeometric3DObject TmpActor;
            //            if (KeyCode == 112) TmpActor = new cGeometric3DObject(PickedActor, Color.Red);
            //            else TmpActor = new cGeometric3DObject(PickedActor, Color.LightGreen);
            //            renWin.Render();
            //        }
            //    }
            //}
        }

        // void RenderWindow_LeftButtonPressEvt(vtkObject sender, vtkObjectEventArgs e)
        // {
        //     int Count = this.iren.GetRepeatCount();

        //     //   if (Count == 0) return;

        //     int[] Pos = new int[2];
        //     Pos = this.iren.GetLastEventPosition();
        //    // Console.WriteLine("Pos:" + Pos[0] + "x" + Pos[1]);

        //     vtkPicker Picker = vtkPicker.New();
        //     int numActors = Picker.Pick(Pos[0], Pos[1], 0, this.ren1);
        //     if (numActors != 1) return;
        //     vtkActor PickedActor = Picker.GetActor();
        //}

        #region Objects Association
        public int KeepOnlyObjectsInsideTheOthers(List<cInteractive3DObject> ListContent, List<cInteractive3DObject> ListContainer)
        {
            int RemovedObjects = 0;
            for (int IdxContent = 0; IdxContent < ListContent.Count; )
            {
                cInteractive3DObject CurrentContent = ListContent[IdxContent];
                cPoint3D TestCentriolPt = new cPoint3D(CurrentContent.GetCentroid().X, CurrentContent.GetCentroid().Y, CurrentContent.GetCentroid().Z);
                for (int IdxContainer = 0; IdxContainer < ListContainer.Count; IdxContainer++)
                {
                    cBiological3DVolume CurrentVol = (cBiological3DVolume)ListContainer[IdxContainer];
                    //  cPoint3D TestPt1 = new cPoint3D(CurrentVol.GetCentroid().X, CurrentVol.GetCentroid().Y, CurrentVol.GetCentroid().Z);

                    if (CurrentVol.IsPointInside(TestCentriolPt))
                    {
                        ListContent.Remove(CurrentContent);
                        RemovedObjects++;
                    }
                }

            }

            return RemovedObjects;
        }

        private bool IsThisObjectInsideTheOther(cInteractive3DObject Content, cBiological3DVolume Container)
        {
            return Container.IsPointInside(Content.GetCentroid());
        }
        #endregion

        #region Add objects to the world

        private void AddObject3D(cObject3D Object3D)
        {
            // Object3D.SetColor(Object3D.Colour.R, Object3D.Colour.G, Object3D.Colour.B);
            // Object3D.vtk_Actor.SetPosition(Object3D.vtk_Actor.GetPosition()[0] * Xres, Object3D.vtk_Actor.GetPosition()[1] * Yres, Object3D.vtk_Actor.GetPosition()[2] * Zres);
            ListObject.Add(Object3D);
            Object3D.AddMeToTheWorld(ren1);
        }

        public void AddBiological3DObject(cInteractive3DObject object3D)
        {
            if (object3D == null) return;
            AddObject3D((cObject3D)object3D);

            for (int i = 0; i < ListNameObjectType.Count; i++)
            {
                if (ListNameObjectType[i] == object3D.GetType())
                    return;
            }
            ListNameObjectType.Add(object3D.GetType());
        }

        public int AddBiological3DObjects(List<cInteractive3DObject> object3DList)
        {
            int NumObj = 0;

            for (int i = 0; i < object3DList.Count; i++)
            {
                if (object3DList[i] == null) continue;
                NumObj++;
                AddBiological3DObject(object3DList[i]);





            }

            return NumObj;
        }

        public int AddBiological3DObjects(List<cInteractive3DObject> object3DList, Color Colour)
        {
            int NumObj = 0;

            for (int i = 0; i < object3DList.Count; i++)
            {
                if (object3DList[i] == null) continue;
                NumObj++;
                object3DList[i].GetActor().GetProperty().SetColor(Colour.R / 255.0, Colour.G / 255.0, Colour.B / 255.0);
                AddBiological3DObject(object3DList[i]);
            }

            return NumObj;
        }

        public int AddGeometric3DObjects(List<cGeometric3DObject> object3DList)
        {
            int NumObj = 0;

            for (int i = 0; i < object3DList.Count; i++)
            {
                if (object3DList[i] == null) continue;
                NumObj++;
                AddObject3D((cObject3D)object3DList[i]);
            }

            return NumObj;
        }

        public void AddGeometric3DObject(cGeometric3DObject object3D)
        {
            AddObject3D((cObject3D)object3D);
        }
        #endregion

        #region World Display Misc.
        public void DisplayBottom(Color color)
        {
            BottomPlane = new c3DPlane(new cPoint3D((float)(SizeX * Xres), 0, 0),
               new cPoint3D(0, (float)(SizeY * Yres), 0),
               new cPoint3D(0, 0, 0), color);
            AddGeometric3DObject(BottomPlane);
            //BottomPlane.vtk_Actor.DragableOff();
            // BottomPlane.vtk_Actor.PickableOff();
            BottomPlane.IsPickable(false);
            BottomPlane.SetToWireFrame();


        }

        public void AddLight(Color color)
        {
            vtkLight Light = new vtkLight();
            //Light.SetFocalPoint(1.875, 0.6125, 0);
            Light.SetPosition(0.875, 1.6125, 1.0);
            Light.SetColor(0.1, 0.0, 0.0);
            ren1.AddLight(Light);
        }

        public void DisplayScale()
        {
            if (legendScaleActor == null)
            {
                legendScaleActor = vtkLegendScaleActor.New();
                ren1.AddActor(legendScaleActor);
            }
        }

        public void SetBackgroundColor(Color color)
        {
            this.ren1.SetBackground(color.R / 255.0, color.G / 255.0, color.B / 255.0);
        }

        public void DisplayText(string Txt)
        {



        }

        #endregion

        public void Render()
        {
            ren1.ResetCamera();


            ren1.GetActiveCamera().Roll(180);
            ren1.GetActiveCamera().Azimuth(180);
            //   ren1.GetActiveCamera().Dolly(1.4);
            renWin.Render();
        }

        public void SimpleRender()
        {
            renWin.Render();
        }

        public void Terminate()
        {
            for (int IdxObj = 0; IdxObj < ListObject.Count; IdxObj++)
            {
                ren1.RemoveActor(ListObject[IdxObj].GetActor());
            }
            //  ren1.Clear();
            renWin.Dispose();
            iren.GetRenderWindow().FinalizeWrapper();
            iren.TerminateApp();
        }



    }
}
