using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Kitware.VTK;
using System.Runtime.InteropServices;


namespace HCSAnalyzer.Classes._3D
{

    public class cList3Dvolumes : List<List<cBiological3DVolume>>
    {
        public int IsContainType(string Type)
        {
            int Pos = 0;

            foreach (List<cBiological3DVolume> SubList in this)
            {
                if (SubList[0].GetType() == Type) return Pos;
                Pos++;
            }
            return -1;
        }


    }

    public class cList3DSpots : List<List<cBiologicalSpot>>
    {
        public int IsContainType(string Type)
        {
            int Pos = 0;

            foreach (List<cBiologicalSpot> SubList in this)
            {
                if (SubList.Count == 0) continue;
                if (SubList[0].GetType() == Type) return Pos;
                Pos++;
            }
            return -1;
        }
    }

    public class cMetaBiologicalObjectList : List<cMetaBiologicalObject>
    {
        public string Name;

        public cMetaBiologicalObjectList(string Name)
        {
            this.Name = Name;
        }

        public cMetaBiologicalObject GetAssociatedMetaObject(cInteractive3DObject BiologicalObjectToIdentify)
        {
            cMetaBiologicalObject CorrespondingMetaObject = null;

            foreach (cMetaBiologicalObject TempMetaObject in this)
                if (TempMetaObject.IsPresent(BiologicalObjectToIdentify)) return TempMetaObject;

            return CorrespondingMetaObject;
        }

        /// <summary>
        /// Associate a biological by identifying the meta object of another biological object
        /// </summary>
        /// <param name="ObjectToIdentify">Object you want to be linked to</param>
        /// <param name="ObjectToBeAssociated">Object you want to link</param>
        /// <returns>the meta object the biological object has been associated with</returns>
        public cMetaBiologicalObject AssociateWith(cInteractive3DObject ObjectToIdentify, cInteractive3DObject ObjectToBeAssociated)
        {
            cMetaBiologicalObject MetaObjectAssociated = null;

            // first check that the volume is associated to a metaobject
            MetaObjectAssociated = GetAssociatedMetaObject(ObjectToIdentify);
            if (MetaObjectAssociated == null) return null;
            MetaObjectAssociated.AddObject(ObjectToBeAssociated);
            return MetaObjectAssociated;
        }

        /// <summary>
        /// Return the closest biological object from the point. (distance is based on the centroid)
        /// </summary>
        /// <param name="Point">Input point</param>
        /// <returns></returns>
        public cInteractive3DObject FindTheClosestObjectCentroidFrom(cPoint3D Point, double DistanceMax)
        {
            cInteractive3DObject ClosestObject = null;
            double MinDist = double.MaxValue;
            double TmpDist;

            foreach (cMetaBiologicalObject MetaObjectAssociated in this)
            {
                foreach (cInteractive3DObject CurrentObject in MetaObjectAssociated)
                {
                    TmpDist = CurrentObject.GetCentroid().DistTo(Point);
                    if ((TmpDist < MinDist) && (TmpDist < DistanceMax))
                    {
                        MinDist = TmpDist;
                        ClosestObject = CurrentObject;
                    }
                }
            }
            return ClosestObject;
        }

        /// <summary>
        /// Return the closest biological object from the point. (distance is based on the centroid)
        /// </summary>
        /// <param name="Point">Input point</param>
        /// <returns></returns>
        public cBiological3DVolume FindTheClosestVolumeCentroidFrom(cPoint3D Point, double DistanceMax)
        {
            cBiological3DVolume ClosestObject = null;
            double MinDist = double.MaxValue;
            double TmpDist;

            foreach (cMetaBiologicalObject MetaObjectAssociated in this)
            {
                foreach (cInteractive3DObject CurrentObject in MetaObjectAssociated)
                {
                    if (((cObject3D)CurrentObject).GetType().Name != "cBiological3DVolume") continue;
                    TmpDist = CurrentObject.GetCentroid().DistTo(Point);
                    if ((TmpDist < MinDist) && (TmpDist < DistanceMax))
                    {
                        MinDist = TmpDist;
                        ClosestObject = (cBiological3DVolume)CurrentObject;
                    }
                }
            }
            return ClosestObject;
        }

