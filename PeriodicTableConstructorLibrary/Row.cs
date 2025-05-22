using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeriodicTableConstructorLibrary
{
    public class Row
    {
        public Cell[] Cells { get; set; }

        public Row(Cell[] cells)
        {
            this.Cells = cells;
        }
    }
}
