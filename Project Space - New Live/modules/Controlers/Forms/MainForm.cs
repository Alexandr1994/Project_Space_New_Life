using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live.modules.Controlers.Forms
{
    class MainForm : Form
    {
       
        private static MainForm form = null;

        public override Vector2f Size
        {
            get { return this.size; } 
            set { this.size = value; }
        }

        public override Vector2f Location 
        {
            get { return this.location; } 
            set { this.location = value; }
        }

        /// <summary>
        /// Конструктор главной формы
        /// </summary>
        private MainForm(View gameView)
        {
            this.Size = gameView.Size;//Установка размера формы равным размеру вида
            this.SetHomeLocation(new Vector2f(0, 0));//Установка управляющих координат равными координатам вида
            this.Location = gameView.Center - gameView.Size / 2;//Установка позиции в 0,0
        }

        /// <summary>
        /// Получить экземпляр главной формы (может существовать только в единственном экземпляре)
        /// </summary>
        /// <param name="gameView">Вид, в котором должена находиться главная форма</param>
        /// <returns></returns>
        public static MainForm GetInstance(View gameView)
        {
            if (form == null)
            {
                form = new MainForm(gameView);
            }
            return form;
        }

        /// <summary>
        /// Проверка на нахождение курсора в области формы
        /// </summary>
        /// <returns></returns>
        protected override bool MoveTest()
        {
            Vector2i mousePosition = Mouse.GetPosition(RenderClass.getInstance().MainWindow);//Получить позицию курсора с учетом позиции окна
            if (mousePosition.X > this.Size.X || mousePosition.Y > this.Size.Y || mousePosition.X < 0 || mousePosition.Y < 0)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Получить отображения всех форм
        /// </summary>
        /// <returns></returns>
        public List<ObjectView> RenderForm()
        {
            return this.GetChildFormView();
        }

        /// <summary>
        /// Получить отображение текущей формы
        /// </summary>
        /// <returns></returns>
        protected override ObjectView GetPersonalView()
        {
            return null;
        }
    }
}
