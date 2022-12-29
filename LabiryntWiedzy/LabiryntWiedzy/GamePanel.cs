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
        public static Rectangle labirynthRec; // prostokat reprezentujacy labirynt
        public static Rectangle questionRec; // prostokat zawierajacy pytanie
        public static Rectangle barMenuRec; // prostokat reprezentujacy pasek menu
        public static Rectangle exit1Rec; // prostokat reprezentujacy wyjscie nr1
        public static Rectangle exit2Rec; // prostokat reprezentujacy wyjscie nr2
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

            labirynthRec = new Rectangle(120, 215, 415, 415);
            questionRec = new Rectangle(665, 165, 300, 250);
            barMenuRec = new Rectangle(35, 20, sWidth - 70 - 1, barHeight);
            exit1Rec = new Rectangle(labirynthRec.Left - 80, labirynthRec.Bottom - 160, 75, 75);
            exit2Rec = new Rectangle(labirynthRec.Right + 7, labirynthRec.Top + 85, 75, 75);

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

            if (gStatus.level==1) g.DrawImage(GPars.bgImages[0], new Point(0, 0));
            else if (gStatus.level == 2 ) g.DrawImage(GPars.bgImages[1], new Point(0, 0));
            else if (gStatus.level == 3) g.DrawImage(GPars.bgImages[2], new Point(0, 0));
            else if (gStatus.level == 4) g.DrawImage(GPars.bgImages[3], new Point(0, 0));
            else g.DrawImage(GPars.bgImages[4], new Point(0, 0));

            if (GPars.pause)
            {
                Rectangle startMenuRec = new Rectangle((sWidth-400)/2, (sHeight-400)/2, 400, 400); // prostokat reprezentujacy startowe menu
                GraphicsPath startMenu = Block.RoundedRect(startMenuRec, 10); // Menu poczatkowe z zaokraglonymi rogami
                g.DrawPath(Whitepen, startMenu); // narysowanie obwodki
                g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), startMenu); // wypelnienie
                g.DrawString("Wybierz poziom", menuFont, Brushes.White, new Point(375, 305));
                g.DrawString("O grze...", menuFont, Brushes.White, new Point(440, 390));
                g.DrawString("Wyjdź z gry", menuFont, Brushes.White, new Point(400, 475));

                if (gStatus.gStarted) g.DrawString("Kontynuuj grę" , menuFont, Brushes.White, new Point(390, 225));
                else g.DrawString("Nowa gra", menuFont, Brushes.White, new Point(420, 225));

            }
            else
            {
                GraphicsPath barMenu = Block.RoundedRect(barMenuRec, 10); // pasek menu z zaokraglonymi rogami
                g.DrawPath(Whitepen, barMenu); // narysowanie obwodki
                g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), barMenu); // wypelnienie

                g.DrawImage(GPars.logoImage, 860, 680);// narysowanie loga
                g.DrawImage(GPars.menuImage, new Point(820, 20)); // narysowanie ikony menu

                g.DrawString("POZIOM :", menuFont, Brushes.White, new Point(40, 23));
                g.DrawString("" + gStatus.level, menuFont, Brushes.White, new Point(185, 23)); // wypisanie numeru aktualnego poziomu
                g.DrawString("PUNKTY :", menuFont, Brushes.White, new Point(300, 23));
                g.DrawString("" + gStatus.points, menuFont, Brushes.White, new Point(452, 23)); // wypisanie numeru aktualnego poziomu

                GraphicsPath labirynth = Block.RoundedRect(labirynthRec, 3); // labirynt z zaokraglonymi rogami
                g.DrawPath(Whitepen, labirynth); // narysowanie obwodki labiryntu
                g.FillPath(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), labirynth); // wypelnienie labiryntu

                GraphicsPath question = Block.RoundedRect(questionRec, 10); // prostokat zawierajacy pytanie z zaokraglonymi rogami
                g.DrawPath(Whitepen, question); // narysowanie obwodki 
                g.FillPath(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), question); // wypelnienie 
               
                g.DrawString("Pytanie nr 1", questionFont, Brushes.White, new Point(750, 170));
                g.DrawString("Jaki ładunek ma neutron?", questionFont, Brushes.White, new Point(685, 210));
                g.DrawString("1) negatywny", questionFont, Brushes.White, new Point(750, 280));
                g.DrawString("2) neutralny", questionFont, Brushes.White, new Point(750, 320));

                g.DrawImage(GPars.ans1Image, exit1Rec.Left, exit1Rec.Top); // narysowanie wyjscia nr1
                g.DrawImage(GPars.ans2Image, exit2Rec.Left, exit2Rec.Top); // narysowanie wyjscia nr2
        
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
                g.DrawImage(blocks[8].icon, blocks[8].rec.Left, blocks[8].rec.Top);
                g.DrawImage(blocks[9].icon, blocks[9].rec.Left, blocks[9].rec.Top);
                g.DrawImage(blocks[10].icon, blocks[10].rec.Left, blocks[10].rec.Top);
                g.DrawImage(blocks[11].icon, blocks[11].rec.Left, blocks[11].rec.Top);
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
                    if (!gStatus.gStarted) gStatus.gStarted = true;
                    GPars.pause = false;
                    Invalidate();
                }
                //Czy wybrano opcję o grze... w startowym menu
                if (e.X > 457 && e.X < 587 && e.Y > 411 && e.Y < 455)
                {
                    gStatus.nextLevel();
                    initLevel();
                    Invalidate();
                }
                //Czy wybrano wyjdz z gry w startowym menu
                if (e.X > 400 && e.X < 615 && e.Y < 542 && e.Y > 500)
                {
                    Application.Exit();
                }
            }
            else if (GPars.pause == false && e.Button == MouseButtons.Left)
            {
                if (e.X > 850 && e.X < 1000 && e.Y > 30 && e.Y < 80)
                {
                    GPars.pause = true;
                    Invalidate();
                }

            }

        } // koniec OnMouseDown


        protected override void OnMouseMove(MouseEventArgs e)
        {
            for (int i=0; i< GPars.noOfObjects; i++)
            {
                if (e.Button == MouseButtons.Left && !GPars.pause) // poruszanie klockiem w poziomie
                {
                    if (MouseDownLocation.X > blocks[i].rec.Left && MouseDownLocation.X < (blocks[i].rec.Left + blocks[i].width) && MouseDownLocation.Y > blocks[i].rec.Top && MouseDownLocation.Y < (blocks[i].rec.Top + blocks[i].height))
                    {
                        if (blocks[i].type == 3 || blocks[i].type == 2 )
                        {
                            if (Block.MoveCheck(blocks[i].rec, i, (e.X - MouseDownLocation.X), 0)) // sprawdzenie mozliwosci wykonania ruchu
                            {
                                blocks[i].rec.Location = new Point((e.X - MouseDownLocation.X) + blocks[i].rec.Left, blocks[i].rec.Top);
                                MouseDownLocation = e.Location;
                                ExitCheck();
                                Invalidate();
                            }
                        }
                    }
                }
                else if (e.Button == MouseButtons.Right && !GPars.pause) // poruszanie klockiem w pionie
                {
                    if (MouseDownLocation.X > blocks[i].rec.Left && MouseDownLocation.X < (blocks[i].rec.Left + blocks[i].width) && MouseDownLocation.Y > blocks[i].rec.Top && MouseDownLocation.Y < (blocks[i].rec.Top + blocks[i].height))
                    {

                        if (blocks[i].type == 3 || blocks[i].type == 1)
                        {
                            if (Block.MoveCheck(blocks[i].rec, i, 0, (e.Y - MouseDownLocation.Y))) // sprawdzenie mozliwosci wykonania ruchu
                            {
                                blocks[i].rec.Location = new Point(blocks[i].rec.Left, (e.Y - MouseDownLocation.Y) + blocks[i].rec.Top);
                                MouseDownLocation = e.Location;
                                ExitCheck();
                                Invalidate();
                            }
                        }
                    }
                }
            }
        }// koniec OnMouseMove



        public void ExitCheck()
        {
            if (blocks[0].rec.IntersectsWith(exit1Rec))
            {
                gStatus.nextLevel();
                if (gStatus.rightAns == 1) gStatus.points += 1;
                initLevel();
            }
            else if (blocks[0].rec.IntersectsWith(exit2Rec))
            {
                gStatus.nextLevel();
                if (gStatus.rightAns == 2) gStatus.points += 1;
                initLevel();
            }

        } //koniec ExitCheck

        private void restartGame() 
        {
            gStatus.resetPoints();
            gStatus.gStarted = false;
            GPars.pause = true;
            blocks[0]= new Block(labirynthRec.X +170, labirynthRec.Y + 170, 75, 75, 3, GPars.blocksImages[0]); // 75x75 --> 1x1
            blocks[1]= new Block(labirynthRec.X, labirynthRec.Y, 160, 75, 2, GPars.blocksImages[4]); // 160x75 --> 2x1
            blocks[2] = new Block(labirynthRec.X + 85, labirynthRec.Y + 85, 160, 75, 2, GPars.blocksImages[4]); // 160x75 --> 2x1
            blocks[3] = new Block(labirynthRec.X, labirynthRec.Y + 85, 75, 160, 1, GPars.blocksImages[1]); // 75x160 --> 1x2
            blocks[4] = new Block(labirynthRec.X + 340 , labirynthRec.Y, 75, 245, 1, GPars.blocksImages[2]); // 75x245 --> 1x3
            blocks[5] = new Block(labirynthRec.X, labirynthRec.Y + 255, 160, 75, 2, GPars.blocksImages[4]); // 160x75 --> 2x1
            blocks[6] = new Block(labirynthRec.X, labirynthRec.Y + 340, 330, 75, 2, GPars.blocksImages[6]); // 330x75 --> 4x1
            blocks[7] = new Block(labirynthRec.X + 255, labirynthRec.Y + 255, 160, 75, 2, GPars.blocksImages[4]); // 160x75 --> 2x1
            blocks[8] = new Block(1500, 1500, 75, 245, 1, GPars.blocksImages[2]); // 75x245 --> 1x3
            blocks[9] = new Block(1500, 1500, 75, 245, 1, GPars.blocksImages[2]); // 75x245 --> 1x3
            blocks[10] = new Block(1500, 1500, 245, 75, 2, GPars.blocksImages[5]); // 245x75 --> 3x1
            blocks[11] = new Block(1500, 1500, 245, 75, 2, GPars.blocksImages[5]); // 245x75 --> 3x1
            gStatus.rightAns = 1;

        }//koniec restartGame()

        public void initLevel() //funkcja do ustawiania polozenia klockow na planszy
        {

           if (gStatus.level == 2 && !GPars.end)
            {
                blocks[0].rec.Location = new Point(labirynthRec.X + 170, labirynthRec.Y );
                blocks[1].rec.Location = new Point(1500, 1500);
                blocks[2].rec.Location = new Point(labirynthRec.X + 85, labirynthRec.Y + 85);
                blocks[3].rec.Location = new Point(labirynthRec.X + 170, labirynthRec.Y + 170);
                blocks[4].rec.Location = new Point(labirynthRec.X + 340, labirynthRec.Y + 85);
                blocks[5].rec.Location = new Point(1500, 1500);
                blocks[6].rec.Location = new Point(1500, 1500);
                blocks[7].rec.Location = new Point(1500, 1500);
                blocks[8].rec.Location = new Point(labirynthRec.X, labirynthRec.Y + 85);
                blocks[9].rec.Location = new Point(labirynthRec.X + 255, labirynthRec.Y);
                blocks[10].rec.Location = new Point(labirynthRec.X+ 170, labirynthRec.Y + 340);
                blocks[11].rec.Location = new Point(1500, 1500);
                gStatus.rightAns = 1;
            }
           else if (gStatus.level == 3 && !GPars.end)
            {
                blocks[0].rec.Location = new Point(labirynthRec.X + 170, labirynthRec.Y + 170);
                blocks[1].rec.Location = new Point(labirynthRec.X, labirynthRec.Y + 170);
                blocks[2].rec.Location = new Point(1500,1500);
                blocks[3].rec.Location = new Point(labirynthRec.X, labirynthRec.Y + 255);
                blocks[4].rec.Location = new Point(labirynthRec.X + 340, labirynthRec.Y );
                blocks[5].rec.Location = new Point(1500,1500);
                blocks[6].rec.Location = new Point(labirynthRec.X, labirynthRec.Y + 85);
                blocks[7].rec.Location = new Point(1500,1500);
                blocks[8].rec.Location = new Point(1500,1500);
                blocks[9].rec.Location = new Point(1500,1500);
                blocks[10].rec.Location = new Point(labirynthRec.X + 170, labirynthRec.Y + 340);
                blocks[11].rec.Location = new Point(1500, 1500);
                gStatus.rightAns = 2;
            }
            else if (gStatus.level == 4 && !GPars.end)
            {
                blocks[0].rec.Location = new Point(labirynthRec.X + 170, labirynthRec.Y + 170);
                blocks[1].rec.Location = new Point(labirynthRec.X + 170, labirynthRec.Y + 85);
                blocks[2].rec.Location = new Point(labirynthRec.X + 85, labirynthRec.Y + 255); 
                blocks[3].rec.Location = new Point(1500, 1500);
                blocks[4].rec.Location = new Point(labirynthRec.X, labirynthRec.Y + 170);
                blocks[5].rec.Location = new Point(1500, 1500);
                blocks[6].rec.Location = new Point(1500, 1500);
                blocks[7].rec.Location = new Point(1500, 1500);
                blocks[8].rec.Location = new Point(labirynthRec.X + 340, labirynthRec.Y);
                blocks[9].rec.Location = new Point(1500, 1500);
                blocks[10].rec.Location = new Point(labirynthRec.X, labirynthRec.Y);
                blocks[11].rec.Location = new Point(labirynthRec.X + 170, labirynthRec.Y + 340);
                gStatus.rightAns = 1;
            }
            else if (gStatus.level == 5 && !GPars.end)
            {

            }


        }//koniec initLevel()


    }


}
