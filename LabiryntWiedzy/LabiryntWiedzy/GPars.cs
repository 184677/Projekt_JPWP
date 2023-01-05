using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LabiryntWiedzy
{
    class GPars
    {
        public static Image[] bgImages; //Tablic grafik tła
        public static Image menuImage; //Obraz ikony Menu
        public static Image logoImage; //Obraz ikony logo
        public static Image ans1Image; //Obraz ikony wyjscia nr1
        public static Image ans2Image; //Obraz ikony wyjścia nr2
        public static Image[] blocksImages; //Tablica grafik obiektów pierwszego planu
        public static Image[] lvlImages; //Tablica grafik ikon wyboru poziomu
        public static Boolean pause = false; //Zmienna stanu określającam czy jest przerwa w grze
        //public static Boolean levelPause = false; // Zmienna stanu określająca czy wybrano menu
        public static Boolean end = false;
        public static Boolean gStarted = false; //Zmienna stanu określająca czy rozpoczeto gre
        public static Boolean gInformation = false; // Zmienna stanu okreslajaca czy wybrano z menu informacje o grze
        public static Boolean gLevelSel = false; // Zmienna stanu okreslajaca czy wybrano z menu wybor poziomow
        public static Boolean gMenu = false; // Zmienna stanu okreslajaca czy wyswietlic menu
        //public static int 
        //public static long startTime;
        //public static double levelTime;
        public static int noOfObjects = 12; //Zmienna pomocnicza określająca liczę obiektów pierwszego planu

       


        public static void loadInitialImages()
        {
            /* Pamietac o ustawiania obrazkow na dpi rownym 96 
             * wtedy dobrze beda odwzorowane wielkosci!!!
             */
            bgImages = new Image[5];
            bgImages[0]= Image.FromFile("images/bg_1024.jpg");
            bgImages[1] = Image.FromFile("images/bg_1024v2.jpg");
            bgImages[2] = Image.FromFile("images/bg_1024v3.jpg");
            bgImages[3] = Image.FromFile("images/bg_1024v4.jpg");
            bgImages[4] = Image.FromFile("images/bg_1024v5.jpg");

            menuImage = Image.FromFile("images/menu_1024.png");
            logoImage = Image.FromFile("images/logo_1024.png");

            ans1Image = Image.FromFile("images/ans_1.png");
            ans2Image = Image.FromFile("images/ans_2.png");

            blocksImages = new Image[7];
            blocksImages[0] = Image.FromFile("images/block_runner.png");
            blocksImages[1] = Image.FromFile("images/block_obstacle1.png"); // pionowy 75x150
            blocksImages[2] = Image.FromFile("images/block_obstacle2.png"); // pionowy 75x225
            blocksImages[3] = Image.FromFile("images/block_obstacle3.png"); // pionowy 75x300
            blocksImages[4] = Image.FromFile("images/block_obstacle4.png"); // poziomy 150x75
            blocksImages[5] = Image.FromFile("images/block_obstacle5.png"); // poziomy 225x75
            blocksImages[6] = Image.FromFile("images/block_obstacle6.png"); // poziomy 300x75

            lvlImages = new Image[5];
            lvlImages[0] = Image.FromFile("images/lvl_1.png");
            lvlImages[1] = Image.FromFile("images/lvl_2.png");
            lvlImages[2] = Image.FromFile("images/lvl_3.png");
            lvlImages[3] = Image.FromFile("images/lvl_4.png");
            lvlImages[4] = Image.FromFile("images/lvl_5.png");
        }//koniec loadInitialImages()


    }//koniec class GameParameters
}
