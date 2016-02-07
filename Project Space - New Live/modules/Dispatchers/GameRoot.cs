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
        private PlayerInterfaceContainer GraphicInterfaceContainer;

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
           // this.GraphicInterface = this.GraphicModule.Form;//Получить указатель на главную форму
            this.ConstructWorld();//Сконструировать игровой мир
            this.playerContainer = PlayerContainer.GetInstanse(this.SystemCollection);//Инициализация игрока            
            this.GraphicInterfaceContainer = new PlayerInterfaceContainer(this.GraphicModule.Form, this.playerContainer);//Сконструировать контейнер игрового интерфейса
            this.ShipsCollection.Add(this.playerContainer.PlayerShip);//создание корабля игрока и установка контроллера
            this.ConstructFleets();//Инициализация кораблей
        }

        /// <summary>
        /// Построение звездных систем (временная реализация)
        /// </summary>
        private void ConstructWorld()
        {
            this.SystemCollection.Add(ResurceStorage.InitSystem1());//сконструировать одну звездную систему
        }

        /// <summary>
        /// Инициализация кораблей (временная реализация)
        /// </summary>
        private void ConstructFleets()
        {
            this.ShipsCollection.Add(new Ship(1000, new Vector2f(-450, 400), 250, ResurceStorage.shipTextures, new Vector2f(15, 30), SystemCollection[0]));
            this.ShipsCollection.Add(new Ship(900, new Vector2f(-450, -400), 250, ResurceStorage.shipTextures, new Vector2f(15, 30), SystemCollection[0]));
            this.ShipsCollection.Add(new Ship(800, new Vector2f(450, -400), 250, ResurceStorage.shipTextures, new Vector2f(15, 30), SystemCollection[0]));
            this.ShipsCollection.Add(new Ship(500, new Vector2f(450, 400), 250, ResurceStorage.shipTextures, new Vector2f(15, 30), SystemCollection[0]));
        }
        

        public void Main()
        {
            while (this.GraphicInterfaceContainer.cont)
            {
                Thread.Sleep(25);
                GraphicModule.MainWindow.Clear(); //перерисовка окна
                foreach (StarSystem currentSystem in this.SystemCollection)
                {
                    currentSystem.Process(this.ShipsCollection);
                }
                this.GraphicInterfaceContainer.Process();
                GraphicModule.RenderProcess(this.playerContainer.ActiveSystem);
                GraphicModule.MainWindow.DispatchEvents();
                GraphicModule.MainWindow.Display();
            }
             

        }


    }
}
