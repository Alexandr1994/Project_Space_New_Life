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
    public class StarSystem
    {

        /// <summary>
        /// Номер системы в коллекции
        /// </summary>
        private int systemIndex;

        /// <summary>
        /// Установить номер системы
        /// </summary>
        /// <param name="index"></param>
        public void SetIndex(int index)
        {
            this.systemIndex = index;
        }

        /// <summary>
        /// фон звездной системы
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
        /// Построить звездную систему
        /// </summary>
        /// <param name="massCenter"></param>
        /// <param name="background"></param>
        public StarSystem(LocalMassCenter massCenter, Texture background)
        {
            this.massCenter = massCenter;//Инициализация компонетов звездной системы
            InitBackgroung(background);//Построение фона звездной системы
        }


        /// <summary>
        /// Построить фон звездной системы
        /// </summary>
        /// <param name="skin"></param>
        private void InitBackgroung(Texture skin)
        {
            background = new ObjectView(new RectangleShape(new Vector2f(2000, 2000)), BlendMode.Alpha);
            background.Image.Texture = skin;
            background.Image.Position = new Vector2f(-1000, -1000);
        }

        /// <summary>
        /// Изменение позиции фона Звездной системы
        /// </summary>
        /// <param name="offset"></param>
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
                if (currentShip.StarSystemIndex == this.systemIndex)
                {
                    this.myShipsCollection.Add(currentShip);
                }    
            }
            massCenter.Process(new Vector2f(0, 0));//работа со звездной составляющей
            foreach (Ship ship in this.myShipsCollection)//работа кораблей находящихся в данной звездной системе
            {
                ship.AnalizeObjectInteraction(this);
                ship.Process(new Vector2f(0, 0));
            }
        }

        /// <summary>
        /// Вернуть коллекция отображений объектов звездной системы
        /// </summary>
        /// <returns></returns>
        public List<ObjectView> GetView()
        {
            List<ObjectView> systemsViews = new List<ObjectView>();

            systemsViews.Add(this.background);//засунуть в возвращаемый массив фон
            systemsViews.AddRange(massCenter.GetView());//Заполнить массив образов системы образами объектов главного центар масс
   
            return systemsViews;
        }

        /// <summary>
        /// Получить коллекцию объектов находящихся в системе (временная реализация)
        /// </summary>
        /// <returns></returns>
        public List<GameObject> GetObjectsInSystem()
        {
            List<GameObject> objectsCollection = this.massCenter.GetObjects();//получить коллекцию звезд и планет
            foreach (GameObject ship in myShipsCollection)//сформировать коллекцию кораблей
            {
                objectsCollection.Add(ship);
            }
            return objectsCollection;
        }

        /// <summary>
        /// Получить коллекцию объектов находящихся в сиcтеме в области радиусом radius около точки point (временная реализация)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
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

        
 }
}
