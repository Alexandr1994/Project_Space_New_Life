using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Сигнатура игровых объектов
    /// </summary>
    public class ObjectSignature
    {

        private Dictionary<String, int> signature;

        public Dictionary<String, int> Signature
        {
            get { return signature; }
        }

    }
}
