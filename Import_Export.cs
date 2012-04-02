using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using LibPlateAnalysis;
using weka.core;
using System.IO;

using System.Data.OleDb;
using System.Drawing.Imaging;
using HCSAnalyzer.Forms;
using HCSAnalyzer.Classes;
using weka.core.converters;
using System.Diagnostics;

namespace HCSAnalyzer
{
    public partial class HCSAnalyzer
    {
        #region User Interface

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;
            int NumDesc = CompleteScreening.ListDescriptors.Count;

            FolderBrowserDialog OpenFolderDialog = new FolderBrowserDialog();
            DialogResult result = OpenFolderDialog.ShowDialog();
            if (result != DialogResult.OK) return;
            string PathName = OpenFolderDialog.SelectedPath;

            this.Cursor = Cursors.WaitCursor;
            // save a CSV file containing all the plates and chosen descriptors
            if (checkBoxExportFullScreen.Checked)
            {
                bool res = DisplayDescriptorsToSave(PathName + "\\fullScreen.csv");
                if (res == false) return;
            }
            // save a CSV file containting the plate designs
            ExportToCSV(PathName, true/*checkBoxExportToCSVIncludeDescriptors.Checked*/, checkBoxExportPlateFormat.Checked, false, /*checkBoxExportToCSVIncludeName.Checked*/ true, /*checkBoxExportToCSVIncludeInfo.Checked*/ true);

            // save the report
            if (checkBoxExportScreeningInformation.Checked)
                richTextBoxForScreeningInformation.SaveFile(PathName + "\\Information.rtf");


            if (treeViewSelectionForExport.Nodes["NodeClassification"].Nodes["NodeClassifTree"].Checked)
            {
                for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
                {
                    cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);
                    Microsoft.Msagl.Drawing.Graph CurrentGraph = DisplayTheGraph(CurrentPlateToProcess);
                    if (CurrentGraph == null) continue;

                    Microsoft.Msagl.GraphViewerGdi.GraphRenderer renderer = new Microsoft.Msagl.GraphViewerGdi.GraphRenderer(CurrentGraph);
                    renderer.CalculateLayout();

                    Bitmap bitmap = new Bitmap(800, 800, PixelFormat.Format32bppPArgb);
                    renderer.Render(bitmap);

                    string CorrectedName = CheckAndCorrectFilemName(CurrentPlateToProcess.Name, false);
                    bitmap.Save(PathName + "\\" + CorrectedName + ".png", ImageFormat.Png);

                    //MemoryStream ms = new MemoryStream();
                    //this.chartForSimpleForm.SaveImage(ms, ChartImageFormat.Bmp);
                    //Bitmap bm = new Bitmap(ms);
                    //Clipboard.SetImage(bitmap);
                    //CompleteScreening.CurrentRichTextBox.Paste();
                }
            }

            if (treeViewSelectionForExport.Nodes["NodeQualityControl"].Nodes["NodeSystematicError"].Checked)
            {
                DataTable ResultSystematicError = ComputeSystematicErrorsTable();
                dataGridViewForQualityControl.DataSource = ResultSystematicError;
                dataGridViewForQualityControl.Update();

                DataGridfViewToCsV(dataGridViewForQualityControl, PathName + "\\SystematicErrorReport.csv");
            }

            if (treeViewSelectionForExport.Nodes["NodeQualityControl"].Nodes["NodeZfactor"].Checked)
            {
                RichTextBox NewBox = new RichTextBox();
                int IdxDesc = CompleteScreening.ListDescriptors.Count;
                int TmpMaxWidth = (int)GlobalInfo.OptionsWindow.numericUpDownMaximumWidth.Value;
                GlobalInfo.OptionsWindow.numericUpDownMaximumWidth.Value = 700;
                // loop on all the desciptors
                for (int Desc = 0; Desc < NumDesc; Desc++)
                {
                    if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;
                    MemoryStream ms = new MemoryStream();
                    SimpleForm GraphForm = BuildZFactor(Desc);

                    foreach (DataPoint CurrentPt in GraphForm.chartForSimpleForm.Series[0].Points)
                        CurrentPt.Label = "";

                    GraphForm.chartForSimpleForm.SaveImage(ms, ChartImageFormat.Bmp);
                    Bitmap bm = new Bitmap(ms);
                    Clipboard.SetImage(bm);
                    NewBox.Paste();
                    NewBox.AppendText("\n");

                    NewBox.AppendText(GraphForm.GetValues());

                }

                NewBox.SaveFile(PathName + "\\ZFactors.rtf");
                GlobalInfo.OptionsWindow.numericUpDownMaximumWidth.Value = TmpMaxWidth;
            }


            if (treeViewSelectionForExport.Nodes["NodeQualityControl"].Nodes["NodeCorrelationMatRank"].Checked)
                ComputeAndDisplayCorrelationMatrix(true, false, PathName + "\\Correlation");

            if (treeViewSelectionForExport.Nodes["NodesiRNA"].Nodes["NodePathwayAnalysis"].Checked)
            {
                int Class = 1;
                PathWayAnalysis(Class).chartForPie.SaveImage(PathName + "\\PathWayAnalysis.emf", ChartImageFormat.Emf);
            }


            if (treeViewSelectionForExport.Nodes["NodeMisc"].Nodes["NodeWekaArff"].Checked)
            {
                cInfoClass InfoClass = CompleteScreening.GetNumberOfClassesBut(-1);
                Instances insts = CompleteScreening.CreateInstancesWithClasses(InfoClass, -1);
                ArffSaver saver = new ArffSaver();
                CSVSaver savercsv = new CSVSaver();
                saver.setInstances(insts);
                saver.setFile(new java.io.File(PathName + "\\data.arff"));
                saver.writeBatch();
            }
            this.Cursor = Cursors.Default;

            Process.Start(PathName);

