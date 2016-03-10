﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.Controlers.Forms
{
    /// <summary>
    /// Текстовая форма
    /// </summary>
    public abstract class TextForm : Form
    {
        /// <summary>
        /// Отображение
        /// </summary>
        protected TextView view;

        /// <summary>
        /// Отображение
        /// </summary>
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
            set
            {
                this.text = value;
                this.ResaveTextString();
            }
        }
        
        /// <summary>
        /// Шрифт
        /// </summary>
        protected Font font = FontsStorage.DefaultFont;

        /// <summary>
        /// Шрифт
        /// </summary>
        public Font Font
        {
            get { return this.font; }
            set
            {
                this.font = value;
                this.ResaveTextString();
            }
        }

        /// <summary>
        /// Размер шрифта
        /// </summary>
        protected uint charSize = 14;

        /// <summary>
        /// Размер шрифта
        /// </summary>
        public uint CharSize
        {
            get { return charSize; }
            set
            {
                charSize = value;
                this.ResaveTextString();
            }
        }

        /// <summary>
        /// Цвет текста
        /// </summary>
        protected Color textColor = Color.Black;

        /// <summary>
        /// Цвет текста
        /// </summary>
        public Color TextColor
        {
            get { return this.textColor; }
            set
            {
                this.textColor = value;
                this.ResaveTextString();
            }
        }

        /// <summary>
        /// Размер
        /// </summary>
        public override Vector2f Size
        {
            get { return this.size; }
        }

        /// <summary>
        /// Перезапись строки
        /// </summary>
        protected void ResaveTextString()
        {
            this.view.TextString.Font = this.font;
            this.view.TextString.CharacterSize = this.charSize;
            this.view.TextString.DisplayedString = this.text;
            this.view.TextString.Color = this.textColor;
            this.size = new Vector2f(this.view.TextString.GetLocalBounds().Width, this.view.TextString.GetLocalBounds().Height);
        }

    }
}
