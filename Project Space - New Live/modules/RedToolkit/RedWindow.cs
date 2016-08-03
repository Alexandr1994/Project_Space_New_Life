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
        /// Collection of widgets on window
        /// </summary>
        private Dictionary<String, Form> widgetsCollection = new Dictionary<String, Form>();

        /// <summary>
        /// Adding widget on Red Window
        /// </summary>
        /// <param name="widget">Widget</param>
        /// <param name="name">Name of widget</param>
        public void AddWidget(Form widget, String name)
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
        public Form GetWidget(String name)
        {
            if (this.widgetsCollection.ContainsKey(name))
            {
                return this.widgetsCollection[name];
            }
            return null;
        }

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
            this.windowThread = new Thread(this.Process);//starting window process
            this.windowThread.Start();
        }

        /// <summary>
        /// Creating custom window
        /// </summary>
        /// <returns>Render Window</returns>
        protected virtual void CustomWindowcreate()
        {
            this.window = new RenderWindow(new VideoMode(300, 300), this.Title, Styles.Default);
            this.window.SetIcon(icon.Size.X, icon.Size.Y, icon.Pixels);
            //setting basic handlers
            this.window.Closed += this.Closing;
            this.window.Resized += this.Resizing;
        }

        /// <summary>
        /// Process function of Red Window
        /// </summary>
        private void Process()
        {
            this.CustomWindowcreate();//creating window
            while (this.window.IsOpen)
            {
                Thread.Sleep(sleepTime);
                this.RefreshWindow();
                if (this.window.HasFocus())
                {
                    this.window.DispatchEvents();
                }
            }
        }

        /// <summary>
        /// Refresh Red Window
        /// </summary>
        public void RefreshWindow()
        {
            this.window.Display();
            this.window.Clear();   
            foreach (KeyValuePair<string, Form> widget in this.widgetsCollection)
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

       
        private void Resizing(object sender, SizeEventArgs e)
        {
            View windowView = this.window.GetView();  
            windowView.Size = new Vector2f(e.Width, e.Height);
            windowView.Center = new Vector2f(e.Width, e.Height) / 2;
            this.window.SetView(windowView);
            this.RefreshWindow();
        }

        private void Closing(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
