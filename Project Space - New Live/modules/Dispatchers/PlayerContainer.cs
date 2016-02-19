using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Controlers.Forms;
using Project_Space___New_Live.modules.Controlers.InterfaceParts;
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

//        /// <summary>
//        /// Конструктор контейнера
//        /// </summary>
//        /// <param name="GameWorld"></param>

        /// <summary>
        /// Конструктор контейнера
        /// </summary>
        /// <param name="GameWorld">Коллекция звездных систем</param>
        /// <param name="playerInterface">Ссылка на графический интерфейс игрока</param>
        private PlayerContainer(List<StarSystem> GameWorld)
        {//Временная реализация конструктора корабля
            //Сохранение в контейнере
            this.playerShip = new Ship(500, new Vector2f(400, 400), 250, ResurceStorage.shipTextures, new Vector2f(15, 30), GameWorld[0]);//Корабля игрока
            this.lastPlayerCoords = this.playerShip.Coords;//последних координат корабля игрока
            this.activeSystem = this.playerShip.StarSystem;//Текущей звездной системы
            this.GameRenderer = RenderModule.getInstance();//Сылки на модуль отрисовки
            //this.playerInterface = playerInterface;
        }

        /// <summary>
        /// Текущий запас прочности корабля игрока  в процентах
        /// </summary>
        /// <returns></returns>
        public float GetHealh()
        {
            return this.playerShip.Health * 100 / this.playerShip.MaxHealth;
        }

        /// <summary>
        /// Текущий энергозапас корабля игрока в процентах
        /// </summary>
        /// <returns></returns>
        public float GetEnergy()
        {
            Battery battery = playerShip.Equipment[(int) Ship.EquipmentNames.Battery] as Battery;
            return (float)battery.Energy / (float)battery.MaxEnergy *100;
        }

        /// <summary>
        /// Текущая мощность экрана энергощита в процентах
        /// </summary>
        /// <returns></returns>
        public float GetShieldPower()
        {
            if (this.PlayerShip.ShieldActive)
            {
                Shield shield = PlayerShip.Equipment[(int)Ship.EquipmentNames.Shield] as Shield;
                return (float)shield.ShieldPower / (float)shield.MaxShieldPower * 100;
            }
            return 0;
        }
        
        /// <summary>
        /// Текущий боезапас активного оружия в процентах
        /// </summary>
        /// <returns></returns>
        public float GetWeaponAmmo()
        {
            return this.PlayerShip.ObjectWeaponSystem.GetAmmoPersent();
        }

        /// <summary>
        /// Радиус действия радара игрока
        /// </summary>
        /// <returns></returns>
        public float GetRadarRange()
        {
            if (this.PlayerShip.Equipment[(int) Ship.EquipmentNames.Radar] == null)
            {//если радар отстутствует вернуть 0
                return 0;
            }
            Radar radar = this.PlayerShip.Equipment[(int) Ship.EquipmentNames.Radar] as Radar;
            return radar.VisibleRadius;//вернуть радиус заны видимости радара
        }

        /// <summary>
        /// Функция работы конетйнера игрока
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
