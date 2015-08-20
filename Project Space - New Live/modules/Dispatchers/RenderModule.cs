using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Controlers.Forms;
using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Графика и отрисовка
    /// </summary>
    class RenderModule
    {

        /// <summary>
        /// Окно программы
        /// </summary>
        private RenderWindow mainWindow;

        public RenderWindow MainWindow
        {
            get { return this.mainWindow; }
        }


        private MainForm mainForm;

        public MainForm Form
        {
            get { return this.mainForm; }
            set { this.mainForm = value; }
        }


        /// <summary>
        /// Доступные видеорежимы
        /// </summary>
        private static VideoMode windowSize = new VideoMode(800, 600);

        /// <summary>
        /// Экземпляр класса отрисовки
        /// </summary>
        private static RenderModule graphicModule = null;

        /// <summary>
        /// Текущий стиль отображения окна
        /// </summary>
        private Styles currentStyle = Styles.Titlebar;

        /// <summary>
        /// Заголовок окна
        /// </summary>
        private String windowTitle = "Project Space - New Life";

        /// <summary>
        /// Вид
        /// </summary>
        private View gameView = new View();

        /// <summary>
        /// Вид
        /// </summary>
        public View GameView
        {
            get { return this.gameView; }
            set
            {
                this.gameView = value;
                
            }
        }

        /// <summary>
        /// Запрет на вызов конструктора извне
        /// </summary>
        private RenderModule()
        {
            this.mainWindow = new RenderWindow(windowSize, windowTitle, currentStyle);//построение главного окна
            this.gameView.Size = (Vector2f)mainWindow.Size;//установка размера вида
            this.gameView.Center =  new Vector2f(gameView.Size.X/2, gameView.Size.Y/2);//установка центра вида
            mainForm = MainForm.GetInstance(gameView);
        }

        /// <summary>
        /// Получить экземпляр класса отрисовки
        /// </summary>
        /// <returns></returns>
        public static RenderModule getInstance()
        {
            if (graphicModule == null)
            {
                graphicModule = new RenderModule();

            }
            return graphicModule;
        }

          /// <summary>
        /// Получить окно нового размера
        /// </summary>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <returns>Новое окно</returns>
        public RenderWindow СhangeVideoMode(uint width, uint height)
        {
            windowSize = new VideoMode(width, height);
            ReconstructWindow();
            return mainWindow;
        }

        /// <summary>
        /// Получить окно нового стиля 
        /// </summary>
        /// <param name="newStyle">Стиль</param>
        /// <returns>Новое окно</returns>
        public RenderWindow changeWindowStyle(Styles newStyle)
        {
            currentStyle = newStyle;
            ReconstructWindow();
            return mainWindow;
        }


        /// <summary>
        /// Перестроение окна
        /// </summary>
        private void ReconstructWindow()
        {
            mainWindow.Close();
            mainWindow = new RenderWindow(windowSize, windowTitle, currentStyle);
        }

        /// <summary>
        /// Управление положением вида и главной формы
        /// </summary>
        private void ViewControl()
        {
            this.mainWindow.SetView(this.gameView);//установка вида отрисовки
           this.Form.Location = gameView.Center - gameView.Size / 2;
        }


        /// <summary>
        /// Метод отрисовки (только интерфейса)
        /// </summary>
        public void RenderProcess()
        {
            this.ViewControl();
            foreach (ObjectView view in this.Form.RenderForm())
            {
                mainWindow.Draw(view.Image, view.State);
            }
        }

        /// <summary>
        /// Метод отрисовки (игровых объектов и интерфейса)
        /// </summary>
        /// <param name="views">Набор игровых объектов</param>
        public void RenderProcess(StarSystem activeStarSystem, List<Ship> ships)
        {
            this.ViewControl();
            List<ObjectView> views = new List<ObjectView>();
            views.AddRange(activeStarSystem.GetView());
            foreach (Ship currentShip in ships)
            {
                views.AddRange(currentShip.View);
            }
            views.AddRange(this.Form.RenderForm());
           foreach (ObjectView view in views)
            {
               mainWindow.Draw(view.Image, view.State);
            }
        }
    }
}
