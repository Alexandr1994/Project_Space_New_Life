using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Project_Space___New_Live.modules.Dispatchers;

namespace Project_Space___New_Live.modules.Controlers
{
    class CircleButton : Button
    {

        private float radius;

        public CircleButton()
        {
            this.view = new ObjectView(new CircleShape(), BlendMode.Alpha);
        }

        protected override bool MoveTest()
        {
            return false;
        }

    }
}
