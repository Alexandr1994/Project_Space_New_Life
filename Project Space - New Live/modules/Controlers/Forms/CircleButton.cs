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

        public CircleButton(Vector2f location, float radius, Texture[] textures)
        {
            this.CatchEvents();
            this.view = new ObjectView(new CircleShape(radius), BlendMode.Alpha);
            this.Location = view.Image.Position = location;
            this.viewStates = textures;
            this.radius = radius;
            this.Size = new Vector2f(radius * 2, radius * 2);
            this.view.Image.Texture = textures[0];
            this.ButtonViewEventReaction();
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
