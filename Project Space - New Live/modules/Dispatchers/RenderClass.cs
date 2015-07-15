﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Графика и отрисовка
    /// </summary>
    class RenderClass
    {

        Font font = new Font("times.ttf");
        /// <summary>
        /// Окно программы
        /// </summary>
        private RenderWindow mainWindow;
        /// <summary>
        /// Доступные видеорежимы
        /// </summary>
        private static VideoMode windowSize = new VideoMode(800, 600);
        /// <summary>
        /// Экземпляр класса отрисовки
        /// </summary>
        private static RenderClass graphicModule = null;
        /// <summary>
        /// Текущий стиль отображения окна
        /// </summary>
        private Styles currentStyle = Styles.Titlebar;
        /// <summary>
        /// Заголовок окна
        /// </summary>
        private String windowTitle = "Project Space - New Life";
        /// <summary>
        /// Запрет на вызов конструктора извне
        /// </summary>
        private RenderClass()
        {
            mainWindow = new RenderWindow(windowSize, windowTitle, currentStyle);
        }

        /// <summary>
        /// Получить экземпляр класса отрисовки
        /// </summary>
        /// <returns></returns>
        public static RenderClass getInstance()
        {
            if (graphicModule == null)
            {
                graphicModule = new RenderClass();

            }
            return graphicModule;
        }

        /// <summary>
        /// Получить главное графическое окно
        /// </summary>
        /// <returns></returns>
        public RenderWindow getMainWindow()
        {
            return mainWindow;
        }

        /// <summary>
        /// Получить окно нового размера
        /// </summary>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <returns>Новое окно</returns>
        public RenderWindow changeVideoMode(uint width, uint height)
        {
            windowSize = new VideoMode(width, height);
            reconstructWindow();
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
            reconstructWindow();
            return mainWindow;
        }


        /// <summary>
        /// Перестроение окна
        /// </summary>
        private void reconstructWindow()
        {
            mainWindow.Close();
            mainWindow = new RenderWindow(windowSize, windowTitle, currentStyle);
        }

        /// <summary>
        /// Метод отрисовка
        /// </summary>
        /// <param name="views"></param>
        public void RenderProcess(List<ObjectView> views)
        {
            foreach (ObjectView view in views)
            {
                mainWindow.Draw(view.Image, view.State);
                DrawDebugLabel(view);
            }
        }

        /// <summary>
        /// Отрисовка координат объекта для отладки
        /// </summary>
        private void DrawDebugLabel(ObjectView drawObject)
        {
            String infoString = drawObject.Image.Position.X.ToString() + " " + drawObject.Image.Position.Y.ToString();
            DrawLabel(drawObject.Image.Position, infoString);  
        }

        public void DrawLabel(Vector2f pos, String infoString)
        {
            Text info = new Text(infoString, font);
            info.CharacterSize = 12;
            info.Color = Color.Green;
            info.Position = pos;
            mainWindow.Draw(info);
        }

    }
}
