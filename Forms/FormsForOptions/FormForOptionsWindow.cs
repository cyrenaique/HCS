using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibPlateAnalysis;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace HCSAnalyzer
{
    [Serializable]
    public partial class FormForOptionsWindow : Form
    {
        public cScreening CurrentScreen;

        public FormForOptionsWindow(cScreening CurrentScreen)
        {
            this.CurrentScreen = CurrentScreen;

            InitializeComponent();
            buttonOk.Focus();
            buttonOk.Select();
            this.comboBoxHierarchicalDistance.SelectedIndex = 0;
            this.comboBoxHierarchicalLinkType.SelectedIndex = 0;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            if((CurrentScreen!=null)&&(CurrentScreen.ListPlatesActive!=null))
                CurrentScreen.GetCurrentDisplayPlate().DisplayDistribution(CurrentScreen.ListDescriptors.CurrentSelectedDescriptor, false);
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            if (this.colorDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            else
                panel1.BackColor = this.colorDialog.Color;
        }

        //public override Type BindToType(string assemblyName, string typeName)
        //{
        //    Type typeToDeserialize = null;

        //    String currentAssembly = Assembly.GetExecutingAssembly().FullName;

        //    // In this case we are always using the current assembly
        //    assemblyName = currentAssembly;

        //    // Get the type using the typeName and assemblyName
        //    typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));

        //    return typeToDeserialize;
        //}



        public void Save(string Path)
        {
            Stream stream = null;
            try
            {
                IFormatter formatter = new BinaryFormatter();
                stream = new FileStream(Path, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, this);
            }
            catch
            {
                // do nothing, just ignore any possible errors
            }
            finally
            {
                if (null != stream)
                    stream.Close();
            }
        }

        //public FormForOptionsWindow Load(string fileName)
        //{
        //    Stream stream = null;
        //    FormForOptionsWindow BrainDataBase = null;
        //    try
        //    {
        //        IFormatter formatter = new BinaryFormatter();
        //        stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        //       // formatter.Binder = this;
        //        BrainDataBase = (FormForOptionsWindow)formatter.Deserialize(stream);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        // do nothing, just ignore any possible errors
        //    }
        //    finally
        //    {
        //        if (null != stream)
        //            stream.Close();
        //    }
        //    return BrainDataBase;
        //}

        private void buttonSaveOptions_Click(object sender, EventArgs e)
        {
            this.Save(@"save00");
        }

        private void FormForOptionsWindow_Load(object sender, EventArgs e)
        {

        }

        private void buttonDRCPlateDesign_Click(object sender, EventArgs e)
        {
            if (CurrentScreen == null) return;
            CurrentScreen.GlobalInfo.WindowForDRCDesign.ShowDialog();
        }


    }
}
