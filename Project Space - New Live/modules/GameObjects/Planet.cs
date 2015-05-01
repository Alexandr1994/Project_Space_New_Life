using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    abstract class Planet : GameObject
    {

        protected int radius;//радиус планеты
        protected int ordit;//орбита(расстояние от центра звезды до центра планеты)
        protected double orbitalAngle;//орбитальный угол планеты
        protected double orbitalSpeed;//орбитальная скорость планеты (рад./ед.вр.)

        /// <summary>
        /// Сконструировать отображение планеты
        /// </summary>
        /// <param name="skin">Текстура</param>
        /// <returns></returns>
        protected override void constructView(Texture skin)
        {
            CircleShape locView = new CircleShape((float)radius);
            locView.Position = coords;
            locView.Texture = skin;
            this.view = locView;
        }

        /// <summary>
        /// Жизнь планеты
        /// </summary>
        protected abstract void planetProcess(Vector2f globalCoords);

        /// <summary>
        /// Движение планеты по орбите
        /// </summary>
        public override void move(Vector2f globalCoords)
        {
            orbitalAngle += orbitalSpeed;//Изменение орбитального угла планеты
            this.coords.X = (float)((globalCoords.X - this.radius) + ordit * Math.Cos(orbitalAngle));//вычисление новой кординаты X
            this.coords.Y = (float)((globalCoords.Y - this.radius) + ordit * Math.Sin(orbitalAngle));//вычисление новой координаты У
        }

        /// <summary>
        /// Получить сингнатуру планеты
        /// </summary>
        /// <returns></returns>
        public abstract override object getSignature();


    }
}
