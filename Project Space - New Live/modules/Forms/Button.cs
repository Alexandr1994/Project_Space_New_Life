using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace Project_Space___New_Live.modules.Forms
{
    /// <summary>
    /// Абстрактная кнопка
    /// </summary>
    public abstract class Button : ImageForm
    {
        /// <summary>
        /// Состояния кнопки
        /// </summary>
        protected enum ViewStates : int
        {
            /// <summary>
            /// Нормальное
            /// </summary>
            Normal = 0,
            /// <summary>
            /// Активное
            /// </summary>
            Active,
            /// <summary>
            /// Зажатое
            /// </summary>
            Pressed,
            /// <summary>
            /// После клика
            /// </summary>
            Clicked
        }

        /// <summary>
        /// Размер
        /// </summary>
        public override Vector2f Size
        {
            get { return this.size; }
            set
            {
                base.Size = value;
                if (this.View != null)
                {
                    this.TextLocationCorrection();
                }
            }
        }

        /// <summary>
        /// Флаг нажатия левой кнопки мыши
        /// </summary>
        private bool leftPressed = false;

        /// <summary>
        /// Надпись на кнопке
        /// </summary>
        private ButtonLabel label;

        /// <summary>
        /// Текст надписи на кнопке
        /// </summary>
        public String Text
        {
            get { return this.label.Text; }
            set
            {
                this.label.Text = value;
                this.TextLocationCorrection();
            }
        }
        
        /// <summary>
        /// Цвета надписи на кнопке
        /// </summary>
        public Color[] TextColors
        {
            get { return this.label.TextColors; }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint FontSize
        {
            get { return this.label.CharSize; }
            set
            {
                this.label.CharSize = value;
                this.TextLocationCorrection();
            }
        }

        /// <summary>
        /// Текстуры состояний отображения формы
        /// </summary>
        protected Texture[] viewStates = new Texture[4];

        /// <summary>
        /// 
        /// </summary>
        protected void TextLocationCorrection()
        {
            if (this.label.Size.X > this.Size.X || this.label.Size.Y > this.Size.Y)
            {
                this.Size = this.label.Size + new Vector2f(40, 20);
            }
            this.label.Location = (this.Size / 2) - new Vector2f(this.label.Size.X / 2, (float)(label.CharSize / 1.5));
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetLabel()
        {
            ButtonLabel temp = new ButtonLabel();
            this.AddForm(temp);
            this.label = temp;
        }

        public bool SetTextColors(Color[] textColors)
        {
            if (textColors.Length == 4)
            {
                this.label.TextColors = textColors;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Установка набора текстур кнопки
        /// </summary>
        /// <param name="viewStates">Массив текстур состояний</param>
        public bool SetViewStates(Texture[] viewStates)
        {
            if (viewStates.Length == 4)
            {
                this.viewStates = viewStates;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Включение графического отображения состояний кнопки
        /// </summary>
        protected void ButtonViewEventReaction()
        {
            this.MouseIn += this.ViewToActiveState;
            this.MouseOut += this.ViewToNormalState;
            this.MouseMove += this.ViewToActiveState;
            this.MouseDown += this.ViewToPressedState;
            this.MouseClick += this.ViewToClickedState;
        }

        /// <summary>
        /// Приведение отображения в нормальное состояние
        /// </summary>
        /// <param name="sender">Форма, в которой возникло событие</param>
        /// <param name="e">Аргументы события</param>
        private void ViewToNormalState(object sender, MouseMoveEventArgs e)
        {
            this.view.Image.Texture = this.viewStates[(int)ViewStates.Normal];
            this.label.ViewToNormalState();
        }

        /// <summary>
        /// Преведение отображения в активное состояние
        /// </summary>
        /// <param name="sender">Форма, в которой возникло событие</param>
        /// <param name="e">Аргументы события</param>
        private void ViewToActiveState(object sender, MouseMoveEventArgs e)
        {
            this.view.Image.Texture = this.viewStates[(int)ViewStates.Active];
            this.label.ViewToActiveState();
        }

        /// <summary>
        /// Преведение отображения в нажатое состояние
        /// </summary>
        /// <param name="sender">Форма, в которой возникло событие</param>
        /// <param name="e">Аргументы события</param>
        private void ViewToPressedState(object sender, MouseButtonEventArgs e)
        {
            this.view.Image.Texture = this.viewStates[(int)ViewStates.Pressed];
            this.label.ViewToPressedState();
            this.leftPressed = true;
        }

        /// <summary>
        /// Преведение отображения в послекликовое состояние
        /// </summary>
        /// <param name="sender">Форма, в которой возникло событие</param>
        /// <param name="e">Аргументы события</param>
        private void ViewToClickedState(object sender, MouseButtonEventArgs e)
        {
            this.view.Image.Texture = this.viewStates[(int)ViewStates.Clicked];
            this.label.ViewToClickedState();
        }

        /// <summary>
        /// Надписть на кнопке
        /// </summary>
        private class ButtonLabel : TextForm
        {
            /// <summary>
            /// Состояния активной строки
            /// </summary>
            protected enum ViewStates : int
            {
                /// <summary>
                /// Нормальное
                /// </summary>
                Normal = 0,
                /// <summary>
                /// Активное
                /// </summary>
                Active,
                /// <summary>
                /// Зажатое
                /// </summary>
                Pressed,
                /// <summary>
                /// После клика
                /// </summary>
                Clicked
            }

            /// <summary>
            /// 
            /// </summary>
            private Color[] textColors = new Color[4];

            /// <summary>
            /// 
            /// </summary>
            public Color[] TextColors
            {
                get { return this.textColors; }
                set { this.textColors = value; }
            }

            /// <summary>
            /// Конструктор отображения кнопки
            /// </summary>
            protected override void CustomConstructor()
            {
                this.TextColors = new[] { Color.Black, Color.Black, Color.Black, Color.Black };
                this.view = new TextView(this.text, BlendMode.Alpha, this.font);
                this.view.TextString.Color = this.TextColors[(int)(ViewStates.Normal)];
                this.ResaveTextString();
            }

            protected override bool PointTest(Vector2f testingPoint)
            {
                return false;
            }

            public void ViewToActiveState()
            {
                this.view.TextString.Color = this.TextColors[(int)(ViewStates.Active)];
            }

            public void ViewToPressedState()
            {
                this.view.TextString.Color = this.TextColors[(int)(ViewStates.Pressed)];
            }

            public void ViewToClickedState()
            {
                this.view.TextString.Color = this.TextColors[(int)(ViewStates.Clicked)];
            }

            public void ViewToNormalState()
            {
                this.view.TextString.Color = this.TextColors[(int)(ViewStates.Normal)];
            }

        }


    }
}
