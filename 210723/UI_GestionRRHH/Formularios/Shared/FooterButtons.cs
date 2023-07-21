using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_GestionRRHH.Formularios.Shared
{

    public enum BtnPosition { Left = 0, Middle = 1, Right = 2 }
    [DefaultEvent("Guardar")]
    public partial class FooterButtons : DevExpress.XtraEditors.XtraUserControl
    {
        private BtnPosition _buttonPosition = BtnPosition.Middle;
        [Category("HNG Style")]
        public event EventHandler Guardar;

        [Category("HNG Style")]
        public event EventHandler Cancelar;

        [Browsable(true), Category("HNG Style")]
        public Color LineColor { get { return _cusLine.BackColor; } set { _cusLine.BackColor = value; } }
        [Browsable(true), Category("HNG Style")]
        public Color SaveBorderColor { get { return _cusSave.Appearance.BorderColor; } set { _cusSave.Appearance.BorderColor = value; } }
        [Browsable(true), Category("HNG Style")]
        public Color SaveBackColor { get { return _cusSave.Appearance.BackColor; } set { _cusSave.Appearance.BackColor = value; } }
        [Browsable(true), Category("HNG Style")]
        public Color CancelBorderColor { get { return _cusCancel.Appearance.BorderColor; } set { _cusCancel.Appearance.BorderColor = value; } }
        [Browsable(true), Category("HNG Style")]
        public Color CancelBackColor { get { return _cusCancel.Appearance.BackColor; } set { _cusCancel.Appearance.BackColor = value; } }
        [Browsable(true), Category("HNG Style")]
        public BtnPosition ButtonPosition
        {
            get { return _buttonPosition; }
            set
            {
                _buttonPosition = value;
                setPosition();
                this.Invalidate();
            }
        }
        public FooterButtons()
        {
            InitializeComponent();
        }

        private void setPosition()
        {
            switch (_buttonPosition)
            {
                case BtnPosition.Left:
                    {
                        _cusCancel.Anchor = AnchorStyles.Top;
                        _cusCancel.Anchor = AnchorStyles.Left;
                        _cusSave.Anchor = AnchorStyles.Top;
                        _cusSave.Anchor = AnchorStyles.Left;
                        break;
                    }
                case BtnPosition.Middle:
                    {
                        _cusCancel.Anchor = AnchorStyles.Top;
                        _cusSave.Anchor = AnchorStyles.Top;
                        break;
                    }
                case BtnPosition.Right:
                    {
                        _cusCancel.Anchor = AnchorStyles.Top;
                        _cusCancel.Anchor = AnchorStyles.Right;
                        _cusSave.Anchor = AnchorStyles.Top;
                        _cusSave.Anchor = AnchorStyles.Right;
                        break;
                    }
            }
        }
        private void FooterButtons_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' || e.KeyChar == (char)Keys.Escape)
                e.Handled = true;
        }

        private void _cusCancel_Click(object sender, EventArgs e)
        {
            if (Cancelar != null)
                Cancelar.Invoke(sender, e);
        }

        private void _cusSave_Click(object sender, EventArgs e)
        {
            if (Guardar != null)
                Guardar.Invoke(sender, e);
        }
    }
}
