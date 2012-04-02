using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Kitware.VTK;
using LibPlateAnalysis;

namespace HCSAnalyzer.Classes._3D
{
    public class cGeometric3DObject : cObject3D
    {

        public vtkAlgorithmOutput AlgoOutPut = null;

        public cGeometric3DObject()
        {
            vtk_Actor.SetPickable(0);
            ObjectType = "Geometrical";
        }

        public int GetHashCode()
        {
            return -1;
        }

        public cGeometric3DObject(vtkActor CurrentActor)
        {
            vtk_Actor = CurrentActor;
        }

        public cGeometric3DObject(vtkActor CurrentActor, Color NewColor)
        {
            CurrentActor.GetProperty().SetColor(NewColor.R / 255.0, NewColor.G / 255.0, NewColor.B / 255.0);
            vtk_Actor = CurrentActor;
            //vtk_Actor.SetPickable(0);
            ObjectType = "Geometrical";
        }
    }

    /// <summary>
    /// 3D sphere Object
    /// </summary>
    public class c3DSphere : cGeometric3DObject
    {
        public double Radius;
        public vtkSphereSource sphere;

        private void CreateSphere(cPoint3D Center, double Radius, Color Colour, int Precision)
        {
            Position = new cPoint3D(Center.X, Center.Y, Center.Z);

            this.Radius = Radius;
            this.Colour = Colour;

            sphere = vtkSphereSource.New();
            sphere.SetThetaResolution(Precision);
            sphere.SetPhiResolution(Precision);
            sphere.SetRadius(Radius);
            vtk_PolyDataMapper = vtkPolyDataMapper.New();
            vtk_PolyDataMapper.SetInputConnection(sphere.GetOutputPort());

            CreateVTK3DObject(2);
        }

        public c3DSphere(cPoint3D Center, double Radius, Color Colour)
        {
            CreateSphere(Center, Radius, Colour, 16);
        }

        public c3DSphere(cPoint3D Center, double Radius, Color Colour, int Precision)
        {
            CreateSphere(Center, Radius, Colour, Precision);
        }

        public c3DSphere(cPoint3D Center, double Radius)
        {
            CreateSphere(Center, Radius, Color.Red, 16);
        }
    }


    public class c3DDRC : cGeometric3DObject
    {
        public vtkParametricSpline Spline;

        private void Create3DDRC(cDRC DRCToDraw, cDRC_Region AssociatedRegion, Color Color, double Min, double Max)
        {
            if (DRCToDraw.ResultFit == null) return;

            Position = new cPoint3D(AssociatedRegion.PosXMin + 0.5, AssociatedRegion.PosYMin + 0.2, 0);
            this.Colour = Color;
            vtkPoints points = vtkPoints.New();

            vtkUnsignedCharArray colors = vtkUnsignedCharArray.New();
            colors.SetName("Colors");
            colors.SetNumberOfComponents(3);
            colors.SetNumberOfTuples(AssociatedRegion.NumConcentrations);

            for (int i = 0; i < AssociatedRegion.NumConcentrations; i++)
            {
                if (i >= DRCToDraw.ResultFit.Y_Estimated.Count) continue;
                double PosZ = 8 - ((DRCToDraw.ResultFit.GetNormalizedY_Estimated()[i]) * 8);

                points.InsertPoint(i, i, 0, PosZ);
                colors.InsertTuple3(i / AssociatedRegion.NumConcentrations, i / AssociatedRegion.NumConcentrations, 255, i / AssociatedRegion.NumConcentrations);
            }

            Spline = vtkParametricSpline.New();
            Spline.SetPoints(points);
            Spline.ClosedOff();

            vtkParametricFunctionSource SplineSource = vtkParametricFunctionSource.New();
            SplineSource.SetParametricFunction(Spline);

            //     SplineSource.GetPolyDataInput(0).GetPointData().AddArray(colors);

            vtkLinearExtrusionFilter extrude = vtkLinearExtrusionFilter.New();
            extrude.SetInputConnection(SplineSource.GetOutputPort());

            //extrude.GetPolyDataInput(0).GetPointData().AddArray(colors);
            extrude.SetScaleFactor(AssociatedRegion.NumReplicate - 0.2);
            //extrude.SetExtrusionTypeToNormalExtrusion();

            extrude.SetExtrusionTypeToVectorExtrusion();
            extrude.SetVector(0, 1, 0);

            vtk_PolyDataMapper = vtkPolyDataMapper.New();
            vtk_PolyDataMapper.SetInputConnection(extrude.GetOutputPort()/*SplineSource.GetOutputPort()*/);
            vtk_PolyDataMapper.GetInput().GetPointData().AddArray(colors);
            vtk_PolyDataMapper.ScalarVisibilityOn();
            vtk_PolyDataMapper.SetScalarModeToUsePointFieldData();
            vtk_PolyDataMapper.SelectColorArray("Colors");

            CreateVTK3DObject(3);
        }

