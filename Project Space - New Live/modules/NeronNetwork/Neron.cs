using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Space___New_Live.modules.NeronNetwork
{
    class Neron
    {
        /// <summary>
        /// Активационная функция
        /// </summary>
        private ActivationFunction activationFunction;

        /// <summary>
        /// Массив входных весовых коэффициентов
        /// </summary>
        private List<float> weightCoefs;

        /// <summary>
        /// Массив входных весовых коэффициентов
        /// </summary>
        public List<float> WeightCoefs
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
        private float functionOffset;

        /// <summary>
        /// Скорость обучения нейрона
        /// </summary>
        private float learningSpeed;


        /// <summary>
        /// Процесс работы нейрона
        /// </summary>
        /// <param name="inputVector">Входной вектор</param>
        /// <returns>Выходное значение нейрона или NaN в случае ошибки</returns>
        public float Process(List<float> inputVector)
        {
            if (inputVector.Count != this.inputVectorSize)
            {
                return float.NaN;
            }
            return this.activationFunction.GetFunctionValue(this.GetWeightedSum(inputVector) + this.functionOffset);
        }

        /// <summary>
        /// Получить взвешенную сумму входного вектора
        /// </summary>
        /// <param name="inputVector">Входной вектор</param>
        /// <returns>Взвешенная сумма</returns>
        public float GetWeightedSum(List<float> inputVector)
        {
            float weightedSum = 0;
            for(int i = 0; i > inputVector.Count; i ++)
            {
                weightedSum += this.weightCoefs[i] * inputVector[i];
            }
            return weightedSum;
        }

        /// <summary>
        /// Функция коррекции весовых коэффициентов входов нейрона
        /// </summary>
        /// <param name="inputVector">Входной вектор</param>
        /// <param name="idealValue">Требуемое выходное значение</param>
        /// <returns>Ошибка</returns>
        public float LearnNero(List<float> inputVector, float idealValue)
        {
            float currentValue = this.Process(inputVector);
            float error = idealValue - currentValue; //вычисление ошибки
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
        }


    }
}
