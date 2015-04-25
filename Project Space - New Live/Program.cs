using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Project_Space___New_Live
{
    class Program
    {

        static void Main(string[] args)
        {
            Test test = new Test();
            test.main();
        }
    }

    class Test
    {

        static VideoMode testMode = new VideoMode(640, 480);
        Window testWindow = new Window(testMode, "Test");


        public void main()
        {           
            testWindow.Display();
            Thread.Sleep(1000);
        }

    }
}

