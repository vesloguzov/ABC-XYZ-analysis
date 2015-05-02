using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC_XYZ_analysis
{
    class Product
    {
        /***
         * Класс продукт 
         ***/
        public int number; //  номер
        public string name; // имя
        public List<double> values_analysis; // список значений объемов продаж за отрезки
        public double sum_values; // сумма бъемов продаж за весь период
        public double average_value; // среднее значение

        /***
         * Конструктор класса
         * имеет имя, номер, массив со значениями объемов продаж
         ***/
        public Product(int number, string name, List<double> values_analysis)
        {
            this.number = number;
            this.name = name;
            this.values_analysis = values_analysis;
        }

        /***
         * Считаем дополнительные поля для
         * продукта: сумма бъемов продаж за весь период, среднее значение
         * Присваиваем продукту
         ***/

        public void CalculateSumAndAverage(Product product)
        {
            double sum = 0;
            double average = 0;
            for (int i = 0; i < product.values_analysis.Count; i++)
                {
                    sum = sum + values_analysis[i]; // сумма значений объемов продаж за отрезки
                }
            product.sum_values = sum;
            average = Math.Round((sum / product.values_analysis.Count), 2); // среднее значение
            product.average_value = average;
        }
    }

}