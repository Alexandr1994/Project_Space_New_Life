using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Модуль движений объекта
    /// </summary>
    public class MoveManager
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
            if (angle > Math.PI)//если угол поворота вектора больше чем 360 градусов
            {
                angle -= (float)(2 * Math.PI);//вернуть его в пределы от 0 до 360 градусов
            }
            if (angle < -Math.PI)
            {
                angle += (float)(2 * Math.PI);//вернуть его в пределы от 0 до 360 градусов
            }
            List<SpeedVector> codirectVectors = this.GetCodirectVectors(angle);
            List<SpeedVector> counterdirectVectors = this.GetCounterdirectVectors(angle);
            if (counterdirectVectors.Count != 0)
            {
                foreach (SpeedVector vector in counterdirectVectors)
                {
                    Vector2f oldVector = new Vector2f();
                    oldVector.X = (float)(vector.Speed * Math.Cos(vector.Angle));
                    oldVector.Y = (float)(vector.Speed * Math.Sin(vector.Angle));
                    Vector2f newVector = new Vector2f();
                    newVector.X = (float)(speed * Math.Cos(angle));
                    newVector.Y = (float)(speed * Math.Sin(angle));
                    float vectorProection = (float)((oldVector.X * newVector.X + oldVector.Y * newVector.Y) / vector.Speed);
                    vector.SpeedAcceleration(vectorProection);
                    //vector.SpeedAcceleration(-speed);
                }
                return;
            }
            if (codirectVectors.Count != 0)
            {
                foreach (SpeedVector vector in codirectVectors)
                {
                    if (this.ConstructResultVector().Speed < maxSpeed)
                    {
                        Vector2f oldVector = new Vector2f();
                        oldVector.X = (float)(vector.Speed * Math.Cos(vector.Angle));
                        oldVector.Y = (float)(vector.Speed * Math.Sin(vector.Angle));
                        Vector2f newVector = new Vector2f();
                        newVector.X = (float)(speed * Math.Cos(angle));
                        newVector.Y = (float)(speed * Math.Sin(angle));
                        float vectorProection = (float)((oldVector.X * newVector.X + oldVector.Y * newVector.Y) / vector.Speed);
                        vector.SpeedAcceleration(vectorProection);
                        //vector.SpeedAcceleration(speed);
                    }
                    vector.VectorRotation(angle - vector.Angle);
                } 
                return;
            }
            speedVectors.Add(new SpeedVector(speed, angle));    
        }

        /// <summary>
        /// Получить векторы угол поворота, которых отличается от angle менее чем на 20 градусов из коллекции
        /// </summary>
        /// <param name="angle">Угол поворота</param>
        /// <returns>Искомые вектора</returns>
        private List<SpeedVector> GetCodirectVectors(float angle)
        {
            List<SpeedVector> retVectors = new List<SpeedVector>();
            foreach (SpeedVector vector in this.speedVectors)
            {
                float divAngle = (float)(vector.Angle - angle);
                if (Math.Abs(divAngle) > -90 * Math.PI / 180 && Math.Abs(divAngle) < 90 * Math.PI / 180)
                {
                    retVectors.Add(vector);
                }
            }
            return retVectors;
        }

        /// <summary>
        /// Получить векторы угол поворота, которых отличается от противолположенного angle угла менее чем на 20 градусов из коллекции
        /// </summary>
        /// <param name="angle">Угол поворота</param>
        /// <returns>Искомые вектора</returns>
        private List<SpeedVector> GetCounterdirectVectors(float angle)
        {
            List<SpeedVector> retVectors = new List<SpeedVector>();
            foreach (SpeedVector vector in this.speedVectors)
            {
                float divAngle = vector.Angle - angle;
                if (Math.Abs(divAngle) > 90 * Math.PI / 180 && Math.Abs(divAngle) < 270 * Math.PI / 180)
                {
                    retVectors.Add(vector);
                }
            }
            return retVectors;
        }

        /// <summary>
        /// Движение вперед
        /// </summary>
        /// <param name="transport">Транспортное средство</param>
        public void GiveForwardThrust(ActiveObject1 transport)
        {
            Engine engine = transport.Equipment[(int)(ActiveObject1.EquipmentNames.Engine)] as Engine;//Получение двигателя транспортного средства
            Battery battery = transport.Equipment[(int)(ActiveObject1.EquipmentNames.Battery)] as Battery;//получение батареи транспортного средства
            if (battery.Uncharge(engine.EnergyNeeds))
            {
                float acceleration = (float)(engine.ForwardThrust / transport.Mass);
                this.AddNewSpeedVector(acceleration, transport.Rotation, engine.MaxForwardSpeed);
            }
        }

        /// <summary>
        /// Реверс/задний ход
        /// </summary>
        /// <param name="transport">Транспортное средство</param>
        public void GiveReversThrust(ActiveObject1 transport)
        {
            Engine engine = transport.Equipment[(int)(ActiveObject1.EquipmentNames.Engine)] as Engine;//Получение двигателя транспортного средства
            Battery battery = transport.Equipment[(int)(ActiveObject1.EquipmentNames.Battery)] as Battery;//получение батареи транспортного средства
            if (battery.Uncharge(engine.EnergyNeeds))
            {
                float acceleration = (float)(engine.ShuntingThrust / transport.Mass);
                this.AddNewSpeedVector(acceleration, transport.Rotation + (float)Math.PI, engine.MaxShuntingSpeed);
            }
        }

        /// <summary>
        /// Боковое движение
        /// </summary>
        /// <param name="transport">Транспортное средство</param>
        /// <param name="directionSign">Знак направления</param>
        public void GiveSideThrust(ActiveObject1 transport, int directionSign)
        {
            Engine engine = transport.Equipment[(int)(ActiveObject1.EquipmentNames.Engine)] as Engine;//Получение двигателя транспортного средства
            Battery battery = transport.Equipment[(int)(ActiveObject1.EquipmentNames.Battery)] as Battery;//получение батареи транспортного средства
            if (battery.Uncharge(engine.EnergyNeeds))
            {
                float acceleration = (float)(engine.ShuntingThrust / transport.Mass);
                directionSign /= Math.Abs(directionSign); //Сохранение только знака числа
                this.AddNewSpeedVector(acceleration, transport.Rotation + (float)(directionSign*Math.PI / 2),engine.MaxShuntingSpeed);
            }
        }

        /// <summary>
        /// Поворот
        /// </summary>
        /// <param name="transport">Транспортное средство</param>
        /// <param name="Sign">Знак направления</param>
        public void GiveRotationThrust(ActiveObject1 transport, int Sign)
        {
            Engine engine = transport.Equipment[(int)(ActiveObject1.EquipmentNames.Engine)] as Engine;//Получение двигателя транспортного средства
            Battery battery = transport.Equipment[(int)(ActiveObject1.EquipmentNames.Battery)] as Battery;//получение батареи транспортного средства
            if (battery.Uncharge(engine.EnergyNeeds))
            {
                Sign = (Sign / Math.Abs(Sign));
                float acceleration = Sign * engine.ShuntingThrust / transport.Mass;
                acceleration /= (float)Math.Sqrt(Math.Pow(transport.ViewPartSize.X, 2) + Math.Pow(transport.ViewPartSize.Y, 2));//вычисление угловой скорости
                if (this.rotationSpeed * acceleration < 0)
                {
                    if (Math.Abs(this.rotationSpeed) > Math.Abs(acceleration))
                    {
                        this.rotationSpeed += acceleration;    
                    }
                    else
                    {
                        this.rotationSpeed = 0;
                    }
                }
                else
                {
                    if (Math.Abs(this.rotationSpeed) < (3 * Math.PI) / 180)
                    {
                        this.rotationSpeed += acceleration;
                    } 
                }
            }
        }

        /// <summary>
        /// Торможение
        /// </summary>
        /// <param name="transport">Транспортное средство</param>
        public void FullStop(ActiveObject1 transport)
        {//Ликвидация всех векторов движения
            Engine engine = transport.Equipment[(int)(ActiveObject1.EquipmentNames.Engine)] as Engine;//Получение двигателя транспортного средства
            Battery battery = transport.Equipment[(int)(ActiveObject1.EquipmentNames.Battery)] as Battery;//получение батареи транспортного средства
            float acceleration = (float)(engine.ShuntingThrust / transport.Mass);//Прямолинейное торможение
            if (this.speedVectors.Count > 0)
            {
                if (battery.Uncharge(engine.EnergyNeeds))
                {
                    foreach (SpeedVector vector in this.speedVectors)
                    {
                        vector.SpeedAcceleration(-acceleration);
                    }
                } 
            }         
            acceleration /= (float)Math.Sqrt(Math.Pow(transport.ViewPartSize.X, 2) + Math.Pow(transport.ViewPartSize.Y, 2));//Вращательное торнможение
            if (Math.Abs(acceleration) > Math.Abs(this.rotationSpeed))
            {
                this.rotationSpeed = 0;
            }
            else
            {
                if (this.rotationSpeed > 0)
                {
                    this.rotationSpeed -= acceleration;
                }
                else
                {
                    this.rotationSpeed += acceleration;
                }
            }
        }

        /// <summary>
        /// Вращение транспортного средства
        /// </summary>
        /// <param name="Sign">Транспортное средство</param>
        public void Rotate(ActiveObject1 transport)
        {
            double acceleration = Math.Sqrt(transport.Environment.MovingResistance) * Math.PI / 180;
            if (Math.Abs(this.rotationSpeed) < acceleration)//стабилизация вращения
            {
                this.rotationSpeed = 0;
            }
            if (this.rotationSpeed > 0)
            {
                this.rotationSpeed -= (float)(acceleration);
            }
            else if (this.rotationSpeed < 0)
            {
                this.rotationSpeed += (float)(acceleration);
            }
            transport.ChangeRotation(this.rotationSpeed);//Изменение угла поворота
        }


        /// <summary>
        /// Процесс движения транспортного средства
        /// </summary>
        /// <param name="transport">Транспортное средство</param>
        public void Process(ActiveObject1 transport)
        {
            (transport.Equipment[(int)(ActiveObject1.EquipmentNames.Engine)]).Deactivate();//Итерационная деактивация двигателя (Двигатель работает с энергозапасом самостоятельно)
            this.Rotate(transport);//Вразение
            SpeedVector movingVector = this.ConstructResultVector();
            transport.ShipAtomMoving(movingVector.Speed, movingVector.Angle);//вычисление координат по текущему вектору
            foreach (SpeedVector vector in speedVectors)//Прямолинейное смещение
            {
                vector.SpeedAcceleration((float)-transport.Environment.MovingResistance);//уменьшение скорости на константу
            }
            this.speedVectors.RemoveAll(vector => vector.Speed < 0.1);//Удаление векторов с нулевой скоростью
        }

        /// <summary>
        /// Сконструировать результирующий вектор скорости корабля
        /// </summary>
        /// <returns>Результирующий вектор скорости корабля</returns>
        public SpeedVector ConstructResultVector()
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
        /// Обработка столкновения между объектами
        /// </summary>
        /// <param name="contacterNoveManager">Модуль движений конатктирующего обьекта</param>
        /// <param name="ownMass">Масса данного обьекта</param>
        /// <param name="contacterMass">Масса контактирующего обьекта</param>
        /// <param name="angle">Угол соударения</param>
        public void CrashMove(MoveManager contacterNoveManager, float ownMass, float contacterMass, float angle)
        {
            SpeedVector ownSpeedVector = this.ConstructResultVector();
            SpeedVector contacterSpeedVector = contacterNoveManager.ConstructResultVector();
            this.speedVectors.Clear();//работа с собственными векторами
            this.ConstructAfterCrashSpeedVector(ownSpeedVector, contacterSpeedVector, ownMass, contacterMass, angle);
            contacterNoveManager.speedVectors.Clear();//работа с векторами контактирующего
            contacterNoveManager.ConstructAfterCrashSpeedVector(contacterSpeedVector, ownSpeedVector, contacterMass, ownMass, angle);
        }


        public void CrashMove(float ownMass, float contacterMass, float angle)
        {
            SpeedVector ownSpeedVector = this.ConstructResultVector();
            SpeedVector contacterSpeedVector = new SpeedVector(0, 0);
            this.speedVectors.Clear();//работа с собственными векторами
            this.ConstructAfterCrashSpeedVector(ownSpeedVector, contacterSpeedVector, ownMass, contacterMass, angle);
        }

        /// <summary>
        /// Движение объекта после попадания
        /// </summary>
        /// <param name="transport">Объект</param>
        /// <param name="shellSpeedVector">Вектор скорости снаряда</param>
        /// <param name="shellMass">Масса снаряда</param>
        public void ShellHit(ActiveObject1 transport, SpeedVector shellSpeedVector, float shellMass)
        {
            this.OnShellReaction(transport, shellSpeedVector, shellMass);
        }

        /// <summary>
        /// Движение Объекта после выстрела
        /// </summary>
        /// <param name="transport">Объект</param>
        /// <param name="shellSpeedVector"></param>
        /// <param name="shellMass"></param>
        public void ShellShoot(ActiveObject1 transport, SpeedVector shellSpeedVector, float shellMass)
        {
            SpeedVector newSpeedVector = new SpeedVector(shellSpeedVector.Speed, (float)(shellSpeedVector.Angle + Math.PI));
            this.OnShellReaction(transport, newSpeedVector, shellMass);
        }

        /// <summary>
        /// Движение Объекта после выстрела/попадания снаряда
        /// </summary>
        /// <param name="transport">Объект</param>
        /// <param name="shellSpeedVector">Вектор скорости снаряда</param>
        /// <param name="shellMass">Масса снаряда</param>
        private void OnShellReaction(ActiveObject1 transport, SpeedVector shellSpeedVector, float shellMass)
        {
            Engine engine = transport.Equipment[(int)(ActiveObject1.EquipmentNames.Engine)] as Engine;//Получение двигателя корабля
            this.AddNewSpeedVector((float)((shellMass * shellSpeedVector.Speed) / transport.Mass), shellSpeedVector.Angle, engine.MaxForwardSpeed);
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
