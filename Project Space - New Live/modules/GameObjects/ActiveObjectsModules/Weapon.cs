using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Оружие
    /// </summary>
    public class Weapon : Equipment
    {

        //ХАРАКТЕРИСТИКИ ОРУЖИЯ

        /// <summary>
        /// Минимальный урон, наносимый объекту
        /// </summary>
        private int objectDamageMin;

        /// <summary>
        /// Минимальный урон, наносимый объекту
        /// </summary>
        public int ObjectDamageMin
        {
            get { return this.objectDamageMin; }
        }

        /// <summary>
        /// Разброс урона, наносимого объекту
        /// </summary>
        private int objectDamageRange;

        /// <summary>
        /// Разброс урона, наносимого объекту
        /// </summary>
        public int ObjectDamageRange
        {
            get { return this.objectDamageRange; }
        }

        /// <summary>
        /// Минимальный урон, наносимый оборудованию
        /// </summary>
        private int equipmentDamageMin;

        /// <summary>
        /// Минимальный урон, наносимый оборудованию
        /// </summary>
        public int EquipmentDamageMin
        {
            get { return this.equipmentDamageMin; }
        }

        /// <summary>
        /// Разброс урона, наносимого оборудованию
        /// </summary>
        private int equipmentDamageRange;

        /// <summary>
        /// Разброс урона, наносимого оборудованию
        /// </summary>
        public int EquipmentDamageRange
        {
            get { return this.equipmentDamageRange; }
        }

        /// <summary>
        /// Угол рассеивание снарядов в рад.
        /// </summary>
        private float dispersion;

        /// <summary>
        /// Угол рассеивание снарядов в рад.
        /// </summary>
        public float Dispersion
        {
            get { return this.dispersion; }
        }

        /// <summary>
        /// Текущий боезапас
        /// </summary>
        private int ammo;

        /// <summary>
        /// Текущий боезапас
        /// </summary>
        public int Ammo
        {
            get { return this.ammo; }
        }

        /// <summary>
        /// Максимальный боезапас
        /// </summary>
        private int maxAmmo;

        /// <summary>
        /// Максимальный боезапас
        /// </summary>
        public int MaxAmmo
        {
            get { return this.maxAmmo; }
        }

        /// <summary>
        /// Временная задержка между выстрелами
        /// </summary>
        private int shootingTimeDelay;

        /// <summary>
        /// Временная задержка между выстрелами
        /// </summary>
        public int ShootingTimeDelay
        {
            get { return this.shootingTimeDelay; }
        }

        //ХАРАКТЕРИСТИКИ ВЫСТРЕЛИВАЕМЫХ СНАРЯДОВ

        /// <summary>
        /// Время жизни выстреливаемого снаряда
        /// </summary>
        private int shellLifeTime;

        /// <summary>
        /// Скорость выстреливаемого снаряда
        /// </summary>
        private float shellSpeed;

        /// <summary>
        /// Масса выстреливаемого снаряда
        /// </summary>
        private int shellMass;

        /// <summary>
        /// Размер выстреливаемого снаряда
        /// </summary>
        private Vector2f shellSize;

        /// <summary>
        /// Набор текстур выстреливаемого снаряда
        /// </summary>
        private Texture[] shellTextures;

        /// <summary>
        /// Расход боезапаса за выстрел
        /// </summary>
        private int shootingAmmoNeeds;

        /// <summary>
        /// Конструктор оружия
        /// </summary>
        /// <param name="mass">Базовая масса</param>
        /// <param name="energyNeeds">Энергопотребление</param>
        /// <param name="objectDamageMin">Минимальный урон, наносимый объекту</param>
        /// <param name="objectDamageRange">Разброс урона, наносимого объекту</param>
        /// <param name="equipmentDamageMin">Минимальный урон, наносимый оборудованию</param>
        /// <param name="equipmentDamageRange">Разброс урона, наносимого оборудованию</param>
        /// <param name="dispersion">Угол рассеивания снарядов в рад.</param>
        /// <param name="maxAmmo">Максимальный боезапас</param>
        /// <param name="shootingTimeDelay">Задержка между выстрелами из оружия</param>
        /// <param name="shellLifeTime">Время жизни выстреливаемого снаряда</param>
        /// <param name="shellSpeed">Скорость выстреливаемого снаряда</param>
        /// <param name="shellMass">Масса выстреливаемого снаряда</param>
        /// <param name="shellSize">Размер выстреливаемого снаряда</param>
        /// <param name="shellSkin">Набор текстуо  выстреливаемого снаряда</param>
        /// <param name="image">Отображение оружия</param>
        /// <param name="shootingAmmoNeeds">Затраты боезапаса на выстрел 1 снаряда</param>
        public Weapon(int mass, int energyNeeds, int objectDamageMin, int objectDamageRange, int equipmentDamageMin, int equipmentDamageRange, float dispersion, int maxAmmo, int shootingTimeDelay, int shellLifeTime, float shellSpeed, int shellMass, Vector2f shellSize, Texture[] shellSkin, Shape image, int shootingAmmoNeeds = 1)
        {
            this.SetCommonCharacteristics(mass, energyNeeds, image);//общие характеристики всех видов оборудования
            //Сохранение параметров оружия
            this.objectDamageMin = objectDamageMin;
            this.objectDamageRange = objectDamageRange;
            this.equipmentDamageMin = equipmentDamageMin;
            this.equipmentDamageRange = equipmentDamageRange;
            this.dispersion = dispersion;
            this.maxAmmo = this.ammo = maxAmmo;
            this.shootingTimeDelay = shootingTimeDelay;
            this.shootingAmmoNeeds = shootingAmmoNeeds;
            //Сохранение параметров выстреливаемых снарядов
            this.shellSize = shellSize;
            this.shellLifeTime = shellLifeTime;
            this.shellMass = shellMass;
            this.shellSpeed = shellSpeed;
            this.shellTextures = shellSkin;
        }

        /// <summary>
        /// Вычисление значения характеристики
        /// </summary>
        /// <param name="characteristicBase">Минимальное значение характеристики (база)</param>
        /// <param name="range">Разброс характеристики</param>
        /// <returns>Значение характеристики с учетом разброса</returns>
        private int CalculateCharacteristic(int characteristicBase, int range)
        {
            Random random = new Random();
            return random.Next(characteristicBase, characteristicBase + range);
        }

        /// <summary>
        /// Вычисление значения характеристики
        /// </summary>
        /// <param name="characteristicBase">Базовое значение характеристики</param>
        /// <param name="range">Возможное отклонение характеристики от базового значения</param>
        /// <returns>Значение характеристики с учетом отклонения</returns>
        private float CalculateCharacteristic(float characteristicBase, float range)
        {
            Random random = new Random();
            int sign = 0;//Получение знака откланения
            while (sign == 0)
            {
                sign = random.Next(-1, 1);
            }
            return (float)(characteristicBase + sign * range * random.NextDouble());
        }

        /// <summary>
        /// Выстрелить из оружия (вернуть снаряд)
        /// </summary>
        /// <param name="shooter">Стреляющий объект</param>
        /// <returns>Новый снаряд или null, если оружие в аварийном состоянии или исчерпас боезапас</returns>
        public Shell Shoot(ActiveObject shooter)
        {
            if (!this.emergensyState && this.Ammo > 0)//если оружие в рабочем состоянии и его боезапас не исчерпан
            {//то произвести выстрел
                int objectDamage = this.CalculateCharacteristic(this.ObjectDamageMin, this.objectDamageRange);//вычисление параметров снаряда
                int equipmentDamage = this.CalculateCharacteristic(this.equipmentDamageMin, this.equipmentDamageRange);
                float angle = this.CalculateCharacteristic(shooter.AttackAngle, this.dispersion);
                this.ammo -= this.shootingAmmoNeeds;//уменьшение боезапаса
                return new Shell(shooter, this.shellMass, shooter.Coords, this.shellSize, objectDamage, equipmentDamage, this.shellSpeed, angle, this.shellLifeTime, this.shellTextures);
            }
            return null;//иначе выстрел не может быть произведен
        }

        /// <summary>
        /// Пополнение боезапаса оружия
        /// </summary>
        /// <param name="newAmmo">Новый боезапас</param>
        public void AmmoCharging(int newAmmo)
        {
            if (this.MaxAmmo - this.Ammo > newAmmo)
            {
                this.ammo = this.Ammo + newAmmo;
            }
            this.ammo = this.MaxAmmo;
        }


        protected override void CustomModification()
        {
            ;
        }
    }
}
