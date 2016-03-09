using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Project_Space___New_Live.modules.Controlers.Forms;
using Project_Space___New_Live.modules.Controlers.InterfaceParts;
using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Контейнер игрового интерфейса
    /// </summary>
    class PlayerInterfaceContainer
    {
        //Флаги

        /// <summary>
        /// Флаг продолжения игры
        /// </summary>
        private bool gameContinue = true;

        /// <summary>
        /// Флаг продолжения игры
        /// </summary>
        public bool GameContinue
        {
            get { return this.gameContinue; }
        }

        /// <summary>
        /// Контейнер игрока
        /// </summary>
        private PlayerContainer playerContainer;

        /// <summary>
        /// Коллекция форм
        /// </summary>
        Dictionary<String ,Form> formsCollection = new Dictionary<string, Form>();  

        /// <summary>
        /// Указатель на главную форму
        /// </summary>
        private MainForm mainForm;

        /// <summary>
        /// Конструктор интерфейса
        /// </summary>
        /// <param name="mainForm">Главная форма</param>
        /// <param name="playerContainer">Контейнер игрока</param>
        public PlayerInterfaceContainer(MainForm mainForm, PlayerContainer playerContainer)
        {
            this.mainForm = mainForm;
            this.playerContainer = playerContainer;
            this.InterfaceConstruct();
        }

        /// <summary>
        /// Построение интерфейса
        /// </summary>
        private void InterfaceConstruct()
        {
            this.formsCollection.Add("RadarScreen", new RadarScreen());//Экран радара
            
            this.formsCollection.Add("HealthBar", new LinearBar());//Индиктор прочности
            this.formsCollection["HealthBar"].Location = new Vector2f(200, 0);
            (this.formsCollection["HealthBar"] as LinearBar).SetTexturets(new Texture[] { ResurceStorage.PanelText, ResurceStorage.healthBar });
            

            this.formsCollection.Add("EnergyBar", new LinearBar());//Индикатор энергии
            this.formsCollection["EnergyBar"].Location = new Vector2f(200, 25);
            (this.formsCollection["EnergyBar"] as LinearBar).SetTexturets(new Texture[] { ResurceStorage.PanelText, ResurceStorage.energyBar });

            this.formsCollection.Add("ProtectBar", new LinearBar());//Индикатор защиты
            this.formsCollection["ProtectBar"].Location = new Vector2f(200, 50);
            (this.formsCollection["ProtectBar"] as LinearBar).SetTexturets(new Texture[] { ResurceStorage.PanelText, ResurceStorage.protectBar });

            this.formsCollection.Add("AmmoBar", new LinearBar());//Индикатор защиты
            this.formsCollection["AmmoBar"].Location = new Vector2f(200, 75);
            (this.formsCollection["AmmoBar"] as LinearBar).SetTexturets(new Texture[] { null, ResurceStorage.ammoBar });
            (this.formsCollection["AmmoBar"] as LinearBar).VisibleSubstrate = false;

            this.formsCollection.Add("EndButton", new CircleButton());//Кнопка окончания
            this.formsCollection["EndButton"].MouseClick += OnClick;
            this.formsCollection["EndButton"].Size = new Vector2f(80, 30);
            this.formsCollection["EndButton"].Location = new Vector2f(this.mainForm.Size.X - 80, 0);

            foreach (KeyValuePair<String, Form> form in this.formsCollection)//добавление форм интерфейса на главную форму
            {
                this.mainForm.AddForm(form.Value);
            }
        }

        /// <summary>
        /// Нажатие на кнопку окончания игры
        /// </summary>
        /// <param name="sender">Форма, в которой возникло событие</param>
        /// <param name="e">Аргументы события</param>
        private void OnClick(object sender, EventArgs e)
        {
            this.gameContinue = false;
        }

        /// <summary>
        /// Процесс работы интерфейса игрока
        /// </summary>
        public void Process()
        {
            this.playerContainer.Process();
            (this.formsCollection["RadarScreen"] as RadarScreen).RadarProcess(this.playerContainer.Environment, this.playerContainer.ControllingObject);
            (this.formsCollection["HealthBar"] as LinearBar).PercentOfBar = this.playerContainer.GetHealh();
            (this.formsCollection["EnergyBar"] as LinearBar).PercentOfBar = this.playerContainer.GetEnergy();
            (this.formsCollection["ProtectBar"] as LinearBar).PercentOfBar = this.playerContainer.GetShieldPower();
            (this.formsCollection["AmmoBar"] as LinearBar).PercentOfBar = this.playerContainer.GetWeaponAmmo();
        }



    }
}
