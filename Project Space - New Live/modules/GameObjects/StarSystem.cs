using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    public class StarSystem : GameEntity
    {


        //Star[] starComponent;//звездная составляющая 
        Planet[] planetComponent;//планетарная система
        RectangleShape background;//фон звездной системы
        LocalMassCenter massCenter;//центр масс


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
            initBackgroung(background);//Построение фона звездной системы
        }


        /// <summary>
        /// Построить фон звездной системы
        /// </summary>
        /// <param name="skin"></param>
        private void initBackgroung(Texture skin)
        {
            background = new RectangleShape();
            background.Texture = skin;
            background.Size = new Vector2f(2000, 2000);
            background.Position = new Vector2f(-1000 + coords.X, -1000 + coords.Y);
        }

        /// <summary>
        /// Процесс жизни звездной системы
        /// </summary>
        public override void process(GameEntity home = null)
        {
            massCenter.process(this);//работа со звездной состовляющей
            if (planetComponent != null)
            {
                for (int i = 0; i < planetComponent.Length; i++)
                {//работа с планетарным компонентом
                    planetComponent[i].process(this);//жизнь планеты
                }
            }
        }


        /// <summary>
        /// Вернуть коллекция отображений объектов звездной системы
        /// </summary>
        /// <returns></returns>
        public List<Shape> getView()
        {
            List<Shape> systemsViews = new List<Shape>();
            List<Shape> starCompanent = massCenter.getView();
            systemsViews.Add(background);//засунуть в возвращаемый массив фон
            foreach (Shape view in starCompanent)//заполнить возвращаемый массив образами звезд
            {
                systemsViews.Add(view);
            }
            if (planetComponent != null)//если звездная система имеет планетарный компанент
            {
                foreach (Planet planet in planetComponent)//заполнить возвращаемый массив образами планет
                {
                    systemsViews.Add(planet.getView());
                }
            }
            return systemsViews;
        }

        /// <summary>
        /// Движение звездной системы, относительно игрока
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="angle"></param>
        public void move(double speed, double angle)
        {
            this.coords.X -= (float)(speed * Math.Cos(angle));
            this.coords.Y -= (float)(speed * Math.Sin(angle));
            Vector2f newPos = new Vector2f();
            newPos.X = (float)(this.background.Position.X - (speed * Math.Cos(angle) / 100));//вычисление новой координаты фона X
            newPos.Y = (float)(this.background.Position.Y - (speed * Math.Sin(angle) / 100));//вычисление новой координаты фона Y
            background.Position = newPos;
        }


    }
}
