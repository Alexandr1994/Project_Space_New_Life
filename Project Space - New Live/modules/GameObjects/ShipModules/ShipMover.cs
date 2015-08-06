using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects.ShipModules
{
    /// <summary>
    /// Модуль движений корабля
    /// </summary>
    public class ShipMover
    {

        private List<SpeedVector> speedVectors = new List<SpeedVector>();


        /// <summary>
        /// Добавить новый вектор скорости
        /// </summary>
        /// <param name="speed">Скорость</param>
        /// <param name="angle">Угол поворота вектора (в рад.)</param>
        public void AddNewSpeedVector(float speed, float angle)
        {
            foreach (SpeedVector vector in speedVectors)
            {//Если среди существующих векторов присутствует вектор, поворот которого отличается от поворота нового вектора менее чем на 5 градусов
                if (Math.Abs(vector.Angle - angle) < (5*Math.PI)/180)
                {//то вместо создания нового вектора следует, изменить текущий
                    vector.SpeedAcceleration(speed);
                    vector.VectorRotation(angle - vector.Angle);
                    return;
                }
            }//В противном случае требуется добавить новый вектор
            speedVectors.Add(new SpeedVector(speed, angle));
        }



        /// <summary>
        /// Процесс движения корабля
        /// </summary>
        /// <param name="ship"></param>
        public void Process(Ship ship)
        {
            foreach (SpeedVector vector in speedVectors)
            {
                ship.ShipAtomMoving(vector.Speed, vector.Angle);//вычисление координат по текущему вектору
                vector.SpeedAcceleration((float)-0.01);//уменьшение скорости на константу

            }
            this.speedVectors.RemoveAll(vector => vector.Speed < 0);
        }


    }
}
