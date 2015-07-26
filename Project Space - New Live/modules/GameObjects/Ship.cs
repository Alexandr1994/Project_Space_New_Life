using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects.ShipEquipment;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    public class Ship : GameObject
    {



        //Оборудование корабля

        /// <summary>
        /// Двигатель корабля
        /// </summary>
        private Engine shipEngine;
        /// <summary>
        /// Реактор корабля
        /// </summary>
        private Reactor shipReactor;
        /// <summary>
        /// Энергобатарея корабля
        /// </summary>
        private Battery shipBattery;


        //Данные необходимые для перемещения корабля
        /// <summary>
        /// Текущая скорость перемещения корабля вперед-назад
        /// </summary>
        private float speed = 0;
        /// <summary>
        /// Текущее ускорение перемещения корабля вперед-назад
        /// </summary>
        private float acceleration = 0;
        /// <summary>
        /// Текущая скорость бокового перемещения корабля
        /// </summary>
        private float sideSpeed = 0;
        /// <summary>
        /// Текущее ускорение бокового перемещения
        /// </summary>
        private float sideAcceleration = 0;
        /// <summary>
        /// Текущая скорость поворота корабля (рад/ед.вр)
        /// </summary>
        private float rotationSpeed = 0;
        /// <summary>
        /// Текущий поворот корабля в рад.
        /// </summary>
        private float rotation = (float)(Math.PI/2);

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
                this.view[i].Image = new RectangleShape(new Vector2f(10, 20));
                this.view[i].Image.Texture = skin[i];
            }
            this.view[0].Image.Position = coords - new Vector2f(-5, -20);//Носовя часть
            this.view[1].Image.Position = coords - new Vector2f(-5, 0);//Кормовая часть
            this.view[2].Image.Position = coords - new Vector2f(-15, -5);//Левое "крыло"
            this.view[3].Image.Position = coords - new Vector2f(5, -5);//Правое "крыло"
        }

        //Методы движения корабля

        /// <summary>
        /// Постоянное перемещение корабля
        /// </summary>
        protected override void Move()
        {
            this.ShipRotation();//Поворот корабля
            this.ShipMainMoving();//Прямое перемещение корабля
            this.ShipSideMoving();//Боковое перемещение корабля

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
        private void ShipMainMoving()
        {
            Vector2f tempCoords = this.coords;
            this.coords.X += (float) (this.speed * Math.Cos(rotation));
            this.coords.Y += (float) (this.speed * Math.Sin(rotation));
            Vector2f delta = this.coords - tempCoords;//Изменение по координатам Х и Y
            foreach (ObjectView partView in this.View)
            {
                partView.Translate(delta);
            }
        }

        /// <summary>
        /// боковое перемещение корабля
        /// </summary>
        private void ShipSideMoving()
        {
            Vector2f tempCoords = this.coords;
            this.coords.X += (float)(this.speed * Math.Cos(rotation));
            this.coords.Y += (float)(this.speed * Math.Sin(rotation));
            Vector2f delta = this.coords - tempCoords;//Изменение по координатам Х и Y
            foreach (ObjectView partView in this.View)
            {
                partView.Translate(delta);
            }
        }

        /// <summary>
        /// Изменения скоростей
        /// </summary>
        private void SpeedChanges()
        {
            this.speed += this.acceleration;
            this.sideSpeed += this.sideAcceleration;
        }

        /// <summary>
        /// Процесс жизни корабля
        /// </summary>
        /// <param name="homeCoords"></param>
        public override void Process(Vector2f homeCoords)
        {
            this.Move();
        }

        /// <summary>
        /// Постороение корабля (временная реализация)
        /// </summary>
        public Ship(Vector2f coords, Texture[] textures)
        {
            this.coords = coords;
            this.ConstructView(textures);
        }

        protected override ObjectSignature ConstructSignature()
        {
            return null;
        }
    }
}
