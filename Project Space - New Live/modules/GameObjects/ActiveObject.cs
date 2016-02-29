using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Controlers;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Активный игровой объект
    /// </summary>
    public class ActiveObject : GameObject
    {

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

        /// <summary>
        /// Текстурная лента визуального эффекта гибели объекта
        /// </summary>
        protected Texture visualEffectSkin;

        /// <summary>
        /// Сконструировать визуальный эффект гибели объекта
        /// </summary>
        /// <param name="effectSize">Размер кадра</param>
        /// <param name="effectTapeLenght">Количество кадров</param>
        /// <returns>Визуальный эффект гибели объекта</returns>
        public virtual VisualEffect ConstructDeathVisualEffect(Vector2f effectSize, int effectTapeLenght)
        {
            return new VisualEffect(this.Coords, effectSize, effectTapeLenght, visualEffectSkin);
        }

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

        /// <summary>
        /// Отображение корабля
        /// </summary>
        public override ObjectView[] View
        {
            get
            {
                if (this.ShieldActive)
                {
                    return this.view;
                }
                ObjectView[] retViews = new ObjectView[4];
                int index = 0;
                while (index != (int)(Parts.Shield))
                {
                    retViews[index] = this.view[index];
                    index++;
                }
                return retViews;
            }
        }


        //КОНТРОЛЛЕР И СРЕДА ОБЪЕКТА
        
        /// <summary>
        /// Конторллер активного объекта 
        /// </summary>
        protected AbstractController brains;

        /// <summary>
        /// Умтановка контроллера объекта
        /// </summary>
        /// <param name="newBrains">Контроллер</param>
        public void SetBrains(AbstractController newBrains)
        {
            if (this.brains == null)
            {
                this.brains = newBrains;
            }
        }



        /// <summary>
        /// Экземпляр среды, в которой находится данный объект
        /// </summary>
        protected BaseEnvironment environment;

        /// <summary>
        /// Экземпляр среды, в которой находится данный объект
        /// </summary>
        public BaseEnvironment Environment
        {
            get { return this.environment; }
            set { this.environment = value; }
        }

        //ОБЩЕЕ ОБОРУДОВАНИЕ/СНАРЯЖЕНИЕ АКТИВНЫХ ОБЪЕКТОВ

        /// <summary>
        /// Реактор/Генератор активного объекта
        /// </summary>
        protected Reactor objectReactor;

        /// <summary>
        /// Энергобатарея активного объекта
        /// </summary>
        protected Battery objectBattery;

        /// <summary>
        /// Радар/сканер активного объекта
        /// </summary>
        protected Radar objectRadar;

        /// <summary>
        /// Энергощит/персональный энергощит активного объекта
        /// </summary>
        protected Shield objectShield;

        /// <summary>
        /// Двигатель транспортного средства
        /// </summary>
        protected Engine transportEngine;

        /// <summary>
        /// Коллекция оборудования корабля
        /// </summary>
        public List<Equipment> Equipment
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

        //ФЛАГИ ОБЪЕКТА

        /// <summary>
        /// Флаг уничтожения активного объекта
        /// </summary>
        protected bool destroyed = false;

        /// <summary>
        /// Флаг уничтожения  активного объекта
        /// </summary>
        public bool Destroyed
        {
            get { return this.destroyed; }
        }

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

        //ПОЛЯ И МЕТОДЫ ДИВЖЕНИЯ ОБЪЕКТА

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
        /// Текущий поворот активного объекта в рад.
        /// </summary>
        protected float rotation = (float)(Math.PI / 2);

        /// <summary>
        /// Текущий поворот активного объекта в рад.
        /// </summary>
        public float Rotation
        {
            get { return this.rotation; }
        }

        /// <summary>
        /// Угол атаки активного объекта
        /// </summary>
        public virtual float AttackAngle
        {
            get { return this.rotation; }
        }

        /// <summary>
        /// Элементарное движение активного объекта
        /// </summary>
        /// <param name="speed">Скорость движения</param>
        /// <param name="angle">Угол поворота вектора скорости</param>
        public virtual void ShipAtomMoving(float speed, float angle)
        {
            Vector2f tempCoords = this.coords;
            this.coords.X += (float)(speed * Math.Cos(angle));
            this.coords.Y += (float)(speed * Math.Sin(angle));
            Vector2f delta = this.coords - tempCoords;//Изменение по координатам Х и Y
            foreach (ObjectView partView in this.view)
            {
                partView.Translate(delta);
            }
        }

        /// <summary>
        /// Изменение угла поворота активного объекта
        /// </summary>
        /// <param name="angle">Угол на который происходит изменение в рад.</param>
        public virtual void ChangeRotation(float angle)
        {
            this.rotation += angle;//изменение текущего поворота корабля
            foreach (ObjectView partView in this.view)
            {
                partView.Rotate(this.coords, angle);//изменение каждой части отображения
            }
            if (this.rotation > 2 * Math.PI)
            {
                this.rotation -= (float)(2 * Math.PI);
            }
        }

        //СОСТОЯНИЕ ОБЪЕКТА

        /// <summary>
        /// Текущий запас прочности
        /// </summary>
        protected int health;

        /// <summary>
        /// Текущий запас прочности
        /// </summary>
        public int Health
        {
            get { return this.health; }
        }

        /// <summary>
        /// Максимальный запас прочности
        /// </summary>
        protected int maxHealth;

        /// <summary>
        /// Максимальный запас прочности
        /// </summary>
        public int MaxHealth
        {
            get { return this.maxHealth; }
        }

        //ЗАЩИТНАЯ СИСТЕМА

        /// <summary>
        /// Текущее состояние энергощита 
        /// </summary>
        public bool ShieldActive
        {
            get
            {
                if (this.objectShield != null)//если щит установлен
                {
                    return this.objectShield.State;//то опросить его состояние
                }
                return false;
            }
        }

        /// <summary>
        /// Активировать энергощит
        /// </summary>
        /// <returns>true - удалось активировать, false - не удалось</returns>
        public bool ActivateShield()
        {
            if (this.objectShield != null)//Если энергощит есть
            {
                return this.objectShield.Activate();
            }
            return false;
        }

        /// <summary>
        /// Деактивировать щит
        /// </summary>
        public void DeactivateShield()
        {
            this.objectShield.Deactivate();
        }

        //БОЕВАЯ СИСТЕМА

        /// <summary>
        /// Оружейная система активного объекта
        /// </summary>
        protected WeaponSystem objectWeaponSystem;

        /// <summary>
        /// Оружейная система корабля
        /// </summary>
        public WeaponSystem ObjectWeaponSystem
        {
            get { return this.objectWeaponSystem; }
        }

        /// <summary>
        /// Открыть огонь
        /// </summary>
        public void OpenFire()
        {
            this.objectWeaponSystem.Shooting = true;
        }

        /// <summary>
        /// Прекратить огонь
        /// </summary>
        public void StopFire()
        {
            this.objectWeaponSystem.Shooting = false;
        }

        /// <summary>
        /// Базовое получение урона активного объекта
        /// </summary>
        /// <param name="damage">Урон, наносимый объекту</param>
        /// <param name="equipmentDamage">Урон, наносимый оборудованию</param>
        /// <param name="damagedPartIndex">Пораженная часть объекта</param>
        public void GetDamage(int damage, int equipmentDamage, int damagedPartIndex)
        {
            if (!this.ShieldActive)//если щит не активен
            {//то нанести урон объекту
                this.WearingEquipment(equipmentDamage, damagedPartIndex);//Нанести повреждения оборудованию
                if (this.health > damage)
                {
                    this.health -= damage;
                }
                else
                {
                    this.health = 0;
                }
            }
            else
            {//иначе нанести урон экрану щита
                objectShield.GetDamageOnShield(damage, equipmentDamage / 5);
            }
        }

        /// <summary>
        /// Наношение урона оборудования
        /// </summary>
        /// <param name="equipmentDamage">Урон наносимый оборудованию</param>
        /// <param name="damagedPartIndex">Пораженная часть корабля</param>
        protected void WearingEquipment(int equipmentDamage, int damagedPartIndex)
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
        /// Нанести урон конкретному оборудованию
        /// </summary>
        /// <param name="equipment">Оборудование</param>
        /// <param name="damage">Величина урона</param>
        protected void WearingCustomEquipment(Equipment equipment, int damage)
        {
            if (equipment != null)
            {
                equipment.Wearing(damage);
            }
        }

        /// <summary>
        /// Восстановление активного объекта
        /// </summary>
        /// <param name="recovery">Величина восстановления</param>
        public void Recovery(int recovery)
        {
            if (this.health < this.MaxHealth)
            {
                if (Math.Abs(this.health - this.MaxHealth) > recovery)
                {
                    this.health += recovery;
                }
                else
                {
                    this.health = this.MaxHealth;
                }
            }
        }

        /// <summary>
        /// Управление энергией активного объекта
        /// </summary>
        protected void EnergyProcess()
        {
            this.objectBattery.ThrowExcessEnergy();//Сбросить избыточную энергию
            this.objectBattery.Charge(this.objectReactor.EnergyGeneration);//Работа реактора
            foreach (Equipment device in this.Equipment)//перебрать коллекцию оборудования/снаряжения
            {
                if (device != null)//если данное оборудование имеется
                {
                    if (this.objectBattery.Energy >= device.EnergyNeeds && device.State)//если оно активированно и энергобатарея может обеспечить его потребность в энергии 
                    {
                        this.objectBattery.Uncharge(device.EnergyNeeds);//то уменьшить заряд батареи на нужную величину
                    }
                    else//иначе деактивировать его
                    {
                        device.Deactivate();
                    }
                }
            }
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
            this.brains.Process();//отработка управляющей системы
            this.EnergyProcess();//отработка энергосистемы
            if ((shell = this.objectWeaponSystem.Process(this)) != null)//если в ходе работы оружейной системы был получен снаряд
            {
                this.Environment.AddNewShell(shell);//то отправить его в коллекцияю снарядов звездной системы
                this.MoveManager.ShellShoot(this, shell.SpeedVector, shell.Mass);//отдача от выстрела
            }
            this.Move();//отработка системы движений
        }

        /// <summary>
        /// Анализирование взаимодействия данного объекта с другими объектами в среде
        /// </summary>
        /// <summary>
        /// Анализирование взаимодействия корабля с объектами в звездной системе
        /// </summary>
        public void AnalizeObjectInteraction()
        {
            this.nearPlanet = false;
            List<GameObject> interactiveObjects = this.Environment.GetObjectsInEnvironment();//получить все объекты в звездной системе
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
                                    if (View[i].BorderContactAnalize(interactObject.View[(int)Star.Views.Star]))
                                    //с самой звездой
                                    {
                                        this.GetDamage(this.MaxHealth, 100, i); //нанести максимальный урон кораблю
                                    }
                                    if (View[i].BorderContactAnalize(interactObject.View[(int)Star.Views.Crown]))
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
                                    this.GetDamage(5, 5, (int)Parts.Shield); //нанести некоторый урон
                                }
                            }
                        }; break;
                    case "Planet"://обработка контакта с планетой
                        {
                            Planet planet = interactObject as Planet;
                            foreach (ObjectView partShipView in this.View)
                            {
                                if (partShipView.BorderContactAnalize(interactObject.View[0]))
                                {
                                    this.nearPlanet = true;
                                }
                            }
                        }; break;
                    case "ActiveObject"://обработка контакта с кораблем
                        {
                            ActiveObject ship = interactObject as ActiveObject;
                            if (ship == this)//если анализируемый корабли является данным кораблем
                            {
                                continue;//то перейти к анализу следующего объекта
                            }
                            for (int i = 0; i < this.View.Length; i++)//проанализировать возможность столкновения каждой части данного корабля
                            {
                                for (int j = 0; j < ship.View.Length; j++)//с каждой частью проверяемого корабля
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
                        }; break;
                    case "Shell"://обработка контакта со снарядом
                        {
                            Shell shell = interactObject as Shell;
                            if (shell.ShooterObject == this)//если снаряд выпущен данным кораблем
                            {
                                continue;//то переходим к анализу сдежующего объекта
                            }
                            for (int i = 0; i < this.View.Length; i++)
                            {
                                if (this.View[i].BorderContactAnalize(shell.View[(int)(Shell.ShellParts.Core)]))//если произошло пересечение отображений корабля и снаряда
                                {
                                    this.GetDamage(shell.ObjectDamage, shell.EquipmentDamage, i);//то нанести кораблю урон
                                    this.MoveManager.ShellHit(this, shell.SpeedVector, shell.Mass);//инерция от попадания
                                    shell.HitToTarget();//и установить флаг окончания жизни снаряда
                                    break;
                                }
                            }
                        }; break;
                }
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
        public ActiveObject(float mass, Vector2f coords, int maxHealth, Texture[] skin, Vector2f partSize, BaseEnvironment startSystem)
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
            this.view = new ObjectView[5];
            int index = 0;
            while (index != (int)(Parts.Shield))
            {
                this.view[index] = new ObjectView(BlendMode.Alpha);
                this.view[index].Image = new RectangleShape(this.ViewPartSize);
                this.view[index].Image.Texture = skin[index];
                index++;
            }
            this.view[(int)Parts.Shield] = new ObjectView(BlendMode.Alpha);
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
            signature.AddCharacteristics(this.mass);
            Vector2f sizes = new Vector2f(this.ViewPartSize.X * 3, this.ViewPartSize.Y * 2);
            signature.AddCharacteristics(sizes);
            return signature;
        }

        //ПАРАМЕТРЫ ТРАНСПОРТНОГО СРЕДСТВА

   /*   

        // ОБОРУДОВАНИЕ ТРАНСПОРТНОГО СРЕДСТВА









        //ФЛАГИ КОРАБЛЯ


        /// <summary>
        /// Звездная система, в которой находится корабль
        /// </summary>
        public StarSystem ShipStarSystem
        {
            get { return this.Environment as StarSystem; }
        }


        




*/

    }
}
