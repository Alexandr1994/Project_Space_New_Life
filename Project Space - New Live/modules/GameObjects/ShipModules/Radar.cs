using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Project_Space___New_Live.modules.GameObjects.ShipModules
{
    class Radar : ShipEquipment
    {

        /// <summary>
        /// Текущий радиус зоны видимости
        /// </summary>
        private float visibleRadius;

        /// <summary>
        /// Текущий радиус зоны видимости
        /// </summary>
        public float VisibleRadius
        {
            get 
            {
                if (this.state == true)//если радар функционирует
                {
                    return this.visibleRadius;//вернуть радиус действия
                }
                return 0;//иначе вернуть 0
            }
        }

        /// <summary>
        /// Начальный радиус зоны видимости
        /// </summary>
        private float baseVisibleRadius;

        /// <summary>
        /// Конструктор радара
        /// </summary>
        /// <param name="mass">Масса</param>
        /// <param name="visibleRadius">Радиус зоны видимоти</param>
        /// <param name="image">Образ</param>
        public Radar(int mass, int visibleRadius, Shape image)
        {
            this.SetCommonCharacteristics(mass, 0, image);//установка общих характеристик (энергопотребление = 0, реактор производит, а не потребляет энергию)
            this.baseVisibleRadius = this.visibleRadius = visibleRadius;//установка текущей и базовой генерируемой энергии\
            this.State = true;
        }

        /// <summary>
        /// Модификация конкретного типа оборудованиея
        /// </summary>
        protected override void CustomModification()
        {//Характеристики улучшаются на 100% на каждое улучшение
            this.visibleRadius = this.baseVisibleRadius + this.baseVisibleRadius * this.Version;
        }

    }
}
