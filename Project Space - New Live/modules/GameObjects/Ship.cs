using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Космический корабль (Космолет или Космоплан)
    /// </summary>
    public class Ship : Transport
    {
        /// <summary>
        /// Индексы частей корабля
        /// </summary>
        public enum Parts : int
        {
            /// <summary>
            /// носовая часть
            /// </summary>
            FrontPart = 0,
            /// <summary>
            /// кормовая часть
            /// </summary>
            FeedPart,
            /// <summary>
            /// правое крыло
            /// </summary>
            RightWing,
            /// <summary>
            /// левое крыло
            /// </summary>
            LeftWing,
            /// <summary>
            /// Энергощит
            /// </summary>
            Shield
        }

        //ФЛАГИ КОРАБЛЯ

        /// <summary>
        /// Флаг нахождения радом с планетой
        /// </summary>
        private bool nearPlanet = false;

        /// <summary>
        /// Флаг нахождения радом с планетой
        /// </summary>
        public bool NearPlanet
        {
            get { return this.nearPlanet; }
        }

        /// <summary>
        /// Отображение корабля
        /// </summary>
        public override ImageView[] View
        {
            get
            {
                if (this.ShieldActive)
                {
                    return this.view;
                }
                ImageView[] retViews = new ImageView[4];
                int index = 0;
                while (index != (int)(Parts.Shield))
                {
                    retViews[index] = this.view[index];
                    index++;
                }
                return retViews;
            }
        }

        /// <summary>
        /// Звездная система, в которой находится корабль
        /// </summary>
        public StarSystem ShipStarSystem
        {
            get { return this.Environment as StarSystem; }
        }

        /// <summary>
        /// Наношение урона оборудования
        /// </summary>
        /// <param name="equipmentDamage">Урон наносимый оборудованию</param>
        /// <param name="damagedPartIndex">Пораженная часть корабля</param>
        protected override void WearingEquipment(int equipmentDamage, int damagedPartIndex)
        {
            switch (damagedPartIndex)//в зависимости от части, которой было нанесено повреждение нанести урон оборудованию 
            {
                case (int)Parts.FrontPart://Носовая часть
                {
                    this.WearingCustomEquipment(this.objectBattery, equipmentDamage);//Износ энергобатареи
                    this.WearingCustomEquipment(this.objectRadar, equipmentDamage);//Износ радара
                }; break;
                case (int)Parts.FeedPart://Кормовая часть
                {
                    this.WearingCustomEquipment(this.objectReactor, equipmentDamage);//Износ реактора
                    this.WearingCustomEquipment(this.transportEngine, equipmentDamage);//Износ двигателя
                }; break;
                case (int)Parts.LeftWing://Левое крыло
                {
                    this.WearingCustomEquipment(this.transportEngine, equipmentDamage);//Износ двигателя
                }; break;
                case (int)(Parts.RightWing)://Правое крыло
                {
                    this.WearingCustomEquipment(this.transportEngine, equipmentDamage);//Износ двигателя
                }; break;
                default: break;
            }
        }
        
        /// <summary>
        /// Постороение корабля
        /// </summary>
        /// <param name="mass">Масса</param>
        /// <param name="coords">Начальные координаты</param>
        /// <param name="maxHealth">НАчальная максимальная прочность</param>
        /// <param name="skin">Массив текстур</param>
        /// <param name="partSize">Размер составной части корабля</param>
        /// <param name="startSystem">Звездная система</param>
        public Ship(float mass, Vector2f coords, int maxHealth, Texture[] skin, Vector2f partSize, StarSystem startSystem)
        {
            this.mass = mass;
            this.coords = coords;
            this.ViewPartSize = partSize;
            this.ConstructView(skin);
            this.maxHealth = this.health = maxHealth;
            this.Environment = startSystem;

            this.transportEngine = new Engine(100, 1, 200, 150, 10, 8, null);//двигатель
            this.objectReactor = new Reactor(100, 5, null);//реактор
            this.objectBattery = new Battery(100, 500, null);//энергобатарея
            this.objectRadar  = new Radar(20, 2500, null);//радар
            this.objectShield = new Shield(20, 3, 100, 0, 1, null);//энергощит 

            this.objectWeaponSystem = new WeaponSystem(3);
            this.objectWeaponSystem.AddWeapon(new Weapon(25, 1, 5, 5, 0, 0, (float) (5*Math.PI/180), 100, 100, 1000, 15, 10, new Vector2f(5, 2), new Texture[] {ResurceStorage.rectangleButtonTextures[0], ResurceStorage.shellHitting}, null));
            this.objectWeaponSystem.AddWeapon(new Weapon(25, 1, 5, 5, 0, 1, (float)(1 * Math.PI / 180), 100, 50, 5000, 25, 1, new Vector2f(25, 1), new Texture[] { ResurceStorage.rectangleButtonTextures[2], ResurceStorage.shellHitting}, null));
        }

        /// <summary>
        /// Построить отображение корабля
        /// </summary>
        /// <param name="skin">Массив текстур частей корабля</param>
        protected override void ConstructView(Texture[] skin)
        {
            //добавить защиту
            this.view = new ImageView[5];
            int index = 0;
            while (index != (int)(Parts.Shield))
            {
                this.view[index] = new ImageView(BlendMode.Alpha);
                this.view[index].Image = new RectangleShape(this.ViewPartSize);
                this.view[index].Image.Texture = skin[index];
                index++;
            }
            this.view[(int)Parts.Shield] = new ImageView(BlendMode.Alpha);
            this.view[(int)Parts.Shield].Image = new CircleShape(this.ViewPartSize.Y);
            this.view[(int)Parts.Shield].Image.Texture = skin[(int) Parts.Shield];
            this.view[(int)Parts.Shield].Image.Position = this.Coords - new Vector2f(this.ViewPartSize.Y, this.ViewPartSize.Y);//Энергощит
            this.view[(int)Parts.FrontPart].Image.Position = this.Coords + new Vector2f(-this.ViewPartSize.X / 2, 0);//Носовя часть
            this.view[(int)Parts.FeedPart].Image.Position = this.Coords + new Vector2f(-this.ViewPartSize.X / 2, -this.ViewPartSize.Y);//Кормовая часть
            this.view[(int)Parts.RightWing].Image.Position = this.Coords + new Vector2f(this.ViewPartSize.X / 2, -this.ViewPartSize.Y * 3 / 4);//Левое "крыло"
            this.view[(int)Parts.LeftWing].Image.Position = this.Coords + new Vector2f(-this.ViewPartSize.X * 3 / 2, -this.ViewPartSize.Y * 3 / 4);//Правое "крыло"
            this.visualEffectSkin = skin[skin.Length - 1];
        }

        /// <summary>
        /// Сконструировать сигнатуру цели
        /// </summary>
        /// <returns>Сигнатура корабля</returns>
        protected override ObjectSignature ConstructSignature()
        {
            ObjectSignature signature = new ObjectSignature();
            Vector2f sizes = new Vector2f(this.ViewPartSize.X * 3, this.ViewPartSize.Y * 2);
            signature.Mass = this.Mass;
            signature.Size = sizes;
            signature.Directon = this.Rotation;
            signature.Coords = this.Coords;
            signature.Speed = this.moveManager.ConstructResultVector().Speed;
            return signature;
        }

        /// <summary>
        /// Анализирование взаимодействия корабля с объектами в звездной системе
        /// </summary>
        public override void AnalizeObjectInteraction()
        {
            this.nearPlanet = false;
            List<GameObject> interactiveObjects = this.ShipStarSystem.GetObjectsInEnvironment();//получить все объекты в звездной системе
            foreach (GameObject interactObject in interactiveObjects)
            {
                switch (interactObject.GetType().Name)
                {
                    case "Star"://обработка контакта со звездой
                    {
                        Star star = interactObject as Star;
                        if (!ShieldActive)//если энергощит не активен
                        {//то проверяем все части корабля
                            for (int i = 0; i < this.View.Length; i++) //если одна из частей корабля контактирует 
                            {
                                if (View[i].BorderContactAnalize(interactObject.View[(int) Star.Views.Star]))
                                //с самой звездой
                                {
                                    this.GetDamage(this.MaxHealth, 100, i); //нанести максимальный урон кораблю
                                }
                                if (View[i].BorderContactAnalize(interactObject.View[(int) Star.Views.Crown]))
                                //со звездной короной
                                {
                                    this.GetDamage(5, 5, i); //нанести некоторый урон
                                }
                            }
                        }
                        else//если энергощит активен
                        {//то проверяем только энергощит
                            if (View[(int)Parts.Shield].BorderContactAnalize(interactObject.View[(int)Star.Views.Star]))
                            //с самой звездой
                            {
                                this.GetDamage(this.MaxHealth, 100, (int)Parts.Shield); //нанести максимальный урон кораблю
                            }
                            if (View[(int)Parts.Shield].BorderContactAnalize(interactObject.View[(int)Star.Views.Crown]))
                            //со звездной короной
                            {
                                this.GetDamage(5, 5, (int) Parts.Shield); //нанести некоторый урон
                            }
                        }
                    };break;
                    case "Planet"://обработка контакта с планетой
                    {
                        Planet planet = interactObject as Planet;
                        foreach (ImageView partShipView in this.View)
                        {
                            if (partShipView.BorderContactAnalize(interactObject.View[0]))
                            {
                                this.nearPlanet = true;
                            }
                        }
                    };break;
                    case "Ship"://обработка контакта с кораблем
                    {
                        Ship ship = interactObject as Ship;
                        if (ship == this)//если анализируемый корабли является данным кораблем
                        {
                            continue;//то перейти к анализу следующего объекта
                        }
                        for (int i = 0; i < this.View.Length; i ++)//проанализировать возможность столкновения каждой части данного корабля
                        {
                            for (int j = 0; j < ship.View.Length; j ++)//с каждой частью проверяемого корабля
                            {
                                 if (this.View[i].BorderContactAnalize(ship.View[j]))//и в случае пересечения отображений
                                {
                                    float deltaX = this.Coords.X - ship.Coords.X;//обработать столкновение
                                    float deltaY = this.Coords.Y - ship.Coords.Y;
                                    float contactAngle = (float)(Math.Atan2(deltaY, deltaX));
                                    this.MoveManager.CrashMove(ship.MoveManager, this.Mass, ship.Mass, contactAngle);
                                    //нанесение урона временная реализация
                                    this.GetDamage(50, 1, i);//и нанести урон как данному
                                    ship.GetDamage(50, 1, j);//так и проверяемому кораблю
                                }
                            }
                        }
                    };break;
                    case "Shell"://обработка контакта со снарядом
                    {
                        Shell shell = interactObject as Shell;
                        if (shell.ShooterObject == this)//если снаряд выпущен данным кораблем
                        {
                            continue;//то переходим к анализу сдежующего объекта
                        }
                        for (int i = 0; i < this.View.Length; i ++)
                        {
                            if (this.View[i].BorderContactAnalize(shell.View[(int)(Shell.ShellParts.Core)]))//если произошло пересечение отображений корабля и снаряда
                            {
                                this.GetDamage(shell.ObjectDamage, shell.EquipmentDamage, i);//то нанести кораблю урон
                            //    this.MoveManager.ShellHit(this, shell.SpeedVector, shell.Mass);//инерция от попадания
                                shell.HitToTarget();//и установить флаг окончания жизни снаряда
                                break;
                            }
                        }
                    };break;
                }
            }
            
        }

    }
}
