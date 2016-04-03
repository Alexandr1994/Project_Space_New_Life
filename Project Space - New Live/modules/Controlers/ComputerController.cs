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
            this.Forward = true;
            this.RotateToTarget(targetCoords, dangerRadius);
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
        /// Процесс работы контроллера
        /// </summary>
        public override void Process(List<ObjectSignature> signaturesCollection)
        {
            //this.MoveToTarget(this.playerContainer.ControllingObject.GetTargetCheckPoint().Coords);
            this.AttackTarget(this.playerContainer.ControllingObject.GetTargetCheckPoint().Coords, 300);
            this.Moving();
        }
    }
}
