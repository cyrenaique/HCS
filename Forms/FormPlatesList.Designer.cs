namespace HCSAnalyzer
{
    partial class PlatesListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlatesListForm));
            this.listBoxAvaliableListPlates = new System.Windows.Forms.ListBox();
            this.listBoxPlateNameToProcess = new System.Windows.Forms.ListBox();
            this.buttonSelectionAdd = new System.Windows.Forms.Button();
            this.buttonSelectionRemove = new System.Windows.Forms.Button();
            this.buttonSelectionClear = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxAvaliableListPlates
            // 
            this.listBoxAvaliableListPlates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxAvaliableListPlates.BackColor = System.Drawing.SystemColors.ControlLight;
            this.listBoxAvaliableListPlates.FormattingEnabled = true;
            this.listBoxAvaliableListPlates.Location = new System.Drawing.Point(12, 51);
            this.listBoxAvaliableListPlates.Name = "listBoxAvaliableListPlates";
            this.listBoxAvaliableListPlates.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxAvaliableListPlates.Size = new System.Drawing.Size(432, 446);
            this.listBoxAvaliableListPlates.Sorted = true;
            this.listBoxAvaliableListPlates.TabIndex = 7;
            // 
            // listBoxPlateNameToProcess
            // 
            this.listBoxPlateNameToProcess.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxPlateNameToProcess.Location = new System.Drawing.Point(595, 51);
            this.listBoxPlateNameToProcess.Name = "listBoxPlateNameToProcess";
            this.listBoxPlateNameToProcess.Size = new System.Drawing.Size(432, 446);
            this.listBoxPlateNameToProcess.Sorted = true;
            this.listBoxPlateNameToProcess.TabIndex = 9;
            // 
            // buttonSelectionAdd
            // 
            this.buttonSelectionAdd.Location = new System.Drawing.Point(455, 72);
            this.buttonSelectionAdd.Name = "buttonSelectionAdd";
            this.buttonSelectionAdd.Size = new System.Drawing.Size(130, 30);
            this.buttonSelectionAdd.TabIndex = 10;
            this.buttonSelectionAdd.Text = "Add ->";
            this.buttonSelectionAdd.UseVisualStyleBackColor = true;
            this.buttonSelectionAdd.Click += new System.EventHandler(this.buttonSelectionAdd_Click);
            // 
            // buttonSelectionRemove
            // 
            this.buttonSelectionRemove.Location = new System.Drawing.Point(455, 120);
            this.buttonSelectionRemove.Name = "buttonSelectionRemove";
            this.buttonSelectionRemove.Size = new System.Drawing.Size(130, 30);
            this.buttonSelectionRemove.TabIndex = 11;
            this.buttonSelectionRemove.Text = "<- Remove";
            this.buttonSelectionRemove.UseVisualStyleBackColor = true;
            this.buttonSelectionRemove.Click += new System.EventHandler(this.buttonSelectionRemove_Click);
            // 
            // buttonSelectionClear
            // 
            this.buttonSelectionClear.Location = new System.Drawing.Point(455, 219);
            this.buttonSelectionClear.Name = "buttonSelectionClear";
            this.buttonSelectionClear.Size = new System.Drawing.Size(130, 30);
            this.buttonSelectionClear.TabIndex = 12;
            this.buttonSelectionClear.Text = "Clear ->";
            this.buttonSelectionClear.UseVisualStyleBackColor = true;
            this.buttonSelectionClear.Click += new System.EventHandler(this.buttonSelectionClear_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(455, 465);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(130, 32);
            this.buttonOk.TabIndex = 13;
            this.buttonOk.Text = "Done";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(157, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Available Plates";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(758, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "To Be Processed";
            // 
            // PlatesListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 514);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonSelectionClear);
            this.Controls.Add(this.buttonSelectionRemove);
            this.Controls.Add(this.buttonSelectionAdd);
            this.Controls.Add(this.listBoxPlateNameToProcess);
            this.Controls.Add(this.listBoxAvaliableListPlates);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PlatesListForm";
            this.Text = "Plates manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox listBoxAvaliableListPlates;
        public System.Windows.Forms.ListBox listBoxPlateNameToProcess;
        private System.Windows.Forms.Button buttonSelectionAdd;
        private System.Windows.Forms.Button buttonSelectionRemove;
        private System.Windows.Forms.Button buttonSelectionClear;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}