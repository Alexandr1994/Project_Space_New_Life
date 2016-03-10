using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Controlers.Forms;

namespace Project_Space___New_Live.modules.Controlers.InterfaceParts
{
    /// <summary>
    /// Контейнер форм интерфейса пользователя
    /// </summary>
    class PlayerStateIndicator : ImageForm
    {

        /// <summary>
        /// Части интерфейса игрока
        /// </summary>
        public enum InterfaceForms : int
        {
            /// <summary>
            /// Индикатор запаса прочности
            /// </summary>
            HealthBar = 0,
            /// <summary>
            /// Индикатор запаса энергии
            /// </summary>
            EnergyBar,
            /// <summary>
            /// Индикатор состояния энергощита
            /// </summary>
            ShieldBar,
            /// <summary>
            /// Идикатор боезапаса текущего оружия
            /// </summary>
            AmmoBar
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        protected override void CustomConstructor()
        {
            ;
        }

    }
}
