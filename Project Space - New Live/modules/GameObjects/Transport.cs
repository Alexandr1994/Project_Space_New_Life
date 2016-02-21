using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.GameObjects.ShipModules;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Транспортное средство
    /// </summary>
    public abstract class Transport : ActiveObject
    {

        //ПАРАМЕТРЫ ТРАНСПОРТНОГО СРЕДСТВА

        /// <summary>
        /// Размер части корабля
        /// </summary>
        private Vector2f viewPartSize;

        /// <summary>
        /// Размер части корабля
        /// </summary>
        public Vector2f ViewPartSize
        {
            get { return this.viewPartSize; }
            set { this.viewPartSize = value; }
        }

        // ОБОРУДОВАНИЕ ТРАНСПОРТНОГО СРЕДСТВА

        /// <summary>
        /// Идентификаторы оборудования транспортного средства
        /// </summary>
        public enum EquipmentNames : int
        {
            /// <summary>
            /// Двигатель
            /// </summary>
            Engine = 0,
            /// <summary>
            /// Реактор
            /// </summary>
            Reactor,
            /// <summary>
            /// Энергобатеря
            /// </summary>
            Battery,
            /// <summary>
            /// Радар
            /// </summary>
            Radar,
            /// <summary>
            /// Щит
            /// </summary>
            Shield
        }

        /// <summary>
        /// Двигатель транспортного средства
        /// </summary>
        protected Engine transportEngine;

        /// <summary>
        /// Коллекция оборудования корабля
        /// </summary>
        public override List<Equipment> Equipment
        {
            get
            {
                List<Equipment> shipEquipment = new List<Equipment>();
                shipEquipment.Add(this.transportEngine);
                shipEquipment.Add(this.objectReactor);
                shipEquipment.Add(this.objectBattery);
                shipEquipment.Add(this.objectRadar);
                shipEquipment.Add(this.objectShield);
                return shipEquipment;
            }
        }

        /// <summary>
        /// Модуль управления движением транспортного средства
        /// </summary>
        private MoveManager moveManager = new MoveManager();

        /// <summary>
        /// Модуль управления движением транспортного средства
        /// </summary>
        public MoveManager MoveManager
        {
            get { return this.moveManager; }
        }

    }
}
