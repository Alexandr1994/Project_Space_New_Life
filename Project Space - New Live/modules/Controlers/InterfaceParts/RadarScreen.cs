using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Controlers.Forms;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.Controlers.InterfaceParts
{
    class RadarScreen : Form
    {
        /// <summary>
        /// Центр экрана радара
        /// </summary>
        private Vector2f radarCenter;

        /// <summary>
        /// Построение экрана радара (временная реализация)
        /// </summary>
        protected override void CustomConstructor()
        {
            this.SetBasicReactions();
            this.view = new ObjectView(new CircleShape(100), BlendMode.Alpha);

            this.view.Image.OutlineThickness = 1;

            this.Location = new Vector2f(0,0);
            this.size = new Vector2f(200, 200);
            this.radarCenter = this.Size/2;
            this.view.Image.FillColor = Color.Black;
        }

        /// <summary>
        /// Работа радара (временная реализация)
        /// </summary>
        /// <param name="activeStarSystem"></param>
        public void RadarProcess(StarSystem activeStarSystem, Ship playerShip)
        {
            this.ChildForms.Clear();//Отчистить коллекцию объектов на радаре 
            foreach (GameObject currentObject in activeStarSystem.GetObjectsInSystem())
            {
                RadarEntity newObject = new RadarEntity();
                newObject.Size = new Vector2f(5, 5);
                newObject.Location = (currentObject.Coords - playerShip.Coords) / 40 + this.radarCenter - newObject.Size / 2;
                this.AddForm(newObject);
            }
            this.AddForm(new VisibleRegion());
            this.RenderPlayerOnRadar();
            
        }


        private void RenderPlayerOnRadar()
        {

            RadarEntity ship = new RadarEntity();
            ship.Location = this.radarCenter;
            ship.Size = new Vector2f(2, 2);
            this.AddForm(ship);
        }

        /// <summary>
        /// /// Проверка на нахождение точки в области формы
        /// </summary>
        /// <param name="testingPoint"></param>
        /// <returns></returns>
        protected override bool PointTest(Vector2f testingPoint)
        {
            this.SetBasicReactions();
            Vector2f center = this.GetPhizicalPosition() + new Vector2f(this.size.X / 2, this.size.Y / 2);//нахождение центра окружности образующей кнопку
            float dX = testingPoint.X - center.X;
            float dY = testingPoint.Y - center.Y;
            float distanse = (float)Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));//нахождение расстояния от точки до центра кнопки
            float angle = (float)Math.Atan(dY / dX);
            float radius = (float)(Math.Sqrt(Math.Pow((this.size.X / 2) * Math.Cos(angle), 2) + Math.Pow((this.size.Y / 2) * Math.Sin(angle), 2)));
            if (distanse < radius)//если это расстояние меньше радиуса
            {
                return true;//то true
            }
            return false;//иначе false
        }

        /// <summary>
        /// Cущность на радаре
        /// </summary>
        private class RadarEntity : Form
        {
            /// <summary>
            /// Переопределение свойства размера (временная реализация)
            /// </summary>
            public override Vector2f Size
            {
                set
                {
                    this.size = value;
                    CircleShape image = this.view.Image as CircleShape;
                    image.Radius = this.size.X;
                    this.view.Image = image;
                }
            }


            /// <summary>
            /// Построение сущности на радаре (временная реализация)
            /// </summary>
            protected override void CustomConstructor()
            {
                this.view = new ObjectView(new CircleShape(), BlendMode.Alpha);
                this.view.Image.FillColor = Color.Green;
            }


            protected override bool PointTest(Vector2f testingPoint)
            {
                return false;
            }
        }

        private class VisibleRegion : Form
        {
            protected override void CustomConstructor()
            {
               
                view = new ObjectView(new RectangleShape(RenderModule.getInstance().GameView.Size / 40), BlendMode.Alpha);
                this.Size = RenderModule.getInstance().GameView.Size/40;
                this.Location = new Vector2f(0,0) + this.Size/2 + new Vector2f(1,1);
                this.view.Image.OutlineThickness = 2;
                this.view.Image.OutlineColor = Color.Green;
                this.view.Image.FillColor = new Color(0,0,0,0);
                Vector2f lol = this.GetPhizicalPosition();
                
            }

            protected override bool PointTest(Vector2f testingPoint)
            {
                return true;
            }
        }

    }
}
