using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Forms {
    public partial class Envelope : Form {
        private const int TableColumnWidth = 74;
        private const int TableHeaderHeight = 30;
        private const int TableLeftFrameWidth = 65;
        private const int PitchDispUnit = 100;
        private const int CutoffDispUnit = 32;
        private const int AmpDispUnit = 30;

        private class Values {
            private PictureBox mPicTime;
            private PictureBox mPicValue;
            private Bitmap mBmpTime;
            private Bitmap mBmpValue;
            private Graphics mGTime;
            private Graphics mGValue;

            public int Attack { get; private set; }
            public int Hold { get; private set; }
            public int Decay { get; private set; }
            public int Release { get; private set; }
            public int Range { get; private set; }

            public int DAttack;
            public int DHold;
            public int DDecay;
            public int DRelease;
            public int DRange;

            public double Rise;
            public double Top;
            public double Sustain;
            public double Fall;

            public Values(PictureBox picTime, PictureBox picValue) {
                mPicTime = picTime;
                mPicValue = picValue;
            }

            public void Commit() {
                Attack += DAttack;
                Hold += DHold;
                Decay += DDecay;
                Release += DRelease;
                Range += DRange;
                DAttack = 0;
                DHold = 0;
                DDecay = 0;
                DRelease = 0;
                DRange = 0;
            }

            public void DrawTime() {
                limit();

                var attack = Attack + DAttack;
                var hold = Hold + DHold;
                var decay = Decay + DDecay;
                var release = Release + DRelease;

                releaseImageTime();

                mGTime.Clear(Color.Transparent);
                mGTime.DrawString(attack.ToString("0ms"),
                    Fonts.Bold,
                    Colors.BFontTable,
                    new RectangleF(0, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    Fonts.AlignMC);
                mGTime.DrawString(hold.ToString("0ms"),
                    Fonts.Bold,
                    Colors.BFontTable,
                    new RectangleF(TableColumnWidth, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    Fonts.AlignMC);
                mGTime.DrawString(decay.ToString("0ms"),
                    Fonts.Bold,
                    Colors.BFontTable,
                    new RectangleF(TableColumnWidth * 2, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    Fonts.AlignMC);
                mGTime.DrawString(release.ToString("0ms"),
                    Fonts.Bold,
                    Colors.BFontTable,
                    new RectangleF(TableColumnWidth * 4, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    Fonts.AlignMC);
                mPicTime.Image = mBmpTime;
            }

            public void DrawTimePitch() {
                limit();

                var attack = Attack + DAttack;
                var hold = Hold + DHold;
                var decay = Decay + DDecay;
                var release = Release + DRelease;
                var range = Range + DRange;

                var maxPitch = Math.Max(Math.Abs(Rise), Math.Max(Math.Abs(Top), Math.Abs(Fall)));
                if (range < maxPitch) {
                    range = (int)(maxPitch / 100.0 + 0.99) * 100;
                    Range = range;
                    DRange = 0;
                }

                DrawValuePitch();

                releaseImageTime();

                mGTime.Clear(Color.Transparent);
                mGTime.DrawString(attack.ToString("0ms"),
                    Fonts.Bold,
                    Colors.BFontTable,
                    new RectangleF(0, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    Fonts.AlignMC);
                mGTime.DrawString(hold.ToString("0ms"),
                    Fonts.Bold,
                    Colors.BFontTable,
                    new RectangleF(TableColumnWidth, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    Fonts.AlignMC);
                mGTime.DrawString(decay.ToString("0ms"),
                    Fonts.Bold,
                    Colors.BFontTable,
                    new RectangleF(TableColumnWidth * 2, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    Fonts.AlignMC);
                mGTime.DrawString(release.ToString("0ms"),
                    Fonts.Bold,
                    Colors.BFontTable,
                    new RectangleF(TableColumnWidth * 3, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    Fonts.AlignMC);
                mGTime.DrawString(range.ToString("0cent"),
                    Fonts.Bold,
                    Colors.BFontTable,
                    new RectangleF(TableColumnWidth * 4, TableHeaderHeight, TableColumnWidth, TableHeaderHeight),
                    Fonts.AlignMC);
                mPicTime.Image = mBmpTime;
            }

            public void DrawValuePitch(int moveCol = -1) {
                var range = Range + DRange;

                if (range < 100) {
                    range = 100;
                }

                if (Rise < -range) {
                    Rise = -range;
                }
                if (range < Rise) {
                    Rise = range;
                }
                if (Top < -range) {
                    Top = -range;
                }
                if (range < Top) {
                    Top = range;
                }
                if (Fall < -range) {
                    Fall = -range;
                }
                if (range < Fall) {
                    Fall = range;
                }

                var pOfs = PitchDispUnit * 2 - 1;
                var pRise = pOfs - (int)(Rise * PitchDispUnit * 2 / range);
                var pTop = pOfs - (int)(Top * PitchDispUnit * 2 / range);
                var pSustain = pOfs;
                var pFall = pOfs - (int)(Fall * PitchDispUnit * 2 / range);
                var psRise = pRise;
                var psTop = pTop;
                var psFall = pFall;

                if (mPicValue.Height < psRise + 20) {
                    psRise = mPicValue.Height - 20;
                }
                if (mPicValue.Height < psTop + 20) {
                    psTop = mPicValue.Height - 20;
                }
                if (mPicValue.Height < psFall + 20) {
                    psFall = mPicValue.Height - 20;
                }

                releaseImageValue();

                mGValue.DrawLine(Colors.PGraphLineAlpha, 0, pRise, TableColumnWidth, pTop);
                mGValue.DrawLine(Colors.PGraphLineAlpha, TableColumnWidth, pTop, TableColumnWidth * 2, pTop);
                mGValue.DrawLine(Colors.PGraphLineAlpha, TableColumnWidth * 2, pTop, TableColumnWidth * 3, pSustain);
                mGValue.DrawLine(Colors.PGraphLineAlpha, TableColumnWidth * 3, pSustain, TableColumnWidth * 4, pFall);

                switch (moveCol) {
                case 0:
                    mGValue.DrawLine(Colors.PGraphLineRed, 0, pRise, TableColumnWidth * 4, pRise);
                    break;
                case 1:
                    mGValue.DrawLine(Colors.PGraphLineRed, 0, pTop, TableColumnWidth * 4, pTop);
                    break;
                case 3:
                    mGValue.DrawLine(Colors.PGraphLineRed, 0, pFall, TableColumnWidth * 4, pFall);
                    break;
                }

                mGValue.FillPie(Colors.BGraphPoint, -4, pRise - 4, 8, 8, 0, 360);
                mGValue.FillPie(Colors.BGraphPoint, TableColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
                mGValue.FillPie(Colors.BGraphPoint, TableColumnWidth * 4 - 4, pFall - 4, 8, 8, 0, 360);
                mGValue.DrawArc(Colors.PTableBorderLight, -4, pRise - 4, 8, 8, 0, 360);
                mGValue.DrawArc(Colors.PTableBorderLight, TableColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
                mGValue.DrawArc(Colors.PTableBorderLight, TableColumnWidth * 4 - 4, pFall - 4, 8, 8, 0, 360);

                mGValue.DrawString(Rise.ToString("0cent"), Fonts.Small, Colors.BFontTable, 3, psRise);
                mGValue.DrawString(Top.ToString("0cent"), Fonts.Small, Colors.BFontTable, TableColumnWidth + 3, psTop);
                mGValue.DrawString(Fall.ToString("0cent"), Fonts.Small, Colors.BFontTable, TableColumnWidth * 4 - 3, psFall, Fonts.AlignTR);

                mPicValue.Image = mBmpValue;
            }

            public void DrawValueCutoff(int moveCol = -1) {
                if (Rise < 32) {
                    Rise = 32;
                }
                if (20000 < Rise) {
                    Rise = 20000;
                }

                if (Top < 32) {
                    Top = 32;
                }
                if (20000 < Top) {
                    Top = 20000;
                }

                if (Sustain < 32) {
                    Sustain = 32;
                }
                if (20000 < Sustain) {
                    Sustain = 20000;
                }

                if (Fall < 32) {
                    Fall = 32;
                }
                if (20000 < Fall) {
                    Fall = 20000;
                }

                var pOfs = mPicValue.Height + (int)(CutoffDispUnit * 4 * 1.5);
                var pRise = pOfs - (int)(Math.Log10(Rise) * CutoffDispUnit * 4);
                var pTop = pOfs - (int)(Math.Log10(Top) * CutoffDispUnit * 4);
                var pSustain = pOfs - (int)(Math.Log10(Sustain) * CutoffDispUnit * 4);
                var pFall = pOfs - (int)(Math.Log10(Fall) * CutoffDispUnit * 4);
                var psRise = pRise;
                var psTop = pTop;
                var psSustain = pSustain;
                var psFall = pFall;

                if (mPicValue.Height < psRise + 20) {
                    psRise = mPicValue.Height - 20;
                }
                if (mPicValue.Height < psTop + 20) {
                    psTop = mPicValue.Height - 20;
                }
                if (mPicValue.Height < psSustain + 20) {
                    psSustain = mPicValue.Height - 20;
                }
                if (mPicValue.Height < psFall + 20) {
                    psFall = mPicValue.Height - 20;
                }

                releaseImageValue();

                mGValue.DrawLine(Colors.PGraphLineAlpha, 0, pRise, TableColumnWidth, pTop);
                mGValue.DrawLine(Colors.PGraphLineAlpha, TableColumnWidth, pTop, TableColumnWidth * 2, pTop);
                mGValue.DrawLine(Colors.PGraphLineAlpha, TableColumnWidth * 2, pTop, TableColumnWidth * 3, pSustain);
                mGValue.DrawLine(Colors.PGraphLineAlpha, TableColumnWidth * 3, pSustain, TableColumnWidth * 4, pSustain);
                mGValue.DrawLine(Colors.PGraphLineAlpha, TableColumnWidth * 4, pSustain, TableColumnWidth * 5, pFall);

                switch (moveCol) {
                case 0:
                    mGValue.DrawLine(Colors.PGraphLineRed, 0, pRise, TableColumnWidth * 5, pRise);
                    break;
                case 1:
                    mGValue.DrawLine(Colors.PGraphLineRed, 0, pTop, TableColumnWidth * 5, pTop);
                    break;
                case 3:
                    mGValue.DrawLine(Colors.PGraphLineRed, 0, pSustain, TableColumnWidth * 5, pSustain);
                    break;
                case 4:
                    mGValue.DrawLine(Colors.PGraphLineRed, 0, pFall, TableColumnWidth * 5, pFall);
                    break;
                }

                mGValue.FillPie(Colors.BGraphPoint, -4, pRise - 4, 8, 8, 0, 360);
                mGValue.FillPie(Colors.BGraphPoint, TableColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
                mGValue.FillPie(Colors.BGraphPoint, TableColumnWidth * 3 - 4, pSustain - 4, 8, 8, 0, 360);
                mGValue.FillPie(Colors.BGraphPoint, TableColumnWidth * 5 - 5, pFall - 4, 8, 8, 0, 360);
                mGValue.DrawArc(Colors.PTableBorderLight, -4, pRise - 4, 8, 8, 0, 360);
                mGValue.DrawArc(Colors.PTableBorderLight, TableColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
                mGValue.DrawArc(Colors.PTableBorderLight, TableColumnWidth * 3 - 4, pSustain - 4, 8, 8, 0, 360);
                mGValue.DrawArc(Colors.PTableBorderLight, TableColumnWidth * 5 - 5, pFall - 4, 8, 8, 0, 360);

                mGValue.DrawString(Rise.ToString("0Hz"), Fonts.Small, Colors.BFontTable, 3, psRise);
                mGValue.DrawString(Top.ToString("0Hz"), Fonts.Small, Colors.BFontTable, TableColumnWidth + 3, psTop);
                mGValue.DrawString(Sustain.ToString("0Hz"), Fonts.Small, Colors.BFontTable, TableColumnWidth * 3 + 3, psSustain);
                mGValue.DrawString(Fall.ToString("0Hz"), Fonts.Small, Colors.BFontTable, TableColumnWidth * 5 - 3, psFall, Fonts.AlignTR);

                mPicValue.Image = mBmpValue;
            }

            public void DrawValueAmp(int moveCol = -1) {
                if (Top < 1 / 1024.0) {
                    Top = 1 / 1024.0;
                }
                if (1.0 < Top) {
                    Top = 1.0;
                }
                if (Sustain < 1 / 1024.0) {
                    Sustain = 1 / 1024.0;
                }
                if (1.0 < Sustain) {
                    Sustain = 1.0;
                }

                var dbTop = 20 * Math.Log10(Top);
                var dbSustain = 20 * Math.Log10(Sustain);

                var pOfs = mPicValue.Height - 1;
                var pRise = pOfs;
                var pTop = AmpDispUnit - (int)(dbTop * AmpDispUnit / 6);
                var pSustain = AmpDispUnit - (int)(dbSustain * AmpDispUnit / 6);
                var pFall = pOfs;
                var psTop = pTop;
                var psSustain = pSustain;

                if (mPicValue.Height < psTop + 20) {
                    psTop = mPicValue.Height - 20;
                }
                if (mPicValue.Height < psSustain + 20) {
                    psSustain = mPicValue.Height - 20;
                }

                releaseImageValue();

                mGValue.DrawLine(Colors.PGraphLineAlpha, 0, pRise, TableColumnWidth, pTop);
                mGValue.DrawLine(Colors.PGraphLineAlpha, TableColumnWidth, pTop, TableColumnWidth * 2, pTop);
                mGValue.DrawLine(Colors.PGraphLineAlpha, TableColumnWidth * 2, pTop, TableColumnWidth * 3, pSustain);
                mGValue.DrawLine(Colors.PGraphLineAlpha, TableColumnWidth * 3, pSustain, TableColumnWidth * 4, pSustain);
                mGValue.DrawLine(Colors.PGraphLineAlpha, TableColumnWidth * 4, pSustain, TableColumnWidth * 5, pFall);

                switch (moveCol) {
                case 1:
                    mGValue.DrawLine(Colors.PGraphLineRed, 0, pTop, TableColumnWidth * 5, pTop);
                    break;
                case 3:
                    mGValue.DrawLine(Colors.PGraphLineRed, 0, pSustain, TableColumnWidth * 5, pSustain);
                    break;
                }

                mGValue.FillPie(Colors.BGraphPoint, TableColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
                mGValue.DrawArc(Colors.PTableBorderLight, TableColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
                mGValue.FillPie(Colors.BGraphPoint, TableColumnWidth * 3 - 4, pSustain - 4, 8, 8, 0, 360);
                mGValue.DrawArc(Colors.PTableBorderLight, TableColumnWidth * 3 - 4, pSustain - 4, 8, 8, 0, 360);

                mGValue.DrawString(dbTop.ToString("0.0db"), Fonts.Small, Colors.BFontTable, TableColumnWidth + 3, psTop);
                mGValue.DrawString(dbSustain.ToString("0.0db"), Fonts.Small, Colors.BFontTable, TableColumnWidth * 3 + 3, psSustain);

                mPicValue.Image = mBmpValue;
            }

            private void limit() {
                var attack = Attack + DAttack;
                var hold = Hold + DHold;
                var decay = Decay + DDecay;
                var release = Release + DRelease;
                var range = Range + DRange;

                if (attack < 1) {
                    Attack = 1;
                    DAttack = 0;
                }
                if (4000 < attack) {
                    Attack = 4000;
                    DAttack = 0;
                }

                if (hold < 1) {
                    Hold = 1;
                    DHold = 0;
                }
                if (4000 < hold) {
                    Hold = 4000;
                    DHold = 0;
                }

                if (decay < 1) {
                    Decay = 1;
                    DDecay = 0;
                }
                if (4000 < decay) {
                    Decay = 4000;
                    DDecay = 0;
                }

                if (release < 1) {
                    Release = 1;
                    DRelease = 0;
                }
                if (4000 < release) {
                    Release = 4000;
                    DRelease = 0;
                }

                if (range < 100) {
                    Range = 100;
                    DRange = 0;
                }
                if (4800 < range) {
                    Range = 4800;
                    DRange = 0;
                }
            }

            private void releaseImageTime() {
                if (null != mBmpTime) {
                    mBmpTime.Dispose();
                    mBmpTime = null;
                    mGTime.Dispose();
                    mGTime = null;
                }
                if (null != mPicTime.Image) {
                    mPicTime.Image.Dispose();
                    mPicTime.Image = null;
                }
                mBmpTime = new Bitmap(TableColumnWidth * 5, TableHeaderHeight * 2);
                mGTime = Graphics.FromImage(mBmpTime);
            }

            private void releaseImageValue() {
                if (null != mBmpValue) {
                    mBmpValue.Dispose();
                    mBmpValue = null;
                    mGValue.Dispose();
                    mGValue = null;
                }
                if (null != mPicValue.Image) {
                    mPicValue.Image.Dispose();
                    mPicValue.Image = null;
                }
                mBmpValue = new Bitmap(mPicValue.Width, mPicValue.Height);
                mGValue = Graphics.FromImage(mBmpValue);
            }
        }

        private bool mTimeScroll = false;
        private bool mValueScroll = false;
        private Point mCurPos;

        private Values mPitch;
        private Values mCutoff;
        private Values mAmp;

        private CommonCtrl mCommonCtrl;

        private TabButton mTabButtons;
        private Bitmap mBmpRow;
        private Bitmap mBmpCol;
        private Bitmap mBmpCell;
        private Graphics mGRow;
        private Graphics mGCol;
        private Graphics mGCell;

        public Envelope() {
            InitializeComponent();
        }

        private void Envelope_Load(object sender, EventArgs e) {
            mCommonCtrl = new CommonCtrl(this);

            mTabButtons = new TabButton(picTabButton, tab_Click, 14.0f, new string[] {
                 "Pitch",
                 "Cutoff",
                 "Amp"
            });

            mPitch = new Values(picHeader, picCell);
            mPitch.DrawTimePitch();
            drawBackgroundPitch();
            mPitch.DrawValuePitch();

            mCutoff = new Values(picHeader, picCell);
            mCutoff.Rise = 20000;
            mCutoff.Top = 20000;
            mCutoff.Sustain = 20000;
            mCutoff.Fall = 20000;

            mAmp = new Values(picHeader, picCell);
            mAmp.Top = 1.0;
            mAmp.Sustain = 1.0;
        }

        private void picHeader_MouseDown(object sender, MouseEventArgs e) {
            var pos = picHeader.PointToClient(Cursor.Position);
            if (pos.Y < TableHeaderHeight) {
                return;
            }
            mCurPos = picHeader.PointToClient(Cursor.Position);
            mTimeScroll = true;
            Cursor.Current = Cursors.VSplit;
        }

        private void picHeader_MouseUp(object sender, MouseEventArgs e) {
            mTimeScroll = false;
            mPitch.Commit();
            mCutoff.Commit();
            mAmp.Commit();
        }

        private void picHeader_MouseMove(object sender, MouseEventArgs e) {
            if (!mTimeScroll) {
                return;
            }

            var pos = picHeader.PointToClient(Cursor.Position);
            var moveCol = mCurPos.X / TableColumnWidth;

            var delta = pos.X - mCurPos.X;
            if (Math.Abs(delta) < 80) {
                delta /= 4;
            } else if (Math.Abs(delta) < 160) {
                delta /= 2;
            } else if (Math.Abs(delta) < 240) {
                delta *= 2;
            } else {
                delta *= 4;
            }

            switch (mTabButtons.CurrentTab) {
            case "Pitch":
                switch (moveCol) {
                case 0:
                    mPitch.DAttack = delta;
                    break;
                case 1:
                    mPitch.DHold = delta;
                    break;
                case 2:
                    mPitch.DDecay = delta;
                    break;
                case 3:
                    mPitch.DRelease = delta;
                    break;
                case 4:
                    mPitch.DRange = (pos.X - mCurPos.X) / 10 * 100;
                    break;
                }
                mPitch.DrawTimePitch();
                drawBackgroundPitchLeft();
                break;
            case "Cutoff":
                switch (moveCol) {
                case 0:
                    mCutoff.DAttack = delta;
                    break;
                case 1:
                    mCutoff.DHold = delta;
                    break;
                case 2:
                    mCutoff.DDecay = delta;
                    break;
                case 4:
                    mCutoff.DRelease = delta;
                    break;
                }
                mCutoff.DrawTime();
                break;
            case "Amp":
                switch (moveCol) {
                case 0:
                    mAmp.DAttack = delta;
                    break;
                case 1:
                    mAmp.DHold = delta;
                    break;
                case 2:
                    mAmp.DDecay = delta;
                    break;
                case 4:
                    mAmp.DRelease = delta;
                    break;
                }
                mAmp.DrawTime();
                break;
            }
        }

        private void picCell_MouseDown(object sender, MouseEventArgs e) {
            mCurPos = picCell.PointToClient(Cursor.Position);
            var moveCol = mCurPos.X / TableColumnWidth;

            switch (mTabButtons.CurrentTab) {
            case "Pitch":
                switch (moveCol) {
                case 0:
                case 1:
                case 3:
                    mValueScroll = true;
                    Cursor.Current = Cursors.HSplit;
                    break;
                }
                break;
            case "Cutoff":
                switch (moveCol) {
                case 0:
                case 1:
                case 3:
                case 4:
                    mValueScroll = true;
                    Cursor.Current = Cursors.HSplit;
                    break;
                }
                break;
            case "Amp":
                switch (moveCol) {
                case 1:
                case 3:
                    mValueScroll = true;
                    Cursor.Current = Cursors.HSplit;
                    break;
                }
                break;
            }
        }

        private void picCell_MouseUp(object sender, MouseEventArgs e) {
            mValueScroll = false;
            switch (mTabButtons.CurrentTab) {
            case "Pitch":
                mPitch.DrawValuePitch();
                break;
            case "Cutoff":
                mCutoff.DrawValueCutoff();
                break;
            case "Amp":
                mAmp.DrawValueAmp();
                break;
            }
        }

        private void picCell_MouseMove(object sender, MouseEventArgs e) {
            if (!mValueScroll) {
                return;
            }

            var pos = picCell.PointToClient(Cursor.Position);
            var moveCol = mCurPos.X / TableColumnWidth;

            switch (mTabButtons.CurrentTab) {
            case "Pitch":
                var pitchPos = PitchDispUnit * 2 - pos.Y;
                var pitch = (int)(pitchPos * mPitch.Range / PitchDispUnit + 0.5) / 2;
                switch (moveCol) {
                case 0:
                    mPitch.Rise = pitch;
                    break;
                case 1:
                    mPitch.Top = pitch;
                    break;
                case 3:
                    mPitch.Fall = pitch;
                    break;
                }
                mPitch.DrawValuePitch(moveCol);
                break;
            case "Cutoff":
                var freqPos = picCell.Height - pos.Y + CutoffDispUnit * 6;
                var freq = Math.Pow(10.0, freqPos * 0.25 / CutoffDispUnit);
                switch (moveCol) {
                case 0:
                    mCutoff.Rise = freq;
                    break;
                case 1:
                    mCutoff.Top = freq;
                    break;
                case 3:
                    mCutoff.Sustain = freq;
                    break;
                case 4:
                    mCutoff.Fall = freq;
                    break;
                }
                mCutoff.DrawValueCutoff(moveCol);
                break;
            case "Amp":
                var db = (int)((AmpDispUnit - pos.Y) * 6.0 * 2 / AmpDispUnit - 0.5) / 2.0;
                var gain = Math.Pow(10.0, db / 20.0);
                switch (moveCol) {
                case 1:
                    mAmp.Top = gain;
                    break;
                case 3:
                    mAmp.Sustain = gain;
                    break;
                }
                mAmp.DrawValueAmp(moveCol);
                break;
            }
        }

        private void tab_Click(object sender, EventArgs e) {
            switch (mTabButtons.CurrentTab) {
            case "Pitch":
                drawBackgroundPitch();
                mPitch.DrawTimePitch();
                mPitch.DrawValuePitch();
                break;
            case "Cutoff":
                drawBackgroundCutoff();
                mCutoff.DrawTime();
                mCutoff.DrawValueCutoff();
                break;
            case "Amp":
                drawBackgroundAmp();
                mAmp.DrawTime();
                mAmp.DrawValueAmp();
                break;
            }
        }

        private void drawBackgroundPitch() {
            releaseBackgroundImage();

            mBmpCol = new Bitmap(TableColumnWidth * 5, TableHeaderHeight * 2);
            mGCol = Graphics.FromImage(mBmpCol);
            mBmpCell = new Bitmap(TableColumnWidth * 5, PitchDispUnit * 4);
            mGCell = Graphics.FromImage(mBmpCell);

            drawBackgroundPitchLeft();
            //
            // header
            //
            mGCol.Clear(Colors.TableHeader);
            mGCol.FillRectangle(Colors.BTableCell,
                0, TableHeaderHeight,
                mBmpCol.Width, TableHeaderHeight);
            //
            mGCol.DrawLine(Colors.PTableBorderBold,
                0, mBmpCol.Height - 1,
                mBmpCol.Width, mBmpCol.Height - 1);
            mGCol.DrawLine(Colors.PTableBorderBold,
                TableColumnWidth * 4, 0,
                TableColumnWidth * 4, mBmpCol.Height);
            //
            mGCol.DrawLine(Colors.PTableBorder,
                TableColumnWidth, 0,
                TableColumnWidth, mBmpCol.Height);
            mGCol.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 2, 0,
                TableColumnWidth * 2, mBmpCol.Height);
            mGCol.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 3, 0,
                TableColumnWidth * 3, mBmpCol.Height);
            //
            mGCol.DrawString("Attack",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(0, 0, TableColumnWidth, TableHeaderHeight),
                Fonts.AlignMC);
            mGCol.DrawString("Hold",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth, 0, TableColumnWidth, TableHeaderHeight),
                Fonts.AlignMC);
            mGCol.DrawString("Decay",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 2, 0, TableColumnWidth, TableHeaderHeight),
                Fonts.AlignMC);
            mGCol.DrawString("Release",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 3, 0, TableColumnWidth, TableHeaderHeight),
                Fonts.AlignMC);
            mGCol.DrawString("Range",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 4, 0, TableColumnWidth, TableHeaderHeight),
                Fonts.AlignMC);
            //
            // table
            //
            mGCell.Clear(Colors.TableCell);
            mGCell.FillRectangle(Colors.BTableHeader,
                TableColumnWidth * 4, 0,
                TableColumnWidth * 4, mBmpCell.Height);
            //
            mGCell.DrawLine(Colors.PTableBorder,
                0, PitchDispUnit - 1,
                mBmpCell.Width - TableColumnWidth, PitchDispUnit - 1);
            mGCell.DrawLine(Colors.PTableBorderLight,
                0, PitchDispUnit * 2 - 1,
                mBmpCell.Width - TableColumnWidth, PitchDispUnit * 2 - 1);
            mGCell.DrawLine(Colors.PTableBorder,
                0, PitchDispUnit * 3 - 1,
                mBmpCell.Width - TableColumnWidth, PitchDispUnit * 3 - 1);
            //
            mGCell.DrawLine(Colors.PTableBorder,
                TableColumnWidth, 0,
                TableColumnWidth, mBmpCell.Height);
            mGCell.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 2, 0,
                TableColumnWidth * 2, mBmpCell.Height);
            mGCell.DrawLine(Colors.PTableBorder,
                TableColumnWidth * 3, 0,
                TableColumnWidth * 3, mBmpCell.Height);

            mGCell.DrawLine(Colors.PTableBorderBold,
                TableColumnWidth * 4, 0,
                TableColumnWidth * 4, mBmpCell.Height);

            setBackgroundImage();
        }

        private void drawBackgroundPitchLeft() {
            if (null != mBmpRow) {
                mBmpRow.Dispose();
                mBmpRow = null;
                mGRow.Dispose();
                mGRow = null;
            }
            if (null != picRow.BackgroundImage) {
                picRow.BackgroundImage.Dispose();
                picRow.BackgroundImage = null;
            }

            mBmpRow = new Bitmap(TableLeftFrameWidth, PitchDispUnit * 4 + mBmpCol.Height);
            mGRow = Graphics.FromImage(mBmpRow);
            mGRow.Clear(Colors.TableHeader);
            //
            mGRow.DrawLine(Colors.PTableBorderBold,
                0, mBmpCol.Height - 1,
                mBmpRow.Width, mBmpCol.Height - 1);
            mGRow.DrawLine(Colors.PTableBorder,
                0, PitchDispUnit + mBmpCol.Height - 1,
                mBmpRow.Width, PitchDispUnit + mBmpCol.Height - 1);
            mGRow.DrawLine(Colors.PTableBorderLight,
                0, PitchDispUnit * 2 + mBmpCol.Height - 1,
                mBmpRow.Width, PitchDispUnit * 2 + mBmpCol.Height - 1);
            mGRow.DrawLine(Colors.PTableBorder,
                0, PitchDispUnit * 3 + mBmpCol.Height - 1,
                mBmpRow.Width, PitchDispUnit * 3 + mBmpCol.Height - 1);
            //
            mGRow.DrawLine(Colors.PTableBorderBold,
                TableLeftFrameWidth - 1, 0,
                TableLeftFrameWidth - 1, mBmpRow.Height);
            mGRow.DrawLine(Colors.PTabBorderBold, 1, 0, 1, mBmpRow.Height);
            mGRow.DrawLine(Colors.PTabBorderBold,
                0, mBmpRow.Height - 1,
                mBmpRow.Width, mBmpRow.Height - 1);
            //
            var range = mPitch.Range + mPitch.DRange;
            mGRow.DrawString(range.ToString("0cent"), Fonts.Small, Colors.BFontTable, new RectangleF(
                -5, mBmpCol.Height,
                TableLeftFrameWidth, PitchDispUnit), Fonts.AlignTR);
            mGRow.DrawString((range / 2).ToString("0cent"), Fonts.Small, Colors.BFontTable, new RectangleF(
                -5, mBmpCol.Height + PitchDispUnit,
                TableLeftFrameWidth, PitchDispUnit), Fonts.AlignTR);
            mGRow.DrawString("0cent", Fonts.Small, Colors.BFontTable, new RectangleF(
                -5, mBmpCol.Height + PitchDispUnit * 2,
                TableLeftFrameWidth, PitchDispUnit), Fonts.AlignTR);
            mGRow.DrawString((-range / 2).ToString("0cent"), Fonts.Small, Colors.BFontTable, new RectangleF(
                -5, mBmpCol.Height + PitchDispUnit * 2,
                TableLeftFrameWidth, PitchDispUnit), Fonts.AlignBR);
            mGRow.DrawString((-range).ToString("0cent"), Fonts.Small, Colors.BFontTable, new RectangleF(
                -5, mBmpCol.Height + PitchDispUnit * 3,
                TableLeftFrameWidth, PitchDispUnit), Fonts.AlignBR);

            picRow.BackgroundImage = mBmpRow;
        }

        private void drawBackgroundCutoff() {
            releaseBackgroundImage();

            var ofs10kTo20k = (int)((Math.Log10(20000) - Math.Log10(10000)) * 4 * CutoffDispUnit);
            var ofs10kTo20k_TabPageColHeight = ofs10kTo20k + TableHeaderHeight * 2;

            mBmpCol = new Bitmap(TableColumnWidth * 5, TableHeaderHeight * 2);
            mGCol = Graphics.FromImage(mBmpCol);
            mBmpRow = new Bitmap(TableLeftFrameWidth, CutoffDispUnit * 10 + ofs10kTo20k + mBmpCol.Height);
            mGRow = Graphics.FromImage(mBmpRow);
            mBmpCell = new Bitmap(TableColumnWidth * 5, CutoffDispUnit * 10 + ofs10kTo20k);
            mGCell = Graphics.FromImage(mBmpCell);
            //
            // header
            //
            mGCol.Clear(Colors.TableHeader);
            mGCol.FillRectangle(Colors.BTableCell,
                0, TableHeaderHeight,
                TableColumnWidth * 3, TableHeaderHeight);
            mGCol.FillRectangle(Colors.BTableCell,
                TableColumnWidth * 4, TableHeaderHeight,
                TableColumnWidth, TableHeaderHeight);
            //
            for (var col = 1; col < 5; col++) {
                mGCol.DrawLine(Colors.PTableBorder,
                    TableColumnWidth * col, 0,
                    TableColumnWidth * col, mBmpCol.Height);
            }
            //
            mGCol.DrawString("Attack",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(0, 0, TableColumnWidth, TableHeaderHeight),
                Fonts.AlignMC);
            mGCol.DrawString("Hold",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth, 0, TableColumnWidth, TableHeaderHeight),
                Fonts.AlignMC);
            mGCol.DrawString("Decay",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 2, 0, TableColumnWidth, TableHeaderHeight),
                Fonts.AlignMC);
            mGCol.DrawString("Sustain",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 3, 0, TableColumnWidth, TableHeaderHeight),
                Fonts.AlignMC);
            mGCol.DrawString("Release",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 4, 0, TableColumnWidth, TableHeaderHeight),
                Fonts.AlignMC);
            //
            mGCol.DrawLine(Colors.PTableBorderBold,
                0, mBmpCol.Height - 1,
                mBmpCol.Width, mBmpCol.Height - 1);
            //
            // left frame
            //
            mGRow.Clear(Colors.TableHeader);
            for (var row = 0; row < 10; row += 2) {
                mGRow.DrawLine(Colors.PTableBorder,
                    0, CutoffDispUnit * row + ofs10kTo20k_TabPageColHeight,
                    mBmpRow.Width, CutoffDispUnit * row + ofs10kTo20k_TabPageColHeight);
            }
            //
            mGRow.DrawString("10kHz", Fonts.Small, Colors.BFontTable, new RectangleF(
                -5, mBmpCol.Height,
                mBmpRow.Width, ofs10kTo20k), Fonts.AlignBR);
            mGRow.DrawString("3.16kHz", Fonts.Small, Colors.BFontTable, new RectangleF(
                -5, CutoffDispUnit + ofs10kTo20k_TabPageColHeight,
                mBmpRow.Width, CutoffDispUnit), Fonts.AlignBR);
            mGRow.DrawString("1kHz", Fonts.Small, Colors.BFontTable, new RectangleF(
                -5, CutoffDispUnit * 3 + ofs10kTo20k_TabPageColHeight,
                mBmpRow.Width, CutoffDispUnit), Fonts.AlignBR);
            mGRow.DrawString("316Hz", Fonts.Small, Colors.BFontTable, new RectangleF(
                -5, CutoffDispUnit * 5 + ofs10kTo20k_TabPageColHeight,
                mBmpRow.Width, CutoffDispUnit), Fonts.AlignBR);
            mGRow.DrawString("100Hz", Fonts.Small, Colors.BFontTable, new RectangleF(
                -5, CutoffDispUnit * 7 + ofs10kTo20k_TabPageColHeight,
                mBmpRow.Width, CutoffDispUnit), Fonts.AlignBR);
            mGRow.DrawString("32Hz", Fonts.Small, Colors.BFontTable, new RectangleF(
                -5, CutoffDispUnit * 9 + ofs10kTo20k_TabPageColHeight,
                mBmpRow.Width, CutoffDispUnit), Fonts.AlignBR);
            //
            mGRow.DrawLine(Colors.PTableBorderBold,
                0, mBmpCol.Height - 1,
                mBmpRow.Width, mBmpCol.Height - 1);
            mGRow.DrawLine(Colors.PTableBorderBold,
                TableLeftFrameWidth - 1, 0,
                TableLeftFrameWidth - 1, mBmpRow.Height);
            //
            // table
            //
            mGCell.Clear(Colors.TableCell);
            for (var row = 0; row < 10; row++) {
                mGCell.DrawLine(0 == (row % 2) ? Colors.PTableBorder : Colors.PTableBorderDark,
                    0, ofs10kTo20k + CutoffDispUnit * row,
                    mBmpCell.Width, ofs10kTo20k + CutoffDispUnit * row);
            }
            //
            for (var col = 1; col < 5; col++) {
                mGCell.DrawLine(Colors.PTableBorder,
                    TableColumnWidth * col, 0,
                    TableColumnWidth * col, mBmpCell.Height);
            }

            setBackgroundImage();
        }

        private void drawBackgroundAmp() {
            releaseBackgroundImage();

            mBmpCol = new Bitmap(TableColumnWidth * 5, TableHeaderHeight * 2);
            mGCol = Graphics.FromImage(mBmpCol);
            mBmpRow = new Bitmap(TableLeftFrameWidth, AmpDispUnit * 11 + mBmpCol.Height);
            mGRow = Graphics.FromImage(mBmpRow);
            mBmpCell = new Bitmap(TableColumnWidth * 5, AmpDispUnit * 11);
            mGCell = Graphics.FromImage(mBmpCell);
            //
            // header
            //
            mGCol.Clear(Colors.TableHeader);
            mGCol.FillRectangle(Colors.BTableCell,
                0, TableHeaderHeight,
                TableColumnWidth * 3, TableHeaderHeight);
            mGCol.FillRectangle(Colors.BTableCell,
                TableColumnWidth * 4, TableHeaderHeight,
                TableColumnWidth, TableHeaderHeight);
            //
            for (var col = 1; col < 5; col++) {
                mGCol.DrawLine(Colors.PTableBorder,
                    TableColumnWidth * col, 0,
                    TableColumnWidth * col, mBmpCol.Height);
            }
            //
            mGCol.DrawString("Attack",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(0, 0, TableColumnWidth, TableHeaderHeight),
                Fonts.AlignMC);
            mGCol.DrawString("Hold",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth, 0, TableColumnWidth, TableHeaderHeight),
                Fonts.AlignMC);
            mGCol.DrawString("Decay",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 2, 0, TableColumnWidth, TableHeaderHeight),
                Fonts.AlignMC);
            mGCol.DrawString("Sustain",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 3, 0, TableColumnWidth, TableHeaderHeight),
                Fonts.AlignMC);
            mGCol.DrawString("Release",
                Fonts.Bold,
                Colors.BFontTable,
                new RectangleF(TableColumnWidth * 4, 0, TableColumnWidth, TableHeaderHeight),
                Fonts.AlignMC);
            //
            mGCol.DrawLine(Colors.PTableBorderBold,
                0, mBmpCol.Height - 1,
                mBmpCol.Width, mBmpCol.Height - 1);
            //
            // left frame
            //
            mGRow.Clear(Colors.TableHeader);
            for (int row = 0, db = 0; row < 12; row += 2, db -= 12) {
                mGRow.DrawLine(Colors.PTableBorder,
                    0, AmpDispUnit * (row + 1) + mBmpCol.Height,
                    mBmpRow.Width, AmpDispUnit * (row + 1) + mBmpCol.Height);
                mGRow.DrawString(db.ToString("0db"), Fonts.Small, Colors.BFontTable,
                    new RectangleF(-5, AmpDispUnit * row + mBmpCol.Height,
                    mBmpRow.Width, AmpDispUnit), Fonts.AlignBR);
            }
            mGRow.DrawLine(Colors.PTableBorderBold,
                0, mBmpCol.Height - 1,
                mBmpRow.Width, mBmpCol.Height - 1);
            mGRow.DrawLine(Colors.PTableBorderBold,
                TableLeftFrameWidth - 1, 0,
                TableLeftFrameWidth - 1, mBmpRow.Height);
            //
            // table
            //
            mGCell.Clear(Colors.TableCell);
            for (var row = 1; row < 12; row++) {
                mGCell.DrawLine(0 == (row % 2) ? Colors.PTableBorderDark : Colors.PTableBorder,
                    0, AmpDispUnit * row,
                    mBmpCell.Width, AmpDispUnit * row);
            }
            for (var col = 1; col < 5; col++) {
                mGCell.DrawLine(Colors.PTableBorder,
                    TableColumnWidth * col, 0,
                    TableColumnWidth * col, mBmpCell.Height);
            }

            setBackgroundImage();
        }

        private void setBackgroundImage() {
            mGRow.DrawLine(Colors.PTabBorderBold,
                0, mBmpRow.Height - 1,
                mBmpRow.Width, mBmpRow.Height - 1);
            mGRow.DrawLine(Colors.PTabBorderBold, 1, 0, 1, mBmpRow.Height);
            mGCell.DrawLine(Colors.PTabBorderBold,
                0, mBmpCell.Height - 1,
                mBmpCell.Width, mBmpCell.Height - 1);
            mGCell.DrawLine(Colors.PTabBorderBold,
                mBmpCell.Width - 1, 0,
                mBmpCell.Width - 1, mBmpCell.Height);
            mGCol.DrawLine(Colors.PTabBorderBold,
                mBmpCol.Width - 1, 0,
                mBmpCol.Width - 1, mBmpCol.Height);
            mGCol.DrawLine(Colors.PTabBorderBold,
                picTabButton.Width - mBmpRow.Width - 3, 1,
                mBmpCol.Width, 1);

            Width = mBmpRow.Width + mBmpCell.Width;
            Height = picTabButton.Height + mBmpCol.Height + mBmpCell.Height;
            picHeader.Width = mBmpCol.Width;
            picHeader.Height = mBmpCol.Height;
            picRow.Width = mBmpRow.Width;
            picRow.Height = mBmpRow.Height;
            picCell.Width = mBmpCell.Width;
            picCell.Height = mBmpCell.Height;
            picHeader.BackgroundImage = mBmpCol;
            picRow.BackgroundImage = mBmpRow;
            picCell.BackgroundImage = mBmpCell;

            picTabButton.Left = 0;
            picTabButton.Top = 0;
            picRow.Left = picTabButton.Left;
            picHeader.Left = picRow.Right;
            picCell.Left = picRow.Right;

            picHeader.Top = picTabButton.Bottom;
            picRow.Top = picTabButton.Bottom;
            picCell.Top = picHeader.Bottom;
        }

        private void releaseBackgroundImage() {
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

            if (null != picHeader.BackgroundImage) {
                picHeader.BackgroundImage.Dispose();
                picHeader.BackgroundImage = null;
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
