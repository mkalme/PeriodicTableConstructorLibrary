using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PeriodicTableConstructorLibrary
{
    public class Table
    {
        public Row[] Rows { get; set; }
        public Point Location { get; set; }
        public Size CellSize { get; set; }

        public Table(Row[] rows, Point location, Size cellSize)
        {
            this.Rows = rows;
            this.Location = location;
            this.CellSize = cellSize;
        }
    }
}
