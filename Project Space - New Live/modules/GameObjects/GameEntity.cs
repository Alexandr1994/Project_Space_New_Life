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
        /// <summary>
        /// координаты объекта
        /// </summary>
        protected Vector2f coords;
        
        /// <summary>
        /// Получить координаты объекта
        /// </summary>
        /// <returns></returns>
        public Vector2f GetCoords()
        {
            return this.coords;
        }

        /// <summary>
        /// Процесс жизни сущности
        /// </summary>
        /// <param name="home"></param>
        public abstract void Process(GameEntity home);

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
