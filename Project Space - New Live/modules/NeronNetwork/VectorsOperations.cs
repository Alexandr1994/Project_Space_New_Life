using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Space___New_Live.modules.NeronNetwork
{
    /// <summary>
    /// Операции над одномерными векторами
    /// </summary>
    static class VectorsOperations
    {
        /// <summary>
        /// Вычитание
        /// </summary>
        /// <param name="arg1">Вектор 1</param>
        /// <param name="arg2">Вектор 2</param>
        /// <returns>Вектор состоящий разностей соответствующих элементов Вектора 1 и Вектора 2 или null, в случае ошибки</returns>
        public static List<double> Substract(List<double> arg1, List<double> arg2)
        {
            if (arg1.Count != arg2.Count)//если размерности векторов не равны
            {
                return null;//ошибка, вернуть null 
            }
            List<double> result = new List<double>();//вектор-результат
            for (int i = 0; i < arg1.Count; i ++)
            {
                result.Add(arg1[i] - arg2[i]);//поэлементное вычисление разности векторов 
            }
            return result;
        }

        /// <summary>
        /// Получение суммы элементов вектора
        /// </summary>
        /// <param name="arg1">Вектор</param>
        /// <returns>Сумма элементов вектора</returns>
        public static double ElementsSum(List<double> arg1)
        {
            double result = 0;
            foreach (double element in arg1)
            {
                result += element;
            }
            return result;
        }

        /// <summary>
        /// Возведение элементов вектора в степень
        /// </summary>
        /// <param name="arg1">Вектор</param>
        /// <param name="power">Требуемая степень</param>
        /// <returns>Вектор элементов возведенных в требуемую степень</returns>
        public static List<double> ElementsInPower(List<double> arg1, double power)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < arg1.Count; i++)
            {
                result.Add(Math.Pow(arg1[i], power));//поэлементное возведение в требуемую степень элементов вектора
            }
            return result;
        }
    }
}