        public c3DDRC(cDRC DRCToDraw, cDRC_Region AssociatedRegion, Color Color, double Min, double Max)
        {
            Create3DDRC(DRCToDraw, AssociatedRegion,Color,  Min, Max);
        }

        public c3DDRC(cDRC DRCToDraw, cDRC_Region AssociatedRegion, double Min, double Max)
        {
            Create3DDRC(DRCToDraw, AssociatedRegion, Color.White, Min, Max);
        }
    }


    public class c3DThinPlate : cGeometric3DObject
    {
       


       // double regularization = 0.0;
        double bending_energy = 0.0;

        //List<Vector3> control_points = new List<Vector3>();

        /// <summary> Thin-Plate-Spline base function
        /// </summary>
        /// <param name="r">the function parameter</param>
        /// <returns></returns>
        public double tps_base_func(double r)
        {
            if (r == 0.0)
                return 0.0;
            else
                return r * r * Math.Log(r);
        }

        /// <summary>
        /// Compute the Thin Plate Spline of the image, return a 2D tab
        /// </summary>
        /// <param name="control_points">Control points  </param>    
        /// <param name="input">Input image to get the dim xy</param>
        public double[,] calc_tps(List<cPoint3D> control_points, cDRC_Region AssociatedRegion, double Regularization)
        {

            int p = control_points.Count;
            if (p < 3) return null;
            double[,] grid = new double[AssociatedRegion.SizeX, AssociatedRegion.SizeY];
            Matrix mtx_l = new Matrix(p + 3, p + 3);
            Matrix mtx_v = new Matrix(p + 3, 1);
            Matrix mtx_orig_k = new Matrix(p, p);
            double a = 0.0;
            for (int i = 0; i < p; ++i)
            {
                for (int j = i + 1; j < p; ++j)
                {
                    cPoint3D pt_i = new cPoint3D(control_points[i].X, control_points[i].Y, control_points[i].Z);
                    cPoint3D pt_j = new cPoint3D(control_points[j].X, control_points[j].Y, control_points[j].Z);

                    pt_i.Y = pt_j.Y = 0;

                    //double elen = Math.Sqrt((pt_i.X - pt_j.X) * (pt_i.X - pt_j.X) + (pt_i.Z - pt_j.Z) * (pt_i.Z - pt_j.Z));
                    double elen = pt_i.DistTo(pt_j);
                    mtx_l[i, j] = mtx_l[j, i] = mtx_orig_k[i, j] = mtx_orig_k[j, i] = tps_base_func(elen);
                    a += elen * 2; // same for upper & lower tri
                }
            }
            a /= (double)(p * p);
            //regularization = 0.3f;
            //Fill the rest of L
            for (int i = 0; i < p; ++i)
            {
                //diagonal: reqularization parameters (lambda * a^2)

                mtx_l[i, i] = mtx_orig_k[i, i] = Regularization * (a * a);



                // P (p x 3, upper right)
                mtx_l[i, p + 0] = 1.0;
                mtx_l[i, p + 1] = control_points[i].X;
                mtx_l[i, p + 2] = control_points[i].Z;

                // P transposed (3 x p, bottom left)
                mtx_l[p + 0, i] = 1.0;
                mtx_l[p + 1, i] = control_points[i].X;
                mtx_l[p + 2, i] = control_points[i].Z;
            }
            // O (3 x 3, lower right)
            for (int i = p; i < p + 3; ++i)
                for (int j = p; j < p + 3; ++j)
                    mtx_l[i, j] = 0.0;


            // Fill the right hand vector V
            for (int i = 0; i < p; ++i)
                mtx_v[i, 0] = control_points[i].Y;

            mtx_v[p + 0, 0] = mtx_v[p + 1, 0] = mtx_v[p + 2, 0] = 0.0;
            // Solve the linear system "inplace" 
            Matrix mtx_v_res = new Matrix(p + 3, 1);

            LuDecomposition ty = new LuDecomposition(mtx_l);



            mtx_v_res = ty.Solve(mtx_v);
            if (mtx_v_res == null)
            {
                return null;
            }


            // Interpolate grid heights
            for (int x = 0; x < AssociatedRegion.SizeX; ++x)
            {
                for (int z = 0; z < AssociatedRegion.SizeY; ++z)
                {

                    //float x = 0f; float z = 0.5f;
                    double h = mtx_v_res[p + 0, 0] + mtx_v_res[p + 1, 0] * (float)x / (float)AssociatedRegion.SizeX + mtx_v_res[p + 2, 0] * (float)z / (float)AssociatedRegion.SizeY;
                    //double h = mtx_v[p + 0, 0] + mtx_v[p + 1, 0] * (float)x + mtx_v[p + 2, 0] * (float)z ;
                    cPoint3D pt_ia;
                    cPoint3D pt_cur = new cPoint3D((float)x / (float)AssociatedRegion.SizeX, 0, (float)z / (float)AssociatedRegion.SizeY);
                    //Vector3 pt_cur = new Vector3((float)x , 0, (float)z);
                    for (int i = 0; i < p; ++i)
                    {
                        pt_ia = control_points[i];
                        pt_ia.Y = 0;
                        h += mtx_v_res[i, 0] * tps_base_func(pt_ia.DistTo(pt_cur));
                    }

                    grid[x, z] = h;
                }
            }
            // Calc bending energy
            Matrix w = new Matrix(p, 1);
            for (int i = 0; i < p; ++i)
                w[i, 0] = mtx_v_res[i, 0];

            Matrix be;

            be = Matrix.Multiply(Matrix.Multiply(w.Transpose(), mtx_orig_k), w);
            bending_energy = be[0, 0];

            Console.WriteLine("be= " + be[0, 0]);
            return grid;


        }

