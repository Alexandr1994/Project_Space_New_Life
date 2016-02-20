using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Project_Space___New_Live.modules.GameObjects.ShipModules
{
    /// <summary>
    /// Реактор
    /// </summary>
    public class Reactor : Equipment
    {

        /// <summary>
        /// Базовая генерируемая энергия
        /// </summary>
        private int baseGeneration;

        /// <summary>
        /// Генерируемая энергия
        /// </summary>
        private int energyGeneration;
        /// <summary>
        /// Генерируемая энергия
        /// </summary>
        public int EnergyGeneration
        {
            get
            {
                if (!this.emergensyState)
                {
                    return this.energyGeneration;
                }
                return 0;//в аварийном состоянии реактор не генерирует энергию
            }
        }

        public Reactor(int mass, int energyGeneration, Shape image)
        {
            this.SetCommonCharacteristics(mass, 0, image);//установка общих характеристик (энергопотребление = 0, реактор производит, а не потребляет энергию)
            this.baseGeneration = this.energyGeneration = energyGeneration;//установка текущей и базовой генерируемой энергии\
            this.Activate();
        }


        /// <summary>
        /// Модификация конкретного типа оборудованиея
        /// </summary>
        protected override void CustomModification()
        {//Характеристики улучшаются на 100% на каждое улучшение
            this.energyGeneration = baseGeneration + baseGeneration*this.Version;
        }
    }
}
