namespace HCSAnalyzer.Forms
{
    partial class FormForSwapClasses
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormForSwapClasses));
            this.comboBoxOriginalClass = new System.Windows.Forms.ComboBox();
            this.buttonSwapClass = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxDestinationClass = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxOriginalClass
            // 
            this.comboBoxOriginalClass.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxOriginalClass.FormattingEnabled = true;
            this.comboBoxOriginalClass.Items.AddRange(new object[] {
            "Unselected (-1)",
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
            this.comboBoxOriginalClass.Location = new System.Drawing.Point(122, 30);
            this.comboBoxOriginalClass.Name = "comboBoxOriginalClass";
            this.comboBoxOriginalClass.Size = new System.Drawing.Size(139, 21);
            this.comboBoxOriginalClass.TabIndex = 2;
            this.comboBoxOriginalClass.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxOriginalClass_DrawItem);
            // 
            // buttonSwapClass
            // 
            this.buttonSwapClass.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSwapClass.Location = new System.Drawing.Point(46, 151);
            this.buttonSwapClass.Name = "buttonSwapClass";
            this.buttonSwapClass.Size = new System.Drawing.Size(193, 29);
            this.buttonSwapClass.TabIndex = 3;
            this.buttonSwapClass.Text = "Swap Classes";
            this.buttonSwapClass.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Original Class";
            // 
            // comboBoxDestinationClass
            // 
            this.comboBoxDestinationClass.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxDestinationClass.FormattingEnabled = true;
            this.comboBoxDestinationClass.Items.AddRange(new object[] {
            "Unselected (-1)",
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
            this.comboBoxDestinationClass.Location = new System.Drawing.Point(122, 95);
            this.comboBoxDestinationClass.Name = "comboBoxDestinationClass";
            this.comboBoxDestinationClass.Size = new System.Drawing.Size(139, 21);
            this.comboBoxDestinationClass.TabIndex = 5;
            this.comboBoxDestinationClass.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxDestinationClass_DrawItem);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Destination Class";
            // 
            // FormForSwapClasses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 204);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxDestinationClass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSwapClass);
            this.Controls.Add(this.comboBoxOriginalClass);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormForSwapClasses";
            this.Text = "Swap Classes";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button buttonSwapClass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox comboBoxOriginalClass;
        public System.Windows.Forms.ComboBox comboBoxDestinationClass;
    }
}