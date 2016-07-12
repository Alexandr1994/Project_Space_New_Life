using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules;
using Project_Space___New_Live.modules.Forms;using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.Dispatchers
{
    ///// <summary>
    ///// Система мониторинга объекта
    ///// </summary>
    //public class ObjectContainer
    //{

    //    /// <summary>
    //    /// Счетчки "гибелей" объекта
    //    /// </summary>
    //    private int deathCounter = 0;

    //    /// <summary>
    //    /// Количество "гибелей" объекта
    //    /// </summary>
    //    public int DeathCount
    //    {
    //        get { return this.deathCounter; }
    //    }

    //    /// <summary>
    //    /// Счетчик побед объекта
    //    /// </summary>
    //    private int winCounter = 0;

    //    /// <summary>
    //    /// Количество побед
    //    /// </summary>
    //    public int WinCount
    //    {
    //        get { return this.winCounter; }
    //    }

    //    /// <summary>
    //    /// Инкремент счетчика побед
    //    /// </summary>
    //    public void AddWin()
    //    {
    //        this.winCounter ++;
    //    }

    //    /// <summary>
    //    /// Сохраняемый нейроконтроллер
    //    /// </summary>
    //    private ComputerController savingComputerController;

    //    /// <summary>
    //    /// Флаг центрирования данного контейнера
    //    /// </summary>
    //    private bool viewCentering = false;

    //    /// <summary>
    //    /// Флаг центрирования данного контейнера
    //    /// </summary>
    //    public bool ViewCentering
    //    {
    //        get { return this.viewCentering; }
    //    }

    //    /// <summary>
    //    /// Флаг ручного управления
    //    /// </summary>
    //    private bool handControlling = false;

    //    /// <summary>
    //    /// Флаг ручного управления
    //    /// </summary>
    //    public bool HandControlling
    //    {
    //        get { return this.handControlling; }
    //    }

    //    /// <summary>
    //    /// Ссылка на графический модуль
    //    /// </summary>
    //    private RenderModule GameRenderer;

       
    //    /// <summary>
    //    /// Координаты объекта на предыдущей итерации
    //    /// </summary>
    //    private Vector2f lastPlayerCoords;

    //    /// <summary>
    //    /// Корабль объекта
    //    /// </summary>
    //    private ActiveObject1 controllingObject = null;

    //    /// <summary>
    //    /// Корабль объекта
    //    /// </summary>
    //    public ActiveObject1 ControllingObject
    //    {
    //        get { return this.controllingObject; }
    //    }

    //    /// <summary>
    //    /// Активная звездная система
    //    /// </summary>
    //    private BaseEnvironment environment = null;

    //    /// <summary>
    //    /// Активная звездная система
    //    /// </summary>
    //    public BaseEnvironment Environment
    //    {
    //        get { return this.environment; }
    //        set { this.environment = value; }//(временно)
    //    }

    //    /// <summary>
    //    /// Конструктор контейнера
    //    /// </summary>
    //    /// <param name="GameWorld">Коллекция звездных систем</param>
    //    public ObjectContainer(BaseEnvironment environment)
    //    {
    //        this.environment = environment;
    //        this.GameRenderer = RenderModule.getInstance();//Ссылка на модуль отрисовки
    //    }

    //    /// <summary>
    //    /// Установить флаг центрирования для данного контейнера
    //    /// </summary>
    //    public void SetViewCentering()
    //    {
    //        this.viewCentering = true;
    //    }

    //    /// <summary>
    //    /// Сбросить флаг центрирования для данного контейнера
    //    /// </summary>
    //    public void UnsetViewCentering()
    //    {
    //        this.viewCentering = false;
    //    }

    //    /// <summary>
    //    /// Установить ручное управление объектом
    //    /// </summary>
    //    public void SetHandControlling()
    //    {
    //        this.handControlling = true;//установить флаг ручного управления
    //        PlayerController controller = PlayerController.GetInstanse();//получить экземпляр ручного контроллера
    //        controller.SetNewController(this);//установить новый Контейнер объекта
    //        this.controllingObject.SetBrains(controller);//Установить объекту ручной контроллер
    //    }

    //    /// <summary>
    //    /// Установить управление нейроконтроллером
    //    /// </summary>
    //    public void UnsetHandControlling()
    //    {
    //        this.handControlling = false;//сбросить флаг ручного управления
    //        this.controllingObject.SetBrains(this.savingComputerController);//установить объекту нейроконтроллер
    //    }

    //    /// <summary>
    //    /// Установить контролируемого объекта
    //    /// </summary>
    //    /// <param name="activeObject">Объект</param>
    //    public void SetControllingActiveObject(ActiveObject1 activeObject, int objectIndex, int decisionTime, bool neuronControlling = false)
    //    {
    //        this.controllingObject = activeObject;
    //        this.lastPlayerCoords = this.controllingObject.Coords;//последних координат корабля объекта
    //        this.savingComputerController = new ComputerController(this, "statistic_object_" + objectIndex.ToString(), decisionTime);//Создание нейроконтроллера для объекта
    //        this.savingComputerController.NeronControlling = neuronControlling;
    //        this.controllingObject.SetBrains(this.savingComputerController);//Установка нейроконтроллера
    //    }

    //    /// <summary>
    //    /// Текущий запас прочности корабля объекта в процентах
    //    /// </summary>
    //    /// <returns></returns>
    //    public float GetHealh()
    //    {
    //        return this.controllingObject.Health * 100 / this.controllingObject.MaxHealth;
    //    }

    //    /// <summary>
    //    /// Текущий энергозапас корабля объекта в процентах
    //    /// </summary>
    //    /// <returns></returns>
    //    public float GetEnergy()
    //    {
    //        Battery battery = this.controllingObject.Equipment[(int)(ActiveObject1.EquipmentNames.Battery)] as Battery;
    //        return (float)battery.Energy / (float)battery.MaxEnergy * 100;
    //    }

    //    /// <summary>
    //    /// Текущая мощность экрана энергощита в процентах
    //    /// </summary>
    //    /// <returns></returns>
    //    public float GetShieldPower()
    //    {
    //        Shield shield = controllingObject.Equipment[(int)ActiveObject1.EquipmentNames.Shield] as Shield;
    //        return (float) shield.ShieldPower / (float) shield.MaxShieldPower * 100;
    //    }
        
    //    /// <summary>
    //    /// Текущий боезапас активного оружия в процентах
    //    /// </summary>
    //    /// <returns></returns>
    //    public float GetWeaponAmmo()
    //    {
    //        return this.controllingObject.ObjectWeaponSystem.GetAmmoPersent();
    //    }

    //    /// <summary>
    //    /// Радиус области видимости объекта
    //    /// </summary>
    //    /// <returns></returns>
    //    public float GetRadarRange()
    //    {
    //        if (this.controllingObject.Equipment[(int)ActiveObject1.EquipmentNames.Radar] == null)
    //        {//если радар отстутствует вернуть 0
    //            return 0;
    //        }
    //        Radar radar = this.controllingObject.Equipment[(int)ActiveObject1.EquipmentNames.Radar] as Radar;
    //        return radar.VisibleRadius;//вернуть радиус заны видимости радара
    //    }

    //    /// <summary>
    //    /// Центрировать камеру на данном объекте
    //    /// </summary>
    //    public void ViewCenteringFunc()
    //    {
    //        if (this.viewCentering)
    //        {
    //            this.lastPlayerCoords = this.controllingObject.Coords;//сохранение новых координат
    //            this.GameRenderer.GameView.Center = this.controllingObject.Coords;//Установка камеры
    //        }
    //    }

    //    /// <summary>
    //    /// Осуществление контроля над объектом
    //    /// </summary>
    //    public void StatePlayerControll()
    //    {
    //        if (this.controllingObject.Destroyed)
    //        {
    //            this.deathCounter ++;
    //            Random rand = new Random();
    //            this.controllingObject.Reborn(new Vector2f((float)(rand.NextDouble() * 1000), (float)(rand.NextDouble() * 1000)));
    //        }
    //    }

    //}

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
            Tank testTank = new Tank(50, new Vector2f(1300, 1300), 150, ResurceStorage.TankTextures, new Vector2f(15, 30));
            testTank.SetBrains(new ComputerController(testTank, 3000));
            testTank.Environment = this.activeEnvironment;
            unitsCollection.Add(testTank);
            testTank = new Tank(150, new Vector2f(700, 700), 150, ResurceStorage.TankTextures, new Vector2f(15, 30));
            testTank.SetBrains(new ComputerController(testTank, 3000));
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
            
            
            this.currentMode = (int)Mode.SpaceMode;
            this.playerShip = new Ship(500, new Vector2f(400, 400), 250, ResurceStorage.shipTextures, new Vector2f(15, 30), GameWorld[0]);//Корабля игрока
            
            this.playerTank = new Tank(150, new Vector2f(1000, 1000), 150, ResurceStorage.TankTextures, new Vector2f(15, 30));
            
            PlayerController controller = PlayerController.GetInstanse(playerShip);
            this.playerShip.SetBrains(controller);
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
