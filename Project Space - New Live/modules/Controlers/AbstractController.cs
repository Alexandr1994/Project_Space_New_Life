using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.GameObjects;

namespace Project_Space___New_Live.modules.Controlers
{
    /// <summary>
    /// Аьъбстрактная система управления кораблем (контроллер)
    /// </summary>
    public abstract class AbstractController
    {
        /// <summary>
        /// Флаг нахождения корабля в зоне планеты
        /// </summary>
        private bool nearPlanet;

        /// <summary>
        /// Флаг нахождения корабля в зоне планеты
        /// </summary>
        public bool NearPlanet
        {
            get { return this.nearPlanet; }
            set { this.nearPlanet = value; }
        }

        /// <summary>
        /// Флаг нахождения корабля в зоне обитаемой планеты
        /// </summary>
        private bool nearInhabitedPlanet;

        /// <summary>
        /// Флаг нахождения корабля в зоне обитаемой планеты
        /// </summary>
        public bool NearInhabitedPlanet
        {
            get { return this.nearInhabitedPlanet; }
            set { this.nearInhabitedPlanet = value; }
        }

        /// <summary>
        /// Ссылка на управляемый корабль
        /// </summary>
        private Ship myShip;

        /// <summary>
        /// Процесс работы контроллера
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// Переодическое обнуление флагов контроллера
        /// </summary>
        protected void RefreshFlags()
        {
            this.nearInhabitedPlanet = false;
            this.nearPlanet = false;
        }

    }
}
