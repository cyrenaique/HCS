using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HCSAnalyzer.Classes;

namespace HCSAnalyzer.Forms
{
    public partial class FormForGenerateScreening : Form
    {

        cGlobalInfo GlobalInfo;

        public FormForGenerateScreening(cGlobalInfo GlobalInfo)
        {
            InitializeComponent();

            this.GlobalInfo = GlobalInfo;

            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;

            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.checkBoxStandardDeviation, "Step: " + GlobalInfo.OptionsWindow.numericUpDownGenerateScreenNoiseStdDev.Value);
            toolTip1.SetToolTip(this.checkBoxShiftRowEffect, "Step: " + GlobalInfo.OptionsWindow.numericUpDownGenerateScreenRowEffectShift.Value);
            toolTip1.SetToolTip(this.checkBoxRatioXY, "Step: " + GlobalInfo.OptionsWindow.numericUpDownGenerateScreenRatioXY.Value);
            toolTip1.SetToolTip(this.checkBoxEdgeEffectIteration, "Step: " + GlobalInfo.OptionsWindow.numericUpDownGenerateScreenDiffusion.Value);
        }

        private void numericUpDownColPosCtrl_ValueChanged(object sender, EventArgs e)
        {
            if ((int)numericUpDownColPosCtrl.Value > (int)numericUpDownColumns.Value-1)
                numericUpDownColPosCtrl.Value = numericUpDownColumns.Value-1;
        }

        private void numericUpDownColNegCtrl_ValueChanged(object sender, EventArgs e)
        {
            if ((int)numericUpDownColNegCtrl.Value > (int)numericUpDownColumns.Value - 1)
                numericUpDownColNegCtrl.Value = numericUpDownColumns.Value - 1;

        }

        private void checkBoxPositiveCtrl_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownColPosCtrl.Enabled = checkBoxPositiveCtrl.Checked;
            numericUpDownPosCtrlMean.Enabled = checkBoxPositiveCtrl.Checked;
            numericUpDownPosCtrlStdv.Enabled = checkBoxPositiveCtrl.Checked;
        }

        private void checkBoxNegativeCtrl_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownColNegCtrl.Enabled = checkBoxNegativeCtrl.Checked;
            numericUpDownNegCtrlMean.Enabled = checkBoxNegativeCtrl.Checked;
            numericUpDownNegCtrlStdv.Enabled = checkBoxNegativeCtrl.Checked;
        }

        private void numericUpDownColumns_ValueChanged(object sender, EventArgs e)
        {
            if ((int)numericUpDownColPosCtrl.Value > (int)numericUpDownColumns.Value - 1)
                numericUpDownColPosCtrl.Value = numericUpDownColumns.Value - 1;
            if ((int)numericUpDownColNegCtrl.Value > (int)numericUpDownColumns.Value - 1)
                numericUpDownColNegCtrl.Value = numericUpDownColumns.Value - 1;

            numericUpDownBowlEffectRatioXY.Value = numericUpDownColumns.Value / numericUpDownRows.Value;
        }


        private void numericUpDownRowEffectIntensity_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownRowEffectIntensity.Enabled = checkBoxRowEffect.Checked;
        }

        private void numericUpDownRows_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownBowlEffectRatioXY.Value = numericUpDownColumns.Value / numericUpDownRows.Value;
        }

        private void checkBoxRowEffect_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownRowEffectIntensity.Enabled = checkBoxRowEffect.Checked;
        }

        private void checkBoxColumnEffect_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownColEffectIntensity.Enabled = checkBoxColumnEffect.Checked;
        }

        private void checkBoxEdgeEffect_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownEdgeEffectIntensity.Enabled = numericUpDownEdgeEffectIteration.Enabled = checkBoxEdgeEffect.Checked;
        }

        private void checkBoxBowlEffect_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownBowlEffectIntensity.Enabled = numericUpDownBowlEffectRatioXY.Enabled = checkBoxBowlEffect.Checked;
        }

    }
}
