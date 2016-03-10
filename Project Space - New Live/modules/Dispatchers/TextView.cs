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
    /// Графическое отображение текста
    /// </summary>
    public class TextView : RenderView
    {
        /// <summary>
        /// Внутренне свойство
        /// </summary>
        public override Transformable InsideView
        {
            get { return this.textString as Transformable; }
            set { this.textString = value as Text; }
        }

        /// <summary>
        /// Текстовая строка
        /// </summary>
        private Text textString = null;

        /// <summary>
        /// Текстовая строка
        /// </summary>
        public Text TextString
        {
            get { return this.textString; }
            set { this.textString = value; }
        }

        /// <summary>
        /// Создание текстового отображения
        /// </summary>
        /// <param name="text">Строка</param>
        /// <param name="mode">Режим отображения строки</param>
        /// <param name="font"Шрифи</param>
        public TextView(String text, BlendMode mode, Font font)
        {
            this.textString = new Text(text, font);
            this.State = new RenderStates(mode);
        }

        /// <summary>
        /// Создание текстового отображения
        /// </summary>
        /// <param name="text">Строка</param>
        /// <param name="textState">Состояние отображения строки</param>
        /// <param name="font">Шрифт</param>
        public TextView(String text, RenderStates textState, Font font)
        {
            this.textString = new Text(text, font);
            this.State = textState;
        }

        public override bool PointAnalize(Vector2f point, Vector2f center)
        {
            List<Vector2f> rectangleVertex = this.FindRectangleVertexes(center);//ищем координаты вершин прямоугольника
            List<Vector3f> rectangleLines = this.FindPoligonBorder(rectangleVertex);//ищем характеристики вершин составляющих прямоугольник
            return this.CommonPointTest(point, rectangleLines);//проверка нахождения точки в области прямоугольника
        }

        /// <summary>
        /// Найти координаты вершин прямоугольника
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        private List<Vector2f> FindRectangleVertexes(Vector2f center)
        {
            Vector2f tempVertex = new Vector2f();
            List<Vector2f> vertexesCollection = new List<Vector2f>();
            float angle = (float)(this.textString.Rotation * Math.PI) / 180;//угол поворота прямоугольника в радианах
            Vector2f sizes = new Vector2f(this.textString.GetLocalBounds().Width, this.textString.GetLocalBounds().Height);
            //определение координат вершин прямоугольника 
            //вершина А  
            tempVertex.X = (float)(center.X - (sizes.X / 2 * Math.Cos(angle) - sizes.Y / 2 * Math.Sin(angle)));
            tempVertex.Y = (float)(center.Y - (sizes.X / 2 * Math.Sin(angle) + sizes.Y / 2 * Math.Cos(angle)));
            vertexesCollection.Add(tempVertex);
            //вершина В 
            tempVertex.X = (float)(center.X + (sizes.X / 2 * Math.Cos(angle) + sizes.Y / 2 * Math.Sin(angle)));
            tempVertex.Y = (float)(center.Y - (-sizes.X / 2 * Math.Sin(angle) + sizes.Y / 2 * Math.Cos(angle)));
            vertexesCollection.Add(tempVertex);
            //Вершина С
            tempVertex.X = (float)(center.X + (sizes.X / 2 * Math.Cos(angle) - sizes.Y / 2 * Math.Sin(angle)));
            tempVertex.Y = (float)(center.Y + (sizes.X / 2 * Math.Sin(angle) + sizes.Y / 2 * Math.Cos(angle)));
            vertexesCollection.Add(tempVertex);
            //Вершина D
            tempVertex.X = (float)(center.X - (sizes.X / 2 * Math.Cos(angle) + sizes.Y / 2 * Math.Sin(angle)));
            tempVertex.Y = (float)(center.Y + (-sizes.X / 2 * Math.Sin(angle) + sizes.Y / 2 * Math.Cos(angle)));
            vertexesCollection.Add(tempVertex);
            return vertexesCollection;
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

        //TODO ЗАРЕШАТЬ
        public override bool BorderContactAnalize(RenderView targetView)
        {
            return true;
        }
    }
}
