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


        //Оборудование корабля

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
            Battery
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
            foreach (ObjectView partView in this.View)
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
        public Ship(float mass, Vector2f coords, Texture[] textures, Vector2f newPartSize, int startSystemIndex)
        {
            this.mass = mass;
            this.coords = coords;
            this.viewPartSize = newPartSize;
            this.ConstructView(textures);
            this.shipEquipment = new List<ShipEquipment>();
            this.pilot = PlayerController.GetInstanse(this);

            this.shipEquipment.Add(new Engine(100, 1, 100, 100, 10, 8, null));
            this.shipEquipment.Add(new Battery(100, 500, null));
            this.shipEquipment.Add(new Reactor(100, 1, null));

        }



        /// <summary>
        /// Построить отображение корабля
        /// </summary>
        /// <param name="skin">Массив текстур частей корабля</param>
        protected override void ConstructView(Texture[] skin)
        {
            this.view = new ObjectView[4];
            for (int i = 0; i < this.view.Length; i++)
            {
                this.view[i] = new ObjectView(BlendMode.Alpha);
                this.view[i].Image = new RectangleShape(viewPartSize);
                this.view[i].Image.Texture = skin[i];
            }
            this.view[0].Image.Position = coords + new Vector2f(-this.viewPartSize.X / 2, 0);//Носовя часть
            this.view[1].Image.Position = coords + new Vector2f(-this.viewPartSize.X / 2, -this.viewPartSize.Y);//Кормовая часть
            this.view[2].Image.Position = coords + new Vector2f(this.viewPartSize.X / 2, -this.viewPartSize.Y * 3 / 4);//Левое "крыло"
            this.view[3].Image.Position = coords + new Vector2f(-this.viewPartSize.X * 3 / 2, -this.viewPartSize.Y * 3 / 4);//Правое "крыло"
        }

        /// <summary>
        /// Процесс жизни корабля
        /// </summary>
        /// <param name="homeCoords"></param>
        public override void Process(Vector2f homeCoords)
        {
            this.pilot.Process();
            this.Move();

        }

        /// <summary>
        /// Сконструировать сигнатуру цели
        /// </summary>
        /// <returns></returns>
        protected override ObjectSignature ConstructSignature()
        {
            return null;
        }


    }
}
