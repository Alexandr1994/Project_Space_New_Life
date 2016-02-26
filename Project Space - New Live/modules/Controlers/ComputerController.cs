using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.GameObjects;
using SFML.System;

namespace Project_Space___New_Live.modules.Controlers
{
    public class ComputerController : AbstractController
    {

        private Transport controlledObject;


        public ComputerController(Transport controlledObject)
        {
            this.controlledObject = controlledObject;
        }

        private void MoveToTarget(Vector2f targetCoords)
        {
            float divX = targetCoords.X - this.controlledObject.Coords.X;
            float divY = targetCoords.Y - this.controlledObject.Coords.Y;
            float distance = (float)(Math.Sqrt(Math.Pow(divX, 2) + Math.Pow(divY, 2)));
            float angle = (float)(Math.Atan2(divY, divX));
            if (angle > 2 * Math.PI)//если угол поворота вектора больше чем 360 градусов
            {
                angle -= (float)(2 * Math.PI);//вернуть его в пределы от 0 до 360 градусов
            }
            this.RotateToTarget(angle);
            if (distance < 100 && distance > 50)
            {
                this.controlledObject.MoveManager.GiveForwardThrust(this.controlledObject);
            }
            if (distance < 50)
            {
                this.controlledObject.MoveManager.GiveForwardThrust(this.controlledObject);
            }
            this.Move();
        }

        private void RotateToTarget(float angle)
        {
            if ((angle - this.controlledObject.Rotation) > (float)(5 * Math.PI / 180))
            {
                this.controlledObject.MoveManager.GiveRotationThrust(this.controlledObject, 1);
            }
            if ((angle - this.controlledObject.Rotation) < (float)(5 * Math.PI / 180))
            {
                this.controlledObject.MoveManager.GiveRotationThrust(this.controlledObject, -1);
            }
        }

        private void Move()
        {
            this.controlledObject.MoveManager.Process(this.controlledObject);
        }

        public override void Process()
        {
         /*   this.MoveToTarget(this.controlledObject.Environment.GetObjectsInEnvironment(this.controlledObject.Coords, 500)[0].Coords);
            this.Move();*/
        }
    }
}
