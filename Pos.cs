using System;
using System.Collections.Generic;
using System.Text;

namespace KLibrary
{
    public class Pos
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public List<int> FirstMovement { get; set; } = new List<int>();
        public List<int> SecondMovement { get; set; } = new List<int>();
        public int FirstPoints { get; set; } = 0;
        public int SecondPoints { get; set; } = 0;
    }
}
