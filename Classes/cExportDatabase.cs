using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibPlateAnalysis
{
    public class cExportDatabase
    {
        string MainPath;
        string CurrentPlateName;
        string CurrentPath;
    //    List<string> ListDescriptorNames;


        public cExportDatabase(string Path)
        {
            this.MainPath = Path;

           // this.ListDescriptorNames = new List<string>();

            //for (int i = 0; i < ListDescriptorName.Count; i++)
            //{
            //    string TmpString = ListDescriptorName[i];
            //    this.ListDescriptorNames.Add(TmpString);
            //}
        }
/*
        public void InitiateNewPlate(string PlateName)
        {
            this.CurrentPlateName = PlateName;
            IEnumerable<string> ListFile = Directory.EnumerateDirectories(this.MainPath);

            if (ListFile.Contains(PlateName))
            {
                CompleteScreening.ConsoleWriteLine("the plate already exist, saving process aborded");
                return;
            }

            this.CurrentPath = MainPath + "\\" + this.CurrentPlateName;
            Directory.CreateDirectory(this.CurrentPath);
        }
        */
        //public void AddWell(int Col, int Row, List<double[]> Values)
        //{
        //    if (this.ListDescriptorNames.Count != Values.Count)
        //    {
        //        CompleteScreening.ConsoleWriteLine("list values and list descriptors do not match ...");
        //        return;
        //    }

        //    string FileName = CurrentPath + "\\" + Col + "x" + Row + ".txt";
        //    StreamWriter stream = new StreamWriter(FileName, true, System.Text.Encoding.ASCII);

        //    for (int i = 0; i < ListDescriptorNames.Count; i++)
        //    {
        //        stream.Write(ListDescriptorNames[i]+"\t");
        //        foreach (double f in Values[i]) stream.Write(f + "\t");
        //        stream.WriteLine();
        //    }
        //    stream.Dispose();
        //}

        public void AddWell(string PlateName, int Col, int Row, List<cDescriptor> ListDescriptors)
        {
            IEnumerable<string> ListFile = Directory.EnumerateDirectories(this.MainPath);
            this.CurrentPath = MainPath + "\\" + PlateName;

            if (ListFile.Contains(PlateName)==false)
                Directory.CreateDirectory(this.CurrentPath);

            string FileName = CurrentPath + "\\" + Col + "x" + Row + ".txt";
            StreamWriter stream = new StreamWriter(FileName, false, System.Text.Encoding.ASCII);

            for (int i = 0; i < ListDescriptors.Count; i++)
            {
                stream.Write(ListDescriptors[i].GetName() + "\t");
                foreach (double f in ListDescriptors[i].Getvalues()) stream.Write(f + "\t");
                stream.WriteLine();
            }
            stream.Dispose();
        }





    }
}
