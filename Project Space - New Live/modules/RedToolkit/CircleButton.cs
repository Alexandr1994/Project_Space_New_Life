using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Project_Space___New_Live.modules;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Project_Space___New_Live.modules.Dispatchers;

namespace RedToolkit
{
    /// <summary>
    /// Круглая/эллиптическая кнопка
    /// </summary>
    class CircleButton : Button
    {

        public override Vector2f Size
        {
            get { return this.size; }
            set
            {
                if (this.View != null)
                {
                    float radius = (this.View.View as CircleShape).Radius;
                    float Xcoef = value.X / radius / 2;
                    float Ycoef = value.Y / radius / 2;
                    this.View.View.Scale = new Vector2f(Xcoef, Ycoef);//Изменение размеров изображения
                    this.size = value;//сохранение размеров
                    this.TextLocationCorrection();
                }
                else
                {
                    size = value;
                }
            }
        }

        /// <summary>
        /// Конструктор круглой кнопки
        /// </summary>
        protected override void CustomConstructor()
        {
            this.Size = new Vector2f(40, 40);
            this.view = new ImageView(new CircleShape(this.Size.X/2), BlendMode.Alpha);
            this.Location = view.Image.Position = new Vector2f(0, 0);
            this.SetViewStates(ResurceStorage.circuleButtonTextures);
            this.view.Image.Texture = this.viewStates[0];
            this.SetLabel();
            this.ButtonViewEventReaction();
          //  this.CatchEvents();
        }

    }
}
