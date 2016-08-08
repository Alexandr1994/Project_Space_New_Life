using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live.modules
{
    /// <summary>
    /// Графическое отображение фигуры
    /// </summary>
    public class ImageView : RenderView
    {

        /// <summary>
        /// Внутренне свойство
        /// </summary>
        public override Transformable View 
        {
            get { return this.image as Transformable; }
            set {this.image = value as Shape;}
        }

        /// Отображение фигуры
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
        /// Создание пустого отображения
        /// </summary>
        public ImageView()
        {

        }

        /// <summary>
        /// Создание отображения
        /// </summary>
        /// <param name="mode">Режим отрисовки</param>
        public ImageView(BlendMode mode)
        {
            this.State = new RenderStates(mode);
        }

        /// <summary>
        /// Создание отображения
        /// </summary>
        /// <param name="image">Отображение</param>
        /// <param name="mode">Режим отрисовки</param>
        public ImageView(Shape image, BlendMode mode)
        {
            this.Image = image;
            this.State = new RenderStates(mode);
        }

        /// <summary>
        /// Создание отображения
        /// </summary>
        /// <param name="image">Отображение</param>
        /// <param name="imageState">Состояние отображения</param>
        public ImageView(Shape image, RenderStates imageState)
        {
            this.Image = image;
            this.State = imageState;
        }

    }
}
