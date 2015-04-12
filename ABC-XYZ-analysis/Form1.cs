using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using ExcelObj = Microsoft.Office.Interop.Excel;
using ABC_XYZ_analysis.Properties;

namespace ABC_XYZ_analysis
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> ExcelFileSettings = new Dictionary<string, string>(); // словарь с настройками для загружаемого excel файла
        public Form1()
        {
            InitializeComponent();
        }
        public Dictionary<string, string> getExcelFileSettings()
        {
            return ExcelFileSettings;
        }

        public void setExcelFileSettings(Dictionary<string, string> ExcelFileSettings)
        {
            this.ExcelFileSettings = ExcelFileSettings;
        }

        public void LoadExclelFile()
        {
            
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.xls;*.xlsx";
            ofd.Filter = "Excel Sheet(*.xlsx)|*.xlsx";
            ofd.Title = "Выберите документ для загрузки данных";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    
                    System.IO.FileStream stream =
                    System.IO.File.Open(ofd.FileName,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);
                    Excel.IExcelDataReader IEDR;
                    int fileformat = ofd.SafeFileName.IndexOf(".xlsx");
                    if (fileformat > -1)
                    {
                        //2007 format *.xlsx
                        IEDR = Excel.ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        //97-2003 format *.xls
                        IEDR = Excel.ExcelReaderFactory.CreateBinaryReader(stream);
                    }

                    DataSet ds = IEDR.AsDataSet();
                    int tables_count = ds.Tables.Count; //количество таблиц в файле
                    string tables_names = ""; // имена таблиц
                    for (int i = 0; i < tables_count; i++)
                    {
                        tables_names += ds.Tables[i].TableName + ","; // собираем имена
                    }

                    ExcelFileSettings.Add("tables_count", tables_count.ToString()); // заполняем данные
                    ExcelFileSettings.Add("tables_names",tables_names);
                    ExcelFileSettings.Add("file_name",ofd.SafeFileName);
                    new LoadExcelFileSettings(this).ShowDialog(); // открываем новое окно

                    //Если данное значение установлено в true
                    //то первая строка используется в качестве 
                    //заголовков для колонок
                    
                    if (ExcelFileSettings["checked_heads"]=="1")
                    {
                        IEDR.IsFirstRowAsColumnNames = true;
                    }
                    else
                    {
                        IEDR.IsFirstRowAsColumnNames = false;
                    }

                    int checked_table = 0; // номер выбранного листа
                    checked_table = Convert.ToInt32(ExcelFileSettings["checked_table"]); // получаем этот номер
                    
                    DataSet ds1 = IEDR.AsDataSet();

                    //Устанавливаем в качестве источника данных dataset 
                    //с указанием номера таблицы. Номер таблицы указавает 
                    //на соответствующий лист в файле нумерация листов 
                    //начинается с нуля.
                    label1.Text = "Путь к файлу: " + ofd.FileName;
                    dataGridView1.DataSource = ds1.Tables[checked_table];   // рисуем выбранный лист
                    IEDR.Close();
                    ExcelFileSettings.Clear();
                }

                catch
                {
                  MessageBox.Show("Произошла ошибка при загрузке файла. Проверьте, не открыт ли у Вас загружаемый документ.");//обработка всех исключений (например, если файл открыт а Экселе)
                }

            }

            else
            {
                MessageBox.Show("Вы не выбрали файл для открытия",
                 "Загрузка данных...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadExclelFile();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
