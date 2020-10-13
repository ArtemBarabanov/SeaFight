using SeaFightInterface.Model;
using SeaFightInterface.Presenter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaFightInterface
{
    public partial class GameForm : Form, IView
    {
        ISound Sound;
        IAnimation Animation;
        ResourceManager imageResources;
        SynchronizationContext UIContext;

        public GameForm(ISound sound, IAnimation animation)
        {
            InitializeComponent();
            imageResources = new ResourceManager(typeof(Properties.Resources));
            UIContext = SynchronizationContext.Current;
            Sound = sound;
            Animation = animation;
            Sound.SeaAndGulls();
        }

        public event Action<int, int> PlayerFieldClickEvent;
        public event Action<int, int> MouseInEvent;
        public event Action MouseOutEvent;
        public event EventHandler ShipDirectionEvent;
        public event Action<int> ChangeShipTypeEvent;
        public event EventHandler StartGameEvent;
        public event Action<int, int> OpponentFieldClickEvent;
        public event Action AutoGenerateShipsEvent;
        public event Action AbortGameEvent;
        public event Action<int, int> MoveFinished;

        //Выстрел игрока
        public void PlayerTargetHit((int, int) destinationPoint)
        {          
            Task.Run(() =>
            {
                Invoke(new Action(() =>
                { OpponentPctrMas[destinationPoint.Item1, destinationPoint.Item2].Image = Properties.Resources.Палуба; }
                ));
                Sound.Explosion();
                Animation.Explosion(OpponentPctrMas[destinationPoint.Item1, destinationPoint.Item2]);
                Animation.StartSmoke(OpponentPctrMas[destinationPoint.Item1, destinationPoint.Item2]);
                MoveFinished(destinationPoint.Item1, destinationPoint.Item2);
            });
        }

        //Промах игрока
        public void PlayerTargetMiss((int, int) destinationPoint)
        {
            Task.Run(() =>
            {
                Sound.Pluk();
                Animation.WaterSplash(OpponentPctrMas[destinationPoint.Item1, destinationPoint.Item2]);
                Invoke(new Action(() =>
                {
                    OpponentPctrMas[destinationPoint.Item1, destinationPoint.Item2].Image = Properties.Resources.Флажок;
                }));
            });
        }

        //Выстрел оппонента
        public void OpponentTargetHit((int, int) destinationPoint)
        {
            Task.Run(() =>
            {
                Sound.Explosion();
                Animation.Explosion(PlayerPctrMas[destinationPoint.Item1, destinationPoint.Item2]);
                Animation.StartSmoke(PlayerPctrMas[destinationPoint.Item1, destinationPoint.Item2]);
                MoveFinished(destinationPoint.Item1, destinationPoint.Item2);
            });
        }

        //Промах оппонента
        public void OpponentTargetMiss((int, int) destinationPoint)
        {
            Task.Run(() =>
            {
                Sound.Pluk();
                Animation.WaterSplash(PlayerPctrMas[destinationPoint.Item1, destinationPoint.Item2]);
            });
        }

        private void CompPctr_Click(object sender, EventArgs e)
        {
            MyPictureBox pctr = sender as MyPictureBox;
            OpponentFieldClickEvent(pctr.PositionX, pctr.PositionY);
        }

        #region Перемещение курсора по полю игрока/клик по полю игрока
        private void Pctr_MouseLeave(object sender, EventArgs e)
        {
            MouseOutEvent();
        }

        private void Pctr_MouseEnter(object sender, EventArgs e)
        {
            MyPictureBox pctr = sender as MyPictureBox;
            MouseInEvent(pctr.PositionX, pctr.PositionY);
        }

        private void PlayerPctr_Click(object sender, EventArgs e)
        {
            MyPictureBox pctr = sender as MyPictureBox;
            PlayerFieldClickEvent(pctr.PositionX, pctr.PositionY);
        }

        public void BackTransperent(List<(int, int)> coordinates)
        {
            for (int i = 0; i < coordinates.Count; i++)
            {
                PlayerPctrMas[coordinates[i].Item1, coordinates[i].Item2].Image = null;
            }
        }

        public void ChangeColorBad(List<(int, int)> coordinates)
        {
            for (int i = 0; i < coordinates.Count; i++)
            {
                PlayerPctrMas[coordinates[i].Item1, coordinates[i].Item2].Image = Properties.Resources.BadPosition;
            }
        }

        public void ChangeColorGood(List<(int, int)> coordinates)
        {
            for (int i = 0; i < coordinates.Count; i++)
            {
                PlayerPctrMas[coordinates[i].Item1, coordinates[i].Item2].Image = Properties.Resources.GoodPosition;
            }
        }
        #endregion

        public void PlacePlayerShip(List<(int,int)> coordinates)
        {
            for (int i = 0; i < coordinates.Count; i++)
            {
                PlayerPctrMas[coordinates[i].Item1, coordinates[i].Item2].Image = Properties.Resources.Палуба;
            }
        }

        private void RadioPctr_Click(object sender, EventArgs e)
        {
            RadioPictureBox rPctr = sender as RadioPictureBox;
            rPctr.Checked = true;
            rPctr.Image = (Image)imageResources.GetObject($@"_{rPctr.Decks}Deckchosen");
            ChangeShipTypeEvent(rPctr.Decks);

            for (int i = 0; i < 4;  i++)
            {
                if (RadioPctr[i].Decks != rPctr.Decks)
                {
                    RadioPctr[i].Checked = false;
                    RadioPctr[i].Image = (Image)imageResources.GetObject($@"_{RadioPctr[i].Decks}Deck");
                }
            }
        }

        #region Смена направления корабля
        private void OrientationPctr_Click(object sender, EventArgs e)
        {
            ShipDirectionEvent(this, EventArgs.Empty);
        }

        public void ArrowHorizontal()
        {
            OrientationPctr.Image = new Bitmap(Properties.Resources.Wheel0);
        }

        public void ArrowVertical()
        {
            OrientationPctr.Image = new Bitmap(Properties.Resources.Wheel90);
        }
        #endregion

        //Изменение счетчика однопалубных кораблей
        public void ChangeOneDeckCount(int x)
        {
            One.Image = (Image)imageResources.GetObject($@"X{x}");
        }
        //Изменение счетчика двупалубных кораблей
        public void ChangeTwoDeckCount(int x)
        {
            Two.Image = (Image)imageResources.GetObject($@"X{x}");
        }
        //Изменение счетчика трехпалубных кораблей
        public void ChangeThreeDeckCount(int x)
        {
            Three.Image = (Image)imageResources.GetObject($@"X{x}");
        }
        //Изменение счетчика четырехпалубных кораблей
        public void ChangeFourDeckCount(int x)
        {
            Four.Image = (Image)imageResources.GetObject($@"X{x}");
        }

        //Начало игры
        private void startButton_Click(object sender, EventArgs e)
        {
            Sound.StopSeaAndGulls();
            Sound.Rynda();
            StartGameEvent(this, EventArgs.Empty);
        }

        //Отображение информации, кто ходит первым
        public void BeginGame(Participants who)
        {
            if (who == Participants.Opponent)
            {
                MessageBox.Show("Компьютер ходит первым!");
            }
            else
            {
                MessageBox.Show("Ты ходишь первым!");
            }
            CreateGameSessionScreen();
        }
        //Изменение счетчиков кораблей игрока во время боя
        public void DecreasePlayerShipCount(int deckNumber, int liveShips)
        {
            UIContext.Post(new SendOrPostCallback(o => { 
                if (deckNumber == 1)
                {
                    One.Image = (Image)imageResources.GetObject($@"X{liveShips}");
                }
                else if (deckNumber == 2)
                {
                    Two.Image = (Image)imageResources.GetObject($@"X{liveShips}");
                }
                else if (deckNumber == 3)
                {
                    Three.Image = (Image)imageResources.GetObject($@"X{liveShips}");
                }
                else if (deckNumber == 4)
                {
                    Four.Image = (Image)imageResources.GetObject($@"X{liveShips}");
                }
            }), null);
        }
        //Изменение счетчиков кораблей противника во время боя
        public void DecreaseOpponentShipCount(int deckNumber, int liveShips)
        {
            UIContext.Post(new SendOrPostCallback(o => {
                if (deckNumber == 1)
                {
                    CompOne.Image = (Image)imageResources.GetObject($@"X{liveShips}");
                }
                else if (deckNumber == 2)
                {
                    CompTwo.Image = (Image)imageResources.GetObject($@"X{liveShips}");
                }
                else if (deckNumber == 3)
                {
                    CompThree.Image = (Image)imageResources.GetObject($@"X{liveShips}");
                }
                else if (deckNumber == 4)
                {
                    CompFour.Image = (Image)imageResources.GetObject($@"X{liveShips}");
                }
            }), null);       
        }

        //Формирование экрана победы
        public void Victory(Participants who)
        {
            UIContext.Post(new SendOrPostCallback(o =>
            {
                CreateVictoryScreen();

                if (who == Participants.Opponent)
                {
                    winPctr.Image = Properties.Resources.Defeat;
                }
                else
                {
                    winPctr.Image = Properties.Resources.Victory;
                }
            }), null);
        }

        //Смена типа корабля при расстановке
        public void ChangeShipType(int decks)
        {
            for (int i = 0; i < 4; i++)
            {
                if (RadioPctr[i].Decks != decks)
                {
                    RadioPctr[i].Checked = false;
                    RadioPctr[i].Image = (Image)imageResources.GetObject($@"_{RadioPctr[i].Decks}Deck");
                }
                else
                {
                    RadioPctr[i].Checked = true;
                    RadioPctr[i].Image = (Image)imageResources.GetObject($@"_{RadioPctr[i].Decks}Deckchosen");
                }
            }
        }
        //Обработка нажатия на кнопку Лень
        private void lazyPctr_Click(object sender, EventArgs e)
        {
            AutoGenerateShipsEvent();
        }

        private void lazyPctr_MouseEnter(object sender, EventArgs e)
        {
            lazyPctr.Image = Properties.Resources.Лень2;
        }

        private void lazyPctr_MouseLeave(object sender, EventArgs e)
        {
            lazyPctr.Image = Properties.Resources.Лень1;
        }
        //Скрытие кнопки В бой
        public void HideStartButton()
        {
            startButton.Visible = false;
        }
        //Отображение кнопки В бой
        public void ShowStartButton()
        {
            startButton.Visible = true;
        }
        //Сворачивание окна
        private void minimizePctr_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        //Обработка закрытия формы
        private void closePctr_Click(object sender, EventArgs e)
        {
            StartForm Main = Owner as StartForm;
            Main?.Show();
            Sound.StopSeaAndGulls();           
            AbortGameEvent?.Invoke();
            Close();

        }
        //Маркировка уничтоженного корабля игрока
        public void MarkPlayerDestroyedShip(List<(int, int)> decks)
        {
            UIContext.Post(new SendOrPostCallback(o => {
            foreach ((int, int) coord in decks)
            {
                PlayerPctrMas[coord.Item1, coord.Item2].Image = Properties.Resources.destroyed;
            }
            }), null);
        }
        //Маркировка уничтоженного корабля противника
        public void MarkOpponentDestroyedShip(List<(int, int)> decks)
        {
            UIContext.Post(new SendOrPostCallback(o => {
            foreach ((int, int) coord in decks)
            {
                OpponentPctrMas[coord.Item1, coord.Item2].Image = Properties.Resources.destroyed;
            }
            }), null);
        }
        //Начало сетевой игры
        public void BeginNetGame(string whoIsFirst)
        {
            UIContext.Post(new SendOrPostCallback(o => {
                CreateGameSessionScreen(); MessageBox.Show($"Оба игрока готовы, игру можно начинать! Первым ходит {whoIsFirst}");
            }), null);
        }
        //Отображение хода игрока
        public void MarkMyTurn()
        {
            UIContext.Post(new SendOrPostCallback(o => {
                Turn.Image = Properties.Resources.myTurn;
            }), null);
        }
        //Отображение хода противника
        public void MarkOpponentTurn()
        {
            UIContext.Post(new SendOrPostCallback(o => {
                Turn.Image = Properties.Resources.opponentTurn;
            }), null);
        }
        //Победа игрока в сетевой игре
        public void NetMyVictory()
        {
            UIContext.Post(new SendOrPostCallback(o => {
                CreateVictoryScreen();
                winPctr.Image = Properties.Resources.Victory;
            }), null);
        }
        //Победа противника в сетевой игре
        public void NetOpponentVictory()
        {
            UIContext.Post(new SendOrPostCallback(o => {
                CreateVictoryScreen();
                winPctr.Image = Properties.Resources.Defeat;
            }), null);
        }
        //Прерывание сетевой игры одним из игроков
        public void NetOpponentAbortGame(string name)
        {
            UIContext.Post(new SendOrPostCallback(o => {
                AbortGameScreen();
                MessageBox.Show($"{name} покинул(-а) игру!");
            }), null);
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            CreateGameMenu();
            CreateFields();
        }

        private void closePctr_MouseEnter(object sender, EventArgs e)
        {
            closePctr.Image = Properties.Resources.CloseLight;
        }

        private void closePctr_MouseLeave(object sender, EventArgs e)
        {
            closePctr.Image = Properties.Resources.Close;
        }

        private void minimizePctr_MouseEnter(object sender, EventArgs e)
        {
            minimizePctr.Image = Properties.Resources.MinimizeLight;
        }

        private void minimizePctr_MouseLeave(object sender, EventArgs e)
        {
            minimizePctr.Image = Properties.Resources.Minimize;
        }

        private void startButton_MouseEnter(object sender, EventArgs e)
        {
            startButton.Image = Properties.Resources.FightLight;
        }

        private void startButton_MouseLeave(object sender, EventArgs e)
        {
            startButton.Image = Properties.Resources.Fight;
        }

#if DEBUG
        public void PlaceCompShip(List<(int, int)> coordinates)
        {
            for (int i = 0; i < coordinates.Count; i++)
            {
                OpponentPctrMas[coordinates[i].Item1, coordinates[i].Item2].Image = Properties.Resources.Палуба;
            }
        }
#endif
    }
}
