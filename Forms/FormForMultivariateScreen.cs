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
    public partial class FormForMultivariateScreen : Form
    {
        public FormForMultivariateScreen(cGlobalInfo GlobalInfo)
        {
            InitializeComponent();
            this.GlobalInfo = GlobalInfo;
            UpDateDisplay();
        }

        private cGlobalInfo GlobalInfo;

        private void numericUpDownDimensionNumber_ValueChanged(object sender, EventArgs e)
        {
            UpDateDisplay();
        }

        public void UpDateDisplay()
        {
            dataGridViewForCompounds.Columns.Clear();

            dataGridViewForCompounds.Columns.Add("Name", "Name");

            DataGridViewComboBoxColumn columnPosition = new DataGridViewComboBoxColumn();

            string[] ListName = new string[(int)this.numericUpDownColumns.Value + 1];
            for (int i = 0; i < ListName.Length - 1; i++)
                ListName[i] = i.ToString();
            ListName[ListName.Length-1] = "Entire plate";

            columnPosition.DataSource = ListName;
            columnPosition.Name = "Column";
            dataGridViewForCompounds.Columns.Add(columnPosition);


            //dataGridViewForCompounds.Columns.Add("Position", "Position");


            DataGridViewCheckBoxColumn columnSelection = new DataGridViewCheckBoxColumn();
            columnSelection.Name = "Selection";
            dataGridViewForCompounds.Columns.Add(columnSelection);



            for (int i = 0; i < (int)numericUpDownDimensionNumber.Value; i++)
            {
                dataGridViewForCompounds.Columns.Add("Mean" + i, "Mean" + i);
                dataGridViewForCompounds.Columns.Add("Stdv" + i, "Stdv" + i);


            }

            for (int i = 0; i < GlobalInfo.GetNumberofDefinedClass(); i++)
            {
                int IdxPosCol = 0;
                dataGridViewForCompounds.Rows.Add();
                dataGridViewForCompounds.Rows[i].Cells[IdxPosCol].Value = "Phenotype " + i;
                dataGridViewForCompounds.Rows[i].Cells[IdxPosCol++].Style.BackColor = GlobalInfo.GetColor(i);


                int Position = i;
                if (i >= ListName.Length) Position = ListName.Length - 1;
                dataGridViewForCompounds.Rows[i].Cells[IdxPosCol].Value = ListName[Position];// Position.ToString();

                if(i==2)
                    dataGridViewForCompounds.Rows[i].Cells[IdxPosCol].Value = ListName[ListName.Length-1];// Position.ToString();

                IdxPosCol++;

                if(i<3)
                    dataGridViewForCompounds.Rows[i].Cells[IdxPosCol].Value = true;
                else
                    dataGridViewForCompounds.Rows[i].Cells[IdxPosCol].Value = false;

                IdxPosCol++;

                for (int j = IdxPosCol - 1; j < (int)numericUpDownDimensionNumber.Value * 2 + IdxPosCol - 1; j++)
                {
                    double Value = 0;
                    if ((j % 2) == 0)
                    {
                        Value = i * 50;
                        dataGridViewForCompounds.Rows[i].Cells[j+1].Style.Font = new Font(dataGridViewForCompounds.Font, FontStyle.Bold);
                    }
                    else
                    {
                        Value = 20;
                    }
                    dataGridViewForCompounds.Rows[i].Cells[j+1].Value = Value.ToString();

                    if(((j%4)==0)||((j%4)==1))
                        dataGridViewForCompounds.Rows[i].Cells[j+1].Style.BackColor = Color.Beige;
                }
            }
        }

        private void numericUpDownColumns_ValueChanged(object sender, EventArgs e)
        {
            UpDateDisplay();
        }

    }
}
