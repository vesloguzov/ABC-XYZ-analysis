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
        public double percent; // процент от общей доли
        public double growing_percent; // процент нарастающим итогом
        public double standard_deviation; // стандартное отконение
        public double coefficient_of_variation; // коэффициент вариации
        public string groupABC; // группа ABC
        public string groupXYZ; // группа XYZ


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



        public void CalculateSumAndAverage(Product product)
        {
            /***
                * Считаем дополнительные поля для
                * продукта: сумма объемов продаж за весь период, среднее значение
                * Присваиваем продукту
            ***/

            double sum = 0;
            double average = 0;
            for (int i = 0; i < product.values_analysis.Count; i++)
                {
                    sum = sum + values_analysis[i]; // сумма значений объемов продаж за отрезки
                }
            product.sum_values = sum;
            average = Math.Round((sum / product.values_analysis.Count), 3); // среднее значение
            product.average_value = average;
        }

        public static double CalculateTotalSum(List<Product> list)
        {
            /***
             * Считаем общую сумму (сумма за квартал, например)
             ***/
            double sum = 0;
            for (int i = 0; i < list.Count; i++)
            {
                sum = sum + list[i].sum_values;
            }
                return sum;
        }

        public static double Share(double value, double total)
        {
            /***
             * считаем процент от числа
             * два параметра - итого и общая сумма
             * взвращает процент
             ***/
            double percent = (value / total) * 100;
            percent = Math.Round(percent, 3);
            return percent;
        }

        public static List<Product> SortList(List<Product> UnsortedList, string param)
        { 
            /***
             * метод сортирует список по процентам, по убыванию
             * возвращает отсортированный список
             ***/
          //  string prop = typeof(Product).GetProperty(propertyName).ToString();
            
           // int value = (int)prop.GetValue(Product, null);

            List<Product> SortedList = new List<Product>();
            if (param == "percent")
            {
                SortedList = UnsortedList.OrderByDescending(obj => obj.percent).ToList();
            }
            if (param == "number") 
            {
                SortedList = UnsortedList.OrderBy(obj => obj.number).ToList();
            }
            return SortedList;
        }

        public static List<Product> GrowingPercent(List<Product> products)
        {
            products[0].growing_percent = products[0].percent;
             for (int i = 1; i < products.Count; i++) // добавляем к продуктам значение поля "нарастающим итогом"
                {
                    products[i].growing_percent = products[i - 1].growing_percent + products[i].percent;
                }
             return products;
        }

        public static List<Product> StandartDeviation(List<Product> products)
        {
            double param1 = 0;
            for (int i = 0; i < products.Count; i++) 
            {
                param1 = 0;
                for (int j = 0; j < products[i].values_analysis.Count; j++)
                {
                    param1 = param1 + Math.Pow((products[i].values_analysis[j] - products[i].average_value), 2);
                }
                param1 = param1 / 3;
                param1 = Math.Pow(param1, 0.5);
                products[i].standard_deviation = param1;
            }
            return products;
        }
        public static List<Product> CoefficientOfVariation(List<Product> products)
        {
            for (int i = 0; i < products.Count; i++) 
            {
                products[i].coefficient_of_variation = (products[i].standard_deviation / products[i].average_value) * 100;
            }
            return products;
        }
    }

}