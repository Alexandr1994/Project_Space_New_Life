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

        //Данные необходимые для перемещения корабля
        
        /// <summary>
        /// Модуль отвечающий за движения корабля
        /// </summary>
        private ShipMover moveManager = new ShipMover();

        private float rotationSpeed = 0;

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
        /// Построить отображение корабля
        /// </summary>
        /// <param name="skin">Массив текстур частей корабля</param>
        protected override void ConstructView(Texture[] skin)
        {
            this.view = new ObjectView[4];
            this.viewPartSize = new Vector2f(10, 20);
            for (int i = 0; i < this.view.Length; i++)
            {
                this.view[i] = new ObjectView(BlendMode.Alpha);
                this.view[i].Image = new RectangleShape(viewPartSize);
                this.view[i].Image.Texture = skin[i];
            }
            this.view[0].Image.Position = coords + new Vector2f(-5, -20);//Носовя часть
            this.view[1].Image.Position = coords + new Vector2f(-5, 0);//Кормовая часть
            this.view[2].Image.Position = coords + new Vector2f(-15, -15);//Левое "крыло"
            this.view[3].Image.Position = coords + new Vector2f(5, -15);//Правое "крыло"
        }

        //Методы движения корабля

        /// <summary>
        /// Постоянное перемещение корабля
        /// </summary>
        protected override void Move()
        {
            ShipRotation();
            this.MoveManager.Process(this); 
        }

        /// <summary>
        /// Поворот корабля
        /// </summary>
        private void ShipRotation()
        {
            this.rotation += this.rotationSpeed;//изменение текущего поворота корабля
            foreach (ObjectView partView in this.view)
            {
                partView.Rotate(this.coords, this.rotationSpeed);//изменение каждой части отображения
            }
        }

        /// <summary>
        /// Перемещение корабля вперед-назад
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
        /// Вращение
        /// </summary>
        /// <param name="SideSingle">Знак ускорения (+) - против (-) - по часовой стрелке</param>
        public void Rotate(int SideSingle)
        {
            Engine engine = this.shipEquipment[(int)EquipmentNames.Engine] as Engine;
            float acceleration = SideSingle / Math.Abs(SideSingle) * engine.ShuntingThrust / this.Mass;
            acceleration /= (float)Math.Sqrt(Math.Pow(viewPartSize.X / 4, 2) + Math.Pow(viewPartSize.Y / 4, 2));//вычисление углового ускорение
            this.rotationSpeed = acceleration;
        }

        /// <summary>
        /// Прекращение вращения
        /// </summary>
        public void StopRotation()
        {
            this.rotationSpeed = 0;
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
        /// Постороение корабля (временная реализация)
        /// </summary>
        public Ship(float mass, Vector2f coords, Texture[] textures)
        {
            this.mass = mass;
            this.coords = coords;
            this.ConstructView(textures);
            this.shipEquipment = new List<ShipEquipment>();

            this.shipEquipment.Add(new Engine(100, 1, 100, 100, 10, null));
            this.shipEquipment.Add(new Battery(100, 500, null));
            this.shipEquipment.Add(new Reactor(100, 1, null));

            this.pilot = PlayerController.GetInstanse(this);
        }

        protected override ObjectSignature ConstructSignature()
        {
            return null;
        }
    }
}
