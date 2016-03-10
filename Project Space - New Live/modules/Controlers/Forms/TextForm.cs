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
    public abstract class TextForm : Form
    {

        protected TextView view;

        protected override RenderView View
        {
            get{return this.view;}
        }

        /// <summary>
        /// Текстовая строка
        /// </summary>
        protected String text;

        /// <summary>
        /// Текстовая строка
        /// </summary>
        public String Text
        {
            get { return this.text; }
            set { this.text = value; }
        }


        private Font font = FontsStorage.DefaultFont;

        public override Vector2f Size
        {
            get { return this.size; }
            set { this.size = value; }
        }
    }
}
