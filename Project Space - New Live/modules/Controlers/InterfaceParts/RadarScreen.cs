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
        /// <param name="player"></param>
        public RadarScreen()
        {
            this.view = new ObjectView(new RectangleShape(new Vector2f(180, 180)), BlendMode.Alpha);
            this.Location = new Vector2f(0,0);
            this.Size = new Vector2f(180, 180);
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
                newObject.Location = currentObject.Coords/40 + this.radarCenter;
                newObject.Size = new Vector2f(5, 5);
                this.AddForm(newObject);
            }
            RadarEntity ship = new RadarEntity();
            ship.Location = playerShip.Coords / 45 + this.radarCenter;
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
            Vector2f coords = this.GetPhizicalPosition();
            if (testingPoint.X > coords.X && testingPoint.Y > coords.Y && testingPoint.X < coords.X + this.Size.X && testingPoint.Y < coords.X + this.Size.Y)
            {
                return true;
            }
            return false;
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
            public RadarEntity()
            {
                this.view = new ObjectView(new CircleShape(), BlendMode.Alpha);
                this.view.Image.FillColor = Color.Green;
            }


            protected override bool PointTest(Vector2f testingPoint)
            {
                return false;
            }
        }

    }
}
