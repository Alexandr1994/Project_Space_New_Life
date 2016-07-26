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
using Project_Space___New_Live.modules;
using Project_Space___New_Live.modules.Forms;

using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using Project_Space___New_Live.modules.Storages;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live.modules.Dispatchers
{


    class TestZone
    {

        public void Main()
        {
            CircleButton test = new CircleButton();
            test.Location = new Vector2f(100, 100);
            test.Size = new Vector2f(100, 100);
            test.Text = "Close";
            RedWindow win = new RedWindow();
            win.AddWidget(test, "testButton");
            win.Start();
            test = new CircleButton();
            test.Location = new Vector2f(100, 100);
            test.Size = new Vector2f(100, 100);
            test.Text = "Close";
            win = new RedWindow();
            win.AddWidget(test, "testButton");
            win.Start();
            test = new CircleButton();
            test.Location = new Vector2f(100, 100);
            test.Size = new Vector2f(100, 100);
            test.Text = "Close";
            win = new RedWindow();
            win.AddWidget(test, "testButton");
            win.Start();
        }

    }



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
                ship.SetBrains(new ComputerController(ship as Transport, 3000));
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
            for (int i = 0; i < this.ShipsCollection.Count; i++) //отчистка уничтоженных кораблей
            {
                if (this.ShipsCollection[i].Destroyed) //Если найден корабль перешедший в уничтоженное состояние
                {
                    this.ShipsCollection.Remove(this.ShipsCollection[i]);
                    //то удаление его из общей коллекции кораблей
                    i--;
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

  /*  class GameRoot
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
        /// Экземпляр среды
        /// </summary>
        private List<StarSystem> environment = new List<StarSystem>();

        /// <summary>
        /// Хранилище информации для отображения
        /// </summary>
        private ObjectContainer[] _objectContainers = new ObjectContainer[2];

        /// <summary>
        /// Коллекция активных объектов
        /// </summary>
        List<ActiveObject> activeObjectsCollection = new List<ActiveObject>();

        /// <summary>
        /// Построение игры
        /// </summary>
        public GameRoot()
        {
            this.GraphicModule = RenderModule.getInstance();//Полученить указатель на модуль отрисовки
            this.ConstructFleets();//Инициализация активных объектов
            this.ConstructWorld();//Конструирование среды
            for (int i = 0; i < this._objectContainers.Length; i++)
            {
                this._objectContainers[i] = new ObjectContainer(environment);//Инициализация игроков   
            }     
            this.GraphicInterfaceContainer = new PlayerInterfaceContainer(this.GraphicModule.Form, this._objectContainers);//Сконструировать контейнер игрового интерфейса
            this.InitPlayerContainers();//Инициализацимя контейнеров игроков
        }

        /// <summary>
        /// Построение среды
        /// </summary>
        private void ConstructWorld()
        {
            this.environment.Add(ResurceStorage.InitSystem2());
         //   this.environment.SetActiveObjectsCollection(this.activeObjectsCollection);
            foreach (ActiveObject activeObject in this.activeObjectsCollection)
            {
                activeObject.Environment = this.environment;
            }
        }

        /// <summary>
        /// Инициализация кораблей (временная реализация)
        /// </summary>
        private void ConstructFleets()
        {
            Texture[] firstObjectTextures = ImageStorage.GreenObject;
            Texture[] secondObjectTextures = ImageStorage.YellowObject;
            firstObjectTextures[1] = secondObjectTextures[0];
            secondObjectTextures[1] = firstObjectTextures[0];
            this.activeObjectsCollection.Add(new Ship(2000, new Vector2f(400, 400), 250, firstObjectTextures, new Vector2f(15, 30)));
            this.activeObjectsCollection.Add(new ActiveObject1(2000, new Vector2f(-450, 400), 250, secondObjectTextures, new Vector2f(15, 30)));
        }

        /// <summary>
        /// Инициализация контейноров игроков
        /// </summary>
        private void InitPlayerContainers()
        {
            this._objectContainers[0].SetControllingActiveObject(this.activeObjectsCollection[0], 0, 1000);
            this._objectContainers[1].SetControllingActiveObject(this.activeObjectsCollection[1], 1, 3000);
            this.environment.SetCheckPoints();//Установка контрольных точек
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
                this.environment.Process();
                foreach (ObjectContainer playerContainer in this._objectContainers)
                {
                    playerContainer.StatePlayerControll();
                }
                this.GraphicInterfaceContainer.Process();
                if (this.GraphicInterfaceContainer.RenderingEnvironment)
                {
                    this.GraphicModule.RenderProcess(this.environment);
                }
                else
                {
                    this.GraphicModule.RenderProcess();
                }
                this.GraphicModule.MainWindow.DispatchEvents();
                this.GraphicModule.MainWindow.Display();
            }
        }


    }*/
}
