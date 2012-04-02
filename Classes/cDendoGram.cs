using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HCSAnalyzer.Forms;
using System.Drawing;
using LibPlateAnalysis;
using weka.clusterers;
using weka.core;
using weka.core.neighboursearch;


namespace HCSAnalyzer.Classes
{
    public class cDendoGram
    {
        public class Node
        {


            public string Name;
            public double BranchLenght;

            public double PosXCorner;
            public double PosX;

            public double PosY;
            //public double PosX;
            public Node ConnectedWith;
            public cWell AssociatedWell = null;

            public Node(string Name, double Lenght, double PosY, double PosX)
            {
                this.Name = Name;
                this.BranchLenght = Lenght;
                this.PosY = PosY;
                this.PosXCorner = PosX + this.BranchLenght;
                this.PosX = PosX;
            }

        }

        public class cOurTree : List<Node>
        {
            public double GetMaxLenght()
            {
                double MaxSize = double.MinValue;
                foreach (Node CurrentNode in this)
                {
                    if (CurrentNode.BranchLenght > MaxSize) MaxSize = CurrentNode.BranchLenght;
                }
                return MaxSize;
            }
        }

        private class ClassNewickParser
        {
            cOurTree OurTree = new cOurTree();
            public cDendoGram OurDendo;

            float CurrentIdx = 0;


            public List<Node> ProcessConnection(string LigneToProcess)
            {
                int Idx = LigneToProcess.IndexOf(',');
                if (Idx == -1) return null;

                string NewLigne = LigneToProcess.Substring(1, LigneToProcess.Length - 2);

                string[] DoubleNode = NewLigne.Split(',');


                Node Node1 = null;
                Node Node2 = null;

                string[] Values = DoubleNode[0].Split(':');
                foreach (Node tNode in OurTree)
                {
                    if (tNode.Name == Values[0])
                    {
                        Node1 = tNode;
                        Node1.BranchLenght = double.Parse(Values[1]);
                        Node1.PosXCorner = Node1.PosX + Node1.BranchLenght;
                        break;
                    }
                }
                if (Node1 == null)
                {
                    double BranchLenght = double.Parse(Values[1]);
                    string Name = Values[0];

                    Node1 = new Node(Name, BranchLenght, CurrentIdx++, 0);
                    if (Name.IndexOf("Input") == -1)
                    {
                        Node1.AssociatedWell = OurDendo.InfoForHierarchical.ListIndexedWells[(int)(double.Parse(Name))];
                    }
                }
                Values = DoubleNode[1].Split(':');
                foreach (Node tNode in OurTree)
                {
                    if (tNode.Name == Values[0])
                    {
                        Node2 = tNode;
                        Node2.BranchLenght = double.Parse(Values[1]);
                        Node2.PosXCorner = Node2.PosX + Node2.BranchLenght;
                        break;
                    }
                }
                if (Node2 == null)
                {
                    double BranchLenght = double.Parse(Values[1]);
                    string Name = Values[0];

                    Node2 = new Node(Name, BranchLenght, CurrentIdx++, 0);
                    if (Name.IndexOf("Input") == -1)
                    {
                        Node2.AssociatedWell = OurDendo.InfoForHierarchical.ListIndexedWells[(int)(double.Parse(Name))];
                    }

                }

                Node1.ConnectedWith = Node2;
                Node2.ConnectedWith = Node1;

                OurTree.Add(Node1);
                OurTree.Add(Node2);

                List<Node> ToReturn = new List<Node>();
                ToReturn.Add(Node1);
                ToReturn.Add(Node2);
                return ToReturn;
            }


