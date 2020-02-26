using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Forms {
    public partial class EQ : Form {
        private struct Band {
            public double Freq;
            public double Gain;
            public double Width;
            public bool Enable;
        }

        private const int TableFooterHeight = 30;
        private const int TableLeftFrameWidth = 50;
        private const int FreqDispCols = 10;
        private const int AmpDispRows = 14;
        private const int MinDB = -36;
        private const int DbDispUnit = 12;
        
        private static int FreqDispUnit;
        private static int FreqDispWidth;
        private static int AmpDispUnit;
        private static int AmpDispHeight;
        private static int ZeroDbPos;

        private Band[] mBands = new Band[10];
        private int mSelectedBand = 0;

        private bool mScrollFreq = false;
        private bool mScrollGain = false;
        private bool mScrollWidth = false;

        private Timer mTimer;
        private CommonCtrl mCommonCtrl;

        private Bitmap mBmpRow;
        private Bitmap mBmpCol;
        private Bitmap mBmpCell;
        private Bitmap mBmpCellValue;
        private Graphics mGRow;
        private Graphics mGCol;
        private Graphics mGCell;
        private Graphics mGCellValue;

        public EQ() {
            InitializeComponent();
        }

        private void EQ_Load(object sender, EventArgs e) {
            mCommonCtrl = new CommonCtrl(this, true, false);
            mCommonCtrl.WindowStateChanged = new EventHandler(EQ_WindowStateChanged);

            for (var b = 0; b < mBands.Length; b++) {
                mBands[b].Freq = 200 * (b + 1);
                mBands[b].Gain = 6;
                mBands[b].Width = 1.0;
                mBands[b].Enable = false;
            }

            EQ_WindowStateChanged(sender, e);

            mTimer = new Timer();
            mTimer.Tick += new EventHandler((object s, EventArgs ev) => {
                draw();
            });
            mTimer.Enabled = true;
            mTimer.Interval = 1;
            mTimer.Start();
        }

        private void EQ_WindowStateChanged(object sender, EventArgs e) {
            if (FormWindowState.Maximized == WindowState) {
                var screen = Screen.FromControl(this);
                var sw = screen.Bounds.Width;
                var sh = screen.Bounds.Height;
                var wf = (sw - picRow.Width) / 12;
                var ha = (sh - mCommonCtrl.FormCtrlBottom - picFooter.Height) / AmpDispRows / 2 * 2;
                FreqDispUnit = wf;
                var ofs10kTo20k = (int)((Math.Log10(20000) - Math.Log10(10000)) * 4 * FreqDispUnit);
                FreqDispWidth = FreqDispUnit * FreqDispCols + ofs10kTo20k + 2;
                AmpDispUnit = ha;
                AmpDispHeight = AmpDispUnit * AmpDispRows;
                ZeroDbPos = AmpDispUnit * (AmpDispRows / 2 - 1);
            } else {
                FreqDispUnit = 50;
                var ofs10kTo20k = (int)((Math.Log10(20000) - Math.Log10(10000)) * 4 * FreqDispUnit);
                FreqDispWidth = FreqDispUnit * FreqDispCols + ofs10kTo20k + 2;
                AmpDispUnit = 30;
                AmpDispHeight = AmpDispUnit * AmpDispRows;
                ZeroDbPos = AmpDispUnit * (AmpDispRows / 2 - 1);
            }
            drawBackground();
        }

        private void picCell_MouseDown(object sender, MouseEventArgs e) {
            var pos = picCell.PointToClient(Cursor.Position);
            if (0 <= pos.Y && pos.Y <= AmpDispUnit) {
                var col = pos.X / FreqDispUnit;
                if (1 <= col && col <= 10) {
                    mSelectedBand = col - 1;
                }
                return;
            }
            if (AmpDispUnit <= pos.Y && pos.Y <= AmpDispUnit * 2) {
                mScrollWidth = true;
                Cursor.Current = Cursors.VSplit;
                return;
            }
            var band = mBands[mSelectedBand];
            var pFreq = posToFreq(pos.X);
            var pGain = posToGain(pos.Y);
            if (band.Freq * 0.9 <= pFreq && pFreq <= band.Freq * 1.1) {
                mScrollFreq = true;
                Cursor.Current = Cursors.VSplit;
                return;
            }
            if (band.Gain - 3 <= pGain && pGain <= band.Gain + 3) {
                mScrollGain = true;
                Cursor.Current = Cursors.HSplit;
                return;
            }
        }

        private void picCell_MouseUp(object sender, MouseEventArgs e) {
            mScrollFreq = false;
            mScrollGain = false;
            mScrollWidth = false;
        }

        private void picCell_MouseMove(object sender, MouseEventArgs e) {
            var pos = picCell.PointToClient(Cursor.Position);
            if (mScrollFreq) {
                mBands[mSelectedBand].Freq = posToFreq(pos.X);
            }
            if (mScrollGain) {
                mBands[mSelectedBand].Gain = posToGain(pos.Y);
            }
            if(mScrollWidth) {
                mBands[mSelectedBand].Width = posToWidth(pos.X);
            }
        }

        private void picCell_DoubleClick(object sender, EventArgs e) {
            var pos = picCell.PointToClient(Cursor.Position);
            if (0 <= pos.Y && pos.Y <= AmpDispUnit) {
                var col = pos.X / FreqDispUnit;
                if (1 <= col && col <= 10) {
                    if (mBands[col - 1].Enable) {
                        mBands[col - 1].Enable = false;
                    } else {
                        mBands[col - 1].Enable = true;
                    }
                }
            }
        }

        private int gainToPos(double db) {
            return mBmpCellValue.Height - ZeroDbPos - (int)(db * AmpDispUnit / 6.0);
        }

        private int freqToPos(double freq) {
            return (int)(Math.Log10(freq) * FreqDispUnit * 4 - FreqDispUnit * 4 * 1.5);
        }

        private int widthToPos(double width) {
            return (int)(mBmpCol.Width / 2.0 + mBmpCol.Width * Math.Log10(width) / 4.0);
        }

        private double posToGain(int pos) {
            return (mBmpCell.Height - pos) * 6.0 / AmpDispUnit + MinDB;
        }

        private double posToFreq(int pos) {
            return Math.Pow(10.0, (pos + FreqDispUnit * 6) * 0.25 / FreqDispUnit);
        }

        private double posToWidth(int pos) {
            return Math.Pow(10.0, 4.0 * pos / mBmpCol.Width - 2.0);
        }

        private void draw() {
            if (mBands[mSelectedBand].Freq < 31.6) {
                mBands[mSelectedBand].Freq = 31.6;
            }
            if (20000 < mBands[mSelectedBand].Freq) {
                mBands[mSelectedBand].Freq = 20000;
            }
            if (mBands[mSelectedBand].Gain < MinDB) {
                mBands[mSelectedBand].Gain = MinDB;
            }
            if (-MinDB < mBands[mSelectedBand].Gain) {
                mBands[mSelectedBand].Gain = -MinDB;
            }
            if (mBands[mSelectedBand].Width < 0.01) {
                mBands[mSelectedBand].Width = 0.01;
            }
            if (100.0 < mBands[mSelectedBand].Width) {
                mBands[mSelectedBand].Width = 100.0;
            }
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
            mBmpCellValue = new Bitmap(FreqDispWidth, AmpDispHeight);
            mGCellValue = Graphics.FromImage(mBmpCellValue);
            //
            for (var col = 1; col <= mBands.Length; col++) {
                mGCellValue.FillRectangle((col - 1) == mSelectedBand ? Colors.BTableHeader : Colors.BTableCell,
                    FreqDispUnit * col + 1, 2, FreqDispUnit - 1, AmpDispUnit - 2);
                mGCellValue.DrawString(string.Format("F{0}", col),
                    Fonts.Bold,
                    mBands[col - 1].Enable ? Colors.BFontTable : Colors.BFontTabButtonDisable,
                    new RectangleF(FreqDispUnit * col, 0, FreqDispUnit, AmpDispUnit),
                    Fonts.AlignMC);
            }
            //
            mGCellValue.SmoothingMode = SmoothingMode.AntiAlias;
            var ay = ZeroDbPos + AmpDispUnit * 2;
            for (var px = 0; px < mBmpCellValue.Width; px++) {
                var db = 0.0;
                foreach(var b in mBands) {
                    if (!b.Enable) {
                        continue;
                    }
                    var w = 2.0 * Math.PI / (b.Width * mBmpCellValue.Width);
                    var wt = w * (b.Freq - posToFreq(px));
                    db += Math.Exp(-wt * wt) * b.Gain;
                }
                var py = gainToPos(db);
                mGCellValue.DrawLine(Colors.PGraphLine, px - 1, ay, px, py);
                ay = py;
            }
            //
            var band = mBands[mSelectedBand];
            var omega = 2.0 * Math.PI / (band.Width * mBmpCellValue.Width);
            var by = ZeroDbPos + AmpDispUnit * 2;
            for (var px = 0; px < mBmpCellValue.Width; px++) {
                var wt = omega * (band.Freq - posToFreq(px));
                var py = gainToPos(Math.Exp(-wt * wt) * band.Gain);
                mGCellValue.DrawLine(Colors.PGraphLineAlpha, px - 1, by, px, py);
                by = py;
            }
            mGCellValue.SmoothingMode = SmoothingMode.None;
            //
            var pGain = gainToPos(band.Gain);
            var pFreq = freqToPos(band.Freq);
            var pWidth = widthToPos(band.Width);
            var psGain = pGain - 17;
            if (psGain < 60) {
                psGain = 60;
            }
            var psFreq = pFreq;
            if (mBmpCell.Width - 50 < psFreq) {
                psFreq = mBmpCell.Width - 50;
            }
            mGCellValue.DrawLine(Colors.PGraphBoundaryValue, 0, pGain, mBmpCell.Width, pGain);
            mGCellValue.DrawLine(Colors.PGraphAuxiliary, pFreq, AmpDispUnit * 2, pFreq, mBmpCell.Height);
            mGCellValue.DrawLine(Colors.PGraphBoundaryValue, pWidth, AmpDispUnit * 2, pWidth, AmpDispUnit);
            mGCellValue.DrawString(band.Gain.ToString("0.0db"),
                Fonts.Small, Colors.BFontTable,
                0, psGain);
            mGCellValue.DrawString(band.Freq.ToString("0Hz"),
                Fonts.Small, Colors.BFontTable,
                psFreq, AmpDispHeight - AmpDispUnit / 2);

            picCell.Image = mBmpCellValue;
        }

        private void drawBackground() {
            releaseImage();

            mBmpCol = new Bitmap(FreqDispWidth, TableFooterHeight);
            mGCol = Graphics.FromImage(mBmpCol);
            mBmpRow = new Bitmap(TableLeftFrameWidth, AmpDispHeight);
            mGRow = Graphics.FromImage(mBmpRow);
            mBmpCell = new Bitmap(FreqDispWidth, AmpDispHeight);
            mGCell = Graphics.FromImage(mBmpCell);
            //
            // left frame
            //
            mGRow.Clear(Colors.TableHeader);
            for (int row = AmpDispRows, db = MinDB; 2 <= row; row -= 2, db += DbDispUnit) {
                mGRow.DrawLine(Colors.PTableBorder,
                    0, AmpDispUnit * row,
                    mBmpRow.Width, AmpDispUnit * row);
                mGRow.DrawString(db.ToString("0db"), Fonts.Small, Colors.BFontTable,
                    new RectangleF(-5, AmpDispUnit * (row - 1),
                    mBmpRow.Width, AmpDispUnit), Fonts.AlignBR);
            }
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
            for (var row = 1; row < AmpDispRows; row++) {
                mGCell.DrawLine(0 == (row % 2) ? Colors.PTableBorder : Colors.PTableBorderDark,
                    0, AmpDispUnit * row,
                    mBmpCell.Width, AmpDispUnit * row);
            }
            for (var col = 0; col < 12; col++) {
                mGCell.DrawLine(0 == (col % 2) ? Colors.PTableBorder : Colors.PTableBorderDark,
                    FreqDispUnit * col, 0,
                    FreqDispUnit * col, mBmpCell.Height);
            }
            mGCell.FillRectangle(Colors.BTableCell,
                1, 0,
                mBmpCell.Width, AmpDispUnit * 2);
            mGCell.DrawLine(Colors.PTableBorderBold,
                0, AmpDispUnit * 2,
                mBmpCell.Width, AmpDispUnit * 2);
            //
            // footer
            //
            mGCol.Clear(Colors.TableHeader);
            for (var col = 0; col < 12; col += 2) {
                mGCol.DrawLine(Colors.PTableBorder,
                    FreqDispUnit * col, 0,
                    FreqDispUnit * col, mBmpCol.Height);
            }
            //
            mGCol.DrawString("32Hz", Fonts.Small, Colors.BFontTable, new RectangleF(
                0, 0,
                FreqDispUnit * 2, mBmpCol.Height), Fonts.AlignML);
            mGCol.DrawString("100Hz", Fonts.Small, Colors.BFontTable, new RectangleF(
                FreqDispUnit * 2, 0,
                FreqDispUnit * 4, mBmpCol.Height), Fonts.AlignML);
            mGCol.DrawString("316Hz", Fonts.Small, Colors.BFontTable, new RectangleF(
                FreqDispUnit * 4, 0,
                FreqDispUnit * 6, mBmpCol.Height), Fonts.AlignML);
            mGCol.DrawString("1kHz", Fonts.Small, Colors.BFontTable, new RectangleF(
                FreqDispUnit * 6, 0,
                FreqDispUnit * 8, mBmpCol.Height), Fonts.AlignML);
            mGCol.DrawString("3.16kHz", Fonts.Small, Colors.BFontTable, new RectangleF(
                FreqDispUnit * 8, 0,
                FreqDispUnit * 10, mBmpCol.Height), Fonts.AlignML);
            mGCol.DrawString("10kHz", Fonts.Small, Colors.BFontTable, new RectangleF(
                FreqDispUnit * 10, 0,
                FreqDispUnit * 12, mBmpCol.Height), Fonts.AlignML);

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
