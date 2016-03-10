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
        public override Transformable InsideView 
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

        /// <summary>
        /// Анализ нахождения точки в области объекта
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool PointAnalize(Vector2f point, Vector2f center)
        {
            switch (this.Image.GetType().Name)//определиение объекта
            {
                case "RectangleShape":
                {
                    return RectanglePointTest(point, center);//прямоугольник
                }
                case "CircleShape":
                {
                    return this.EllipsePointTest(point, center);//круг/эллипс
                }
             /*   case "ConvexShape":
                {
                    return this.ConvexPointTest(point, center);//произвольный многоугольник
                }*/
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
            List<Vector2f> rectangleVertex = this.FindRectangleVertexes(center);//ищем координаты вершин прямоугольника
            List<Vector3f> rectangleLines = this.FindPoligonBorder(rectangleVertex);//ищем характеристики вершин составляющих прямоугольник
            return this.CommonPointTest(point, rectangleLines);//проверка нахождения точки в области прямоугольника
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
        /// Найти координаты вершин прямоугольника
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        private List<Vector2f> FindRectangleVertexes(Vector2f center)
        {
            Vector2f tempVertex = new Vector2f();
            List<Vector2f> vertexesCollection = new List<Vector2f>();
            float angle = (float)(this.image.Rotation * Math.PI) / 180;//угол поворота прямоугольника в радианах
            Vector2f sizes = (this.image as RectangleShape).Size;
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
                if (i == vertexColletion.Count-1)
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
        private float EllipseFuncValue(Vector2f point, Vector2f halfAxises, Vector2f center)
        {
            float angle = (float)(this.image.Rotation * Math.PI) / 180;//угол поворота эллипас в радианах
            Vector2f deltaCoords = point - center;//разность координат целевой точки и центра эллипас
            float shX = (float)(deltaCoords.X * Math.Cos(angle) + deltaCoords.Y * Math.Sin(angle));//X с учетом поворота эллипас
            float shY = (float)(-deltaCoords.X * Math.Sin(angle) + deltaCoords.Y * Math.Cos(angle));//Y с учетом поворота эллипас
            return (float)((Math.Pow(shX, 2) / Math.Pow(halfAxises.X, 2)) + (Math.Pow(shY, 2) / Math.Pow(halfAxises.Y, 2)) - 1);
        }

        /// <summary>
        /// Анализ пересечения границ объектов
        /// </summary>
        /// <returns></returns>
        public override bool BorderContactAnalize(RenderView targetView)
        {
            
            
            ImageView targetView2 = targetView as ImageView;
            
                //в независимости от типов отображений, если центр одного из отображений находится в области другого 
            if (this.PointAnalize(targetView.ViewCenter, this.ViewCenter) || targetView.PointAnalize(this.ViewCenter, targetView.ViewCenter))
            {//то объекты пересекаются
                return true;    
            }
            switch (this.Image.GetType().Name)//в зависимости от типа данного отображения
            {
                case "RectangleShape":
                {
                    switch (targetView2.Image.GetType().Name)//и типа проверяемого вызываем один и методов проверки
                    {
                        case "RectangleShape":
                        {//проверка прямоугольник и прямоугольник
                            return this.RectangleAndRectangleContactAnalize(targetView2);
                        }
                        case "CircleShape":
                        {//проверка эллипс и прямоугольник
                            return this.RectangleAndEllipceContactAnalize(targetView2);
                        }    
                    }
                }
                    break;
                case "CircleShape"://и типа проверяемого вызываем один и методов проверки
                {
                    switch (targetView2.Image.GetType().Name)
                    {//проверка эллипс и прямоугобник
                        case "RectangleShape":
                            {
                                return targetView2.RectangleAndEllipceContactAnalize(this);
                            }
                        case "CircleShape":
                            {//проверка эллипс и эллипс
                                return this.EllipseAndEllipseContactAnalize(targetView2);
                            }
                    }
                }
                    break;
            }
            return false;//в противном случае false
        }

        /// <summary>
        /// Анализ пересечения прямоугольника и эллипса
        /// </summary>
        /// <param name="targetView"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        private bool RectangleAndEllipceContactAnalize(ImageView targetView)
        {
            Vector2f center = this.ViewCenter;
            List<Vector2f> vertexesCoords = this.FindRectangleVertexes(center);//Поиск координат вершин
            List<Vector3f> linesCollection = this.FindPoligonBorder(vertexesCoords);//Поиск параметров функций прямых составляющих многоугольник
            //targetView - эллипс
            Vector2f halfAxises = new Vector2f();//полуоси
            halfAxises.X = (targetView.Image as CircleShape).Radius * targetView.Image.Scale.X;//нахождение полуоси Х
            halfAxises.Y = (targetView.Image as CircleShape).Radius * targetView.Image.Scale.Y;//нахождение полуоси Y
            Vector2f targetCenter = targetView.ViewCenter;
            float targetAngle = (float) (targetView.Image.Rotation*Math.PI/180);
            for (int i = 0; i < linesCollection.Count; i++)
            {
                //Индекс начальной вершины отрезка
                int vertexIndex1 = i;
                int vertexIndex2 = 0;
                if (i < linesCollection.Count - 1)
                {
                    vertexIndex2 = i + 1;
                }
                //Преобразование характеристик прямой
                float coefK = (float) (linesCollection[i].X/-linesCollection[i].Y);
                float constB = (float) (linesCollection[i].Z/-linesCollection[i].Y);
                //Вычисление промежуточных данных
                //Промежуточные данные первой ступени
                float C = (float) (Math.Cos(targetAngle) + coefK*Math.Sin(targetAngle));
                float D =
                    (float)
                        (constB*Math.Sin(targetAngle) - targetCenter.Y*Math.Sin(targetAngle) -
                         targetCenter.X*Math.Cos(targetAngle));
                float E = (float) (coefK*Math.Cos(targetAngle) - Math.Sin(targetAngle));
                float F =
                    (float)
                        (targetCenter.X*Math.Sin(targetAngle) + constB*Math.Cos(targetAngle) -
                         targetCenter.Y*Math.Cos(targetAngle));
                //Промежуточные данные второй ступени
                float G =
                    (float) ((Math.Pow(C, 2)/Math.Pow(halfAxises.X, 2)) + (Math.Pow(E, 2)/Math.Pow(halfAxises.Y, 2)));
                float H = (float) ((2*D*C/Math.Pow(halfAxises.X, 2)) + (2*E*F/Math.Pow(halfAxises.Y, 2)));
                float I =
                    (float)
                        ((Math.Pow(D, 2)/Math.Pow(halfAxises.X, 2)) + (Math.Pow(F, 2)/Math.Pow(halfAxises.Y, 2)) - 1);
                //Вычисление координат точек пересечения
                Vector2f[] crossPoints = new Vector2f[2] {new Vector2f(), new Vector2f()};
                //Вычисление точек пересечения
                crossPoints[0].X = (float) ((-H + Math.Sqrt(Math.Pow(H, 2) - 4*G*I))/(2*G));
                crossPoints[0].Y = (float) (coefK*crossPoints[0].X + constB);
                crossPoints[1].X = (float) ((-H - Math.Sqrt(Math.Pow(H, 2) - 4*G*I))/(2*G));
                crossPoints[1].Y = (float) (coefK*crossPoints[1].X + constB);
                if (crossPoints[0].X.Equals(float.NaN) && crossPoints[1].X.Equals(float.NaN))
                    //если х координаты точек пересечения не найдены
                {
                    continue; //Перейти к следующей точке
                }

                //Определение нахождения данной точник на требуемом отрнезке
                for (int j = 0; j < crossPoints.Length; j++)
                {
                    if (this.OnLineTest(vertexesCoords[vertexIndex1], vertexesCoords[vertexIndex2], crossPoints[j]))
                    {
                        return true;                        
                    }
                }
            }
            return false;//Нет соприкосновения
        }

        /// <summary>
        /// Проверак принадлежности точки отрезку
        /// </summary>
        /// <param name="beginCoords">Координаты начала отрезка</param>
        /// <param name="endCoords">Координаты конца отрезка</param>
        /// <param name="point">Проверяемая точка</param>
        private bool OnLineTest(Vector2f beginCoords, Vector2f endCoords, Vector2f point)
        {
            if (beginCoords.X < endCoords.X)//ограничение по X
            {
                if (beginCoords.X > point.X ||
                    endCoords.X < point.X)
                {
                    return false;//точка за пределами отрезка
                }
            }
            else
            {
                if (beginCoords.X < point.X ||
                    endCoords.X > point.X)
                {
                    return false;//точка за пределами отрезка
                }
            }
            if (beginCoords.Y < endCoords.Y)//ограничение по Y
            {
                if (beginCoords.Y <= point.Y &&
                    endCoords.Y >= point.Y)
                {
                    return true;//точка в области отрезка
                }
            }
            else
            {
                if (beginCoords.Y >= point.Y &&
                    endCoords.Y <= point.Y)
                {
                    return true;///точка в области отрезка
                }
            }
            return false;//точка за пределами отрезка
        }

        /// <summary>
        /// Проверка пересечения прямоугольников
        /// </summary>
        /// <param name="targetView"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        private bool RectangleAndRectangleContactAnalize(ImageView targetView)
        {
            //поиск собственных вершин и границы
            List<Vector2f> selfVertexes = this.FindRectangleVertexes(this.ViewCenter);
            List<Vector3f> selfLines = this.FindPoligonBorder(selfVertexes);
            //поиск вершин и границы целевой фигуры
            List<Vector2f> targetVertexes = targetView.FindRectangleVertexes(targetView.ViewCenter);
            List<Vector3f> targetLines = targetView.FindPoligonBorder(targetVertexes);

            for (int i = 0; i < selfLines.Count; i ++)
            {
                int selfIndex1 = i;
                int selfIndex2 = 0;
                if (i < selfLines.Count - 1)
                {
                    selfIndex2 = i + 1;
                }
                Vector2f crossPoint = new Vector2f();//точка пересечения
                float selfK = (float) (selfLines[i].X / -selfLines[i].Y);//параметры прямой линии
                float selfB = (float) (selfLines[i].Z / -selfLines[i].Y);
                for (int j = 0; j < targetLines.Count; j ++)
                {
                    int targetIndex1 = j;
                    int targetIndex2 = 0;
                    if (j < targetLines.Count - 1)
                    {
                        targetIndex2 = j + 1;
                    }
                    float targetK = (float)(targetLines[j].X / -targetLines[j].Y);//параметры прямой линии
                    float targetB = (float)(targetLines[j].Z / -targetLines[j].Y);
                    float dK = selfK - targetK;
                    float dB = targetB - selfB;
                    crossPoint.X = (float) (dB / dK);//поиск точки пересечения
                    crossPoint.Y = selfK*crossPoint.X + selfB;
                    if (OnLineTest(selfVertexes[selfIndex1], selfVertexes[selfIndex2], crossPoint) &&
                            OnLineTest(targetVertexes[targetIndex1], targetVertexes[targetIndex2], crossPoint))
                    {//и она лежит на отрезках
                            return true;//значит прямоугольники пересекаются
                        
                    }
                }
            }
            return false;
        }

        private bool EllipseAndEllipseContactAnalize(ImageView targetView)
        {
            Vector2f targetCenter = targetView.ViewCenter;
            Vector2f center = this.ViewCenter;
            Vector2f delta = center - targetCenter;
            double distance = Math.Sqrt(Math.Pow(delta.X, 2) + Math.Pow(delta.Y, 2));//расстояние между центарми отображений
            float betweenAngle = (float)(Math.Acos(delta.X / distance));
            float selfAngle = (float) ((this.Image.Rotation * Math.PI / 180) - betweenAngle);//угол поворота данного отображения
            float targetAngle = (float) (-(targetView.Image.Rotation * Math.PI / 180) + betweenAngle);//угол поворота проверяемого отображения
            Vector2f selfHalfAxises = new Vector2f();
            Vector2f targetHalfAxises = new Vector2f();
            selfHalfAxises.X = (this.Image as CircleShape).Radius * this.Image.Scale.X;//нахождение полуоси Х
            selfHalfAxises.Y = (this.Image as CircleShape).Radius * this.Image.Scale.Y;//нахождение полуоси Y
            targetHalfAxises.X = (targetView.Image as CircleShape).Radius * targetView.Image.Scale.X;//нахождение полуоси Х
            targetHalfAxises.Y = (targetView.Image as CircleShape).Radius * targetView.Image.Scale.Y;//нахождение полуоси Y
            float selfRadius = //вычисляем расстояние от центра до дуги данного эллипса
                (float)
                    (Math.Sqrt(Math.Pow(selfHalfAxises.X * Math.Cos(selfAngle), 2) +
                               Math.Pow(selfHalfAxises.Y * Math.Sin(selfAngle), 2)));
            float targetRadius = //вычисляем расстояние от центра до дуги проверяемого эллипса
                (float)
                    (Math.Sqrt(Math.Pow(targetHalfAxises.X * Math.Cos(targetAngle), 2) +
                               Math.Pow(targetHalfAxises.Y * Math.Sin(targetAngle), 2)));
            if (Math.Abs(distance) < Math.Abs(selfRadius + targetRadius))//если расстояние между центрами эллипсов меньше
            {                                                                //суммы расстояний от центов до дуг эллипсов,
                return true;//те эллипсы пересекаются
            }
            return false;
        }

    }
}
