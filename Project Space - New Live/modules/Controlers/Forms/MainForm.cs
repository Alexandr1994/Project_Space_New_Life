﻿using System;
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

        /// <summary>
        /// Конструктор главной формы
        /// </summary>
        private MainForm(View gameView)
        {     
            this.Size = gameView.Size;//Установка размера формы равным размеру вида
            this.Location = gameView.Center - gameView.Size / 2;//Установка позиции в 0,0
            this.view = new ObjectView(new RectangleShape(this.Size), BlendMode.Multiply);
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
            /*
            Vector2i mousePosition = Mouse.GetPosition(RenderModule.getInstance().MainWindow);//Получить позицию курсора с учетом позиции окна
            if (mousePosition.X > this.Size.X || mousePosition.Y > this.Size.Y || mousePosition.X < 0 || mousePosition.Y < 0)
            {
                return true;
            }*/
            return false;
        }

        /// <summary>
        /// Вернуть нулевое смещение относительно начала координат
        /// </summary>
        /// <returns></returns>
        protected override Vector2f GetPhizicalPosition()
        {
            return new Vector2f(0, 0);
        }

        /// <summary>
        /// Получить отображения всех форм
        /// </summary>
        /// <returns></returns>
        public List<ObjectView> RenderForm()
        {
            return this.GetChildFormView();
        }

    }
}
