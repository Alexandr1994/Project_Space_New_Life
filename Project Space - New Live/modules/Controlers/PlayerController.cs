using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using Project_Space___New_Live.modules.GameObjects.ShipModules;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live.modules.Controlers
{
    /// <summary>
    /// Игровой контроллер
    /// </summary>
    public class PlayerController : AbstractController
    {
        /// <summary>
        /// Ссылка на экземпляр класса-отрисовщика 
        /// </summary>
        private RenderModule GameRenderer;

        /// <summary>
        /// Экземпляр класса-игрового контроллера
        /// </summary>
        static private PlayerController GameController;

        /// <summary>
        /// Ссылка на управляемый корабль
        /// </summary>
        private Ship PlayerShip = null;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="playerShip">Корабль игрока</param>
        private PlayerController(Ship playerShip)
        {
            this.PlayerShip = playerShip;
            this.GameRenderer = RenderModule.getInstance();//Получение класса отрисовщика
            this.GameRenderer.MainWindow.KeyPressed += this.OnKey;
            this.GameRenderer.MainWindow.KeyReleased += this.FromKey;
            this.GameRenderer.Form.MouseDown += this.OnButton;
            this.GameRenderer.Form.MouseUp += this.FromButton;
        }

        /// <summary>
        /// Получение экзепляра класса игрового контроллера
        /// </summary>
        /// <param name="playerShip">Корабль игрока</param>
        /// <returns>Ссылка на экземпляр игрового контроллера</returns>
        public static PlayerController GetInstanse(Ship playerShip)
        {
            if (GameController == null)
            {
                GameController = new PlayerController(playerShip);
            }
            return GameController;
        }

        //Флаги управления

        //Управление движением
        private bool Forward = false;
        private bool Reverse = false;
        private bool LeftFly = false;
        private bool RightFly = false;
        private bool LeftRotate = false;
        private bool RightRotate = false;
        private bool StopMoving = false;

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
                case Keyboard.Key.LControl:
                    {
                        this.ShieldProcess();
                    }; break;
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
                    this.PlayerShip.OpenFire();
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
                    this.PlayerShip.StopFire();
                }; break;
                default: break;
            }
        }


        /// <summary>
        /// Обработка движений корабля
        /// </summary>
        private void Moving()
        {
            if (Forward)
            {
                this.PlayerShip.MoveManager.GiveForwardThrust(this.PlayerShip);
            }
            if (Reverse)
            {
                this.PlayerShip.MoveManager.GiveReversThrust(this.PlayerShip);
            }
            if (LeftFly)
            {
                this.PlayerShip.MoveManager.GiveSideThrust(this.PlayerShip, -1);
            }
            if (RightFly)
            {
                this.PlayerShip.MoveManager.GiveSideThrust(this.PlayerShip, 1);
            }
            if (LeftRotate)
            {
                this.PlayerShip.MoveManager.GiveRotationThrust(this.PlayerShip, -1);
            }
            if (RightRotate)
            {
                this.PlayerShip.MoveManager.GiveRotationThrust(this.PlayerShip, 1);
            }
            if (StopMoving)
            {
                this.PlayerShip.MoveManager.FullStop(this.PlayerShip);
            }           
        }

        /// <summary>
        /// Процесс работы контроллера
        /// </summary>
        public override void Process()
        {
            this.RefreshFlags();
            this.Moving();
        }

        /// <summary>
        /// Работа с энергощитом
        /// </summary>
        private void ShieldProcess()
        {
            if (this.PlayerShip.ShieldActive)
            {
                this.PlayerShip.ShieldActive = false;
            }
            else
            {
                this.PlayerShip.ShieldActive = true;
            }
        }
    }
}
