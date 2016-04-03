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
        private static PlayerContainer container = null;
        
        /// <summary>
        /// Координаты игрока на предыдущей итерации
        /// </summary>
        private Vector2f lastPlayerCoords;

        /// <summary>
        /// Корабль игрока
        /// </summary>
        private ActiveObject controllingObject = null;

        /// <summary>
        /// Корабль игрока
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
        public PlayerContainer(BaseEnvironment environment)
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
        /// Установить ручное управление Игроком
        /// </summary>
        public void SetHandControlling()
        {
            this.handControlling = true;//установить флаг ручного управления
            PlayerController controller = PlayerController.GetInstanse();//получить экземпляр ручного контроллера
            controller.SetNewController(this);//установить новый Контейнер Игрока
            this.controllingObject.SetBrains(controller);//Установить Игроку ручной контроллер
        }

        /// <summary>
        /// Установить управление нейроконтроллером
        /// </summary>
        public void UnsetHandControlling()
        {
            this.handControlling = false;//сбросить флаг ручного управления
            this.controllingObject.SetBrains(this.savingComputerController);//установить Игроку нейроконтроллер
        }

        /// <summary>
        /// Установить контролируемого игрока
        /// </summary>
        /// <param name="activeObject">Игрок</param>
        public void SetControllingActiveObject(ActiveObject activeObject)
        {
            this.controllingObject = activeObject;
            this.lastPlayerCoords = this.controllingObject.Coords;//последних координат корабля игрока
            this.savingComputerController = new ComputerController(this);//Создание нейроконтроллера для игрока
            this.controllingObject.SetBrains(this.savingComputerController);//Установка нейроконтроллера
        }

        /// <summary>
        /// Текущий запас прочности корабля игрока  в процентах
        /// </summary>
        /// <returns></returns>
        public float GetHealh()
        {
            return this.controllingObject.Health * 100 / this.controllingObject.MaxHealth;
        }

        /// <summary>
        /// Текущий энергозапас корабля игрока в процентах
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
        /// Радиус действия радара игрока
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
        /// Центрировать камеру на данном игроке
        /// </summary>
        public void ViewCenteringFunc()
        {
            if (this.viewCentering)
            {
                this.lastPlayerCoords = this.controllingObject.Coords;//сохранение новых координат
                this.GameRenderer.GameView.Center = this.controllingObject.Coords;//Установка камеры
            }
        }


    }
}
