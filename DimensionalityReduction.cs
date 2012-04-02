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
using weka.classifiers;
using weka.attributeSelection;
using HCSAnalyzer.Classes;

namespace HCSAnalyzer
{
    public partial class HCSAnalyzer
    {
        #region Dimensionality reduction UI
        private void comboBoxReduceDimMultiClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBoxSupervisedDimRec.Clear();
            numericUpDownNewDimension.Enabled = true;

            switch (comboBoxReduceDimMultiClass.SelectedIndex)
            {
                case 0:
                    richTextBoxSupervisedDimRec.AppendText("InfoGain.\nFor more information, go to: http://en.wikipedia.org/wiki/Information_gain_in_decision_trees \n");
                    break;
                case 1:
                    richTextBoxSupervisedDimRec.AppendText("OneR.\n");
                    break;
                case 2:
                    richTextBoxSupervisedDimRec.AppendText("Supervised Greedy stepwise.\nFor more information, go to: http://en.wikipedia.org/wiki/Stepwise_regression \n");
                    numericUpDownNewDimension.Enabled = false;
                    break;
            }
        }


        private void ClickOnLink(string Link)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            if (GlobalInfo.OptionsWindow.radioButtonIE.Checked)
                proc.StartInfo.FileName = "iexplore";
            else
                proc.StartInfo.FileName = "chrome";
            proc.StartInfo.Arguments = Link;
            proc.Start();
        }

        private void richTextBoxSupervisedDimRec_LinkClicked(object sender, LinkClickedEventArgs e)
        {
             ClickOnLink(e.LinkText);
        }

        private void richTextBoxUnsupervisedDimRec_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            ClickOnLink(e.LinkText);
        }

        private void comboBoxReduceDimSingleClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBoxUnsupervisedDimRec.Clear();
            numericUpDownNewDimension.Enabled = true;

            switch (comboBoxReduceDimSingleClass.SelectedIndex)
            {
                case 0:
                    richTextBoxUnsupervisedDimRec.AppendText("Principal Component Analysis.\nFor more information, go to: http://en.wikipedia.org/wiki/Principal_component_analysis \n");
                    break;
                case 1:
                    richTextBoxUnsupervisedDimRec.AppendText("Greedy Stepwise.\nFor more information, go to: http://en.wikipedia.org/wiki/Stepwise_regression \n");
                    numericUpDownNewDimension.Enabled = false;
                    break;
                case 2:
                   
                    break;
            }
        }


        private void radioButtonDimRedSupervised_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxSupervised.Enabled = true;
            groupBoxUnsupervised.Enabled = false;
            numericUpDownNewDimension.Enabled = true;
        }

        private void radioButtonDimRedUnsupervised_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxSupervised.Enabled = false;
            groupBoxUnsupervised.Enabled = true;
            if (comboBoxReduceDimSingleClass.SelectedIndex == 1) numericUpDownNewDimension.Enabled = false;
        }

        private void buttonReduceDim_Click(object sender, EventArgs e)
        {
            if (CompleteScreening == null) return;
            int New_Dim = (int)numericUpDownNewDimension.Value;
            if (New_Dim >= CompleteScreening.GetNumberOfActiveDescriptor())
            {
                System.Windows.Forms.DialogResult ResError = MessageBox.Show("The dimension number cannot be higher (or equal) than the current one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Windows.Forms.DialogResult ResWin = MessageBox.Show("By applying this process, selected descriptors list will be modified ! Proceed ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ResWin == System.Windows.Forms.DialogResult.No) return;

            List<cPlate> ListPlate = new List<cPlate>();
            //if (radioButtonDimRecCurrentPlate.Checked)
            //{
            //    ListPlate.Add(CompleteScreening.GetCurrentDisplayPlate());
            //}
            //else
            //{
                int NumberOfPlates = CompleteScreening.ListPlatesActive.Count;

                for (int PlateIdx = 0; PlateIdx < NumberOfPlates; PlateIdx++)
                {
                    cPlate CurrentPlateToProcess = CompleteScreening.ListPlatesActive.GetPlate(CompleteScreening.ListPlatesActive[PlateIdx].Name);
                    ListPlate.Add(CurrentPlateToProcess);
                }

         //   }


            if (radioButtonDimRedUnsupervised.Checked)
                ReduceDimensionUnSupervised(ListPlate, New_Dim, comboBoxReduceDimSingleClass.SelectedIndex);
            else
                ReduceDimensionSupervised(ListPlate, New_Dim, comboBoxDimReductionNeutralClass.SelectedIndex, comboBoxReduceDimMultiClass.SelectedIndex);
        }
        #endregion

        #region Reduction algorithms
        private int[] ReduceByInfoGain(weka.core.Instances insts)
        {
            weka.attributeSelection.InfoGainAttributeEval AttribEvaluator = new weka.attributeSelection.InfoGainAttributeEval();
            AttribEvaluator.buildEvaluator(insts);
            Ranker search2 = new Ranker();
            int[] rang = search2.search(AttribEvaluator, insts);

            return rang;
        }

        private int[] ReduceByGreedySupervised(weka.core.Instances insts)
        {
            int[] rang = null;

            AttributeSelection filter = new AttributeSelection();  // package weka.filters.supervised.attribute!
            CfsSubsetEval eval = new CfsSubsetEval();
            GreedyStepwise search = new GreedyStepwise();
            search.setSearchBackwards(true);
            filter.setEvaluator(eval);
            filter.setSearch(search);
            filter.SelectAttributes(insts);
            rang = filter.selectedAttributes();

            return rang;
        }

        private int[] ReduceByGreedy(weka.core.Instances insts)
        {
            int[] rang = null;
            AttributeSelection filter = new AttributeSelection();  // package weka.filters.supervised.attribute!
            CfsSubsetEval eval = new CfsSubsetEval();
            GreedyStepwise search = new GreedyStepwise();
            search.setSearchBackwards(true);
            filter.setEvaluator(eval);
            filter.setSearch(search);
            filter.SelectAttributes(insts);
            rang = filter.selectedAttributes();

            return rang;
        }

        private int[] ReduceByOneR(weka.core.Instances insts)
        {
            List<int> Rank = new List<int>();
            List<cScoreAndClass> ListScoreAndClass = new List<cScoreAndClass>();
      

            weka.attributeSelection.OneRAttributeEval Attrib = new OneRAttributeEval();
            Attrib.buildEvaluator(insts);
            richTextBoxSupervisedDimRec.AppendText("\n\nScores:");
            int realIdx = 0;
            for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
            {
                if (CompleteScreening.ListDescriptors[i].IsActive() == false) continue;
                double TmpScore = Attrib.evaluateAttribute(realIdx++);
                richTextBoxSupervisedDimRec.AppendText("\n " + CompleteScreening.ListDescriptors[i].GetName() + "\t: " + TmpScore);
                ListScoreAndClass.Add(new cScoreAndClass(realIdx-1, TmpScore));
            }

            ListScoreAndClass.Sort(delegate(cScoreAndClass p1, cScoreAndClass p2) { return p1.Score.CompareTo(p2.Score); });
            for(int i=ListScoreAndClass.Count-1;i>=0;i--)
            {
                Rank.Add(ListScoreAndClass[i].Class);
            }
            return Rank.ToArray();
        }

        private int[] ReduceByPCA(weka.core.Instances insts)
        {
            int[] rang = null;
            PrincipalComponents filter = new PrincipalComponents();  // package weka.filters.supervised.attribute!
            filter.setTransformBackToOriginal(true);
            filter.buildEvaluator(insts);

            Ranker search2 = new Ranker();
            search2.setNumToSelect(2);
            search2.setGenerateRanking(true);
            rang = search2.search(filter, insts);
           
            return rang;
        }
        #endregion

        #region Reduction Main Functions
        private void ReduceDimensionSupervised(List<cPlate> ListPlate, int New_Dim, int NeutralClass, int Algo)
        {
            int NumClasses = CompleteScreening.GetNumberOfClasses();
            if (NumClasses <= 1)
            {
                System.Windows.Forms.DialogResult ResError = MessageBox.Show("You have to select at least 2 classes.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            weka.core.Instances insts = null;

            if (ListPlate.Count == 1)
            {
                cInfoClass InfoClass = CompleteScreening.GetCurrentDisplayPlate().GetNumberOfClassesBut(NeutralClass);
                insts = CompleteScreening.GetCurrentDisplayPlate().CreateInstancesWithClasses(InfoClass, NeutralClass);
            }
            else
            {
                cInfoClass InfoClass = CompleteScreening.GetNumberOfClassesBut(NeutralClass);
                insts = CompleteScreening.CreateInstancesWithClasses(InfoClass, NeutralClass);
            }

            cInfoDescriptors InfoDescriptors = CompleteScreening.BuildInfoDesc();
            int RealIdx = 0;
            GlobalInfo.ConsoleWriteLine("Feature selection results: \n");

            int[] rang = null;
            switch (Algo)
            {
                case 0:
                    rang = ReduceByInfoGain(insts);
                    for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                    {
                        if (CompleteScreening.ListDescriptors[i].IsActive() == false) continue;

                        if (rang[RealIdx] < New_Dim)
                        {
                            //CompleteScreening.ListDescriptors.SetItemState(i, true);

                            //checkedListBoxActiveDescriptors.SetItemCheckState(i, CheckState.Checked);
                            CompleteScreening.ListDescriptors.SetItemState(i,true);
                        }
                        else
                        {
                            //CompleteScreening.ListDescriptors.SetItemState(i, false);
                            //checkedListBoxActiveDescriptors.SetItemCheckState(i, CheckState.Unchecked);
                            CompleteScreening.ListDescriptors.SetItemState(i, false);
                        }
                        RealIdx++;
                    }
                    break;
                case 1:
                   // return;
                    rang = ReduceByOneR(insts);
                    for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                    {
                        if (CompleteScreening.ListDescriptors[i].IsActive() == false) continue;

                        if (rang[RealIdx] < New_Dim)
                        {
                           // CompleteScreening.ListDescriptors.SetItemState(i, true);
                            //checkedListBoxActiveDescriptors.SetItemCheckState(i, CheckState.Checked);
                            CompleteScreening.ListDescriptors.SetItemState(i, true);
                        }
                        else
                        {
                            //CompleteScreening.ListDescriptors.SetItemState(i, false);
                            //checkedListBoxActiveDescriptors.SetItemCheckState(i, CheckState.Unchecked);
                            CompleteScreening.ListDescriptors.SetItemState(i, false);
                        }
                        RealIdx++;
                    }
                    break;
                case 2:
                    rang = ReduceByGreedySupervised(insts);

                    for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                    {
                      //  CompleteScreening.ListDescriptors.SetItemState(i, false);
                        //checkedListBoxActiveDescriptors.SetItemCheckState(i, CheckState.Unchecked);
                        CompleteScreening.ListDescriptors.SetItemState(i, false);
                    }

                    for (int i = 0; i < rang.Length-1; i++)
                    {
                        //checkedListBoxActiveDescriptors.SetItemCheckState(rang[i], CheckState.Checked);
                        //CompleteScreening.ListDescriptors.SetItemState(rang[i], true);
                        CompleteScreening.ListDescriptors.SetItemState(rang[i], true);
                    }
                    break;

                default:
                    break;
            }
            RefreshInfoScreeningRichBox();
        }

        private void ReduceDimensionUnSupervised(List<cPlate> ListPlate, int New_Dim, int Algo)
        {
            weka.core.Instances insts = null;

            if(ListPlate.Count==1)
                insts = CompleteScreening.GetCurrentDisplayPlate().CreateInstancesWithoutClass();// CreateInstanceWithoutClass(CurrentTable);
            else
                insts = CompleteScreening.CreateInstancesWithoutClass();

            int[] rang = null;

            int RealIdx = 0;
            GlobalInfo.ConsoleWriteLine("Feature selection results: \n");
            
            switch (Algo)
            {
                case 0:
                    rang = ReduceByPCA(insts);
                    for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                    {
                        if (CompleteScreening.ListDescriptors[i].IsActive() == false) continue;
                        if (rang[RealIdx] < New_Dim)
                        {
                            //checkedListBoxActiveDescriptors.SetItemCheckState(i, CheckState.Checked);
                            CompleteScreening.ListDescriptors.SetItemState(i,true);
                        }
                        else
                        {
                            //checkedListBoxActiveDescriptors.SetItemCheckState(i, CheckState.Unchecked);
                            CompleteScreening.ListDescriptors.SetItemState(i,false);
                        }
                        RealIdx++;
                    }
                    break;
                case 1:
                    rang = ReduceByGreedy(insts);
                    for (int i = 0; i < CompleteScreening.ListDescriptors.Count; i++)
                    {
                       // checkedListBoxActiveDescriptors.SetItemCheckState(i, CheckState.Unchecked);
                        CompleteScreening.ListDescriptors.SetItemState(i,false);
                    }

                    for (int i = 0; i < rang.Length-1; i++)
                    {
                      //  checkedListBoxActiveDescriptors.SetItemCheckState(rang[i], CheckState.Checked);
                        CompleteScreening.ListDescriptors.SetItemState(rang[i],true);
                    }
                    break;

                default:
                    break;
            }





            RefreshInfoScreeningRichBox();
        }
        #endregion
    }
}
