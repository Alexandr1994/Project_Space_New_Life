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

        private Planet[] planets;//Планеты контролируемые центром масс
        Star[] stars;//звезды контролируемые центорм масс
        LocalMassCenter[] massCenters;//локальные центры масс контролируемые данным центром масс

        /// <summary>
        /// Создание локального центра масс, управляющего звездами
        /// </summary>
        /// <param name="orbit">расстояние от центра звездной системы</param>
        /// <param name="startOrbitalAngle">//начальный орбитальный угол</param>
        /// <param name="orbitalSpeed">орбитальная угловая скорость</param>
        /// <param name="stars">Звезды, управляемые центром масс</param>
        public LocalMassCenter(int orbit, double startOrbitalAngle, double orbitalSpeed, Star[] stars, Planet[] planets)
        {
            this.orbit = orbit;
            this.orbitalAngle = startOrbitalAngle;
            this.orbitalSpeed = orbitalSpeed;
            this.stars = stars;
            this.planets = planets;
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

        /// <summary>
        /// Управление подчиненными данного центара масс
        /// </summary>
        private void ProcessCompanent(GameEntity[] companents)
        {
            if (companents != null)
            {
                foreach (GameEntity entity in companents)
                {
                    entity.Process(this.Coords);
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
            ProcessCompanent(planets);//управление подчиненными планетами
            this.Move();//вычислить идеальные координтаы
            this.CorrectObjectPoint(homeCoords);//выполнить коррекцию относительно глобальных координт
        }

        /// <summary>
        /// Получить массив отображений компанента
        /// </summary>
        /// <param name="companetn"></param>
        /// <returns></returns>
        private List<ObjectView> GetCompanentViews(GameObject[] companetn)
        {
            List<ObjectView> retViews = new List<ObjectView>();//массив возвращаемых отображений
            if (companetn != null)//если центр масс имеет данные комапненты, передать их отображения в возвращаемый массив
            {
                for (int i = 0; i < companetn.Length; i++)
                {
                    foreach (ObjectView view in companetn[i].View)//получить все отображения данного компанента центра масс
                    {
                        retViews.Add(view);
                    }
                }
            }
            return retViews;
        }


        /// <summary>
        /// Получить все отображения данного центра масс
        /// </summary>
        /// <returns></returns>
        public List<ObjectView> GetView()
        {
            List<ObjectView> retViews = new List<ObjectView>();//массив возвращаемых отображений
            if (massCenters != null)//если центр масс имеет подчиненными центры масс, извлечь из них отображения звезд
            {
                for (int i = 0; i < massCenters.Length; i++)
                {//получить все отображения звезд в подчиненном центре масс
                    foreach (ObjectView singleView in massCenters[i].GetView())
                    {//перевести полученные отображения в возвращаемый массив
                        retViews.Add(singleView);
                    }
                }
            }
            retViews.AddRange(this.GetCompanentViews(this.stars));//добавить в массив возращаемых отображений отображения звезд
            retViews.AddRange(this.GetCompanentViews(this.planets));//добавить в массив возвращаемых отображений отображения планет
            return retViews;//вернуть все звездные отображения
        }

        /// <summary>
        /// Получить массив управляемых объектов (временная реализация)
        /// </summary>
        /// <returns></returns>
        public List<GameObject> GetObjects()
        {
            List<GameObject> retValue = new List<GameObject>();
            if (this.stars != null)
            {
                retValue.AddRange(this.stars);//Получить все звезды данного центар масс
            }
            if (this.planets != null)
            {
                retValue.AddRange(this.planets);//Получить все планеты данного центра масс
            }
            if (this.massCenters != null)
            {
                foreach (LocalMassCenter currentCenter in this.massCenters)//получить все объект подчиненных центров масс
                {
                    retValue.AddRange(currentCenter.GetObjects());
                }
            }
           return retValue;//Вернуть все объекты в данном и подчиненных центрах масс
        }

    }
}