        private cBiological3DVolume FindTheClosestVolumeFrom(cPoint3D Point, double DistanceMax, out cPoint3D ClosestPt)
        {
            ClosestPt = new cPoint3D(-1, -1, -1);
            cBiological3DVolume ClosestObject = null;
            double MinDist = double.MaxValue;

            double[] StartPoint = new double[3];
            StartPoint[0] = Point.X;
            StartPoint[1] = Point.Y;
            StartPoint[2] = Point.Z;

            foreach (cMetaBiologicalObject MetaObjectAssociated in this)
            {
                foreach (cInteractive3DObject CurrentObject in MetaObjectAssociated)
                {
                    if (((cObject3D)CurrentObject).GetType().Name != "cBiological3DVolume") continue;

                    cBiological3DVolume CurrentVolume = (cBiological3DVolume)(CurrentObject);
                    StartPoint[0] = Point.X - CurrentVolume.GetPosition().X;
                    StartPoint[1] = Point.Y - CurrentVolume.GetPosition().Y;
                    StartPoint[2] = Point.Z - CurrentVolume.GetPosition().Z;

                    IntPtr unmanagedPointer = Marshal.UnsafeAddrOfPinnedArrayElement(StartPoint, 0);

                    vtkCellLocator cellLocator = new vtkCellLocator();
                    cellLocator.SetDataSet(CurrentVolume.GetPolydata());
                    cellLocator.BuildLocator();

                    //Find the closest points to TestPoint
                    double[] closestPoint = new double[3];

                    IntPtr unmanagedPointer1 = Marshal.UnsafeAddrOfPinnedArrayElement(closestPoint, 0);

                    //the coordinates of the closest point will be returned here
                    double closestPointDist2 = 0;
                    int cellId = 0;
                    int subId = 0;
                    cellLocator.FindClosestPoint(unmanagedPointer, unmanagedPointer1, ref cellId, ref subId, ref closestPointDist2);


                    double RealDist = Math.Sqrt((Point.X - closestPoint[0] - CurrentVolume.GetPosition().X) * (Point.X - closestPoint[0] - CurrentVolume.GetPosition().X) +
                                    (Point.Y - closestPoint[1] - CurrentVolume.GetPosition().Y) * (Point.Y - closestPoint[1] - CurrentVolume.GetPosition().Y) +
                                    (Point.Z - closestPoint[2] - CurrentVolume.GetPosition().Z) * (Point.Z - closestPoint[2] - CurrentVolume.GetPosition().Z));

                    if ((closestPointDist2 < MinDist) && (RealDist < DistanceMax))
                    {
                        ClosestPt.X = (float)closestPoint[0] + CurrentVolume.GetPosition().X;
                        ClosestPt.Y = (float)closestPoint[1] + CurrentVolume.GetPosition().Y;
                        ClosestPt.Z = (float)closestPoint[2] + CurrentVolume.GetPosition().Z;

                        MinDist = closestPointDist2;
                        ClosestObject = CurrentVolume;
                    }
                }
            }
            return ClosestObject;
        }

