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
    class SystemRoot
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
        private PlayerContainer[] playerContainers = new PlayerContainer[2];

        /// <summary>
        /// Коллекция активных объектов
        /// </summary>
        List<ActiveObject> activeObjectsCollection = new List<ActiveObject>();

        /// <summary>
        /// Построение игры
        /// </summary>
        public SystemRoot()
        {
            this.GraphicModule = RenderModule.getInstance();//Полученить указатель на модуль отрисовки
            this.ConstructFleets();//Инициализация активных объектов
            this.ConstructWorld();//Конструирование среды
            for (int i = 0; i < this.playerContainers.Length; i++)
            {
                this.playerContainers[i] = new PlayerContainer(environment);//Инициализация игроков   
            }     
            this.GraphicInterfaceContainer = new PlayerInterfaceContainer(this.GraphicModule.Form, this.playerContainers);//Сконструировать контейнер игрового интерфейса
            
        }

        /// <summary>
        /// Построение звездных систем (временная реализация)
        /// </summary>
        private void ConstructWorld()
        {
            this.environment = ResurceStorage.InitSystem1();//сконструировать одну звездную систему
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
            this.activeObjectsCollection.Add(new ActiveObject(2000, new Vector2f(400, 400), 250, ResurceStorage.shipTextures, new Vector2f(15, 30)));
            this.activeObjectsCollection.Add(new ActiveObject(2000, new Vector2f(-450, 400), 250, ResurceStorage.shipTextures, new Vector2f(15, 30)));
        }

        /// <summary>
        /// Главная функция корня игры
        /// </summary>
        public void Main()
        {
            for (int i = 0; i < this.playerContainers.Length; i++)
            {
                this.playerContainers[i].SetControllingActiveObject(this.activeObjectsCollection[i]); 
            }     
            this.environment.SetCheckPoints();
            while (this.GraphicInterfaceContainer.GameContinue)
            {
                Thread.Sleep(sleepTime);
                GraphicModule.MainWindow.Clear(); //перерисовка окна
                this.environment.Process();
                this.GraphicInterfaceContainer.Process();
                this.GraphicModule.RenderProcess(this.environment);
                this.GraphicModule.MainWindow.DispatchEvents();
                this.GraphicModule.MainWindow.Display();
            }
        }


    }
}
