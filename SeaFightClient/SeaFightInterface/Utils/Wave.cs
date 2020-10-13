using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFightInterface.Model
{
    enum Direction
    {
        Grow,
        Shrink
    }
    class Wave
    {
        public int StartsWithX { get; set; }
        public int EndsWithX { get; set; }
        public int Y { get; set; }
        public Direction GrowDirection { get; set; }
    }
}
