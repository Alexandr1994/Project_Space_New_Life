using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
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

        }

        /// <summary>
        /// Получение экзепляра класса игрового контроллера
        /// </summary>
        /// <param name="playerShip"></param>
        /// <returns></returns>
        static PlayerController GetInstanse(Ship playerShip)
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










        public override void Process()
        {
            throw new NotImplementedException();
        }
    }
}
