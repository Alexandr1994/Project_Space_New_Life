using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Project_Space___New_Live.modules.Dispatchers;

namespace Project_Space___New_Live.modules.Controlers.Forms
{
    public abstract class Form
    {

        
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
            set { this.location = value;}
        }

        /// <summary>
        /// Видимость формыи дочерних форм
        /// </summary>
        private bool visible;
        /// <summary>
        /// Видимость формыи дочерних форм
        /// </summary>
        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
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
                if(this.view.Image != null)
                {
                    float Xcoef = value.X / this.size.X;
                    float Ycoef = value.Y / this.size.Y;
                    //Изменение размеров изображения
                    this.view.Image.Scale = new Vector2f(Xcoef, Ycoef);
                    this.size = value;//сохранение размеров
                }
                else
                {
                    size = value;
                }
            }
        }

        /// <summary>
        /// Надпись на форме
        /// </summary>
        private String text;
        /// <summary>
        /// Надпись на форме
        /// </summary>
        public String Text
        {
            get {return this.text; }
            set { this.text = value; }
        }

        /// <summary>
        /// Указатель на родительскую форму (если таковая есть)
        /// </summary>
        protected Form parentForm;

        /// <summary>
        /// Установка родителской формы
        /// </summary>
        /// <param name="parentForm"></param>
        protected void SetPatentForm(Form parentForm)
        {
            this.parentForm = parentForm;
        }


        /// <summary>
        /// Массив дочерних форм
        /// </summary>
        private List<Form> childForms = new List<Form>();

        /// <summary>
        /// Массив дочерних форм
        /// </summary>
        protected List<Form> ChildForms
        {
            get { return childForms; }
        }

        /// <summary>
        /// Добавить дочернюю форму
        /// </summary>
        /// <param name="newForm"></param>
        public void AddForm(Form newForm)
        {
            this.childForms.Add(newForm);
            newForm.SetPatentForm(this);
        }

        /// <summary>
        /// Удалить форму
        /// </summary>
        /// <param name="targetForm"></param>
        public void RemoveForm(Form targetForm)
        {
            this.childForms.Remove(targetForm);
        }

        /// <summary>
        /// отображение
        /// </summary>
  
        protected ObjectView view = new ObjectView();
        /// <summary>
        /// Отображения текущией и дочерних форм
        /// </summary>
        /// <returns></returns>
        protected List<ObjectView> GetChildFormView()
        {
            List<ObjectView> retValue = new List<ObjectView>();
            view.Image.Position = this.GetGraphicPosition();//коррекция отображения
            if (this.Visible)//если форма видимая
            {
                retValue.Add(this.view); //добавление отображени в массив
                foreach (Form childForm in this.ChildForms)
                    //добавление в массив возвращаемых значений дочерних отображений фолрм
                {
                    if (this.ChildFormFallTest(childForm)) //проверка если дочерняя форма не "падает" с текущей
                    {
//получаем отображение данной дочерней формы
                        foreach (ObjectView locView in childForm.GetChildFormView())
                        {
                            retValue.Add(locView);
                        }

                    }
                }
                this.CatchEvents(); //обнаружение событий данной формы
            }
            return retValue;
        }

        /// <summary>
        /// Анализ формы на "падение" с родительской формы.
        /// Если за пределами родительской формы лежит болше половины дочерней формы,
        ///  то дочерняя форма не будет отображаться и отлавливать события
        /// </summary>
        /// <returns></returns>
        private bool ChildFormFallTest(Form childForm)
        {
            Vector2f centerOfForm = childForm.GetPhizicalPosition() + childForm.Size / 2;//Нахождение центра формы
            return this.PointTest(centerOfForm);
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
        private bool CursorOnForm = false;
        /// <summary>
        /// Флаг удержания левой кнопки мыши
        /// </summary>
        private bool ButtonPresed = false;
        /// <summary>
        /// Возникает при вхождении курсора в область формы
        /// </summary>
        public event EventHandler MouseIn = null;
        /// <summary>
        /// Возникает при покидании курсором области формы
        /// </summary>
        public event EventHandler MouseOut = null;
        /// <summary>
        /// Возникает при нахождении курсора в области формы
        /// </summary>
        public event EventHandler MouseMove = null;
        /// <summary>
        /// Возникает при отжатии левой кнопки мыши
        /// </summary>
        public event EventHandler MouseUp = null;
        /// <summary>
        /// Возникает при нажатии на левую кнопку мыши
        /// </summary>
        public event EventHandler MouseDown = null;
        /// <summary>
        /// Событие клика кнопки
        /// </summary>
        public event EventHandler MouseClick = null;

        /// <summary>
        /// Отлавливание событий
        /// </summary>
        internal void CatchEvents()
        {
            if (this.MoveTest())//если курсор находится на форме
            {
                if (this.MouseMove != null)//в независимости от предыдущего нахождения курсора
                {//при условии что есть обработчик события
                    this.MouseMove(this, new MouseMoveEventArgs(new MouseMoveEvent()));
                }
                if (!this.CursorOnForm)//и до этого он не находился на ней
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
                    if (this.ButtonPresed)//если кнопка была зажата
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
                if (this.CursorOnForm)//если курср раннее находился на форме
                {
                    if (this.MouseOut != null)
                    {//при условии что есть обработчик события
                        this.MouseOut(this, new MouseMoveEventArgs(new MouseMoveEvent()));//то возникает событие MouseOut
                    }
                }
            }
        }

        /// <summary>
        /// Проверка нахождения точнки в областях дочерних форм
        /// </summary>
        /// <returns></returns>
        private bool ChildMoveTest(Vector2f point)
        {
            foreach (Form currentForm in this.childForms)
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
        /// <returns></returns>
        private bool MoveTest()
        {
            Vector2i mousePoint = Mouse.GetPosition(RenderModule.getInstance().MainWindow);//Получить позицию мыши в окне
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
        /// /// Проверка на нахождение точки в области формы
        /// </summary>
        /// <param name="testingPoint"></param>
        /// <returns></returns>
        protected virtual bool PointTest(Vector2f testingPoint)
        {
            Vector2f center = this.GetPhizicalPosition() + new Vector2f(this.size.X / 2, this.size.Y / 2);//нахождение центра окружности образующей кнопку
            return this.view.PointAnalize(testingPoint, center);
        }

        /// <summary>
        /// Получение графической позиции формы
        /// </summary>
        /// <returns></returns>
        protected Vector2f GetGraphicPosition()
        {
            Vector2f point = new Vector2f(0, 0);
            if (this.parentForm != null)//Если форма имеет родительскую форму
            {//получить позицию с учетом её положения
                point = this.parentForm.GetGraphicPosition();
            }
            return point += this.Location;
        }

        /// <summary>
        /// Получение физической позиции формы(отличается от графической на смещение камеры)
        /// </summary>
        /// <returns></returns>
        protected virtual Vector2f GetPhizicalPosition()
        {
            Vector2f point = new Vector2f(0, 0);
            if (this.parentForm != null)//Если форма имеет родительскую форму
            {//получить позицию с учетом её положения
                point = this.parentForm.GetPhizicalPosition();
            }
            return point += this.Location;
        }

        /// <summary>
        /// Устванвка базовых реакций формы
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InReaction(object sender, EventArgs e)
        {
            this.CursorOnForm = true;//флаг нахождения курсора на форме устанавливается в true
            this.clicks = 0;//сброс счетчика кликов
        }

        /// <summary>
        /// Элементарная реакция на покидание курсора в области формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutReaction(object sender, EventArgs e)
        {
            this.CursorOnForm = false;//флаг нахождения курсора на форме устанавливается в false
            this.ButtonPresed = false;//флаг нажатия кнопки на форме устанавливается в false
            this.clicks = 0;//сброс счетчика кликов
            this.click = false;
        }

        /// <summary>
        /// Элементарная реакция на нажатие левой кнопки мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownReaction(object sender, EventArgs e)
        {
            this.ButtonPresed = true;//флаг нажатия кнопки на форме устанавливается в true
            if (!this.click)
            {
                this.click = true;
            }
        }

        /// <summary>
        /// Элементарная реакция на отжатия левой кнопки мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpReaction(object sender, EventArgs e)
        {
            this.ButtonPresed = false;//флаг нажатия кнопки на форме устанавливается в true
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
        public Form()
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
