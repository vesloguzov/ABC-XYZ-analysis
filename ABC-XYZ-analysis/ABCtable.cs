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
    public partial class ABCtable : Form
    {
        private Form1 MainForm;

        public ABCtable()
        {
            InitializeComponent();
        }

        public ABCtable(Form1 MainForm)
        {
            this.MainForm = MainForm;
            InitializeComponent();
          }

        private void ABCtable_Load(object sender, EventArgs e)
        {
            List<Product> local = MainForm.getProductsList();
            //local = Product.SortList();

            for (int i = 0; i < local.Count; i++)
            {
                if (local[i].group == "A")
                {
                    listBoxGroupA.Items.Add(local[i].number.ToString()+". "+local[i].name);
                }
                if (local[i].group == "B")
                {
                    listBoxGroupB.Items.Add(local[i].number.ToString() + ". " + local[i].name);
                }
                if (local[i].group == "C")
                {
                    listBoxGroupC.Items.Add(local[i].number.ToString() + ". " + local[i].name);
                }
            }

        }
    }
}
