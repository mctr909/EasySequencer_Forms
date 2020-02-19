using System.Drawing;
using System.Drawing.Drawing2D;

namespace Forms {
    static class Colors {
        public static Color FormColor = Color.FromArgb(47, 95, 144);

        public static Color GraphPoint = Color.FromArgb(47, TableCell.R, TableCell.G, TableCell.B);
        public static Color GraphLine = Color.FromArgb(127, 127, 127);
        public static Color GraphLineAlpha = Color.FromArgb(95, 191, 191, 191);
        public static Color GraphLineAlphaRed = Color.FromArgb(127, 255, 0, 0);
        public static Color GraphLineRed = Color.FromArgb(191, 31, 31);
        public static Color GraphLineYellow = Color.FromArgb(191, 191, 0);
        public static Color GraphLineGreen = Color.FromArgb(31, 167, 31);

        public static Color ButtonLeave = Color.FromArgb(47, 47, 191);
        public static Color ButtonHover = Color.FromArgb(191, 47, 47);

        public static Color TabBorder = Color.FromArgb(0, 0, 0);
        public static Color TabButtonEnable = Color.FromArgb(63, 63, 63);
        public static Color TabButtonDisable = Color.FromArgb(31, 31, 31);
        public static Color FontTabButtonEnable = Color.FromArgb(255, 255, 255);
        public static Color FontTabButtonDisable = Color.FromArgb(127, 127, 127);

        public static Color TableFont = Color.FromArgb(255, 255, 255);
        public static Color TableHeader = Color.FromArgb(63, 63, 63);
        public static Color TableCell = Color.FromArgb(23, 31, 31);
        public static Color TableBorder = Color.FromArgb(114, 99, 78);
        public static Color TableBorderDark = Color.FromArgb(76, 66, 52);
        public static Color TableBorderLight = Color.FromArgb(184, 169, 150);

        public static Pen PGraphLine = new Pen(GraphLine, 3.0f) {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };
        public static Pen PGraphLineAlpha = new Pen(GraphLineAlpha, 3.0f) {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };
        public static Pen PGraphLineRed = new Pen(GraphLineRed, 5.0f) {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };
        public static Pen PGraphLineYellow = new Pen(GraphLineYellow, 5.0f) {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };
        public static Pen PGraphLineGreen = new Pen(GraphLineGreen, 5.0f) {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };

        public static Pen PGraphAuxiliary = new Pen(GraphLineRed, 1.0f);
        public static Pen PGraphBoundaryValue = new Pen(GraphLineAlphaRed, 3.0f);
        public static Pen PTabBorder = new Pen(TabBorder, 1.0f);
        public static Pen PTabBorderBold = new Pen(TabBorder, 2.0f);
        public static Pen PTableBorder = new Pen(TableBorder, 1.0f);
        public static Pen PTableBorderBold = new Pen(TableBorder, 2.0f);
        public static Pen PTableBorderDark = new Pen(TableBorderDark, 1.0f);
        public static Pen PTableBorderLight = new Pen(TableBorderLight, 1.0f);
        public static Pen PTableBorderLightBold = new Pen(TableBorderLight, 2.0f);

        public static Brush BTabButtonEnable = new Pen(TabButtonEnable, 1.0f).Brush;
        public static Brush BTabButtonDisable = new Pen(TabButtonDisable, 1.0f).Brush;
        public static Brush BTableHeader = new Pen(TableHeader, 1.0f).Brush;
        public static Brush BTableCell = new Pen(TableCell, 1.0f).Brush;

        public static Brush BFontTabButtonEnable = new Pen(FontTabButtonEnable, 1.0f).Brush;
        public static Brush BFontTabButtonDisable = new Pen(FontTabButtonDisable, 1.0f).Brush;
        public static Brush BFontTable = new Pen(TableFont, 1.0f).Brush;
        public static Brush BGraphPoint = new Pen(GraphPoint, 1.0f).Brush;
    }
}
