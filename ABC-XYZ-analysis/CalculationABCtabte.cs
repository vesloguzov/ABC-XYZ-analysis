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
    public partial class CalculationABCtabte : Form
    {
        private Form1 MainForm;

        public CalculationABCtabte()
        {
            InitializeComponent();
        }

        public CalculationABCtabte(Form1 MainForm)
        {
            this.MainForm = MainForm;
            InitializeComponent();
        }



        private void EstimatesToDataGridView(List<Product> ProductsList, Dictionary<string, int> columnsList)
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

            dataGridView1.ColumnCount = (2 + columnsList.Count()) + 4; 

            dataGridView1.Columns[0].Name = "Номер"; // имя колонки
            dataGridView1.Columns[1].Name = "Имя продукта"; // имя колонки

            for (int i = 0; i < columnsList.Count; i++)
            {
                dataGridView1.Columns[i + 2].Name = columnsList.Keys.ToList()[i]; // добавляем имена колонок остальных
            }

            // добавляем колонки
            dataGridView1.Columns[dataGridView1.ColumnCount - 4].Name = "Сумма";
            dataGridView1.Columns[dataGridView1.ColumnCount - 3].Name = "Процент";
            dataGridView1.Columns[dataGridView1.ColumnCount - 2].Name = "Нарастающим итогом";
            dataGridView1.Columns[dataGridView1.ColumnCount - 1].Name = "Группа ABC";

            for (int i = 0; i < ProductsList.Count; i++)
            {
                List<string> row = new List<string> { ProductsList[i].number.ToString(), ProductsList[i].name.ToString() }; // создаем строку для добавления в datagridview, добавляем в нее имя и номер продукта

                for (int j = 0; j < ProductsList[i].values_analysis.Count; j++)
                {
                    row.Add(ProductsList[i].values_analysis[j].ToString()); // добавляем в строку значения данных для расчета(колонки объемов продаж)
                }

                row.Add(ProductsList[i].sum_values.ToString());  // добавляем в строку сумму объемов продаж за периоды
                row.Add(ProductsList[i].percent.ToString()); // добавляем в строку процент
                row.Add(ProductsList[i].growing_percent.ToString()); // добавляем в строку нарастающий итог
                row.Add(ProductsList[i].groupABC); // добавляем группу ABC

                dataGridView1.Rows.Add(row.ToArray<string>()); // добавляем строку в datagridview



            }

            dataGridView1.Columns[0].Width = 45; // задаем ширину столбца "номер товара"

            foreach (DataGridViewColumn column in dataGridView1.Columns) // запрещение сортироки (клик на имя столбца)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


        }

        private void CalculationABCtabte_Load(object sender, EventArgs e)
        {
            List<Product> local_products= MainForm.getProductsList();
            Dictionary<string, int> local_columns = MainForm.getColumnsList();
            EstimatesToDataGridView(local_products, local_columns);
        }
    }
}
