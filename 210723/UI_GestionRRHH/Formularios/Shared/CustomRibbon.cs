using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_GestionRRHH.Formularios.Shared
{
    public enum SentPosition { SentToBack = 1, BringToFront = 2 }
    public enum BorderStyles { None = 0, UnderLine = 1, Border = 2, Radius = 3 }
    public partial class CustomRibbon : DevExpress.XtraEditors.PanelControl
    {
        public CustomRibbon()
        {
            InitializeComponent();
            this.Controls.Add(description);

            SetBordeStyle(this._borderStyles);
        }
        #region HNG Properties
        SentPosition _subtitleMove = SentPosition.SentToBack;

        [Browsable(true),Localizable(true), Category("HNG Style")]
        public SentPosition SubtitleMove
        {
            get { return _subtitleMove; }
            set
            {
                _subtitleMove = value;
                switch (_subtitleMove)
                {
                    case SentPosition.SentToBack:
                        this.description.SendToBack();
                        break;
                    case SentPosition.BringToFront:
                        this.description.BringToFront();
                        break;
                }
            }
        }
        [Browsable(true), Localizable(true), Category("HNG Style")]
        public Color SubtitleTextColor { get { return description.ForeColor; } set { description.ForeColor = value; this.Invalidate(); } }
        [Browsable(true), Localizable(true), Category("HNG Style")]
        public string SubtitleText { get { return description.Text; } set { description.Text = value; } }
        [Browsable(true), Localizable(true), Category("HNG Style")]
        public Color SubtitleBackColor { get { return description.BackColor; } set { description.BackColor = value; } }
        #endregion

        #region HNG Styles
        private BorderStyles _borderStyles = BorderStyles.None;
        private int _borderRadius = 0;
        private int _borderSize = 0;
        private bool _underlineStyle = false;
        private Color _borderColor = Color.FromArgb(64, 64, 64);

        [Browsable(true), Localizable(true), Category("HNG Style")]
        public Color BorderColor
        {
            get => _borderColor; set
            {
                _borderColor = value;
                this.Invalidate();
            }
        }

        [Browsable(true), Localizable(true), Category("HNG Style")]
        public BorderStyles BorderStyles
        {
            get { return this._borderStyles; }
            set
            {
                this._borderStyles = value;
                SetBordeStyle(value);
            }
        }
        private void SetBordeStyle(BorderStyles styles)
        {
            var b = this._borderStyles == BorderStyles.UnderLine ? (this._borderSize == 0 ? 2 : this._borderSize)
                    : (this._borderSize == 0 ? 1 : this._borderSize);
            var r = this._borderStyles == BorderStyles.UnderLine ? 1
                : this._borderStyles == BorderStyles.None ? 0
                : this._borderStyles == BorderStyles.Radius ? 5
                : (this._borderRadius == 0 ? 1 : this._borderRadius);

            ManageBorderStyle(styles, b, r);
        }
        private void ManageBorderStyle(BorderStyles @value, int @border = 0, int @radius = 0)
        {
            switch (@value)
            {
                case BorderStyles.None:
                    {
                        this._underlineStyle = false;
                        this._borderSize = 0;
                        this._borderRadius = 0;
                        this.Padding = new Padding(0);
                        this.Invalidate();
                        return;
                    }
                case BorderStyles.UnderLine:
                    {
                        this._underlineStyle = true;
                        this._borderRadius = 1;
                        this._borderSize = @border;
                        this.Padding = new Padding(0, 0, 0, @border);
                        this.Invalidate();
                        return;
                    }
                case BorderStyles.Border:
                    {
                        this._underlineStyle = false;
                        this._borderSize = @border;
                        this._borderRadius = @radius;
                        this.Padding = new Padding(@border);//@border > 3 ? new Padding(4) : new Padding(2);
                        this.Invalidate();
                        return;
                    }
                case BorderStyles.Radius:
                    {
                        this._underlineStyle = false;
                        this._borderSize = @border;
                        this._borderRadius = 5;
                        this.Padding = new Padding((@border * 2) + 1);//@border > 3 ? new Padding(4) : new Padding(2);
                        this.Invalidate();
                        return;
                    }
            }

        }

        [Browsable(true), Localizable(true),ReadOnly(true), Category("HNG Style")]
        public int BorderRadius
        {
            get { return this._borderRadius; }
            set
            {
                SetStyleBorderRadius(value);
            }
        }
        private void SetStyleBorderRadius(int value)
        {
            switch (this.BorderStyles)
            {
                case BorderStyles.None:
                    {
                        this._borderRadius = 0;
                        break;
                    }
                case BorderStyles.UnderLine:
                    {
                        this._borderRadius = 1;
                        break;
                    }
                case BorderStyles.Border:
                    {
                        this._borderRadius = 1;
                        break;
                    }
                case BorderStyles.Radius:
                    {
                        this._borderRadius = 5;
                        break;
                    }
            }
            this._borderRadius = value > 12 ? 12 : value;
        }

        [Browsable(true), Localizable(true), Category("HNG Style")]
        public int BorderSize
        {
            get { return this._borderSize; }
            set
            {
                SetStyleBorderSize(value);
                // ManageBorderStyle(this._borderStyles);
            }
        }

        private void SetStyleBorderSize(int value)
        {
            // bool f = false;
            //switch (this.BorderStyles)
            //{
            //    case BorderStyles.None:
            //        {
            //           // f = true;
            //            this._borderSize = 0;
            //            break;
            //        }
            //}
            //    case BorderStyles.UnderLine:
            //        {
            //            this._borderSize = 2;
            //            break;
            //        }
            //case BorderStyles.Border:
            //    {
            //        this._borderSize = 2;
            //        break;
            //    }
            //case BorderStyles.Radius:
            //    {
            //        this._borderSize = 2;
            //        break;
            //    }
            //}
            this._borderSize = /*f ? 0 : */value > 4 ? 4 : value;
            //SetBordeStyle(this.BorderStyles);
            //ManageBorderStyle(_borderStyles, this._borderSize);

        }
        #endregion

        #region HNG Events Styles
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_borderStyles == BorderStyles.None) return;

            Graphics graphics = e.Graphics;
            if (_borderRadius > 1 && _borderSize > 0)
            {
                var rectangleBorderSmooth = this.ClientRectangle;
                var rectangleBorder = Rectangle.Inflate(rectangleBorderSmooth, -_borderSize, -_borderSize);
                int smoothSize = _borderSize > 0 ? _borderSize : 1;
                using (GraphicsPath pathSmooth = GetGraphicsPath(rectangleBorderSmooth, _borderRadius))
                using (GraphicsPath pathBorder = GetGraphicsPath(rectangleBorder, (_borderRadius - _borderSize)))
                using (Pen penSmooth = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(_borderColor, _borderSize))
                {
                    this.Region = new Region(pathSmooth);
                    //if (_borderRadius > 15) SetTextBoxRoundedRegion();
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    penBorder.Alignment = PenAlignment.Center;
                    //if (_isFocused) penBorder.Color = _focusColor;

                    if (_underlineStyle)
                    {
                        graphics.DrawPath(penSmooth, pathSmooth);
                        graphics.SmoothingMode = SmoothingMode.None;
                        graphics.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    }
                    else
                    {
                        graphics.DrawPath(penSmooth, pathSmooth);
                        graphics.DrawPath(penBorder, pathBorder);
                    }
                }
            }
            else
            {
                using (Pen penBorder = new Pen(_borderColor, _borderSize))
                {
                    this.Region = new Region(this.ClientRectangle);
                    penBorder.Alignment = PenAlignment.Inset;
                    //if (_isFocused) penBorder.Color = _focusColor;
                    if (_underlineStyle)
                        graphics.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    else
                        graphics.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                }
            }

        }
        public GraphicsPath GetGraphicsPath(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;
            path.StartFigure();
            path.AddArc(rectangle.X, rectangle.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rectangle.Right - curveSize, rectangle.Y, curveSize, curveSize, 270, 90);
            path.AddArc((rectangle.Right - curveSize), (rectangle.Bottom - curveSize), curveSize, curveSize, 0, 90);
            path.AddArc(rectangle.X, (rectangle.Bottom - curveSize), curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            //if (this.DesignMode)
            //    updateControlHeight();
        }
        #endregion
    }
}
