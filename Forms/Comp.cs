using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
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
        private bool mScrollOutputGain = false;

        private double mThreshold = -12.0;
        private double mGain = -6.0;
        private double mRatio = 1.0;
        private double mOutputGain = 6.0;

        private CommonCtrl mCommonCtrl;

        private IntPtr mPtrUvL;
        private IntPtr mPtrUvR;

        private Bitmap mBmpRow;
        private Bitmap mBmpRowValue;
        private Bitmap mBmpCol;
        private Bitmap mBmpCell;
        private Bitmap mBmpCellValue;
        private Graphics mGRow;
        private Graphics mGRowValue;
        private Graphics mGCol;
        private Graphics mGCell;
        private Graphics mGCellValue;

        public Comp() {
            InitializeComponent();
        }

        private void Comp_Load(object sender, EventArgs e) {
            mCommonCtrl = new CommonCtrl(this);
            drawBackground();
            mRatio  = - mThreshold / (mGain - mThreshold);
            draw();
        }

        private void picRow_MouseDown(object sender, MouseEventArgs e) {
            mScrollOutputGain = true;
            Cursor.Current = Cursors.HSplit;
        }

        private void picRow_MouseUp(object sender, MouseEventArgs e) {
            mScrollOutputGain = false;
        }

        private void picRow_MouseMove(object sender, MouseEventArgs e) {
            var pos = picRow.PointToClient(Cursor.Position);
            var dbUnit = 5.0 * DbDispUnit / AmpDispUnit;

            if (mScrollOutputGain) {
                mOutputGain = (int)((picRow.Height - pos.Y) * dbUnit + 0.5) / 10.0 + MinDB;
                mOutputGain *= -1.0;
                limit();
                draw();
            }
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
                limit();
                draw();
            }
            if (mScrollGain) {
                mGain = (int)((picCell.Height - pos.Y) * dbUnit + 0.5) / 10.0 + MinDB;
                limit();
                draw();
            }
        }

        private void limit() {
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

            mRatio = -mThreshold / (mGain - mThreshold);
            if (0 == mThreshold) {
                mRatio = 1;
            }

            if (mOutputGain < 0.0) {
                mOutputGain = 0.0;
            }
            if (-mGain < mOutputGain) {
                mOutputGain = -mGain;
            }
        }

        private void draw() {
            var dbUnit = AmpDispUnit * 2.0 / DbDispUnit;
            var pThresholdX = (int)((mThreshold - MinDB) * dbUnit);
            var pThresholdY = picCell.Height - pThresholdX;
            var pGainX = (int)((mGain - MinDB) * dbUnit);
            var pGainY = picCell.Height - pGainX;
            var pGainYT = pGainY - (int)(AmpDispUnit * (AmpDispCols - ZeroDbCol) / mRatio);
            var pOutputGain = (int)((-mOutputGain - MinDB) * dbUnit);
            var pOutputGainY = picCell.Height - pOutputGain;
            var pOutputGainYL = picRow.Height - pOutputGain;
            var pOutputOfs = AmpDispUnit - pOutputGain % AmpDispUnit;
            var mod = (AmpDispUnit * 2 - pOutputGain % (AmpDispUnit * 2) - 1) / AmpDispUnit;

            var psThresholdY = pThresholdY;
            var psGainY = pGainY;
            if (picCell.Height < psThresholdY + 20) {
                psThresholdY = picCell.Height - 20;
            }
            if (picCell.Height < psGainY + 20) {
                psGainY = picCell.Height - 20;
            }
            //
            // left frame
            //
            if (null != picRow.Image) {
                picRow.Image.Dispose();
                picRow.Image = null;
            }
            if (null != mBmpRowValue) {
                mBmpRowValue.Dispose();
                mBmpRowValue = null;
                mGRowValue.Dispose();
                mGRowValue = null;
            }
            mBmpRowValue = new Bitmap(picRow.Width, picRow.Height);
            mGRowValue = Graphics.FromImage(mBmpRowValue);
            //
            for (int row = 9, db = MinDB; 1 <= row; row -= 2, db += DbDispUnit) {
                mGRowValue.DrawLine(Colors.PTableBorder,
                    0, AmpDispUnit * (row + 1) + pOutputGainYL,
                    mBmpRow.Width, AmpDispUnit * (row + 1) + pOutputGainYL);
                mGRowValue.DrawString(db.ToString("0.0db"), Fonts.Small, Colors.BFontTable, new RectangleF(
                    -3, AmpDispUnit * row + pOutputGainYL,
                    mBmpRowValue.Width, AmpDispUnit), Fonts.AlignBR);
            }
            //
            mGRowValue.DrawString("0.0db", Fonts.Small, Colors.BFontTable, new RectangleF(
                -3, pOutputGainYL - AmpDispUnit,
                mBmpRowValue.Width, AmpDispUnit), Fonts.AlignBR);
            mGRowValue.DrawLine(Colors.PGraphLineRed, 0, pOutputGainYL, mBmpRowValue.Width, pOutputGainYL);
            picRow.Image = mBmpRowValue;
            //
            // table
            //
            if (null != picCell.Image) {
                picCell.Image.Dispose();
                picCell.Image = null;
            }
            if (null != mBmpCellValue) {
                mBmpCellValue.Dispose();
                mBmpCellValue = null;
                mGCellValue.Dispose();
                mGCellValue = null;
            }
            mBmpCellValue = new Bitmap(picCell.Width, picCell.Height);
            mGCellValue = Graphics.FromImage(mBmpCellValue);
            //
            for (var row = 1; row < AmpDispRows; row++) {
                mGCellValue.DrawLine(0 == ((row + mod) % 2) ? Colors.PTableBorder : Colors.PTableBorderDark,
                    0, AmpDispUnit * row + pOutputOfs,
                    mBmpCell.Width, AmpDispUnit * row + pOutputOfs);
            }
            for (var y = AmpDispUnit * 3 / 2 + pOutputOfs % AmpDispUnit; y < AmpDispHeight; y += AmpDispUnit) {
                for (var x = AmpDispUnit / 2; x < AmpDispWidth; x += AmpDispUnit) {
                    mBmpCellValue.SetPixel(x, y, Colors.TableBorder);
                }
            }
            //
            mGCellValue.DrawLine(Colors.PGraphLine, 0, picCell.Height, pThresholdX, pThresholdY);
            mGCellValue.DrawLine(Colors.PGraphLine, pThresholdX, pThresholdY, AmpDispUnit * ZeroDbCol, pGainY);
            mGCellValue.DrawLine(Colors.PGraphLineAlpha, AmpDispUnit * ZeroDbCol, pGainY, AmpDispWidth, pGainYT);
            //
            mGCellValue.DrawLine(Colors.PTableBorderLight, 0, pOutputGainY, mBmpCellValue.Width, pOutputGainY);
            mGCellValue.DrawLine(Colors.PGraphLineRed, pThresholdX, AmpDispUnit, pThresholdX, AmpDispHeight);
            mGCellValue.DrawLine(Colors.PGraphLineRed, AmpDispUnit * ZeroDbCol, pGainY, AmpDispWidth, pGainY);
            //
            mGCellValue.FillPie(Colors.BGraphPoint, pThresholdX - 4, pThresholdY - 4, 8, 8, 0, 360);
            mGCellValue.DrawArc(Colors.PTableBorderLight, pThresholdX - 4, pThresholdY - 4, 8, 8, 0, 360);
            mGCellValue.FillPie(Colors.BGraphPoint, AmpDispUnit * ZeroDbCol - 4, pGainY - 4, 8, 8, 0, 360);
            mGCellValue.DrawArc(Colors.PTableBorderLight, AmpDispUnit * ZeroDbCol - 4, pGainY - 4, 8, 8, 0, 360);
            //
            mGCellValue.DrawString(mRatio.ToString("0.0"), Fonts.Bold, Colors.BFontTable, new RectangleF(
                AmpDispUnit * ZeroDbCol, AmpDispUnit,
                AmpDispUnit * (AmpDispCols - ZeroDbCol), AmpDispUnit), Fonts.AlignMC);
            mGCellValue.DrawString((mThreshold + mOutputGain).ToString("0.0db"),
                Fonts.Small, Colors.BFontTable, pThresholdX + 3, psThresholdY);
            mGCellValue.DrawString(mThreshold.ToString("0.0db"),
                Fonts.Small, Colors.BFontTable, pThresholdX + 3, AmpDispUnit * 5 / 4);
            mGCellValue.DrawString((mGain + mOutputGain).ToString("0.0db"),
                Fonts.Small, Colors.BFontTable, AmpDispUnit * ZeroDbCol + 3, psGainY);

            picCell.Image = mBmpCellValue;
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
            mGRow.DrawString("Out", Fonts.Bold, Colors.BFontTable, new RectangleF(
                0, 0,
                TableLeftFrameWidth, AmpDispUnit), Fonts.AlignMC);
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
            for (var col = 1; col < AmpDispCols; col++) {
                mGCell.DrawLine(0 == (col % 2) ? Colors.PTableBorder : Colors.PTableBorderDark,
                    AmpDispUnit * col, 0,
                    AmpDispUnit * col, mBmpCell.Height);
            }
            mGCell.FillRectangle(Colors.BTableHeader,
                0, 0,
                mBmpCell.Width, AmpDispUnit);
            mGCell.DrawString("Threshold", Fonts.Bold, Colors.BFontTable, new RectangleF(
                0, 0,
                mBmpCell.Width - AmpDispUnit * (AmpDispCols - ZeroDbCol), AmpDispUnit), Fonts.AlignMC);
            mGCell.DrawString("Ratio", Fonts.Bold, Colors.BFontTable, new RectangleF(
                AmpDispUnit * ZeroDbCol, 0,
                AmpDispUnit * (AmpDispCols - ZeroDbCol), AmpDispUnit), Fonts.AlignMC);
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
            mGCol.DrawString("In", Fonts.Bold, Colors.BFontTable, new RectangleF(
                -8, 0,
                mBmpCol.Width, mBmpCol.Height), Fonts.AlignMR);
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
