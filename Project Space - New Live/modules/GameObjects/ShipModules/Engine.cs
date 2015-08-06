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
    class Engine : ShipEquipment
    {
        /// <summary>
        /// Направления модификации двигательной установки
        /// </summary>
        public enum UpgrateDirectionID : int
        {
            Base = 0,//базовая версия
            ForwardSpeed,//Улучшение маршевых скоростных характеристик
            ShuntingSpeed,//Улучшение маневровых
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
        /// Базовая максимальная скорость
        /// </summary>
        private float baseMaxSpeed;


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
        /// Максимальная скорость
        /// </summary>
        private float maxSpeed;
        /// <summary>
        /// Максимальная скорость
        /// </summary>
        public float MaxSpeed
        {
            get { return this.maxSpeed; }
        }

        /// <summary>
        /// текущая тяга маневровых двигателей
        /// </summary>
        private float shuntingThrust;

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
        /// Двигательная установка
        /// </summary>
        /// <param name="mass">Масса</param>
        /// <param name="energyNeeds">Энергопотребление</param>
        /// <param name="forvardCharacteristics">Маршевые характеристики: Макс. скорость(X) и ускорение(Y)</param>
        /// <param name="shuningCharacteristics">Маневровые характеристики: Макс. скорость(X) и ускорение(Y)</param>
        /// <param name="rotateSpeed">Скорость разворота</param>
        /// <param name="image">Изображение</param>
        public Engine(int mass, int energyNeeds,float forwardThrust, float shuntingThrust, float maxSpeed, Shape image)
        {
            this.SetCommonCharacteristics(mass, energyNeeds, image);//сохранение общих характеристик
            //установка текущих характеристик двигательной установки и сохранение базовых характеристик
            this.baseForwardThrust = this.forwardThrust = forwardThrust;
            this.baseShuntingThrust = this.shuntingThrust = shuntingThrust;
            this.baseMaxSpeed = this.maxSpeed = maxSpeed;
            this.State = false;
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
            //Изменение текущик параметров по направлению улучшения маневровых характеристик
            this.shuntingThrust = this.baseShuntingThrust + (shuntingUpdates * this.baseShuntingThrust / 5);

            this.maxSpeed = this.baseMaxSpeed + (this.Version*this.baseMaxSpeed/5);

        }

        
        
    }
}
