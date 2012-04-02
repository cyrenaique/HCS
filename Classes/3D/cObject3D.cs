using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Kitware.VTK;


namespace HCSAnalyzer.Classes._3D
{

    /// <summary>
    /// cPoint3D class
    /// </summary>
    public class cPoint3D
    {
        public cPoint3D(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public double X;
        public double Y;
        public double Z;


        public double DistTo(cPoint3D DestPoint)
        {
            return Math.Sqrt((DestPoint.X - this.X) * (DestPoint.X - this.X) + (DestPoint.Y - this.Y) * (DestPoint.Y - this.Y) + (DestPoint.Z - this.Z) * (DestPoint.Z - this.Z));
        }


    }


    /// <summary>
    /// cObject3D
    /// </summary>
    public abstract class cObject3D
    {
        public cPoint3D Position;
        public Color Colour;
        public vtkPolyDataMapper vtk_PolyDataMapper;
        protected vtkActor vtk_Actor;
        public string ObjectType;

        protected List<vtkActor> ListActors;


        public vtkActor GetActor()
        {
            return vtk_Actor;
        }

        public cObject3D()
        {
            vtk_Actor = vtkActor.New();
        }

        public void AddMeToTheWorld(vtkRenderer World)
        {
            if (vtk_Actor != null)
                World.AddViewProp(this.vtk_Actor);

            if (ListActors != null)
                for (int i = 0; i < ListActors.Count; i++) World.AddViewProp(this.ListActors[i]);
        }


        protected void BackfaceCulling(bool IsDoubleFaced)
        {
            if (IsDoubleFaced)
                vtk_Actor.GetProperty().BackfaceCullingOff();
            else
                vtk_Actor.GetProperty().BackfaceCullingOn();
        }

        /// <summary>
        /// Create a vtkActor object, with a specific interpolation maode
        /// </summary>
        /// <param name="InterpolationMode">0 : flat, 1 : Gouraud, 2 : Phong</param>
        protected void CreateVTK3DObject(int InterpolationMode)
        {


            vtk_Actor.SetMapper(vtk_PolyDataMapper);


            if (InterpolationMode == 0)
            {
                vtk_Actor.GetProperty().SetColor(this.Colour.R / 255.0, this.Colour.G / 255.0, this.Colour.B / 255.0);
                vtk_Actor.GetProperty().SetInterpolationToFlat();
            }
            else if (InterpolationMode == 1)
            {
                vtk_Actor.GetProperty().SetColor(this.Colour.R / 255.0, this.Colour.G / 255.0, this.Colour.B / 255.0);
                vtk_Actor.GetProperty().SetInterpolationToGouraud();
            }
            else if (InterpolationMode == 2)
            {
                vtk_Actor.GetProperty().SetSpecularColor(this.Colour.R / 255.0, this.Colour.G / 255.0, this.Colour.B / 255.0);
                vtk_Actor.GetProperty().SetAmbient(0.8);
                vtk_Actor.GetProperty().SetDiffuse(0.1);
                vtk_Actor.GetProperty().SetInterpolationToPhong();
                vtk_Actor.GetProperty().SetSpecular(0.5);
                vtk_Actor.GetProperty().SetSpecularPower(4.0);
            }
            else if (InterpolationMode == 3)
            {
               // vtk_Actor.GetProperty().SetSpecularColor(this.Colour.R / 255.0, this.Colour.G / 255.0, this.Colour.B / 255.0);
                vtk_Actor.GetProperty().SetAmbient(0.8);
                vtk_Actor.GetProperty().SetDiffuse(0.1);
                vtk_Actor.GetProperty().SetInterpolationToPhong();
                vtk_Actor.GetProperty().SetSpecular(0.5);
                vtk_Actor.GetProperty().SetSpecularPower(4.0);
            }

            //                vtk_Actor.GetProperty().SetInterpolationToGouraud();
            vtk_Actor.SetPosition(Position.X, Position.Y, Position.Z);
            //ListObject
        }

        public void SetOpacity(double Opacity)
        {
            vtk_Actor.GetProperty().SetOpacity(Opacity);
        }

        public void SetColor(Color Colour)
        {
            vtk_Actor.GetProperty().SetColor(Colour.R, Colour.G, Colour.B);
        }

        public void SetToWireFrame()
        {
            vtk_Actor.GetProperty().SetRepresentationToWireframe();
        }

        public void SetToSurface()
        {
            vtk_Actor.GetProperty().SetRepresentationToSurface();
        }

        public void IsPickable(bool Pickable)
        {
            if (Pickable)
                vtk_Actor.PickableOn();
            else
                vtk_Actor.PickableOff();
        }

        #region Text Display associated
        vtkFollower TextActor;
        vtkPolyDataMapper TextMapper;
        vtkVectorText TextVTK;

        public void AddText(String Text, c3DWorld CurrentWorld, double scale)
        {
            if (TextActor == null)
            {
                TextActor = vtkFollower.New();
                TextMapper = vtkPolyDataMapper.New();
                TextVTK = vtkVectorText.New();

                TextVTK.SetText(Text);
                TextMapper.SetInputConnection(TextVTK.GetOutputPort());
                TextActor.SetMapper(TextMapper);

                //TextActor.SetPosition(this.GetActor().GetCenter()[0]-1, this.GetActor().GetCenter()[1]-1, this.GetActor().GetCenter()[2]-2);
                TextActor.SetPosition(Position.X , Position.Y , Position.Z - 1);
                TextActor.SetPickable(0);
                CurrentWorld.ren1.AddActor(TextActor);
                TextActor.SetCamera(CurrentWorld.ren1.GetActiveCamera());
            }
            else
            {
                TextVTK.SetText(Text);
            }
            TextActor.SetScale(scale);
        }

        public void AddText(String Text, c3DWorld CurrentWorld, double scale, Color colour)
        {
            AddText(Text, CurrentWorld, scale);
            TextActor.GetProperty().SetColor(Colour.R / 255.0, Colour.G / 255.0, Colour.B / 255.0);
        }

        public void HideText()
        {
            if (TextActor != null)
                TextActor.VisibilityOff();
        }


        public void ShowText()
        {
            if (TextActor != null)
                TextActor.VisibilityOn();
        }

        #endregion
    }
}