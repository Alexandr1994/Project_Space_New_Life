﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Controlers;
using Project_Space___New_Live.modules.Controlers.Forms;
using Project_Space___New_Live.modules.Controlers.InterfaceParts;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live.modules.Dispatchers
{
    class GameRoot
    {

        /// <summary>
        /// Модуль отрисовки
        /// </summary>
        private RenderModule GraphicModule;

        /// <summary>
        /// Главная форма
        /// </summary>
        private MainForm GraphicInterface;

        /// <summary>
        /// Коллекция Звездных систем
        /// </summary>
        List<StarSystem> SystemCollection = new List<StarSystem>();

        /// <summary>
        /// Хранилище информации об игроке и активной звездной системе
        /// </summary>
        private PlayerContainer playerContainer;

        /// <summary>
        /// Коллекция космических кораблей
        /// </summary>
        List<Ship> ShipsCollection = new List<Ship>();

        /// <summary>
        /// Построение игры
        /// </summary>
        public GameRoot()
        {
            this.GraphicModule = RenderModule.getInstance();//Полученить указатель на модуль отрисовки
            this.GraphicInterface = this.GraphicModule.Form;//Получить указатель на главную форму
            this.ConstructWorld();//Сконструировать игровой мир
            this.playerContainer = PlayerContainer.GetInstanse(this.SystemCollection, new PlayerInterfaceContainer(this.GraphicInterface));//Инициализация игрока
            this.ShipsCollection.Add(this.playerContainer.PlayerShip);//создание корабля игрока и установка контроллера
        }

        /// <summary>
        /// Построение звездных систем (временная реализация
        /// </summary>
        private void ConstructWorld()
        {
            this.SystemCollection.Add(ResurceStorage.initSystem());//сконструировать одну звездную систему
        }


        private void OnClick(object sender, EventArgs e)
        {
            cont = false;
        }

        private bool cont = true;

        public void Main()
        {

            /* TESTING ENTITIES
            Vector2f centerCoords = new Vector2f(250, 225);
            Vector2f sizes = new Vector2f(100, 50);
            ObjectView testView = new ObjectView(new CircleShape(sizes.X/2), BlendMode.Add);
            testView.Image.FillColor = Color.Blue;
            testView.Image.Scale = new Vector2f(sizes.X / sizes.X, sizes.Y / sizes.X);
            testView.Image.Position = new Vector2f(200,200);
            float halfaxisX = (testView.Image as CircleShape).Radius * testView.Image.Scale.X;
            float halfaxisY = (testView.Image as CircleShape).Radius * testView.Image.Scale.Y;
            while (cont)
            {
                Thread.Sleep(25);
                GraphicModule.MainWindow.DispatchEvents();
                GraphicModule.MainWindow.Clear(); //перерисовка окна
                testView.Rotate(centerCoords, (float)Math.PI / 360);
                float angle = (float) (testView.Image.Rotation*Math.PI)/180;
                Vector2f A = new Vector2f();
                A.X = (float)(centerCoords.X + halfaxisX * Math.Cos(angle));
                A.Y = (float)(centerCoords.Y + halfaxisX * Math.Sin(angle));
                CircleShape a = new CircleShape(4);
                a.FillColor = Color.Green;
                a.Position = new Vector2f(A.X - 2, A.Y - 2); ;
                Vector2f B = new Vector2f();
                B.X = (float)(centerCoords.X + halfaxisY * Math.Cos(angle + Math.PI / 2));
                B.Y = (float)(centerCoords.Y + halfaxisY * Math.Sin(angle + Math.PI/2));
                CircleShape b = new CircleShape(4);
                b.FillColor = Color.Yellow;
                b.Position = new Vector2f(B.X - 2, B.Y - 2); ;
                /*Vector2f A = new Vector2f();
                A.X = (float)(centerCoords.X - (sizes.X / 2 * Math.Cos(angle) - sizes.Y / 2 * Math.Sin(angle)));
                A.Y = (float)(centerCoords.Y - (sizes.X / 2 * Math.Sin(angle) + sizes.Y / 2 * Math.Cos(angle)));
                Vector2f B = new Vector2f();
                B.X = (float)(centerCoords.X + (sizes.X / 2 * Math.Cos(angle) + sizes.Y / 2 * Math.Sin(angle)));
                B.Y = (float)(centerCoords.Y - (-sizes.X / 2 * Math.Sin(angle) + sizes.Y / 2 * Math.Cos(angle)));
                CircleShape b = new CircleShape(4);
                b.FillColor = Color.Yellow;
                b.Position = new Vector2f(B.X - 2, B.Y - 2);;
                Vector2f C = new Vector2f();
                C.X = (float)(centerCoords.X + (sizes.X / 2 * Math.Cos(angle) - sizes.Y / 2 * Math.Sin(angle)));
                C.Y = (float)(centerCoords.Y + (sizes.X / 2 * Math.Sin(angle) + sizes.Y / 2 * Math.Cos(angle)));
                CircleShape c = new CircleShape(4);
                c.FillColor = Color.Green;
                c.Position = new Vector2f(C.X - 2, C.Y - 2); ;
                Vector2f D = new Vector2f();
                D.X = (float)(centerCoords.X - (sizes.X / 2 * Math.Cos(angle) + sizes.Y / 2 * Math.Sin(angle)));
                D.Y = (float)(centerCoords.Y + (-sizes.X / 2 * Math.Sin(angle) + sizes.Y / 2 * Math.Cos(angle)));
                CircleShape d = new CircleShape(4);
                d.FillColor = Color.Yellow;
                d.Position = new Vector2f(D.X - 2, D.Y - 2)
                Vector2i coords = Mouse.GetPosition(GraphicModule.MainWindow);
                if (testView.PointAnalize((Vector2f)coords))
                {
                    testView.Image.FillColor = Color.Red;
                }
                else
                {
                    testView.Image.FillColor = Color.Blue;
                }
                GraphicModule.MainWindow.Draw(testView.Image);
                GraphicModule.MainWindow.Draw(a);
                GraphicModule.MainWindow.Draw(b);
               /* GraphicModule.MainWindow.Draw(c);
                GraphicModule.MainWindow.Draw(d);
                GraphicModule.MainWindow.Display();
            }*/


            LinearBar scale = new LinearBar();
            scale.Location = new Vector2f(200,0);
            scale.SetTexturets(new Texture[] {ResurceStorage.PanelText ,ResurceStorage.energyBar});
            this.GraphicInterface.AddForm(scale);

            CircleButton button = new CircleButton();
            button.MouseClick += OnClick;
            button.Size = new Vector2f(80, 30);
            button.Location = new Vector2f(720,0);

            this.GraphicInterface.AddForm(button);

            while (cont)
            {
                Thread.Sleep(25);
                GraphicModule.MainWindow.DispatchEvents();
                GraphicModule.MainWindow.Clear(); //перерисовка окна
                foreach (Ship currentShip in this.ShipsCollection)
                {
                    currentShip.Process(new Vector2f(0,0));
                    currentShip.AnalizeObjectInteraction(this.SystemCollection[currentShip.StarSystemIndex]);
                }
                foreach (StarSystem currentSystem in this.SystemCollection)
                {
                    currentSystem.Process();
                }
                this.playerContainer.Process();

                scale.PercentOfBar = this.playerContainer.GetEnergy();

                GraphicModule.RenderProcess(this.playerContainer.ActiveSystem, ShipsCollection);
                GraphicModule.MainWindow.Display();
            }
        }


    }
}
