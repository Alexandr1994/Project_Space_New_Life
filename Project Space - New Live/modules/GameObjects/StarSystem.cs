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
    public class StarSystem
    {


        /// <summary>
        /// координаты объекта
        /// </summary>
        protected Vector2f coords;
        /// <summary>
        /// планетарная система
        /// </summary>
        Planet[] planetComponent;
        /// <summary>
        /// фон звездной системы
        /// </summary>
        RectangleShape background;
        /// <summary>
        /// центр масс
        /// </summary>
        LocalMassCenter massCenter;


        /// <summary>
        /// Построить звездную систему
        /// </summary>
        /// <param name="Coords"></param>
        /// <param name="massCenter"></param>
        /// <param name="planetComponent"></param>
        /// <param name="background"></param>
        public StarSystem(Vector2f Coords, LocalMassCenter massCenter ,Planet[] planetComponent, Texture background)
        {
            this.massCenter = massCenter;//Инициализация компонетов звездной системы
            this.planetComponent = planetComponent;
            this.coords = Coords;
            InitBackgroung(background);//Построение фона звездной системы
        }


        /// <summary>
        /// Построить фон звездной системы
        /// </summary>
        /// <param name="skin"></param>
        private void InitBackgroung(Texture skin)
        {
            background = new RectangleShape();
            background.Texture = skin;
            background.Size = new Vector2f(2000, 2000);
            background.Position = new Vector2f(-1000 + coords.X, -1000 + coords.Y);
        }

        /// <summary>
        /// Процесс жизни звездной системы
        /// </summary>
        public void Process()
        {
            massCenter.Process(this.GetCoords());//работа со звездной состовляющей
            if (planetComponent != null)
            {
                for (int i = 0; i < planetComponent.Length; i++)
                {//работа с планетарным компонентом
                    planetComponent[i].Process(this.GetCoords());//жизнь планеты
                }
            }
        }


        /// <summary>
        /// Вернуть коллекция отображений объектов звездной системы
        /// </summary>
        /// <returns></returns>
        public List<ObjectView> GetView()
        {
            List<ObjectView> systemsViews = new List<ObjectView>();
            List<ObjectView> starCompanent = massCenter.GetView();
            systemsViews.Add(new ObjectView(background, BlendMode.None));//засунуть в возвращаемый массив фон
            foreach (ObjectView view in starCompanent)//заполнить возвращаемый массив образами звезд
            {
                systemsViews.Add(view);
            }
            if (planetComponent != null)//если звездная система имеет планетарный компанент
            {
                foreach (Planet planet in planetComponent)//заполнить возвращаемый массив образами планет
                {
                    foreach (ObjectView view in planet.View)
                    {
                        systemsViews.Add(view);    
                    }
                }
            }
            return systemsViews;
        }

        /// <summary>
        /// Движение звездной системы, относительно игрока
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="angle"></param>
        public void Move(double speed, double angle)
        {
            this.coords.X -= (float)(speed * Math.Cos(angle));
            this.coords.Y -= (float)(speed * Math.Sin(angle));
            Vector2f newPos = new Vector2f();
            newPos.X = (float)(this.background.Position.X - (speed * Math.Cos(angle) / 100));//вычисление новой координаты фона X
            newPos.Y = (float)(this.background.Position.Y - (speed * Math.Sin(angle) / 100));//вычисление новой координаты фона Y
            background.Position = newPos;
        }

        /// <summary>
        /// Получить координаты объекта
        /// </summary>
        /// <returns></returns>
        public Vector2f GetCoords()
        {
            return this.coords;
        }

        /// <summary>
        /// Скорректировать координаты объекта отностиельно
        /// </summary>
        /// <param name="correction">Коррекция</param>
        public void CorrectObjectPoint(Vector2f correction)
        {
            coords.X += correction.X;
            coords.Y += correction.Y;
        }

    }
}
