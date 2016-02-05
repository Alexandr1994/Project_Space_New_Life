using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.System;
using SFML.Graphics;


namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Абстрактный игровой объект
    /// </summary>
    public abstract class GameObject : GameEntity
    {

        /// <summary>
        /// Масса объекта
        /// </summary>
        protected float mass;

        /// <summary>
        /// Отображение объекта
        /// </summary>
        protected ObjectView[] view;

        /// <summary>
        /// Отображение объекта
        /// </summary>
        public virtual ObjectView[] View 
        {
            get { return this.view;}           
        }

        /// <summary>
        /// Получить сигнатуру(набор общих характеристик) объекта
        /// </summary>
        /// <returns>Сигнатура объекта</returns>
        public ObjectSignature GetSignature()
        {
            return ConstructSignature();//сконструировать сигнатуру объекта
        }

        /// <summary>
        /// Построение сигнатуры конкретного игрового объекта
        /// </summary>
        /// <returns>Сигнатура</returns>
        protected abstract ObjectSignature ConstructSignature();


        /// <summary>
        /// Построить отображение объекта
        /// </summary>
        /// <param name="skin">Массив текстур</param>
        protected abstract void ConstructView(Texture[] skin);

    }
}
