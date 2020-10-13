using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeaFightServer.Models
{
    public class GameSession
    {
        public string ID { get; private set; }
        public List<Player> Players { get; private set; }
        public string WhoFirstId { get; private set; }
        public string WhoFirstName { get; private set; }
        
        Random rand = new Random();

        bool isVictory;

        string Turn;
        public string VictoryId { get; private set; }
        
        public event EventHandler WinEvent;
        public event Action<string, string> MyHitEvent;
        public event Action<string, string> MyMissEvent;
        public event Action<string, string> OpponentHitEvent;
        public event Action<string, string> OpponentMissEvent;
        public event Action<string, string, string, string, string> MyShipDestroyedEvent;
        public event Action<string, string, string, string, string> OpponentShipDestroyedEvent;
        public event EventHandler EveryOneReadyEvent;

        public GameSession(string id, List<Player> players)
        {
            ID = id;
            Players = players;
            foreach (Player p in Players)
            {
                p.EnterGame();
            }
            WhoFirstId = WhoIsFirst().Id;
            WhoFirstName = Players.Where(p => p.Id == WhoFirstId).FirstOrDefault().Name;
            Turn = WhoFirstId;
        }

        /// <summary>
        /// Добавление кораблей
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ships"></param>
        public void AddShip(string id, string ships)
        {
            Player currentPlayer = Players.Find(p => p.Id == id);

            if (currentPlayer.GetShips().Count > 0)
            {
                currentPlayer.GetShips().Clear();
            }

            string[] Ships = ships.Trim('|').Split('|');

            foreach (string s in Ships)
            {
                string deck = s.Split(':')[0];
                string [] coordinates = s.Split(':')[1].Trim('-').Split('-');

                if (deck == "1")
                {
                    Ship oneDeck = new Ship(int.Parse(deck));

                    foreach (string coord in coordinates)
                    {
                        oneDeck.decks.Add(new Deck() { X = int.Parse(coord[0].ToString()), Y = int.Parse(coord[1].ToString()) });
                    }

                    currentPlayer.GetShips().Add(oneDeck);
                }
                else if (deck == "2")
                {
                    Ship twoDeck = new Ship(int.Parse(deck));

                    foreach (string coord in coordinates)
                    {
                        twoDeck.decks.Add(new Deck() { X = int.Parse(coord[0].ToString()), Y = int.Parse(coord[1].ToString()) });
                    }

                    currentPlayer.GetShips().Add(twoDeck);
                }
                else if(deck == "3")
                {
                    Ship threeDeck = new Ship(int.Parse(deck));

                    foreach (string coord in coordinates)
                    {
                        threeDeck.decks.Add(new Deck() { X = int.Parse(coord[0].ToString()), Y = int.Parse(coord[1].ToString()) });
                    }

                    currentPlayer.GetShips().Add(threeDeck);
                }
                else if (deck == "4")
                {
                    Ship fourDeck = new Ship(int.Parse(deck));

                    foreach (string coord in coordinates)
                    {
                        fourDeck.decks.Add(new Deck() { X = int.Parse(coord[0].ToString()), Y = int.Parse(coord[1].ToString()) });
                    }

                    currentPlayer.GetShips().Add(fourDeck);
                }
            }

            PopulateSea(currentPlayer, currentPlayer.GetShips());

            EveryOneReady();
        }

        private void PopulateSea(Player player, List<Ship> Ships)
        {
            foreach (Ship s in Ships)
            {
                foreach (Deck d in s.decks)
                {
                    player.GetSea()[d.X, d.Y].IsOccupied = true;
                }
            }
        }

        /// <summary>
        /// Готовы ли все к игре?
        /// </summary>
        private void EveryOneReady()
        {
            if (Players[0].GetShips().Count != 0 && Players[1].GetShips().Count != 0)
            {
                EveryOneReadyEvent(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Один ход
        /// </summary>
        /// <param name="id"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Move(string id, string x, string y)
        {
            if (!isVictory && id == Turn)
            {
                int coordX = int.Parse(x);
                int coordY = int.Parse(y);

                Player Opponent = Players.Find(p => p.Id != id);

                if (Opponent.GetSea()[coordX, coordY].IsOccupied)
                {
                    var woundedShip = (from ship in Opponent.GetShips() from decks in ship.decks where decks.X == int.Parse(x) && decks.Y == int.Parse(y) select ship).ToList();
                    woundedShip[0].decks.Find(temp => temp.X == int.Parse(x) && temp.Y == int.Parse(y)).IsDamaged = true;
                    if (woundedShip[0].decks.FindAll(d => d.IsDamaged == true).Count() == woundedShip[0].decks.Count)
                    {
                        woundedShip[0].IsDestroyed = true;
                    };
                    MyHitEvent(id, x + y);
                    OpponentHitEvent(Opponent.Id, x + y);
                }
                else
                {
                    MyMissEvent(id, x + y);
                    OpponentMissEvent(Opponent.Id, x + y);
                    Turn = Opponent.Id;
                }
            }
        }

        private void CheckForPlayerShipDestroyed(string id, string x, string y)
        {
            Player Me = Players.Find(p => p.Id == id);
            Player Opponent = Players.Find(p => p.Id != id);

            var ship = (from s in Me.GetShips() from d in s.decks where d.X == int.Parse(x) && d.Y == int.Parse(y) select s).ToList();

            if (ship.Count != 0 && ship[0].IsDestroyed)
            {
                MyShipDestroyedEvent(id, Opponent.Name, ship[0].ToString(), ship[0].decks.Count.ToString(), Me.GetShips().FindAll(s => s.decks.Count() == ship[0].decks.Count() && s.IsDestroyed == false).Count().ToString());
            }
        }

        private void CheckForOpponentShipDestroyed(string id, string x, string y)
        {
            Player Opponent = Players.Find(p => p.Id != id);

            var ship = (from s in Opponent.GetShips() from d in s.decks where d.X == int.Parse(x) && d.Y == int.Parse(y) select s).ToList();

            if (ship.Count != 0 && ship[0].IsDestroyed)
            {
                OpponentShipDestroyedEvent(id, Opponent.Name, ship[0].ToString(), ship[0].decks.Count.ToString(), Opponent.GetShips().FindAll(s => s.decks.Count() == ship[0].decks.Count() && s.IsDestroyed == false).Count().ToString());
            }
        }

        /// <summary>
        /// Проверка кораблей на уничтожение
        /// </summary>
        /// <param name="id"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CompletingTurn(string id, string x, string y) 
        {
            if (id != Turn)
            {
                CheckForPlayerShipDestroyed(id, x, y);
            }
            else
            {
                CheckForOpponentShipDestroyed(id, x, y);
            }
            CheckVictory();
        }

        /// <summary>
        /// Определение, кто ходит первым
        /// </summary>
        /// <returns></returns>
        private Player WhoIsFirst()
        {
            int x = rand.Next(0, 2);
            return Players[x];
        }

        /// <summary>
        /// Проверка на победу
        /// </summary>
        private void CheckVictory()
        {
            int goodPlayerShips = Players[0].GetShips().FindAll(s => s.IsDestroyed == false).Count;
            int goodOpponentShips = Players[1].GetShips().FindAll(s => s.IsDestroyed == false).Count;

            if (goodOpponentShips == 0)
            {
                isVictory = true;
                VictoryId = Players[0].Id;
                WinEvent(this, EventArgs.Empty);
            }
            else if (goodPlayerShips == 0)
            {
                isVictory = true;
                VictoryId = Players[1].Id;
                WinEvent(this, EventArgs.Empty);
            }
        }
    }   
}