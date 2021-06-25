using System.Drawing;
using System.Drawing.Drawing2D;

namespace Forms {
    static class Colors {
        public static Color FormColor = Color.FromArgb(63, 112, 167);

        private static Color CLine = Color.FromArgb(127, 127, 127);
        private static Color CLineA = Color.FromArgb(95, 191, 191, 191);
        private static Color CLineRA = Color.FromArgb(127, 255, 0, 0);
        private static Color CLineR = Color.FromArgb(191, 31, 31);
        private static Color CLineY = Color.FromArgb(211, 211, 0);
        private static Color CLineG = Color.FromArgb(31, 167, 31);

        private static Color CTabBorder = Color.FromArgb(0, 0, 0);
        private static Color CTabEnable = Color.FromArgb(63, 63, 63);
        private static Color CTabEnableText = Color.FromArgb(255, 255, 255);
        private static Color CTabDisable = Color.FromArgb(31, 31, 31);
        private static Color CTabDisableText = Color.FromArgb(127, 127, 127);

        private static Color CTableFont = Color.FromArgb(255, 255, 255);
        public static Color CTableHeader = Color.FromArgb(63, 63, 63);
        public static Color CTableCell = Color.FromArgb(23, 31, 31);
        public static Color CTableBorderDark = Color.FromArgb(76, 66, 52);
        public static Color CTableBorder = Color.FromArgb(114, 99, 78);
        public static Color CTableBorderLight = Color.FromArgb(184, 169, 150);

        private static Color CPoint = Color.FromArgb(47, CTableCell.R, CTableCell.G, CTableCell.B);

        public static Pen PLine = new Pen(CLine, 3.0f) {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };
        public static Pen PLineA = new Pen(CLineA, 3.0f) {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };
        public static Pen PLineRA = new Pen(CLineRA, 3.0f) {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };
        public static Pen PLineBoldR = new Pen(CLineR, 5.0f) {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };
        public static Pen PLineBoldY = new Pen(CLineY, 5.0f) {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };
        public static Pen PLineBoldG = new Pen(CLineG, 5.0f) {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };
        public static Pen PLineAuxiliary = new Pen(CLineR, 1.0f);

        public static Pen PTabBorder = new Pen(CTabBorder, 1.0f);
        public static Pen PTabBorderBold = new Pen(CTabBorder, 2.0f);
        public static Pen PTableBorder = new Pen(CTableBorder, 1.0f);
        public static Pen PTableBorderBold = new Pen(CTableBorder, 2.0f);
        public static Pen PTableBorderDark = new Pen(CTableBorderDark, 1.0f);
        public static Pen PTableBorderLight = new Pen(CTableBorderLight, 1.0f);
        public static Pen PTableBorderLightBold = new Pen(CTableBorderLight, 2.0f);

        public static Brush BTabButtonEnable = new Pen(CTabEnable, 1.0f).Brush;
        public static Brush BTabButtonEnableFont = new Pen(CTabEnableText, 1.0f).Brush;
        public static Brush BTabButtonDisable = new Pen(CTabDisable, 1.0f).Brush;
        public static Brush BTabButtonDisableFont = new Pen(CTabDisableText, 1.0f).Brush;

        public static Brush BTableHeader = new Pen(CTableHeader, 1.0f).Brush;
        public static Brush BTableCell = new Pen(CTableCell, 1.0f).Brush;
        public static Brush BTableFont = new Pen(CTableFont, 1.0f).Brush;
        public static Brush BPoint = new Pen(CPoint, 1.0f).Brush;
    }
}
