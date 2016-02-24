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
    /// Игровой контроллер непосредственного управления объектом 
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
        /// Ссылка на хранилище управляемых объектов
        /// </summary>
        private PlayerContainer playerContainer = null;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="playerObject">Корабль игрока</param>
        private PlayerController(PlayerContainer playerContainer)
        {
            this.playerContainer = playerContainer;
            this.GameRenderer = RenderModule.getInstance();//Получение класса отрисовщика
            this.GameRenderer.MainWindow.KeyPressed += this.OnKey;
            this.GameRenderer.MainWindow.KeyReleased += this.FromKey;
            this.GameRenderer.Form.MouseDown += this.OnButton;
            this.GameRenderer.Form.MouseUp += this.FromButton;
            this.GameRenderer.Form.MouseMove += this.OnMouseMove;
            this.GameRenderer.Form.MouseOut += this.OnMouseOut;
        }

        /// <summary>
        /// Получение экзепляра класса игрового контроллера
        /// </summary>
        /// <param name="playerShip">Корабль игрока</param>
        /// <returns>Ссылка на экземпляр игрового контроллера</returns>
        public static PlayerController GetInstanse(PlayerContainer playerContainer)
        {
            if (GameController == null)
            {
                GameController = new PlayerController(playerContainer);
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
                case Keyboard.Key.Space:
                {
                    if (this.NearPlanet)
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
                    this.playerContainer.ActiveTransport.OpenFire();
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
                    this.playerContainer.ActiveTransport.StopFire();
                }; break;
                default: break;
            }
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


        private void OnMouseOut(object sender, MouseMoveEventArgs Args)
        {
            this.playerContainer.ActiveTransport.StopFire();
        }

        /// <summary>
        /// Обработка движений корабля
        /// </summary>
        private void Moving()
        {
            if (Forward)
            {
                this.playerContainer.ActiveTransport.MoveManager.GiveForwardThrust(this.playerContainer.ActiveTransport);
            }
            if (Reverse)
            {
                this.playerContainer.ActiveTransport.MoveManager.GiveReversThrust(this.playerContainer.ActiveTransport);
            }
            if (LeftFly)
            {
                this.playerContainer.ActiveTransport.MoveManager.GiveSideThrust(this.playerContainer.ActiveTransport, -1);
            }
            if (RightFly)
            {
                this.playerContainer.ActiveTransport.MoveManager.GiveSideThrust(this.playerContainer.ActiveTransport, 1);
            }
            if (LeftRotate)
            {
                this.playerContainer.ActiveTransport.MoveManager.GiveRotationThrust(this.playerContainer.ActiveTransport, -1);
            }
            if (RightRotate)
            {
                this.playerContainer.ActiveTransport.MoveManager.GiveRotationThrust(this.playerContainer.ActiveTransport, 1);
            }
            if (StopMoving)
            {
                this.playerContainer.ActiveTransport.MoveManager.FullStop(this.playerContainer.ActiveTransport);
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
            if (this.playerContainer.ActiveTransport.ShieldActive)//Если энергощит активен
            {
                this.playerContainer.ActiveTransport.DeactivateShield();//деактивировать его
            }
            else
            {
                this.playerContainer.ActiveTransport.ActivateShield();//иначе активировать
            }
        }
    }
}
