using System.Drawing;

namespace Forms {
    public class Fonts {
        public static Font Bold = new Font("Segoe UI", 11.0f, FontStyle.Bold);
        public static Font Small = new Font("Segoe UI", 9.0f, FontStyle.Regular);
        public static Font SmallBold = new Font("Segoe UI", 9.0f, FontStyle.Bold);

        /// <summary>中央</summary>
        public static StringFormat AlignMC = new StringFormat {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        /// <summary>中央上</summary>
        public static StringFormat AlignTC = new StringFormat {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Near
        };
        /// <summary>右上</summary>
        public static StringFormat AlignTR = new StringFormat {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Near
        };
        /// <summary>右下</summary>
        public static StringFormat AlignBR = new StringFormat {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Far
        };
        /// <summary>左中段</summary>
        public static StringFormat AlignML = new StringFormat {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center
        };
        /// <summary>右中段</summary>
        public static StringFormat AlignMR = new StringFormat {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Center
        };
    }
}
