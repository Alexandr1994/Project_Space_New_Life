using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.GameObjects;
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
        /// Текстурная лента визуального эффекта подрыва снаряда
        /// </summary>
        private Texture visualEffectSkin;

        /// <summary>
        /// Сконструировать визуальный эффект подрыва снаряда
        /// </summary>
        /// <param name="effectSize">Размер кадра</param>
        /// <param name="effectTapeLenght">Количество кадров</param>
        /// <returns>Визуальный эффект подрыва снаряда</returns>
        public virtual VisualEffect ConstructDeathVisualEffect(Vector2f effectSize, int effectTapeLenght)
        {
            return new VisualEffect(this.Coords, effectSize, effectTapeLenght, visualEffectSkin);
        }

        /// <summary>
        /// Объект, выстреливший данный снаряд
        /// </summary>
        private ActiveObject1 shooterObject;

        /// <summary>
        /// Объект, выстреливший данный снаряд
        /// </summary>
        public ActiveObject1 ShooterObject
        {
            get { return this.shooterObject; }
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
        /// Урон, наносимый данным снарядом объекту
        /// </summary>
        private int objectDamage;

        /// <summary>
        /// Урон, наносимый, данным снарядом объекту
        /// </summary>
        public int ObjectDamage
        {
            get { return this.objectDamage; }
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

        /// <summary>
        /// Сконструировать сигнатуру
        /// </summary>
        /// <returns></returns>
        protected override ObjectSignature ConstructSignature()
        {
            ObjectSignature signature = new ObjectSignature();
            signature.Coords = this.Coords;
            signature.Mass = this.Mass;
            signature.Size = this.size;
            signature.Speed = this.SpeedVector.Speed;
            signature.Directon = this.SpeedVector.Angle;
            return signature;
        }

        /// <summary>
        /// Конструктор снаряда
        /// </summary>
        /// <param name="shooter">Объект-стрелок</param>
        /// <param name="mass">Масса снаряда</param>
        /// <param name="coords">Начальные координаты</param>
        /// <param name="size">Размеры</param>
        /// <param name="objectDamage">Урон наносимый объекту</param>
        /// <param name="equipmentDamage">Урон наносимый оборудоваию</param>
        /// <param name="speed">Скорость</param>
        /// <param name="angle">Угол поворота вектора направления</param>
        /// <param name="lifeTime">Время жизни</param>
        /// <param name="skin">Массив текстур</param>
        public Shell(ActiveObject1 shooter, float mass, Vector2f coords, Vector2f size, int objectDamage, int equipmentDamage, float speed, float angle, int lifeTime, Texture[] skin)
        {
            this.shooterObject = shooter;
            this.mass = mass;
            this.coords = coords;
            this.size = size;
            this.objectDamage = objectDamage;
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
            this.view = new ImageView[1];
            this.view[(int)ShellParts.Core] = new ImageView(new RectangleShape(new Vector2f(1, 1)), BlendMode.Alpha);
            this.view[(int) ShellParts.Core].Image.Scale = this.size;
            this.View[(int) ShellParts.Core].Image.Position = this.Coords - new Vector2f(this.size.X, this.size.Y);
            this.view[(int) ShellParts.Core].Image.Texture = skin[0];
            this.view[(int) ShellParts.Core].Rotate(this.Coords, this.SpeedVector.Angle);
            this.visualEffectSkin = skin[skin.Length - 1];
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
            foreach (ImageView partView in this.view)
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
