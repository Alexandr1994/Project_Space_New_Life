using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects.ShipEquipment
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

        /// <summary>
        /// Набор базовых характеристик оборудования
        /// </summary>
        private Dictionary<String, float> baseCharacteristics; 

        /// <summary>
        /// Маршевая максимальная маршевая скорость
        /// </summary>
        private float forwardSpeed;
        
        /// <summary>
        /// Маршевая максимальная скорость скорость
        /// </summary>
        public float ForwardSpeed
        {
            get
            {
                if (this.emergensyState)
                {//в аварийном состоянии максимальная скорость маршевых двигателей состовляет 10% от номинальной
                    return this.forwardSpeed/10;
                }
                return this.forwardSpeed;
            }
        }

        /// <summary>
        /// Ускорение маршевых двигателей
        /// </summary>
        private float forwardAcceleration;

        /// <summary>
        /// Ускорение маршевых двигателей
        /// </summary>
        public float ForwardAcceleration
        {
            get
            {
                if (this.emergensyState)
                {//в аварийном состоянии ускорение маршевых двигателей состовляет 20% от номинального
                    return this.forwardAcceleration/5;
                }
                return this.forwardAcceleration;
            }    
        }

        /// <summary>
        /// Максимальнгая маневровая скорость
        /// </summary>
        private float shuntingSpeed;

        /// <summary>
        /// Максимальнгая боковых маневровая скорость
        /// </summary>
        public float ShuntingSpeed
        {
            get
            {
                if (this.emergensyState)
                {//в аварийном состоянии максимальная скорость маршевых двигателей состовляет 20% от номинальной
                    return this.shuntingSpeed / 5;
                }
                return this.shuntingSpeed;
            }
        }

        /// <summary>
        /// Ускорение маневровых двигателей
        /// </summary>
        private float shuntingAcceleration;

        /// <summary>
        /// Ускорение боковых маневровых двигателей
        /// </summary>
        public float ShuntingAccleleration
        {
            get
            {
                if (this.emergensyState)
                {//в аварийном состоянии максимальная скорость маршевых двигателей состовляет 20% от номинальной
                    return this.shuntingAcceleration / 5;
                }
                return this.shuntingAcceleration;
            }
        }

        /// <summary>
        /// Скорость поворота корабля (в радианах/ ед. вр.)
        /// </summary>
        private float rotationSpeed;

        /// <summary>
        /// Скорость поворота корабля (в радианах/ ед. вр.)
        /// </summary>
        public float RotationSpeed
        {
            get { return this.rotationSpeed; }
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
        public Engine(int mass, int energyNeeds,Vector2f forwardCharacteristics, Vector2f shuningCharacteristics, float rotateSpeed, Shape image)
        {
            this.SetCommonCharacteristics(mass, energyNeeds, image);//сохранение общих характеристик
            //установка текущих характеристик двигательной установки и сохранение базовых характеристик
            this.baseCharacteristics.Add("ForwardSpeed",this.forwardSpeed = forwardCharacteristics.X);
            this.baseCharacteristics.Add("ForwardAcceleration" ,this.forwardAcceleration = forwardCharacteristics.Y);
            this.baseCharacteristics.Add("ShuntingSpeed" ,this.shuntingSpeed = shuningCharacteristics.X);
            this.baseCharacteristics.Add("ShuntingAcceleration",this.shuntingAcceleration = shuningCharacteristics.Y);
            this.baseCharacteristics.Add("Rotation", this.rotationSpeed = rotateSpeed);
        }


        /// <summary>
        /// Изменение конкретных характеристик двигателя
        /// </summary>
        protected override void CustomModification()
        {//Характеристики улучшаются на 20% на каждое улучшение
            //определение количества модификаций по возможным направлениям
            int forwardUpdates = this.upgrateDirectionsHistory.Count(i => i == (int)UpgrateDirectionID.ForwardSpeed);
            int shuntingUpdates = this.upgrateDirectionsHistory.Count(i => i == (int) UpgrateDirectionID.ShuntingSpeed);
            // soon
            //Изменение текущих параметров по направлению улучшения маршевых характеристик
            this.forwardSpeed = baseCharacteristics["ForwardSpeed"] + (forwardUpdates * baseCharacteristics["ForwardSpeed"]/5);
            this.forwardAcceleration = baseCharacteristics["ForwardAcceleration"] + (forwardUpdates * baseCharacteristics["ForwardAcceleration"] / 5);
            //Изменение текущик параметров по направлению улучшения маневровых и поворотных характеристик
            this.shuntingSpeed = baseCharacteristics["ShuntingSpeed"] + (shuntingUpdates * baseCharacteristics["ShuntingSpeed"] / 5);
            this.shuntingSpeed = baseCharacteristics["ShuntingAcceleration"] + (shuntingUpdates * baseCharacteristics["ShuntingAcceleration"] / 5);
            this.rotationSpeed = baseCharacteristics["Rotation"] + (shuntingUpdates * baseCharacteristics["ShuntingSpeed"] / 5);
        }

        
        
    }
}
