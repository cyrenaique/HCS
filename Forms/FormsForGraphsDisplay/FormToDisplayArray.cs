using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HCSAnalyzer.Controls
{
    public partial class FormToDisplayArray : Form
    {
        public FormToDisplayArray()
        {
            InitializeComponent();
            DisplayMatrix();
           // this.panelForArray.GetToolTipText += new System.EventHandler<ToolTipEventArgs>(this.AssociatedChart_GetToolTipText);
        }

        Boolean bHaveMouse;
        Point ptOriginal = new Point();
        Point ptLast = new Point();

        // Called when the left mouse button is pressed. 
        public void MyMouseDown(Object sender, MouseEventArgs e)
        {
            // Make a note that we "have the mouse".
            bHaveMouse = true;
            // Store the "starting point" for this rubber-band rectangle.
            ptOriginal.X = e.X;
            ptOriginal.Y = e.Y;
            // Special value lets us know that no previous
            // rectangle needs to be erased.
            ptLast.X = -1;
            ptLast.Y = -1;
        }
        // Convert and normalize the points and draw the reversible frame.
        private void MyDrawReversibleRectangle(Point p1, Point p2)
        {
            Rectangle rc = new Rectangle();

            // Convert the points to screen coordinates.
            p1 = PointToScreen(p1);
            p2 = PointToScreen(p2);
            // Normalize the rectangle.
            if (p1.X < p2.X)
            {
                rc.X = p1.X;
                rc.Width = p2.X - p1.X;
            }
            else
            {
                rc.X = p2.X;
                rc.Width = p1.X - p2.X;
            }
            if (p1.Y < p2.Y)
            {
                rc.Y = p1.Y;
                rc.Height = p2.Y - p1.Y;
            }
            else
            {
                rc.Y = p2.Y;
                rc.Height = p1.Y - p2.Y;
            }
            // Draw the reversible frame.
            ControlPaint.DrawReversibleFrame(rc,
                            Color.Red, FrameStyle.Dashed);
        }
        // Called when the left mouse button is released.
        public void MyMouseUp(Object sender, MouseEventArgs e)
        {
            // Set internal flag to know we no longer "have the mouse".
            bHaveMouse = false;
            // If we have drawn previously, draw again in that spot
            // to remove the lines.
            if (ptLast.X != -1)
            {
                Point ptCurrent = new Point(e.X, e.Y);
                MyDrawReversibleRectangle(ptOriginal, ptLast);
            }
            // Set flags to know that there is no "previous" line to reverse.
            ptLast.X = -1;
            ptLast.Y = -1;
            ptOriginal.X = -1;
            ptOriginal.Y = -1;
        }
        // Called when the mouse is moved.
        public void MyMouseMove(Object sender, MouseEventArgs e)
        {
            Point ptCurrent = new Point(e.X, e.Y);
            // If we "have the mouse", then we draw our lines.
            if (bHaveMouse)
            {
                // If we have drawn previously, draw again in
                // that spot to remove the lines.
                if (ptLast.X != -1)
                {
                    MyDrawReversibleRectangle(ptOriginal, ptLast);
                }
                // Update last point.
                ptLast = ptCurrent;
                // Draw new lines.
                MyDrawReversibleRectangle(ptOriginal, ptCurrent);
            }
        }
        // Set up delegates for mouse events.
        protected override void OnLoad(System.EventArgs e)
        {
            MouseDown += new MouseEventHandler(MyMouseDown);
            MouseUp += new MouseEventHandler(MyMouseUp);
            MouseMove += new MouseEventHandler(MyMouseMove);
            DisplayMatrix();
            System.Windows.Forms.ToolTip ToolTip = new System.Windows.Forms.ToolTip();
            ToolTip.SetToolTip(this, "Hello");

            bHaveMouse = false;
        }

        // should be something like : DisplayMatrix(cPlate PateToDisplay)
        //private void DisplayMatrix()

        private void DisplayMatrix()
        {
            int PosXMatrix = 5;
            int PosYMatrix = 5;
            Color BorderColor = Color.BlueViolet;
            Color CenterColor = Color.Blue;

            
            int NumCol = 24;
            int NumRow = 16;

            int Cell_Width = 600 / NumCol;
            int Cell_Height = 300 / NumRow;

            int GutterSize = Cell_Width / 4;
            float WidthBorder = GutterSize;
            double[,] MatrixToDisplay = new double[NumCol, NumRow];
            System.Drawing.Graphics formGraphics = this.CreateGraphics();

            for (int j = 0; j < NumRow; j++)
                for (int i = 0; i < NumCol; i++)
                {
                    int PosX = PosXMatrix + i * (GutterSize + Cell_Width);
                    int PosY = PosYMatrix + j * (GutterSize + Cell_Height);

                    SolidBrush Brush = new SolidBrush(CenterColor);
                    System.Drawing.Pen myPen = new System.Drawing.Pen(BorderColor, WidthBorder);
                    Rectangle Rect = new Rectangle(PosX, PosY, Cell_Width, Cell_Height);
                    formGraphics.DrawRectangle(myPen, Rect);
                    formGraphics.FillRectangle(Brush, Rect);
                }
        }


        private void FormToDisplayArray_Paint(object sender, PaintEventArgs e)
        {
            DisplayMatrix();
        }

        private void FormToDisplayArray_ClientSizeChanged(object sender, EventArgs e)
        {
            DisplayMatrix();
        }

        private void FormToDisplayArray_Load(object sender, EventArgs e)
        {
            DisplayMatrix();
            //System.Windows.Forms.ToolTip ToolTip = new System.Windows.Forms.ToolTip();
            //ToolTip.SetToolTip(this, "Hello");
        }


    }
}
