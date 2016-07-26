using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live.modules.Forms
{
    /// <summary>
    /// Главная форма
    /// </summary>
    class MainForm : ImageForm
    {
       /// <summary>
       /// Экземпляр формы
       /// </summary>
        private static MainForm form = null;

        /// <summary>
        /// Конструктор главной формы
        /// </summary>
        protected override void CustomConstructor()
        {
            this.Size = gameViewSize;//Установка размера формы равным размеру вида
            this.view = new ImageView(new RectangleShape(this.Size), BlendMode.Multiply);    
            this.Location = new Vector2f(0,0);//Установка позиции в 0,0
        }

        /// <summary>
        /// Ссылка на размер игрового вида
        /// </summary>
        private static Vector2f gameViewSize; 

        /// <summary>
        /// Получить экземпляр главной формы (может существовать только в единственном экземпляре)
        /// </summary>
        /// <param name="gameView">Вид, в котором должена находиться главная форма</param>
        /// <returns>Экземпляр главной формы</returns>
        public static MainForm GetInstance(View gameView)
        {
            if (form == null)
            {
                gameViewSize = gameView.Size;
                form = new MainForm();
            }
            return form;
        }

        /// <summary>
        /// /// Проверка на нахождение точки в области формы
        /// </summary>
        /// <param name="testingPoint">Координаты проверяемая точка</param>
        /// <returns>true - если точка находится в области формы, иначе - false</returns>
        protected override bool PointTest(Vector2f testingPoint)
        {
            if (testingPoint.X > this.Size.X || testingPoint.Y > this.Size.Y || testingPoint.X < 0 || testingPoint.Y < 0)//Если точка находится за пределами окна
            {
                return false;//то точка находится вне области формы
            }
            return true;//иначе точка в области формы
        }

        /// <summary>
        /// Получить физическую позицию формы
        /// </summary>
        /// <returns></returns>
        protected override Vector2f GetPhizicalPosition()
        {
            return new Vector2f(0, 0);
        }

        /// <summary>
        /// Получить отображения всех форм
        /// </summary>
        /// <returns>Коллекция отображений форм</returns>
        public List<RenderView> RenderForm()
        {
            return this.GetFormView(null);
        }
    }
}
