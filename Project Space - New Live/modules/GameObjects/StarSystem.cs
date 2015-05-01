using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    class StarSystem : GameObject
    {

        private int radius;//радиус звезды
        private Texture background;//фон звездной системы
        private List<Planet> planetSysytem = new List<Planet>();





        /// <summary>
        /// Звезда не двигается
        /// </summary>
        public override void move(Vector2f globalCoords)
        {
           
        }

        /// <summary>
        /// Постороить отображение звезды
        /// </summary>
        /// <param name="skin">Текстура</param>
        /// <returns></returns>
        protected override void constructView(Texture skin)
        {
            
        }


        /// <summary>
        /// Получить сигнатуру звезды (массу и радиус)
        /// </summary>
        /// <returns></returns>
        public override object getSignature()
        {
            Tuple<int, int> locSignature = new Tuple<int,int>(mass, radius);
            Object r = new Object();
            return locSignature;
        }

    }
}
