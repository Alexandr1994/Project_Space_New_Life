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
    class RectButton : Button
    {
        /// <summary>
        /// Прямоугольная кнопка кнопка
        /// </summary>
        /// <param name="textures">Внешний набор текстур</param>
        public RectButton(Texture[] textures)
        {
            this.view = new ObjectView(new RectangleShape(new Vector2f(80, 20)), BlendMode.Alpha);
            this.Location = view.Image.Position = new Vector2f(0, 0);
            this.viewStates = textures;
            this.size = new Vector2f(80, 20);
            this.view.Image.Texture = textures[0];
            this.ButtonViewEventReaction();
            this.CatchEvents();
        }

        /// <summary>
        /// Проверка на нахождение курсора в области формы
        /// </summary>
        /// <returns></returns>
        protected override bool MoveTest()
        {
            Vector2i mousePosition = Mouse.GetPosition(RenderModule.getInstance().MainWindow);
            Vector2f coords = this.GetPhizicalPosition();
            if (mousePosition.X > coords.X && mousePosition.Y > coords.Y && mousePosition.X < coords.X + this.Size.X && mousePosition.Y < coords.X + this.Size.Y)
            {
                return true;
            }
            return false;
        }
    }
}
