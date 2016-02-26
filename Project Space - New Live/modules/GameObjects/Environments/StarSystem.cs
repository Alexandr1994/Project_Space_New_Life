using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Звездная система
    /// </summary>
    public class StarSystem : BaseEnvironment
    {

        /// <summary>
        /// Главный центр масс
        /// </summary>
        private LocalMassCenter massCenter;

        /// <summary>
        /// Построить звездную систему
        /// </summary>
        /// <param name="massCenter">Готовый центр масс</param>
        /// <param name="background">Текстура фона</param>
        public StarSystem(LocalMassCenter massCenter, Texture background)
        {
            this.MovingResistance = 0.5;
            this.massCenter = massCenter; //Инициализация компонетов звездной системы
            this.InitBackgroung(background); //Построение фона звездной системы
            this.myShellsCollection = new List<Shell>();
            this.myEffectsCollection = new List<VisualEffect>();
        }

        /// <summary>
        /// Построить фон звездной системы
        /// </summary>
        /// <param name="skin">Текстура фона</param>
        protected override void InitBackgroung(Texture skin)
        {
            this.background = new ObjectView(new RectangleShape(new Vector2f(2000, 2000)), BlendMode.Alpha);
            this.background.Image.Texture = skin;
            this.background.Image.Position = new Vector2f(-500, -1000);
        }

        /// <summary>
        /// Изменение позиции фона Звездной системы
        /// </summary>
        /// <param name="offset">Смещение</param>
        public override void OffsetBackground(Vector2f currentCoords, Vector2f lastCoords)
        {
            Vector2f offset = lastCoords - currentCoords;
            this.background.Translate(new Vector2f((float)(offset.X * (-0.9)), (float)(offset.Y * (-0.9))));
        }


        /// <summary>
        /// Процесс жизни звездной системы
        /// </summary>
        protected override void CustomProcess()
        {
            massCenter.Process(new Vector2f(0, 0)); //работа со звездной составляющей
        }

        /// <summary>
        /// Получить отображения звезд и планет звездной системы
        /// </summary>
        /// <returns></returns>
        protected override List<ObjectView> GetCustomViews()
        {
            return this.massCenter.GetView();
        }

        /// <summary>
        /// Получить объекты звездной системы
        /// </summary>
        /// <returns>Коллекция объектов звездной системы</returns>
        protected override List<GameObject> GetCustomEnvironmentObjects()
        {
            return this.massCenter.GetObjects(); //получить коллекцию звезд и планет
        }

        /// <summary>
        /// Установить или обновить коллекцию кораблей в звездной системе
        /// </summary>
        /// <param name="activeObjectsCollection">Новая коллекция кораблей</param>
        public override void RefreshActiveObjectsCollection(List<ActiveObject> activeObjectsCollection)
        {
            this.myActiveObjectsCollection = new List<ActiveObject>(); //освобождение коллекции
            foreach (Ship currentShip in activeObjectsCollection) //перезаполнение коллекции
            {
                if (currentShip.Environment == this)
                {
                    this.myActiveObjectsCollection.Add(currentShip);

                }
            }
        }

    }
}
