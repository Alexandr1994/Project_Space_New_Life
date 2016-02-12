using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

        static public Texture[] shipTextures = new Texture[5];
        static public Texture[] circuleButtonTextures = new Texture[4];
        static public Texture[] rectangleButtonTextures = new Texture[4];
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
        static private int[] orbits = { 600, 1000, 1700 }; //орибиты планет 

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static ResurceStorage()
        {
            string lol = Directory.GetCurrentDirectory();
            shipTextures[0] = new Texture("Resources/textPlayer0.png");
            shipTextures[1] = new Texture("Resources/textPlayer3.png");
            shipTextures[2] = new Texture("Resources/textPlayer2.png");
            shipTextures[3] = new Texture("Resources/textPlayer1.png");
            shipTextures[4] = new Texture("Resources/Shield.png");
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
        static public StarSystem InitSystem1()
        {
            Texture[] texts = new Texture[2];
            texts[0] = starText;
            texts[1] = crownText;
            Star[] stars = new Star[1];
            stars[0] = new Star(10000000, 250, 0, 0, 0, texts);
            texts[0] = planetText;
            texts[1] = shadowTexture;
            Planet[] planets = new Planet[3];
            for (int i = 0; i < planets.Length; i ++)
            {
                planets[i] = new Planet(10, 50, orbits[i], 0.002 / (1 + i), texts);
            }
            LocalMassCenter center = new LocalMassCenter(0, 0, 0, stars, planets);
            return new StarSystem(center, backText);
        }

        static public StarSystem InitSystem2()
        {
            Texture[] texts = new Texture[2];
            texts[0] = starText;
            texts[1] = crownText;
            Star[] stars = new Star[2];
            stars[0] = new Star(10000000, 250, 750, Math.PI, (float)(0.1 * Math.PI / 180), texts);
            stars[1] = new Star(10000000, 250, 750, 0, (float)(0.1 * Math.PI / 180), texts);
            texts[0] = planetText;
            texts[1] = shadowTexture;
            Planet[] planets = new Planet[3];
            for (int i = 0; i < planets.Length; i++)
            {

                planets[i] = new Planet(10, 50, orbits[i]*2, 0.002 / (1 + i), texts);
            }
            LocalMassCenter center = new LocalMassCenter(0, 0, 0, stars, planets);
            return new StarSystem(center, backText);
        }


    }
}
