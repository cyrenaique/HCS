using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using weka.core;
using System.Data;
using weka.clusterers;
using LibPlateAnalysis;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Msagl.GraphViewerGdi;
using HCSAnalyzer.Forms;
using weka.attributeSelection;
using weka.filters;
using weka.classifiers;
using HCSAnalyzer.Classes;

namespace HCSAnalyzer
{
    public partial class HCSAnalyzer
    {
        #region User interface
        private void comboBoxCLassificationMethod_SelectedIndexChanged(object sender, EventArgs e)
        {

            richTextBoxInfoClassif.Clear();

            switch (comboBoxCLassificationMethod.SelectedIndex)
            {
                case 0:
                    richTextBoxInfoClassif.AppendText("C4.5.\nFor more information, go to: http://en.wikipedia.org/wiki/C4.5_algorithm \n");
                    break;
                case 1:
                    richTextBoxInfoClassif.AppendText("Support Vector Machine.\nFor more information, go to: http://en.wikipedia.org/wiki/Support_vector_machine \n");
                    break;
                case 2:
                    richTextBoxInfoClassif.AppendText("Neural Network.\nFor more information, go to: http://en.wikipedia.org/wiki/Artificial_neural_network \n");
                    break;
                case 3:
                    richTextBoxInfoClassif.AppendText("K-Nearest Neighbor(s).\nFor more information, go to: http://en.wikipedia.org/wiki/K-nearest_neighbor_algorithm \n");
                    break;

            }

        }

