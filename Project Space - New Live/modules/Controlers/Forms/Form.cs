﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Project_Space___New_Live.modules.Dispatchers;

namespace Project_Space___New_Live.modules.Controlers
{
    public abstract class Form
    {

        /// <summary>
        /// Позиция управляющей формы
        /// </summary>
        protected Vector2f homeLocation;

        /// <summary>
        /// Устанеовка позиции управляющей формы
        /// </summary>
        /// <param name="newHomeLocation"></param>
        protected void SetHomeLocation(Vector2f newHomeLocation)
        {
            this.homeLocation = newHomeLocation;
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
        /// Позиция
        /// </summary>
        protected Vector2f location;
        /// <summary>
        /// Позиция
        /// </summary>
        public abstract Vector2f Location
        {
            get;
            set;
        }

        /// <summary>
        /// Размер
        /// </summary>
        protected Vector2f size;
        /// <summary>
        /// Размер
        /// </summary>
        public abstract Vector2f Size
        {
            get; 
            set; 
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
            newForm.SetHomeLocation(this.location);
            this.childForms.Add(newForm);
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
        /// Отображения дочерних форм
        /// </summary>
        /// <returns></returns>
        protected List<ObjectView> GetChildFormView()
        {
            List<ObjectView> retValue = new List<ObjectView>();
            if (this.GetPersonalView() != null)
            {
                retValue.Add(this.GetPersonalView());
            }
            foreach (Form childForm in this.ChildForms)//добавление в массив возвращаемых значений дочерних отображений фолрм
            {    
                foreach (ObjectView view in childForm.GetChildFormView())
                {
                    retValue.Add(view);
                }               
            }
            this.CatchEvents();//обнаружение событий данной формы
            return retValue;
        }

        /// <summary>
        /// Получить отображение текущей формы
        /// </summary>
        /// <returns></returns>
        protected abstract ObjectView GetPersonalView();

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
        /// Отлавливание событий
        /// </summary>
        protected void CatchEvents()
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
        protected abstract bool MoveTest();

    }
}
