using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;


namespace Project_Space___New_Live.modules.GameObjects
{
    public abstract class GameObject : GameEntity
    {
        /// <summary>
        /// масса объекта
        /// </summary>
        protected int mass;
        /// <summary>
        /// отображение объекта
        /// </summary>
        protected Shape view;

        /// <summary>
        /// Получить сигнатуру(набор общих характеристик) обекта
        /// </summary>
        /// <returns></returns>
        public abstract object GetSignature();

        /// <summary>
        /// Построить отображение объекта (Х)
        /// </summary>
        /// <param name="skin">Текстура</param>
        /// <returns></returns>
        protected abstract void ConstructView(Texture skin);

        /// <summary>
        /// Получить отображение объекта
        /// </summary>
        /// <returns></returns>
        public Shape GetView()
        {
            return view;
        }
    }
}
