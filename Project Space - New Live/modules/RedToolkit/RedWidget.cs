using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Project_Space___New_Live.modules;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Project_Space___New_Live.modules.Dispatchers;

namespace RedToolkit
{
    /// <summary>
    /// Basic RedToolkit widget
    /// <para></para>
    /// Базовый виджет RedToolkit
    /// </summary>
    public abstract class RedWidget
    {

        //RED WIDGET PARAMETRS AND PROPERTIES
        //ПАРАМЕТРЫ И СВОЙСТВА RED WIDGET

        /// <summary>
        /// Image of widget
        /// <para></para>
        /// Отображение виджета
        /// </summary>
        protected abstract RenderView View { get; }

        /// <summary>
        /// Widget location
        /// <para></para>
        /// Положение виджета
        /// </summary>
        protected Vector2f location;

        /// <summary>
        /// Widget location
        /// <para></para>
        /// Положение виджета
        /// </summary>
        public virtual Vector2f Location
        {
            get { return this.location; }
            set
            {
                this.location = value;
            }
        }

        /// <summary>
        /// Red Window view offset
        /// <para></para>
        /// Сдвиг вида окна RedWindow
        /// </summary>
        private Vector2f viewOffset = new Vector2f();

        /// <summary>
        /// Widget size
        /// <para></para>
        /// Размер виджета
        /// </summary>
        protected Vector2f size;

        /// <summary>
        /// Widget size
        /// <para></para>
        /// Размер виджета
        /// </summary>
        public abstract Vector2f Size { get; set; }

        /// <summary>
        /// Pointer on parent widget
        /// <para></para>
        /// Указатель на родительский виджет
        /// </summary>
        protected RedWidget ParentRedWidget;

        /// <summary>
        /// Collection of child widgets
        /// <para></para>
        /// Коллекция дочерних виджетов
        /// </summary>
        private List<RedWidget> childWidgetsCollections = new List<RedWidget>();

        /// <summary>
        /// Collection of child widgets
        /// <para></para>
        /// Коллекция дочерних виджетов
        /// </summary>
        protected List<RedWidget> ChildWidgetsCollections
        {
            get { return childWidgetsCollections; }
        }

        /// <summary>
        /// Count of press events
        /// <para></para>
        /// Счетчик нажатий на левую кнопку мыши
        /// </summary>
        protected int leftMousePressCount = 0;

        /// <summary>
        /// Count of release events
        /// <para></para>
        /// Счетчик отжатий левой кнопки мыши
        /// </summary>
        protected int leftMouseReleaseCount = 0;

        //Red Widget flags
        //Флаги Red Widget

        /// <summary>
        /// Flag of visibility of widget
        /// <para></para>
        /// Флаг видимости виджета 
        /// </summary>
        private bool visible;

        /// <summary>
        /// Flag of visibility of widget
        /// <para></para>
        /// Флаг видимости виджета 
        /// </summary>
        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        /// <summary>
        /// Flag of click
        /// <para></para>
        /// Флаг клика
        /// </summary>
        protected bool click = false;

        /// <summary>
        /// Flag of being cursor in region of widget
        /// <para></para>
        /// Флаг нахождения курсора в области виджета
        /// </summary>
        protected bool cursorOnForm = false;

        /// <summary>
        /// Flag of holding left button of mouse
        /// Флаг удержания левой кнопки мыши
        /// </summary>
        protected bool buttonPresed = false;

        /// <summary>
        /// Flag of binding to right border of parent widget or window
        /// <para></para>
        /// Флаг привязки к правой границе родительского виджета или окна
        /// </summary>
        private bool alignRight = false;

        /// <summary>
        /// Flag of binding to right border of parent widget or window
        /// <para></para>
        /// Флаг привязки к правой границе родительского виджета или окна
        /// </summary>
        public bool AlignRight
        {
            get { return alignRight; }
            set { alignRight = value; }
        }

