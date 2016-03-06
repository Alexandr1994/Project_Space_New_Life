using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Space___New_Live.modules.GameObjects;
using SFML.Graphics;

namespace Project_Space___New_Live.modules.Dispatchers
{
    /// <summary>
    /// Временный склад ресурсов
    /// </summary>
    static class ResurceStorage
    {
        static public Font font = new Font("Resources/times.ttf");
        static public Texture[] shipTextures = new Texture[6];
        static public Texture[] circuleButtonTextures = new Texture[4];
        static public Texture[] rectangleButtonTextures = new Texture[4];
        static public Texture[] TankTextures = new Texture[7];
        static public Texture energyBar = new Texture("Resources/energyBar.png");
        static public Texture healthBar = new Texture("Resources/healthBar.png");
        static public Texture protectBar = new Texture("Resources/protectBar.png");
        static public Texture ammoBar = new Texture("Resources/ammoBar.png");
        static public Texture noise = new Texture("Resources/Noize.gif");
        static private Texture backText = new Texture("Resources/testBackground.png"); //фон
        static private Texture planetText = new Texture("Resources/testPlanetText.jpg"); //загруженная текстура планет 
        static private Texture starText = new Texture("Resources/testStarText.jpg"); //загруженная текстура звезды
        static private Texture shadowTexture = new Texture("Resources/shadow.png");//тень
        static private Texture crownText = new Texture("Resources/crown.png");//звездная корона
        static public Texture PanelText = new Texture("Resources/Panel.png");//звездная корона
        static public Texture shipExplosion = new Texture("Resources/SuperExp.png");//Взрыв корабля
        static public Texture shellHitting = new Texture("Resources/Hitting.png");//Взрыв снаряда
        static public Texture RockTexture = new Texture("Resources/land.png");//текстура камня
        static private int[] orbits = { 600, 1000, 1700 }; //орибиты планет 

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static ResurceStorage()
        {
            shipTextures[0] = new Texture("Resources/textPlayer0.png");
            shipTextures[1] = new Texture("Resources/textPlayer3.png");
            shipTextures[2] = new Texture("Resources/textPlayer2.png");
            shipTextures[3] = new Texture("Resources/textPlayer1.png");
            shipTextures[4] = new Texture("Resources/Shield.png");
            shipTextures[5] = new Texture("Resources/SuperExp.png");
            TankTextures[0] = new Texture("Resources/tank0.png");
            TankTextures[1] = new Texture("Resources/tank1.png");
            TankTextures[2] = new Texture("Resources/tank2.png");
            TankTextures[3] = new Texture("Resources/tank3.png");
            TankTextures[4] = new Texture("Resources/tank4.png");
            TankTextures[5] = new Texture("Resources/Shield.png");
            TankTextures[6] = new Texture("Resources/SuperExp.png");
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
            return new BaseEnvironment(noise, (float)0.5);
        }
    
    }
}
