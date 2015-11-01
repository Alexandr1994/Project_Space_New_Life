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
            return false;
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

        /// <summary>
        /// Найти центр объекта отображения
        /// </summary>
        /// <returns></returns>
        public Vector2f FindImageCenter()
        {
            Vector2f center = new Vector2f();
            FloatRect imageParams = this.Image.GetGlobalBounds();
            center.X = imageParams.Left + imageParams.Width/2;
            center.Y = imageParams.Top + imageParams.Height/2;
            return center;
        }
        
        /// <summary>
        /// Анализ нахождения точки в области объекта
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool PointAnalize(Vector2f point, Vector2f center)
        {
            switch (this.Image.GetType().Name)//определиение объекта
            {
                case "RectangleShape":
                {
                    return RectanglePointTest(point, center);//прямоугольник
                }
                    ;break;
                case "CircleShape":
                {
                    return this.EllipsePointTest(point, center);//круг/эллипс
                }
                    ;break;
            }
            return false;
        }

        /// <summary>
        /// Проверка на нахождение точки в области прямоугольника
        /// </summary>
        /// <param name="point"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        private bool RectanglePointTest(Vector2f point, Vector2f center)
        {
            float angle = (float)(this.image.Rotation * Math.PI) / 180;//угол поворота прямоугольника в радианах
            Vector2f sizes = (this.image as RectangleShape).Size;
            //определение координат вершин прямоугольника 
            //вершина А  
            Vector2f A = new Vector2f();
            A.X = (float)(center.X - (sizes.X / 2 * Math.Cos(angle) - sizes.Y / 2 * Math.Sin(angle)));
            A.Y = (float)(center.Y - (sizes.X / 2 * Math.Sin(angle) + sizes.Y / 2 * Math.Cos(angle)));
            //вершина В 
            Vector2f B = new Vector2f();
            B.X = (float)(center.X + (sizes.X / 2 * Math.Cos(angle) + sizes.Y / 2 * Math.Sin(angle)));
            B.Y = (float)(center.Y - (-sizes.X / 2 * Math.Sin(angle) + sizes.Y / 2 * Math.Cos(angle)));
            //Вершина С
            Vector2f C = new Vector2f();
            C.X = (float)(center.X + (sizes.X / 2 * Math.Cos(angle) - sizes.Y / 2 * Math.Sin(angle)));
            C.Y = (float)(center.Y + (sizes.X / 2 * Math.Sin(angle) + sizes.Y / 2 * Math.Cos(angle)));
            //Вершина D
            Vector2f D = new Vector2f();
            D.X = (float)(center.X - (sizes.X / 2 * Math.Cos(angle) + sizes.Y / 2 * Math.Sin(angle)));
            D.Y = (float)(center.Y + (-sizes.X / 2 * Math.Sin(angle) + sizes.Y / 2 * Math.Cos(angle)));
            //Определение характеристик прямых составляющих прямоугольник
            Vector3f lineAB = this.FindLineCharacteristics(A, B);
            Vector3f lineBC = this.FindLineCharacteristics(B, C);
            Vector3f lineCD = this.FindLineCharacteristics(C, D);
            Vector3f lineDA = this.FindLineCharacteristics(D, A);
            //определение занчений функций в целевлй точке
            float valueAB = this.LineFuncValue(point, lineAB);
            float valueBC = this.LineFuncValue(point, lineBC);
            float valueCD = this.LineFuncValue(point, lineCD);
            float valueDA = this.LineFuncValue(point, lineDA);
            //если значение хоть одной функции в заданной точке меньше 0 то точка за пределами прямоугольника
            if (valueCD > 0 && valueAB > 0 && valueBC > 0 && valueDA > 0)
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Найти характесистики A, B и C прямой линии Ax+By+C=0
        /// </summary>
        /// <param name="beginPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns>Х - коэффициент А, Y - коэффициент В, Z - смещене C</returns>
        private Vector3f FindLineCharacteristics(Vector2f beginPoint, Vector2f endPoint)
        {
            Vector3f retValue = new Vector3f();
            retValue.X = beginPoint.Y - endPoint.Y;//коэффициент А
            retValue.Y = endPoint.X - beginPoint.X;//коэффициент В
            retValue.Z = endPoint.Y * beginPoint.X - endPoint.X * beginPoint.Y;//смещение С
            return retValue;
        }

        /// <summary>
        /// Рассчет значения функции в точке
        /// </summary>
        /// <param name="point">Координаты точки</param>
        /// <param name="lineCharacteristics">Характеристики линии</param>
        /// <returns></returns>
        private float LineFuncValue(Vector2f point, Vector3f lineCharacteristics)
        {
            return (float)(point.X * lineCharacteristics.X + point.Y * lineCharacteristics.Y + lineCharacteristics.Z);
        }

        /// <summary>
        /// Проверка на нахождение объекта в области круга/эллипаса 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        private bool EllipsePointTest(Vector2f point, Vector2f center)
        {
            Vector2f halfAxises = new Vector2f();//полуоси
            halfAxises.X = (this.Image as CircleShape).Radius * this.Image.Scale.X;//нахождение полуоси Х
            halfAxises.Y = (this.Image as CircleShape).Radius * this.Image.Scale.Y;//нахождение полуоси Y
            float value = this.EllipseFuncValue(point, halfAxises, center);
            if (value < 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Вычисление значения функции в целевой точке
        /// </summary>
        /// <param name="point"></param>
        /// <param name="halfAxises"></param>
        /// <returns></returns>
        public float EllipseFuncValue(Vector2f point, Vector2f halfAxises, Vector2f center)
        {
            float angle = (float)(this.image.Rotation * Math.PI) / 180;//угол поворота эллипас в радианах
            Vector2f deltaCoords = point - center;//разность координат целевой точки и центра эллипас
            float shX = (float)(deltaCoords.X * Math.Cos(angle) + deltaCoords.Y * Math.Sin(angle));//X с учетом поворота эллипас
            float shY = (float)(-deltaCoords.X * Math.Sin(angle) + deltaCoords.Y * Math.Cos(angle));//Y с учетом поворота эллипас
            return (float)((Math.Pow(shX, 2) / Math.Pow(halfAxises.X, 2)) + (Math.Pow(shY, 2) / Math.Pow(halfAxises.Y, 2)) - 1);
        }

    }
}
