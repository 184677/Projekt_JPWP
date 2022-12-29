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
        public static Image logoImage; // Obraz ikony logo
        public static Image ans1Image;
        public static Image ans2Image;
        public static Image[] blocksImages; //Tablica grafik obiektów pierwszego planu
        public static Boolean pause = false; //Zmienna stanu określającam czy jest przerwa w grze
        public static Boolean levelPause = false; // Zmienna stanu określająca czy wybrano menu
        public static Boolean end = false;
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
        }//koniec loadInitialImages()


    }//koniec class GameParameters
}
