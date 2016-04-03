using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using SFML.System;

namespace Project_Space___New_Live.modules.Controlers
{
    public class ComputerController : AbstractController
    {

        public ComputerController(PlayerContainer playerContainer)
        {
            this.playerContainer = playerContainer;
        }

        private void MoveToTarget(Vector2f targetCoords)
        {
           /* float divX = targetCoords.X - this.controlledObject.Coords.X;
            float divY = targetCoords.Y - this.controlledObject.Coords.Y;
            float distance = (float)(Math.Sqrt(Math.Pow(divX, 2) + Math.Pow(divY, 2)));
            float controllingRotation = this.AngleNormalize(this.controlledObject.Rotation);
            float angle = (float)(Math.Atan2(divY, divX));*/
            this.Forward = true;
            this.RotateToTarget(targetCoords, 300);
          //  if (distance < 100 && distance > 50)
           /* {
                this.controlledObject.MoveManager.GiveForwardThrust(this.controlledObject);
            }*/
           /* if (distance > 50)
            {*/
            
            /* }*/

        }

        /// <summary>
        /// Управление поворотом игрока
        /// </summary>
        /// <param name="targetCoords">Координаты цели</param>
        /// <param name="dangerRadius">Радиус опасной зоны, около цели (По-умолчанию 0)</param>
        private void RotateToTarget(Vector2f targetCoords, float dangerRadius = 0)
        {
            Vector2f divCoords = targetCoords - this.playerContainer.ControllingObject.Coords;
            float distance = (float)(Math.Sqrt(Math.Pow(divCoords.X, 2) + Math.Pow(divCoords.Y, 2)));
            float relativeAngle = (float) (Math.Atan2(divCoords.Y,  divCoords.X));
            float angleBetween = this.FindAngleBetweenVectors(this.playerContainer.ControllingObject.Coords, this.playerContainer.ControllingObject.Rotation, targetCoords);
            if (Math.Abs(angleBetween) > 5*Math.PI/180)
            {
                if (distance > dangerRadius)
                {

                    if (Math.Abs(relativeAngle) > Math.PI/2 && angleBetween < Math.PI/2)
                    {
                        return;
                    }
                    if (relativeAngle < this.playerContainer.ControllingObject.Rotation)
                    {
                        this.LeftRotate = true;
                        this.RightRotate = false;
                    }
                    else
                    {
                        this.RightRotate = true;
                        this.LeftRotate = false;
                    }
                    this.LeftFly = false;
                    this.RightFly = false;
                }
                else
                {
                    this.LeftFly = false;
                    this.RightFly = false;
                    if (relativeAngle >= this.playerContainer.ControllingObject.Rotation)
                    {
                        this.LeftRotate = true;
                        this.RightRotate = false;
                    }
                    else
                    {
                        this.RightRotate = true;
                        this.LeftRotate = false;
                    }
                }
            }
            else
            {
                if (distance <= dangerRadius)
                {
                    if (relativeAngle >= this.playerContainer.ControllingObject.Rotation)
                    {
                        this.LeftRotate = true;
                        this.RightRotate = false;
                    }
                    else
                    {
                        this.RightRotate = true;
                        this.LeftRotate = false;
                    } 
                }
                else
                {
                    this.LeftRotate = false;
                    this.RightRotate = false;
                } 
            }
        }

        /// <summary>
        /// Найти угол между вектором поворота объекта 1 и объектом 2
        /// </summary>
        /// <param name="selfCoords">Координаты объекта 1 (Принимаются за локальное начало координат)</param>
        /// <param name="selfRotation">Угол поворота объекта 1(Для вычисления вектора)</param>
        /// <param name="targetCoords">Координаты объекта 2</param>
        /// <returns>Угол поворота</returns>
        private float FindAngleBetweenVectors(Vector2f selfCoords, float selfRotation ,Vector2f targetCoords)
        {
            float divX = targetCoords.X - selfCoords.X;
            float divY = targetCoords.Y - selfCoords.Y;
            float distance = (float)(Math.Sqrt(Math.Pow(divX, 2) + Math.Pow(divY, 2)));
            float angleBetween = (float)((divX * Math.Cos(selfRotation)) + (divY * Math.Sin(selfRotation)));
            angleBetween = (float)(Math.Acos(angleBetween / (distance)));
            return angleBetween;
        }
     
        public override void Process()
        {
            this.MoveToTarget(this.playerContainer.ControllingObject.GetTargetCheckPoint().Coords);
            this.Moving();
        }
    }
}
