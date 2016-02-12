using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using Project_Space___New_Live.modules.Controlers;
using Project_Space___New_Live.modules.Controlers.Forms;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live
{
    /// <summary>
    /// Точка запуска
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Главная функция
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            GameRoot Game = new GameRoot();
            Game.Main();
        }
    }

}