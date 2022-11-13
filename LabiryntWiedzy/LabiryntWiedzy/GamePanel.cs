﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;


namespace LabiryntWiedzy
{
    public class GamePanel : Panel
    {
        public int sWidth; //Szerokość pola graficznego gry
        public int sHeight; //Wysokość pola graficznego gry
        public int barHeight; //Wysokość paska menu
        //public Font menuFont; //Czcionki stosowane w pasku Menu
        //public Font alertFont; //Czcionki stosowane jako alert w polu gry
        //private Block[] fBlocks; //Tablica obiektów pierwszego planu - klocki

        public GamePanel(int width, int height)
        {
            
            this.sWidth = width;
            this.sHeight = height;
            Size = new System.Drawing.Size(sWidth, sHeight);
            Location = new System.Drawing.Point(0, 0);

            //menuFont = new Font(FontFamily.GenericSansSerif, 36.0F, FontStyle.Bold);
            //alertFont = new Font(FontFamily.GenericSansSerif, 92.0F, FontStyle.Bold);

            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Drawbg); // rysowanie tla

        }

        private void Drawbg(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;        
            g.DrawImage(GPars.bgImage, new Point(0, 0));  
        }


    }
}