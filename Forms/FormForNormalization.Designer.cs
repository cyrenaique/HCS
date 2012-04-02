namespace HCSAnalyzer
{
    partial class FormForNormalization
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormForNormalization));
            this.radioButtonNormalizationDivideNegCtrl = new System.Windows.Forms.RadioButton();
            this.radioButtonNormalizationNegPosCtrl = new System.Windows.Forms.RadioButton();
            this.buttonStartNormalize = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radioButtonNormalizationDivideNegCtrl
            // 
            this.radioButtonNormalizationDivideNegCtrl.AutoSize = true;
            this.radioButtonNormalizationDivideNegCtrl.Checked = true;
            this.radioButtonNormalizationDivideNegCtrl.Location = new System.Drawing.Point(56, 29);
            this.radioButtonNormalizationDivideNegCtrl.Name = "radioButtonNormalizationDivideNegCtrl";
            this.radioButtonNormalizationDivideNegCtrl.Size = new System.Drawing.Size(110, 17);
            this.radioButtonNormalizationDivideNegCtrl.TabIndex = 0;
            this.radioButtonNormalizationDivideNegCtrl.TabStop = true;
            this.radioButtonNormalizationDivideNegCtrl.Text = "Negative Ctrl Only";
            this.radioButtonNormalizationDivideNegCtrl.UseVisualStyleBackColor = true;
            // 
            // radioButtonNormalizationNegPosCtrl
            // 
            this.radioButtonNormalizationNegPosCtrl.AutoSize = true;
            this.radioButtonNormalizationNegPosCtrl.Location = new System.Drawing.Point(47, 76);
            this.radioButtonNormalizationNegPosCtrl.Name = "radioButtonNormalizationNegPosCtrl";
            this.radioButtonNormalizationNegPosCtrl.Size = new System.Drawing.Size(151, 17);
            this.radioButtonNormalizationNegPosCtrl.TabIndex = 1;
            this.radioButtonNormalizationNegPosCtrl.Text = "Negative and positive Ctrls";
            this.radioButtonNormalizationNegPosCtrl.UseVisualStyleBackColor = true;
            // 
            // buttonStartNormalize
            // 
            this.buttonStartNormalize.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonStartNormalize.Location = new System.Drawing.Point(12, 130);
            this.buttonStartNormalize.Name = "buttonStartNormalize";
            this.buttonStartNormalize.Size = new System.Drawing.Size(199, 28);
            this.buttonStartNormalize.TabIndex = 2;
            this.buttonStartNormalize.Text = "Normalize";
            this.buttonStartNormalize.UseVisualStyleBackColor = true;
            // 
            // FormForNormalization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 170);
            this.Controls.Add(this.buttonStartNormalize);
            this.Controls.Add(this.radioButtonNormalizationNegPosCtrl);
            this.Controls.Add(this.radioButtonNormalizationDivideNegCtrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormForNormalization";
            this.Text = "Normalization";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RadioButton radioButtonNormalizationDivideNegCtrl;
        public System.Windows.Forms.RadioButton radioButtonNormalizationNegPosCtrl;
        private System.Windows.Forms.Button buttonStartNormalize;
    }
}