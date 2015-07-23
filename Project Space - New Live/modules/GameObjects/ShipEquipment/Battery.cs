using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Project_Space___New_Live.modules.GameObjects.ShipEquipment
{
    /// <summary>
    /// Энергобатарея
    /// </summary>
    class Battery : ShipEquipment
    {
        /// <summary>
        /// Базовая емкость
        /// </summary>
        private int baseMaxEnergy;

        /// <summary>
        /// Максимальная емкость
        /// </summary>
        private int maxEnergy;
        /// <summary>
        /// Текущая максимальная емкость
        /// </summary>
        public int MaxEnergy
        {//Емкость батареи тем ниже, чем выше процент износа, минимальная емкость равна 20% от номинальной
            get { return (int)(this.maxEnergy * (100 - (0.8*this.WearState))); }
        }

        /// <summary>
        /// Запас энергии
        /// </summary>
        private int energy;

        public int Energy
        {
            get { return this.energy; }
        }

        /// <summary>
        /// Разрядка батареи
        /// </summary>
        /// <param name="energyNeed">Количество затраченной энергии</param>
        /// <returns>true в случае успеха, иначе false</returns>
        public bool Uncharge(int energyNeed)
        {
            if (this.energy > energyNeed)
            {//уменьшение количества энергии, если энергия еще есть 
                this.energy -= energyNeed;
                return true;//оборудование может выполнять свои функции
            }
            return false;//оборудование не может выполнять свои функции в виду недостатка энергии
        }

        /// <summary>
        /// Частичная зарядка батареи
        /// </summary>
        /// <param name="newEnergy">Увеличение заряда батареи</param>
        public void Charge(int newEnergy)
        {
            if ((this.energy + newEnergy) < this.MaxEnergy)
            {
                this.energy += newEnergy;
            }
            else
            {
                this.energy = this.MaxEnergy;
            }
        }

        /// <summary>
        /// Полнаязарядка батареи
        /// </summary>
        public void fullCharge()
        {
            this.energy = this.MaxEnergy;
        }

        public Battery(int mass, int maxEnergy, Shape image)
        {
            this.SetCommonCharacteristics(mass, 0, image);//установка общих характеристик (энергопотребление = 0)
            this.baseMaxEnergy = this.maxEnergy = maxEnergy;
            this.State = true;
        }

        protected override void CustomModification()
        {//Характеристики улучшаются на 50% на каждое улучшение
            this.maxEnergy = baseMaxEnergy + (this.Version*(baseMaxEnergy/2));
        }
    }
}
