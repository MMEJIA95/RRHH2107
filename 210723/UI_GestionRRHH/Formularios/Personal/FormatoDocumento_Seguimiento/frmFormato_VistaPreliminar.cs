using BE_GestionRRHH.FormatoMD;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_GestionRRHH.Formularios.Documento;
using UI_GestionRRHH.Formularios.FormatoDocumentos;

namespace UI_GestionRRHH.Formularios.Personal.FormatoDocumento_Seguimiento
{
    public partial class frmFormato_VistaPreliminar : HNG_Tools.SimpleModalForm
    {
        private readonly UnitOfWork unit;
        private eTrabajador_InfoFormatoMD_Vista data;
        private List<eFormatoMD_Parametro> _listadoParametros;
        public frmFormato_VistaPreliminar()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            configurar_formulario();
            data = null;
        }

        private void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;
        }
        internal void CargarData(eTrabajador_InfoFormatoMD_Vista dataUuario, string word)
        {
            data = dataUuario;
            _listadoParametros = new List<eFormatoMD_Parametro>();
            _listadoParametros = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eFormatoMD_Parametro>(
                 new pQFormatoMD() { Opcion = 6, });
            /*------------------*/
            Vista(word); 
        }

        private void Vista(string word)
        {
            if (data == null) return;

            //unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Renderizando plantilla", "Cargando...");
            var help = new FormatoMDHelper(recPlantilla);
            help.MostrarDocumento_RichEditControl(word);
            help.MostrarDescripcion_Parametros(data.cod_empresa, data.cod_trabajador, _listadoParametros);

            //SplashScreenManager.CloseForm();
        }
        private void Imprimir()
        {
            //Imprimir:
            // if (lblEmitirFormato.Tag == null) return;


            var frm = new frmImpresora();
            frm.CargarImpresoras();


            frm.ShowDialog();
            // unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Procesando impresión", "Cargando...");
            if (frm.Result == DialogResult.OK)
            {
                //   MatrizEnvioImpresion("imprimir", trabajadorCheck, documentoCheck);
                new FormatoMDHelper().ImpresionSimple(recPlantilla.WordMLText, frm.txtCopia.Text);
            }
            // SplashScreenManager.CloseForm();
        }
        private void ExportarPDF()
        {
            exportarFormatoDocuemento_PDF(true);
        }
        private void EnviarMail()
        {
            if (data == null) return;

            //string path = exportarFormatoDocuemento_PDF();
            //var frm = new frmFormatoEmail();
            //frm.Text = "Formato de Correos";
            //frm.CargarInfo(mailSplit: $"{txtEmail1.Text.Trim()},{txtEmail2.Text.Trim()}",
            //    adjuntoSplit: path,
            //    dscTrabajador: lblNombreTrabajador.Text,
            //    dscDocumento: data.dsc_formatoMD_vinculo,
            //    codEmpresa: cod_empresa);
            //frm.ShowDialog();
        }
        string exportarFormatoDocuemento_PDF(bool openFolder = false)
        {
            // Load a DOCX document.
            //recPlantilla.LoadDocument("Documents\\MovieRentals.docx", DocumentFormat.OpenXml);

            // Specify PDF export options.
            PdfExportOptions options = new PdfExportOptions();

            // Specify document compatibility with the PDF/A-3a specification.
            options.PdfACompatibility = PdfACompatibility.PdfA3a;

            // Export the document to PDF.


            if (data == null) return "";

            var folder = $@"{Application.StartupPath}\\Documento_Exportado\\";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            //Preguntar con que nombre se va a descargar el archivo.
            string file = $"{data.cod_empresa}-{data.cod_trabajador}-{data.dsc_formatoMD_vinculo.ToLower()}";
            string path = $"{folder}{file}.pdf";
            recPlantilla.ExportToPdf(path, options);

            if (openFolder) Process.Start(folder);
            return path;
        }
        private void frmFormato_VistaPreliminar_Load(object sender, EventArgs e)
        {

        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Imprimir();
        }

        private void btnDescargarPDF_Click(object sender, EventArgs e)
        {
            ExportarPDF();
        }

        private void brnEnviarMail_Click(object sender, EventArgs e)
        {
            EnviarMail();
        }
    }
}