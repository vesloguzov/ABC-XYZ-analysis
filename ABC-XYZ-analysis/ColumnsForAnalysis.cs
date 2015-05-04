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
        public Boolean state = false;
        public Dictionary<string, int> local = new Dictionary<string, int>();
        public int NameIndex;

        private void ColumnsForAnalysis_Load(object sender, EventArgs e)
        {
            
            local = MainForm.getColumnsList();

            try
            {
                for (int i = 0; i < local.Count; i++)
                {
                    checkedListBox1.Items.Add(local.Keys.ToList()[i]); //добавляем имена колонок в листчекбокс
                    comboBox1.Items.Add(local.Keys.ToList()[i]);
                }
                comboBox1.SelectedIndex = 0;
                NameIndex = 0;
                local.Add("name", comboBox1.SelectedIndex);
                // checkedListBox1.SetItemChecked(1, true);
            }
            catch {
                MessageBox.Show("Ошибка!");
                Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                local.Clear(); // чистим локальный список имен столбцов
                for (int i = 0; i < checkedListBox1.Items.Count; i++) // перезаписываем текщий список, оставляем в нем только отмеченные столбцы
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        local.Add(checkedListBox1.Items[i].ToString(), i);
                    }
                }
                NameIndex = comboBox1.SelectedIndex;
                local.Add("name", NameIndex);

                if(local.Count == 1)
                {
                    throw new Exception("Отметьте нужные столбцы!");
                }

                
                MainForm.setColumnsList(local); // отдаем список в главную форму
                Close();
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            NameIndex = comboBox1.SelectedIndex;

            checkedListBox1.SetItemChecked(NameIndex, false);

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                if (i != NameIndex)
                {
                    checkedListBox1.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked));
                }
            state = !state;
        }
        
    }
}
