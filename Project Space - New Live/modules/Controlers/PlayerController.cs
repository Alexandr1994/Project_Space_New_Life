using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live.modules
{
    /// <summary>
    /// Игровой контроллер непосредственного управления объектом 
    /// </summary>
    public class PlayerController : AbstractController
    {
        /// <summary>
        /// Конетейнер игрока
        /// </summary>
        private PlayerContainer playerContainer;


        /// <summary>
        /// Ссылка на экземпляр класса-отрисовщика 
        /// </summary>
        private RenderModule GameRenderer;

        /// <summary>
        /// Экземпляр класса-игрового контроллера
        /// </summary>
        static private PlayerController GameController;

        /// <summary>
        /// Объект, сохранающий статистические данные
        /// </summary>
        private DataSaver dataSaver;

        /// <summary>
        /// Таймер сохранения статистики
        /// </summary>
        private Clock dataSaverClock;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="playerObject">Корабль игрока</param>
        private PlayerController(PlayerContainer playerContainer)
        {
            this.playerContainer = playerContainer;
            this.dataSaverClock = new Clock();//инициализация таймера записи
            this.GameRenderer = RenderModule.getInstance();//Получение класса отрисовщика
            this.GameRenderer.MainWindow.KeyPressed += this.OnKey;
            this.GameRenderer.MainWindow.KeyReleased += this.FromKey;
            this.GameRenderer.Form.MouseDown += this.OnButton;
            this.GameRenderer.Form.MouseUp += this.FromButton;
            this.GameRenderer.Form.MouseOut += this.OnMouseOut;
            this.GameRenderer.Form.MouseMove += this.OnMouseMove;
        }


        public void SetControllingObject(Transport transport)
        {
            this.ControllingObject = transport;
        }

        /// <summary>
        /// Получение экзепляра класса игрового контроллера
        /// </summary>
        /// <returns>Ссылка на экземпляр ручного контроллера</returns>
        public static PlayerController GetInstanse(PlayerContainer playerContainer)
        {
            if (GameController == null)
            {
                GameController = new PlayerController(playerContainer);
            }
            return GameController;
        }

        /// <summary>
        /// Переустановка Контейнера Игрока
        /// </summary>
        /// <param name="newContainer"></param>
        public void SetNewController(Transport newContainer)
        {
            this.ControllingObject = newContainer;
        }

        /// <summary>
        /// Обрабочик нажатий на клавиши
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="Args">Аргументы</param>
        private void OnKey(object sender, KeyEventArgs Args)
        {
            switch (Args.Code)
            {//(Модификация в скором времени)
                case Keyboard.Key.W:
                {
                    this.Forward = true;
                }; break;
                case Keyboard.Key.S:
                {
                    this.Reverse = true;
                }; break;
                case Keyboard.Key.A :
                {
                    this.LeftRotate = true;
                }; break;
                case Keyboard.Key.D:
                {
                    this.RightRotate = true;
                }; break;
                case Keyboard.Key.Z:
                {
                    this.LeftFly = true;
                }; break;
                case Keyboard.Key.C:
                {
                    this.RightFly = true;
                }; break;
                case  Keyboard.Key.X:
                {
                    this.StopMoving = true;
                }; break;
                case Keyboard.Key.Num1:
                {
                    this.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(0);
                }; break;
                case Keyboard.Key.Num2:
                {
                    this.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(1);
                }; break;
                case Keyboard.Key.Num3:
                {
                    this.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(2);
                }; break;
                case Keyboard.Key.LControl:
                {
                    this.ShieldProcess();
                }; break;
                case Keyboard.Key.Space:
                    {
                        if (this.playerContainer.PlayerShip.NearPlanet)
                        {
                            this.playerContainer.OnPlanetLanding();
                        }
                    }; break;
                default: break;
            }
        }

        /// <summary>
        /// Обработчик отжатий клавиши
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="Args">Аргументы</param>
        private void FromKey(object sender, KeyEventArgs Args)
        {
            switch (Args.Code)
            {//(Модификация в скором времени)
                case Keyboard.Key.W:
                {
                    this.Forward = false;
                }; break;
                case Keyboard.Key.S:
                {
                    this.Reverse = false;
                }; break;
                case Keyboard.Key.A:
                {
                    this.LeftRotate = false;
                }; break;
                case Keyboard.Key.D:
                {
                    this.RightRotate = false;
                }; break;
                case Keyboard.Key.Z:
                {
                    this.LeftFly = false;
                }; break;
                case Keyboard.Key.C:
                {
                    this.RightFly = false;
                }; break;
                case Keyboard.Key.X:
                {
                    this.StopMoving = false;
                }; break;
                default: break;
            }
        }

        /// <summary>
        /// Обработка нажатий на кнопки мыши
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="Args">Аргументы</param>
        private void OnButton(object sender, MouseButtonEventArgs Args)
        {
            switch (Args.Button)
            {
                case Mouse.Button.Left:
                {
                    this.ControllingObject.OpenFire();
                }; break;
                default: break;
            }
        }

        /// <summary>
        /// Обработка отжатий кнопки мыши
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="Args">Аргументы</param>
        private void FromButton(object sender, MouseButtonEventArgs Args)
        {
            switch (Args.Button)
            {
                case Mouse.Button.Left:
                {
                    this.ControllingObject.StopFire();
                }; break;
                default: break;
            }
        }

        /// <summary>
        /// Отработка выхода курсора из области главной формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Args"></param>
        private void OnMouseOut(object sender, MouseMoveEventArgs Args)
        {
            this.ControllingObject.StopFire();
        }

        private void OnMouseMove(object sender, MouseMoveEventArgs Args)
        {
            if (this.playerContainer.CurrentMode == PlayerContainer.Mode.TankMode)
            {
                Vector2f coords = this.GameRenderer.GameView.Center - this.GameRenderer.GameView.Size / 2;

                float coordsDifX = -(this.playerContainer.ActiveTransport.Coords.X - coords.X) + (float)(Mouse.GetPosition().X);
                float coordsDifY = -(this.playerContainer.ActiveTransport.Coords.Y - coords.Y) + (float)(Mouse.GetPosition().Y);
                float newAngle = (float)(Math.Atan2(coordsDifY, coordsDifX));
                this.playerContainer.PlayerTank.RotateTurret(newAngle);
            }
        }

        /// <summary>
        /// Процесс работы контроллера
        /// </summary>
        public override void Process(List<ObjectSignature> signaturesCollection)
        {
            this.TimerCheck();
            this.Moving();
        }

        /// <summary>
        /// Работа с энергощитом
        /// </summary>
        private void ShieldProcess()
        {
            if (this.ControllingObject.ShieldActive)//Если энергощит активен
            {
                this.ControllingObject.DeactivateShield();//деактивировать его
            }
            else
            {
                this.ControllingObject.ActivateShield();//иначе активировать
            }
        }

        /// <summary>
        /// Проверка таймера сохранения данных
        /// </summary>
        private void TimerCheck()
        {
            if (this.dataSaverClock.ElapsedTime.AsSeconds() > 60)//если прошла минута с прошлой записи статистических данных
            {
             //   this.dataSaver.WriteData(this.ObjectContainer.WinCount, this.ObjectContainer.DeathCount, 0);//сделать новую запись
                this.dataSaverClock.Restart();//перезапуск таймера
            }
        }

    }
}
