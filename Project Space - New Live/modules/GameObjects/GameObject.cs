using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace Project_Space___New_Live
{
    abstract class GameObject
    {
        private int mass;//масса объекта
        private Vector2i coords;//координаты объекта
        Shape view;//отображение объекта


        /// <summary>
        /// Получить координаты объекта
        /// </summary>
        /// <returns></returns>
        public Vector2i getCoords()
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
        /// <param name="skin"></param>
        /// <returns></returns>
        protected abstract Shape coonstructView(Texture skin);
    }
}
