using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Графическое отображение
    /// </summary>
    public class ObjectView
    {
        /// <summary>
        /// Отображение
        /// </summary>
        private Shape image = null;

        /// <summary>
        /// Свойство отображения
        /// </summary>
        public Shape Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

        /// <summary>
        /// Состояние отображения
        /// </summary>
        private RenderStates state;
        /// <summary>
        /// Свойство состояния отображения
        /// </summary>
        public RenderStates State
        {
            get { return this.state;}
            set { this.state = value; }
        }

        /// <summary>
        /// Создание пустого отображения
        /// </summary>
        public ObjectView()
        {

        }

        /// <summary>
        /// Создание отображения
        /// </summary>
        /// <param name="mode">Режим отрисовки</param>
        public ObjectView(BlendMode mode)
        {
            this.State = new RenderStates(mode);
        }

        /// <summary>
        /// Создание отображения
        /// </summary>
        /// <param name="image">Отображение</param>
        /// <param name="mode">Режим отрисовки</param>
        public ObjectView(Shape image, BlendMode mode)
        {
            this.Image = image;
            this.State = new RenderStates(mode);
        }

        /// <summary>
        /// Создание  отображения
        /// </summary>
        /// <param name="image">Отображение</param>
        /// <param name="imageState">Состояние отображения</param>
        public ObjectView(Shape image, RenderStates imageState)
        {
            this.Image = image;
            this.State = imageState;
        }


    }
}
