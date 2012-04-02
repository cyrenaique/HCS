namespace HCSAnalyzer.Forms
{
    partial class FormForPathway
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
            this.listBoxPathways = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxPathways
            // 
            this.listBoxPathways.FormattingEnabled = true;
            this.listBoxPathways.Location = new System.Drawing.Point(20, 19);
            this.listBoxPathways.Name = "listBoxPathways";
            this.listBoxPathways.Size = new System.Drawing.Size(145, 225);
            this.listBoxPathways.TabIndex = 0;
            // 
            // FormForPathway
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 262);
            this.Controls.Add(this.listBoxPathways);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormForPathway";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox listBoxPathways;
    }
}