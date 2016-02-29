using System;
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

        /// <summary>
        /// Фон среды
        /// </summary>
        protected ObjectView background;

        /// <summary>
        /// Фон среды
        /// </summary>
        public ObjectView Background
        {
            get { return this.background; }
        }

        /// <summary>
        /// Коллекция активных объектов, находящихся в данной звездной системе
        /// </summary>
        protected List<ActiveObject> myActiveObjectsCollection;

        /// <summary>
        /// Коллекция снарядов, находящихся в данной звездной системе
        /// </summary>
        protected List<Shell> myShellsCollection;

        /// <summary>
        /// Коллекция визуальных эффектов
        /// </summary>
        protected List<VisualEffect> myEffectsCollection;


                /// <summary>
        /// Построить звездную систему
        /// </summary>
        /// <param name="massCenter">Готовый центр масс</param>
        /// <param name="background">Текстура фона</param>
        public BaseEnvironment(Texture background, float moveResistance)
        {
            this.MovingResistance = moveResistance;
            this.InitBackgroung(background); //Построение фона звездной системы
            this.myShellsCollection = new List<Shell>();
            this.myEffectsCollection = new List<VisualEffect>();
        }

        /// <summary>
        /// Процесс жизни игровой среды
        /// </summary>
        public void Process()
        {
            this.CustomProcess();
            for(int i = 0; i < this.myActiveObjectsCollection.Count; i ++)//работа активных объектоа находящихся в данной звездной системе
            {
                this.myActiveObjectsCollection[i].Process(new Vector2f(0, 0));
                this.myActiveObjectsCollection[i].AnalizeObjectInteraction();
                if (this.myActiveObjectsCollection[i].Destroyed)//если установлен флаг уничтожения активногог объекта
                {
                    this.myEffectsCollection.Add(this.myActiveObjectsCollection[i].ConstructDeathVisualEffect(new Vector2f(144, 144), 52));
                    this.myActiveObjectsCollection.Remove(this.myActiveObjectsCollection[i]);//удалить его из коллекции
                }
            }
            for (int i = 0; i < this.myShellsCollection.Count; i ++)//работа со снарядами в данной звездной системе
            {
                this.myShellsCollection[i].Process(new Vector2f(0, 0));
                if (this.myShellsCollection[i].LifeOver)//если время жизни снаряда вышло
                {
                    this.myEffectsCollection.Add(this.myShellsCollection[i].ConstructDeathVisualEffect(new Vector2f(30, 30), 19));
                    this.myShellsCollection.Remove(this.myShellsCollection[i]);//удалить его из коллекции
                    i --;
                }
            }
            for (int i = 0; i < this.myEffectsCollection.Count; i ++)//работа с визуальными эффектами
            {
                this.myEffectsCollection[i].Process();
                if (this.myEffectsCollection[i].LifeOver)//если время жизни визуального эффекта вышло
                {
                    this.myEffectsCollection.Remove(this.myEffectsCollection[i]);//удалить его из коллекции
                    i --;
                }
            }
        }

        /// <summary>
        /// Процесс жизни конкретной игровой среды
        /// </summary>
        protected void CustomProcess()
        {
            ;
        }

        /// <summary>
        /// Установить или обновить коллекцию активных объектов в середе
        /// </summary>
        /// <param name="activeObjectsCollection">Новая коллекция активных объектов</param>
        public virtual void RefreshActiveObjectsCollection(List<ActiveObject> activeObjectsCollection)
        {
            this.myActiveObjectsCollection = activeObjectsCollection;
        }
    
        /// <summary>
        /// Добавление снаряда в коллекцию снарядов в данной звездной системе
        /// </summary>
        public void AddNewShell(Shell newShell)
        {
            this.myShellsCollection.Add(newShell);
        }

        /// <summary>
        /// Получить уникальные объекты конкретной среды
        /// </summary>
        /// <returns>Коллекция уникальных объектов среды</returns>
        protected List<GameObject> GetCustomEnvironmentObjects()
        {
            return new List<GameObject>();
        }

        /// <summary>
        /// Получить коллекцию объектов находящихся в среде
        /// </summary>
        /// <returns>Коллекция объектов в звездной системе</returns>
        public List<GameObject> GetObjectsInEnvironment()
        {
            List<GameObject> objectsCollection = this.GetCustomEnvironmentObjects();//получить коллекцию уникальных объектов
            foreach (GameObject activeObject in this.myActiveObjectsCollection) //сформировать коллекцию активных объектов
            {
                objectsCollection.Add(activeObject);
            }
            foreach (GameObject shell in this.myShellsCollection) //сформировать коллекцию снарядов
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
        /// <returns>Коллекция объектов звездной системы в указанной области</returns>
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

        protected void InitBackgroung(Texture skin)
        {
            this.background = new ObjectView(new RectangleShape(new Vector2f(2000, 2000)), BlendMode.Alpha);
            this.background.Image.Texture = skin;
            this.background.Image.Position = new Vector2f(-500, -1000);
        }

        /// <summary>
        /// Изменение позиции фона Звездной системы
        /// </summary>
        /// <param name="offset">Смещение</param>
        public void OffsetBackground(Vector2f currentCoords, Vector2f lastCoords)
        {
            Vector2f offset = lastCoords - currentCoords;
            this.background.Translate(new Vector2f((float)(offset.X * (-0.9)), (float)(offset.Y * (-0.9))));
        }

        /// <summary>
        /// Получить отображения уникальных объектов среды
        /// </summary>
        /// <returns></returns>
        protected List<ObjectView> GetCustomViews()
        {
            return new List<ObjectView>();
        }

        /// <summary>
        /// Вернуть коллекцию отображений объектов звездной системы
        /// </summary>
        /// <returns>Коллекция отображений объектов в звездной системе</returns>
        public List<ObjectView> GetView()
        {
            List<ObjectView> environmentViews = new List<ObjectView>();
            environmentViews.Add(this.background);
            environmentViews.AddRange(this.GetCustomViews());
            foreach (GameObject shell in this.myShellsCollection)
            {
                environmentViews.AddRange(shell.View);
            }
            foreach (ActiveObject activeObject in this.myActiveObjectsCollection)
            {
                environmentViews.AddRange(activeObject.View);
            }
            foreach (VisualEffect effect in this.myEffectsCollection)
            {
                environmentViews.Add(effect.View);
            }
            return environmentViews;
        }

    }
}
