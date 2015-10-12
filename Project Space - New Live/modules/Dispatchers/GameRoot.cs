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
