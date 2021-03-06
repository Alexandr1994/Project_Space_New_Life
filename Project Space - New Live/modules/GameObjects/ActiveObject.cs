﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Активный игровой объект
    /// </summary>
    public abstract class ActiveObject : GameObject
    {
        /// <summary>
        /// Текстурная лента визуального эффекта гибели объекта
        /// </summary>
        protected Texture visualEffectSkin;

        /// <summary>
        /// Сконструировать визуальный эффект гибели объекта
        /// </summary>
        /// <param name="effectSize">Размер кадра</param>
        /// <param name="effectTapeLenght">Количество кадров</param>
        /// <returns>Визуальный эффект гибели объекта</returns>
        public virtual VisualEffect ConstructDeathVisualEffect(Vector2f effectSize, int effectTapeLenght)
        {
            return new VisualEffect(this.Coords, effectSize, effectTapeLenght, visualEffectSkin);
        }

        //КОНТРОЛЛЕР И СРЕДА ОБЪЕКТА
        
        /// <summary>
        /// Конторллер активного объекта 
        /// </summary>
        protected AbstractController brains;

        /// <summary>
        /// Умтановка контроллера объекта
        /// </summary>
        /// <param name="newBrains">Контроллер</param>
        public void SetBrains(AbstractController newBrains)
        {
            if (this.brains == null)
            {
                this.brains = newBrains;
            }
        }

        /// <summary>
        /// Экземпляр среды, в которой находится данный объект
        /// </summary>
        protected BaseEnvironment environment;

        /// <summary>
        /// Экземпляр среды, в которой находится данный объект
        /// </summary>
        public BaseEnvironment Environment
        {
            get { return this.environment; }
            set { this.environment = value; }
        }

        //ОБЩЕЕ ОБОРУДОВАНИЕ/СНАРЯЖЕНИЕ АКТИВНЫХ ОБЪЕКТОВ

        /// <summary>
        /// Коллекция оборудования/снаряжения активного объекта
        /// </summary>
        public abstract List<Equipment> Equipment { get; }  

        /// <summary>
        /// Реактор/Генератор активного объекта
        /// </summary>
        protected Reactor objectReactor;

        /// <summary>
        /// Энергобатарея активного объекта
        /// </summary>
        protected Battery objectBattery;

        /// <summary>
        /// Радар/сканер активного объекта
        /// </summary>
        protected Radar objectRadar;

        /// <summary>
        /// Энергощит/персональный энергощит активного объекта
        /// </summary>
        protected Shield objectShield;


        //ФЛАГИ ОБЪЕКТА

        /// <summary>
        /// Флаг уничтожения активного объекта
        /// </summary>
        protected bool destroyed = false;

        /// <summary>
        /// Флаг уничтожения  активного объекта
        /// </summary>
        public bool Destroyed
        {
            get { return this.destroyed; }
        }

        //ПОЛЯ И МЕТОДЫ ДИВЖЕНИЯ ОБЪЕКТА

        /// <summary>
        /// Текущий поворот активного объекта в рад.
        /// </summary>
        protected float rotation = (float)(Math.PI / 2);

        /// <summary>
        /// Текущий поворот активного объекта в рад.
        /// </summary>
        public float Rotation
        {
            get { return this.rotation; }
        }

        /// <summary>
        /// Угол атаки активного объекта
        /// </summary>
        public virtual float AttackAngle
        {
            get { return this.rotation; }
        }

        /// <summary>
        /// Элементарное движение активного объекта
        /// </summary>
        /// <param name="speed">Скорость движения</param>
        /// <param name="angle">Угол поворота вектора скорости</param>
        public virtual void ShipAtomMoving(float speed, float angle)
        {
            Vector2f tempCoords = this.coords;
            this.coords.X += (float)(speed * Math.Cos(angle));
            this.coords.Y += (float)(speed * Math.Sin(angle));
            Vector2f delta = this.coords - tempCoords;//Изменение по координатам Х и Y
            foreach (ImageView partView in this.view)
            {
                partView.Translate(delta);
            }
        }

        /// <summary>
        /// Изменение угла поворота активного объекта
        /// </summary>
        /// <param name="angle">Угол на который происходит изменение в рад.</param>
        public virtual void ChangeRotation(float angle)
        {
            this.rotation += angle;//изменение текущего поворота корабля
            foreach (ImageView partView in this.view)
            {
                partView.Rotate(this.coords, angle);//изменение каждой части отображения
            }
            if (this.rotation > 2 * Math.PI)
            {
                this.rotation -= (float)(2 * Math.PI);
            }
        }

        //СОСТОЯНИЕ ОБЪЕКТА

        /// <summary>
        /// Текущий запас прочности
        /// </summary>
        protected int health;

        /// <summary>
        /// Текущий запас прочности
        /// </summary>
        public int Health
        {
            get { return this.health; }
        }

        /// <summary>
        /// Максимальный запас прочности
        /// </summary>
        protected int maxHealth;

        /// <summary>
        /// Максимальный запас прочности
        /// </summary>
        public int MaxHealth
        {
            get { return this.maxHealth; }
        }

        //ЗАЩИТНАЯ СИСТЕМА

        /// <summary>
        /// Текущее состояние энергощита 
        /// </summary>
        public bool ShieldActive
        {
            get
            {
                if (this.objectShield != null)//если щит установлен
                {
                    return this.objectShield.State;//то опросить его состояние
                }
                return false;
            }
        }

        /// <summary>
        /// Активировать энергощит
        /// </summary>
        /// <returns>true - удалось активировать, false - не удалось</returns>
        public bool ActivateShield()
        {
            if (this.objectShield != null)//Если энергощит есть
            {
                return this.objectShield.Activate();
            }
            return false;
        }

        /// <summary>
        /// Деактивировать щит
        /// </summary>
        public void DeactivateShield()
        {
            this.objectShield.Deactivate();
        }

        //БОЕВАЯ СИСТЕМА

        /// <summary>
        /// Оружейная система активного объекта
        /// </summary>
        protected WeaponSystem objectWeaponSystem;

        /// <summary>
        /// Оружейная система корабля
        /// </summary>
        public WeaponSystem ObjectWeaponSystem
        {
            get { return this.objectWeaponSystem; }
        }

        /// <summary>
        /// Открыть огонь
        /// </summary>
        public void OpenFire()
        {
            this.objectWeaponSystem.Shooting = true;
        }

        /// <summary>
        /// Прекратить огонь
        /// </summary>
        public void StopFire()
        {
            this.objectWeaponSystem.Shooting = false;
        }

        /// <summary>
        /// Базовое получение урона активного объекта
        /// </summary>
        /// <param name="damage">Урон, наносимый объекту</param>
        /// <param name="equipmentDamage">Урон, наносимый оборудованию</param>
        /// <param name="damagedPartIndex">Пораженная часть объекта</param>
        public void GetDamage(int damage, int equipmentDamage, int damagedPartIndex)
        {
            if (!this.ShieldActive)//если щит не активен
            {//то нанести урон объекту
                this.WearingEquipment(equipmentDamage, damagedPartIndex);//Нанести повреждения оборудованию
                if (this.health > damage)
                {
                    this.health -= damage;
                }
                else
                {
                    this.health = 0;
                }
            }
            else
            {//иначе нанести урон экрану щита
                objectShield.GetDamageOnShield(damage, equipmentDamage / 5);
            }
        }

        /// <summary>
        /// Наношение урона оборудованию
        /// </summary>
        /// <param name="equipmentDamage">Урон наносимый оборудованию</param>
        /// <param name="damagedPartIndex">Пораженная часть активного объекта</param>
        protected abstract void WearingEquipment(int equipmentDamage, int damagedPartIndex);

        /// <summary>
        /// Нанести урон конкретному оборудованию
        /// </summary>
        /// <param name="equipment">Оборудование</param>
        /// <param name="damage">Величина урона</param>
        protected void WearingCustomEquipment(Equipment equipment, int damage)
        {
            if (equipment != null)
            {
                equipment.Wearing(damage);
            }
        }

        /// <summary>
        /// Восстановление активного объекта
        /// </summary>
        /// <param name="recovery">Величина восстановления</param>
        public void Recovery(int recovery)
        {
            if (this.health < this.MaxHealth)
            {
                if (Math.Abs(this.health - this.MaxHealth) > recovery)
                {
                    this.health += recovery;
                }
                else
                {
                    this.health = this.MaxHealth;
                }
            }
        }

        /// <summary>
        /// Управление энергией активного объекта
        /// </summary>
        protected void EnergyProcess()
        {
            this.objectBattery.ThrowExcessEnergy();//Сбросить избыточную энергию
            this.objectBattery.Charge(this.objectReactor.EnergyGeneration);//Работа реактора
            foreach (Equipment device in this.Equipment)//перебрать коллекцию оборудования/снаряжения
            {
                if (device != null)//если данное оборудование имеется
                {
                    if (this.objectBattery.Energy >= device.EnergyNeeds && device.State)//если оно активированно и энергобатарея может обеспечить его потребность в энергии 
                    {
                        this.objectBattery.Uncharge(device.EnergyNeeds);//то уменьшить заряд батареи на нужную величину
                    }
                    else//иначе деактивировать его
                    {
                        device.Deactivate();
                    }
                }
            }
        }
        
        /// <summary>
        /// Анализирование взаимодействия данного объекта с другими объектами в среде
        /// </summary>
        public abstract void AnalizeObjectInteraction();

    }
}
