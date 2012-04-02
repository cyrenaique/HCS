using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace HCSAnalyzer.Forms.FormsForOptions
{
    public partial class FormToDisplayDRC : Form
    {

        public List<Chart> LChart = new List<Chart>();
        public List<RichTextBox> LRichTextBox = new List<RichTextBox>();

        public int NumberOfXDRC = 0;
        public int NumberOfYDRC = 0;


        public FormToDisplayDRC()
        {
            InitializeComponent();



        }

        private void fitHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int NewWidth = this.panelForDRC.Width / NumberOfXDRC;

            for (int j = 0; j < NumberOfYDRC; j++)
                for (int i = 0; i < NumberOfXDRC; i++)
                {
                    LChart[i + j * NumberOfXDRC].Width = NewWidth;
                    LChart[i + j * NumberOfXDRC].Location = new Point((LChart[i + j * NumberOfXDRC].Width + 5) * i, (LChart[i + j * NumberOfXDRC].Height + 5) * j);
                }
        }

        private void fitVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int NewHeight = this.panelForDRC.Height / NumberOfYDRC;

            for (int j = 0; j < NumberOfYDRC; j++)
                for (int i = 0; i < NumberOfXDRC; i++)
                {
                    LChart[i + j * NumberOfXDRC].Height = NewHeight;
                    LChart[i + j * NumberOfXDRC].Location = new Point((LChart[i + j * NumberOfXDRC].Width + 5) * i, (LChart[i + j * NumberOfXDRC].Height + 5) * j);
                }
        }
    }
}