            public cOurTree ParseString(string Ligne)
            {
                int IdxInput = 0;
                while (Ligne.IndexOf("(") != -1)
                {
                    for (int IdxFerm = 1; IdxFerm < Ligne.Length; IdxFerm++)
                    {
                        char C = Ligne[IdxFerm];
                        if (C == ')')
                        {
                            int Idx = IdxFerm;
                            for (int IdxOuvr = IdxFerm; IdxOuvr > 0; IdxOuvr--)
                            {
                                Idx--;
                                char D = Ligne[IdxOuvr];
                                if (D == '(')
                                {
                                    Idx++;
                                    break;
                                }
                            }
                            string ToProcess = Ligne.Substring(Idx, IdxFerm - Idx + 1);
                            List<Node> NewNodes = ProcessConnection(ToProcess);

                            if (NewNodes == null) break;

                            string NewNodeName = "Input" + IdxInput++;

                            string Res = Ligne.Replace(ToProcess, NewNodeName);


                            double MinPosY = -1;
                            if (NewNodes[0].PosY < NewNodes[1].PosY)
                                MinPosY = NewNodes[0].PosY;
                            else
                                MinPosY = NewNodes[1].PosY;

                            Node NewNode = new Node(NewNodeName, -1, MinPosY + Math.Abs(NewNodes[0].PosY - NewNodes[1].PosY) / 2.0, NewNodes[0].PosXCorner);
                            OurTree.Add(NewNode);
                            Ligne = Res;
                            break;
                        }
                    }
                }
                return OurTree;
            }
        }

        private cOurTree OurTree;
        private System.Windows.Forms.Panel PanelDendogram;
        cGlobalInfo GlobalInfo;
        public cInfoForHierarchical InfoForHierarchical;

        private void GenerateDendogram(string ToProcess, string LinkType, string Distance)
        {
            ClassNewickParser NewickParser = new ClassNewickParser();
            NewickParser.OurDendo = this;
            FormDendogram DendogramDisplay = new FormDendogram();
            PanelDendogram = DendogramDisplay.panelForDendogram;


            DendogramDisplay.CurrentDendo = this;
            DendogramDisplay.Text = "Dendogram. " + Distance + " distance. Link type: " + LinkType;
            DendogramDisplay.Show();
            DendogramDisplay.GlobalInfo = GlobalInfo;

            OurTree = NewickParser.ParseString(ToProcess);
        }

        public class BhattacharyyaDistance : NormalizableDistance
        {

            /** for serialization. */
            private static long serialVersionUID = 1068606253458807903L;

            /**
             * Constructs an Euclidean Distance object, Instances must be still set.
             */
            public BhattacharyyaDistance()
            {
                //base.initialize();
            }

            /**
             * Constructs an Euclidean Distance object and automatically initializes the
             * ranges.
             * 
             * @param data         the instances the distance function should work on
             */
            public BhattacharyyaDistance(Instances data)
            {
                base.setInstances(data);
            }

            /**
             * Returns a string describing this object.
             * 
             * @return                 a description of the evaluator suitable for
             *                              displaying in the explorer/experimenter gui
             */
            public override String globalInfo()
            {
                return
                    "Implementing Euclidean distance (or similarity) function.\n\n"
                  + "One object defines not one distance but the data model in which "
                  + "the distances between objects of that data model can be computed.\n\n"
                  + "Attention: For efficiency reasons the use of consistency checks "
                  + "(like are the data models of the two instances exactly the same), "
                  + "is low.\n\n"
                  + "For more information, see:\n\n";
                //+ getTechnicalInformation().toString();
            }

            /**
             * Returns an instance of a TechnicalInformation object, containing 
             * detailed information about the technical background of this class,
             * e.g., paper reference or book this class is based on.
             * 
             * @return                 the technical information about this class
             */
            //public TechnicalInformation getTechnicalInformation() {
            //  TechnicalInformation       result;

            //  //result = new TechnicalInformation(Type.MISC);
            //  //result.setValue(Field.AUTHOR, "Wikipedia");
            //  //result.setValue(Field.TITLE, "Euclidean distance");
            //  //result.setValue(Field.URL, "http://en.wikipedia.org/wiki/Euclidean_distance");

            //  return result;
            //}

            /**
             * Calculates the distance between two instances.
             * 
             * @param first         the first instance
             * @param second    the second instance
             * @return                 the distance between the two given instances
             */
            public override double distance(Instance first, Instance second)
            {
                double Value = 0;
                for (int i = 0; i < first.numAttributes(); i++)
                {
                    Value += Math.Sqrt(first.value(i) * second.value(i));
                }

                return Value;
                // return Math.Sqrt(distance(first, second, Double.PositiveInfinity));
            }

