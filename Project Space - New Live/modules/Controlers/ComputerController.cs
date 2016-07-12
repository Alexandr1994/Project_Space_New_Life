using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using NeuronNetwork;
using Project_Space___New_Live.modules.Storages;
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
        private ObjectSignature enemy;

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
        /// Флаг, управления с помощью нейросети
        /// </summary>
        private bool neronControlling = false;

        /// <summary>
        /// Флаг, управления с помощью нейросети
        /// </summary>
        public bool NeronControlling
        {
            get { return this.neronControlling; }
            set { this.neronControlling = value; }
        }

        /// <summary>
        /// ИНС принятия решений
        /// </summary>
        private Perceptron decisionNetwork;

        /// <summary>
        /// ИНС класификации объектов в зоне видимости
        /// </summary>
        private Perceptron objectClassNetwork;

        /// <summary>
        /// ИНС запуска защитной системы
        /// </summary>
        private Perceptron protectionNetwork;

        /// <summary>
        /// Конструктор компьтерного контроллера
        /// </summary>
        /// <param name="ControllingObject"></param>
        public ComputerController(Transport ControllingObject, int decisionTime = 1000)
        {
            this.ControllingObject = ControllingObject;
            this.decisionTime = decisionTime;
            this.dataSaverClock = new Clock();//запуск таймеров
            this.decisionClock = new Clock(); ;
            //построение нейросетей
        //    this.decisionNetwork = new Perceptron(2, new List<int>(){6, 3}, new List<ActivationFunction.Types>() {ActivationFunction.Types.Sigmoidal, ActivationFunction.Types.Sigmoidal}, 20);
            this.decisionNetwork = new Perceptron(1, new List<int>() { 3 }, new List<ActivationFunction.Types>() { ActivationFunction.Types.Sigmoidal }, 20);
            this.objectClassNetwork = new Perceptron(1, new List<int>(){3}, new List<ActivationFunction.Types>() {ActivationFunction.Types.Linear}, 3 );
            this.protectionNetwork = new Perceptron(1, new List<int>() {2}, new List<ActivationFunction.Types>() { ActivationFunction.Types.Linear }, 4);

            this.NetworksLearn();
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
                float maxSpeed = (this.ControllingObject.Equipment[(int)(Transport.EquipmentNames.Engine)] as Engine).MaxForwardSpeed;
                float currentSpeed = this.ControllingObject.MoveManager.ConstructResultVector().Speed;
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
            Vector2f divCoords = targetCoords - this.ControllingObject.Coords;//Вычисление основынх параметров
            float distance = (float)(Math.Sqrt(Math.Pow(divCoords.X, 2) + Math.Pow(divCoords.Y, 2)));
            float relativeAngle = (float) (Math.Atan2(divCoords.Y,  divCoords.X));
            float angleBetween = this.FindAngleBetweenVectors(this.ControllingObject.Coords, this.ControllingObject.Rotation, targetCoords);
            if (angleBetween > 5 * Math.PI / 180)//если угол между векторами направление и положения цели больше порога
            {
                if (distance > dangerRadius)//если расстояние до цели больше радиуса опасной зоны
                {
                    if (Math.Abs(relativeAngle) > 4 * Math.PI / 5 && angleBetween < Math.PI / 2)//проверить нахождение в "мертовой" угловой зоне
                    {
                        return;//если объект в мертвой зоне сохранить его предыдушее состояние
                    }
                    if (relativeAngle < this.ControllingObject.Rotation)//иначе оценить куда нужно произвести поворот и установить соответствующие флаги
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
                    if (relativeAngle >= this.ControllingObject.Rotation)//то начать уклонение
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
                    if (relativeAngle >= this.ControllingObject.Rotation)//и начать уклонение если требуется
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
                    this.Attack(targetCoords, dangerRadius);
                }; break;
                case EnergyMode.Economic:
                {
                    this.Attack(targetCoords, dangerRadius);
                }; break;
            }
            
        }
     

        /// <summary>
        /// Процесс работы контроллера
        /// </summary>
        /// <param name="signaturesCollection">Коллекция сигнатур объектов в зоне видимости</param>
        public override void Process(List<ObjectSignature> signaturesCollection)
        {
            this.enemy = this.DetectEnemy(signaturesCollection);//находим текущие координаты противника 
            this.TimerCheck();//проверка таймеров
            this.EnergyAnalise();//производим анализ энергозапаса 
            if (float.IsNaN(this.enemy.Coords.X) || float.IsNaN(this.enemy.Coords.Y))//если противник не обнаружен
            {
            //    this.MoveToTarget(this.ControllingObject.GetTargetCheckPoint().Coords);//двигаться к целевой контрольной точке (Стратегия Игнорирования)
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
        private void TakeDecision()
        {
            this.decisionClock.Restart();//перезапуск таймера принятия решений
            if (this.neronControlling)
            {
                

                List<double> outputVector = this.decisionNetwork.Process(this.ConstructInputVector());
                int maxNeuronIndex = outputVector.IndexOf(outputVector.Max());
                switch (maxNeuronIndex)
                {
                    case 0:
                    {
                        this.currentDecistion = Decistion.Attack;
                    } ; break;
                    case 1:
                    {
                            this.currentDecistion = Decistion.Avoid;
                    }; break;
                    case 2:
                    {
                            this.currentDecistion = Decistion.Ignore;
                    }; break;
                }

            }
            else
            {
                if ((this.ControllingObject.Health / this.ControllingObject.MaxHealth) < 25 ||
                    !this.ControllingObject.ObjectWeaponSystem.HasAmmo)
                {
                    this.currentDecistion = Decistion.Avoid;
                }
                else
                {
                    this.currentDecistion = Decistion.Attack;
                }
            }
            this.decisionCount ++;//инкремент счетчика решений
        }

        /// <summary>
        /// Сформировать входной вектор
        /// </summary>
        /// <returns>Входной вектор</returns>
        private List<double> ConstructInputVector()
        {
            List<double> inputVector = new List<double>();
            //составояющая собственной боеспособности
       /*     inputVector.Add(this.ObjectContainer.GetHealh() / 100);//оставшийся запас прочности
            inputVector.Add(this.ObjectContainer.GetEnergy() / 100);//количество энергии
            foreach (Equipment equipment in this.ControllingObject.Equipment)
            //вектор состояния оборудования
            {
                inputVector.Add(equipment.WearState/100);
            }
            inputVector.Add(this.ObjectContainer.GetShieldPower() / 100);
            for (int i = 0; i < this.ObjectContainer.ControllingObject.ObjectWeaponSystem.MaxWeaponsCount; i ++)
                //вектор оружия
            {
                if (i > this.ObjectContainer.ControllingObject.ObjectWeaponSystem.WeaponsCount - 1)
                {
                    inputVector.Add(0);
                    inputVector.Add(0);
                }
                else
                {
                    Weapon weapon = this.ObjectContainer.ControllingObject.ObjectWeaponSystem.GetWeapon(i);
                    inputVector.Add(weapon.Ammo/weapon.MaxAmmo);
                    inputVector.Add(weapon.WearState/100);
                }
            }
            //состовляющая опастности противника
            Vector2f delaCoords = this.ObjectContainer.ControllingObject.Coords - this.enemy.Coords;
            inputVector.Add((Math.Sqrt(Math.Pow(delaCoords.X, 2) + Math.Pow(delaCoords.Y, 2))) / this.ObjectContainer.GetRadarRange());
            inputVector.Add(this.FindAngleBetweenVectors(this.enemy.Coords, this.enemy.Directon, this.ObjectContainer.ControllingObject.Coords) / Math.PI);
            inputVector.Add(this.enemy.Speed);
            inputVector.Add(CharacterLimmits.NormSize(this.enemy.Size).X);
            inputVector.Add(CharacterLimmits.NormSize(this.enemy.Size).Y);
            inputVector.Add(CharacterLimmits.NormMass(enemy.Mass));*/
            //сформированный входной вектор
            return inputVector;
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
                    this.AttackTarget(this.enemy.Coords, 525);//атаковать противника
                }; break;
                case Decistion.Avoid://избегание
                {
                 //   this.MoveToTarget(this.ControllingObject.GetHomeCheckPoint().Coords, 0);//иначе двигаться к домашней контрольной точке
                }; break;
                case Decistion.Ignore://игнорирование
                default:
                {
                //    this.MoveToTarget(this.ControllingObject.GetTargetCheckPoint().Coords);//двигаться к целевой контрольной точке
                }; break;
            }
        }

        /// <summary>
        /// Найти противника среди объектов в зоне видимости
        /// </summary>
        /// <param name="signaturesCollection">Коллекция сигнатур объектов в зоне видимости</param>
        /// <returns>Текущие координаты противника или пара NaN, если противник не обнаружен в зоне видимости</returns>
        private ObjectSignature DetectEnemy(List<ObjectSignature> signaturesCollection)
        {   
            //TODO реализовать нейросетевой анализ
            List<ObjectSignature> shellsCharactersCollection = new List<ObjectSignature>();
            ObjectSignature enemy = new ObjectSignature();
            enemy.Coords =  new Vector2f(float.NaN, float.NaN); ;
            foreach (ObjectSignature signature in signaturesCollection)//поиск обекта-противника в зоне видимости
            {
              //  List<double> outputVector = this.objectClassNetwork.Process()

                Vector2f normedSize = CharacterLimmits.NormSize(signature.Size);
                float normedMass = CharacterLimmits.NormMass(signature.Mass);

                if (normedSize.X > 0.1 && normedSize.Y > 0.1 && normedMass > 0.2)//если параметры размера и массы превышают пороговые
                {
                    enemy = signature;//возврат текущих координат как координат объекта-противника
                }
                if (normedSize.X < 0.1 && normedSize.Y > 0.1 && normedMass > 0 && normedMass < 0.2)
                {
                    shellsCharactersCollection.Add(signature);
                }
            }
            this.ProtectionAnalize(shellsCharactersCollection);
            return enemy;//возврат пары NaN если противник не обнаружен в зоне видимости
        }

        /// <summary>
        /// Проверка таймеров
        /// </summary>
        private void TimerCheck()
        {
            if (this.decisionClock.ElapsedTime.AsMilliseconds() > this.decisionTime)//если прошло время принятия решения
            {
                this.TakeDecision();//принять решение
                this.decisionClock.Restart();//перезапуск таймера
            }
            if (this.dataSaverClock.ElapsedTime.AsSeconds() > 60)//если прошла минута с прошлой записи статистических данных
            {
              //  this.dataSaver.WriteData(this.ObjectContainer.WinCount, this.ObjectContainer.DeathCount, this.decisionCount);//сделать новую запись
                this.decisionCount = 0;//обнулить счетчик принятия решений
                this.dataSaverClock.Restart();//перезапуск таймера
            }
        }

        /// <summary>
        /// Атака в максимальном энергорежиме
        /// </summary>
        /// <param name="targetCoords">Координаты цели</param>
        /// <param name="dangerRadius">Радиус опасной зоны, около цели (По-умолчанию 300)</param>
        private void Attack(Vector2f targetCoords, float dangerRadius = 300)
        {
            Vector2f divCoords = targetCoords - this.ControllingObject.Coords;//Вычисление основынх параметров
            float distance = (float)(Math.Sqrt(Math.Pow(divCoords.X, 2) + Math.Pow(divCoords.Y, 2)));
            float angleBetween = this.FindAngleBetweenVectors(this.ControllingObject.Coords, this.ControllingObject.Rotation, targetCoords);//Вычисление параметров
            this.MoveToTarget(targetCoords, dangerRadius);//наведение на цель
            if (!this.ControllingObject.ObjectWeaponSystem.HasAmmo)//если боезапаса нет
            {
                return;//окончить работу боевой функции
            }
            Weapon currentWeapon = this.ControllingObject.ObjectWeaponSystem.GetActiveWeapon();//получить активное оружие
            if (currentWeapon.Ammo < 1)//если боезапас данного оружия израсходован
            {
                int activeWeaponIndex = this.ControllingObject.ObjectWeaponSystem.IndexOfActiveWeapon;//получить индекс текущего оружия
                if (!this.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(activeWeaponIndex + 1))//установить индекс следующего оружия
                {
                    this.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(0);//если не удалось то установить индекс самого первого оружия в коллекции
                }
                return;//окончить работу боевой функции
            }
            if (distance < currentWeapon.Range)//если объект находится в зоне поражения
            {
                if (angleBetween < currentWeapon.Dispersion)//и прицеливания
                {
                    this.ControllingObject.OpenFire();//то открыть огонь
                    return;//окончить работу боевой функции
                }
            }
            this.ControllingObject.StopFire();//иначе прекратить огонь
        }

        /// <summary>
        /// Управление энергорежимами
        /// </summary>
        private void EnergyAnalise()
        {
           /* switch (this.currentEnergyMode)//в зависимости от текущего энерго режима
            {
                case EnergyMode.Maximal://Максимальный режим
                {
                    if ((this.ControllingObject.Equipment as ) < 20)//если запас энерги упал ниже 20% 
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
            }*/
        }

        public void ProtectionAnalize(List<ObjectSignature> shellsSignaturesCollection)
        {
          /*  float normedEnergy = this.ObjectContainer.GetEnergy() / 100;
            float normedShieldPower = this.ObjectContainer.GetShieldPower() / 100;
            int normedShellsCount = 0;
            if (shellsSignaturesCollection.Count > 0)
            {
                foreach (ObjectSignature shellSignature in shellsSignaturesCollection)
                {
                    double normedAngle = this.FindAngleBetweenVectors(shellSignature.Coords, shellSignature.Directon, this.ObjectContainer.ControllingObject.Coords) / Math.PI;
                    if (normedAngle < 0.028)
                    {
                        normedShellsCount ++;
                    }
                }
                normedShellsCount = normedShellsCount / shellsSignaturesCollection.Count;
            }
            if (normedEnergy > 0.5 && normedShieldPower > 0 && normedShellsCount > 0.3)
            {
                this.ObjectContainer.ControllingObject.ActivateShield();
            }
            else
            {
                this.ObjectContainer.ControllingObject.DeactivateShield();
            }*/
        }

        /// <summary>
        /// Обучение нейронных сететй
        /// </summary>
        public void NetworksLearn()
        {
        /*    LearningHelper helper = new LearningHelper();
            //обучение сети принятия решений
            List<List<List<double>>> learningSets = helper.LoadLearningSet("decision.nls", 20, 3);
            this.decisionNetwork.Learning(learningSets[0], 0.001, 500, learningSets[1]);
            //Обучение классифицирующей сети 
       /*     learningSets = helper.LoadLearningSet("class", 3, 3);
            this.decisionNetwork.Learning(learningSets[0], 0.001, 10000, learningSets[1]);
            //Обучение защитной сети 
            learningSets = helper.LoadLearningSet("protect", 4, 2);
            this.decisionNetwork.Learning(learningSets[0], 0.001, 1000, learningSets[1]);*/
        }




    }
}
