using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Абстрактная контрольная точка
    /// </summary>
    public abstract class CheckPoint : GameObject
    {
        /// <summary>
        /// Игрок хозяин контрольной точки
        /// </summary>
        protected ActiveObject masterObject;

        /// <summary>
        /// Игрок хозяин контрольной точки
        /// </summary>
        public ActiveObject MasterObject
        {
            get { return this.masterObject; }
        }

        /// <summary>
        /// Смена Игрока-хозяина
        /// </summary>
        /// <param name="newMaster">Новый Игрок-Хозяин</param>
        /// <param name="newSkin">Новая текстура</param>
        public void ChangeMaster(ActiveObject newMaster, Texture[] newSkin)
        {
            this.masterObject = newMaster;
            this.view[0].Image.Texture = newSkin[0];
        }

        protected override void ConstructView(Texture[] skin)
        {
            this.view = new ImageView[1];
            this.view[0] = new ImageView(new CircleShape(50), BlendMode.Alpha);
            this.view[0].Image.Position = this.Coords - new Vector2f(50, 50);
            this.view[0].Image.Texture = skin[0];
        }

        public override void Process(Vector2f homeCoords)
        {
            this.Move();
        }

        protected override ObjectSignature ConstructSignature()
        {
            ObjectSignature retValue = new ObjectSignature();
            retValue.AddCharacteristics(this.Mass);
            retValue.AddCharacteristics(this.view[0].GetSize());
            return retValue;
        }
    }
}
