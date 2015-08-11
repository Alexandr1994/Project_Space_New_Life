using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace Project_Space___New_Live.modules.Controlers
{
    public abstract class Button : Form
    {

        /// <summary>
        /// Состояния кнопки
        /// </summary>
        private enum ViewStates : int
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
            Pressed
        }

        /// <summary>
        /// отображение кнопки
        /// </summary>
        protected ObjectView view = new ObjectView();

        /// <summary>
        /// Текстуры состояний отображения формы
        /// </summary>
        protected Texture[] viewStates = new Texture[3];

        /// <summary>
        /// Включение графического отображения состояний кнопки
        /// </summary>
        protected void ButtonViewEventReaction()
        {
            this.MouseIn += this.ViewToActiveState;
            this.MouseOut += this.ViewToNormalState;
            this.MouseDown += this.ViewToPressedState;
            this.MouseUp += this.ViewToNormalState;
        }


        /// <summary>
        /// Отображения формы
        /// </summary>
        /// <returns></returns>
        public abstract List<ObjectView> GetFormView()
        {
            this.CatchEvents();//обнаружение событий данной формы
            List<ObjectView> retValue = new List<ObjectView>();
            retValue.Add(this.view);//добавление в массив возвращаемых занчений отображения данной формы
            foreach (Form childForm in this.ChildForms)//добавление в массив возвращаемых значений дочерних отображений фолрм
            {
                retValue.AddRange(childForm.GetFormView());
            }
            return retValue;
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
        private abstract void ViewToPressedState(object sender, EventArgs e)
        {
            this.view.Image.Texture = this.viewStates[(int)(ViewStates.Pressed)];
        }

    }
}
