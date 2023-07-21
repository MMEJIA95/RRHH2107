using BE_GestionRRHH;
using BL_GestionRRHH;
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

namespace UI_GestionRRHH.Formularios.Correspondencia
{
    public partial class frmCorrespondencia_FormatoParaEnvio : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;
        string _codEmailFormato = string.Empty;
        string _codEmpresa = string.Empty;
        string _codTipoFormato=string.Empty;
        public frmCorrespondencia_FormatoParaEnvio()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            configurar_formulario();
            InicializarDatos();
        }
        void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;
        }

        private void InicializarDatos()
        {
            lblLeyendaFormato.Text = "PARÁMETROS: Asunto <b>{empresa}</b>, Saludo <b>{nombre}</b>, Detalle <b>{periodo}</b>, Despedida ninguno.";
            lblLeyendaFormato.AllowHtmlString = true;
            lblLeyendaFormato.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            lblLeyendaFormato.Appearance.Options.UseTextOptions = true;

        }
        internal void CargarDatos(string codEmpresa, string codTipoFormato)
        {
            _codEmpresa=codEmpresa;
            _codTipoFormato = codTipoFormato;
            var emailFormatos = unit.EmailingBoleta.ConsultaVarias_EmailingBoletas<eEmailFormato>
                (new pEmailingBoleta() { Opcion = 6, Cod_empresaSplit = _codEmpresa, Cod_tipo_formato=_codTipoFormato });
            if (emailFormatos != null && emailFormatos.Count > 0)
            {
                var obj = emailFormatos[0];
                txtAsunto.Text = obj.dsc_asunto;
                txtCuerpo.Text = obj.dsc_cuerpo;
                _codEmailFormato = obj.cod_emailingFormato;
            }
        }

        private void GuardarCambios()
        {
            if (!txtAsunto.Text.ToLower().Contains("{empresa}"))
            {
                unit.Globales.Mensaje(blGlobales.TipoMensaje.Alerta, "En el Asunto es necesario colocar el parámetro {empresa} " +
                    ", vuelve a intentarlo.", "Actualizar Formato");

                return;
            }
            if (!txtCuerpo.Text.ToLower().Contains("{nombre}"))
            {
                unit.Globales.Mensaje(blGlobales.TipoMensaje.Alerta, "En el cuerpo es necesario colocar el parámetro {nombre} " +
                    "después del saludo, vuelve a intentarlo.", "Actualizar Formato");

                return;
            }
            if (!txtCuerpo.Text.ToLower().Contains("{periodo}"))
            {
                unit.Globales.Mensaje(blGlobales.TipoMensaje.Alerta, "En el cuerpo es necesario colocar el parámetro {periodo} " +
                    "que hace referencia al Periodo en curso, vuelve a intentarlo.", "Actualizar Formato");
                txtCuerpo.Focus();
                return;
            }

            var result = unit.EmailingBoleta.Actualizar_FormatoEmail<eSqlMessage>(new eEmailFormato()
            {
                cod_emailingFormato = _codEmailFormato,
                cod_empresa=_codEmpresa,
                cod_tipo_formato= _codTipoFormato,
                dsc_asunto = txtAsunto.Text,
                dsc_cuerpo = txtCuerpo.Text,
                dsc_rutaAdjunto =""// txtAdjunto.Text
            });

            if (result.IsSuccess) { this.Close(); }
            //var sms = result.Outmessage;
        }

        private void frmCorrespondencia_FormatoParaEnvio_Load(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarCambios();
        }
    }
}