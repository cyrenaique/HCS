namespace HCSAnalyzer.Controls
{
    partial class FormToDisplayArray
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormToDisplayArray));
            this.SuspendLayout();
            // 
            // FormToDisplayArray
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1009, 456);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormToDisplayArray";
            this.Text = "Array Display";
            this.Load += new System.EventHandler(this.FormToDisplayArray_Load);
            this.ClientSizeChanged += new System.EventHandler(this.FormToDisplayArray_ClientSizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormToDisplayArray_Paint);
            this.ResumeLayout(false);

        }

        #endregion



    }
}