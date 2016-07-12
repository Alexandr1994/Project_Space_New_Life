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
    /// Планета
    /// </summary>
    public class Planet : SphereObject
    {

        /// <summary>
        /// Перечисление отображений
        /// </summary>
        public enum Views : int
        {
            /// <summary>
            /// Планета
            /// </summary>
            Planet = 0,
            /// <summary>
            /// Тень на планете
            /// </summary>
            Shadow
        }


        /// <summary>
        /// Сконструировать новую планету
        /// </summary>
        /// <param name="mass">Масса</param>
        /// <param name="radius">Радиус</param>
        /// <param name="orbit">Орбита</param>
        /// <param name="orbitalSpeed">Орбитальная скорость в рад.</param>
        /// <param name="Skin">Текструа</param>
        public Planet(float mass, int radius, int orbit, double orbitalSpeed, SFML.Graphics.Texture[] Skin)
        {
            Random random = new Random();
            this.mass = mass;//инициализировать основные характеристики планеты
            this.radius = radius;
            this.orbit = orbit;
            this.orbitalSpeed = orbitalSpeed;
            this.orbitalAngle = random.Next() % (2 * Math.PI);//задать случайный пворот планеты
            this.Move();//сформировать координаты планеты
            this.ConstructView(Skin);//сконструировать отображение планеты
            this.view[(int)Views.Shadow].Rotate(this.coords, (float)this.orbitalAngle);
        }

        /// <summary>
        /// Сконструировать отображение планеты
        /// </summary>
        /// <param name="skin">Массив текстур</param>
        protected override void ConstructView(Texture[] skin)
        {
            this.view = new ImageView[2];
            BlendMode[] modes = new[] {BlendMode.Alpha, BlendMode.Multiply};
            for (int i = 0; i < this.view.Length; i ++)
            {
                this.view[i] = new ImageView(new CircleShape((float)radius), modes[i]);//создание нового ObjectView
                this.view[i].Image.Position = coords - new Vector2f(radius, radius);//установка позиции отображегния ObjectView
                this.view[i].Image.Texture = skin[i];//установка текстуры отображения ObjectMode
            }
        }

        /// <summary>
        /// Процесс жизни планеты
        /// </summary>
        /// <param name="homeCoords">Координаты управляющей сущности</param>
        public override void Process(Vector2f homeCoords)
        {
            this.View[(int)Views.Shadow].Rotate(this.coords, (float)this.orbitalSpeed);
            this.OrbitalMoving(homeCoords);//движение планеты по заданой орбите 
        }


        /// <summary>
        /// Функция движения планеты по орбите
        /// </summary>
        /// <param name="homeCoords">Координаты управляющей сущности</param>
        protected override void OrbitalMoving(Vector2f homeCoords)
        {
            Vector2f offsets = this.coords;
            this.Move();//вычислить идеальные координтаы
           // this.CorrectObjectPoint(homeCoords);//выполнить коррекцию относительно глобальных координт
            offsets = this.coords - offsets;//вычислить смещения
            foreach (ImageView locView in View)
            {
                locView.Translate(offsets);
            }
        }
        
        /// <summary>
        /// Построение сигнатуры планеты
        /// </summary>
        /// <returns>Сигнатура планеты</returns>
        protected override ObjectSignature ConstructSignature()
        {
            ObjectSignature signature = new ObjectSignature();
        //    signature.AddCharacteristics(this.mass);
            Vector2f sizes = new Vector2f(this.radius * 2, this.radius * 2);
       //     signature.AddCharacteristics(sizes);
            return signature;
        }
    }
}
