using System;
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
        private double angSpeed = 3*Math.PI/180;
        private static Texture backText = new Texture("testBackground.png"); //загруженная текстура планет 
        private static Texture img = new Texture("textPlayer.png"); //загруженная текстура планет 
        private RectangleShape player = new RectangleShape(new Vector2f(20, 10));
        private double speed = 3;
        private Texture starText = new Texture("testStarText.jpg"); //загруженная текстура звезды
        private  Texture shadowTexture = new Texture("shadow.png");//тень
        private  Texture crownText = new Texture("crown.png");//звездная корона
        private double[] angle = new double[5]; //орбитальные углы планет
        private double angleS;
        private Vector2f coords = new Vector2f(400, 225); //идеальные координаты
        private bool down;
        private bool left; //флаги контроля перемещения играока
        //Random rand = new Random();

        //CircleShape[] planets = new CircleShape[5];//переменные планет: образы
        private int[] orbits = {600, 140, 300, 200}; //орибиты планет 
        private Texture planetText = new Texture("testPlanetText.jpg"); //загруженная текстура планет 
        private bool right;
        //CircleShape star = new CircleShape();
        private StarSystem system;
        private RenderClass testRenderer;

        //   static VideoMode testMode = new VideoMode(800, 450);//переменные окна: видеорежим
        private RenderWindow testWindow; //окно
        private bool up;

        private void onKey(object sender, KeyEventArgs e) //обработка нажатия на клавишу
        {
            switch (e.Code)
            {
                case Keyboard.Key.Escape:
                {
                    testWindow.Close(); //закрытие окна
                }
                    ;
                    break;
                case Keyboard.Key.Right:
                {
                    right = true;
                }
                    ;
                    break;
                case Keyboard.Key.Left:
                {
                    left = true;
                }
                    ;
                    break;
                case Keyboard.Key.Up:
                {
                    up = true;
                }
                    ;
                    break;
                case Keyboard.Key.Down:
                {
                    down = true;
                }
                    ;
                    break;
                case Keyboard.Key.Return:
                {
//наложение текстуры
                    //star.Texture = new Texture(img);
                    //for (int i = 0; i < planets.Length; i++)
                    //{
                    //    planets[i].Texture = new Texture(img);
                    //}
                }
                    ;
                    break;
                case Keyboard.Key.Space:
                {
//изменение формата отображение окна
                    testWindow = testRenderer.changeWindowStyle(Styles.Default);
                    testWindow.KeyPressed += onKey;
                    testWindow.KeyReleased += fromKey;
                }
                    ;
                    break;
                case Keyboard.Key.RShift:
                {
                    testWindow = testRenderer.changeVideoMode(1024, 768);
                    testWindow.KeyPressed += onKey;
                    testWindow.KeyReleased += fromKey;
                }
                    ;
                    break;
                default:
                {
                    testWindow.SetTitle(e.Code.ToString());
                }
                    ;
                    break;
            }
        }

        private void fromKey(object sender, KeyEventArgs e) //обработка отжатия клавиш
        {
            switch (e.Code)
            {
                case Keyboard.Key.Right:
                {
                    right = false;
                }
                    ;
                    break;
                case Keyboard.Key.Left:
                {
                    left = false;
                }
                    ;
                    break;
                case Keyboard.Key.Up:
                {
                    up = false;
                }
                    ;
                    break;
                case Keyboard.Key.Down:
                {
                    down = false;
                }
                    ;
                    break;
                default:
                {
                    testWindow.SetTitle(e.Code.ToString());
                }
                    ;
                    break;
            }
        }

        private void act() //переодическая функция управления
        {
            float dX = 0;
            float dY = 0;
            RenderStates state = new RenderStates(BlendMode.None);
            Transform rotate = player.Transform;
            if (left) //перемещение
            {
                angleS -= angSpeed;
            }
            if (right)
            {
                angleS += angSpeed;
            }
            if (up)
            {
                dX = (float)(player.Position.X + speed * Math.Cos(angleS));
                dY = (float)(player.Position.Y + speed * Math.Sin(angleS));
                player.Position = new Vector2f(dX, dY);
            }
            if (down)
            {
                dX = (float)(player.Position.X - speed * Math.Cos(angleS));
                dY = (float)(player.Position.Y - speed * Math.Sin(angleS));
                player.Position = new Vector2f(dX, dY);
            }
            coords.X = player.Position.X + player.Size.X / 2;
            coords.Y = player.Position.Y + player.Size.Y / 2;
            rotate.Rotate((float)(angleS * 180 / Math.PI), coords);

            state.Transform = rotate;
            testWindow.Draw(player, state);
        }



        private void planetProcess(CircleShape planet, double orbit, double angle) //вычисление новых координат планеты
        {
            //Vector2f starPoint = star.Position;
            //Vector2f point = new Vector2f();//50 - радиус звезды, 20 
            //point.X = (float)((starPoint.X + star.Radius - planet.Radius) + (orbit * Math.Cos(angle)));
            //point.Y = (float)((starPoint.Y + star.Radius - planet.Radius) + (orbit * Math.Sin(angle)));
            //planet.Position = point;
        }

        private void initSystem() //инициализацимя звездной системы
        {
            Texture[] texts = new Texture[2];


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

            //Star[] stars = new Star[3];
            //for (int i = 0; i < stars.Length - 1; i++)
            //{
            //  stars[i] = new Star(1000, 80, 110, 180 * (i+1) * (Math.PI/180), 0.002, starText);
            //}
            //stars[2] = new Star(1000, 80, 400, new Random().Next(), 0.0005, starText);
            //Planet[] planets = new Planet[3];
            //for(int i = 0; i < planets.Length; i++)
            //{
            //    planets[i] = new Planet(10, 10, orbits[i], 0.1 / (1 + i), planetText);
            //}
            player.Texture = img;
            player.Position = new Vector2f(coords.X - 25, coords.Y - 25);
            player.OutlineColor = Color.Magenta;
            player.OutlineThickness = 10;

            LocalMassCenter[] locCenters = new LocalMassCenter[2];
            texts[0] = starText;
            texts[1] = crownText;
            for (var i = 0; i < locCenters.Length; i++)
            {
                var starsDouble = new Star[2];
                for (var j = 0; j < starsDouble.Length; j++)
                {
                    starsDouble[j] = new Star(1000, 40, 55, 180*(j + 1)*(Math.PI/180), 0.008, texts);
                }
                locCenters[i] = new LocalMassCenter(150, (i + 1)*(Math.PI), 0.004, starsDouble);
            }
 

            texts[0] = planetText;
            texts[1] = shadowTexture;

            Planet[] planets = new Planet[1];
            for (int i = 0; i < planets.Length; i++)
            {
                planets[i] = new Planet(10, 10, orbits[i], 0.1 / (1 + i), texts);
            }
            var center = new LocalMassCenter(0, 0, 0, locCenters);
            system = new StarSystem(new Vector2f(400, 225), center, null, backText);
        }

        public void main()
        {
            testRenderer = RenderClass.getInstance();
            testWindow = testRenderer.getMainWindow();


      //      initSystem();
            testWindow.KeyPressed += onKey;
            testWindow.KeyReleased += fromKey;


            //пока окно открыто ловить события и перерисовывать окно
            while (testWindow.IsOpen)
            {
                Thread.Sleep(25);
                testWindow.DispatchEvents();
                testWindow.Clear(); //перерисовка окна
        //        system.Process();
         //       testRenderer.RenderProcess(system.GetView());
                act();
                testWindow.Display(); //перерисовка окна
            }
        }
    }
}