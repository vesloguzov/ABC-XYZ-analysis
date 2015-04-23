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
    public partial class ColumnsForAnalysis : Form
    {
        private Form1 MainForm;

        public ColumnsForAnalysis()
        {
            InitializeComponent();
        }

        public ColumnsForAnalysis(Form1 MainForm)
        {
        this.MainForm = MainForm;
           InitializeComponent();

        }
        public Dictionary<string, int> local = new Dictionary<string, int>();

        private void ColumnsForAnalysis_Load(object sender, EventArgs e)
        {

            local = MainForm.getColumnsList();

            for (int i = 0; i < local.Count; i++)
            {
                checkedListBox1.Items.Add(local.Keys.ToList()[i]); //добавляем имена колонок в листчекбокс
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           //List<string> lol = checkedListBox1.SelectedItems.OfType;
           //List<string>  lol1 = lol;
        }
    }
}
