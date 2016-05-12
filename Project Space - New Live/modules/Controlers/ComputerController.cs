using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using SFML.System;

namespace Project_Space___New_Live.modules
{
    /// <summary>
    /// Компьютерный контроллер
    /// </summary>
    public class ComputerController : AbstractController
    {
        /// <summary>
        /// Энергетические режимы
        /// </summary>
        private enum EnergyMode : int
        {
            /// <summary>
            /// Максимальный режим
            /// </summary>
            Maximal = 0,
            /// <summary>
            /// Экономичный режим
            /// </summary>
            Economic
        }

        /// <summary>
        /// Набор возможных решений
        /// </summary>
        private enum Decistion : int
        {
            /// <summary>
            /// Атаковать противника
            /// </summary>
            Attack = 0,
            /// <summary>
            /// Избегать противника
            /// </summary>
            Avoid,
            /// <summary>
            /// Игнорировать противника
            /// </summary>
            Ignore
        }

        /// <summary>
        /// Текущее принятое решение
        /// </summary>
        private Decistion currentDecistion = Decistion.Ignore;

        /// <summary>
        /// Количество принятых решений
        /// </summary>
        private int decisionCount = 0;

        /// <summary>
        /// Объект-противник
        /// </summary>
        private Vector2f coordsEnemy;

        /// <summary>
        /// Объект, сохранающий статистические данные
        /// </summary>
        private DataSaver dataSaver;

        /// <summary>
        /// Таймер сохранения статистики
        /// </summary>
        private Clock dataSaverClock;

        //Таймер принятия решений
        private Clock decisionClock;

        /// <summary>
        /// Период принятия решения
        /// </summary>
        private int decisionTime;


        /// <summary>
        /// Текущий энекретический режим
        /// </summary>
        private EnergyMode currentEnergyMode = EnergyMode.Maximal;

        /// <summary>
        /// Конструктор компьтерного контроллера
        /// </summary>
        /// <param name="ObjectContainer"></param>
        public ComputerController(ObjectContainer ObjectContainer, String dataFileName, int decisionTime = 1000)
        {
            this.ObjectContainer = ObjectContainer;
            this.dataSaver = new DataSaver(dataFileName);
            this.decisionTime = decisionTime;
            this.dataSaverClock = new Clock();//запуск таймеров
            this.decisionClock = new Clock(); ;
        }

        /// <summary>
        /// Движение к цели
        /// </summary>
        /// <param name="targetCoords">Координаты цели</param>
        /// <param name="dangerRadius">Радиус опасной зоны, около цели (По-умолчанию 0)/param>
        private void MoveToTarget(Vector2f targetCoords, float dangerRadius = 0)
        {
            if (this.currentEnergyMode == EnergyMode.Maximal)
            {
                this.Forward = true;
                this.RotateToTarget(targetCoords, dangerRadius);
            }
            else
            {
                float maxSpeed = (this.ObjectContainer.ControllingObject.Equipment[(int)(ActiveObject.EquipmentNames.Engine)] as Engine).MaxForwardSpeed;
                float currentSpeed = this.ObjectContainer.ControllingObject.MoveManager.ConstructResultVector().Speed;
                double speedPersent = 100 * currentSpeed / maxSpeed;
                if (speedPersent < 25)
                {
                    this.Forward = true;
                }
                else
                {
                    this.Forward = false;
                }
                this.RotateToTarget(targetCoords, dangerRadius);

            }
        }

