namespace HCSAnalyzer.Forms
{
    partial class FormDendogram
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDendogram));
            this.panelForDendogram = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelForDendogram
            // 
            this.panelForDendogram.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelForDendogram.AutoScroll = true;
            this.panelForDendogram.Location = new System.Drawing.Point(9, 12);
            this.panelForDendogram.Name = "panelForDendogram";
            this.panelForDendogram.Size = new System.Drawing.Size(635, 443);
            this.panelForDendogram.TabIndex = 0;
            this.panelForDendogram.Paint += new System.Windows.Forms.PaintEventHandler(this.panelForDendogram_Paint);
            this.panelForDendogram.Resize += new System.EventHandler(this.panelForDendogram_Resize);
            // 
            // FormDendogram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 464);
            this.Controls.Add(this.panelForDendogram);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDendogram";
            this.Text = "Dendogram";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panelForDendogram;
    }
}