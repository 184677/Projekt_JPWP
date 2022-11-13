using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LabiryntWiedzy
{
    class GPars
    {
        //public static long GAME_TIME = long.MaxValue; //Dopuszczalny czas gry
        //public readonly static long NO_LEVELS = 5; //Liczba poziomów gry
        public static Image bgImage; //Obraz tła
        //public static Image menuImage; //Obraz ikony Menu
        //public static Image menuGameImage; //Obraz ikony Menu powrotu do gry
        //public static Image logoImage; // Obraz ikony logo
        public static Image[] blocks; //Tablica obiektów pierwszego planu
        //public static Boolean pause = false; //Zmienna stanu określającam czy jest przerwa w grze
        //public static Boolean levelPause = false; //Zmienna stanu określająca czy wybrano menu
        //public static long startTime; //Zmienna pomocnicza określająca początkowy czas gry
        //public static double levelTime; //Zmienna pomocnicza określająca czas ukończenia aktualnego poziomu
        //public static int MoveMODE = 1; //Zmienna pomocnicza określająca aktualny poziom gry
        //public static Boolean end = false; //Zmienna pomocnicza określająca status ukończenia gry
        //public static int noOfObjects = 10; //Zmienna pomocnicza określająca maksymalną liczę obiektów pierwszego planu
        //public static int gWidth = 1024; //Szerokość pola graficznego gry
        //public static int gHeight = 768; //Wysokość pola graficznego gry


        public static void loadInitialImages()
        {
            /* Pamietac o ustawiania obrazkow na dpi rownym 96 
             * wtedy dobrze beda odwzorowane wielkosci!!!
             */

            bgImage= Image.FromFile("images/bg_1024.jpg");
            //menuImage= Image.FromFile("images/menu_1024.jpg");
            //menuGameImage = Image.FromFile("images/gra_1024.png");
            //logoImage = Image.FromFile("images/logo.png");
            


            blocks = new Image[2];
            blocks[0] = Image.FromFile("images/block_runner.jpg");
            blocks[1] = Image.FromFile("images/block_obstacle1.jpg");
            //blocks[2] = Image.FromFile("images/block_obstacle_2.png");
            //blocks[3] = Image.FromFile("images/block_obstacle_3.png");
            //blocks[4] = Image.FromFile("images/block_obstacle_4.png");
            //blocks[5] = Image.FromFile("images/block_obstacle_5.png");
            //blocks[6] = Image.FromFile("images/block_obstacle_6.png");
        }//koniec loadInitialImages()


    }//koniec class GameParameters
}