        public cBiological3DVolume FindTheClosestVolumeFrom(cInteractive3DObject SourceObject, double DistanceMax, out cPoint3D ClosestPt)
        {

            if (((cObject3D)SourceObject).GetType().Name == "cBiologicalSpot")
                return FindTheClosestVolumeFrom(SourceObject.GetCentroid(), DistanceMax, out ClosestPt);



            //ClosestPt = new cPoint3D(0, 0, 0);
            //cBiological3DVolume ClosestObject = null;
            //double MinDist = double.MaxValue;

            //double[] StartPoint = new double[3];





            //StartPoint[0] = Point.X;
            //StartPoint[1] = Point.Y;
            //StartPoint[2] = Point.Z;

            //foreach (cMetaBiologicalObject MetaObjectAssociated in this)
            //{
            //    foreach (cBiological3DObject CurrentObject in MetaObjectAssociated)
            //    {
            //        if (((cObject3D)CurrentObject).GetType().Name != "cBiological3DVolume") continue;

            //        cBiological3DVolume CurrentVolume = (cBiological3DVolume)(CurrentObject);
            //        StartPoint[0] = Point.X - CurrentVolume.GetPosition().X;
            //        StartPoint[1] = Point.Y - CurrentVolume.GetPosition().Y;
            //        StartPoint[2] = Point.Z - CurrentVolume.GetPosition().Z;

            //        IntPtr unmanagedPointer = Marshal.UnsafeAddrOfPinnedArrayElement(StartPoint, 0);

            //        vtkCellLocator cellLocator = new vtkCellLocator();
            //        cellLocator.SetDataSet(CurrentVolume.GetPolydata());
            //        cellLocator.BuildLocator();

            //        //Find the closest points to TestPoint
            //        double[] closestPoint = new double[3];

            //        IntPtr unmanagedPointer1 = Marshal.UnsafeAddrOfPinnedArrayElement(closestPoint, 0);

            //        //the coordinates of the closest point will be returned here
            //        double closestPointDist2 = 0;
            //        int cellId = 0;
            //        int subId = 0;
            //        cellLocator.FindClosestPoint(unmanagedPointer, unmanagedPointer1, ref cellId, ref subId, ref closestPointDist2);

            //        if ((closestPointDist2 < MinDist) && (closestPointDist2 < DistanceMax))
            //        {
            //            ClosestPt.X = (float)closestPoint[0];
            //            ClosestPt.Y = (float)closestPoint[1];
            //            ClosestPt.Z = (float)closestPoint[2];
            //            MinDist = closestPointDist2;
            //            ClosestObject = CurrentVolume;
            //        }
            //    }
            //}
            //return ClosestObject;

            ClosestPt = null;
            cBiological3DVolume ClosestObject = null;

            return ClosestObject;

        }


    }



    public class cMetaBiologicalObject : List<cInteractive3DObject>
    {
        private cList3Dvolumes List3Dvolumes;
        private cList3DSpots List3DSpots;
        public string Name;
        public cInformation Information;
        cMetaBiologicalObjectList AssociatedList;
        cInteractive3DObject MasterObject;

        public cMetaBiologicalObject(string Name, cMetaBiologicalObjectList AssociatedList, cInteractive3DObject MasterObject)
        {
            this.Name = Name;
            this.AssociatedList = AssociatedList;
            //ListBiologicalObjects = new List<cBiological3DObject>();
            Information = new cInformation(this);
            List3Dvolumes = new cList3Dvolumes();
            List3DSpots = new cList3DSpots();
            this.MasterObject = AddObject(MasterObject);
        }

        public cInteractive3DObject GetMasterObject()
        {
            return this.MasterObject;
        }

        public cList3Dvolumes GetVolumesList()
        {
            return this.List3Dvolumes;
        }

        public cList3DSpots GetSpotsList()
        {
            return this.List3DSpots;
        }

        /// <summary>
        /// check if a specific biolo
        /// </summary>
        /// <param name="Biological3DObjectToIdentify"></param>
        /// <returns></returns>
        public bool IsPresent(cInteractive3DObject Biological3DObjectToIdentify)
        {
            bool Toreturn = false;

            foreach (cInteractive3DObject TmpBiologicalObject in this)
                if (TmpBiologicalObject == Biological3DObjectToIdentify) return true;
            return Toreturn;
        }

        public List<cBiological3DVolume> GetListVolumes(string Type)
        {
            foreach (List<cBiological3DVolume> ListcBiological3DVolume in this.List3Dvolumes)
            {
                if (ListcBiological3DVolume[0].GetType() == Type) return ListcBiological3DVolume;
            }

            return null;
        }

        public List<cBiologicalSpot> GetListSpots(string Type)
        {
            foreach (List<cBiologicalSpot> ListcList3DSpots in this.List3DSpots)
            {
                if (ListcList3DSpots[0].GetType() == Type) return ListcList3DSpots;
            }

            return null;
        }

        public List<string> GetListTypes()
        {
            List<string> ListTypes = new List<string>();

            foreach (List<cBiological3DVolume> ListVol in this.List3Dvolumes)
                ListTypes.Add(ListVol[0].GetType());

            foreach (List<cBiologicalSpot> ListSpot in this.List3DSpots)
                ListTypes.Add(ListSpot[0].GetType());

            return ListTypes;

        }

        #region Information about the Meta Object

        public class cInformation
        {
            cMetaBiologicalObject CurrentMetaObject;
            public cInformation(cMetaBiologicalObject CurrentMetaObject)
            {
                this.CurrentMetaObject = CurrentMetaObject;
            }

