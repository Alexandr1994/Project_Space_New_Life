using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronNetwork
{
    /// <summary>
    /// Однослойная ИНС
    /// </summary>
    class SingleLayerNetwork : NeronNetwork
    {

        /// <summary>
        /// Конструктор однослойной ИНС
        /// </summary>
        /// <param name="neuronsCount">Количесто нейронов</param>
        /// <param name="inputType">Тип активационной функции</param>
        /// <param name="inputVectorLenght">Размер входного вектора</param>
        public SingleLayerNetwork(int neuronsCount,ActivationFunction.Types layerFuncType, int inputVectorLenght)
        {
            List<Neuron> newLayer = new List<Neuron>();
            for (int i = 0; i < neuronsCount; i++)//инициализация неронов в данном слое
            {
                newLayer.Add(new Neuron(layerFuncType, inputVectorLenght));
            }
            this.neuronLayers.Add(newLayer);//добавление нового слоя в коллекцию нейронов
        }

        /// <summary>
        /// Процесс работы нейросети
        /// </summary>
        /// <param name="inputVector">Входной вектор</param>
        /// <returns></returns>
        public override List<double> Process(List<double> inputVector)
        {
            List<double> outputVector = new List<double>();
            for (int i = 0; i < this.neuronLayers[0].Count; i ++)
            {
                if (inputVector.Count == this.neuronLayers[0][i].InputVectorSize)
                {
                    outputVector.Add(this.neuronLayers[0][i].Process(inputVector));//в выходной вектор добавить состояние текущего нейрона
                }
                else
                {
                    return null; //иначе - ошибка, венуть null
                }                
            }
            return outputVector;//вернуть выходной вектор
        }
        
        /// <summary>
        /// Обучение ИНС
        /// </summary>
        /// <param name="inputVectors">Коллекция входов ИНС обучающих пар</param>
        /// <param name="idealOutVectors">Колекция требуемых выходов ИНС обучающих пар</param>
        /// <param name="precision">Точность</param>
        /// <param name="maxIterationCount">Максимальное количество итераций обучения</param>
        /// <returns>true или false в случае ошибки</returns>
        public override bool Learning(List<List<double>> inputVectors, double precision, int maxIterationCount, List<List<double>> idealOutVectors)
        {
            double error = Double.MaxValue;//ошибка обучения
            int iterationCount = 0;
            if (inputVectors.Count != idealOutVectors.Count)//проверка на обучающей выборки
            {
                return false;
            }
            while (error > precision && iterationCount < maxIterationCount)
            {
                error = 0;
                for (int i = 0; i < idealOutVectors.Count; i ++)
                {
                    List<double> ResultVector = this.Process(inputVectors[i]);
                    List<double> errors = VectorsOperations.Substract(idealOutVectors[i], ResultVector);
                    error += VectorsOperations.ElementsSum(VectorsOperations.ElementsInPower(errors, 2)) / 2;
                    for (int j = 0; j < errors.Count; j ++)
                    {
                        if (Math.Abs(errors[j]) > 0)
                        {
                            for (int k = 0; k < inputVectors[i].Count; k ++)//обучение по правилу Хебба
                            {
                                if (inputVectors[i][k] != 0)
                                {
                                    this.neuronLayers[0][j].WeightCorrection(errors[j] * this.learningCoef, k);
                                }
                            }
                        }
                    }
                }
                if (this.learningCoef > 0.001)
                {
                    this.learningCoef -= 5 / maxIterationCount;
                }
                iterationCount ++;
            }
            this.learningCoef = 0.95;//восстановление обучающего коэффициента
            return true;
        }


    }
}
