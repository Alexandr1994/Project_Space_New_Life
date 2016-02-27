using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Controlers;
using Project_Space___New_Live.modules.Controlers.Forms;
using Project_Space___New_Live.modules.Controlers.InterfaceParts;
using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.Dispatchers
{
    public class PlayerContainer
    {

        /// <summary>
        /// Режимы работы контейнер 
        /// </summary>
        public enum Mode : int
        {
            /// <summary>
            /// Космический режим
            /// </summary>
            SpaceMode = 0,
            /// <summary>
            /// Танковый режим
            /// </summary>
            TankMode
        }

        /// <summary>
        /// Текущий режим
        /// </summary>
        private Mode currentMode;

        /// <summary>
        /// Текущий режим
        /// </summary>
        public Mode CurrentMode
        {
            get { return this.currentMode; }
        }

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
        /// Танк игрока
        /// </summary>
        private Tank playerTank;

        /// <summary>
        /// Танк игрока
        /// </summary>
        public Tank PlayerTank
        {
            get { return this.playerTank; }
        }

        public Transport ActiveTransport
        {
            get
            {
                switch (this.currentMode)
                {
                    case Mode.SpaceMode:
                    {
                        return this.PlayerShip;
                    }
                    case Mode.TankMode:
                    {
                        return this.PlayerTank;
                    }
                    default:
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Активная звездная система
        /// </summary>
        private BaseEnvironment activeEnvironment = null;

        /// <summary>
        /// Активная звездная система
        /// </summary>
        public BaseEnvironment ActiveEnvironment
        {
            get { return this.activeEnvironment; }
            set { this.activeEnvironment = value; }//(временно)
        }

        /// <summary>
        /// Поучить экземпляр контейнера
        /// </summary>
        /// <param name="GameWorld">Коллекция звездных систем</param>
        /// <returns>Экземпляр контейнера игрока</returns>
        public static PlayerContainer GetInstanse(List<StarSystem> GameWorld)
        {
            if (container == null)
            {
                container = new PlayerContainer(GameWorld);
            }
            return container;
        }

        /// <summary>
        /// Десантирование танка на планету (ВРЕМЕННАЯ РЕАЛИЗАЦИЯ!!)
        /// </summary>
        public void OnPlanetLanding()
        {
            this.lastPlayerCoords = this.PlayerTank.Coords;
            this.currentMode = Mode.TankMode;

            List<Wall> wallsSystem = new List<Wall>();
            wallsSystem.Add(new Wall(new Vector2f(300, 450), new Vector2f(150, 20), new Texture[] { ResurceStorage.PanelText }, false));
            wallsSystem.Add(new Wall(new Vector2f(450, 450), new Vector2f(20, 100), new Texture[] { ResurceStorage.PanelText }, false));
            wallsSystem.Add(new Wall(new Vector2f(470, 550), new Vector2f(150, 20), new Texture[] { ResurceStorage.PanelText }, false));
            wallsSystem.Add(new Wall(new Vector2f(530, 450), new Vector2f(20, 100), new Texture[] { ResurceStorage.PanelText }, false));

            this.activeEnvironment = this.PlayerTank.Environment = new BattleField(wallsSystem, ResurceStorage.RockTexture);
            List<ActiveObject> unitsCollection = new List<ActiveObject>();
            unitsCollection.Add(this.PlayerTank);
            Tank testTank = new Tank(50, new Vector2f(1300, 1300), 150,  ResurceStorage.TankTextures, new Vector2f(15, 30));
            testTank.SetBrains(new ComputerController(testTank));
            testTank.Environment = this.activeEnvironment;
            unitsCollection.Add(testTank);
            testTank = new Tank(150, new Vector2f(700, 700), 150, ResurceStorage.TankTextures, new Vector2f(15, 30));
            testTank.SetBrains(new ComputerController(testTank));
            testTank.Environment = this.activeEnvironment;
            unitsCollection.Add(testTank);
            this.activeEnvironment.RefreshActiveObjectsCollection(unitsCollection);
        }

        /// <summary>
        /// Конструктор контейнера
        /// </summary>
        /// <param name="GameWorld">Коллекция звездных систем</param>
        /// <param name="playerInterface">Ссылка на графический интерфейс игрока</param>
        private PlayerContainer(List<StarSystem> GameWorld)
        {//Временная реализация конструктора корабля
            //Сохранение в контейнере
            PlayerController controller = PlayerController.GetInstanse(this);
            this.currentMode = (int) Mode.SpaceMode;
            this.playerShip = new Ship(500, new Vector2f(400, 400), 250, ResurceStorage.shipTextures, new Vector2f(15, 30), GameWorld[0]);//Корабля игрока
            this.playerShip.SetBrains(controller);
            this.playerTank = new Tank(150, new Vector2f(1000, 1000), 150,  ResurceStorage.TankTextures, new Vector2f(15, 30));
            this.PlayerTank.SetBrains(controller);
            this.lastPlayerCoords = this.playerShip.Coords;//последних координат корабля игрока
            this.activeEnvironment = this.playerShip.ShipStarSystem;//Текущей звездной системы
            this.GameRenderer = RenderModule.getInstance();//Сылки на модуль отрисовки
            //this.playerInterface = playerInterface;

        }

        /// <summary>
        /// Текущий запас прочности корабля игрока  в процентах
        /// </summary>
        /// <returns></returns>
        public float GetHealh()
        {
            return this.ActiveTransport.Health * 100 / this.ActiveTransport.MaxHealth;
        }

        /// <summary>
        /// Текущий энергозапас корабля игрока в процентах
        /// </summary>
        /// <returns></returns>
        public float GetEnergy()
        {
            Battery battery = this.ActiveTransport.Equipment[(int)(Transport.EquipmentNames.Battery)] as Battery;
            return (float)battery.Energy / (float)battery.MaxEnergy * 100;
        }

        /// <summary>
        /// Текущая мощность экрана энергощита в процентах
        /// </summary>
        /// <returns></returns>
        public float GetShieldPower()
        {
            if (this.ActiveTransport.ShieldActive)
            {
                Shield shield = ActiveTransport.Equipment[(int)Transport.EquipmentNames.Shield] as Shield;
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
            return this.ActiveTransport.ObjectWeaponSystem.GetAmmoPersent();
        }

        /// <summary>
        /// Радиус действия радара игрока
        /// </summary>
        /// <returns></returns>
        public float GetRadarRange()
        {
            if (this.ActiveTransport.Equipment[(int)Transport.EquipmentNames.Radar] == null)
            {//если радар отстутствует вернуть 0
                return 0;
            }
            Radar radar = this.ActiveTransport.Equipment[(int)Transport.EquipmentNames.Radar] as Radar;
            return radar.VisibleRadius;//вернуть радиус заны видимости радара
        }

        /// <summary>
        /// Функция работы конетйнера игрока
        /// </summary>
        public void Process()
        {
            this.activeEnvironment.OffsetBackground(this.ActiveTransport.Coords, this.lastPlayerCoords);//установить смещение фона активной звездной системы
            this.lastPlayerCoords = this.ActiveTransport.Coords;//сохранение новых координат
            this.GameRenderer.GameView.Center = this.ActiveTransport.Coords;//Установка камеры
        }


    }
}
