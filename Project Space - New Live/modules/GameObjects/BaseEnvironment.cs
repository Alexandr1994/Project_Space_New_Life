using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Абстрактная игровая среда
    /// </summary>
    public abstract class BaseEnvironment
    {
        /// <summary>
        /// Сопротивление среды перемещению объектов в ней
        /// </summary>
        private double movingResistance;

        public double MovingResistance
        {
            get { return this.movingResistance; }
            set { this.movingResistance = value; }
        }

        /// <summary>
        /// Фон звездной системы
        /// </summary>
        protected ObjectView background;

        /// <summary>
        /// Управление позицией фона звездной системы
        /// </summary>
        public ObjectView Background
        {
            get { return this.background; }
        }

        /// <summary>
        /// Коллекция кораблей, находящихся в данной звездной системе
        /// </summary>
        protected List<ActiveObject> myActiveObjectsCollection;

        /// <summary>
        /// Коллекция снарядов, находящихся в данной звездной системе
        /// </summary>
        protected List<Shell> myShellsCollection;

        /// <summary>
        /// Коллекция взрывов (визуальных эффектов)
        /// </summary>
        protected List<VisualEffect> myEffectsCollection;

        /// <summary>
        /// Процесс жизни игровой среды
        /// </summary>
        public void Process()
        {
            this.CustomProcess();
            foreach (ActiveObject activeObject in this.myActiveObjectsCollection)//работа кораблей находящихся в данной звездной системе
            {
                activeObject.Process(new Vector2f(0, 0));
                activeObject.AnalizeObjectInteraction();
                if (activeObject.Destroyed)
                {
                    this.myEffectsCollection.Add(new VisualEffect(activeObject.Coords, new Vector2f(144, 144), 52, ResurceStorage.shipExplosion));
                }
            }
            for (int i = 0; i < this.myShellsCollection.Count; i++)//работа со снарядами в данной звездной системе
            {
                this.myShellsCollection[i].Process(new Vector2f(0, 0));
                if (this.myShellsCollection[i].LifeOver)//если время жизни снаряда вышло
                {
                    this.myEffectsCollection.Add(new VisualEffect(this.myShellsCollection[i].Coords, new Vector2f(32, 32), 19, ResurceStorage.shellHitting));
                    this.myShellsCollection.Remove(this.myShellsCollection[i]);//удалить его из коллекции
                    i--;
                }
            }
            for (int i = 0; i < this.myEffectsCollection.Count; i++)//работа с визуальными эффектами
            {
                this.myEffectsCollection[i].Process();
                if (this.myEffectsCollection[i].LifeOver)//если время жизни визуального эффекта вышло
                {
                    this.myEffectsCollection.Remove(this.myEffectsCollection[i]);//удалить его из коллекции
                    i--;
                }
            }
        }

        /// <summary>
        /// Процесс жизни конкретной игровой среды
        /// </summary>
        protected abstract void CustomProcess();

        /// <summary>
        /// Установить или обновить коллекцию активных объектов в середе
        /// </summary>
        /// <param name="activeObjectsCollection">Новая коллекция активных объектов</param>
        public virtual void RefreshActiveObjectsCollection(List<ActiveObject> activeObjectsCollection)
        {
            this.myActiveObjectsCollection = activeObjectsCollection;
        }
    
        /// <summary>
        /// Добавление снаряда в коллекцию снарядов в данной звездной системе
        /// </summary>
        public void AddNewShell(Shell newShell)
        {
            this.myShellsCollection.Add(newShell);
        }

    }
}
