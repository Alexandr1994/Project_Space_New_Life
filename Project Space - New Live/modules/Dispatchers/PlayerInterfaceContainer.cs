using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RedToolkit;
using Project_Space___New_Live.modules.GameObjects;
using Project_Space___New_Live.modules.Storages;
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
        /// Флаг отрисовки моделей среды и объектов
        /// </summary>
        private bool renderingEnvironment = true;


        /// <summary>
        /// Флаг продолжения игры
        /// </summary>
        public bool GameContinue
        {
            get { return this.gameContinue; }
        }

        /// <summary>
        /// Флаг отрисовки моделей среды и объектов
        /// </summary>
        public bool RenderingEnvironment
        {
            get { return this.renderingEnvironment; }
        }

        /// <summary>
        /// Контейнер игрока
        /// </summary>
        private PlayerContainer playerContainer;

        /// <summary>
        /// Коллекция форм
        /// </summary>
        Dictionary<String ,RedWidget> formsCollection = new Dictionary<string, RedWidget>();  

        /// <summary>
        /// Указатель на главную форму
        /// </summary>
        private MainRedWidget _mainRedWidget;

        /// <summary>
        /// Конструктор интерфейса
        /// </summary>
        /// <param name="_mainRedWidget">Главная форма</param>
        /// <param name="_objectContainers">Контейнер игрока</param>
        public PlayerInterfaceContainer(MainRedWidget _mainRedWidget, PlayerContainer playerContainer)
        {
            this._mainRedWidget = _mainRedWidget;
            this.playerContainer = playerContainer;
            this.InterfaceConstruct();
        }

        private void MoveViewUp(object sender, EventArgs e)
        {
            RenderModule.getInstance().GameView.Center += new Vector2f(0, -20);//Сместить вид вверх 
        }

        private void MoveViewDown(object sender, EventArgs e)
        {
            RenderModule.getInstance().GameView.Center += new Vector2f(0, 20);//Сместить вид вниз 
        }

        private void MoveViewLeft(object sender, EventArgs e)
        {
            RenderModule.getInstance().GameView.Center += new Vector2f(-20, 0);//Сместить вид влево 
        }

        private void MoveViewRight(object sender, EventArgs e)
        {
            RenderModule.getInstance().GameView.Center += new Vector2f(20, 0);//Сместить вид вправо 
        }

        /// <summary>
        /// Построение интерфейса
        /// </summary>
        private void InterfaceConstruct()
        {
            Label label = new Label();
            Button circleBtn = new CircleButton();
            LinearBar linearBar = new LinearBar();

            //Формы отображения состояния игрока 1
            this.formsCollection.Add("RadarScreen", new RadarScreen());//Экран радара
            linearBar.Location = new Vector2f(200, 0);//Индиктор прочности
            linearBar.SetTexturets(new Texture[] { null, ImageStorage.RedYellowBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("HealthBar", linearBar);
            linearBar = new LinearBar();//Индикатор энергии
            linearBar.Location = new Vector2f(200, 20);
            linearBar.SetTexturets(new Texture[] { null, ImageStorage.BlueBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("EnergyBar", linearBar);
            linearBar = new LinearBar();//Индикатор боезапаса
            linearBar.Location = new Vector2f(200, 40);
            linearBar.SetTexturets(new Texture[] { null, ImageStorage.RedWhiteBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("AmmoBar", linearBar);
            linearBar = new LinearBar();//Индикатор защиты
            linearBar.Location = new Vector2f(200, 60);
            linearBar.SetTexturets(new Texture[] { null, ImageStorage.GreenYellowBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("ProtectBar", linearBar);

            
            foreach (KeyValuePair<String, RedWidget> form in this.formsCollection)//добавление форм интерфейса на главную форму
            {
                this._mainRedWidget.AddWidget(form.Value);
            }
        }



        /// <summary>
        /// Нажатие на кнопку окончания игры
        /// </summary>
        /// <param name="sender">Форма, в которой возникло событие</param>
        /// <param name="e">Аргументы события</param>
        private void OnEndClick(object sender, EventArgs e)
        {
            this.gameContinue = false;
        }

        /// <summary>
        /// Нажатие на кнопку графики
        /// </summary>
        /// <param name="sender">Форма, в которой возникло событие</param>
        /// <param name="e">Аргументы события</param>
        private void OnRendClick(object sender, EventArgs e)
        {
            if (this.renderingEnvironment)
            {
                this.renderingEnvironment = false;
            }
            else
            {
                this.renderingEnvironment = true;
            }
        }

        /// <summary>
        /// Процесс работы интерфейса игрока
        /// </summary>
        public void Process()
        {
            //Процесс отображения состояния Игрока 1
            this.playerContainer.Process();
            (this.formsCollection["RadarScreen"] as RadarScreen).RadarProcess(this.playerContainer.ActiveEnvironment, this.playerContainer.PlayerShip);
            (this.formsCollection["HealthBar"] as LinearBar).PercentOfBar = this.playerContainer.GetHealh();
            (this.formsCollection["EnergyBar"] as LinearBar).PercentOfBar = this.playerContainer.GetEnergy();
            (this.formsCollection["ProtectBar"] as LinearBar).PercentOfBar = this.playerContainer.GetShieldPower();
            (this.formsCollection["AmmoBar"] as LinearBar).PercentOfBar = this.playerContainer.GetWeaponAmmo();
            //Процесс отображения состояния Игрока 2
        }



    }
}
