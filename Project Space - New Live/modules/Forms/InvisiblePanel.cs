using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.Forms
{
    /// <summary>
    /// Невидимая панель
    /// </summary>
    class InvisiblePanel : Panel
    {
        /// <summary>
        /// Изменение размера формы
        /// </summary>
        public override Vector2f Size
        {
            get { return this.size; }
            set
            {
                this.size = value;
                RectangleShape tempImage = this.view.Image as RectangleShape;
                tempImage.Size = this.size;
                this.view.Image = tempImage;
            }
        }

        /// <summary>
        /// Конструктор прямоугольной панели
        /// </summary>
        protected override void CustomConstructor()
        {
            view = new ImageView(new RectangleShape(new Vector2f(200, 200)), BlendMode.Alpha);
            this.Location = view.Image.Position = new Vector2f(0, 0);
            this.size = new Vector2f(200, 200);
            this.SetPanelTexture(null);
            this.view.Image.FillColor = new Color(0, 0, 0, 0);
        }
    }
}
