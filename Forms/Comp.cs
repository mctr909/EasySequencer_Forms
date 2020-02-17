using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Forms {
    public partial class Comp : Form {
        private const int TableFooterHeight = 30;
        private const int TableLeftFrameWidth = 50;
        private const int AmpDispUnit = 30;
        private const int AmpDispCols = 12;
        private const int AmpDispRows = 12;
        private const int AmpDispWidth = AmpDispUnit * AmpDispCols;
        private const int AmpDispHeight = AmpDispUnit * AmpDispRows;
        private const int MinDB = -60;
        private const int ZeroDbCol = 10;
        private const int ZeroDbRow = 2;
        private const int DbDispUnit = 12;

        private bool mScrollThreshold = false;
        private bool mScrollGain = false;

        private double mThreshold = -9.0;
        private double mGain = -6.0;

        private CommonCtrl mCommonCtrl;

        private Bitmap mBmpRow;
        private Bitmap mBmpCol;
        private Bitmap mBmpCell;
        private Bitmap mBmpValue;
        private Graphics mGRow;
        private Graphics mGCol;
        private Graphics mGCell;
        private Graphics mGValue;

        public Comp() {
            InitializeComponent();
        }

        private void Comp_Load(object sender, EventArgs e) {
            mCommonCtrl = new CommonCtrl(this);
            drawBackground();
            draw();
        }

        private void picCell_MouseDown(object sender, MouseEventArgs e) {
            var pos = picCell.PointToClient(Cursor.Position);
            if (pos.X <= AmpDispUnit * ZeroDbCol && 0 <= pos.X) {
                mScrollThreshold = true;
                Cursor.Current = Cursors.VSplit;
            }
            if (pos.X <= AmpDispWidth && AmpDispUnit * ZeroDbCol <= pos.X) {
                mScrollGain = true;
                Cursor.Current = Cursors.HSplit;
            }
        }

        private void picCell_MouseUp(object sender, MouseEventArgs e) {
            mScrollThreshold = false;
            mScrollGain = false;
        }

        private void picCell_MouseMove(object sender, MouseEventArgs e) {
            var pos = picCell.PointToClient(Cursor.Position);
            var dbUnit = 5.0 * DbDispUnit / AmpDispUnit;
            if (mScrollThreshold) {
                mThreshold = (int)(pos.X * dbUnit + 0.5) / 10.0 + MinDB;
                draw();
                return;
            }
            if (mScrollGain) {
                mGain = (int)((picCell.Height - pos.Y) * dbUnit + 0.5) / 10.0 + MinDB;
                draw();
                return;
            }
        }

        private void draw() {
            if (mThreshold < MinDB) {
                mThreshold = MinDB;
            }
            if (0 < mThreshold) {
                mThreshold = 0;
            }

            if (mGain < MinDB) {
                mGain = MinDB;
            }
            if (0.0 < mGain) {
                mGain = 0.0;
            }
            if (mGain < mThreshold) {
                mGain = mThreshold;
            }

            var ratio = -mThreshold / (mGain - mThreshold);
            if (0 == mThreshold) {
                ratio = 1;
            }

            var pThresholdX = (int)((mThreshold - MinDB) * AmpDispUnit * 2 / DbDispUnit);
            var pThresholdY = picCell.Height - pThresholdX;
            var pGainX = (int)((mGain - MinDB) * AmpDispUnit * 2 / DbDispUnit);
            var pGainY = picCell.Height - pGainX;
            var pGainYT = pGainY - (int)(AmpDispUnit * (AmpDispCols - ZeroDbCol) / ratio);

            var psThresholdY = pThresholdY;
            var psGainY = pGainY;
            if (picCell.Height < psThresholdY + 20) {
                psThresholdY = picCell.Height - 20;
            }
            if (picCell.Height < psGainY + 20) {
                psGainY = picCell.Height - 20;
            }

            if (null != picCell.Image) {
                picCell.Image.Dispose();
                picCell.Image = null;
            }
            if (null != mBmpValue) {
                mBmpValue.Dispose();
                mBmpValue = null;
                mGValue.Dispose();
                mGValue = null;
            }
            mBmpValue = new Bitmap(picCell.Width, picCell.Height);
            mGValue = Graphics.FromImage(mBmpValue);

            mGValue.DrawLine(Colors.PGraphLine, 0, picCell.Height, pThresholdX, pThresholdY);
            mGValue.DrawLine(Colors.PGraphLine, pThresholdX, pThresholdY, AmpDispUnit * ZeroDbCol, pGainY);
            mGValue.DrawLine(Colors.PGraphLineAlpha, AmpDispUnit * ZeroDbCol, pGainY, AmpDispWidth, pGainYT);

            mGValue.DrawLine(Colors.PGraphLineRed, pThresholdX, 0, pThresholdX, AmpDispHeight);
            mGValue.DrawLine(Colors.PGraphLineRed, AmpDispUnit * ZeroDbCol, pGainY, AmpDispWidth, pGainY);

            mGValue.FillPie(Colors.BGraphPoint, pThresholdX - 4, pThresholdY - 4, 8, 8, 0, 360);
            mGValue.DrawArc(Colors.PTableBorderLight, pThresholdX - 4, pThresholdY - 4, 8, 8, 0, 360);
            mGValue.FillPie(Colors.BGraphPoint, AmpDispUnit * ZeroDbCol - 4, pGainY - 4, 8, 8, 0, 360);
            mGValue.DrawArc(Colors.PTableBorderLight, AmpDispUnit * ZeroDbCol - 4, pGainY - 4, 8, 8, 0, 360);

            mGValue.DrawString(ratio.ToString("0.0"), Fonts.Bold, Colors.BFontTable, new RectangleF(
                AmpDispUnit * ZeroDbCol, AmpDispUnit,
                AmpDispUnit * (AmpDispCols - ZeroDbCol), AmpDispUnit), Fonts.AlignMC);

            mGValue.DrawString(mThreshold.ToString("0.0db"), Fonts.Small, Colors.BFontTable, pThresholdX + 3, psThresholdY);
            mGValue.DrawString(mGain.ToString("0.0db"), Fonts.Small, Colors.BFontTable, AmpDispUnit * ZeroDbCol + 3, psGainY);

            picCell.Image = mBmpValue;
        }

        private void drawBackground() {
            releaseImage();

            mBmpCol = new Bitmap(AmpDispWidth, TableFooterHeight);
            mGCol = Graphics.FromImage(mBmpCol);
            mBmpRow = new Bitmap(TableLeftFrameWidth, AmpDispHeight);
            mGRow = Graphics.FromImage(mBmpRow);
            mBmpCell = new Bitmap(AmpDispWidth, AmpDispHeight);
            mGCell = Graphics.FromImage(mBmpCell);
            //
            // left frame
            //
            mGRow.Clear(Colors.TableHeader);
            for (int row = AmpDispRows - 1, db = MinDB; 1 <= row; row -= 2, db += DbDispUnit) {
                mGRow.DrawLine(0 == db ? Colors.PTableBorderLight : Colors.PTableBorder,
                    0, AmpDispUnit * (row + 1),
                    mBmpRow.Width, AmpDispUnit * (row + 1));
                mGRow.DrawString(db.ToString("0db"), Fonts.Small, Colors.BFontTable,
                    new RectangleF(-3, AmpDispUnit * row,
                    mBmpRow.Width, AmpDispUnit), Fonts.AlignBR);
            }
            mGRow.DrawLine(Colors.PTableBorderBold,
                TableLeftFrameWidth - 1, 0,
                TableLeftFrameWidth - 1, mBmpRow.Height);
            //
            // table
            //
            mGCell.Clear(Colors.TableCell);
            mGCell.FillRectangle(Colors.BTableHeader,
                mBmpCell.Width, 0,
                mBmpCell.Width, mBmpCell.Height);
            for (var row = 1; row < AmpDispRows; row++) {
                mGCell.DrawLine(0 == (row % 2) ? Colors.PTableBorder : Colors.PTableBorderDark,
                    0, AmpDispUnit * row,
                    mBmpCell.Width, AmpDispUnit * row);
            }
            for (var col = 1; col < AmpDispCols; col++) {
                mGCell.DrawLine(0 == (col % 2) ? Colors.PTableBorder : Colors.PTableBorderDark,
                    AmpDispUnit * col, 0,
                    AmpDispUnit * col, mBmpCell.Height);
            }
            for (var y = AmpDispUnit / 2; y < AmpDispHeight; y += AmpDispUnit) {
                for (var x = AmpDispUnit / 2; x < AmpDispWidth; x += AmpDispUnit) {
                    mBmpCell.SetPixel(x, y, Colors.TableBorder);
                }
            }
            mGCell.DrawString("Ratio", Fonts.Bold, Colors.BFontTable, new RectangleF(
                AmpDispUnit * ZeroDbCol, 0,
                AmpDispUnit * (AmpDispCols - ZeroDbCol), AmpDispUnit), Fonts.AlignMC);
            mGCell.DrawLine(Colors.PTableBorderLight,
                0, AmpDispUnit * ZeroDbRow,
                mBmpCell.Width, AmpDispUnit * ZeroDbRow);
            mGCell.DrawLine(Colors.PTableBorderLight,
                AmpDispUnit * ZeroDbCol, 0,
                AmpDispUnit * ZeroDbCol, mBmpCell.Height);
            //
            // footer
            //
            mGCol.Clear(Colors.TableHeader);
            for (int col = 0, db = MinDB; col < AmpDispCols; col += 2, db += DbDispUnit) {
                mGCol.DrawLine(0 == db ? Colors.PTableBorderLight : Colors.PTableBorder,
                    AmpDispUnit * col, 0,
                    AmpDispUnit * col, mBmpCol.Height);
                mGCol.DrawString(db.ToString("0db"), Fonts.Small, Colors.BFontTable,
                    new RectangleF(AmpDispUnit * col, 0,
                    AmpDispUnit * col, mBmpCol.Height), Fonts.AlignML);
            }
            mGCol.DrawLine(Colors.PTableBorderBold, 0, 1, mBmpCol.Width, 1);

            setImage();
        }

        private void setImage() {
            mGRow.DrawLine(Colors.PTabBorderBold, 0, 1, mBmpRow.Width, 1);
            mGRow.DrawLine(Colors.PTabBorderBold, 1, 0, 1, mBmpRow.Height);
            mGRow.DrawLine(Colors.PTabBorderBold,
                0, mBmpRow.Height - 1,
                mBmpRow.Width, mBmpRow.Height - 1);
            mGCol.DrawLine(Colors.PTabBorderBold, 1, 0, 1, mBmpCol.Height - 1);
            mGCol.DrawLine(Colors.PTabBorderBold,
                0, mBmpCol.Height - 1,
                mBmpCol.Width - 1, mBmpCol.Height - 1);
            mGCol.DrawLine(Colors.PTabBorderBold,
                mBmpCol.Width - 1, 0,
                mBmpCol.Width - 1, mBmpCol.Height);
            mGCell.DrawLine(Colors.PTabBorderBold,
                mBmpCell.Width - 1, 0,
                mBmpCell.Width - 1, mBmpCell.Height);
            mGCell.DrawLine(Colors.PTabBorderBold,
                0, 1,
                mBmpCell.Width - 1, 1);

            Width = mBmpRow.Width + mBmpCell.Width;
            Height = mCommonCtrl.FormCtrlBottom + mBmpCol.Height + mBmpCell.Height;

            picFooter.Width = mBmpCol.Width;
            picFooter.Height = mBmpCol.Height;
            picRow.Width = mBmpRow.Width;
            picRow.Height = mBmpRow.Height;
            picCell.Width = mBmpCell.Width;
            picCell.Height = mBmpCell.Height;
            picFooter.BackgroundImage = mBmpCol;
            picRow.BackgroundImage = mBmpRow;
            picCell.BackgroundImage = mBmpCell;

            picRow.Left = 0;
            picCell.Left = picRow.Right;
            picFooter.Left = picRow.Right;

            picRow.Top = mCommonCtrl.FormCtrlBottom;
            picCell.Top = mCommonCtrl.FormCtrlBottom;
            picFooter.Top = picCell.Bottom;
        }

        private void releaseImage() {
            if (null != mBmpCol) {
                mBmpCol.Dispose();
                mBmpCol = null;
                mGCol.Dispose();
                mGCol = null;
            }
            if (null != mBmpRow) {
                mBmpRow.Dispose();
                mBmpRow = null;
                mGRow.Dispose();
                mGRow = null;
            }
            if (null != mBmpCell) {
                mBmpCell.Dispose();
                mBmpCell = null;
                mGCell.Dispose();
                mGCell = null;
            }

            if (null != picFooter.BackgroundImage) {
                picFooter.BackgroundImage.Dispose();
                picFooter.BackgroundImage = null;
            }
            if (null != picRow.BackgroundImage) {
                picRow.BackgroundImage.Dispose();
                picRow.BackgroundImage = null;
            }
            if (null != picCell.BackgroundImage) {
                picCell.BackgroundImage.Dispose();
                picCell.BackgroundImage = null;
            }
        }
    }
}
