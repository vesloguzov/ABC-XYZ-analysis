﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using ExcelApp = Microsoft.Office.Interop.Excel;
using ABC_XYZ_analysis.Properties;

//using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;

namespace ABC_XYZ_analysis
{
    /***
        * Что можно реализовать интересного:
        * 1.
        * 2. 
        * 3. Можно даже рисовать графики в Excel (есть библиотека)
        * 4.
        * 5.Изменить прогресс-бар на начальной форме
        * 6.Теория расчета (если делать не будем, то удалить из MenuStrip)
        * 7.
        * 
        * 
        * Что надо поправить:
        * 1. Закрытие форм
        * 2. 
        * 3.
        * 
        * 
        * Что обязательно нужно реализовать:
        * 1. Добавить алерт в форму ColumnsForAnalysis.cs при закрытии
        * 2.
        * 3.
    ***/

    public partial class MainForm : Form
    {
        private List<Product> ProductsList = new List<Product>(); // все товары, основной список
        //private Dictionary<string>

        private Dictionary<string, string> ExcelFileSettings = new Dictionary<string, string>(); // словарь с настройками для загружаемого excel файла

        public Dictionary<string, int> ColumnsList = new Dictionary<string, int>();//имена колонок и их номера входной таблицы
        
        public MainForm()
        {
            // стартовая форма открывается
            new StartForm().ShowDialog();

            InitializeComponent();
            
             
            //делаем плавное появление формы
              
            Opacity = 0;
            Timer timer = new Timer();
            timer.Tick += new EventHandler((sender, e) =>
            {
                if ((Opacity += 0.05d) == 1) timer.Stop();
            });
            timer.Interval = 10;
            timer.Start();
            

            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
            dataGridView1.DataError += new DataGridViewDataErrorEventHandler(DataGridView1_DataError);
            this.dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(DataGridView1_CellValueChanged);
        }
        public void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            //MessageBox.Show("Ошибка ввода!");
            anError.ThrowException = false;
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
        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadExclelFile();
        }

        public void LoadExclelFile()
        {
            /***
             * метод загружает файл Excel в datagridview
             * здесь же вызывается форма выбора листа
             * и установление флажка "в первой строке - имена стролбцов"
             ***/

            

            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.DefaultExt = "*.xls;*.xlsx";
            ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
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
                         dataGridView1.Columns.Clear();
                        dataGridView1.DataSource = ds1.Tables[checked_table];   // рисуем выбранный лист
                        CleanNullRowsColumns(); //
                       // richTextBox1.Text += "Открыт файл: " + ofd.SafeFileName + "\n";

                        label1.Text = "Путь к файлу: " + ofd.FileName;
                    }
                    catch
                    {
                    }

