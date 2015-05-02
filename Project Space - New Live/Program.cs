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
using Project_Space___New_Live.modules.GameObjects;


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


        Texture starText = new Texture("D:/testStarText.jpg");//загруженная текстура звезды
        Texture planetText = new Texture("D:/testPlanetText.jpg");//загруженная текстура планет 
        Texture backText = new Texture("D:/testBackground.png");//загруженная текстура планет 

        static VideoMode testMode = new VideoMode(800, 600);//переменные окна: видеорежим
        RenderWindow testWindow = new RenderWindow(testMode, "Test");//окно
        //CircleShape star = new CircleShape();
        StarSystem system;

        //Random rand = new Random();

        //CircleShape[] planets = new CircleShape[5];//переменные планет: образы
        int[] orbits = { 220, 280, 600, 400 };//орибиты планет 
        double[] angle = new double[5];//орбитальные углы планет
        
        bool left = false;//флаги контроля перемещения играока
        bool right = false;
        bool up = false;
        bool down = false;
        

        private void onKey(object sender,KeyEventArgs e)//обработка нажатия на клавишу
        {
            switch (e.Code)
            {
                case Keyboard.Key.Escape:
                {
                    testWindow.Close();//закрытие окна
                };break;
                case Keyboard.Key.Right:
                {
                    right = true;
                }; break;
                case Keyboard.Key.Left:
                    {
                        left = true;
                    }; break;
                case Keyboard.Key.Up:
                    {
                        up = true;
                    }; break;
                case Keyboard.Key.Down:
                    {
                        down = true;
                    }; break;
                case Keyboard.Key.Return:
                    {//наложение текстуры
                        //star.Texture = new Texture(img);
                        //for (int i = 0; i < planets.Length; i++)
                        //{
                        //    planets[i].Texture = new Texture(img);
                        //}
                    }; break;
                case Keyboard.Key.Space:
                    {//изменение формата отображение окна
                        testWindow.Close();
                        testWindow = new RenderWindow(testMode, "LOL!", Styles.Fullscreen);
                        testWindow.KeyPressed += onKey;
                        testWindow.KeyReleased += fromKey;

                    }; break;
                default:
                {
                    testWindow.SetTitle(e.Code.ToString());
                }; break;

            }
        }

        private void fromKey(object sender, KeyEventArgs e)//обработка отжатия клавиш
        {
            switch (e.Code)
            {
                case Keyboard.Key.Right:
                    {
                        right = false;
                    }; break;
                case Keyboard.Key.Left:
                    {
                        left = false;
                    }; break;
                case Keyboard.Key.Up:
                    {
                        up = false;
                    }; break;
                case Keyboard.Key.Down:
                    {
                        down = false;
                    }; break;
                default:
                    { 
                        testWindow.SetTitle(e.Code.ToString());
                    }; break;

            }
        }


        private void act()//переодическая функция управления
        {
            if (left)//перемещение
            {
                system.move(-3, 0);
            }
            if (right)
            {
                system.move(3, 0);
            }
            if (up)
            {
                system.move(0, -3);
            }
            if (down)
            {
                system.move(0, 3);
            }
        }

        private void systema()//переодическая функция окружения
        {
            
            act();
            //testWindow.Draw(star);//отрисовка звезды
            //for (int i = 0; i < planets.Length; i++)//отрисовка планет и приращение орбитальных углов планет
            //{
            //    angle[i] += 0.05 / (i + 1);//приращение орбитального угла
            //    planetProcess(planets[i], orbits[i], angle[i]);//вычисление новых координат планеты
            //    testWindow.Draw(planets[i]);//переотрисовка планет
            //}
            system.systemProcess(testWindow);

        }

        private void planetProcess(CircleShape planet, double orbit, double angle)//вычисление новых координат планеты
        {
            //Vector2f starPoint = star.Position;
            //Vector2f point = new Vector2f();//50 - радиус звезды, 20 
            //point.X = (float)((starPoint.X + star.Radius - planet.Radius) + (orbit * Math.Cos(angle)));
            //point.Y = (float)((starPoint.Y + star.Radius - planet.Radius) + (orbit * Math.Sin(angle)));
            //planet.Position = point;
        }


        private void initSystem()//инициализацимя звездной системы
        {
            //Color[] colors = { Color.Red, Color.Green, Color.Blue, Color.Magenta, Color.Cyan};
            //star.Position = new Vector2f(100, 100);//инициализация звезды
            //star.Radius = 50;
            //star.OutlineColor = Color.Yellow;
            //star.FillColor = Color.Yellow;
            //for (int i = 0; i < planets.Length; i++)//инициализация планет
            //{
            //    angle[i] = rand.Next();
            //    Vector2f locPos = star.Position;
            //    locPos.X += (float)(orbits[i] + 50 + 10);
            //    locPos.Y += (float)(50 - 10);
            //    planets[i] = new CircleShape();
            //    planets[i].Position = locPos;
            //    planets[i].Radius = 20;
            //    planets[i].FillColor = colors[i];
            //}

            Star[] stars = new Star[3];
            for (int i = 0; i < stars.Length - 1; i++)
            {
              stars[i] = new Star(1000, 80, 110, 180 * (i+1) * (Math.PI/180), 0.002, starText);
            }
            stars[2] = new Star(1000, 80, 400, new Random().Next(), 0.0005, starText);
            Planet[] planets = new Planet[3];
            for(int i = 0; i < planets.Length; i++)
            {
                planets[i] = new Planet(10, 10, orbits[i], 0.1 / (1 + i), planetText);
            }
            system = new StarSystem(new Vector2f(400, 300), stars, planets, backText);

        }

        public void main()
        {
            
            initSystem();
            testWindow.KeyPressed += onKey;
            testWindow.KeyReleased += fromKey;

            //пока окно открыто ловить события и перерисовывать окно
            while(testWindow.IsOpen)
            {
                Thread.Sleep(25);
                testWindow.DispatchEvents();
                testWindow.Clear();//перерисовка окна
                systema();
                testWindow.Display();//перерисовка окна
             
            }
            
        }



    }
}

