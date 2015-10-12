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
        static public Texture[] shipTextures = new Texture[4];
        static public Texture[] circuleButtonTextures = new Texture[4];
        static public Texture[] rectangleButtonTextures = new Texture[4];
        static public Texture energyBar = new Texture("energyBar.png");
        static public Texture healthBar = new Texture("healthBar.png");
        static public Texture protectBar = new Texture("protectBar.png");
        static private Texture backText = new Texture("testBackground.png"); //фон
        static private Texture planetText = new Texture("testPlanetText.jpg"); //загруженная текстура планет 
        static private Texture starText = new Texture("testStarText.jpg"); //загруженная текстура звезды
        static private Texture shadowTexture = new Texture("shadow.png");//тень
        static private Texture crownText = new Texture("crown.png");//звездная корона
        static public Texture PanelText = new Texture("Panel.png");//звездная корона
        static private int[] orbits = { 600, 1000, 1700 }; //орибиты планет 

        /// <summary>
        /// Статический конструктор
        /// </summary>
        static ResurceStorage()
        {
            shipTextures[0] = new Texture("textPlayer0.png");
            shipTextures[1] = new Texture("textPlayer3.png");
            shipTextures[2] = new Texture("textPlayer2.png");
            shipTextures[3] = new Texture("textPlayer1.png");
            circuleButtonTextures[0] = new Texture("Norm.png");
            circuleButtonTextures[1] = new Texture("Act.png");
            circuleButtonTextures[2] = new Texture("Click.png");
            circuleButtonTextures[3] = new Texture("Click2.png");
            rectangleButtonTextures[0] = new Texture("Norm1.png");
            rectangleButtonTextures[1] = new Texture("Act1.png");
            rectangleButtonTextures[2] = new Texture("Click1.png");
            rectangleButtonTextures[3] = new Texture("Click21.png");
            
        }

        /// <summary>
        /// инициализацимя звездной системы
        /// </summary>
        /// <returns></returns>
        static public StarSystem initSystem()
        {
            Texture[] texts = new Texture[2];
            texts[0] = starText;
            texts[1] = crownText;
            Star[] stars = new Star[1];
            stars[0] = new Star(10000000, 250, 0, 0, 0, texts);
            texts[0] = planetText;
            texts[1] = shadowTexture;
            Planet[] planets = new Planet[3];
            for (int i = 0; i < planets.Length; i++)
            {

                planets[i] = new Planet(10, 50, orbits[i], 0.002 / (1 + i), texts);
            }
            LocalMassCenter center = new LocalMassCenter(0, 0, 0, stars, planets);
            return new StarSystem(center, backText);
        }

    }
}
