using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Сигнатура игровых объектов
    /// </summary>
    public class ObjectSignature
    {

        /// <summary>
        /// Количество характеристик в наборе
        /// </summary>
        protected int CharacteristicsCount = 2;

        /// <summary>
        /// Размеры
        /// </summary>
        private Vector2f size;
        /// <summary>
        /// Размеры
        /// </summary>
        public Vector2f Size
        {
            get { return this.size; }
        }
        /// <summary>
        /// Масса
        /// </summary>
        private float mass;
        /// <summary>
        /// Масса
        /// </summary>
        public float Mass
        {
            get { return this.mass; }
        }

    }
}
