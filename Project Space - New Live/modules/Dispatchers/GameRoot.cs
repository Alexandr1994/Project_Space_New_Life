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
        /// Экземпляр среды
        /// </summary>
        private BaseEnvironment environment;

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
            this.environment = ResurceStorage.InitSystem2();
            this.environment.SetActiveObjectsCollection(this.activeObjectsCollection);
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
            this.activeObjectsCollection.Add(new ActiveObject(2000, new Vector2f(400, 400), 250, firstObjectTextures, new Vector2f(15, 30)));
            this.activeObjectsCollection.Add(new ActiveObject(2000, new Vector2f(-450, 400), 250, secondObjectTextures, new Vector2f(15, 30)));
        }

        /// <summary>
        /// Инициализация контейноров игроков
        /// </summary>
        private void InitPlayerContainers()
        {
            for (int i = 0; i < this._objectContainers.Length; i++)//Установка игроков
            {
                this._objectContainers[i].SetControllingActiveObject(this.activeObjectsCollection[i]);
            }
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
                this.GraphicModule.RenderProcess(this.environment);
                this.GraphicModule.MainWindow.DispatchEvents();
                this.GraphicModule.MainWindow.Display();
            }
        }


    }
}
