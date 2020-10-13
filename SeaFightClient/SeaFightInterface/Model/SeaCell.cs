using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFightInterface.Model
{
    class SeaCell
    {
        public int BorderCount { get; set; }
        public bool IsVisited { get; set; }
        public bool IsOccupied { get; set; }
    }
}
