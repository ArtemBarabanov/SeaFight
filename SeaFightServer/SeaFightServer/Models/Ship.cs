using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SeaFightServer.Models
{
    public enum ShipOrientation
    {
        Horizontal,
        Vertical
    }

    public class Ship
    {
        public int deckNumber { get; set; }
        public List<Deck> decks = new List<Deck>();
        public bool IsChosen { get; set; }
        public bool IsDestroyed { get; set; }

        public Ship(int deckNumber) 
        {
            this.deckNumber = deckNumber;
        }

        public override string ToString()
        {
            StringBuilder stringDecks = new StringBuilder();
            foreach (Deck d in decks)
            {
                stringDecks.Append($"{d.X}{d.Y}-");
            }
            return stringDecks.ToString().Trim('-');
        }
    }
}
