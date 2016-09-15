using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules;
using RedToolkit;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using Project_Space___New_Live.modules.Storages;
using SFML.Graphics;
using SFML.System;

namespace RedToolkit
{
    /// <summary>
    /// Экран радара
    /// </summary>
    class RadarScreen : ImageRedWidget
    {

        public override Vector2f Size
        {
            get { return this.size; }
            set
            {
                if (this.View != null)
                {
                    float radius = (this.View.View as CircleShape).Radius;
                    float Xcoef = value.X / radius / 2;
                    float Ycoef = value.Y / radius / 2;
                    this.View.View.Scale = new Vector2f(Xcoef, Ycoef);//Изменение размеров изображения
                    this.size = value;//сохранение размеров
                }
                else
                {
                    size = value;
                }
            }
        }

        /// <summary>
        /// Размер видимой области
        /// </summary>
        Vector2f viewSize;

        /// <summary>
        /// Центр экрана радара
        /// </summary>
        private Vector2f radarCenter;
       
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
            this.view = new ImageView(new CircleShape(this.Size.X/2), BlendMode.Alpha);
            this.view.Image.FillColor = Color.Black;
            this.view.Image.OutlineThickness = 1;
            this.viewSize = RenderModule.getInstance().GameView.Size;
            this.radarCenter = this.Size/2;
            this.NoiseConstruct();
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
        /// Найти сотношение размера объекта на экране радара и его реального размера   
        /// </summary>
        /// <param name="visibleRadius">Радиус зоны видимости радара</param>
        /// <returns>Масштабирующий коэффициент</returns>
        private double GetSizeCoeffic(double visibleRadius)
        {
            return this.Size.X/2 / visibleRadius;    
        }


        /// <summary>
        /// Процесс работы экрана радара
        /// </summary>
        /// <param name="activeBaseEnvironmentкущая звездная система</param>
        /// <param name="player">Корабль игрока</param>
        public void RadarProcess(BaseEnvironment activeBaseEnvironment, ActiveObject player)
        {
            float radarCoeffic = 0;
            Radar playerRadar = player.Equipment[(int) Transport.EquipmentNames.Radar] as Radar;
            this.ChildWidgetsCollections.Clear();//Отчистить коллекцию объектов на радаре
            if (playerRadar != null && playerRadar.State)//Если радар имеется и функционирует
            {//то начать посторение отображения
                this.noise.Visible = false;//Спрятать радарный шум
                radarCoeffic = (float) this.GetSizeCoeffic(playerRadar.VisibleRadius);
                this.RenderPlayerOnRadar(player, radarCoeffic);
                foreach (GameObject currentObject in activeBaseEnvironment.GetObjectsInEnvironment(player.Coords, playerRadar.VisibleRadius))
                {
                    ObjectSignature currentSignature = currentObject.GetSignature();
                    if (currentSignature != null)
                    {
                        Vector2f size = currentSignature.Size; //Размер объекта
                        RadarEntity newObject = new RadarEntity();
                        newObject.Size = size*radarCoeffic;
                        newObject.Location = this.radarCenter - newObject.Size + (currentObject.Coords - player.Coords) * radarCoeffic;
                        this.AddWidget(newObject);
                    }
                }
                this.rerenderBasicSymbols(radarCoeffic);
            }
            else
            {
                this.noise.NoiseProcess();//иначе вывести радарный шум
                this.noise.Visible = true;
                this.AddWidget(this.noise);
            }
        }

