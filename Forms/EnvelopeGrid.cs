using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Forms;

public abstract class EnvelopeGrid {
    public int ColumnWidth { get; protected set; }

    protected const int TimeHeight = 30;
    protected const int LeftFrameWidth = 65;

    protected int DispUnit;

    protected PictureBox mPicHeader;
    protected PictureBox mPicValue;
    protected PictureBox mPicLeftFrame;

    protected Bitmap mBmpHeader;
    protected Graphics mGHeader;
    protected Bitmap mBmpValue;
    protected Graphics mGValue;

    protected Bitmap mBmpHeaderBackground;
    protected Graphics mGHeaderBackground;
    protected Bitmap mBmpValueBackground;
    protected Graphics mGValueBackground;

    protected Bitmap mBmpLeftFrame;
    protected Graphics mGLeftFrame;

    public int Attack { get; protected set; }
    public int Hold { get; protected set; }
    public int Decay { get; protected set; }
    public int Release { get; protected set; }
    public int Range { get; protected set; }

    public int DAttack { get; protected set; }
    public int DHold { get; protected set; }
    public int DDecay { get; protected set; }
    public int DRelease { get; protected set; }
    public int DRange { get; protected set; }

    public double Rise;
    public double Top;
    public double Sustain;
    public double Fall;

