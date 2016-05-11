using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.Forms
{
    /// <summary>
    /// Фигурная форма
    /// </summary>
    public abstract class ImageForm : Form
    {
        /// <summary>
        /// Отображение
        /// </summary>
        protected ImageView view;

        /// <summary>
        /// Отображение
        /// </summary>
        protected override RenderView View
        {
            get { return this.view; }
        }

    }
}
