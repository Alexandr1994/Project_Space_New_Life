using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Орбитальная контрольная точка
    /// </summary>
    class OrbitalCheckPoint : CheckPoint
    {
        /// <summary>
        /// Последние координаты контрольной точки
        /// </summary>
        private Vector2f lastCoord = new Vector2f(0, 0);

        /// <summary>
        /// Центр вращения контрольной точки
        /// </summary>
        private Vector2f movingCenter;

        /// <summary>
        /// Орбита(расстояние от центра масс звездной системы до центра объекта)
        /// </summary>
        private int orbit;

        /// <summary>
        /// Орбитальный угол объекта в рад.
        /// </summary>
        private double orbitalAngle;

        /// <summary>
        /// орбитальная скорость объекта в рад./ед.вр.
        /// </summary>
        private double orbitalSpeed;

        /// <summary>
        /// Движение объекта по орбите
        /// </summary>
        protected override void Move()
        {
            orbitalAngle += orbitalSpeed;//Изменение орбитального угла планеты
            this.coords.X = (float)((orbit * Math.Cos(orbitalAngle)));//вычисление новой кординаты X
            this.coords.Y = (float)((orbit * Math.Sin(orbitalAngle)));//вычисление новой координаты У
            this.coords += this.movingCenter;
            Vector2f offset = this.coords - this.lastCoord;
            if (this.view != null)
            {
                this.view[0].Translate(offset);
                this.view[0].Rotate(this.coords, (float)(3 * Math.PI / 180));
            }
            this.lastCoord = this.coords;
        }

        /// <summary>
        /// Конструктор орбитальной контрольной точки
        /// </summary>
        /// <param name="startCoords">Центр вращения</param>
        /// <param name="orbit">Орибита</param>
        /// <param name="startOrbitalAngle">Начальный орбитальный поворот</param>
        /// <param name="orbitalSpeed">Орбитальная скорость</param>
        /// <param name="skin">Текстура</param>
        public OrbitalCheckPoint(Vector2f startCoords, int orbit, double startOrbitalAngle, double orbitalSpeed, Texture[] skin)
        {
            this.movingCenter = startCoords;
            this.orbit = orbit;
            this.orbitalAngle = startOrbitalAngle;
            this.orbitalSpeed = orbitalSpeed;
            this.Move();//сформировать координаты
            this.ConstructView(skin);//построить отображение
        }

        public override void Reset()
        {
            Random random = new Random();
            this.orbitalAngle = (float)(random.NextDouble() * 2 * Math.PI);
            this.Move();//сформировать координаты
            this.ConstructView(new Texture[] { this.view[0].Image.Texture });
        }
    }
}
