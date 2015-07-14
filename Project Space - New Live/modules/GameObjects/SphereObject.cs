using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    public abstract class SphereObject : GameObject
    {

        /// <summary>
        /// радиус объекта
        /// </summary>
        protected int radius;
        /// <summary>
        /// орбита(расстояние от центра масс звездной системы до центра объекта)
        /// </summary>
        protected int orbit;
        /// <summary>
        /// орбитальный угол объекта
        /// </summary>
        protected double orbitalAngle;
        /// <summary>
        /// орбитальная скорость объекта (рад./ед.вр.)
        /// </summary>
        protected double orbitalSpeed;


        /// <summary>
        /// Движение объекта по орбите
        /// </summary>
        /// <param name="speed">Угловая скорость</param>
        /// <param name="angle">Текущий орбитальный угол</param>
        protected override void Move()
        {
            orbitalAngle += orbitalSpeed;//Изменение орбитального угла планеты
            this.coords.X = (float)((orbit * Math.Cos(orbitalAngle)));//вычисление новой кординаты X
            this.coords.Y = (float)((orbit * Math.Sin(orbitalAngle)));//вычисление новой координаты У
        }

        /// <summary>
        /// Функция перемещения объекта по орбите
        /// </summary>
        /// <param name="homeCoords">Координаты управляющей сущности</param>
        protected abstract void OrbitalMoving(Vector2f homeCoords);


    }
}
