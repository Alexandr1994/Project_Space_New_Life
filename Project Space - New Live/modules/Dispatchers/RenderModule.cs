﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RedToolkit;
using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Модуль графика и отрисовки
    /// </summary>
    class RenderModule
    {

        /// <summary>
        /// Окно программы
        /// </summary>
        private RenderWindow mainWindow;

        /// <summary>
        /// Окно программы
        /// </summary>
        public RenderWindow MainWindow
        {
            get { return this.mainWindow; }
        }

        /// <summary>
        /// Главная форма (Интерфейс)
        /// </summary>
        private MainRedWidget _mainRedWidget;

        /// <summary>
        /// Главная форма (Интерфейс)
        /// </summary>
        public MainRedWidget RedWidget
        {
            get { return this._mainRedWidget; }
            set { this._mainRedWidget = value; }
        }

        /// <summary>
        /// Доступные видеорежимы
        /// </summary>
        private static VideoMode windowSize = new VideoMode(1280, 720);

        /// <summary>
        /// Экземпляр модуля отрисовки
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
            this.gameView.Center =  new Vector2f(gameView.Size.X / 2, gameView.Size.Y / 2);//установка центра вида
            _mainRedWidget = MainRedWidget.GetInstance(gameView);
        }

        /// <summary>
        /// Получить экземпляр модуля отрисовки
        /// </summary>
        /// <returns>Ссылка на экземпляр модуля отрисовки</returns>
        public static RenderModule getInstance()
        {
            if (graphicModule == null)
            {
                self:graphicModule = new RenderModule();
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
           this.RedWidget.Location = gameView.Center - gameView.Size / 2;
        }


        /// <summary>
        /// Метод отрисовки (только интерфейса)
        /// </summary>
        public void RenderProcess()
        {
            this.ViewControl();
            foreach (RenderView view in this.RedWidget.RenderForm())
            {
                mainWindow.Draw(view.View as Drawable, view.State);
            }
        }

        /// <summary>
        /// Метод отрисовки (игровых объектов и интерфейса)
        /// </summary>
        /// <param name="activeBaseEnvironmentтивная звездная система</param>
        public void RenderProcess(BaseEnvironment activeBaseEnvironment)
        {
            this.ViewControl();
            List<RenderView> views = new List<RenderView>();
            views.AddRange(activeBaseEnvironment.GetView());
            views.AddRange(this.RedWidget.RenderForm());

            foreach (RenderView view in views)
            {
                mainWindow.Draw(view.View as Drawable, view.State);
            }
        }
    }
}
