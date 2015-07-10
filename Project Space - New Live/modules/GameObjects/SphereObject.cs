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
        /// Сконструировать отображение объекта
        /// </summary>
        /// <param name="skin">Текстура</param>
        /// <returns></returns>
        protected override void ConstructView(Texture[] skin)
        {
            this.view = new ObjectView[2];
            for (int i = 0; i < this.view.Length; i++)
            {
                this.view[i] = new ObjectView(new CircleShape((float)radius), BlendMode.Alpha);//создание нового ObjectView
                this.view[i].Image.Position = coords;//установка позиции отображегния ObjectView
                this.view[i].Image.Texture = skin[i];//установка текстуры отображения ObjectMode
            }

        }

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
        /// Получить сингнатуру объекта
        /// </summary>
        /// <returns></returns>
        public override object GetSignature()
        {
            Tuple<int, int> localSignature = new Tuple<int, int>(this.mass, this.radius);
            return localSignature;
        }

        /// <summary>
        /// Функция перемещения объекта по орбите
        /// </summary>
        /// <param name="homeCoords">Координаты управляющей сущности</param>
        protected void OrbitalMoving(Vector2f homeCoords)
        {
            this.Move();//вычеслить идеальные координтаы
            this.CorrectObjectPoint(homeCoords);//выполнить коррекцию относительно глобальных координт
            foreach (ObjectView locView in this.view)
            {
                locView.Image.Position = new Vector2f(coords.X - this.radius, coords.Y - this.radius);//вычислить координаты отображений объекта
            }
        }


    }
}
