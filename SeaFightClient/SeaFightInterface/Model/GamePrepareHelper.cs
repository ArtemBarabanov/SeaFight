using SeaFightInterface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFightInterface.Presenter
{
    public enum ShipOrientation
    {
        Horizontal,
        Vertical
    }

    class GamePrepareHelper
    {
        GameState Game;
        List<(int, int)> tempShip;
        int currentShipDecks;
        ShipOrientation currentOrientation;
        Random rand = new Random();

        int OneDeckCount;
        int TwoDeckCount;
        int ThreeDeckCount;
        int FourDeckCount;
        public GamePrepareHelper(IView _iView, GameState game) 
        {
            Game = game;
            OneDeckCount = game.OneDeckCount;
            TwoDeckCount = game.TwoDeckCount;
            ThreeDeckCount = game.ThreeDeckCount;
            FourDeckCount = game.FourDeckCount;
        }

        public event Action<List<(int, int)>> BackTransperent;
        public event Action<List<(int, int)>> ChangeColorGood;
        public event Action<List<(int, int)>> ChangeColorBad;
        public event Action<int> ChangeOneDeckCount;
        public event Action<int> ChangeTwoDeckCount;
        public event Action<int> ChangeThreeDeckCount;
        public event Action<int> ChangeFourDeckCount;
        public event Action<int> ShipTypeChanged;
        public event Action ShowStartButton;
        public event Action HideStartButton;
        public event Action ArrowVertical;
        public event Action ArrowHorizontal;
        public event Action<List<(int, int)>> PlacePlayerShip;
        #if DEBUG
        public event Action<List<(int, int)>> PlaceCompShip;
        #endif

        public void MouseOut(GameState game) 
        {
            if (tempShip != null && CheckOccupation(tempShip, game.GetPlayerSea()))
            {
                   BackTransperent(tempShip);
            }
        }

        public void MouseIn(GameState game, int x, int y) 
        {
            if (tempShip != null)
            {
                MoveShip(x, y);

                if (CheckBoundaries(tempShip) && CheckOccupation(tempShip, game.GetPlayerSea()))
                {
                    if (CheckCollisions(tempShip, game.GetPlayerSea()))
                    {
                        ChangeColorGood(tempShip);
                    }
                    else
                    {
                        ChangeColorBad(tempShip);
                    }
                }
                else
                {
                    BackTransperent(tempShip);
                }
            }
        }

        private void MoveShip(int x, int y) 
        {
            if (tempShip.Count == 0)
            {
                CreateTempShip(x, y);
            }
            else
            {
                if (currentOrientation == ShipOrientation.Horizontal)
                {
                    for (int i = 0; i < tempShip.Count; i++)
                    {
                        tempShip[i] = (x + i, y);
                    }
                }
                else
                {
                    for (int i = 0; i < tempShip.Count; i++)
                    {
                        tempShip[i] = (x, y + i);
                    }
                }
            }
        }

        public void PlayerFieldClick(GameState gameState, int x, int y) 
        {
            if (!gameState.GetPlayerSea()[x, y].IsOccupied)
            {
                if (currentShipDecks != 0 && tempShip.Count != 0 && CheckCollisions(tempShip, gameState.GetPlayerSea()))
                {
                    if (currentShipDecks == 1 && OneDeckCount > 0)
                    {
                        OneDeck oneDeck = new OneDeck();
                        gameState.GetPlayerShips().Add(oneDeck);
                        OneDeckCount--;
                        ChangeOneDeckCount(OneDeckCount);
                        oneDeck.CreateShip(tempShip, gameState.GetPlayerSea());
                        PlacePlayerShip(tempShip);
                        tempShip.Clear();
                    }
                    else if (currentShipDecks == 2 && TwoDeckCount > 0)
                    {
                        TwoDeck twoDeck = new TwoDeck(currentOrientation);
                        gameState.GetPlayerShips().Add(twoDeck);
                        TwoDeckCount--;
                        ChangeTwoDeckCount(TwoDeckCount);
                        twoDeck.CreateShip(tempShip, gameState.GetPlayerSea());
                        PlacePlayerShip(tempShip);
                        tempShip.Clear();
                    }
                    else if (currentShipDecks == 3 && ThreeDeckCount > 0)
                    {
                        ThreeDeck threeDeck = new ThreeDeck(currentOrientation);
                        gameState.GetPlayerShips().Add(threeDeck);
                        ThreeDeckCount--;
                        ChangeThreeDeckCount(ThreeDeckCount);
                        threeDeck.CreateShip(tempShip, gameState.GetPlayerSea());
                        PlacePlayerShip(tempShip);
                        tempShip.Clear();
                    }
                    else if (currentShipDecks == 4 && FourDeckCount > 0)
                    {
                        FourDeck fourDeck = new FourDeck(currentOrientation);
                        gameState.GetPlayerShips().Add(fourDeck);
                        FourDeckCount--;
                        ChangeFourDeckCount(FourDeckCount);
                        fourDeck.CreateShip(tempShip, gameState.GetPlayerSea());
                        PlacePlayerShip(tempShip);
                        tempShip.Clear();
                    }
                }
            }
            else
            {
                List<(int, int)> mas = new List<(int, int)>();
                var ship = (from s in gameState.GetPlayerShips() from d in s.decks where d.X == x && d.Y == y select s).ToList();

                if (ship != null)
                {
                    if (ship[0].DeckNumber == 1)
                    {
                        OneDeckCount++;
                        ChangeOneDeckCount(OneDeckCount);
                    }
                    else if (ship[0].DeckNumber == 2)
                    {
                        TwoDeckCount++;
                        ChangeTwoDeckCount(TwoDeckCount);
                    }
                    else if (ship[0].DeckNumber == 3)
                    {
                        ThreeDeckCount++;
                        ChangeThreeDeckCount(ThreeDeckCount);
                    }
                    else if (ship[0].DeckNumber == 4)
                    {
                        FourDeckCount++;
                        ChangeFourDeckCount(FourDeckCount);
                    }
                    gameState.GetPlayerShips().Remove(ship[0]);
                    ship[0].DeleteShip(gameState.GetPlayerSea());

                    foreach (Deck deck in ship[0].decks)
                    {
                        mas.Add((deck.X, deck.Y));
                    }

                    ShipTypeChanged(ship[0].DeckNumber);
                    tempShip.Clear();
                    foreach (Deck d in ship[0].decks)
                    {
                        tempShip.Add((d.X, d.Y));
                    }
                    currentShipDecks = ship[0].DeckNumber;
                    BackTransperent(mas);
                }
                else
                {
                    foreach (Deck deck in ship[0].decks)
                    {
                        mas.Add((deck.X, deck.Y));
                    }

                    ship[0].IsChosen = false;
                    PlacePlayerShip(mas);
                }
            }

            if (gameState.GetPlayerShips().Count == 10)
            {
                ShowStartButton();
            }
            else
            {
                HideStartButton();
            }
        }

        /// <summary>
        /// Определение границ поля
        /// </summary>
        /// <param name="tempShip"></param>
        /// <returns></returns>
        private bool CheckBoundaries(List<(int, int)> tempShip)
        {
            for (int i = 0; i < tempShip.Count; i++)
            {
                if ((tempShip[i].Item1 > 9) || (tempShip[i].Item1 == -1) || (tempShip[i].Item2 > 9) || (tempShip[i].Item2 == -1))
                {
                    if (tempShip.Count != 0)
                    {
                        tempShip.Clear();
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Определение касаний с другими кораблями
        /// </summary>
        /// <param name="tempShip"></param>
        /// <param name="sea"></param>
        /// <returns></returns>
        private bool CheckCollisions(List<(int, int)> tempShip, SeaCell[,] sea)
        {
            for (int i = 0; i < tempShip.Count; i++)
            {
                int x = tempShip[i].Item1;
                int y = tempShip[i].Item2;

                if ((x < 10) && (x > -1) && (y < 10) && (y > -1)
                    && sea[x, y].BorderCount > 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Проверка поля на занятость
        /// </summary>
        /// <param name="tempShip"></param>
        /// <param name="sea"></param>
        /// <returns></returns>
        private bool CheckOccupation(List<(int, int)> tempShip, SeaCell[,] sea)
        {
            for (int i = 0; i < tempShip.Count; i++)
            {
                int x = tempShip[i].Item1;
                int y = tempShip[i].Item2;

                if ((x < 10) && (x > -1) && (y < 10) && (y > -1)
                    && sea[x, y].IsOccupied)
                {
                    tempShip.Clear();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Автоматическое размещение кораблей
        /// </summary>
        /// <param name="ships"></param>
        /// <param name="sea"></param>
        private void AutoShipPosition(List<Ship> ships, SeaCell[,] sea)
        {
            for (int i = 0; i < 1;)
            {
                ShipOrientation orientation;
                int X = rand.Next(0, 10);
                int Y = rand.Next(0, 10);
                if (rand.Next(1, 3) == 1)
                {
                    orientation = ShipOrientation.Horizontal;
                    tempShip = new List<(int, int)> { (X, Y), (X + 1, Y), (X + 2, Y), (X + 3, Y) };
                }
                else
                {
                    orientation = ShipOrientation.Vertical;
                    tempShip = new List<(int, int)> { (X, Y), (X, Y + 1), (X, Y + 2), (X, Y + 3) };
                }

                if (CheckBoundaries(tempShip) && CheckOccupation(tempShip, sea))
                {
                    if (CheckCollisions(tempShip, sea))
                    {
                        FourDeck fourDeck = new FourDeck(orientation);
                        fourDeck.CreateShip(tempShip, sea);
                        ships.Add(fourDeck);
                        i++;
                    }
                }
            }
            for (int i = 0; i < 2;)
            {
                ShipOrientation orientation;
                int X = rand.Next(0, 10);
                int Y = rand.Next(0, 10);
                if (rand.Next(1, 3) == 1)
                {
                    orientation = ShipOrientation.Horizontal;
                    tempShip = new List<(int, int)> { (X, Y), (X + 1, Y), (X + 2, Y) };
                }
                else
                {
                    orientation = ShipOrientation.Vertical;
                    tempShip = new List<(int, int)> { (X, Y), (X, Y + 1), (X, Y + 2) };
                }

                if (CheckBoundaries(tempShip) && CheckOccupation(tempShip, sea))
                {
                    if (CheckCollisions(tempShip, sea))
                    {
                        ThreeDeck threeDeck = new ThreeDeck(orientation);
                        threeDeck.CreateShip(tempShip, sea);
                        ships.Add(threeDeck);
                        i++;
                    }
                }
            }
            for (int i = 0; i < 3;)
            {
                ShipOrientation orientation;
                int X = rand.Next(0, 10);
                int Y = rand.Next(0, 10);
                if (rand.Next(1, 3) == 1)
                {
                    orientation = ShipOrientation.Horizontal;
                    tempShip = new List<(int, int)> { (X, Y), (X + 1, Y) };
                }
                else
                {
                    orientation = ShipOrientation.Vertical;
                    tempShip = new List<(int, int)> { (X, Y), (X, Y + 1) };
                }

                if (CheckBoundaries(tempShip) && CheckOccupation(tempShip, sea))
                {
                    if (CheckCollisions(tempShip, sea))
                    {
                        TwoDeck twoDeck = new TwoDeck(orientation);
                        twoDeck.CreateShip(tempShip, sea);
                        ships.Add(twoDeck);
                        i++;
                    }
                }
            }
            for (int i = 0; i < 4;)
            {
                int X = rand.Next(0, 10);
                int Y = rand.Next(0, 10);
                tempShip = new List<(int, int)> { (X, Y) };
                if (CheckBoundaries(tempShip) && CheckOccupation(tempShip, sea))
                {
                    if (CheckCollisions(tempShip, sea))
                    {
                        OneDeck oneDeck = new OneDeck();
                        oneDeck.CreateShip(tempShip, sea);
                        ships.Add(oneDeck);
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// Изменение направления корабля
        /// </summary>
        public void ChangeShipOrientation() 
        {
            if (currentOrientation == ShipOrientation.Horizontal)
            {
                currentOrientation = ShipOrientation.Vertical;
                ArrowVertical();
            }
            else
            {
                currentOrientation = ShipOrientation.Horizontal;
                ArrowHorizontal();
            }
        }

        /// <summary>
        /// Выбор типа корабля
        /// </summary>
        /// <param name="decks"></param>
        public void ChangeShipType(int decks) 
        {
            currentShipDecks = decks;
            CreateTempShip(0, 0);
        }

        private void CreateTempShip(int x, int y) 
        {
            tempShip = new List<(int, int)>(currentShipDecks);
            if (currentOrientation == ShipOrientation.Horizontal)
            {
                for (int i = 0; i < currentShipDecks; i++)
                {
                    tempShip.Add((x + i, y));
                }
            }
            else 
            {
                for (int i = 0; i < currentShipDecks; i++)
                {
                    tempShip.Add((x, y + i));
                }
            }
        }

        public void AutoPlayerShips(GameState game) 
        {
            AllShipsDelete(game.GetPlayerShips(), game);
            AutoShipPosition(game.GetPlayerShips(), game.GetPlayerSea());
            foreach (Ship s in game.GetPlayerShips())
            {
                PlacePlayerShip(s.decks.Select(d => (d.X, d.Y)).ToList());
            }

            OneDeckCount = 0;
            TwoDeckCount = 0;
            ThreeDeckCount = 0;
            FourDeckCount = 0;
            ChangeOneDeckCount(OneDeckCount);
            ChangeTwoDeckCount(TwoDeckCount);
            ChangeThreeDeckCount(ThreeDeckCount);
            ChangeFourDeckCount(FourDeckCount);
            ShowStartButton();
        }

        public void AutoOpponentShips(GameState game)
        {
            AutoShipPosition(game.GetOpponentShips(), game.GetOpponentSea());
            #if DEBUG
            foreach (Ship s in game.GetOpponentShips())
            {
                PlaceCompShip(s.decks.Select(d => (d.X, d.Y)).ToList());
            }
            #endif
        }

        //Удаление всех кораблей при автогенерации
        private void AllShipsDelete(List<Ship> PlayerShips, GameState gameState)
        {
            if (PlayerShips.Count != 0)
            {
                for (int i = 0; i < PlayerShips.Count;)
                {
                    PlayerShips[i].DeleteShip(gameState.GetPlayerSea());
                    BackTransperent(PlayerShips[i].decks.Select(d => (d.X, d.Y)).ToList());
                    PlayerShips.Remove(PlayerShips[i]);
                }
            }
        }

        //Удаление одного корабля при повторном клике на нем
        private void ShipDeleteEvent(List<Ship> PlayerShips, GameState gameState)
        {
            Ship ship = PlayerShips.Find(s => s.IsChosen);

            if (ship != null)
            {
                if (ship.DeckNumber == 1)
                {
                    OneDeckCount++;
                    ChangeOneDeckCount(OneDeckCount);
                }
                else if (ship.DeckNumber == 2)
                {
                    TwoDeckCount++;
                    ChangeTwoDeckCount(TwoDeckCount);
                }
                else if (ship.DeckNumber == 3)
                {
                    ThreeDeckCount++;
                    ChangeThreeDeckCount(ThreeDeckCount);
                }
                else if (ship.DeckNumber == 4)
                {
                    FourDeckCount++;
                    ChangeFourDeckCount(FourDeckCount);
                }
                PlayerShips.Remove(ship);
                ship.DeleteShip(gameState.GetPlayerSea());
                BackTransperent(ship.decks.Select(d => (d.X, d.Y)).ToList());
            }
            HideStartButton();
        }
    }
}
