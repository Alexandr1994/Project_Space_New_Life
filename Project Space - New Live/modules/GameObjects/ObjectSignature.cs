using System;
using System.Collections;
using System.Collections.Generic;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Сигнатура игровых объектов
    /// </summary>
    public class ObjectSignature
    {
        /// <summary>
        /// Индексы характеристик
        /// </summary>
        public enum CharactsKeys : int
        {
            /// <summary>
            /// Масса (double)
            /// </summary>
            Mass = 0,
            /// <summary>
            /// Размер (Vector2f)
            /// </summary>
            Size
        }



        /// <summary>
        /// Количество характеристик в наборе
        /// </summary>
        protected int characteristicsCount;

        /// <summary>
        /// Коллекция харатеристик в сигнатуре
        /// </summary>
        private List<object> characteristics = new List<object>();
        /// <summary>
        /// Коллекция харатеристик в сигнатуре
        /// </summary>
        public List<object> Characteristics
        {
            get
            {
                return this.characteristics;
            }
        }


        /// <summary>
        /// Конструктор сигнатуры
        /// </summary>
        public ObjectSignature()
        {
            this.characteristicsCount = 0;
        }

        /// <summary>
        /// Добавить характеристику
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public void AddCharacteristics(object value, Type type)
        {
            this.characteristics.Add(value);
            this.characteristicsCount++;
        }


    }
}
