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

        List<Wall> myWallsCollection; 

        /// <summary>
        /// Конструктор поля боя
        /// </summary>
        /// <param name="background">Текстура фона</param>
        public BattleField(List<Wall> walls, Texture background)
        {
            this.myWallsCollection = new List<Wall>();
            this.myWallsCollection.AddRange(walls);
            this.MovingResistance = 0.5;
            this.InitBackgroung(background);
            this.myActiveObjectsCollection = new List<ActiveObject>();
            this.myShellsCollection = new List<Shell>();
            this.myEffectsCollection = new List<VisualEffect>();
        }

        /// <summary>
        /// Построить фон поля боя
        /// </summary>
        /// <param name="skin">Текстура фона</param>
        protected override void InitBackgroung(Texture skin)
        {
            this.background = new ImageView(new RectangleShape(new Vector2f(10000, 10000)), BlendMode.Add);
            this.background.Image.Position = new Vector2f(-5000, -5000);
            this.background.Image.Texture = skin;
            this.background.Image.Texture.Repeated = true;
            this.background.Image.Texture.Smooth = true;
            this.background.Image.TextureRect = new IntRect(0, 0, 100, 10);
            Vector2i textureSize = new Vector2i((int)(this.background.Image.Texture.Size.X), (int)(this.background.Image.Texture.Size.Y));
            this.background.Image.TextureRect = new IntRect(new Vector2i(0, 0), textureSize);
        }

        /// <summary>
        /// Смещение фона
        /// </summary>
        /// <param name="offset">Смещение</param>
        public override void OffsetBackground(Vector2f currentCoords, Vector2f lastCoords)
        {
            ;
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
        protected override List<RenderView> GetCustomViews()
        {
            List<RenderView> retViews = new List<RenderView>();
            foreach (Wall wall in this.myWallsCollection)
            {
                retViews.AddRange(wall.View);
            }
            return retViews;
        }

        /// <summary>
        /// ВРЕМЕННАЯ РЕАЛИЗАЦИЯ
        /// </summary>
        protected override List<GameObject> GetCustomEnvironmentObjects()
        {
            List<GameObject> retObjects = new List<GameObject>();
            retObjects.AddRange(this.myWallsCollection);
            return retObjects;
        }
        
    }
}
