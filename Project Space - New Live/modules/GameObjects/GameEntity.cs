using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Project_Space___New_Live
{
    public abstract class GameEntity
    {

        protected Vector2f coords;//координаты объекта
        
        /// <summary>
        /// Получить координаты объекта
        /// </summary>
        /// <returns></returns>
        public Vector2f getCoords()
        {
            return this.coords;
        }
   
        /// <summary>
        /// Функция движения объекта
        /// </summary>
        /// <param name="speed">Скорость движениея</param>
        protected abstract void move(double speed);

        /// <summary>
        /// Скорректировать координаты объекта отностиельно
        /// </summary>
        /// <param name="correction">Коррекция</param>
        public void correctObjectPoint(Vector2f correction)
        {
            coords.X += correction.X;
            coords.Y += correction.Y;
        }

        

    }
}
