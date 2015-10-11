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

        /// <summary>
        /// Коллекция векторов скоростей прямолинейного движения 
        /// </summary>
        private List<SpeedVector> speedVectors = new List<SpeedVector>();

        /// <summary>
        /// Скорость вращетельного движения
        /// </summary>
        private float rotationSpeed = 0;

        /// <summary>
        /// Добавить новый вектор скорости
        /// </summary>
        /// <param name="speed">Скорость</param>
        /// <param name="angle">Угол поворота вектора (в рад.)</param>
        private void AddNewSpeedVector(float speed, float angle, float maxSpeed)
        {
            foreach (SpeedVector vector in speedVectors)
            {//Если среди существующих векторов присутствует вектор, поворот которого отличается от поворота нового вектора менее чем на 5 градусов
                if (Math.Abs(vector.Angle - angle) < (Math.PI * 5 )/ 180)
                {//то вместо создания нового вектора следует, изменить текущий
                    if (maxSpeed > (vector.Speed + speed))
                    {
                        vector.SpeedAcceleration(speed);
                    }
                    vector.VectorRotation(angle - vector.Angle);
                    return;
                }//Или вектор угол которого отличается на 180 градусов (PI)
                if ((Math.Abs(vector.Angle - (angle - (float)Math.PI)) < (2 * Math.PI / 180)) || (Math.Abs(angle - (vector.Angle - (float)Math.PI)) < (2 * Math.PI / 180)))
                {//то скорость данного вектора уменьшится
                    vector.SpeedAcceleration(-speed);
                }
            }//В противном случае требуется добавить новый вектор
            speedVectors.Add(new SpeedVector(speed, angle));
        }

        /// <summary>
        /// Начать движение вперед (дать тягу на маршевый двигатель)
        /// </summary>
        /// <param name="ship">Корабль</param>
        public void GiveForwardThrust(Ship ship)
        {
            Engine engine = ship.Equipment[(int)Ship.EquipmentNames.Engine] as Engine;//Получение двигателя корабля
            Battery battery = ship.Equipment[(int)Ship.EquipmentNames.Battery] as Battery;//получение батареи корабля
            if (battery.Uncharge(engine.EnergyNeeds))
            {
                float acceleration = (float) (engine.ForwardThrust/ship.Mass);
                this.AddNewSpeedVector(acceleration, ship.Rotation, engine.MaxForwardSpeed);
            }
        }

        /// <summary>
        /// Начать движение назад (реверс)
        /// </summary>
        /// <param name="ship">Корабль</param>
        public void GiveReversThrust(Ship ship)
        {
            Engine engine = ship.Equipment[(int)Ship.EquipmentNames.Engine] as Engine;//Получение двигателя корабля
            Battery battery = ship.Equipment[(int)Ship.EquipmentNames.Battery] as Battery;//получение батареи корабля
            if (battery.Uncharge(engine.EnergyNeeds))
            {
                float acceleration = (float) (engine.ShuntingThrust/ship.Mass);
                this.AddNewSpeedVector(acceleration, ship.Rotation + (float) Math.PI, engine.MaxShuntingSpeed);
            }
        }

        /// <summary>
        /// Начать боковое движение
        /// </summary>
        /// <param name="ship">Корабль</param>
        /// <param name="directionSign">Знак направления</param>
        public void GiveSideThrust(Ship ship, int directionSign)
        {
            Engine engine = ship.Equipment[(int)Ship.EquipmentNames.Engine] as Engine;//Получение двигателя корабля
            Battery battery = ship.Equipment[(int)Ship.EquipmentNames.Battery] as Battery;//получение батареи корабля
            if (battery.Uncharge(engine.EnergyNeeds))
            {
                float acceleration = (float) (engine.ShuntingThrust/ship.Mass);
                directionSign /= Math.Abs(directionSign); //Сохранение только знака числа
                this.AddNewSpeedVector(acceleration, ship.Rotation + (float) (directionSign*Math.PI/2),engine.MaxShuntingSpeed);
            }
        }

        /// <summary>
        /// Инмульс экстренного прекращения движения
        /// </summary>
        public void FullStop(Ship ship)
        {//Ликвидация всех векторов движения
            Engine engine = ship.Equipment[(int)Ship.EquipmentNames.Engine] as Engine;//Получение двигателя корабля
            Battery battery = ship.Equipment[(int)Ship.EquipmentNames.Battery] as Battery;//получение батареи корабля
            foreach (SpeedVector vector in speedVectors)
            {
             if (battery.Uncharge(engine.EnergyNeeds))
                {
                    float acceleration = (float) (engine.ShuntingThrust/ship.Mass);
                    vector.SpeedAcceleration(-acceleration);
                }
            }
        }


        /// <summary>
        /// Начать вращательное движение
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="Sign"></param>
        public void GiveRotationThrust(Ship ship, int Sign)
        {
            Engine engine = ship.Equipment[(int)Ship.EquipmentNames.Engine] as Engine;//Получение двигателя корабля
            Battery battery = ship.Equipment[(int)Ship.EquipmentNames.Battery] as Battery;//получение батареи корабля
            if (battery.Uncharge(engine.EnergyNeeds))
            {
                this.rotationSpeed = Sign / Math.Abs(Sign) * engine.ShuntingThrust / ship.Mass;
                this.rotationSpeed /= (float)Math.Sqrt(Math.Pow(ship.ViewPartSize.X / 4, 2) + Math.Pow(ship.ViewPartSize.Y / 4, 2));//вычисление угловой скорости
            }
        }


        /// <summary>
        /// Вращение корабля
        /// </summary>
        /// <param name="Sign"></param>
        public void Rotate(Ship ship)
        {
            if (this.rotationSpeed > 0)//стабилизация вращения
            {
                this.rotationSpeed -= (float)(0.01 * Math.PI / 180);
            }
            else if (this.rotationSpeed < 0)
            {
                this.rotationSpeed += (float)(0.01 * Math.PI / 180);
            }
            ship.ChangeRotation(this.rotationSpeed);//Изменение угла поворота
        }


        /// <summary>
        /// Процесс движения корабля
        /// </summary>
        /// <param name="ship"></param>
        public void Process(Ship ship)
        {
            this.Rotate(ship);//Вразение
            foreach (SpeedVector vector in speedVectors)//Прямолинейное смещение
            {
                ship.ShipAtomMoving(vector.Speed, vector.Angle);//вычисление координат по текущему вектору
                vector.SpeedAcceleration((float)-0.02);//уменьшение скорости на константу
            }
            this.speedVectors.RemoveAll(vector => vector.Speed < 0);//Удаление векторов с нулевой скоростью
        }


    }
}
