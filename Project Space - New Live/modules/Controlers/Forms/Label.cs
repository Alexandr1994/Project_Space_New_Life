using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.Controlers.Forms
{
    /// <summary>
    /// Надпись
    /// </summary>
    public class Label : TextForm
    {
        /// <summary>
        /// Конструктор надписи
        /// </summary>
        protected override void CustomConstructor()
        {
            this.view = new TextView(this.text, BlendMode.Alpha, this.font);
            this.ResaveTextString();
        }
    }
}
