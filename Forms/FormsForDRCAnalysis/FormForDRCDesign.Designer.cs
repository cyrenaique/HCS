namespace HCSAnalyzer.Forms
{
    partial class FormForDRCDesign
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormForDRCDesign));
            this.buttonApply = new System.Windows.Forms.Button();
            this.radioButtonOrientationLine = new System.Windows.Forms.RadioButton();
            this.radioButtonOrientationColumn = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownConcentrationNumber = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownReplication = new System.Windows.Forms.NumericUpDown();
            this.dataGridViewForConcentration = new System.Windows.Forms.DataGridView();
            this.panelForDesignDisplay = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonFill = new System.Windows.Forms.Button();
            this.tabPageConcentrations = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownDilutionFactor = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownConcentrationStartingValue = new System.Windows.Forms.NumericUpDown();
            this.radioButtonConcentrationFromThePlate = new System.Windows.Forms.RadioButton();
            this.radioButtonConcentrationsManual = new System.Windows.Forms.RadioButton();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownConcentrationNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReplication)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForConcentration)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPageConcentrations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDilutionFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownConcentrationStartingValue)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonApply
            // 
            this.buttonApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonApply.Location = new System.Drawing.Point(8, 249);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(200, 33);
            this.buttonApply.TabIndex = 0;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // radioButtonOrientationLine
            // 
            this.radioButtonOrientationLine.AutoSize = true;
            this.radioButtonOrientationLine.Checked = true;
            this.radioButtonOrientationLine.Location = new System.Drawing.Point(38, 28);
            this.radioButtonOrientationLine.Name = "radioButtonOrientationLine";
            this.radioButtonOrientationLine.Size = new System.Drawing.Size(45, 17);
            this.radioButtonOrientationLine.TabIndex = 1;
            this.radioButtonOrientationLine.TabStop = true;
            this.radioButtonOrientationLine.Text = "Line";
            this.radioButtonOrientationLine.UseVisualStyleBackColor = true;
            this.radioButtonOrientationLine.CheckedChanged += new System.EventHandler(this.radioButtonOrientationLine_CheckedChanged);
            // 
            // radioButtonOrientationColumn
            // 
            this.radioButtonOrientationColumn.AutoSize = true;
            this.radioButtonOrientationColumn.Location = new System.Drawing.Point(96, 28);
            this.radioButtonOrientationColumn.Name = "radioButtonOrientationColumn";
            this.radioButtonOrientationColumn.Size = new System.Drawing.Size(60, 17);
            this.radioButtonOrientationColumn.TabIndex = 2;
            this.radioButtonOrientationColumn.Text = "Column";
            this.radioButtonOrientationColumn.UseVisualStyleBackColor = true;
            this.radioButtonOrientationColumn.CheckedChanged += new System.EventHandler(this.radioButtonOrientationColumn_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonOrientationLine);
            this.groupBox1.Controls.Add(this.radioButtonOrientationColumn);
            this.groupBox1.Location = new System.Drawing.Point(8, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 67);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Orientation";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Concentration Number";
            // 
            // numericUpDownConcentrationNumber
            // 
            this.numericUpDownConcentrationNumber.Location = new System.Drawing.Point(131, 100);
            this.numericUpDownConcentrationNumber.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericUpDownConcentrationNumber.Name = "numericUpDownConcentrationNumber";
            this.numericUpDownConcentrationNumber.ReadOnly = true;
            this.numericUpDownConcentrationNumber.Size = new System.Drawing.Size(68, 20);
            this.numericUpDownConcentrationNumber.TabIndex = 5;
            this.numericUpDownConcentrationNumber.ValueChanged += new System.EventHandler(this.numericUpDownConcentrationNumber_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Replicate Number";
            // 
            // numericUpDownReplication
            // 
            this.numericUpDownReplication.Location = new System.Drawing.Point(131, 140);
            this.numericUpDownReplication.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericUpDownReplication.Name = "numericUpDownReplication";
            this.numericUpDownReplication.ReadOnly = true;
            this.numericUpDownReplication.Size = new System.Drawing.Size(68, 20);
            this.numericUpDownReplication.TabIndex = 6;
            // 
            // dataGridViewForConcentration
            // 
            this.dataGridViewForConcentration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewForConcentration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewForConcentration.Location = new System.Drawing.Point(195, 6);
            this.dataGridViewForConcentration.Name = "dataGridViewForConcentration";
            this.dataGridViewForConcentration.Size = new System.Drawing.Size(231, 236);
            this.dataGridViewForConcentration.TabIndex = 7;
            // 
            // panelForDesignDisplay
            // 
            this.panelForDesignDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelForDesignDisplay.BackColor = System.Drawing.Color.White;
            this.panelForDesignDisplay.Location = new System.Drawing.Point(6, 6);
            this.panelForDesignDisplay.Name = "panelForDesignDisplay";
            this.panelForDesignDisplay.Size = new System.Drawing.Size(348, 236);
            this.panelForDesignDisplay.TabIndex = 8;
            this.panelForDesignDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.panelForDesignDisplay_Paint);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPageConcentrations);
            this.tabControl1.Location = new System.Drawing.Point(221, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(440, 274);
            this.tabControl1.TabIndex = 9;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonFill);
            this.tabPage1.Controls.Add(this.panelForDesignDisplay);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(432, 248);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DRC Design";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonFill
            // 
            this.buttonFill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFill.Location = new System.Drawing.Point(359, 6);
            this.buttonFill.Name = "buttonFill";
            this.buttonFill.Size = new System.Drawing.Size(66, 50);
            this.buttonFill.TabIndex = 9;
            this.buttonFill.Text = "Fill";
            this.buttonFill.UseVisualStyleBackColor = true;
            this.buttonFill.Click += new System.EventHandler(this.buttonFill_Click);
            // 
            // tabPageConcentrations
            // 
            this.tabPageConcentrations.Controls.Add(this.label4);
            this.tabPageConcentrations.Controls.Add(this.numericUpDownDilutionFactor);
            this.tabPageConcentrations.Controls.Add(this.label3);
            this.tabPageConcentrations.Controls.Add(this.numericUpDownConcentrationStartingValue);
            this.tabPageConcentrations.Controls.Add(this.radioButtonConcentrationFromThePlate);
            this.tabPageConcentrations.Controls.Add(this.radioButtonConcentrationsManual);
            this.tabPageConcentrations.Controls.Add(this.dataGridViewForConcentration);
            this.tabPageConcentrations.Location = new System.Drawing.Point(4, 22);
            this.tabPageConcentrations.Name = "tabPageConcentrations";
            this.tabPageConcentrations.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConcentrations.Size = new System.Drawing.Size(432, 248);
            this.tabPageConcentrations.TabIndex = 1;
            this.tabPageConcentrations.Text = "Concentrations";
            this.tabPageConcentrations.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Dilution Factor";
            // 
            // numericUpDownDilutionFactor
            // 
            this.numericUpDownDilutionFactor.DecimalPlaces = 3;
            this.numericUpDownDilutionFactor.Location = new System.Drawing.Point(93, 108);
            this.numericUpDownDilutionFactor.Maximum = new decimal(new int[] {
            -402653184,
            -1613725636,
            54210108,
            0});
            this.numericUpDownDilutionFactor.Name = "numericUpDownDilutionFactor";
            this.numericUpDownDilutionFactor.Size = new System.Drawing.Size(83, 20);
            this.numericUpDownDilutionFactor.TabIndex = 12;
            this.numericUpDownDilutionFactor.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownDilutionFactor.ValueChanged += new System.EventHandler(this.numericUpDownDilutionFactor_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Starting Value";
            // 
            // numericUpDownConcentrationStartingValue
            // 
            this.numericUpDownConcentrationStartingValue.DecimalPlaces = 3;
            this.numericUpDownConcentrationStartingValue.Location = new System.Drawing.Point(93, 78);
            this.numericUpDownConcentrationStartingValue.Maximum = new decimal(new int[] {
            -402653184,
            -1613725636,
            54210108,
            0});
            this.numericUpDownConcentrationStartingValue.Name = "numericUpDownConcentrationStartingValue";
            this.numericUpDownConcentrationStartingValue.Size = new System.Drawing.Size(83, 20);
            this.numericUpDownConcentrationStartingValue.TabIndex = 10;
            this.numericUpDownConcentrationStartingValue.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownConcentrationStartingValue.ValueChanged += new System.EventHandler(this.numericUpDownConcentrationStartingValue_ValueChanged);
            // 
            // radioButtonConcentrationFromThePlate
            // 
            this.radioButtonConcentrationFromThePlate.AutoSize = true;
            this.radioButtonConcentrationFromThePlate.Checked = true;
            this.radioButtonConcentrationFromThePlate.Location = new System.Drawing.Point(17, 17);
            this.radioButtonConcentrationFromThePlate.Name = "radioButtonConcentrationFromThePlate";
            this.radioButtonConcentrationFromThePlate.Size = new System.Drawing.Size(92, 17);
            this.radioButtonConcentrationFromThePlate.TabIndex = 8;
            this.radioButtonConcentrationFromThePlate.TabStop = true;
            this.radioButtonConcentrationFromThePlate.Text = "From the plate";
            this.radioButtonConcentrationFromThePlate.UseVisualStyleBackColor = true;
            this.radioButtonConcentrationFromThePlate.CheckedChanged += new System.EventHandler(this.radioButtonConcentrationFromThePlate_CheckedChanged);
            // 
            // radioButtonConcentrationsManual
            // 
            this.radioButtonConcentrationsManual.AutoSize = true;
            this.radioButtonConcentrationsManual.Location = new System.Drawing.Point(17, 40);
            this.radioButtonConcentrationsManual.Name = "radioButtonConcentrationsManual";
            this.radioButtonConcentrationsManual.Size = new System.Drawing.Size(60, 17);
            this.radioButtonConcentrationsManual.TabIndex = 9;
            this.radioButtonConcentrationsManual.Text = "Manual";
            this.radioButtonConcentrationsManual.UseVisualStyleBackColor = true;
            this.radioButtonConcentrationsManual.CheckedChanged += new System.EventHandler(this.radioButtonConcentrationsManual_CheckedChanged);
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonClose.Location = new System.Drawing.Point(8, 210);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(200, 33);
            this.buttonClose.TabIndex = 10;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // FormForDRCDesign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 288);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.numericUpDownReplication);
            this.Controls.Add(this.numericUpDownConcentrationNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonApply);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormForDRCDesign";
            this.Text = "DRC Design";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownConcentrationNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownReplication)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForConcentration)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPageConcentrations.ResumeLayout(false);
            this.tabPageConcentrations.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDilutionFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownConcentrationStartingValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonApply;
        public System.Windows.Forms.RadioButton radioButtonOrientationColumn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.NumericUpDown numericUpDownConcentrationNumber;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown numericUpDownReplication;
        public System.Windows.Forms.DataGridView dataGridViewForConcentration;
        public System.Windows.Forms.RadioButton radioButtonOrientationLine;
        public System.Windows.Forms.Panel panelForDesignDisplay;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPageConcentrations;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.NumericUpDown numericUpDownDilutionFactor;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.NumericUpDown numericUpDownConcentrationStartingValue;
        public System.Windows.Forms.RadioButton radioButtonConcentrationFromThePlate;
        public System.Windows.Forms.RadioButton radioButtonConcentrationsManual;
        private System.Windows.Forms.Button buttonFill;
        private System.Windows.Forms.Button buttonClose;
    }
}