        private void Create(cDRC_Region AssociatedRegion, Color Color, double Regularization)
        {
            Position = new cPoint3D(AssociatedRegion.PosXMin + 1, AssociatedRegion.PosYMin + 0.7, 0);

            List<cPoint3D> ListPtSigma = new List<cPoint3D> ( );

            double GlobalMin = double.MaxValue;
            double GlobalMax = double.MinValue;
            for (int j = 0; j < AssociatedRegion.SizeY; j++)
                for (int i = 0; i < AssociatedRegion.SizeX; i++)
                {
                    cWell TmpWell = AssociatedRegion.GetListWells()[j][i];
                    if (TmpWell == null) continue;
                    double PosZ = TmpWell.ListDescriptors[TmpWell.AssociatedPlate.ParentScreening.ListDescriptors.CurrentSelectedDescriptor].GetValue();
                    if (PosZ >= GlobalMax) GlobalMax = PosZ;
                    if (PosZ <= GlobalMin) GlobalMin = PosZ;

                    ListPtSigma.Add(new cPoint3D(i/(double)AssociatedRegion.SizeX,PosZ,j/(double)AssociatedRegion.SizeY));
                }

            if (GlobalMax == GlobalMin) return;



            double[,] ResultThinPlate = calc_tps(ListPtSigma, AssociatedRegion, Regularization);

            if (ResultThinPlate == null)
            {
               // MessageBox.Show("Error in computing the associated thinplate !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

          //  Position = new cPoint3D(0, 0, 0);
            this.Colour = Color;
            vtkPoints points = vtkPoints.New();
           // vtkPoints points0 = vtkPoints.New();

            //vtkUnsignedCharArray colors = vtkUnsignedCharArray.New();
            //colors.SetName("Colors");
            //colors.SetNumberOfComponents(3);
            //colors.SetNumberOfTuples(AssociatedRegion.SizeX * AssociatedRegion.SizeY);

            


            int Idx = 0;
            for(int j=0;j<AssociatedRegion.SizeY;j++)
                for (int i = 0; i < AssociatedRegion.SizeX; i++)
            {
                cWell TmpWell = AssociatedRegion.GetListWells()[j][i];

                double PosZ  = 8 - ( (ResultThinPlate[i,j]-GlobalMin)/(GlobalMax-GlobalMin))*8;
                
                    
                //points.InsertPoint(Idx,TmpWell.GetPosX(), TmpWell.GetPosY(), PosZ);

                    //  points.InsertPoint(Idx, TmpWell.GetPosX(), TmpWell.GetPosY(), 0);
                points.InsertPoint(Idx++, i, j, PosZ);
              //  colors.InsertTuple3(Idx++, 1, 1, 1);

            }
 
              vtkPolyData profile = vtkPolyData.New();
            profile.SetPoints(points);

            vtkDelaunay2D del = vtkDelaunay2D.New();
            del.SetInput(profile);
            del.SetTolerance(0.001);



        vtkButterflySubdivisionFilter subdivisionFilter = vtkButterflySubdivisionFilter.New();
    subdivisionFilter.SetInput(del.GetOutput());
    subdivisionFilter.SetNumberOfSubdivisions(2);
    subdivisionFilter.Update();
 





            vtk_PolyDataMapper = vtkPolyDataMapper.New();

            AlgoOutPut = subdivisionFilter.GetOutputPort();

            vtk_PolyDataMapper.SetInputConnection(AlgoOutPut);
           // vtk_PolyDataMapper.GetInput().GetPointData().AddArray(colors);
           // vtk_PolyDataMapper.ScalarVisibilityOn();
           // vtk_PolyDataMapper.SetScalarModeToUsePointFieldData();
           // vtk_PolyDataMapper.SelectColorArray("Colors");





 //         vtkLinearExtrusionFilter  extrude = vtkLinearExtrusionFilter.New();
 //         extrude.SetInput(cutter.GetOutput());
 //extrude.SetScaleFactor(1);
 //extrude.SetExtrusionTypeToNormalExtrusion();
 //extrude.SetVector(1, 1, 1);

  //vtkRotationalExtrusionFilter extrude = vtkRotationalExtrusionFilter.New();
  //extrude.SetInput(cutter.GetOutput());
  //extrude.SetResolution(60);
  //extrude.Update();

 

//vtk_PolyDataMapper.SetInputConnection(tubeFilter.GetOutputPort());
 



            CreateVTK3DObject(3);

            SetColor(Color);
           // SetOpacity(1);
            SetToSurface();
        }

        public c3DThinPlate(cDRC_Region AssociatedRegion, Color Color, double Regularization)
        {
            Create(AssociatedRegion, Color, Regularization);
        }

        public c3DThinPlate(cDRC_Region AssociatedRegion, double Regularization)
        {
            Create(AssociatedRegion, Color.White, Regularization);
        }
    }





