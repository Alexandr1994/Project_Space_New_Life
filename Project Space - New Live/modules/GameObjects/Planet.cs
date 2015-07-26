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
    public class Planet : SphereObject
    {

        /// <summary>
        /// Перечисление отображений
        /// </summary>
        private enum Views : int
        {
            Planet = 0,//планеты
            Shadow//тени на планете
        }


        /// <summary>
        /// Сконструировать новую необитаемую планету
        /// </summary>
        /// <param name="mass">Масса</param>
        /// <param name="radius">Радиус</param>
        /// <param name="orbit">Орбита</param>
        /// <param name="orbitalSpeed">Орбитальная скорость</param>
        /// <param name="Skin">Текструа</param>
        public Planet(float mass, int radius, int orbit, double orbitalSpeed, SFML.Graphics.Texture[] Skin)
        {
            Random random = new Random();
            this.mass = mass;//инициализировать основные характеристики планеты
            this.radius = radius;
            this.orbit = orbit;
            this.orbitalSpeed = orbitalSpeed;
            this.orbitalAngle = random.Next()%(2*Math.PI);//задать случайный пворот планеты
            this.Move();//сформировать координаты планеты
            this.ConstructView(Skin);//сконструировать отображение планеты
            this.view[(int)Views.Shadow].Rotate(this.coords, (float)this.orbitalAngle);
        }

        /// <summary>
        /// Сконструировать отображение планеты
        /// </summary>
        /// <param name="skin">Текстура</param>
        /// <returns></returns>
        protected override void ConstructView(Texture[] skin)
        {
            this.view = new ObjectView[2];
            BlendMode[] modes = new[] {BlendMode.Alpha, BlendMode.Multiply};
            for (int i = 0; i < this.view.Length; i++)
            {
                this.view[i] = new ObjectView(new CircleShape((float)radius), modes[i]);//создание нового ObjectView
                this.view[i].Image.Position = coords - new Vector2f(radius, radius);//установка позиции отображегния ObjectView
                this.view[i].Image.Texture = skin[i];//установка текстуры отображения ObjectMode
            }
         //   
        }



        /// <summary>
        /// Жизнь объекта
        /// </summary>
        /// <param name="homeCoords">Коордтнаты управляющей сущности</param>
        public override void Process(Vector2f homeCoords)
        {
            this.View[(int)Views.Shadow].Rotate(this.coords, (float)this.orbitalSpeed);
            this.OrbitalMoving(homeCoords);//движение планеты по заданой орбите 
        }


        /// <summary>
        /// Функция перемещения объекта по орбите
        /// </summary>
        /// <param name="homeCoords">Координаты управляющей сущности</param>
        protected override void OrbitalMoving(Vector2f homeCoords)
        {
            Vector2f offsets = this.coords;
            this.Move();//вычислить идеальные координтаы
            this.CorrectObjectPoint(homeCoords);//выполнить коррекцию относительно глобальных координт
            offsets = this.coords - offsets;//вычислить смещения
            foreach (ObjectView locView in View)
            {
                locView.Translate(offsets);
           //     locView.Image.Position = new Vector2f(coords.X - this.radius, coords.Y - this.radius);//вычислить координаты отображений объект
            }
        }
        




        /// <summary>
        /// Построение сигнатуры планеты
        /// </summary>
        /// <returns></returns>
        protected override ObjectSignature ConstructSignature()
        {
            return null;
        }
    }
}
