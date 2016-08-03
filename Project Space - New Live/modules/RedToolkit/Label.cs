using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;

namespace RedToolkit
{
    /// <summary>
    /// Надпись
    /// </summary>
    public class Label : TextRedWidget
    {

        /// <summary>
        /// Цвет текста
        /// </summary>
        public Color TextColor
        {
            get { return this.view.TextString.Color; }
            set { this.view.TextString.Color = value; }
        }

        /// <summary>
        /// Конструктор надписи
        /// </summary>
        protected override void CustomConstructor()
        {
            this.view = new TextView(this.text, BlendMode.Alpha, this.font);
            this.TextColor = Color.Black;
            this.ResaveTextString();
        }
    }
}