            /**
             * Calculates the distance (or similarity) between two instances. Need to
             * pass this returned distance later on to postprocess method to set it on
             * correct scale. <br/>
             * P.S.: Please don't mix the use of this function with
             * distance(Instance first, Instance second), as that already does post
             * processing. Please consider passing Double.POSITIVE_INFINITY as the cutOffValue to
             * this function and then later on do the post processing on all the
             * distances.
             *
             * @param first         the first instance
             * @param second    the second instance
             * @param stats        the structure for storing performance statistics.
             * @return                 the distance between the two given instances or 
             *                              Double.POSITIVE_INFINITY.
             */
            public override double distance(Instance first, Instance second, PerformanceStats stats)
            { //debug method pls remove after use
                double Value = 0;


                for (int i = 0; i < first.numAttributes(); i++)
                {

                    Value += Math.Sqrt(first.value(i) * second.value(i));

                }


                return Value;
                //return Math.Sqrt(distance(first, second, Double.PositiveInfinity, stats));
            }



            /**
             * Updates the current distance calculated so far with the new difference
             * between two attributes. The difference between the attributes was 
             * calculated with the difference(int,double,double) method.
             * 
             * @param currDist   the current distance calculated so far
             * @param diff         the difference between two new attributes
             * @return                 the update distance
             * @see                     #difference(int, double, double)
             */
            protected override double updateDistance(double currDist, double diff)
            {
                double result;

                result = currDist;
                //result += diff * diff;
                result += diff;

                return result;
            }


            protected override double difference(int index, double val1, double val2)
            {
                return val1 * val2;
                //return base.difference(index, val1, val2);
            }



            /**
             * Does post processing of the distances (if necessary) returned by
             * distance(distance(Instance first, Instance second, double cutOffValue). It
             * is necessary to do so to get the correct distances if
             * distance(distance(Instance first, Instance second, double cutOffValue) is
             * used. This is because that function actually returns the squared distance
             * to avoid inaccuracies arising from floating point comparison.
             * 
             * @param distances the distances to post-process
             */
            public override void postProcessDistances(double[] distances)
            {
                for (int i = 0; i < distances.Length; i++)
                {
                    distances[i] = Math.Sqrt(distances[i]);
                }
            }

            /**
             * Returns the squared difference of two values of an attribute.
             * 
             * @param index      the attribute index
             * @param val1        the first value
             * @param val2        the second value
             * @return                 the squared difference
             */
            ////public double sqDifference(int index, double val1, double val2)
            ////{
            ////    double val = difference(index, val1, val2);
            ////    return val * val;
            ////}

            /**
             * Returns value in the middle of the two parameter values.
             * 
             * @param ranges     the ranges to this dimension
             * @return                 the middle value
             */
            //public double getMiddle(double[] ranges)
            //{

            //    double middle = ranges[R_MIN] + ranges[R_WIDTH] * 0.5;
            //    return middle;
            //}

            /**
             * Returns the index of the closest point to the current instance.
             * Index is index in Instances object that is the second parameter.
             *
             * @param instance the instance to assign a cluster to
             * @param allPoints all points
             * @param pointList             the list of points
             * @return                 the index of the closest point
            * @throws Exception          if something goes wrong
             */
            //public int closestPoint(Instance instance, Instances allPoints,
            //                                   int[] pointList)
            //{
            //    double minDist = int.MaxValue;
            //    int bestPoint = 0;
            //    for (int i = 0; i < pointList.Length; i++)
            //    {
            //        double dist = distance(instance, allPoints.instance(pointList[i]), Double.PositiveInfinity);
            //        if (dist < minDist)
            //        {
            //            minDist = dist;
            //            bestPoint = i;
            //        }
            //    }
            //    return pointList[bestPoint];
            //}

            /**
             * Returns true if the value of the given dimension is smaller or equal the
             * value to be compared with.
             * 
             * @param instance the instance where the value should be taken of
             * @param dim         the dimension of the value
             * @param value       the value to compare with
             * @return                 true if value of instance is smaller or equal value
             */
            //public luebool vaIsSmallerEqual(Instance instance, int dim,
            //                                                  double value)
            //{  //This stays
            //    return instance.value(dim) <= value;
            //}

            /**
             * Returns the revision string.
             * 
             * @return                 the revision
             */
            public override String getRevision()
            {
                return RevisionUtils.extract("$Revision: 5953 $");
            }



        }