        /// <summary>
        /// Flag of binding to left border of parent widget or window
        /// <para></para>
        /// Флаг привязки к левой границе родительского виджета или окна
        /// </summary>
        private bool aliginLeft = false;

        /// <summary>
        /// Flag of binding to left border of parent widget or window
        /// <para></para>
        /// Флаг привязки к левой границе родительского виджета или окна
        /// </summary>
        public bool AliginLeft
        {
            get { return aliginLeft; }
            set { aliginLeft = value; }
        }

        /// <summary>
        /// Flag of binding to top border of parent widget or window
        /// <para></para>
        /// Флаг привязки к верхней границе родительского виджета или окна
        /// </summary>
        private bool alignUp = false;

        /// <summary>
        /// Flag of binding to top border of parent widget or window
        /// <para></para>
        /// Флаг привязки к верхней границе родительского виджета или окна
        /// </summary>
        public bool AlignUp
        {
            get { return alignUp; }
            set { alignUp = value; }
        }

        /// <summary>
        /// Flag of binding to bottom border of parent widget or window
        /// <para></para>
        /// Флаг привязки к нижней границе родительского виджета или окна
        /// </summary>
        private bool alignDown = false;

        /// <summary>
        /// Flag of binding to bottom border of parent widget or window
        /// <para></para>
        /// Флаг привязки к нижней границе родительского виджета или окна
        /// </summary>
        public bool AlignDown
        {
            get { return alignDown; }
            set { alignDown = value; }
        }

        /// <summary>
        /// X-coordinate of left border of widget
        /// <para></para>
        /// Х-координата левой границы виджета
        /// </summary>
        public float BorderLeft
        {
            get { return this.GetPosition().X; }
        }

        /// <summary>
        /// X-coordinate of right border of widget
        /// <para></para>
        /// Х-координата правой границы виджета
        /// </summary>
        public float BorderRight
        {
            get { return this.BorderLeft + this.Size.X; }
        }

        /// <summary>
        /// Y-coordinate of top border of widget
        /// <para></para>
        /// Y-координата вержней границы виджета
        /// </summary>
        public float BorderTop
        {
            get { return this.GetPosition().Y; }
        }

        /// <summary>
        /// Y-coordinate of bottom border of widget
        /// <para></para>
        /// Y-координата нижней границы виджета
        /// </summary>
        public float BorderBottom
        {
            get { return this.BorderTop + this.Size.Y; }
        }

        //EVENTS OF RED WIDGET
        //СОБЫТИЯ RED WIDGET

        //Red Widget mouse events
        //события мыши Red Widget

        //Red Widget mouse move events
        //События движения мыши Red Widget

        /// <summary>
        /// Event of cursor entering in widget region
        /// <para></para> 
        /// Событие входа курсора в область виджета
        /// </summary>
        public event EventHandler<MouseMoveEventArgs> MouseIn = null;

        /// <summary>
        /// Event of cursor leaving of widget region
        /// <para></para> 
        /// Событие выхода курсора из область виджета
        /// </summary>
        public event EventHandler<MouseMoveEventArgs> MouseOut = null;

        /// <summary>
        /// Event of cursor moving in widget region
        /// <para></para> 
        /// Событие движения курсора в области виджета
        /// </summary>
        public event EventHandler<MouseMoveEventArgs> MouseMove = null;

        //Red Widget mouse button events
        //События кнопок мыши Red Widget

        /// <summary>
        /// Event of mouse button release
        /// <para></para>
        /// Событие отжатия кнопки мыши
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseUp = null;

        /// <summary>
        /// Event of mouse button press
        /// <para></para>
        /// Событие нажатия кнопки мыши
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseDown = null;

        /// <summary>
        /// Event of click
        /// <para></para> 
        /// Событие клика
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseClick = null;

        //Red Widget keyboard events
        //События клавиатуры Red Widget

        /// <summary>
        /// Event of key press
        /// <para></para>
        /// Событие нажатия на клавишу
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyDown = null;

