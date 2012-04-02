namespace HCSAnalyzer.Forms
{
    partial class FormForClassificationTree
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormForClassificationTree));
            this.richTextBoxConsoleForClassification = new System.Windows.Forms.RichTextBox();
            this.gViewerForTreeClassif = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxConsoleForClassification
            // 
            this.richTextBoxConsoleForClassification.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxConsoleForClassification.Location = new System.Drawing.Point(-1, 3);
            this.richTextBoxConsoleForClassification.Name = "richTextBoxConsoleForClassification";
            this.richTextBoxConsoleForClassification.Size = new System.Drawing.Size(331, 484);
            this.richTextBoxConsoleForClassification.TabIndex = 14;
            this.richTextBoxConsoleForClassification.Text = "";
            // 
            // gViewerForTreeClassif
            // 
            this.gViewerForTreeClassif.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gViewerForTreeClassif.AsyncLayout = false;
            this.gViewerForTreeClassif.AutoScroll = true;
            this.gViewerForTreeClassif.BackwardEnabled = true;
            this.gViewerForTreeClassif.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gViewerForTreeClassif.BuildHitTree = false;
            this.gViewerForTreeClassif.CurrentLayoutMethod = Microsoft.Msagl.GraphViewerGdi.LayoutMethod.SugiyamaScheme;
            this.gViewerForTreeClassif.ForwardEnabled = false;
            this.gViewerForTreeClassif.Graph = null;
            this.gViewerForTreeClassif.LayoutAlgorithmSettingsButtonVisible = false;
            this.gViewerForTreeClassif.LayoutEditingEnabled = true;
            this.gViewerForTreeClassif.Location = new System.Drawing.Point(3, 3);
            this.gViewerForTreeClassif.MouseHitDistance = 0.05D;
            this.gViewerForTreeClassif.Name = "gViewerForTreeClassif";
            this.gViewerForTreeClassif.NavigationVisible = true;
            this.gViewerForTreeClassif.NeedToCalculateLayout = true;
            this.gViewerForTreeClassif.PanButtonPressed = false;
            this.gViewerForTreeClassif.SaveAsImageEnabled = true;
            this.gViewerForTreeClassif.SaveAsMsaglEnabled = false;
            this.gViewerForTreeClassif.SaveButtonVisible = true;
            this.gViewerForTreeClassif.SaveGraphButtonVisible = true;
            this.gViewerForTreeClassif.SaveInVectorFormatEnabled = true;
            this.gViewerForTreeClassif.Size = new System.Drawing.Size(564, 484);
            this.gViewerForTreeClassif.TabIndex = 13;
            this.gViewerForTreeClassif.ToolBarIsVisible = true;
            this.gViewerForTreeClassif.ZoomF = 1D;
            this.gViewerForTreeClassif.ZoomFraction = 0.5D;
            this.gViewerForTreeClassif.ZoomWindowThreshold = 0.05D;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gViewerForTreeClassif);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBoxConsoleForClassification);
            this.splitContainer1.Size = new System.Drawing.Size(907, 490);
            this.splitContainer1.SplitterDistance = 570;
            this.splitContainer1.TabIndex = 15;
            // 
            // FormForClassificationTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 495);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormForClassificationTree";
            this.Text = "Classification Information";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox richTextBoxConsoleForClassification;
        public Microsoft.Msagl.GraphViewerGdi.GViewer gViewerForTreeClassif;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}