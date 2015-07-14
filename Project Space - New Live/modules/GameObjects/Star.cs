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
    public class Star : SphereObject
    {
        /// <summary>
        /// Перечисление индексов
        /// </summary>
        private enum Views
        {
            Star,//изображения звезды
            Crown//изображение короны
        }



        /// <summary>
        /// Сконструировать звезду
        /// </summary>
        /// <param name="mass">Масса</param>
        /// <param name="radius">Рудиус</param>
        /// <param name="orbit">Расстояние от центра масс звездной системы</param>
        /// <param name="startOrbitalAngle">Начальный поворот звезды</param>
        /// <param name="orbitalSpeed">//Скорость варщениея вокруг центра масс (рад/ед.вр.)</param>
        /// <param name="Skin">Текстура</param>
        public Star(int mass, int radius, int orbit, double startOrbitalAngle, double orbitalSpeed, SFML.Graphics.Texture[] Skin)
        {
            Random random = new Random();
            this.mass = mass;//инициализировать основные характеристики звезды
            this.radius = radius;
            this.orbit = orbit;
            this.orbitalAngle = startOrbitalAngle;
            this.orbitalSpeed = orbitalSpeed;
            this.Move();//сформировать координаты звезды
            this.ConstructView(Skin);//построить отображение звезды
        }

        /// <summary>
        /// Сконструировать отображение звезды
        /// </summary>
        /// <param name="skin">Текстура</param>
        /// <returns></returns>
        protected override void ConstructView(Texture[] skin)
        {
            this.view = new ObjectView[2];
            for (int i = 0; i < this.view.Length; i++)
            {
                this.view[i] = new ObjectView(new CircleShape((float)(radius * ((0.5 * i) + 1))), BlendMode.Alpha);//создание нового ObjectView
                this.view[i].Image.Position = coords - new Vector2f(radius, radius);//установка позиции отображегния ObjectView
                this.view[i].Image.Texture = skin[i];//установка текстуры отображения ObjectMode
            }
            
        }


        /// <summary>
        /// Построение сигнатуры звезды
        /// </summary>
        /// <returns></returns>
        protected override ObjectSignature ConstructSignature()
        {
            return null;
        }


        /// <summary>
        /// Жизнь объекта
        /// </summary>
        /// <param name="homeCoords">Коордтнаты управляющей сущности</param>
        public override void Process(Vector2f homeCoords)
        {
            this.OrbitalMoving(homeCoords);
        }

        /// <summary>
        /// Функция перемещения объекта по орбите
        /// </summary>
        /// <param name="homeCoords">Координаты управляющей сущности</param>
        protected override void OrbitalMoving(Vector2f homeCoords)
        {
            this.Move();//вычеслить идеальные координтаы
            this.CorrectObjectPoint(homeCoords);//выполнить коррекцию относительно глобальных координт
            for(int i = 0; i < view.Length; i++)
            {
                view[i].Image.Position = new Vector2f((float)(coords.X - this.radius * ((0.5 * i) + 1)), (float)(coords.Y - this.radius * ((0.5 * i) + 1)));//вычислить координаты отображений объекта
            }
        }

    }
}
