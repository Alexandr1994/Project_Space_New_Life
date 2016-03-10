using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.Controlers.Forms
{
    public abstract class ImageForm : Form
    {


        protected ImageView view;

        protected override RenderView View
        {
            get { return this.view; }
        }

        /// <summary>
        /// Размер
        /// </summary>
        public override Vector2f Size
        {
            get { return this.size; }
            set
            {
                if (this.View != null)
                {
                    float Xcoef = value.X / this.size.X;
                    float Ycoef = value.Y / this.size.Y;
                    this.view.Image.Scale = new Vector2f(Xcoef, Ycoef);//Изменение размеров изображения
                    this.size = value;//сохранение размеров
                }
                else
                {
                    size = value;
                }
            }
        }

    }
}
