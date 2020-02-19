using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Forms {
    public partial class Comp : Form {
        private const int TableFooterHeight = 30;
        private const int TableLeftFrameWidth = 50;
        private const int AmpDispCols = 12;
        private const int AmpDispRows = 12;
        private const int MinDB = -60;
        private const int ZeroDbCol = 10;
        private const int DbDispUnit = 12;
        private const double ValueBorderGrip = 2.0;

        private static int AmpDispUnit = 30;
        private static int AmpDispWidth = AmpDispUnit * AmpDispCols;
        private static int AmpDispHeight = AmpDispUnit * AmpDispRows;

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

        private Timer mTimer;
        private Timer mTimer2;
        private double theta = 0.0;

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
            mCommonCtrl = new CommonCtrl(this, true);
            mCommonCtrl.WindowStateChanged = new EventHandler(Comp_WindowStateChanged);
            mRatio = -mThreshold / (mGain - mThreshold);
            drawBackground();

            mPtrUvL = Marshal.AllocHGlobal(8);
            mPtrUvR = Marshal.AllocHGlobal(8);
            Marshal.StructureToPtr(0.0, mPtrUvL, true);
            Marshal.StructureToPtr(0.0, mPtrUvR, true);

            mTimer = new Timer();
            mTimer.Tick += new EventHandler((object s, EventArgs ev) => {
                draw();
            });
            mTimer.Enabled = true;
            mTimer.Interval = 1;
            mTimer.Start();

            mTimer2 = new Timer();
            mTimer2.Tick += new EventHandler((object s, EventArgs ev) => {
                Marshal.StructureToPtr(0.5 - 0.5 * Math.Cos(2.7*theta), mPtrUvL, true);
                Marshal.StructureToPtr(0.5 - 0.5 * Math.Sin(2.0*theta), mPtrUvR, true);
                theta += Math.PI * 2 / 100;
            });
            mTimer2.Enabled = true;
            mTimer2.Interval = 1;
            mTimer2.Start();
        }

        private void Comp_WindowStateChanged(object sender, EventArgs e) {
            if (FormWindowState.Maximized == WindowState) {
                var screen = Screen.FromControl(this);
                var sw = screen.Bounds.Width;
                var sh = screen.Bounds.Height;
                var w = (sw - picRow.Width) / AmpDispCols / 2 * 2;
                var h = (sh - mCommonCtrl.FormCtrlBottom - picFooter.Height) / AmpDispRows / 2 * 2;
                AmpDispUnit = Math.Min(w, h);
                AmpDispWidth = AmpDispUnit * AmpDispCols;
                AmpDispHeight = AmpDispUnit * AmpDispRows;
            } else {
                AmpDispUnit = 30;
                AmpDispWidth = AmpDispUnit * AmpDispCols;
                AmpDispHeight = AmpDispUnit * AmpDispRows;
            }
            drawBackground();
        }

        private void picRow_MouseDown(object sender, MouseEventArgs e) {
            var pos = picCell.PointToClient(Cursor.Position);
            var outputGain = -posToDb(picRow.Height - pos.Y);
            if (mOutputGain - ValueBorderGrip < outputGain && outputGain < mOutputGain + ValueBorderGrip) {
                mScrollOutputGain = true;
                Cursor.Current = Cursors.HSplit;
            }
        }

        private void picRow_MouseUp(object sender, MouseEventArgs e) {
            mScrollOutputGain = false;
        }

        private void picRow_MouseMove(object sender, MouseEventArgs e) {
            var pos = picRow.PointToClient(Cursor.Position);
            if (mScrollOutputGain) {
                mOutputGain = -posToDb(picRow.Height - pos.Y);
                limit();
                return;
            }
            var outputGain = -posToDb(picRow.Height - pos.Y);
            if (mOutputGain - ValueBorderGrip < outputGain && outputGain < mOutputGain + ValueBorderGrip) {
                Cursor.Current = Cursors.HSplit;
            }
        }

        private void picCell_MouseDown(object sender, MouseEventArgs e) {
            var pos = picCell.PointToClient(Cursor.Position);
            var threshold = posToDb(pos.X);
            var gain = posToDb(picCell.Height - pos.Y);
            if (0 <= pos.X && pos.X < AmpDispUnit * ZeroDbCol &&
                mThreshold - ValueBorderGrip < threshold && threshold < mThreshold + ValueBorderGrip
            ) {
                mScrollThreshold = true;
                Cursor.Current = Cursors.VSplit;
            }
            if (AmpDispUnit * ZeroDbCol <= pos.X && pos.X < AmpDispWidth &&
                mGain - ValueBorderGrip < gain && gain < mGain + ValueBorderGrip
            ) {
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
            if (mScrollThreshold) {
                mThreshold = posToDb(pos.X);
                limit();
                return;
            }
            if (mScrollGain) {
                mGain = posToDb(picCell.Height - pos.Y);
                limit();
                return;
            }
            var threshold = posToDb(pos.X);
            var gain = posToDb(picCell.Height - pos.Y);
            if (0 <= pos.X && pos.X < AmpDispUnit * ZeroDbCol &&
                mThreshold - ValueBorderGrip < threshold && threshold < mThreshold + ValueBorderGrip
            ) {
                Cursor.Current = Cursors.VSplit;
            }
            if (AmpDispUnit * ZeroDbCol <= pos.X && pos.X < AmpDispWidth &&
                mGain - ValueBorderGrip < gain && gain < mGain + ValueBorderGrip
            ) {
                Cursor.Current = Cursors.HSplit;
            }
        }

        private double posToDb(int pos) {
            return (int)(pos * DbDispUnit * 2.0 / AmpDispUnit + 0.5) / 4.0 + MinDB;
        }

        private double dbToPos(double db) {
            return (db - MinDB) * AmpDispUnit * 2.0 / DbDispUnit;
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
            if (-mThreshold < mOutputGain) {
                mOutputGain = -mThreshold;
            }
        }

        private void draw() {
            var pThresholdX = (int)dbToPos(mThreshold);
            var pThresholdY = AmpDispHeight - pThresholdX;
            var pGain = (int)dbToPos(mGain);
            var pGainY = AmpDispHeight - pGain;
            var pGainYR = pGainY - (int)(AmpDispUnit * (AmpDispCols - ZeroDbCol) / mRatio);
            var pOutputGainX = (int)dbToPos(-mOutputGain);
            var pOutputGainY = AmpDispHeight - pOutputGainX;
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
            mBmpRowValue = new Bitmap(TableLeftFrameWidth, AmpDispHeight);
            mGRowValue = Graphics.FromImage(mBmpRowValue);
            //
            for (int row = 0, db = 0; row < AmpDispRows; row += 2, db -= DbDispUnit) {
                mGRowValue.DrawLine(Colors.PTableBorder,
                    0, AmpDispUnit * row + pOutputGainY,
                    mBmpRow.Width, AmpDispUnit * row + pOutputGainY);
                mGRowValue.DrawString(db.ToString("0.0db"), Fonts.Small, Colors.BFontTable, new RectangleF(
                    -3, AmpDispUnit * (row - 1) + pOutputGainY,
                    mBmpRowValue.Width, AmpDispUnit), Fonts.AlignBR);
            }
            //
            mGRowValue.DrawString("0.0db", Fonts.Small, Colors.BFontTable, new RectangleF(
                -3, pOutputGainY - AmpDispUnit,
                mBmpRowValue.Width, AmpDispUnit), Fonts.AlignBR);
            mGRowValue.DrawLine(Colors.PGraphBoundaryValue, 0, pOutputGainY, mBmpRowValue.Width, pOutputGainY);
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
            mBmpCellValue = new Bitmap(AmpDispWidth, AmpDispHeight);
            mGCellValue = Graphics.FromImage(mBmpCellValue);
            //
            for (var row = 1; row < AmpDispRows; row++) {
                mGCellValue.DrawLine(0 == (row % 2) ? Colors.PTableBorder : Colors.PTableBorderDark,
                    0, AmpDispUnit * row + pOutputGainY,
                    mBmpCell.Width, AmpDispUnit * row + pOutputGainY);
            }
            for (var y = AmpDispUnit / 2 + pOutputGainY; y < AmpDispHeight; y += AmpDispUnit) {
                for (var x = AmpDispUnit / 2; x < AmpDispWidth; x += AmpDispUnit) {
                    mBmpCellValue.SetPixel(x, y, Colors.TableBorder);
                }
            }
            //
            mGCellValue.SmoothingMode = SmoothingMode.AntiAlias;
            mGCellValue.DrawLine(Colors.PGraphLine, 0, AmpDispHeight, pThresholdX, pThresholdY);
            mGCellValue.DrawLine(Colors.PGraphLine, pThresholdX, pThresholdY, AmpDispUnit * ZeroDbCol, pGainY);
            mGCellValue.DrawLine(Colors.PGraphLineAlpha, AmpDispUnit * ZeroDbCol, pGainY, AmpDispWidth, pGainYR);
            //
            mGCellValue.DrawLine(Colors.PTableBorderLight, 0, pOutputGainY, mBmpCellValue.Width, pOutputGainY);
            mGCellValue.DrawLine(Colors.PGraphBoundaryValue, pThresholdX, AmpDispUnit, pThresholdX, AmpDispHeight);
            mGCellValue.DrawLine(Colors.PGraphBoundaryValue, AmpDispUnit * ZeroDbCol, pGainY, AmpDispWidth, pGainY);
            //
            var uvL = Marshal.PtrToStructure<double>(mPtrUvL);
            var uvR = Marshal.PtrToStructure<double>(mPtrUvR);
            var uvDb = Math.Max(uvL, uvR);
            if (uvDb < Math.Pow(10.0, MinDB / 20.0)) {
                uvDb = Math.Pow(10.0, MinDB / 20.0);
            }
            uvDb = Math.Log10(uvDb) * 20;
            var pUv = (float)dbToPos(uvDb);
            var pUvRa = (float)dbToPos(mThreshold + (uvDb - mThreshold) / mRatio);
            var pThX = (float)dbToPos(mThreshold);
            var pThY = AmpDispHeight - pThX;
            if (uvDb < mThreshold) {
                mGCellValue.DrawLine(Colors.PGraphLineGreen, 0, AmpDispHeight, pUv, AmpDispHeight - pUv);
            } else if (pUvRa <= pOutputGainX) {
                mGCellValue.DrawLine(Colors.PGraphLineGreen, 0, AmpDispHeight, pThX, pThY);
                mGCellValue.DrawLine(Colors.PGraphLineYellow, pThX, pThY, pUv, AmpDispHeight - pUvRa);
            } else {
                mGCellValue.DrawLine(Colors.PGraphLineGreen, 0, AmpDispHeight, pThX, pThY);
                mGCellValue.DrawLine(Colors.PGraphLineRed, pThX, pThY, pUv, AmpDispHeight - pUvRa);
            }
            mGCellValue.SmoothingMode = SmoothingMode.None;
            //
            mGCellValue.FillPie(Colors.BGraphPoint, pThresholdX - 4, pThresholdY - 4, 8, 8, 0, 360);
            mGCellValue.DrawArc(Colors.PTableBorderLight, pThresholdX - 4, pThresholdY - 4, 8, 8, 0, 360);
            mGCellValue.FillPie(Colors.BGraphPoint, AmpDispUnit * ZeroDbCol - 4, pGainY - 4, 8, 8, 0, 360);
            mGCellValue.DrawArc(Colors.PTableBorderLight, AmpDispUnit * ZeroDbCol - 4, pGainY - 4, 8, 8, 0, 360);
            //
            var psThresholdY = pThresholdY;
            if (AmpDispHeight < psThresholdY + 20) {
                psThresholdY = AmpDispHeight - 20;
            }
            var psGainY = pGainY;
            if (AmpDispHeight < psGainY + 20) {
                psGainY = AmpDispHeight - 20;
            }
            mGCellValue.DrawString(mThreshold.ToString("0.00db"), Fonts.Bold, Colors.BFontTable, new RectangleF(
                0, AmpDispUnit,
                AmpDispUnit * ZeroDbCol, AmpDispUnit), Fonts.AlignMC);
            mGCellValue.DrawString(mRatio.ToString("0.0"), Fonts.Bold, Colors.BFontTable, new RectangleF(
                AmpDispUnit * ZeroDbCol, AmpDispUnit,
                AmpDispUnit * (AmpDispCols - ZeroDbCol), AmpDispUnit), Fonts.AlignMC);
            mGCellValue.DrawString((mThreshold + mOutputGain).ToString("0.00db"),
                Fonts.Small, Colors.BFontTable, pThresholdX + 3, psThresholdY);
            mGCellValue.DrawString((mGain + mOutputGain).ToString("0.00db"),
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
            mGRow.DrawString("Gain", Fonts.Bold, Colors.BFontTable, new RectangleF(
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
                    AmpDispUnit * col, AmpDispUnit * 2,
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
            mGCell.DrawLine(Colors.PTableBorderBold,
                0, AmpDispUnit * 2,
                mBmpCell.Width, AmpDispUnit * 2);
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
                mGCol.DrawString(db.ToString("0db"), Fonts.Small, Colors.BFontTable, new RectangleF(
                    AmpDispUnit * col, 0,
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
