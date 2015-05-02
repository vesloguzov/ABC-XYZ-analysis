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
        private List<Product> ProductsList = new List<Product>(); // все товары, основной список
        //private Dictionary<string>

        private Dictionary<string, string> ExcelFileSettings = new Dictionary<string, string>(); // словарь с настройками для загружаемого excel файла

        public Dictionary<string, int> ColumnsList = new Dictionary<string, int>();//имена колонок и их номера входной таблицы
        
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }
        /*
         * геттреы/ сеттеры для передачи 
         * данных между формами
         */
        internal List<Product> getProductsList()
        {
            return ProductsList;
        }
        internal void setProductsList(List<Product> ProductsList)
        {
            this.ProductsList = ProductsList;
        }
        public Dictionary<string, int> getColumnsList()
        {
            return ColumnsList;
        }
        public void setColumnsList(Dictionary<string, int> ColumnsList)
        {
            this.ColumnsList = ColumnsList;
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
            /***
             * метод загружает файл Excel в datagridview
             * здесь же вызывается форма выбора листа
             * и установление флажка "в первой строке - имена стролбцов"
             ***/

            dataGridView1.DataSource = null;
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
                  
                    
                    new LoadExcelFileSettings(this).ShowDialog(); // открываем новое окно настроек загрузки файла

                    //Если данное значение установлено в true
                    //то первая строка используется в качестве 
                    //заголовков для колонок
                  
                        if (ExcelFileSettings["checked_heads"] == "1")
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
                        try
                        {   
                        dataGridView1.DataSource = ds1.Tables[checked_table];   // рисуем выбранный лист
                        label1.Text = "Путь к файлу: " + ofd.FileName;
                    }
                    catch
                    {
                    }

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
        private List<Product> DataToDictionary(Dictionary<string, int> columns)
        {
            /****
             *  Метод переводит данные из DataGridView в 
             *  список типа List<Product>. Входное значение - 
             *  словарь с выбранными колонками (колонки со значениями 
             *  продаж и колонка имени).
             ***/
            List<Product> products = new List<Product>(); // локальный список товаров
            int NameColumnIndex = columns["name"]; // узнаем в каком столбце имена товаров
            columns.Remove("name");

           // bool correct = true;

            try
            {

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    Product product;
                    List<double> values_analysis = new List<double>();

                    for (int j = 0; j < columns.Count; j++)
                    {
                        values_analysis.Add(Convert.ToDouble(dataGridView1.Rows[i].Cells[columns.Values.ToList()[j]].Value)); // в список values_analysis записываем данные объемов продаж 
                    }

                    product = new Product(i, dataGridView1.Rows[i].Cells[NameColumnIndex].Value.ToString(), values_analysis); // создаем экземпляр продукта (номер, имя, значения объемов продаж)
                    product.CalculateSumAndAverage(product); // считаем дополнительные поля для продукта 
                    products.Add(product); // добавляем продукт в список

                    //setProductsList(products);
                }

            }
            catch {
                MessageBox.Show("Ошибка в данных. Убедитесь, нет пустых значений ячеек!");
            }
           // label2.Text = "Complete";
            return products;
             }

        private void ColumnsForAnalysis()
        {
            /***
             * в методе происходит работа с выбором
             * колонок, а также данные сохраняются в 
             * ProductsList (до этого хранились только в
             * datagridview)
             ***/
 
            setProductsList(new List<Product>()); // очищаем список продуктов

            ColumnsList.Clear();
            for (int i = 0; i < dataGridView1.Columns.Count; i++) // собираем имена колонок
            {
                ColumnsList.Add(dataGridView1.Columns[i].Name, i);
            }
            new ColumnsForAnalysis(this).ShowDialog();
            ColumnsList = getColumnsList();

            ProductsList = DataToDictionary(ColumnsList);
            label2.Text = "Complete";
        
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ColumnsForAnalysis();
            EstimatesToDataGridView(ProductsList, ColumnsList);
            Form2 f2 = new Form2();
            //f2.Show();
             f2.listView2.Items.Clear();

        }

        private void EstimatesToDataGridView(List<Product> ListOfProducts, Dictionary<string, int> ColumnsList)
        {
            /***
             * метод удаляет из datagridview данные,
             * который были загружены изначально и 
             * загружает в него те данные (столбцы), которые были 
             * отобраны. к этому этапу мы должны иметь на 100% праввильные
             * данные для анализа
             ***/

            //dataGridView1.Rows.Clear(); 
            dataGridView1.DataSource = null; // полность очищаем datagridview

            dataGridView1.ColumnCount = 2 + ColumnsList.Count(); // уазываем количество колонок (2 колонки - имя, номер. Остальные колонки с данными)
            dataGridView1.Columns[0].Name = "Номер"; // имя колонки
            dataGridView1.Columns[1].Name = "Имя продукта"; // имя колонки

            for (int i = 0; i < ColumnsList.Count; i++)
            {
                dataGridView1.Columns[i + 2].Name = ColumnsList.Keys.ToList()[i]; // добавляем имена колонок остальных
            }

            for (int i = 0; i < ProductsList.Count; i++)
            {
                List<string> row = new List<string> { ProductsList[i].number.ToString(), ProductsList[i].name.ToString()}; // создаем строку для добавления в datagridview, добавляем в нее имя и номер продукта
                
                for (int j = 0; j < ProductsList[i].values_analysis.Count; j++)
                {
                    row.Add(ProductsList[i].values_analysis[j].ToString()); // добавляем в строку значения данных для расчета(колонки объемов продаж)
                }
                    dataGridView1.Rows.Add(row.ToArray<string>()); // добавляем строку в datagridview
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            /***
             * подтверждения выхода из основнй программы
            ***/

            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = DialogResult.Yes != MessageBox.Show("Вы действительно хотите выйти ?", "Внимание",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
