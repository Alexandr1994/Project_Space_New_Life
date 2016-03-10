using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Хранищище шрифтов
    /// </summary>
    public static class FontsStorage
    {

        /// <summary>
        /// Шрифт Times New Roman
        /// </summary>
        private static Font timesNewRoman = new Font("Resources/Fonts/Times New Roman.ttf");

        /// <summary>
        /// Шрифт Times New Roman
        /// </summary>
        static public Font TimesNewRoman
        {
            get { return timesNewRoman; }
        }

        /// <summary>
        /// Шрифт Arial
        /// </summary>
        private static Font arial = new Font("Resources/Fonts/Arial.ttf");

        /// <summary>
        /// Шрифт Arial
        /// </summary>
        static public Font Arial
        {
            get { return arial; }
        }

        /// <summary>
        /// Шрифт Calibri
        /// </summary>
        private static Font calibri = new Font("Resources/Fonts/Calibri.ttf");

        /// <summary>
        /// Шрифт Calibri
        /// </summary>
        static public Font Calibri
        {
            get { return calibri; }
        }

        /// <summary>
        /// Шрифт Comic Sans
        /// </summary>
        private static Font comicSans = new Font("Resources/Fonts/Comic Sans.ttf");

        /// <summary>
        /// Шрифт Comic Sans
        /// </summary>
        public static Font ComicSans
        {
            get { return comicSans; }
        }
        
        /// <summary>
        /// Шрифт по умолчанию
        /// </summary>
        public static Font DefaultFont
        {
            get { return timesNewRoman; }
        }

    }
}
