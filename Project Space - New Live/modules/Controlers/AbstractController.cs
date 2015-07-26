using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.GameObjects;

namespace Project_Space___New_Live.modules.Controlers
{
    public abstract class AbstractController
    {

        private Ship myShip;

        public abstract void Process();

    }
}
