using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
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
        protected MoveManager moveManager = new MoveManager();

        /// <summary>
        /// Модуль управления движением транспортного средства
        /// </summary>
        public MoveManager MoveManager
        {
            get { return this.moveManager; }
        }

        /// <summary>
        /// Постоянное перемещение корабля
        /// </summary>
        protected override void Move()
        {
            this.MoveManager.Process(this);
        }

        /// <summary>
        /// Процесс жизни корабля+
        /// </summary>
        /// <param name="homeCoords">Координаты начала отсчета</param>
        public override void Process(Vector2f homeCoords)
        {
            if (this.Health < 1)//Если оставшийся запас прочности упал до 0
            {
                this.destroyed = true;//то установить флаг уничтожения корабля
                return;
            }
            Shell shell;
            this.brains.Process(new List<ObjectSignature>());//отработка управляющей системы
            this.EnergyProcess();//отработка энергосистемы
            if ((shell = this.objectWeaponSystem.Process(this)) != null)//если в ходе работы оружейной системы был получен снаряд
            {
                this.environment.AddNewShell(shell);//то отправить его в коллекцияю снарядов звездной системы
                this.MoveManager.ShellShoot(this, shell.SpeedVector, shell.Mass);//отдача от выстрела
            }
            this.Move();//отработка системы движений
        }

    }
}
