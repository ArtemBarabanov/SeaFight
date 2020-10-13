using SeaFightInterface.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaFightInterface
{
    public partial class GameForm
    {
        MyPictureBox[,] PlayerPctrMas;
        MyPictureBox[,] OpponentPctrMas;
        RadioPictureBox[] RadioPctr = new RadioPictureBox[4];
        PictureBox OrientationPctr;
        PictureBox OrientationSign;
        PictureBox One;
        PictureBox Two;
        PictureBox Three;
        PictureBox Four;
        PictureBox CompOne;
        PictureBox CompTwo;
        PictureBox CompThree;
        PictureBox CompFour;
        PictureBox oneDeck;
        PictureBox twoDeck;
        PictureBox threeDeck;
        PictureBox fourDeck;
        PictureBox winPctr;
        PictureBox Turn;

        /// <summary>
        /// Создание начального игрового меню
        /// </summary>
        private void CreateGameMenu()
        {
            CreateShipChoice();
            CreateDirectionButton();
            CreatePlacementCounters();
        }

        /// <summary>
        /// Создание счетчиков размещения кораблей игрока
        /// </summary>
        private void CreatePlacementCounters()
        {
            One = new PictureBox();
            One.Location = new Point(300, 500);
            One.Size = new Size(60, 40);
            One.BackColor = Color.Transparent;
            One.Image = Properties.Resources.X4;
            Controls.Add(One);
            Two = new PictureBox();
            Two.Location = new Point(300, 550);
            Two.Size = new Size(60, 40);
            Two.BackColor = Color.Transparent;
            Two.Image = Properties.Resources.X3;
            Controls.Add(Two);
            Three = new PictureBox();
            Three.Location = new Point(300, 600);
            Three.Size = new Size(60, 40);
            Three.BackColor = Color.Transparent;
            Three.Image = Properties.Resources.X2;
            Controls.Add(Three);
            Four = new PictureBox();
            Four.Location = new Point(300, 650);
            Four.Size = new Size(60, 40);
            Four.BackColor = Color.Transparent;
            Four.Image = Properties.Resources.X1;
            Controls.Add(Four);
        }

        /// <summary>
        /// Создание кнопки выбора направления корабля
        /// </summary>
        private void CreateDirectionButton()
        {
            OrientationPctr = new PictureBox();
            OrientationPctr.Location = new Point(400, 500);
            OrientationPctr.Size = new Size(100, 100);
            OrientationPctr.BackColor = Color.Transparent;
            OrientationPctr.Image = Properties.Resources.Wheel0;
            OrientationPctr.Click += OrientationPctr_Click;
            Controls.Add(OrientationPctr);

            OrientationSign = new PictureBox();
            OrientationSign.Location = new Point(390, 600);
            OrientationSign.Size = new Size(130, 30);
            OrientationSign.BackColor = Color.Transparent;
            OrientationSign.Image = Properties.Resources.SignDirection;
            Controls.Add(OrientationSign);
        }

        /// <summary>
        /// Создание меню выбора кораблей
        /// </summary>
        private void CreateShipChoice()
        {
            int X = 80;
            int Y = 500;

            for (int i = 0; i < 4; i++)
            {
                RadioPctr[i] = new RadioPictureBox();
                RadioPctr[i].Location = new Point(X, Y);
                RadioPctr[i].Decks = i + 1;
                RadioPctr[i].Click += RadioPctr_Click;
                if (i == 0)
                {
                    RadioPctr[i].Image = Properties.Resources._1Deck;
                    RadioPctr[i].Size = new Size(40, 40);
                }
                else if (i == 1)
                {
                    RadioPctr[i].Image = Properties.Resources._2Deck;
                    RadioPctr[i].Size = new Size(80, 40);
                }
                else if (i == 2)
                {
                    RadioPctr[i].Image = Properties.Resources._3Deck;
                    RadioPctr[i].Size = new Size(120, 40);
                }
                else if (i == 3)
                {
                    RadioPctr[i].Image = Properties.Resources._4Deck;
                    RadioPctr[i].Size = new Size(160, 40);
                }

                Controls.Add(RadioPctr[i]);

                Y += 50;
            }
        }

        /// <summary>
        /// Создание клеточного поля компьютера и игрока
        /// </summary>
        private void CreateFields()
        {
            //Поле игрока
            PlayerPctrMas = new MyPictureBox[10, 10];

            int X = 80;
            int Y = 80;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    PlayerPctrMas[i, j] = new MyPictureBox();
                    PlayerPctrMas[i, j].Location = new Point(X, Y);
                    PlayerPctrMas[i, j].Size = new Size(40, 40);
                    PlayerPctrMas[i, j].PositionX = i;
                    PlayerPctrMas[i, j].PositionY = j;
                    PlayerPctrMas[i, j].Click += PlayerPctr_Click;
                    PlayerPctrMas[i, j].MouseEnter += Pctr_MouseEnter;
                    PlayerPctrMas[i, j].MouseLeave += Pctr_MouseLeave;
                    PlayerPctrMas[i, j].BackColor = Color.Transparent;
                    Controls.Add(PlayerPctrMas[i, j]);
                    Y += 40;
                }
                Y = 80;
                X += 40;
            }

            //Поле компьютера
            OpponentPctrMas = new MyPictureBox[10, 10];

            X = 760;
            Y = 80;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    OpponentPctrMas[i, j] = new MyPictureBox();
                    OpponentPctrMas[i, j].Location = new Point(X, Y);
                    OpponentPctrMas[i, j].Size = new Size(40, 40);
                    OpponentPctrMas[i, j].PositionX = i;
                    OpponentPctrMas[i, j].PositionY = j;
                    OpponentPctrMas[i, j].Enabled = false;
                    OpponentPctrMas[i, j].Click += CompPctr_Click;
                    OpponentPctrMas[i, j].BackColor = Color.Transparent;
                    Controls.Add(OpponentPctrMas[i, j]);
                    Y += 40;
                }
                Y = 80;
                X += 40;
            }
        }

        /// <summary>
        /// Создание Экрана победы (окончание игры)
        /// </summary>
        private void CreateVictoryScreen()
        {
            AbortGameScreen();
            winPctr = new PictureBox()
            {
                Location = new Point(350, 500),
                Size = new Size(600, 200),
                BackColor = Color.Transparent,
            };
            Controls.Add(winPctr);
        }

        private void AbortGameScreen() 
        {
            One?.Dispose();
            Two?.Dispose();
            Three?.Dispose();
            Four?.Dispose();
            CompOne?.Dispose();
            CompTwo?.Dispose();
            CompThree?.Dispose();
            CompFour?.Dispose();
            oneDeck?.Dispose();
            twoDeck?.Dispose();
            threeDeck?.Dispose();
            fourDeck?.Dispose();
            startButton?.Dispose();
            lazyPctr?.Dispose();
            OrientationPctr?.Dispose();
            OrientationSign?.Dispose();

            for (int i = 0; i < RadioPctr.Length; i++)
            {
                RadioPctr[i]?.Dispose();
                RadioPctr[i] = null;
            }

            One = null;
            Two = null;
            Three = null;
            Four = null;
            CompOne = null;
            CompTwo = null;
            CompThree = null;
            CompFour = null;
            oneDeck = null;
            twoDeck = null;
            threeDeck = null;
            fourDeck = null;
            startButton = null;
            lazyPctr = null;
            OrientationPctr = null;
            OrientationSign = null;
            BlockFields();
        }

        private void BlockFields() 
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    PlayerPctrMas[i, j].Enabled = false;
                    OpponentPctrMas[i, j].Enabled = false;
                }
            }
        }

        /// <summary>
        /// Создание Экрана игровой сессии (во время уже самой игры)
        /// </summary>
        private void CreateGameSessionScreen()
        {
            OrientationPctr.Dispose();
            OrientationSign.Dispose();
            lazyPctr.Dispose();
            startButton.Visible = false;
            lazyPctr.Click -= lazyPctr_Click;
            lazyPctr.MouseLeave -= lazyPctr_MouseLeave;
            lazyPctr.MouseEnter -= lazyPctr_MouseEnter;
            OrientationSign.Click -= OrientationPctr_Click;

            OrientationPctr = null;
            lazyPctr = null;
            OrientationSign = null;

            //Типы кораблей игрока
            for (int i = 0; i < RadioPctr.Length; i++)
            {
                if (RadioPctr[i] != null)
                {
                    RadioPctr[i].Image = (Image)imageResources.GetObject($@"_{i + 1}Deck");
                    RadioPctr[i].Enabled = false;
                }
            }

            //Счетчики кораблей игрока
            One.Image = Properties.Resources.X4;
            Two.Image = Properties.Resources.X3;
            Three.Image = Properties.Resources.X2;
            Four.Image = Properties.Resources.X1;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    OpponentPctrMas[i, j].Enabled = true;
                    PlayerPctrMas[i, j].Enabled = false;
                }
            }

            //Счетчики типов кораблей компьютера
            CompOne = new PictureBox();
            CompOne.Location = new Point(980, 500);
            CompOne.Size = new Size(60, 40);
            CompOne.BackColor = Color.Transparent;
            CompOne.Image = Properties.Resources.X4;
            Controls.Add(CompOne);
            CompTwo = new PictureBox();
            CompTwo.Location = new Point(980, 550);
            CompTwo.Size = new Size(60, 40);
            CompTwo.BackColor = Color.Transparent;
            CompTwo.Image = Properties.Resources.X3;
            Controls.Add(CompTwo);
            CompThree = new PictureBox();
            CompThree.Location = new Point(980, 600);
            CompThree.Size = new Size(60, 40);
            CompThree.BackColor = Color.Transparent;
            CompThree.Image = Properties.Resources.X2;
            Controls.Add(CompThree);
            CompFour = new PictureBox();
            CompFour.Location = new Point(980, 650);
            CompFour.Size = new Size(60, 40);
            CompFour.BackColor = Color.Transparent;
            CompFour.Image = Properties.Resources.X1;
            Controls.Add(CompFour);

            //Типы кораблей компьютера
            oneDeck = new PictureBox();
            oneDeck.Image = Properties.Resources._1Deck;
            oneDeck.BackColor = Color.Transparent;
            oneDeck.Location = new Point(760, 500);
            oneDeck.Size = new Size(40, 40);
            Controls.Add(oneDeck);
            twoDeck = new PictureBox();
            twoDeck.Image = Properties.Resources._2Deck;
            twoDeck.BackColor = Color.Transparent;
            twoDeck.Location = new Point(760, 550);
            twoDeck.Size = new Size(80, 40);
            Controls.Add(twoDeck);
            threeDeck = new PictureBox();
            threeDeck.Image = Properties.Resources._3Deck;
            threeDeck.BackColor = Color.Transparent;
            threeDeck.Location = new Point(760, 600);
            threeDeck.Size = new Size(120, 40);
            Controls.Add(threeDeck);
            fourDeck = new PictureBox();
            fourDeck.Image = Properties.Resources._4Deck;
            fourDeck.BackColor = Color.Transparent;
            fourDeck.Location = new Point(760, 650);
            Controls.Add(fourDeck);
            fourDeck.Size = new Size(160, 40);

            CreateTurnIcon();
        }

        private void CreateTurnIcon()
        {
            Turn = new PictureBox()
            {
                Size = new Size(100, 100),
                Location = new Point(565, 210),
                BackColor = Color.Transparent
            };
            Controls.Add(Turn);
        }

        Wave[] Waves = { 
            new Wave() { StartsWithX = 140, Y = 140, EndsWithX = 140 }, 
            new Wave() { StartsWithX = 371, Y = 140, EndsWithX = 389 }, 
            new Wave() { StartsWithX = 211, Y = 220, EndsWithX = 229 }, 
            new Wave() { StartsWithX = 451, Y = 220, EndsWithX = 469 }, 
            new Wave() { StartsWithX = 340, Y = 260, EndsWithX = 340 },
            new Wave() { StartsWithX = 180, Y = 340, EndsWithX = 180 },
            new Wave() { StartsWithX = 380, Y = 340, EndsWithX = 380 },
            new Wave() { StartsWithX = 291, Y = 420, EndsWithX = 309 },
            new Wave() { StartsWithX = 131, Y = 460, EndsWithX = 149 }, 
            new Wave() { StartsWithX = 451, Y = 460, EndsWithX = 469 },
            
            new Wave() { StartsWithX = 940, Y = 100, EndsWithX = 940 }, 
            new Wave() { StartsWithX = 1060, Y = 140, EndsWithX = 1060 }, 
            new Wave() { StartsWithX = 780, Y = 180, EndsWithX = 780 }, 
            new Wave() { StartsWithX = 891, Y = 180, EndsWithX = 909 }, 
            new Wave() { StartsWithX = 971, Y = 260, EndsWithX = 989 }, 
            new Wave() { StartsWithX = 820, Y = 300, EndsWithX = 820 }, 
            new Wave() { StartsWithX = 1100, Y = 340, EndsWithX = 1100 }, 
            new Wave() { StartsWithX = 891, Y = 380, EndsWithX = 909 }, 
            new Wave() { StartsWithX = 1051, Y = 420, EndsWithX = 1069 }, 
            new Wave() { StartsWithX = 820, Y = 460, EndsWithX = 820 } };

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            Pen WavePen = new Pen(Color.Blue, 3);
            Pen LinesPen = new Pen(Color.MediumPurple, 1);

            //Волны
            for (int i = 0; i < Waves.Length; i++)
            {
                gr.DrawLine(WavePen, Waves[i].StartsWithX, Waves[i].Y, Waves[i].EndsWithX, Waves[i].Y);
            }

            //Горизонтальные линии
            for (int i = 0; i < Width / 40; i++)
            {
                gr.DrawLine(LinesPen, i * 40, 0, i * 40, Height);
            }

            //Вертикальные линии
            for (int i = 0; i < Height / 40; i++)
            {
                gr.DrawLine(LinesPen, 0, i * 40, Width, i * 40);
            }
        }

        private void Waves_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < Waves.Length; i++)
            {
                if (Waves[i].GrowDirection == Direction.Grow)
                {
                    Waves[i].EndsWithX += 1;
                    Waves[i].StartsWithX -= 1;
                    if (Waves[i].EndsWithX - Waves[i].StartsWithX == 20)
                    {
                        Waves[i].GrowDirection = Direction.Shrink;
                    }
                }
                else
                {
                    Waves[i].EndsWithX -= 1;
                    Waves[i].StartsWithX += 1;
                    if (Waves[i].EndsWithX - Waves[i].StartsWithX == 0)
                    {
                        Waves[i].GrowDirection = Direction.Grow;
                    }
                }

            }
            Invalidate();
        }
    }
}
