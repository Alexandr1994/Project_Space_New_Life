using System;
using System.Collections;
using System.Collections.Generic;
using Project_Space___New_Live.modules.Storages;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Сигнатура игровых объектов
    /// </summary>
    public class ObjectSignature
    {

        /// <summary>
        /// Координаты
        /// </summary>
        private Vector2f coords;

        /// <summary>
        /// Масса
        /// </summary>
        private float mass;

        /// <summary>
        /// Размеры
        /// </summary>
        private Vector2f size;

        /// <summary>
        /// Скорость
        /// </summary>
        private float speed;

        /// <summary>
        /// Направление
        /// </summary>
        private float directon;

        /// <summary>
        /// Координаты
        /// </summary>
        public Vector2f Coords
        {
            get { return coords; }
            set { coords = value; }
        }

        /// <summary>
        /// Масса
        /// </summary>
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        /// <summary>
        /// Размеры
        /// </summary>
        public Vector2f Size
        {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
        /// Скорость
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        /// <summary>
        /// Направление
        /// </summary>
        public float Directon
        {
            get { return directon; }
            set { directon = value; }
        }
    }
}
