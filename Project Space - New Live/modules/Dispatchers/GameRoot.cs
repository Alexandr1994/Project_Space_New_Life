using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Messaging;
using System.Security.AccessControl;
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
        /// Временная задержка
        /// </summary>
        private static int sleepTime = 25;

        /// <summary>
        /// Временная задержка
        /// </summary>
        public static int SleepTime
        {
            get { return sleepTime; }
        }

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
        List<ActiveObject> ShipsCollection = new List<ActiveObject>();
        //List<Ship> ShipsCollection = new List<Ship>();

        /// <summary>
        /// Построение игры
        /// </summary>
        public GameRoot()
        {
            this.GraphicModule = RenderModule.getInstance();//Полученить указатель на модуль отрисовки
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
            this.ShipsCollection.Add(new Ship(500, new Vector2f(450, 450), 250, ResurceStorage.shipTextures, new Vector2f(15, 30), SystemCollection[0]));
            foreach (ActiveObject ship in this.ShipsCollection)
            {
                ship.SetBrains(new ComputerController(ship as Transport));
            }
        }

        /// <summary>
        /// Процесс игры в космосе
        /// </summary>
        private void SpaceGameProcess()
        {
            foreach (StarSystem currentSystem in this.SystemCollection) //Отработка игрового мира
            {
                currentSystem.RefreshActiveObjectsCollection(this.ShipsCollection as List<ActiveObject>);
                currentSystem.Process();
            }
            for (int i = 0; i < this.ShipsCollection.Count; i ++) //отчистка уничтоженных кораблей
            {
                if (this.ShipsCollection[i].Destroyed) //Если найден корабль перешедший в уничтоженное состояние
                {
                    this.ShipsCollection.Remove(this.ShipsCollection[i]);
                    //то удаление его из общей коллекции кораблей
                    i --;
                }
            }
        }

        /// <summary>
        /// Процесс игры на танковом поле боя
        /// </summary>
        private void BattleFieldGameProcess()
        {
            this.playerContainer.PlayerTank.TankBattleField.Process();
        }

        /// <summary>
        /// Главная функция корня игры
        /// </summary>
        public void Main()
        {
            while (this.GraphicInterfaceContainer.GameContinue)
            {
                Thread.Sleep(sleepTime);
                GraphicModule.MainWindow.Clear(); //перерисовка окна
                switch (this.playerContainer.CurrentMode)//в зависимости от текущего игрового режима
                {
                    case PlayerContainer.Mode.SpaceMode://выполнить итерационный процесс игры в космосе
                    {
                        this.SpaceGameProcess();
                    }; break;
                    case PlayerContainer.Mode.TankMode://выполнить итерационный процесс игры на танковом поле боя
                    {
                        this.BattleFieldGameProcess();
                    }; break;
                }
                this.GraphicInterfaceContainer.Process();
                this.GraphicModule.RenderProcess(this.playerContainer.ActiveEnvironment);
                GraphicModule.MainWindow.DispatchEvents();
                GraphicModule.MainWindow.Display();
            }
        }


    }
}
