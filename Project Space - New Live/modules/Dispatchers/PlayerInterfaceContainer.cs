using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Project_Space___New_Live.modules.Forms;
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
        /// Флаг продолжения игры
        /// </summary>
        public bool GameContinue
        {
            get { return this.gameContinue; }
        }

        /// <summary>
        /// Контейнер игрока
        /// </summary>
        private ObjectContainer[] objectContainers = new ObjectContainer[2];

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
        /// <param name="_objectContainers">Контейнер игрока</param>
        public PlayerInterfaceContainer(MainForm mainForm, ObjectContainer[] _objectContainers)
        {
            this.mainForm = mainForm;
            this.objectContainers = _objectContainers;
            this.InterfaceConstruct();
        }

        /// <summary>
        /// Добавление зон сдвига камеры
        /// </summary>
        private void ConstructScrollerAreas()
        {
            int referenceSize = 30;
            Panel panel = new InvisiblePanel();//Зона сдвига вверх и влево
            panel.Size = new Vector2f(referenceSize, referenceSize);
            panel.Location = new Vector2f(0, 0);
            panel.MouseMove += this.MoveViewUp;
            panel.MouseMove += this.MoveViewLeft;
            this.formsCollection.Add("LeftTopScroller", panel);
            panel = new InvisiblePanel();//Зона сдвига влево
            panel.Size = new Vector2f(referenceSize, this.mainForm.Size.Y - referenceSize * 2);
            panel.Location = new Vector2f(0, referenceSize);
            panel.MouseMove += this.MoveViewLeft;
            this.formsCollection.Add("LeftScroller", panel);
            panel = new InvisiblePanel();//Зона сдвига вниз и влево
            panel.Size = new Vector2f(referenceSize, referenceSize);
            panel.Location = new Vector2f(0, this.mainForm.Size.Y - referenceSize);
            panel.MouseMove += this.MoveViewDown;
            panel.MouseMove += this.MoveViewLeft;
            this.formsCollection.Add("LeftDownScroller", panel);
            panel = new InvisiblePanel();//Зона сдвига вверх и вправо
            panel.Size = new Vector2f(referenceSize, referenceSize);
            panel.Location = new Vector2f(this.mainForm.Size.X - referenceSize, 0);
            panel.MouseMove += this.MoveViewUp;
            panel.MouseMove += this.MoveViewRight;
            this.formsCollection.Add("RightTopScroller", panel);
            panel = new InvisiblePanel();//Зона сдвига вправо
            panel.Size = new Vector2f(referenceSize, this.mainForm.Size.Y - referenceSize * 2);
            panel.Location = new Vector2f(this.mainForm.Size.X - referenceSize, referenceSize);
            panel.MouseMove += this.MoveViewRight;
            this.formsCollection.Add("RightScroller", panel);
            panel = new InvisiblePanel();//Зона сдвига вниз и вправо
            panel.Size = new Vector2f(referenceSize, referenceSize);
            panel.Location = new Vector2f(this.mainForm.Size.X - referenceSize, this.mainForm.Size.Y - referenceSize);
            panel.MouseMove += this.MoveViewDown;
            panel.MouseMove += this.MoveViewRight;
            this.formsCollection.Add("RightDownScroller", panel);
            panel = new InvisiblePanel();//Зона сдвига вниз
            panel.Size = new Vector2f(this.mainForm.Size.X - referenceSize * 2, referenceSize);
            panel.Location = new Vector2f(referenceSize, this.mainForm.Size.Y - referenceSize);
            panel.MouseMove += this.MoveViewDown;
            this.formsCollection.Add("DownScroller", panel);
            panel = new InvisiblePanel();//Зона сдвига вверх
            panel.Size = new Vector2f(this.mainForm.Size.X - referenceSize * 2, referenceSize);
            panel.Location = new Vector2f(referenceSize, 0);
            panel.MouseMove += this.MoveViewUp;
            this.formsCollection.Add("UpScroller", panel);
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
            this.ConstructScrollerAreas();
            Label label = new Label();
            Button circleBtn = new CircleButton();
            LinearBar linearBar = new LinearBar();

            //Формы отображения состояния игрока 1
            this.formsCollection.Add("RadarScreen1", new RadarScreen());//Экран радара
            linearBar.Location = new Vector2f(200, 0);//Индиктор прочности
            linearBar.SetTexturets(new Texture[] { null, ImageStorage.RedYellowBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("HealthBar1", linearBar);
            linearBar = new LinearBar();//Индикатор энергии
            linearBar.Location = new Vector2f(200, 20);
            linearBar.SetTexturets(new Texture[] { null, ImageStorage.BlueBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("EnergyBar1", linearBar);
            linearBar = new LinearBar();//Индикатор боезапаса
            linearBar.Location = new Vector2f(200, 40);
            linearBar.SetTexturets(new Texture[] { null, ImageStorage.RedWhiteBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("AmmoBar1", linearBar);
            linearBar = new LinearBar();//Индикатор защиты
            linearBar.Location = new Vector2f(200, 60);
            linearBar.SetTexturets(new Texture[] { null, ImageStorage.GreenYellowBar });
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
            label.TextColor = Color.Red;
            label.Location = new Vector2f(10, 200);
            label.Text = this.objectContainers[0].DeathCount.ToString();
            this.formsCollection.Add("defeatCount1" ,label);
            label = new Label();
            label.TextColor = Color.Green;
            label.Location = new Vector2f(10, 230);
            label.Text = this.objectContainers[0].WinCount.ToString();
            this.formsCollection.Add("winCount1", label);
            
            //Формы отображения состояния игрока 2
            this.formsCollection.Add("RadarScreen2", new RadarScreen());//Экран радара
            this.formsCollection["RadarScreen2"].Location = this.mainForm.Size - new Vector2f(170, 170);
            linearBar = new LinearBar();//Индиктор прочности
            linearBar.Location = this.mainForm.Size - new Vector2f(200 + 200, 80);
            linearBar.SetTexturets(new Texture[] { null, ImageStorage.RedYellowBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("HealthBar2", linearBar);
            linearBar = new LinearBar();//Индикатор энергии
            linearBar.Location = this.mainForm.Size - new Vector2f(200 + 200, 60);
            linearBar.SetTexturets(new Texture[] { null, ImageStorage.BlueBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("EnergyBar2", linearBar);
            linearBar = new LinearBar();//Индикатор боезапаса
            linearBar.Location = this.mainForm.Size - new Vector2f(200 + 200, 40);
            linearBar.SetTexturets(new Texture[] { null, ImageStorage.RedWhiteBar });
            linearBar.VisibleSubstrate = false;
            this.formsCollection.Add("AmmoBar2", linearBar);
            linearBar = new LinearBar();//Индикатор защиты
            linearBar.Location = this.mainForm.Size - new Vector2f(200 + 200, 20);
            linearBar.SetTexturets(new Texture[] { null, ImageStorage.GreenYellowBar });
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
            label = new Label();
            label.TextColor = Color.Red;
            label.Location = new Vector2f(this.mainForm.Size.X - 20, this.mainForm.Size.Y - 200);
            label.Text = this.objectContainers[1].DeathCount.ToString();
            this.formsCollection.Add("defeatCount2", label);
            label = new Label();
            label.TextColor = Color.Green;
            label.Location = new Vector2f(this.mainForm.Size.X - 20, this.mainForm.Size.Y - 230);
            label.Text = this.objectContainers[1].WinCount.ToString();
            this.formsCollection.Add("winCount2", label);


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
            if (this.objectContainers[0].ViewCentering)//если камера центрирована относитьльно Игрока
            {
                this.objectContainers[0].UnsetViewCentering();//Сбросить центрирование
            }
            else
            {
                this.objectContainers[0].SetViewCentering();//Иначе отцентрировать камеру отнсительно Игрока
            }
            this.objectContainers[1].UnsetViewCentering();//Сбросить центрирование камеры относительно Игрока-противника
        }

        /// <summary>
        /// Установить/Сбросить центрирование относительно Игрока 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CenterOnPlayer2(object sender, EventArgs e)
        {
            if (this.objectContainers[1].ViewCentering)//если камера центрирована относитьльно Игрока
            {
                this.objectContainers[1].UnsetViewCentering();//Сбросить центрирование
            }
            else
            {
                this.objectContainers[1].SetViewCentering();//Иначе отцентрировать камеру отнсительно Игрока
            }
            this.objectContainers[0].UnsetViewCentering();//Сбросить центрирование камеры относительно Игрока-противника
        }

        /// <summary>
        /// Установить/Сбросить ручное управление Игроком 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandControlPlayer1(object sender, EventArgs e)
        {
            this.objectContainers[1].UnsetHandControlling();//установить Игроку-противнику нейроконтроллер
            if (this.objectContainers[0].HandControlling)//если Игрок управляется в ручную
            {
                this.objectContainers[0].UnsetHandControlling();//установить в качестве контроллера нейроконтроллер
            }
            else
            {
                this.objectContainers[0].SetHandControlling();//иначе установить ручное управление
            }
        }

        /// <summary>
        /// Установить/Сбросить ручное управление Игроком 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandControlPlayer2(object sender, EventArgs e)
        {
            this.objectContainers[0].UnsetHandControlling();//установить Игроку-противнику нейроконтроллер
            if (this.objectContainers[1].HandControlling)//если Игрок управляется в ручную
            {
                this.objectContainers[1].UnsetHandControlling();//установить в качестве контроллера нейроконтроллер
            }
            else
            {
                this.objectContainers[1].SetHandControlling();//иначе установить ручное управление
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
            this.objectContainers[0].ViewCenteringFunc();
            (this.formsCollection["RadarScreen1"] as RadarScreen).RadarProcess(this.objectContainers[0].Environment, this.objectContainers[0].ControllingObject);
            (this.formsCollection["HealthBar1"] as LinearBar).PercentOfBar = this.objectContainers[0].GetHealh();
            (this.formsCollection["EnergyBar1"] as LinearBar).PercentOfBar = this.objectContainers[0].GetEnergy();
            (this.formsCollection["ProtectBar1"] as LinearBar).PercentOfBar = this.objectContainers[0].GetShieldPower();
            (this.formsCollection["AmmoBar1"] as LinearBar).PercentOfBar = this.objectContainers[0].GetWeaponAmmo();
            (this.formsCollection["defeatCount1"] as Label).Text = this.objectContainers[0].DeathCount.ToString();
            (this.formsCollection["winCount1"] as Label).Text = this.objectContainers[0].WinCount.ToString();
            //Процесс отображения состояния Игрока 2
            this.objectContainers[1].ViewCenteringFunc();
            (this.formsCollection["RadarScreen2"] as RadarScreen).RadarProcess(this.objectContainers[1].Environment, this.objectContainers[1].ControllingObject);
            (this.formsCollection["HealthBar2"] as LinearBar).PercentOfBar = this.objectContainers[1].GetHealh();
            (this.formsCollection["EnergyBar2"] as LinearBar).PercentOfBar = this.objectContainers[1].GetEnergy();
            (this.formsCollection["ProtectBar2"] as LinearBar).PercentOfBar = this.objectContainers[1].GetShieldPower();
            (this.formsCollection["AmmoBar2"] as LinearBar).PercentOfBar = this.objectContainers[1].GetWeaponAmmo();
            (this.formsCollection["defeatCount2"] as Label).Text = this.objectContainers[1].DeathCount.ToString();
            (this.formsCollection["winCount2"] as Label).Text = this.objectContainers[1].WinCount.ToString();
        }



    }
}
