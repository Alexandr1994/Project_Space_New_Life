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
    public class StarSystem
    {

        /// <summary>
        /// фон звездной системы
        /// </summary>
        RectangleShape background;
        /// <summary>
        /// Главный центр масс
        /// </summary>
        LocalMassCenter massCenter;


        /// <summary>
        /// Построить звездную систему
        /// </summary>
        /// <param name="massCenter"></param>
        /// <param name="background"></param>
        public StarSystem(LocalMassCenter massCenter, Texture background)
        {
            this.massCenter = massCenter;//Инициализация компонетов звездной системы
            InitBackgroung(background);//Построение фона звездной системы
        }


        /// <summary>
        /// Построить фон звездной системы
        /// </summary>
        /// <param name="skin"></param>
        private void InitBackgroung(Texture skin)
        {
            background = new RectangleShape();
            background.Texture = skin;
            background.Size = new Vector2f(2000, 2000);
            background.Position = new Vector2f(-1000, -1000);
        }

        /// <summary>
        /// Процесс жизни звездной системы
        /// </summary>
        public void Process()
        {
            massCenter.Process(new Vector2f(0, 0));//работа со звездной состовляющей
        }


        /// <summary>
        /// Вернуть коллекция отображений объектов звездной системы
        /// </summary>
        /// <returns></returns>
        public List<ObjectView> GetView()
        {
            List<ObjectView> systemsViews = new List<ObjectView>();
            List<ObjectView> systemObjects = massCenter.GetView();
            systemsViews.Add(new ObjectView(background, BlendMode.None));//засунуть в возвращаемый массив фон
            foreach (ObjectView view in systemObjects)//заполнить возвращаемый массив образами звезд
            {
                systemsViews.Add(view);
            }
            return systemsViews;
        }

 }
}
