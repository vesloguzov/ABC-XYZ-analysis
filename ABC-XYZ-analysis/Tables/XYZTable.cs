using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelApp = Microsoft.Office.Interop.Excel;
using System.IO;

namespace ABC_XYZ_analysis
{
    public partial class XYZTable : Form
    {
        private MainForm MainForm;

        public XYZTable()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // открываем эту форму по центру
        }

        public XYZTable(MainForm MainForm)
        {
            this.StartPosition = FormStartPosition.CenterScreen; // открываем эту форму по центру
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

        private void ExportTableToExcel()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Files|*.xls";

            sfd.FileName = "Таблица XYZ";

            DialogResult drSaveFile = sfd.ShowDialog();
            try
            {
                if (drSaveFile == System.Windows.Forms.DialogResult.OK)
                {
                    ExcelApp.Application ExcelApp = new ExcelApp.Application();
                    ExcelApp.Application.Workbooks.Add(Type.Missing);

                    ExcelApp.Columns.ColumnWidth = 25;


                    ExcelApp.Cells[1, 1] = "Группа X";
                    for (int i = 1; i < listBoxGroupX.Items.Count + 1; i++)
                    {
                        ExcelApp.Cells[i + 1, 1] = listBoxGroupX.Items[i - 1].ToString();
                    }

                    ExcelApp.Cells[1, 2] = "Группа Y";
                    for (int i = 1; i < listBoxGroupY.Items.Count + 1; i++)
                    {
                        ExcelApp.Cells[i + 1, 2] = listBoxGroupY.Items[i - 1];
                    }


                    ExcelApp.Cells[1, 3] = "Группа Z";
                    for (int i = 1; i < listBoxGroupZ.Items.Count + 1; i++)
                    {
                        ExcelApp.Cells[i + 1, 3] = listBoxGroupZ.Items[i - 1];
                    }


                    ExcelApp.ActiveWorkbook.SaveAs(sfd.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8, null, null, null,
                     null, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, null, null, null, null, null);

                    FileInfo fileInfo = new FileInfo(sfd.FileName);
                    ExcelApp.ActiveWorkbook.Saved = true;
                    MessageBox.Show("Файл ''" + fileInfo.Name + "'' успешно сохранен в каталог: " + fileInfo.DirectoryName);
                    ExcelApp.Quit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохрании файла: " + ex.Message);
            }
        }
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportTableToExcel();
        }
    }
}