    /// <summary>
    /// 2D Delaunay mesh
    /// </summary>
    public class c2DDelaunay : cGeometric3DObject
    {
        private void CreateDelaunay(List<cPoint3D> ListPts, Color Colour, bool IsWire)
        {
            Position = new cPoint3D(0, 0, 0);
            this.Colour = Colour;
            vtkPoints ListCentroid = vtkPoints.New();

            for (int i = 0; i < ListPts.Count; i++)
                ListCentroid.InsertPoint(i, ListPts[i].X, ListPts[i].Y, ListPts[i].Z);

            vtkPolyData profile = vtkPolyData.New();
            profile.SetPoints(ListCentroid);

            vtkDelaunay2D del = vtkDelaunay2D.New();
            del.SetInput(profile);
            del.SetTolerance(0.001);

            vtk_PolyDataMapper = vtkPolyDataMapper.New();
            vtk_PolyDataMapper.SetInputConnection(del.GetOutputPort());

            CreateVTK3DObject(0);
            if (IsWire) SetToWireFrame();
            else SetToSurface();
        }

        public c2DDelaunay(List<cPoint3D> ListPts, Color Colour, bool IsWire)
        {
            CreateDelaunay(ListPts, Colour, IsWire);
        }

        public c2DDelaunay(List<cPoint3D> ListPts, bool IsWire)
        {
            CreateDelaunay(ListPts, Color.PapayaWhip, IsWire);
        }
    }


