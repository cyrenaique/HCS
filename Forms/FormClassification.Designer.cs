namespace HCSAnalyzer
{
    partial class FormClassification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClassification));
            this.buttonClassification = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxForNeutralClass = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // buttonClassification
            // 
            this.buttonClassification.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonClassification.Location = new System.Drawing.Point(21, 100);
            this.buttonClassification.Name = "buttonClassification";
            this.buttonClassification.Size = new System.Drawing.Size(193, 29);
            this.buttonClassification.TabIndex = 0;
            this.buttonClassification.Text = "Classify";
            this.buttonClassification.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Neutral Class";
            // 
            // comboBoxForNeutralClass
            // 
            this.comboBoxForNeutralClass.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxForNeutralClass.FormattingEnabled = true;
            this.comboBoxForNeutralClass.Items.AddRange(new object[] {
            "Positive (0)",
            "Negative (1)",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.comboBoxForNeutralClass.Location = new System.Drawing.Point(103, 38);
            this.comboBoxForNeutralClass.Name = "comboBoxForNeutralClass";
            this.comboBoxForNeutralClass.Size = new System.Drawing.Size(121, 21);
            this.comboBoxForNeutralClass.TabIndex = 27;
            this.comboBoxForNeutralClass.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxForNeutralClass_DrawItem);
            // 
            // FormClassification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 149);
            this.Controls.Add(this.comboBoxForNeutralClass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonClassification);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormClassification";
            this.Text = "Classification";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button buttonClassification;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox comboBoxForNeutralClass;
    }
}