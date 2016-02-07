using Project_Space___New_Live.modules.Controlers.Forms;
using Project_Space___New_Live.modules.Controlers.InterfaceParts;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Контейнер игрового интерфейса
    /// </summary>
    class PlayerInterfaceContainer
    {

        // Набор форм интерфейса игрока
        /// <summary>
        /// Экран радара
        /// </summary>
        private RadarScreen radarScr;

        public RadarScreen RadarScr
        {
            get { return this.radarScr; }
        }
        

        /// <summary>
        /// Указатель на главную форму
        /// </summary>
        private MainForm gameInterface;

        /// <summary>
        /// Конструктор интерфейса
        /// </summary>
        /// <param name="gameInterface"></param>
        public PlayerInterfaceContainer(MainForm gameInterface)
        {
            this.gameInterface = gameInterface;
            gameInterface.AddForm(this.radarScr = new RadarScreen());
        }


        

    }
}
