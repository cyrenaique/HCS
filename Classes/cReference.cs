using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibPlateAnalysis;

namespace HCSAnalyzer.Classes
{
    public class cReference : List<cExtendedList>
    {



        public cReference(List<cWell> WellsForReference)
        {


            foreach (cDescriptor Desc in WellsForReference[0].ListDescriptors)
            {
                cExtendedList NewList = new cExtendedList();


                
                for (int i = 0; i < Desc.GetAssociatedType().GetBinNumber(); i++)
                {
                    double CurrentVal = 0;
                    foreach (cWell CurrentWell in WellsForReference)
                    {

                        CurrentVal += Desc.Getvalue(i);
                    }
                    CurrentVal /= (double)WellsForReference.Count;
                    NewList.Add(CurrentVal);
                }
                this.Add(NewList);
            }



        }

    }
}
