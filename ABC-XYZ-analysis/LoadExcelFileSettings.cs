using System;
using ABC_XYZ_analysis.Properties;
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
        private MainForm MainForm;

        public LoadExcelFileSettings()
        {
            InitializeComponent();
            //this.FormClosing += new FormClosingEventHandler(LoadExcelFileSettings_FormClosing);
        }

        public LoadExcelFileSettings(MainForm MainForm)
        {
           this.MainForm = MainForm;
           InitializeComponent();
           this.FormClosing += new FormClosingEventHandler(LoadExcelFileSettings_FormClosing);
          }

        string checked_table = "0";
        string checked_heads = "0";

        public Dictionary<string, string> local = new Dictionary<string, string>();

        public Exception Exc()
        {
            return new Exception();
        }

        public void LoadExcelFileSettings_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            Dictionary<string, string> settings = MainForm.getExcelFileSettings();
            local = MainForm.getExcelFileSettings();
            local.Add("checked_table", checked_table);
            local.Add("checked_heads", checked_heads);
            string s = settings["tables_names"]; // имена таблиц 
            String[] tables_names = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries); // парсим имена в массив
            string file_name = settings["file_name"];// имя самого файла
            label3.Text = "Вы собираетесь загрузить файл: "+file_name;
            label1.Text = "Листов в Вашем документе: " + settings["tables_count"] + ". Пожалуйста, выберите нужный для работы лист.";
            for (int i = 0; i < tables_names.Length; i++)
            {
                comboBox1.Items.Add(tables_names[i]); // добавляем имена в комбобокс
            }
            comboBox1.SelectedIndex = 0; // по дефолту выбран первый лист
            MainForm.setExcelFileSettings(local);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checked_table = comboBox1.SelectedIndex.ToString(); // номер выбранной таблицы
            //string checked_heads = "";
            if (checkBox1.Checked)
            {
                checked_heads = "1"; // есть ли заголовки
            }
            else
            {
                checked_heads = "0";
            }
            local["checked_table"] = checked_table;
            local["checked_heads"] = checked_heads;
            MainForm.setExcelFileSettings(local);
            Dispose();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        
        private void LoadExcelFileSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            
               if (e.CloseReason == CloseReason.UserClosing)
                    e.Cancel = true;
            
           //throw new Exception("Загрузка была прервана!");
         
        }
        

        /*
        private void LoadExcelFileSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
          //  DialogResult ans
          //  MessageBox.Show("No");
           e.Cancel = DialogResult.Yes != MessageBox.Show("Прервать загрузку документа?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
           if (e.Cancel == false)
           {
               local["checked_table"] = "-1";
           }
        }*/
         
    }
}
