using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace Project_Space___New_Live.modules.Controlers.Forms
{
    public abstract class Button : Form
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
        /// Флаг нажатия левой кнопки мыши
        /// </summary>
        private bool LeftPressed = false;

        /// <summary>
        /// Текстуры состояний отображения формы
        /// </summary>
        protected Texture[] viewStates = new Texture[4];

        /// <summary>
        /// Включение графического отображения состояний кнопки
        /// </summary>
        protected void ButtonViewEventReaction()
        {
            this.MouseIn += this.ViewToActiveState;
            this.MouseOut += this.ViewToNormalState;
            this.MouseMove += this.ViewToActiveState;
            this.MouseDown += this.ViewToPressedState;
            this.MouseUp += this.ButtonClickTest;
            this.MouseClick += this.ViewToClickedState;
        }

        /// <summary>
        /// Приведение отображения в нормальное состояние
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewToNormalState(object sender, EventArgs e)
        {
            this.view.Image.Texture = this.viewStates[(int) (ViewStates.Normal)];
        }

        /// <summary>
        /// Преведение отображения в активное состояние
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewToActiveState(object sender, EventArgs e)
        {
            this.view.Image.Texture = this.viewStates[(int)(ViewStates.Active)];
        }

        /// <summary>
        /// Преведение отображения в нажатое состояние
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewToPressedState(object sender, EventArgs e)
        {
            this.view.Image.Texture = this.viewStates[(int)(ViewStates.Pressed)];
            this.LeftPressed = true;
        }

        /// <summary>
        /// Преведение отображения в послекликовое состояние
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewToClickedState(object sender, EventArgs e)
        {
            this.view.Image.Texture = this.viewStates[(int)(ViewStates.Clicked)];
        }

        /// <summary>
        /// Преведение отображения в состояние после клика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClickTest(object sender, EventArgs e)
        {
            if (LeftPressed)
            {
                this.MouseClick(this, new MouseButtonEventArgs(new MouseButtonEvent()));
            }
            this.LeftPressed = false;
        }

        /// <summary>
        /// Событие клика кнопки
        /// </summary>
        public event EventHandler MouseClick = null;

    }
}
