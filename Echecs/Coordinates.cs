using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Coordinates
    {
        private int x, y;

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public bool IsSameAs (Coordinates Asked)
        {
            if (Asked.X == X && Asked.Y == Y)
                return true;

            return false;
        }
    }
}
