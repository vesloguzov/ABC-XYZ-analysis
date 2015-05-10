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
        private MainForm MainForm;

        public ColumnsForAnalysis()
        {
            InitializeComponent();
        }

        public ColumnsForAnalysis(MainForm MainForm)
        {
        this.MainForm = MainForm;
           InitializeComponent();
           this.FormClosing += new FormClosingEventHandler(ColumnsForAnalysis_FormClosing);

        }
        public Boolean state = false;
        public Dictionary<string, int> local = new Dictionary<string, int>(); // локальный словарь колонок
        public int NameIndex; // Индекс столбца с именами

        private void ColumnsForAnalysis_Load(object sender, EventArgs e)
        {
            local = MainForm.getColumnsList(); // получаем колонки
            try
            {
                for (int i = 0; i < local.Count; i++)
                {
                    checkedListBox1.Items.Add(local.Keys.ToList()[i]); //добавляем имена колонок в листчекбокс
                    comboBox1.Items.Add(local.Keys.ToList()[i]);
                }
                comboBox1.SelectedIndex = 0; // по умолчанию  выбираем нулевой элемент
                NameIndex = 0;
                local.Add("name", comboBox1.SelectedIndex); // записывеам в словарь индекс столбца с именами
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
                NameIndex = comboBox1.SelectedIndex; // получаем индекс столбца с именами
                local.Add("name", NameIndex); // записывеам в словарь индекс столбца с именами

                if(local.Count == 1)
                {
                    throw new Exception("Отметьте нужные столбцы!");
                }

                
                MainForm.setColumnsList(local); // отдаем словарь в главную форму
                Dispose();
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
            /***
             * отмечаем/снимаем значения чекбоксов всех,
             * кроме того, который выбран как столбец имен
            ***/

            NameIndex = comboBox1.SelectedIndex; // получаем индекс столбца с именами

            checkedListBox1.SetItemChecked(NameIndex, false); // убираем все галочки

            for (int i = 0; i < checkedListBox1.Items.Count; i++) // идем по всем элементам checkedListBoxа
                if (i != NameIndex) // если это не столбец с именами
                {
                    checkedListBox1.SetItemCheckState(i, (state ? CheckState.Checked : CheckState.Unchecked)); // отмечаем или снимаем
                }
            state = !state;
        }

        private void ColumnsForAnalysis_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            //  DialogResult ans
            //  MessageBox.Show("No");
            e.Cancel = DialogResult.Yes != MessageBox.Show("Прервать операцию?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (e.Cancel == false)
            {
                // если операция прервана, отдаем пустой словарь
                MainForm.setColumnsList(new Dictionary<string,int>());
            }
        }  
        
    }
}
