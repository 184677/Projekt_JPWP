using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Media;

namespace LabiryntWiedzy
{
    public class GamePanel : Panel
    {
        public int sWidth; //Szerokość pola graficznego gry
        public int sHeight; //Wysokość pola graficznego gry
        public int barHeight; //Wysokość paska menu
        public GameStatus gStatus; // zmienna reprezentujaca stan gry  0-stan poczatkowy, rysowanie menu
        public FontFamily fontFamily; //
        public Font menuFont; //Czcionki stosowane w pasku Menu
        public Font questionFont; //Czcionki stosowane do wypisania pytan
        public static Block[] blocks; // tablica obiektów pierwszego planu - klocki
        private Point MouseDownLocation;

        public GamePanel(int width, int height)
        {
            gStatus = new GameStatus();
            gStatus.reset();

            DoubleBuffered = true; // zapobieganie efektu typu blinking
            this.sWidth = width;
            this.sHeight = height;
            barHeight = 65;
            Size = new System.Drawing.Size(sWidth, sHeight);
            Location = new System.Drawing.Point(0, 0);

            fontFamily = new FontFamily("Haettenschweiler");
            menuFont = new Font(fontFamily, 52, FontStyle.Regular, GraphicsUnit.Pixel);
            questionFont = new Font(fontFamily, 32, FontStyle.Regular, GraphicsUnit.Pixel);
            SoundPlayer sp = new SoundPlayer();
            //sp.SoundLocation = "music/bg_music.wav";
            //sp.PlayLooping();

            blocks = new Block[GPars.noOfObjects];
            restartGame();

        } // koniec GamePanel()



