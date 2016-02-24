using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects.ShipModules;
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

        public BattleField TankBattleField
        {
            get { return this.environment as BattleField; }
        }


        public Tank(float mass, Vector2f coords, int maxHealth, Texture[] skin, Vector2f partSize)
        {
            this.mass = mass;
            this.coords = coords;
            this.ViewPartSize = partSize;
            this.ConstructView(skin);
            this.brains = brains;
            this.maxHealth = this.health = maxHealth;
            this.transportEngine = new Engine(100, 1, 100, 100, 10, 8, null);//двигатель
            this.objectReactor = new Reactor(100, 5, null);//реактор
            this.objectBattery = new Battery(100, 500, null);//энергобатарея
            this.objectRadar  = new Radar(20, 2500, null);//радар
            this.objectShield = new Shield(20, 3, 100, 0, 1, null);//энергощит 
            this.objectWeaponSystem = new WeaponSystem(3);
            //this.objectWeaponSystem.AddWeapon(new Weapon(25, 1, 5, 5, 0, 0, (float) (5*Math.PI/180), 100, 100, 1000, 15, 10, new Vector2f(5, 2), new Texture[] {ResurceStorage.rectangleButtonTextures[0]}, null));
            this.objectWeaponSystem.AddWeapon(new Weapon(25, 1, 5, 5, 0, 1, (float)(5*Math.PI/180), 100, 250, 5000, 25, 1, new Vector2f(15, 2), new Texture[]{ResurceStorage.rectangleButtonTextures[3]}, null));
            
            // this.shipEquipment.Add(null);//энергощит 
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
