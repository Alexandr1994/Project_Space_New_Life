using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Project_Space___New_Live.modules.Storages
{
    /// <summary>
    /// Хранилище изображений
    /// </summary>
    public static class ImageStorage
    {

        //фон

        static private Texture background = new Texture("Resources/Images/background.png");

        public static Texture Background
        {
            get { return background; }
        }

        //ЛЕНТЫ ВИЗУАЛЬНЫХ ЭФФЕКТОВ

        static private Texture hitting = new Texture("Resources/Images/Hitting.png");

        static private Texture explosion_1 = new Texture("Resources/Images/explosion_1.png");

        static private Texture explosion_2 = new Texture("Resources/Images/explosion_2.png");

        static private Texture noise = new Texture("Resources/Images/Noize.gif");

        public static Texture Hitting
        {
            get { return hitting; }
        }

        public static Texture Explosion1
        {
            get { return explosion_1; }
        }

        public static Texture Explosion2
        {
            get { return explosion_2; }
        }

        public static Texture Noise
        {
            get { return noise; }
        }

        //ТЕКСТУРЫ ОБЪЕКТА

        public static Texture[] BlueObject
        {
            get
            {
                Texture[] textures = new Texture[8];
                textures[0] = bluePointer;
                textures[2] = new Texture("Resources/Images/blue_ship_front.png");
                textures[3] = new Texture("Resources/Images/blue_ship_back.png");
                textures[4] = new Texture("Resources/Images/blue_ship_left.png");
                textures[5] = new Texture("Resources/Images/blue_ship_right.png");
                textures[6] = new Texture("Resources/Images/blue_shield.png");
                textures[7] = new Texture("Resources/Images/explosion_1.png");
                return textures;
            }
        }

        public static Texture[] RedObject
        {
            get
            {
                Texture[] textures = new Texture[8];
                textures[0] = RedPointer;
                textures[2] = new Texture("Resources/Images/red_ship_front.png");
                textures[3] = new Texture("Resources/Images/red_ship_back.png");
                textures[4] = new Texture("Resources/Images/red_ship_left.png");
                textures[5] = new Texture("Resources/Images/red_ship_right.png");
                textures[6] = new Texture("Resources/Images/red_shield.png");
                textures[7] = new Texture("Resources/Images/explosion_1.png");
                return textures;
            }
        }

        public static Texture[] GreenObject
        {
            get
            {
                Texture[] textures = new Texture[8];
                textures[0] = GreenPointer;
                textures[2] = new Texture("Resources/Images/green_ship_front.png");
                textures[3] = new Texture("Resources/Images/green_ship_back.png");
                textures[4] = new Texture("Resources/Images/green_ship_left.png");
                textures[5] = new Texture("Resources/Images/green_ship_right.png");
                textures[6] = new Texture("Resources/Images/green_shield.png");
                textures[7] = new Texture("Resources/Images/explosion_1.png");
                return textures;
            }
        }

        public static Texture[] YellowObject
        {
            get
            {
                Texture[] textures = new Texture[8];
                textures[0] = YellowPointer;
                textures[2] = new Texture("Resources/Images/yellow_ship_front.png");
                textures[3] = new Texture("Resources/Images/yellow_ship_back.png");
                textures[4] = new Texture("Resources/Images/yellow_ship_left.png");
                textures[5] = new Texture("Resources/Images/yellow_ship_right.png");
                textures[6] = new Texture("Resources/Images/yellow_shield.png");
                textures[7] = new Texture("Resources/Images/explosion_1.png");
                return textures;
            }
        }

        //ТЕКСТУРЫ ЦЕЛЕУКАЗАТЕЛЕЙ

        static private Texture bluePointer = new Texture("Resources/Images/bluePointer.png");

        static private Texture redPointer = new Texture("Resources/Images/redPointer.png");

        static private Texture greenPointer = new Texture("Resources/Images/greenPointer.png");

        static private Texture yellowPointer = new Texture("Resources/Images/yellowPointer.png");

        public static Texture YellowPointer
        {
            get { return yellowPointer; }
        }

        public static Texture GreenPointer
        {
            get { return greenPointer; }
        }

        public static Texture RedPointer
        {
            get { return redPointer; }
        }

        public static Texture BluePointer
        {
            get { return bluePointer; }
        }

        //ТЕКСТУРЫ ИНДИКАТОРНЫХ ЛИНИЙ

        static private Texture blue_bar = new Texture("Resources/Images/blue_bar.png");

        static private Texture green_yellow_bar = new Texture("Resources/Images/green_yellow_bar.png");

        static private Texture red_white_bar = new Texture("Resources/Images/red_white_bar.png");

        static private Texture red_yellow_bar = new Texture("Resources/Images/red_yellow_bar.png");

        public static Texture RedYellowBar
        {
            get { return red_yellow_bar; }
        }

        public static Texture RedWhiteBar
        {
            get { return red_white_bar; }
        }

        public static Texture GreenYellowBar
        {
            get { return green_yellow_bar; }
        }

        public static Texture BlueBar
        {
            get { return blue_bar; }
        }

        //ТЕКСТУРЫ КОНТРОЛЬНЫХ ТОЧЕК

        static private Texture blueCheckPoint = new Texture("Resources/Images/BlueCheckPoint.png");

        static private Texture redCheckPoint = new Texture("Resources/Images/RedCheckPoint.png");

        static private Texture greenCheckPoint = new Texture("Resources/Images/GreenCheckPoint.png");

        static private Texture yellowCheckPoint = new Texture("Resources/Images/YellowCheckPoint.png");

        public static Texture YellowCheckPoint
        {
            get { return yellowCheckPoint; }
        }

        public static Texture GreenCheckPoint
        {
            get { return greenCheckPoint; }
        }

        public static Texture RedCheckPoint
        {
            get { return redCheckPoint; }
        }

        public static Texture BlueCheckPoint
        {
            get { return blueCheckPoint; }
        }
    }
}
