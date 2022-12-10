using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace LabiryntWiedzy
{
    public class GameWindow:Form
    { 
        
        public GameWindow(int width, int height)
        {
            
            Size = new System.Drawing.Size(width, height); // ustawienie wymiarow okna
            StartPosition = FormStartPosition.CenterScreen;  // ustawienie pozycji poczatkowej na srodek
            FormBorderStyle = FormBorderStyle.FixedSingle; // zablokowanie mozliwosci zmiany rozmiaru okna
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; // ukrycie ramki okna i przyciskow
            initGUI(width, height);
            Application.Run(this);  // wyswietlenie okna

        }//koniec GameWindow()

        private void initGUI(int width, int height)
        {
            GPars.loadInitialImages();
            //this.Cursor = new Cursor("images/default.ico"); // ustawienie inne wygladu kursora myszki
            Controls.Add(new GamePanel(width, height)); // dodanie panelu gry zawierający grafikę i akcję

        }//koniec initGUI()

    }
}
