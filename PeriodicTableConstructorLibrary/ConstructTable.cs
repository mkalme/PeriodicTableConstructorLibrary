using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Drawing;
using System.Diagnostics;

namespace PeriodicTableConstructorLibrary
{
    public class ConstructTable
    {
        public static Table CreateTable(string xmlTest) {
            XmlDocument documemt = new XmlDocument();
            documemt.LoadXml(xmlTest);

            XmlNode tableNode = documemt.SelectSingleNode("/table");

            Table table = new Table(getTableRows(tableNode),
                                              getTableStartingPoint(tableNode),
                                              getTableCellSize(tableNode)
            );

            return table;
        }

        //Table
        private static Row[] getTableRows(XmlNode table) {
            XmlNodeList allRows = table.SelectNodes("/table/row");
            Row[] rows = new Row[allRows.Count];

            for (int i = 0; i < allRows.Count; i++){
                XmlNodeList allCells = allRows[i].ChildNodes;
                Cell[] cells = new Cell[allCells.Count];

                for (int b = 0; b < allCells.Count; b++){
                    Cell cell = new Cell(
                                                getCellAttributes(allCells[b]),
                                                getCellSpacer(allCells[b]),
                                                getCellType(allCells[b]),
                                                getCellColor(allCells[b])
                    );

                    cells[b] = cell;
                }

                rows[i] = new Row(cells);
            }

            return rows;
        }
        private static Point getTableStartingPoint(XmlNode table) {
            return new Point(0, 0);
        }
        private static Size getTableCellSize(XmlNode table) {
            Size cellSize = new Size(25, 25);

            if (nodeHasAttribute(table, "cellWidth")) {
                int cellWidth = cellSize.Width;

                Int32.TryParse(table.Attributes["cellWidth"].Value, out cellWidth);

                if (cellWidth > 0) {
                    cellSize.Width = cellWidth;
                }
            }
            if (nodeHasAttribute(table, "cellHeight")){
                int cellHeight = cellSize.Height;

                Int32.TryParse(table.Attributes["cellHeight"].Value, out cellHeight);

                if (cellHeight > 0) {
                    cellSize.Height = cellHeight;
                }
            }

            return cellSize;
        }

        //Cell
        private static Dictionary<string, Attribute> getCellAttributes(XmlNode cell)
        {
            Dictionary<string, Attribute> attributes = new Dictionary<string, Attribute>();

            XmlNodeList atts = cell.SelectNodes("attribute");
            for (int i = 0; i < atts.Count; i++){
                if (nodeHasAttribute(atts[i], "name")){
                    //Name
                    string name = atts[i].Attributes["name"].Value;

                    Attribute attribute = new Attribute(
                                                        atts[i].InnerText,
                                                        getAttributeFont(atts[i]),
                                                        getAttributeLineSpacing(atts[i]),
                                                        getAttributeAlignment(atts[i]),
                                                        getAttributePadding(atts[i]),
                                                        getAttributeColor(atts[i]),
                                                        getAttributeVisibility(atts[i])
                    );

                    attributes.Add(name, attribute);
                }
            }

            return attributes;
        }
        private static Spacer getCellSpacer(XmlNode cell)
        {
            Spacer spacer = new Spacer(0, 0);

            //Right
            if (nodeHasAttribute(cell, "spacer")){
                int spacerRight = 0;
                Int32.TryParse(cell.Attributes["spacer"].Value, out spacerRight);

                spacer.Right = spacerRight;
            }

            //Bottom
            if (nodeHasAttribute(cell.ParentNode, "spacer")){
                int spacerBottom = 0;
                Int32.TryParse(cell.ParentNode.Attributes["spacer"].Value, out spacerBottom);

                spacer.Bottom = spacerBottom;
            }

            return spacer;
        }
        private static string getCellType(XmlNode cell)
        {
            if (cell.Name.Equals("elem"))
            {
                return CellType.NormalCellType;
            }
            else
            {
                return CellType.BlankCellType;
            }
        }
        private static Color getCellColor(XmlNode cell){
            Color color = Color.White;

            if (nodeHasAttribute(cell, "color"))
            {
                color = ColorTranslator.FromHtml(cell.Attributes["color"].Value);
            }

            return color;
        }

