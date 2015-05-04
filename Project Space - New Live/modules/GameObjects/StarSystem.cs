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
        /// Процесс жизни звездной системы  (БЫДЛОКОД К ИСПРАВЛЕНЮ)
        /// </summary>
        public void process(RenderTarget Target)
        {
            //for (int i = 0; i < starComponent.Length; i++)
            //{//работа со звездным компонентом
            //    starComponent[i].process(this);//движение звезд звездной системы
            //}
            massCenter.process(this);//работа со звездной состовляющей
            for (int i = 0; i < planetComponent.Length; i++)
            {//работа с планетарным компонентом
                planetComponent[i].process(this);//жизнь планеты
            }
            renderSystem(Target);//отрисовка
        }

        /// <summary>
        /// Отрисовать звездную систему (БЫДЛОКОД К ИНКАПСУЛЯЦИИ В СПЕЦИАЛЬНЫЙ КЛАСС)
        /// </summary>
        /// <param name="Target"></param>
        private void renderSystem(RenderTarget Target)
        {
            Target.Draw(background);//отрисовать фон
            List<Shape> starComponent = massCenter.getStarsViews();
            foreach (Shape view in starComponent)
            {
                Target.Draw(view);
            }
            for (int i = 0; i < planetComponent.Length; i++)
            {//отрисовать планеты
                Target.Draw(planetComponent[i].getView());//движение звезд звездной системы
            }


        }

        /// <summary>
        /// Движение звездной системы (БЫДЛОКОД К ИСПРАВЛЕНЮ)
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        protected override void move(double speed)
        {
           // this.coords.X -= deltaX;
          //  this.coords.Y -= deltaY;
            background.Position = new Vector2f(-1000 + coords.X, -1000 + coords.Y);
        }


    }
}
