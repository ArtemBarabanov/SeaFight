using SeaFightInterface.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFightInterface.Model
{
    class TwoDeck: Ship
    {
        public TwoDeck(ShipOrientation orientation)
        {
            this.orientation = orientation;
            DeckNumber = 2;
        }
    }
}
