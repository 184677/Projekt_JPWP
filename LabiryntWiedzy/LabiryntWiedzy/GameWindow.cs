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
            BackColor = System.Drawing.Color.Aquamarine;  // ROBOCZO ustawienie koloru tła 
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; // ukrycie ramki okna i przyciskow
            initGUI(width, height);
            Application.Run(this);  // wyswietlenie okna

        }//koniec GameWindow()

        private void initGUI(int width, int height)
        {
            GPars.loadInitialImages();

            Button a = new Button(); // ROBOCZO przycisk do zamykania okna
            a.Size = new System.Drawing.Size(200, 50);
            a.BackColor = System.Drawing.Color.Pink;
            a.Location = new System.Drawing.Point(400, 0);
            a.Text = "Zamknij okno";
            a.Click += new System.EventHandler(this.buttonPressed);

            Controls.Add(a);

            Controls.Add(new GamePanel(width, height)); // dodanie panelu gry zawierający grafikę i akcję

        }//koniec initGUI()

        private void buttonPressed(object sender, System.EventArgs e)
        {
           this.Close();
        }


    }
}
