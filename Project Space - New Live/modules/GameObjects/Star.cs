using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    public class Star : SphereObject
    {

        /// <summary>
        /// Сконструировать звезду
        /// </summary>
        /// <param name="mass">Масса</param>
        /// <param name="radius">Рудиус</param>
        /// <param name="orbit">Расстояние от центра масс звездной системы</param>
        /// <param name="startOrbitalAngle">Начальный поворот звезды</param>
        /// <param name="orbitalSpeed">//Скорость варщениея вокруг центра масс (рад/ед.вр.)</param>
        /// <param name="Skin">Текстура</param>
        public Star(int mass, int radius, int orbit, double startOrbitalAngle, double orbitalSpeed, SFML.Graphics.Texture Skin)
        {
            Random random = new Random();
            this.mass = mass;//инициализировать основные характеристики звезды
            this.radius = radius;
            this.orbit = orbit;
            this.orbitalAngle = startOrbitalAngle;
            this.orbitalSpeed = orbitalSpeed;
            this.Move();//сформировать координаты звезды
            this.ConstructView(Skin);//построить отображение звезды
        }
    }
}
