using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Globalization;
using System.Diagnostics;

namespace LabiryntWiedzy
{
    /// <summary>
    /// Klasa - kontener parametrów
    /// Odczytuje zasoby graficzne
    /// </summary>
    public class GPars
    {
        /// <summary>
        /// Tablic grafik tła
        /// </summary>
        public static Image[] bgImages;
        /// <summary>
        /// Obraz ikony Menu
        /// </summary>
        public static Image menuImage;
        /// <summary>
        /// Obraz ikony logo
        /// </summary>
        public static Image logoImage;
        /// <summary>
        /// Obraz ikony wyjścia nr 1
        /// </summary>
        public static Image ans1Image;
        /// <summary>
        /// Obraz ikony wyjścia nr 2
        /// </summary>
        public static Image ans2Image;
        /// <summary>
        /// Grafika z wytłumaczaniem jak grać
        /// </summary>
        public static Image howToPlayImage;
        /// <summary>
        /// Tablica grafik obiektów pierwszego planu - klocków
        /// </summary>
        public static Image[] blocksImages;
        /// <summary>
        /// Tablica grafik ikon wyboru poziomu
        /// </summary>
        public static Image[] lvlImages;
        /// <summary>
        /// Zmienna stanu określająca czy jest przerwa w grze
        /// </summary>
        public static Boolean pause = false;
        /// <summary>
        /// Zmienna stanu określająca zakończono gre
        /// </summary>
        public static Boolean end = false;
        /// <summary>
        /// Zmienna stanu określająca czy skończono poziom
        /// </summary>
        public static Boolean lvlEnd = false;
        /// <summary>
        /// Zmienna stanu określająca czy rozpoczęto grę
        /// </summary>
        public static Boolean gStarted = false;
        /// <summary>
        /// Zmienna stanu określająca czy wybrano z menu pozycję "O grze..."
        /// </summary>
        public static Boolean gInformation = false;
        /// <summary>
        /// Zmienna stanu określająca czy wybrano z menu pozycję "Wybierz poziom"
        /// </summary>
        public static Boolean gLevelSel = false;
        /// <summary>
        /// Zmienna stanu określająca czy wyświetlić startowe menu
        /// </summary>
        public static Boolean gMenu = false;
        /// <summary>
        /// Zmienna określająca prawidłową odpowiedź w danym poziomie
        /// </summary>
        public static int questionAns;
        /// <summary>
        /// Zmienna określająca czy wybrano prawidłową odpowiedź
        /// </summary>
        public static Boolean rightAns = false;
        /// <summary>
        /// Zmienna pomocnicza określająca liczę obiektów pierwszego planu - klocków
        /// </summary>
        public static int noOfObjects = 12;
        /// <summary>
        /// Zmienna pomocnicza określająca liczbę poziomów w grze
        /// </summary>
        public static int noOfLevels = 5;
        /// <summary>
        /// Stoper do mierzenia czasu przejścia poszczególnych poziomów
        /// </summary>
        public static Stopwatch stopwatch;

        /// <summary>
        /// Metoda ładowania początkowych zasobów gry
        /// </summary>
        public static void loadInitialImages()
        {
            /* Pamietac o ustawiania obrazkow na dpi rownym 96 
             * wtedy dobrze bedą odwzorowane wielkości!!!
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

            howToPlayImage = Image.FromFile("images/h2p.png");

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
