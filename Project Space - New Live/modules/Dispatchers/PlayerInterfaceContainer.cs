using System;
using Project_Space___New_Live.modules.Controlers.Forms;
using Project_Space___New_Live.modules.Controlers.InterfaceParts;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Контейнер игрового интерфейса
    /// </summary>
    class PlayerInterfaceContainer
    {

        /// <summary>
        /// Контейнер игрока
        /// </summary>
        private PlayerContainer playerContainer;


        // Набор форм интерфейса игрока
        /// <summary>
        /// Экран радара
        /// </summary>
        private RadarScreen radarScr;

        public RadarScreen RadarScr
        {
            get { return this.radarScr; }
        }
        
        private LinearBar[] stateBars = new LinearBar[3];

        /// <summary>
        /// Указатель на главную форму
        /// </summary>
        private MainForm gameInterface;

        /// <summary>
        /// Конструктор интерфейса
        /// </summary>
        /// <param name="gameInterface"></param>
        public PlayerInterfaceContainer(MainForm gameInterface, PlayerContainer playerContainer)
        {
            this.gameInterface = gameInterface;
            gameInterface.AddForm(this.radarScr = new RadarScreen());
            this.playerContainer = playerContainer;
            this.TempInterfaceConstruct();
        }

        private void TempInterfaceConstruct()
        {
            stateBars[0] = new LinearBar();
            stateBars[0].Location = new Vector2f(200, 0);
            stateBars[0].SetTexturets(new Texture[] { ResurceStorage.PanelText, ResurceStorage.healthBar });
            this.gameInterface.AddForm(stateBars[0]);

            stateBars[1] = new LinearBar();
            stateBars[1].Location = new Vector2f(200, 20);
            stateBars[1].SetTexturets(new Texture[] { ResurceStorage.PanelText, ResurceStorage.energyBar });
            this.gameInterface.AddForm(stateBars[1]);

            stateBars[2] = new LinearBar();
            stateBars[2].Location = new Vector2f(200, 40);
            stateBars[2].SetTexturets(new Texture[] { ResurceStorage.PanelText, ResurceStorage.energyBar });
            this.gameInterface.AddForm(stateBars[2]);

            CircleButton button = new CircleButton();
            button.MouseClick += OnClick;
            button.Size = new Vector2f(80, 30);
            button.Location = new Vector2f(720, 0);
            this.gameInterface.AddForm(button);
        }

        private void OnClick(object sender, EventArgs e)
        {
            cont = false;
        }

        public bool cont = true;

        public void Process()
        {
            this.playerContainer.Process();
            this.RadarScr.RadarProcess(this.playerContainer.ActiveSystem, this.playerContainer.PlayerShip);
            stateBars[0].PercentOfBar = this.playerContainer.GetHealh();
            stateBars[1].PercentOfBar = this.playerContainer.GetEnergy();
            stateBars[2].PercentOfBar = this.playerContainer.GetShieldPower();

        }
    }
}