        protected override void OnPaint( PaintEventArgs e)
        {
            Pen Whitepen = new Pen(Brushes.White);
            Whitepen.Width = 4.0F;
            Pen Redpen = new Pen(Brushes.Red);
            Redpen.Width = 4.0F;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawImage(GPars.bgImage, new Point(0, 0));
            if (GPars.pause==true)
            {
                Rectangle startMenuRec = new Rectangle((sWidth-400)/2, (sHeight-400)/2, 400, 400); // prostokat reprezentujacy startowe menu
                GraphicsPath startMenu = Block.RoundedRect(startMenuRec, 10); // Menu poczatkowe z zaokraglonymi rogami
                g.DrawPath(Whitepen, startMenu); // narysowanie obwodki
                g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), startMenu); // wypelnienie
                g.DrawString("Nowa gra", menuFont, Brushes.White, new Point(420, 225));
                g.DrawString("Wybierz poziom", menuFont, Brushes.White, new Point(375, 305));
                g.DrawString("O grze...", menuFont, Brushes.White, new Point(440, 390));
                g.DrawString("Wyjdź z gry", menuFont, Brushes.White, new Point(400, 475));
            }
            else if (GPars.pause == false) 
            {

                Rectangle barMenuRec = new Rectangle(35, 20, sWidth - 70 -1, barHeight); // prostokat reprezentujacy pasek menu
                GraphicsPath barMenu = Block.RoundedRect(barMenuRec, 10); // pasek menu z zaokraglonymi rogami
                g.DrawPath(Whitepen, barMenu); // narysowanie obwodki
                g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), barMenu); // wypelnienie

                g.DrawImage(GPars.logoImage, 860, 680);// narysowanie loga

                g.DrawImage(GPars.menuImage, new Point(820, 20));
                g.DrawString("POZIOM :", menuFont, Brushes.White, new Point(40, 23));
                g.DrawString("" + gStatus.level, menuFont, Brushes.White, new Point(185, 23));
                g.DrawString("PUNKTY :", menuFont, Brushes.White, new Point(300, 23));
                g.DrawString("" + gStatus.points, menuFont, Brushes.White, new Point(452, 23));

                Rectangle labirynthRec = new Rectangle(120, 215, 415 , 415); // prostokat reprezentujacy labirynt
                GraphicsPath labirynth = Block.RoundedRect(labirynthRec, 3); // labirynt z zaokraglonymi rogami
                g.DrawPath(Whitepen, labirynth); // narysowanie obwodki labiryntu
                g.FillPath(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), labirynth); // wypelnienie labiryntu

                Rectangle questionRec = new Rectangle(665, 165, 300, 250); // prostokat zawierajacy pytanie
                GraphicsPath question = Block.RoundedRect(questionRec, 10); // prostokat zawierajacy pytanie z zaokraglonymi rogami
                g.DrawPath(Whitepen, question); // narysowanie obwodki 
                g.FillPath(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), question); // wypelnienie 
               
                g.DrawString("Pytanie nr 1", questionFont, Brushes.White, new Point(750, 170));
                g.DrawString("Jaki ładunek ma neutron?", questionFont, Brushes.White, new Point(685, 210));
                g.DrawString("1) negatywny", questionFont, Brushes.White, new Point(750, 280));
                g.DrawString("2) neutralny", questionFont, Brushes.White, new Point(750, 320));


                g.DrawImage(GPars.ans1Image, new Point(labirynthRec.Left-80, labirynthRec.Bottom - 160)); // narysowanie wyjscia nr1
                g.DrawImage(GPars.ans2Image, new Point(labirynthRec.Right+7 , labirynthRec.Top +85 )); // narysowanie wyjscia nr2
        
                g.DrawLine(Redpen, new Point(labirynthRec.Right, labirynthRec.Top + 75), new Point(labirynthRec.Right, labirynthRec.Top + 170)); //linia wyjscia nr1
                g.DrawLine(Redpen, new Point(labirynthRec.Left, labirynthRec.Bottom - 170), new Point(labirynthRec.Left, labirynthRec.Bottom - 75)); // linia wyjscia nr2

                g.DrawImage(blocks[0].icon, blocks[0].rec.Left, blocks[0].rec.Top);
                g.DrawImage(blocks[1].icon, blocks[1].rec.Left, blocks[1].rec.Top);
                g.DrawImage(blocks[2].icon, blocks[2].rec.Left, blocks[2].rec.Top);
                g.DrawImage(blocks[3].icon, blocks[3].rec.Left, blocks[3].rec.Top);
                g.DrawImage(blocks[4].icon, blocks[4].rec.Left, blocks[4].rec.Top);
                g.DrawImage(blocks[5].icon, blocks[5].rec.Left, blocks[5].rec.Top);
                g.DrawImage(blocks[6].icon, blocks[6].rec.Left, blocks[6].rec.Top);
                g.DrawImage(blocks[7].icon, blocks[7].rec.Left, blocks[7].rec.Top);
            }
            Whitepen.Dispose();
            Redpen.Dispose();
        }

        protected override void OnMouseDown( MouseEventArgs e)
        {
            MouseDownLocation = e.Location;
          
            if (GPars.pause == true && e.Button == MouseButtons.Left)
            {
                //Czy wybrano opcję nowa gra w startowym menu
                if (e.X > 439 && e.X < 592 && e.Y > 246 && e.Y < 281)
                {
                    GPars.pause = false;
                    Invalidate();
                }
                //Czy wybrano opcję o grze... w startowym menu
                if (e.X > 457 && e.X < 587 && e.Y > 411 && e.Y < 455)
                {
                    GPars.bgImage = Image.FromFile("images/bg_1024v2.png");
                    Invalidate();
                }
                //Czy wybrano wyjdz z gry w startowym menu
                if (e.X > 400 && e.X < 615 && e.Y < 542 && e.Y > 500)
                {
                    Application.Exit();
                }
            }
            else if (gStatus.level == 1 && e.Button == MouseButtons.Left)
            {
                if (e.X > 850 && e.X < 1000 && e.Y > 30 && e.Y < 80)
                {
                    GPars.pause = true;
                    Invalidate();
                }

            }

        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            for (int i=0; i< GPars.noOfObjectsInPanel; i++)
            {
                if (e.Button == MouseButtons.Left && gStatus.level == 1) // poruszanie klockiem w poziomie
                {
                    if (MouseDownLocation.X > blocks[i].rec.Left && MouseDownLocation.X < (blocks[i].rec.Left + blocks[i].width) && MouseDownLocation.Y > blocks[i].rec.Top && MouseDownLocation.Y < (blocks[i].rec.Top + blocks[i].height))
                    {
                        if (blocks[i].type == 3 || blocks[i].type == 2 )
                        {
                            if (Block.MoveCheck(blocks[i].rec, i, (e.X - MouseDownLocation.X), 0)) // sprawdzenie mozliwosci wykonania ruchu
                            {
                                blocks[i].rec.Location = new Point((e.X - MouseDownLocation.X) + blocks[i].rec.Left, blocks[i].rec.Top);
                                MouseDownLocation = e.Location;
                                Invalidate();
                            }
                        }
                    }
                }
                else if (e.Button == MouseButtons.Right && gStatus.level == 1) // poruszanie klockiem w pionie
                {
                    if (MouseDownLocation.X > blocks[i].rec.Left && MouseDownLocation.X < (blocks[i].rec.Left + blocks[i].width) && MouseDownLocation.Y > blocks[i].rec.Top && MouseDownLocation.Y < (blocks[i].rec.Top + blocks[i].height))
                    {

                        if (blocks[i].type == 3 || blocks[i].type == 1)
                        {
                            if (Block.MoveCheck(blocks[i].rec, i, 0, (e.Y - MouseDownLocation.Y))) // sprawdzenie mozliwosci wykonania ruchu
                            {
                                blocks[i].rec.Location = new Point(blocks[i].rec.Left, (e.Y - MouseDownLocation.Y) + blocks[i].rec.Top);
                                MouseDownLocation = e.Location;                               
                                Invalidate();
                            }
                        }
                    }
                }
            }
        }


        

        private void restartGame() // zastanowic sie czy nie lepiej od razu zdefiniowac wszystkie klocki i potem rysowac tylko te ktore chcemy
        {
            gStatus.resetPoints();
            GPars.pause = true;
            blocks[0]= new Block(120+170, 215+170, 75, 75, 3, GPars.blocks[0]);
            blocks[1]= new Block(120, 215, 150, 75, 2, GPars.blocks[4]);
            blocks[2] = new Block(120+85, 215+85, 150, 75, 2, GPars.blocks[4]);
            blocks[3] = new Block(120, 215+85, 75, 150, 1, GPars.blocks[1]);
            blocks[4] = new Block(340+120, 215, 75, 225, 1, GPars.blocks[2]);
            blocks[5] = new Block(120, 215+255, 150, 75, 2, GPars.blocks[4]);
            blocks[6] = new Block(120, 215 + 340, 300, 75, 2, GPars.blocks[6]);
            blocks[7] = new Block(120+255, 215+255, 150, 75, 2, GPars.blocks[4]);
        }//koniec restartGame()



        

    }


}
