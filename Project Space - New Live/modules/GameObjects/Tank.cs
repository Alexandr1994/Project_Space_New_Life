using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Танк
    /// </summary>
    public class Tank : Transport
    {
        /// <summary>
        /// Индексы частей танка
        /// </summary>
        public enum Parts
        {
            /// <summary>
            /// Лобовая часть танка
            /// </summary>
            FrontPart = 0,
            /// <summary>
            /// Тыловая часть
            /// </summary>
            BackPart,
            /// <summary>
            /// Правая часть
            /// </summary>
            RightSide,
            /// <summary>
            /// Левая часть
            /// </summary>
            LeftSide,
            /// <summary>
            /// Башня
            /// </summary>
            Turret,
            /// <summary>
            /// Шит
            /// </summary>
            Shield
        }

        /// <summary>
        /// Отображение танка
        /// </summary>
        public override ObjectView[] View
        {
            get
            {
                if (this.ShieldActive)
                {
                    return this.view;
                }
                ObjectView[] retViews = new ObjectView[5];
                int index = 0;
                while (index != (int)(Parts.Shield))
                {
                    retViews[index] = this.view[index];
                    index++;
                }
                return retViews;
            }
        }

        /// <summary>
        /// Поле боя, на котором находится танк
        /// </summary>
        public BattleField TankBattleField
        {
            get { return this.environment as BattleField; }
        }

        /// <summary>
        /// Угол поворота башни танка
        /// </summary>
        private float turretRotation = (float)(Math.PI / 2);

        /// <summary>
        /// Угол поворота башни танка
        /// </summary>
        public float TurretRotation
        {
            get { return turretRotation; }
        }

        /// <summary>
        /// Угол атаки
        /// </summary>
        public override float AttackAngle
        {
            get {return (float)(this.turretRotation + 2 * Math.PI);}
        }

        /// <summary>
        /// Изменение угла поворота танка
        /// </summary>
        /// <param name="angle">Угол на который происходит изменение в рад.</param>
        public override void ChangeRotation(float angle)
        {
            this.rotation += angle;//изменение текущего поворота корабля
            int index = 0;
            while (index != (int)(Parts.Turret))
            {
                this.view[index].Rotate(this.coords, angle);//изменение каждой части отображения
                index ++;
            }
            if (this.rotation > 2 * Math.PI)
            {
                this.rotation -= (float)(2 * Math.PI);
            }
        }

        /// <summary>
        /// Изменение угла поворота башни танка
        /// </summary>
        /// <param name="angle">Новый угол поворота</param>
        public void RotateTurret(float angle)
        {
            if (angle > Math.PI)//если угол поворота вектора больше чем 360 градусов
            {
                angle -= (float)(2 * Math.PI);//вернуть его в пределы от 0 до 360 градусов
            }
            if (angle < -Math.PI)
            {
                angle += (float)(2 * Math.PI);//вернуть его в пределы от 0 до 360 градусов
            }
            this.View[(int)(Parts.Turret)].Rotate(this.View[(int)(Parts.Turret)].ImageCenter, angle - this.TurretRotation);
            this.turretRotation = angle;
        }

        /// <summary>
        /// Конструктор танка
        /// </summary>
        /// <param name="mass">Масса</param>
        /// <param name="coords">Начальные координаты</param>
        /// <param name="maxHealth">Максимальный запас прочности</param>
        /// <param name="skin">Набор текстур</param>
        /// <param name="partSize">Размер основных частей отображения</param>
        public Tank(float mass, Vector2f coords, int maxHealth, Texture[] skin, Vector2f partSize)
        {
            this.mass = mass;
            this.coords = coords;
            this.ViewPartSize = partSize;
            this.ConstructView(skin);
            this.maxHealth = this.health = maxHealth;
            this.transportEngine = new Engine(100, 1, 100, 100, 10, 8, null);//двигатель
            this.objectReactor = new Reactor(100, 5, null);//реактор
            this.objectBattery = new Battery(100, 500, null);//энергобатарея
            this.objectRadar  = new Radar(20, 2500, null);//радар
            this.objectShield = new Shield(20, 3, 100, 0, 1, null);//энергощит 
            this.objectWeaponSystem = new WeaponSystem(3);
            //this.objectWeaponSystem.AddWeapon(new Weapon(25, 1, 5, 5, 0, 0, (float) (5*Math.PI/180), 100, 100, 1000, 15, 10, new Vector2f(5, 2), new Texture[] {ResurceStorage.rectangleButtonTextures[0]}, null));
            this.objectWeaponSystem.AddWeapon(new Weapon(25, 1, 5, 5, 0, 1, (float)(5*Math.PI/180), 100, 2000, 5000, 25, 1, new Vector2f(15, 2), new Texture[]{ResurceStorage.rectangleButtonTextures[3]}, null));
        }

        /// <summary>
        /// Построить отображение танка
        /// </summary>
        /// <param name="skin">Массив текстур частей танка</param>
        protected override void ConstructView(Texture[] skin)
        {
            //TODO Придумать защиту
            this.view = new ObjectView[6];
            int index = 0;
            while (index != (int)(Parts.Shield))
            {
                this.view[index] = new ObjectView(BlendMode.Alpha);
                this.view[index].Image = new RectangleShape(this.ViewPartSize);
                this.view[index].Image.Texture = skin[index];
                index++;
            }
            this.view[(int)Parts.Shield] = new ObjectView(BlendMode.Alpha);
            this.view[(int)Parts.Shield].Image = new CircleShape(this.ViewPartSize.Y);
            this.view[(int)Parts.Shield].Image.Texture = skin[(int)Ship.Parts.Shield];
            this.view[(int)Parts.Shield].Image.Position = this.Coords - new Vector2f(this.ViewPartSize.Y, this.ViewPartSize.Y);//Энергощит
            this.view[(int)Parts.FrontPart].Image.Position = this.Coords + new Vector2f(-this.ViewPartSize.X / 2, 0);//Лобовая часть
            this.view[(int)Parts.BackPart].Image.Position = this.Coords + new Vector2f(-this.ViewPartSize.X / 2, -this.ViewPartSize.Y);//Тыловая часть
            this.view[(int)Parts.RightSide].Image.Position = this.Coords + new Vector2f(this.ViewPartSize.X / 2, -this.ViewPartSize.Y / 2);//Левый борт
            this.view[(int)Parts.LeftSide].Image.Position = this.Coords + new Vector2f(-this.ViewPartSize.X * 3 / 2, -this.ViewPartSize.Y / 2);//Правое борт;
            this.view[(int)Parts.Turret].Image.Position = this.Coords + new Vector2f(-this.ViewPartSize.X / 2, -this.ViewPartSize.Y * 2 / 3);//Башня;
        }

        /// <summary>
        /// Вернуть сигнатуру танка
        /// </summary>
        /// <returns>Сигнатура танка</returns>
        protected override ObjectSignature ConstructSignature()
        {
            ObjectSignature signature = new ObjectSignature();
            signature.AddCharacteristics(this.mass);


            Vector2f sizes = new Vector2f(this.ViewPartSize.X * 3, this.ViewPartSize.Y * 2);
            signature.AddCharacteristics(sizes);
            return signature;
        }

        protected override void WearingEquipment(int equipmentDamage, int damagedPartIndex)
        {
            ;
        }


        public override void AnalizeObjectInteraction()
        {
            ;
        }
    }
}
