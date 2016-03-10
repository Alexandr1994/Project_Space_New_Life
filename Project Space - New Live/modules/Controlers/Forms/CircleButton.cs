using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Project_Space___New_Live.modules.Dispatchers;

namespace Project_Space___New_Live.modules.Controlers.Forms
{
    /// <summary>
    /// Круглая/эллиптическая кнопка
    /// </summary>
    class CircleButton : Button
    {

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
            this.ButtonViewEventReaction();
            this.CatchEvents();
        }

    }
}
