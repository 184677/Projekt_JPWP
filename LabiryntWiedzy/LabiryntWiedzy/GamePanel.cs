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
    /// <summary>
    /// Główny obszar graficzny gry,
    /// klasa dziedzicząca po klasie Panel
    /// </summary>
    public class GamePanel : Panel
    {
        /// <summary>
        /// Szerokość pola graficznego gry
        /// </summary>
        public int sWidth;
        /// <summary>
        /// Wysokość pola graficznego gry
        /// </summary>
        public int sHeight;
        /// <summary>
        /// Wysokość paska menu
        /// </summary>
        public int barHeight;
        /// <summary>
        /// Obiekt reprezentujący status gry
        /// </summary>
        public GameStatus gStatus;
        /// <summary>
        /// Rodzina stosowanych czcionek
        /// </summary>
        public FontFamily fontFamily;
        /// <summary>
        /// Czcionka stosowana w pasku Menu
        /// </summary>
        public Font menuFont;
        /// <summary>
        /// Czcionka stosowana w pasku Menu - wersja zmniejszona
        /// </summary>
        public Font menuFontv2;
        /// <summary>
        /// Czcionka stosowane do wypisania pytan
        /// </summary>
        public Font questionFont;
        /// <summary>
        /// Tablica obiektów pierwszego planu - klocki
        /// </summary>
        public static Block[] blocks;
        /// <summary>
        /// Prostokąt reprezentujący startowe menu
        /// </summary>
        public static Rectangle startMenuRec;
        /// <summary>
        /// Prostokąt reprezentujący labirynt
        /// </summary>
        public static Rectangle labirynthRec;
        /// <summary>
        /// Prostokąt reprezentujący panel z pytaniem
        /// </summary>
        public static Rectangle questionRec;
        /// <summary>
        /// Prostokąt reprezentujący pasek menu
        /// </summary>
        public static Rectangle barMenuRec;
        /// <summary>
        /// Prostokąt reprezentujący skrócony pasek menu
        /// </summary>
        public static Rectangle shortMenuRec;
        /// <summary>
        /// Prostokąt reprezentujący wyjście nr 1
        /// </summary>
        public static Rectangle exit1Rec;
        /// <summary>
        /// Prostokąt reprezentujący wyjście nr 2
        /// </summary>
        public static Rectangle exit2Rec;
        /// <summary>
        /// Obiekt do wyświetlania pytania
        /// </summary>
        public static Label questionLabel;
        /// <summary>
        /// Punkt do wyliczenia zmian w położeniu myszki 
        /// </summary>
        private Point MouseDownLocation;


        /// <summary>
        /// Konstruktor klasy pola graficznego gry, 
        /// ustawienia początkowe oraz ładowanie zasobów
        /// </summary>
        /// <param name="width">Szerokość pola graficznego gry</param>
        /// <param name="height">Wysokość pola graficznego gry</param>
        public GamePanel(int width, int height)
        {
            gStatus = new GameStatus();
            gStatus.reset(); 
            gStatus.deleteResults(); // usunięcie zapisanego pliku z wynikami

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
            sp.SoundLocation = "music/bg_music.wav";
            sp.PlayLooping();

            GPars.stopwatch = new Stopwatch();

            blocks = new Block[GPars.noOfObjects];
            restartGame();

        } // koniec GamePanel()


        /// <summary>
        /// Metoda odpowiedzielna za odrysowanie panelu, 
        /// wypełnienie w zależności od stanu gry i wybrania odpowiedniej pozycji w menu
        /// </summary>
        /// <param name="e">Argument obsługi zdarzenia rysowania</param>
        protected override void OnPaint( PaintEventArgs e)
        {
            Pen Whitepen = new Pen(Brushes.White); // Obiekt do rysowania obwódki w prostokątach z zaokrąglonymi rogami
            Whitepen.Width = 4.0F;
            Pen Redpen = new Pen(Brushes.Red); // Obiekt do rysowania linii wyjścia z labiryntu 
            Redpen.Width = 4.0F;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias; // Ustawienie trybu lepszej jakości grafiki

            if (gStatus.level==1) g.DrawImage(GPars.bgImages[0], new Point(0, 0)); // Rysowanie 1 tła dla 1 lvl
            else if (gStatus.level == 2 ) g.DrawImage(GPars.bgImages[1], new Point(0, 0)); // Rysowanie 2 tła dla 2 lvl
            else if (gStatus.level == 3) g.DrawImage(GPars.bgImages[2], new Point(0, 0)); // Rysowanie 3 tła dla 3 lvl
            else if (gStatus.level == 4) g.DrawImage(GPars.bgImages[3], new Point(0, 0)); // Rysowanie 4 tła dla 4 lvl
            else g.DrawImage(GPars.bgImages[4], new Point(0, 0)); // Rysowanie 5 tła dla 5 lvl

            g.DrawImage(GPars.logoImage, 860, 680); // Narysowanie loga

            if (GPars.pause) // Przerwa w grze
            {
                questionLabel.Visible = false; // Wyłączenie wyświetlania pytania

                if (GPars.gMenu) // Wybrano menu
                {
                    GraphicsPath startMenu = Block.RoundedRect(startMenuRec, 10); // Prostokąt startowego menu z zaokrąglonymi rogami
                    g.DrawPath(Whitepen, startMenu); // Narysowanie obwódki
                    g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), startMenu); // Wypełnienie
                    g.DrawString("Wybierz poziom", menuFont, Brushes.White, new Point(375, 305));
                    g.DrawString("O grze...", menuFont, Brushes.White, new Point(440, 390));
                    g.DrawString("Wyjdź z gry", menuFont, Brushes.White, new Point(400, 475));
                    questionLabel.Visible = false;

                    if (GPars.gStarted && !GPars.end) g.DrawString("Kontynuuj grę", menuFont, Brushes.White, new Point(390, 225)); // Jeśli zaczęto rozgrywke
                    else g.DrawString("Nowa gra", menuFont, Brushes.White, new Point(420, 225));
                }

                else if (GPars.gLevelSel) // Wybrano pozycje w menu "Wybierz poziom"
                {
                    GraphicsPath shortMenu = Block.RoundedRect(shortMenuRec, 10); // Prostokąt menu z zaokraglonymi rogami
                    g.DrawPath(Whitepen, shortMenu); // Narysowanie obwódki
                    g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), shortMenu); // Wypełnienie

                    Rectangle lvlMenuRec1 = new Rectangle(250, 121, 525, 75); // Prostokąt na którym wyświetlany jest napis "Wybierz poziom"
                    GraphicsPath lvlMenu1 = Block.RoundedRect(lvlMenuRec1, 10); // Prostokąt z zaokrąglonymi rogami
                    g.DrawPath(Whitepen, lvlMenu1); // Narysowanie obwódki
                    g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), lvlMenu1); // Wypełnienie

                    Rectangle lvlMenuRec2 = new Rectangle(250, 271 - 35, 525, 295); // Prostakąt na którym wyświetlane są przyciski do wyboru konkretnego poziomu
                    GraphicsPath lvlMenu2 = Block.RoundedRect(lvlMenuRec2, 10); // Prostokąt z zaokrąglonymi rogami
                    g.DrawPath(Whitepen, lvlMenu2); // Narysowanie obwódki
                    g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), lvlMenu2); // Wypłnienie

                    g.DrawString("Wybierz poziom:", menuFont, Brushes.White, new Point(250+120, 128));

                    g.DrawImage(GPars.lvlImages[0], 325, 271);
                    g.DrawImage(GPars.lvlImages[1], 325+150, 271);
                    g.DrawImage(GPars.lvlImages[2], 325+300, 271);
                    g.DrawImage(GPars.lvlImages[3], 325+75, 271+150);
                    g.DrawImage(GPars.lvlImages[4], 325+225, 271+150);

                    g.DrawImage(GPars.logoImage, 860, 680);// Narysowanie loga
                    g.DrawImage(GPars.menuImage, new Point(820, 20)); // Narysowanie ikony menu
                }
                else if (GPars.gInformation) // Wybrano pozycje w menu "O grze...."
                {
                    GraphicsPath shortMenu = Block.RoundedRect(shortMenuRec, 10); // Prostokąt menu z zaokraglonymi rogami
                    g.DrawPath(Whitepen, shortMenu); // Narysowanie obwódki
                    g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), shortMenu); // Wypełnienie

                    g.DrawImage(GPars.howToPlayImage, 0, 0); // Narysowanie grafiki z intrukcjami do gry

                    g.DrawImage(GPars.logoImage, 860, 680);// Narysowanie loga
                    g.DrawImage(GPars.menuImage, new Point(820, 20)); // Narysowanie ikony menu
                }
            }
            else // Rozpoczęto grę
            {
                GraphicsPath barMenu = Block.RoundedRect(barMenuRec, 10); // Pasek menu z zaokrąglonymi rogami
                g.DrawPath(Whitepen, barMenu); // Narysowanie obwódki
                g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), barMenu); // Wypełnienie

                g.DrawImage(GPars.menuImage, new Point(820, 20)); // Narysowanie ikony menu

                g.DrawString("POZIOM :", menuFont, Brushes.White, new Point(40, 23));
                g.DrawString("" + gStatus.level, menuFont, Brushes.White, new Point(185, 23)); // Wypisanie numeru aktualnego poziomu
                g.DrawString("PUNKTY :", menuFont, Brushes.White, new Point(300, 23));
                g.DrawString("" + gStatus.points, menuFont, Brushes.White, new Point(452, 23)); // Wypisanie liczby zdobytych punktów

                GraphicsPath labirynth = Block.RoundedRect(labirynthRec, 3); // Prostokąt labiryntu z zaokrąglonymi rogami
                g.DrawPath(Whitepen, labirynth); // Narysowanie obwódki labiryntu
                g.FillPath(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), labirynth); // Wypelnienie prostokąta labiryntu

                GraphicsPath question = Block.RoundedRect(questionRec, 10); // Prostokat z zaokrąglonymi rogami zawierający pytanie 
                g.DrawPath(Whitepen, question); // Narysowanie obwódki 
                g.FillPath(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), question); // Wypełnienie 

                questionLabel.Visible = true; // Włączenie wyświetlania pytania

                g.DrawImage(GPars.ans1Image, exit1Rec.Left, exit1Rec.Top); // Narysowanie wyjścia nr 1
                g.DrawImage(GPars.ans2Image, exit2Rec.Left, exit2Rec.Top); // Narysowanie wyjścia nr 2

                g.DrawLine(Redpen, new Point(labirynthRec.Right, labirynthRec.Top + 75), new Point(labirynthRec.Right, labirynthRec.Top + 170)); // Linia wyjścia nr 1
                g.DrawLine(Redpen, new Point(labirynthRec.Left, labirynthRec.Bottom - 170), new Point(labirynthRec.Left, labirynthRec.Bottom - 75)); // Linia wyjścia nr 2

                for (int i = 0; i < GPars.noOfObjects; i++) g.DrawImage(blocks[i].icon, blocks[i].rec.Left, blocks[i].rec.Top); // Rysowanie klocków

                if(GPars.lvlEnd) // Zakończono poziom
                {
                    GraphicsPath nextLvlPanel = Block.RoundedRect(new Rectangle(questionRec.Left, questionRec.Bottom + 50, questionRec.Width, labirynthRec.Bottom-questionRec.Bottom - 50), 10); // Prostokąt z zaokrąglonymi rogami do wyświetlania czy wybrano poprawne wyjście
                    g.DrawPath(Whitepen, nextLvlPanel); // Narysowanie obwódki
                    g.FillPath(new SolidBrush(Color.FromArgb(130, 0, 0, 0)), nextLvlPanel); // Wypełnienie

                    if (GPars.rightAns) g.DrawString("Dobra odpowiedź!", menuFontv2, Brushes.Lime, new Point(questionRec.Left + 30 , questionRec.Bottom + 70));
                    else g.DrawString("Zła odpowiedź!", menuFontv2, Brushes.Red, new Point(questionRec.Left + 45 , questionRec.Bottom + 70));

                    if (GPars.end) g.DrawString("Koniec Gry", menuFontv2, Brushes.White, new Point(questionRec.Left + 75, questionRec.Bottom + 135)); // Jeśli skończono grę
                    else g.DrawString("Następny poziom", menuFontv2, Brushes.White, new Point(questionRec.Left + 35, questionRec.Bottom + 135));

                }
       
            }
            Whitepen.Dispose();
            Redpen.Dispose();
        } // koniec OnPaint()


        /// <summary>
        /// Metoda odpowiedzialna za obsługę zdarzeń - wciśnięcie przycisku myszki
        /// umożliwia wybranie odpowiedniej pozycji menu, przejście do kolejnego poziomu i śledzi zmianę położenia myszki
        /// </summary>
        /// <param name="e">Argument obsługi zdarzenia przyciśnięcia przycisku myszki</param>
        protected override void OnMouseDown( MouseEventArgs e)
        {
            MouseDownLocation = e.Location;

            if (e.Button == MouseButtons.Left) // Wciśnięto lewy przycisk myszy
            {
                if (GPars.pause && GPars.gMenu) // Plansza z menu startowym
                {
                    if (e.X > 395 && e.X < 630 && e.Y > 225 && e.Y < 285) // Wybranie opcji "Nowa gra" w startowym menu
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
                    if (e.X > 375 && e.X < 650 && e.Y > 310 && e.Y < 365) // Wybranie opcji "Wybór poziomu" w startowym menu
                    {
                        GPars.gMenu = false;
                        GPars.gLevelSel = true;
                        Invalidate();
                    }
                    if (e.X > 440 && e.X < 590 && e.Y > 395 && e.Y < 450) // Wybranie opcji "O grze..." w startowym menu
                    {
                        GPars.gMenu = false;
                        GPars.gInformation = true;
                        Invalidate();
                    }
                    if (e.X > 400 && e.X < 615 && e.Y < 535 && e.Y > 480) // Wybranie opcji "Wyjdź z gry" w startowym menu
                    {
                        gStatus.saveResults();
                        Application.Exit();
                    }
                }

                else if(GPars.pause && !GPars.gMenu && GPars.gLevelSel) // Plansza z wybórem poziomu
                {
                    Boolean flagSelected = false; // Flaga sprawdzająca czy wybrano poziom
                    if (e.X > 325 && e.X < (325+75) && e.Y < (271 + 75) && e.Y > 271) // Wybrany poziom 1
                    {
                        flagSelected = true;
                        gStatus.level = 1;
                    }
                    else if (e.X > (325 + 150) && e.X < (325 + 225) && e.Y < (271 + 75) && e.Y > 271) // Wybrany poziom 2
                    {
                        flagSelected = true;
                        gStatus.level = 2;
                    } 
                    else if (e.X > (325 + 300) && e.X < (325 + 375) && e.Y < (271 + 75) && e.Y > 271) // Wybrany poziom 3
                    {
                        flagSelected = true;
                        gStatus.level = 3;
                    }
                    else if (e.X > (325 + 75) && e.X < (325 + 150) && e.Y < (271 + 225) && e.Y > (271 + 150)) // Wybrany poziom 4
                    {
                        flagSelected = true;
                        gStatus.level = 4;
                    }
                    else if (e.X > (325 + 225) && e.X < (325 + 300) && e.Y < (271 + 225) && e.Y > (271 + 150)) // Wybrany poziom 5
                    {
                        flagSelected = true;
                        gStatus.level = 5;
                    }

                    if (flagSelected) // Wybrano poziom
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

                if (!GPars.pause && !GPars.gMenu && GPars.lvlEnd ) // Wciśnięcie przycisku "Następny poziom"
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

                if (!GPars.gMenu || !GPars.pause) // Wyjście do startowego menu
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

        /// <summary>
        /// Metoda odpowiedzialna za poruszanie klockami za pomocą przesuwania myszki
        /// </summary>
        /// <param name="e">Argument obsługi zdarzenia przesunięcia myszki</param>
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


        /// <summary>
        /// Sprawdzenie czy czerwony klocek dotarł do jednego z 2 wyjść
        /// </summary>
        private void ExitCheck()
        {
            if (blocks[0].rec.IntersectsWith(exit1Rec) || blocks[0].rec.IntersectsWith(exit2Rec)) // Kolizja z jednym z 2 wyjść
            {
                GPars.stopwatch.Stop(); // Zatrzymanie odmierzania czasu
                gStatus.saveLevelPassTime(); // Zapisanie czasu przejścia poziomu
                GPars.lvlEnd = true;
                if (GPars.questionAns == 1 && blocks[0].rec.IntersectsWith(exit1Rec) ) // Kolzija z 1 wyjściem
                {
                    GPars.rightAns = true;
                    gStatus.points += 1;
                }
                else if (GPars.questionAns == 2 && blocks[0].rec.IntersectsWith(exit2Rec) ) // Kolizja z 2 wyjściem
                {
                    GPars.rightAns = true;
                    gStatus.points += 1;
                }
                else GPars.rightAns = false;

                if(gStatus.level==GPars.noOfLevels) GPars.end = true; // Jeśli brak kolejnych poziomów - koniec gry
            }

        } //koniec ExitCheck


        /// <summary>
        /// Restart gry - ustawienie parametrów oraz obiektów pierwszego planu
        /// </summary>
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

        /// <summary>
        /// Inicjalizajca poziomu - ustawienie położenia klocków w labiryncie i zmiana wyświetlanego pytania w zależności on numeru poziomu
        /// </summary>
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
