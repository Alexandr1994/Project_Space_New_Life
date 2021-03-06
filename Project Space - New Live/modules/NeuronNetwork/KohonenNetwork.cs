﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronNetwork
{
    /// <summary>
    /// Простая ИНС Кохонена (Слой Кохонена)
    /// </summary>
    class KohonenNetwork : NeronNetwork
    {

        /// <summary>
        /// Конструктор простой ИНС Кохонена
        /// </summary>
        /// <param name="neuronsCount">Количесто нейронов</param>
        /// <param name="inputVectorLenght">Размер входного вектора</param>
        /// <param name="inputType">Тип активационной функции входного слоя</param>
        public KohonenNetwork(int neuronsCount, int inputVectorLenght, ActivationFunction.Types inputType = ActivationFunction.Types.Sigmoidal)
        {
            List<Neuron>  neuronLayer = new List<Neuron>();
            neuronLayer = new List<Neuron>();
            for (int i = 0; i < inputVectorLenght; i++)//построение входного слоя
            {
                neuronLayer.Add(new Neuron(inputType, 1));
            }
            this.neuronLayers.Add(neuronLayer);
            neuronLayer = new List<Neuron>();
            for (int i = 0; i < neuronsCount; i ++)//построение слоя Кохонена
            {
                neuronLayer.Add(new Neuron(ActivationFunction.Types.Linear, inputVectorLenght));
            }
            this.neuronLayers.Add(neuronLayer);
        }

        /// <summary>
        /// Процесс работы ИНС Кохонена
        /// </summary>
        /// <param name="inputVector"></param>
        /// <returns></returns>
        public override List<double> Process(List<double> inputVector)
        {
            List<double> outputVector = new List<double>();
            for (int i = 0; i < this.neuronLayers.Count; i ++)//цикл отработки слоев ИНС
            {

                for (int j = 0; j < NeuronLayers[i].Count; j ++)//цикл отработки неронов в слое ИНС
                {
                    if (i == 0)
                    {
                        outputVector.Add(this.neuronLayers[i][j].Process(new List<double>(){ inputVector[j] }));
                    }
                    else
                    {
                        outputVector.Add(this.neuronLayers[i][j].Process(inputVector));
                    }
                    
                }
                inputVector = outputVector;
                outputVector = new List<double>();
            }
            outputVector = inputVector;
            double maxValue = outputVector.Max();//получаем максимальное значение выходного вектора
            for (int i = 0; i < outputVector.Count; i ++)//отработка соревновательной функции
            {
                if (outputVector[i] == maxValue)//если текущий элемент выходного вектора является максимальным
                {
                    outputVector[i] = 1;//то приравнять его занчение к 1
                }
                else
                {
                    outputVector[i] = 0;//иначе к 0
                }
            }
            return outputVector;
        }

        /// <summary>
        /// Обучение простой ИНС Кохонена
        /// </summary>
        /// <param name="inputVectors">Набор входных векторов</param>
        /// <param name="precision">Точность</param>
        /// <param name="maxIterationCount">Максимальное количество эпох обучения</param>
        /// <param name="idealOutVectors">Требуемые выходные значения (НЕ ИСПОЛЬЗУЕТСЯ)</param>
        /// <returns>true или false, в случае ошибки</returns>
        public override bool Learning(List<List<double>> inputVectors, double precision, int maxIterationCount, List<List<double>> idealOutVectors = null)
        {
            idealOutVectors = null;
            double error = Double.MaxValue;
            int iterationCount = 0;
            while (error > precision && iterationCount < maxIterationCount)
            {
                error = 0;
                foreach (List<double> inputVector in inputVectors)
                {
                    double locError = 0;
                    List<double> outputVector = this.Process(inputVector);
                    int winNeronIndex = outputVector.IndexOf(1);//поиск вектора победителя
                    for (int j = 0; j < this.neuronLayers[1][winNeronIndex].WeightCoefs.Count; j++)//цикл коррекции весовых коэффициентов нейрона победителя
                    {
                        locError += (inputVector[j] - this.neuronLayers[1][winNeronIndex].WeightCoefs[j]);
                        this.neuronLayers[1][winNeronIndex].WeightCorrection(this.learningCoef * (inputVector[j] - this.neuronLayers[1][winNeronIndex].WeightCoefs[j]), j);
                    }
                    locError = Math.Sqrt(locError);
                    error += locError;
                }
                iterationCount ++;
            }
            return true;
        }
    }
}
