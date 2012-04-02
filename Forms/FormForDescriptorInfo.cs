using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibPlateAnalysis
{
    public partial class FormForDescriptorInfo : Form
    {

        public cDescriptorsType CurrentDesc = null;

        public FormForDescriptorInfo(cDescriptorsType CurrentDesc)
        {
            this.CurrentDesc = CurrentDesc;
            InitializeComponent(); 
            this.textBoxNameDescriptor.Text = CurrentDesc.GetName();
            this.labelDataType.Text = CurrentDesc.GetDataType();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            if(CurrentDesc.GetName()!=this.textBoxNameDescriptor.Text)
                this.CurrentDesc.ChangeName(this.textBoxNameDescriptor.Text);
        }


    }
}
