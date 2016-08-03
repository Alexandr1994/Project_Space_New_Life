using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.Window;

namespace RedToolkit
{
    class ActiveLabel : TextForm
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
        /// 
        /// </summary>
        protected override void CustomConstructor()
        {
            this.TextColors = new[] {Color.Black, Color.Blue, Color.Red, Color.Magenta};
            this.view = new TextView(this.text, BlendMode.Alpha, this.font);
            this.view.TextString.Color = this.TextColors[(int)(ViewStates.Normal)];
            this.SetViewReactions();
            this.ResaveTextString();
        }

        private void SetViewReactions()
        {
            this.MouseIn += this.ViewToActiveState;
            this.MouseMove += this.ViewToActiveState;
            this.MouseDown += this.ViewToPressedState;
            this.MouseUp += this.ViewToClickedState;
            this.MouseOut += this.ViewToNormalState;
        }

        private void ViewToActiveState(object sender, MouseMoveEventArgs e)
        {
            this.view.TextString.Color = this.TextColors[(int)(ViewStates.Active)];
        }

        private void ViewToPressedState(object sender, MouseButtonEventArgs e)
        {
            this.view.TextString.Color = this.TextColors[(int) (ViewStates.Pressed)];
        }

        private void ViewToClickedState(object sender, MouseButtonEventArgs e)
        {
            this.view.TextString.Color = this.TextColors[(int) (ViewStates.Clicked)];
        }

        private void ViewToNormalState(object sender, MouseMoveEventArgs e)
        {
            this.view.TextString.Color = this.TextColors[(int)(ViewStates.Normal)];
        }



    }
}
