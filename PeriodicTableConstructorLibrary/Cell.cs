using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PeriodicTableConstructorLibrary
{
    public class Cell
    {
        public Dictionary<string, Attribute> Attributes { get; set; }
        public Spacer Spacer { get; set; }
        public Color BackColor { get; set; }
        public string CellType { get; set; }

        public Cell(Dictionary<string, Attribute> attributes, Spacer spacer, string cellType, Color backColor)
        {
            this.Attributes = attributes;
            this.Spacer = spacer;
            this.BackColor = backColor;
            this.CellType = cellType;
        }
    }
}
