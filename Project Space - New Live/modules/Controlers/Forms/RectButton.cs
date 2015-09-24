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
    class RectButton : Button
    {
        /// <summary>
        /// Прямоугольная кнопка кнопка
        /// </summary>
        protected override void CustomConstructor()
        {
            this.view = new ObjectView(new RectangleShape(new Vector2f(80, 20)), BlendMode.Alpha);
            this.Location = view.Image.Position = new Vector2f(0, 0);
            this.SetViewStates(ResurceStorage.rectangleButtonTextures);
            this.size = new Vector2f(80, 20);
            this.view.Image.Texture = this.viewStates[0];
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
            Vector2f coords = this.GetPhizicalPosition();
            if (testingPoint.X > coords.X && testingPoint.Y > coords.Y && testingPoint.X < coords.X + this.Size.X && testingPoint.Y < coords.X + this.Size.Y)
            {
                return true;
            }
            return false;
        }

    }
}