    /// <summary>
    /// 3D Line Object
    /// </summary>
    public class c3DLine : cGeometric3DObject
    {
        public vtkLineSource Line;
        private cPoint3D Point1;
        private cPoint3D Point2;

        private void CreateLine(cPoint3D Point1, cPoint3D Point2, Color Colour)
        {
            Position = new cPoint3D(Point1.X, Point1.Y, Point1.Z);

            Position = new cPoint3D(0, 0, 0);

            this.Colour = Colour;
            Line = vtkLineSource.New();

            this.Point1 = new cPoint3D(Point1.X, Point1.Y, Point1.Z);
            this.Point2 = new cPoint3D(Point2.X, Point2.Y, Point2.Z);

            Line.SetPoint1(Point1.X, Point1.Y, Point1.Z);
            Line.SetPoint2(Point2.X, Point2.Y, Point2.Z);


            vtk_PolyDataMapper = vtkPolyDataMapper.New();
            vtk_PolyDataMapper.SetInputConnection(Line.GetOutputPort());


            CreateVTK3DObject(0);
        }

        public c3DLine(cPoint3D Point1, cPoint3D Point2, Color Colour)
        {
            CreateLine(Point1, Point2, Colour);
        }

        public c3DLine(cPoint3D Point1, cPoint3D Point2)
        {
            CreateLine(Point1, Point2, Color.White);
        }

        public void DisplayLenght(c3DWorld CurrentWorld, double scale)
        {
            vtkFollower TextActor = vtkFollower.New();
            vtkPolyDataMapper TextMapper = vtkPolyDataMapper.New();
            vtkVectorText TextVTK = vtkVectorText.New();


            double Dist = Point1.DistTo(Point2);

            TextVTK.SetText(Dist.ToString("N2"));
            TextMapper.SetInputConnection(TextVTK.GetOutputPort());
            TextActor.SetMapper(TextMapper);
            TextActor.SetPosition(this.GetActor().GetCenter()[0], this.GetActor().GetCenter()[1], this.GetActor().GetCenter()[2]);
            TextActor.SetPickable(0);
            TextActor.SetScale(scale);

            CurrentWorld.ren1.AddActor(TextActor);
            TextActor.SetCamera(CurrentWorld.ren1.GetActiveCamera());

        }
    }



    /// <summary>
    /// 3D Line Object
    /// </summary>
    public class c3DText : cGeometric3DObject
    {
        // public vtkLineSource Line;
        private cPoint3D Point1;
        // private cPoint3D Point2;

