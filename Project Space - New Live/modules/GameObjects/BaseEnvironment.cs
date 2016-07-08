﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Абстрактная игровая среда
    /// </summary>
    public class BaseEnvironment
    {
        /// <summary>
        /// Фон среды
        /// </summary>
        private ImageView background;

        /// <summary>
        /// Фон среды
        /// </summary>
        public ImageView Background
        {
            get { return this.background; }
        }

        /// <summary>
        /// Сопротивление среды перемещению объектов в ней
        /// </summary>
        private double movingResistance;

        /// <summary>
        /// Сопротивление среды перемещению объектов в ней
        /// </summary>
        public double MovingResistance
        {
            get { return this.movingResistance; }
            set { this.movingResistance = value; }
        }

        private CheckPoint[] checkPoints = new CheckPoint[2];

        /// <summary>
        /// Коллекция активных объектов
        /// </summary>
        private List<ActiveObject> activeObjectsCollection;

        /// <summary>
        /// Коллекция снарядов
        /// </summary>
        private List<Shell> shellsCollection;

        /// <summary>
        /// Коллекция визуальных эффектов
        /// </summary>
        private List<VisualEffect> effectsCollection;

        /// <summary>
        /// Конструктор активной среды
        /// </summary>
        /// <param name="background">Фон</param>
        /// <param name="moveResistance">Сопротивление перемещению объектов в среде</param>
        public BaseEnvironment(Texture background, float moveResistance, CheckPoint[] checkPoints)
        {
            this.MovingResistance = moveResistance;
            this.InitBackgroung(background); //Построение фона среды
            this.shellsCollection = new List<Shell>();
            this.effectsCollection = new List<VisualEffect>();
            this.checkPoints = checkPoints;
        }

        /// <summary>
        /// Инициализация фона
        /// </summary>
        /// <param name="skin"></param>
        private void InitBackgroung(Texture skin)
        {
            this.background = new ImageView(new RectangleShape(new Vector2f(16000, 14000)), BlendMode.Alpha);
            this.background.Image.Texture = skin;
            this.background.Image.Texture.Repeated = true;
            this.background.Image.Texture.Smooth = true;
            this.background.Image.TextureRect = new IntRect(0, 0, (int)(skin.Size.X), (int)(skin.Size.Y));
            this.background.Image.Position = new Vector2f(-8000, -7000);
        }

        /// <summary>
        /// Процесс жизни среды
        /// </summary>
        public void Process()
        {
            for(int i = 0; i < this.activeObjectsCollection.Count; i ++)//работа активных объекта находящихся в данной среде
            {
                if (!this.activeObjectsCollection[i].Destroyed)
                {
                    this.activeObjectsCollection[i].Process(new Vector2f(0, 0));
                    this.activeObjectsCollection[i].AnalizeObjectInteraction();
                    if (this.activeObjectsCollection[i].Destroyed) //если установлен флаг уничтожения активногог объекта
                    {
                        this.activeObjectsCollection[i].KillerActiveObject.AddWin();
                        this.effectsCollection.Add(this.activeObjectsCollection[i].ConstructDeathVisualEffect(new Vector2f(144, 144), 52));
                        //this.activeObjectsCollection.Remove(this.activeObjectsCollection[i]);//удалить его из коллекции
                    }
                }
            }
            for (int i = 0; i < this.shellsCollection.Count; i ++)//работа со снарядами в данной среде
            {
                this.shellsCollection[i].Process(new Vector2f(0, 0));
                if (this.shellsCollection[i].LifeOver)//если время жизни снаряда вышло
                {
                    this.effectsCollection.Add(this.shellsCollection[i].ConstructDeathVisualEffect(new Vector2f(32, 32), 19));
                    this.shellsCollection.Remove(this.shellsCollection[i]);//удалить его из коллекции
                    i --;
                }
            }
            for (int i = 0; i < this.effectsCollection.Count; i ++)//работа с визуальными эффектами
            {
                this.effectsCollection[i].Process();
                if (this.effectsCollection[i].LifeOver)//если время жизни визуального эффекта вышло
                {
                    this.effectsCollection.Remove(this.effectsCollection[i]);//удалить его из коллекции
                    i --;
                }
            }
            for (int i = 0; i < this.checkPoints.Length; i++)
            {
                this.checkPoints[i].Process(new Vector2f(0, 0));
            }
        }

        /// <summary>
        /// Установить массив активных объектов
        /// </summary>
        /// <param name="activeObjectsCollection">Новая коллекция активных объектов</param>
        public virtual void SetActiveObjectsCollection(List<ActiveObject> activeObjectsCollection)
        {
            this.activeObjectsCollection = activeObjectsCollection;
        }
    
        /// <summary>
        /// Добавление снаряда в коллекцию снарядов
        /// </summary>
        public void AddNewShell(Shell newShell)
        {
            this.shellsCollection.Add(newShell);
        }

        /// <summary>
        /// Получить коллекцию объектов находящихся в среде
        /// </summary>
        /// <returns>Коллекция объектов в среде</returns>
        public List<GameObject> GetObjectsInEnvironment()
        {
            List<GameObject> objectsCollection = new List<GameObject>();
            for (int i = 0; i < this.checkPoints.Length; i++)
            {
                objectsCollection.Add(checkPoints[i]);
            }
            foreach (GameObject activeObject in this.activeObjectsCollection) //сформировать коллекцию активных объектов
            {
                objectsCollection.Add(activeObject);
            }
            foreach (GameObject shell in this.shellsCollection) //сформировать коллекцию снарядов
            {
                objectsCollection.Add(shell);
            }
            return objectsCollection;//вернуть общую коллекцию
        }

        /// <summary>
        /// Получить коллекцию объектов находящихся в среде, в области радиусом radius, около точки point
        /// </summary>
        /// <param name="point">Центр круговой области</param>
        /// <param name="radius">Радиус круговой области</param>
        /// <returns>Коллекция объектов среды в указанной области</returns>
        public List<GameObject> GetObjectsInEnvironment(Vector2f point, double radius)
        {
            List<GameObject> ret_value = new List<GameObject>();
            foreach (GameObject candidat in this.GetObjectsInEnvironment())//получить все возвращаемые объекты
            {
                //Получить расстояние до кандидата в возвращаемые объекты
                float distanse = (float)(Math.Sqrt(Math.Pow(candidat.Coords.X - point.X, 2) + Math.Pow(candidat.Coords.Y - point.Y, 2)));
                if (distanse < radius) //если кандидат находится в указанной области
                {
                    ret_value.Add(candidat); //то добавить его в коллекцию возвращаемых объектов
                }
            }
            return ret_value;
        }

        /// <summary>
        /// Вернуть коллекцию отображений объектов среды
        /// </summary>
        /// <returns>Коллекция отображений объектов в среды</returns>
        public List<ImageView> GetView()
        {
            List<ImageView> environmentViews = new List<ImageView>();
            environmentViews.Add(this.background);
            for (int i = 0; i < this.checkPoints.Length; i++)
            {
                environmentViews.AddRange(checkPoints[i].View);
            }
            foreach (GameObject shell in this.shellsCollection)
            {
                environmentViews.AddRange(shell.View);
            }
            foreach (ActiveObject activeObject in this.activeObjectsCollection)
            {
                environmentViews.AddRange(activeObject.View);
            }
            foreach (VisualEffect effect in this.effectsCollection)
            {
                environmentViews.Add(effect.View);
            }
            return environmentViews;
        }

        /// <summary>
        /// Распределение контрольных точек между Игроками
        /// </summary>
        public void SetCheckPoints()
        {
            this.activeObjectsCollection[0].SetCheckPoints(this.checkPoints[0], this.checkPoints[1]);
            this.activeObjectsCollection[1].SetCheckPoints(this.checkPoints[1], this.checkPoints[0]);
        }

    }
}