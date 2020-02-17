using System.Drawing;

namespace Forms {
    public struct Range {
        public int X1 { get; private set; }
        public int Y1 { get; private set; }
        public int X2 { get; private set; }
        public int Y2 { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Rectangle Rect { get; private set; }
        public RectangleF Layout { get; private set; }
        public string Name { get; private set; }
        public Range(int x1, int y1, int x2, int y2, string name = "") {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Width = X2 - X1 + 1;
            Height = Y2 - Y1 + 1;
            Rect = new Rectangle(X1 + 1, Y1 + 1, Width - 1, Height - 1);
            Layout = new RectangleF(X1 + 4, Y1 + 2, Width, Height);
            Name = name;
        }
        public bool IsInRange(Point pos) {
            return X1 <= pos.X && pos.X <= X2 &&
                Y1 <= pos.Y && pos.Y <= Y2;
        }
    }
}
