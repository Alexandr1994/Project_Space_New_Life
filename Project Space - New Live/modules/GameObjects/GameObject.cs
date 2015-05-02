using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace Project_Space___New_Live
{
    public abstract class GameObject
    {
        protected int mass;//масса объекта
        protected Vector2f coords;//координаты объекта
        protected Shape view;//отображение объекта


        /// <summary>
        /// Получить координаты объекта
        /// </summary>
        /// <returns></returns>
        public Vector2f getCoords()
        {
            return this.coords;
        }

        /// <summary>
        /// Получить сигнатуру(набор общих характеристик) обекта
        /// </summary>
        /// <returns></returns>
        public abstract object getSignature();
        
        /// <summary>
        /// Функция движения объекта
        /// </summary>
        public abstract void move();


        /// <summary>
        /// Построить отображение объекта (Х)
        /// </summary>
        /// <param name="skin">Текстура</param>
        /// <returns></returns>
        protected abstract void constructView(Texture skin);


        /// <summary>
        /// Скорректировать координаты объекта отностиельно
        /// </summary>
        /// <param name="correction">Коррекция</param>
        public void correctObjectPoint(Vector2f correction)
        {
            coords.X += correction.X;
            coords.Y += correction.Y;
        }

        /// <summary>
        /// Получить отображение объекта
        /// </summary>
        /// <returns></returns>
        public Shape getView()
        {
            return view;
        }
    }
}
