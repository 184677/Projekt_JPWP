using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LabiryntWiedzy
{
    class GPars
    {
        public static Image bgImage; //Obraz tła
        public static Image menuImage; //Obraz ikony Menu
        public static Image logoImage; // Obraz ikony logo
        public static Image ans1Image;
        public static Image ans2Image;
        public static Image[] blocks; //Tablica grafik obiektów pierwszego planu
        public static Boolean pause = false; //Zmienna stanu określającam czy jest przerwa w grze
        public static Boolean levelPause = false; // Zmienna stanu określająca czy wybrano menu
        public static Boolean end = false;
        public static long startTime;
        public static double levelTime;
        public static int noOfObjects = 10; //Zmienna pomocnicza określająca maksymalną liczę obiektów pierwszego planu
        public static int noOfObjectsInPanel = 8;
       


        public static void loadInitialImages()
        {
            /* Pamietac o ustawiania obrazkow na dpi rownym 96 
             * wtedy dobrze beda odwzorowane wielkosci!!!
             */

            bgImage= Image.FromFile("images/bg_1024.png");
            menuImage= Image.FromFile("images/menu_1024.png");
            logoImage = Image.FromFile("images/logo_1024.png");

            ans1Image = Image.FromFile("images/ans_1.png");
            ans2Image = Image.FromFile("images/ans_2.png");

            blocks = new Image[7];
            blocks[0] = Image.FromFile("images/block_runner.png");
            blocks[1] = Image.FromFile("images/block_obstacle1.png"); // pionowy 75x150
            blocks[2] = Image.FromFile("images/block_obstacle2.png"); // pionowy 75x225
            blocks[3] = Image.FromFile("images/block_obstacle3.png"); // pionowy 75x300
            blocks[4] = Image.FromFile("images/block_obstacle4.png"); // poziomy 150x75
            blocks[5] = Image.FromFile("images/block_obstacle5.png"); // poziomy 225x75
            blocks[6] = Image.FromFile("images/block_obstacle6.png"); // poziomy 300x75
        }//koniec loadInitialImages()


    }//koniec class GameParameters
}
