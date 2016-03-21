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
        private PlayerController()
        {
            this.GameRenderer = RenderModule.getInstance();//Получение класса отрисовщика
            this.GameRenderer.MainWindow.KeyPressed += this.OnKey;
            this.GameRenderer.MainWindow.KeyReleased += this.FromKey;
            this.GameRenderer.Form.MouseDown += this.OnButton;
            this.GameRenderer.Form.MouseUp += this.FromButton;
            this.GameRenderer.Form.MouseOut += this.OnMouseOut;
        }

        /// <summary>
        /// Получение экзепляра класса игрового контроллера
        /// </summary>
        /// <returns>Ссылка на экземпляр ручного контроллера</returns>
        public static PlayerController GetInstanse()
        {
            if (GameController == null)
            {
                GameController = new PlayerController();
            }
            return GameController;
        }

        /// <summary>
        /// Переустановка Контейнера Игрока
        /// </summary>
        /// <param name="newContainer"></param>
        public void SetNewController(PlayerContainer newContainer)
        {
            this.playerContainer = newContainer;
        }


        //Общие флаги управления

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
                case Keyboard.Key.Num1:
                {
                    this.playerContainer.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(0);
                }; break;
                case Keyboard.Key.Num2:
                {
                    this.playerContainer.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(1);
                }; break;
                case Keyboard.Key.Num3:
                {
                    this.playerContainer.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(2);
                }; break;
                case Keyboard.Key.LControl:
                {
                    this.ShieldProcess();
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
                    this.playerContainer.ControllingObject.OpenFire();
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
                    this.playerContainer.ControllingObject.StopFire();
                }; break;
                default: break;
            }
        }


        private void OnMouseOut(object sender, MouseMoveEventArgs Args)
        {
            this.playerContainer.ControllingObject.StopFire();
        }

        /// <summary>
        /// Обработка движений корабля
        /// </summary>
        private void Moving()
        {
            if (Forward)
            {
                this.playerContainer.ControllingObject.MoveManager.GiveForwardThrust(this.playerContainer.ControllingObject);
            }
            if (Reverse)
            {
                this.playerContainer.ControllingObject.MoveManager.GiveReversThrust(this.playerContainer.ControllingObject);
            }
            if (LeftFly)
            {
                this.playerContainer.ControllingObject.MoveManager.GiveSideThrust(this.playerContainer.ControllingObject, -1);
            }
            if (RightFly)
            {
                this.playerContainer.ControllingObject.MoveManager.GiveSideThrust(this.playerContainer.ControllingObject, 1);
            }
            if (LeftRotate)
            {
                this.playerContainer.ControllingObject.MoveManager.GiveRotationThrust(this.playerContainer.ControllingObject, -1);
            }
            if (RightRotate)
            {
                this.playerContainer.ControllingObject.MoveManager.GiveRotationThrust(this.playerContainer.ControllingObject, 1);
            }
            if (StopMoving)
            {
                this.playerContainer.ControllingObject.MoveManager.FullStop(this.playerContainer.ControllingObject);
            }           
        }

        /// <summary>
        /// Процесс работы контроллера
        /// </summary>
        public override void Process()
        {
            this.Moving();
            this.RefreshFlags();
        }

        /// <summary>
        /// Работа с энергощитом
        /// </summary>
        private void ShieldProcess()
        {
            if (this.playerContainer.ControllingObject.ShieldActive)//Если энергощит активен
            {
                this.playerContainer.ControllingObject.DeactivateShield();//деактивировать его
            }
            else
            {
                this.playerContainer.ControllingObject.ActivateShield();//иначе активировать
            }
        }
    }
}
