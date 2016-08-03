using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.DataTypes
{
    class WidgetView
    {

        private Vector2f location;


        public Vector2f Location
        {
            get { return this.location; }
            set
            {
                Vector2f offset = this.location - value;
                this.location = value;
                this.Translate(-offset);
            }
        }

        private Vector2f size;

        public Vector2f Size
        {
            get { return this.size; }
        }


        private ConvexShape image;

        public ConvexShape Image
        {
            get { return this.image; }
        }


        public WidgetView(ConvexShape image)
        {
            this.image = image;
            this.location = image.Position;
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
                FloatRect imageParams = this.Image.GetGlobalBounds();
                center.X = imageParams.Left + imageParams.Width / 2;
                center.Y = imageParams.Top + imageParams.Height / 2;
                return center;
            }
        }

        /// <summary>
        /// Повернуть фигуру на угол относительно точки
        /// </summary>
        /// <param name="rotationCenter">Центр вращения</param>
        /// <param name="angle">Угол, на который будет произведен поворот в рад.</param>
        public void Rotate(Vector2f rotationCenter, float angle)
        {
            Shape image = this.Image as Shape;
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
        /// <param name="rotationCenter">Центр вращения</param>
        /// <param name="angle">Угол, на который будет произведен поворот в рад.</param>
        /// <returns></returns>
        private Vector2f GetNewPosition(Vector2f currentPosition, Vector2f rotationCenter, float angle)
        {
            float newX = (float)(rotationCenter.X + ((currentPosition.X - rotationCenter.X) * (Math.Cos(angle)) - ((currentPosition.Y - rotationCenter.Y) * (Math.Sin(angle)))));
            float newY = (float)(rotationCenter.Y + ((currentPosition.X - rotationCenter.X) * (Math.Sin(angle)) + ((currentPosition.Y - rotationCenter.Y) * (Math.Cos(angle)))));
            return new Vector2f(newX, newY);
        }

        /// <summary>
        /// Проверка точки на принадлежность многоугольнику
        /// </summary>
        /// <param name="point"></param>
        /// <param name="poligonLines"></param>
        /// <returns></returns>
        private bool CommonPointTest(Vector2f point, List<Vector3f> poligonLines)
        {
            foreach (Vector3f line in poligonLines)
            {
                if (this.LineFuncValue(point, line) < 0)//определение значениz функции в целевой точке
                {//если значение функции в заданной точке меньше 0 то точка за пределами многоугольника
                    return false;
                }
            }
            return true;//в случае, если все значения больше 0, то точка в области многоугольника
        }
        
        public bool PointAnalize(Vector2f point, Vector2f center) 
        {
            float angle = (float)(this.image.Rotation * Math.PI) / 180;//угол поворота прямоугольника в радианах
            ConvexShape tempShape = this.Image as ConvexShape;
            List<Vector2f> convexPoints = new List<Vector2f>();
            for (int i = 0; i < tempShape.GetPointCount(); i++)
            {
                Vector2f tempPoint = tempShape.GetPoint((uint)i) + this.location;
                float tempX = (float)(center.X + ((tempPoint.X - center.X) * (Math.Cos(angle)) - ((tempPoint.Y - center.Y) * (Math.Sin(angle)))));
                float tempY = (float)(center.Y + ((tempPoint.X - center.X) * (Math.Sin(angle)) + ((tempPoint.Y - center.Y) * (Math.Cos(angle)))));
                tempPoint = new Vector2f(tempX, tempY);
                convexPoints.Add(tempPoint);
            }
            List<Vector3f> convexLines = this.FindPoligonBorder(convexPoints);
            return this.CommonPointTest(point, convexLines);
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
        /// Найти характеристики прямых составляющих многоугольник
        /// </summary>
        /// <param name="vertexColletion"></param>
        /// <returns></returns>
        private List<Vector3f> FindPoligonBorder(List<Vector2f> vertexColletion)
        {
            List<Vector3f> linesCollection = new List<Vector3f>();
            for (int i = 0; i < vertexColletion.Count; i++)//перебрать все пары точек
            {//и найти характеристики всех линий
                if (i == vertexColletion.Count - 1)
                {
                    linesCollection.Add(this.FindLineCharacteristics(vertexColletion[i], vertexColletion[0]));
                    break;
                }
                linesCollection.Add(this.FindLineCharacteristics(vertexColletion[i], vertexColletion[i + 1]));
            }
            return linesCollection;
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
        /// Переместить изображение
        /// </summary>
        /// <param name="offsets">Смещение по осям Х и Y</param>
        public void Translate(Vector2f offsets)
        {
            this.Image.Position = new Vector2f(this.Image.Position.X + offsets.X, this.Image.Position.Y + offsets.Y);
        }
        
    }
}
