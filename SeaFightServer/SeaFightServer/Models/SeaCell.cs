using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeaFightServer.Models
{
    public class SeaCell
    {
        public bool IsBorder { get; set; }
        public bool IsVisited { get; set; }
        public bool IsOccupied { get; set; }
    }
}