        public cDendoGram(cGlobalInfo GlobalInfo, bool IsFullScreen)
        {
            this.GlobalInfo = GlobalInfo;

            this.InfoForHierarchical = null;

            if (IsFullScreen)
                this.InfoForHierarchical = GlobalInfo.CurrentScreen.CreateInstancesWithUniqueClasse();
            else
                this.InfoForHierarchical = GlobalInfo.CurrentScreen.GetCurrentDisplayPlate().CreateInstancesWithUniqueClasse();

            this.InfoForHierarchical.UpDateMinMax(GlobalInfo.CurrentScreen);

            weka.clusterers.HierarchicalClusterer HClusterer = new HierarchicalClusterer();

            string OptionDistance = " -A \"weka.core.";
            //string OptionDistance = "";

            switch (GlobalInfo.OptionsWindow.comboBoxHierarchicalDistance.SelectedIndex)
            {
                case 0:
                    //OptionDistance += "BhattacharyyaDistance";
                    OptionDistance += "EuclideanDistance";
                    break;
                case 1:
                    OptionDistance += "ManhattanDistance";
                    break;
                case 2:
                    OptionDistance += "ChebyshevDistance";
                    break;
                default:
                    break;
            }

            OptionDistance += " -R first-last\"";


            string[] TAGS_LINK_TYPE = { "SINGLE", "COMPLETE","AVERAGE", "MEAN", "CENTROID", "WARD", "ADJCOMPLETE"};

            string WekaOption = "-L " + TAGS_LINK_TYPE[GlobalInfo.OptionsWindow.comboBoxHierarchicalLinkType.SelectedIndex] + OptionDistance;

            HClusterer.setOptions(weka.core.Utils.splitOptions(WekaOption));

            //BhattacharyyaDistance Dist2 = new BhattacharyyaDistance();
            //HClusterer.setDistanceFunction(Dist2);

            HClusterer.setNumClusters(1);
            HClusterer.buildClusterer(this.InfoForHierarchical.Ninsts);

            GenerateDendogram(HClusterer.graph(), TAGS_LINK_TYPE[GlobalInfo.OptionsWindow.comboBoxHierarchicalLinkType.SelectedIndex], GlobalInfo.OptionsWindow.comboBoxHierarchicalDistance.SelectedItem.ToString());
        }


        public cOurTree GetTree()
        {
            return this.OurTree;
        }

        private Microsoft.Msagl.Drawing.Graph ComputeAndDisplayGraph()
        {
            int CurrentPos = 0;
            int NextReturnPos = CurrentPos;
            List<int> ListNodeId = new List<int>();


            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");

            //// Start with the nodes
            //while (NextReturnPos != -1)
            //{
            //        Microsoft.Msagl.Drawing.Node Currentnode = new Microsoft.Msagl.Drawing.Node(Id.ToString());
            //        Currentnode.Label.FontColor = Microsoft.Msagl.Drawing.Color.Red;
            //        Currentnode.LabelText = FinalLabel;
            //        graph.AddNode(Currentnode);
            //}

            //// now the connections
            //while (NextReturnPos != -1)
            //{
            //    string stringNodeIdxStart = StringToProcess1.Remove(StringToProcess1.IndexOf("-"));
            //    int NodeIdxStart = Convert.ToInt32(stringNodeIdxStart);

            //    string stringNodeIdxEnd = StringToProcess1.Remove(0, StringToProcess1.IndexOf("N") + 1);
            //    int NodeIdxSEnd = Convert.ToInt32(stringNodeIdxEnd);

            //    int LabelPos = DotString.IndexOf("label=");
            //    LabelPos += 7;

            //    string CurrLabelString = DotString.Remove(0, LabelPos);

            //    Microsoft.Msagl.Drawing.Edge Currentedge = new Microsoft.Msagl.Drawing.Edge(stringNodeIdxStart, ""/*NodeIdx.ToString()*/, stringNodeIdxEnd);
            //    Currentedge.LabelText = CurrLabelString.Remove(CurrLabelString.IndexOf("]") - 1);
            //    graph.Edges.Add(Currentedge);
            //}



            //CurrentControl.Graph = graph;
            //CurrentControl.LayoutEditingEnabled = true;

            return graph;


        }





    }
}