        //private void CreateLine(cPoint3D Point1, cPoint3D Point2, Color Colour)
        //{
        //    Position = new cPoint3D(Point1.X, Point1.Y, Point1.Z);

        //    Position = new cPoint3D(0, 0, 0);

        //    this.Colour = Colour;
        //    Line = vtkLineSource.New();

        //    this.Point1 = new cPoint3D(Point1.X, Point1.Y, Point1.Z);
        //    this.Point2 = new cPoint3D(Point2.X, Point2.Y, Point2.Z);

        //    Line.SetPoint1(Point1.X, Point1.Y, Point1.Z);
        //    Line.SetPoint2(Point2.X, Point2.Y, Point2.Z);


        //    vtk_PolyDataMapper = vtkPolyDataMapper.New();
        //    vtk_PolyDataMapper.SetInputConnection(Line.GetOutputPort());


        //    CreateVTK3DObject(0);
        //}

        public c3DText(c3DWorld CurrentWorld, string TextToDisplay, cPoint3D Position, Color Colour, double Scale)
        {
            // CreateLine(Point1, Point2, Colour);

            vtkFollower TextActor = vtkFollower.New();
            vtkPolyDataMapper TextMapper = vtkPolyDataMapper.New();
            vtkVectorText TextVTK = vtkVectorText.New();


            //  double Dist = Point1.DistTo(Point2);

            TextVTK.SetText(TextToDisplay);
            TextMapper.SetInputConnection(TextVTK.GetOutputPort());
            TextActor.SetMapper(TextMapper);
            TextActor.SetPosition(Position.X, Position.Y, Position.Z);
            TextActor.SetPickable(0);
            TextActor.SetScale(Scale);

            CurrentWorld.ren1.AddActor(TextActor);
            TextActor.SetCamera(CurrentWorld.ren1.GetActiveCamera());

        }
    }



    /// <summary>
    /// 3D cube object
    /// </summary>
    public class c3DCube : cGeometric3DObject
    {
        public vtkCubeSource BoundingBox;

        public void Create(cPoint3D MinPt, cPoint3D MaxPt, Color Colour)
        {
            Position = new cPoint3D(0, 0, 0);

            this.Colour = Colour;
            BoundingBox = vtkCubeSource.New();
            BoundingBox.SetBounds(MinPt.X, MaxPt.X, MinPt.Y, MaxPt.Y, MinPt.Z, MaxPt.Z);

            vtk_PolyDataMapper = vtkPolyDataMapper.New();
            vtk_PolyDataMapper.SetInputConnection(BoundingBox.GetOutputPort());

            CreateVTK3DObject(0);
        }
    }


    /// <summary>
    /// 3D plane object
    /// </summary>
    public class c3DPlane : cGeometric3DObject
    {
        public vtkPlaneSource Plane;

        private void CreatePlane(cPoint3D Axis1, cPoint3D Axis2, cPoint3D Origin, Color Colour)
        {
            Position = new cPoint3D(Origin.X, Origin.Y, Origin.Z);

            this.Colour = Colour;
            Plane = vtkPlaneSource.New();
            Plane.SetPoint1(Axis1.X, Axis1.Y, Axis1.Z);
            Plane.SetPoint2(Axis2.X, Axis2.Y, Axis2.Z);
            Plane.SetOrigin(0, 0, 0);
            Plane.SetXResolution(1);
            Plane.SetYResolution(1);

            vtk_PolyDataMapper = vtkPolyDataMapper.New();
            vtk_PolyDataMapper.SetInputConnection(Plane.GetOutputPort());

            CreateVTK3DObject(0);
        }

        public c3DPlane(cPoint3D Point1, cPoint3D Point2, cPoint3D Origin, Color Colour)
        {
            CreatePlane(Point1, Point2, Origin, Colour);
        }

        public c3DPlane(cPoint3D Point1, cPoint3D Point2, cPoint3D Origin)
        {
            CreatePlane(Point1, Point2, Origin, Color.White);
        }
    }
}
