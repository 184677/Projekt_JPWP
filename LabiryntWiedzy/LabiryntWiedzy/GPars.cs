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
        //public static Image menuGameImage; //Obraz ikony Menu powrotu do gry
        //public static Image logoImage; // Obraz ikony logo
        public static Image[] blocks; //Tablica grafik obiektów pierwszego planu
        public static int noOfObjects = 10; //Zmienna pomocnicza określająca maksymalną liczę obiektów pierwszego planu
        public static int gameStatus = 0; //Zmienna stanu określająca stan gry 0-startowe menu, 1-rozpoczecie rozgrywki  


        public static void loadInitialImages()
        {
            /* Pamietac o ustawiania obrazkow na dpi rownym 96 
             * wtedy dobrze beda odwzorowane wielkosci!!!
             */

            bgImage= Image.FromFile("images/bg_1024.jpg");
            menuImage= Image.FromFile("images/menu_1024.png");
            //menuGameImage = Image.FromFile("images/gra_1024.png");
            //logoImage = Image.FromFile("images/logo.png");
            


            blocks = new Image[3];
            blocks[0] = Image.FromFile("images/block_runner.jpg");
            blocks[1] = Image.FromFile("images/block_obstacle1.jpg");
            blocks[2] = Image.FromFile("images/block_obstacle2.jpg");
            //blocks[3] = Image.FromFile("images/block_obstacle_3.png");
            //blocks[4] = Image.FromFile("images/block_obstacle_4.png");
            //blocks[5] = Image.FromFile("images/block_obstacle_5.png");
            //blocks[6] = Image.FromFile("images/block_obstacle_6.png");
        }//koniec loadInitialImages()


    }//koniec class GameParameters
}
