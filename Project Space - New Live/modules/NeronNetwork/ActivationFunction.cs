using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Space___New_Live.modules.NeronNetwork
{
    /// <summary>
    /// Активационная функция
    /// </summary>
    class ActivationFunction
    {
        /// <summary>
        /// Типы переключательных функций
        /// </summary>
        public enum Types:int
        {
            /// <summary>
            /// Пороговая
            /// </summary>
            Threshold = 0,
            /// <summary>
            /// Линейная
            /// </summary>
            Linear,
            /// <summary>
            /// Линейная с насыщением
            /// </summary>
            LineatWithSaturation,
            /// <summary>
            /// Сигмоидальная
            /// </summary>
            Sigmoidal
        }

        /// <summary>
        /// Порог для (пороговой)
        /// Предел насыщения (для линейной с насыщением)
        /// </summary>
        private float threshold = 0;

        /// <summary>
        /// Порог для (пороговой)
        /// Предел насыщения (для линейной с насыщением)
        /// </summary>
        public float Threshold
        {
            get { return this.threshold; }
        }

        /// <summary>
        /// Коэффициент активации
        /// </summary>
        private float activateCoef = 0;

        /// <summary>
        /// Коэффициент активации
        /// </summary>
        public float ActivateCoef
        {
            get { return this.activateCoef; }
        }

        /// <summary>
        /// Тип активационной функции
        /// </summary>
        private Types functionType;

        /// <summary>
        /// Тип активационной функции
        /// </summary>
        public Types FunctionType
        {
            get { return this.functionType; }
        }

        /// <summary>
        /// Получить значение активационной функции
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns>Значение функции или NaN если активационная функция не выбрана</returns>
        public float GetFunctionValue(float arg)
        {
            switch (this.functionType)
            {
                case Types.Threshold:
                {
                    return this.GetThresholdValue(arg);
                }
                case Types.Linear:
                {
                    return this.GetLinearValue(arg);
                }
                case Types.LineatWithSaturation:
                {
                    return this.GetLinearWithSaturationValue(arg);
                }
                case Types.Sigmoidal:
                {
                    return this.GetSigmoidalValue(arg);
                }
                default:
                {
                    return float.NaN;
                }
            }
        }

        /// <summary>
        /// Получить значение пороговой функции
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns>Значение функции</returns>
        private float GetThresholdValue(float arg)
        {
            if (arg >= this.threshold)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Получить значение линейной функции
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns>Значение функции</returns>
        private float GetLinearValue(float arg)
        {
            return this.activateCoef * arg;
        }

        /// <summary>
        /// Получить значение линейной функции с насыщением
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns>Значение функции</returns>
        private float GetLinearWithSaturationValue(float arg)
        {
            if (Math.Abs(arg) > this.threshold)
            {
                if (arg > 0)
                {
                    return 1;
                }
                return 0;
            }
            return this.activateCoef * arg;
        }

        /// <summary>
        /// Получить значение сигмоидальной униполярной функции
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns>Значение функции</returns>
        private float GetSigmoidalValue(float arg)
        {
            return (float)(1 / (1 + Math.Exp(-this.activateCoef * arg)));
        }

        /// <summary>
        /// Конструктор активационной функции
        /// </summary>
        /// <param name="type"></param>
        /// <param name="threshold"></param>
        /// <param name="activateCoef"></param>
        public ActivationFunction(Types type, float threshold = 0, float activateCoef = 1)
        {
            this.functionType = type;
            this.threshold = threshold;
            this.activateCoef = activateCoef;
        }

    }
}
