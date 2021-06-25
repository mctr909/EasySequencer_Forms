using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Forms {
    public class TabButton {
        private Font mFont;
        private List<Range> mTabList = new List<Range>();
        private PictureBox mPix;
        private Bitmap mBmp;
        private Graphics mG;
        private EventHandler mTabSelectedHandler;

        public string CurrentTab { get; private set; }

        public TabButton(PictureBox pix, EventHandler tabSelectedHandler, float tabSize, string[] tabs) {
            mFont = new Font("Segoe UI", tabSize, FontStyle.Bold);
            var tmpG = Graphics.FromImage(new Bitmap(1, 1));
            var sumWidth = 0;
            var sumHeight = (int)(tmpG.MeasureString("A", mFont).Height + 4);
            foreach (var tab in tabs) {
                var width = (int)(tmpG.MeasureString(tab, mFont).Width * 1.25f);
                mTabList.Add(new Range(sumWidth, 0, sumWidth + width - 1, sumHeight - 1, tab));
                sumWidth += width;
            }
            sumWidth += 2;
            mPix = pix;
            mPix.Width = sumWidth;
            mPix.Height = sumHeight;
            mPix.Click += new EventHandler(click);
            mTabSelectedHandler = tabSelectedHandler;
            mBmp = new Bitmap(sumWidth, sumHeight);
            mG = Graphics.FromImage(mBmp);
            CurrentTab = tabs[0];
            draw();
        }

        private void click(object sender, EventArgs e) {
            var pos = mPix.PointToClient(Cursor.Position);
            foreach (var tab in mTabList) {
                if (tab.IsInRange(pos) && CurrentTab != tab.Name) {
                    CurrentTab = tab.Name;
                    draw();
                    mTabSelectedHandler.Invoke(sender, e);
                    return;
                }
            }
        }

        private void draw() {
            mG.Clear(Color.Transparent);
            foreach (var tab in mTabList) {
                if (tab.Name == CurrentTab) {
                    mG.FillRectangle(Colors.BTabButtonEnable, tab.Rect);
                    mG.DrawString(tab.Name, mFont, Colors.BTabButtonEnableFont, tab.Layout);
                    mG.DrawLine(Colors.PTabBorderBold, tab.X1 + 1, 1, tab.X2 + 1, 1);
                    mG.DrawLine(Colors.PTabBorderBold, tab.X1 + 1, 1, tab.X1 + 1, tab.Y2 + 1);
                    mG.DrawLine(Colors.PTabBorderBold, tab.X2 + 1, 1, tab.X2 + 1, tab.Y2 + 1);
                } else {
                    mG.FillRectangle(Colors.BTabButtonDisable, tab.Rect);
                    mG.DrawString(tab.Name, mFont, Colors.BTabButtonDisableFont, tab.Layout);
                    mG.DrawLine(Colors.PTabBorder, tab.X1 + 1, 1, tab.X2, 1);
                    mG.DrawLine(Colors.PTabBorder, tab.X1, 2, tab.X1, tab.Y2 + 1);
                    mG.DrawLine(Colors.PTabBorder, tab.X2 + 1, 2, tab.X2 + 1, tab.Y2 + 1);
                    mG.DrawLine(Colors.PTabBorderBold, tab.X1, tab.Y2, tab.X2 + 1, tab.Y2);
                }
            }
            mPix.Image = mBmp;
        }
    }
}
