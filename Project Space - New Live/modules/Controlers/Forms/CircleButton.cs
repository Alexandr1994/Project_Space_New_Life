using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Project_Space___New_Live.modules.Dispatchers;

namespace Project_Space___New_Live.modules.Controlers.Forms
{
    class CircleButton : Button
    {
        public override Vector2f Size
        {
            get { return this.size; }
            set
            {//Уравнивание значений
                //Нахождение коэффициентов масштабирования размеров
                float Xcoef = value.X/this.size.X;
                float Ycoef = value.Y/this.size.Y;
                //Изменение размеров изображения
                this.view.Image.Scale = new Vector2f(Xcoef, Ycoef);
                this.size = value;//сохранение размеров
            }
        }

        /// <summary>
        /// Круглая кнопка
        /// </summary>
        /// <param name="textures">Внешний набор текстур</param>
        public CircleButton(Texture[] textures)
        {
            this.view = new ObjectView(new CircleShape(20), BlendMode.Alpha);
            this.Location = view.Image.Position = new Vector2f(0, 0);
            this.viewStates = textures;
            this.size = new Vector2f(40, 40);
            this.view.Image.Texture = textures[0];
            this.ButtonViewEventReaction();
            this.CatchEvents();
        }

        /// <summary>
        /// /// Проверка на нахождение точки в области формы
        /// </summary>
        /// <param name="testingPoint"></param>
        /// <returns></returns>
        protected override bool PointTest(Vector2f testingPoint)
        {
            Vector2f center = this.GetPhizicalPosition() + new Vector2f(this.size.X / 2, this.size.Y / 2);//нахождение центра окружности образующей кнопку
            float dX = testingPoint.X - center.X;
            float dY = testingPoint.Y - center.Y;
            float distanse = (float)Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));//нахождение расстояния от точки до центра кнопки
            float angle = (float)Math.Atan(dY / dX);
            float radius = (float)(Math.Sqrt(Math.Pow((this.size.X / 2) * Math.Cos(angle), 2) + Math.Pow((this.size.Y / 2) * Math.Sin(angle), 2)));
            if (distanse < radius)//если это расстояние меньше радиуса
            {
                return true;//то true
            }
            return false;//иначе false
        }


    }
}
