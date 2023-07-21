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
using BE_GestionRRHH;
using BL_GestionRRHH;

namespace UI_GestionRRHH.Formularios.Personal
{
    public partial class frmMensaje : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;
        public string mensaje = "", dsc_documento = "", resultado = "", opcion = "", dni = "", tipo_documento,empresa="";

        private void lkpTipoDocumento_EditValueChanged(object sender, EventArgs e)
        {


            txtmensaje.Text = "INGRESE EL NÚMERO DE " + lkpTipoDocumento.Text.ToString();
            if (lkpTipoDocumento.EditValue != null)
            {
                eProveedor obj = new eProveedor();
                obj = unit.Proveedores.Validar_NroDocumento<eProveedor>(19, "", lkpTipoDocumento.EditValue.ToString());
                txtdni.Properties.MaxLength = obj.ctd_digitos;
            }
        }

        private void frmMensaje_Load(object sender, EventArgs e)
        {


        }

        private void txtdni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
             (e.KeyChar != '.'))
            {
                e.Handled = true;
            }


            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;

        }

        public eTrabajador eTrab = new eTrabajador();
        public frmMensaje()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            Text = "VALIDACIÓN DE DOCUMENTO";
            unit.Trabajador.CargaCombosLookUp("TipoDocumentoTrabajador", lkpTipoDocumento, "cod_tipo_documento", "dsc_tipo_documento", "", valorDefecto: true);
            lkpTipoDocumento.EditValue = "DI001";
            txtmensaje.Text = "INGRESE EL NÚMERO DE " + lkpTipoDocumento.Text.ToString();
            configurar_formulario();
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string respuesta = "";

            if (opcion == "1")
            {

                if (txtdni.EditValue != null)
                {
                    eProveedor obj = new eProveedor();
                    obj = unit.Proveedores.Validar_NroDocumento<eProveedor>(19, "", lkpTipoDocumento.EditValue.ToString());
                    int nrodocumento = Convert.ToInt32(txtdni.Text.Length);
                    if (nrodocumento < obj.ctd_digitos) { MessageBox.Show("Los digitos del documento debe ser igual a " + obj.ctd_digitos, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtdni.Focus(); return; }
                    txtdni.Properties.MaxLength = obj.ctd_digitos;
                }


                if (btnGuardar.Text == "VALIDAR")
                {

                    eTrab = unit.Trabajador.Obtener_cod_trabajador<eTrabajador>(79, empresa, txtdni.Text.ToString());
                    if (eTrab != null)
                    {
                        dsc_documento = eTrab.cod_trabajador;
                        txtmensaje.Text = "EL TRABAJADOR YA SE ENCUENTRA REGISTRADO, CODIGO DE TRABAJADOR: " + dsc_documento;
                        respuesta = "SI";
                        btnGuardar.Text = "ACEPTAR";
                        resultado = respuesta;
                        return;
                    }
                    else
                    {
                        tipo_documento = lkpTipoDocumento.EditValue.ToString();
                        dni = txtdni.Text;
                        respuesta = "NO";
                        btnGuardar.Text = "ACEPTAR";
                        resultado = respuesta;
                        return;
                    }

                }


            }
            if (btnGuardar.Text == "ACEPTAR")
            {

                this.Close();
            }


        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            string respuesta = "";
            if (keyData == Keys.Enter)
            {

                validar(opcion = "1");

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void validar(string opcion = "0")
        {
            string respuesta = "";
            if (opcion == "1")
            {

                if (txtdni.EditValue != null)
                {
                    eProveedor obj = new eProveedor();
                    obj = unit.Proveedores.Validar_NroDocumento<eProveedor>(19, "", lkpTipoDocumento.EditValue.ToString());
                    int nrodocumento = Convert.ToInt32(txtdni.Text.Length);
                    if (nrodocumento == 0)
                    {
                        { MessageBox.Show("Debe Ingresar ingresar un número de  " + obj.dsc_tipo_documento, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtdni.Focus(); return; }

                    }else if (nrodocumento < obj.ctd_digitos) { MessageBox.Show("Los digitos del documento debe ser igual a " + obj.ctd_digitos, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtdni.Focus(); return; }
                    txtdni.Properties.MaxLength = obj.ctd_digitos;

                }


                if (btnGuardar.Text == "VALIDAR")
                {

                    eTrab = unit.Trabajador.Obtener_cod_trabajador<eTrabajador>(79, "", txtdni.Text.ToString());
                    if (eTrab != null)
                    {
                        dsc_documento = eTrab.cod_trabajador;
                        txtmensaje.Text = "EL TRABAJADOR YA SE ENCUENTRA REGISTRADO, CODIGO DE TRABAJADOR: " + dsc_documento;
                        respuesta = "SI";
                        btnGuardar.Text = "ACEPTAR";
                        resultado = respuesta;
                        return;
                    }
                    else
                    {
                        tipo_documento = lkpTipoDocumento.EditValue.ToString();
                        dni = txtdni.Text;
                        respuesta = "NO";
                        btnGuardar.Text = "ACEPTAR";
                        resultado = respuesta;
                        return;
                    }

                }

            }
            if (btnGuardar.Text == "ACEPTAR")
            {

                this.Close();
            }

        }
    }
}