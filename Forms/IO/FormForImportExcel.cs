using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibPlateAnalysis;

namespace HCSAnalyzer
{
    public partial class FormForImportExcel : Form
    {

        private bool FirstTime = true;
        public bool IsImportCSV =  false;
        public bool IsAppend;

        public cScreening CurrentScreen = null;
        public HCSAnalyzer thisHCSAnalyzer = null;

        public FormForImportExcel()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            int NumPlateName = 0;
            int NumRow = 0;
            int NumCol = 0;
            int NumWellPos = 0;
            int NumLocusID = 0;
            int NumConcentration = 0;
            int NumName = 0;
            int NumInfo = 0;
            int NumClass = 0;

            int numDescritpor = 0;

            for (int i = 0; i < this.dataGridViewForImport.Rows.Count; i++)
            {
                string CurrentVal = this.dataGridViewForImport.Rows[i].Cells[2].Value.ToString();
                if ((CurrentVal == "Plate name")&&((bool)dataGridViewForImport.Rows[i].Cells[1].Value))
                    NumPlateName++;
                if ((CurrentVal == "Row")&&((bool)dataGridViewForImport.Rows[i].Cells[1].Value))
                    NumRow++;
                if ((CurrentVal == "Column")&&((bool)dataGridViewForImport.Rows[i].Cells[1].Value))
                    NumCol++;
                if ((CurrentVal == "Well position")&&((bool)dataGridViewForImport.Rows[i].Cells[1].Value))
                    NumWellPos++;
                if ((CurrentVal == "Locus ID")&&((bool)dataGridViewForImport.Rows[i].Cells[1].Value))
                    NumLocusID++;
                if ((CurrentVal == "Concentration") && ((bool)dataGridViewForImport.Rows[i].Cells[1].Value))
                    NumConcentration++;
                if ((CurrentVal == "Name")&&((bool)dataGridViewForImport.Rows[i].Cells[1].Value))
                    NumName++;
                if ((CurrentVal == "Info")&&((bool)dataGridViewForImport.Rows[i].Cells[1].Value))
                    NumInfo++;
                if ((CurrentVal == "Class")&&((bool)dataGridViewForImport.Rows[i].Cells[1].Value))
                    NumClass++;
                if ((CurrentVal == "Descriptor") && ((bool)dataGridViewForImport.Rows[i].Cells[1].Value))
                    numDescritpor++;
            }

            if (NumPlateName != 1)
            {
                MessageBox.Show("One and only one \"Plate Name\" has to be selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((NumRow != 1)&&(CurrentScreen.GlobalInfo.OptionsWindow.radioButtonWellPosModeDouble.Checked==true))
            {
                MessageBox.Show("One and only one \"Row\" has to be selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((NumCol != 1) && (CurrentScreen.GlobalInfo.OptionsWindow.radioButtonWellPosModeDouble.Checked == true))
            {
                MessageBox.Show("One and only one \"Column\" has to be selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((NumWellPos != 1) && (CurrentScreen.GlobalInfo.OptionsWindow.radioButtonWellPosModeSingle.Checked == true))
            {
                MessageBox.Show("One and only one \"Well position\" has to be selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (NumName > 1)
            {
                MessageBox.Show("You cannot select more than one \"Name\" !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (NumClass > 1)
            {
                MessageBox.Show("You cannot select more than one \"Class\" !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (NumInfo > 1)
            {
                MessageBox.Show("You cannot select more than one \"Info\" !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (NumLocusID > 1)
            {
                MessageBox.Show("You cannot select more than one \"Locus ID\" !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } 
            if (NumConcentration > 1)
            {
                MessageBox.Show("You cannot select more than one \"Concentration\" !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((numDescritpor < 1)&&(IsImportCSV))
            {
                MessageBox.Show("You need to select at least one \"Descriptor\" !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsImportCSV)
                thisHCSAnalyzer.LoadingProcedureForCSVImport(IsAppend);
            else
                thisHCSAnalyzer.LoadingProcedure();
            
            this.Dispose();
        }



       
    }
}
