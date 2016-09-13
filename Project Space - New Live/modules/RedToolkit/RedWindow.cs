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
        /// <summary>
        /// Sleep time of window thread
        /// </summary>
        const int sleepTime = 30;

        /// <summary>
        /// Window thread
        /// </summary>
        private Thread windowThread;

        /// <summary>
        /// Window object 
        /// </summary>
        private RenderWindow window;

        /// <summary>
        /// Red Window background
        /// </summary>
        private ImageView background;

        /// <summary>
        /// Background color of Red Window
        /// </summary>
        public Color BackgroundColor
        {
            get { return this.background.Image.FillColor; }
            set { this.background.Image.FillColor = value; }
        }

        /// <summary>
        /// Background image of Red Window
        /// </summary>
        public Texture BackgroundImage
        {
            get { return this.background.Image.Texture; }
            set { this.background.Image.Texture = value;}
        }


        /// <summary>
        /// Check having focus of Red Window 
        /// </summary>
        public bool Focused
        {
            get { return this.window.HasFocus(); }
        }

        /// <summary>
        /// Red Window sizes
        /// </summary>
        private Vector2u size = new Vector2u(300, 300);

        /// <summary>
        /// Red Window sizes
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
        /// Red Window location
        /// </summary>
        public Vector2i Location
        {
            get { return this.window.Position; }
            set { this.window.Position = value; }
        }
       
        /// <summary>
        /// Window title
        /// </summary>
        private String title = "Red Window";

        /// <summary>
        /// Window title
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
        /// Red Window icon
        /// </summary>
        private Image icon = new Image("Resources/RedTheme/RedIcon.png");

        /// <summary>
        /// Red Window Icon
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

        /// <summary>
        /// Event of changing position of Red Window view
        /// </summary>
        public event EventHandler<ViewEventArgs> ViewChanged = null;

        /// <summary>
        /// Mouse in event
        /// </summary>
        public event EventHandler<EventArgs> MouseIn = null;

        /// <summary>
        /// Mouse out event
        /// </summary>
        public event EventHandler<EventArgs> MouseOut = null;

        /// <summary>
        /// Mouse move event
        /// </summary>
        public event EventHandler<MouseMoveEventArgs> MouseMove = null;

        /// <summary>
        /// Mouse button release event
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseUp = null;

        /// <summary>
        /// Mouse button press event 
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseDown = null;

        /// <summary>
        /// Mouse Click event
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseClick = null;

        /// <summary>
        /// Collection of widgets on window
        /// </summary>
        private Dictionary<String, RedWidget> widgetsCollection = new Dictionary<String, RedWidget>();

        /// <summary>
        /// Adding widget on Red Window
        /// </summary>
        /// <param name="widget">Widget</param>
        /// <param name="name">Name of widget</param>
        public void AddWidget(RedWidget widget, String name)
        {
            this.widgetsCollection.Add(name, widget);
        }
        
        /// <summary>
        /// Removing widget from Red Window
        /// </summary>
        /// <param name="name">Name of widget</param>
        /// <returns>Result of action</returns>
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
        /// Getting widget on Red Window
        /// </summary>
        /// <param name="name">Name of Widget</param>
        /// <returns>Widget or null, if collection don't has widget with that name</returns>
        public RedWidget GetWidget(String name)
        {
            if (this.widgetsCollection.ContainsKey(name))
            {
                return this.widgetsCollection[name];
            }
            return null;
        }

        /// <summary>
        /// Red Window start mutex
        /// </summary>
        private Mutex startMutex;

        /// <summary>
        /// Getting basic window 
        /// </summary>
        /// <returns></returns>
        internal Window GetWindow()
        {
            return this.window;
        }

        /// <summary>
        /// Show Red window
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
        /// Creating custom window
        /// </summary>
        /// <returns>Render Window</returns>
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
        /// Process function of Red Window
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
        /// Refresh Red Window
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
        /// Close Red Window
        /// </summary>
        public void Close()
        {
            this.window.Close();
        }

        /// <summary>
        /// Resizing of Red Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event arguments</param>
        private void Resizing(object sender, SizeEventArgs e)
        {
            View windowView = this.window.GetView();  
            windowView.Size = new Vector2f(e.Width, e.Height);
            windowView.Center = new Vector2f(e.Width, e.Height) / 2;
            this.window.SetView(windowView);
            (this.background.Image as RectangleShape).Size = windowView.Size;
        }

        /// <summary>
        /// Changing of Red Window's view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewChanging(object sender, ViewEventArgs e)
        {
            foreach (KeyValuePair<string, RedWidget> widget in this.widgetsCollection)
            {
                widget.Value.WidgetCorrection(e.Offset, e.Angle);
            }
        }


        private void Closing(object sender, EventArgs e)
        {
            this.Close();
        }

        //RED WINDOW EVENTS CATCHER

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

        /// <summary>
        /// RedWindow view moving 
        /// </summary>
        /// <param name="offset">Moving offset</param>
        public void MoveView(Vector2f offset)
        {
            View view = this.window.GetView();
            view.Center += offset;
            this.window.SetView(view);
            if (this.ViewChanged != null)
            {
                this.ViewChanged(this, new ViewEventArgs(offset, 0));
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
        /// View changing event args
        /// </summary>
        /// <param name="offset">Moving offset</param>
        /// <param name="angle">Rotation angle</param>
        public ViewEventArgs(Vector2f offset, float angle)
        {
            this.Offset = offset;
            this.Angle = angle;
        }

    }
}
