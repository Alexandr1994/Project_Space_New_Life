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
        ObjectView background;
        /// <summary>
        /// Главный центр масс
        /// </summary>
        LocalMassCenter massCenter;

        /// <summary>
        /// Управление позицией фона звездной системы
        /// </summary>
        public ObjectView Background
        {
            get { return this.background; }
        }



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
            background = new ObjectView(new RectangleShape(new Vector2f(2000, 2000)), BlendMode.Alpha);
            background.Image.Texture = skin;
            background.Image.Position = new Vector2f(-1000, -1000);
        }

        /// <summary>
        /// Изменение позиции фона Звездной системы
        /// </summary>
        /// <param name="offset"></param>
        public void OffsetBackground(Vector2f offset)
        {
            this.background.Translate(offset);
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

            systemsViews.Add(this.background);//засунуть в возвращаемый массив фон
            systemsViews.AddRange(massCenter.GetView());//Заполнить массив образов системы образами объектов главного центар масс
   
            return systemsViews;
        }

        /// <summary>
        /// Получить коллекцию объектов находящихся в ситеме (временная реализация)
        /// </summary>
        /// <returns></returns>
        public List<GameObject> GetObjectsInSystem()
        {
            return this.massCenter.GetObjects();
        }

 }
}
