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
    /// Локальный цента масс
    /// </summary>
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
        /// орбитальная скорость объекта в рад./ед.вр.
        /// </summary>
        private double orbitalSpeed;

        /// <summary>
        /// Планеты контролируемые данным центром масс
        /// </summary>
        private Planet[] planets;

        /// <summary>
        /// Звезды контролируемые данным центорм масс
        /// </summary>
        Star[] stars;

        /// <summary>
        /// Локальные центры масс контролируемые данным центром масс
        /// </summary>
        LocalMassCenter[] massCenters;

        /// <summary>
        /// Создание локального центра масс, управляющего звездами и планетами
        /// </summary>
        /// <param name="orbit">Расстояние от центра звездной системы</param>
        /// <param name="startOrbitalAngle">Начальный орбитальный угол</param>
        /// <param name="orbitalSpeed">Орбитальная угловая скорость в рад./ед.вр.</param>
        /// <param name="stars">Массив звезд, управляемых данным центром масс</param>
        /// <param name="planets">Массив планет, управляемых данным центром масс</param>
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
        /// <param name="orbit">Расстояние от центра звездной системы</param>
        /// <param name="startOrbitalAngle">Начальный орбитальный угол в рад.</param>
        /// <param name="orbitalSpeed">Орбитальная угловая скорость в рад. / ед. вр.</param>
        /// <param name="locMasCenters">Центры масс, управляемые данным</param>
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
        protected override void Move()
        {
            this.orbitalAngle += orbitalSpeed;//Изменение орбитального угла планеты
            this.coords.X = (float)((orbit * Math.Cos(orbitalAngle)));//вычисление новой кординаты X
            this.coords.Y = (float)((orbit * Math.Sin(orbitalAngle)));//вычисление новой координаты У
        }

        /// <summary>
        /// Управление подчиненными сущностями данного центара масс
        /// </summary>
        /// <param name="companents">Массив подчиненных сущностей</param>
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
        /// Процесс жизни локального центра масс
        /// </summary>
        /// <param name="homeCoords">Координаты управляющей сущности</param>
        public override void Process(Vector2f homeCoords)
        {
            this.ProcessCompanent(massCenters);//управление подчиненными центрами масс
            this.ProcessCompanent(stars);//управление подчиненными звездами
            this.ProcessCompanent(planets);//управление подчиненными планетами
            this.Move();//вычислить идеальные координтаы
        //    this.CorrectObjectPoint(homeCoords);//выполнить коррекцию относительно глобальных координт
        }

        /// <summary>
        /// Получить массив отображений компанента данного центра масс
        /// </summary>
        /// <param name="companetn">Компанент, чьи отображения нужно получить</param>
        /// <returns>Коллекция отображений</returns>
        private List<ImageView> GetCompanentViews(GameObject[] companetn)
        {
            List<ImageView> retViews = new List<ImageView>();//массив возвращаемых отображений
            if (companetn != null)//если центр масс имеет данные комапненты, передать их отображения в возвращаемый массив
            {
                for (int i = 0; i < companetn.Length; i ++)
                {
                    foreach (ImageView view in companetn[i].View)//получить все отображения данного компанента центра масс
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
        /// <returns>Коллекция отображений</returns>
        public List<RenderView> GetView()
        {
            List<RenderView> retViews = new List<RenderView>();//массив возвращаемых отображений
            if (this.massCenters != null)//если центр масс имеет подчиненными центры масс, извлечь из них отображения звезд
            {
                for (int i = 0; i < this.massCenters.Length; i++)
                {//получить все отображения звезд в подчиненном центре масс
                    foreach (ImageView singleView in this.massCenters[i].GetView())
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
        /// Получить массив управляемых игровых объектов (временная реализация)
        /// </summary>
        /// <returns>Коллекция игровых объектов</returns>
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
