﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Project_Space___New_Live.modules.Controlers.Forms
{
    abstract class Panel : Form
    {

        /// <summary>
        /// Установка текстуры формы
        /// </summary>
        /// <param name="texture"></param>
        public void SetPanelTexture(Texture texture)
        {
            this.view.Image.Texture = texture;
        }

    }
}