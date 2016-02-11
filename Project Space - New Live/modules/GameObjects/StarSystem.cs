﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects.Shells;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Звездная система
    /// </summary>
    public class StarSystem
    {
       /// <summary>
        /// Фон звездной системы
        /// </summary>
        ObjectView background;

        /// <summary>
        /// Главный центр масс
        /// </summary>
        LocalMassCenter massCenter;

        /// <summary>
        /// Управление позицией фона звездной системы
        /// </summary>
        public ObjectView Background
        {
            get { return this.background; }
        }

        /// <summary>
        /// Коллекция кораблей, находящихся в данной звездной системе
        /// </summary>
        private List<Ship> myShipsCollection;

        /// <summary>
        /// Коллекция снарядов, находящихся в данной звездной системе
        /// </summary>
        private List<Shell> myShellsCollection;

        /// <summary>
        /// Коллекция взрывов (визуальных эффектов)
        /// </summary>
        private List<VisualEffect> myExplosionsCollection; 

        /// <summary>
        /// Построить звездную систему
        /// </summary>
        /// <param name="massCenter"></param>
        /// <param name="background"></param>
        public StarSystem(LocalMassCenter massCenter, Texture background)
        {
            this.massCenter = massCenter;//Инициализация компонетов звездной системы
            InitBackgroung(background);//Построение фона звездной системы
            this.myShellsCollection = new List<Shell>();
            this.myExplosionsCollection = new List<VisualEffect>();
        }


        /// <summary>
        /// Построить фон звездной системы
        /// </summary>
        /// <param name="skin">Текстура фона</param>
        private void InitBackgroung(Texture skin)
        {
            this.background = new ObjectView(new RectangleShape(new Vector2f(2000, 2000)), BlendMode.Alpha);
            this.background.Image.Texture = skin;
            this.background.Image.Position = new Vector2f(-1000, -1000);
        }

        /// <summary>
        /// Изменение позиции фона Звездной системы
        /// </summary>
        /// <param name="offset">Смещение</param>
        public void OffsetBackground(Vector2f offset)
        {
            this.background.Translate(offset);
        }


        /// <summary>
        /// Процесс жизни звездной системы
        /// </summary>
        public void Process(List<Ship> shipsCollection)
        {
            this.myShipsCollection = new List<Ship>();//освобождение коллекции
            foreach (Ship currentShip in shipsCollection)//перезаполнение коллекции
            {
                if (currentShip.StarSystem == this)
                {
                    this.myShipsCollection.Add(currentShip);
                }    
            }
            massCenter.Process(new Vector2f(0, 0));//работа со звездной составляющей
            foreach (Ship ship in this.myShipsCollection)//работа кораблей находящихся в данной звездной системе
            {
                ship.AnalizeObjectInteraction();
                ship.Process(new Vector2f(0, 0));
                if (ship.Destroyed)
                {
                    this.myExplosionsCollection.Add(new VisualEffect(ship.Coords, new Vector2f(144, 144), 1500, ResurceStorage.shipExplosion));
                }
            }

            for (int i = 0; i < this.myShellsCollection.Count; i ++)//работа со снарядами в данной звездной системе
            {
                this.myShellsCollection[i].Process(new Vector2f(0, 0));
                if (this.myShellsCollection[i].LifeOver)//если время жизни снаряда вышло
                {
                    this.myShellsCollection.Remove(this.myShellsCollection[i]);//удалить его из коллекции
                    i --;
                }
            }
            for (int i = 0; i < this.myExplosionsCollection.Count; i ++)//работа с визуальными эффектами
            {
                this.myExplosionsCollection[i].Process();
                if (this.myExplosionsCollection[i].LifeOver)//если время жизни визуального эффекта вышло
                {
                    this.myExplosionsCollection.Remove(this.myExplosionsCollection[i]);//удалить его из коллекции
                    i --;
                }

            }
        }

        /// <summary>
        /// Вернуть коллекцию отображений объектов звездной системы
        /// </summary>
        /// <returns>Коллекция отображений объектов в звездной системе</returns>
        public List<ObjectView> GetView()
        {
            List<ObjectView> systemsViews = new List<ObjectView>();

            systemsViews.Add(this.background);//засунуть в возвращаемый массив фон
            systemsViews.AddRange(massCenter.GetView());//Заполнить массив образов системы образами объектов главного центра масс
            foreach (GameObject shell in this.myShellsCollection)//Заполнить массив образов системы образами снарядов, принадлежащих данной системе
            {
                systemsViews.AddRange(shell.View);
            }
            foreach (Ship ship in this.myShipsCollection)//Заполнить массив образов системы образами кораблей, принадлежащих данной системе
            {
                systemsViews.AddRange(ship.View);
            }
            foreach (VisualEffect explosion in this.myExplosionsCollection)
            {
                systemsViews.Add(explosion.View);
            }
            return systemsViews;
        }

        /// <summary>
        /// Получить коллекцию объектов находящихся в системе (временная реализация)
        /// </summary>
        /// <returns>Коллекция объектов в звездной системе</returns>
        public List<GameObject> GetObjectsInSystem()
        {
            List<GameObject> objectsCollection = this.massCenter.GetObjects();//получить коллекцию звезд и планет
            foreach (GameObject ship in this.myShipsCollection)//сформировать коллекцию кораблей
            {
                objectsCollection.Add(ship);
            }
            foreach (GameObject shell in this.myShellsCollection)//сформировать коллекциб снарядов
            {
                objectsCollection.Add(shell);
            }
            return objectsCollection;
        }

        /// <summary>
        /// Получить коллекцию объектов находящихся в сиcтеме, в области радиусом radius, около точки point
        /// </summary>
        /// <param name="point">Центр круговой области</param>
        /// <param name="radius">Радиус круговой области</param>
        /// <returns>Коллекция объектов звездной системы в указанной области</returns>
        public List<GameObject> GetObjectsInSystem(Vector2f point, double radius)
        {
            List<GameObject> ret_value = new List<GameObject>();
            foreach (GameObject candidat in this.GetObjectsInSystem())
            {
                //Получить расстояние до кандидата в возвращаемые объекты
                float distanse =
                    (float)Math.Sqrt(Math.Pow(candidat.Coords.X - point.X, 2) + Math.Pow(candidat.Coords.Y - point.Y, 2));
                if (distanse < radius)//если кандидат находится в указанной области
                {
                    ret_value.Add(candidat);//то добавить его в коллекцию возвращаемых объектов
                }
            }
            return ret_value;
        }

        /// <summary>
        /// Добавление снаряда в коллекцию снарядов в данной звездной системе
        /// </summary>
        public void AddNewShell(Shell newShell)
        {
            this.myShellsCollection.Add(newShell);
        }
 }
}