        /// <summary>
        /// Отрисовка базовых символов на радаре
        /// </summary>
        /// <param name="radarCoeffic">Масштабирубщий коэффициент</param>
        private void rerenderBasicSymbols(float radarCoeffic)
        {
            VisibleRegion visibleRegion = new VisibleRegion();
            visibleRegion.Size = this.viewSize * radarCoeffic;
            visibleRegion.Location = radarCenter - visibleRegion.Size / 2;
            this.AddWidget(visibleRegion);//Вывести область видимости
            RadarRing radarRing = new RadarRing();
            radarRing.Size = this.size / 4;
            radarRing.Location = this.radarCenter - radarRing.Size;
            this.AddWidget(radarRing);//Вывести радарное кольцо
            radarRing = new RadarRing();
            radarRing.Size = new Vector2f((float)(this.Size.X / 2.7), (float)(this.Size.Y / 2.7));
            radarRing.Location = this.radarCenter - radarRing.Size;
            this.AddWidget(radarRing);
            RadarLine radarLine = new RadarLine();
            radarLine.Location = new Vector2f(0, this.radarCenter.Y);
            radarLine.Size = new Vector2f(this.radarCenter.X - (visibleRegion.Size.X / 2), 2);
            this.AddWidget(radarLine);
            radarLine = new RadarLine();
            radarLine.Location = new Vector2f(this.radarCenter.X + (visibleRegion.Size.X / 2), this.radarCenter.Y);
            radarLine.Size = new Vector2f(this.radarCenter.X - (visibleRegion.Size.X / 2), 2);
            this.AddWidget(radarLine);
            radarLine = new RadarLine();
            radarLine.Location = new Vector2f(this.radarCenter.X, 0);
            radarLine.Size = new Vector2f(2, this.radarCenter.Y - (visibleRegion.Size.Y / 2));
            this.AddWidget(radarLine);
            radarLine = new RadarLine();
            radarLine.Location = new Vector2f(this.radarCenter.X, this.radarCenter.Y + (visibleRegion.Size.Y / 2));
            radarLine.Size = new Vector2f(2, this.radarCenter.Y - (visibleRegion.Size.Y / 2));
            this.AddWidget(radarLine);
        }

        /// <summary>
        /// Отрисовка игрока на экране радара
        /// </summary>
        /// <param name="player">Корабль игрока</param>
        /// <param name="radarCoeffic">Масщтабирующий коэффициент</param>
        private void RenderPlayerOnRadar(ActiveObject player, float radarCoeffic)
        { 
            RadarEntity ship = new RadarEntity();
            ObjectSignature playerSignature = player.GetSignature();
            ship.Size = playerSignature.Size * radarCoeffic;
            ship.Location = this.radarCenter - ship.Size/2;
            this.AddWidget(ship);
        }

        /// <summary>
        /// Cущность на радаре
        /// </summary>
        private class RadarEntity : ImageRedWidget
        {
            /// <summary>
            /// Переопределение свойства размера (временная реализация)
            /// </summary>
            public override Vector2f Size
            {
                get { return this.size; }
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
                this.view = new ImageView(new CircleShape(), BlendMode.Alpha);
                this.view.Image.FillColor = Color.Green;
            }

            /// <summary>
            /// Сущность на радаре никогда не проходит проверку на нахождение точки в её области
            /// </summary>
            /// <param name="testingPoint">Проверяемая точка</param>
            /// <returns>false</returns>
            protected override bool PointTest(Vector2f testingPoint)
            {
                return false;
            }
        }

        /// <summary>
        /// Радарный шум
        /// </summary>
        private class RadarNoise : ImageRedWidget
        {

            public override Vector2f Size
            {
                get { return this.size; }
                set
                {
                    if (this.View != null)
                    {
                        float radius = (this.View.View as CircleShape).Radius;
                        float Xcoef = value.X / radius / 2;
                        float Ycoef = value.Y / radius / 2;
                        this.View.View.Scale = new Vector2f(Xcoef, Ycoef);//Изменение размеров изображения
                        this.size = value;//сохранение размеров
                    }
                    else
                    {
                        size = value;
                    }
                }
            }

            /// <summary>
            /// Конструктор радарного шума
            /// </summary>
            protected override void CustomConstructor()
            {
                this.Size = new Vector2f(100,100);
                this.Location = new Vector2f(0,0);
                this.view = new ImageView(new CircleShape(this.Size.X / 2), BlendMode.Alpha);
                this.view.Image.Texture = ImageStorage.Noise;
                this.view.Image.Texture.Repeated = this.view.Image.Texture.Smooth = true;

            }

            /// <summary>
            /// Радарный шум никогда не проходит проверку на нахождение точки в её области
            /// </summary>
            /// <param name="testingPoint">Проверяемая точка</param>
            /// <returns>false</returns>
            protected override bool PointTest(Vector2f testingPoint)
            {
                return false;
            }

