using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FOIE;

namespace FOIE
{
    public partial class SettingsForm : Form
    {


        public SettingsForm()
        {
            InitializeComponent();

            cfgFileNameLabel.Text = Editor.cfgPath;
        }

        private void settingsButton1_Click(object sender, EventArgs e)
        {
           changeConfigFile();
        }


        string newcfg = "";
        private void changeConfigFile()
        {
            newcfg = getFileName("CFG file|*.cfg", true);
            if (newcfg != null)
            {
                cfgFileNameLabel.Text = newcfg;
            }
            
        }

        private string getFileName(string Filter, bool sameFolderRequired)
        {
            string AppPath = Path.GetDirectoryName(Application.ExecutablePath); ;
            openFileDialog1.InitialDirectory = AppPath;
            openFileDialog1.Filter = Filter;

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return null;
            }
            else
            {
                if (sameFolderRequired)
                {
                    while (!openFileDialog1.FileName.Contains(openFileDialog1.InitialDirectory))
                    {
                        MessageBox.Show("Please select file which is in the default folder", "Wrong folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        openFileDialog1.ShowDialog();
                    }
                }
                return System.IO.Path.GetFileName(openFileDialog1.FileName);
                //return openFileDialog1.FileName;
            }
        }

        //public string ReturnValue1 { get; set; }
        private void buttonOk_Click(object sender, EventArgs e)
        {
            //this.ReturnValue1 = "Something";
            if (newcfg != null)
            {
                Editor.cfgPath = newcfg;
            }
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
