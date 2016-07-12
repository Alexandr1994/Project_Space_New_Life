using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;

namespace Project_Space___New_Live.modules
{
    /// <summary>
    /// Аьъбстрактная система управления кораблем (контроллер)
    /// </summary>
    public abstract class AbstractController
    {
        /// <summary>
        /// Ссылка на хранилище управляемых объектов
        /// </summary>
        protected Transport ControllingObject = null;

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
            if (LeftRotate)
            {
                this.ControllingObject.MoveManager.GiveRotationThrust(this.ControllingObject, -1);
            }
            if (RightRotate)
            {
                this.ControllingObject.MoveManager.GiveRotationThrust(this.ControllingObject, 1);
            }
            if (Forward)
            {
                this.ControllingObject.MoveManager.GiveForwardThrust(this.ControllingObject);
            }
            if (Reverse)
            {
                this.ControllingObject.MoveManager.GiveReversThrust(this.ControllingObject);
            }
            if (LeftFly)
            {
                this.ControllingObject.MoveManager.GiveSideThrust(this.ControllingObject, -1);
            }
            if (RightFly)
            {
                this.ControllingObject.MoveManager.GiveSideThrust(this.ControllingObject, 1);
            }
            if (StopMoving)
            {
                this.ControllingObject.MoveManager.FullStop(this.ControllingObject);
            }
        }

        /// <summary>
        /// Процесс работы контроллера
        /// </summary>
        public abstract void Process(List<ObjectSignature> signaturesCollection);


    }
}