                    IEDR.Close();
                    ExcelFileSettings.Clear();
                    UpdateForm();
                }

                catch
                {
                    richTextBox1.Text += "Ошибка при загрузке файла" + "\n";
                  MessageBox.Show("Произошла ошибка при загрузке файла. Проверьте, не открыт ли у Вас загружаемый документ.");//обработка всех исключений (например, если файл открыт а Экселе)
                }

            }

            else
            {
                MessageBox.Show("Вы не выбрали файл для открытия",
                 "Загрузка данных...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            try
            {
                int NameColumnIndex = columns["name"]; // узнаем в каком столбце имена товаров
                columns.Remove("name");

                //List<DataGridCell> invalid_cells = new List<DataGridCell>();

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    Product product;
                    List<double> values_analysis = new List<double>();


                    for (int j = 0; j < columns.Count; j++)
                    {
                        try
                        {
                            values_analysis.Add(Convert.ToDouble(dataGridView1.Rows[i].Cells[columns.Values.ToList()[j]].Value)); // в список values_analysis записываем данные объемов продаж 
                            // dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;        

                        }
                        catch
                        {
                            // invalid_cells.Add(new DataGridCell(i,j));
                            // continue;
                        }
                    }

                    product = new Product(i + 1, dataGridView1.Rows[i].Cells[NameColumnIndex].Value.ToString(), values_analysis); // создаем экземпляр продукта (номер, имя, значения объемов продаж)
                    product.CalculateSumAndAverage(product); // считаем дополнительные поля для продукта 
                    products.Add(product); // добавляем продукт в список
                }

                for (int i = 0; i < products.Count; i++) // добавляем к продуктам значение поля "процент"
                {
                    products[i].percent = Product.Share(products[i].sum_values, Product.CalculateTotalSum(products));
                }

            }
            catch
            {
                MessageBox.Show("Ошибка в данных. Убедитесь, что нет пустых значений ячеек!");
            }
            return products;
        }

        private Dictionary<string, int> ColumnsForAnalysis()
        {
            /***
             * в методе происходит работа с выбором
             * колонок
             ***/

            Dictionary<string, int> columnsList = new Dictionary<string, int>();

            for (int i = 0; i < dataGridView1.Columns.Count; i++) // собираем имена колонок
            {
                columnsList.Add(dataGridView1.Columns[i].Name, i);
            }

            setColumnsList(columnsList);
            new ColumnsForAnalysis(this).ShowDialog();
            columnsList = getColumnsList();
            setColumnsList(new Dictionary<string, int>());

            return columnsList;
        }

        private void EstimatesToDataGridView(List<Product> ListOfProducts, Dictionary<string, int> columnsList)
        {
            /***
             * метод удаляет из datagridview данные,
             * который были загружены изначально и 
             * загружает в него те данные (столбцы), которые были 
             * отобраны. к этому этапу мы должны иметь на 100% праввильные
             * данные для анализа
             ***/
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = null; // это тут должно быть обязательно!

            dataGridView1.ColumnCount = (2 + columnsList.Count());
            /*
            dataGridView1.ColumnCount = (2 + columnsList.Count()) + 8; // указываем количество колонок (2 колонки - имя, номер. Остальные колонки с данными + 2 колонки (среднее значение и сумма по периодам) + 1 доля в позици + 1 доля нарастающим итогом + 1 группа) + 3 для XYZ
            */
            dataGridView1.Columns[0].Name = "Номер"; // имя колонки
            dataGridView1.Columns[1].Name = "Имя продукта"; // имя колонки

            for (int i = 0; i < columnsList.Count; i++)
            {
                dataGridView1.Columns[i + 2].Name = columnsList.Keys.ToList()[i]; // добавляем имена колонок остальных
            }

            // добавляем колонки
            /*dataGridView1.Columns[dataGridView1.ColumnCount - 8].Name = "Сумма"; 
            dataGridView1.Columns[dataGridView1.ColumnCount - 7].Name = "Среднее значение";
            dataGridView1.Columns[dataGridView1.ColumnCount - 6].Name = "Процент";
            dataGridView1.Columns[dataGridView1.ColumnCount - 5].Name = "Нарастающим итогом";
            dataGridView1.Columns[dataGridView1.ColumnCount - 4].Name = "Группа ABC";
            dataGridView1.Columns[dataGridView1.ColumnCount - 3].Name = "Стандартное отклонение";
            dataGridView1.Columns[dataGridView1.ColumnCount - 2].Name = "Коэффициент вариации";
            dataGridView1.Columns[dataGridView1.ColumnCount - 1].Name = "Группа XYZ";
            */

            for (int i = 0; i < ProductsList.Count; i++)
            {
                List<string> row = new List<string> { ProductsList[i].number.ToString(), ProductsList[i].name.ToString() }; // создаем строку для добавления в datagridview, добавляем в нее имя и номер продукта

                for (int j = 0; j < ProductsList[i].values_analysis.Count; j++)
                {
                    row.Add(ProductsList[i].values_analysis[j].ToString()); // добавляем в строку значения данных для расчета(колонки объемов продаж)
                }

                /*
                row.Add(ProductsList[i].sum_values.ToString());  // добавляем в строку сумму объемов продаж за периоды
                row.Add(ProductsList[i].average_value.ToString()); // добавляем в строку среднее значение объемов продаж за периоды
                row.Add(ProductsList[i].percent.ToString()); // добавляем в строку процент
                row.Add(ProductsList[i].growing_percent.ToString()); // добавляем в строку нарастающий итог
                row.Add(ProductsList[i].groupABC); // добавляем группу ABC
                row.Add(ProductsList[i].standard_deviation.ToString());// добавляем в строку стендартное отклонение
                row.Add(ProductsList[i].coefficient_of_variation.ToString());// добавляем в строку коэфф вариации
                row.Add(ProductsList[i].groupXYZ);// добавляем в строку группу XYZ
                */



                dataGridView1.Rows.Add(row.ToArray<string>()); // добавляем строку в datagridview



            }

           // richTextBox1.Text += "Полная сумма: " + Product.CalculateTotalSum(ProductsList).ToString() + "\n";

            dataGridView1.Columns[0].Width = 45; // задаем ширину столбца "номер товара"

            foreach (DataGridViewColumn column in dataGridView1.Columns) // запрещение сортироки (клик на имя столбца)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


        }

        private List<Product> Deviation_Variation_GroupXYZ(List<Product> list, int value1, int value2)
        {
            /***
             * метод всем товарам считает стандартное отклонения,
             * считает коэффициент вариации,
             * присваивает группу XYZ
            ***/

            list = Product.StandartDeviation(list); // присваиваем товарам стандартное отклонение
            list = Product.CoefficientOfVariation(list); // присваиваем коэффициент вариации

            // назначаем группы
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].coefficient_of_variation < value1)
                {
                    list[i].groupXYZ = "X";
                }
                else
                {
                    if (list[i].coefficient_of_variation < value2)
                    {
                        list[i].groupXYZ = "Y";
                    }
                    else
                    {
                        list[i].groupXYZ = "Z";
                    }
                }
            }
            return list;
        }

        private List<Product> Sort_GrowingPercent_GroupABC(List<Product> list, int value1, int value2)
        {
            /***
             * метод сортирует лист по процентам,
             * присваевает нарастающий итог,
             * назначает группу ABC
            ***/

            list = Product.SortList(list, "percent"); // сортируем список товаров
            Product.GrowingPercent(list); // отсортированному списку присваиваем нарастающий итог

            //присваиваем группы
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].growing_percent < value1)
                {
                    list[i].groupABC = "A";
                }
                else
                {
                    if (list[i].growing_percent < value2)
                    {
                        list[i].groupABC = "B";
                    }
                    else
                    {
                        list[i].groupABC = "C";
                    }
                }
            }

            return list;
        }

        private void CleanNullRowsColumns()
        {
            /***
             * метод удаляет из dataGridView
             * пустые строки, столбцы.
             * Если среди оставщихся клеток есть пустые,
             * в записывается значение "0"
            ***/

            //цикл удаляет пустые строки
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                double param = 0;
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    if (dataGridView1.Rows[i].Cells[j].Value.ToString() != "")
                    {
                        param = 1;
                        break;
                    }
                }
                if (param == 0)
                {
                    dataGridView1.Rows.RemoveAt(i);
                    i = 0;
                }

            }

            // цикл удаляет пустые столбцы
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                double param = 0;
                for (int j = 0; j < dataGridView1.Rows.Count - 1; j++)
                {
                    if (dataGridView1.Rows[j].Cells[i].Value.ToString() != "")
                    {
                        param = 1;
                        break;
                    }
                }
                if (param == 0)
                {
                    dataGridView1.Columns.RemoveAt(i);
                    i = 0;
                }

            }

            //цикл вставляет нули в пустые клетки
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    if (dataGridView1.Rows[i].Cells[j].Value.ToString() == "")
                    {
                        dataGridView1.Rows[i].Cells[j].Value = "0";
                    }
                }
            }
        }

        private void ExportToExcel()
        {
            /***
             * метод записывает данные из dataGridView в файл Excek
            ***/
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Files|*.xls"; // отображаемый тип

            sfd.FileName = "Исходные данные"; // имя по умолчанию

            DialogResult drSaveFile = sfd.ShowDialog();
            try
            {
                if (drSaveFile == System.Windows.Forms.DialogResult.OK)
                {
                    ExcelApp.Application ExcelApp = new ExcelApp.Application();
                    ExcelApp.Application.Workbooks.Add(Type.Missing);

                    //ExcelApp.ActiveWorkbook.FileFormat = XlFileFormat.xlExcel8;   
                    // Change properties of the Workbook   
                    ExcelApp.Columns.ColumnWidth = 25;
                    //ExcelApp.Rows = Color.Red;
                    // Storing header part in Excel

                    for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                    {
                        ExcelApp.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText; // записываем заголовки dataGridView

                    }

                    // Storing Each row and column value to excel sheet   
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            ExcelApp.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString(); // записываем остальную таблицу
                            // ExcelApp.Cells.BorderAround
                        }
                    }

                    //Save Copy by giving file Path   
                    // ExcelApp.ActiveWorkbook.SaveCopyAs("C:\\" + FileName);   

                    //OR using SaveFileDialog   
                    //ExcelApp.ActiveWorkbook.SaveCopyAs(sfd.FileName);

                    //OR even you can use SaveAs function   
                    ExcelApp.ActiveWorkbook.SaveAs(sfd.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8, null, null, null,
                     null, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, null, null, null, null, null);

                    FileInfo fileInfo = new FileInfo(sfd.FileName); // получаем инфо о файле
                    ExcelApp.ActiveWorkbook.Saved = true; // сохраняем файл
                    MessageBox.Show("Файл ''" + fileInfo.Name + "'' успешно сохранен в каталог: " + fileInfo.DirectoryName);
                    richTextBox1.Text += "Файл ''" + fileInfo.Name + "'' успешно сохранен" + "\n";
                    ExcelApp.Quit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохрании файла: " + ex.Message);
                richTextBox1.Text += "Ошибка при сохрании файла." + "\n";
            }
        }

        private void AddColumn(string column_name)
        {
            /***
             * метод добавляет столбец в dataGridView
             * на входе имя столбца
            ***/
            var new_column = new DataGridViewTextBoxColumn(); // создем колонку
            new_column.HeaderText = column_name; // присваиваем текст в заголовке
            new_column.Name = column_name; // присваиваем имя
            //dataGridView1.Columns.Count = dataGridView1.Columns.Count;
            if (checkBox1.Checked) // если отмечен "добавить в начало"
            {
                dataGridView1.Columns.Insert(0, new_column); // добавляем столбец в нулевую позицию
                MessageBox.Show("Столбец успешно добавлен в начало");
            }
            else // если не отмечен
            {
                dataGridView1.Columns.Insert(dataGridView1.Columns.Count, new_column); // добавляем в конец
                MessageBox.Show("Столбец успешно добавлен в конец");
            }
            New_column_name.Text = ""; // очищаем input "имя столбца"
        }

        private void DeleteColumn(int index)
        {
            /***
             * метод по индексу удаляет столбец из dataGridView
            ***/
            dataGridView1.Columns.RemoveAt(index);
            UpdateForm();

        }

        private void UpdateForm()
        {
            /***
             * метод обновляет значения в comboBox__delete_column
            ***/
            comboBox__delete_column.Items.Clear(); // очищаем комбобокс

            if (dataGridView1.Columns.Count != 0)//если в dataGridView есть стобцы
            {
                for (int i = 0; i < dataGridView1.Columns.Count; i++) 
                {
                    comboBox__delete_column.Items.Add(dataGridView1.Columns[i].Name); // имена столбцов добавляем в комбобокс
                }
                comboBox__delete_column.SelectedIndex = 0; // по умолчанию выбираем нулевой элемент
            }
        }

        private List<DataGridCell> DataValidate(int name_column_index)
        {
            /***
             * метод проверяет корректность ячеек.
             * во всех столбцах, короме столбца имен должны быть числа
            ***/ 
            List<DataGridCell> invalid_cells_list = new List<DataGridCell>(); // список ячеек

            for (int i = 0; i < dataGridView1.Rows.Count; i++) // циклами проходим все dataGridView 
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Style.BackColor = dataGridView1.RowsDefaultCellStyle.BackColor; // делаем ячейке цвет по умолчанию
                    if (j != name_column_index) // если столбец не является столбцом имен 
                    {

                        double l;
                        string cell_str = dataGridView1.Rows[i].Cells[j].Value.ToString(); // получаем значение ячейки
                        cell_str = cell_str.Replace(".",","); //заменяем точки на запятые
                        if (double.TryParse(cell_str, out l) == false) // если значение ячейки не число
                        {
                            invalid_cells_list.Add(new DataGridCell(i, j)); //добавляем ячейку в список неверных ячеек
                        }
                    }
                }

            }

            return invalid_cells_list;
        }

        private void ShowInvalidCells(List<DataGridCell> invalid_cells_list)
        {
            /***
             * метод отмечает красным цветом неверные ячейки
             * на вход подается список неверных яччеек
            ***/ 
            for (int i = 0; i < invalid_cells_list.Count; i++) // перебираем неверные ячейки
            {

                dataGridView1.Rows[invalid_cells_list[i].RowNumber].Cells[invalid_cells_list[i].ColumnNumber].Style.BackColor = Color.Red; // отмечем красным цветом
            }
        }

        private void MakeAnalysis()
        {
           
            setProductsList(new List<Product>());
            setColumnsList(new Dictionary<string, int>());

            if (dataGridView1.ColumnCount != 0)//если DGV не пуста
            {
                dataGridView1.AllowUserToAddRows = false;
                CleanNullRowsColumns(); // удаляем пустые строки и столбцы

                ColumnsList = ColumnsForAnalysis(); // получаем столбцы для анализа

                List<DataGridCell> invalid_cells_list = DataValidate(ColumnsList["name"]);
                //ShowInvalidCells(invalid_cells_list);
                if (invalid_cells_list.Count == 0)
                {
                    ProductsList = DataToDictionary(ColumnsList);  // переводим данные из DGV в словарь
                    dataGridView1.Columns.Clear(); // чистим datagridview
                    ProductsList = Sort_GrowingPercent_GroupABC(ProductsList, 80, 95); // сортируем, добавляем нарастающий итог и группу ABC
                    richTextBox1.Text += "Проведен ABC-анализ" + "\n";
                    ProductsList = Deviation_Variation_GroupXYZ(ProductsList, 15, 30); // добавляем среднее отклонение, коэфф вариации и группу XYZ
                    richTextBox1.Text += "Проведен XYZ-анализ" + "\n";
                    //EstimatesToDataGridView(ProductsList, ColumnsList); // показываем в DGVS
                    richTextBox1.Text += "Анализы совмещены!" + "\n";
                 
                }
                else
                {
                    ShowInvalidCells(invalid_cells_list);
                    MessageBox.Show("Найдены неверные значения в некоторых ячейках. Они отмечены красным");
                    richTextBox1.Text += "Ошибка! Неверные значения ячеек." + "\n";
                }
            }
            else
            {
                richTextBox1.Text += "Ошибка!" + "\n";
                MessageBox.Show("Данные не загружены!");
            }
            UpdateForm();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setProductsList(new List<Product>()); // обнуляем список товаров
            setColumnsList(new Dictionary<string, int>()); // обнулем колонки

            if (dataGridView1.ColumnCount != 0)//если DGV не пуста
            {
                dataGridView1.AllowUserToAddRows = false;
                CleanNullRowsColumns(); // удаляем пустые строки и столбцы

                ColumnsList = ColumnsForAnalysis(); // получаем столбцы для анализа

                List<DataGridCell> invalid_cells_list = DataValidate(ColumnsList["name"]); // получаем список неверных ячеек
                //ShowInvalidCells(invalid_cells_list);
                if (invalid_cells_list.Count == 0) // если список неверных ячеек пуст
                {
                    ProductsList = DataToDictionary(ColumnsList);  // переводим данные из DGV в словарь
                    //dataGridView1.Columns.Clear(); // чистим datagridview
                    ProductsList = Sort_GrowingPercent_GroupABC(ProductsList, 80, 95); // сортируем, добавляем нарастающий итог и группу ABC
                    richTextBox1.Text += "Успешно проведен ABC-анализ" + "\n";
                    ProductsList = Deviation_Variation_GroupXYZ(ProductsList, 15, 30); // добавляем среднее отклонение, коэфф вариации и группу XYZ
                    //richTextBox1.Text += "Проведен XYZ-анализ" + "\n";
                    //EstimatesToDataGridView(ProductsList, ColumnsList); // показываем в DGVS
                   // richTextBox1.Text += "Анализы совмещены!" + "\n";
                    
                    new ABCtable(this).ShowDialog(); // показываем таблицу
                }
                else
                {
                    ShowInvalidCells(invalid_cells_list); // отмечаем ячейки с неверными значениеями
                    richTextBox1.Text += "Ошибка! Неверные значения ячеек." + "\n";
                    MessageBox.Show("Найдены неверные значения в некоторых ячейках. Они отмечены красным");
                }
            }
            else
            {
                richTextBox1.Text += "Ошибка!" + "\n";
                MessageBox.Show("Данные не загружены!");
            }
            UpdateForm();  
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            setProductsList(new List<Product>()); // обнуляем список продуктов
            setColumnsList(new Dictionary<string, int>()); // обнуляем колонки

            if (dataGridView1.ColumnCount != 0)//если DGV не пуста
            {
                dataGridView1.AllowUserToAddRows = false;
                CleanNullRowsColumns(); // удаляем пустые строки и столбцы

                ColumnsList = ColumnsForAnalysis(); // получаем столбцы для анализа

                List<DataGridCell> invalid_cells_list = DataValidate(ColumnsList["name"]); // получем список неправильных ячеек
                //ShowInvalidCells(invalid_cells_list);
                if (invalid_cells_list.Count == 0) // если список пустой
                {
                    ProductsList = DataToDictionary(ColumnsList);  // переводим данные из DGV в словарь
                    //dataGridView1.Columns.Clear(); // чистим datagridview
                    ProductsList = Sort_GrowingPercent_GroupABC(ProductsList, 80, 95); // сортируем, добавляем нарастающий итог и группу ABC
                    //richTextBox1.Text += "Проведен ABC-анализ" + "\n";
                    ProductsList = Deviation_Variation_GroupXYZ(ProductsList, 15, 30); // добавляем среднее отклонение, коэфф вариации и группу XYZ
                    richTextBox1.Text += "Успешно проведен XYZ-анализ" + "\n";
                    //EstimatesToDataGridView(ProductsList, ColumnsList); // показываем в DGVS
                    //richTextBox1.Text += "Анализы совмещены!" + "\n";
                    
                    new XYZTable(this).ShowDialog();
                }
                else
                {
                    ShowInvalidCells(invalid_cells_list); // отменчаем ячейки с неверными значениями
                    richTextBox1.Text += "Ошибка! Неверные значения ячеек." + "\n";
                    MessageBox.Show("Найдены неверные значения в некоторых ячейках. Они отмечены красным");
                }
            }
            else
            {
                richTextBox1.Text += "Ошибка!" + "\n";
                MessageBox.Show("Данные не загружены!");
            }
            UpdateForm(); // обновляем
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            setProductsList(new List<Product>()); // обнуляем список продуктов
            setColumnsList(new Dictionary<string, int>()); // обнуляем колонк

            if (dataGridView1.ColumnCount != 0)//если DGV не пуста
            {
                dataGridView1.AllowUserToAddRows = false;
                CleanNullRowsColumns(); // удаляем пустые строки и столбцы

                ColumnsList = ColumnsForAnalysis(); // получаем столбцы для анализа

                List<DataGridCell> invalid_cells_list = DataValidate(ColumnsList["name"]); // получаем список неправильных ячеек
                //ShowInvalidCells(invalid_cells_list);
                if (invalid_cells_list.Count == 0) // если этот список пуст
                {
                    ProductsList = DataToDictionary(ColumnsList);  // переводим данные из DGV в словарь
                    //dataGridView1.Columns.Clear(); // чистим datagridview
                    ProductsList = Sort_GrowingPercent_GroupABC(ProductsList, 80, 95); // сортируем, добавляем нарастающий итог и группу ABC
                    richTextBox1.Text += "Проведен ABC-анализ" + "\n";
                    ProductsList = Deviation_Variation_GroupXYZ(ProductsList, 15, 30); // добавляем среднее отклонение, коэфф вариации и группу XYZ
                    richTextBox1.Text += "Проведен XYZ-анализ" + "\n";
                    //EstimatesToDataGridView(ProductsList, ColumnsList); // показываем в DGVS
                    richTextBox1.Text += "Анализы совмещены. ABC+XYZ-анализ успешно проведен" + "\n";
                    
                    new ABC_XYZtable(this).ShowDialog(); //показываем таблицу
                }
                else
                {
                    ShowInvalidCells(invalid_cells_list); // выделяем неправльные ячейки
                    richTextBox1.Text += "Ошибка! Неверные значения ячеек." + "\n";
                    MessageBox.Show("Найдены неверные значения в некоторых ячейках. Они отмечены красным");
                }
            }
            else
            {
                richTextBox1.Text += "Ошибка!" + "\n";
                MessageBox.Show("Данные не загружены!");
            }
            UpdateForm(); 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ExportToExcel(); 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (New_column_name.Text != "") // если поле с именем не пусто
            {
                AddColumn(New_column_name.Text); // добавляем колонку
                UpdateForm(); // обновляем форму
            }
            else
            {
                MessageBox.Show("Введите имя!");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox__delete_column.Items.Count != 0) // если комбобокс не пустой
            {
                DeleteColumn(comboBox__delete_column.SelectedIndex); //удаляем колонку по индексу из комбобокса
            }
            else
            {
                MessageBox.Show("Нет столбцов!");
            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            /***
             * подтверждения выхода из основнй программы
            ***/

            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = DialogResult.Yes != MessageBox.Show("Вы действительно хотите выйти ?", "Внимание",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.ColumnCount > 0) // если dataGridView не пуста
            {
                ExportToExcel(); // сохраняем в эксель
            }
            else {
                MessageBox.Show("Нечего сохранть!");
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();//форма "О программе"
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            /***
             * метод отлавливает изменение значения клетки dataGridView
            ***/
 
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = dataGridView1.DefaultCellStyle.BackColor; // присваеваем клетке цвет по умолчанию
            string cell_value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(); // плучаем значение этой клетки
            cell_value = cell_value.Replace(".",","); // заменяем точки на запятые
            double u;
            if (double.TryParse(cell_value, out u)) // если значение это число
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = dataGridView1.DefaultCellStyle.BackColor; // делаем цвет по умолчанию
            }
            else
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red; // если не число, то выделяем красным
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /***
             * метод очищает dataGridView
            ***/
            if (dataGridView1.ColumnCount > 0) // если dataGridView не пуста
            {
                if (MessageBox.Show("Очисть рабочую область?", "Подтвердите закрытие", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // если "Да"
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = null;
                    richTextBox1.Text += "Рабочая область очищена" + "\n";
                }
                else
                {
                    // user clicked no
                }
            }
            else
            {
                MessageBox.Show("Рабочая область итак пуста.");
            }
        }
    }
}
