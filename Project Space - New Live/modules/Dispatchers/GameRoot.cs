using System;
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

            // TESTING ENTITIES
            Vector2f centerCoords = new Vector2f(250, 225);
            Vector2f centerCoords1 = new Vector2f(345, 245);
            Vector2f centerCoords2 = new Vector2f(425, 235);
            Vector2f centerCoords3 = new Vector2f(495, 245);
            Vector2f sizes = new Vector2f(100, 50);

            ObjectView testView2 = new ObjectView(new CircleShape(sizes.X/2), BlendMode.Add);
            testView2.Image.FillColor = Color.Blue;
            testView2.Image.Scale = new Vector2f(sizes.X / sizes.X, sizes.Y / sizes.X);
            testView2.Image.Position = new Vector2f(380,210);

            ObjectView testView = new ObjectView(new RectangleShape(sizes), BlendMode.Add);
            testView.Image.FillColor = Color.Blue;
            testView.Image.Position = new Vector2f(200, 210);

            ObjectView testView1 = new ObjectView(new RectangleShape(sizes), BlendMode.Add);
            testView1.Image.FillColor = Color.Cyan;
            testView1.Image.Position = new Vector2f(295, 220);

            ObjectView testView3 = new ObjectView(new CircleShape(sizes.X / 2), BlendMode.Add);
            testView3.Image.FillColor = Color.Cyan;
            testView3.Image.Scale = new Vector2f(sizes.X / sizes.X, sizes.Y / sizes.X);
            testView3.Image.Position = new Vector2f(445, 220);


            while (cont)
            {
                Thread.Sleep(25);
                GraphicModule.MainWindow.DispatchEvents();
                GraphicModule.MainWindow.Clear(); //перерисовка окна
                testView.Rotate(centerCoords, (float)Math.PI / 360);
                testView1.Rotate(centerCoords1, (float)Math.PI / 180);
                testView2.Rotate(centerCoords2, (float)Math.PI / 360);
                testView3.Rotate(centerCoords3, (float)Math.PI / 180);
              
                Vector2i coords = Mouse.GetPosition(GraphicModule.MainWindow);
                if (testView.PointAnalize((Vector2f)coords, testView.FindImageCenter()))
                {
                    testView.Image.FillColor = Color.Red;
                }
                else
                {
                    testView.Image.FillColor = Color.Blue;
                }
                testView1.Image.FillColor = Color.Cyan;
                if (testView1.PointAnalize((Vector2f)coords, testView1.FindImageCenter()))
                {
                    testView1.Image.FillColor = Color.Magenta;
                }
                if (testView1.RectangleAndRectangleContactAnalize(testView, testView1.FindImageCenter()))
                {
                    testView1.Image.FillColor = Color.Green;
                }
                if (testView1.RectangleAndEllipceContactAnalize(testView2, testView1.FindImageCenter()))
                {
                    testView1.Image.FillColor = Color.Yellow;
                }
                testView3.Image.FillColor = Color.Cyan;
                if (testView3.PointAnalize((Vector2f)coords, testView3.FindImageCenter()))
                {
                    testView3.Image.FillColor = Color.Magenta;
                }
                if (testView3.EllipseAndEllipseContactAnalize(testView2, testView3.FindImageCenter()))
                {
                    testView3.Image.FillColor = Color.Green;
                }
                GraphicModule.MainWindow.Draw(testView.Image);
                GraphicModule.MainWindow.Draw(testView1.Image);
                GraphicModule.MainWindow.Draw(testView2.Image);
                GraphicModule.MainWindow.Draw(testView3.Image);
               
                GraphicModule.MainWindow.Display();
            }


           /* LinearBar scale = new LinearBar();
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
            }*/
        }


    }
}
