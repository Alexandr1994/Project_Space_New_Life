using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Controlers;
using Project_Space___New_Live.modules.Controlers.Forms;using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Система мониторинга объекта
    /// </summary>
    public class ObjectContainer
    {

        /// <summary>
        /// Счетчки "гибелей" объекта
        /// </summary>
        private int deathCounter = 0;

        /// <summary>
        /// Количество "гибелей" объекта
        /// </summary>
        public int DeathCount
        {
            get { return this.deathCounter; }
        }

        /// <summary>
        /// Счетчик побед объекта
        /// </summary>
        private int winCounter = 0;

        /// <summary>
        /// Количество побед
        /// </summary>
        public int WinCount
        {
            get { return this.winCounter; }
        }

        /// <summary>
        /// Инкремент счетчика побед
        /// </summary>
        public void AddWin()
        {
            this.winCounter ++;
        }

        /// <summary>
        /// Сохраняемый нейроконтроллер
        /// </summary>
        private ComputerController savingComputerController;

        /// <summary>
        /// Флаг центрирования данного контейнера
        /// </summary>
        private bool viewCentering = false;

        /// <summary>
        /// Флаг центрирования данного контейнера
        /// </summary>
        public bool ViewCentering
        {
            get { return this.viewCentering; }
        }

        /// <summary>
        /// Флаг ручного управления
        /// </summary>
        private bool handControlling = false;

        /// <summary>
        /// Флаг ручного управления
        /// </summary>
        public bool HandControlling
        {
            get { return this.handControlling; }
        }

        /// <summary>
        /// Ссылка на графический модуль
        /// </summary>
        private RenderModule GameRenderer;

        /// <summary>
        /// Экземпляр контейнера
        /// </summary>
        private static ObjectContainer container = null;
        
        /// <summary>
        /// Координаты объекта на предыдущей итерации
        /// </summary>
        private Vector2f lastPlayerCoords;

        /// <summary>
        /// Корабль объекта
        /// </summary>
        private ActiveObject controllingObject = null;

        /// <summary>
        /// Корабль объекта
        /// </summary>
        public ActiveObject ControllingObject
        {
            get { return this.controllingObject; }
        }

        /// <summary>
        /// Активная звездная система
        /// </summary>
        private BaseEnvironment environment = null;

        /// <summary>
        /// Активная звездная система
        /// </summary>
        public BaseEnvironment Environment
        {
            get { return this.environment; }
            set { this.environment = value; }//(временно)
        }

        /// <summary>
        /// Конструктор контейнера
        /// </summary>
        /// <param name="GameWorld">Коллекция звездных систем</param>
        public ObjectContainer(BaseEnvironment environment)
        {
            this.environment = environment;
            this.GameRenderer = RenderModule.getInstance();//Ссылка на модуль отрисовки
        }

        /// <summary>
        /// Установить флаг центрирования для данного контейнера
        /// </summary>
        public void SetViewCentering()
        {
            this.viewCentering = true;
        }

        /// <summary>
        /// Сбросить флаг центрирования для данного контейнера
        /// </summary>
        public void UnsetViewCentering()
        {
            this.viewCentering = false;
        }

        /// <summary>
        /// Установить ручное управление объектом
        /// </summary>
        public void SetHandControlling()
        {
            this.handControlling = true;//установить флаг ручного управления
            PlayerController controller = PlayerController.GetInstanse();//получить экземпляр ручного контроллера
            controller.SetNewController(this);//установить новый Контейнер объекта
            this.controllingObject.SetBrains(controller);//Установить объекту ручной контроллер
        }

        /// <summary>
        /// Установить управление нейроконтроллером
        /// </summary>
        public void UnsetHandControlling()
        {
            this.handControlling = false;//сбросить флаг ручного управления
            this.controllingObject.SetBrains(this.savingComputerController);//установить объекту нейроконтроллер
        }

        /// <summary>
        /// Установить контролируемого объекта
        /// </summary>
        /// <param name="activeObject">Объект</param>
        public void SetControllingActiveObject(ActiveObject activeObject)
        {
            this.controllingObject = activeObject;
            this.lastPlayerCoords = this.controllingObject.Coords;//последних координат корабля объекта
            this.savingComputerController = new ComputerController(this);//Создание нейроконтроллера для объекта
            this.controllingObject.SetBrains(this.savingComputerController);//Установка нейроконтроллера
        }

        /// <summary>
        /// Текущий запас прочности корабля объекта в процентах
        /// </summary>
        /// <returns></returns>
        public float GetHealh()
        {
            return this.controllingObject.Health * 100 / this.controllingObject.MaxHealth;
        }

        /// <summary>
        /// Текущий энергозапас корабля объекта в процентах
        /// </summary>
        /// <returns></returns>
        public float GetEnergy()
        {
            Battery battery = this.controllingObject.Equipment[(int)(ActiveObject.EquipmentNames.Battery)] as Battery;
            return (float)battery.Energy / (float)battery.MaxEnergy * 100;
        }

        /// <summary>
        /// Текущая мощность экрана энергощита в процентах
        /// </summary>
        /// <returns></returns>
        public float GetShieldPower()
        {
            if (this.controllingObject.ShieldActive)
            {
                Shield shield = controllingObject.Equipment[(int)ActiveObject.EquipmentNames.Shield] as Shield;
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
            return this.controllingObject.ObjectWeaponSystem.GetAmmoPersent();
        }

        /// <summary>
        /// Радиус области видимости объекта
        /// </summary>
        /// <returns></returns>
        public float GetRadarRange()
        {
            if (this.controllingObject.Equipment[(int)ActiveObject.EquipmentNames.Radar] == null)
            {//если радар отстутствует вернуть 0
                return 0;
            }
            Radar radar = this.controllingObject.Equipment[(int)ActiveObject.EquipmentNames.Radar] as Radar;
            return radar.VisibleRadius;//вернуть радиус заны видимости радара
        }

        /// <summary>
        /// Центрировать камеру на данном объекте
        /// </summary>
        public void ViewCenteringFunc()
        {
            if (this.viewCentering)
            {
                this.lastPlayerCoords = this.controllingObject.Coords;//сохранение новых координат
                this.GameRenderer.GameView.Center = this.controllingObject.Coords;//Установка камеры
            }
        }

        /// <summary>
        /// Осуществление контроля над объектом
        /// </summary>
        public void StatePlayerControll()
        {
            if (this.controllingObject.Destroyed)
            {
                this.deathCounter ++;
                Random rand = new Random();
                this.controllingObject.Reborn(new Vector2f((float)(rand.NextDouble() * 5000), (float)(rand.NextDouble() * 5000)));
            }
        }

    }
}
