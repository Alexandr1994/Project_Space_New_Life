using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.Controlers.Forms
{
    /// <summary>
    /// Линейный индикатор
    /// </summary>
    class LinearBar : Form
    {
        public override Vector2f Size {
            get { return this.size; }
            set
            {
                this.size = value;
                RectangleShape temp = this.view.Image as RectangleShape;
                temp.Size = this.size;
                this.view.Image = temp;

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

        public LinearBar()
        {
            this.view = new ObjectView(new RectangleShape(new Vector2f(200, 20)), BlendMode.Alpha);
            this.Size = new Vector2f(200, 20);
            this.AddForm(this.lineOfBar = new BarLine());
            this.PercentOfBar = 100;
        }

        /// <summary>
        /// Проверка на наличие точки в области формы
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

        /// <summary>
        /// Линия индикатора
        /// </summary>
        private class BarLine : Form
        {

            /// <summary>
            /// Полная длина индикатора
            /// </summary>
            private float fullLenght;

            public override Vector2f Size
            {
                get { return this.size; }
                set
                {
                    this.size = value;
                    RectangleShape temp = this.view.Image as RectangleShape;
                    temp.Size = this.size;
                    this.view.Image = temp;

                }
            }

            public BarLine()
            {
                this.view = new ObjectView(new RectangleShape(new Vector2f(200, 20)), BlendMode.Alpha);
               
                this.view.Image.FillColor = Color.Cyan;
               
                this.Size = new Vector2f(200, 20);
                this.fullLenght = this.Size.X;
            }

            public void ChangePersent(float persent)
            {
                this.Size = new Vector2f(this.fullLenght * persent/100, this.size.Y);
            }

            /// <summary>
            /// Проверка на наличие точки в области формы
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
}
