using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Project_Space___New_Live.modules.GameObjects
{
    public class LocalMassCenter : GameEntity
    {
        /// <summary>
        /// орбита(расстояние от центра масс звездной системы до локального центра масс)
        /// </summary>
        private int orbit;
        /// <summary>
        /// орбитальный угол объекта
        /// </summary>
        private double orbitalAngle;
        /// <summary>
        /// орбитальная скорость объекта (рад./ед.вр.)
        /// </summary>
        private double orbitalSpeed;
        
        Star[] stars = null;//звезды контролируемые центорм масс
        LocalMassCenter[] massCenters = null;//локальные центры масс контролируемые данным центром масс

        /// <summary>
        /// Создание локального центра масс, управляющего звездами
        /// </summary>
        /// <param name="orbit">расстояние от центра звездной системы</param>
        /// <param name="startOrbitalAngle">//начальный орбитальный угол</param>
        /// <param name="orbitalSpeed">орбитальная угловая скорость</param>
        /// <param name="stars">Звезды, управляемые центром масс</param>
        public LocalMassCenter(int orbit, double startOrbitalAngle, double orbitalSpeed, Star[] stars)
        {
            this.orbit = orbit;
            this.orbitalAngle = startOrbitalAngle;
            this.orbitalSpeed = orbitalSpeed;
            this.stars = stars;
            this.Move();
        }

        /// <summary>
        /// Создание локального центра масс, управляющего другими локальными центрами масс
        /// </summary>
        /// <param name="orbit">расстояние от центра звездной системы</param>
        /// <param name="startOrbitalAngle">//начальный орбитальный угол</param>
        /// <param name="orbitalSpeed">орбитальная угловая скорость</param>
        /// <param name="locMasCenters">Центры масс, кправляемые данным центром масс</param>
        public LocalMassCenter(int orbit, double startOrbitalAngle, double orbitalSpeed, LocalMassCenter[] locMasCenters)
        {
            this.orbit = orbit;
            this.orbitalAngle = startOrbitalAngle;
            this.orbitalSpeed = orbitalSpeed;
            this.massCenters = locMasCenters;
            this.Move();
        }

        /// <summary>
        /// Движение локального центра масс
        /// </summary>
        /// <param name="speed">Скорость</param>
        protected override void Move()
        {
            orbitalAngle += orbitalSpeed;//Изменение орбитального угла планеты
            this.coords.X = (float)((orbit * Math.Cos(orbitalAngle)));//вычисление новой кординаты X
            this.coords.Y = (float)((orbit * Math.Sin(orbitalAngle)));//вычисление новой координаты У
        }

        //Вероятно объединение 2 следующих методов

        /// <summary>
        /// Управление звездами
        /// </summary>
        private void StarProcess()
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].Process(this.GetCoords());
            }
        }

        /// <summary>
        /// Управление подчиненными данного центара масс
        /// </summary>
        private void ProcessCompanent(GameEntity[] companents)
        {
            if (companents != null)
            {
                foreach (GameEntity entity in companents)
                {
                    entity.Process(this.GetCoords());
                }
            }
        }

        /// <summary>
        /// Функция "жизни" локального центра масс
        /// </summary>
        /// <param name="homeCoords">Коордтнаты управляющей сущности</param>
        public override void Process(Vector2f homeCoords)
        {
            ProcessCompanent(massCenters);//управление подчиненными центрами масс
            ProcessCompanent(stars);//управление подчиненными звездами
            this.Move();//вычислить идеальные координтаы
            this.CorrectObjectPoint(homeCoords);//выполнить коррекцию относительно глобальных координт
        }

        /// <summary>
        /// Получить все отображения данного центра масс
        /// </summary>
        /// <returns></returns>
        public List<SFML.Graphics.Shape> GetView()
        {
            List<SFML.Graphics.Shape> retViews = new List<SFML.Graphics.Shape>();//массив возвращаемых отображений
            if (stars != null)//если центр масс имеет звезды, передать их отображения в возвращаемый массив
            {
                for (int i = 0; i < stars.Length; i++)
                {
                    retViews.Add(stars[i].GetView());//получить все звездные отображения данного центра масс
                }
            }
            if (massCenters != null)//если центр масс имеет подчиненными центры масс, извлечь из них отображения звезд
            {
                for (int i = 0; i < massCenters.Length; i++)
                {//получить все отображения звезд в подчиненном центре масс
                    foreach (SFML.Graphics.Shape singleView in massCenters[i].GetView())
                    {//перевести полученные отображения в возвращаемый массив
                        retViews.Add(singleView);
                    }
                }
            }
            return retViews;//вернуть все звездные отображения
        }
    }
}
