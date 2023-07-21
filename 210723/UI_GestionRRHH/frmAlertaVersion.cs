using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BE_GestionRRHH;
using BL_GestionRRHH;
using System.Diagnostics;
using System.Configuration;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace UI_GestionRRHH
{
    public partial class frmAlertaVersion : DevExpress.XtraEditors.XtraForm
    {
        private readonly UnitOfWork unit;

        //public frmLogin frmHandler = new frmLogin();
        public eUsuario user;
        readonly eVersion eVersion;
        public string Entorno = "";
        public eVersion objDescargaOrigen;
        public frmAlertaVersion()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            user = new eUsuario();
            eVersion = new eVersion();

        }
        //public frmAlertaVersion(frmLogin frm)
        //{
        //    InitializeComponent();
        //    frmHandler = frm;
        //}

        private void frmAlertaVersion_Load(object sender, EventArgs e)
        {
            //layoutControl1.BackColor = Color.FromArgb(colorVerde[0], colorVerde[1], colorVerde[2]);
            Entorno = "REMOTO"; //blCrypt.Desencrypta(ConfigurationManager.AppSettings[blCrypt.Encrypta("conexion")]);
            if (Entorno == "REMOTO") lblTipoActualizacion.Text = "Actualización remota";
            //else { lblTipoActualizacion.Text = "Actualización local"; }
            bsAlerta.DataSource = null;
            //bsAlerta.DataSource = blVersion.ObtenerListaDetalle<eVersion>(lblVersion.Text);

            CargarHistorialVersiones(lblVersion.Text.Replace("Versión ", ""));
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                lblTipoActualizacion.Text = "Actualizando, por favor espere... ";
                //lblTipoActualizacion.ForeColor = Color.Red ;
                btnAceptar.Enabled = false;
                btnAceptar.Appearance.ForeColor = System.Drawing.Color.Gray;

                //Eliminar carpeta DESCARGAS
                if (Directory.Exists(@"C:\IMPERIUM-Software\Soluciones\RRHH\Soluciones\RRHH\Descargas"))
                {
                    System.IO.Directory.Delete(@"C:\IMPERIUM-Software\Soluciones\RRHH\Descargas");
                }

                //Crear archivo de lotes
                if (File.Exists(@"C:\IMPERIUM-Software\Soluciones\RRHH\ActualizaIMPERIUM-GestionRRHH.bat"))
                {
                    File.Delete(@"C:\IMPERIUM-Software\Soluciones\RRHH\ActualizaIMPERIUM-GestionRRHH.bat");
                }

                string Ejecutable = @"C:\IMPERIUM-Software\Soluciones\RRHH\Imperium-GestionRRHH.exe " + Program.Sesion.SolucionAbrir.Solucion + ", " + Program.Sesion.SolucionAbrir.Token + ", " + Program.Sesion.SolucionAbrir.User + ", " + Program.Sesion.SolucionAbrir.Key + ", " + Program.Sesion.SolucionAbrir.Entorno; ;
                string CD = @"CD \ ";
                string ArchivoBAT = "";

                if (Entorno == "REMOTO")
                {
                    string LineaCopia = @"C:\IMPERIUM-Software\Soluciones\RRHH\Descargas C:\IMPERIUM-Software\Soluciones\RRHH";
                    string LineaBorra = @"C:\IMPERIUM-Software\Soluciones\RRHH\Descargas";

                    //ArchivoBAT = "ECHO OFF \nECHO Copiando archivos del sistema...\nTASKKILL /F /IM Imperium-GestionRRHH.exe\nC:\n" + CD + "\nXCOPY " + LineaCopia + " /s/y/d\nRD " + LineaBorra + " /S /Q\nECHO Ejecutando el sistema...\nSTART " + Ejecutable + "\nEXIT";
                    ArchivoBAT = "ECHO OFF \nECHO Copiando archivos del sistema...\nTASKKILL /F /IM Imperium-GestionRRHH.exe\nC:\n" + CD + "\nXCOPY " + LineaCopia + " /s/y\nRD /S /Q " + LineaBorra
                        + "\nECHO Ejecutando el sistema...\nSTART " + Ejecutable + " \nDEL /F /Q /S " + @"C:\IMPERIUM-Software\Soluciones\RRHH\ActualizaIMPERIUM-GestionRRHH.bat" + "\nEXIT";
                }
                //else
                //{
                //    string LineaCopia = @"\\sl-limfs01\NSV-COLTUR\Sistema C:\IMPERIUM-Software";

                //    ArchivoBAT = "ECHO OFF \nECHO Copiando archivos del sistema...\nTASKKILL /F /IM Imperium-GestionRRHH.exe\nC:\n" + CD + "\nXCOPY " + LineaCopia + " /s/y/d\nECHO Ejecutando el sistema...\nSTART " + Ejecutable + "\nEXIT";
                //}

                File.WriteAllText(@"C:\IMPERIUM-Software\Soluciones\RRHH\ActualizaIMPERIUM-GestionRRHH.bat", ArchivoBAT);


                if (Entorno == "REMOTO")
                {
                    WebClient webClient = new WebClient();

                    if (!Directory.Exists(@"C:\IMPERIUM-Software\Soluciones\RRHH\Descargas"))
                    {
                        Directory.CreateDirectory(@"C:\IMPERIUM-Software\Soluciones\RRHH\Descargas");
                    }

                    if (System.IO.File.Exists(@"C:\IMPERIUM-Software\Soluciones\RRHH\Descargas\IMPERIUM-GestionRRHH.zip"))
                    {
                        System.IO.File.Delete(@"C:\IMPERIUM-Software\Soluciones\RRHH\Descargas\IMPERIUM-GestionRRHH.zip");
                    }

                    //objDescargaOrigen = unit.Version.ObtenerVersion<eVersion>(6);
                    string DescargaOrigen = "";
                    //if (objDescargaOrigen != null) DescargaOrigen = objDescargaOrigen.OrigenDescarga;
                    if (Program.Sesion.Global.RutaDescarga != "") DescargaOrigen = Program.Sesion.Global.RutaDescarga;

                    if (DescargaOrigen != "")
                    {
                        webClient.DownloadFileAsync(new Uri(DescargaOrigen), @"C:\IMPERIUM-Software\Soluciones\RRHH\Descargas\IMPERIUM-GestionRRHH.zip");
                        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completado);
                        webClient.DownloadProgressChanged += Wc_DownloadProgressChanged;
                    }
                }
                //else //Produccion, Desarrollo, QA
                //{
                //    frmPrincipal frmMain = new frmPrincipal();
                //    this.Close();
                //    frmMain.Close();
                //    Process.Start(ConfigurationManager.AppSettings["rutaBatActualiza"].ToString());

                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se encontro el sitio web " + objDescargaOrigen.OrigenDescarga + Environment.NewLine + ex.ToString(), "Acceso no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                // frmHandler.Close(); // eliminar, si no e necesita el Login en esta solución.
                Application.Exit(); // Considerar cerrar la aplicación luego de actualizar la versión.
            }
        }
        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBarActualizado.EditValue = e.ProgressPercentage;
        }

        public void Completado(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(@"C:\IMPERIUM-Software\Soluciones\RRHH\Descargas\IMPERIUM-GestionRRHH.zip"))
                {
                    ZipFile.ExtractToDirectory(@"C:\IMPERIUM-Software\Soluciones\RRHH\Descargas\IMPERIUM-GestionRRHH.zip", @"C:\IMPERIUM-Software\Soluciones\RRHH\Descargas");
                }
                if (System.IO.File.Exists(@"C:\IMPERIUM-Software\Soluciones\RRHH\Descargas\IMPERIUM-GestionRRHH.zip"))
                {
                    System.IO.File.Delete(@"C:\IMPERIUM-Software\Soluciones\RRHH\Descargas\IMPERIUM-GestionRRHH.zip");
                }
                if (System.IO.File.Exists(@"C:\IMPERIUM-Software\Soluciones\RRHH\Descargas\ActualizaIMPERIUM-GestionRRHH.bat"))
                {
                    System.IO.File.Delete(@"C:\IMPERIUM-Software\Soluciones\RRHH\Descargas\ActualizaIMPERIUM-GestionRRHH.bat");
                }
                this.Close();
                // frmHandler.Close();
                Process.Start(@"C:\IMPERIUM-Software\Soluciones\RRHH\ActualizaIMPERIUM-GestionRRHH.bat");
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se encontro el sitio web " + objDescargaOrigen.OrigenDescarga + Environment.NewLine + ex.ToString(), "Acceso no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                // frmHandler.Close();
            }
        }

        private void CargarHistorialVersiones(string version)
        {
            List<eVersion.eVersionDetalle> histVersion = unit.Version.Cargar_HistorialVersiones_Detalle<eVersion.eVersionDetalle>(3, 0, version, Program.Sesion.SolucionAbrir.Solucion);

            bsListadoHistorialDetalle.DataSource = histVersion;
        }

        private void gvHistorialVersiones_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {

        }

        private void gvHistorialVersiones_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvHistorialVersiones_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

        }

        private void gvHistorialVersiones_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }


    }
}