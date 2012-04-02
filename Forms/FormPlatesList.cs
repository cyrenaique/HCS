using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HCSAnalyzer
{
    public partial class PlatesListForm : Form
    {

       // public HCSAnalyzer ParentPlugin;

        public PlatesListForm()
        {
            InitializeComponent();
        }

        private void buttonSelectionAdd_Click(object sender, EventArgs e)
        {
            if (listBoxAvaliableListPlates.SelectedItem == null) return;
            for(int i=0;i<listBoxAvaliableListPlates.SelectedItems.Count;i++)
                listBoxPlateNameToProcess.Items.Add(listBoxAvaliableListPlates.SelectedItems[i]);
            
            //ParentPlugin.RefreshInfoScreeningRichBox();
        }

        private void buttonSelectionRemove_Click(object sender, EventArgs e)
        {
            if (listBoxPlateNameToProcess.SelectedItem == null) return;
            listBoxPlateNameToProcess.Items.Remove(listBoxPlateNameToProcess.SelectedItem);
      
            //ParentPlugin.RefreshInfoScreeningRichBox();
        }

        private void buttonSelectionClear_Click(object sender, EventArgs e)
        {
            listBoxPlateNameToProcess.Items.Clear();
            //ParentPlugin.RefreshInfoScreeningRichBox();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {

           // this.ShowDialog();
        }


 

    }
}
