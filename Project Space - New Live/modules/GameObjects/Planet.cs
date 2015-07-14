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
        public Planet(int mass, int radius, int orbit, double orbitalSpeed, SFML.Graphics.Texture[] Skin)
        {
            Random random = new Random();
            this.mass = mass;//инициализировать основные характеристики планеты
            this.radius = radius;
            this.orbit = orbit;
            this.orbitalSpeed = orbitalSpeed;
            this.orbitalAngle = random.Next();//задать случайный пворот планеты
            this.Move();//сформировать координаты планеты
            this.ConstructView(Skin);//сконструировать отображение планеты
            this.ShadowViewControler((float) ((180/Math.PI)*this.orbitalAngle), coords);
        }

        /// <summary>
        /// Сконструировать отображение планеты
        /// </summary>
        /// <param name="skin">Текстура</param>
        /// <returns></returns>
        protected override void ConstructView(Texture[] skin)
        {
            this.view = new ObjectView[2];
            for (int i = 0; i < this.view.Length; i++)
            {
                this.view[i] = new ObjectView(new CircleShape((float)radius), BlendMode.Alpha);//создание нового ObjectView
                this.view[i].Image.Position = coords - new Vector2f(radius, radius);//установка позиции отображегния ObjectView
                this.view[i].Image.Texture = skin[i];//установка текстуры отображения ObjectMode
            }
            ShadowViewControler((float)((180 / Math.PI) * this.orbitalSpeed), this.coords);
        }



        /// <summary>
        /// Жизнь объекта
        /// </summary>
        /// <param name="homeCoords">Коордтнаты управляющей сущности</param>
        public override void Process(Vector2f homeCoords)
        {
            this.OrbitalMoving(homeCoords);//движение планеты по заданой орбите
            this.ShadowViewControler((float) ((180 / Math.PI) * this.orbitalSpeed), this.coords);
        }

       

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaAngle">Угол поворота в градусах</param>
        /// <param name="homePoint">Точка, относительно которой происходит вращение</param>
        private void ShadowViewControler(float deltaAngle, Vector2f homePoint)
        {
            RenderStates tempState = this.View[(int)Views.Shadow].State;//получение ссылки на состоняние отображения
            Transform transformAction = tempState.Transform;//получение копии трансформирующего воздействрия 
            transformAction.Rotate(deltaAngle, homePoint);//произведение тарнсформирующего  воздействия
            tempState.Transform = transformAction;
            this.View[(int)Views.Shadow].State = tempState;
        }

        /// <summary>
        /// Функция перемещения объекта по орбите
        /// </summary>
        /// <param name="homeCoords">Координаты управляющей сущности</param>
        protected override void OrbitalMoving(Vector2f homeCoords)
        {
            this.Move();//вычислить идеальные координтаы
            this.CorrectObjectPoint(homeCoords);//выполнить коррекцию относительно глобальных координт
            this.View[(int)Views.Planet].Image.Position = new Vector2f(coords.X - this.radius, coords.Y - this.radius);//вычислить координаты отображений объект
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
