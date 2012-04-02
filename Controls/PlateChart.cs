using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace HCSAnalyzer
{
    public class PlateChart : Chart
    {
        public PlateChart()
        {
            SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
}
