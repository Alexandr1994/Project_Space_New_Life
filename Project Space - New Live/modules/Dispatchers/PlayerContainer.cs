using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.GameObjects;
using Project_Space___New_Live.modules.GameObjects.ShipModules;
using SFML.System;

namespace Project_Space___New_Live.modules.Dispatchers
{
    class PlayerContainer
    {

        /// <summary>
        /// Ссылка на графический модуль
        /// </summary>
        private RenderModule GameRenderer;

        /// <summary>
        /// Экземпляр контейнера
        /// </summary>
        private static PlayerContainer container = null;
        
        /// <summary>
        /// Координаты игрока на предыдущей итерации
        /// </summary>
        private Vector2f lastPlayerCoords;

        /// <summary>
        /// Корабль игрока
        /// </summary>
        private Ship playerShip = null;
        /// <summary>
        /// Корабль игрока
        /// </summary>
        public Ship PlayerShip
        {
            get { return this.playerShip; }
        }

        /// <summary>
        /// Активная звездная система
        /// </summary>
        private StarSystem activeSystem = null;
        /// <summary>
        /// Активная звездная система
        /// </summary>
        public StarSystem ActiveSystem
        {
            get { return this.activeSystem; }
            set { this.activeSystem = value; }//(временно)
        }

        /// <summary>
        /// Поучить экземпляр контейнера
        /// </summary>
        /// <param name="GameWorld"></param>
        /// <returns></returns>
        public static PlayerContainer GetInstanse(List<StarSystem> GameWorld)
        {
            if (container == null)
            {
                container = new PlayerContainer(GameWorld);
            }
            return container;
        }

        /// <summary>
        /// Конструктор контейнера
        /// </summary>
        /// <param name="GameWorld"></param>
        private PlayerContainer(List<StarSystem> GameWorld)
        {//Временная реализация конструктора корабля
            //Сохранение в контейнере
            this.playerShip = new Ship(800, new Vector2f(400, 400), ResurceStorage.shipTextures, new Vector2f(15, 30), 0);//Корабля игрока
            this.lastPlayerCoords = this.playerShip.Coords;//последних координат корабля игрока
            this.activeSystem = GameWorld[this.playerShip.StarSystemIndex];//Текузей звездной системы
            this.GameRenderer = RenderModule.getInstance();//ССылки на модуль отрисовки
        }

        public float GetEnergy()
        {
            Battery battery = playerShip.Equipment[(int) Ship.EquipmentNames.Battery] as Battery;
            return (float)battery.Energy / (float)battery.MaxEnergy *100;
        }

        /// <summary>
        /// Основаная функция
        /// </summary>
        public void Process()
        {
            Vector2f shipOffset = this.lastPlayerCoords - this.playerShip.Coords;//Вычисление смещения игрока
            this.lastPlayerCoords = this.playerShip.Coords;//сохранение новых координат
            this.activeSystem.OffsetBackground(shipOffset*(float)(-0.90));//установить смещение фона активной звездной системы
            GameRenderer.GameView.Center = this.PlayerShip.Coords;//Установка камеры
        }


    }
}
