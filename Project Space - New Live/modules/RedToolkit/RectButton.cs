﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace RedToolkit
{
    /// <summary>
    /// Прямоугольная/квадратная кнопка
    /// </summary>
    class RectButton : Button
    {

        /// <summary>
        /// Размер
        /// </summary>
        public override Vector2f Size
        {
            get { return this.size; }
            set
            {
                this.size = value;
                if (this.View != null)
                {
                    this.TextLocationCorrection();
                    (this.View.View as RectangleShape).Size = value;
                }
            }
        }

        /// <summary>
        /// Прямоугольная кнопка кнопка
        /// </summary>
        protected override void CustomConstructor()
        {
            this.Size = new Vector2f(80, 20);
            this.Location = new Vector2f(0, 0);
            this.view = new ImageView(new RectangleShape(this.Size), BlendMode.Alpha);  
            this.SetViewStates(ResurceStorage.rectangleButtonTextures);
            this.view.Image.Texture = this.viewStates[0];
            this.SetLabel();
            this.ButtonViewEventReaction();
           // this.CatchEvents();
        }

       
    }
}
