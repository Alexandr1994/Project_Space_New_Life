using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Project_Space___New_Live.modules;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace RedToolkit
{

    public class RedWindow
    {
        //RED WINDOW PARAMETRS AND PROPERTIES
        //ПАРАМЕТРЫ И СВОЙСТВА RED WINDOW

        /// <summary>
        /// <para>Sleep time of window thread
        /// <para>Время ожидания нити(потока) окна
        /// </summary>
        const int sleepTime = 30;

        /// <summary>
        /// <para>Window thread
        /// <para>Нить(поток) окна
        /// </summary>
        private Thread windowThread;

        /// <summary>
        /// <para>SFML Window
        /// <para>Окно SFML
        /// </summary>
        private RenderWindow window;

        /// <summary>
        /// <para>Red Window start mutex
        /// <para>Стартовый мьютекс Red Window
        /// </summary>
        private Mutex startMutex;

        /// <summary>
        /// <para>Red Window background
        /// <para>Фон окна Red Window
        /// </summary>
        private ImageView background;

        /// <summary>
        /// <para>Background color of Red Window
        /// <para>Фоновая заливка окна Red Window
        /// </summary>
        public Color BackgroundColor
        {
            get { return this.background.Image.FillColor; }
            set { this.background.Image.FillColor = value; }
        }

        /// <summary>
        /// Background image of Red Window
        /// <para>Фоновый рисунок Red Window
        /// </summary>
        public Texture BackgroundImage
        {
            get { return this.background.Image.Texture; }
            set { this.background.Image.Texture = value;}
        }


        /// <summary>
        /// <para>Check having focus of Red Window
        /// <para>Флаг фокуса окна Red Window 
        /// </summary>
        public bool Focused
        {
            get { return this.window.HasFocus(); }
        }

        /// <summary>
        /// <para>Red Window sizes
        /// <para>Размер окна<para>
        /// </summary>
        private Vector2u size = new Vector2u(300, 300);

        /// <summary>
        /// <para>Red Window sizes
        /// <para>Размер окна Red Window
        /// </summary>
        public Vector2u Size
        {
            get { return this.size; }
            set
            {
                this.size = value;
                this.window.Size = this.size;
            }
        }

        /// <summary>
        /// <para>Red Window location
        /// <para>Положение окна Red Window
        /// </summary>
        public Vector2i Location
        {
            get { return this.window.Position; }
            set { this.window.Position = value; }
        }
       
        /// <summary>
        /// <para>Window title
        /// <para>Заголовок окна Red Window
        /// </summary>
        private String title = "Red Window";

        /// <summary>
        /// <para>Window title
        /// <para>Заголовок окна Red Window
        /// </summary>
        public String Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                this.window.SetTitle(this.title);
            }
        }

        /// <summary>
        /// <para>Red Window icon
        /// <para> Иконка Red Window
        /// </summary>
        private Image icon = new Image("Resources/RedTheme/RedIcon.png");

        /// <summary>
        /// <para>Red Window icon
        /// <para>Иконка Red Window
        /// </summary>
        public Image Icon
        {
            get { return this.icon; }
            set
            {
                this.icon = value;
                if (this.window != null)
                {
                    this.window.SetIcon(icon.Size.X, icon.Size.Y, icon.Pixels);
                }
            }
        }

        //RED WINDOW EVENTS
        //СОБЫТИЯ ОКНА RED WINDOW

        /// <summary>
        /// <para>Event of changing position or rotation of Red Window View
        /// <para>Событие изменения позиции или поворота вида окна Red Window
        /// </summary>
        public event EventHandler<ViewEventArgs> ViewChanged = null;

        //Red Window mouse events
        //события мыши окна Red Window

        //Red Window mouse move events
        //События движения мыши окна Red Window

        /// <summary>
        /// <para>Event of cursor entering in window region 
        /// <para>Событие входа курсора в область окна
        /// </summary>
        public event EventHandler<EventArgs> MouseIn = null;

        /// <summary>
        /// <para>Event of cursor leaving of window region 
        /// <para>Событие выхода курсора из область окна
        /// </summary>
        public event EventHandler<EventArgs> MouseOut = null;

        /// <summary>
        /// <para>Event of cursor moving in window region 
        /// <para>Событие движения курсора в области окна
        /// </summary>
        public event EventHandler<MouseMoveEventArgs> MouseMove = null;

        //Red Window mouse button events
        //События кнопок мыши окна Red Window

        /// <summary>
        /// <para>Event of mouse button release 
        /// <para>Событие отжатия кнопки мыши
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseUp = null;

        /// <summary>
        /// <para>Event of mouse button press
        /// <para>Событие нажатия кнопки мыши
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseDown = null;

        /// <summary>
        /// <para>Event of click 
        /// <para>Событие клика
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseClick = null;

        /// <summary>
        /// <para>Collection of widgets on window
        /// <para>Коллекция виджетов окна
        /// </summary>
        private Dictionary<String, RedWidget> widgetsCollection = new Dictionary<String, RedWidget>();

        //RED WINDOW METHODS
        //МЕТОДЫ RED WINDOW

        /// <summary>
        /// <para>Adding widget on Red Window
        /// <para>Добавление виджета на окно Red Window
        /// </summary>
        /// <param name="widget"><para>Widget<para>Виджет</param>
        /// <param name="name"><para>Name of widget<para>Имя виджета</param>
        public void AddWidget(RedWidget widget, String name)
        {
            this.widgetsCollection.Add(name, widget);
        }
        
        /// <summary>
        /// <para>Removing widget from Red Window
        /// <para>Удаление виджета с окна Red Window
        /// </summary>
        /// <param name="name"><para>Name of widget<para>Имя виджета</param>
        /// <returns><para>Result of action<para>Результат операции</returns>
        public bool RemoveWidget(String name)
        {
            if (this.widgetsCollection.ContainsKey(name))
            {
                this.widgetsCollection.Remove(name);
                return true;
            }
            return false;
        }

        /// <summary>
        /// <para>Getting widget on Red Window
        /// <para>Получение виджета на окне Red Window
        /// </summary>
        /// <param name="name"><para>Name of Widget<para>Имя виджета</param>
        /// <returns><para>Widget or null, if collection don't has widget with that name<para>Виджет или null, если вт колекции нет виджета с указанным именем</returns>
        public RedWidget GetWidget(String name)
        {
            if (this.widgetsCollection.ContainsKey(name))
            {
                return this.widgetsCollection[name];
            }
            return null;
        }



        /// <summary>
        /// <para>Getting basic window
        /// <para>Получение базового окна
        /// </summary>
        /// <returns>SFML-окно</returns>
        internal Window GetWindow()
        {
            return this.window;
        }

        /// <summary>
        /// <para>Show Red Window
        /// <para>Запустить Red Window
        /// </summary>
        public void Start()
        {
            this.startMutex = new Mutex();
            this.windowThread = new Thread(this.Process);//starting window process
            this.windowThread.Start();
            Thread.Sleep(40);
            this.startMutex.WaitOne();
            this.window.RequestFocus();
            this.startMutex.ReleaseMutex();
        }

        /// <summary>
        /// <para>Creating custom window
        /// <para>Создание окна
        /// </summary>
        /// <returns><para>Render Window<para>SFML-окно</returns>
        protected virtual void CustomWindowcreate()
        {
            this.window = new RenderWindow(new VideoMode(300, 300), this.Title, Styles.Default);
            this.background = new ImageView(new RectangleShape(new Vector2f(300, 300)), BlendMode.Add);
            this.window.SetIcon(icon.Size.X, icon.Size.Y, icon.Pixels);
            //setting basic handlers
            this.window.Closed += this.Closing;
            this.window.Resized += this.Resizing;
            this.window.MouseButtonPressed += this.MouseDownCatcher;
            this.window.MouseButtonReleased += this.MouseUpCatcher;
            this.window.MouseMoved += this.MouseMoveCatcher;
            this.window.MouseEntered += this.MouseInCather;
            this.window.MouseLeft += this.MouseOutCather;
            this.ViewChanged += this.ViewChanging;
        }

        /// <summary>
        /// <para>Process function of Red Window
        /// <para>Процессный метод Red Window
        /// </summary>
        private void Process()
        {
            this.startMutex.WaitOne();
            this.CustomWindowcreate();//creating window
            this.startMutex.ReleaseMutex();
            while (this.window.IsOpen)
            {
                Thread.Sleep(sleepTime);
                this.window.DispatchEvents();
                this.RefreshWindow();
            }
        }

        /// <summary>
        /// <para>Refresh Red Window
        /// <para>Перерисовка окна Red Window
        /// </summary>
        public void RefreshWindow()
        {
            this.window.Display();
            this.window.Clear();  
            this.window.Draw(this.background.Image);
            foreach (KeyValuePair<string, RedWidget> widget in this.widgetsCollection)
            {
                foreach (RenderView view in widget.Value.GetFormView(this))
                {
                    this.window.Draw(view.View as Drawable, view.State);
                }
            }
        }

        /// <summary>
        /// <para>Red Window view moving <
        /// <para>Перемещение вида окна Red Window
        /// </summary>
        /// <param name="offset"><para>Moving offset<para>Сдвиг</param>
        public void MoveView(Vector2f offset)
        {
            View view = this.window.GetView();
            view.Center += offset;
            this.window.SetView(view);
            if (this.ViewChanged != null)
            {
                this.ViewChanged(this, new ViewEventArgs(offset, 0, view.Center));
            }
        }

        /// <summary>
        /// <para>Red Window view rotation
        /// <para>Поворот вида окна Red Window
        /// </summary>
        /// <param name="angle"><para>Rotation angle (in deg)<para>Угол поворота (в градусах)</param>
        public void RotateView(float angle)
        {
            View view = this.window.GetView();
            view.Rotation += angle;
            this.window.SetView(view);
            if (this.ViewChanged != null)
            {
                this.ViewChanged(this, new ViewEventArgs(new Vector2f(0, 0), angle, view.Center));
            }
        }

        /// <summary>
        /// <para>Resizing of Red Window
        /// <para>Изменение размеров окна Red Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Resizing(object sender, SizeEventArgs e)
        {
            View windowView = this.window.GetView();  
            windowView.Size = new Vector2f(e.Width, e.Height);
            windowView.Center = new Vector2f(e.Width, e.Height) / 2;
            this.window.SetView(windowView);
            (this.background.Image as RectangleShape).Size = windowView.Size;
        }

        /// <summary>
        /// <para>Changing of Red Window's view
        /// <para>Изменение вида окна Red Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewChanging(object sender, ViewEventArgs e)
        {
            foreach (KeyValuePair<string, RedWidget> widget in this.widgetsCollection)
            {
                widget.Value.WidgetCorrection(e.Offset, e.Angle, e.Center);
            }
        }

        /// <summary>
        /// <para>Closing Red Window
        /// <para>Закрытие окна Red Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Closing(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// <para>Close Red Window
        /// <para>Закрытие окна Red Window
        /// </summary>
        public void Close()
        {
            this.window.Close();
        }

        //RED WINDOW EVENTS CATCHER
        //ОПРЕДЕЛЕНИЕ СОБЫТИЙ RED WINDOW 

        private void MouseDownCatcher(object sender, MouseButtonEventArgs e)
        {
            if (this.MouseDown != null)
            {
                this.MouseDown(this, e);
            }
        }

        private void MouseUpCatcher(object sender, MouseButtonEventArgs e)
        {
            if (this.MouseUp != null)
            {
                this.MouseUp(this, e);
            }
        }

        private void MouseMoveCatcher(object sender, MouseMoveEventArgs e)
        {
            if (this.MouseDown != null)
            {
                this.MouseMove(this, e);
            }
        }

        private void MouseInCather(object sender, EventArgs e)
        {
            if (this.MouseIn != null)
            {
                this.MouseIn(this, e);
            }
        }

        private void MouseOutCather(object sender, EventArgs e)
        {
            if (this.MouseDown != null)
            {
                this.MouseOut(this, e);
            }
        }
    }

    /// <summary>
    /// View-changig event args 
    /// </summary>
    public class ViewEventArgs : EventArgs
    {
        /// <summary>
        /// View-moving offset
        /// </summary>
        public Vector2f Offset;

        /// <summary>
        /// View-rotation angle
        /// </summary>
        public float Angle;

        /// <summary>
        /// View center
        /// </summary>
        public Vector2f Center;

        /// <summary>
        /// View changing event args
        /// </summary>
        /// <param name="offset">Moving offset</param>
        /// <param name="angle">Rotation angle</param>
        /// <param name="center">View center</param>
        public ViewEventArgs(Vector2f offset, float angle, Vector2f center)
        {
            this.Offset = offset;
            this.Angle = angle;
            this.Center = center;
        }

    }
}
