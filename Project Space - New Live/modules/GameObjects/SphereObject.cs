using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    public abstract class SphereObject : GameObject
    {

        protected int radius;//радиус объекта
        protected int orbit;//орбита(расстояние от центра масс звездной системы до центра объекта)
        protected double orbitalAngle;//орбитальный угол объекта
        protected double orbitalSpeed;//орбитальная скорость объекта (рад./ед.вр.)

        /// <summary>
        /// Сконструировать отображение объекта
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
        /// Движение объекта по орбите
        /// </summary>
        protected override void move(double speed)
        {
            orbitalAngle += speed;//Изменение орбитального угла планеты
            this.coords.X = (float)((orbit * Math.Cos(orbitalAngle)));//вычисление новой кординаты X
            this.coords.Y = (float)((orbit * Math.Sin(orbitalAngle)));//вычисление новой координаты У
        }

        /// <summary>
        /// Получить сингнатуру объекта
        /// </summary>
        /// <returns></returns>
        public override object getSignature()
        {
            Tuple<int, int> localSignature = new Tuple<int, int>(this.mass, this.radius);
            return localSignature;
        }


        /// <summary>
        /// Жизнь объекта
        /// </summary>
        /// <param name="home">Управляющая текцщим объектом сущность</param>
        public override void process(GameEntity home)
        {
            this.move(orbitalSpeed);//вычеслить идеальные координтаы
            this.correctObjectPoint(home.getCoords());//выполнить коррекцию относительно глобальных координт
            this.view.Position = new Vector2f(coords.X - this.radius, coords.Y - this.radius);//вычислить координаты отображения объекта
        }

    }
}
