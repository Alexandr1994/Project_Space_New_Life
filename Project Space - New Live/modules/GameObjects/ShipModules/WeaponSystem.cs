﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects.ShipModules
{
    public class WeaponSystem
    {
        /// <summary>
        /// Флаг ведения стрельбы 
        /// </summary>
        private bool shooting = false;

        /// <summary>
        /// Флаг ведения стрельбы 
        /// </summary>
        public bool Shooting
        {
            get { return this.shooting; }
            set
            {
                if (this.WeaponsCount > 0) //если в коллекции есть оружие
                {
                    this.shooting = value; //то возможно изменение флага
                }
                else//иначе установить флаг в false
                {
                    this.shooting = false;
                }
                
            }
        }

        /// <summary>
        /// Коллекция оружия в оружейной системе
        /// </summary>
        private List<Weapon> weaponsCollection;

        /// <summary>
        /// Максимальное количество оружия 
        /// </summary>
        private int maxWeaponsCount;

        /// <summary>
        /// Текущее количество оружия
        /// </summary>
        public int MaxWeaponsCount
        {
            get { return this.maxWeaponsCount; }
        }

        /// <summary>
        /// Количество занятых оружейных слотов
        /// </summary>
        public int WeaponsCount
        {
            get { return this.weaponsCollection.Count; }
        }

        /// <summary>
        /// Индекс активного оружия в коллекции
        /// </summary>
        private int indexOfActiveWeapon;

        /// <summary>
        /// Индекс активного оружия в коллекции
        /// </summary>
        public int IndexOfActiveWeapon
        {
            get { return this.indexOfActiveWeapon; }
        }

        /// <summary>
        /// Таймер ведения огня
        /// </summary>
        private Clock shootingTimer;

        /// <summary>
        /// Активное оружие
        /// </summary>
        private Weapon ActiveWeapon
        {
            get { return this.weaponsCollection[this.indexOfActiveWeapon]; }
        }

        /// <summary>
        /// Конструктор оружейной системы
        /// </summary>
        /// <param name="weaponCount">Стартовое количество оружейных слотов</param>
        public WeaponSystem(int weaponCount)
        {
            this.maxWeaponsCount = weaponCount;
            this.weaponsCollection = new List<Weapon>();
            this.shootingTimer = new Clock();
        }

        /// <summary>
        /// Процесс работы оружейной системы
        /// </summary>
        /// <param name="shooter">Корабль стрелок</param>
        /// <returns>Снаряд или null, если огонь не ведется или не может быть открыт</returns>
        public Shell Process(ActiveObject shooter)
        {
            if (this.shooting)//если ведется огонь
            {
                if (this.shootingTimer.ElapsedTime.AsMilliseconds() > this.ActiveWeapon.ShootingTimeDelay)//и если прошла задержка между выстрелами
                {
                    this.shootingTimer.Restart();//то перезапустить таймер
                    return this.weaponsCollection[this.indexOfActiveWeapon].Shoot(shooter);//и вернуть снаряд
                } 
            }
            return null;//огонь не ведется = вернуть null
        }

        /// <summary>
        /// Увеличить возможное количество оружия
        /// </summary>
        /// <returns></returns>
        public void AddWeaponSlot()
        {
            this.maxWeaponsCount ++;
        }

        /// <summary>
        /// Уменьшить возможное количество оружия
        /// </summary>
        /// <returns>true - удалось, false - не удалось, все оружейные места заняты</returns>
        public bool RemoveWeaponSlot()
        {
            if (this.maxWeaponsCount > this.WeaponsCount)
            {
                this.maxWeaponsCount --;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Добавить новое оружие
        /// </summary>
        /// <returns>true - удалось, false - не удалось, все оружейные места заняты</returns>
        public bool AddWeapon(Weapon weapon)
        {
            if (this.maxWeaponsCount > this.WeaponsCount)
            {
                this.weaponsCollection.Add(weapon);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Заменить оружие в коллекции
        /// </summary>
        /// <param name="weapon">Новое оружие</param>
        /// <param name="index">Индекс заменяемого оружия</param>
        /// <returns>true - удалось, false - не удалось, в коллекции нет оружия с таким индексом</returns>
        public bool ChangeWeapon(Weapon weapon, int index)
        {
            if (index < this.WeaponsCount)
            {
                this.weaponsCollection[index] = weapon;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Удалить оружие из коллекции
        /// </summary>
        /// <param name="index">Индекс удаляемого оружия</param>
        /// <returns>true - удалось, false - не удалось, в коллекции нет оружия с таким индексом</returns>
        public bool RemoveWeapon(int index)
        {
            if (index < this.WeaponsCount)
            {
                this.weaponsCollection.RemoveAt(index);
                this.indexOfActiveWeapon = 0;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Выбрать активное оружие
        /// </summary>
        /// <param name="index">Новый индекс активного оружия</param>
        /// <returns>true - удалось, false - не удалось, в коллекции нет оружия с таким индексом</returns>
        public bool SetActiveWeaponIndex(int newIndex)
        {
            if (this.WeaponsCount > newIndex)
            {
                this.indexOfActiveWeapon = newIndex;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Получить текущий боезапас в процентах от максимального
        /// </summary>
        /// <returns>Текущий боезапас в процентах</returns>
        public int GetAmmoPersent()
        {
            if (this.WeaponsCount > 0)
            {
                return (this.weaponsCollection[this.indexOfActiveWeapon].Ammo * 100) / this.weaponsCollection[this.indexOfActiveWeapon].MaxAmmo;
            }
            return 0;
        }

    }
}