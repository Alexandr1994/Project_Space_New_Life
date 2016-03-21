using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Графическое отображение фигуры
    /// </summary>
    public class ImageView : RenderView
    {

        /// <summary>
        /// Внутренне свойство
        /// </summary>
        public override Transformable View 
        {
            get { return this.image as Transformable; }
            set {this.image = value as Shape;}
        }

        /// Отображение фигуры
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
        /// Создание пустого отображения
        /// </summary>
        public ImageView()
        {

        }

        /// <summary>
        /// Создание отображения
        /// </summary>
        /// <param name="mode">Режим отрисовки</param>
        public ImageView(BlendMode mode)
        {
            this.State = new RenderStates(mode);
        }

        /// <summary>
        /// Создание отображения
        /// </summary>
        /// <param name="image">Отображение</param>
        /// <param name="mode">Режим отрисовки</param>
        public ImageView(Shape image, BlendMode mode)
        {
            this.Image = image;
            this.State = new RenderStates(mode);
        }

        /// <summary>
        /// Создание отображения
        /// </summary>
        /// <param name="image">Отображение</param>
        /// <param name="imageState">Состояние отображения</param>
        public ImageView(Shape image, RenderStates imageState)
        {
            this.Image = image;
            this.State = imageState;
        }

        /*private bool ConvexPointTest(Vector2f point, Vector2f center) 
        {
            float angle = (float)(this.image.Rotation * Math.PI) / 180;//угол поворота прямоугольника в радианах
            ConvexShape tempShape = this.Image as ConvexShape;
            List<Vector2f> convexPoints = new List<Vector2f>();
            for (int i = 0; i < tempShape.GetPointCount(); i++)
            {
                Vector2f tempPoint = tempShape.GetPoint((uint)i) + this.Location;
                float tempX = (float)(center.X + ((tempPoint.X - center.X) * (Math.Cos(angle)) - ((tempPoint.Y - center.Y) * (Math.Sin(angle)))));
                float tempY = (float)(center.Y + ((tempPoint.X - center.X) * (Math.Sin(angle)) + ((tempPoint.Y - center.Y) * (Math.Cos(angle)))));
                tempPoint = new Vector2f(tempX, tempY);
                convexPoints.Add(tempPoint);
            }
            List<Vector3f> convexLines = this.FindPoligonBorder(convexPoints);
            return this.CommonPointTest(point, convexLines);
        }*/

      

    }
}
