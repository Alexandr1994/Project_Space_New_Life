using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Controlers;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects.ShipModules;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    public class Ship : GameObject
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

        /// <summary>
        /// Идентификаторы оборудования корабля
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

        /// <summary>
        /// Оборудование корабля
        /// </summary>
        private List<ShipEquipment> shipEquipment;

        /// <summary>
        /// Оборудование корабля
        /// </summary>
        public List<ShipEquipment> Equipment
        {
            get { return this.shipEquipment; }
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
                while (index != (int) (Parts.Shield))
                {
                    retViews[index] = this.view[index];
                    index++;
                }
                return retViews;
            }
            
        }

        //Флаги корабля

        /// <summary>
        /// Текущее состояние энергощита 
        /// </summary>
        public bool ShieldActive
        {
            get
            {
                if (this.Equipment[(int) (EquipmentNames.Shield)] != null)//если щит установлен
                {
                    return this.Equipment[(int) (EquipmentNames.Shield)].State;//то опросить его состояние
                }
                return false;
            }
            set
            {
                if (this.Equipment[(int) (EquipmentNames.Shield)] != null)//если щит установлен
                {//изменить свойстово
                    (this.Equipment[(int)(EquipmentNames.Shield)] as Shield).State = value;
                }    
            }
        }

        //Параметры корабля

        /// <summary>
        /// Текущий запас прочности
        /// </summary>
        private int health;
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
        private int maxHealth;
        /// <summary>
        /// Максимальный запас прочности
        /// </summary>
        public int MaxHealth
        {
            get { return this.maxHealth; }
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
            set { this.viewPartSize = value;}
        }


        /// <summary>
        /// Полная масса корабля 
        /// </summary>
        public float Mass
        {//Скоро полное описание
            get { return this.mass; }
        }

        /// <summary>
        /// Конторллер корабля
        /// </summary>
        private AbstractController pilot; 

        //Данные и методы элементарного движения корабля (смещение и вращение)
        
        /// <summary>
        /// Модуль отвечающий за движения корабля
        /// </summary>
        private ShipMover moveManager = new ShipMover();


        /// <summary>
        /// Модуль отвечающий за движения корабля
        /// </summary>
        public ShipMover MoveManager
        {
            get { return this.moveManager; }
        }

        /// <summary>
        /// Текущий поворот корабля в рад.
        /// </summary>
        private float rotation = (float)(Math.PI/2);

        /// <summary>
        /// Текущий поворот корабля в рад.
        /// </summary>
        public float Rotation
        {
            get { return this.rotation; }
        }

        /// <summary>
        /// Индекс звездной системы, в которой находится корабль
        /// </summary>
        private int starSystemIndex;

        /// <summary>
        /// Индекс звездной системы, в которой находится корабль
        /// </summary>
        public int StarSystemIndex
        {
            get { return this.starSystemIndex; }
            set { this.starSystemIndex = value; }
        }

        /// <summary>
        /// Базовое получение урона кораблем
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="equipmentDamage"></param>
        /// <param name="damagedPartIndex"></param>
        public void GetDamage(int damage, int equipmentDamage, int damagedPartIndex)
        {
            if (!this.ShieldActive)//если щит не активен
            {//то нанести урон кораблю
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
                (this.Equipment[(int)(EquipmentNames.Shield)] as Shield).GetDamageOnShield(damage, equipmentDamage/5);
            }
        }

        /// <summary>
        /// Наношение урона оборудования
        /// </summary>
        /// <param name="equipmentDamage"></param>
        /// <param name="damagedPartIndex"></param>
        private void WearingEquipment(int equipmentDamage, int damagedPartIndex)
        {
            switch (damagedPartIndex)//в зависимости от части, которой было нанесено повреждение нанести урон оборудованию 
            {
                case (int)Parts.FrontPart://Носовая часть
                    {
                        this.Equipment[(int)(EquipmentNames.Battery)].Wearing(equipmentDamage);//Износ энергобатареи
                        this.Equipment[(int)(EquipmentNames.Radar)].Wearing(equipmentDamage);//Износ радара
                    }
                    ; break;
                case (int)Parts.FeedPart://Кормовая часть
                    {
                        this.Equipment[(int)(EquipmentNames.Reactor)].Wearing(equipmentDamage);//Износ реактора
                        this.Equipment[(int)(EquipmentNames.Engine)].Wearing(equipmentDamage);//Износ двигателя
                    }
                    ; break;
                case (int)Parts.LeftWing://Левое крыло
                    {
                        this.Equipment[(int)(EquipmentNames.Engine)].Wearing(equipmentDamage);//Износ двигателя
                    }
                    ; break;
                case (int)(Parts.RightWing)://Правое крыло
                    {
                        this.Equipment[(int)(EquipmentNames.Engine)].Wearing(equipmentDamage);//Износ двигателя
                    }
                    ; break;
                default: break;
            }
        }


        /// <summary>
        /// Ремонт корабля
        /// </summary>
        /// <param name="recovery"></param>
        public void Repair(int recovery)
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
        /// Постоянное перемещение корабля
        /// </summary>
        protected override void Move()
        {
            this.MoveManager.Process(this); 
        }

        /// <summary>
        /// Элементароне движение корабля
        /// </summary>
        public void ShipAtomMoving(float speed, float angle)
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
        /// Изменение поворота корабля
        /// </summary>
        /// <param name="angle">Угол на который происходит изменение</param>
        public void ChangeRotation(float angle)
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

        /// <summary>
        /// Постороение корабля
        /// </summary>
        /// <param name="mass">Масса</param>
        /// <param name="coords">Начальные координаты</param>
        /// <param name="textures">Набор текстур</param>
        /// <param name="newPartSize">Начальный размер составных частей</param>
        /// <param name="startSystemIndex">Индекс стартовой звездной системы</param>
        public Ship(float mass, Vector2f coords, int maxHealth, Texture[] skin, Vector2f partSize, int startSystemIndex)
        {
            this.mass = mass;
            this.coords = coords;
            this.viewPartSize = partSize;
            this.ConstructView(skin);
            this.shipEquipment = new List<ShipEquipment>();
            this.pilot = PlayerController.GetInstanse(this);
            this.maxHealth = this.health = maxHealth;

            this.shipEquipment.Add(new Engine(100, 1, 100, 100, 10, 8, null));//двигатель
            this.shipEquipment.Add(new Reactor(100, 1, null));//реактор
            this.shipEquipment.Add(new Battery(100, 500, null));//энергобатарея
            this.shipEquipment.Add(new Radar(20, 1500, null));//радар
            this.shipEquipment.Add(new Shield(20, 3, 100, 0, 1, null));//энергощит 
            // this.shipEquipment.Add(null);//энергощит 
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
                this.view[index].Image = new RectangleShape(this.viewPartSize);
                this.view[index].Image.Texture = skin[index];
                index++;
            }
            this.view[(int)Parts.Shield] = new ObjectView(BlendMode.Alpha);
            this.view[(int)Parts.Shield].Image = new CircleShape(this.viewPartSize.Y);
            this.view[(int)Parts.Shield].Image.Texture = skin[(int) Parts.Shield];
            this.view[(int)Parts.Shield].Image.Position = this.Coords - new Vector2f(this.viewPartSize.Y, this.viewPartSize.Y);//Энергощит
            this.view[(int)Parts.FrontPart].Image.Position = this.Coords + new Vector2f(-this.viewPartSize.X / 2, 0);//Носовя часть
            this.view[(int)Parts.FeedPart].Image.Position = this.Coords + new Vector2f(-this.viewPartSize.X / 2, -this.viewPartSize.Y);//Кормовая часть
            this.view[(int)Parts.RightWing].Image.Position = this.Coords + new Vector2f(this.viewPartSize.X / 2, -this.viewPartSize.Y * 3 / 4);//Левое "крыло"
            this.view[(int)Parts.LeftWing].Image.Position = this.Coords + new Vector2f(-this.viewPartSize.X * 3 / 2, -this.viewPartSize.Y * 3 / 4);//Правое "крыло"
        }

        /// <summary>
        /// Процесс жизни корабля
        /// </summary>
        /// <param name="homeCoords"></param>
        public override void Process(Vector2f homeCoords)
        {
            this.pilot.Process();
            this.EnergyProcess();



            
            this.Move();
        }

        /// <summary>
        /// Управление энергией корабля
        /// </summary>
        private void EnergyProcess()
        {
            Reactor shipReactor = this.shipEquipment[(int)EquipmentNames.Reactor] as Reactor;
            Battery shipBattery = this.shipEquipment[(int)EquipmentNames.Battery] as Battery;
            shipBattery.Charge(shipReactor.EnergyGeneration);
            for (int i = (int)(EquipmentNames.Radar); i < this.Equipment.Count; i++)
            {
                if (shipBattery.Energy >= this.Equipment[i].EnergyNeeds)
                {
                    if (this.Equipment[i].State)
                    {
                        shipBattery.Uncharge(this.Equipment[i].EnergyNeeds);
                    }
                }
                else
                {
                    this.Equipment[i].State = false;
                }              
            }
        }

        /// <summary>
        /// Сконструировать сигнатуру цели
        /// </summary>
        /// <returns></returns>
        protected override ObjectSignature ConstructSignature()
        {
            ObjectSignature signature = new ObjectSignature();
            signature.AddCharacteristics(this.mass, typeof(float));


            Vector2f sizes = new Vector2f(this.ViewPartSize.X * 3, this.ViewPartSize.Y * 2);
            signature.AddCharacteristics(sizes, sizes.GetType());
            return signature;
        }

        /// <summary>
        /// Анализирование взаимодействия корабля с объектами в звездной системе
        /// </summary>
        public void AnalizeObjectInteraction(StarSystem system)
        {
            List<GameObject> interactiveObjects = system.GetObjectsInSystem();//получить все объекты в звездной системе
            foreach (GameObject interactObject in interactiveObjects)
            {

                switch (interactObject.GetType().Name)
                {
                    case "Star"://проверка контакта со звездой
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
                                    break;
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
                                break;
                            }
                            if (View[(int)Parts.Shield].BorderContactAnalize(interactObject.View[(int)Star.Views.Crown]))
                            //со звездной короной
                            {
                                this.GetDamage(5, 5, (int) Parts.Shield); //нанести некоторый урон
                            }
                        }
                    };break;
                    case "Planet":
                    {
                        Planet planet = interactObject as Planet;
                        foreach (ObjectView partShipView in this.View)
                        {
                            if (partShipView.BorderContactAnalize(interactObject.View[0]))
                            {
                                this.Repair(1);
                            }
                        }
                    };break;
                }
            }
            
        }


    }
}
