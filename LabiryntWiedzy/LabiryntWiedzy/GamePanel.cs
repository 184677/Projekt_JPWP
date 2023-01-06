using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Media;

using System.Diagnostics;
using System.Threading;

namespace LabiryntWiedzy
{
    public class GamePanel : Panel
    {
        public int sWidth; //Szerokość pola graficznego gry
        public int sHeight; //Wysokość pola graficznego gry
        public int barHeight; //Wysokość paska menu
        public GameStatus gStatus; // zmienna reprezentujaca stan gry  0-stan poczatkowy, rysowanie menu
        public FontFamily fontFamily; //Rodzina używanych czcionek
        public Font menuFont; //Czcionka stosowana w pasku Menu
        public Font menuFontv2; //Czcionka stosowana w pasku Menu - wersja zmniejszona
        public Font questionFont; //Czcionka stosowane do wypisania pytan
        public static Block[] blocks; // tablica obiektów pierwszego planu - klocki
        public static Rectangle startMenuRec; // // prostokat reprezentujacy startowe menu
        public static Rectangle labirynthRec; // prostokat reprezentujacy labirynt
        public static Rectangle questionRec; // prostokat zawierajacy pytanie
        public static Rectangle barMenuRec; // prostokat reprezentujacy pasek menu
        public static Rectangle shortMenuRec; // prostokat reprezentujacy skrocony pasek menu
        public static Rectangle exit1Rec; // prostokat reprezentujacy wyjscie nr1
        public static Rectangle exit2Rec; // prostokat reprezentujacy wyjscie nr2
        public static Label questionLabel; // kontrolka label do wyswietlania pytania
        private Point MouseDownLocation;

        public GamePanel(int width, int height)
        {
            gStatus = new GameStatus();
            gStatus.reset();
            gStatus.deleteResults();

            DoubleBuffered = true; // zapobieganie efektu typu blinking
            this.sWidth = width;
            this.sHeight = height;
            barHeight = 65;
            Size = new System.Drawing.Size(sWidth, sHeight);
            Location = new System.Drawing.Point(0, 0);

            fontFamily = new FontFamily("Haettenschweiler");
            menuFont = new Font(fontFamily, 52, FontStyle.Regular, GraphicsUnit.Pixel);
            menuFontv2 = new Font(fontFamily, 42, FontStyle.Regular, GraphicsUnit.Pixel);
            questionFont = new Font(fontFamily, 32, FontStyle.Regular, GraphicsUnit.Pixel);

            startMenuRec = new Rectangle((sWidth - 400) / 2, (sHeight - 400) / 2, 400, 400);
            labirynthRec = new Rectangle(120, 215, 415, 415);
            questionRec = new Rectangle(665, 165, 300, 250);
            barMenuRec = new Rectangle(35, 20, sWidth - 70 - 1, barHeight);
            exit1Rec = new Rectangle(labirynthRec.Left - 80, labirynthRec.Bottom - 160, 75, 75);
            exit2Rec = new Rectangle(labirynthRec.Right + 7, labirynthRec.Top + 85, 75, 75);
            shortMenuRec = new Rectangle(805, 20, 185 - 1, barHeight);

            questionLabel = new Label();
            questionLabel.Size = new Size(questionRec.Width-20, questionRec.Height - 10);
            questionLabel.Location = new Point(questionRec.Left + 10, questionRec.Top + 5);
            questionLabel.BackColor = Color.FromArgb(0, 0, 0, 0);
            questionLabel.Font = questionFont;
            questionLabel.ForeColor = Color.White;
            questionLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(questionLabel);

            SoundPlayer sp = new SoundPlayer();
            //sp.SoundLocation = "music/bg_music.wav";
            //sp.PlayLooping();

            GPars.stopwatch = new Stopwatch();

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

            g.DrawImage(GPars.logoImage, 860, 680);// narysowanie loga

            if (GPars.pause)
            {
                questionLabel.Visible = false;

                if (GPars.gMenu)
                {
                    GraphicsPath startMenu = Block.RoundedRect(startMenuRec, 10); // Menu poczatkowe z zaokraglonymi rogami
                    g.DrawPath(Whitepen, startMenu); // narysowanie obwodki
                    g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), startMenu); // wypelnienie
                    g.DrawString("Wybierz poziom", menuFont, Brushes.White, new Point(375, 305));
                    g.DrawString("O grze...", menuFont, Brushes.White, new Point(440, 390));
                    g.DrawString("Wyjdź z gry", menuFont, Brushes.White, new Point(400, 475));
                    questionLabel.Visible = false;

                    if (GPars.gStarted && !GPars.end) g.DrawString("Kontynuuj grę", menuFont, Brushes.White, new Point(390, 225));
                    else g.DrawString("Nowa gra", menuFont, Brushes.White, new Point(420, 225));
                }

