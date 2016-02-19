using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects.ShipModules;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Активный игровой объект
    /// </summary>
    public abstract class ActiveObject : GameObject
    {

        /// <summary>
        /// Текущий поворот активного объекта в рад.
        /// </summary>
        protected float rotation = (float)(Math.PI / 2);

        /// <summary>
        /// Текущий поворот активного объекта в рад.
        /// </summary>
        public float Rotation
        {
            get { return this.rotation; }
        }

        /// <summary>
        /// Текущий запас прочности
        /// </summary>
        protected int health;

        /// <summary>
        /// Текущий запас прочности
        /// </summary>
        public int Health
        {
            get { return this.health; }
        }

        /// <summary>
        /// Максимальный запас прочности
        /// </summary>
        protected int maxHealth;

        /// <summary>
        /// Максимальный запас прочности
        /// </summary>
        public int MaxHealth
        {
            get { return this.maxHealth; }
        }


        /// <summary>
        /// Оружейная система активного объекта
        /// </summary>
        protected WeaponSystem objectWeaponSystem;

        /// <summary>
        /// Оружейная система корабля
        /// </summary>
        public WeaponSystem ObjectWeaponSystem
        {
            get { return this.objectWeaponSystem; }
        }

        /// <summary>
        /// Открыть огонь
        /// </summary>
        public void OpenFire()
        {
            this.objectWeaponSystem.Shooting = true;
        }

        /// <summary>
        /// Прекратить огонь
        /// </summary>
        public void StopFire()
        {
            this.objectWeaponSystem.Shooting = false;
        }

        /// <summary>
        /// Элементарное движение активного объекта
        /// </summary>
        /// <param name="speed">Скорость движения</param>
        /// <param name="angle">Угол поворота вектора скорости</param>
        public void ShipAtomMoving(float speed, float angle)
        {
            Vector2f tempCoords = this.coords;
            this.coords.X += (float)(speed * Math.Cos(angle));
            this.coords.Y += (float)(speed * Math.Sin(angle));
            Vector2f delta = this.coords - tempCoords;//Изменение по координатам Х и Y
            foreach (ObjectView partView in this.view)
            {
                partView.Translate(delta);
            }
        }

        /// <summary>
        /// Изменение поворота активного объекта
        /// </summary>
        /// <param name="angle">Угол на который происходит изменение</param>
        public void ChangeRotation(float angle)
        {
            this.rotation += angle;//изменение текущего поворота корабля
            foreach (ObjectView partView in this.view)
            {
                partView.Rotate(this.coords, angle);//изменение каждой части отображения
            }
            if (this.rotation > 2 * Math.PI)
            {
                this.rotation -= (float)(2 * Math.PI);
            }
        }

        /// <summary>
        /// Анализирование взаимодействия данного объекта с другими объектами в среде
        /// </summary>
        public abstract void AnalizeObjectInteraction();

    }
}
