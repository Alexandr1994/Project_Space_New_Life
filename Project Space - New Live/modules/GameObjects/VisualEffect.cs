using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    /// <summary>
    /// Визуальный эффект
    /// </summary>
    public class VisualEffect
    {
        /// <summary>
        /// Отображение визуального эффекта
        /// </summary>
        ObjectView view;

        /// <summary>
        /// Отображение визуального эффекта
        /// </summary>
        public ObjectView View
        {
            get { return this.view; }
        }

        /// <summary>
        /// Время жизни эффекта
        /// </summary>
        private int lifeTime;

        /// <summary>
        /// Флаг окончания времени существования визуального эффекта
        /// </summary>
        private bool lifeOver = false;

        /// <summary>
        /// Флаг окончания времени существования визуального эффекта
        /// </summary>
        public bool LifeOver
        {
            get { return this.lifeOver; }
        }

        /// <summary>
        /// Таймер времени жизни визуального эффекта
        /// </summary>
        private Clock lifeTimer;

        /// <summary>
        /// Размеры отображения визуального эффекта
        /// </summary>
        private Vector2f sizes;

        /// <summary>
        /// Количество изображений в текстурной ленте
        /// </summary>
        private int tapeLenght;

        /// <summary>
        /// Конструктор визуального эффекта
        /// </summary>
        /// <param name="coords">Координаты</param>
        /// <param name="sizes">Размеры</param>
        /// <param name="tapeLenght">Колличество изображений в текстурной ленте</param>
        /// <param name="skinTape">Текстурная лента</param>
        public VisualEffect(Vector2f coords, Vector2f sizes, int tapeLenght, Texture skinTape)
        {
            this.view = new ObjectView(new RectangleShape(sizes), BlendMode.Alpha);
            this.view.Image.Position = coords - new Vector2f(sizes.X/2, sizes.Y/2);
            this.sizes = sizes;
            this.view.Image.Texture = skinTape;
            this.tapeLenght = tapeLenght;
            this.lifeTime = 1 + tapeLenght * SystemRoot.SleepTime;//вычисление времени жизни эффекта
            this.view.Image.TextureRect = new IntRect(new Vector2i(0, 0), new Vector2i((int)sizes.X, (int)sizes.Y));
            this.lifeTimer = new Clock();//запуск таймера
            this.lifeTimer.Restart();
            
        }

        /// <summary>
        /// Процесс работы визуального эффекта
        /// </summary>
        public void Process()
        {
            float XOffset = (float)(this.view.Image.Texture.Size.X / tapeLenght);//вычисление смещения до следующего участка ленты
            Vector2i textureOffset = new Vector2i(this.View.Image.TextureRect.Left + (int)XOffset, 0);//Переход на следующий участок ленты текстуры
            this.view.Image.TextureRect = new IntRect(textureOffset, new Vector2i((int)sizes.X, (int)sizes.Y));
            if (this.lifeTimer.ElapsedTime.AsMilliseconds() > this.lifeTime)//если время существования визуальногол эффекто превысило пороговое значение
            {
                this.lifeOver = true;//установить флаг окончания времени жизни визуального эффекта
            }
        }

    }
}
