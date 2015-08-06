using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using Project_Space___New_Live.modules.GameObjects.ShipModules;
using SFML.Graphics;
using SFML.Window;

namespace Project_Space___New_Live.modules.Controlers
{
    public class PlayerController : AbstractController
    {

        /// <summary>
        /// Ссылка на экземпляр класса-отрисовщика 
        /// </summary>
        private RenderClass GameRenderer;

        /// <summary>
        /// Экземпляр класса-игрового контроллера
        /// </summary>
        static private PlayerController GameController;

        /// <summary>
        /// Ссылка на корабль
        /// </summary>
        private Ship PlayerShip = null;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="playerShip">Корабль игрока</param>
        private PlayerController(Ship playerShip)
        {
            this.PlayerShip = playerShip;
            GameRenderer = RenderClass.getInstance();//Получение класса отрисовщика
            GameRenderer.MainWindow.KeyPressed += OnKey;
            GameRenderer.MainWindow.KeyReleased += FromKey;
        }

        /// <summary>
        /// Получение экзепляра класса игрового контроллера
        /// </summary>
        /// <param name="playerShip"></param>
        /// <returns></returns>
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
        /// <param name="sender"></param>
        /// <param name="Args"></param>
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
                default: break;   
            }
        }

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
                this.PlayerShip.MoveManager.GiveSideThrust(this.PlayerShip, 1);
            }
            if (RightFly)
            {
                this.PlayerShip.MoveManager.GiveSideThrust(this.PlayerShip, -1);
            }
            if (LeftRotate)
            {
                this.PlayerShip.Rotate(1);
            }
            if (RightRotate)
            {
                this.PlayerShip.Rotate(-1);
            }
            if (StopMoving)
            {
                this.PlayerShip.MoveManager.FullStop(this.PlayerShip);
            }
            if (!RightRotate && !LeftRotate)
            {
                this.PlayerShip.StopRotation();
            }
            
        }

        public override void Process()
        {
            Moving();
            GameRenderer.GameView.Center = this.PlayerShip.Coords;
        }
    }
}
