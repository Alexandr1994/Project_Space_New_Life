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
        /// Размер видимой области
        /// </summary>
        Vector2f viewSize;
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
            this.Size = new Vector2f(170, 170);
            this.view = new ObjectView(new CircleShape(this.Size.X/2), BlendMode.Alpha);
            this.view.Image.FillColor = Color.Black;
            this.view.Image.OutlineThickness = 1;
            this.viewSize = RenderModule.getInstance().GameView.Size;
            this.Location = new Vector2f(5, 5);
            this.radarCenter = this.Size/2;
            
        }

        /// <summary>
        /// Найти сотношение размеров на радаре и в игровой зоне  
        /// </summary>
        /// <param name="visibleRadius"></param>
        /// <returns></returns>
        private double GetSizeCoeffic(double visibleRadius)
        {
            return this.Size.X/2 / visibleRadius;    
        }


        /// <summary>
        /// Работа радара (временная реализация)
        /// </summary>
        /// <param name="activeStarSystem"></param>
        public void RadarProcess(StarSystem activeStarSystem, Ship playerShip)
        {
            float radarCoeffic = 0;
            Radar playerRadar = playerShip.Equipment[(int) Ship.EquipmentNames.Radar] as Radar;
            this.ChildForms.Clear();//Отчистить коллекцию объектов на радаре
            if (playerRadar != null && playerRadar.State)
            {
                radarCoeffic = (float) this.GetSizeCoeffic(playerRadar.VisibleRadius);
                this.rerenderBasicSymbols(radarCoeffic);

                this.RenderPlayerOnRadar(playerShip, radarCoeffic);
                foreach (GameObject currentObject in activeStarSystem.GetObjectsInSystem(playerShip.Coords, playerRadar.VisibleRadius))
                {
                    ObjectSignature currentSignature = currentObject.GetSignature();
                    if (currentSignature != null)
                    {
                        float mass = (float) currentSignature.Characteristics[(int) ObjectSignature.CharactsKeys.Mass];
                            //Масса объекта
                        Vector2f size = (Vector2f) currentSignature.Characteristics[(int) ObjectSignature.CharactsKeys.Size];
                            //Размер объекта
                        RadarEntity newObject = new RadarEntity();

                        newObject.Size = size*radarCoeffic;
                        newObject.Location = this.radarCenter - newObject.Size + (currentObject.Coords - playerShip.Coords) * radarCoeffic;
                        this.AddForm(newObject);
                    }
                }
            }

        }

        /// <summary>
        /// Отрисовка базовых символов на радаре
        /// </summary>
        /// <param name="radarCoeffic"></param>
        private void rerenderBasicSymbols(float radarCoeffic)
        {
            VisibleRegion region = new VisibleRegion();
            region.Size = this.viewSize * radarCoeffic;
            region.Location = radarCenter - region.Size / 2;
            this.AddForm(region);
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
            float radius = (this.view.Image as CircleShape).Radius;
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
                    if (this.view.Image != null)
                    {
                        CircleShape image = this.view.Image as CircleShape;
                        image.Radius = this.size.X;
                        this.view.Image = image;
                    }
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
                this.Size = new Vector2f(20, 15);
                view = new ObjectView(new RectangleShape(this.Size), BlendMode.Alpha);
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
