using System;
using System.Windows.Forms;

namespace LabiryntWiedzy
{
    /// <summary>
    /// Gra - Labirynt Wiedzy
    /// Autor - Oskar Bogucki 184677
    /// </summary>
    class Program
    {
        /// <summary>
        /// Metoda uruchamia grę
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            int gameWidth = 1024; // Szerokość okna
            int gameHeight = 768; // Wysokość okna

            GameWindow gw = new GameWindow(gameWidth, gameHeight); // Utworzenie obiektu klasy GameWindow - konstruktor wywołuje dalszą akcję
        }
    }//koniec Main()
}//koniec class Program()
