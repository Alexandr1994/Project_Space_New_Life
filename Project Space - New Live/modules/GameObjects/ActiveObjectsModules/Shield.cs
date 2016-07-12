using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Project_Space___New_Live.modules.GameObjects
{
    public class Shield : Equipment
    {

        public override bool State
        { 
            get { return this.state; }
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
            this.shieldPower = this.baseMaxShieldPower = this.maxShieldPower = maxShieldPower;
            this.shieldRegeneration = shieldRegeneration;
            this.baseShieldRegeneration = baseShieldRegeneration;
            this.state = false;
        }

        /// <summary>
        /// Активация щита
        /// </summary>
        /// <returns>true - удалось активировать, false - не удалось</returns>
        public override bool Activate()
        {
            if (!this.emergensyState && this.shieldPower > 0)//если щит не в аварийном состоянии
            {//то установить текущую мощность экрана в максимальную и установить состояние в активное
                this.shieldPower = this.maxShieldPower;
                this.state = true;
                return true;//вернуть true
            }
            return false;//иначе активироать щит не удалось, вернуть false
        }

        /// <summary>
        /// Деактивация щита
        /// </summary>
        public override void Deactivate()
        {
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
                this.Deactivate();
                this.shieldPower = 0;
            }
            this.Wearing(equipmentDamage);
            if (emergensyState)
            {
                this.Deactivate();
                this.shieldPower = 0;
            }
        }

        /// <summary>
        /// Изменение конкретных характеристик энергощита
        /// </summary>
        protected override void CustomModification()
        {
            this.maxShieldPower = this.baseMaxShieldPower + (this.Version * this.baseMaxShieldPower);
            this.shieldRegeneration = this.Version * this.baseShieldRegeneration;
        }

        /// <summary>
        /// Восстановление ресурса щита
        /// </summary>
        public void RegenerateShield(int recoveryValue)
        {
            if (this.shieldPower + recoveryValue < this.MaxShieldPower)
            {
                this.shieldPower += recoveryValue;
            }
            else
            {
                this.shieldPower = this.MaxShieldPower;
            }
        }

    }
}
