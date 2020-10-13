using SeaFightInterface.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFightInterface.Model
{
    public enum Participants
    {
        Human,
        Opponent
    }

    class GameState
    {
        SeaCell[,] PlayerSea;
        SeaCell[,] OpponentSea;
        Random rand = new Random();
        Computer Opponent;
        List<Ship> PlayerShips = new List<Ship>();
        List<Ship> OpponentShips = new List<Ship>();
        bool IsOpponentTurn;
        bool IsVictory;

        public event Action<List<(int, int)>> OpponentShipDestroyed;
        public event Action<List<(int, int)>> PlayerShipDestroyed;
        public event Action<(int, int)> PlayerShipHit;
        public event Action<(int, int)> OpponentShipHit;
        public event Action<(int, int)> PlayerShipMiss;
        public event Action<(int, int)> OpponentShipMiss;
        public event Action<Participants> VictoryHappened;
        public event Action<int, int> DecreaseOpponentShipCount;
        public event Action<int, int> DecreasePlayerShipCount;
        public event Action<Participants> FirstChosen;
        public event Action MarkMyTurnEvent;
        public event Action MarkOpponentTurnEvent;

        public int OneDeckCount { get; }
        public int TwoDeckCount { get; }
        public int ThreeDeckCount { get; }
        public int FourDeckCount { get; }

        public GameState()
        {
            CreateOcean();
            Opponent = new Computer();
            OneDeckCount = 4;
            TwoDeckCount = 3;
            ThreeDeckCount = 2;
            FourDeckCount = 1;
        }

        /// <summary>
        /// Создание полей игрока и противника
        /// </summary>
        private void CreateOcean()
        {
            PlayerSea = new SeaCell[10, 10];
            OpponentSea = new SeaCell[10, 10];

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    PlayerSea[i, j] = new SeaCell();
                    OpponentSea[i, j] = new SeaCell();
                }
            }
        }

        public List<Ship> GetPlayerShips()
        {
            return PlayerShips;
        }

        public List<Ship> GetOpponentShips()
        {
            return OpponentShips;
        }

        public SeaCell[,] GetPlayerSea()
        {
            return PlayerSea;
        }

        public SeaCell[,] GetOpponentSea()
        {
            return OpponentSea;
        }

        /// <summary>
        /// Старт игры
        /// </summary>
        public async void StartGame() 
        {
            Participants First = WhoFirst();
            FirstChosen(First);

            if (First == Participants.Opponent)
            {
                IsOpponentTurn = true;
                MarkOpponentTurnEvent();
                await CompTurn();
            }
            else 
            {
                MarkMyTurnEvent();
            }
        }

        /// <summary>
        /// Кто ходит первым?
        /// </summary>
        private Participants WhoFirst()
        {
            int choice = rand.Next(0, 2);

            if (choice == 0)
            {
                return Participants.Opponent;
            }
            else
            {
                return Participants.Human;
            }
        }

        /// <summary>
        /// Проверка на уничтожение корабля противника
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void CheckForOpponentShipDestroyed(int x, int y) 
        {
            var ship = (from s in OpponentShips from d in s.decks where d.X == x && d.Y == y select s).ToList();
            if (ship[0].decks.FindAll(d => d.IsDamaged == true).Count() == ship[0].DeckNumber)
            {
                ship[0].IsDestroyed = true;
                OpponentShipDestroyed((from d in ship[0].decks select (d.X, d.Y)).ToList());
                CheckVictory(PlayerShips, OpponentShips);
                if (!IsVictory)
                {
                    DecreaseOpponentShipCount(ship[0].DeckNumber, OpponentShips.FindAll(s => s.DeckNumber == ship[0].DeckNumber && s.IsDestroyed == false).Count());
                }
            }
        }

        /// <summary>
        /// Проверка на уничтожение корабля игрока
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void CheckForPlayerShipDestroyed(int x, int y)
        {
            var ship = (from s in PlayerShips from d in s.decks where d.X == x && d.Y == y select s).ToList();
            if (ship[0].decks.FindAll(d => d.IsDamaged == true).Count() == ship[0].DeckNumber)
            {
                ship[0].IsDestroyed = true;
                PlayerShipDestroyed((from d in ship[0].decks select (d.X, d.Y)).ToList());
                
                if (!IsVictory)
                {
                    DecreasePlayerShipCount(ship[0].DeckNumber, PlayerShips.FindAll(s => s.DeckNumber == ship[0].DeckNumber && s.IsDestroyed == false).Count());
                }
            };
        }

        /// <summary>
        /// Ход
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public async void Turn(int x, int y)
        {
            if (!IsOpponentTurn && !IsVictory && !OpponentSea[x, y].IsVisited)
            {
                HumanTurn(x, y);
                if (IsOpponentTurn && !IsVictory)
                {
                    await CompTurn();
                }
            }
        }

        /// <summary>
        /// Ход компьютера
        /// </summary>
        /// <returns></returns>
        private async Task CompTurn()
        {
            while (IsOpponentTurn && !IsVictory)
            {
                (int, int) Coordinates = await Opponent.OpponentMove(PlayerSea, PlayerShips);

                if (PlayerSea[Coordinates.Item1, Coordinates.Item2].IsOccupied)
                {
                    OpponentShipHit((Coordinates.Item1, Coordinates.Item2));                    
                    (from ship in PlayerShips from decks in ship.decks where decks.X == Coordinates.Item1 && decks.Y == Coordinates.Item2 select decks).ToList()[0].IsDamaged = true;
                    CheckVictory(PlayerShips, OpponentShips);
                }
                else
                {
                    OpponentShipMiss((Coordinates.Item1, Coordinates.Item2));
                    IsOpponentTurn = false;
                    MarkMyTurnEvent();
                }
            }
        }

        /// <summary>
        /// Ход игрока
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void HumanTurn(int x, int y)
        {
            if (OpponentSea[x, y].IsOccupied)
            {
                PlayerShipHit((x, y));
               (from ship in OpponentShips from decks in ship.decks where decks.X == x && decks.Y == y select decks).ToList()[0].IsDamaged = true;
            }
            else
            {
                PlayerShipMiss((x, y));
                IsOpponentTurn = true;
                MarkOpponentTurnEvent();
            }
            OpponentSea[x, y].IsVisited = true;
        }

        /// <summary>
        /// Проверка на уничтожение кораблей и победу в конце хода
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CompletingTurn(int x, int y) 
        {
            if (IsOpponentTurn)
            {
                CheckForPlayerShipDestroyed(x, y);
            }
            else 
            {
                CheckForOpponentShipDestroyed(x, y);
            }
        }

        /// <summary>
        /// Проверка на победу
        /// </summary>
        /// <param name="playerShips"></param>
        /// <param name="compShips"></param>
        private void CheckVictory(List<Ship> playerShips, List<Ship> compShips)
        {
            int goodPlayerShips = playerShips.FindAll(s => s.IsDestroyed == false).Count;
            int goodCompShips = compShips.FindAll(s => s.IsDestroyed == false).Count;

            if (goodCompShips == 0)
            {
                IsVictory = true;
                VictoryHappened(Participants.Human);
            }

            if (goodPlayerShips == 0)
            {
                IsVictory = true;
                VictoryHappened(Participants.Opponent);
            }
        }
    }
}
