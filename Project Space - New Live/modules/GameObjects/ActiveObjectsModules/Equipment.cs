﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;

namespace Project_Space___New_Live.modules.GameObjects
{
    public abstract class Equipment
    {

        /// <summary>
        /// Состояние (активно-неактивно)
        /// </summary>
        protected bool state;

        /// <summary>
        /// Состояние (активно-неактивно)
        /// </summary>
        public virtual bool State
        {//неактивное оборудование не потребляет энергии 
            get { return this.state; }
        }

        /// <summary>
        /// Флаг аварийного состояния (true - объект в аварийном состоянии)
        /// </summary>
        protected bool emergensyState;

        /// <summary>
        /// Изображение оборудования
        /// </summary>
        private ImageView view;
        /// <summary>
        /// Изображение оборудования
        /// </summary>
        public ImageView View
        {
            get { return this.view; }
        }

        /// <summary>
        /// Масса
        /// </summary>
        private int mass;
        /// <summary>
        /// Масса
        /// </summary>
        public int Mass
        {
            get { return this.mass; }
        }

        /// <summary>
        /// Версия (уровень модификации)
        /// </summary>
        private int version;
        /// <summary>
        /// Версия (уровень модификации)
        /// </summary>
        public int Version
        {
            get { return this.version; }
        }

        /// <summary>
        /// Текущий уровень износа оборудования (в процентах) 
        /// </summary>
        protected int wearState = 0;
        /// <summary>
        /// Текущий уровень износа оборудования (в процентах) 
        /// </summary>
        public int WearState
        {
            get { return this.wearState; }
        }

        /// <summary>
        /// Энергопотребление
        /// </summary>
        private int energyNeeds;
        /// <summary>
        /// Энергопотребление
        /// </summary>
        public int EnergyNeeds
        {
            get { return this.energyNeeds; }
        }

        /// <summary>
        /// Модификация оборудования
        /// </summary>
        public void Upgrate()
        {
            this.version ++;//увеличение версии
            this.CustomModification();//улучшение оборудования
        }

        /// <summary>
        /// Даунгрейт оборудования
        /// </summary>
        public void Downgrate()
        {
            if (version > 0)
            {
                this.version--; //умениьшение версии
                this.CustomModification(); //модификация оборудования
            }
        }

        /// <summary>
        /// Модификация конкретного типа оборудованиея
        /// </summary>
        /// <param name="directionID"></param>
        protected abstract void CustomModification();

        /// <summary>
        /// Функция изнашивания оборудования
        /// </summary>
        /// <param name="damage"></param>
        public void Wearing(int damage)
        {
            if (this.wearState < 100)//если степень износа менее 100%
            {
                this.wearState += damage; //увеличение процента износа
                if (this.wearState >= 100) //если оборудование изношенно на 100 и более %
                {
                    this.wearState = 100; //то установить степень износа в 100%
                    this.emergensyState = true; //и установить состояние оборудования в аварийное
                }
            }
        }

        /// <summary>
        /// Восстановление оборудования (установка износа в 0%)
        /// </summary>
        public void Repair()
        {
            this.wearState = 0;//установка процента износа в 0
            this.emergensyState = false;//установка состояния в нормальное
        }

        /// <summary>
        /// Сохранение общих характеристик
        /// </summary>
        /// <param name="mass">Масса</param>
        /// <param name="energyNeeds">Энергопотребление</param>
        /// <param name="image">Изображение</param>
        protected void SetCommonCharacteristics(int mass, int energyNeeds, Shape image)
        {
            this.mass = mass;//установка массы
            this.energyNeeds = energyNeeds;//установка энергопотребления
            this.view = new ImageView(image, BlendMode.Alpha);//построение отображения
            this.Repair();//установка состояния и износа
            this.version = 0;//Установка версии оборудования в 0
        }

        /// <summary>
        /// Активация оборудования
        /// </summary>
        /// <returns>true - удалось активировать, false - не удалось</returns>
        public virtual bool Activate()
        {
            if (!this.emergensyState)//если оборудование не в аварийном состоянии
            {//то установить состояние в активное
                this.state = true;
                return true;//вернуть true
            }
            return false;//иначе активироать оборудование не удалось, вернуть false
        }

        /// <summary>
        /// Деактивация оборудования
        /// </summary>
        public virtual void Deactivate()
        {
            this.state = false;//то установить состояние в неактивное
        }

    }
}
