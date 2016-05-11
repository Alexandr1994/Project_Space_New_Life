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
        protected ObjectContainer ObjectContainer = null;

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
                this.ObjectContainer.ControllingObject.MoveManager.GiveRotationThrust(this.ObjectContainer.ControllingObject, -1);
            }
            if (RightRotate)
            {
                this.ObjectContainer.ControllingObject.MoveManager.GiveRotationThrust(this.ObjectContainer.ControllingObject, 1);
            }
            if (Forward)
            {
                this.ObjectContainer.ControllingObject.MoveManager.GiveForwardThrust(this.ObjectContainer.ControllingObject);
            }
            if (Reverse)
            {
                this.ObjectContainer.ControllingObject.MoveManager.GiveReversThrust(this.ObjectContainer.ControllingObject);
            }
            if (LeftFly)
            {
                this.ObjectContainer.ControllingObject.MoveManager.GiveSideThrust(this.ObjectContainer.ControllingObject, -1);
            }
            if (RightFly)
            {
                this.ObjectContainer.ControllingObject.MoveManager.GiveSideThrust(this.ObjectContainer.ControllingObject, 1);
            }
            if (StopMoving)
            {
                this.ObjectContainer.ControllingObject.MoveManager.FullStop(this.ObjectContainer.ControllingObject);
            }
        }

        /// <summary>
        /// Инкремент счетчика побед
        /// </summary>
        public void AddWin()
        {
            this.ObjectContainer.AddWin();
        }


        /// <summary>
        /// Процесс работы контроллера
        /// </summary>
        public abstract void Process(List<ObjectSignature> signaturesCollection);

        /// <summary>
        /// Переодическое обнуление флагов контроллера
        /// </summary>
        protected void RefreshFlags()
        {
           ;
        }

    }
}
