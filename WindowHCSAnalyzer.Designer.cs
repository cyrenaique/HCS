using System.Windows.Forms;
namespace HCSAnalyzer
{
    partial class HCSAnalyzer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Classification Tree");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Classification", new System.Windows.Forms.TreeNode[] {
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Correlation Matrix and Ranking");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Systematic Errors Table");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Z-Factors");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Quality Control", new System.Windows.Forms.TreeNode[] {
            treeNode13,
            treeNode14,
            treeNode15});
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Pathway Analysis");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("siRNA screening", new System.Windows.Forms.TreeNode[] {
            treeNode17});
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Weka .Arff File");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Misc", new System.Windows.Forms.TreeNode[] {
            treeNode19});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HCSAnalyzer));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageDistribution = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonGlobalOnlySelected = new System.Windows.Forms.Button();
            this.checkBoxDisplayClasses = new System.Windows.Forms.CheckBox();
            this.buttonSizeIncrease = new System.Windows.Forms.Button();
            this.buttonSizeDecrease = new System.Windows.Forms.Button();
            this.checkBoxApplyToAllPlates = new System.Windows.Forms.CheckBox();
            this.labelMax = new System.Windows.Forms.Label();
            this.panelForLUT = new System.Windows.Forms.Panel();
            this.buttonGlobalSelection = new System.Windows.Forms.Button();
            this.labelMin = new System.Windows.Forms.Label();
            this.panelForPlate = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.tabPageDImRed = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButtonDimRedSupervised = new System.Windows.Forms.RadioButton();
            this.radioButtonDimRedUnsupervised = new System.Windows.Forms.RadioButton();
            this.groupBoxUnsupervised = new System.Windows.Forms.GroupBox();
            this.richTextBoxUnsupervisedDimRec = new System.Windows.Forms.RichTextBox();
            this.comboBoxReduceDimSingleClass = new System.Windows.Forms.ComboBox();
            this.numericUpDownNewDimension = new System.Windows.Forms.NumericUpDown();
            this.buttonReduceDim = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxSupervised = new System.Windows.Forms.GroupBox();
            this.comboBoxDimReductionNeutralClass = new System.Windows.Forms.ComboBox();
            this.richTextBoxSupervisedDimRec = new System.Windows.Forms.RichTextBox();
            this.comboBoxReduceDimMultiClass = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.tabPageQualityQtrl = new System.Windows.Forms.TabPage();
            this.buttonRejectPlates = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxRejectionPositiveCtrl = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxRejectionNegativeCtrl = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.numericUpDownRejectionThreshold = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.richTextBoxInformationRejection = new System.Windows.Forms.RichTextBox();
            this.comboBoxRejection = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.richTextBoxInformationForPlateCorrection = new System.Windows.Forms.RichTextBox();
            this.comboBoxMethodForCorrection = new System.Windows.Forms.ComboBox();
            this.buttonCorrectionPlateByPlate = new System.Windows.Forms.Button();
            this.dataGridViewForQualityControl = new System.Windows.Forms.DataGridView();
            this.buttonQualityControl = new System.Windows.Forms.Button();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.tabPageNormalization = new System.Windows.Forms.TabPage();
            this.buttonNormalize = new System.Windows.Forms.Button();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.comboBoxNormalizationPositiveCtrl = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxNormalizationNegativeCtrl = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.richTextBoxInfoForNormalization = new System.Windows.Forms.RichTextBox();
            this.comboBoxMethodForNormalization = new System.Windows.Forms.ComboBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.tabPageClassification = new System.Windows.Forms.TabPage();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.radioButtonClusterFullScreen = new System.Windows.Forms.RadioButton();
            this.radioButtonClusterPlateByPlate = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBoxAutomatedClusterNumber = new System.Windows.Forms.CheckBox();
            this.richTextBoxInfoClustering = new System.Windows.Forms.RichTextBox();
            this.comboBoxClusteringMethod = new System.Windows.Forms.ComboBox();
            this.numericUpDownClusterNumber = new System.Windows.Forms.NumericUpDown();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.radioButtonClassifGlobal = new System.Windows.Forms.RadioButton();
            this.comboBoxNeutralClassForClassif = new System.Windows.Forms.ComboBox();
            this.radioButtonClassifPlateByPlate = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.richTextBoxInfoClassif = new System.Windows.Forms.RichTextBox();
            this.comboBoxCLassificationMethod = new System.Windows.Forms.ComboBox();
            this.buttonCluster = new System.Windows.Forms.Button();
            this.buttonStartClassification = new System.Windows.Forms.Button();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.tabPageExport = new System.Windows.Forms.TabPage();
            this.treeViewSelectionForExport = new System.Windows.Forms.TreeView();
            this.checkBoxExportPlateFormat = new System.Windows.Forms.CheckBox();
            this.checkBoxExportFullScreen = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBoxExportScreeningInformation = new System.Windows.Forms.CheckBox();
            this.richTextBoxForScreeningInformation = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonExport = new System.Windows.Forms.Button();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.imageListForTab = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.labelNumClasses = new System.Windows.Forms.Label();
            this.comboBoxClass = new System.Windows.Forms.ComboBox();
            this.menuStripFile = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateScreenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.univariateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multivariateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveScreentoCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentPlateTomtrToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toARFFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appendDescriptorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAverageValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAverageValuesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.swapClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.applySelectionToScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchToDistributionModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripcomboBoxPlateList = new System.Windows.Forms.ToolStripComboBox();
            this.clusteringToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.visualizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scatterPointsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.distributionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PCAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lDAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.classificationTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xYScatterPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hierarchicalTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qualityControlsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.zscoreSinglePlateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalProbabilityPlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.systematicErrorsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dRCAnalysisToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.displayDRCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayRespondingDRCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.screenAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visualizationToolStripMenuItemPCA = new System.Windows.Forms.ToolStripMenuItem();
            this.scatterPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.histogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stackedHistogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xYScatterPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lDAToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pCAToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.hierarchicalClusteringToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.qualityControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zscoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sSMDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalProbabilityPlotToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.correlationMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coeffOfVariationEvolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.descriptorEvolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemGeneAnalysis = new System.Windows.Forms.ToolStripMenuItem();
            this.findGeneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pahtwaysAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dRCAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertDRCToWellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.platesManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doseResponseManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutHCSAnalyzerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkedListBoxActiveDescriptors = new System.Windows.Forms.CheckedListBox();
            this.comboBoxDescriptorToDisplay = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label8 = new System.Windows.Forms.Label();
            this.contextMenuStripForLUT = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlMain.SuspendLayout();
            this.tabPageDistribution.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelForPlate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.tabPageDImRed.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBoxUnsupervised.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNewDimension)).BeginInit();
            this.groupBoxSupervised.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.tabPageQualityQtrl.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRejectionThreshold)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForQualityControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.tabPageNormalization.SuspendLayout();
            this.groupBox15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.tabPageClassification.SuspendLayout();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClusterNumber)).BeginInit();
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.tabPageExport.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            this.menuStripFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuStripForLUT.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.AllowDrop = true;
            this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMain.Controls.Add(this.tabPageDistribution);
            this.tabControlMain.Controls.Add(this.tabPageDImRed);
            this.tabControlMain.Controls.Add(this.tabPageQualityQtrl);
            this.tabControlMain.Controls.Add(this.tabPageNormalization);
            this.tabControlMain.Controls.Add(this.tabPageClassification);
            this.tabControlMain.Controls.Add(this.tabPageExport);
            this.tabControlMain.ImageList = this.imageListForTab;
            this.tabControlMain.Location = new System.Drawing.Point(3, 3);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1186, 488);
            this.tabControlMain.TabIndex = 5;
            this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
            this.tabControlMain.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControlMain_DragDrop);
            this.tabControlMain.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabControlMain_DragEnter);
            // 
            // tabPageDistribution
            // 
            this.tabPageDistribution.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageDistribution.Controls.Add(this.panel1);
            this.tabPageDistribution.Controls.Add(this.panelForPlate);
            this.tabPageDistribution.ImageIndex = 0;
            this.tabPageDistribution.Location = new System.Drawing.Point(4, 33);
            this.tabPageDistribution.Name = "tabPageDistribution";
            this.tabPageDistribution.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDistribution.Size = new System.Drawing.Size(1178, 451);
            this.tabPageDistribution.TabIndex = 0;
            this.tabPageDistribution.Text = "Current Plate";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.buttonGlobalOnlySelected);
            this.panel1.Controls.Add(this.checkBoxDisplayClasses);
            this.panel1.Controls.Add(this.buttonSizeIncrease);
            this.panel1.Controls.Add(this.buttonSizeDecrease);
            this.panel1.Controls.Add(this.checkBoxApplyToAllPlates);
            this.panel1.Controls.Add(this.labelMax);
            this.panel1.Controls.Add(this.panelForLUT);
            this.panel1.Controls.Add(this.buttonGlobalSelection);
            this.panel1.Controls.Add(this.labelMin);
            this.panel1.Location = new System.Drawing.Point(1000, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(174, 439);
            this.panel1.TabIndex = 34;
            // 
            // buttonGlobalOnlySelected
            // 
            this.buttonGlobalOnlySelected.Location = new System.Drawing.Point(13, 73);
            this.buttonGlobalOnlySelected.Name = "buttonGlobalOnlySelected";
            this.buttonGlobalOnlySelected.Size = new System.Drawing.Size(90, 39);
            this.buttonGlobalOnlySelected.TabIndex = 3;
            this.buttonGlobalOnlySelected.Text = "Global only selected";
            this.buttonGlobalOnlySelected.UseVisualStyleBackColor = true;
            this.buttonGlobalOnlySelected.Click += new System.EventHandler(this.buttonGlobalOnlySelected_Click);
            // 
            // checkBoxDisplayClasses
            // 
            this.checkBoxDisplayClasses.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxDisplayClasses.AutoSize = true;
            this.checkBoxDisplayClasses.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.checkBoxDisplayClasses.Location = new System.Drawing.Point(19, 239);
            this.checkBoxDisplayClasses.Name = "checkBoxDisplayClasses";
            this.checkBoxDisplayClasses.Size = new System.Drawing.Size(78, 23);
            this.checkBoxDisplayClasses.TabIndex = 5;
            this.checkBoxDisplayClasses.Text = "Display class";
            this.checkBoxDisplayClasses.UseVisualStyleBackColor = true;
            this.checkBoxDisplayClasses.CheckedChanged += new System.EventHandler(this.checkBoxDisplayClasses_CheckedChanged);
            // 
            // buttonSizeIncrease
            // 
            this.buttonSizeIncrease.Image = global::HCSAnalyzer.Properties.Resources.zoom_in_41;
            this.buttonSizeIncrease.Location = new System.Drawing.Point(63, 363);
            this.buttonSizeIncrease.Name = "buttonSizeIncrease";
            this.buttonSizeIncrease.Size = new System.Drawing.Size(48, 40);
            this.buttonSizeIncrease.TabIndex = 7;
            this.buttonSizeIncrease.UseVisualStyleBackColor = true;
            this.buttonSizeIncrease.Click += new System.EventHandler(this.buttonSizeIncrease_Click);
            // 
            // buttonSizeDecrease
            // 
            this.buttonSizeDecrease.Image = global::HCSAnalyzer.Properties.Resources.zoom_out_41;
            this.buttonSizeDecrease.Location = new System.Drawing.Point(12, 363);
            this.buttonSizeDecrease.Name = "buttonSizeDecrease";
            this.buttonSizeDecrease.Size = new System.Drawing.Size(45, 40);
            this.buttonSizeDecrease.TabIndex = 6;
            this.buttonSizeDecrease.UseVisualStyleBackColor = true;
            this.buttonSizeDecrease.Click += new System.EventHandler(this.buttonSizeDecrease_Click);
            // 
            // checkBoxApplyToAllPlates
            // 
            this.checkBoxApplyToAllPlates.AutoSize = true;
            this.checkBoxApplyToAllPlates.Checked = true;
            this.checkBoxApplyToAllPlates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxApplyToAllPlates.Location = new System.Drawing.Point(9, 168);
            this.checkBoxApplyToAllPlates.Name = "checkBoxApplyToAllPlates";
            this.checkBoxApplyToAllPlates.Size = new System.Drawing.Size(108, 17);
            this.checkBoxApplyToAllPlates.TabIndex = 4;
            this.checkBoxApplyToAllPlates.Text = "Apply to all plates";
            this.checkBoxApplyToAllPlates.UseVisualStyleBackColor = true;
            this.checkBoxApplyToAllPlates.CheckedChanged += new System.EventHandler(this.checkBoxApplyToAllPlates_CheckedChanged);
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.labelMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMax.Location = new System.Drawing.Point(110, 8);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(20, 12);
            this.labelMax.TabIndex = 10;
            this.labelMax.Text = "###";
            this.labelMax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelForLUT
            // 
            this.panelForLUT.BackgroundImage = global::HCSAnalyzer.Properties.Resources.LUT;
            this.panelForLUT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelForLUT.Location = new System.Drawing.Point(120, 28);
            this.panelForLUT.Name = "panelForLUT";
            this.panelForLUT.Size = new System.Drawing.Size(27, 294);
            this.panelForLUT.TabIndex = 30;
            this.panelForLUT.Paint += new System.Windows.Forms.PaintEventHandler(this.panelForLUT_Paint);
            // 
            // buttonGlobalSelection
            // 
            this.buttonGlobalSelection.Location = new System.Drawing.Point(13, 28);
            this.buttonGlobalSelection.Name = "buttonGlobalSelection";
            this.buttonGlobalSelection.Size = new System.Drawing.Size(90, 39);
            this.buttonGlobalSelection.TabIndex = 2;
            this.buttonGlobalSelection.Text = "Global";
            this.buttonGlobalSelection.UseVisualStyleBackColor = true;
            this.buttonGlobalSelection.Click += new System.EventHandler(this.buttonGlobalSelection_Click);
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMin.Location = new System.Drawing.Point(110, 334);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(20, 12);
            this.labelMin.TabIndex = 11;
            this.labelMin.Text = "###";
            this.labelMin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelForPlate
            // 
            this.panelForPlate.AllowDrop = true;
            this.panelForPlate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelForPlate.AutoScroll = true;
            this.panelForPlate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(37)))), ((int)(((byte)(63)))));
            this.panelForPlate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelForPlate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelForPlate.Controls.Add(this.pictureBox3);
            this.panelForPlate.Location = new System.Drawing.Point(6, 6);
            this.panelForPlate.Name = "panelForPlate";
            this.panelForPlate.Size = new System.Drawing.Size(992, 439);
            this.panelForPlate.TabIndex = 0;
            this.panelForPlate.Paint += new System.Windows.Forms.PaintEventHandler(this.panelForPlate_Paint);
            this.panelForPlate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelForPlate_MouseDown);
            this.panelForPlate.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelForPlate_MouseMove);
            this.panelForPlate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelForPlate_MouseUp);
            this.panelForPlate.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.panelForPlate_MouseWheel);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox3.Image = global::HCSAnalyzer.Properties.Resources.DarkLogo;
            this.pictureBox3.Location = new System.Drawing.Point(838, 289);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(147, 143);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // tabPageDImRed
            // 
            this.tabPageDImRed.Controls.Add(this.panel2);
            this.tabPageDImRed.Controls.Add(this.pictureBox4);
            this.tabPageDImRed.ImageIndex = 5;
            this.tabPageDImRed.Location = new System.Drawing.Point(4, 33);
            this.tabPageDImRed.Name = "tabPageDImRed";
            this.tabPageDImRed.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDImRed.Size = new System.Drawing.Size(1178, 451);
            this.tabPageDImRed.TabIndex = 8;
            this.tabPageDImRed.Text = "Dimensionality Reduction";
            this.tabPageDImRed.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.Controls.Add(this.radioButtonDimRedSupervised);
            this.panel2.Controls.Add(this.radioButtonDimRedUnsupervised);
            this.panel2.Controls.Add(this.groupBoxUnsupervised);
            this.panel2.Controls.Add(this.numericUpDownNewDimension);
            this.panel2.Controls.Add(this.buttonReduceDim);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.groupBoxSupervised);
            this.panel2.Location = new System.Drawing.Point(6, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(577, 439);
            this.panel2.TabIndex = 9;
            // 
            // radioButtonDimRedSupervised
            // 
            this.radioButtonDimRedSupervised.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonDimRedSupervised.AutoSize = true;
            this.radioButtonDimRedSupervised.Location = new System.Drawing.Point(378, 55);
            this.radioButtonDimRedSupervised.Name = "radioButtonDimRedSupervised";
            this.radioButtonDimRedSupervised.Size = new System.Drawing.Size(78, 17);
            this.radioButtonDimRedSupervised.TabIndex = 3;
            this.radioButtonDimRedSupervised.TabStop = true;
            this.radioButtonDimRedSupervised.Text = "Supervised";
            this.radioButtonDimRedSupervised.UseVisualStyleBackColor = true;
            this.radioButtonDimRedSupervised.CheckedChanged += new System.EventHandler(this.radioButtonDimRedSupervised_CheckedChanged);
            // 
            // radioButtonDimRedUnsupervised
            // 
            this.radioButtonDimRedUnsupervised.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonDimRedUnsupervised.AutoSize = true;
            this.radioButtonDimRedUnsupervised.Checked = true;
            this.radioButtonDimRedUnsupervised.Location = new System.Drawing.Point(105, 55);
            this.radioButtonDimRedUnsupervised.Name = "radioButtonDimRedUnsupervised";
            this.radioButtonDimRedUnsupervised.Size = new System.Drawing.Size(90, 17);
            this.radioButtonDimRedUnsupervised.TabIndex = 2;
            this.radioButtonDimRedUnsupervised.TabStop = true;
            this.radioButtonDimRedUnsupervised.Text = "Unsupervised";
            this.radioButtonDimRedUnsupervised.UseVisualStyleBackColor = true;
            this.radioButtonDimRedUnsupervised.CheckedChanged += new System.EventHandler(this.radioButtonDimRedUnsupervised_CheckedChanged);
            // 
            // groupBoxUnsupervised
            // 
            this.groupBoxUnsupervised.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxUnsupervised.Controls.Add(this.richTextBoxUnsupervisedDimRec);
            this.groupBoxUnsupervised.Controls.Add(this.comboBoxReduceDimSingleClass);
            this.groupBoxUnsupervised.Location = new System.Drawing.Point(17, 78);
            this.groupBoxUnsupervised.Name = "groupBoxUnsupervised";
            this.groupBoxUnsupervised.Size = new System.Drawing.Size(263, 314);
            this.groupBoxUnsupervised.TabIndex = 7;
            this.groupBoxUnsupervised.TabStop = false;
            // 
            // richTextBoxUnsupervisedDimRec
            // 
            this.richTextBoxUnsupervisedDimRec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxUnsupervisedDimRec.Location = new System.Drawing.Point(6, 89);
            this.richTextBoxUnsupervisedDimRec.Name = "richTextBoxUnsupervisedDimRec";
            this.richTextBoxUnsupervisedDimRec.ReadOnly = true;
            this.richTextBoxUnsupervisedDimRec.Size = new System.Drawing.Size(251, 218);
            this.richTextBoxUnsupervisedDimRec.TabIndex = 5;
            this.richTextBoxUnsupervisedDimRec.Text = "";
            this.richTextBoxUnsupervisedDimRec.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxUnsupervisedDimRec_LinkClicked);
            // 
            // comboBoxReduceDimSingleClass
            // 
            this.comboBoxReduceDimSingleClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxReduceDimSingleClass.FormattingEnabled = true;
            this.comboBoxReduceDimSingleClass.Items.AddRange(new object[] {
            "PCA",
            "Greedy Stepwise"});
            this.comboBoxReduceDimSingleClass.Location = new System.Drawing.Point(40, 25);
            this.comboBoxReduceDimSingleClass.Name = "comboBoxReduceDimSingleClass";
            this.comboBoxReduceDimSingleClass.Size = new System.Drawing.Size(182, 21);
            this.comboBoxReduceDimSingleClass.TabIndex = 4;
            this.comboBoxReduceDimSingleClass.SelectedIndexChanged += new System.EventHandler(this.comboBoxReduceDimSingleClass_SelectedIndexChanged);
            // 
            // numericUpDownNewDimension
            // 
            this.numericUpDownNewDimension.Location = new System.Drawing.Point(285, 16);
            this.numericUpDownNewDimension.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownNewDimension.Name = "numericUpDownNewDimension";
            this.numericUpDownNewDimension.Size = new System.Drawing.Size(83, 20);
            this.numericUpDownNewDimension.TabIndex = 1;
            this.numericUpDownNewDimension.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // buttonReduceDim
            // 
            this.buttonReduceDim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonReduceDim.Enabled = false;
            this.buttonReduceDim.Location = new System.Drawing.Point(208, 395);
            this.buttonReduceDim.Name = "buttonReduceDim";
            this.buttonReduceDim.Size = new System.Drawing.Size(150, 37);
            this.buttonReduceDim.TabIndex = 9;
            this.buttonReduceDim.Text = "Reduce Dimensionality";
            this.buttonReduceDim.UseVisualStyleBackColor = true;
            this.buttonReduceDim.Click += new System.EventHandler(this.buttonReduceDim_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(198, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "New Dimension";
            // 
            // groupBoxSupervised
            // 
            this.groupBoxSupervised.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxSupervised.Controls.Add(this.comboBoxDimReductionNeutralClass);
            this.groupBoxSupervised.Controls.Add(this.richTextBoxSupervisedDimRec);
            this.groupBoxSupervised.Controls.Add(this.comboBoxReduceDimMultiClass);
            this.groupBoxSupervised.Controls.Add(this.label6);
            this.groupBoxSupervised.Enabled = false;
            this.groupBoxSupervised.Location = new System.Drawing.Point(286, 78);
            this.groupBoxSupervised.Name = "groupBoxSupervised";
            this.groupBoxSupervised.Size = new System.Drawing.Size(263, 314);
            this.groupBoxSupervised.TabIndex = 8;
            this.groupBoxSupervised.TabStop = false;
            // 
            // comboBoxDimReductionNeutralClass
            // 
            this.comboBoxDimReductionNeutralClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxDimReductionNeutralClass.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxDimReductionNeutralClass.FormattingEnabled = true;
            this.comboBoxDimReductionNeutralClass.Items.AddRange(new object[] {
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
            this.comboBoxDimReductionNeutralClass.Location = new System.Drawing.Point(111, 57);
            this.comboBoxDimReductionNeutralClass.Name = "comboBoxDimReductionNeutralClass";
            this.comboBoxDimReductionNeutralClass.Size = new System.Drawing.Size(133, 21);
            this.comboBoxDimReductionNeutralClass.TabIndex = 7;
            this.comboBoxDimReductionNeutralClass.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxDimReductionNeutralClass_DrawItem);
            // 
            // richTextBoxSupervisedDimRec
            // 
            this.richTextBoxSupervisedDimRec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxSupervisedDimRec.Location = new System.Drawing.Point(6, 89);
            this.richTextBoxSupervisedDimRec.Name = "richTextBoxSupervisedDimRec";
            this.richTextBoxSupervisedDimRec.ReadOnly = true;
            this.richTextBoxSupervisedDimRec.Size = new System.Drawing.Size(251, 218);
            this.richTextBoxSupervisedDimRec.TabIndex = 8;
            this.richTextBoxSupervisedDimRec.Text = "";
            this.richTextBoxSupervisedDimRec.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxSupervisedDimRec_LinkClicked);
            // 
            // comboBoxReduceDimMultiClass
            // 
            this.comboBoxReduceDimMultiClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxReduceDimMultiClass.FormattingEnabled = true;
            this.comboBoxReduceDimMultiClass.Items.AddRange(new object[] {
            "InfoGain",
            "OneR",
            "Greedy"});
            this.comboBoxReduceDimMultiClass.Location = new System.Drawing.Point(40, 25);
            this.comboBoxReduceDimMultiClass.Name = "comboBoxReduceDimMultiClass";
            this.comboBoxReduceDimMultiClass.Size = new System.Drawing.Size(182, 21);
            this.comboBoxReduceDimMultiClass.TabIndex = 6;
            this.comboBoxReduceDimMultiClass.SelectedIndexChanged += new System.EventHandler(this.comboBoxReduceDimMultiClass_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Neutral Class";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox4.Image = global::HCSAnalyzer.Properties.Resources.WhitePicture;
            this.pictureBox4.Location = new System.Drawing.Point(1025, 302);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(147, 143);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 12;
            this.pictureBox4.TabStop = false;
            // 
            // tabPageQualityQtrl
            // 
            this.tabPageQualityQtrl.Controls.Add(this.buttonRejectPlates);
            this.tabPageQualityQtrl.Controls.Add(this.groupBox1);
            this.tabPageQualityQtrl.Controls.Add(this.groupBox2);
            this.tabPageQualityQtrl.Controls.Add(this.buttonCorrectionPlateByPlate);
            this.tabPageQualityQtrl.Controls.Add(this.dataGridViewForQualityControl);
            this.tabPageQualityQtrl.Controls.Add(this.buttonQualityControl);
            this.tabPageQualityQtrl.Controls.Add(this.pictureBox5);
            this.tabPageQualityQtrl.ImageIndex = 1;
            this.tabPageQualityQtrl.Location = new System.Drawing.Point(4, 33);
            this.tabPageQualityQtrl.Name = "tabPageQualityQtrl";
            this.tabPageQualityQtrl.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageQualityQtrl.Size = new System.Drawing.Size(1178, 451);
            this.tabPageQualityQtrl.TabIndex = 7;
            this.tabPageQualityQtrl.Text = "Systematic Error Identification & Correction";
            this.tabPageQualityQtrl.UseVisualStyleBackColor = true;
            // 
            // buttonRejectPlates
            // 
            this.buttonRejectPlates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRejectPlates.Enabled = false;
            this.buttonRejectPlates.Location = new System.Drawing.Point(760, 411);
            this.buttonRejectPlates.Name = "buttonRejectPlates";
            this.buttonRejectPlates.Size = new System.Drawing.Size(150, 34);
            this.buttonRejectPlates.TabIndex = 14;
            this.buttonRejectPlates.Text = "Reject Plates";
            this.buttonRejectPlates.UseVisualStyleBackColor = true;
            this.buttonRejectPlates.Click += new System.EventHandler(this.buttonRejectPlates_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.comboBoxRejectionPositiveCtrl);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.comboBoxRejectionNegativeCtrl);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.numericUpDownRejectionThreshold);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.richTextBoxInformationRejection);
            this.groupBox1.Controls.Add(this.comboBoxRejection);
            this.groupBox1.Location = new System.Drawing.Point(674, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(318, 399);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rejection";
            // 
            // comboBoxRejectionPositiveCtrl
            // 
            this.comboBoxRejectionPositiveCtrl.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxRejectionPositiveCtrl.FormattingEnabled = true;
            this.comboBoxRejectionPositiveCtrl.Items.AddRange(new object[] {
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
            this.comboBoxRejectionPositiveCtrl.Location = new System.Drawing.Point(184, 135);
            this.comboBoxRejectionPositiveCtrl.Name = "comboBoxRejectionPositiveCtrl";
            this.comboBoxRejectionPositiveCtrl.Size = new System.Drawing.Size(108, 21);
            this.comboBoxRejectionPositiveCtrl.TabIndex = 31;
            this.comboBoxRejectionPositiveCtrl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxRejectionPositiveCtrl_DrawItem);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(199, 119);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 13);
            this.label11.TabIndex = 33;
            this.label11.Text = "Positive Control";
            // 
            // comboBoxRejectionNegativeCtrl
            // 
            this.comboBoxRejectionNegativeCtrl.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxRejectionNegativeCtrl.FormattingEnabled = true;
            this.comboBoxRejectionNegativeCtrl.Items.AddRange(new object[] {
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
            this.comboBoxRejectionNegativeCtrl.Location = new System.Drawing.Point(27, 135);
            this.comboBoxRejectionNegativeCtrl.Name = "comboBoxRejectionNegativeCtrl";
            this.comboBoxRejectionNegativeCtrl.Size = new System.Drawing.Size(110, 21);
            this.comboBoxRejectionNegativeCtrl.TabIndex = 30;
            this.comboBoxRejectionNegativeCtrl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxRejectionNegativeCtrl_DrawItem);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(39, 119);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(86, 13);
            this.label12.TabIndex = 32;
            this.label12.Text = "Negative Control";
            // 
            // numericUpDownRejectionThreshold
            // 
            this.numericUpDownRejectionThreshold.DecimalPlaces = 2;
            this.numericUpDownRejectionThreshold.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownRejectionThreshold.Location = new System.Drawing.Point(132, 62);
            this.numericUpDownRejectionThreshold.Name = "numericUpDownRejectionThreshold";
            this.numericUpDownRejectionThreshold.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownRejectionThreshold.TabIndex = 6;
            this.numericUpDownRejectionThreshold.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(67, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Threshold";
            // 
            // richTextBoxInformationRejection
            // 
            this.richTextBoxInformationRejection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxInformationRejection.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxInformationRejection.ForeColor = System.Drawing.SystemColors.WindowText;
            this.richTextBoxInformationRejection.Location = new System.Drawing.Point(6, 173);
            this.richTextBoxInformationRejection.Name = "richTextBoxInformationRejection";
            this.richTextBoxInformationRejection.ReadOnly = true;
            this.richTextBoxInformationRejection.Size = new System.Drawing.Size(305, 220);
            this.richTextBoxInformationRejection.TabIndex = 4;
            this.richTextBoxInformationRejection.Text = "";
            this.richTextBoxInformationRejection.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxInformationRejection_LinkClicked);
            // 
            // comboBoxRejection
            // 
            this.comboBoxRejection.FormattingEnabled = true;
            this.comboBoxRejection.Items.AddRange(new object[] {
            "Z-Factor"});
            this.comboBoxRejection.Location = new System.Drawing.Point(68, 28);
            this.comboBoxRejection.Name = "comboBoxRejection";
            this.comboBoxRejection.Size = new System.Drawing.Size(182, 21);
            this.comboBoxRejection.TabIndex = 3;
            this.comboBoxRejection.SelectedIndexChanged += new System.EventHandler(this.comboBoxRejection_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.richTextBoxInformationForPlateCorrection);
            this.groupBox2.Controls.Add(this.comboBoxMethodForCorrection);
            this.groupBox2.Location = new System.Drawing.Point(406, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(262, 399);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Correction";
            // 
            // richTextBoxInformationForPlateCorrection
            // 
            this.richTextBoxInformationForPlateCorrection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxInformationForPlateCorrection.Location = new System.Drawing.Point(6, 103);
            this.richTextBoxInformationForPlateCorrection.Name = "richTextBoxInformationForPlateCorrection";
            this.richTextBoxInformationForPlateCorrection.ReadOnly = true;
            this.richTextBoxInformationForPlateCorrection.Size = new System.Drawing.Size(251, 290);
            this.richTextBoxInformationForPlateCorrection.TabIndex = 4;
            this.richTextBoxInformationForPlateCorrection.Text = "";
            this.richTextBoxInformationForPlateCorrection.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxInformationForPlateCorrection_LinkClicked);
            // 
            // comboBoxMethodForCorrection
            // 
            this.comboBoxMethodForCorrection.FormattingEnabled = true;
            this.comboBoxMethodForCorrection.Items.AddRange(new object[] {
            "B-Score",
            "Diffusion Model"});
            this.comboBoxMethodForCorrection.Location = new System.Drawing.Point(38, 32);
            this.comboBoxMethodForCorrection.Name = "comboBoxMethodForCorrection";
            this.comboBoxMethodForCorrection.Size = new System.Drawing.Size(182, 21);
            this.comboBoxMethodForCorrection.TabIndex = 3;
            this.comboBoxMethodForCorrection.SelectedIndexChanged += new System.EventHandler(this.comboBoxMethodForCorrection_SelectedIndexChanged);
            // 
            // buttonCorrectionPlateByPlate
            // 
            this.buttonCorrectionPlateByPlate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCorrectionPlateByPlate.Enabled = false;
            this.buttonCorrectionPlateByPlate.Location = new System.Drawing.Point(463, 411);
            this.buttonCorrectionPlateByPlate.Name = "buttonCorrectionPlateByPlate";
            this.buttonCorrectionPlateByPlate.Size = new System.Drawing.Size(150, 34);
            this.buttonCorrectionPlateByPlate.TabIndex = 5;
            this.buttonCorrectionPlateByPlate.Text = "Plate-by-plate Correction";
            this.buttonCorrectionPlateByPlate.UseVisualStyleBackColor = true;
            this.buttonCorrectionPlateByPlate.Click += new System.EventHandler(this.buttonCorrectionPlateByPlate_Click);
            // 
            // dataGridViewForQualityControl
            // 
            this.dataGridViewForQualityControl.AllowUserToAddRows = false;
            this.dataGridViewForQualityControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewForQualityControl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewForQualityControl.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewForQualityControl.Name = "dataGridViewForQualityControl";
            this.dataGridViewForQualityControl.Size = new System.Drawing.Size(394, 399);
            this.dataGridViewForQualityControl.TabIndex = 1;
            this.dataGridViewForQualityControl.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewForQualityControl_CellContentDoubleClick);
            // 
            // buttonQualityControl
            // 
            this.buttonQualityControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonQualityControl.Enabled = false;
            this.buttonQualityControl.Location = new System.Drawing.Point(6, 411);
            this.buttonQualityControl.Name = "buttonQualityControl";
            this.buttonQualityControl.Size = new System.Drawing.Size(394, 34);
            this.buttonQualityControl.TabIndex = 2;
            this.buttonQualityControl.Text = "Systematic error identification";
            this.buttonQualityControl.UseVisualStyleBackColor = true;
            this.buttonQualityControl.Click += new System.EventHandler(this.buttonQualityControl_Click);
            // 
            // pictureBox5
            // 
            this.pictureBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox5.Image = global::HCSAnalyzer.Properties.Resources.WhitePicture;
            this.pictureBox5.Location = new System.Drawing.Point(1025, 302);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(147, 143);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox5.TabIndex = 13;
            this.pictureBox5.TabStop = false;
            // 
            // tabPageNormalization
            // 
            this.tabPageNormalization.Controls.Add(this.buttonNormalize);
            this.tabPageNormalization.Controls.Add(this.groupBox15);
            this.tabPageNormalization.Controls.Add(this.pictureBox6);
            this.tabPageNormalization.ImageIndex = 2;
            this.tabPageNormalization.Location = new System.Drawing.Point(4, 33);
            this.tabPageNormalization.Name = "tabPageNormalization";
            this.tabPageNormalization.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNormalization.Size = new System.Drawing.Size(1178, 451);
            this.tabPageNormalization.TabIndex = 3;
            this.tabPageNormalization.Text = "Normalization";
            this.tabPageNormalization.UseVisualStyleBackColor = true;
            // 
            // buttonNormalize
            // 
            this.buttonNormalize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNormalize.Enabled = false;
            this.buttonNormalize.Location = new System.Drawing.Point(169, 411);
            this.buttonNormalize.Name = "buttonNormalize";
            this.buttonNormalize.Size = new System.Drawing.Size(150, 34);
            this.buttonNormalize.TabIndex = 5;
            this.buttonNormalize.Text = "Normalize";
            this.buttonNormalize.UseVisualStyleBackColor = true;
            this.buttonNormalize.Click += new System.EventHandler(this.buttonNormalize_Click);
            // 
            // groupBox15
            // 
            this.groupBox15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox15.Controls.Add(this.comboBoxNormalizationPositiveCtrl);
            this.groupBox15.Controls.Add(this.label7);
            this.groupBox15.Controls.Add(this.comboBoxNormalizationNegativeCtrl);
            this.groupBox15.Controls.Add(this.label4);
            this.groupBox15.Controls.Add(this.richTextBoxInfoForNormalization);
            this.groupBox15.Controls.Add(this.comboBoxMethodForNormalization);
            this.groupBox15.Location = new System.Drawing.Point(6, 6);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(476, 399);
            this.groupBox15.TabIndex = 8;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Normalization";
            // 
            // comboBoxNormalizationPositiveCtrl
            // 
            this.comboBoxNormalizationPositiveCtrl.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxNormalizationPositiveCtrl.FormattingEnabled = true;
            this.comboBoxNormalizationPositiveCtrl.Items.AddRange(new object[] {
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
            this.comboBoxNormalizationPositiveCtrl.Location = new System.Drawing.Point(335, 92);
            this.comboBoxNormalizationPositiveCtrl.Name = "comboBoxNormalizationPositiveCtrl";
            this.comboBoxNormalizationPositiveCtrl.Size = new System.Drawing.Size(120, 21);
            this.comboBoxNormalizationPositiveCtrl.TabIndex = 3;
            this.comboBoxNormalizationPositiveCtrl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxNormalizationPositiveCtrl_DrawItem);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(257, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "Positive Class";
            // 
            // comboBoxNormalizationNegativeCtrl
            // 
            this.comboBoxNormalizationNegativeCtrl.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxNormalizationNegativeCtrl.FormattingEnabled = true;
            this.comboBoxNormalizationNegativeCtrl.Items.AddRange(new object[] {
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
            this.comboBoxNormalizationNegativeCtrl.Location = new System.Drawing.Point(100, 92);
            this.comboBoxNormalizationNegativeCtrl.Name = "comboBoxNormalizationNegativeCtrl";
            this.comboBoxNormalizationNegativeCtrl.Size = new System.Drawing.Size(120, 21);
            this.comboBoxNormalizationNegativeCtrl.TabIndex = 2;
            this.comboBoxNormalizationNegativeCtrl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxNormalizationNegativeCtrl_DrawItem);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Negative Class";
            // 
            // richTextBoxInfoForNormalization
            // 
            this.richTextBoxInfoForNormalization.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxInfoForNormalization.Location = new System.Drawing.Point(6, 141);
            this.richTextBoxInfoForNormalization.Name = "richTextBoxInfoForNormalization";
            this.richTextBoxInfoForNormalization.ReadOnly = true;
            this.richTextBoxInfoForNormalization.Size = new System.Drawing.Size(464, 252);
            this.richTextBoxInfoForNormalization.TabIndex = 4;
            this.richTextBoxInfoForNormalization.Text = "";
            this.richTextBoxInfoForNormalization.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxInfoForNormalization_LinkClicked);
            // 
            // comboBoxMethodForNormalization
            // 
            this.comboBoxMethodForNormalization.FormattingEnabled = true;
            this.comboBoxMethodForNormalization.Items.AddRange(new object[] {
            "Percent of control ",
            "Normalized percent inhibition",
            "Z-score"});
            this.comboBoxMethodForNormalization.Location = new System.Drawing.Point(147, 29);
            this.comboBoxMethodForNormalization.Name = "comboBoxMethodForNormalization";
            this.comboBoxMethodForNormalization.Size = new System.Drawing.Size(182, 21);
            this.comboBoxMethodForNormalization.TabIndex = 1;
            this.comboBoxMethodForNormalization.SelectedIndexChanged += new System.EventHandler(this.comboBoxMethodForNormalization_SelectedIndexChanged);
            // 
            // pictureBox6
            // 
            this.pictureBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox6.Image = global::HCSAnalyzer.Properties.Resources.WhitePicture;
            this.pictureBox6.Location = new System.Drawing.Point(1025, 302);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(147, 143);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox6.TabIndex = 13;
            this.pictureBox6.TabStop = false;
            // 
            // tabPageClassification
            // 
            this.tabPageClassification.Controls.Add(this.groupBox12);
            this.tabPageClassification.Controls.Add(this.groupBox11);
            this.tabPageClassification.Controls.Add(this.buttonCluster);
            this.tabPageClassification.Controls.Add(this.buttonStartClassification);
            this.tabPageClassification.Controls.Add(this.pictureBox7);
            this.tabPageClassification.ImageIndex = 3;
            this.tabPageClassification.Location = new System.Drawing.Point(4, 33);
            this.tabPageClassification.Name = "tabPageClassification";
            this.tabPageClassification.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageClassification.Size = new System.Drawing.Size(1178, 451);
            this.tabPageClassification.TabIndex = 4;
            this.tabPageClassification.Text = "Classification & Clustering";
            this.tabPageClassification.UseVisualStyleBackColor = true;
            // 
            // groupBox12
            // 
            this.groupBox12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox12.Controls.Add(this.radioButtonClusterFullScreen);
            this.groupBox12.Controls.Add(this.radioButtonClusterPlateByPlate);
            this.groupBox12.Controls.Add(this.label10);
            this.groupBox12.Controls.Add(this.checkBoxAutomatedClusterNumber);
            this.groupBox12.Controls.Add(this.richTextBoxInfoClustering);
            this.groupBox12.Controls.Add(this.comboBoxClusteringMethod);
            this.groupBox12.Controls.Add(this.numericUpDownClusterNumber);
            this.groupBox12.Location = new System.Drawing.Point(9, 6);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(263, 399);
            this.groupBox12.TabIndex = 5;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Clustering";
            // 
            // radioButtonClusterFullScreen
            // 
            this.radioButtonClusterFullScreen.AutoSize = true;
            this.radioButtonClusterFullScreen.Checked = true;
            this.radioButtonClusterFullScreen.Location = new System.Drawing.Point(153, 74);
            this.radioButtonClusterFullScreen.Name = "radioButtonClusterFullScreen";
            this.radioButtonClusterFullScreen.Size = new System.Drawing.Size(78, 17);
            this.radioButtonClusterFullScreen.TabIndex = 28;
            this.radioButtonClusterFullScreen.TabStop = true;
            this.radioButtonClusterFullScreen.Text = "Full Screen";
            this.radioButtonClusterFullScreen.UseVisualStyleBackColor = true;
            // 
            // radioButtonClusterPlateByPlate
            // 
            this.radioButtonClusterPlateByPlate.AutoSize = true;
            this.radioButtonClusterPlateByPlate.Location = new System.Drawing.Point(36, 74);
            this.radioButtonClusterPlateByPlate.Name = "radioButtonClusterPlateByPlate";
            this.radioButtonClusterPlateByPlate.Size = new System.Drawing.Size(91, 17);
            this.radioButtonClusterPlateByPlate.TabIndex = 27;
            this.radioButtonClusterPlateByPlate.Text = "Plate By Plate";
            this.radioButtonClusterPlateByPlate.UseVisualStyleBackColor = true;
            this.radioButtonClusterPlateByPlate.CheckedChanged += new System.EventHandler(this.radioButtonClusterPlateByPlate_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 118);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Cluster Number";
            // 
            // checkBoxAutomatedClusterNumber
            // 
            this.checkBoxAutomatedClusterNumber.AutoSize = true;
            this.checkBoxAutomatedClusterNumber.Location = new System.Drawing.Point(176, 117);
            this.checkBoxAutomatedClusterNumber.Name = "checkBoxAutomatedClusterNumber";
            this.checkBoxAutomatedClusterNumber.Size = new System.Drawing.Size(77, 17);
            this.checkBoxAutomatedClusterNumber.TabIndex = 22;
            this.checkBoxAutomatedClusterNumber.Text = "Automated";
            this.checkBoxAutomatedClusterNumber.UseVisualStyleBackColor = true;
            this.checkBoxAutomatedClusterNumber.CheckedChanged += new System.EventHandler(this.checkBoxAutomatedClusterNumber_CheckedChanged);
            // 
            // richTextBoxInfoClustering
            // 
            this.richTextBoxInfoClustering.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxInfoClustering.Location = new System.Drawing.Point(6, 150);
            this.richTextBoxInfoClustering.Name = "richTextBoxInfoClustering";
            this.richTextBoxInfoClustering.ReadOnly = true;
            this.richTextBoxInfoClustering.Size = new System.Drawing.Size(251, 243);
            this.richTextBoxInfoClustering.TabIndex = 0;
            this.richTextBoxInfoClustering.Text = "";
            this.richTextBoxInfoClustering.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxInfoClustering_LinkClicked);
            // 
            // comboBoxClusteringMethod
            // 
            this.comboBoxClusteringMethod.FormattingEnabled = true;
            this.comboBoxClusteringMethod.Items.AddRange(new object[] {
            "K-Means",
            "EM",
            "Hierarchical"});
            this.comboBoxClusteringMethod.Location = new System.Drawing.Point(40, 28);
            this.comboBoxClusteringMethod.Name = "comboBoxClusteringMethod";
            this.comboBoxClusteringMethod.Size = new System.Drawing.Size(182, 21);
            this.comboBoxClusteringMethod.TabIndex = 19;
            this.comboBoxClusteringMethod.SelectedIndexChanged += new System.EventHandler(this.comboBoxClusteringMethod_SelectedIndexChanged);
            // 
            // numericUpDownClusterNumber
            // 
            this.numericUpDownClusterNumber.Location = new System.Drawing.Point(91, 115);
            this.numericUpDownClusterNumber.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownClusterNumber.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownClusterNumber.Name = "numericUpDownClusterNumber";
            this.numericUpDownClusterNumber.Size = new System.Drawing.Size(66, 20);
            this.numericUpDownClusterNumber.TabIndex = 21;
            this.numericUpDownClusterNumber.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // groupBox11
            // 
            this.groupBox11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox11.Controls.Add(this.radioButtonClassifGlobal);
            this.groupBox11.Controls.Add(this.comboBoxNeutralClassForClassif);
            this.groupBox11.Controls.Add(this.radioButtonClassifPlateByPlate);
            this.groupBox11.Controls.Add(this.label5);
            this.groupBox11.Controls.Add(this.richTextBoxInfoClassif);
            this.groupBox11.Controls.Add(this.comboBoxCLassificationMethod);
            this.groupBox11.Location = new System.Drawing.Point(295, 6);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(263, 399);
            this.groupBox11.TabIndex = 5;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Classification";
            // 
            // radioButtonClassifGlobal
            // 
            this.radioButtonClassifGlobal.AutoSize = true;
            this.radioButtonClassifGlobal.Location = new System.Drawing.Point(144, 74);
            this.radioButtonClassifGlobal.Name = "radioButtonClassifGlobal";
            this.radioButtonClassifGlobal.Size = new System.Drawing.Size(78, 17);
            this.radioButtonClassifGlobal.TabIndex = 6;
            this.radioButtonClassifGlobal.TabStop = true;
            this.radioButtonClassifGlobal.Text = "Full Screen";
            this.radioButtonClassifGlobal.UseVisualStyleBackColor = true;
            // 
            // comboBoxNeutralClassForClassif
            // 
            this.comboBoxNeutralClassForClassif.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxNeutralClassForClassif.FormattingEnabled = true;
            this.comboBoxNeutralClassForClassif.Items.AddRange(new object[] {
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
            this.comboBoxNeutralClassForClassif.Location = new System.Drawing.Point(113, 111);
            this.comboBoxNeutralClassForClassif.Name = "comboBoxNeutralClassForClassif";
            this.comboBoxNeutralClassForClassif.Size = new System.Drawing.Size(133, 21);
            this.comboBoxNeutralClassForClassif.TabIndex = 26;
            this.comboBoxNeutralClassForClassif.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxNeutralClassForClassif_DrawItem);
            // 
            // radioButtonClassifPlateByPlate
            // 
            this.radioButtonClassifPlateByPlate.AutoSize = true;
            this.radioButtonClassifPlateByPlate.Checked = true;
            this.radioButtonClassifPlateByPlate.Location = new System.Drawing.Point(27, 74);
            this.radioButtonClassifPlateByPlate.Name = "radioButtonClassifPlateByPlate";
            this.radioButtonClassifPlateByPlate.Size = new System.Drawing.Size(91, 17);
            this.radioButtonClassifPlateByPlate.TabIndex = 2;
            this.radioButtonClassifPlateByPlate.TabStop = true;
            this.radioButtonClassifPlateByPlate.Text = "Plate By Plate";
            this.radioButtonClassifPlateByPlate.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "To Be Classified";
            // 
            // richTextBoxInfoClassif
            // 
            this.richTextBoxInfoClassif.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxInfoClassif.Location = new System.Drawing.Point(6, 150);
            this.richTextBoxInfoClassif.Name = "richTextBoxInfoClassif";
            this.richTextBoxInfoClassif.ReadOnly = true;
            this.richTextBoxInfoClassif.Size = new System.Drawing.Size(251, 243);
            this.richTextBoxInfoClassif.TabIndex = 0;
            this.richTextBoxInfoClassif.Text = "";
            this.richTextBoxInfoClassif.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxInfoClassif_LinkClicked);
            // 
            // comboBoxCLassificationMethod
            // 
            this.comboBoxCLassificationMethod.FormattingEnabled = true;
            this.comboBoxCLassificationMethod.Items.AddRange(new object[] {
            "C4.5",
            "Support Vector Machine",
            "Neural Network",
            "K Nearest Neighbor(s)"});
            this.comboBoxCLassificationMethod.Location = new System.Drawing.Point(40, 28);
            this.comboBoxCLassificationMethod.Name = "comboBoxCLassificationMethod";
            this.comboBoxCLassificationMethod.Size = new System.Drawing.Size(182, 21);
            this.comboBoxCLassificationMethod.TabIndex = 19;
            this.comboBoxCLassificationMethod.SelectedIndexChanged += new System.EventHandler(this.comboBoxCLassificationMethod_SelectedIndexChanged);
            // 
            // buttonCluster
            // 
            this.buttonCluster.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCluster.Enabled = false;
            this.buttonCluster.Location = new System.Drawing.Point(67, 411);
            this.buttonCluster.Name = "buttonCluster";
            this.buttonCluster.Size = new System.Drawing.Size(150, 34);
            this.buttonCluster.TabIndex = 1;
            this.buttonCluster.Text = "Cluster";
            this.buttonCluster.UseVisualStyleBackColor = true;
            this.buttonCluster.Click += new System.EventHandler(this.buttonCluster_Click);
            // 
            // buttonStartClassification
            // 
            this.buttonStartClassification.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStartClassification.Enabled = false;
            this.buttonStartClassification.Location = new System.Drawing.Point(352, 411);
            this.buttonStartClassification.Name = "buttonStartClassification";
            this.buttonStartClassification.Size = new System.Drawing.Size(150, 34);
            this.buttonStartClassification.TabIndex = 1;
            this.buttonStartClassification.Text = "Classify";
            this.buttonStartClassification.UseVisualStyleBackColor = true;
            this.buttonStartClassification.Click += new System.EventHandler(this.buttonStartClassification_Click_1);
            // 
            // pictureBox7
            // 
            this.pictureBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox7.Image = global::HCSAnalyzer.Properties.Resources.WhitePicture;
            this.pictureBox7.Location = new System.Drawing.Point(1025, 302);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(147, 143);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox7.TabIndex = 13;
            this.pictureBox7.TabStop = false;
            // 
            // tabPageExport
            // 
            this.tabPageExport.Controls.Add(this.treeViewSelectionForExport);
            this.tabPageExport.Controls.Add(this.checkBoxExportPlateFormat);
            this.tabPageExport.Controls.Add(this.checkBoxExportFullScreen);
            this.tabPageExport.Controls.Add(this.groupBox5);
            this.tabPageExport.Controls.Add(this.groupBox4);
            this.tabPageExport.Controls.Add(this.groupBox3);
            this.tabPageExport.Controls.Add(this.buttonExport);
            this.tabPageExport.Controls.Add(this.pictureBox8);
            this.tabPageExport.ImageIndex = 4;
            this.tabPageExport.Location = new System.Drawing.Point(4, 33);
            this.tabPageExport.Name = "tabPageExport";
            this.tabPageExport.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExport.Size = new System.Drawing.Size(1178, 451);
            this.tabPageExport.TabIndex = 5;
            this.tabPageExport.Text = "Report Export";
            this.tabPageExport.UseVisualStyleBackColor = true;
            // 
            // treeViewSelectionForExport
            // 
            this.treeViewSelectionForExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewSelectionForExport.CheckBoxes = true;
            this.treeViewSelectionForExport.FullRowSelect = true;
            this.treeViewSelectionForExport.Location = new System.Drawing.Point(394, 148);
            this.treeViewSelectionForExport.Name = "treeViewSelectionForExport";
            treeNode11.Name = "NodeClassifTree";
            treeNode11.Text = "Classification Tree";
            treeNode12.Name = "NodeClassification";
            treeNode12.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode12.Text = "Classification";
            treeNode13.Checked = true;
            treeNode13.Name = "NodeCorrelationMatRank";
            treeNode13.Text = "Correlation Matrix and Ranking";
            treeNode14.Checked = true;
            treeNode14.Name = "NodeSystematicError";
            treeNode14.Text = "Systematic Errors Table";
            treeNode15.Checked = true;
            treeNode15.Name = "NodeZfactor";
            treeNode15.Text = "Z-Factors";
            treeNode16.Checked = true;
            treeNode16.Name = "NodeQualityControl";
            treeNode16.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode16.Text = "Quality Control";
            treeNode17.Name = "NodePathwayAnalysis";
            treeNode17.Text = "Pathway Analysis";
            treeNode18.Name = "NodesiRNA";
            treeNode18.Text = "siRNA screening";
            treeNode19.Name = "NodeWekaArff";
            treeNode19.Text = "Weka .Arff File";
            treeNode20.Name = "NodeMisc";
            treeNode20.Text = "Misc";
            this.treeViewSelectionForExport.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode12,
            treeNode16,
            treeNode18,
            treeNode20});
            this.treeViewSelectionForExport.Size = new System.Drawing.Size(485, 206);
            this.treeViewSelectionForExport.TabIndex = 16;
            // 
            // checkBoxExportPlateFormat
            // 
            this.checkBoxExportPlateFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxExportPlateFormat.AutoSize = true;
            this.checkBoxExportPlateFormat.Checked = true;
            this.checkBoxExportPlateFormat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxExportPlateFormat.Location = new System.Drawing.Point(719, 20);
            this.checkBoxExportPlateFormat.Name = "checkBoxExportPlateFormat";
            this.checkBoxExportPlateFormat.Size = new System.Drawing.Size(85, 17);
            this.checkBoxExportPlateFormat.TabIndex = 10;
            this.checkBoxExportPlateFormat.Text = "Plate Format";
            this.checkBoxExportPlateFormat.UseVisualStyleBackColor = true;
            // 
            // checkBoxExportFullScreen
            // 
            this.checkBoxExportFullScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxExportFullScreen.AutoSize = true;
            this.checkBoxExportFullScreen.Checked = true;
            this.checkBoxExportFullScreen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxExportFullScreen.Location = new System.Drawing.Point(470, 20);
            this.checkBoxExportFullScreen.Name = "checkBoxExportFullScreen";
            this.checkBoxExportFullScreen.Size = new System.Drawing.Size(79, 17);
            this.checkBoxExportFullScreen.TabIndex = 9;
            this.checkBoxExportFullScreen.Text = "Full Screen";
            this.checkBoxExportFullScreen.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.checkBoxExportScreeningInformation);
            this.groupBox5.Controls.Add(this.richTextBoxForScreeningInformation);
            this.groupBox5.Location = new System.Drawing.Point(9, 8);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(375, 437);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "                                             ";
            // 
            // checkBoxExportScreeningInformation
            // 
            this.checkBoxExportScreeningInformation.AutoSize = true;
            this.checkBoxExportScreeningInformation.Checked = true;
            this.checkBoxExportScreeningInformation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxExportScreeningInformation.Location = new System.Drawing.Point(13, -1);
            this.checkBoxExportScreeningInformation.Name = "checkBoxExportScreeningInformation";
            this.checkBoxExportScreeningInformation.Size = new System.Drawing.Size(129, 17);
            this.checkBoxExportScreeningInformation.TabIndex = 11;
            this.checkBoxExportScreeningInformation.Text = "Screening Information";
            this.checkBoxExportScreeningInformation.UseVisualStyleBackColor = true;
            // 
            // richTextBoxForScreeningInformation
            // 
            this.richTextBoxForScreeningInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxForScreeningInformation.Location = new System.Drawing.Point(6, 23);
            this.richTextBoxForScreeningInformation.Name = "richTextBoxForScreeningInformation";
            this.richTextBoxForScreeningInformation.Size = new System.Drawing.Size(363, 408);
            this.richTextBoxForScreeningInformation.TabIndex = 0;
            this.richTextBoxForScreeningInformation.Text = "";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.pictureBox2);
            this.groupBox4.Location = new System.Drawing.Point(641, 43);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(238, 87);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::HCSAnalyzer.Properties.Resources.Capture1;
            this.pictureBox2.Location = new System.Drawing.Point(13, 19);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(212, 46);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.pictureBox1);
            this.groupBox3.Location = new System.Drawing.Point(394, 43);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(238, 87);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::HCSAnalyzer.Properties.Resources.Capture;
            this.pictureBox1.Location = new System.Drawing.Point(13, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(212, 46);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Enabled = false;
            this.buttonExport.Location = new System.Drawing.Point(557, 56);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(160, 39);
            this.buttonExport.TabIndex = 0;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // pictureBox8
            // 
            this.pictureBox8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox8.Image = global::HCSAnalyzer.Properties.Resources.WhitePicture;
            this.pictureBox8.Location = new System.Drawing.Point(1025, 302);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(147, 143);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox8.TabIndex = 15;
            this.pictureBox8.TabStop = false;
            // 
            // imageListForTab
            // 
            this.imageListForTab.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListForTab.ImageStream")));
            this.imageListForTab.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListForTab.Images.SetKeyName(0, "Picture1.png");
            this.imageListForTab.Images.SetKeyName(1, "Picture2.png");
            this.imageListForTab.Images.SetKeyName(2, "Picture3.png");
            this.imageListForTab.Images.SetKeyName(3, "Picture4.png");
            this.imageListForTab.Images.SetKeyName(4, "Picture5.png");
            this.imageListForTab.Images.SetKeyName(5, "Picture6.png");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Class selection";
            // 
            // labelNumClasses
            // 
            this.labelNumClasses.AutoSize = true;
            this.labelNumClasses.Location = new System.Drawing.Point(140, 20);
            this.labelNumClasses.Name = "labelNumClasses";
            this.labelNumClasses.Size = new System.Drawing.Size(28, 13);
            this.labelNumClasses.TabIndex = 29;
            this.labelNumClasses.Text = "###";
            // 
            // comboBoxClass
            // 
            this.comboBoxClass.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxClass.FormattingEnabled = true;
            this.comboBoxClass.Items.AddRange(new object[] {
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
            this.comboBoxClass.Location = new System.Drawing.Point(3, 17);
            this.comboBoxClass.Name = "comboBoxClass";
            this.comboBoxClass.Size = new System.Drawing.Size(117, 21);
            this.comboBoxClass.TabIndex = 1;
            this.comboBoxClass.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxClass_DrawItem);
            this.comboBoxClass.SelectedIndexChanged += new System.EventHandler(this.comboBoxClass_SelectedIndexChanged);
            // 
            // menuStripFile
            // 
            this.menuStripFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.copyAverageValuesToolStripMenuItem,
            this.toolStripcomboBoxPlateList,
            this.clusteringToolStripMenuItem1,
            this.screenAnalysisToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.pluginsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStripFile.Location = new System.Drawing.Point(0, 0);
            this.menuStripFile.Name = "menuStripFile";
            this.menuStripFile.Size = new System.Drawing.Size(1379, 27);
            this.menuStripFile.TabIndex = 12;
            this.menuStripFile.Text = "File";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.loadScreenToolStripMenuItem,
            this.generateScreenToolStripMenuItem1,
            this.toolStripSeparator2,
            this.exportToolStripMenuItem,
            this.appendDescriptorsToolStripMenuItem,
            this.linkToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 23);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Image = global::HCSAnalyzer.Properties.Resources.db_comit;
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.importToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.importToolStripMenuItem.Text = "Import Screen";
            this.importToolStripMenuItem.ToolTipText = "Load screen from regular format";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // loadScreenToolStripMenuItem
            // 
            this.loadScreenToolStripMenuItem.Image = global::HCSAnalyzer.Properties.Resources.document_open_5;
            this.loadScreenToolStripMenuItem.Name = "loadScreenToolStripMenuItem";
            this.loadScreenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadScreenToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.loadScreenToolStripMenuItem.Text = "Load Histogram Based Screen";
            this.loadScreenToolStripMenuItem.ToolTipText = "Load distributions based screens";
            this.loadScreenToolStripMenuItem.Click += new System.EventHandler(this.loadScreenToolStripMenuItem_Click);
            // 
            // generateScreenToolStripMenuItem1
            // 
            this.generateScreenToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.univariateToolStripMenuItem,
            this.multivariateToolStripMenuItem});
            this.generateScreenToolStripMenuItem1.Name = "generateScreenToolStripMenuItem1";
            this.generateScreenToolStripMenuItem1.Size = new System.Drawing.Size(271, 22);
            this.generateScreenToolStripMenuItem1.Text = "Generate Screen";
            // 
            // univariateToolStripMenuItem
            // 
            this.univariateToolStripMenuItem.Name = "univariateToolStripMenuItem";
            this.univariateToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.univariateToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.univariateToolStripMenuItem.Text = "Univariate";
            this.univariateToolStripMenuItem.Click += new System.EventHandler(this.univariateToolStripMenuItem_Click);
            // 
            // multivariateToolStripMenuItem
            // 
            this.multivariateToolStripMenuItem.Name = "multivariateToolStripMenuItem";
            this.multivariateToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.G)));
            this.multivariateToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.multivariateToolStripMenuItem.Text = "Multivariate";
            this.multivariateToolStripMenuItem.Click += new System.EventHandler(this.multivariateToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(268, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveScreentoCSVToolStripMenuItem,
            this.currentPlateTomtrToolStripMenuItem,
            this.toARFFToolStripMenuItem});
            this.exportToolStripMenuItem.Enabled = false;
            this.exportToolStripMenuItem.Image = global::HCSAnalyzer.Properties.Resources.db_update;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.exportToolStripMenuItem.Text = "Save Screen";
            // 
            // SaveScreentoCSVToolStripMenuItem
            // 
            this.SaveScreentoCSVToolStripMenuItem.Name = "SaveScreentoCSVToolStripMenuItem";
            this.SaveScreentoCSVToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.SaveScreentoCSVToolStripMenuItem.Text = "To CSV";
            this.SaveScreentoCSVToolStripMenuItem.Click += new System.EventHandler(this.toExcelToolStripMenuItem_Click);
            // 
            // currentPlateTomtrToolStripMenuItem
            // 
            this.currentPlateTomtrToolStripMenuItem.Name = "currentPlateTomtrToolStripMenuItem";
            this.currentPlateTomtrToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.currentPlateTomtrToolStripMenuItem.Text = "To MTR";
            this.currentPlateTomtrToolStripMenuItem.ToolTipText = "Warning: only the selected descriptor will be saved in this format";
            this.currentPlateTomtrToolStripMenuItem.Click += new System.EventHandler(this.currentPlateTomtrToolStripMenuItem_Click);
            // 
            // toARFFToolStripMenuItem
            // 
            this.toARFFToolStripMenuItem.Name = "toARFFToolStripMenuItem";
            this.toARFFToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.toARFFToolStripMenuItem.Text = "To ARFF";
            this.toARFFToolStripMenuItem.Click += new System.EventHandler(this.toARFFToolStripMenuItem_Click);
            // 
            // appendDescriptorsToolStripMenuItem
            // 
            this.appendDescriptorsToolStripMenuItem.Enabled = false;
            this.appendDescriptorsToolStripMenuItem.Image = global::HCSAnalyzer.Properties.Resources.db_add;
            this.appendDescriptorsToolStripMenuItem.Name = "appendDescriptorsToolStripMenuItem";
            this.appendDescriptorsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.appendDescriptorsToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.appendDescriptorsToolStripMenuItem.Text = "Add Plates";
            this.appendDescriptorsToolStripMenuItem.Click += new System.EventHandler(this.appendAssayToolStripMenuItem_Click);
            // 
            // linkToolStripMenuItem
            // 
            this.linkToolStripMenuItem.Enabled = false;
            this.linkToolStripMenuItem.Image = global::HCSAnalyzer.Properties.Resources.insert_link;
            this.linkToolStripMenuItem.Name = "linkToolStripMenuItem";
            this.linkToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.linkToolStripMenuItem.Text = "Link Data";
            this.linkToolStripMenuItem.Click += new System.EventHandler(this.linkToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(268, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::HCSAnalyzer.Properties.Resources.application_exit;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(271, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // copyAverageValuesToolStripMenuItem
            // 
            this.copyAverageValuesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyAverageValuesToolStripMenuItem1,
            this.copyClassesToolStripMenuItem,
            this.swapClassesToolStripMenuItem,
            this.toolStripSeparator3,
            this.applySelectionToScreenToolStripMenuItem,
            this.toolStripSeparator1,
            this.optionsToolStripMenuItem,
            this.switchToDistributionModeToolStripMenuItem});
            this.copyAverageValuesToolStripMenuItem.Name = "copyAverageValuesToolStripMenuItem";
            this.copyAverageValuesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyAverageValuesToolStripMenuItem.Size = new System.Drawing.Size(39, 23);
            this.copyAverageValuesToolStripMenuItem.Text = "Edit";
            // 
            // copyAverageValuesToolStripMenuItem1
            // 
            this.copyAverageValuesToolStripMenuItem1.Enabled = false;
            this.copyAverageValuesToolStripMenuItem1.Name = "copyAverageValuesToolStripMenuItem1";
            this.copyAverageValuesToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.C)));
            this.copyAverageValuesToolStripMenuItem1.Size = new System.Drawing.Size(282, 22);
            this.copyAverageValuesToolStripMenuItem1.Text = "Copy values to clipboard";
            this.copyAverageValuesToolStripMenuItem1.ToolTipText = "Copy the average values of the current plate and descriptor to the clipboard";
            this.copyAverageValuesToolStripMenuItem1.Click += new System.EventHandler(this.copyAverageValuesToolStripMenuItem1_Click);
            // 
            // copyClassesToolStripMenuItem
            // 
            this.copyClassesToolStripMenuItem.Enabled = false;
            this.copyClassesToolStripMenuItem.Name = "copyClassesToolStripMenuItem";
            this.copyClassesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.copyClassesToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.copyClassesToolStripMenuItem.Text = "Copy classes to clipboard";
            this.copyClassesToolStripMenuItem.ToolTipText = "Copy current plate classes to the clipboard";
            this.copyClassesToolStripMenuItem.Click += new System.EventHandler(this.copyClassesToolStripMenuItem_Click);
            // 
            // swapClassesToolStripMenuItem
            // 
            this.swapClassesToolStripMenuItem.Enabled = false;
            this.swapClassesToolStripMenuItem.Name = "swapClassesToolStripMenuItem";
            this.swapClassesToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.swapClassesToolStripMenuItem.Text = "Swap Classes";
            this.swapClassesToolStripMenuItem.Click += new System.EventHandler(this.swapClassesToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(279, 6);
            // 
            // applySelectionToScreenToolStripMenuItem
            // 
            this.applySelectionToScreenToolStripMenuItem.Enabled = false;
            this.applySelectionToScreenToolStripMenuItem.Name = "applySelectionToScreenToolStripMenuItem";
            this.applySelectionToScreenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.applySelectionToScreenToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.applySelectionToScreenToolStripMenuItem.Text = "Apply selection To screen";
            this.applySelectionToScreenToolStripMenuItem.ToolTipText = "Apply the current plate classes to all the rest of the screen";
            this.applySelectionToScreenToolStripMenuItem.Click += new System.EventHandler(this.applySelectionToScreenToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(279, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = global::HCSAnalyzer.Properties.Resources.configure_4;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // switchToDistributionModeToolStripMenuItem
            // 
            this.switchToDistributionModeToolStripMenuItem.CheckOnClick = true;
            this.switchToDistributionModeToolStripMenuItem.Name = "switchToDistributionModeToolStripMenuItem";
            this.switchToDistributionModeToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.switchToDistributionModeToolStripMenuItem.Text = "Distribution Mode";
            this.switchToDistributionModeToolStripMenuItem.Click += new System.EventHandler(this.switchToDistributionModeToolStripMenuItem_Click);
            // 
            // toolStripcomboBoxPlateList
            // 
            this.toolStripcomboBoxPlateList.DropDownWidth = 121;
            this.toolStripcomboBoxPlateList.Name = "toolStripcomboBoxPlateList";
            this.toolStripcomboBoxPlateList.Size = new System.Drawing.Size(400, 23);
            this.toolStripcomboBoxPlateList.SelectedIndexChanged += new System.EventHandler(this.toolStripcomboBoxPlateList_SelectedIndexChanged);
            // 
            // clusteringToolStripMenuItem1
            // 
            this.clusteringToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.visualizationToolStripMenuItem,
            this.qualityControlsToolStripMenuItem1,
            this.dRCAnalysisToolStripMenuItem1});
            this.clusteringToolStripMenuItem1.Name = "clusteringToolStripMenuItem1";
            this.clusteringToolStripMenuItem1.Size = new System.Drawing.Size(45, 23);
            this.clusteringToolStripMenuItem1.Text = "Plate";
            // 
            // visualizationToolStripMenuItem
            // 
            this.visualizationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scatterPointsToolStripMenuItem1,
            this.distributionToolStripMenuItem,
            this.PCAToolStripMenuItem,
            this.lDAToolStripMenuItem,
            this.classificationTreeToolStripMenuItem,
            this.xYScatterPointsToolStripMenuItem,
            this.hierarchicalTreeToolStripMenuItem});
            this.visualizationToolStripMenuItem.Enabled = false;
            this.visualizationToolStripMenuItem.Name = "visualizationToolStripMenuItem";
            this.visualizationToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.visualizationToolStripMenuItem.Text = "Visualization";
            // 
            // scatterPointsToolStripMenuItem1
            // 
            this.scatterPointsToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("scatterPointsToolStripMenuItem1.Image")));
            this.scatterPointsToolStripMenuItem1.Name = "scatterPointsToolStripMenuItem1";
            this.scatterPointsToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.scatterPointsToolStripMenuItem1.Size = new System.Drawing.Size(211, 22);
            this.scatterPointsToolStripMenuItem1.Text = "Scatter Points";
            this.scatterPointsToolStripMenuItem1.Click += new System.EventHandler(this.scatterPointsToolStripMenuItem1_Click);
            // 
            // distributionToolStripMenuItem
            // 
            this.distributionToolStripMenuItem.Name = "distributionToolStripMenuItem";
            this.distributionToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.distributionToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.distributionToolStripMenuItem.Text = "Histogram";
            this.distributionToolStripMenuItem.Click += new System.EventHandler(this.distributionToolStripMenuItem_Click);
            // 
            // PCAToolStripMenuItem
            // 
            this.PCAToolStripMenuItem.Name = "PCAToolStripMenuItem";
            this.PCAToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.PCAToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.PCAToolStripMenuItem.Text = "PCA";
            this.PCAToolStripMenuItem.Click += new System.EventHandler(this.aToolStripMenuItem_Click);
            // 
            // lDAToolStripMenuItem
            // 
            this.lDAToolStripMenuItem.Name = "lDAToolStripMenuItem";
            this.lDAToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.lDAToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.lDAToolStripMenuItem.Text = "LDA";
            this.lDAToolStripMenuItem.Click += new System.EventHandler(this.lDAToolStripMenuItem_Click);
            // 
            // classificationTreeToolStripMenuItem
            // 
            this.classificationTreeToolStripMenuItem.Name = "classificationTreeToolStripMenuItem";
            this.classificationTreeToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.classificationTreeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.classificationTreeToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.classificationTreeToolStripMenuItem.Text = "Classification Tree";
            this.classificationTreeToolStripMenuItem.Click += new System.EventHandler(this.classificationTreeToolStripMenuItem_Click);
            // 
            // xYScatterPointsToolStripMenuItem
            // 
            this.xYScatterPointsToolStripMenuItem.Name = "xYScatterPointsToolStripMenuItem";
            this.xYScatterPointsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.xYScatterPointsToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.xYScatterPointsToolStripMenuItem.Text = "XY Scatter Points";
            this.xYScatterPointsToolStripMenuItem.Click += new System.EventHandler(this.xYScatterPointsToolStripMenuItem_Click);
            // 
            // hierarchicalTreeToolStripMenuItem
            // 
            this.hierarchicalTreeToolStripMenuItem.Name = "hierarchicalTreeToolStripMenuItem";
            this.hierarchicalTreeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.hierarchicalTreeToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.hierarchicalTreeToolStripMenuItem.Text = "Hierarchical Tree";
            this.hierarchicalTreeToolStripMenuItem.Click += new System.EventHandler(this.hierarchicalTreeToolStripMenuItem_Click);
            // 
            // qualityControlsToolStripMenuItem1
            // 
            this.qualityControlsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zscoreSinglePlateToolStripMenuItem,
            this.normalProbabilityPlotToolStripMenuItem,
            this.toolStripMenuItem2,
            this.systematicErrorsToolStripMenuItem1});
            this.qualityControlsToolStripMenuItem1.Enabled = false;
            this.qualityControlsToolStripMenuItem1.Name = "qualityControlsToolStripMenuItem1";
            this.qualityControlsToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.qualityControlsToolStripMenuItem1.Text = "Quality Controls";
            // 
            // zscoreSinglePlateToolStripMenuItem
            // 
            this.zscoreSinglePlateToolStripMenuItem.Name = "zscoreSinglePlateToolStripMenuItem";
            this.zscoreSinglePlateToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.zscoreSinglePlateToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.zscoreSinglePlateToolStripMenuItem.Text = "Z-score";
            this.zscoreSinglePlateToolStripMenuItem.Click += new System.EventHandler(this.zscoreSinglePlateToolStripMenuItem_Click);
            // 
            // normalProbabilityPlotToolStripMenuItem
            // 
            this.normalProbabilityPlotToolStripMenuItem.Name = "normalProbabilityPlotToolStripMenuItem";
            this.normalProbabilityPlotToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.normalProbabilityPlotToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.normalProbabilityPlotToolStripMenuItem.Text = "Normal Probability Plot";
            this.normalProbabilityPlotToolStripMenuItem.Click += new System.EventHandler(this.normalProbabilityPlotToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItem2.Size = new System.Drawing.Size(241, 22);
            this.toolStripMenuItem2.Text = "Correlation Matrix";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // systematicErrorsToolStripMenuItem1
            // 
            this.systematicErrorsToolStripMenuItem1.Name = "systematicErrorsToolStripMenuItem1";
            this.systematicErrorsToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.systematicErrorsToolStripMenuItem1.Size = new System.Drawing.Size(241, 22);
            this.systematicErrorsToolStripMenuItem1.Text = "Systematic Errors";
            this.systematicErrorsToolStripMenuItem1.Click += new System.EventHandler(this.systematicErrorsToolStripMenuItem1_Click);
            // 
            // dRCAnalysisToolStripMenuItem1
            // 
            this.dRCAnalysisToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayDRCToolStripMenuItem,
            this.displayRespondingDRCToolStripMenuItem});
            this.dRCAnalysisToolStripMenuItem1.Enabled = false;
            this.dRCAnalysisToolStripMenuItem1.Name = "dRCAnalysisToolStripMenuItem1";
            this.dRCAnalysisToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.dRCAnalysisToolStripMenuItem1.Text = "DRC Analysis";
            // 
            // displayDRCToolStripMenuItem
            // 
            this.displayDRCToolStripMenuItem.Name = "displayDRCToolStripMenuItem";
            this.displayDRCToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.displayDRCToolStripMenuItem.Text = "Display DRC";
            this.displayDRCToolStripMenuItem.Click += new System.EventHandler(this.displayDRCToolStripMenuItem_Click);
            // 
            // displayRespondingDRCToolStripMenuItem
            // 
            this.displayRespondingDRCToolStripMenuItem.Name = "displayRespondingDRCToolStripMenuItem";
            this.displayRespondingDRCToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.displayRespondingDRCToolStripMenuItem.Text = "Display Responding DRC";
            this.displayRespondingDRCToolStripMenuItem.Click += new System.EventHandler(this.displayRespondingDRCToolStripMenuItem_Click);
            // 
            // screenAnalysisToolStripMenuItem
            // 
            this.screenAnalysisToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.visualizationToolStripMenuItemPCA,
            this.qualityControlToolStripMenuItem,
            this.toolStripSeparator6,
            this.toolStripMenuItemGeneAnalysis,
            this.dRCAnalysisToolStripMenuItem});
            this.screenAnalysisToolStripMenuItem.Name = "screenAnalysisToolStripMenuItem";
            this.screenAnalysisToolStripMenuItem.Size = new System.Drawing.Size(54, 23);
            this.screenAnalysisToolStripMenuItem.Text = "Screen";
            // 
            // visualizationToolStripMenuItemPCA
            // 
            this.visualizationToolStripMenuItemPCA.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scatterPointsToolStripMenuItem,
            this.histogramToolStripMenuItem,
            this.stackedHistogramToolStripMenuItem,
            this.xYScatterPointToolStripMenuItem,
            this.lDAToolStripMenuItem1,
            this.pCAToolStripMenuItem2,
            this.hierarchicalClusteringToolStripMenuItem1});
            this.visualizationToolStripMenuItemPCA.Enabled = false;
            this.visualizationToolStripMenuItemPCA.Name = "visualizationToolStripMenuItemPCA";
            this.visualizationToolStripMenuItemPCA.Size = new System.Drawing.Size(160, 22);
            this.visualizationToolStripMenuItemPCA.Text = "Visualization";
            // 
            // scatterPointsToolStripMenuItem
            // 
            this.scatterPointsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("scatterPointsToolStripMenuItem.Image")));
            this.scatterPointsToolStripMenuItem.Name = "scatterPointsToolStripMenuItem";
            this.scatterPointsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
            this.scatterPointsToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.scatterPointsToolStripMenuItem.Text = "Scatter Points";
            this.scatterPointsToolStripMenuItem.Click += new System.EventHandler(this.scatterPointsToolStripMenuItem_Click);
            // 
            // histogramToolStripMenuItem
            // 
            this.histogramToolStripMenuItem.Name = "histogramToolStripMenuItem";
            this.histogramToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.H)));
            this.histogramToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.histogramToolStripMenuItem.Text = "Histogram";
            this.histogramToolStripMenuItem.Click += new System.EventHandler(this.histogramToolStripMenuItem_Click);
            // 
            // stackedHistogramToolStripMenuItem
            // 
            this.stackedHistogramToolStripMenuItem.Name = "stackedHistogramToolStripMenuItem";
            this.stackedHistogramToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.stackedHistogramToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.stackedHistogramToolStripMenuItem.Text = "Stacked Histogram";
            this.stackedHistogramToolStripMenuItem.Click += new System.EventHandler(this.stackedHistogramToolStripMenuItem_Click);
            // 
            // xYScatterPointToolStripMenuItem
            // 
            this.xYScatterPointToolStripMenuItem.Name = "xYScatterPointToolStripMenuItem";
            this.xYScatterPointToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Y)));
            this.xYScatterPointToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.xYScatterPointToolStripMenuItem.Text = "XY Scatter Points";
            this.xYScatterPointToolStripMenuItem.Click += new System.EventHandler(this.xYScatterPointToolStripMenuItem_Click);
            // 
            // lDAToolStripMenuItem1
            // 
            this.lDAToolStripMenuItem1.Name = "lDAToolStripMenuItem1";
            this.lDAToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.lDAToolStripMenuItem1.Size = new System.Drawing.Size(246, 22);
            this.lDAToolStripMenuItem1.Text = "LDA";
            this.lDAToolStripMenuItem1.Click += new System.EventHandler(this.lDAToolStripMenuItem1_Click);
            // 
            // pCAToolStripMenuItem2
            // 
            this.pCAToolStripMenuItem2.Name = "pCAToolStripMenuItem2";
            this.pCAToolStripMenuItem2.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.pCAToolStripMenuItem2.Size = new System.Drawing.Size(246, 22);
            this.pCAToolStripMenuItem2.Text = "PCA";
            this.pCAToolStripMenuItem2.Click += new System.EventHandler(this.pCAToolStripMenuItem2_Click);
            // 
            // hierarchicalClusteringToolStripMenuItem1
            // 
            this.hierarchicalClusteringToolStripMenuItem1.Name = "hierarchicalClusteringToolStripMenuItem1";
            this.hierarchicalClusteringToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.I)));
            this.hierarchicalClusteringToolStripMenuItem1.Size = new System.Drawing.Size(246, 22);
            this.hierarchicalClusteringToolStripMenuItem1.Text = "Hierarchical Tree";
            this.hierarchicalClusteringToolStripMenuItem1.Click += new System.EventHandler(this.hierarchicalClusteringToolStripMenuItem1_Click);
            // 
            // qualityControlToolStripMenuItem
            // 
            this.qualityControlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zscoreToolStripMenuItem,
            this.sSMDToolStripMenuItem,
            this.normalProbabilityPlotToolStripMenuItem1,
            this.correlationMatrixToolStripMenuItem,
            this.coeffOfVariationEvolutionToolStripMenuItem,
            this.descriptorEvolutionToolStripMenuItem});
            this.qualityControlToolStripMenuItem.Enabled = false;
            this.qualityControlToolStripMenuItem.Name = "qualityControlToolStripMenuItem";
            this.qualityControlToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.qualityControlToolStripMenuItem.Text = "Quality Controls";
            // 
            // zscoreToolStripMenuItem
            // 
            this.zscoreToolStripMenuItem.Name = "zscoreToolStripMenuItem";
            this.zscoreToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Z)));
            this.zscoreToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.zscoreToolStripMenuItem.Text = "Z-score";
            this.zscoreToolStripMenuItem.Click += new System.EventHandler(this.zscoreToolStripMenuItem_Click_1);
            // 
            // sSMDToolStripMenuItem
            // 
            this.sSMDToolStripMenuItem.Name = "sSMDToolStripMenuItem";
            this.sSMDToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.sSMDToolStripMenuItem.Text = "SSMD";
            this.sSMDToolStripMenuItem.Click += new System.EventHandler(this.sSMDToolStripMenuItem_Click);
            // 
            // normalProbabilityPlotToolStripMenuItem1
            // 
            this.normalProbabilityPlotToolStripMenuItem1.Name = "normalProbabilityPlotToolStripMenuItem1";
            this.normalProbabilityPlotToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.normalProbabilityPlotToolStripMenuItem1.Size = new System.Drawing.Size(273, 22);
            this.normalProbabilityPlotToolStripMenuItem1.Text = "Normal Probability Plot";
            this.normalProbabilityPlotToolStripMenuItem1.Click += new System.EventHandler(this.normalProbabilityPlotToolStripMenuItem1_Click);
            // 
            // correlationMatrixToolStripMenuItem
            // 
            this.correlationMatrixToolStripMenuItem.Name = "correlationMatrixToolStripMenuItem";
            this.correlationMatrixToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.M)));
            this.correlationMatrixToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.correlationMatrixToolStripMenuItem.Text = "Correlation Matrix";
            this.correlationMatrixToolStripMenuItem.Click += new System.EventHandler(this.correlationMatrixToolStripMenuItem_Click);
            // 
            // coeffOfVariationEvolutionToolStripMenuItem
            // 
            this.coeffOfVariationEvolutionToolStripMenuItem.Name = "coeffOfVariationEvolutionToolStripMenuItem";
            this.coeffOfVariationEvolutionToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.coeffOfVariationEvolutionToolStripMenuItem.Text = "Coeff. of Variation Evolution";
            this.coeffOfVariationEvolutionToolStripMenuItem.Click += new System.EventHandler(this.coeffOfVariationEvolutionToolStripMenuItem_Click);
            // 
            // descriptorEvolutionToolStripMenuItem
            // 
            this.descriptorEvolutionToolStripMenuItem.Name = "descriptorEvolutionToolStripMenuItem";
            this.descriptorEvolutionToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.descriptorEvolutionToolStripMenuItem.Text = "Descriptor Evolution";
            this.descriptorEvolutionToolStripMenuItem.Click += new System.EventHandler(this.descriptorEvolutionToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(157, 6);
            // 
            // toolStripMenuItemGeneAnalysis
            // 
            this.toolStripMenuItemGeneAnalysis.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findGeneToolStripMenuItem,
            this.pahtwaysAnalysisToolStripMenuItem});
            this.toolStripMenuItemGeneAnalysis.Enabled = false;
            this.toolStripMenuItemGeneAnalysis.Name = "toolStripMenuItemGeneAnalysis";
            this.toolStripMenuItemGeneAnalysis.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItemGeneAnalysis.Text = "Gene Analysis";
            // 
            // findGeneToolStripMenuItem
            // 
            this.findGeneToolStripMenuItem.Name = "findGeneToolStripMenuItem";
            this.findGeneToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F)));
            this.findGeneToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.findGeneToolStripMenuItem.Text = "Find Gene";
            this.findGeneToolStripMenuItem.Click += new System.EventHandler(this.findGeneToolStripMenuItem_Click);
            // 
            // pahtwaysAnalysisToolStripMenuItem
            // 
            this.pahtwaysAnalysisToolStripMenuItem.Name = "pahtwaysAnalysisToolStripMenuItem";
            this.pahtwaysAnalysisToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.pahtwaysAnalysisToolStripMenuItem.Text = "Pathways analysis";
            this.pahtwaysAnalysisToolStripMenuItem.Click += new System.EventHandler(this.pahtwaysAnalysisToolStripMenuItem_Click);
            // 
            // dRCAnalysisToolStripMenuItem
            // 
            this.dRCAnalysisToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.convertDRCToWellToolStripMenuItem});
            this.dRCAnalysisToolStripMenuItem.Enabled = false;
            this.dRCAnalysisToolStripMenuItem.Name = "dRCAnalysisToolStripMenuItem";
            this.dRCAnalysisToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.dRCAnalysisToolStripMenuItem.Text = "DRC Analysis";
            // 
            // convertDRCToWellToolStripMenuItem
            // 
            this.convertDRCToWellToolStripMenuItem.Name = "convertDRCToWellToolStripMenuItem";
            this.convertDRCToWellToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.convertDRCToWellToolStripMenuItem.Text = "Convert DRC to Well";
            this.convertDRCToWellToolStripMenuItem.Click += new System.EventHandler(this.convertDRCToWellToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consoleToolStripMenuItem,
            this.platesManagerToolStripMenuItem,
            this.doseResponseManagerToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // consoleToolStripMenuItem
            // 
            this.consoleToolStripMenuItem.CheckOnClick = true;
            this.consoleToolStripMenuItem.Image = global::HCSAnalyzer.Properties.Resources.format_justify_fill;
            this.consoleToolStripMenuItem.Name = "consoleToolStripMenuItem";
            this.consoleToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.C)));
            this.consoleToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.consoleToolStripMenuItem.Text = "Console";
            this.consoleToolStripMenuItem.Click += new System.EventHandler(this.consoleToolStripMenuItem_Click);
            // 
            // platesManagerToolStripMenuItem
            // 
            this.platesManagerToolStripMenuItem.Enabled = false;
            this.platesManagerToolStripMenuItem.Name = "platesManagerToolStripMenuItem";
            this.platesManagerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.P)));
            this.platesManagerToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.platesManagerToolStripMenuItem.Text = "Plates manager";
            this.platesManagerToolStripMenuItem.Click += new System.EventHandler(this.platesManagerToolStripMenuItem_Click);
            // 
            // doseResponseManagerToolStripMenuItem
            // 
            this.doseResponseManagerToolStripMenuItem.Enabled = false;
            this.doseResponseManagerToolStripMenuItem.Name = "doseResponseManagerToolStripMenuItem";
            this.doseResponseManagerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D)));
            this.doseResponseManagerToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.doseResponseManagerToolStripMenuItem.Text = "Dose Response Designer";
            this.doseResponseManagerToolStripMenuItem.Click += new System.EventHandler(this.doseResponseManagerToolStripMenuItem_Click);
            // 
            // pluginsToolStripMenuItem
            // 
            this.pluginsToolStripMenuItem.Enabled = false;
            this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
            this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(63, 23);
            this.pluginsToolStripMenuItem.Text = "Plug-ins";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutHCSAnalyzerToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.aboutToolStripMenuItem.Text = "Help";
            // 
            // aboutHCSAnalyzerToolStripMenuItem
            // 
            this.aboutHCSAnalyzerToolStripMenuItem.Image = global::HCSAnalyzer.Properties.Resources.help_about;
            this.aboutHCSAnalyzerToolStripMenuItem.Name = "aboutHCSAnalyzerToolStripMenuItem";
            this.aboutHCSAnalyzerToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.aboutHCSAnalyzerToolStripMenuItem.Text = "About HCS analyzer";
            this.aboutHCSAnalyzerToolStripMenuItem.Click += new System.EventHandler(this.aboutHCSAnalyzerToolStripMenuItem_Click);
            // 
            // checkedListBoxActiveDescriptors
            // 
            this.checkedListBoxActiveDescriptors.AllowDrop = true;
            this.checkedListBoxActiveDescriptors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxActiveDescriptors.CheckOnClick = true;
            this.checkedListBoxActiveDescriptors.FormattingEnabled = true;
            this.checkedListBoxActiveDescriptors.Location = new System.Drawing.Point(3, 123);
            this.checkedListBoxActiveDescriptors.Name = "checkedListBoxActiveDescriptors";
            this.checkedListBoxActiveDescriptors.Size = new System.Drawing.Size(173, 364);
            this.checkedListBoxActiveDescriptors.TabIndex = 8;
            this.checkedListBoxActiveDescriptors.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxDescriptorActive_SelectedIndexChanged);
            this.checkedListBoxActiveDescriptors.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkedListBoxActiveDescriptors_MouseDown);
            // 
            // comboBoxDescriptorToDisplay
            // 
            this.comboBoxDescriptorToDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDescriptorToDisplay.FormattingEnabled = true;
            this.comboBoxDescriptorToDisplay.Location = new System.Drawing.Point(3, 70);
            this.comboBoxDescriptorToDisplay.Name = "comboBoxDescriptorToDisplay";
            this.comboBoxDescriptorToDisplay.Size = new System.Drawing.Size(173, 21);
            this.comboBoxDescriptorToDisplay.TabIndex = 9;
            this.comboBoxDescriptorToDisplay.SelectedIndexChanged += new System.EventHandler(this.comboBoxDescriptorToDisplay_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Current Descriptor";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(0, 35);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControlMain);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.checkedListBoxActiveDescriptors);
            this.splitContainer2.Panel2.Controls.Add(this.label8);
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Panel2.Controls.Add(this.comboBoxDescriptorToDisplay);
            this.splitContainer2.Panel2.Controls.Add(this.comboBoxClass);
            this.splitContainer2.Panel2.Controls.Add(this.labelNumClasses);
            this.splitContainer2.Size = new System.Drawing.Size(1375, 494);
            this.splitContainer2.SplitterDistance = 1192;
            this.splitContainer2.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 104);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Descriptor List";
            // 
            // contextMenuStripForLUT
            // 
            this.contextMenuStripForLUT.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStripForLUT.Name = "contextMenuStripForLUT";
            this.contextMenuStripForLUT.Size = new System.Drawing.Size(170, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(169, 22);
            this.toolStripMenuItem1.Text = "Copy to clipboard";
            // 
            // HCSAnalyzer
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1379, 530);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.menuStripFile);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStripFile;
            this.Name = "HCSAnalyzer";
            this.Text = "HCS analyzer v1.0.4.1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HCSAnalyzer_FormClosed);
            this.Load += new System.EventHandler(this.HCSAnalyzer_Load);
            this.Shown += new System.EventHandler(this.HCSAnalyzer_Shown);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.panelForPlate_MouseWheel);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageDistribution.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelForPlate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.tabPageDImRed.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBoxUnsupervised.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNewDimension)).EndInit();
            this.groupBoxSupervised.ResumeLayout(false);
            this.groupBoxSupervised.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.tabPageQualityQtrl.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRejectionThreshold)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForQualityControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.tabPageNormalization.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.tabPageClassification.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClusterNumber)).EndInit();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.tabPageExport.ResumeLayout(false);
            this.tabPageExport.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            this.menuStripFile.ResumeLayout(false);
            this.menuStripFile.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuStripForLUT.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageDistribution;
        private System.Windows.Forms.MenuStrip menuStripFile;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadScreenToolStripMenuItem;
        private System.Windows.Forms.Button buttonSizeIncrease;
        private System.Windows.Forms.Button buttonSizeDecrease;
        private System.Windows.Forms.CheckedListBox checkedListBoxActiveDescriptors;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem currentPlateTomtrToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clusteringToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copyAverageValuesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyAverageValuesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copyClassesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem screenAnalysisToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxClass;
        private System.Windows.Forms.Button buttonGlobalSelection;
        private System.Windows.Forms.CheckBox checkBoxApplyToAllPlates;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem platesManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label labelNumClasses;
        private System.Windows.Forms.ToolStripMenuItem qualityControlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sSMDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zscoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveScreentoCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGeneAnalysis;
        private System.Windows.Forms.ToolStripMenuItem pahtwaysAnalysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem qualityControlsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem zscoreSinglePlateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalProbabilityPlotToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem applySelectionToScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systematicErrorsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem visualizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PCAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem distributionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scatterPointsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutHCSAnalyzerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findGeneToolStripMenuItem;
        public System.Windows.Forms.Panel panelForLUT;
        public System.Windows.Forms.Label labelMax;
        public System.Windows.Forms.Label labelMin;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem lDAToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPageNormalization;
        private System.Windows.Forms.TabPage tabPageClassification;
        private System.Windows.Forms.TabPage tabPageExport;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panelForPlate;
        private System.Windows.Forms.Button buttonCorrectionPlateByPlate;
        private System.Windows.Forms.Button buttonNormalize;
        private System.Windows.Forms.Button buttonStartClassification;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ToolStripMenuItem linkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem descriptorEvolutionToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPageQualityQtrl;
        private System.Windows.Forms.ComboBox comboBoxDescriptorToDisplay;
        private System.Windows.Forms.CheckBox checkBoxDisplayClasses;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButtonClassifPlateByPlate;
        private System.Windows.Forms.Button buttonQualityControl;
        private System.Windows.Forms.ToolStripMenuItem appendDescriptorsToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridViewForQualityControl;
        private System.Windows.Forms.Button buttonGlobalOnlySelected;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RichTextBox richTextBoxForScreeningInformation;
        private System.Windows.Forms.ToolStripMenuItem classificationTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem normalProbabilityPlotToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem correlationMatrixToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxExportPlateFormat;
        private System.Windows.Forms.CheckBox checkBoxExportFullScreen;
        private System.Windows.Forms.CheckBox checkBoxExportScreeningInformation;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.ImageList imageListForTab;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem scatterPointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visualizationToolStripMenuItemPCA;
        private System.Windows.Forms.ToolStripMenuItem histogramToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonReduceDim;
        private System.Windows.Forms.GroupBox groupBoxUnsupervised;
        private System.Windows.Forms.NumericUpDown numericUpDownNewDimension;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPageDImRed;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.RichTextBox richTextBoxInfoClustering;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.RichTextBox richTextBoxInfoClassif;
        private System.Windows.Forms.Button buttonCluster;
        private System.Windows.Forms.ComboBox comboBoxClusteringMethod;
        private System.Windows.Forms.ComboBox comboBoxCLassificationMethod;
        private System.Windows.Forms.RadioButton radioButtonClassifGlobal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBoxSupervised;
        private System.Windows.Forms.ComboBox comboBoxReduceDimMultiClass;
        private System.Windows.Forms.ComboBox comboBoxReduceDimSingleClass;
        private System.Windows.Forms.RichTextBox richTextBoxSupervisedDimRec;
        private System.Windows.Forms.RichTextBox richTextBoxUnsupervisedDimRec;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radioButtonDimRedSupervised;
        private System.Windows.Forms.RadioButton radioButtonDimRedUnsupervised;
        private System.Windows.Forms.ComboBox comboBoxDimReductionNeutralClass;
        private System.Windows.Forms.ComboBox comboBoxNeutralClassForClassif;
        private System.Windows.Forms.ComboBox comboBoxMethodForCorrection;
        private System.Windows.Forms.RichTextBox richTextBoxInformationForPlateCorrection;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.RichTextBox richTextBoxInfoForNormalization;
        private System.Windows.Forms.ComboBox comboBoxMethodForNormalization;
        private System.Windows.Forms.ComboBox comboBoxNormalizationPositiveCtrl;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxNormalizationNegativeCtrl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.PictureBox pictureBox8;
        private Label label8;
        private ToolStripMenuItem swapClassesToolStripMenuItem;
        public ToolStripComboBox toolStripcomboBoxPlateList;
        private Button buttonRejectPlates;
        private GroupBox groupBox1;
        private RichTextBox richTextBoxInformationRejection;
        private ComboBox comboBoxRejection;
        private NumericUpDown numericUpDownRejectionThreshold;
        private Label label9;
        private TreeView treeViewSelectionForExport;
        private ToolStripMenuItem generateScreenToolStripMenuItem1;
        private ToolStripMenuItem univariateToolStripMenuItem;
        private ToolStripMenuItem multivariateToolStripMenuItem;
        private ToolStripMenuItem stackedHistogramToolStripMenuItem;
        private ToolStripMenuItem xYScatterPointsToolStripMenuItem;
        private ToolStripMenuItem xYScatterPointToolStripMenuItem;
        private ToolStripMenuItem coeffOfVariationEvolutionToolStripMenuItem;
        private ContextMenuStrip contextMenuStripForLUT;
        private ToolStripMenuItem toolStripMenuItem1;
        private RadioButton radioButtonClusterFullScreen;
        private RadioButton radioButtonClusterPlateByPlate;
        private Label label10;
        private CheckBox checkBoxAutomatedClusterNumber;
        private NumericUpDown numericUpDownClusterNumber;
        private ToolStripMenuItem lDAToolStripMenuItem1;
        private ToolStripMenuItem pCAToolStripMenuItem2;
        private ToolStripMenuItem pluginsToolStripMenuItem;
        private ToolStripMenuItem hierarchicalClusteringToolStripMenuItem1;
        private ToolStripMenuItem hierarchicalTreeToolStripMenuItem;
        private ToolStripMenuItem toARFFToolStripMenuItem;
        private ToolStripMenuItem doseResponseManagerToolStripMenuItem;
        private ToolStripMenuItem dRCAnalysisToolStripMenuItem;
        private ToolStripMenuItem convertDRCToWellToolStripMenuItem;
        private ToolStripMenuItem dRCAnalysisToolStripMenuItem1;
        private ToolStripMenuItem displayRespondingDRCToolStripMenuItem;
        private ToolStripMenuItem displayDRCToolStripMenuItem;
        private ToolStripMenuItem switchToDistributionModeToolStripMenuItem;
        private ComboBox comboBoxRejectionPositiveCtrl;
        private Label label11;
        private ComboBox comboBoxRejectionNegativeCtrl;
        private Label label12;
    }
}

