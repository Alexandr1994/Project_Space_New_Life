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
        /// Движение звездной системы (НОРМАЛЬНОЕ - СКОРО) 
        /// </summary>
        /// <param name="speed"></param>
        protected override void move(double speed)
        {
            throw new NotImplementedException();
        } 

        /// <summary>
        /// Движение звездной системы (БЫДЛОКОД К ИСПРАВЛЕНЮ - ОТЛАДОЧНАЯ ФУНКЦИЯ)
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public void move(float deltaX, float deltaY)
        {
            this.coords.X -= deltaX;
            this.coords.Y -= deltaY;
            background.Position = new Vector2f(-1000 + coords.X, -1000 + coords.Y);
        }


    }
}
