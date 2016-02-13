using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Project_Space___New_Live.modules.GameObjects.ShipModules
{
    class Shield : ShipEquipment
    {

        public override bool State
        { 
            get { return this.state; }
            set
            {
                if (value)
                {
                    this.ActiveShield();
                }
                else
                {   
                    this.DeactiveShield();
                }
            }
        }

        /// <summary>
        /// Направления модификации генератора энергощита
        /// </summary>
        public enum UpgrateDirectionID : int
        {
            /// <summary>
            /// базовая версия
            /// </summary>
            Base = 0,
            /// <summary>
            /// Улучшение характеристик мощности экрана энергощита
            /// </summary>
            DefencivePower,
            /// <summary>
            /// Улучшение характеристик восстановления экрана энергощита
            /// </summary>
            RegenerationPower
        }

        /// <summary>
        /// Текущая мощность экрана щита
        /// </summary>
        private int shieldPower;

        /// <summary>
        /// Текущая мощность экрана щита
        /// </summary>
        public int ShieldPower
        {
            get { return this.shieldPower; }
        }

        /// <summary>
        /// Базовая максимальная мощность экрана энергощита
        /// </summary>
        private int baseMaxShieldPower;

        /// <summary>
        /// Максимальная мощность экрана щита
        /// </summary>
        private int maxShieldPower;

        /// <summary>
        /// Максимальная мощность экрана щита
        /// </summary>
        public int MaxShieldPower
        {
            get { return this.maxShieldPower; }
        }

        /// <summary>
        /// Базовая способность к восстановлению экрана энергощита
        /// </summary>
        private int baseShieldRegeneration;

        /// <summary>
        /// Восстановляемость экрана щита
        /// </summary>
        private int shieldRegeneration;

        /// <summary>
        /// Восстановляемость экрана щита
        /// </summary>
        public int ShieldRegeneration
        {
            get { return this.ShieldRegeneration; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mass">Масса</param>
        /// <param name="energyNeeds">Энергопотребление</param>
        /// <param name="maxShieldPower">Максимальная мощность экрана щита</param>
        /// <param name="shieldRegeneration">Начальная восстанавливаемость экрана щита</param>
        /// <param name="baseShieldRegeneration">Базовая восстанавливаемость экрана щита</param>
        /// <param name="image">Изображение</param>
        public Shield(int mass, int energyNeeds, int maxShieldPower, int shieldRegeneration, int baseShieldRegeneration, Shape image)
        {
            this.SetCommonCharacteristics(mass, energyNeeds, image);//сохранение общих характеристик
            //установка текущих характеристик двигательной установки и сохранение базовых характеристик
            this.baseMaxShieldPower = this.maxShieldPower = maxShieldPower;
            this.shieldRegeneration = shieldRegeneration;
            this.baseShieldRegeneration = baseShieldRegeneration;
            this.state = false;
        }

        /// <summary>
        /// Активация щита
        /// </summary>
        private void ActiveShield()
        {
            if (!this.emergensyState)//если щит не в аварийном состоянии
            {//то установить текущую мощность экрана в максимальную и установить состояние в активное
                this.shieldPower = this.maxShieldPower;
                this.state = true;
            }
        }

        /// <summary>
        /// Деактивация щита
        /// </summary>
        private void DeactiveShield()
        {
            this.shieldPower = 0;
            this.state = false;
        }

        /// <summary>
        /// Получение урона экраном щита
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="equipmentDamage"></param>
        public void GetDamageOnShield(int damage, int equipmentDamage)
        {
            if (this.shieldPower - damage > 0)
            {
                this.shieldPower -= damage;
            }
            else
            {
                this.DeactiveShield();
            }
            this.Wearing(equipmentDamage);
            if (emergensyState)
            {
                this.DeactiveShield();
            }
        }

        /// <summary>
        /// Изменение конкретных характеристик энергощита
        /// </summary>
        protected override void CustomModification()
        {
            //определение количества модификаций по возможным направлениям
            int defenciveUpdates = this.upgrateDirectionsHistory.Count(i => i == (int)UpgrateDirectionID.DefencivePower);
            int regenerationUpdates = this.upgrateDirectionsHistory.Count(i => i == (int)UpgrateDirectionID.RegenerationPower);
            //Изменение текущих параметров по направлению улучшения мощности экрана щита
            this.maxShieldPower = this.baseMaxShieldPower + (defenciveUpdates * this.baseMaxShieldPower);

            //Изменение текущих параметров по направлению улучшения восстанавливаемости экрана щита
            this.shieldRegeneration = regenerationUpdates * this.baseShieldRegeneration;

        }
    }
}
