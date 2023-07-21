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
    [DefaultEvent("MouseDown")]
    public partial class HeaderForm : DevExpress.XtraEditors.XtraUserControl
    {
        [Browsable(true), Category("HNG Style")]
        public override Color BackColor { get { return _cusTitle.Appearance.BackColor; } set { _cusTitle.Appearance.BackColor = value; } }
        [Browsable(true), Category("HNG Style")]
        public override Color ForeColor { get { return _cusTitle.Appearance.ForeColor; } set { _cusTitle.Appearance.ForeColor = value; } }
        [Browsable(true), Category("HNG Style")]
        public override string Text
        {
            get => base.Text; set
            {
                base.Text = value ?? "";
                _cusTitle.Text = value ?? "";
            }
        }

        public HeaderForm()
        {
            InitializeComponent();
            _cusTitle.Text = base.Text;
        }

        private void HeaderForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' || e.KeyChar == (char)Keys.Escape)
                e.Handled = true;
        }

        private void _cusTitle_MouseDown(object sender, MouseEventArgs e)
        {
            this.OnMouseDown(e);
        }

        private void _cusTitle_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }
    }
}
