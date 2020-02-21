using System;
using System.Drawing;
using System.Windows.Forms;

namespace Forms {
    public class CommonCtrl {
        private Form mForm;

        private PictureBox mBtnClose = new PictureBox();
        private PictureBox mBtnMaximize = new PictureBox();
        private PictureBox mBtnMinimize = new PictureBox();
        private PictureBox mBtnHelp = new PictureBox();

        private bool mMoving = false;
        private Point mCurPos;

        public int FormCtrlLeft { get; private set; }
        public int FormCtrlBottom { get; private set; }

        public EventHandler WindowStateChanged;
        public EventHandler HelpClicked;

        public CommonCtrl(Form form, bool enableMaximize = false, bool enableHelp = false) {
            mForm = form;

            ((System.ComponentModel.ISupportInitialize)mBtnClose).BeginInit();
            ((System.ComponentModel.ISupportInitialize)mBtnMaximize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)mBtnMinimize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)mBtnHelp).BeginInit();
            //
            // btnClose
            //
            mBtnClose.BackColor = Color.Transparent;
            mBtnClose.Image = Properties.Resources.close_leave;
            mBtnClose.Location = new Point(237, 4);
            mBtnClose.Name = "btnClose";
            mBtnClose.Size = new Size(30, 30);
            mBtnClose.TabIndex = 7;
            mBtnClose.TabStop = false;
            mBtnClose.Click += new EventHandler((object s, EventArgs e) => {
                mForm.Close();
            });
            mBtnClose.MouseEnter += new EventHandler((object s, EventArgs e) => {
                if (null != mBtnClose.Image) {
                    mBtnClose.Image.Dispose();
                    mBtnClose.Image = null;
                }
                mBtnClose.Image = Properties.Resources.close_hover;
            });
            mBtnClose.MouseLeave += new EventHandler((object s, EventArgs e) => {
                if (null != mBtnClose.Image) {
                    mBtnClose.Image.Dispose();
                    mBtnClose.Image = null;
                }
                mBtnClose.Image = Properties.Resources.close_leave;
            });
            //
            // btnMaximize
            //
            mBtnMaximize.BackColor = Color.Transparent;
            mBtnMaximize.Image = Properties.Resources.maximize_leave;
            mBtnMaximize.Location = new Point(201, 4);
            mBtnMaximize.Name = "btnMaximize";
            mBtnMaximize.Size = new Size(30, 30);
            mBtnMaximize.TabIndex = 10;
            mBtnMaximize.TabStop = false;
            mBtnMaximize.Click += new EventHandler((object s, EventArgs e) => {
                if (FormWindowState.Maximized == mForm.WindowState) {
                    mForm.WindowState = FormWindowState.Normal;
                } else {
                    mForm.WindowState = FormWindowState.Maximized;
                }
                if (null != WindowStateChanged) {
                    WindowStateChanged.Invoke(s, e);
                }
            });
            mBtnMaximize.MouseEnter += new EventHandler((object s, EventArgs e) => {
                if (null != mBtnMaximize.Image) {
                    mBtnMaximize.Image.Dispose();
                    mBtnMaximize.Image = null;
                }
                mBtnMaximize.Image = Properties.Resources.maximize_hover;
            });
            mBtnMaximize.MouseLeave += new EventHandler((object s, EventArgs e) => {
                if (null != mBtnMaximize.Image) {
                    mBtnMaximize.Image.Dispose();
                    mBtnMaximize.Image = null;
                }
                mBtnMaximize.Image = Properties.Resources.maximize_leave;
            });
            //
            // btnMinimize
            //
            mBtnMinimize.BackColor = Color.Transparent;
            mBtnMinimize.Image = Properties.Resources.minimize_leave;
            mBtnMinimize.Location = new Point(201, 4);
            mBtnMinimize.Name = "btnMinimize";
            mBtnMinimize.Size = new Size(30, 30);
            mBtnMinimize.TabIndex = 10;
            mBtnMinimize.TabStop = false;
            mBtnMinimize.Click += new EventHandler((object s, EventArgs e) => {
                mForm.WindowState = FormWindowState.Minimized;
                if (null != WindowStateChanged) {
                    WindowStateChanged.Invoke(s, e);
                }
            });
            mBtnMinimize.MouseEnter += new EventHandler((object s, EventArgs e) => {
                if (null != mBtnMinimize.Image) {
                    mBtnMinimize.Image.Dispose();
                    mBtnMinimize.Image = null;
                }
                mBtnMinimize.Image = Properties.Resources.minimize_hover;
            });
            mBtnMinimize.MouseLeave += new EventHandler((object s, EventArgs e) => {
                if (null != mBtnMinimize.Image) {
                    mBtnMinimize.Image.Dispose();
                    mBtnMinimize.Image = null;
                }
                mBtnMinimize.Image = Properties.Resources.minimize_leave;
            });
            //
            // btnHelp
            //
            mBtnHelp.BackColor = Color.Transparent;
            mBtnHelp.Image = Properties.Resources.help_leave;
            mBtnHelp.Location = new Point(201, 4);
            mBtnHelp.Name = "btnHelp";
            mBtnHelp.Size = new Size(30, 30);
            mBtnHelp.TabIndex = 10;
            mBtnHelp.TabStop = false;
            mBtnHelp.Click += new EventHandler((object s, EventArgs e) => {
                if (null != HelpClicked) {
                    HelpClicked.Invoke(s, e);
                }
            });
            mBtnHelp.MouseEnter += new EventHandler((object s, EventArgs e) => {
                if (null != mBtnHelp.Image) {
                    mBtnHelp.Image.Dispose();
                    mBtnHelp.Image = null;
                }
                mBtnHelp.Image = Properties.Resources.help_hover;
            });
            mBtnHelp.MouseLeave += new EventHandler((object s, EventArgs e) => {
                if (null != mBtnHelp.Image) {
                    mBtnHelp.Image.Dispose();
                    mBtnHelp.Image = null;
                }
                mBtnHelp.Image = Properties.Resources.help_leave;
            });
            //
            // form
            //
            mForm.BackColor = Colors.FormColor;
            mForm.MouseDown += new MouseEventHandler((object s, MouseEventArgs e) => {
                mMoving = true;
                mCurPos = Cursor.Position;
            });
            mForm.MouseUp += new MouseEventHandler((object s, MouseEventArgs e) => {
                mMoving = false;
            });
            mForm.MouseMove += new MouseEventHandler((object s, MouseEventArgs e) => {
                if (mMoving) {
                    var screen = Screen.FromControl(mForm);
                    var sw = screen.Bounds.Width;
                    var sh = screen.Bounds.Height;
                    var dx = Cursor.Position.X - mCurPos.X;
                    var dy = Cursor.Position.Y - mCurPos.Y;
                    var left = mForm.Left + dx;
                    var top = mForm.Top + dy;
                    if (left < 96 - mForm.Width) {
                        left = 96 - mForm.Width;
                    }
                    if (sw - 96 < left) {
                        left = sw - 96;
                    }
                    if (top < 64 - mForm.Height) {
                        top = 64 - mForm.Height;
                    }
                    if (sh - 64 < top) {
                        top = sh - 64;
                    }
                    mForm.Left = left;
                    mForm.Top = top;
                    mCurPos = Cursor.Position;
                }
            });
            mForm.SizeChanged += new EventHandler((object s, EventArgs e) => {
                mBtnClose.Top = 0;
                mBtnMaximize.Top = 0;
                mBtnMinimize.Top = 0;
                mBtnHelp.Top = 0;
                if (!enableMaximize) {
                    mBtnMaximize.Width = 0;
                }
                if (!enableHelp) {
                    mBtnHelp.Width = 0;
                }
                mBtnClose.Left = mForm.Width - mBtnClose.Width;
                mBtnMaximize.Left = mBtnClose.Left - mBtnMaximize.Width - (0 == mBtnMaximize.Width ? 0 : 4);
                mBtnMinimize.Left = mBtnMaximize.Left - mBtnMinimize.Width - 4;
                mBtnHelp.Left = mBtnMinimize.Left - mBtnHelp.Width - 4;

                FormCtrlLeft = mBtnMinimize.Left;
                FormCtrlBottom = mBtnClose.Bottom;
            });
            mForm.Controls.Add(mBtnClose);
            if (enableMaximize) {
                mForm.Controls.Add(mBtnMaximize);
            }
            if (enableHelp) {
                mForm.Controls.Add(mBtnHelp);
            }
            mForm.Controls.Add(mBtnMinimize);

            ((System.ComponentModel.ISupportInitialize)mBtnClose).EndInit();
            ((System.ComponentModel.ISupportInitialize)mBtnMaximize).EndInit();
            ((System.ComponentModel.ISupportInitialize)mBtnMinimize).EndInit();
            ((System.ComponentModel.ISupportInitialize)mBtnHelp).EndInit();
        }
    }
}
