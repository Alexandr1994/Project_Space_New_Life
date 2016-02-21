using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Звездная система
    /// </summary>
    public class StarSystem : BaseEnvironment
    {

        /// <summary>
        /// Главный центр масс
        /// </summary>
        private LocalMassCenter massCenter;

        /// <summary>
        /// Построить звездную систему
        /// </summary>
        /// <param name="massCenter"></param>
        /// <param name="background"></param>
        public StarSystem(LocalMassCenter massCenter, Texture background)
        {
            this.MovingResistance = 0.001;
            this.massCenter = massCenter; //Инициализация компонетов звездной системы
            InitBackgroung(background); //Построение фона звездной системы
            this.myShellsCollection = new List<Shell>();
            this.myEffectsCollection = new List<VisualEffect>();
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
        protected override void CustomProcess()
        {
            massCenter.Process(new Vector2f(0, 0)); //работа со звездной составляющей
        }

        /// <summary>
        /// Вернуть коллекцию отображений объектов звездной системы
        /// </summary>
        /// <returns>Коллекция отображений объектов в звездной системе</returns>
        public List<ObjectView> GetView()
        {
            List<ObjectView> systemsViews = new List<ObjectView>();

            systemsViews.Add(this.background); //засунуть в возвращаемый массив фон
            systemsViews.AddRange(massCenter.GetView());
                //Заполнить массив образов системы образами объектов главного центра масс
            foreach (GameObject shell in this.myShellsCollection)
                //Заполнить массив образов системы образами снарядов, принадлежащих данной системе
            {
                systemsViews.AddRange(shell.View);
            }
            foreach (Ship ship in this.myActiveObjectsCollection)
                //Заполнить массив образов системы образами кораблей, принадлежащих данной системе
            {
                systemsViews.AddRange(ship.View);
            }
            foreach (VisualEffect explosion in this.myEffectsCollection)
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
            List<GameObject> objectsCollection = this.massCenter.GetObjects(); //получить коллекцию звезд и планет
            foreach (GameObject ship in this.myActiveObjectsCollection) //сформировать коллекцию кораблей
            {
                objectsCollection.Add(ship);
            }
            foreach (GameObject shell in this.myShellsCollection) //сформировать коллекциб снарядов
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
                    (float)
                        Math.Sqrt(Math.Pow(candidat.Coords.X - point.X, 2) + Math.Pow(candidat.Coords.Y - point.Y, 2));
                if (distanse < radius) //если кандидат находится в указанной области
                {
                    ret_value.Add(candidat); //то добавить его в коллекцию возвращаемых объектов
                }
            }
            return ret_value;
        }

        /// <summary>
        /// Установить или обновить коллекцию кораблей в звездной системе
        /// </summary>
        /// <param name="activeObjectsCollection">Новая коллекция кораблей</param>
        public override void RefreshActiveObjectsCollection(List<ActiveObject> activeObjectsCollection)
        {
            this.myActiveObjectsCollection = new List<ActiveObject>(); //освобождение коллекции
            foreach (Ship currentShip in activeObjectsCollection) //перезаполнение коллекции
            {
                if (currentShip.Environment == this)
                {
                    this.myActiveObjectsCollection.Add(currentShip);

                }
            }
        }

    }
}