            /// <summary>
            /// Процесс изменения текстуры шума
            /// </summary>
            public void NoiseProcess()
            {
                Random random = new Random();
                int sign = 0;
                while (sign == 0)
                {
                    sign = random.Next(-1, 1);
                }
                IntRect tempRect = this.view.Image.TextureRect;
                tempRect.Left += random.Next((int)this.size.X) * sign;
                tempRect.Top += random.Next((int)this.size.Y) * sign;
                this.view.Image.TextureRect = tempRect;
            }

        }

        /// <summary>
        /// Видимая заона на экране радара
        /// </summary>
        private class VisibleRegion : ImageRedWidget
        {

            /// <summary>
            /// Размер
            /// </summary>
            public override Vector2f Size
            {
                get { return this.size; }
                set
                {
                    this.size = value;
                    if (this.View != null)
                    {
                        (this.View.View as RectangleShape).Size = value;
                    }
                }
            }

            /// <summary>
            /// Конструктор видимой области
            /// </summary>
            protected override void CustomConstructor()
            {
                this.Size = new Vector2f(40, 30);
                view = new ImageView(new RectangleShape(this.Size), BlendMode.Alpha);
                this.view.Image.OutlineThickness = 2;
                this.view.Image.OutlineColor = new Color(0, 150, 0, 150);
                this.view.Image.FillColor = new Color(0, 0, 0, 0);
            }

            /// <summary>
            /// Зона видимости никогда не проходит проверку на нахождение точки в её области
            /// </summary>
            /// <param name="testingPoint">Проверяемая точка</param>
            /// <returns>false</returns>
            protected override bool PointTest(Vector2f testingPoint)
            {
                return false;
            }
        }

        /// <summary>
        /// Кольцо радарной зоны
        /// </summary>
        private class RadarRing : ImageRedWidget
        {

            public override Vector2f Size
            {
                get { return this.size; }
                set
                {
                    if (this.View != null)
                    {
                        float radius = (this.View.View as CircleShape).Radius;
                        float Xcoef = value.X / radius / 2;
                        float Ycoef = value.Y / radius / 2;
                        this.View.View.Scale = new Vector2f(Xcoef, Ycoef);//Изменение размеров изображения
                        this.size = value;//сохранение размеров
                    }
                    else
                    {
                        size = value;
                    }
                }
            }

            /// <summary>
            /// Конструктор радарного кольца
            /// </summary>
            protected override void CustomConstructor()
            {
                this.Size = new Vector2f(100, 100);
                this.view = new ImageView(new CircleShape(this.size.X), BlendMode.Alpha);
                this.view.Image.OutlineThickness = 4;
                this.view.Image.OutlineColor = new Color(0, 150, 0, 150);
                this.view.Image.FillColor = new Color(0, 0, 0, 0);
            }

            /// <summary>
            /// Радарное кольцо никогда не проходит проверку на нахождение точки в её области
            /// </summary>
            /// <param name="testingPoint">Проверяемая точка</param>
            /// <returns>false</returns>
            protected override bool PointTest(Vector2f testingPoint)
            {
                return false;
            }
        }

        /// <summary>
        /// Радарная линия
        /// </summary>
        private class RadarLine : ImageRedWidget
        {

            /// <summary>
            /// Размер
            /// </summary>
            public override Vector2f Size
            {
                get { return this.size; }
                set
                {
                    this.size = value;
                    if (this.View != null)
                    {
                        (this.View.View as RectangleShape).Size = value;
                    }
                }
            }

            /// <summary>
            /// Конструктор радарной линии
            /// </summary>
            protected override void CustomConstructor()
            {
                this.Size = new Vector2f(100, 4);
                this.view = new ImageView(new RectangleShape(this.size), BlendMode.Alpha);
                this.view.Image.FillColor = new Color(0, 150, 0, 150);
            }

            /// <summary>
            /// Радарная оиния никогда не проходит проверку на нахождение точки в её области
            /// </summary>
            /// <param name="testingPoint">Проверяемая точка</param>
            /// <returns>false</returns>
            protected override bool PointTest(Vector2f testingPoint)
            {
                return false;
            }
        }


    }
}
