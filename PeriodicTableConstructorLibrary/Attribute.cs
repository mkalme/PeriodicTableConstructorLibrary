using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PeriodicTableConstructorLibrary
{
    public class Attribute
    {
        public string Text { get; set; }
        public Font Font { get; set; }
        public int LineSpacing { get; set; }
        public string Alignment { get; set; }
        public Padding Padding { get; set; }
        public Color Color { get; set; }
        public bool Visible { get; set; }

        public Attribute(string text, Font font, int lineSpacing, string alignment, Padding padding, Color color, bool visible)
        {
            this.Text = text;
            this.Font = font;
            this.LineSpacing = lineSpacing;
            this.Alignment = alignment;
            this.Padding = padding;
            this.Visible = visible;
            this.Color = color;
        }

        public Attribute Clone()
        {
            return new Attribute(Text, Font, LineSpacing, Alignment, Padding, Color, Visible);
        }
    }
}
