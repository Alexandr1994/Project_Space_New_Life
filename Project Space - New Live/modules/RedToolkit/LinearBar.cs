﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;

namespace RedToolkit
{
    /// <summary>
    /// Линейный индикатор
    /// </summary>
    class LinearBar : ImageRedWidget
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
                    (this.View.View as RectangleShape).Size = value;
                }
            }
        }

        private bool visibleSubsrate = true;

        public bool VisibleSubstrate
        {
            get { return this.visibleSubsrate; }
            set
            {
                this.visibleSubsrate = value;
                this.HideShowSubstrate();
            }
        }

        /// <summary>
        /// Указатель на форму линии шкалы
        /// </summary>
        private BarLine lineOfBar;

        /// <summary>
        /// Показатель процента заполнения шкалы
        /// </summary>
        private float percentOfBar;

        /// <summary>
        /// Показатель процента заполнения шкалы
        /// </summary>
        public float PercentOfBar
        {
            get { return this.percentOfBar; }
            set
            {
                this.percentOfBar = value;
                this.lineOfBar.ChangePersent(this.percentOfBar);
            }
        }

        /// <summary>
        /// Конструктор индикаторной линии
        /// </summary>
        protected override void CustomConstructor()
        {
            this.Size = new Vector2f(200, 20);
            this.view = new ImageView(new RectangleShape(this.Size), BlendMode.Alpha);
            this.AddWidget(this.lineOfBar = new BarLine());
            this.PercentOfBar = 100;
            if (!this.visibleSubsrate)
            {
                this.view.Image.FillColor = new Color(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Установка текстур линейного индикатора
        /// </summary>
        /// <param name="textures">Массив текстур, где 1-ый элемент - текстура инидикатора, а 2-ой текстура подложки</param>
        /// <returns></returns>
        public bool SetTexturets(Texture[] textures)
        {
            if (textures.Length >= 2)
            {
                if (this.view.Image != null)
                {
                    this.view.Image.Texture = textures[0];
                    this.lineOfBar.LineTexture = textures[1];
                    return true;
                }
            }
            return false;
        }

        private void HideShowSubstrate()
        {
            if (this.visibleSubsrate)
            {
                this.view.Image.FillColor = new Color(0, 0, 0, Byte.MaxValue);
            }
            else
            {
                this.view.Image.FillColor = new Color(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Линия индикатора
        /// </summary>
        private class BarLine : ImageRedWidget
        {

            /// <summary>
            /// Установка текстуры линиии индикатора
            /// </summary>
            public Texture LineTexture
            {
                set
                {
                    if (this.view.Image != null)
                    {
                        this.view.Image.Texture = value;
                    }
                }
            }

            /// <summary>
            /// Полная длина индикатора
            /// </summary>
            private float fullLenght;

            /// <summary>
            /// Размер индикаторной линии
            /// </summary>
            public override Vector2f Size
            {
                get { return this.size; }
                set
                {
                    this.size = value;
                    if (this.View != null)
                    {
                        RectangleShape temp = this.view.Image as RectangleShape;
                        temp.Size = this.size;
                        this.view.Image = temp;
                    } 
                }
            }

            /// <summary>
            /// Конструктор заполняющей линии
            /// </summary>
            protected override void CustomConstructor()
            {
                this.Size = new Vector2f(200, 20);
                this.view = new ImageView(new RectangleShape(this.Size), BlendMode.Alpha);   
                this.fullLenght = this.Size.X;
            }

            /// <summary>
            /// Изменене показаний индикатора
            /// </summary>
            /// <param name="persent">Новые показания индикатора в процентах</param>
            public void ChangePersent(float persent)
            {
                this.Size = new Vector2f(this.fullLenght * persent/100, this.size.Y);
            }
           
        }

    }
}
