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
    class PlayerStateIndicator
    {

        /// <summary>
        /// Части интерфейса игрока
        /// </summary>
        enum InterfaceForms : int
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
        /// Набор форм интерфейса игрока
        /// </summary>
        private List<Form> playerInterface = new List<Form>();

        /// <summary>
        /// Указатель на главную форму
        /// </summary>
        private MainForm gameInterface;

        /// <summary>
        /// Конструктор интерфейса
        /// </summary>
        /// <param name="gameInterface"></param>
        public PlayerStateIndicator(MainForm gameInterface)
        {
            this.gameInterface = gameInterface;

        }



    }
}