        /// <summary>
        /// Управление поворотом игрока
        /// </summary>
        /// <param name="targetCoords">Координаты цели</param>
        /// <param name="dangerRadius">Радиус опасной зоны, около цели (По-умолчанию 0)</param>
        private void RotateToTarget(Vector2f targetCoords, float dangerRadius = 0)
        {
            Vector2f divCoords = targetCoords - this.ObjectContainer.ControllingObject.Coords;//Вычисление основынх параметров
            float distance = (float)(Math.Sqrt(Math.Pow(divCoords.X, 2) + Math.Pow(divCoords.Y, 2)));
            float relativeAngle = (float) (Math.Atan2(divCoords.Y,  divCoords.X));
            float angleBetween = this.FindAngleBetweenVectors(this.ObjectContainer.ControllingObject.Coords, this.ObjectContainer.ControllingObject.Rotation, targetCoords);
            if (angleBetween > 5 * Math.PI / 180)//если угол между векторами направление и положения цели больше порога
            {
                if (distance > dangerRadius)//если расстояние до цели больше радиуса опасной зоны
                {
                    if (Math.Abs(relativeAngle) > 4 * Math.PI / 5 && angleBetween < Math.PI / 2)//проверить нахождение в "мертовой" угловой зоне
                    {
                        return;//если объект в мертвой зоне сохранить его предыдушее состояние
                    }
                    if (relativeAngle < this.ObjectContainer.ControllingObject.Rotation)//иначе оценить куда нужно произвести поворот и установить соответствующие флаги
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
                {//если же объект находится в радиусе опасной зоны
                    if (relativeAngle >= this.ObjectContainer.ControllingObject.Rotation)//то начать уклонение
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
            else//если угол между векторами направление и положения цели меньше порога
            {
                if (distance <= dangerRadius)//то оценить нахождение в опасной зоне
                {
                    if (relativeAngle >= this.ObjectContainer.ControllingObject.Rotation)//и начать уклонение если требуется
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
                {//иначе прекратить вращение, т.к. Игрок сориентирован в нужную сторону
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

        /// <summary>
        /// Атаковать цель
        /// </summary>
        /// <param name="targetCoords">Координаты цели</param>
        /// <param name="dangerRadius">Радиус опасной зоны, около цели (По-умолчанию 300)</param>
        private void AttackTarget(Vector2f targetCoords, float dangerRadius = 300)
        {
            switch (currentEnergyMode)
            {
                case EnergyMode.Maximal:
                {
                    this.MaximumAttack(targetCoords, dangerRadius);
                }; break;
                case EnergyMode.Economic:
                {
                    this.MaximumAttack(targetCoords, dangerRadius);
                }; break;
            }
            
        }
     

        /// <summary>
        /// Процесс работы контроллера
        /// </summary>
        /// <param name="signaturesCollection">Коллекция сигнатур объектов в зоне видимости</param>
        public override void Process(List<ObjectSignature> signaturesCollection)
        {
            this.TimerCheck();//проверка таймеров
            this.coordsEnemy = this.DetectEnemy(signaturesCollection);//находим текущие координаты противника 
            this.EnergyAnalise();//производим анализ энергозапаса 
            if (float.IsNaN(this.coordsEnemy.X) || float.IsNaN(this.coordsEnemy.Y))//если противник не обнаружен
            {
                this.MoveToTarget(this.ObjectContainer.ControllingObject.GetTargetCheckPoint().Coords);//двигаться к целевой контрольной точке (Стратегия Игнорирования)
            }
            else
            {
                this.Action();//иначе действовать в соответсвии с принятым решением
            }
            this.Moving();//движения
        }

        /// <summary>
        /// Принять решение
        /// </summary>
        private void TakeDecison()
        {
            this.decisionClock.Restart();//перезапуск таймера принятия решений
            //TODO реализовать принятие решений с помощью ИНС
            this.currentDecistion = Decistion.Attack;
            this.decisionCount ++;//инкремент счетчика решений
        }

        /// <summary>
        /// Дейстиве
        /// </summary>
        private void Action()
        {
            switch (this.currentDecistion)//выбор алгоритма в соответствии с принятым решением
            {
                case Decistion.Attack://нападение
                {
                    this.AttackTarget(this.coordsEnemy, 525);//атаковать противника
                }; break;
                case Decistion.Avoid://избегание
                {
                    this.MoveToTarget(this.ObjectContainer.ControllingObject.GetHomeCheckPoint().Coords, 0);//иначе двигаться к домашней контрольной точке
                }; break;
                case Decistion.Ignore://игнорирование
                default:
                {
                    this.MoveToTarget(this.ObjectContainer.ControllingObject.GetTargetCheckPoint().Coords);//двигаться к целевой контрольной точке
                }; break;
            }
        }

        /// <summary>
        /// Найти противника среди объектов в зоне видимости
        /// </summary>
        /// <param name="signaturesCollection">Коллекция сигнатур объектов в зоне видимости</param>
        /// <returns>Текущие координаты противника или пара NaN, если противник не обнаружен в зоне видимости</returns>
        private Vector2f DetectEnemy(List<ObjectSignature> signaturesCollection)
        {   
            //TODO реализовать нейросетевой анализ
            foreach (ObjectSignature signature in signaturesCollection)//поиск обекта-противника в зоне видимости
            {
                Vector2f sizes = (Vector2f)signature.Characteristics[(int)(ObjectSignature.CharactsKeys.Size)];
                float mass = (float)signature.Characteristics[(int)(ObjectSignature.CharactsKeys.Mass)];
                if (sizes.X > 30 && sizes.Y > 30 && mass > 500)//если параметры размера и массы превышают пороговые
                {
                    return (Vector2f)signature.Characteristics[(int)(ObjectSignature.CharactsKeys.Coords)];//возврат текущих координат как координат объекта-противника
                }
            }
            return new Vector2f(float.NaN, float.NaN);//возврат пары NaN если противник не обнаружен в зоне видимости
        }

        /// <summary>
        /// Проверка таймеров
        /// </summary>
        private void TimerCheck()
        {
            if (this.decisionClock.ElapsedTime.AsMilliseconds() > this.decisionTime)//если прошло время принятия решения
            {
                this.TakeDecison();//принять решение
                this.decisionClock.Restart();//перезапуск таймера
            }
            if (this.dataSaverClock.ElapsedTime.AsSeconds() > 60)//если прошла минута с прошлой записи статистических данных
            {
                this.dataSaver.WriteData(this.ObjectContainer.WinCount, this.ObjectContainer.DeathCount, this.decisionCount);//сделать новую запись
                this.decisionCount = 0;//обнулить счетчик принятия решений
                this.dataSaverClock.Restart();//перезапуск таймера
            }
        }

        /// <summary>
        /// Атака в максимальном энергорежиме
        /// </summary>
        /// <param name="targetCoords">Координаты цели</param>
        /// <param name="dangerRadius">Радиус опасной зоны, около цели (По-умолчанию 300)</param>
        private void MaximumAttack(Vector2f targetCoords, float dangerRadius = 300)
        {
            Vector2f divCoords = targetCoords - this.ObjectContainer.ControllingObject.Coords;//Вычисление основынх параметров
            float distance = (float)(Math.Sqrt(Math.Pow(divCoords.X, 2) + Math.Pow(divCoords.Y, 2)));
            float angleBetween = this.FindAngleBetweenVectors(this.ObjectContainer.ControllingObject.Coords, this.ObjectContainer.ControllingObject.Rotation, targetCoords);//Вычисление параметров
            this.MoveToTarget(targetCoords, dangerRadius);//наведение на цель
            if (!this.ObjectContainer.ControllingObject.ObjectWeaponSystem.HasAmmo)//если боезапаса нет
            {
                return;//окончить работу боевой функции
            }
            Weapon currentWeapon = this.ObjectContainer.ControllingObject.ObjectWeaponSystem.GetActiveWeapon();//получить активное оружие
            if (currentWeapon.Ammo < 1)//если боезапас данного оружия израсходован
            {
                int activeWeaponIndex = this.ObjectContainer.ControllingObject.ObjectWeaponSystem.IndexOfActiveWeapon;//получить индекс текущего оружия
                if (!this.ObjectContainer.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(activeWeaponIndex + 1))//установить индекс следующего оружия
                {
                    this.ObjectContainer.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(0);//если не удалось то установить индекс самого первого оружия в коллекции
                }
                return;//окончить работу боевой функции
            }
            if (distance < currentWeapon.Range)//если объект находится в зоне поражения
            {
                if (angleBetween < currentWeapon.Dispersion)//и прицеливания
                {
                    this.ObjectContainer.ControllingObject.OpenFire();//то открыть огонь
                    return;//окончить работу боевой функции
                }
            }
            this.ObjectContainer.ControllingObject.StopFire();//иначе прекратить огонь
        }

        /// <summary>
        /// Атака в экономичном энергорежиме
        /// </summary>
        /// <param name="targetCoords">Координаты цели</param>
        /// <param name="dangerRadius">Радиус опасной зоны, около цели (По-умолчанию 300)</param>
        private void EconomicAttack(Vector2f targetCoords, float dangerRadius = 300)
        {
            Vector2f divCoords = targetCoords - this.ObjectContainer.ControllingObject.Coords;//Вычисление основынх параметров
            float distance = (float)(Math.Sqrt(Math.Pow(divCoords.X, 2) + Math.Pow(divCoords.Y, 2)));
            float angleBetween = this.FindAngleBetweenVectors(this.ObjectContainer.ControllingObject.Coords, this.ObjectContainer.ControllingObject.Rotation, targetCoords);//Вычисление параметров
            this.MoveToTarget(targetCoords, dangerRadius);//наведение на цель
            if (!this.ObjectContainer.ControllingObject.ObjectWeaponSystem.HasAmmo)//если боезапаса нет
            {
                return;//окончить работу боевой функции
            }
            Weapon currentWeapon = this.ObjectContainer.ControllingObject.ObjectWeaponSystem.GetActiveWeapon();//получить активное оружие
            if (currentWeapon.Ammo < 1)//если боезапас данного оружия израсходован
            {
                int activeWeaponIndex = this.ObjectContainer.ControllingObject.ObjectWeaponSystem.IndexOfActiveWeapon;//получить индекс текущего оружия
                if (!this.ObjectContainer.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(activeWeaponIndex + 1))//установить индекс следующего оружия
                {
                    this.ObjectContainer.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(0);//если не удалось то установить индекс самого первого оружия в коллекции
                }
                return;//окончить работу боевой функции
            }
            if (distance < currentWeapon.Range)//если объект находится в зоне поражения
            {
                if (angleBetween < currentWeapon.Dispersion)//и прицеливания
                {
                    if (this.ObjectContainer.GetEnergy() > 15)
                    {
                        this.ObjectContainer.ControllingObject.OpenFire();//то открыть огонь
                        return;//окончить работу боевой функции
                    }
                }
            }
            this.ObjectContainer.ControllingObject.StopFire();//иначе прекратить огонь
        }

        /// <summary>
        /// Управление энергорежимами
        /// </summary>
        private void EnergyAnalise()
        {
            switch (this.currentEnergyMode)//в зависимости от текущего энерго режима
            {
                case EnergyMode.Maximal://Максимальный режим
                {
                    if (this.ObjectContainer.GetEnergy() < 20)//если запас энерги упал ниже 20% 
                    {
                        this.currentEnergyMode = EnergyMode.Economic;//переход в экономичный режим
                    }
                    return;
                }
                case EnergyMode.Economic://экономичный режим
                {
                    if (this.ObjectContainer.GetEnergy() > 60)//если запас энерги возрос выше 20%
                    {
                        this.currentEnergyMode = EnergyMode.Maximal;//переход в максимальный режим
                    }
                    return;
                }
            }
        }

    }
}
