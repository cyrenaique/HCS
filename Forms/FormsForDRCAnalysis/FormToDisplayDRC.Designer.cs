namespace HCSAnalyzer.Forms.FormsForOptions
{
    partial class FormToDisplayDRC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormToDisplayDRC));
            this.panelForDRC = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fitHorizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fitVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelForDRC
            // 
            this.panelForDRC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelForDRC.AutoScroll = true;
            this.panelForDRC.Location = new System.Drawing.Point(8, 27);
            this.panelForDRC.Name = "panelForDRC";
            this.panelForDRC.Size = new System.Drawing.Size(710, 540);
            this.panelForDRC.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(725, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fitHorizontalToolStripMenuItem,
            this.fitVerticalToolStripMenuItem});
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.displayToolStripMenuItem.Text = "Display";
            // 
            // fitHorizontalToolStripMenuItem
            // 
            this.fitHorizontalToolStripMenuItem.Name = "fitHorizontalToolStripMenuItem";
            this.fitHorizontalToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.fitHorizontalToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.fitHorizontalToolStripMenuItem.Text = "Fit Horizontal";
            this.fitHorizontalToolStripMenuItem.Click += new System.EventHandler(this.fitHorizontalToolStripMenuItem_Click);
            // 
            // fitVerticalToolStripMenuItem
            // 
            this.fitVerticalToolStripMenuItem.Name = "fitVerticalToolStripMenuItem";
            this.fitVerticalToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.fitVerticalToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.fitVerticalToolStripMenuItem.Text = "Fit Vertical";
            this.fitVerticalToolStripMenuItem.Click += new System.EventHandler(this.fitVerticalToolStripMenuItem_Click);
            // 
            // FormToDisplayDRC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 577);
            this.Controls.Add(this.panelForDRC);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormToDisplayDRC";
            this.Text = "DRCs";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Panel panelForDRC;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fitHorizontalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fitVerticalToolStripMenuItem;

    }
}