using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Net.Mail;
using System.Diagnostics;

namespace HCSAnalyzer.Forms
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.richTextBoxAbout.AppendText("HCS Analyzer " + String.Format("Version {0}", AssemblyVersion) + "\n\n");
            this.richTextBoxAbout.AppendText(AssemblyCopyright + "\n");
            this.richTextBoxAbout.AppendText(AssemblyCompany + "\n\n");
            this.richTextBoxAbout.AppendText("Main developper: Thierry Dorval\n");
            this.richTextBoxAbout.AppendText("Additional developper: Arnaud Ogier\n\n");
            this.richTextBoxAbout.AppendText("Acknowledgement: H. K. Moon for the plugins developement support.\n");
            this.richTextBoxAbout.AppendText("V. Makarenkov and P. Dragiev for the B-score implementation.\n\n");
            this.richTextBoxAbout.AppendText("Webpage: " + "http://hcs-analyzer.ip-korea.org");
            this.richTextBoxAbout.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(Link_Clicked);
        }


        protected void Link_Clicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
           // if (GlobalInfo.OptionsWindow.radioButtonIE.Checked)
                proc.StartInfo.FileName = "iexplore";
          //  else
           //     proc.StartInfo.FileName = "chrome";
            proc.StartInfo.Arguments = e.LinkText;
            proc.Start();
            //Process.Start("mailto://dorvalt@ip-korea.org" + "?subject=HCS Analyzer feedback");
        }


        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion




    }
}
