using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live.modules.Forms
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
        /// Window sizes
        /// </summary>
        private Vector2u size = new Vector2u(300, 300);

        /// <summary>
        /// Window sizes
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
        /// Process function of Red Window
        /// </summary>
        private void Process()
        {
            this.window = new RenderWindow(new VideoMode(300, 300), this.Title, Styles.Close);//creating window
            while (this.window.IsOpen)
            {
                Thread.Sleep(sleepTime);
                this.window.Display();
                foreach (KeyValuePair<string, Form> widget in this.widgetsCollection)
                {
                    foreach (RenderView view in widget.Value.GetFormView(this))
                    {
                        this.window.Draw(view.View as Drawable, view.State);
                    }
                }
                if (this.window.HasFocus())
                {
                    this.window.DispatchEvents();
                }
            }
        }

    }
}
