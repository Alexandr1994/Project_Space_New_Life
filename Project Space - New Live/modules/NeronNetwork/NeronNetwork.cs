using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Space___New_Live.modules.NeronNetwork
{
    /// <summary>
    /// Искусственная нейронная сеть
    /// </summary>
    abstract class NeronNetwork
    {

        /// <summary>
        /// Обучающий коэффициент сети
        /// </summary>
        protected double learningCoef = 0.95;

        /// <summary>
        /// Нейронные слои
        /// </summary>
        protected List<List<Neron>> neronLayers = new List<List<Neron>>();

        /// <summary>
        /// Нейронные слои
        /// </summary>
        public List<List<Neron>> NeronLayers
        {
            get { return this.neronLayers; }
        }


        public void CorrectNeron(double correction, int layer, int index, int weightIndex)
        {
            this.neronLayers[layer][index].WeightCorrection(correction, weightIndex);
        }

        /// <summary>
        /// Процесс работы нейросети
        /// </summary>
        /// <param name="inputVector">Входной вектор</param>
        /// <returns>Выходной вектор или null в случае ошибки (несоответствия размерности векторов количеству входов нейронов)</returns>
        public abstract List<double> Process(List<double> inputVector);

        /// <summary>
        /// Обучение ИНС
        /// </summary>
        /// <param name="inputVectors">Коллекция входов ИНС обучающих пар</param>
        /// <param name="precision">Точность</param>
        /// <param name="maxIterationCount">Максимальное количество итераций обучения</param>
        /// <param name="idealOutVectors">Колекция требуемых выходов ИНС обучающих пар, при обучении без учителя не указывается</param>
        /// <returns>true или false в случае ошибки</returns>
        public abstract bool Learning(List<List<double>> inputVectors, double precision, int maxIterationCount, List<List<double>> idealOutVectors = null);

        /// <summary>
        /// Получить активационную функцию неронов указанного слоя
        /// </summary>
        /// <param name="layerIndex">Индекс слоя ИНС</param>
        /// <returns>Активационная функция</returns>
        protected ActivationFunction GetLayerActivationFunction(int layerIndex)
        {
            return this.NeronLayers[layerIndex][0].NeronActivationFunction;
        }

    }
}
