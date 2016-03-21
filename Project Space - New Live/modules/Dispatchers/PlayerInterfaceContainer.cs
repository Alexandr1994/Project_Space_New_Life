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
        private PlayerContainer[] playerContainers = new PlayerContainer[2];

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
        /// <param name="playerContainers">Контейнер игрока</param>
        public PlayerInterfaceContainer(MainForm mainForm, PlayerContainer[] playerContainers)
        {
            this.mainForm = mainForm;
            this.playerContainers = playerContainers;
            this.InterfaceConstruct();
        }

        /// <summary>
        /// Построение интерфейса
        /// </summary>
        private void InterfaceConstruct()
        {
            Button circleBtn = new CircleButton();
            LinearBar linearBar = new LinearBar();

            //Формы отображения состояния игрока 1
            this.formsCollection.Add("RadarScreen1", new RadarScreen());//Экран радара
            linearBar.Location = new Vector2f(200, 0);//Индиктор прочности
            linearBar.SetTexturets(new Texture[] { null, ResurceStorage.healthBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("HealthBar1", linearBar);
            linearBar = new LinearBar();//Индикатор энергии
            linearBar.Location = new Vector2f(200, 20);
            linearBar.SetTexturets(new Texture[] { null, ResurceStorage.energyBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("EnergyBar1", linearBar);
            linearBar = new LinearBar();//Индикатор боезапаса
            linearBar.Location = new Vector2f(200, 40);
            linearBar.SetTexturets(new Texture[] { null, ResurceStorage.ammoBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("AmmoBar1", linearBar);
            linearBar = new LinearBar();//Индикатор защиты
            linearBar.Location = new Vector2f(200, 60);
            linearBar.SetTexturets(new Texture[] { null, ResurceStorage.protectBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("ProtectBar1", linearBar);
            circleBtn.Location = new Vector2f(200, 85);//Кнопка центрирования относительно Игрока 1
            circleBtn.FontSize = 10;
            circleBtn.Text = "Отцентрировать";
            circleBtn.MouseClick += this.CenterOnPlayer1;
            this.formsCollection.Add("centerBtn1",circleBtn);
            circleBtn = new CircleButton();//Кнопка переключения управления Игрока 1
            circleBtn.FontSize = 9;
            circleBtn.Text = "Ручное управление";
            circleBtn.Location = new Vector2f(200, 120);
            circleBtn.MouseClick += this.HandControlPlayer1;
            this.formsCollection.Add("controlBtn1", circleBtn);

            
            //Формы отображения состояния игрока 2
            this.formsCollection.Add("RadarScreen2", new RadarScreen());//Экран радара
            this.formsCollection["RadarScreen2"].Location = this.mainForm.Size - new Vector2f(170, 170);
            linearBar = new LinearBar();//Индиктор прочности
            linearBar.Location = this.mainForm.Size - new Vector2f(200 + 200, 80);
            linearBar.SetTexturets(new Texture[] { null, ResurceStorage.healthBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("HealthBar2", linearBar);
            linearBar = new LinearBar();//Индикатор энергии
            linearBar.Location = this.mainForm.Size - new Vector2f(200 + 200, 60);
            linearBar.SetTexturets(new Texture[] { null, ResurceStorage.energyBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("EnergyBar2", linearBar);
            linearBar = new LinearBar();//Индикатор боезапаса
            linearBar.Location = this.mainForm.Size - new Vector2f(200 + 200, 40);
            linearBar.SetTexturets(new Texture[] { null, ResurceStorage.ammoBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("AmmoBar2", linearBar);
            linearBar = new LinearBar();//Индикатор защиты
            linearBar.Location = this.mainForm.Size - new Vector2f(200 + 200, 20);
            linearBar.SetTexturets(new Texture[] { null, ResurceStorage.protectBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("ProtectBar2", linearBar);
            circleBtn = new CircleButton();//Кнопка центрирования относительно Игрока 2
            circleBtn.FontSize = 10;
            circleBtn.Text = "Отцентрировать";
            circleBtn.Location = this.mainForm.Size - new Vector2f(200, 85) - circleBtn.Size;
            circleBtn.MouseClick += this.CenterOnPlayer2;
            this.formsCollection.Add("centerBtn2", circleBtn);
            circleBtn = new CircleButton();//Кнопка переключения управления Игрока 2
            circleBtn.FontSize = 9;
            circleBtn.Text = "Ручное управление";
            circleBtn.Location = this.mainForm.Size - new Vector2f(200, 120) - circleBtn.Size; ;
            circleBtn.MouseClick += this.HandControlPlayer2;
            this.formsCollection.Add("controlBtn2", circleBtn);


            this.formsCollection.Add("EndButton", new RectButton());//Кнопка окончания
            (this.formsCollection["EndButton"] as Button).Text = "Выход";
            (this.formsCollection["EndButton"] as Button).FontSize = 24;
            this.formsCollection["EndButton"].MouseClick += OnClick;
            this.formsCollection["EndButton"].Size = new Vector2f(80, 30);
            this.formsCollection["EndButton"].Location = new Vector2f(this.mainForm.Size.X - 80, 0);



            foreach (KeyValuePair<String, Form> form in this.formsCollection)//добавление форм интерфейса на главную форму
            {
                this.mainForm.AddForm(form.Value);
            }
        }

        /// <summary>
        /// Установить/Сбросить центрирование относительно Игрока 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CenterOnPlayer1(object sender, EventArgs e)
        {
            if (this.playerContainers[0].ViewCentering)//если камера центрирована относитьльно Игрока
            {
                this.playerContainers[0].UnsetViewCentering();//Сбросить центрирование
            }
            else
            {
                this.playerContainers[0].SetViewCentering();//Иначе отцентрировать камеру отнсительно Игрока
            }
            this.playerContainers[1].UnsetViewCentering();//Сбросить центрирование камеры относительно Игрока-противника
        }

        /// <summary>
        /// Установить/Сбросить центрирование относительно Игрока 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CenterOnPlayer2(object sender, EventArgs e)
        {
            if (this.playerContainers[1].ViewCentering)//если камера центрирована относитьльно Игрока
            {
                this.playerContainers[1].UnsetViewCentering();//Сбросить центрирование
            }
            else
            {
                this.playerContainers[1].SetViewCentering();//Иначе отцентрировать камеру отнсительно Игрока
            }
            this.playerContainers[0].UnsetViewCentering();//Сбросить центрирование камеры относительно Игрока-противника
        }

        /// <summary>
        /// Установить/Сбросить ручное управление Игроком 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandControlPlayer1(object sender, EventArgs e)
        {
            this.playerContainers[1].UnsetHandControlling();//установить Игроку-противнику нейроконтроллер
            if (this.playerContainers[0].HandControlling)//если Игрок управляется в ручную
            {
                this.playerContainers[0].UnsetHandControlling();//установить в качестве контроллера нейроконтроллер
            }
            else
            {
                this.playerContainers[0].SetHandControlling();//иначе установить ручное управление
            }
        }

        /// <summary>
        /// Установить/Сбросить ручное управление Игроком 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandControlPlayer2(object sender, EventArgs e)
        {
            this.playerContainers[0].UnsetHandControlling();//установить Игроку-противнику нейроконтроллер
            if (this.playerContainers[1].HandControlling)//если Игрок управляется в ручную
            {
                this.playerContainers[1].UnsetHandControlling();//установить в качестве контроллера нейроконтроллер
            }
            else
            {
                this.playerContainers[1].SetHandControlling();//иначе установить ручное управление
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
            //Процесс отображения состояния Игрока 1
            this.playerContainers[0].ViewCenteringFunc();
            (this.formsCollection["RadarScreen1"] as RadarScreen).RadarProcess(this.playerContainers[0].Environment, this.playerContainers[0].ControllingObject);
            (this.formsCollection["HealthBar1"] as LinearBar).PercentOfBar = this.playerContainers[0].GetHealh();
            (this.formsCollection["EnergyBar1"] as LinearBar).PercentOfBar = this.playerContainers[0].GetEnergy();
            (this.formsCollection["ProtectBar1"] as LinearBar).PercentOfBar = this.playerContainers[0].GetShieldPower();
            (this.formsCollection["AmmoBar1"] as LinearBar).PercentOfBar = this.playerContainers[0].GetWeaponAmmo();
            //Процесс отображения состояния Игрока 2
            this.playerContainers[1].ViewCenteringFunc();
            (this.formsCollection["RadarScreen2"] as RadarScreen).RadarProcess(this.playerContainers[1].Environment, this.playerContainers[1].ControllingObject);
            (this.formsCollection["HealthBar2"] as LinearBar).PercentOfBar = this.playerContainers[1].GetHealh();
            (this.formsCollection["EnergyBar2"] as LinearBar).PercentOfBar = this.playerContainers[1].GetEnergy();
            (this.formsCollection["ProtectBar2"] as LinearBar).PercentOfBar = this.playerContainers[1].GetShieldPower();
            (this.formsCollection["AmmoBar2"] as LinearBar).PercentOfBar = this.playerContainers[1].GetWeaponAmmo();
        }



    }
}
