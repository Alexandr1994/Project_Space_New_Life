using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Space___New_Live.modules
{
    /// <summary>
    /// Вектор скорости
    /// </summary>
    public class SpeedVector
    {
        /// <summary>
        /// Скорость
        /// </summary>
        private float speed;

        /// <summary>
        /// Скорость
        /// </summary>
        public float Speed
        {
            get { return this.speed; }
        }

        /// <summary>
        /// Угол поворота вектора (в рад)
        /// </summary>
        private float angle;

        /// <summary>
        /// Угол поворота вектора (в рад)
        /// </summary>
        public float Angle
        {
            get { return this.angle; }
        }

        public SpeedVector(float speed, float angle)
        {
            this.speed = speed;
            this.angle = angle;
        }

        /// <summary>
        /// Ускорение
        /// </summary>
        /// <param name="acceleration">Изменение скорости</param>
        public void SpeedAcceleration(float acceleration)
        {
            this.speed += acceleration;
        }

        /// <summary>
        /// Вращение скоростного вектора
        /// </summary>
        /// <param name="angle">Угол на который производится поворот</param>
        public void VectorRotation(float angle)
        {
            this.angle += angle;
        }

    }
}
