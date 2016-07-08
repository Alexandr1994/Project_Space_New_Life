using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronNetwork
{
    /// <summary>
    /// Загрузчик обучающей выборки из файла
    /// </summary>
    class LearningHelper
    {
        /// <summary>
        /// Загрузить обучающую выборук
        /// </summary>
        /// <param name="filename">Имя файла с обучающей выборкой</param>
        /// <returns>Коллекцая, первый элемент которой коллекция входных векторов, второй элемент - коллекция желательных выходных векторов</returns>
        public List<List<List<double>>> LoadLearningSet(string filename, int inputVectorSize, int outputVectorSize)
        {
            StreamReader reader = new StreamReader(filename);//открыть поток
            List<List<double>> inputVectors = new List<List<double>>();
            List<List<double>> outputVectors = new List<List<double>>();
            while (!reader.EndOfStream)//пока поток не окончен
            {
                String line = reader.ReadLine();//считать строку
                string[] subLines = line.Split('|');//разбить еще на подстроки по символу '|'
                List<string[]> elements = new List<string[]>();
                foreach (string subline in subLines)
                {
                    elements.Add(subline.Split('#'));//построить коллекцию значений в строке, разбив подстроки по символу ','
                }
                List<double> newVector = new List<double>();//сформировать входной вектор
                for (int i = 0; i < inputVectorSize; i++)
                {
                    newVector.Add(Convert.ToDouble(elements[0][i]));
                }
                inputVectors.Add(newVector);
                newVector = new List<double>();//сформировать желательны выходной вектор
                for (int i = 0; i < outputVectorSize; i++)
                {
                    newVector.Add(Convert.ToDouble(elements[1][i]));
                }
                outputVectors.Add(newVector);
            }
            reader.Close();//закрыть поток
            return new List<List<List<double>>>(){inputVectors, outputVectors};//вернуть полученную обучающую выборку 
        }

    }
}
