using BE_GestionRRHH;
using BL_GestionRRHH;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_GestionRRHH.Formularios.Correspondencia
{
    public partial class frmCorrespondencia_EmailEdit : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;
        string _docTrabajador = string.Empty;
        public frmCorrespondencia_EmailEdit()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            configurar_formulario();
        }
        void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;
        }

        internal void CargarDatos(string dni_trabajador)
        {
            _docTrabajador = dni_trabajador;
            var correos = unit.EmailingBoleta.ConsultaVarias_EmailingBoletas<eTrabajador>(
                new pEmailingBoleta() { Opcion = 8, Dsc_documento_trabajadorSplit = _docTrabajador });
            if (correos != null && correos.Count > 0)
            {
                var obj = correos[0];
                txtEmail1.Text = obj.dsc_mail_1;
                txtEmail2.Text = obj.dsc_mail_2;
                lblTrabajador.Text = $"Trabajador: {obj.dsc_nombres_completos}";
            }
        }
        bool ValidateEmail(string email) { return new EmailAddressAttribute().IsValid(email); }
        void Actualizar()
        {
            if (!ValidateEmail(txtEmail1.Text))
            {
                unit.Globales.Mensaje(blGlobales.TipoMensaje.Alerta, "El correo no es válido, vuelve a intentarlo.", "Actualizar Correo");
                txtEmail1.Focus();
                return;
            }
            if (!ValidateEmail(txtEmail2.Text) && txtEmail2.Text.Length > 0)
            {
                unit.Globales.Mensaje(blGlobales.TipoMensaje.Alerta, "El correo no es válido, vuelve a intentarlo.", "Actualizar Correo");
                txtEmail2.Focus();
                return;
            }


            var result = unit.EmailingBoleta.ConsultaVarias_EmailingBoletas<eSqlMessage>(
                new pEmailingBoleta()
                {
                    Opcion = 9,
                    Dsc_email_trabajadorSplit = txtEmail1.Text.ToLower().Trim(), //Email1, obligatorio
                    Dsc_archivo_enviadoSplit = txtEmail2.Text.ToLower().Trim(), // Email2, opcional
                    Cod_usuario_enviado = Program.Sesion.Usuario.cod_usuario,
                    Dsc_documento_trabajadorSplit = _docTrabajador // DNI para actualizar todos los registros...
                });
            if (result != null && result.Count > 0)
            {
                if (result[0].IsSuccess) { this.Close(); }

            }
            //var sms = result[0].Outmessage;
        }

        private void frmCorrespondencia_EmailEdit_Load(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Actualizar();
        }
    }
}