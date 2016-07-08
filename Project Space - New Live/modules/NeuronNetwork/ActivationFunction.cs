using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronNetwork
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
        private double threshold = 0;

        /// <summary>
        /// Порог для (пороговой)
        /// Предел насыщения (для линейной с насыщением)
        /// </summary>
        public double Threshold
        {
            get { return this.threshold; }
        }

        /// <summary>
        /// Коэффициент активации
        /// </summary>
        private double activateCoef = 0;

        /// <summary>
        /// Коэффициент активации
        /// </summary>
        public double ActivateCoef
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
        public double GetFunctionValue(double arg)
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
                    return double.NaN;
                }
            }
        }

        /// <summary>
        /// Получить значение первой производной активационной функции
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns>Значение первой производной активационной функции</returns>
        public double GetDerivativeFunctionValue(double arg)
        {
            switch (this.functionType)
            {
                case Types.Threshold:
                {
                    return this.GetDerivativeThresholdValue();
                }
                case Types.Linear:
                {
                    return this.GetDerivativeLinearValue();
                }
                case Types.LineatWithSaturation:
                {
                    return this.GetDerivativeLinearWithSaturationValue(arg);
                }
                case Types.Sigmoidal:
                {
                    return this.GerDerivativeSigmoidalValue(arg);
                }
                default:
                {
                    return double.NaN;
                }
            }
        }


        /// <summary>
        /// Получить значение пороговой функции
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns>Значение функции</returns>
        private double GetThresholdValue(double arg)
        {
            if (arg >= this.threshold)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Получить значение первой производной пороговой функции
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns>Значение производной функции</returns>
        private double GetDerivativeThresholdValue()
        {
            return 0;

        }

        /// <summary>
        /// Получить значение линейной функции
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns>Значение функции</returns>
        private double GetLinearValue(double arg)
        {
            return this.activateCoef * arg;
        }

        /// <summary>
        /// Получить значение первой производной линейной функции
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns>Значение производной функции</returns>
        private double GetDerivativeLinearValue()
        {
            return this.activateCoef;
        }

        /// <summary>
        /// Получить значение линейной функции с насыщением
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns>Значение функции</returns>
        private double GetLinearWithSaturationValue(double arg)
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
        /// Получить значение первой производной линейной функции с насыщением
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns>Значение функции</returns>
        private double GetDerivativeLinearWithSaturationValue(double arg)
        {
            if (Math.Abs(arg) > this.threshold)
            {
                return 0;
            }
            return this.activateCoef;
        }

        /// <summary>
        /// Получить значение сигмоидальной функции
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns>Значение функции</returns>
        private double GetSigmoidalValue(double arg)
        {
            return 1 / (1 + Math.Exp(-this.activateCoef * arg));
        }

        /// <summary>
        /// Получить значение первой производной сигмоидальной функции
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <returns>Значение функции</returns>
        private double GerDerivativeSigmoidalValue(double arg)
        {
            return this.GetSigmoidalValue(arg) * (1 - this.GetSigmoidalValue(arg));
        }


        /// <summary>
        /// Конструктор активационной функции
        /// </summary>
        /// <param name="type"></param>
        /// <param name="threshold"></param>
        /// <param name="activateCoef"></param>
        public ActivationFunction(Types type, double threshold = 0, double activateCoef = 1)
        {
            this.functionType = type;
            this.threshold = threshold;
            this.activateCoef = activateCoef;
        }

    }
}
