using SeaFightInterface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFightInterface.Presenter
{
    class Computer
    {
        int compX;
        int compY;
        bool isHaunting;
        Random rand = new Random();
        List<Deck> EnemyShip = new List<Deck>();
        string previousMove;

        public async Task<(int, int)> OpponentMove(SeaCell[,] sea, List<Ship> MyShips)
        {
            await Task.Delay(5000);

            if (!isHaunting)
            {
                do
                {
                    compX = rand.Next(0, 10);
                    compY = rand.Next(0, 10);
                }
                while (sea[compX, compY].IsVisited);

                if (sea[compX, compY].IsOccupied)
                {
                    previousMove = compX + " " + compY;
                    sea[compX, compY].IsVisited = true;
                }
                else
                {
                    sea[compX, compY].IsVisited = true;
                    previousMove = null;
                }
                if (previousMove != null)
                {
                    compX = int.Parse(previousMove.Split(' ')[0]);
                    compY = int.Parse(previousMove.Split(' ')[1]);

                    ShipProcessing(compX, compY, sea, MyShips);
                }
            }
            else if (isHaunting)
            {
                //1 - вверх, 2 - направо, 3 - вниз, 4 - налево
                int direction = 1;
                compX = int.Parse(previousMove.Split(' ')[0]);
                compY = int.Parse(previousMove.Split(' ')[1]);

                if (direction == 1)
                {
                    if (compY < 9 && !sea[compX, compY + 1].IsVisited)
                    {
                        compY++;
                    }
                    else
                    {
                        direction = 2;
                    }
                    if (compY == 9)
                    {
                        direction = 2;
                    }
                }
                if (direction == 2)
                {
                    if (compX < 9 && !sea[compX + 1, compY].IsVisited)
                    {
                        compX++;
                    }
                    else
                    {
                        direction = 3;
                    }
                    if (compX == 9)
                    {
                        direction = 3;
                    }
                }
                if (direction == 3)
                {
                    if (compY > 0 && !sea[compX, compY - 1].IsVisited)
                    {
                        compY--;
                    }
                    else
                    {
                        direction = 4;
                    }
                    if (compY == 0)
                    {
                        direction = 4;
                    }
                }
                if (direction == 4)
                {
                    if (compX > 0 && !sea[compX - 1, compY].IsVisited)
                    {
                        compX--;
                    }
                }

                if (sea[compX, compY].IsOccupied)
                {
                    sea[compX, compY].IsVisited = true;
                    ShipProcessing(compX, compY, sea, MyShips);
                    previousMove = compX + " " + (compY);
                }
                else if (!sea[compX, compY].IsVisited)
                {
                    sea[compX, compY].IsVisited = true;
                }

                if (EnemyShip.Count >= 2)
                {
                    if (EnemyShip[0].X == EnemyShip[1].X)
                    {
                        foreach (Deck deck in EnemyShip)
                        {
                            if (deck.Y + 1 < 10 && !(sea[deck.X, deck.Y + 1].IsVisited))
                            {
                                previousMove = deck.X + " " + deck.Y;
                            }
                            if (deck.Y - 1 >= 0 && !(sea[deck.X, deck.Y - 1].IsVisited))
                            {
                                previousMove = deck.X + " " + deck.Y;
                            }
                        }
                    }
                    if (EnemyShip[0].Y == EnemyShip[1].Y)
                    {
                        foreach (Deck deck in EnemyShip)
                        {
                            if (deck.X + 1 < 10 && !(sea[deck.X + 1, deck.Y].IsVisited))
                            {
                                previousMove = deck.X + " " + deck.Y;
                            }
                            if (deck.X - 1 >= 0 && !(sea[deck.X - 1, deck.Y].IsVisited))
                            {
                                previousMove = deck.X + " " + deck.Y;
                            }
                        }
                    }
                }
            }

            return (compX, compY);
        }

        private void ShipProcessing(int compX, int compY, SeaCell[,] sea, List<Ship> MyShips)
        {
            foreach (Ship ship in MyShips)
            {
                if (ship.decks.Find(temp => temp.X == compX && temp.Y == compY) != null)
                {
                    if (ship.decks.Find(temp => temp.X == compX && temp.Y == compY).IsDamaged == false)
                    {
                        ship.decks.Find(temp => temp.X == compX && temp.Y == compY).IsDamaged = true;
                        sea[compX, compY].IsVisited = true;
                        if (compX + 1 < 10 && compY + 1 < 10)
                        {
                            sea[compX + 1, compY + 1].IsVisited = true;
                        }
                        if (compX + 1 < 10 && compY - 1 >= 0)
                        {
                            sea[compX + 1, compY - 1].IsVisited = true;
                        }
                        if (compX - 1 >= 0 && compY + 1 < 10)
                        {
                            sea[compX - 1, compY + 1].IsVisited = true;
                        }
                        if (compX - 1 >= 0 && compY - 1 >= 0)
                        {
                            sea[compX - 1, compY - 1].IsVisited = true;
                        }
                        EnemyShip.Add(new Deck() { X = compX, Y = compY });
                        isHaunting = true;

                        if (ship.decks.FindAll(d => d.IsDamaged == true).Count() == ship.DeckNumber)
                        {
                            ship.IsDestroyed = true;

                            foreach (Deck deck in ship.decks)
                            {
                                if (deck.Y + 1 < 10 && !sea[deck.X, deck.Y + 1].IsOccupied)
                                {
                                    sea[deck.X, deck.Y + 1].IsVisited = true;
                                }
                                if (deck.Y - 1 >= 0 && !sea[deck.X, deck.Y - 1].IsOccupied)
                                {
                                    sea[deck.X, deck.Y - 1].IsVisited = true;
                                }
                                if (deck.X - 1 >= 0 && !sea[deck.X - 1, deck.Y].IsOccupied)
                                {
                                    sea[deck.X - 1, deck.Y].IsVisited = true;
                                }
                                if (deck.X + 1 < 10 && !sea[deck.X + 1, deck.Y].IsOccupied)
                                {
                                    sea[deck.X + 1, deck.Y].IsVisited = true;
                                }
                            }
                            EnemyShip.Clear();
                            previousMove = null;
                            isHaunting = false;
                        }
                    }
                }
            }
        }
    }
}
