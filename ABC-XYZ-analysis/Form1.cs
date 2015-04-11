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

namespace ABC_XYZ_analysis
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.xls;*.xlsx";
            ofd.Filter = "Excel Sheet(*.xlsx)|*.xlsx";
            ofd.Title = "Выберите документ для загрузки данных";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
                System.IO.FileStream stream =
                System.IO.File.Open(ofd.FileName,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read); //обработать исключение если файл открыт

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

                //Если данное значение установлено в true
                //то первая строка используется в качестве 
                //заголовков для колонок
                if (checkBox1.Checked)
                {
                    IEDR.IsFirstRowAsColumnNames = true;
                }
                else {
                    IEDR.IsFirstRowAsColumnNames = false;
                }
                DataSet ds = IEDR.AsDataSet();

                //Устанавливаем в качестве источника данных dataset 
                //с указанием номера таблицы. Номер таблицы указавает 
                //на соответствующий лист в файле нумерация листов 
                //начинается с нуля.
                int tables_count = ds.Tables.Count;
                listBox1.Items.Add( "Число страниц: "+tables_count.ToString());
                for (int i = 0; i < tables_count; i++)
                {
                    listBox1.Items.Add("Имя "+i+"й "+"таблицы: "+ ds.Tables[i].TableName);
                }

                    dataGridView1.DataSource = ds.Tables[0];
                IEDR.Close();
            }
            else
            {
                MessageBox.Show("Вы не выбрали файл для открытия",
                 "Загрузка данных...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            //Задаем расширение имени файла по умолчанию.
            ofd.DefaultExt = "*.xls;*.xlsx";
            //Задаем строку фильтра имен файлов, которая определяет
            //варианты, доступные в поле "Файлы типа" диалогового
            //окна.
            ofd.Filter = "Excel Sheet(*.xlsx)|*.xlsx";
            //Задаем заголовок диалогового окна.
            ofd.Title = "Выберите документ для загрузки данных";
            ExcelObj.Application app = new ExcelObj.Application();
            ExcelObj.Workbook workbook;
            ExcelObj.Worksheet NwSheet;
            ExcelObj.Range ShtRange;
            DataTable dt = new DataTable();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;

                workbook = app.Workbooks.Open(ofd.FileName, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value);

                //Устанавливаем номер листа из котрого будут извлекаться данные
                //Листы нумеруются от 1
                NwSheet = (ExcelObj.Worksheet)workbook.Sheets.get_Item(1);
                ShtRange = NwSheet.UsedRange;
                for (int Cnum = 1; Cnum <= ShtRange.Columns.Count; Cnum++)
                {
                    dt.Columns.Add(
                       new DataColumn((ShtRange.Cells[1, Cnum] as ExcelObj.Range).Value2.ToString()));
                }
                dt.AcceptChanges();

                string[] columnNames = new String[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    columnNames[0] = dt.Columns[i].ColumnName;
                }

                for (int Rnum = 2; Rnum <= ShtRange.Rows.Count; Rnum++)
                {
                    DataRow dr = dt.NewRow();
                    for (int Cnum = 1; Cnum <= ShtRange.Columns.Count; Cnum++)
                    {
                        if ((ShtRange.Cells[Rnum, Cnum] as ExcelObj.Range).Value2 != null)
                        {
                            dr[Cnum - 1] =
                (ShtRange.Cells[Rnum, Cnum] as ExcelObj.Range).Value2.ToString();
                        }
                    }
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }

                dataGridView1.DataSource = dt;
                app.Quit();
            }
            else
                Application.Exit();
        }
    }
}
