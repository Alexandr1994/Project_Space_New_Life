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
    /// Абстрактная форма интерфейса
    /// </summary>
    public abstract class RedWidget
    {

        /// <summary>
        /// Отображение
        /// </summary>
        protected abstract RenderView View { get; }


        /// <summary>
        /// Позиция
        /// </summary>
        protected Vector2f location;

        /// <summary>
        /// Позиция
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
        /// Размер
        /// </summary>
        protected Vector2f size;

        /// <summary>
        /// Размер
        /// </summary>
        public virtual Vector2f Size
        {
            get { return this.size; }
            set
            {
                if (this.View != null)
                {
                    float Xcoef = value.X / this.size.X;
                    float Ycoef = value.Y / this.size.Y;
                    this.View.View.Scale = new Vector2f(Xcoef, Ycoef);//Изменение размеров изображения
                    this.size = value;//сохранение размеров
                }
                else
                {
                    size = value;
                }
            }
        }

        /// <summary>
        /// Видимость формы и дочерних форм
        /// </summary>
        private bool visible;

        /// <summary>
        /// Видимость формы и дочерних форм
        /// </summary>
        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        /// <summary>
        /// Указатель на родительскую форму, при её наличии
        /// </summary>
        protected RedWidget ParentRedWidget;

        /// <summary>
        /// Установка родителской формы
        /// </summary>
        /// <param name="ParentRedWidget">Родительская форма</param>
        protected void SetPatentForm(RedWidget ParentRedWidget)
        {
            this.ParentRedWidget = ParentRedWidget;
        }

        /// <summary>
        /// Коллекция дочерних форм
        /// </summary>
        private List<RedWidget> childForms = new List<RedWidget>();

        /// <summary>
        /// Коллекция дочерних форм
        /// </summary>
        protected List<RedWidget> ChildForms
        {
            get { return childForms; }
        }

        /// <summary>
        /// Добавить дочернюю форму
        /// </summary>
        /// <param name="newRedWidget">Новая дочерняя форма</param>
        public void AddForm(RedWidget newRedWidget)
        {
            this.childForms.Add(newRedWidget);
            newRedWidget.SetPatentForm(this);
        }

        /// <summary>
        /// Удалить дочернюю форму
        /// </summary>
        /// <param name="targetRedWidget">Форма предназначенная к удалению</param>
        public void RemoveForm(RedWidget targetRedWidget)
        {
            this.childForms.Remove(targetRedWidget);
        }

        /// <summary>
        /// Получить отображения текущией и дочерних форм
        /// </summary>
        /// <returns>Коллекция отображений форм</returns>
        public List<RenderView> GetFormView(RedWindow window)
        {
            List<RenderView> retValue = new List<RenderView>();
            this.View.View.Position = this.GetPosition();//коррекция отображения;
            if (this.Visible)//если форма видимая
            {
                retValue.Add(this.View); //добавление отображени в массив
                foreach (RedWidget childForm in this.ChildForms)//добавление в массив возвращаемых значений дочерних отображений фолрм
                {
                    if (this.ChildFormFallTest(childForm)) //проверка если дочерняя форма не "падает" с текущей
                    {
                        foreach (RenderView locView in childForm.GetFormView(window))//получаем отображение данной дочерней формы
                        {
                            retValue.Add(locView);
                        }

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
        /// Анализ формы на нахождение ей в области родительской формы
        /// </summary>
        /// <returns>true - если хотя бы часть проверяемой формы находится в пределах родительской, иначе false</returns>
        private bool ChildFormFallTest(RedWidget childRedWidget)
        {
            return true;//вернуть true
        }

        /// <summary>
        /// Количество нажатий на форму
        /// </summary>
        private int clicks = 0;

        /// <summary>
        /// Флаг клика
        /// </summary>
        private bool click = false; 

        /// <summary>
        /// Флаг нахождения курсора на форме
        /// </summary>
        private bool cursorOnForm = false;

        /// <summary>
        /// Флаг удержания левой кнопки мыши
        /// </summary>
        private bool buttonPresed = false;

        /// <summary>
        /// Возникает при вхождении курсора в область формы
        /// </summary>
        public event EventHandler<MouseMoveEventArgs> MouseIn = null;

        /// <summary>
        /// Возникает при покидании курсором области формы
        /// </summary>
        public event EventHandler<MouseMoveEventArgs> MouseOut = null;

        /// <summary>
        /// Возникает при нахождении курсора в области формы
        /// </summary>
        public event EventHandler<MouseMoveEventArgs> MouseMove = null;

        /// <summary>
        /// Возникает при отжатии левой кнопки мыши
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseUp = null;

        /// <summary>
        /// Возникает при нажатии на левую кнопку мыши
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseDown = null;

        /// <summary>
        /// Событие клика кнопки
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseClick = null;

        /// <summary>
        /// Анализ возникновения событий
        /// </summary>
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
        /// Проверка нахождения точки в областях дочерних форм
        /// </summary>
        /// <returns>true если курсор находится в области одной из дочерних форм, иначе - false</returns>
        private bool ChildMoveTest(Vector2f point)
        {
            foreach (RedWidget currentForm in this.childForms)
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
        /// Проверка на нахождение курсора в области формы
        /// </summary>
        /// <returns>true - если курсор находится в области данной формы, иначе - false</returns>
        private bool MoveTest(RedWindow window)
        {
            Vector2i mousePoint = Mouse.GetPosition(window.GetWindow());//Получить позицию мыши в окне
            if(this.PointTest(new Vector2f(mousePoint.X, mousePoint.Y)))//если курсор находится в области данной формы
            {
                if (ChildMoveTest(new Vector2f(mousePoint.X, mousePoint.Y))) //Проверить все дочерние формы данной формы
                {//если задетектированно нахождение курсора на области формы на одном из уровней дочерних форм, 
                    return false; //то курсор находится за пределами области данной формы   
                }
                return true;//если курсор находится вне областей дочерних форм разных уровней вложенности, то true
            }
            return false;//иначе false
        }

        /// <summary>
        /// Проверка на нахождение точки в области формы
        /// </summary>
        /// <param name="testingPoint">Координаты проверяемой точки</param>
        /// <returns>true если точка в области формы, иначе - false</returns>
        protected virtual bool PointTest(Vector2f testingPoint)
        {
            Vector2f center = this.GetPosition() + new Vector2f(this.size.X / 2, this.size.Y / 2);//нахождение центра формы
            return this.View.PointAnalize(testingPoint, center);
        }

        /// <summary>
        /// Получение графической позиции формы
        /// </summary>
        /// <returns>Координаты графической позиции формы</returns>
        protected Vector2f GetPosition()
        {
            Vector2f point = new Vector2f(0, 0);
            if (this.ParentRedWidget != null)//Если форма имеет родительскую форму
            {//получить позицию с учетом её положения
                point = this.ParentRedWidget.GetPosition();
            }
            return point += this.Location;
        }

        /// <summary>
        /// Устванвка базовых реакций формы на события
        /// </summary>
        protected void SetBasicReactions()
        {
            this.MouseIn += InReaction;
            this.MouseOut += OutReaction;
            this.MouseDown += DownReaction;
            this.MouseUp += UpReaction;
        }

        /// <summary>
        /// Элементарная реакция на вхождение курсора в область формы
        /// </summary>
        /// <param name="sender">Форма, в которой возникло событие</param>
        /// <param name="e">Аргументы события</param>
        private void InReaction(object sender, MouseMoveEventArgs e)
        {
            this.cursorOnForm = true;//флаг нахождения курсора на форме устанавливается в true
            this.clicks = 0;//сброс счетчика кликов
        }

        /// <summary>
        /// Элементарная реакция на покидание курсора в области формы
        /// </summary>
        /// <param name="sender">Форма, в которой возникло событие</param>
        /// <param name="e">Аргументы события</param>
        private void OutReaction(object sender, MouseMoveEventArgs e)
        {
            this.cursorOnForm = false;//флаг нахождения курсора на форме устанавливается в false
            this.buttonPresed = false;//флаг нажатия кнопки на форме устанавливается в false
            this.clicks = 0;//сброс счетчика кликов
            this.click = false;
        }

        /// <summary>
        /// Элементарная реакция на нажатие левой кнопки мыши
        /// </summary>
        /// <param name="sender">Форма, в которой возникло событие</param>
        /// <param name="e">Аргументы события</param>
        private void DownReaction(object sender, MouseButtonEventArgs e)
        {
            this.buttonPresed = true;//флаг нажатия кнопки на форме устанавливается в true
            if (!this.click)
            {
                this.click = true;
            }
        }

        /// <summary>
        /// Элементарная реакция на отжатия левой кнопки мыши
        /// </summary>
        /// <param name="sender">Форма, в которой возникло событие</param>
        /// <param name="e">Аргументы события</param>
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

        /// <summary>
        /// Абстрактный конструктор формы
        /// </summary>
        public RedWidget()
        {
            this.SetBasicReactions();//установка базовых слушателей формы
            this.CustomConstructor();//установка базовых характеристик формы 
            this.Visible = true;
        }

        /// <summary>
        /// Конструктор конкретной формы
        /// </summary>
        protected abstract void CustomConstructor();

    }
}
