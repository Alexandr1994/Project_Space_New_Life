using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
        /// <param name="angle">Угол поворота вектора в рад.</param>
        /// <param name="maxSpeed">Максимальная скорость</param>
        private void AddNewSpeedVector(float speed, float angle, float maxSpeed)
        {
            foreach (SpeedVector vector in speedVectors)
            {//Если среди существующих векторов присутствует вектор, поворот которого отличается от поворота нового вектора менее чем на 5 градусов
                if (Math.Abs(vector.Angle - angle) < (Math.PI * 5 ) / 180)
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
            Engine engine = ship.Equipment[(int)(Ship.EquipmentNames.Engine)] as Engine;//Получение двигателя корабля
            Battery battery = ship.Equipment[(int)(Ship.EquipmentNames.Battery)] as Battery;//получение батареи корабля
            if (battery.Uncharge(engine.EnergyNeeds))
            {
                float acceleration = (float) (engine.ForwardThrust / ship.Mass);
                this.AddNewSpeedVector(acceleration, ship.Rotation, engine.MaxForwardSpeed);
            }
        }

        /// <summary>
        /// Начать движение назад (реверс)
        /// </summary>
        /// <param name="ship">Корабль</param>
        public void GiveReversThrust(Ship ship)
        {
            Engine engine = ship.Equipment[(int)(Ship.EquipmentNames.Engine)] as Engine;//Получение двигателя корабля
            Battery battery = ship.Equipment[(int)(Ship.EquipmentNames.Battery)] as Battery;//получение батареи корабля
            if (battery.Uncharge(engine.EnergyNeeds))
            {
                float acceleration = (float)(engine.ShuntingThrust / ship.Mass);
                this.AddNewSpeedVector(acceleration, ship.Rotation + (float)Math.PI, engine.MaxShuntingSpeed);
            }
        }

        /// <summary>
        /// Начать боковое движение
        /// </summary>
        /// <param name="ship">Корабль</param>
        /// <param name="directionSign">Знак направления</param>
        public void GiveSideThrust(Ship ship, int directionSign)
        {
            Engine engine = ship.Equipment[(int)(Ship.EquipmentNames.Engine)] as Engine;//Получение двигателя корабля
            Battery battery = ship.Equipment[(int)(Ship.EquipmentNames.Battery)] as Battery;//получение батареи корабля
            if (battery.Uncharge(engine.EnergyNeeds))
            {
                float acceleration = (float)(engine.ShuntingThrust/ship.Mass);
                directionSign /= Math.Abs(directionSign); //Сохранение только знака числа
                this.AddNewSpeedVector(acceleration, ship.Rotation + (float)(directionSign*Math.PI / 2),engine.MaxShuntingSpeed);
            }
        }

        /// <summary>
        /// Начать вращательное движение
        /// </summary>
        /// <param name="ship">Корабль</param>
        /// <param name="Sign">Знак направления</param>
        public void GiveRotationThrust(Ship ship, int Sign)
        {
            Engine engine = ship.Equipment[(int)(Ship.EquipmentNames.Engine)] as Engine;//Получение двигателя корабля
            Battery battery = ship.Equipment[(int)(Ship.EquipmentNames.Battery)] as Battery;//получение батареи корабля
            if (battery.Uncharge(engine.EnergyNeeds))
            {
                float acceleration = Sign / Math.Abs(Sign) * engine.ShuntingThrust / ship.Mass;
                acceleration /= (float)Math.Sqrt(Math.Pow(ship.ViewPartSize.X / 4, 2) + Math.Pow(ship.ViewPartSize.Y / 4, 2));//вычисление угловой скорости
                if (this.rotationSpeed * acceleration > 0)
                {
                    this.rotationSpeed = acceleration;
                }
                else
                {
                    this.rotationSpeed += acceleration * 2;
                }
            }
        }

        /// <summary>
        /// Инмульс экстренного прекращения движения
        /// </summary>
        /// <param name="ship">Корабль</param>
        public void FullStop(Ship ship)
        {//Ликвидация всех векторов движения
            Engine engine = ship.Equipment[(int)(Ship.EquipmentNames.Engine)] as Engine;//Получение двигателя корабля
            Battery battery = ship.Equipment[(int)(Ship.EquipmentNames.Battery)] as Battery;//получение батареи корабля
            foreach (SpeedVector vector in speedVectors)
            {
             if (battery.Uncharge(engine.EnergyNeeds))
                {
                    float acceleration = (float)(engine.ShuntingThrust / ship.Mass);
                    vector.SpeedAcceleration(-acceleration);
                }
            }
        }

        /// <summary>
        /// Вращение корабля
        /// </summary>
        /// <param name="Sign">Корабль</param>
        public void Rotate(Ship ship)
        {
            if (this.rotationSpeed > 0)//стабилизация вращения
            {
                this.rotationSpeed -= (float)(0.04 * Math.PI / 180);
            }
            else if (this.rotationSpeed < 0)
            {
                this.rotationSpeed += (float)(0.04 * Math.PI / 180);
            }
            ship.ChangeRotation(this.rotationSpeed);//Изменение угла поворота
        }


        /// <summary>
        /// Процесс движения корабля
        /// </summary>
        /// <param name="ship">Корабль</param>
        public void Process(Ship ship)
        {
            (ship.Equipment[(int)(Ship.EquipmentNames.Engine)]).Deactivate();//Итерационная деактивация двигателя (Двигатель работает с энергозапасом самостоятельно)
            this.Rotate(ship);//Вразение
            SpeedVector movingVector = this.ConstructResultVector();
            ship.ShipAtomMoving(movingVector.Speed, movingVector.Angle);//вычисление координат по текущему вектору
            foreach (SpeedVector vector in speedVectors)//Прямолинейное смещение
            {
                vector.SpeedAcceleration((float)-0.04);//уменьшение скорости на константу
            }
            this.speedVectors.RemoveAll(vector => vector.Speed < 0);//Удаление векторов с нулевой скоростью
        }

        /// <summary>
        /// Сконструировать результирующий вектор скорости корабля
        /// </summary>
        /// <returns>Результирующий вектор скорости корабля</returns>
        private SpeedVector ConstructResultVector()
        {
            SpeedVector resultVector = null;//создаем "пустой" результирующий вектор скорости
            foreach (SpeedVector currentVector in this.speedVectors)//поочередно складываем все вектора скоростей с результирующим вектором
            {
                if (resultVector == null)//если результирующий вектор - пустой, то результирующий вектор равен текущему вектору
                {
                    resultVector = new SpeedVector(currentVector.Speed, currentVector.Angle);
                    continue;
                }
                Vector2f tempDecVector = new Vector2f();//иначе вычисляем декартовы координаты результирующего вектора
                tempDecVector.X = (float)(resultVector.Speed * Math.Cos(resultVector.Angle) + currentVector.Speed * Math.Cos(currentVector.Angle));//х-состовляющая
                tempDecVector.Y = (float)(resultVector.Speed * Math.Sin(resultVector.Angle) + currentVector.Speed * Math.Sin(currentVector.Angle));//у-состовляющая
                float newSpeed = (float) (Math.Sqrt(Math.Pow(tempDecVector.X, 2) + Math.Pow(tempDecVector.Y, 2)));//вычисляем величину скорости нового вектора
                float newAngle = 0;//определяем угол данного вектора
                newAngle = (float)(Math.Atan2(tempDecVector.Y, tempDecVector.X));//то произвести вычисление
                resultVector = new SpeedVector(newSpeed, newAngle);
            }
            if (resultVector == null)
            {
                resultVector = new SpeedVector(0, 0);
            }
            return resultVector;
        }

        /// <summary>
        /// Обработка столкновения между кораблями
        /// </summary>
        /// <param name="contacterNoveManager">Модуль движений конатктирующего корабля</param>
        /// <param name="ownMass">Масса данного корабля</param>
        /// <param name="contacterMass">Масса контактирующего корабля</param>
        /// <param name="angle">Угол соударения</param>
        public void CrashMove(ShipMover contacterNoveManager, float ownMass, float contacterMass, float angle)
        {
            SpeedVector ownSpeedVector = this.ConstructResultVector();
            SpeedVector contacterSpeedVector = contacterNoveManager.ConstructResultVector();
            this.speedVectors.Clear();//работа с собственными векторами
            this.ConstructAfterCrashSpeedVector(ownSpeedVector, contacterSpeedVector, ownMass, contacterMass, angle);
            contacterNoveManager.speedVectors.Clear();//работа с векторами контактирующего
            contacterNoveManager.ConstructAfterCrashSpeedVector(contacterSpeedVector, ownSpeedVector, contacterMass, ownMass, angle);
        }

        /// <summary>
        /// Движение корабля после попадания
        /// </summary>
        /// <param name="ship">Корабль</param>
        /// <param name="shellSpeedVector">Вектор скорости снаряда</param>
        /// <param name="shellMass">Масса снаряда</param>
        public void ShellHit(Ship ship, SpeedVector shellSpeedVector, float shellMass)
        {
            this.OnShellReaction(ship, shellSpeedVector, shellMass);
        }

        /// <summary>
        /// Движение корабля после выстрела
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="shellSpeedVector"></param>
        /// <param name="shellMass"></param>
        public void ShellShoot(Ship ship, SpeedVector shellSpeedVector, float shellMass)
        {
            SpeedVector newSpeedVector = new SpeedVector(shellSpeedVector.Speed, (float)(shellSpeedVector.Angle + Math.PI));
            this.OnShellReaction(ship, newSpeedVector, shellMass);
        }

        /// <summary>
        /// Движение корабля после выстрела/попадания снаряда
        /// </summary>
        /// <param name="ship">Корабль</param>
        /// <param name="shellSpeedVector">Вектор скорости снаряда</param>
        /// <param name="shellMass">Масса снаряда</param>
        private void OnShellReaction(Ship ship, SpeedVector shellSpeedVector, float shellMass)
        {
            Engine engine = ship.Equipment[(int)(Ship.EquipmentNames.Engine)] as Engine;//Получение двигателя корабля
            this.AddNewSpeedVector((float)((shellMass * shellSpeedVector.Speed) / ship.Mass), shellSpeedVector.Angle, engine.MaxForwardSpeed);
        }

        /// <summary>
        /// Сконструировать вектор скорости после столкновения
        /// </summary>
        /// <param name="ownSpeedVector">Собственный вектор скорости</param>
        /// <param name="contacterSpeedVector">Вектор скорости конатктирующего</param>
        /// <param name="ownMass">Собственная масса</param>
        /// <param name="contacterMass">Масса контактирующего</param>
        /// <param name="angle">Угол соударения</param>
        private void ConstructAfterCrashSpeedVector(SpeedVector ownSpeedVector, SpeedVector contacterSpeedVector, float ownMass, float contacterMass, float angle)
        {
            double coefA = (ownSpeedVector.Speed * Math.Cos(ownSpeedVector.Angle - angle) * (ownMass - contacterMass)
                + 2 * contacterMass * contacterSpeedVector.Speed * Math.Cos(contacterSpeedVector.Angle - angle))
                / (ownMass + contacterMass);
            double coefB = ownSpeedVector.Speed * Math.Sin(ownSpeedVector.Angle - angle);
            double newXSpeed = coefA * Math.Cos(angle) + coefB * Math.Cos(angle - Math.PI / 2);//Получение Х и Y состовляющих скорости
            double newYSpeed = coefA * Math.Sin(angle) + coefB * Math.Sin(angle - Math.PI / 2);
            float newSpeed = (float)(Math.Sqrt(Math.Pow(newXSpeed, 2) + Math.Pow(newYSpeed, 2)));//Построение и добавление вектора
            float newAngle = (float)(Math.Atan2(newYSpeed, newXSpeed));
            this.speedVectors.Add(new SpeedVector(newSpeed, newAngle));
        }


    }
}
