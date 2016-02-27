using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Project_Space___New_Live
{
    /// <summary>
    /// Абстрактная игровая сущность
    /// </summary>
    public abstract class GameEntity
    {
        /// <summary>
        /// Координаты объекта
        /// </summary>
        protected Vector2f coords;
        /// <summary>
        /// Координаты объекта
        /// </summary>
        public Vector2f Coords
        {
            get { return this.coords; }
        }
        
        /// <summary>
        /// Процесс жизни сущности
        /// </summary>
        /// <param name="homeCoords">Координаты начала отсчета</param>
        public abstract void Process(Vector2f homeCoords);

        /// <summary>
        /// Скорректировать координаты сущности
        /// </summary>
        /// <param name="correction">Коррекция</param>
        public void CorrectObjectPoint(Vector2f correction)
        {
            this.coords.X += correction.X;
            this.coords.Y += correction.Y;
        }

        /// <summary>
        /// Функция движения сущности
        /// </summary>
        protected abstract void Move();

    }
}