        private void buttonStartClassification_Click_1(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            if (!CompleteScreening.IsSelectedDescriptors())
            {
                MessageBox.Show("You have to check at least one descriptor !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            this.Cursor = Cursors.WaitCursor;
            Classification(comboBoxNeutralClassForClassif.SelectedIndex, radioButtonClassifPlateByPlate.Checked, comboBoxCLassificationMethod.SelectedIndex);
            this.Cursor = Cursors.Default;

            switch (comboBoxCLassificationMethod.SelectedIndex)
            {
                case 0:
                    if (radioButtonClassifPlateByPlate.Checked)
                        MessageBox.Show("C4.5 classification process finished ! \n Press (Ctrl+T) for current plate tree.", "Process over !", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        MessageBox.Show("C4.5 classification process finished ! \n", "Process over !", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case 1:
                    MessageBox.Show("Support Vector Machine classification process finished !", "Process over !", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case 2:
                    MessageBox.Show("Neural Network classification process finished !", "Process over !", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case 3:
                    MessageBox.Show("K-Nearest Neighbor(s) classification process finished !", "Process over !", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                default:
                    break;
            }

        }

        private void richTextBoxInfoClassif_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            ClickOnLink(e.LinkText);
        }

        private void classificationTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;

            FormForClassificationTree WindowForTree = new FormForClassificationTree();

            WindowForTree.Text = CompleteScreening.GetCurrentDisplayPlate().Name;
            string StringForTree = CompleteScreening.GetCurrentDisplayPlate().GetInfoClassif().StringForTree;
            if ((StringForTree == null) || (StringForTree.Length == 0))
            {
                MessageBox.Show("No tree avaliable for the selected plate !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            WindowForTree.gViewerForTreeClassif.Graph = ComputeAndDisplayGraph(StringForTree.Remove(StringForTree.Length - 3, 3));

            WindowForTree.richTextBoxConsoleForClassification.Clear();
            WindowForTree.richTextBoxConsoleForClassification.AppendText(CompleteScreening.GetCurrentDisplayPlate().GetInfoClassif().StringForQuality);
            WindowForTree.richTextBoxConsoleForClassification.AppendText(CompleteScreening.GetCurrentDisplayPlate().GetInfoClassif().ConfusionMatrix);

            WindowForTree.Show();
        }
        #endregion

        #region Tree graph display functions
        private Microsoft.Msagl.Drawing.Graph ComputeAndDisplayGraph(string DotString)
        {
            int CurrentPos = 0;
            int NextReturnPos = CurrentPos;
            List<int> ListNodeId = new List<int>();
            string TmpDotString = DotString;

            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");

            while (NextReturnPos != -1)
            {
                int NextBracket = DotString.IndexOf("[");
                string StringToProcess = DotString.Remove(NextBracket - 1);
                string StringToProcess1 = StringToProcess.Remove(0, 1);

                if (StringToProcess1.Contains("N") == false)
                {
                    int Id = Convert.ToInt32(StringToProcess1);
                    Microsoft.Msagl.Drawing.Node Currentnode = new Microsoft.Msagl.Drawing.Node(Id.ToString());
                    Currentnode.Label.FontColor = Microsoft.Msagl.Drawing.Color.Red;
                    int LabelPos = DotString.IndexOf("label=\"");
                    string LabelString = DotString.Remove(0, LabelPos + 7);
                    LabelPos = LabelString.IndexOf("\"");
                    string FinalLabel = LabelString.Remove(LabelPos);
                    Currentnode.LabelText = FinalLabel;
                    graph.AddNode(Currentnode);
                }

                NextReturnPos = DotString.IndexOf("\n");
                if (NextReturnPos != -1)
                {
                    string TmpString = DotString.Remove(0, NextReturnPos + 1);
                    DotString = TmpString;
                }
            }

            DotString = TmpDotString;
            NextReturnPos = 0;
            while (NextReturnPos != -1)
            {
                int NextBracket = DotString.IndexOf("[");
                string StringToProcess = DotString.Remove(NextBracket - 1);
                string StringToProcess1 = StringToProcess.Remove(0, 1);

                if (StringToProcess1.Contains("N"))
                {
                    string stringNodeIdxStart = StringToProcess1.Remove(StringToProcess1.IndexOf("-"));
                    int NodeIdxStart = Convert.ToInt32(stringNodeIdxStart);

                    string stringNodeIdxEnd = StringToProcess1.Remove(0, StringToProcess1.IndexOf("N") + 1);
                    int NodeIdxSEnd = Convert.ToInt32(stringNodeIdxEnd);

                    int LabelPos = DotString.IndexOf("label=");
                    LabelPos += 7;

                    string CurrLabelString = DotString.Remove(0, LabelPos);

                    Microsoft.Msagl.Drawing.Edge Currentedge = new Microsoft.Msagl.Drawing.Edge(stringNodeIdxStart, ""/*NodeIdx.ToString()*/, stringNodeIdxEnd);
                    Currentedge.LabelText = CurrLabelString.Remove(CurrLabelString.IndexOf("]") - 1);
                    graph.Edges.Add(Currentedge);
                }

                NextReturnPos = DotString.IndexOf("\n");

                if (NextReturnPos != -1)
                {
                    string TmpString = DotString.Remove(0, NextReturnPos + 1);
                    DotString = TmpString;
                }
            }
            return graph;
        }

        private Microsoft.Msagl.Drawing.Graph DisplayTheGraph(cPlate PlateForTree)
        {
            FormForClassificationTree WindowForTree = new FormForClassificationTree();

            WindowForTree.Text = PlateForTree.Name;
            string StringForTree = PlateForTree.GetInfoClassif().StringForTree;
            if ((StringForTree == null) || (StringForTree.Length == 0))
                return null;

            WindowForTree.richTextBoxConsoleForClassification.Clear();
            WindowForTree.richTextBoxConsoleForClassification.AppendText(CompleteScreening.GetCurrentDisplayPlate().GetInfoClassif().StringForQuality);
            WindowForTree.richTextBoxConsoleForClassification.AppendText(CompleteScreening.GetCurrentDisplayPlate().GetInfoClassif().ConfusionMatrix);

            return ComputeAndDisplayGraph(StringForTree.Remove(StringForTree.Length - 3, 3));

        }
        #endregion

        #region Classification Main functions
        private void Classification(int NeutralClass, bool IsPlateByPlate, int IdxClassifier)
        {
            if (IsPlateByPlate)
                ClassificationPlateByPlate(NeutralClass, IdxClassifier);
            else
                ClassificationGlobal(NeutralClass, IdxClassifier);
        }

        /// <summary>
        /// Global classification
        /// </summary>
        /// <param name="NeutralClass">Neutral class</param>
        /// <param name="IdxClassifier">Classifier Index (0:J48), (1:SVM), (2:NN), (3:KNN)</param>
        private void ClassificationGlobal(int NeutralClass, int IdxClassifier)
        {
            cInfoClass InfoClass = CompleteScreening.GetNumberOfClassesBut(NeutralClass);

            if (InfoClass.NumberOfClass <= 1)
            {
                richTextBoxInfoClassif.AppendText("Screening not processed.\n");
                return;
            }

            weka.core.Instances insts = CompleteScreening.CreateInstancesWithClasses(InfoClass, NeutralClass);
            Classifier ClassificationModel = null;

            switch (IdxClassifier)
            {
                case 0: // J48
                    ClassificationModel = new weka.classifiers.trees.J48();
                    weka.classifiers.trees.J48 J48Model = (weka.classifiers.trees.J48)ClassificationModel;
                    J48Model.setMinNumObj((int)GlobalInfo.OptionsWindow.numericUpDownJ48MinNumObjects.Value);
                    richTextBoxInfoClassif.AppendText("\nC4.5 : " + InfoClass.NumberOfClass + " classes");
                    break;
                case 1: // SVM
                    ClassificationModel = new weka.classifiers.functions.SMO();
                    break;
                case 2: // NN
                    ClassificationModel = new weka.classifiers.functions.MultilayerPerceptron();
                    break;
                case 3: // KNN
                    ClassificationModel = new weka.classifiers.lazy.IBk((int)CompleteScreening.GlobalInfo.OptionsWindow.numericUpDownKofKNN.Value);
                    break;
                default:
                    break;
            }


            weka.core.Instances train = new weka.core.Instances(insts, 0, insts.numInstances());

            ClassificationModel.buildClassifier(train);
            GlobalInfo.ConsoleWriteLine(ClassificationModel.ToString());

            weka.classifiers.Evaluation evaluation = new weka.classifiers.Evaluation(insts);
            evaluation.crossValidateModel(ClassificationModel, insts, 2, new java.util.Random(1));


            GlobalInfo.ConsoleWriteLine(evaluation.toSummaryString());
            GlobalInfo.ConsoleWriteLine(evaluation.toMatrixString());

            // update classification information of the current plate
            string Text = "";
            switch (IdxClassifier)
            {
                case 0: // J48
                    Text = "J48 - ";
                    break;
                case 1: // SVM
                    //  ClassificationModel = new weka.classifiers.functions.SMO();
                    Text = "SVM - ";
                    break;
                case 2: // NN
                    // ClassificationModel = new weka.classifiers.functions.MultilayerPerceptron();
                    Text = "Neural Network - ";
                    break;
                case 3: // KNN
                    // ClassificationModel = new weka.classifiers.lazy.IBk((int)CompleteScreening.GlobalInfo.OptionsWindow.numericUpDownKofKNN.Value);
                    Text = "K-Nearest Neighbor(s) - ";
                    break;
                default:
                    break;
            }
            richTextBoxInfoClassif.AppendText(Text + InfoClass.NumberOfClass + " classes.");

            // CurrentPlateToProcess.GetInfoClassif().StringForQuality = evaluation.toSummaryString();
            //  CurrentPlateToProcess.GetInfoClassif().ConfusionMatrix = evaluation.toMatrixString();
            foreach (cPlate CurrentPlateToProcess in CompleteScreening.ListPlatesActive)
            {
                foreach (cWell TmpWell in CurrentPlateToProcess.ListActiveWells)
                {
                    // return;
                    weka.core.Instance currentInst = TmpWell.CreateInstanceForNClasses(InfoClass).instance(0);
                    double predictedClass = ClassificationModel.classifyInstance(currentInst);
                    TmpWell.SetClass(InfoClass.ListBackAssociation[(int)predictedClass]);
                }
            }
            return;
        }

        /// <summary>
        /// Plate by plate classification
        /// </summary>
        /// <param name="NeutralClass">Neutral class</param>
        /// <param name="IdxClassifier">Classifier Index (0:J48), (1:SVM), (2:NN), (3:KNN)</param>
        private void ClassificationPlateByPlate(int NeutralClass, int IdxClassifier)
        {

            int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;

            for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
            {
                cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);
                cInfoClass InfoClass = CurrentPlateToProcess.GetNumberOfClassesBut(NeutralClass);
                // return;
                if (InfoClass.NumberOfClass <= 1)
                {
                    richTextBoxInfoClassif.AppendText(CurrentPlateToProcess.Name + " not processed.\n");
                    continue;
                }

                weka.core.Instances insts = CurrentPlateToProcess.CreateInstancesWithClasses(InfoClass, NeutralClass);
                Classifier ClassificationModel = null;
                string Text = "";
                switch (IdxClassifier)
                {
                    case 0: // J48
                        ClassificationModel = new weka.classifiers.trees.J48();
                        weka.classifiers.trees.J48 J48Model = (weka.classifiers.trees.J48)ClassificationModel;
                        J48Model.setMinNumObj((int)GlobalInfo.OptionsWindow.numericUpDownJ48MinNumObjects.Value);
                        Text = "J48 - ";
                        break;
                    case 1: // SVM
                        ClassificationModel = new weka.classifiers.functions.SMO();
                        Text = "SVM - ";
                        break;
                    case 2: // NN
                        ClassificationModel = new weka.classifiers.functions.MultilayerPerceptron();
                        Text = "Neural Network - ";
                        break;
                    case 3: // KNN
                        ClassificationModel = new weka.classifiers.lazy.IBk((int)CompleteScreening.GlobalInfo.OptionsWindow.numericUpDownKofKNN.Value);
                        Text = "K-Nearest Neighbor(s) - ";
                        break;
                    default:
                        break;
                }
                richTextBoxInfoClassif.AppendText(Text + InfoClass.NumberOfClass + " classes - Plate: ");

                richTextBoxInfoClassif.AppendText(CurrentPlateToProcess.Name + " OK \n");
                weka.core.Instances train = new weka.core.Instances(insts, 0, insts.numInstances());

                ClassificationModel.buildClassifier(train);
                GlobalInfo.ConsoleWriteLine(ClassificationModel.ToString());

                weka.classifiers.Evaluation evaluation = new weka.classifiers.Evaluation(insts);
                evaluation.crossValidateModel(ClassificationModel, insts, 2, new java.util.Random(1));


                GlobalInfo.ConsoleWriteLine(evaluation.toSummaryString());
                GlobalInfo.ConsoleWriteLine(evaluation.toMatrixString());

                // update classification information of the current plate
                switch (IdxClassifier)
                {
                    case 0: // J48
                        weka.classifiers.trees.J48 CurrentClassifier = (weka.classifiers.trees.J48)(ClassificationModel);
                        CurrentPlateToProcess.GetInfoClassif().StringForTree = CurrentClassifier.graph().Remove(0, CurrentClassifier.graph().IndexOf("{") + 2);
                        break;
                    /*case 1: // SVM

                        break;
                    case 2: // NN

                        break;
                    case 3: // KNN

                        break;*/
                    default:
                        break;
                }

                CurrentPlateToProcess.GetInfoClassif().StringForQuality = evaluation.toSummaryString();
                CurrentPlateToProcess.GetInfoClassif().ConfusionMatrix = evaluation.toMatrixString();

                foreach (cWell TmpWell in CurrentPlateToProcess.ListActiveWells)
                {
                    weka.core.Instance currentInst = TmpWell.CreateInstanceForNClasses(InfoClass).instance(0);
                    double predictedClass = ClassificationModel.classifyInstance(currentInst);

                    TmpWell.SetClass(InfoClass.ListBackAssociation[(int)predictedClass]);
                }
            }
            return;
        }
        #endregion

    }
}

