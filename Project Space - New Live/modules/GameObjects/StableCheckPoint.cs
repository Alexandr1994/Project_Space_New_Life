using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Стабильная контрольная точка
    /// </summary>
    public class StableCheckPoint : CheckPoint
    {
        /// <summary>
        /// Конструктор стабильной контрольной точки
        /// </summary>
        /// <param name="startCoords"></param>
        /// <param name="startMaster"></param>
        /// <param name="skin"></param>
        public StableCheckPoint(Vector2f startCoords, Texture[] skin)
        {
            this.coords = startCoords;
            this.ConstructView(skin);
        }

        /// <summary>
        /// Стабильная конторольная точка - неподвижна
        /// </summary>
        protected override void Move()
        {
            this.view[0].Rotate(this.view[0].ViewCenter, (float)(3 * Math.PI / 180));
        }
    }
}