            MessageBox.Show("Export done !");
        }

        /// <summary>
        /// Save screen to MTR format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void currentPlateTomtrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            SaveFileDialog CurrSaveFileDialog = new SaveFileDialog();
            CurrSaveFileDialog.Filter = "mtr files (*.mtr)|*.mtr|text files (*.txt)|*.txt";
            System.Windows.Forms.DialogResult Res = CurrSaveFileDialog.ShowDialog();
            if (Res != System.Windows.Forms.DialogResult.OK) return;
            string PathName = CurrSaveFileDialog.FileName;

            if (PathName == "") return;

            StreamWriter stream = new StreamWriter(PathName, true, System.Text.Encoding.ASCII);
            int iValue = CompleteScreening.GetNumberOfActivePlates();
            stream.Write(iValue.ToString() + " ");
            iValue = CompleteScreening.Rows;
            stream.Write(iValue.ToString() + " ");
            iValue = CompleteScreening.Columns;
            stream.Write(iValue.ToString() + "\r\n");

            for (int Col = 0; Col < CompleteScreening.Columns; Col++)
            {
                for (int Row = 0; Row < CompleteScreening.Rows; Row++)
                {
                    for (int SeqPos = 0; SeqPos < CompleteScreening.GetNumberOfActivePlates(); SeqPos++)
                    {
                        cPlate CurrentPlate = CompleteScreening.ListPlatesActive.GetPlate(SeqPos);
                        cWell TmpWell = CurrentPlate.GetWell(Col, Row, false);
                        double Value = -1;
                        if (TmpWell != null)
                            Value = TmpWell.GetAverageValuesList()[comboBoxDescriptorToDisplay.SelectedIndex];
                        stream.Write(Value.ToString() + "\t");

                    }
                    stream.Write("\r\n");
                }
            }
            stream.Dispose();
        }

        private void appendAssayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;
            OpenFileDialog CurrOpenFileDialog = new OpenFileDialog();

            CurrOpenFileDialog.Filter = "csv files (*.csv)|*.csv";
            CurrOpenFileDialog.Multiselect = true;
            DialogResult Res = CurrOpenFileDialog.ShowDialog();
            if (Res != DialogResult.OK) return;

            if (CurrOpenFileDialog.FileNames == null) return;

            //if (CurrOpenFileDialog.FileNames[0].Remove(0, CurrOpenFileDialog.FileNames[0].Length - 4) == ".mtr") LoadMTRAssay(CurrOpenFileDialog);
            //if (CurrOpenFileDialog.FileNames[0].Remove(0, CurrOpenFileDialog.FileNames[0].Length - 4) == ".txt") LoadTXTAssay(CurrOpenFileDialog);
            if (CurrOpenFileDialog.FileNames[0].Remove(0, CurrOpenFileDialog.FileNames[0].Length - 4) == ".csv") LoadCSVAssay(CurrOpenFileDialog.FileNames, true);
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog CurrOpenFileDialog = new OpenFileDialog();
            CurrOpenFileDialog.Filter = "csv files (*.csv)|*.csv|txt files (*.txt)|*.txt|mtr files (*.mtr)|*.mtr";
            CurrOpenFileDialog.Multiselect = true;



            DialogResult Res = CurrOpenFileDialog.ShowDialog();
            if (Res != DialogResult.OK) return;

            if (CurrOpenFileDialog.FileNames == null) return;

            bool ResultLoading = false;

            if (CurrOpenFileDialog.FileNames[0].Remove(0, CurrOpenFileDialog.FileNames[0].Length - 4) == ".mtr")
            {
                LoadMTRAssay(CurrOpenFileDialog);
                ResultLoading = true;
            }
            if (CurrOpenFileDialog.FileNames[0].Remove(0, CurrOpenFileDialog.FileNames[0].Length - 4) == ".txt")
            {
                LoadTXTAssay(CurrOpenFileDialog);
                ResultLoading = true;
            }
            if (CurrOpenFileDialog.FileNames[0].Remove(0, CurrOpenFileDialog.FileNames[0].Length - 4) == ".csv")
            {
                if(LoadCSVAssay(CurrOpenFileDialog.FileNames, false)==false) return;
                
            }


            UpdateUIAfterLoading();


        }

        private void loadScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog OpenFolderDialog = new FolderBrowserDialog();

            DialogResult result = OpenFolderDialog.ShowDialog();
            if (result == DialogResult.OK)
            {



                string Path = OpenFolderDialog.SelectedPath;
                if (CompleteScreening != null) CompleteScreening.Close3DView();


                GlobalInfo = new cGlobalInfo(CompleteScreening);
                GlobalInfo.OptionsWindow.Visible = false;
                GlobalInfo.ComboForSelectedDesc = this.comboBoxDescriptorToDisplay;
                GlobalInfo.CheckedListBoxForDescActive = this.checkedListBoxActiveDescriptors;

                //new Kitware.VTK.RenderWindowControl novelRender = new RenderWindowControl

                GlobalInfo.renderWindowControlForVTK = null;//renderWindowControlForVTK;


                MyConsole = new FormConsole();
                MyConsole.Visible = false;

                PlateListWindow = new PlatesListForm();
                GlobalInfo.PlateListWindow = PlateListWindow;

                GlobalInfo.panelForPlate = this.panelForPlate;

                comboBoxClass.SelectedIndex = 1;






                CompleteScreening = new cScreening("My first Screening", GlobalInfo);

                CompleteScreening.ListDescriptors.Clean();

                FormForPlateDimensions PlateDim = new FormForPlateDimensions();
                if (PlateDim.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                CompleteScreening.LoadData(Path, (int)PlateDim.numericUpDownColumns.Value, (int)PlateDim.numericUpDownRows.Value);

            

                if (CompleteScreening.GetNumberOfOriginalPlates() == 0)
                {
                    GlobalInfo.ConsoleWriteLine("No plate loaded !");
                    return;
                }

                for (int IdxPlate = 0; IdxPlate < CompleteScreening.GetNumberOfOriginalPlates(); IdxPlate++)
                {
                    string Name = CompleteScreening.ListPlatesActive.GetPlate(IdxPlate).Name;
                    this.toolStripcomboBoxPlateList.Items.Add(Name);
                    PlateListWindow.listBoxPlateNameToProcess.Items.Add(Name);
                    PlateListWindow.listBoxAvaliableListPlates.Items.Add(Name);

                }

                UpdateUIAfterLoading();

                StartingUpDateUI();

                CompleteScreening.CurrentDisplayPlateIdx = 0;
                //checkedListBoxActiveDescriptors.Items.Clear();
                CompleteScreening.ListDescriptors.CurrentSelectedDescriptor = 0;



             //   this.toolStripcomboBoxPlateList.SelectedIndex = CompleteScreening.CurrentDisplayPlateIdx;
          //      comboBoxDescriptorToDisplay.SelectedIndex = CompleteScreening.ListDescriptors.CurrentSelectedDescriptor;
                CompleteScreening.SetSelectionType(comboBoxClass.SelectedIndex - 1);
                CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.CurrentDisplayPlateIdx).DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, true);

            //    this.toolStripcomboBoxPlateList.SelectedIndex = CompleteScreening.CurrentDisplayPlateIdx;
            }
        }

        #endregion

        #region CSV functions
        private class CsvRow : List<string>
        {
            public string LineText { get; set; }
        }

        private class CsvFileReader : StreamReader
        {
            public CsvFileReader(Stream stream)
                : base(stream)
            {
            }

            public CsvFileReader(string filename)
                : base(filename)
            {
            }

            /// <summary>
            /// Reads a row of data from a CSV file
            /// </summary>
            /// <param name="row"></param>
            /// <returns></returns>
            public bool ReadRow(CsvRow row)
            {

                row.LineText = ReadLine();
                if (String.IsNullOrEmpty(row.LineText))
                    return false;

                int pos = 0;
                int rows = 0;

                while (pos < row.LineText.Length)
                {
                    string value;

                    // Special handling for quoted field
                    if (row.LineText[pos] == '"')
                    {
                        // Skip initial quote
                        pos++;

                        // Parse quoted value
                        int start = pos;
                        while (pos < row.LineText.Length)
                        {
                            // Test for quote character
                            if (row.LineText[pos] == '"')
                            {
                                // Found one
                                pos++;

                                // If two quotes together, keep one
                                // Otherwise, indicates end of value
                                if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                                {
                                    pos--;
                                    break;
                                }
                            }
                            pos++;
                        }
                        value = row.LineText.Substring(start, pos - start);
                        value = value.Replace("\"\"", "\"");
                    }
                    else
                    {
                        // Parse unquoted value
                        int start = pos;
                        while (pos < row.LineText.Length && row.LineText[pos] != ',')
                            pos++;
                        value = row.LineText.Substring(start, pos - start);
                    }

                    // Add field to list
                    if (rows < row.Count)
                        row[rows] = value;
                    else
                        row.Add(value);
                    rows++;

                    // Eat up to and including next comma
                    while (pos < row.LineText.Length && row.LineText[pos] != ',')
                        pos++;
                    if (pos < row.LineText.Length)
                        pos++;
                }
                // Delete any unused items
                while (row.Count > rows)
                    row.RemoveAt(rows);

                // Return true if any columns read
                //return (row.Count > 0);
                return true;
            }
        }

        private int[] ConvertPosition(string PosString)
        {
            int[] Pos = new int[2];

            Pos[1] = Convert.ToInt16(PosString[0]) - 64;
            string PosY = PosString.Remove(0, 1);
            bool IsTrue = int.TryParse(PosY, out Pos[0]);

            if (!IsTrue) return null;

            return Pos;
        }

        private string ConvertPosition(int PosX, int PosY)
        {
            char character = (char)(PosY + 64);
            string ToReturn = character.ToString() + PosX.ToString("00");
            return ToReturn;
        }

        private bool LoadCSVAssay(string[] FileNames, bool IsAppend)
        {
            if (CompleteScreening == null) IsAppend = false;

            PathNames = FileNames;
            if (IsAppend == false)
            { 
                if (CompleteScreening != null) CompleteScreening.Close3DView();
                CompleteScreening = new cScreening("CSV imported Screening", GlobalInfo);
               
            }
            //CompleteScreening = new cScreening("MTR imported Screening", GlobalInfo);
            StartingUpDateUI();
            //PathNames = CurrOpenFileDialog.FileNames;

            if (PathNames == null) return false;
            // Window form creation
            FromExcel = new FormForImportExcel();
            if (IsAppend)
            {
                FromExcel.numericUpDownColumns.Value = (decimal)CompleteScreening.Columns;
                FromExcel.numericUpDownColumns.ReadOnly = true;
                FromExcel.numericUpDownRows.Value = (decimal)CompleteScreening.Rows;
                FromExcel.numericUpDownRows.ReadOnly = true;
            }

            FromExcel.IsImportCSV = true;


            int Mode = 2;
            if (GlobalInfo.OptionsWindow.radioButtonWellPosModeSingle.Checked) Mode = 1;
            CsvFileReader CSVsr = new CsvFileReader(PathNames[0]);

            CsvRow Names = new CsvRow();
            if (!CSVsr.ReadRow(Names))
            {
                CSVsr.Close();
                return false;
            }

            int NumPreview = 4;
            List<CsvRow> LCSVRow = new List<CsvRow>();
            for (int Idx = 0; Idx < NumPreview; Idx++)
            {
                CsvRow TNames = new CsvRow();
                if (!CSVsr.ReadRow(TNames))
                {
                    CSVsr.Close();
                    return false;
                }
                LCSVRow.Add(TNames);
            }

            // FromExcel.dataGridViewForImport.RowsDefaultCellStyle.BackColor = Color.Bisque;
            FromExcel.dataGridViewForImport.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;


            DataGridViewColumn ColName = new DataGridViewColumn();
            FromExcel.dataGridViewForImport.Columns.Add("Data Name", "Data Name");

            DataGridViewCheckBoxColumn columnSelection = new DataGridViewCheckBoxColumn();
            columnSelection.Name = "Selection";
            FromExcel.dataGridViewForImport.Columns.Add(columnSelection);

            DataGridViewComboBoxColumn columnType = new DataGridViewComboBoxColumn();
            if (GlobalInfo.OptionsWindow.radioButtonWellPosModeDouble.Checked)
                columnType.DataSource = new string[] { "Plate name", "Column", "Row", "Class", "Name", "Locus ID", "Concentration", "Info", "Descriptor" };
            else
                columnType.DataSource = new string[] { "Plate name", "Well position", "Class", "Name", "Locus ID", "Concentration", "Info", "Descriptor" };
            columnType.Name = "Type";
            FromExcel.dataGridViewForImport.Columns.Add(columnType);

            for (int i = 0; i < LCSVRow.Count; i++)
            {
                DataGridViewColumn NewCol = new DataGridViewColumn();
                NewCol.ReadOnly = true;
                FromExcel.dataGridViewForImport.Columns.Add("Readout " + i, "Readout " + i);
            }



            if (LCSVRow[0].Count > Names.Count)
            {
                CSVsr.Close();
                MessageBox.Show("Inconsistent column number. Check your CSV file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;

            }

            for (int i = 0; i < Names.Count; i++)
            {
                int IdxRow = 0;
                FromExcel.dataGridViewForImport.Rows.Add();
                FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = Names[i];

                if (i == 0) FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = true;
                else if (i == 1) FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = true;
                else if ((i == 2) && (GlobalInfo.OptionsWindow.radioButtonWellPosModeDouble.Checked)) FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = true;
                else FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = false;

                if (i == 0)
                {
                    FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = "Plate name";
                }
                else if (i == 1)
                {
                    if (GlobalInfo.OptionsWindow.radioButtonWellPosModeDouble.Checked)
                    {
                        FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = "Column";
                    }
                    else
                    {
                        FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = "Well position";
                    }
                }
                else if (i == 2)
                {
                    if (GlobalInfo.OptionsWindow.radioButtonWellPosModeDouble.Checked)
                    {
                        FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = "Row";
                    }
                    else
                        FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = "Descriptor";
                }
                else
                {
                    FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = "Descriptor";
                }

                for (int j = 0; j < LCSVRow.Count; j++)
                {
                    if(i<LCSVRow[j].Count)
                        FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow + j].Value = LCSVRow[j][i].ToString();
                    else
                        FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow + j].Value = "";
                }
            }

            FromExcel.dataGridViewForImport.Update();
            FromExcel.dataGridViewForImport.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewForImport_MouseClick);

            FromExcel.CurrentScreen = CompleteScreening;
            FromExcel.thisHCSAnalyzer = this;
            FromExcel.IsAppend = IsAppend;

            if (FromExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK) return true;
            else return false;
        }

        public void LoadingProcedureForCSVImport(bool IsAppend)
        {
            int Mode = 2;
            if (GlobalInfo.OptionsWindow.radioButtonWellPosModeSingle.Checked) Mode = 1;
            CsvFileReader CSVsr = new CsvFileReader(PathNames[0]);

            CsvRow OriginalNames = new CsvRow();
            if (!CSVsr.ReadRow(OriginalNames))
            {
                CSVsr.Close();
                return;
            }
            int ColSelectedForName = GetColIdxFor("Name");
            int ColLocusID = GetColIdxFor("Locus ID");
            int ColConcentration = GetColIdxFor("Concentration");
            int ColInfo = GetColIdxFor("Info");
            int ColClass = GetColIdxFor("Class");
            int ColPlateName = GetColIdxFor("Plate name");
            int ColCol = GetColIdxFor("Column");
            int ColRow = GetColIdxFor("Row");
            int ColWellPos = GetColIdxFor("Well position");
            int[] ColsForDescriptors = GetColsIdxFor("Descriptor");

            int WellLoaded = 0;
            int FailToLoad = 0;


            if (!IsAppend)
            {
                CompleteScreening.Columns = (int)FromExcel.numericUpDownColumns.Value;
                CompleteScreening.Rows = (int)FromExcel.numericUpDownRows.Value;
                CompleteScreening.ListDescriptors.Clean();
            }
            int ShiftIdx = CompleteScreening.ListDescriptors.Count;


            for (int IdxFile = 0; IdxFile < PathNames.Length; IdxFile++)
            {
                string CurrentFileName = PathNames[IdxFile];

                CSVsr = new CsvFileReader(CurrentFileName);

                CsvRow Names = new CsvRow();

                #region Check the validity of the header
                if (!CSVsr.ReadRow(Names))
                {
                    CSVsr.Close();
                    GlobalInfo.ConsoleWriteLine(CurrentFileName + ": file opening failed.");
                    goto NEXTFILE;
                }

                if (Names.Count != OriginalNames.Count)
                {
                    CSVsr.Close();
                    GlobalInfo.ConsoleWriteLine(CurrentFileName + ": file opening failed.");
                    goto NEXTFILE;
                }
                for (int IdxName = 0; IdxName < Names.Count; IdxName++)
                {

                    if (Names[IdxName] != OriginalNames[IdxName])
                    {
                        CSVsr.Close();
                        GlobalInfo.ConsoleWriteLine(CurrentFileName + ": file opening failed.");
                        goto NEXTFILE;
                    }
                }
                #endregion


                if (!IsAppend)
                {
                    for (int idxDesc = 0; idxDesc < ColsForDescriptors.Length; idxDesc++)
                        CompleteScreening.ListDescriptors.AddNew(new cDescriptorsType(Names[ColsForDescriptors[idxDesc]], true, 1));
                }

                while (CSVsr.EndOfStream != true)
                {
                NEXT: ;
                    CsvRow CurrentDesc = new CsvRow();
                    if (CSVsr.ReadRow(CurrentDesc) == false) break;

                    string PlateName = CurrentDesc[ColPlateName];
                    //return;
                    // check if the plate exist already
                    cPlate CurrentPlate = CompleteScreening.GetPlateIfNameIsContainIn(PlateName);
                    if (CurrentPlate == null)
                    {
                        CurrentPlate = new cPlate("Cpds", PlateName, CompleteScreening);
                        CompleteScreening.AddPlate(CurrentPlate);
                    }

                    int[] Pos = new int[2];
                    if (Mode == 1)
                    {
                        Pos = ConvertPosition(CurrentDesc[ColWellPos]);
                        if (Pos == null)
                        {
                            if (MessageBox.Show("Error in converting the current well position.\nGo to Edit->Options->Import-Export->Well Position Mode to fix this.\nDo you want continue ?", "Loading error !", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.No)
                            {
                                CSVsr.Close();
                                return;
                            }
                            else
                                goto NEXTLOOP;
                        }
                    }
                    else
                    {
                      
                        Pos[1] = Convert.ToInt16(CurrentDesc[ColRow]);
                    }


                    List<cDescriptor> LDesc = new List<cDescriptor>();
                    for (int idxDesc = 0; idxDesc < ColsForDescriptors.Length; idxDesc++)
                    {
                        double Value;
                        if ((double.TryParse(CurrentDesc[ColsForDescriptors[idxDesc]], out Value)) && (!double.IsNaN(Value)))
                        {
                            cDescriptor CurrentDescriptor = new cDescriptor(Value, CompleteScreening.ListDescriptors[idxDesc/* + ShiftIdx*/],CompleteScreening);
                            LDesc.Add(CurrentDescriptor);
                        }
                        else
                        {
                            FailToLoad++;
                            goto NEXTLOOP;
                        }
                    }
                    if ((Pos[0] > CompleteScreening.Columns) || (Pos[1] > CompleteScreening.Rows))
                    {
                        if (MessageBox.Show("Well position exceeds plate dimensions.\nDo you want continue ?", "Loading error !", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.No)
                        {
                            CSVsr.Close();
                            return;
                        }
                        else
                            goto NEXTLOOP;
                    }


                    cWell CurrentWell = new cWell(LDesc, Pos[0], Pos[1], CompleteScreening, CurrentPlate);


                    CurrentPlate.AddWell(CurrentWell);
                    WellLoaded++;

                    if ((ColSelectedForName != -1)&&(ColSelectedForName<CurrentDesc.Count)) CurrentWell.Name = CurrentDesc[ColSelectedForName];


                    if (ColLocusID != -1)
                    {
                        double CurrentValue;

                        if (!double.TryParse(CurrentDesc[ColLocusID], out CurrentValue))
                            goto NEXTSTEP;

                        CurrentWell.LocusID = CurrentValue;

                    }
                    if (ColConcentration != -1)
                    {
                        double CurrentValue;

                        if (!double.TryParse(CurrentDesc[ColConcentration], out CurrentValue))
                            goto NEXTSTEP;

                      
                        CurrentWell.Concentration = CurrentValue;

                    }
                NEXTSTEP: ;

                if ((ColInfo != -1) && (ColInfo < CurrentDesc.Count))
                        CurrentWell.Info = CurrentDesc[ColInfo];

                    if (ColClass != -1)
                    {
                        int CurrentValue;
                        if (!int.TryParse(CurrentDesc[ColClass], out CurrentValue))
                            goto NEXTLOOP;
                        CurrentWell.SetClass(CurrentValue);
                    }

                NEXTLOOP: ;

                }
            NEXTFILE: ;
                CSVsr.Close();
            }
            CompleteScreening.UpDatePlateListWithFullAvailablePlate();

            FromExcel.Dispose();

            MessageBox.Show("CSV file loaded:\n" + WellLoaded + " well(s) loaded\n" + FailToLoad + " well(s) rejected.", "Process finished !", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.toolStripcomboBoxPlateList.Items.Clear();
            for (int IdxPlate = 0; IdxPlate < CompleteScreening.GetNumberOfOriginalPlates(); IdxPlate++)
            {
                string Name = CompleteScreening.ListPlatesActive.GetPlate(IdxPlate).Name;
                this.toolStripcomboBoxPlateList.Items.Add(Name);
                PlateListWindow.listBoxPlateNameToProcess.Items.Add(Name);
                PlateListWindow.listBoxAvaliableListPlates.Items.Add(Name);
            }
            UpdateUIAfterLoading();
        //    CompleteScreening.CurrentDisplayPlateIdx = 0;
            CompleteScreening.SetSelectionType(comboBoxClass.SelectedIndex - 1);

           // toolStripcomboBoxPlateList.SelectedIndex = 0;

            CompleteScreening.ListDescriptors.SetCurrentSelectedDescriptor(0);
            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(0, true);
            return;
        }

        #endregion

        #region Link To Data

        FormForImportExcel FromExcel;
        string[] PathNames;

        private void dataGridViewForImport_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks != 1) return;
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                ToolStripMenuItem ToolStripMenuItem_Info = new ToolStripMenuItem("Select All");

                ToolStripMenuItem ToolStripMenuItem_Kegg = new ToolStripMenuItem("Unselect All");
                contextMenuStrip.Items.AddRange(new ToolStripItem[] { ToolStripMenuItem_Info, ToolStripMenuItem_Kegg });

                contextMenuStrip.Show(Control.MousePosition);

                ToolStripMenuItem_Info.Click += new System.EventHandler(this.SelectAll);
                ToolStripMenuItem_Kegg.Click += new System.EventHandler(this.UnselectAll);

            }

        }

        private void SelectAll(object sender, EventArgs e)
        {
            for (int i = 0; i < FromExcel.dataGridViewForImport.Rows.Count; i++)
                FromExcel.dataGridViewForImport.Rows[i].Cells[1].Value = true;
        }

        private void UnselectAll(object sender, EventArgs e)
        {
            for (int i = 0; i < FromExcel.dataGridViewForImport.Rows.Count; i++)
                FromExcel.dataGridViewForImport.Rows[i].Cells[1].Value = false;
        }

        private void linkToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (CompleteScreening == null) return;

            OpenFileDialog CurrOpenFileDialog = new OpenFileDialog();
            CurrOpenFileDialog.Filter = "csv files (*.csv)|*.csv";
            System.Windows.Forms.DialogResult Res = CurrOpenFileDialog.ShowDialog();
            if (Res != System.Windows.Forms.DialogResult.OK) return;
            PathNames = CurrOpenFileDialog.FileNames;

            if (PathNames == null) return;

            // Window form creation
            FromExcel = new FormForImportExcel();
            FromExcel.numericUpDownColumns.Value = (decimal)CompleteScreening.Columns;
            FromExcel.numericUpDownColumns.ReadOnly = true;
            FromExcel.numericUpDownRows.Value = (decimal)CompleteScreening.Rows;
            FromExcel.numericUpDownRows.ReadOnly = true;

            int Mode = 2;
            if (GlobalInfo.OptionsWindow.radioButtonWellPosModeSingle.Checked) Mode = 1;



            CsvFileReader CSVsr = new CsvFileReader(PathNames[0]);

            CsvRow Names = new CsvRow();
            if (!CSVsr.ReadRow(Names))
            {
                CSVsr.Close();
                return;
            }

            int NumPreview = 4;
            List<CsvRow> LCSVRow = new List<CsvRow>();
            for (int Idx = 0; Idx < NumPreview; Idx++)
            {
                CsvRow TNames = new CsvRow();
                if (!CSVsr.ReadRow(TNames))
                {
                    CSVsr.Close();
                    return;
                }
                LCSVRow.Add(TNames);
            }

            //for (int i = 0; i < Names.Count; i++)
            //{
            //    FromExcel.comboBoxClass.Items.Add(Names[i]);
            //    FromExcel.comboBoxLocusID.Items.Add(Names[i]);
            //    FromExcel.comboBoxInfo.Items.Add(Names[i]);
            //}

            DataGridViewColumn ColName = new DataGridViewColumn();
            FromExcel.dataGridViewForImport.Columns.Add("Data Name", "Data Name");

            DataGridViewCheckBoxColumn columnSelection = new DataGridViewCheckBoxColumn();
            columnSelection.Name = "Selection";
            FromExcel.dataGridViewForImport.Columns.Add(columnSelection);

            DataGridViewComboBoxColumn columnType = new DataGridViewComboBoxColumn();
            if (CompleteScreening.GlobalInfo.OptionsWindow.radioButtonWellPosModeDouble.Checked)
                columnType.DataSource = new string[] { "Plate name", "Column", "Row", "Class", "Name", "Locus ID", "Concentration", "Info" };
            else
                columnType.DataSource = new string[] { "Plate name", "Well position", "Class", "Name", "Locus ID", "Concentration", "Info" };
            columnType.Name = "Type";
            FromExcel.dataGridViewForImport.Columns.Add(columnType);

            for (int i = 0; i < LCSVRow.Count; i++)
            {
                DataGridViewColumn NewCol = new DataGridViewColumn();
                NewCol.ReadOnly = true;
                FromExcel.dataGridViewForImport.Columns.Add("Readout " + i, "Readout " + i);
            }


            for (int i = 0; i < Names.Count; i++)
            {
                int IdxRow = 0;
                FromExcel.dataGridViewForImport.Rows.Add();
                FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = Names[i];

                if (i == 0) FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = true;
                else if (i == 1) FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = true;
                else if ((i == 2) && (CompleteScreening.GlobalInfo.OptionsWindow.radioButtonWellPosModeDouble.Checked)) FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = true;
                else FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = false;

                if (i == 0)
                {
                    FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = "Plate name";
                }
                else if (i == 1)
                {
                    if (CompleteScreening.GlobalInfo.OptionsWindow.radioButtonWellPosModeDouble.Checked)
                    {
                        FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = "Column";
                    }
                    else
                    {
                        FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = "Well position";
                    }
                }
                else if (i == 2)
                {
                    if (CompleteScreening.GlobalInfo.OptionsWindow.radioButtonWellPosModeDouble.Checked)
                    {
                        FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = "Row";
                    }
                    else
                        FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = "Name";
                }
                else
                {
                    FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow++].Value = "Name";
                }

                for (int j = 0; j < LCSVRow.Count; j++)
                    FromExcel.dataGridViewForImport.Rows[i].Cells[IdxRow + j].Value = LCSVRow[j][i].ToString();
            }

            FromExcel.dataGridViewForImport.Update();
            FromExcel.dataGridViewForImport.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewForImport_MouseClick);
            FromExcel.CurrentScreen = CompleteScreening;
            FromExcel.thisHCSAnalyzer = this;

            FromExcel.ShowDialog();
        }

        private int GetColIdxFor(string StringToBeDetected)
        {
            int Pos = -1;

            for (int i = 0; i < FromExcel.dataGridViewForImport.Rows.Count; i++)
            {
                string CurrentVal = FromExcel.dataGridViewForImport.Rows[i].Cells[2].Value.ToString();
                bool IsSelected = (bool)FromExcel.dataGridViewForImport.Rows[i].Cells[1].Value;

                if ((CurrentVal == StringToBeDetected) && (IsSelected == true)) Pos = i;
            }

            return Pos;
        }

        private int[] GetColsIdxFor(string StringToBeDetected)
        {
            List<int> Pos = new List<int>();
            for (int i = 0; i < FromExcel.dataGridViewForImport.Rows.Count; i++)
            {
                string CurrentVal = FromExcel.dataGridViewForImport.Rows[i].Cells[2].Value.ToString();
                bool IsSelected = (bool)FromExcel.dataGridViewForImport.Rows[i].Cells[1].Value;

                if ((CurrentVal == StringToBeDetected) && (IsSelected == true))
                    Pos.Add(i);
            }

            return Pos.ToArray();
        }

        public void LoadingProcedure()
        {
            int Mode = 2;
            if (GlobalInfo.OptionsWindow.radioButtonWellPosModeSingle.Checked) Mode = 1;
            CsvFileReader CSVsr = new CsvFileReader(PathNames[0]);
            if (!CSVsr.BaseStream.CanRead)
            {
                MessageBox.Show("Cannot read the file !", "Process finished !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CSVsr.Close();
                return;
            }
            CsvRow Names = new CsvRow();
            if (!CSVsr.ReadRow(Names))
            {
                CSVsr.Close();
                return;
            }
            int ColSelectedForName = GetColIdxFor("Name");
            int ColLocusID = GetColIdxFor("Locus ID");
            int ColConcentration = GetColIdxFor("Concentration");
            int ColInfo = GetColIdxFor("Info");
            int ColClass = GetColIdxFor("Class");
            int ColPlateName = GetColIdxFor("Plate name");
            int ColCol = GetColIdxFor("Column");
            int ColRow = GetColIdxFor("Row");
            int ColWellPos = GetColIdxFor("Well position");


            while (CSVsr.EndOfStream != true)
            {
            NEXT: ;
                CsvRow CurrentDesc = new CsvRow();
                if (CSVsr.ReadRow(CurrentDesc) == false) break;

                string PlateName = CurrentDesc[ColPlateName];
                //return;
                // check if the plate exist already
                cPlate CurrentPlate = CompleteScreening.GetPlateIfNameIsContainIn(PlateName);
                if (CurrentPlate == null) goto NEXT;

                int[] Pos = new int[2];
                if (Mode == 1)
                {
                    Pos = ConvertPosition(CurrentDesc[ColWellPos]);
                }
                else
                {
                    if (!int.TryParse(CurrentDesc[ColCol], out Pos[0]))
                    {
                        if (MessageBox.Show("Error in converting the current well position.\nGo to Edit->Options->Import-Export->Well Position Mode to fix this.\nDo you want continue ?", "Loading error !", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.No)
                        {
                            CSVsr.Close();
                            return;
                        }
                        else
                            goto NEXTLOOP;
                    }

                    Pos[1] = Convert.ToInt16(CurrentDesc[ColRow]);
                }

                cWell CurrentWell = CurrentPlate.GetWell(Pos[0] - 1, Pos[1] - 1, false);
                if (CurrentWell == null) goto NEXT;

                if (ColSelectedForName != -1)
                {
                    CurrentWell.Name = CurrentDesc[ColSelectedForName];
                }
                else
                {
                    CurrentWell.SetAsNoneSelected();
                }

                if (ColLocusID != -1)
                {
                    double CurrentValue;

                    if (!double.TryParse(CurrentDesc[ColLocusID], out CurrentValue))
                        goto NEXTSTEP;

                    CurrentWell.LocusID = CurrentValue;

                }
                if (ColConcentration != -1)
                {
                    double CurrentValue;

                    if (!double.TryParse(CurrentDesc[ColConcentration], out CurrentValue))
                        goto NEXTSTEP;


                    CurrentWell.Concentration =  CurrentValue;

                }
            NEXTSTEP: ;

                if (ColInfo != -1)
                    CurrentWell.Info = CurrentDesc[ColInfo];

                if (ColClass != -1)
                {
                    int CurrentValue;
                    if (!int.TryParse(CurrentDesc[ColClass], out CurrentValue))
                        goto NEXTLOOP;
                    CurrentWell.SetClass(CurrentValue);
                }

            NEXTLOOP: ;

            }

            CSVsr.Close();

            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, false);

            MessageBox.Show("File loaded", "Process finished !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        #endregion

        #region export to CSV
        private void toARFFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            SaveFileDialog CurrSaveFileDialog = new SaveFileDialog();
            CurrSaveFileDialog.Filter = "arff files (*.arff)|*.arff";
            System.Windows.Forms.DialogResult Res = CurrSaveFileDialog.ShowDialog();
            if (Res != System.Windows.Forms.DialogResult.OK) return;
            string PathName = CurrSaveFileDialog.FileName;

            if (PathName == "") return;

            cInfoClass InfoClass = CompleteScreening.GetNumberOfClassesBut(-1);
            Instances insts = CompleteScreening.CreateInstancesWithClasses(InfoClass, -1);
            ArffSaver saver = new ArffSaver();
            CSVSaver savercsv = new CSVSaver();
            saver.setInstances(insts);
            saver.setFile(new java.io.File(PathName));
            saver.writeBatch();

        }




        public string CheckAndCorrectFilemName(string FileName, bool IsWarn)
        {
            string[] ListChr = new string[] { "?", ">", "<", "*", ":", "|", "/", "\\" };

            bool Error = false;
            for (int i = 0; i < ListChr.Length; i++)
            {
                if (FileName.Contains(ListChr[i]))
                {
                    Error = true;
                    break;
                }
            }

            if (Error)
            {
                if (IsWarn) MessageBox.Show("The plate name contains character(s) that cannot be used !\nThey will be replaced by: _", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                for (int i = 0; i < ListChr.Length; i++)
                {
                    string TmpName = FileName.Replace(ListChr[i], "_");
                    FileName = TmpName;
                }
            }

            return FileName;
        }

        private void DataGridfViewToCsV(DataGridView dGV, string filename)
        {
            using (StreamWriter myFile = new StreamWriter(filename, false, Encoding.Default))
            {
                // Export titles:  
                string sHeaders = "";
                for (int j = 0; j < dGV.Columns.Count; j++) { sHeaders = sHeaders.ToString() + dGV.Columns[j].HeaderText + ","; }
                myFile.WriteLine(sHeaders);

                // Export data.  
                for (int i = 0; i < dGV.RowCount - 1; i++)
                {
                    string stLine = "";
                    for (int j = 0; j < dGV.Rows[i].Cells.Count; j++) { stLine = stLine.ToString() + dGV.Rows[i].Cells[j].Value + ","; }
                    myFile.WriteLine(stLine);
                }
                myFile.Close();
            }

        }

        /// <summary>
        /// Save screen to CSV file format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            //int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;
            //int NumDesc = CompleteScreening.ListDescriptors.Count;

            SaveFileDialog CurrSaveFileDialog = new SaveFileDialog();
            CurrSaveFileDialog.Filter = "csv files (*.csv)|*.csv";
            System.Windows.Forms.DialogResult Res = CurrSaveFileDialog.ShowDialog();
            if (Res != System.Windows.Forms.DialogResult.OK) return;
            string CurrentPathforCSV = CurrSaveFileDialog.FileName;


            DisplayDescriptorsToSave(CurrentPathforCSV);

            MessageBox.Show("CSV file saved !");


        }

        private bool DisplayDescriptorsToSave(string CurrentPathforCSV)
        {
            // Window form creation
            FormSaveScreening FormToSave = new FormSaveScreening();
            FormToSave.Name = "Save to CSV";

            int Mode = 2;
            if (GlobalInfo.OptionsWindow.radioButtonWellPosModeSingle.Checked) Mode = 1;

            DataGridViewColumn ColName = new DataGridViewColumn();
            FormToSave.dataGridView.Columns.Add("Data Name", "Data Name");

            DataGridViewCheckBoxColumn columnSelection = new DataGridViewCheckBoxColumn();
            columnSelection.Name = "Selection";
            FormToSave.dataGridView.Columns.Add(columnSelection);

            DataGridViewColumn ColExample = new DataGridViewColumn();
            FormToSave.dataGridView.Columns.Add("Well 0", "Well 0");

            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.Font = new Font(FormToSave.dataGridView.Font, FontStyle.Bold);

            FormToSave.dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;



            int RowIdx = 0;
            FormToSave.dataGridView.Rows.Add();
            FormToSave.dataGridView.Rows[RowIdx].Cells[0].Value = "Plate Name";
            FormToSave.dataGridView.Rows[RowIdx].Cells[1].Value = true;
            FormToSave.dataGridView.Rows[RowIdx++].DefaultCellStyle = style;

            if (Mode == 1)
            {
                FormToSave.dataGridView.Rows.Add();
                FormToSave.dataGridView.Rows[RowIdx].Cells[0].Value = "Well Position";
                FormToSave.dataGridView.Rows[RowIdx].Cells[1].Value = true;
                FormToSave.dataGridView.Rows[RowIdx++].DefaultCellStyle = style;
            }
            else
            {
                FormToSave.dataGridView.Rows.Add();
                FormToSave.dataGridView.Rows[RowIdx].Cells[0].Value = "Column";
                FormToSave.dataGridView.Rows[RowIdx].Cells[1].Value = true;
                FormToSave.dataGridView.Rows[RowIdx++].DefaultCellStyle = style;

                FormToSave.dataGridView.Rows.Add();
                FormToSave.dataGridView.Rows[RowIdx].Cells[0].Value = "Row";
                FormToSave.dataGridView.Rows[RowIdx].Cells[1].Value = true;
                FormToSave.dataGridView.Rows[RowIdx++].DefaultCellStyle = style;
            }

            FormToSave.dataGridView.Rows.Add();
            FormToSave.dataGridView.Rows[RowIdx].Cells[0].Value = "Name";
            FormToSave.dataGridView.Rows[RowIdx].Cells[1].Value = true;
            FormToSave.dataGridView.Rows[RowIdx++].DefaultCellStyle = style;

            FormToSave.dataGridView.Rows.Add();
            FormToSave.dataGridView.Rows[RowIdx].Cells[0].Value = "Class";
            FormToSave.dataGridView.Rows[RowIdx].Cells[1].Value = true;
            FormToSave.dataGridView.Rows[RowIdx++].DefaultCellStyle = style;

            FormToSave.dataGridView.Rows.Add();
            FormToSave.dataGridView.Rows[RowIdx].Cells[0].Value = "Locus ID";
            FormToSave.dataGridView.Rows[RowIdx].Cells[1].Value = true;
            FormToSave.dataGridView.Rows[RowIdx++].DefaultCellStyle = style;

            FormToSave.dataGridView.Rows.Add();
            FormToSave.dataGridView.Rows[RowIdx].Cells[0].Value = "Concentration";
            FormToSave.dataGridView.Rows[RowIdx].Cells[1].Value = true;
            FormToSave.dataGridView.Rows[RowIdx++].DefaultCellStyle = style;

            FormToSave.dataGridView.Rows.Add();
            FormToSave.dataGridView.Rows[RowIdx].Cells[0].Value = "Info";
            FormToSave.dataGridView.Rows[RowIdx].Cells[1].Value = true;
            FormToSave.dataGridView.Rows[RowIdx++].DefaultCellStyle = style;

            int NumDescriptor = CompleteScreening.ListDescriptors.Count;

            for (int PlateIdx = 0; PlateIdx < CompleteScreening.ListPlatesActive.Count; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(PlateIdx);

                for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                    for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                    {
                        cWell TmpWell = CurrentPlateToProcess.GetWell(IdxValue, IdxValue0, true);
                        if (TmpWell == null) continue;

                        RowIdx = 0;

                        FormToSave.dataGridView.Rows[RowIdx++].Cells[2].Value = CurrentPlateToProcess.Name;
                        if (Mode == 1)
                        {
                            string PosString = ConvertPosition(TmpWell.GetPosX(), TmpWell.GetPosY());
                            FormToSave.dataGridView.Rows[RowIdx++].Cells[2].Value = PosString;
                        }
                        else
                        {
                            FormToSave.dataGridView.Rows[RowIdx++].Cells[2].Value = TmpWell.GetPosX();
                            FormToSave.dataGridView.Rows[RowIdx++].Cells[2].Value = TmpWell.GetPosY();
                        }

                        FormToSave.dataGridView.Rows[RowIdx++].Cells[2].Value = TmpWell.Name;

                        FormToSave.dataGridView.Rows[RowIdx++].Cells[2].Value = TmpWell.GetClass();

                        if (TmpWell.LocusID == -1)
                            FormToSave.dataGridView.Rows[RowIdx++].Cells[2].Value = "";
                        else
                            FormToSave.dataGridView.Rows[RowIdx++].Cells[2].Value = TmpWell.LocusID;
                                          
                        if (TmpWell.Concentration == null)
                            FormToSave.dataGridView.Rows[RowIdx++].Cells[2].Value = "";
                        else
                            FormToSave.dataGridView.Rows[RowIdx++].Cells[2].Value = TmpWell.Concentration;   
                        
                        FormToSave.dataGridView.Rows[RowIdx++].Cells[2].Value = TmpWell.Info;

                        for (int N = 0; N < NumDescriptor; N++)
                        {
                            FormToSave.dataGridView.Rows.Add();
                            FormToSave.dataGridView.Rows[N + RowIdx].Cells[0].Value = CompleteScreening.ListDescriptors[N].GetName();
                            FormToSave.dataGridView.Rows[N + RowIdx].Cells[2].Value = TmpWell.ListDescriptors[N].GetValue();

                            if (CompleteScreening.ListDescriptors[N].IsActive())
                                FormToSave.dataGridView.Rows[N + RowIdx].Cells[1].Value = true;
                            else
                                FormToSave.dataGridView.Rows[N + RowIdx].Cells[1].Value = false;
                        }

                        goto NEXTSTEP;
                    }
            }

        NEXTSTEP: ;
            FormToSave.dataGridView.Update();

            if (FormToSave.ShowDialog() != System.Windows.Forms.DialogResult.OK) return false;
            ExportToCSV(CurrentPathforCSV, FormToSave.dataGridView);

            return true;

        }

        private void ExportToCSV(string PathName, DataGridView GridView)
        {
            DataGridView GridToSave = new DataGridView();

            for (int Row = 0; Row < GridView.RowCount; Row++)
            {
                if ((bool)GridView.Rows[Row].Cells[1].Value == true)
                {
                    string NameCol = (string)GridView.Rows[Row].Cells[0].Value;
                    GridToSave.Columns.Add(NameCol, NameCol);
                }
            }

            int NumDescriptor = CompleteScreening.ListDescriptors.Count;
            int RowPos = 0;

            for (int PlateIdx = 0; PlateIdx < CompleteScreening.ListPlatesActive.Count; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(PlateIdx);

                for (int IdxValue = 0; IdxValue < CompleteScreening.Columns; IdxValue++)
                    for (int IdxValue0 = 0; IdxValue0 < CompleteScreening.Rows; IdxValue0++)
                    {
                        cWell TmpWell = CurrentPlateToProcess.GetWell(IdxValue, IdxValue0, true);
                        if (TmpWell == null) continue;

                        GridToSave.Rows.Add();

                        int ColPos = 0;
                        int RealPos = 0;

                        if ((bool)GridView.Rows[RealPos++].Cells[1].Value)
                            GridToSave.Rows[RowPos].Cells[ColPos++].Value = CurrentPlateToProcess.Name;

                        if (GlobalInfo.OptionsWindow.radioButtonWellPosModeSingle.Checked)
                        {
                            if ((bool)GridView.Rows[RealPos++].Cells[1].Value)
                                GridToSave.Rows[RowPos].Cells[ColPos++].Value = ConvertPosition(TmpWell.GetPosX(), TmpWell.GetPosY());
                        }
                        else
                        {
                            if ((bool)GridView.Rows[RealPos++].Cells[1].Value)
                            {
                                GridToSave.Rows[RowPos].Cells[ColPos++].Value = TmpWell.GetPosX();
                            }
                            if ((bool)GridView.Rows[RealPos++].Cells[1].Value)
                            {
                                GridToSave.Rows[RowPos].Cells[ColPos++].Value = TmpWell.GetPosY();
                            }
                        }

                        if ((bool)GridView.Rows[RealPos++].Cells[1].Value)
                            GridToSave.Rows[RowPos].Cells[ColPos++].Value = TmpWell.Name;

                        if ((bool)GridView.Rows[RealPos++].Cells[1].Value)
                            GridToSave.Rows[RowPos].Cells[ColPos++].Value = TmpWell.GetClass();

                        if ((bool)GridView.Rows[RealPos++].Cells[1].Value)
                        {
                            int LID = (int)TmpWell.LocusID;
                            if (LID == -1)
                                GridToSave.Rows[RowPos].Cells[ColPos++].Value = "";
                            else
                                GridToSave.Rows[RowPos].Cells[ColPos++].Value = LID;
                        }

                        if ((bool)GridView.Rows[RealPos++].Cells[1].Value)
                            GridToSave.Rows[RowPos].Cells[ColPos++].Value = (string)TmpWell.Info;


                        if ((bool)GridView.Rows[RealPos++].Cells[1].Value)
                            GridToSave.Rows[RowPos].Cells[ColPos++].Value = TmpWell.Concentration;


                        for (int N = 0; N < NumDescriptor; N++)
                        {
                            if ((bool)GridView.Rows[RealPos + N].Cells[1].Value)
                                GridToSave.Rows[RowPos].Cells[ColPos++].Value = TmpWell.ListDescriptors[N].GetValue();
                        }
                        RowPos++;
                    }
            }
            DataGridfViewToCsV(GridToSave, PathName);
        }

        private void ExportToCSV(string PathName, bool IsAddDescriptors, bool IsPlateMode, bool IsFullScreen, bool IsIncludeNames, bool IsIncludeInfo)
        {
            if (CompleteScreening == null) return;

            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;
            int NumDesc = CompleteScreening.ListDescriptors.Count;
            bool FirstWarning = true;


            if (IsPlateMode)
            {
                // loop on all the plate
                for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
                {
                    cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);

                    // string CorrectedName = CurrentPlateToProcess.Name.Replace("/", "_");
                    //CorrectedName = CurrentPlateToProcess.Name.Replace("/", "_");

                    string CorrectedName = CheckAndCorrectFilemName(CurrentPlateToProcess.Name, FirstWarning);
                    if (CorrectedName != CurrentPlateToProcess.Name)
                    {
                        FirstWarning = false;
                    }
                    StreamWriter stream = new StreamWriter(PathName + "\\" + CorrectedName + ".csv", false, System.Text.Encoding.ASCII);

                    string FirstLine = "Class";
                    for (int Col = 1; Col <= CompleteScreening.Columns; Col++)
                        FirstLine += "," + Col;
                    stream.WriteLine(FirstLine);

                    for (int Row = 0; Row < CompleteScreening.Rows; Row++)
                    {
                        byte[] strArray = new byte[1];
                        strArray[0] = (byte)(Row + 65);
                        string Chara = Encoding.UTF7.GetString(strArray);
                        string CurrentLine = Chara;

                        for (int Col = 0; Col < CompleteScreening.Columns; Col++)
                        {
                            cWell TmpWell = CurrentPlateToProcess.GetWell(Col, Row, true);

                            if (TmpWell == null)
                                CurrentLine += ",";
                            else
                                CurrentLine += "," + TmpWell.GetClass();
                        }
                        stream.WriteLine(CurrentLine);
                    }
                    stream.WriteLine("");


                    if (IsIncludeNames)
                    {
                        FirstLine = "Names";
                        for (int Col = 1; Col <= CompleteScreening.Columns; Col++)
                            FirstLine += "," + Col;
                        stream.WriteLine(FirstLine);

                        for (int Row = 0; Row < CompleteScreening.Rows; Row++)
                        {
                            byte[] strArray = new byte[1];
                            strArray[0] = (byte)(Row + 65);
                            string Chara = Encoding.UTF7.GetString(strArray);
                            string CurrentLine = Chara;

                            for (int Col = 0; Col < CompleteScreening.Columns; Col++)
                            {

                                cWell TmpWell = CurrentPlateToProcess.GetWell(Col, Row, true);

                                if (TmpWell == null)
                                    CurrentLine += ",";
                                else
                                    CurrentLine += "," + TmpWell.Name;
                            }
                            stream.WriteLine(CurrentLine);
                        }
                        stream.WriteLine("");
                    }

                    if (IsIncludeInfo)
                    {
                        FirstLine = "Info";
                        for (int Col = 1; Col <= CompleteScreening.Columns; Col++)
                            FirstLine += "," + Col;
                        stream.WriteLine(FirstLine);

                        for (int Row = 0; Row < CompleteScreening.Rows; Row++)
                        {
                            byte[] strArray = new byte[1];
                            strArray[0] = (byte)(Row + 65);
                            string Chara = Encoding.UTF7.GetString(strArray);
                            string CurrentLine = Chara;

                            for (int Col = 0; Col < CompleteScreening.Columns; Col++)
                            {

                                cWell TmpWell = CurrentPlateToProcess.GetWell(Col, Row, true);

                                if (TmpWell == null)
                                    CurrentLine += ",";
                                else
                                    CurrentLine += "," + TmpWell.Info;
                            }
                            stream.WriteLine(CurrentLine);
                        }
                        stream.WriteLine("");
                    }


                    for (int Desc = 0; Desc < NumDesc; Desc++)
                    {
                        if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;

                        FirstLine = CompleteScreening.ListDescriptors[Desc].GetName();
                        for (int Col = 1; Col <= CompleteScreening.Columns; Col++)
                            FirstLine += "," + Col;
                        stream.WriteLine(FirstLine);

                        for (int Row = 0; Row < CompleteScreening.Rows; Row++)
                        {
                            byte[] strArray = new byte[1];
                            strArray[0] = (byte)(Row + 65);
                            string Chara = Encoding.UTF7.GetString(strArray);
                            string CurrentLine = Chara;

                            for (int Col = 0; Col < CompleteScreening.Columns; Col++)
                            {
                                cWell TmpWell = CurrentPlateToProcess.GetWell(Col, Row, true);

                                if (TmpWell == null)
                                    CurrentLine += ",";
                                else
                                    CurrentLine += "," + TmpWell.ListDescriptors[Desc].GetValue();
                            }
                            stream.WriteLine(CurrentLine);
                        }
                        stream.WriteLine("");
                    }
                    stream.Dispose();
                }
            }

            if (IsFullScreen)
            {
                StreamWriter stream = new StreamWriter(PathName + "\\FullScreen.csv", false, System.Text.Encoding.ASCII);
                string TitleLine = "Plate Name,Column,Row,Class";
                if (IsIncludeNames) TitleLine += ",Name";
                if (IsIncludeInfo) TitleLine += ",Info";
                if (IsAddDescriptors)
                {
                    for (int Desc = 0; Desc < NumDesc; Desc++)
                    {
                        if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;
                        else
                            TitleLine += "," + CompleteScreening.ListDescriptors[Desc].GetName();
                    }
                }
                stream.WriteLine(TitleLine);

                // loop on all the plate
                for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
                {
                    cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);

                    for (int Col = 0; Col < CompleteScreening.Columns; Col++)
                        for (int Row = 0; Row < CompleteScreening.Rows; Row++)
                        {
                            cWell TmpWell = CurrentPlateToProcess.GetWell(Col, Row, true);

                            if (TmpWell == null) continue;

                            string CurrentLine = CurrentPlateToProcess.Name + ",";
                            int _Class = TmpWell.GetClass();

                            int _Column = Col + 1;
                            int _Row = Row + 1;

                            CurrentLine += _Column + "," + _Row + "," + _Class;

                            if (IsIncludeNames)
                                CurrentLine += "," + TmpWell.Name;

                            if (IsIncludeInfo)
                                CurrentLine += "," + TmpWell.Info;

                            if (IsAddDescriptors)
                            {
                                for (int Desc = 0; Desc < NumDesc; Desc++)
                                {
                                    if (CompleteScreening.ListDescriptors[Desc].IsActive() == false) continue;
                                    else
                                        CurrentLine += "," + TmpWell.ListDescriptors[Desc].GetValue();
                                }
                            }
                            stream.WriteLine(CurrentLine);
                        }
                }
                stream.Dispose();
            }


        }
        #endregion

        #region import TXT
        private void LoadTXTAssay(OpenFileDialog CurrOpenFileDialog)
        {
            FormForPlateDimensions PlateDim = new FormForPlateDimensions();
            if (PlateDim.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
           
          if (CompleteScreening != null) CompleteScreening.Close3DView();  
            CompleteScreening = new cScreening("TXT imported Screening", GlobalInfo);
            CompleteScreening.ImportFromTXT(CurrOpenFileDialog.FileNames, CurrOpenFileDialog.SafeFileNames, (int)PlateDim.numericUpDownColumns.Value, (int)PlateDim.numericUpDownRows.Value);
            StartingUpDateUI();

            if (CompleteScreening.GetNumberOfOriginalPlates() == 0)
            {
                GlobalInfo.ConsoleWriteLine("No plate loaded !");
                return;
            }

            this.toolStripcomboBoxPlateList.Items.Clear();
            for (int IdxPlate = 0; IdxPlate < CompleteScreening.GetNumberOfActivePlates(); IdxPlate++)
            {
                string Name = CompleteScreening.ListPlatesActive.GetPlate(IdxPlate).Name;
                this.toolStripcomboBoxPlateList.Items.Add(Name);
                PlateListWindow.listBoxPlateNameToProcess.Items.Add(Name);
                PlateListWindow.listBoxAvaliableListPlates.Items.Add(Name);
            }

          //  this.toolStripcomboBoxPlateList.SelectedIndex = CompleteScreening.CurrentDisplayPlateIdx = 0;
            CompleteScreening.SetSelectionType(comboBoxClass.SelectedIndex - 1);

            CompleteScreening.UpDatePlateListWithFullAvailablePlate();
            UpdateUIAfterLoading();
            
           // comboBoxDescriptorToDisplay.SelectedIndex = 0;
          //  CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, true);
        }
        #endregion

        #region import MTR
        private void LoadMTRAssay(OpenFileDialog CurrOpenFileDialog)
        {
            if (CompleteScreening != null) CompleteScreening.Close3DView();

            CompleteScreening = new cScreening("MTR imported Screening", GlobalInfo);
            StartingUpDateUI();

            CompleteScreening.ImportFromMTR(CurrOpenFileDialog.FileNames, CurrOpenFileDialog.SafeFileNames);

            if (CompleteScreening.GetNumberOfActivePlates() == 0)
            {
                GlobalInfo.ConsoleWriteLine("No plate loaded !");
                return;
            }

            this.toolStripcomboBoxPlateList.Items.Clear();
            for (int IdxPlate = 0; IdxPlate < CompleteScreening.GetNumberOfActivePlates(); IdxPlate++)
            {
                string Name = CompleteScreening.ListPlatesActive.GetPlate(IdxPlate).Name;
                this.toolStripcomboBoxPlateList.Items.Add(Name);
                PlateListWindow.listBoxPlateNameToProcess.Items.Add(Name);
                PlateListWindow.listBoxAvaliableListPlates.Items.Add(Name);
                CompleteScreening.ListPlatesActive.GetPlate(IdxPlate).UpDataMinMax();
            }

            CompleteScreening.CurrentDisplayPlateIdx = 0;
            CompleteScreening.SetSelectionType(comboBoxClass.SelectedIndex - 1);
            comboBoxDescriptorToDisplay.SelectedIndex = 0;
            toolStripcomboBoxPlateList.SelectedIndex = 0;

            CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, true);
        }
        #endregion

        #region Generate artificial screening data
        private void univariateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening != null) CompleteScreening.Close3DView();

            CompleteScreening = new cScreening("Generated Screen", GlobalInfo);

            FormForGenerateScreening WindowGenerateScreening = new FormForGenerateScreening(GlobalInfo);
            if (WindowGenerateScreening.ShowDialog() != DialogResult.OK) return;

            int NumPlate = (int)WindowGenerateScreening.numericUpDownPlateNumber.Value;
            int NumRow = (int)WindowGenerateScreening.numericUpDownRows.Value;
            int NumCol = (int)WindowGenerateScreening.numericUpDownColumns.Value;

            double MeanCpds = (double)WindowGenerateScreening.numericUpDownCpdsMean.Value;
            double StdevCpds = (double)WindowGenerateScreening.numericUpDownCpdsStdev.Value;

            double MeanPos = (double)WindowGenerateScreening.numericUpDownPosCtrlMean.Value;
            double StdevPos = (double)WindowGenerateScreening.numericUpDownPosCtrlStdv.Value;

            double MeanNeg = (double)WindowGenerateScreening.numericUpDownNegCtrlMean.Value;
            double StdevNeg = (double)WindowGenerateScreening.numericUpDownNegCtrlStdv.Value;

            CompleteScreening.Rows = NumRow;
            CompleteScreening.Columns = NumCol;

            CompleteScreening.ListPlatesAvailable = new cExtendPlateList();

            // create the descriptor
            CompleteScreening.ListDescriptors.Clean();
            CompleteScreening.ListDescriptors.AddNew(new cDescriptorsType("Descriptor", true, 1));
            CompleteScreening.ListDescriptors.CurrentSelectedDescriptor = 0;


            Random rand = new Random();

            int StepForDiffusion = (int)GlobalInfo.OptionsWindow.numericUpDownGenerateScreenDiffusion.Value;
            double StepForRatioXY = (double)GlobalInfo.OptionsWindow.numericUpDownGenerateScreenRatioXY.Value;
            double StepForNoiseStandardDeviation = (double)GlobalInfo.OptionsWindow.numericUpDownGenerateScreenNoiseStdDev.Value;
            double StepForShiftRow = (double)GlobalInfo.OptionsWindow.numericUpDownGenerateScreenRowEffectShift.Value;

            cEdgeEffect EdgeEffect = null;
            if (WindowGenerateScreening.checkBoxEdgeEffect.Checked)
            {
                if (WindowGenerateScreening.checkBoxEdgeEffectIteration.Checked)
                    EdgeEffect = new cEdgeEffect(CompleteScreening, (int)WindowGenerateScreening.numericUpDownEdgeEffectIteration.Value + 1 + (int)WindowGenerateScreening.numericUpDownPlateNumber.Value * StepForDiffusion);
                else
                    EdgeEffect = new cEdgeEffect(CompleteScreening, (int)WindowGenerateScreening.numericUpDownEdgeEffectIteration.Value + 1);
            }

            for (int IdxPlate = 0; IdxPlate < NumPlate; IdxPlate++)
            {
                string PlateName = "Plate_" + IdxPlate;

                cPlate CurrentPlate = new cPlate("Cpds", PlateName, CompleteScreening);
                // check if the plate exist already
                CompleteScreening.AddPlate(CurrentPlate);

                if (WindowGenerateScreening.checkBoxStandardDeviation.Checked)
                    StdevCpds = (double)WindowGenerateScreening.numericUpDownCpdsStdev.Value + StepForNoiseStandardDeviation * IdxPlate;


                for (int X = 1; X <= NumCol; X++)
                    for (int Y = 1; Y <= NumRow; Y++)
                    {
                        // Create the descriptor list and add it to the well, then add the well
                        List<cDescriptor> LDesc = new List<cDescriptor>();
                        for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                        {
                            cDescriptor Desc = null;

                            double u1 = rand.NextDouble();
                            double u2 = rand.NextDouble();
                            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
                            double randNormal = MeanCpds + StdevCpds * randStdNormal;

                            Desc = new cDescriptor(randNormal, CompleteScreening.ListDescriptors[i],CompleteScreening);
                            LDesc.Add(Desc);
                        }

                        cWell CurrentWell = new cWell(LDesc, X, Y, CompleteScreening, CurrentPlate);
                        CurrentWell.Name = "Cpds";
                        CurrentPlate.AddWell(CurrentWell);
                    }


                if (WindowGenerateScreening.checkBoxPositiveCtrl.Checked)
                {
                    for (int Y = 0; Y < NumRow; Y++)
                    {
                        cWell CurrentWell = CurrentPlate.GetWell((int)WindowGenerateScreening.numericUpDownColPosCtrl.Value, Y, false);
                        CurrentWell.Name = "Positive Ctrl";
                        CurrentWell.SetClass(0);

                        for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                        {
                            double u1 = rand.NextDouble();
                            double u2 = rand.NextDouble();
                            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
                            double randNormal = MeanPos + StdevPos * randStdNormal;

                            CurrentWell.ListDescriptors[i].SetHistoValues(randNormal);
                           // CurrentWell.ListDescriptors[i].Getvalue() = randNormal;
                        }
                    }
                }


                if (WindowGenerateScreening.checkBoxNegativeCtrl.Checked)
                {
                    for (int Y = 0; Y < NumRow; Y++)
                    {
                        cWell CurrentWell = CurrentPlate.GetWell((int)WindowGenerateScreening.numericUpDownColNegCtrl.Value, Y, false);
                        CurrentWell.Name = "Negative Ctrl";
                        CurrentWell.SetClass(1);

                        for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                        {
                            double u1 = rand.NextDouble();
                            double u2 = rand.NextDouble();
                            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
                            double randNormal = MeanNeg + StdevNeg * randStdNormal;

                            CurrentWell.ListDescriptors[i].SetHistoValues(randNormal);
                           // CurrentWell.ListDescriptors[i].Getvalue() = randNormal;
                        }
                    }
                }



                if (WindowGenerateScreening.checkBoxRowEffect.Checked)
                {
                    double ShiftForRow = (double)WindowGenerateScreening.numericUpDownRowEffectIntensity.Value;
                    if (WindowGenerateScreening.checkBoxShiftRowEffect.Checked)
                        ShiftForRow += StepForShiftRow * IdxPlate;

                    for (int X = 0; X < NumCol; X++)
                        for (int Y = 0; Y < NumRow; Y++)
                        {
                            cWell CurrentWell = CurrentPlate.GetWell(X, Y, false);

                            for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                            {
                                double NewVal = CurrentWell.ListDescriptors[i].GetValue() * ((Y + 1) + ShiftForRow);
                                CurrentWell.ListDescriptors[i].SetHistoValues(NewVal);
                            }
                        }
                }

                if (WindowGenerateScreening.checkBoxColumnEffect.Checked)
                {
                    for (int X = 0; X < NumCol; X++)
                        for (int Y = 0; Y < NumRow; Y++)
                        {
                            cWell CurrentWell = CurrentPlate.GetWell(X, Y, false);

                            for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                            {
                               CurrentWell.ListDescriptors[i].SetHistoValues(CurrentWell.ListDescriptors[i].GetValue()  * ((X + 1) + (double)WindowGenerateScreening.numericUpDownColEffectIntensity.Value));
                            }
                        }
                }

                if (WindowGenerateScreening.checkBoxBowlEffect.Checked)
                {
                    double CenterX = (double)NumCol / 2.0 - 0.5;
                    double CenterY = (double)NumRow / 2.0 - 0.5;

                    double CurrentRatioXY = (double)WindowGenerateScreening.numericUpDownBowlEffectRatioXY.Value;

                    if (WindowGenerateScreening.checkBoxRatioXY.Checked)
                        CurrentRatioXY = (double)WindowGenerateScreening.numericUpDownBowlEffectRatioXY.Value + StepForRatioXY * IdxPlate;

                    for (int X = 0; X < NumCol; X++)
                        for (int Y = 0; Y < NumRow; Y++)
                        {
                            cWell CurrentWell = CurrentPlate.GetWell(X, Y, false);

                            for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                            {
                                double CurrentValue = (float)Math.Sqrt((X - CenterX) * (X - CenterX) / CurrentRatioXY + (Y - CenterY) * (Y - CenterY));
                                CurrentWell.ListDescriptors[i].SetHistoValues(CurrentWell.ListDescriptors[i].GetValue() * (CurrentValue + (double)WindowGenerateScreening.numericUpDownBowlEffectIntensity.Value));
                            }
                        }
                }

                if (WindowGenerateScreening.checkBoxEdgeEffect.Checked)
                {
                    int IdxDiff;

                    if (WindowGenerateScreening.checkBoxEdgeEffectIteration.Checked)
                        IdxDiff = (int)WindowGenerateScreening.numericUpDownEdgeEffectIteration.Value + IdxPlate * StepForDiffusion;
                    else
                        IdxDiff = (int)WindowGenerateScreening.numericUpDownEdgeEffectIteration.Value;

                    double[,] DiffusionMap = EdgeEffect.GetDiffusion(IdxDiff);

                    for (int X = 0; X < NumCol; X++)
                        for (int Y = 0; Y < NumRow; Y++)
                        {
                            cWell CurrentWell = CurrentPlate.GetWell(X, Y, false);

                            for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                            {
                                double CurrentValue = DiffusionMap[X, Y];
                                CurrentWell.ListDescriptors[i].SetHistoValues(CurrentWell.ListDescriptors[i].GetValue() * (CurrentValue + (double)WindowGenerateScreening.numericUpDownEdgeEffectIntensity.Value));
                            }
                        }
                }
            }

            CompleteScreening.UpDatePlateListWithFullAvailablePlate();

            for (int idxP = 0; idxP < CompleteScreening.ListPlatesActive.Count; idxP++)
                CompleteScreening.ListPlatesActive[idxP].UpDataMinMax();

            StartingUpDateUI();

            this.toolStripcomboBoxPlateList.Items.Clear();
            for (int IdxPlate = 0; IdxPlate < CompleteScreening.GetNumberOfActivePlates(); IdxPlate++)
            {
                string Name = CompleteScreening.ListPlatesActive.GetPlate(IdxPlate).Name;
                this.toolStripcomboBoxPlateList.Items.Add(Name);
                PlateListWindow.listBoxPlateNameToProcess.Items.Add(Name);
                PlateListWindow.listBoxAvaliableListPlates.Items.Add(Name);
                CompleteScreening.ListPlatesActive.GetPlate(IdxPlate).UpDataMinMax();
            }

            CompleteScreening.CurrentDisplayPlateIdx = 0;
            CompleteScreening.SetSelectionType(comboBoxClass.SelectedIndex - 1);

         

            //string NamePlate = PlateListWindow.listBoxAvaliableListPlates.Items[0].ToString();

            //toolStripcomboBoxPlateList.Text = NamePlate+" ";// toolStripcomboBoxPlateList.Items[0].ToString();
         //   toolStripcomboBoxPlateList.SelectedIndex = 0;
         //   CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, true);

            UpdateUIAfterLoading();
        }

        public class cRandomDistribution
        {
            Accord.Statistics.Distributions.Multivariate.MultivariateNormalDistribution rand;
            public int IdxClass;
            public int ColPosi;
            public double[][] MultivariateDistrib;


            public cRandomDistribution(double[] means, double[,] covariances, int IdxClass, int ColumnPosition, int Dim, int NumPt)
            {
                this.IdxClass = IdxClass;
                this.ColPosi = ColumnPosition;
                this.rand = new Accord.Statistics.Distributions.Multivariate.MultivariateNormalDistribution(means, covariances);
                MultivariateDistrib = rand.Generate(NumPt);
            }
        }

        private void multivariateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormForMultivariateScreen WindowMultivariateScreen = new FormForMultivariateScreen(GlobalInfo);

            if (WindowMultivariateScreen.ShowDialog() != DialogResult.OK) return;

            int NumPlate = (int)WindowMultivariateScreen.numericUpDownPlateNumber.Value;
            int NumRow = (int)WindowMultivariateScreen.numericUpDownRows.Value;
            int NumCol = (int)WindowMultivariateScreen.numericUpDownColumns.Value;
            int NumDesc = (int)WindowMultivariateScreen.numericUpDownDimensionNumber.Value;

            if (CompleteScreening != null) CompleteScreening.Close3DView();

            CompleteScreening = new cScreening("Generated Screen", GlobalInfo);

            CompleteScreening.ListDescriptors.Clean();
            CompleteScreening.Rows = NumRow;
            CompleteScreening.Columns = NumCol;
            CompleteScreening.ListPlatesAvailable = new cExtendPlateList();

            for (int i = 0; i < NumDesc; i++)
                CompleteScreening.ListDescriptors.AddNew(new cDescriptorsType("Descriptor_" + i, true, 1));

            // let's generate all the distributions
            List<cRandomDistribution> ListDistributions = new List<cRandomDistribution>();
            for (int i = 0; i < GlobalInfo.GetNumberofDefinedClass(); i++)
            {
                if ((bool)WindowMultivariateScreen.dataGridViewForCompounds.Rows[i].Cells[2].Value)
                {
                    double[] means = new double[NumDesc];
                    double[,] covariances = new double[NumDesc, NumDesc];
                    for (int Dim = 0; Dim < NumDesc; Dim++)
                    {
                        means[Dim] = Convert.ToDouble(WindowMultivariateScreen.dataGridViewForCompounds.Rows[i].Cells[2 * Dim + 3].Value.ToString());
                        covariances[Dim, Dim] = Math.Pow(Convert.ToDouble(WindowMultivariateScreen.dataGridViewForCompounds.Rows[i].Cells[2 * Dim + 4].Value.ToString()), 2);
                    }

                    cRandomDistribution CurrentDistrib;
                    if ((string)WindowMultivariateScreen.dataGridViewForCompounds.Rows[i].Cells[1].Value == "Entire plate")

                        CurrentDistrib = new cRandomDistribution(means, covariances, i, -1, NumDesc, NumRow * NumCol * NumPlate);
                    else
                        CurrentDistrib = new cRandomDistribution(means, covariances, i, Convert.ToInt16((string)WindowMultivariateScreen.dataGridViewForCompounds.Rows[i].Cells[1].Value), NumDesc, NumRow * NumPlate);

                    ListDistributions.Add(CurrentDistrib);
                }
            }

            // generate the plates
            for (int IdxPlate = 0; IdxPlate < NumPlate; IdxPlate++)
            {
                string PlateName = "Plate_" + IdxPlate;
                cPlate CurrentPlate = new cPlate("Cpds", PlateName, CompleteScreening);
                CompleteScreening.AddPlate(CurrentPlate);
            }


            // Place the values
            for (int IdxDistri = 0; IdxDistri < ListDistributions.Count; IdxDistri++)
            {
                if (ListDistributions[IdxDistri].ColPosi != -1) continue;

                for (int IdxPlate = 0; IdxPlate < NumPlate; IdxPlate++)
                {
                    cPlate CurrentPlate = CompleteScreening.ListPlatesAvailable[IdxPlate];
                    for (int X = 1; X <= NumCol; X++)
                        for (int Y = 1; Y <= NumRow; Y++)
                        {
                            // Create the descriptor list and add it to the well, then add the well
                            List<cDescriptor> LDesc = new List<cDescriptor>();
                            for (int i = 0; i < NumDesc; i++)
                            {
                                cDescriptor Desc = null;
                                double randNormal = ListDistributions[IdxDistri].MultivariateDistrib[(X - 1) + (Y - 1) * NumCol + IdxPlate * NumRow * NumCol][i];

                                Desc = new cDescriptor(randNormal, CompleteScreening.ListDescriptors[i],CompleteScreening);
                                LDesc.Add(Desc);
                            }

                            cWell CurrentWell = CurrentPlate.GetWell(X - 1, Y - 1, false);
                            if (CurrentWell == null)
                            {
                                CurrentWell = new cWell(LDesc, X, Y, CompleteScreening, CurrentPlate);
                                CurrentPlate.AddWell(CurrentWell);
                            }
                            CurrentWell.Name = "Cpds";
                            CurrentWell.SetClass(ListDistributions[IdxDistri].IdxClass);
                        }
                }

            }

            for (int IdxPlate = 0; IdxPlate < NumPlate; IdxPlate++)
            {
                cPlate CurrentPlate = CompleteScreening.ListPlatesAvailable[IdxPlate];
                for (int IdxDistri = 0; IdxDistri < ListDistributions.Count; IdxDistri++)
                {
                    if (ListDistributions[IdxDistri].ColPosi != -1)
                    {
                        for (int Y = 1; Y <= NumRow; Y++)
                        {
                            // Create the descriptor list and add it to the well, then add the well
                            List<cDescriptor> LDesc = new List<cDescriptor>();
                            for (int i = 0; i < NumDesc; i++)
                            {
                                cDescriptor Desc = null;
                                double randNormal = ListDistributions[IdxDistri].MultivariateDistrib[(Y - 1) + IdxPlate * NumRow][i];

                                Desc = new cDescriptor(randNormal, CompleteScreening.ListDescriptors[i],CompleteScreening);
                                LDesc.Add(Desc);
                            }

                            cWell CurrentWell = CurrentPlate.GetWell(ListDistributions[IdxDistri].ColPosi, Y - 1, false);
                            if (CurrentWell == null)
                            {
                                CurrentWell = new cWell(LDesc, ListDistributions[IdxDistri].ColPosi + 1, Y, CompleteScreening, CurrentPlate);
                                CurrentPlate.AddWell(CurrentWell);
                            }
                            else
                            {
                                for (int i = 0; i < NumDesc; i++)
                                {
                                    CurrentWell.ListDescriptors[i].SetHistoValues(ListDistributions[IdxDistri].MultivariateDistrib[(Y - 1) + IdxPlate * NumRow][i]);
                                    //CurrentWell.ListDescriptors[i].Getvalue() = ListDistributions[IdxDistri].MultivariateDistrib[(Y - 1) + IdxPlate * NumRow][i];
                                }

                            }
                            CurrentWell.Name = "Cpds";
                            CurrentWell.SetClass(ListDistributions[IdxDistri].IdxClass);
                        }
                    }
                }
            }


            CompleteScreening.UpDatePlateListWithFullAvailablePlate();

            for (int idxP = 0; idxP < CompleteScreening.ListPlatesActive.Count; idxP++)
                CompleteScreening.ListPlatesActive[idxP].UpDataMinMax();

            StartingUpDateUI();

            this.toolStripcomboBoxPlateList.Items.Clear();
            for (int IdxPlate = 0; IdxPlate < CompleteScreening.GetNumberOfActivePlates(); IdxPlate++)
            {
                string Name = CompleteScreening.ListPlatesActive.GetPlate(IdxPlate).Name;
                this.toolStripcomboBoxPlateList.Items.Add(Name);
                PlateListWindow.listBoxPlateNameToProcess.Items.Add(Name);
                PlateListWindow.listBoxAvaliableListPlates.Items.Add(Name);
                CompleteScreening.ListPlatesActive.GetPlate(IdxPlate).UpDataMinMax();
            }
            CompleteScreening.CurrentDisplayPlateIdx = 0;
            CompleteScreening.SetSelectionType(comboBoxClass.SelectedIndex - 1);

            CompleteScreening.ListDescriptors.CurrentSelectedDescriptor = 0;
         //   comboBoxDescriptorToDisplay.SelectedIndex = 0;
           // toolStripcomboBoxPlateList.SelectedIndex = 0;
           // CompleteScreening.GetCurrentDisplayPlate().DisplayDistribution(CompleteScreening.ListDescriptors.CurrentSelectedDescriptor, true);

            UpdateUIAfterLoading();
        }
        #endregion
    }
}