        /// <summary>
        /// Event of key released
        /// <para></para>
        /// Событие отжатия клавиши
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyUp = null;

        //RED WIDGET METHODS
        //МЕТОДЫ RED WIDGET

        public RedWidget()
        {
            this.SetBasicReactions();//установка базовых слушателей формы
            this.CustomConstructor();//установка базовых характеристик формы 
            this.Visible = true;
        }

        protected abstract void CustomConstructor();

        /// <summary>
        /// Setting parent widget
        /// <para></para>
        /// Установка родителского виджета
        /// </summary>
        /// <param name="ParentRedWidget">Parent widget / Родительский виджет</param>
        protected void SetPatentWidget(RedWidget ParentRedWidget)
        {
            this.ParentRedWidget = ParentRedWidget;
        }

        /// <summary>
        /// Adding child widget
        /// <para></para>
        /// Добавление дочернего виджета
        /// </summary>
        /// <param name="newRedWidget">New child widget / Новая дочерняя форма</param>
        public void AddWidget(RedWidget newRedWidget)
        {
            this.childWidgetsCollections.Add(newRedWidget);
            newRedWidget.SetPatentWidget(this);
        }

        /// <summary>
        /// Removing child widget
        /// <para></para>
        /// Удаление дочернего виджета
        /// </summary>
        /// <param name="targetRedWidget">Widget to removing / Виджет к удалению</param>
        public void RemoveWinget(RedWidget targetRedWidget)
        {
            if (this.childWidgetsCollections.IndexOf(targetRedWidget) > 0)
            {
                this.childWidgetsCollections.Remove(targetRedWidget);
            }
        }

        /// <summary>
        /// Removing child widget by id
        /// <para></para>
        /// Удаление дочернего виджета по Id
        /// </summary>
        /// <param name="targetRedWidgetid">Id of widget to removing / Id виджета к удалению</param>
        public void RemoveWingetById(int targetRedWidgetId)
        {
            if (this.childWidgetsCollections.Count >= targetRedWidgetId - 1)
            {
                this.childWidgetsCollections.RemoveAt(targetRedWidgetId);
            }
        }

        /// <summary>
        /// Getting view of current and all child widgets
        /// <para></para>
        /// Получить отображения текущего и всех дочерних виджетов
        /// </summary>
        /// <returns>Collection of widgets views / Коллекция отображений виджетов</returns>
        public List<RenderView> GetWidgetView(RedWindow window)
        {
            List<RenderView> retValue = new List<RenderView>();
            this.View.View.Position = this.GetPosition();//коррекция отображения;
            this.WidgetViewCorrection();
            if (this.Visible)//если форма видимая
            {
                retValue.Add(this.View); //добавление отображени в массив
                foreach (RedWidget childForm in this.ChildWidgetsCollections)//добавление в массив возвращаемых значений дочерних отображений фолрм
                {

                    foreach (RenderView locView in childForm.GetWidgetView(window))//получаем отображение данной дочерней формы
                    {
                        retValue.Add(locView);
                    }
                }
                if (window.Focused)
                {
                    this.CatchEvents(window); //обнаружение событий данной формы
                }
            }
            return retValue;
        }