                else if (GPars.gLevelSel)
                {
                    GraphicsPath shortMenu = Block.RoundedRect(shortMenuRec, 10); // prostokat menu z zaokraglonymi rogami
                    g.DrawPath(Whitepen, shortMenu); // narysowanie obwodki
                    g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), shortMenu); // wypelnienie

                    Rectangle lvlMenuRec1 = new Rectangle(250, 121, 525, 75);
                    GraphicsPath lvlMenu1 = Block.RoundedRect(lvlMenuRec1, 10); // prostokat z zaokraglonymi rogami
                    g.DrawPath(Whitepen, lvlMenu1); // narysowanie obwodki
                    g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), lvlMenu1); // wypelnienie

                    Rectangle lvlMenuRec2 = new Rectangle(250, 271 - 35, 525, 295);
                    GraphicsPath lvlMenu2 = Block.RoundedRect(lvlMenuRec2, 10); // prostokat z zaokraglonymi rogami
                    g.DrawPath(Whitepen, lvlMenu2); // narysowanie obwodki
                    g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), lvlMenu2); // wypelnienie

                    g.DrawString("Wybierz poziom:", menuFont, Brushes.White, new Point(250+120, 128));

                    g.DrawImage(GPars.lvlImages[0], 325, 271);
                    g.DrawImage(GPars.lvlImages[1], 325+150, 271);
                    g.DrawImage(GPars.lvlImages[2], 325+300, 271);
                    g.DrawImage(GPars.lvlImages[3], 325+75, 271+150);
                    g.DrawImage(GPars.lvlImages[4], 325+225, 271+150);

                    g.DrawImage(GPars.logoImage, 860, 680);// narysowanie loga
                    g.DrawImage(GPars.menuImage, new Point(820, 20)); // narysowanie ikony menu
                }
                else if (GPars.gInformation)
                {
                    GraphicsPath shortMenu = Block.RoundedRect(shortMenuRec, 10); // prostokat menu z zaokraglonymi rogami
                    g.DrawPath(Whitepen, shortMenu); // narysowanie obwodki
                    g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), shortMenu); // wypelnienie

                    g.DrawImage(GPars.howToPlayImage, 0, 0);

                    g.DrawImage(GPars.logoImage, 860, 680);// narysowanie loga
                    g.DrawImage(GPars.menuImage, new Point(820, 20)); // narysowanie ikony menu
                }
            }
            else
            {
                GraphicsPath barMenu = Block.RoundedRect(barMenuRec, 10); // pasek menu z zaokraglonymi rogami
                g.DrawPath(Whitepen, barMenu); // narysowanie obwodki
                g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), barMenu); // wypelnienie

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

                questionLabel.Visible = true;

                g.DrawImage(GPars.ans1Image, exit1Rec.Left, exit1Rec.Top); // narysowanie wyjscia nr1
                g.DrawImage(GPars.ans2Image, exit2Rec.Left, exit2Rec.Top); // narysowanie wyjscia nr2

                g.DrawLine(Redpen, new Point(labirynthRec.Right, labirynthRec.Top + 75), new Point(labirynthRec.Right, labirynthRec.Top + 170)); //linia wyjscia nr1
                g.DrawLine(Redpen, new Point(labirynthRec.Left, labirynthRec.Bottom - 170), new Point(labirynthRec.Left, labirynthRec.Bottom - 75)); // linia wyjscia nr2

                for (int i = 0; i < GPars.noOfObjects; i++) g.DrawImage(blocks[i].icon, blocks[i].rec.Left, blocks[i].rec.Top); // rysowanie klockow

                if(GPars.lvlEnd)
                {
                    GraphicsPath nextLvlPanel = Block.RoundedRect(new Rectangle(questionRec.Left, questionRec.Bottom + 50, questionRec.Width, labirynthRec.Bottom-questionRec.Bottom - 50), 10); // pasek menu z zaokraglonymi rogami
                    g.DrawPath(Whitepen, nextLvlPanel); // narysowanie obwodki
                    g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), nextLvlPanel); // wypelnienie

                    if (GPars.rightAns) g.DrawString("Dobra odpowiedź!", menuFontv2, Brushes.Lime, new Point(questionRec.Left + 30 , questionRec.Bottom + 70));
                    else g.DrawString("Zła odpowiedź!", menuFontv2, Brushes.Red, new Point(questionRec.Left + 45 , questionRec.Bottom + 70));

                    if (GPars.end) g.DrawString("Koniec Gry", menuFontv2, Brushes.White, new Point(questionRec.Left + 75, questionRec.Bottom + 135));
                    else g.DrawString("Następny poziom", menuFontv2, Brushes.White, new Point(questionRec.Left + 35, questionRec.Bottom + 135));

                }
       
            }
            Whitepen.Dispose();
            Redpen.Dispose();
        }

        protected override void OnMouseDown( MouseEventArgs e)
        {
            MouseDownLocation = e.Location;

            if (e.Button == MouseButtons.Left)
            {
                if (GPars.pause && GPars.gMenu)
                {
                    //Czy wybrano opcję nowa gra w startowym menu
                    if (e.X > 395 && e.X < 630 && e.Y > 225 && e.Y < 285)
                    {
                        GPars.stopwatch.Start();
                        if (GPars.end)
                        {
                            gStatus.reset();
                            restartGame();
                        }
                        if (!GPars.gStarted) GPars.gStarted = true;
                        GPars.pause = false;
                        GPars.gMenu = false;
                        Invalidate();
                    }
                    //Czy wybrano opcję wybor poziomu w startowym menu
                    if (e.X > 375 && e.X < 650 && e.Y > 310 && e.Y < 365)
                    {
                        GPars.gMenu = false;
                        GPars.gLevelSel = true;
                        Invalidate();
                    }
                    //Czy wybrano opcję o grze... w startowym menu
                    if (e.X > 440 && e.X < 590 && e.Y > 395 && e.Y < 450)
                    {
                        GPars.gMenu = false;
                        GPars.gInformation = true;
                        Invalidate();
                    }
                    //Czy wybrano wyjdz z gry w startowym menu
                    if (e.X > 400 && e.X < 615 && e.Y < 535 && e.Y > 480)
                    {
                        gStatus.saveResults();
                        Application.Exit();
                    }
                }

                else if(GPars.pause && !GPars.gMenu && GPars.gLevelSel) // wybrana pozycja "wybor poziomu"
                {
                    Boolean flagSelected = false; // flaga sprawdzajaca czy wybrano poziom
                    if (e.X > 325 && e.X < (325+75) && e.Y < (271 + 75) && e.Y > 271) // wybrany poziom 1
                    {
                        flagSelected = true;
                        gStatus.level = 1;
                    }
                    else if (e.X > (325 + 150) && e.X < (325 + 225) && e.Y < (271 + 75) && e.Y > 271) // wybrany poziom 2
                    {
                        flagSelected = true;
                        gStatus.level = 2;
                    } 
                    else if (e.X > (325 + 300) && e.X < (325 + 375) && e.Y < (271 + 75) && e.Y > 271) // wybrany poziom 3
                    {
                        flagSelected = true;
                        gStatus.level = 3;
                    }
                    else if (e.X > (325 + 75) && e.X < (325 + 150) && e.Y < (271 + 225) && e.Y > (271 + 150)) // wybrany poziom 4
                    {
                        flagSelected = true;
                        gStatus.level = 4;
                    }
                    else if (e.X > (325 + 225) && e.X < (325 + 300) && e.Y < (271 + 225) && e.Y > (271 + 150)) // wybrany poziom 5
                    {
                        flagSelected = true;
                        gStatus.level = 5;
                    }

                    if (flagSelected)
                    {
                        gStatus.resetPoints();
                        GPars.pause = false;
                        GPars.end = false;
                        GPars.lvlEnd = false;
                        GPars.gLevelSel = false;
                        GPars.gStarted = true;
                        initLevel();
                        GPars.stopwatch.Reset();
                        GPars.stopwatch.Start();
                        Invalidate();
                    }
                }

                if (!GPars.pause && !GPars.gMenu && GPars.lvlEnd )
                {
                    if (e.X > 700 && e.X < 935 && e.Y < 595 && e.Y > 555)
                    {
                        gStatus.nextLevel();
                        if (!GPars.end)
                        {
                            GPars.lvlEnd = false;
                            initLevel();
                            GPars.stopwatch.Reset();
                            GPars.stopwatch.Start();
                        }
                        Invalidate();
                    }
                }

                if (!GPars.gMenu || !GPars.pause)
                {
                    if (e.X > 820 && e.X < 975 && e.Y > 30 && e.Y < 80)
                    {
                        GPars.stopwatch.Stop();
                        GPars.pause = true;
                        GPars.gMenu = true;
                        GPars.gLevelSel = false;
                        GPars.gInformation = false;
                        Invalidate();
                    }
                }
            }
        } // koniec OnMouseDown


        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!GPars.pause && !GPars.end && !GPars.lvlEnd)
            {
                if (e.Button == MouseButtons.Left) // poruszanie klockiem w poziomie
                {
                    for (int i = 0; i < GPars.noOfObjects; i++)
                    {
                        if (MouseDownLocation.X > blocks[i].rec.Left && MouseDownLocation.X < (blocks[i].rec.Left + blocks[i].width) && MouseDownLocation.Y > blocks[i].rec.Top && MouseDownLocation.Y < (blocks[i].rec.Top + blocks[i].height))
                        {
                            if (blocks[i].type == 3 || blocks[i].type == 2)
                            {
                                if (blocks[i].MoveCheck(i, (e.X - MouseDownLocation.X), 0)) // sprawdzenie mozliwosci wykonania ruchu
                                {
                                    blocks[i].rec.Location = new Point((e.X - MouseDownLocation.X) + blocks[i].rec.Left, blocks[i].rec.Top);
                                    MouseDownLocation = e.Location;
                                    ExitCheck();
                                    Invalidate();
                                }
                            }
                        }
                    }
                }
                else if (e.Button == MouseButtons.Right) // poruszanie klockiem w pionie
                {
                    for (int i = 0; i < GPars.noOfObjects; i++)
                    {
                        if (MouseDownLocation.X > blocks[i].rec.Left && MouseDownLocation.X < (blocks[i].rec.Left + blocks[i].width) && MouseDownLocation.Y > blocks[i].rec.Top && MouseDownLocation.Y < (blocks[i].rec.Top + blocks[i].height))
                        {

                            if (blocks[i].type == 3 || blocks[i].type == 1)
                            {
                                if (blocks[i].MoveCheck(i, 0, (e.Y - MouseDownLocation.Y))) // sprawdzenie mozliwosci wykonania ruchu
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
            }

        }// koniec OnMouseMove



        private void ExitCheck()
        {
            if (blocks[0].rec.IntersectsWith(exit1Rec) || blocks[0].rec.IntersectsWith(exit2Rec))
            {
                GPars.stopwatch.Stop();
                gStatus.saveLevelPassTime();
                GPars.lvlEnd = true;
                if (GPars.questionAns == 1 && blocks[0].rec.IntersectsWith(exit1Rec) )
                {
                    GPars.rightAns = true;
                    gStatus.points += 1;
                }
                else if (GPars.questionAns == 2 && blocks[0].rec.IntersectsWith(exit2Rec) )
                {
                    GPars.rightAns = true;
                    gStatus.points += 1;
                }
                else GPars.rightAns = false;

                if(gStatus.level==GPars.noOfLevels) GPars.end = true;
            }

        } //koniec ExitCheck

        private void restartGame() 
        {
            gStatus.reset();
            GPars.stopwatch.Stop();
            GPars.stopwatch.Restart();
            GPars.pause = true;
            GPars.end = false;
            GPars.lvlEnd = false;
            GPars.gStarted = false;
            GPars.gInformation = false;
            GPars.gLevelSel = false;
            GPars.gMenu = true;
            blocks[0] = new Block(1, 1, 75, 75, 3, GPars.blocksImages[0]); // 75x75 --> 1x1
            blocks[1] = new Block(1, 1, 160, 75, 2, GPars.blocksImages[4]); // 160x75 --> 2x1
            blocks[2] = new Block(1, 1, 160, 75, 2, GPars.blocksImages[4]); // 160x75 --> 2x1
            blocks[3] = new Block(1, 1, 75, 160, 1, GPars.blocksImages[1]); // 75x160 --> 1x2
            blocks[4] = new Block(1, 1, 75, 245, 1, GPars.blocksImages[2]); // 75x245 --> 1x3
            blocks[5] = new Block(1, 1, 160, 75, 2, GPars.blocksImages[4]); // 160x75 --> 2x1
            blocks[6] = new Block(1, 1, 330, 75, 2, GPars.blocksImages[6]); // 330x75 --> 4x1
            blocks[7] = new Block(1, 1, 160, 75, 2, GPars.blocksImages[4]); // 160x75 --> 2x1
            blocks[8] = new Block(1, 1, 75, 245, 1, GPars.blocksImages[2]); // 75x245 --> 1x3
            blocks[9] = new Block(1, 1, 75, 245, 1, GPars.blocksImages[2]); // 75x245 --> 1x3
            blocks[10] = new Block(1, 1, 245, 75, 2, GPars.blocksImages[5]); // 245x75 --> 3x1
            blocks[11] = new Block(1, 1, 245, 75, 2, GPars.blocksImages[5]); // 245x75 --> 3x1
            initLevel();
        }//koniec restartGame()

        private void initLevel() //funkcja do ustawiania polozenia klockow na planszy
        {

            if (gStatus.level == 1 && !GPars.end)
            {
                blocks[0].rec.Location = new Point(labirynthRec.X + 170, labirynthRec.Y + 170);
                blocks[1].rec.Location = new Point(labirynthRec.X, labirynthRec.Y);
                blocks[2].rec.Location = new Point(labirynthRec.X + 85, labirynthRec.Y + 85);
                blocks[3].rec.Location = new Point(labirynthRec.X, labirynthRec.Y + 85);
                blocks[4].rec.Location = new Point(labirynthRec.X + 340, labirynthRec.Y);
                blocks[5].rec.Location = new Point(labirynthRec.X, labirynthRec.Y + 255);
                blocks[6].rec.Location = new Point(labirynthRec.X, labirynthRec.Y + 340);
                blocks[7].rec.Location = new Point(labirynthRec.X + 255, labirynthRec.Y + 255);
                blocks[8].rec.Location = new Point(1500, 1500);
                blocks[9].rec.Location = new Point(1500, 1500);
                blocks[10].rec.Location = new Point(1500, 1500);
                blocks[11].rec.Location = new Point(1500, 1500);
                questionLabel.Text = "Pytanie nr 1: \n\nJaki ładunek elektryczny ma elektron?\n1) neutralny\n2) ujemny";
                GPars.questionAns = 2;
            }

            else if (gStatus.level == 2 && !GPars.end)
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
                questionLabel.Text = "Pytanie nr 2: \n\nKto jest autorem wiersza pt. \"Nic dwa razy\"?\n1) Wisława Szymborska\n2) Sanah";
                GPars.questionAns = 1;
            }
           else if (gStatus.level == 3 && !GPars.end)
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
                questionLabel.Text = "Pytanie nr 3: \n\nCo trafia do niebieskiego pojemnika na śmieci?\n1) Plastik\n2) Papier";
                GPars.questionAns = 2;
            }
            else if (gStatus.level == 4 && !GPars.end)
            {

                blocks[0].rec.Location = new Point(labirynthRec.X + 170, labirynthRec.Y + 170);
                blocks[1].rec.Location = new Point(labirynthRec.X, labirynthRec.Y + 170);
                blocks[2].rec.Location = new Point(1500, 1500);
                blocks[3].rec.Location = new Point(labirynthRec.X, labirynthRec.Y + 255);
                blocks[4].rec.Location = new Point(labirynthRec.X + 340, labirynthRec.Y);
                blocks[5].rec.Location = new Point(1500, 1500);
                blocks[6].rec.Location = new Point(labirynthRec.X, labirynthRec.Y + 85);
                blocks[7].rec.Location = new Point(1500, 1500);
                blocks[8].rec.Location = new Point(1500, 1500);
                blocks[9].rec.Location = new Point(1500, 1500);
                blocks[10].rec.Location = new Point(labirynthRec.X + 170, labirynthRec.Y + 340);
                blocks[11].rec.Location = new Point(1500, 1500);
                questionLabel.Text = "Pytanie nr 4: \n\nZ jakich pierwiastków składa się woda?\n1) Węgla i tlenu\n2) Wodoru i tlenu";
                GPars.questionAns = 2;

            }
            else if (gStatus.level == 5 && !GPars.end)
            {
                blocks[0].rec.Location = new Point(labirynthRec.X + 170, labirynthRec.Y + 170);
                blocks[1].rec.Location = new Point(labirynthRec.X, labirynthRec.Y);
                blocks[2].rec.Location = new Point(labirynthRec.X + 170, labirynthRec.Y + 255);
                blocks[3].rec.Location = new Point(labirynthRec.X + 255, labirynthRec.Y);
                blocks[4].rec.Location = new Point(labirynthRec.X, labirynthRec.Y + 85);
                blocks[5].rec.Location = new Point(1500, 1500);
                blocks[6].rec.Location = new Point(1500, 1500);
                blocks[7].rec.Location = new Point(1500, 1500);
                blocks[8].rec.Location = new Point(labirynthRec.X + 85, labirynthRec.Y + 170);
                blocks[9].rec.Location = new Point(labirynthRec.X + 340, labirynthRec.Y + 85);
                blocks[10].rec.Location = new Point(labirynthRec.X + 170, labirynthRec.Y + 340);
                blocks[11].rec.Location = new Point(1500, 1500);
                questionLabel.Text = "Pytanie nr 5: \n\nKto postawił pierwszy krok na księżycu?\n1) Neil Armstrong \n2) Louis Armstrong";
                GPars.questionAns = 1;
            }


        }//koniec initLevel()


    }


}
