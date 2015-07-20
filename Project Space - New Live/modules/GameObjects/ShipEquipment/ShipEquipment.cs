using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;

namespace Project_Space___New_Live.modules.GameObjects.ShipEquipment
{
    public abstract class ShipEquipment
    {

        /// <summary>
        /// Изображение оборудования
        /// </summary>
        private ObjectView view;
        /// <summary>
        /// Изображение оборудования
        /// </summary>
        public ObjectView View
        {
            get { return this.view; }
        }

        /// <summary>
        /// Масса
        /// </summary>
        private int mass;
        /// <summary>
        /// Масса
        /// </summary>
        public int Mass
        {
            get { return this.mass; }
        }

        /// <summary>
        /// Версия (уровень модификации)
        /// </summary>
        private int version;
        /// <summary>
        /// Версия (уровень модификации)
        /// </summary>
        public int Version
        {
            get { return this.version; }
        }

        /// <summary>
        /// История улучшений
        /// </summary>
        private List<int> upgrateDirectionsHistory; 


        /// <summary>
        /// Текущий уровень износа оборудования (в процентах) 
        /// </summary>
        protected int wearState = 0;
        /// <summary>
        /// Текущий уровень износа оборудования (в процентах) 
        /// </summary>
        public int WearState
        {
            get { return this.wearState; }
        }

        /// <summary>
        /// Энергопотребление
        /// </summary>
        private int energyNeeds;
        /// <summary>
        /// Энергопотребление
        /// </summary>
        public int EnergyNeeds
        {
            get { return this.energyNeeds; }
        }

        /// <summary>
        /// Восстановление оборудования (установка износа в 0%)
        /// </summary>
        public void Repair()
        {
            this.wearState = 0;
        }

        /// <summary>
        /// Модификация оборудования
        /// </summary>
        /// <param name="directionID"></param>
        public void Upgrate(int directionID)
        {
            this.version ++;//увеличение версии
            this.upgrateDirectionsHistory.Add(directionID);//сохраниение в истории данных об улучшении
            this.CustomUpgrate();//улучшение оборудования
        }

        /// <summary>
        /// Модификация конкретного типа оборудованиея
        /// </summary>
        /// <param name="directionID"></param>
        protected abstract void CustomUpgrate();

        /// <summary>
        /// Даунгрейт оборудования
        /// </summary>
        public void Downgrate()
        {
            this.version --;//умениьшение версии
            this.CustomDowngrate();//даунгрейт оборудования
            this.upgrateDirectionsHistory.RemoveAt(upgrateDirectionsHistory.Count - 1);//уничтожение данных о последнем улучшении в истории
        }

        /// <summary>
        /// Даунгрейт конкретного типа оборудования
        /// </summary>
        protected abstract void CustomDowngrate();
    }
}
