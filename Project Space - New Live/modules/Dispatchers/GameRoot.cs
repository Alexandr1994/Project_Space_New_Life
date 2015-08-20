using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Controlers;
using Project_Space___New_Live.modules.Controlers.Forms;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
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

        private PlayerController playerController;

        /// <summary>
        /// Коллекция Звездных систем
        /// </summary>
        List<StarSystem> SystemCollection = new List<StarSystem>();

        /// <summary>
        /// Индекс игрока в коллекции кораблей
        /// </summary>
        private const int PlayerID = 0; 

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
            this.ShipsCollection.Add(new Ship(800, new Vector2f(400, 400), ResurceStorage.shipTextures, new Vector2f(15, 30), 0));//создание корабля игрока и получение контроллера
        }

        /// <summary>
        /// Построение звездных систем (временная реализация
        /// </summary>
        private void ConstructWorld()
        {
            this.SystemCollection.Add(ResurceStorage.initSystem());//сконструировать одну звездную систему
        }

        public void Main()
        {
            while (true)
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
                GraphicModule.RenderProcess(this.SystemCollection[this.ShipsCollection[PlayerID].StarSystemIndex], ShipsCollection);
                GraphicModule.MainWindow.Display();
            }
        }


    }
}
