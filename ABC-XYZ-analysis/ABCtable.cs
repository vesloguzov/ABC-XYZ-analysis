﻿using System;
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
            /*
            Opacity = 0;
            Timer timer = new Timer();
            timer.Tick += new EventHandler((sender, e) =>
            {
                if ((Opacity += 0.05d) == 1) timer.Stop();
            });
            timer.Interval = 10;
            timer.Start();
             */
        }

        public ABCtable(Form1 MainForm)
        {
            this.MainForm = MainForm;
            InitializeComponent();
          }

        private void ABCtable_Load(object sender, EventArgs e)
        {
            List<Product> local = MainForm.getProductsList();
            local = Product.SortList(local, "number");

            for (int i = 0; i < local.Count; i++)
            {
                if (local[i].groupABC == "A")
                {
                    listBoxGroupA.Items.Add(local[i].number.ToString()+". "+local[i].name);
                }
                if (local[i].groupABC == "B")
                {
                    listBoxGroupB.Items.Add(local[i].number.ToString() + ". " + local[i].name);
                }
                if (local[i].groupABC == "C")
                {
                    listBoxGroupC.Items.Add(local[i].number.ToString() + ". " + local[i].name);
                }
            }

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void показатьПромеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CalculationABCtabte(MainForm).ShowDialog();
        }
    }
}
