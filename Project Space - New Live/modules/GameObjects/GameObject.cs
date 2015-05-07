using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;


namespace Project_Space___New_Live.modules.GameObjects
{
    public abstract class GameObject : GameEntity
    {

        protected int mass;//масса объекта
        protected Shape view;//отображение объекта

        /// <summary>
        /// Функция движения объекта
        /// </summary>
        /// <param name="speed">Скорость движениея</param>
        /// <param name="angle">Угол поворота или орбитальный угол</param>
        protected abstract void move();

        /// <summary>
        /// Получить сигнатуру(набор общих характеристик) обекта
        /// </summary>
        /// <returns></returns>
        public abstract object getSignature();

        /// <summary>
        /// Построить отображение объекта (Х)
        /// </summary>
        /// <param name="skin">Текстура</param>
        /// <returns></returns>
        protected abstract void constructView(Texture skin);

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
