using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeriodicTableConstructorLibrary
{
    public class Spacer
    {
        public int Right { get; set; }
        public int Bottom { get; set; }

        public Spacer(int right, int bottom)
        {
            this.Right = right;
            this.Bottom = bottom;
        }
    }
}
