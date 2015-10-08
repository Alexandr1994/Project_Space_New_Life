using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Controlers.Forms;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using Project_Space___New_Live.modules.GameObjects.ShipModules;
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
        /// Коэффициент уменьшения размера
        /// </summary>
        private int sizeCoef;


        /// <summary>
        /// Построение экрана радара
        /// </summary>
        protected override void CustomConstructor()
        {
            this.view = new ObjectView(new CircleShape(85), BlendMode.Alpha);
            this.view.Image.OutlineThickness = 1;
            this.Location = new Vector2f(5, 5);
            this.size = new Vector2f(170, 170);
            this.radarCenter = this.Size/2;
            this.view.Image.FillColor = Color.Black;
        }

        private double GetSizeCoeffic(Radar playerRadar)
        {
            float screenRadius = this.Size.X/2;
            if (playerRadar != null && playerRadar.State)
            {
                return screenRadius / playerRadar.VisibleRadius;    
            }
            return 0;
        }


        /// <summary>
        /// Работа радара (временная реализация)
        /// </summary>
        /// <param name="activeStarSystem"></param>
        public void RadarProcess(StarSystem activeStarSystem, Ship playerShip)
        {
            float radarCoeffic = 0;
            this.ChildForms.Clear();//Отчистить коллекцию объектов на радаре 
            foreach (GameObject currentObject in activeStarSystem.GetObjectsInSystem())
            {
               
                ObjectSignature currentSignature = currentObject.GetSignature();
                if (currentSignature != null)
                {
                    float mass = (float)currentSignature.Characteristics[(int)ObjectSignature.CharactsKeys.Mass];//Масса объекта
                    Vector2f size = (Vector2f)currentSignature.Characteristics[(int)ObjectSignature.CharactsKeys.Size];//Размер объекта
                    RadarEntity newObject = new RadarEntity();
                    radarCoeffic =
                        (float) this.GetSizeCoeffic(playerShip.Equipment[(int) Ship.EquipmentNames.Radar] as Radar);
                    newObject.Size = size * radarCoeffic;
                    newObject.Location = (currentObject.Coords - playerShip.Coords) * radarCoeffic + this.radarCenter - newObject.Size / 2;
                    this.AddForm(newObject);
                }
            }
            VisibleRegion region = new VisibleRegion();
            region.Size = new Vector2f(800, 600)*radarCoeffic;
            region.Location = radarCenter - new Vector2f(2,2) - region.Size/2 ;
            this.AddForm(region);
            
            this.RenderPlayerOnRadar(playerShip, radarCoeffic);
            
        }


        private void RenderPlayerOnRadar(Ship player, float radarCoeffic)
        { 

            RadarEntity ship = new RadarEntity();
            ObjectSignature playerSignature = player.GetSignature();
            ship.Size = (Vector2f)playerSignature.Characteristics[(int)ObjectSignature.CharactsKeys.Size] * radarCoeffic;
            ship.Location = this.radarCenter - ship.Size/2;
            
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
            if (distanse == 0)//Если расстояние от центра до точки равно 0
            {
                return true;//то true
            }
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
                view = new ObjectView(new RectangleShape(new Vector2f(20, 15)), BlendMode.Alpha);
                this.size = new Vector2f(20,15);
                this.view.Image.OutlineThickness = 2;
                this.view.Image.OutlineColor = Color.Green;
                this.view.Image.FillColor = new Color(0,0,0,0);
            }

            protected override bool PointTest(Vector2f testingPoint)
            {
                return false;
            }
        }

        private class RadarRing : Form
        {
            protected override void CustomConstructor()
            {
                
            }

            protected override bool PointTest(Vector2f testingPoint)
            {
                return false;
            }
        }

    }
}
