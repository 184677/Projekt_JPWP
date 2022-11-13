using System;
using System.Windows.Forms;

namespace LabiryntWiedzy
{
    class Program
    {
        static void Main(string[] args)
        {
            int gameWidth = 1024; //szerokosc okna
            int gameHeight = 768; //wysokosc okna

            //utwórz obiekt klasy GameWindow - konstruktor wywołuje dalszą akcję
            GameWindow gw = new GameWindow(gameWidth, gameHeight);
            
        }
    }//koniec Main()
}//koniec class Program()
