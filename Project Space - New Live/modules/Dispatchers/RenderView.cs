using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Абстрактное графическое отображение
    /// </summary>
    public abstract class RenderView
    {
        /// <summary>
        /// Внутреннее отображение
        /// </summary>
        protected abstract Drawable InsideView
        {
            get; 
            set; 
        }

        /// <summary>
        /// Состояние отображения
        /// </summary>
        private RenderStates state;

        /// <summary>
        /// Состояние отображения
        /// </summary>
        public RenderStates State
        {
            get { return this.state; }
            set { this.state = value; }
        }

        /// <summary>
        /// Центр объекта отображения
        /// </summary>
        /// <returns>Координаты центра отображения</returns>
        public Vector2f ViewCenter
        {
            get
            {
                Vector2f center = new Vector2f();
                FloatRect imageParams = new FloatRect();
                String typeName = this.InsideView.GetType().Name;
                if (typeName == "Text")
                {
                    imageParams = (this.InsideView as Text).GetGlobalBounds();
                }
                if(typeName == "RectangleShape" || typeName == "CircleShape" || typeName == "ConvexShape")
                {
                    imageParams = (this.InsideView as Shape).GetGlobalBounds();
                }
                center.X = imageParams.Left + imageParams.Width / 2;
                center.Y = imageParams.Top + imageParams.Height / 2;
                return center;
            }
        }


        /// <summary>
        /// Повернуть изображение на угол относительно точки
        /// </summary>
        /// <param name="rotationCenter">Центр вращения</param>
        /// <param name="angle">Угол, на который будет произведен поворот в рад.</param>
        public void Rotate(Vector2f rotationCenter, float angle)
        {
            String typeName = this.InsideView.GetType().Name;
            if (typeName == "Text")
            {
                this.TextRotate(rotationCenter, angle);
            }
            if(typeName == "RectangleShape" || typeName == "CircleShape" || typeName == "ConvexShape")
            {
                    this.ObjectRotate(rotationCenter, angle);
            }
        }

        /// <summary>
        /// Повернуть текст на угол относительно точки
        /// </summary>
        /// <param name="rotationCenter">Центр вращения</param>
        /// <param name="angle">Угол, на который будет произведен поворот в рад.</param>
        private void TextRotate(Vector2f rotationCenter, float angle)
        {
            Text textString = this.InsideView as Text;
            textString.Rotation += (float)(angle * (180 / Math.PI));//поворот изображения
            if (textString.Rotation > 360)
            {
                textString.Rotation -= 360;
            }
            textString.Position = this.GetNewPosition(textString.Position, rotationCenter, angle);//Установка скорректированной позиции объекта
        }

        /// <summary>
        /// Повернуть фигуру на угол относительно точки
        /// </summary>
        /// <param name="rotationCenter">Центр вращения</param>
        /// <param name="angle">Угол, на который будет произведен поворот в рад.</param>
        private void ObjectRotate(Vector2f rotationCenter, float angle)
        {
            Shape image = this.InsideView as Shape; 
            image.Rotation += (float)(angle * (180 / Math.PI));//поворот изображения
            if (image.Rotation > 360)
            {
                image.Rotation -= 360;
            }
            image.Position = this.GetNewPosition(image.Position, rotationCenter, angle);//Установка скорректированной позиции объекта
        }

        /// <summary>
        /// Вычислить новую позицию отображения
        /// </summary>
        /// <param name="currentPosition">Текущая позиция</param>
        //// <param name="rotationCenter">Центр вращения</param>
        /// <param name="angle">Угол, на который будет произведен поворот в рад.</param>
        /// <returns></returns>
        private Vector2f GetNewPosition(Vector2f currentPosition, Vector2f rotationCenter, float angle)
        {
            float newX = (float)(rotationCenter.X + ((currentPosition.X - rotationCenter.X) * (Math.Cos(angle)) - ((currentPosition.Y - rotationCenter.Y) * (Math.Sin(angle)))));
            float newY = (float)(rotationCenter.Y + ((currentPosition.X - rotationCenter.X) * (Math.Sin(angle)) + ((currentPosition.Y - rotationCenter.Y) * (Math.Cos(angle)))));
            return new Vector2f(newX, newY);
        }

        /// <summary>
        /// Переместить изображение
        /// </summary>
        /// <param name="offsets">Смещение по осям Х и Y</param>
        public void Translate(Vector2f offsets)
        {
            String typeName = this.InsideView.GetType().Name;
            if (typeName == "Text")
            {
                this.TranslateText(offsets);
            }
            if (typeName == "RectangleShape" || typeName == "CircleShape" || typeName == "ConvexShape")
            {
                this.TranslateImage(offsets);
            }
        }

        /// <summary>
        /// Переместить текст
        /// </summary>
        /// <param name="offsets">Смещение по осям Х и Y</param>
        private void TranslateText(Vector2f offsets)
        {
            Text textString = this.InsideView as Text;
            textString.Position = new Vector2f(textString.Position.X + offsets.X, textString.Position.Y + offsets.Y);
        }

        /// <summary>
        /// Переместить изображение
        /// </summary>
        /// <param name="offsets">Смещение по осям Х и Y</param>
        private void TranslateImage(Vector2f offsets)
        {
            Shape image = this.InsideView as Shape;
            image.Position = new Vector2f(image.Position.X + offsets.X, image.Position.Y + offsets.Y);
        }

    }
}
