using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Графическое отображение текста
    /// </summary>
    public class TextView : RenderView
    {
        /// <summary>
        /// Внутренне свойство
        /// </summary>
        public override Transformable View
        {
            get { return this.textString as Transformable; }
            set { this.textString = value as Text; }
        }

        /// <summary>
        /// Текстовая строка
        /// </summary>
        private Text textString = null;

        /// <summary>
        /// Текстовая строка
        /// </summary>
        public Text TextString
        {
            get { return this.textString; }
            set { this.textString = value; }
        }

        /// <summary>
        /// Создание текстового отображения
        /// </summary>
        /// <param name="text">Строка</param>
        /// <param name="mode">Режим отображения строки</param>
        /// <param name="font"Шрифи</param>
        public TextView(String text, BlendMode mode, Font font)
        {
            this.textString = new Text(text, font);
            this.State = new RenderStates(mode);
        }

        /// <summary>
        /// Создание текстового отображения
        /// </summary>
        /// <param name="text">Строка</param>
        /// <param name="textState">Состояние отображения строки</param>
        /// <param name="font">Шрифт</param>
        public TextView(String text, RenderStates textState, Font font)
        {
            this.textString = new Text(text, font);
            this.State = textState;
        }

    }
}
