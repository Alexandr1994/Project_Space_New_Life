using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live
{
    internal class Program
    {

  
        private static void Main(string[] args)
        {

            var test = new Test();
            test.main();
        } 
    }

    internal class Test
    {
        private static Texture backText = new Texture("testBackground.png"); //загруженная текстура планет 
        private static Texture img = new Texture("textPlayer.png"); //загруженная текстура планет 
        private Texture starText = new Texture("testStarText.jpg"); //загруженная текстура звезды
        private  Texture shadowTexture = new Texture("shadow.png");//тень
        private  Texture crownText = new Texture("crown.png");//звездная корона
        private double[] angle = new double[5]; //орбитальные углы планет
        private double angleS;    
        private int[] orbits = {600, 1000, 1700}; //орибиты планет 
        private Texture planetText = new Texture("testPlanetText.jpg"); //загруженная текстура планет 
        private StarSystem system;
        private RenderClass testRenderer;




        private void initSystem() //инициализацимя звездной системы
        {
            Texture[] texts = new Texture[2];
            texts[0] = starText;
            texts[1] = crownText;
            Star[] stars = new Star[1];
            stars[0] = new Star(10000000, 250, 0, 0,0 , texts);
            


            texts[0] = planetText;
            texts[1] = shadowTexture;

            Planet[] planets = new Planet[3];
            for (int i = 0; i < planets.Length; i++)
            {
              
                planets[i] = new Planet(10, 50, orbits[i], 0.002 / (1 + i), texts);
            }
            LocalMassCenter center = new LocalMassCenter(0, 0, 0, stars, planets);

            system = new StarSystem(center,backText);
        }

        public void main()
        {
            testRenderer = RenderClass.getInstance();
            initSystem();
            
            Texture[] textures = new Texture[4];
            for (int i = 0; i < 4; i++)
            {
                textures[i] = new Texture("textPlayer.png");
            }
            Ship testPlayer = new Ship(1000, new Vector2f(400, 400), textures, new Vector2f(10, 20));
            //пока окно открыто ловить события и перерисовывать окно
            while (testRenderer.MainWindow.IsOpen)
            {
                Thread.Sleep(25);
               // testWindow.SetView(test);
                testRenderer.MainWindow.DispatchEvents();
                testRenderer.MainWindow.Clear(); //перерисовка окна
                system.Process();
                testPlayer.Process(new Vector2f(0, 0));
                testRenderer.RenderProcess(system.GetView());
                
                foreach (ObjectView testingView in testPlayer.View)
                {
                    testRenderer.MainWindow.Draw(testingView.Image, testingView.State);
                }
                testRenderer.MainWindow.Display(); //перерисовка окна
            }
        }
    }
}