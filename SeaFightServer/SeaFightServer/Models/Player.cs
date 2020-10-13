using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeaFightServer.Models
{
    public class Player
    {
        public string Name { get; private set; }
        public string Id { get; private set; }
        public bool IsBusy { get; set; }
        SeaCell[,] Sea { get; set; }
        List<Ship> Ships { get; set; }

        public Player(string name, string id)
        {
            Id = id;
            Name = name;
        }

        public void EnterGame()
        {
            Sea = new SeaCell[10, 10];
            CreateSea();
            Ships = new List<Ship>();
        }

        public void Quit()
        {
            IsBusy = false;
            Sea = null;
            Ships = null;
        }

        private void CreateSea()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Sea[i, j] = new SeaCell();
                }
            }
        }

        public SeaCell[,] GetSea()
        {
            return Sea;
        }

        public List<Ship> GetShips()
        {
            return Ships;
        }
    }
}