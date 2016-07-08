using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.Dispatchers;
using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Project_Space___New_Live.modules
{
    /// <summary>
    /// Игровой контроллер непосредственного управления объектом 
    /// </summary>
    public class PlayerController : AbstractController
    {
        /// <summary>
        /// Ссылка на экземпляр класса-отрисовщика 
        /// </summary>
        private RenderModule GameRenderer;

        /// <summary>
        /// Экземпляр класса-игрового контроллера
        /// </summary>
        static private PlayerController GameController;

        /// <summary>
        /// Объект, сохранающий статистические данные
        /// </summary>
        private DataSaver dataSaver;

        /// <summary>
        /// Таймер сохранения статистики
        /// </summary>
        private Clock dataSaverClock;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="playerObject">Корабль игрока</param>
        private PlayerController()
        {
            this.dataSaverClock = new Clock();//инициализация таймера записи
            this.dataSaver = new DataSaver("human_data");//инициализация файла для сохранения данных
            this.GameRenderer = RenderModule.getInstance();//Получение класса отрисовщика
            this.GameRenderer.MainWindow.KeyPressed += this.OnKey;
            this.GameRenderer.MainWindow.KeyReleased += this.FromKey;
            this.GameRenderer.Form.MouseDown += this.OnButton;
            this.GameRenderer.Form.MouseUp += this.FromButton;
            this.GameRenderer.Form.MouseOut += this.OnMouseOut;
        }

        /// <summary>
        /// Получение экзепляра класса игрового контроллера
        /// </summary>
        /// <returns>Ссылка на экземпляр ручного контроллера</returns>
        public static PlayerController GetInstanse()
        {
            if (GameController == null)
            {
                GameController = new PlayerController();
            }
            return GameController;
        }

        /// <summary>
        /// Переустановка Контейнера Игрока
        /// </summary>
        /// <param name="newContainer"></param>
        public void SetNewController(ObjectContainer newContainer)
        {
            this.ObjectContainer = newContainer;
        }

        /// <summary>
        /// Обрабочик нажатий на клавиши
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="Args">Аргументы</param>
        private void OnKey(object sender, KeyEventArgs Args)
        {
            switch (Args.Code)
            {//(Модификация в скором времени)
                case Keyboard.Key.W:
                {
                    this.Forward = true;
                }; break;
                case Keyboard.Key.S:
                {
                    this.Reverse = true;
                }; break;
                case Keyboard.Key.A :
                {
                    this.LeftRotate = true;
                }; break;
                case Keyboard.Key.D:
                {
                    this.RightRotate = true;
                }; break;
                case Keyboard.Key.Z:
                {
                    this.LeftFly = true;
                }; break;
                case Keyboard.Key.C:
                {
                    this.RightFly = true;
                }; break;
                case  Keyboard.Key.X:
                {
                    this.StopMoving = true;
                }; break;
                case Keyboard.Key.Num1:
                {
                    this.ObjectContainer.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(0);
                }; break;
                case Keyboard.Key.Num2:
                {
                    this.ObjectContainer.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(1);
                }; break;
                case Keyboard.Key.Num3:
                {
                    this.ObjectContainer.ControllingObject.ObjectWeaponSystem.SetActiveWeaponIndex(2);
                }; break;
                case Keyboard.Key.LControl:
                {
                    this.ShieldProcess();
                }; break;
                default: break;
            }
        }

        /// <summary>
        /// Обработчик отжатий клавиши
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="Args">Аргументы</param>
        private void FromKey(object sender, KeyEventArgs Args)
        {
            switch (Args.Code)
            {//(Модификация в скором времени)
                case Keyboard.Key.W:
                {
                    this.Forward = false;
                }; break;
                case Keyboard.Key.S:
                {
                    this.Reverse = false;
                }; break;
                case Keyboard.Key.A:
                {
                    this.LeftRotate = false;
                }; break;
                case Keyboard.Key.D:
                {
                    this.RightRotate = false;
                }; break;
                case Keyboard.Key.Z:
                {
                    this.LeftFly = false;
                }; break;
                case Keyboard.Key.C:
                {
                    this.RightFly = false;
                }; break;
                case Keyboard.Key.X:
                {
                    this.StopMoving = false;
                }; break;
                default: break;
            }
        }

        /// <summary>
        /// Обработка нажатий на кнопки мыши
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="Args">Аргументы</param>
        private void OnButton(object sender, MouseButtonEventArgs Args)
        {
            switch (Args.Button)
            {
                case Mouse.Button.Left:
                {
                    this.ObjectContainer.ControllingObject.OpenFire();
                }; break;
                default: break;
            }
        }

        /// <summary>
        /// Обработка отжатий кнопки мыши
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="Args">Аргументы</param>
        private void FromButton(object sender, MouseButtonEventArgs Args)
        {
            switch (Args.Button)
            {
                case Mouse.Button.Left:
                {
                    this.ObjectContainer.ControllingObject.StopFire();
                }; break;
                default: break;
            }
        }

        /// <summary>
        /// Отработка выхода курсора из области главной формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Args"></param>
        private void OnMouseOut(object sender, MouseMoveEventArgs Args)
        {
            this.ObjectContainer.ControllingObject.StopFire();
        }

        /// <summary>
        /// Процесс работы контроллера
        /// </summary>
        public override void Process(List<ObjectSignature> signaturesCollection)
        {
            this.TimerCheck();
            this.Moving();
        }

        /// <summary>
        /// Работа с энергощитом
        /// </summary>
        private void ShieldProcess()
        {
            if (this.ObjectContainer.ControllingObject.ShieldActive)//Если энергощит активен
            {
                this.ObjectContainer.ControllingObject.DeactivateShield();//деактивировать его
            }
            else
            {
                this.ObjectContainer.ControllingObject.ActivateShield();//иначе активировать
            }
        }

        /// <summary>
        /// Проверка таймера сохранения данных
        /// </summary>
        private void TimerCheck()
        {
            if (this.dataSaverClock.ElapsedTime.AsSeconds() > 60)//если прошла минута с прошлой записи статистических данных
            {
                this.dataSaver.WriteData(this.ObjectContainer.WinCount, this.ObjectContainer.DeathCount, 0);//сделать новую запись
                this.dataSaverClock.Restart();//перезапуск таймера
            }
        }

    }
}
