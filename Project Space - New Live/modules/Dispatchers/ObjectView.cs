using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Графическое отображение
    /// </summary>
    public class ObjectView
    {
        /// <summary>
        /// Отображение
        /// </summary>
        private Shape image = null;

        /// <summary>
        /// Свойство отображения
        /// </summary>
        public Shape Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

        /// <summary>
        /// Состояние отображения
        /// </summary>
        private RenderStates state;
        /// <summary>
        /// Свойство состояния отображения
        /// </summary>
        public RenderStates State
        {
            get { return this.state;}
            set { this.state = value; }
        }

        /// <summary>
        /// Создание пустого отображения
        /// </summary>
        public ObjectView()
        {

        }

        /// <summary>
        /// Создание отображения
        /// </summary>
        /// <param name="mode">Режим отрисовки</param>
        public ObjectView(BlendMode mode)
        {
            this.State = new RenderStates(mode);
        }

        /// <summary>
        /// Создание отображения
        /// </summary>
        /// <param name="image">Отображение</param>
        /// <param name="mode">Режим отрисовки</param>
        public ObjectView(Shape image, BlendMode mode)
        {
            this.Image = image;
            this.State = new RenderStates(mode);
        }

        /// <summary>
        /// Создание  отображения
        /// </summary>
        /// <param name="image">Отображение</param>
        /// <param name="imageState">Состояние отображения</param>
        public ObjectView(Shape image, RenderStates imageState)
        {
            this.Image = image;
            this.State = imageState;
        }

        /// <summary>
        /// Повернуть изображение на угол относительно точнки
        /// </summary>
        /// <param name="rotationCenter">Центр вращения</param>
        /// <param name="angle">Угол, на который будет произведен поворот (в радианах)</param>
        public void Rotate(Vector2f rotationCenter, float angle)
        {
            //вычсление новых координат 
            float newX = (float)(rotationCenter.X + ((image.Position.X - rotationCenter.X) * (Math.Cos(angle)) - ((image.Position.Y - rotationCenter.Y) * (Math.Sin(angle)))));
            float newY = (float)(rotationCenter.Y + ((image.Position.X - rotationCenter.X) * (Math.Sin(angle)) + ((image.Position.Y - rotationCenter.Y) * (Math.Cos(angle)))));
            this.image.Rotation += (float)(angle * (180 / Math.PI));//поворот изображения
            if (this.Image.Rotation > 360)
            {
                this.Image.Rotation -= 360;
            }
            this.image.Position = new Vector2f(newX, newY);//Установка скорректированной позиции объекта
        }

        /// <summary>
        /// Переместить изображение
        /// </summary>
        /// <param name="offsets">Смещение по осям Х и Y</param>
        public void Translate(Vector2f offsets)
        {
            this.image.Position = new Vector2f(this.image.Position.X + offsets.X, this.image.Position.Y + offsets.Y);
        }

        /// <summary>
        /// Проверка соприкосновения отображений
        /// </summary>
        /// <param name="contactObject"></param>
        /// <returns></returns>
        public bool ContactAnalize(ObjectView contactObject)
        {
            switch (this.Image.GetType().Name)//определить тип данного отображения
            {
                case "RectangleShape" ://данное отображение - прямоугольник
                {
                    switch (contactObject.Image.GetType().Name)//отобразить тип проверяемого отображенния
                    {
                        case "RectangleShape"://проверяемое отображение прямоугольник
                            {
                                return this.RectInRectAnalize(contactObject.Image as RectangleShape);
                            };
                        case "CircleShape"://проверяемое отображение - круг
                            {
                                return this.CircInRectAnalize(contactObject.Image as CircleShape);
                            };
                    }
                }; break;
                case "CircleShape" ://данное отображение - круг
                {
                    switch (contactObject.Image.GetType().Name)
                    {
                        case "RectangleShape"://проверяемое отображение прямоугольник
                            {
                                return this.RectInCircAnalize(contactObject.Image as RectangleShape);
                            };
                        case "CircleShape"://проверяемое отображение - круг
                            {
                                return this.CircInCircAnalize(contactObject.Image as CircleShape);
                            };
                    }
                }; break;
            }
        }

        /// <summary>
        /// Проверк соприкосновения прямоугольных отображений
        /// </summary>
        /// <param name="contactObject"></param>
        /// <returns></returns>
        private bool RectInRectAnalize(RectangleShape contactObject)
        {
            return false;
        }

        /// <summary>
        /// Проверка соприкосновения круглого с данным прямоугольным отображением
        /// </summary>
        /// <param name="contactObject"></param>
        /// <returns></returns>
        private bool CircInRectAnalize(CircleShape contactObject)
        {
            return false;
        }

        /// <summary>
        /// проверка соприкосновения прямоугольного с данным круглым отображением
        /// </summary>
        /// <param name="contactObject"></param>
        /// <returns></returns>
        private bool RectInCircAnalize(RectangleShape contactObject)
        {
            return false;
        }

        /// <summary>
        /// Проверка соприкосновения круглых отображений
        /// </summary>
        /// <param name="contactObject"></param>
        /// <returns></returns>
        private bool CircInCircAnalize(CircleShape contactObject)
        {
            return false;
        }

    }
}
