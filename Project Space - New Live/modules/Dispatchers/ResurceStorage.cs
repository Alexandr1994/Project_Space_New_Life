using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.GameObjects;
using Project_Space___New_Live.modules.Storages;
using SFML.Graphics;
using SFML.System;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Временный склад ресурсов
    /// </summary>
    static class ResurceStorage
    {
        
        static public Texture[] circuleButtonTextures = new Texture[4];
        static public Texture[] rectangleButtonTextures = new Texture[4];
        static public Texture PanelText = new Texture("Resources/Panel.png");

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static ResurceStorage()
        {
            circuleButtonTextures[0] = new Texture("Resources/Norm.png");
            circuleButtonTextures[1] = new Texture("Resources/Act.png");
            circuleButtonTextures[2] = new Texture("Resources/Click.png");
            circuleButtonTextures[3] = new Texture("Resources/Click2.png");
            rectangleButtonTextures[0] = new Texture("Resources/Norm1.png");
            rectangleButtonTextures[1] = new Texture("Resources/Act1.png");
            rectangleButtonTextures[2] = new Texture("Resources/Click1.png");
            rectangleButtonTextures[3] = new Texture("Resources/Click21.png");  
        }

        /// <summary>
        /// Инициализация тестовой звездной системы 1
        /// </summary>
        /// <returns></returns>
        static public BaseEnvironment InitSystem1()
        {
            Random random = new Random();
            CheckPoint[] checkPoints = new CheckPoint[2];
            checkPoints[0] = new StableCheckPoint(new Vector2f((float)(random.NextDouble() * 1500), (float)(random.NextDouble() * 1500)), new[] { ImageStorage.BlueCheckPoint });
            checkPoints[1] = new StableCheckPoint(new Vector2f((float)(random.NextDouble() * 1500), (float)(random.NextDouble() * 1500)), new[] { ImageStorage.RedCheckPoint });
            return new BaseEnvironment(ImageStorage.Background3, (float)0.5, checkPoints);
        }

        /// <summary>
        /// Инициализация тестовой звездной системы 2
        /// </summary>
        /// <returns></returns>
        static public BaseEnvironment InitSystem2()
        {
            Random random = new Random();
            CheckPoint[] checkPoints = new CheckPoint[2];
            float angle = (float)(random.NextDouble() * 2 * Math.PI);
            checkPoints[0] = new OrbitalCheckPoint(new Vector2f(1000, 1000), random.Next(1000), (float)(angle - random.NextDouble() * Math.PI), (float)(random.NextDouble() * 0.1 * Math.PI / 180), new[] { ImageStorage.GreenCheckPoint });
            angle = (float)(random.NextDouble() * 2 * Math.PI);
            checkPoints[1] = new OrbitalCheckPoint(new Vector2f(-1000, -1000), random.Next(1000), (float)(angle - random.NextDouble() * Math.PI), (float)(random.NextDouble() * 0.1 * Math.PI / 180), new[] { ImageStorage.YellowCheckPoint }); 
            return new BaseEnvironment(ImageStorage.Background3, (float)0.5, checkPoints);
        }

    
    }
}
