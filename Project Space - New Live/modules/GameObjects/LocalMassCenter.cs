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

        protected int orbit;//орбита(расстояние от центра масс звездной системы до локального центра масс)
        protected double orbitalAngle;//орбитальный угол объекта
        protected double orbitalSpeed;//орбитальная скорость объекта (рад./ед.вр.)
        
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
            this.move(0);
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
            this.move(0);
        }

        /// <summary>
        /// Движение локального центра масс
        /// </summary>
        /// <param name="speed">Скорость</param>
        protected override void move(double speed)
        {
            orbitalAngle += speed;//Изменение орбитального угла планеты
            this.coords.X = (float)((orbit * Math.Cos(orbitalAngle)));//вычисление новой кординаты X
            this.coords.Y = (float)((orbit * Math.Sin(orbitalAngle)));//вычисление новой координаты У
        }

        //Вероятно объединение 2 следующих методов

        /// <summary>
        /// Управление звездами
        /// </summary>
        private void starProcess()
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].process(this);
            }
        }

        /// <summary>
        /// Управление подчиненными данного центара масс
        /// </summary>
        private void processCompanent(GameEntity[] companents)
        {
            if (companents != null)
            {
                foreach (GameEntity entity in companents)
                {
                    entity.process(this);
                }
            }
        }

        /// <summary>
        /// Функция "жизни" локального центра масс
        /// </summary>
        /// <param name="home"></param>
        public override void process(GameEntity home)
        {
            processCompanent(massCenters);//управление подчиненными центрами масс
            processCompanent(stars);//управление подчиненными звездами
            this.move(orbitalSpeed);//вычислить идеальные координтаы
            this.correctObjectPoint(home.getCoords());//выполнить коррекцию относительно глобальных координт
        }

        /// <summary>
        /// Получить все отображения данного центра масс
        /// </summary>
        /// <returns></returns>
        public List<SFML.Graphics.Shape> getView()
        {
            List<SFML.Graphics.Shape> retViews = new List<SFML.Graphics.Shape>();//массив возвращаемых отображений
            if (stars != null)//если центр масс имеет звезды, передать их отображения в возвращаемый массив
            {
                for (int i = 0; i < stars.Length; i++)
                {
                    retViews.Add(stars[i].getView());//получить все звездные отображения данного центра масс
                }
            }
            if (massCenters != null)//если центр масс имеет подчиненными центры масс, извлечь из них отображения звезд
            {
                for (int i = 0; i < massCenters.Length; i++)
                {//получить все отображения звезд в подчиненном центре масс
                    foreach (SFML.Graphics.Shape singleView in massCenters[i].getView())
                    {//перевести полученные отображения в возвращаемый массив
                        retViews.Add(singleView);
                    }
                }
            }
            return retViews;//вернуть все звездные отображения
        }


    }
}
