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
    /// Танковое поле боя
    /// </summary>
    public class BattleField : BaseEnvironment
    {


        public BattleField(Texture background)
        {
            this.MovingResistance = 0.5;
            this.InitBackgroung(background);
            this.myActiveObjectsCollection = new List<ActiveObject>();
            this.myShellsCollection = new List<Shell>();
            this.myEffectsCollection = new List<VisualEffect>();
        }

        /// <summary>
        /// Фон поля боя
        /// </summary>
        /// <param name="skin">Текстура фона</param>
        protected override void InitBackgroung(Texture skin)
        {
            this.background = new ObjectView(new RectangleShape(new Vector2f(2000, 2000)), BlendMode.Add);
            this.background.Image.Position = new Vector2f(0, 0);
            this.background.Image.Texture = skin;
            this.background.Image.Texture.Repeated = true;
            //this.background.Image.Texture.Smooth = true;
            Vector2i textureSize = new Vector2i((int)(this.background.Image.Texture.Size.X), (int)(this.background.Image.Texture.Size.Y));
            this.background.Image.TextureRect = new IntRect(new Vector2i(0, 0), textureSize);
        }

        /// <summary>
        /// Смещение фона
        /// </summary>
        /// <param name="offset">Смещение</param>
        public override void OffsetBackground(Vector2f currentCoords, Vector2f lastCoords)
        {
            this.background.Translate((currentCoords - lastCoords));
            this.background.Image.TextureRect = new IntRect((int)currentCoords.X, (int)currentCoords.Y, 10000, 10000);
        }

        /// <summary>
        /// ВРЕМЕННАЯ РЕАЛИЗАЦИЯ
        /// </summary>
        protected override void CustomProcess()
        {
            ;
        }

        /// <summary>
        /// ВРЕМЕННАЯ РЕАЛИЗАЦИЯ
        /// </summary>
        protected override List<ObjectView> GetCustomViews()
        {
            return new List<ObjectView>();
        }

        /// <summary>
        /// ВРЕМЕННАЯ РЕАЛИЗАЦИЯ
        /// </summary>
        protected override List<GameObject> GetCustomEnvironmentObjects()
        {
            return new List<GameObject>();
        }
        
    }
}
