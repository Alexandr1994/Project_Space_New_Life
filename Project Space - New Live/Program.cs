using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using Project_Space___New_Live.modules;
using RedToolkit;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live
{
    internal class Program
    {

        private static void Main(string[] args)
        {
//            GameRoot game = new GameRoot();
//            game.Main();

              TestZone test = new TestZone();
              test.Main();

        } 
    }

}