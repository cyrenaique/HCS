using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibPlateAnalysis;
using HCSAnalyzer.Classes;

namespace HCSAnalyzer.Forms
{
    public partial class FormForSwapClasses : Form
    {

        cGlobalInfo GlobalInfo;

        public FormForSwapClasses(cGlobalInfo GlobalInfo)
        {
            InitializeComponent();
            comboBoxDestinationClass.SelectedIndex = 0;
            comboBoxOriginalClass.SelectedIndex = 0;
            this.GlobalInfo = GlobalInfo;
        }

        private void comboBoxDestinationClass_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index > 0)
            {
                SolidBrush BrushForColor = new SolidBrush(GlobalInfo.GetColor(e.Index - 1));
                e.Graphics.FillRectangle(BrushForColor, e.Bounds.X + 1, e.Bounds.Y + 1, 10, 10);
            }
            e.Graphics.DrawString(comboBoxOriginalClass.Items[e.Index].ToString(), comboBoxOriginalClass.Font,
                System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X + 15, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            e.DrawFocusRectangle();
        }

        private void comboBoxOriginalClass_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index > 0)
            {
                SolidBrush BrushForColor = new SolidBrush(GlobalInfo.GetColor(e.Index - 1));
                e.Graphics.FillRectangle(BrushForColor, e.Bounds.X + 1, e.Bounds.Y + 1, 10, 10);
            }
            e.Graphics.DrawString(comboBoxOriginalClass.Items[e.Index].ToString(), comboBoxOriginalClass.Font,
                System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X + 15, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            e.DrawFocusRectangle();
        }
    }
}
