using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;

namespace Project_Space___New_Live.modules.Controlers
{
    /// <summary>
    /// Аьъбстрактная система управления кораблем (контроллер)
    /// </summary>
    public abstract class AbstractController
    {
        /// <summary>
        /// Ссылка на хранилище управляемых объектов
        /// </summary>
        protected PlayerContainer playerContainer = null;

        //Общие флаги управления

        //Управление движением
        protected bool Forward = false;
        protected bool Reverse = false;
        protected bool LeftFly = false;
        protected bool RightFly = false;
        protected bool LeftRotate = false;
        protected bool RightRotate = false;
        protected bool StopMoving = false;

        /// <summary>
        /// Обработка движений корабля
        /// </summary>
        protected void Moving()
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
        public abstract void Process();

        /// <summary>
        /// Переодическое обнуление флагов контроллера
        /// </summary>
        protected void RefreshFlags()
        {
           ;
        }

    }
}
