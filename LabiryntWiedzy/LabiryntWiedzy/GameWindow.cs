using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace LabiryntWiedzy
{
    /// <summary>
    /// Okno główne gry
    /// </summary>
    public class GameWindow:Form
    {
        /// <summary>
        /// Główny konstruktor klasy - ustawienie parametrów i rozpoczęcie akcji
        /// </summary>
        /// <param name="width">Szerokość okna</param>
        /// <param name="height">Wysokość okna</param>
        public GameWindow(int width, int height)
        {
            
            Size = new System.Drawing.Size(width, height); // Ustawienie wymiarów okna
            StartPosition = FormStartPosition.CenterScreen;  // Ustawienie pozycji początkowej na środek
            FormBorderStyle = FormBorderStyle.FixedSingle; // Zablokowanie możliwości zmiany rozmiaru okna
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; // Ukrycie ramki okna i przycisków
            initGUI(width, height); // Wywołanie metody budowy interfejsu
            Application.Run(this);  // Wyświetlenie okna

        }//koniec GameWindow()

        /// <summary>
        /// Utworzenie interfejsu graficznego użytkownika
        /// </summary>
        /// <param name="width">Szerokość okna</param>
        /// <param name="height">Wysokość okna</param>
        private void initGUI(int width, int height)
        {
            GPars.loadInitialImages();
            Controls.Add(new GamePanel(width, height)); // Dodanie panelu gry zawierającego grafikę i akcję
        }//koniec initGUI()
    }
}
