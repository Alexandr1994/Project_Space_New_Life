using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronNetwork
{
    /// <summary>
    /// Пеоцептрон (персептрон)
    /// </summary>
    class Perceptron : NeronNetwork
    {
        /// <summary>
        /// Конструктор перцептрона (персептрона)
        /// </summary>
        /// <param name="layerCount">Количество слоев нейросети (без учета входного)</param>
        /// <param name="neuronsCount">Массив количеств нейронов в каждом слое (без учета входного)</param>
        /// <param name="layerFuncType">Типы активационных функций неронов в каждом слое (без учета входного)</param>
        /// <param name="inputVectorLenght">Размер входного вектора</param>
        /// <param name="inputType">Тип активационной функции входного слоя</param>
        public Perceptron(int layerCount, List<int> neuronsCount, List<ActivationFunction.Types> layerFuncType, int inputVectorLenght, ActivationFunction.Types inputType = ActivationFunction.Types.Sigmoidal)
        {
            if (layerCount != neuronsCount.Count || layerCount != layerFuncType.Count)//если сверка на количества слоев нейронов с количеством их характеристик
            {
                throw new Exception("Not fit nerons layers counts in args!");//исключение в случае насовпадения
            }
            
            List<Neuron> newLayer = new List<Neuron>();
            for (int i = 0; i < inputVectorLenght; i ++)
            {
                newLayer.Add(new Neuron(inputType, 1));
            }
            this.neuronLayers.Add(newLayer);//добавление входного слоя в коллекцию нейронов

            for (int i = 0; i < layerCount; i++)//инициализация слоев нейронов
            {
                newLayer = new List<Neuron>();
                for (int j = 0; j < neuronsCount[i]; j++)//инициализация неронов в данном слое
                {
                    if (i == 0)
                    {
                        newLayer.Add(new Neuron(layerFuncType[i], inputVectorLenght));
                    }
                    else
                    {//иначе размерность входного вектора нейрнов данного слоя равна количеству нейронов в предидущем слое
                        newLayer.Add(new Neuron(layerFuncType[i], neuronsCount[i - 1]));
                    }
                }
                this.neuronLayers.Add(newLayer);//добавление нового слоя в коллекцию нейронов
            }
        }


        /// <summary>
        /// Процесс работы персептрона (перцептрона)
        /// </summary>
        /// <param name="inputVector">Входной вектор</param>
        /// <returns>Выходной вектор или null в случае ошибки (несоответствия размерности векторов количеству входов нейронов)</returns>
        public override List<double> Process(List<double> inputVector)
        {
            return this.GetLayerOut(inputVector, this.neuronLayers.Count - 1);//вернуть выходной вектор
        }

        /// <summary>
        /// Получение выхода требуемого слоя нейросети
        /// </summary>
        /// <param name="inputVector">Входной вектор</param> 
        /// <param name="layerIndex">Индекс требуемого слоя</param>
        /// <returns>Выходной вектор требуемого слоя</returns>
        private List<double> GetLayerOut(List<double> inputVector, int layerIndex)
        {
            List<double> outputVector = new List<double>();
            for (int i = 0; i <= layerIndex; i ++)
            {
                List<double> tempVector = new List<double>();//временный вектор
                if (i == 0)//если отрабатывает первый слой
                {//то временный вектор приравнпивается к входному вектору
                    tempVector.AddRange(inputVector);
                }
                else
                {//иначе временный вектор приравнивается к результирующему на данной итерации
                    tempVector.AddRange(outputVector);
                }
                outputVector.Clear();//отчистка выходного вектора
                for(int j = 0; j < this.neuronLayers[i].Count; j ++) //цикл отработки слоя нейросети
                {
                    //если размерность входного вектроа для текущего нейрона соответствует количеству его входов
                    if (i == 0)//отработка входного слоя (если имеется)
                    {
                        outputVector.Add(this.neuronLayers[i][j].Process(new List<double>(){tempVector[j]}));
                    }
                    else
                    {
                        if (tempVector.Count == this.neuronLayers[i][j].InputVectorSize)
                        {
                            outputVector.Add(this.neuronLayers[i][j].Process(tempVector));
                                //в выходной вектор добавить состояние текущего нейрона
                        }
                        else
                        {
                            return null; //иначе - ошибка, венуть null
                        }
                    }
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

            double error = Double.MaxValue;
            int iterationCount = 0;
            if (inputVectors.Count != idealOutVectors.Count)
            {
                return false;
            }
            while (Math.Abs(error) > precision && maxIterationCount > iterationCount)
            //while (error > precision)
            {
                error = 0;
                for (int i = 0; i < inputVectors.Count; i ++)
                {
                    List<double> ResultVector = this.Process(inputVectors[i]);
                    if (idealOutVectors[i].Count != ResultVector.Count)//если размерности выходных векторов не равны
                    {
                        continue;//то обучающая пара некорректна, переход к следующей
                    }
                    List<double> errors = VectorsOperations.Substract(ResultVector, idealOutVectors[i]);
                    error += VectorsOperations.ElementsSum(VectorsOperations.ElementsInPower(errors, 2)) / 2;
                    this.LayersCorrection(inputVectors[i], ResultVector, idealOutVectors[i]);

                }
                iterationCount ++;
                if (this.learningCoef > 0.001)
                {
                    this.learningCoef -= 5 / maxIterationCount;
                }
            }
            this.learningCoef = 0.95;//восстановление обучающего коэффициента
            return true;
        }

        private void LayersCorrection(List<double> inputVector, List<double> resultVector, List<double> idealOutVactor)
        {
            List<double> previosCorrection = new List<double>();
            for (int i = 0; i < this.neuronLayers.Count; i ++)
            {

                List<double> preOutVector = new List<double>();
                if (i > 0)
                {
                    preOutVector = this.GetLayerOut(inputVector, i - 1);
                }
                else
                {
                    preOutVector = inputVector;
                }

                for (int j = this.neuronLayers[i].Count - 1; j >= 0; j --)
                {
                    if (i == this.neuronLayers.Count - 1)//обучение выходного слоя
                    {
                        double delta = this.GetLayerActivationFunction(i).GetDerivativeFunctionValue(resultVector[j]) * (idealOutVactor[j] - resultVector[j]);
                       // double delta = (idealOutVactor[j] - resultVector[j]);
                        for (int k = 0; k < this.neuronLayers[i][j].WeightCoefs.Count; k++)
                        {
                            double correction = delta * preOutVector[k];
                            previosCorrection.Add(correction);
                            this.CorrectNeuron(this.learningCoef * correction, i, j, k);
                        }
                    }
                    else
                    {
                        List<double> temp = this.GetLayerOut(inputVector, i);
                        double delta = VectorsOperations.ElementsSum(previosCorrection) * temp[j] * (1 - temp[j]);
                        previosCorrection.Clear();
                        if (i == 0)//обучение входного слоя
                        {
                            double correction = delta * preOutVector[j];
                            previosCorrection.Add(correction);
                            this.CorrectNeuron(this.learningCoef * correction, i, j, 0);
                        }
                        else//обучение скрытых слоев
                        {
                            for (int k = 0; k < this.neuronLayers[i][j].WeightCoefs.Count; k++)
                            {
                                double correction = delta * preOutVector[k];
                                previosCorrection.Add(correction);
                                this.CorrectNeuron(this.learningCoef * correction, i, j, k);

                            }
                        }
                        
                    }
                }
             
   

            }
        }


    }
}
