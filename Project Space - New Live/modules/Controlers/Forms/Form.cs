using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Размер
        /// </summary>
        protected Vector2f size;
        /// <summary>
        /// Размер
        /// </summary>
        public virtual Vector2f Size
        {
            get { return this.size; }
            set { this.size = value; }
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
            retValue.Add(this.view);//добавление отображени в массив
            foreach (Form childForm in this.ChildForms)//добавление в массив возвращаемых значений дочерних отображений фолрм
            {
                if (this.ChildFormFallTest(childForm))//проверка если дочерняя форма не "падает" с текущей
                {//получаем отображение данной дочерней формы
                    foreach (ObjectView locView in childForm.GetChildFormView())
                    {
                        retValue.Add(locView);
                    }               

                }
            }
            this.CatchEvents();//обнаружение событий данной формы
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
        public event EventHandler MouseIn = NotReaction;
        /// <summary>
        /// Возникает при покидании курсором области формы
        /// </summary>
        public event EventHandler MouseOut = NotReaction;
        /// <summary>
        /// Возникает при нахождении курсора в области формы
        /// </summary>
        public event EventHandler MouseMove = NotReaction;
        /// <summary>
        /// Возникает при отжатии левой кнопки мыши
        /// </summary>
        public event EventHandler MouseUp = NotReaction;
        /// <summary>
        /// Возникает при нажатии на левую кнопку мыши
        /// </summary>
        public event EventHandler MouseDown = NotReaction;

        /// <summary>
        /// Отлавливание событий
        /// </summary>
        internal void CatchEvents()
        {
            if (this.MoveTest())//если курсор находится на форме
            {
                if (!this.CursorOnForm)//и до этого он не находился на ней
                {
                    this.MouseIn(this, new MouseMoveEventArgs(new MouseMoveEvent()));//то возникает событие MouseIn 
                    this.CursorOnForm = true;//и флаг нахождения курсора на форме устанавливается в true
                }//в независимости от предыдущего нахождения курсора
                this.MouseMove(this, new MouseMoveEventArgs(new MouseMoveEvent()));//то возникает событие MouseMove 
                if (Mouse.IsButtonPressed(Mouse.Button.Left))//если левая кнопка мыши зажата
                {
                    this.MouseDown(this, new MouseButtonEventArgs(new MouseButtonEvent()));//то возникает событие MouseDown
                    this.ButtonPresed = true;//установка флага зажатия кнопки в true
                }
                if (!Mouse.IsButtonPressed(Mouse.Button.Left))//если левая кнопка мыши отжата
                {
                    if (this.ButtonPresed)//если кнопка была зажата
                    {
                        this.MouseUp(this, new MouseButtonEventArgs(new MouseButtonEvent()));//то возникает событие MouseUp
                        this.ButtonPresed = false;//установка флага зажатия кнопки в false
                     }
                } 
            }
            else
            {
                if (this.CursorOnForm)//если курср раннее находился на форме
                {
                    this.MouseOut(this, new MouseMoveEventArgs(new MouseMoveEvent()));//то возникает событие MouseOut
                    this.CursorOnForm = false;
                    this.ButtonPresed = false;
                }
            }
        }

        /// <summary>
        /// Проверка на нахождение курсора в области формы
        /// </summary>
        /// <returns></returns>
        private bool MoveTest()
        {
            Vector2i mousePoint = Mouse.GetPosition(RenderModule.getInstance().MainWindow);
            return this.PointTest(new Vector2f(mousePoint.X, mousePoint.Y));
        }

        /// <summary>
        /// /// Проверка на нахождение точки в области формы
        /// </summary>
        /// <param name="testingPoint"></param>
        /// <returns></returns>
        protected abstract bool PointTest(Vector2f testingPoint);

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

        private static void NotReaction(object sender, EventArgs e)
        {
            
        }
        

    }
}