        //Attribute
        private static Font getAttributeFont(XmlNode atts) {
            string fontFamily = "Microsoft Sans Serif";
            int fontSize = 12;
            FontStyle fontStyle = FontStyle.Regular;

            string[] fontSplit = { };
            if (nodeHasAttribute(atts, "font")) {
                fontSplit = atts.Attributes["font"].Value.Split(new string[] { ";" }, StringSplitOptions.None);
            }

            if (fontSplit.Length > 0) {
                fontFamily = fontSplit[0];
            }
            if (fontSplit.Length > 1) {
                Int32.TryParse(fontSplit[1].Trim(), out fontSize);
            }
            if (fontSplit.Length > 2) {
                switch (fontSplit[2].ToLower().Trim()) {
                    case "bold":
                        fontStyle = FontStyle.Bold;
                        break;
                    case "italic":
                        fontStyle = FontStyle.Italic;
                        break;
                    case "regular":
                        fontStyle = FontStyle.Regular;
                        break;
                    case "strikeout":
                        fontStyle = FontStyle.Strikeout;
                        break;
                    case "underline":
                        fontStyle = FontStyle.Underline;
                        break;
                    default:
                        fontStyle = FontStyle.Regular;
                        break;
                }
            }

            Font font = new Font(fontFamily, fontSize, fontStyle, GraphicsUnit.Point);

            return font;
        }
        private static int getAttributeLineSpacing(XmlNode atts)
        {
            int lineSpacing = 8;

            string[] fontSplit = { };
            if (nodeHasAttribute(atts, "font")){
                fontSplit = atts.Attributes["font"].Value.Split(new string[] { ";" }, StringSplitOptions.None);
            }
            if (fontSplit.Length > 3){
                Int32.TryParse(fontSplit[3].Trim(), out lineSpacing);
            }

            return lineSpacing;
        }
        private static string getAttributeAlignment(XmlNode atts) {
            string stringFormat = AttributeAlignmentType.TopLeft; 

            if (nodeHasAttribute(atts, "alignment")) {
                switch (atts.Attributes["alignment"].Value.ToLower()) {
                    case "center":
                        stringFormat = AttributeAlignmentType.Center;
                        break;
                    case "top-right":
                        stringFormat = AttributeAlignmentType.TopRight;
                        break;
                    case "top-left":
                        stringFormat = AttributeAlignmentType.TopLeft;
                        break;
                    default:
                        stringFormat = AttributeAlignmentType.TopLeft;
                        break;
                }
            }

            return stringFormat;
        }
        private static Padding getAttributePadding(XmlNode atts) {
            Padding padding = new Padding(0, 0);

            if (nodeHasAttribute(atts, "padding")) {
                string[] doubles = atts.Attributes["padding"].Value.Trim().Split(new string[] { ";" }, StringSplitOptions.None);

                double x = 0;
                double y = 0;

                Double.TryParse(doubles[0].Replace(".", ","), out x);
                Double.TryParse(doubles[1].Replace(".", ","), out y);

                if (x > 0) {
                    padding.X = x;
                }
                if (y > 0) {
                    padding.Y = y;
                }
            }

            return padding;
        }
        private static Color getAttributeColor(XmlNode atts)
        {
            Color color = Color.Black;

            if (nodeHasAttribute(atts, "color"))
            {
                color = ColorTranslator.FromHtml(atts.Attributes["color"].Value);
            }

            return color;
        }
        private static bool getAttributeVisibility(XmlNode atts) {
            bool visible = true;

            if (nodeHasAttribute(atts, "visible")) {
                bool.TryParse(atts.Attributes["visible"].Value, out visible);
            }

            return visible;
        }

        //Scripts
        private static bool nodeHasAttribute(XmlNode node, string attribute)
        {
            bool hasOne = false;

            for (int i = 0; i < node.Attributes.Count; i++)
            {
                if (node.Attributes[i].Name.Equals(attribute))
                {
                    hasOne = true;
                    goto after_loop;
                }
            }
        after_loop:

            return hasOne;
        }
    }
}
