using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.GameObjects.ShipModules;
using SFML.Graphics;
using SFML.System;
using Project_Space___New_Live.modules.Dispatchers;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Снаряд
    /// </summary>
    public class Shell : GameObject
    {

        /// <summary>
        /// Индексы частей снаряда
        /// </summary>
        public enum ShellParts
        {
            /// <summary>
            /// Боевая часть снаряда
            /// </summary>
            Core = 0
        }
        
        /// <summary>
        /// Корабль, выстреливший данный снаряд
        /// </summary>
        private Ship shooterShip;

        /// <summary>
        /// Корабль, выстреливший данный снаряд
        /// </summary>
        public Ship ShooterShip
        {
            get { return this.shooterShip; }
        }

        /// <summary>
        /// Вектор скорости снаряда
        /// </summary>
        private SpeedVector speedVector;

        /// <summary>
        /// Вектор скорости снаряда
        /// </summary>
        public SpeedVector SpeedVector
        {
            get { return this.speedVector; }
        }

        /// <summary>
        /// Урон, наносимый данным снарядом кораблю
        /// </summary>
        private int shipDamage;

        /// <summary>
        /// Урон, наносимый, данным снарядом кораблю
        /// </summary>
        public int ShipDamage
        {
            get { return this.shipDamage; }
        }

        /// <summary>
        /// Урон, наносимый, данным снарядом оборудованию
        /// </summary>
        private int equipmentDamage;

        /// <summary>
        /// Урон, наносимый, данным снарядом оборудованию
        /// </summary>
        public int EquipmentDamage
        {
            get { return this.equipmentDamage; }
        }

        /// <summary>
        /// Время жизни данного снаряда
        /// </summary>
        private int lifeTime;

        /// <summary>
        /// Таймер жизни данного снаряда
        /// </summary>
        private Clock lifeTimer;

        /// <summary>
        /// Флаг окончания времени жизни снаряда
        /// </summary>
        private bool lifeOver = false;

        /// <summary>
        /// Флаг окончания времени жизни снаряда
        /// </summary>
        public bool LifeOver
        {
            get { return this.lifeOver; }
        }

        /// <summary>
        /// Размер снаряда
        /// </summary>
        private Vector2f size;

        protected override ObjectSignature ConstructSignature()
        {
            return null;
        }

        public Shell(Ship shooter, float mass, Vector2f coords, Vector2f size, int shipDamage, int equipmentDamage, float speed, float angle, int lifeTime, Texture[] skin)
        {
            this.shooterShip = shooter;
            this.mass = mass;
            this.coords = coords;
            this.size = size;
            this.shipDamage = shipDamage;
            this.equipmentDamage = equipmentDamage;
            this.speedVector = new SpeedVector(speed, angle);
            this.lifeTime = lifeTime;
            this.lifeTimer = new Clock();
            this.ConstructView(skin);
        }

        /// <summary>
        /// Постороить отображение снаряда
        /// </summary>
        /// <param name="skin">Массив текстур частей снаряда</param>
        protected override void ConstructView(Texture[] skin)
        {
            this.view = new ObjectView[1];
            this.view[(int)ShellParts.Core] = new ObjectView(new CircleShape(1), BlendMode.Alpha);
            this.view[(int) ShellParts.Core].Image.Scale = this.size;
            this.View[(int) ShellParts.Core].Image.Position = this.Coords - new Vector2f(this.size.X, this.size.Y);
            this.view[(int) ShellParts.Core].Image.Texture = skin[0];
            this.view[(int) ShellParts.Core].Rotate(this.Coords, this.SpeedVector.Angle);
        }

        /// <summary>
        /// Движение снаряда
        /// </summary>
        protected override void Move()
        {
            Vector2f tempCoords = this.coords;
            this.coords.X += (float)(this.speedVector.Speed * Math.Cos(this.speedVector.Angle));
            this.coords.Y += (float)(this.speedVector.Speed * Math.Sin(this.speedVector.Angle));
            Vector2f delta = this.coords - tempCoords;//Изменение по координатам Х и Y
            foreach (ObjectView partView in this.view)
            {
                partView.Translate(delta);
            }
        }

        /// <summary>
        /// Процесс жизни снаряда
        /// </summary>
        /// <param name="homeCoords">Координаты начала отсчета</param>
        public override void Process(Vector2f homeCoords)
        {
            this.Move();
            if (this.lifeTimer.ElapsedTime.AsMilliseconds() > this.lifeTime)
            {
                this.lifeOver = true;
            }
        }

        /// <summary>
        /// Установка флага окончания времени жизни снаряда
        /// </summary>
        public void HitToTarget()
        {
            this.lifeOver = true;
        }

    }
}
