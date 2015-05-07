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
    public partial class XYZTable : Form
    {
        private Form1 MainForm;

        public XYZTable()
        {
            InitializeComponent();
        }

        public XYZTable(Form1 MainForm)
        {
            this.MainForm = MainForm;
            InitializeComponent();
          }
        private void XYZTable_Load(object sender, EventArgs e)
        {
            List<Product> local = MainForm.getProductsList();
            local = Product.SortList(local, "number");

            for (int i = 0; i < local.Count; i++)
            {
                if (local[i].groupXYZ == "X")
                {
                    listBoxGroupX.Items.Add(local[i].number.ToString() + ". " + local[i].name);
                }
                if (local[i].groupXYZ == "Y")
                {
                    listBoxGroupY.Items.Add(local[i].number.ToString() + ". " + local[i].name);
                }
                if (local[i].groupXYZ == "Z")
                {
                    listBoxGroupZ.Items.Add(local[i].number.ToString() + ". " + local[i].name);
                }
            }
        }

        private void показатьРасчетнуюТаблицуToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void показатьРасчетнуюТаблицуToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            new CalculationXYZtable(MainForm).ShowDialog();
        }
    }
}
