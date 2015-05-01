using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    class UnhabitatePlanet : Planet
    {
        /// <summary>
        /// Сконструировать новую необитаемую планету
        /// </summary>
        /// <param name="mass">Масса</param>
        /// <param name="radius">Радиус</param>
        /// <param name="orbit">Орбита</param>
        /// <param name="orbitalSpeed">Орбитальная скорость</param>
        /// <param name="Skin">Текструа</param>
        public UnhabitatePlanet(int mass, int radius, int orbit, double orbitalSpeed, SFML.Graphics.Texture Skin)
        {
            Random random = new Random();
            this.mass = mass;//инициализировать основные характеристики планеты
            this.radius = radius;
            this.ordit = orbit;
            this.orbitalSpeed = orbitalSpeed;
            this.orbitalAngle = random.Next();//задать случайный пворот планеты
            //this.move();//сформировать координаты планеты
            this.constructView(Skin);

        }

        protected override void planetProcess(Vector2f globalCoords)
        {
            this.move(globalCoords);
        }

        /// <summary>
        /// Получить сигнатуру необитаемой планеты
        /// </summary>
        /// <returns></returns>
        public override object getSignature()
        {
            Tuple<int, int> localSignature = new Tuple<int, int>(this.mass, this.radius);
            return localSignature;
        }
    }
}
