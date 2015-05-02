using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    public class StarSystem
    {



        Star[] starComponent;//звездная составляющая 
        Planet[] planetComponent;//планетарная система
        
        Vector2f globalCoords;//глобальнек координаты звездной системы, относительно игрока
        RectangleShape background;//фон звездной системы

        /// <summary>
        /// Построить звездную систему
        /// </summary>
        /// <param name="Coords"></param>
        /// <param name="starComponent"></param>
        /// <param name="planetComponent"></param>
        /// <param name="background"></param>
        public StarSystem(Vector2f Coords,Star[] starComponent ,Planet[] planetComponent, Texture background)
        {
            this.starComponent = starComponent;//Инициализация компонетов звездной системы
            this.planetComponent = planetComponent;
            this.globalCoords = Coords;
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
            background.Position = new Vector2f(-1000 + globalCoords.X, -1000 + globalCoords.Y);
        }

        /// <summary>
        /// Процесс жизни звездной системы  (БЫДЛОКОД К ИСПРАВЛЕНЮ)
        /// </summary>
        public void systemProcess(RenderTarget Target)
        {
            for (int i = 0; i < starComponent.Length; i++)
            {//работа со звездным компонентом
                starComponent[i].process(this);//движение звезд звездной системы
            }
            for (int i = 0; i < planetComponent.Length; i++)
            {//работа с планетарным компонентом
                planetComponent[i].process(this);//жизнь планеты
            }
            renderSystem(Target);//отрисовка
        }

        /// <summary>
        /// Получить клобадьные координаты
        /// </summary>
        /// <returns></returns>
        public Vector2f getGlobalCoords()
        {
            return this.globalCoords;
        }

        /// <summary>
        /// Отрисовать звездную систему (БЫДЛОКОД К ИНКАПСУЛЯЦИИ В СПЕЦИАЛЬНЫЙ КЛАСС)
        /// </summary>
        /// <param name="Target"></param>
        private void renderSystem(RenderTarget Target)
        {
            Target.Draw(background);//отрисовать фон
            for (int i = 0; i < starComponent.Length; i++)
            {//отрисовать звезды
                Target.Draw(starComponent[i].getView());//движение звезд звездной системы
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
        public void move(float deltaX, float deltaY)
        {
            this.globalCoords.X -= deltaX;
            this.globalCoords.Y -= deltaY;
            background.Position = new Vector2f(-1000 + globalCoords.X, -1000 + globalCoords.Y);
        }


    }
}
