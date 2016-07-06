using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Project_Space___New_Live.modules.Storages
{
    /// <summary>
    /// Хранилище пределов характеристик сигнатуры объектов
    /// </summary>
    class CharacterLimmits
    {
        /// <summary>
        /// Максимальный показатель размера
        /// </summary>
        private static float maxSize = 300;

        /// <summary>
        /// Максимальный показатель массы
        /// </summary>
        private static float maxMass = 3000;

        /// <summary>
        /// Максималбное количество снарядов в среде 
        /// </summary>
        private static int maxShellsCount = 30;

        /// <summary>
        /// Нормирование массы
        /// </summary>
        /// <param name="mass">Масса</param>
        /// <returns>Нормированная масса</returns>
        public static float NormMass(float mass)
        {
            if (mass > maxMass)
            {
                return 1;
            }
            return mass / maxMass;
        }

        /// <summary>
        /// Нормирование размера
        /// </summary>
        /// <param name="size">Размеры</param>
        /// <returns>Нормированные размеры</returns>
        public static Vector2f NormSize(Vector2f size)
        {
            Vector2f normedSize = new Vector2f();
            if (size.X > maxSize)
            {
                normedSize.X = 1;
            }
            else
            {
                normedSize.X = size.X / maxSize;
            }
            if (size.Y > maxSize)
            {
                normedSize.Y = 1;
            }
            else
            {
                normedSize.Y = size.Y / maxSize;
            }
            return normedSize;
        }

        /// <summary>
        /// Нормирование количества снарядов
        /// </summary>
        /// <param name="size">Количество снарядов</param>
        /// <returns>Нормированное количество снарядов</returns>
        public static float NormShellsCollection(int shellsCount)
        {
            if (shellsCount > maxShellsCount)
            {
                return 1;
            }
            return shellsCount / maxShellsCount;
        }

    }
}
