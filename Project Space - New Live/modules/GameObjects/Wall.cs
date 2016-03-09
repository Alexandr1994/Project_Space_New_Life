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
    /// <summary>
    /// Препядствие
    /// </summary>
    public class Wall : GameObject
    {

        /// <summary>
        /// Прозрачность для снарядов
        /// </summary>
        private bool shellTransparancy;

        /// <summary>
        /// Размер препядствия
        /// </summary>
        protected Vector2f size;

        /// <summary>
        /// Размер препядствия
        /// </summary>
        public Vector2f Size
        {
            get { return this.size; }
        }

        /// <summary>
        /// Конструктор препядствия
        /// </summary>
        /// <param name="coords">Координаты препядствия</param>
        /// <param name="size">Размеры препядствия</param>
        /// <param name="skin">Массив текстур препядствия</param>
        /// <param name="shellTrancparansy">Прозрачность для снарядов</param>
        /// <param name="mass">Масса препядствия</param>
        public Wall(Vector2f coords, Vector2f size, Texture[] skin, bool shellTrancparansy, int mass = 10000)
        {
            this.mass = mass;
            this.shellTransparancy = shellTrancparansy;
            this.coords = coords;
            this.size = size;
            this.ConstructView(skin);
        }

        /// <summary>
        /// Построить отображение препядствия
        /// </summary>
        /// <param name="skin">Массив текстур препядствия</param>
        protected override void ConstructView(Texture[] skin)
        {
            //TODO Зaщита от неправильной размерности
            this.view = new ImageView[1];
            this.view[0] = new ImageView(new RectangleShape(this.size), BlendMode.Alpha);
            this.view[0].Image.Texture = skin[0];
            this.view[0].Image.Position = this.coords - this.size / 2;
        }

        /// <summary>
        /// Препядствия не перемещаются
        /// </summary>
        protected override void Move()
        {
            ;
        }

        /// <summary>
        /// Внутри препядствия не происходит никаких процессов
        /// </summary>
        /// <param name="homeCoords"></param>
        public override void Process(Vector2f homeCoords)
        {
            ;
        }

        /// <summary>
        /// Сконструировать сигнатуру препядствия
        /// </summary>
        /// <returns></returns>
        protected override ObjectSignature ConstructSignature()
        {
            ObjectSignature signature = new ObjectSignature();
            signature.AddCharacteristics(this.mass);
            signature.AddCharacteristics(this.size);
            return signature;;
        }
    }
}
