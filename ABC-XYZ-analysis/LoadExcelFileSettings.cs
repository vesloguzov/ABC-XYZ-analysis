using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABC_XYZ_analysis
{
    public partial class LoadExcelFileSettings : Form
    {
        private Form1 MainForm;

        public LoadExcelFileSettings()
        {
            InitializeComponent();
        }

        public LoadExcelFileSettings(Form1 MainForm)
        {
        this.MainForm = MainForm;
           InitializeComponent();
        }

        private void LoadExcelFileSettings_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> settings = MainForm.getExcelFileSettings();
            label1.Text = settings["tables_count"];
            label1.Text = settings["tables_count"];
            label1.Text = settings["tables_count"];
            label1.Text = settings["tables_count"];

        }
    }
}