            /// <summary>
            /// return the name of meta-object
            /// </summary>
            /// <returns>Name</returns>
            public string GetName()
            {
                return CurrentMetaObject.Name;
            }

            /// <summary>
            /// Return the number of object of a certain type contained within the MetaObject
            /// </summary>
            /// <param name="Type">Type of interest</param>
            /// <returns>The number of object</returns>
            public int GetNumberOfObjects(string Type)
            {
                int Number = 0;

                foreach (cInteractive3DObject TmpBiologicalObject in CurrentMetaObject)
                {
                    if (TmpBiologicalObject.GetType() == Type) Number++;
                }
                return Number;
            }

            /// <summary>
            /// return the number of biological object included in the meta object.
            /// </summary>
            /// <returns></returns>
            public int NumberOfObjects()
            {
                return CurrentMetaObject.Count();
            }

            /// <summary>
            /// Return all the descriptors coming from the global meta object family 
            /// </summary>
            /// <returns>the list of different types</returns>
            public List<string> GetGlobalDescriptorNames()
            {
                List<string> DescriptorNames = new List<string>();

                if (CurrentMetaObject.AssociatedList == null) return DescriptorNames;
                foreach (cMetaBiologicalObject Meta in CurrentMetaObject.AssociatedList)
                {
                    List<string> ListTypes = Meta.GetListTypes();
                    foreach (string Type in ListTypes)
                    {
                        int OccurenceNumber = 0;
                        for (int i = 0; i < DescriptorNames.Count; i++)
                        {
                            string DescName = DescriptorNames[i];
                            if (DescName == Type) OccurenceNumber++; ;
                        }
                        if (OccurenceNumber == 0) DescriptorNames.Add(Type);
                    }
                }
                return DescriptorNames;
            }

            /// <summary>
            /// Return the number of each type contained within this Meta-Object.
            /// </summary>
            /// <param name="Descriptors">List of the types of interest</param>
            /// <returns></returns>
            public List<double> GetInformation(List<string> Descriptors)
            {
                List<double> DescriptorList = new List<double>();
                if (Descriptors == null) return null;
                foreach (string DescType in Descriptors)
                    DescriptorList.Add(this.GetNumberOfObjects(DescType));

                return DescriptorList;
            }
        }

        #endregion


        /// <summary>
        /// Return the meta object signature composed of: the master object signature + the metaobject composition
        /// </summary>
        /// <returns>the signature as describe above</returns>
        public List<double> GetSignature()
        {
            cInteractive3DObject MasterObj = this.GetMasterObject();
            List<double> ListDescMaster = null;

            cObject3D Master3d = (cObject3D)(MasterObj);

            if (Master3d.GetType().ToString().IndexOf("cBiological3DVolume") != -1)
            {
                cBiological3DVolume TmpVol = (cBiological3DVolume)Master3d;
                ListDescMaster = TmpVol.Information.GetInformation();
            }
            else if (Master3d.GetType().ToString().IndexOf("cBiologicalSpot") != -1)
            {
                cBiologicalSpot TmpSpot = (cBiologicalSpot)Master3d;
                ListDescMaster = TmpSpot.Information.GetInformation();
            }
            List<double> ResMetaObject = this.Information.GetInformation(this.Information.GetGlobalDescriptorNames());

            ListDescMaster.AddRange(ResMetaObject);

            return ListDescMaster;
        }



        /// <summary>
        /// return 
        /// </summary>
        /// <returns></returns>
        public List<string> GetSignatureNames()
        {



            List<string> ListProfilName = new List<string>();

            // ListProfilName.Add("Meta Object Name");

            cInteractive3DObject MasterObj = this.GetMasterObject();
            cObject3D Master3d = (cObject3D)(MasterObj);

            if (Master3d.GetType().ToString().IndexOf("cBiological3DVolume") != -1)
            {
                cBiological3DVolume TmpVol = (cBiological3DVolume)Master3d;
                ListProfilName.AddRange(TmpVol.Information.GetDescriptorNames());
            }
            else if (Master3d.GetType().ToString().IndexOf("cBiologicalSpot") != -1)
            {
                cBiologicalSpot TmpSpot = (cBiologicalSpot)Master3d;
                ListProfilName.AddRange(TmpSpot.Information.GetDescriptorNames());
            }

            List<string> Descriptors = this.Information.GetGlobalDescriptorNames();

            ListProfilName.AddRange(Descriptors);
            return ListProfilName;

        }

