using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_GestionRRHH.Tools;

namespace UI_GestionRRHH.Formularios.Personal.FormatoDocumento_Seguimiento
{

    public partial class frmFormato_Adjuntar : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;
        private readonly Adjunto adjunto;
        private string cod_trabajador;
        public frmFormato_Adjuntar()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            adjunto = new Adjunto(unit);
            cod_trabajador = string.Empty;
            configurar_formulario();
        }
        public class AdjuntoFormatoDatos { public string Archivo { get; set; } public string Identificador { get; set; } public string Formato { get; set; } }
        private List<AdjuntoFormatoDatos> AdjuntoList;
        private void configurar_formulario()
        {
            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoAdjunto, gvListadoAdjunto);
            this.TitleBackColor = Program.Sesion.Colores.Verde;
        }
        internal DialogResult CargarDocumentos(string cod_trabajador = "")
        {
            this.cod_trabajador = cod_trabajador;
            if (!adjunto.CrearRutaPDF(true)) { return DialogResult.Cancel; }
            /*-----------------------------------------------*/
            ProcesarDocumentos(adjunto.PathOrigin);
            /*-----------------------------------------------*/
            return DialogResult.OK;
        }
        private void ProcesarDocumentos(string[] path)
        {
            AdjuntoList = new List<AdjuntoFormatoDatos>();
            //var listTrabajador = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento
            foreach (string file in path)
            {
                var flname = Path.GetFileName(file);
                //MessageBox.Show(file);
                var id = obtenerIdentificadorDelPDF(flname);
                if (id.Equals(cod_trabajador))
                {
                    AdjuntoList.Add(new AdjuntoFormatoDatos()
                    {
                        Archivo = flname,
                        Identificador = id,
                        Formato = ""
                    });
                }
            }

            bsListadoAdjunto.DataSource = AdjuntoList;
            gvListadoAdjunto.RefreshData();
        }
        private string obtenerIdentificadorDelPDF(string ruta) { return ruta.Split('_')[0] ?? ""; }

        private string[] obtenerRutaPDF()
        {
            string[] path = null;
            using (var o = new System.Windows.Forms.OpenFileDialog())
            {
                //o.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                o.Multiselect = true;
                o.Filter = "Archivo PDF |*.pdf";
                o.Title = "Abrir formatos";
                o.ShowDialog();
                if (o.FileName != "") { path = o.FileNames; }
            }
            return path;
        }
        private void frmFormato_Adjuntar_Load(object sender, EventArgs e)
        {

        }
    }
}