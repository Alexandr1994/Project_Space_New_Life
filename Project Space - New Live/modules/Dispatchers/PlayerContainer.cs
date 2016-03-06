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
        /// Поучить экземпляр контейнера
        /// </summary>
        /// <param name="GameWorld">Коллекция звездных систем</param>
        /// <returns>Экземпляр контейнера игрока</returns>
        public static PlayerContainer GetInstanse(BaseEnvironment environment)
        {
            if (container == null)
            {
                container = new PlayerContainer(environment);
            }
            return container;
        }

        /// <summary>
        /// Десантирование танка на планету (ВРЕМЕННАЯ РЕАЛИЗАЦИЯ!!)
        /// </summary>
        public void OnPlanetLanding()
        {
            ;
        }

        /// <summary>
        /// Конструктор контейнера
        /// </summary>
        /// <param name="GameWorld">Коллекция звездных систем</param>
        private PlayerContainer(BaseEnvironment environment)
        {
            this.environment = environment;
            this.GameRenderer = RenderModule.getInstance();//Сылки на модуль отрисовки
        }

        public void SetControllingActiveObject(ActiveObject activeObject)
        {
            this.controllingObject = activeObject;
            PlayerController controller = PlayerController.GetInstanse(this);
            this.controllingObject.SetBrains(controller);
            this.lastPlayerCoords = this.controllingObject.Coords;//последних координат корабля игрока
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
        /// Функция работы конетйнера игрока
        /// </summary>
        public void Process()
        {
            this.lastPlayerCoords = this.controllingObject.Coords;//сохранение новых координат
            this.GameRenderer.GameView.Center = this.controllingObject.Coords;//Установка камеры
        }


    }
}
