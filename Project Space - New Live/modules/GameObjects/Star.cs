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
    /// Звезда
    /// </summary>
    public class Star : SphereObject
    {
        /// <summary>
        /// Перечисление отображений
        /// </summary>
        public enum Views:int
        {
            /// <summary>
            /// Звезда
            /// </summary>
            Star,
            /// <summary>
            /// Звездная корона
            /// </summary>
            Crown
        }



        /// <summary>
        /// Сконструировать звезду
        /// </summary>
        /// <param name="mass">Масса</param>
        /// <param name="radius">Радиус</param>
        /// <param name="orbit">Орбита (расстояние от центра масс звездной системы)</param>
        /// <param name="startOrbitalAngle">Угол начального поворота звезды в рад.</param>
        /// <param name="orbitalSpeed">//Скорость вращениея вокруг центра масс в рад/ед.вр.</param>
        /// <param name="Skin">Массив текстур</param>
        public Star(float mass, int radius, int orbit, double startOrbitalAngle, double orbitalSpeed, SFML.Graphics.Texture[] Skin)
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
        /// <param name="skin">Массив текстура</param>
        protected override void ConstructView(Texture[] skin)
        {
            this.view = new ImageView[2];
            for (int i = 0; i < this.view.Length; i ++)
            {
                this.view[i] = new ImageView(new CircleShape((float)(radius * ((0.5 * i) + 1))), BlendMode.Alpha);//создание нового ObjectView
                this.view[i].Image.Position = coords - new Vector2f(radius, radius);//установка позиции отображегния ObjectView
                this.view[i].Image.Texture = skin[i];//установка текстуры отображения ObjectMode
            }
            
        }


        /// <summary>
        /// Построение сигнатуры звезды
        /// </summary>
        /// <returns>Сигнатура звезды</returns>
        protected override ObjectSignature ConstructSignature()
        {
            ObjectSignature signature = new ObjectSignature();
       //     signature.AddCharacteristics(this.mass);
            Vector2f sizes = new Vector2f(this.radius * 2, this.radius * 2);
       //     signature.AddCharacteristics(sizes);
            return signature;
        }


        /// <summary>
        /// Процесс жизни звезды
        /// </summary>
        /// <param name="homeCoords">Коордтнаты управляющей сущности</param>
        public override void Process(Vector2f homeCoords)
        {
            this.OrbitalMoving(homeCoords);
        }

        /// <summary>
        /// Функция движения звезды по орбите
        /// </summary>
        /// <param name="homeCoords">Координаты управляющей сущности</param>
        protected override void OrbitalMoving(Vector2f homeCoords)
        {
            this.Move();//вычислить идеальные координтаы
        //orie    this.CorrectObjectPoint(homeCoords);//выполнить коррекцию относительно глобальных координт
            for(int i = 0; i < view.Length; i ++)
            {
                view[i].Image.Position = new Vector2f((float)(coords.X - this.radius * ((0.5 * i) + 1)), (float)(coords.Y - this.radius * ((0.5 * i) + 1)));//вычислить координаты отображений объекта
            }
        }

    }
}
