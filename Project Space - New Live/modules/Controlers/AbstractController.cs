using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.GameObjects;

namespace Project_Space___New_Live.modules.Controlers
{
    /// <summary>
    /// Аьъбстрактная система управления кораблем (контроллер)
    /// </summary>
    public abstract class AbstractController
    {




        /// <summary>
        /// Процесс работы контроллера
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// Переодическое обнуление флагов контроллера
        /// </summary>
        protected void RefreshFlags()
        {
           ;
        }

    }
}
