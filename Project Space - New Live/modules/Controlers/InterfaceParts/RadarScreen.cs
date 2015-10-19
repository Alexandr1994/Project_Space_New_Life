using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
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
        /// Форма зоны видимости
        /// </summary>
     //   private VisibleRegion visReg;
        /// <summary>
        /// Форма шума
        /// </summary>
        private RadarNoise noise;


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
            this.Location = new Vector2f(10, 10);
            this.view = new ObjectView(new CircleShape(this.Size.X/2), BlendMode.Alpha);
            this.view.Image.FillColor = Color.Black;
            this.view.Image.OutlineThickness = 1;
            this.viewSize = RenderModule.getInstance().GameView.Size;
            this.radarCenter = this.Size/2;
            this.NoiseConstruct();
           // this.visReg = new VisibleRegion();
        }

        /// <summary>
        /// Постороить радарный шум
        /// </summary>
        private void NoiseConstruct()
        {
            this.noise = new RadarNoise();
            this.noise.Size = this.Size;
            this.noise.Visible = false;
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
            if (playerRadar != null && playerRadar.State)//Если радар имеется и функционирует
            {//то начать посторение отображения
                this.noise.Visible = false;
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
            else
            {//иначе вывести шум
                this.noise.NoiseProcess();
                this.noise.Visible = true;
                this.AddForm(this.noise);
            }
        }

        /// <summary>
        /// Отрисовка базовых символов на радаре
        /// </summary>
        /// <param name="radarCoeffic"></param>
        private void rerenderBasicSymbols(float radarCoeffic)
        {
            VisibleRegion visReg = new VisibleRegion();
            visReg.Size = this.viewSize * radarCoeffic;
            visReg.Location = radarCenter - visReg.Size / 2;
            this.AddForm(visReg);//Вывести область видимости
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

        /// <summary>
        /// Шум радара
        /// </summary>
        private class RadarNoise : Form
        {

            /// <summary>
            /// Текущий знак изменения текстуры
            /// </summary>
            private int currentSign = 1;
            
            /// <summary>
            /// Конструктор радарного шума
            /// </summary>
            protected override void CustomConstructor()
            {
                this.Size = new Vector2f(100,100);
                this.Location = new Vector2f(0,0);
                this.view = new ObjectView(new CircleShape(this.Size.X/2), BlendMode.Alpha);
                this.view.Image.Texture = ResurceStorage.noise;
                this.view.Image.Texture.Repeated = this.view.Image.Texture.Smooth = true;

            }

            /// <summary>
            /// Шум радара никогда непроходит проверку на нахождение точки в его области
            /// </summary>
            /// <param name="testingPoint"></param>
            /// <returns></returns>
            protected override bool PointTest(Vector2f testingPoint)
            {
                return false;
            }

            /// <summary>
            /// Процесс изменения текстуры шума
            /// </summary>
            public void NoiseProcess()
            {
                Random rand = new Random();
                int sign = this.currentSign/Math.Abs(this.currentSign);
                this.view.Image.TextureRect =
                    new IntRect(this.view.Image.TextureRect.Left + 150, this.view.Image.TextureRect.Top,
                        this.view.Image.TextureRect.Width, this.view.Image.TextureRect.Height);
                
            }

        }


        /// <summary>
        /// Видимая заона на экране радара
        /// </summary>
        private class VisibleRegion : Form
        {
            

            protected override void CustomConstructor()
            {
                this.Size = new Vector2f(40, 30);
                view = new ObjectView(new RectangleShape(this.Size), BlendMode.Alpha);
                this.view.Image.OutlineThickness = 2;
                this.view.Image.OutlineColor = Color.Green;
                this.view.Image.FillColor = new Color(0,0,0,0);
            }

            /// <summary>
            /// Зона видимости никогда не проходит проверку на нахождение курсора в её области
            /// </summary>
            /// <param name="testingPoint"></param>
            /// <returns></returns>
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