        public cInteractive3DObject AddObject(cInteractive3DObject Object)
        {
            this.Add(Object);
            Object.DefineMetaContainer(this);

            // add this object in the right list
            cObject3D Object3d = (cObject3D)Object;

            if (Object3d.GetType().Name == "cBiological3DVolume")
            {
                int Idx = List3Dvolumes.IsContainType(Object.GetType());
                if (Idx == -1)
                {
                    List3Dvolumes.Add(new List<cBiological3DVolume>());
                    List3Dvolumes[List3Dvolumes.Count - 1].Add((cBiological3DVolume)Object);
                }
                else
                {
                    List3Dvolumes[Idx].Add((cBiological3DVolume)Object);
                }
            }
            else if (Object3d.GetType().Name == "cBiologicalSpot")
            {
                int Idx = List3DSpots.IsContainType(Object.GetType());
                if (Idx == -1)
                {
                    List3DSpots.Add(new List<cBiologicalSpot>());
                    List3DSpots[List3DSpots.Count - 1].Add((cBiologicalSpot)Object);
                }
                else
                {
                    List3DSpots[Idx].Add((cBiologicalSpot)Object);
                }
            }

            return Object;
        }

        vtkActor BoundingBoxActor = null;

        private List<cPoint3D> GetMinMaxPt()
        {
            List<cPoint3D> ListMinMax = new List<cPoint3D>();

            double MinX = double.MaxValue, MinY = double.MaxValue, MinZ = double.MaxValue;
            double MaxX = double.MinValue, MaxY = double.MinValue, MaxZ = double.MinValue;

            foreach (List<cBiologicalSpot> ListSpot in this.List3DSpots)
            {
                if (ListSpot.Count == 0) continue;
                foreach (cBiologicalSpot Spot in ListSpot)
                {
                    double[] Tmp = Spot.GetActor().GetBounds();
                    if (Tmp[0] < MinX) MinX = Tmp[0];
                    if (Tmp[2] < MinY) MinY = Tmp[2];
                    if (Tmp[4] < MinZ) MinZ = Tmp[4];

                    if (Tmp[1] > MaxX) MaxX = Tmp[1];
                    if (Tmp[3] > MaxY) MaxY = Tmp[3];
                    if (Tmp[5] > MaxZ) MaxZ = Tmp[5];
                }
            }
            foreach (List<cBiological3DVolume> ListVolume in this.List3Dvolumes)
            {
                if (ListVolume.Count == 0) continue;
                {
                    foreach (cBiological3DVolume Volume in ListVolume)
                    {
                        double[] Tmp = Volume.GetActor().GetBounds();
                        if (Tmp[0] < MinX) MinX = Tmp[0];
                        if (Tmp[2] < MinY) MinY = Tmp[2];
                        if (Tmp[4] < MinZ) MinZ = Tmp[4];

                        if (Tmp[1] > MaxX) MaxX = Tmp[1];
                        if (Tmp[3] > MaxY) MaxY = Tmp[3];
                        if (Tmp[5] > MaxZ) MaxZ = Tmp[5];
                    }
                }
            }

            cPoint3D Min = new cPoint3D((float)MinX, (float)MinY, (float)MinZ);
            ListMinMax.Add(Min);

            cPoint3D Max = new cPoint3D((float)MaxX, (float)MaxY, (float)MaxZ);
            ListMinMax.Add(Max);

            return ListMinMax;
        }

        public cGeometric3DObject GenerateAndDisplayBoundingBox(float width, Color Colour, bool IsWired, c3DWorld CurrentWorld)
        {
            c3DCube Box = new c3DCube();
            List<cPoint3D> MinMaxPt = this.GetMinMaxPt();

            vtkCubeSource BoundingBox = vtkCubeSource.New();
            BoundingBox.SetBounds(MinMaxPt[0].X, MinMaxPt[1].X, MinMaxPt[0].Y, MinMaxPt[1].Y, MinMaxPt[0].Z, MinMaxPt[1].Z);

            vtkPolyDataMapper SphereMapper = vtkPolyDataMapper.New();
            vtkActor SphereActor = vtkActor.New();
            SphereMapper = vtkPolyDataMapper.New();

            SphereMapper.SetInputConnection(BoundingBox.GetOutputPort());
            SphereActor.SetMapper(SphereMapper);

            double[] CenterPos = SphereActor.GetCenter();
            SphereActor.SetPickable(0);
            if (IsWired) SphereActor.GetProperty().SetRepresentationToWireframe();
            else
                SphereActor.GetProperty().SetOpacity(0.2);
            //Color C = Color.YellowGreen;
            SphereActor.GetProperty().SetColor(Colour.R / 255.0, Colour.G / 255.0, Colour.B / 255.0);
            CurrentWorld.ren1.AddActor(SphereActor);
            return Box;
        }

