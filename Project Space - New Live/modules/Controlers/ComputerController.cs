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
        /// Текущий энекретический режим
        /// </summary>
        private EnergyMode currentEnergyMode = EnergyMode.Maximal;

        /// <summary>
        /// Конструктор компьтерного контроллера
        /// </summary>
        /// <param name="playerContainer"></param>
        public ComputerController(PlayerContainer playerContainer)
        {
            this.playerContainer = playerContainer;
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
                float maxSpeed = (this.playerContainer.ControllingObject.Equipment[(int)(ActiveObject.EquipmentNames.Engine)] as Engine).MaxForwardSpeed;
                float currentSpeed = this.playerContainer.ControllingObject.MoveManager.ConstructResultVector().Speed;
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
            Vector2f divCoords = targetCoords - this.playerContainer.ControllingObject.Coords;//Вычисление основынх параметров
            float distance = (float)(Math.Sqrt(Math.Pow(divCoords.X, 2) + Math.Pow(divCoords.Y, 2)));
            float relativeAngle = (float) (Math.Atan2(divCoords.Y,  divCoords.X));
            float angleBetween = this.FindAngleBetweenVectors(this.playerContainer.ControllingObject.Coords, this.playerContainer.ControllingObject.Rotation, targetCoords);
            if (angleBetween > 5 * Math.PI / 180)//если угол между векторами направление и положения цели больше порога
            {
                if (distance > dangerRadius)//если расстояние до цели больше радиуса опасной зоны
                {
                    if (Math.Abs(relativeAngle) > 4 * Math.PI / 5 && angleBetween < Math.PI / 2)//проверить нахождение в "мертовой" угловой зоне
                    {
                        return;//если объект в мертвой зоне сохранить его предыдушее состояние
                    }
                    if (relativeAngle < this.playerContainer.ControllingObject.Rotation)//иначе оценить куда нужно произвести поворот и установить соответствующие флаги
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
                    if (relativeAngle >= this.playerContainer.ControllingObject.Rotation)//то начать уклонение
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
                    if (relativeAngle >= this.playerContainer.ControllingObject.Rotation)//и начать уклонение если требуется
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
        public override void Process(List<ObjectSignature> signaturesCollection)
        {
            this.EnergyAnalise();
            this.AttackTarget(this.playerContainer.ControllingObject.GetTargetCheckPoint().Coords, 300);
            this.Moving();
        }




        /// <summary>
        /// Атака в максимальном энергорежиме
        /// </summary>
        /// <param name="targetCoords">Координаты цели</param>
        /// <param name="dangerRadius">Радиус опасной зоны, около цели (По-умолчанию 300)</param>
        private void MaximumAttack(Vector2f targetCoords, float dangerRadius = 300)
        {
            Vector2f divCoords = targetCoords - this.playerContainer.ControllingObject.Coords;//Вычисление основынх параметров
            float distance = (float)(Math.Sqrt(Math.Pow(divCoords.X, 2) + Math.Pow(divCoords.Y, 2)));
            float angleBetween = this.FindAngleBetweenVectors(this.playerContainer.ControllingObject.Coords, this.playerContainer.ControllingObject.Rotation, targetCoords);//Вычисление параметров
            this.MoveToTarget(targetCoords, dangerRadius);//наведение на цель
            if (!this.playerContainer.ControllingObject.ObjectWeaponSystem.HasAmmo)//если боезапаса нет
            {
                return;//окончить работу боевой функции
            }
            Weapon currentWeapon = this.playerContainer.ControllingObject.ObjectWeaponSystem.GetActiveWeapon();//получить активное оружие
            if (currentWeapon.Ammo < 1)//если боезапас данного оружия израсходован
            {
                int activeWeaponIndex = this.playerContainer.ControllingObject.ObjectWeaponSystem.IndexOfActiveWeapon;//получить индекс текущего оружия
                if (!this.playerContainer.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(activeWeaponIndex + 1))//установить индекс следующего оружия
                {
                    this.playerContainer.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(0);//если не удалось то установить индекс самого первого оружия в коллекции
                }
                return;//окончить работу боевой функции
            }
            if (distance < currentWeapon.Range)//если объект находится в зоне поражения
            {
                if (angleBetween < currentWeapon.Dispersion)//и прицеливания
                {
                    this.playerContainer.ControllingObject.OpenFire();//то открыть огонь
                    return;//окончить работу боевой функции
                }
            }
            this.playerContainer.ControllingObject.StopFire();//иначе прекратить огонь
        }

        /// <summary>
        /// Атака в экономичном энергорежиме
        /// </summary>
        /// <param name="targetCoords">Координаты цели</param>
        /// <param name="dangerRadius">Радиус опасной зоны, около цели (По-умолчанию 300)</param>
        private void EconomicAttack(Vector2f targetCoords, float dangerRadius = 300)
        {
            Vector2f divCoords = targetCoords - this.playerContainer.ControllingObject.Coords;//Вычисление основынх параметров
            float distance = (float)(Math.Sqrt(Math.Pow(divCoords.X, 2) + Math.Pow(divCoords.Y, 2)));
            float angleBetween = this.FindAngleBetweenVectors(this.playerContainer.ControllingObject.Coords, this.playerContainer.ControllingObject.Rotation, targetCoords);//Вычисление параметров
            this.MoveToTarget(targetCoords, dangerRadius);//наведение на цель
            if (!this.playerContainer.ControllingObject.ObjectWeaponSystem.HasAmmo)//если боезапаса нет
            {
                return;//окончить работу боевой функции
            }
            Weapon currentWeapon = this.playerContainer.ControllingObject.ObjectWeaponSystem.GetActiveWeapon();//получить активное оружие
            if (currentWeapon.Ammo < 1)//если боезапас данного оружия израсходован
            {
                int activeWeaponIndex = this.playerContainer.ControllingObject.ObjectWeaponSystem.IndexOfActiveWeapon;//получить индекс текущего оружия
                if (!this.playerContainer.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(activeWeaponIndex + 1))//установить индекс следующего оружия
                {
                    this.playerContainer.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(0);//если не удалось то установить индекс самого первого оружия в коллекции
                }
                return;//окончить работу боевой функции
            }
            if (distance < currentWeapon.Range)//если объект находится в зоне поражения
            {
                if (angleBetween < currentWeapon.Dispersion)//и прицеливания
                {
                    if (this.playerContainer.GetEnergy() > 15)
                    {
                        this.playerContainer.ControllingObject.OpenFire();//то открыть огонь
                        return;//окончить работу боевой функции
                    }
                }
            }
            this.playerContainer.ControllingObject.StopFire();//иначе прекратить огонь
        }

        /// <summary>
        /// Управление энергорежимами
        /// </summary>
        private void EnergyAnalise()
        {
            switch (this.currentEnergyMode)
            {
                case EnergyMode.Maximal:
                {
                    if (this.playerContainer.GetEnergy() < 20)
                    {
                        this.currentEnergyMode = EnergyMode.Economic;
                    }
                    return;
                }
                case EnergyMode.Economic:
                {
                    if (this.playerContainer.GetEnergy() > 60)
                    {
                        this.currentEnergyMode = EnergyMode.Maximal;
                    }
                    return;
                }
            }
        }

    }
}