    public EnvelopeGrid(PictureBox picHeader, PictureBox picLeftFrame, PictureBox picValue) {
        mPicHeader = picHeader;
        mPicValue = picValue;
        mPicLeftFrame = picLeftFrame;
        Minimize();
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

    public void DrawHeader() {
        if (null != mBmpHeader) {
            mBmpHeader.Dispose();
            mBmpHeader = null;
            mGHeader.Dispose();
            mGHeader = null;
        }
        if (null != mPicHeader.Image) {
            mPicHeader.Image.Dispose();
            mPicHeader.Image = null;
        }
        mBmpHeader = new Bitmap(ColumnWidth * 5, TimeHeight * 2);
        mGHeader = Graphics.FromImage(mBmpHeader);

        limitHeader();
        drawHeader();

        mPicHeader.Image = mBmpHeader;
    }

    public void DrawValue(int col = -1, Point pos = new Point()) {
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

        drawValue(col, pos);

        mPicValue.Image = mBmpValue;
    }

    public void DrawBackground() {
        if (null != mBmpHeaderBackground) {
            mBmpHeaderBackground.Dispose();
            mBmpHeaderBackground = null;
            mGHeaderBackground.Dispose();
            mGHeaderBackground = null;
        }
        if (null != mBmpLeftFrame) {
            mBmpLeftFrame.Dispose();
            mBmpLeftFrame = null;
            mGLeftFrame.Dispose();
            mGLeftFrame = null;
        }
        if (null != mBmpValueBackground) {
            mBmpValueBackground.Dispose();
            mBmpValueBackground = null;
            mGValueBackground.Dispose();
            mGValueBackground = null;
        }

        if (null != mPicHeader.BackgroundImage) {
            mPicHeader.BackgroundImage.Dispose();
            mPicHeader.BackgroundImage = null;
        }
        if (null != mPicLeftFrame.BackgroundImage) {
            mPicLeftFrame.BackgroundImage.Dispose();
            mPicLeftFrame.BackgroundImage = null;
        }
        if (null != mPicValue.BackgroundImage) {
            mPicValue.BackgroundImage.Dispose();
            mPicValue.BackgroundImage = null;
        }

        drawBackground();

        mGLeftFrame.DrawLine(
            Colors.PTabBorderBold,
            0, mBmpLeftFrame.Height - 1,
            mBmpLeftFrame.Width, mBmpLeftFrame.Height - 1
        );
        mGLeftFrame.DrawLine(Colors.PTabBorderBold, 1, 0, 1, mBmpLeftFrame.Height);
        mGValueBackground.DrawLine(
            Colors.PTabBorderBold,
            0, mBmpValueBackground.Height - 1,
            mBmpValueBackground.Width, mBmpValueBackground.Height - 1
        );
        mGValueBackground.DrawLine(
            Colors.PTabBorderBold,
            mBmpValueBackground.Width - 1, 0,
            mBmpValueBackground.Width - 1, mBmpValueBackground.Height
        );
        mGHeaderBackground.DrawLine(
            Colors.PTabBorderBold,
            mBmpHeaderBackground.Width - 1, 0,
            mBmpHeaderBackground.Width - 1, mBmpHeaderBackground.Height
        );

        mPicHeader.Width = mBmpHeaderBackground.Width;
        mPicHeader.Height = mBmpHeaderBackground.Height;
        mPicLeftFrame.Width = mBmpLeftFrame.Width;
        mPicLeftFrame.Height = mBmpLeftFrame.Height;
        mPicValue.Width = mBmpValueBackground.Width;
        mPicValue.Height = mBmpValueBackground.Height;
        mPicHeader.BackgroundImage = mBmpHeaderBackground;
        mPicLeftFrame.BackgroundImage = mBmpLeftFrame;
        mPicValue.BackgroundImage = mBmpValueBackground;
    }

    public virtual void Maximize(int width, int height) { }

    public virtual void Minimize() { }

    public virtual void MouseMoveHeader(int col, int delta) { }

    protected virtual void drawHeader() {
        var attack = Attack + DAttack;
        var hold = Hold + DHold;
        var decay = Decay + DDecay;
        var release = Release + DRelease;

        mGHeader.Clear(Color.Transparent);
        mGHeader.DrawString(
            attack.ToString("0ms"),
            Fonts.Bold,
            Colors.BTableFont,
            new RectangleF(0, TimeHeight, ColumnWidth, TimeHeight),
            Fonts.AlignMC
        );
        mGHeader.DrawString(
            hold.ToString("0ms"),
            Fonts.Bold,
            Colors.BTableFont,
            new RectangleF(ColumnWidth, TimeHeight, ColumnWidth, TimeHeight),
            Fonts.AlignMC
        );
        mGHeader.DrawString(
            decay.ToString("0ms"),
            Fonts.Bold,
            Colors.BTableFont,
            new RectangleF(ColumnWidth * 2, TimeHeight, ColumnWidth, TimeHeight),
            Fonts.AlignMC
        );
        mGHeader.DrawString(
            release.ToString("0ms"),
            Fonts.Bold,
            Colors.BTableFont,
            new RectangleF(ColumnWidth * 4, TimeHeight, ColumnWidth, TimeHeight),
            Fonts.AlignMC
        );
    }

    protected virtual void drawValue(int col, Point pos) { }

    protected virtual void drawBackground() { }

    protected virtual void limitHeader() {
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
}

public class AmpValues : EnvelopeGrid {
    public AmpValues(PictureBox picTime, PictureBox picLeftFrame, PictureBox picValue)
        : base(picTime, picLeftFrame, picValue) {
        Top = 1.0;
        Sustain = Math.Pow(10.0, -6.0 / 20.0);
        Attack = 1;
        Hold = 1;
        Decay = 10;
        Release = 10;
    }

    public override void Maximize(int width, int height) {
        ColumnWidth = width / 5 / 2 * 2;
        DispUnit = Math.Min(ColumnWidth, height / 11 / 2 * 2);
    }

    public override void Minimize() {
        ColumnWidth = 70;
        DispUnit = 25;
    }

    public override void MouseMoveHeader(int col, int delta) {
        switch (col) {
        case 0:
            DAttack = delta;
            break;
        case 1:
            DHold = delta;
            break;
        case 2:
            DDecay = delta;
            break;
        case 4:
            DRelease = delta;
            break;
        default:
            DrawHeader();
            return;
        }
        DrawHeader();
    }

    protected override void drawValue(int col, Point pos) {
        var db = (int)((DispUnit - pos.Y) * 6.0 * 2 / DispUnit - 0.5) / 2.0;
        var gain = Math.Pow(10.0, db / 20.0);
        switch (col) {
        case 1:
            Top = gain;
            break;
        case 3:
            Sustain = gain;
            break;
        }

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
        var pTop = DispUnit - (int)(dbTop * DispUnit / 6);
        var pSustain = DispUnit - (int)(dbSustain * DispUnit / 6);
        var pFall = pOfs;

        var psTop = pTop;
        if (mPicValue.Height < psTop + 20) {
            psTop = mPicValue.Height - 20;
        }
        var psSustain = pSustain;
        if (mPicValue.Height < psSustain + 20) {
            psSustain = mPicValue.Height - 20;
        }

        //
        mGValue.SmoothingMode = SmoothingMode.AntiAlias;
        mGValue.DrawLine(Colors.PLineA, 0, pRise, ColumnWidth, pTop);
        mGValue.DrawLine(Colors.PLineA, ColumnWidth, pTop, ColumnWidth * 2, pTop);
        mGValue.DrawLine(Colors.PLineA, ColumnWidth * 2, pTop, ColumnWidth * 3, pSustain);
        mGValue.DrawLine(Colors.PLineA, ColumnWidth * 3, pSustain, ColumnWidth * 4, pSustain);
        mGValue.DrawLine(Colors.PLineA, ColumnWidth * 4, pSustain, ColumnWidth * 5, pFall);
        switch (col) {
        case 1:
            mGValue.DrawLine(Colors.PLineAuxiliary, 0, pTop, ColumnWidth * 5, pTop);
            break;
        case 3:
            mGValue.DrawLine(Colors.PLineAuxiliary, 0, pSustain, ColumnWidth * 5, pSustain);
            break;
        }
        mGValue.SmoothingMode = SmoothingMode.None;
        //
        mGValue.FillPie(Colors.BPoint, ColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
        mGValue.DrawArc(Colors.PTableBorderLight, ColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
        mGValue.FillPie(Colors.BPoint, ColumnWidth * 3 - 4, pSustain - 4, 8, 8, 0, 360);
        mGValue.DrawArc(Colors.PTableBorderLight, ColumnWidth * 3 - 4, pSustain - 4, 8, 8, 0, 360);
        //
        mGValue.DrawString(dbTop.ToString("0.0db"), Fonts.Small, Colors.BTableFont, ColumnWidth + 3, psTop);
        mGValue.DrawString(dbSustain.ToString("0.0db"), Fonts.Small, Colors.BTableFont, ColumnWidth * 3 + 3, psSustain);
    }

    protected override void drawBackground() {
        mBmpHeaderBackground = new Bitmap(ColumnWidth * 5, TimeHeight * 2);
        mGHeaderBackground = Graphics.FromImage(mBmpHeaderBackground);
        mBmpLeftFrame = new Bitmap(LeftFrameWidth, DispUnit * 11 + mBmpHeaderBackground.Height);
        mGLeftFrame = Graphics.FromImage(mBmpLeftFrame);
        mBmpValueBackground = new Bitmap(ColumnWidth * 5, DispUnit * 11);
        mGValueBackground = Graphics.FromImage(mBmpValueBackground);

        //
        // time
        //
        mGHeaderBackground.Clear(Colors.CTableHeader);
        mGHeaderBackground.FillRectangle(
            Colors.BTableCell,
            0, TimeHeight,
            ColumnWidth * 3, TimeHeight
        );
        mGHeaderBackground.FillRectangle(
            Colors.BTableCell,
            ColumnWidth * 4, TimeHeight,
            ColumnWidth, TimeHeight
        );
        //
        for (var x = 1; x <= 4; x++) {
            mGHeaderBackground.DrawLine(
                Colors.PTableBorder,
                ColumnWidth * x, 0,
                ColumnWidth * x, mBmpHeaderBackground.Height
            );
        }
        //
        var title = new string[] { "Attack", "Hold", "Decay", "Sustain", "Release" };
        for (int x = 0; x < title.Length; x++) {
            mGHeaderBackground.DrawString(
                title[x],
                Fonts.Bold,
                Colors.BTableFont,
                new RectangleF(ColumnWidth * x, 0, ColumnWidth, TimeHeight),
                Fonts.AlignMC
            );
        }
        //
        mGHeaderBackground.DrawLine(
            Colors.PTableBorderBold,
            0, mBmpHeaderBackground.Height - 1,
            mBmpHeaderBackground.Width, mBmpHeaderBackground.Height - 1
        );

        //
        // left frame
        //
        mGLeftFrame.Clear(Colors.CTableHeader);
        for (int y = 1, db = 0; y <= 11; y += 2, db -= 12) {
            mGLeftFrame.DrawLine(
                Colors.PTableBorder,
                0, DispUnit * y + mBmpHeaderBackground.Height,
                mBmpLeftFrame.Width, DispUnit * y + mBmpHeaderBackground.Height
            );
            mGLeftFrame.DrawString(
                db.ToString("0db"),
                Fonts.Small,
                Colors.BTableFont,
                new RectangleF(-5, DispUnit * (y - 1) + mBmpHeaderBackground.Height, mBmpLeftFrame.Width, DispUnit),
                Fonts.AlignBR
            );
        }
        mGLeftFrame.DrawLine(
            Colors.PTableBorderBold,
            0, mBmpHeaderBackground.Height - 1,
            mBmpLeftFrame.Width, mBmpHeaderBackground.Height - 1
        );
        mGLeftFrame.DrawLine(
            Colors.PTableBorderBold,
            LeftFrameWidth - 1, 0,
            LeftFrameWidth - 1, mBmpLeftFrame.Height
        );

        //
        // value
        //
        mGValueBackground.Clear(Colors.CTableCell);
        for (var x = 1; x <= 4; x++) {
            mGValueBackground.DrawLine(
                Colors.PTableBorder,
                ColumnWidth * x, 0,
                ColumnWidth * x, mBmpValueBackground.Height
            );
        }
        for (var y = 1; y <= 10; y++) {
            mGValueBackground.DrawLine(
                0 == (y % 2) ? Colors.PTableBorderDark : Colors.PTableBorder,
                0, DispUnit * y,
                mBmpValueBackground.Width, DispUnit * y
            );
        }
    }
}

public class CutoffValues : EnvelopeGrid {
    public CutoffValues(PictureBox picTime, PictureBox picLeftFrame, PictureBox picValue)
        : base(picTime, picLeftFrame, picValue) {
        Rise = 20000;
        Top = 4000;
        Sustain = 4000;
        Fall = 4000;
        Attack = 10;
        Hold = 10;
        Decay = 10;
        Release = 10;
    }

    public override void Maximize(int width, int height) {
        ColumnWidth = width / 5 / 2 * 2;
        DispUnit = Math.Min(ColumnWidth, (int)(height / 11.25) / 2 * 2);
    }

    public override void Minimize() {
        ColumnWidth = 70;
        DispUnit = 32;
    }

    public override void MouseMoveHeader(int col, int delta) {
        switch (col) {
        case 0:
            DAttack = delta;
            break;
        case 1:
            DHold = delta;
            break;
        case 2:
            DDecay = delta;
            break;
        case 4:
            DRelease = delta;
            break;
        default:
            DrawHeader();
            return;
        }
        DrawHeader();
    }

    protected override void drawValue(int col, Point pos) {
        var freqPos = mPicValue.Height - pos.Y + DispUnit * 6;
        var freq = Math.Pow(10.0, freqPos * 0.25 / DispUnit);
        switch (col) {
        case 0:
            Rise = freq;
            break;
        case 1:
            Top = freq;
            break;
        case 3:
            Sustain = freq;
            break;
        case 4:
            Fall = freq;
            break;
        }

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

        var pOfs = mPicValue.Height + (int)(DispUnit * 4 * 1.5);
        var pRise = pOfs - (int)(Math.Log10(Rise) * DispUnit * 4);
        var pTop = pOfs - (int)(Math.Log10(Top) * DispUnit * 4);
        var pSustain = pOfs - (int)(Math.Log10(Sustain) * DispUnit * 4);
        var pFall = pOfs - (int)(Math.Log10(Fall) * DispUnit * 4);

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

        //
        mGValue.SmoothingMode = SmoothingMode.AntiAlias;
        mGValue.DrawLine(Colors.PLineA, 0, pRise, ColumnWidth, pTop);
        mGValue.DrawLine(Colors.PLineA, ColumnWidth, pTop, ColumnWidth * 2, pTop);
        mGValue.DrawLine(Colors.PLineA, ColumnWidth * 2, pTop, ColumnWidth * 3, pSustain);
        mGValue.DrawLine(Colors.PLineA, ColumnWidth * 3, pSustain, ColumnWidth * 4, pSustain);
        mGValue.DrawLine(Colors.PLineA, ColumnWidth * 4, pSustain, ColumnWidth * 5, pFall);
        switch (col) {
        case 0:
            mGValue.DrawLine(Colors.PLineAuxiliary, 0, pRise, ColumnWidth * 5, pRise);
            break;
        case 1:
            mGValue.DrawLine(Colors.PLineAuxiliary, 0, pTop, ColumnWidth * 5, pTop);
            break;
        case 3:
            mGValue.DrawLine(Colors.PLineAuxiliary, 0, pSustain, ColumnWidth * 5, pSustain);
            break;
        case 4:
            mGValue.DrawLine(Colors.PLineAuxiliary, 0, pFall, ColumnWidth * 5, pFall);
            break;
        }
        mGValue.SmoothingMode = SmoothingMode.None;
        //
        mGValue.FillPie(Colors.BPoint, -4, pRise - 4, 8, 8, 0, 360);
        mGValue.FillPie(Colors.BPoint, ColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
        mGValue.FillPie(Colors.BPoint, ColumnWidth * 3 - 4, pSustain - 4, 8, 8, 0, 360);
        mGValue.FillPie(Colors.BPoint, ColumnWidth * 5 - 5, pFall - 4, 8, 8, 0, 360);
        mGValue.DrawArc(Colors.PTableBorderLight, -4, pRise - 4, 8, 8, 0, 360);
        mGValue.DrawArc(Colors.PTableBorderLight, ColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
        mGValue.DrawArc(Colors.PTableBorderLight, ColumnWidth * 3 - 4, pSustain - 4, 8, 8, 0, 360);
        mGValue.DrawArc(Colors.PTableBorderLight, ColumnWidth * 5 - 5, pFall - 4, 8, 8, 0, 360);
        //
        mGValue.DrawString(freqToStr(Rise), Fonts.Small, Colors.BTableFont, 3, psRise);
        mGValue.DrawString(freqToStr(Top), Fonts.Small, Colors.BTableFont, ColumnWidth + 3, psTop);
        mGValue.DrawString(freqToStr(Sustain), Fonts.Small, Colors.BTableFont, ColumnWidth * 3 + 3, psSustain);
        mGValue.DrawString(freqToStr(Fall), Fonts.Small, Colors.BTableFont, ColumnWidth * 5 - 3, psFall, Fonts.AlignTR);
    }

    protected override void drawBackground() {
        var ofs10kTo20k = (int)((Math.Log10(20000) - Math.Log10(10000)) * 4 * DispUnit);
        var ofs10kTo20k_TabPageColHeight = ofs10kTo20k + TimeHeight * 2;

        mBmpHeaderBackground = new Bitmap(ColumnWidth * 5, TimeHeight * 2);
        mGHeaderBackground = Graphics.FromImage(mBmpHeaderBackground);
        mBmpLeftFrame = new Bitmap(LeftFrameWidth, DispUnit * 10 + ofs10kTo20k + mBmpHeaderBackground.Height);
        mGLeftFrame = Graphics.FromImage(mBmpLeftFrame);
        mBmpValueBackground = new Bitmap(ColumnWidth * 5, DispUnit * 10 + ofs10kTo20k);
        mGValueBackground = Graphics.FromImage(mBmpValueBackground);

        //
        // time
        //
        mGHeaderBackground.Clear(Colors.CTableHeader);
        mGHeaderBackground.FillRectangle(
            Colors.BTableCell,
            0, TimeHeight,
            ColumnWidth * 3, TimeHeight
        );
        mGHeaderBackground.FillRectangle(
            Colors.BTableCell,
            ColumnWidth * 4, TimeHeight,
            ColumnWidth, TimeHeight
        );
        //
        for (var x = 1; x <= 4; x++) {
            mGHeaderBackground.DrawLine(
                Colors.PTableBorder,
                ColumnWidth * x, 0,
                ColumnWidth * x, mBmpHeaderBackground.Height
            );
        }
        //
        var title = new string[] { "Attack", "Hold", "Decay", "Sustain", "Release" };
        for (int x = 0; x < title.Length; x++) {
            mGHeaderBackground.DrawString(
                title[x],
                Fonts.Bold,
                Colors.BTableFont,
                new RectangleF(ColumnWidth * x, 0, ColumnWidth, TimeHeight),
                Fonts.AlignMC
            );
        }
        //
        mGHeaderBackground.DrawLine(
            Colors.PTableBorderBold,
            0, mBmpHeaderBackground.Height - 1,
            mBmpHeaderBackground.Width, mBmpHeaderBackground.Height - 1
        );

        //
        // left frame
        //
        mGLeftFrame.Clear(Colors.CTableHeader);
        var freq = new string[] { "10kHz", "3.16kHz", "1kHz", "316Hz", "100Hz", "32Hz" };
        for (int i = 0, y = 0; i < freq.Length; i++, y += 2) {
            var liney = DispUnit * y + ofs10kTo20k_TabPageColHeight;
            var texty = DispUnit * (y - 1) + ofs10kTo20k_TabPageColHeight;
            mGLeftFrame.DrawLine(Colors.PTableBorder, 0, liney, mBmpLeftFrame.Width, liney);
            mGLeftFrame.DrawString(
                freq[i],
                Fonts.Small,
                Colors.BTableFont,
                new RectangleF(-5, texty, mBmpLeftFrame.Width, DispUnit),
                Fonts.AlignBR
            );
        }
        //
        mGLeftFrame.DrawLine(
            Colors.PTableBorderBold,
            0, mBmpHeaderBackground.Height - 1,
            mBmpLeftFrame.Width, mBmpHeaderBackground.Height - 1
        );
        mGLeftFrame.DrawLine(
            Colors.PTableBorderBold,
            LeftFrameWidth - 1, 0,
            LeftFrameWidth - 1, mBmpLeftFrame.Height
        );

        //
        // value
        //
        mGValueBackground.Clear(Colors.CTableCell);
        for (var x = 1; x <= 4; x++) {
            mGValueBackground.DrawLine(
                Colors.PTableBorder,
                ColumnWidth * x, 0,
                ColumnWidth * x, mBmpValueBackground.Height
            );
        }
        for (var y = 0; y <= 9; y++) {
            mGValueBackground.DrawLine(
                0 == (y % 2) ? Colors.PTableBorder : Colors.PTableBorderDark,
                0, ofs10kTo20k + DispUnit * y,
                mBmpValueBackground.Width, ofs10kTo20k + DispUnit * y
            );
        }
    }

    private string freqToStr(double freq) {
        if (freq < 1000) {
            return freq.ToString("0Hz");
        } else {
            return (freq / 1000.0).ToString("0.00kHz");
        }
    }
}

public class PitchValues : EnvelopeGrid {
    public PitchValues(PictureBox picTime, PictureBox picLeftFrame, PictureBox picValue)
        : base(picTime, picLeftFrame, picValue) {
        Range = 100;
        Attack = 10;
        Decay = 10;
        Release = 10;
    }

    public override void Maximize(int width, int height) {
        ColumnWidth = width / 5 / 2 * 2;
        DispUnit = Math.Min(ColumnWidth, height / 4 / 2 * 2);
    }

    public override void Minimize() {
        ColumnWidth = 70;
        DispUnit = 64;
    }

    public override void MouseMoveHeader(int col, int delta) {
        switch (col) {
        case 0:
            DAttack = delta;
            break;
        case 1:
            DDecay = delta;
            break;
        case 2:
            DRelease = delta;
            break;
        case 3:
        case 4:
            DRange = delta / 10 * 100;
            DrawValue();
            break;
        default:
            DrawHeader();
            return;
        }
        DrawHeader();
    }

    protected override void drawHeader() {
        var attack = Attack + DAttack;
        var decay = Decay + DDecay;
        var release = Release + DRelease;
        var range = Range + DRange;

        var maxPitch = Math.Max(Math.Abs(Rise), Math.Max(Math.Abs(Top), Math.Abs(Fall)));
        if (range < maxPitch) {
            range = (int)(maxPitch / 100.0 + 0.99) * 100;
            Range = range;
            DRange = 0;
        }

        mGHeader.Clear(Color.Transparent);
        mGHeader.DrawString(
            attack.ToString("0ms"),
            Fonts.Bold,
            Colors.BTableFont,
            new RectangleF(0, TimeHeight, ColumnWidth, TimeHeight),
            Fonts.AlignMC
        );
        mGHeader.DrawString(
            decay.ToString("0ms"),
            Fonts.Bold,
            Colors.BTableFont,
            new RectangleF(ColumnWidth, TimeHeight, ColumnWidth, TimeHeight),
            Fonts.AlignMC
        );
        mGHeader.DrawString(
            release.ToString("0ms"),
            Fonts.Bold,
            Colors.BTableFont,
            new RectangleF(ColumnWidth * 2, TimeHeight, ColumnWidth, TimeHeight),
            Fonts.AlignMC
        );
        mGHeader.DrawString(
            range.ToString("0cent"),
            Fonts.Bold,
            Colors.BTableFont,
            new RectangleF(ColumnWidth * 3, TimeHeight, ColumnWidth * 2, TimeHeight),
            Fonts.AlignMC
        );
    }

    protected override void drawValue(int col, Point pos) {
        var pitchPos = DispUnit * 2 - pos.Y;
        var pitch = (int)(pitchPos * Range / DispUnit + 0.5) / 2;
        switch (col) {
        case 0:
            Rise = pitch;
            break;
        case 1:
            Top = pitch;
            break;
        case 2:
            Fall = pitch;
            break;
        }

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

        var pOfs = DispUnit * 2 - 1;
        var pRise = pOfs - (int)(Rise * DispUnit * 2 / range);
        var pTop = pOfs - (int)(Top * DispUnit * 2 / range);
        var pSustain = pOfs;
        var pFall = pOfs - (int)(Fall * DispUnit * 2 / range);

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

        //
        mGValue.SmoothingMode = SmoothingMode.AntiAlias;
        mGValue.DrawLine(Colors.PLineA, 0, pRise, ColumnWidth, pTop);
        mGValue.DrawLine(Colors.PLineA, ColumnWidth, pTop, ColumnWidth * 2, pSustain);
        mGValue.DrawLine(Colors.PLineA, ColumnWidth * 2, pSustain, ColumnWidth * 3, pFall);
        switch (col) {
        case 0:
            mGValue.DrawLine(Colors.PLineAuxiliary, 0, pRise, ColumnWidth * 3, pRise);
            break;
        case 1:
            mGValue.DrawLine(Colors.PLineAuxiliary, 0, pTop, ColumnWidth * 3, pTop);
            break;
        case 2:
            mGValue.DrawLine(Colors.PLineAuxiliary, 0, pFall, ColumnWidth * 3, pFall);
            break;
        }
        mGValue.SmoothingMode = SmoothingMode.None;
        //
        mGValue.FillPie(Colors.BPoint, -4, pRise - 4, 8, 8, 0, 360);
        mGValue.FillPie(Colors.BPoint, ColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
        mGValue.FillPie(Colors.BPoint, ColumnWidth * 3 - 4, pFall - 4, 8, 8, 0, 360);
        mGValue.DrawArc(Colors.PTableBorderLight, -4, pRise - 4, 8, 8, 0, 360);
        mGValue.DrawArc(Colors.PTableBorderLight, ColumnWidth - 4, pTop - 4, 8, 8, 0, 360);
        mGValue.DrawArc(Colors.PTableBorderLight, ColumnWidth * 3 - 4, pFall - 4, 8, 8, 0, 360);
        //
        mGValue.DrawString(Rise.ToString("0cent"), Fonts.Small, Colors.BTableFont, 3, psRise);
        mGValue.DrawString(Top.ToString("0cent"), Fonts.Small, Colors.BTableFont, ColumnWidth + 3, psTop);
        mGValue.DrawString(Fall.ToString("0cent"), Fonts.Small, Colors.BTableFont, ColumnWidth * 3 - 3, psFall, Fonts.AlignTR);
    }

    protected override void drawBackground() {
        mBmpHeaderBackground = new Bitmap(ColumnWidth * 5, TimeHeight * 2);
        mGHeaderBackground = Graphics.FromImage(mBmpHeaderBackground);
        mBmpValueBackground = new Bitmap(ColumnWidth * 5, DispUnit * 4);
        mGValueBackground = Graphics.FromImage(mBmpValueBackground);

        drawLeftFrame();

        //
        // time
        //
        mGHeaderBackground.Clear(Colors.CTableHeader);
        mGHeaderBackground.FillRectangle(
            Colors.BTableCell,
            0, TimeHeight,
            mBmpHeaderBackground.Width, TimeHeight
        );
        mGHeaderBackground.DrawLine(
            Colors.PTableBorderBold,
            0, mBmpHeaderBackground.Height - 1,
            mBmpHeaderBackground.Width, mBmpHeaderBackground.Height - 1
        );
        //
        for (int x = 1; x <= 3; x++) {
            mGHeaderBackground.DrawLine(
                Colors.PTableBorder,
                ColumnWidth * x, 0,
                ColumnWidth * x, mBmpHeaderBackground.Height
            );
        }
        var title = new string[] { "Attack", "Decay", "Release" };
        for (int x = 0; x < title.Length; x++) {
            mGHeaderBackground.DrawString(
                title[x],
                Fonts.Bold,
                Colors.BTableFont,
                new RectangleF(ColumnWidth * x, 0, ColumnWidth, TimeHeight),
                Fonts.AlignMC
            );
        }
        // range
        mGHeaderBackground.DrawString(
            "Range",
            Fonts.Bold,
            Colors.BTableFont,
            new RectangleF(ColumnWidth * 3, 0, ColumnWidth * 2, TimeHeight),
            Fonts.AlignMC
        );

        //
        // value
        //
        mGValueBackground.Clear(Colors.CTableCell);
        for (int x = 1; x <= 3; x++) {
            mGValueBackground.DrawLine(
                Colors.PTableBorder,
                ColumnWidth * x, 0,
                ColumnWidth * x, mBmpValueBackground.Height
            );
        }
        for (int y = 1; y <= 3; y++) {
            mGValueBackground.DrawLine(
                y == 2 ? Colors.PTableBorderLight : Colors.PTableBorder,
                0, DispUnit * y - 1,
                mBmpValueBackground.Width - ColumnWidth * 2, DispUnit * y - 1
            );
        }
        // range
        mGValueBackground.FillRectangle(
            Colors.BTableHeader,
            ColumnWidth * 3, 0,
            ColumnWidth * 3, mBmpValueBackground.Height
        );
    }

    private void drawLeftFrame() {
        if (null != mBmpLeftFrame) {
            mBmpLeftFrame.Dispose();
            mBmpLeftFrame = null;
            mGLeftFrame.Dispose();
            mGLeftFrame = null;
        }
        if (null != mPicLeftFrame.BackgroundImage) {
            mPicLeftFrame.BackgroundImage.Dispose();
            mPicLeftFrame.BackgroundImage = null;
        }

        mBmpLeftFrame = new Bitmap(LeftFrameWidth, DispUnit * 4 + mBmpHeaderBackground.Height);
        mGLeftFrame = Graphics.FromImage(mBmpLeftFrame);
        mGLeftFrame.Clear(Colors.CTableHeader);

        //
        mGLeftFrame.DrawLine(
            Colors.PTableBorderBold,
            0, mBmpHeaderBackground.Height - 1,
            mBmpLeftFrame.Width, mBmpHeaderBackground.Height - 1
        );
        for (int y = 1; y <= 3; y++) {
            mGLeftFrame.DrawLine(
                y == 2 ? Colors.PTableBorderLight : Colors.PTableBorder,
                0, DispUnit * y + mBmpHeaderBackground.Height - 1,
                mBmpLeftFrame.Width, DispUnit * y + mBmpHeaderBackground.Height - 1
            );
        }
        //
        mGLeftFrame.DrawLine(
            Colors.PTableBorderBold,
            LeftFrameWidth - 1, 0,
            LeftFrameWidth - 1, mBmpLeftFrame.Height
        );
        mGLeftFrame.DrawLine(Colors.PTabBorderBold, 1, 0, 1, mBmpLeftFrame.Height);
        mGLeftFrame.DrawLine(
            Colors.PTabBorderBold,
            0, mBmpLeftFrame.Height - 1,
            mBmpLeftFrame.Width, mBmpLeftFrame.Height - 1
        );
        //
        var range = Range + DRange;
        for (int y = 0; y <= 4; y++) {
            var cent = range - range * y / 2;
            var py = mBmpHeaderBackground.Height + DispUnit * (cent < 0 ? (y - 1) : y);
            var align = cent < 0 ? Fonts.AlignBR : Fonts.AlignTR;
            mGLeftFrame.DrawString(
                cent.ToString("0cent"),
                Fonts.Small,
                Colors.BTableFont,
                new RectangleF(-5, py, LeftFrameWidth, DispUnit),
                align
            );
        }

        mPicLeftFrame.BackgroundImage = mBmpLeftFrame;
    }
}