        /// <summary>
        /// Cathcing events
        /// <para></para>
        /// Анализ возникновения событий
        /// </summary>
        /// <param name="window">Window / Окно</param>
        internal void CatchEvents(RedWindow window)
        {
            if (this.MoveTest(window))//если курсор находится на форме
            {
                if (this.MouseMove != null)//в независимости от предыдущего нахождения курсора
                {//при условии что есть обработчик события
                    this.MouseMove(this, new MouseMoveEventArgs(new MouseMoveEvent()));
                }
                if (!this.cursorOnForm)//и до этого он не находился на ней
                {
                    if (this.MouseIn != null)
                    {//при условии что есть обработчик события
                        this.MouseIn(this, new MouseMoveEventArgs(new MouseMoveEvent()));//то возникает событие MouseIn 
                    }  
                }
                if (Mouse.IsButtonPressed(Mouse.Button.Left))//если левая кнопка мыши зажата
                {
                    if (this.MouseDown != null)
                    {//при условии что есть обработчик события
                        this.MouseDown(this, new MouseButtonEventArgs(new MouseButtonEvent()));//то возникает событие MouseDown
                    }
                }
                if (!Mouse.IsButtonPressed(Mouse.Button.Left))//если левая кнопка мыши отжата
                {
                    if (this.buttonPresed)//если кнопка была зажата
                    {
                        if (this.MouseUp != null)
                        {//при условии что есть обработчик события
                            this.MouseUp(this, new MouseButtonEventArgs(new MouseButtonEvent()));//то возникает событие MouseUp
                        }
                    }
                } 
            }
            else
            {
                if (this.cursorOnForm)//если курср раннее находился на форме
                {
                    if (this.MouseOut != null)
                    {//при условии что есть обработчик события
                        this.MouseOut(this, new MouseMoveEventArgs(new MouseMoveEvent()));//то возникает событие MouseOut
                    }
                }
            }
        }

        /// <summary>
        /// Analising of being points in region of child widgets
        /// <para></para>
        /// Проверка нахождения точки в области дочерних виджетов
        /// </summary>
        /// <param name="testingPoint">Analizing poing / Поверяемая точка</param>
        private bool ChildMoveTest(Vector2f point)
        {
            foreach (RedWidget currentForm in this.childWidgetsCollections)
            {
                if (currentForm.PointTest(point))//проверка данного уровня дочерних форм на нахождение курсора в их области
                {
                    return true;//если курсор находится в области одной из дочерних форм, то вернуть true
                }
                if (currentForm.ChildMoveTest(point))//проверка более вложенного уровня дочерних форм
                {
                    return true;//если курсор находится на дочерней форме на более вложенном уровне, то вернуть true
                }
            }
            return false;//иначе вернуть false
        }

