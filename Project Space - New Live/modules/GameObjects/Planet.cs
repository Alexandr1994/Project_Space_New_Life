using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    public class Planet : SphereObject
    {
        /// <summary>
        /// Сконструировать новую необитаемую планету
        /// </summary>
        /// <param name="mass">Масса</param>
        /// <param name="radius">Радиус</param>
        /// <param name="orbit">Орбита</param>
        /// <param name="orbitalSpeed">Орбитальная скорость</param>
        /// <param name="Skin">Текструа</param>
        public Planet(int mass, int radius, int orbit, double orbitalSpeed, SFML.Graphics.Texture Skin)
        {
            Random random = new Random();
            this.mass = mass;//инициализировать основные характеристики планеты
            this.radius = radius;
            this.orbit = orbit;
            this.orbitalSpeed = orbitalSpeed;
            this.orbitalAngle = random.Next();//задать случайный пворот планеты
            this.move(0);//сформировать координаты планеты
            this.constructView(Skin);//сконструировать отображение планеты

        }
        
    }
}
