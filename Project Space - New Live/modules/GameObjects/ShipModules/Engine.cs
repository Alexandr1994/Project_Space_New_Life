using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects.ShipModules
{
    /// <summary>
    /// Двигательная установка
    /// </summary>
    public class Engine : Equipment
    {
        /// <summary>
        /// Направления модификации двигательной установки
        /// </summary>
        public enum UpgrateDirectionID : int
        {
            /// <summary>
            /// базовая версия
            /// </summary>
            Base = 0,
            /// <summary>
            /// Улучшение маршевых скоростных характеристик
            /// </summary>
            ForwardSpeed,
            /// <summary>
            /// Улучшение маневровых характеристик
            /// </summary>
            ShuntingSpeed
        }

        
        // Набор базовых характеристик оборудования
        
        /// <summary>
        /// Базовая тяга маршевого двигателя
        /// </summary>
        private float baseForwardThrust;

        /// <summary>
        /// Базовая тяга маневровых двинателей
        /// </summary>
        private float baseShuntingThrust;

        /// <summary>
        /// Базовая максимальная маршевая скорость
        /// </summary>
        private float baseMaxForwardSpeed;


        /// <summary>
        /// Базовая максимальная маневровая скорость
        /// </summary>
        private float baseMaxShuntingSpeed;

        //Текущие характеристики оборудования

        /// <summary>
        /// Текущая тяга маршевого двигателя
        /// </summary>
        private float forwardThrust;
        
        /// <summary>
        /// Текущая тяга маршевого двигателя
        /// </summary>
        public float ForwardThrust
        {
            get
            {
                if (this.emergensyState)
                {//в аварийном состоянии тяга маршевых двигателей состовляет 20% от номинальной
                    return this.forwardThrust / 5;
                }
                return this.forwardThrust;
            }
        }

        /// <summary>
        /// Максимальная маршевая скорость
        /// </summary>
        private float maxForwardSpeed;
        /// <summary>
        /// Максимальная маршевая скорость
        /// </summary>
        public float MaxForwardSpeed
        {
            get { return this.maxForwardSpeed; }
        }

        /// <summary>
        /// текущая тяга маневровых двигателей
        /// </summary>
        private float shuntingThrust;

        /// <summary>
        /// Максимальная маневровая скорость
        /// </summary>
        private float maxShuntingSpeed;
        /// <summary>
        /// Максимальная маневровая скорость
        /// </summary>
        public float MaxShuntingSpeed
        {
            get { return this.maxShuntingSpeed; }
        }


        /// <summary>
        /// текущая тяга маневровых двигателей
        /// </summary>
        public float ShuntingThrust
        {
            get
            {
                if (this.emergensyState)
                {//в аварийном состоянии максимальная скорость маршевых двигателей состовляет 20% от номинальной
                    return this.shuntingThrust / 5;
                }
                return this.shuntingThrust;
            }
        }
        
        /// <summary> 
        /// <param name="mass">Масса</param>
        /// <param name="energyNeeds">Энергопотребление</param>
        /// <param name="forwardThrust">Маршевая тяга</param>
        /// <param name="shuntingThrust">Маневровая тяга</param>
        /// <param name="maxForwardSpeed">Ограничение по скорости прямого движения</param>
        /// <param name="maxShuntingSpeed">Ограничение по скорости бокого движения</param>
        /// <param name="image">Изображение</param>
        /// </summary> 
        public Engine(int mass, int energyNeeds,float forwardThrust, float shuntingThrust, float maxForwardSpeed, float maxShuntingSpeed, Shape image)
        {
            this.SetCommonCharacteristics(mass, energyNeeds, image);//сохранение общих характеристик
            //установка текущих характеристик двигательной установки и сохранение базовых характеристик
            this.baseForwardThrust = this.forwardThrust = forwardThrust;
            this.baseShuntingThrust = this.shuntingThrust = shuntingThrust;
            this.baseMaxForwardSpeed = this.maxForwardSpeed = maxForwardSpeed;
            this.baseMaxShuntingSpeed = this.maxShuntingSpeed = maxShuntingSpeed;
            this.Deactivate();
        }


        /// <summary>
        /// Изменение конкретных характеристик двигателя
        /// </summary>
        protected override void CustomModification()
        {//Характеристики улучшаются на 20% на каждое улучшение
            //определение количества модификаций по возможным направлениям
            int forwardUpdates = this.upgrateDirectionsHistory.Count(i => i == (int)UpgrateDirectionID.ForwardSpeed);
            int shuntingUpdates = this.upgrateDirectionsHistory.Count(i => i == (int) UpgrateDirectionID.ShuntingSpeed);
            //Изменение текущих параметров по направлению улучшения маршевых характеристик
            this.forwardThrust = this.baseForwardThrust + (forwardUpdates * this.baseForwardThrust / 5);
            this.maxForwardSpeed = this.baseMaxForwardSpeed + (this.Version * this.baseMaxForwardSpeed / 5);
            //Изменение текущик параметров по направлению улучшения маневровых характеристик
            this.shuntingThrust = this.baseShuntingThrust + (shuntingUpdates * this.baseShuntingThrust / 5);
            this.maxShuntingSpeed = this.baseMaxShuntingSpeed + (this.Version * this.baseMaxShuntingSpeed / 5);
        }

        
        
    }
}
