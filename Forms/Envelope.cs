using System;
using System.Drawing;
using System.Windows.Forms;

namespace Forms {
    public partial class Envelope : Form {
        private bool mTimeScroll = false;
        private bool mValueScroll = false;
        private Point mCurPos;

        private CommonCtrl mCommonCtrl;

        private TabButton mTabButtons;

        private EnvelopeGrid mAmp;
        private EnvelopeGrid mCutoff;
        private EnvelopeGrid mPitch;

        public Envelope() {
            InitializeComponent();
        }

        private void Envelope_Load(object sender, EventArgs e) {
            mCommonCtrl = new CommonCtrl(this, true);
            mCommonCtrl.WindowStateChanged = new EventHandler(Envelope_WindowStateChanged);
            mTabButtons = new TabButton(picTabButton, tab_Click, 14.0f, new string[] {
                "Amp",
                "Cutoff",
                "Pitch"
            });

            mAmp = new AmpValues(picTime, picLeftFrame, picValue);
            mAmp.DrawHeader();
            mAmp.DrawBackground();
            mAmp.DrawValue();
            setPos();

            mCutoff = new CutoffValues(picTime, picLeftFrame, picValue);
            mPitch = new PitchValues(picTime, picLeftFrame, picValue);
        }

        private void Envelope_WindowStateChanged(object sender, EventArgs e) {
            if (FormWindowState.Maximized == WindowState) {
                var screen = Screen.FromControl(this);
                var sw = screen.Bounds.Width - picLeftFrame.Width;
                var sh = screen.Bounds.Height - picTime.Height - mCommonCtrl.FormCtrlBottom;
                mAmp.Maximize(sw, sh);
                mCutoff.Maximize(sw, sh);
                mPitch.Maximize(sw, sh);
            } else {
                mAmp.Minimize();
                mCutoff.Minimize();
                mPitch.Minimize();
            }
            tab_Click(sender, e);
        }

        private void picHeader_MouseDown(object sender, MouseEventArgs e) {
            mCurPos = picTime.PointToClient(Cursor.Position);
            mTimeScroll = true;
            Cursor.Current = Cursors.VSplit;
        }

        private void picHeader_MouseUp(object sender, MouseEventArgs e) {
            mTimeScroll = false;
            switch (mTabButtons.CurrentTab) {
            case "Amp":
                mAmp.Commit();
                break;
            case "Cutoff":
                mCutoff.Commit();
                break;
            case "Pitch":
                mPitch.Commit();
                break;
            }
        }

        private void picHeader_MouseMove(object sender, MouseEventArgs e) {
            if (!mTimeScroll) {
                return;
            }
            var pos = picTime.PointToClient(Cursor.Position);
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
            case "Amp":
                mAmp.MouseMoveHeader(mCurPos.X / mAmp.ColumnWidth, delta);
                break;
            case "Cutoff":
                mCutoff.MouseMoveHeader(mCurPos.X / mCutoff.ColumnWidth, delta);
                break;
            case "Pitch":
                mPitch.MouseMoveHeader(mCurPos.X / mPitch.ColumnWidth, delta);
                mPitch.DrawBackground();
                break;
            }
        }

        private void picCell_MouseDown(object sender, MouseEventArgs e) {
            mCurPos = picValue.PointToClient(Cursor.Position);
            switch (mTabButtons.CurrentTab) {
            case "Amp":
                switch (mCurPos.X / mAmp.ColumnWidth) {
                case 1:
                case 3:
                    mValueScroll = true;
                    Cursor.Current = Cursors.HSplit;
                    break;
                }
                break;
            case "Cutoff":
                switch (mCurPos.X / mCutoff.ColumnWidth) {
                case 0:
                case 1:
                case 3:
                case 4:
                    mValueScroll = true;
                    Cursor.Current = Cursors.HSplit;
                    break;
                }
                break;
            case "Pitch":
                switch (mCurPos.X / mPitch.ColumnWidth) {
                case 0:
                case 1:
                case 2:
                    mValueScroll = true;
                    Cursor.Current = Cursors.HSplit;
                    break;
                }
                break;
            }
        }

        private void picCell_MouseUp(object sender, MouseEventArgs e) {
            mValueScroll = false;
        }

        private void picCell_MouseMove(object sender, MouseEventArgs e) {
            if (!mValueScroll) {
                return;
            }
            var pos = picValue.PointToClient(Cursor.Position);
            switch (mTabButtons.CurrentTab) {
            case "Amp":
                mAmp.DrawValue(mCurPos.X / mAmp.ColumnWidth, pos);
                break;
            case "Cutoff":
                mCutoff.DrawValue(mCurPos.X / mCutoff.ColumnWidth, pos);
                break;
            case "Pitch":
                mPitch.DrawValue(mCurPos.X / mPitch.ColumnWidth, pos);
                break;
            }
        }

        private void tab_Click(object sender, EventArgs e) {
            switch (mTabButtons.CurrentTab) {
            case "Amp":
                mAmp.DrawBackground();
                mAmp.DrawHeader();
                mAmp.DrawValue();
                break;
            case "Cutoff":
                mCutoff.DrawBackground();
                mCutoff.DrawHeader();
                mCutoff.DrawValue();
                break;
            case "Pitch":
                mPitch.DrawBackground();
                mPitch.DrawHeader();
                mPitch.DrawValue();
                break;
            }
            setPos();
        }

        private void setPos() {
            Width = picLeftFrame.Width + picValue.Width;
            Height = picTabButton.Height + picTime.Height + picValue.Height;

            picTabButton.Left = 0;
            picTabButton.Top = 0;
            picLeftFrame.Left = picTabButton.Left;
            picTime.Left = picLeftFrame.Right;
            picValue.Left = picLeftFrame.Right;

            picTime.Top = picTabButton.Bottom;
            picLeftFrame.Top = picTabButton.Bottom;
            picValue.Top = picTime.Bottom;
        }
    }
}
