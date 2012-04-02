using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HCSAnalyzer;
using HCSAnalyzer.Classes;
using LibPlateAnalysis;

namespace HCSPlugin
{
	public partial class Plugin : Form
	{
        protected Plugin()
		{
            InitializeComponent();
		}

        public static cScreening CurrentScreen;

	}
}