        /// <summary>
        /// Analising of being cursor in region of widget
        /// <para></para>
        /// Проверка нахождения курсора в области виджета
        /// </summary>
        private bool MoveTest(RedWindow window)
        {
            Vector2i mousePoint = Mouse.GetPosition(window.GetWindow());//Getting cursor position in window / Получить позицию мыши в окне
            if(this.PointTest(new Vector2f(mousePoint.X, mousePoint.Y)))//If cursor in region of this widget / если курсор находится в области данного виджета
            {
                if (ChildMoveTest(new Vector2f(mousePoint.X, mousePoint.Y))) //Check of child widgets / Проверить все дочерние виджетыы
                {
                    return false; 
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Analising of being points in region widget
        /// <para></para>
        /// Проверка нахождения точки в области виджета
        /// </summary>
        /// <param name="testingPoint">Analizing poing / Поверяемая точка</param>
        protected virtual bool PointTest(Vector2f testingPoint)
        {
            Vector2f center = this.GetPosition() + new Vector2f(this.size.X / 2, this.size.Y / 2);
            return this.View.PointAnalize(testingPoint, center);
        }

        /// <summary>
        /// Getting widget global position
        /// <para></para>
        /// Получение глобального положения виджета
        /// </summary>
        /// <returns>Global widget position / Глобальное положение виджета</returns>
        protected Vector2f GetPosition()
        {
            Vector2f point = new Vector2f(0, 0);
            if (this.ParentRedWidget != null)
            {
                point = this.ParentRedWidget.GetPosition();
            }
            return point += this.Location + this.viewOffset;
        }

        /// <summary>
        /// Setting basic events handlers
        /// <para></para>
        /// Устванвка базовых слушателей событий
        /// </summary>
        protected void SetBasicReactions()
        {
            this.MouseIn += InReaction;
            this.MouseOut += OutReaction;
            this.MouseDown += DownReaction;
            this.MouseUp += UpReaction;
        }

        /// <summary>
        /// Widget view correction after Red Window's View changing
        /// <para></para>
        /// Коррекция отображения виджета после изменения вида окна Red Window
        /// </summary>
        internal void WidgetViewChangeCorrection(Vector2f viewOffset, float viewRotation, Vector2f center)
        {
            //TODO: ROTATION CORRECTION
            this.viewOffset += viewOffset;
        }

        internal void WidgetResizeCorrection(Borders borders ,Vector2f sizeChanging)
        {
            if (this.AlignUp && this.AlignDown)
            {
                this.Size = new Vector2f(this.Size.X, this.Size.Y + sizeChanging.Y);
            }
            if (this.AliginLeft && this.AlignRight)
            {
                this.Size = new Vector2f(this.Size.X + sizeChanging.X, this.Size.Y);
            }
            foreach (RedWidget childWidget in this.childWidgetsCollections)
            {
                childWidget.WidgetResizeCorrection(new Borders(this.BorderLeft, this.BorderTop, this.BorderRight, this.BorderBottom), sizeChanging);
            }
            //if (!this.AliginLeft || this.AlignDown)
            //{
            //    this.Location = new Vector2f(this.Location.X + sizeChanging.X, this.Location.Y);
            //}
            //if (!this.AlignUp || this.AlignRight)
            //{
            //    this.Location = new Vector2f(this.Location.X, this.Location.Y + sizeChanging.Y);
            //}
        }

        /// <summary>
        /// Checking and correcting widgets regions
        /// <para></para>
        /// Проверка и корректировка областей виджетов
        /// </summary>
        private void WidgetViewCorrection()
        {
            foreach (RedWidget childWidget in this.childWidgetsCollections)
            {
                if (this.BorderLeft > childWidget.BorderLeft)
                {
                    childWidget.Location = new Vector2f(0, childWidget.Location.Y);
                }
                if (this.BorderTop > childWidget.BorderTop)
                {
                    childWidget.Location = new Vector2f(childWidget.Location.X, 0);
                }
                float dx = childWidget.BorderRight - this.BorderRight;
                if (dx > 0)
                {
                    this.Size = new Vector2f(this.Size.X + dx, this.Size.Y);
                }
                float dy = childWidget.BorderBottom - this.BorderBottom;
                if (dy > 0)
                {
                    this.Size = new Vector2f(this.Size.X, this.Size.Y + dy);
                }
            }
        }

        //RED WIDGET BASIC REACTIONS
        //БАЗОЫЙ РЕАКЦИИ RED WIDGET

        private void InReaction(object sender, MouseMoveEventArgs e)
        {
            this.cursorOnForm = true;//флаг нахождения курсора на форме устанавливается в true
            this.leftMousePressCount = 0;//сброс счетчика кликов
            this.leftMouseReleaseCount = 0;
        }

        private void OutReaction(object sender, MouseMoveEventArgs e)
        {
            this.cursorOnForm = false;//флаг нахождения курсора на форме устанавливается в false
            this.buttonPresed = false;//флаг нажатия кнопки на форме устанавливается в false
            this.leftMousePressCount = 0;//сброс счетчика кликов
            this.leftMouseReleaseCount = 0;
            this.click = false;
        }

        private void DownReaction(object sender, MouseButtonEventArgs e)
        {
            this.buttonPresed = true;//флаг нажатия кнопки на форме устанавливается в true
            if (!this.click)
            {
                this.click = true;
            }
        }

        private void UpReaction(object sender, MouseButtonEventArgs e)
        {
            this.buttonPresed = false;//флаг нажатия кнопки на форме устанавливается в true
            if (this.click)
            {
                if (this.MouseClick != null)
                {//если имеются слушатели события
                    this.MouseClick(this, new MouseButtonEventArgs(new MouseButtonEvent()));
                }      
            }
        }

    }
}
