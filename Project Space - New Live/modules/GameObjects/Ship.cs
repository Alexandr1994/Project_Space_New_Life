﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Controlers;
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

        
        //Общие данные о корабле

        /// <summary>
        /// Размер части корабля
        /// </summary>
        private Vector2f viewPartSize;
        /// <summary>
        /// Полная масса корабля 
        /// </summary>
        private float Mass
        {//Скоро полное описание
            get { return this.mass; }
        }

        /// <summary>
        /// Конторллер корабля
        /// </summary>
        private AbstractController pilot; 

        //Данные необходимые для перемещения корабля
        /// <summary>
        /// Текущая скорость перемещения корабля вперед-назад
        /// </summary>
        private float speed = 0;
        /// <summary>
        /// <summary>
        /// Текущая скорость бокового перемещения корабля
        /// </summary>
        private float sideSpeed = 0;
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
            this.ShipRotation();//Поворот корабля
            this.ShipSideMoving();//Боковое перемещение корабля
            this.ShipMainMoving();//Прямое перемещение корабля     
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
            this.coords.X += (float)(this.sideSpeed * Math.Sin(rotation));
            this.coords.Y += (float)(this.sideSpeed * Math.Cos(rotation));
            Vector2f delta = this.coords - tempCoords;//Изменение по координатам Х и Y
            foreach (ObjectView partView in this.View)
            {
                partView.Translate(delta);
            }
        }

        /// <summary>
        /// Ускорение маршевого двигателя
        /// </summary>
        public void ForwardAcceleration()
        {
            float acceleration = this.shipEngine.ForwardThrust / this.Mass;
            if (this.speed < (this.shipEngine.MaxSpeed + acceleration))
            {
                this.speed += acceleration;
            }
        }

        /// <summary>
        /// Ускорение реверсного двигателя
        /// </summary>
        public void ReverseAcceleration()
        {
            float acceleration = this.shipEngine.ShuntingThrust / this.Mass;
            if (this.speed > -(this.shipEngine.MaxSpeed + acceleration))
            {
                this.speed -= acceleration;
            }
        }

        /// <summary>
        /// Ускорение боковых дивгателей
        /// </summary>
        /// <param name="SideSingle">Знак ускорения (+) - смещение вправо (-) - смещение влево </param>
        public void SideAcceleration(int SideSingle)
        {
            float acceleration = (SideSingle / Math.Abs(SideSingle)) * this.shipEngine.ShuntingThrust/this.Mass;
            if (Math.Abs(this.speed) < (this.shipEngine.MaxSpeed + acceleration))
            {
                this.sideSpeed += acceleration;
            }   
        }

        /// <summary>
        /// Вращение
        /// </summary>
        /// <param name="SideSingle">Знак ускорения (+) - против (-) - по часовой стрелке</param>
        public void Rotation(int SideSingle)
        {
            float acceleration = SideSingle / Math.Abs(SideSingle) * this.shipEngine.ShuntingThrust / this.Mass;
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
            
            this.shipEngine = new Engine(100, 1, 150, 200, 5,null);
            this.shipBattery = new Battery(100, 500, null);
            this.shipReactor = new Reactor(100, 1, null);

            this.pilot = PlayerController.GetInstanse(this);
        }

        protected override ObjectSignature ConstructSignature()
        {
            return null;
        }
    }
}
