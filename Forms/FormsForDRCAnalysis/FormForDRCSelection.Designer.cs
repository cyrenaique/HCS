namespace HCSAnalyzer.Forms.FormsForDRCAnalysis
{
    partial class FormForDRCSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormForDRCSelection));
            this.buttonProcess = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownWindowMinValue = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRelativeErrorMaxValue = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxMOAClassification = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWindowMinValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRelativeErrorMaxValue)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonProcess
            // 
            this.buttonProcess.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonProcess.Location = new System.Drawing.Point(69, 221);
            this.buttonProcess.Name = "buttonProcess";
            this.buttonProcess.Size = new System.Drawing.Size(147, 29);
            this.buttonProcess.TabIndex = 0;
            this.buttonProcess.Text = "Process";
            this.buttonProcess.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Window minimum value";
            // 
            // numericUpDownWindowMinValue
            // 
            this.numericUpDownWindowMinValue.DecimalPlaces = 2;
            this.numericUpDownWindowMinValue.Location = new System.Drawing.Point(168, 33);
            this.numericUpDownWindowMinValue.Name = "numericUpDownWindowMinValue";
            this.numericUpDownWindowMinValue.Size = new System.Drawing.Size(90, 20);
            this.numericUpDownWindowMinValue.TabIndex = 2;
            this.numericUpDownWindowMinValue.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // numericUpDownRelativeErrorMaxValue
            // 
            this.numericUpDownRelativeErrorMaxValue.DecimalPlaces = 2;
            this.numericUpDownRelativeErrorMaxValue.Location = new System.Drawing.Point(168, 71);
            this.numericUpDownRelativeErrorMaxValue.Name = "numericUpDownRelativeErrorMaxValue";
            this.numericUpDownRelativeErrorMaxValue.Size = new System.Drawing.Size(90, 20);
            this.numericUpDownRelativeErrorMaxValue.TabIndex = 4;
            this.numericUpDownRelativeErrorMaxValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Relative error maximum value";
            // 
            // checkBoxMOAClassification
            // 
            this.checkBoxMOAClassification.AutoSize = true;
            this.checkBoxMOAClassification.Location = new System.Drawing.Point(60, 170);
            this.checkBoxMOAClassification.Name = "checkBoxMOAClassification";
            this.checkBoxMOAClassification.Size = new System.Drawing.Size(164, 17);
            this.checkBoxMOAClassification.TabIndex = 5;
            this.checkBoxMOAClassification.Text = "Mode Of Action Classification";
            this.checkBoxMOAClassification.UseVisualStyleBackColor = true;
            // 
            // FormForDRCSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.checkBoxMOAClassification);
            this.Controls.Add(this.numericUpDownRelativeErrorMaxValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownWindowMinValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonProcess);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormForDRCSelection";
            this.Text = "DRC Selection";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWindowMinValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRelativeErrorMaxValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonProcess;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.NumericUpDown numericUpDownWindowMinValue;
        public System.Windows.Forms.NumericUpDown numericUpDownRelativeErrorMaxValue;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.CheckBox checkBoxMOAClassification;
    }
}