using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Project_Space___New_Live.modules.Dispatchers;

namespace Project_Space___New_Live.modules.Controlers
{
    class CircleButton : Button
    {

        private float radius;

        /// <summary>
        /// Круглая кнопка
        /// </summary>
        /// <param name="textures"></param>
        public CircleButton(Texture[] textures)
        {
            this.view = new ObjectView(new CircleShape(10), BlendMode.Alpha);
            this.Location = view.Image.Position = new Vector2f(0, 0);
            this.viewStates = textures;
            this.radius = 10;
            this.Size = new Vector2f(radius * 2, radius * 2);
            this.view.Image.Texture = textures[0];
            this.ButtonViewEventReaction();
        }

        /// <summary>
        /// Изменение размещения кнопки
        /// </summary>
        protected override void ChangeLocation()
        {
            this.view.Image.Position = this.Location;
        }

        /// <summary>
        /// Изменение размера кнопки
        /// </summary>
        protected override void ChangeSize()
        {
            CircleShape image = this.view.Image as CircleShape;//Извлечение изображения
            this.radius = image.Radius = this.Size.X/2;//изменеие размера
            this.view.Image = image;//сохранение изображения
        }

        /// <summary>
        /// Проверка на нахождение курсора в области формы
        /// </summary>
        /// <returns></returns>
        protected override bool MoveTest()
        {
            Vector2f center = this.Location + new Vector2f(radius, radius);//нахождение центра окружности образующей кнопку
            Vector2i mousePosition = Mouse.GetPosition(RenderClass.getInstance().MainWindow);//Нахождение текущей позиции мыши
            float distanse = (float)Math.Sqrt(Math.Pow(((float)mousePosition.X - center.X), 2) + Math.Pow(((float)mousePosition.Y - center.Y), 2));//нахождение расстояния от курсора до центра кнопки 
            if (distanse < radius)//если это расстояние меньше радиуса
            {
                return true;//то true
            }
            return false;//иначе false
        }

    }
}
