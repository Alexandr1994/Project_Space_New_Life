using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Space___New_Live.modules.NeronNetwork
{
    /// <summary>
    /// Искусственный нейрон
    /// </summary>
    class Neron
    {
        /// <summary>
        /// Активационная функция
        /// </summary>
        private ActivationFunction activationFunction;

        /// <summary>
        /// Активационная функция
        /// </summary>
        public ActivationFunction NeronActivationFunction
        {
            get { return this.activationFunction; }
        }


        /// <summary>
        /// Массив входных весовых коэффициентов
        /// </summary>
        private List<double> weightCoefs;

        /// <summary>
        /// Массив входных весовых коэффициентов
        /// </summary>
        public List<double> WeightCoefs
        {
            get { return this.weightCoefs; }
        }

        /// <summary>
        /// Размер входного вектора
        /// </summary>
        private int inputVectorSize;

        /// <summary>
        /// Размер входного вектора
        /// </summary>
        public int InputVectorSize
        {
            get { return this.inputVectorSize; }
        }
        
        /// <summary>
        /// Смещение активационной функции
        /// </summary>
        private double functionOffset;

        /// <summary>
        /// Скорость обучения нейрона
        /// </summary>
        private double learningSpeed;


        /// <summary>
        /// Конструктор нейрона
        /// </summary>
        /// <param name="function">Активационная функция</param>
        /// <param name="inputVectorLenght">Количество входов</param>
        public Neron(ActivationFunction.Types function, int inputVectorLenght, double threshold = 0, double activateCoef = 1)
        {
            this.weightCoefs = new List<double>();
            this.activationFunction = new ActivationFunction(function, threshold, activateCoef);//установка активационной функции
            Random random = new Random();
            this.inputVectorSize = inputVectorLenght;//сохранение размера входного вектора
            for (int i = 0; i < inputVectorLenght; i ++)//инициализация весов входов от 0 до 10
            {
                this.weightCoefs.Add(random.NextDouble() * 100);//установка нового входного веса
            }
        }

        /// <summary>
        /// Процесс работы нейрона
        /// </summary>
        /// <param name="inputVector">Входной вектор</param>
        /// <returns>Выходное значение нейрона или NaN в случае ошибки</returns>
        public double Process(List<double> inputVector)
        {
            if (inputVector.Count != this.inputVectorSize)
            {
                return double.NaN;
            }
            return this.activationFunction.GetFunctionValue(this.GetWeightedSum(inputVector) + this.functionOffset);
        }

        /// <summary>
        /// Получить взвешенную сумму входного вектора
        /// </summary>
        /// <param name="inputVector">Входной вектор</param>
        /// <returns>Взвешенная сумма</returns>
        public double GetWeightedSum(List<double> inputVector)
        {
            double weightedSum = 0;
            for(int i = 0; i < inputVector.Count; i ++)
            {
                weightedSum += this.weightCoefs[i] * inputVector[i];
            }
            return weightedSum;
        }

      /*  /// <summary>
        /// Функция коррекции весовых коэффициентов входов нейрона
        /// </summary>
        /// <param name="inputVector">Входной вектор</param>
        /// <param name="idealValue">Требуемое выходное значение</param>
        /// <returns>Ошибка</returns>
        public double LearnNero(List<double> inputVector, double idealValue)
        {
            double currentValue = this.Process(inputVector);
            double error = idealValue - currentValue; //вычисление ошибки
            if (Math.Abs(error) > 0)
            {
                for (int i = 0; i < inputVector.Count; i++)
                {
                    if (inputVector[i] != 0)
                    {
                        this.weightCoefs[i] += error * this.learningSpeed;
                    }
                }
            }
            return Math.Abs(error);
        }*/

        /// <summary>
        /// Корректировка весового коэффициента с идексом index
        /// </summary>
        /// <param name="correction">коррекция</param>
        /// <param name="index">индекс коэффициента</param>
        public void WeightCorrection(double correction, int index)
        {
            this.weightCoefs[index] += correction;
        }


    }
}