        public List<cGeometric3DObject> GenerateLines(cInteractive3DObject Source, string Type, float width)
        {
            List<cGeometric3DObject> ListLine = new List<cGeometric3DObject>();


            foreach (List<cBiologicalSpot> ListSpot in this.List3DSpots)
            {
                if (ListSpot.Count == 0) continue;
                if (ListSpot[0].GetType() == Type)
                {
                    foreach (cBiologicalSpot Spot in ListSpot)
                    {

                        c3DLine Line = new c3DLine(Source.GetCentroid(), new cPoint3D((float)Spot.GetActor().GetPosition()[0], (float)Spot.GetActor().GetPosition()[1], (float)Spot.GetActor().GetPosition()[2]));
                        Line.GetActor().GetProperty().SetLineWidth(width);
                        ListLine.Add(Line);
                    }
                }
            }


            foreach (List<cBiological3DVolume> ListVolume in this.List3Dvolumes)
            {
                if (ListVolume.Count == 0) continue;
                if (ListVolume[0].GetType() == Type)
                {
                    foreach (cBiological3DVolume Volume in ListVolume)
                    {
                        c3DLine Line = new c3DLine(Source.GetCentroid(), Volume.GetCentroid());
                        Line.GetActor().GetProperty().SetLineWidth(width);
                        ListLine.Add(Line);
                    }
                }
            }
            return ListLine;
        }

        #region Delaunay
        public c2DDelaunay GenerateDelaunay(bool IsWire)
        {
            List<cPoint3D> ListPts = new List<cPoint3D>();
            for (int i = 0; i < this.Count; i++)
            {
                ListPts.Add(this[i].GetCentroid());
            }

            c2DDelaunay CurrentDelaunay = new c2DDelaunay(ListPts, IsWire);
            if (IsWire)
            {

                CurrentDelaunay.GetActor().GetProperty().SetLineWidth(2);
            }
            return CurrentDelaunay;
        }

        public c2DDelaunay GenerateDelaunay(float width, bool IsWire)
        {
            List<cPoint3D> ListPts = new List<cPoint3D>();
            for (int i = 0; i < this.Count; i++)
            {
                ListPts.Add(this[i].GetCentroid());
            }

            c2DDelaunay CurrentDelaunay = new c2DDelaunay(ListPts, IsWire);
            if (IsWire)
            {
                CurrentDelaunay.GetActor().GetProperty().SetLineWidth(width);
                CurrentDelaunay.GetActor().GetProperty().SetLineStipplePattern(61680);
            }

            return CurrentDelaunay;
        }

        public c2DDelaunay GenerateDelaunay(string Type, float width, bool IsWire)
        {
            List<cPoint3D> ListPts = new List<cPoint3D>();
            for (int i = 0; i < this.Count; i++)
            {
                cInteractive3DObject TmpObject = this[i];
                if (TmpObject.GetType() == Type) ListPts.Add(this[i].GetCentroid());
            }

            c2DDelaunay CurrentDelaunay = new c2DDelaunay(ListPts, IsWire);
            if (IsWire)
            {
                CurrentDelaunay.GetActor().GetProperty().SetLineWidth(width);
                CurrentDelaunay.GetActor().GetProperty().SetLineStipplePattern(61680);
            }

            return CurrentDelaunay;
        }


        public c2DDelaunay GenerateDelaunay(string Type, float width, Color Colour, bool IsWire)
        {
            c2DDelaunay CurrentDelaunay = GenerateDelaunay(Type, width, IsWire);
            CurrentDelaunay.GetActor().GetProperty().SetColor(Colour.R / 255.0, Colour.G / 255.0, Colour.B / 255.0);
            return CurrentDelaunay;
        }

        #endregion
    }

}