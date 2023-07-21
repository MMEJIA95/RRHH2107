using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_GestionRRHH.Formularios.Documento
{
    public partial class frmImpresora : DevExpress.XtraEditors.XtraForm
    {
        internal DialogResult Result;
        internal int numCopia=1;
        public frmImpresora()
        {
            InitializeComponent();
            Result = DialogResult.Cancel;
        }

        internal void CargarImpresoras()
        {
            var obj = new List<PrintDef>();
            int i = 0;
            foreach (string prtn in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                i++;
                obj.Add(new PrintDef()
                {
                    ID =i,
                    PrintName = prtn,
                });
            }

            lckImpresoras.Properties.DataSource = obj.OrderBy(o=>o.PrintName).ToList();
            lckImpresoras.Properties.DisplayMember = "PrintName";
            lckImpresoras.Properties.ValueMember = "ID";

            PrinterSettings settings = new PrinterSettings();
            int index = obj.FindIndex(n => n.PrintName.ToLower().Contains(settings.PrinterName.ToLower()));

            lckImpresoras.EditValue = index+1;
        }
        class PrintDef
        {
            public int ID { get; set; }
            public string PrintName { get; set; }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            numCopia = int.Parse(txtCopia.Text);
            Result = DialogResult.OK;
            this.Close();
        }

        private void chkCopia_CheckedChanged(object sender, EventArgs e)
        {
            txtCopia.ReadOnly = !chkCopia.Checked;
            if (chkCopia.Checked) txtCopia.Focus();
        }

        private void txtCopia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar)) e.Handled = true;
        }
    }
}