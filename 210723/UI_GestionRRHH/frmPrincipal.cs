using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using System.Configuration;
using System.Globalization;
using System.Net;
using BE_GestionRRHH;
using BL_GestionRRHH;
using DevExpress.Utils;
using System.Xml;
using UI_GestionRRHH.Formularios.Sistema.Accesos;
using UI_GestionRRHH.Formularios.Sistema.Sistema;
using UI_GestionRRHH.Formularios.Sistema.Configuracion_del_Sistema;
using UI_GestionRRHH.Formularios.Sistema.Configuraciones_Maestras;
using UI_GestionRRHH.Formularios.Personal;
using System.IO;
using UI_GestionRRHH.Formularios.Documento;
using DevExpress.XtraSplashScreen;
using UI_GestionRRHH.Formularios.Correspondencia;
using DevExpress.XtraEditors;
using UI_GestionRRHH.Formularios.FormatoDocumentos;

namespace UI_GestionRRHH
{
    public partial class frmPrincipal : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly UnitOfWork unit;

        public string cod_empresa = "", Entorno = "LOCAL", Servidor = "", BBDD = "", FormatoFecha = "";
        public string formName;

        public frmPrincipal()
        {
            InitializeComponent();
            unit = new UnitOfWork();
        }
        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            frmDashBoardPrincipal frm = new frmDashBoardPrincipal();
            frm.MdiParent = this;
            frm.Show();
            InhabilitarBotones();
            Inicializar();
            HabilitarBotones();
            btnEliminarExportados.Enabled = true;

            //  retirar en producción
            //btnListadoDocumentacion.Enabled = true;
            //btnMaestroDeDocumentos.Enabled = true;
            //btnImpresionDocumentos.Enabled = true;
            //btnCorrespondencia.Enabled = true;
            //btnControlVacaciones.Enabled = true;
            //btnAusencias.Enabled = true;
            //btnSoluciones.Enabled = true;
            //btnSeguimientoFormatos.Enabled = true;
            //this.frmHandler.Close();
            CrearSoluciones();
        }

        #region Espacio de Soluciones
        void CrearSoluciones()
        {
            var modulos = unit.Sistema.ListarSolucion<eSolucionUsuario_Consulta>(
                opcion: 1, cod_usuario: Program.Sesion.Usuario.cod_usuario);
            if (modulos != null && modulos.Count > 0)
            {
                new ConfigSesion().CrearButtonModulos(modulos, rpgSolucion);
            }
        }
        #endregion


        private void Inicializar()
        {
            string IP = ObtenerIP();
            //ObtenerUsuario();

            Entorno = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("conexion")].ToString());
            string Servidor = Entorno == "LOCAL" ? unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("ServidorLOCAL")].ToString()) : unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("ServidorREMOTO")].ToString());
            string BBDD = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("BBDD")].ToString());
            string Version = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("VersionApp")].ToString());
            string nombrePC = Environment.MachineName;
            lblServidor.Caption = "Conectado a -> " + Servidor + " - " + BBDD;
            lblIPAddress.Caption = "IP : " + IP;
            lblHostName.Caption = "Nombre Equipo : " + nombrePC;
            lblVersion.Caption = "Versión: " + Version;
            lblUsuario.Caption = Program.Sesion.Usuario.dsc_usuario.ToUpper();
            //entorno = Entorno;
            switch (Entorno)
            {
                case "LOCAL": lblEntorno.Caption = "LOCAL"; lblEntorno.ItemAppearance.Normal.BackColor = Color.Green; lblEntorno.ItemAppearance.Normal.ForeColor = Color.White; break;
                case "REMOTO": lblEntorno.Caption = "REMOTO"; lblEntorno.ItemAppearance.Normal.BackColor = Color.DarkKhaki; lblEntorno.ItemAppearance.Normal.ForeColor = Color.Black; break;
            }
            lblEntorno.Caption = Entorno;
            SuperToolTip tool = new SuperToolTip();
            SuperToolTipSetupArgs args = new SuperToolTipSetupArgs();
            args.Contents.Text = Servidor + " -> " + BBDD;
            tool.Setup(args);
            lblServidor.SuperTip = tool;
        }
        private void ObtenerUsuario()
        {
            // user = blUsu.ObtenerUsuarioLogin<eUsuario>(1, Program.Sesion.Usuario.cod_usuario);
            //// revisar
            eUsuario user = unit.Usuario.ObtenerUsuarioLogin<eUsuario>(1, Program.Sesion.Usuario.cod_usuario);
        }

        private string ObtenerIP()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }

        private void InhabilitarBotones()
        {
            foreach (var item in ribbon.Items)
            {
                if (item.GetType() == typeof(BarButtonItem))
                {
                    if (((BarButtonItem)item).Name != "btnCambiarContraseña" && ((BarButtonItem)item).Name != "btnHistorialVersiones" &&
                        ((BarButtonItem)item).Name != "btnAcercaDeSistema")
                    {
                        ((BarButtonItem)item).Enabled = false;
                    }
                }
            }
        }
        private void HabilitarBotones()
        {
            List<eVentana> listPermisos = unit.Sistema.ListarMenuxUsuario<eVentana>(Program.Sesion.Usuario.cod_usuario, null, Program.Sesion.Global.Solucion);

            if (listPermisos.Count > 0)
            {
                for (int i = 0; i < listPermisos.Count; i++)
                {
                    foreach (var item in ribbon.Items)
                    {
                        if (item.GetType() == typeof(BarButtonItem))
                        {
                            if (((BarButtonItem)item).Name == listPermisos[i].dsc_menu)
                            {
                                ((BarButtonItem)item).Enabled = true;
                            }
                        }
                    }
                }
            }
        }

        private void btnListadoUsuario_ItemClick(object sender, ItemClickEventArgs e)
        {
            //formName = "ListaUsuarios";
            //if (Application.OpenForms["frmListadoUsuario"] != null)
            //{
            //    Application.OpenForms["frmListadoUsuario"].Activate();
            //}
            //else
            //{
            //    frmListadoUsuario frm = new frmListadoUsuario();
            //    frm.MdiParent = this;
            //    frm.Show();
            //}
            new HNG.SistemasHNG().Unit.Forms.OpenMdiListaUsuarios(this);
        }

        private void btnOpcionesSistema_ItemClick(object sender, ItemClickEventArgs e)
        {
            //formName = "ListaOpcionesSistema";
            //if (Application.OpenForms["frmOpcionesSistema"] != null)
            //{
            //    Application.OpenForms["frmOpcionesSistema"].Activate();
            //}
            //else
            //{
            //    frmOpcionesSistema frm = new frmOpcionesSistema();
            //    frm.MdiParent = this;
            //    frm.Show();
            //}
            new HNG.SistemasHNG().Unit.Forms.OpenMdiListaOpcionesSistema(this);
        }
        
        private void btnAsignacionPermiso_ItemClick(object sender, ItemClickEventArgs e)
        {
            //formName = "ListaPermisos";
            //if (Application.OpenForms["frmAsignacionPermiso"] != null)
            //{
            //    Application.OpenForms["frmAsignacionPermiso"].Activate();
            //}
            //else
            //{
            //    frmAsignacionPermiso frm = new frmAsignacionPermiso();
            //    frm.MdiParent = this;
            //    frm.Show();
            //}
            new HNG.SistemasHNG().Unit.Forms.OpenMdiListaPermisos(this);
        }

        private void btnUndNegocioTipoGastoCostoEmpresa_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (Application.OpenForms["frmMantUnidades_Negocio"] != null)
            //{
            //    Application.OpenForms["frmMantUnidades_Negocio"].Activate();
            //}
            //else
            //{
            //    frmMantUnidades_Negocio frm = new frmMantUnidades_Negocio();
            //    frm.MdiParent = this;
            //    frm.Show();
            //}
            new HNG.SistemasHNG().Unit.Forms.OpenMdiMantUnidades_Negocio(this);
        }

        private void btnTipoCambio_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (Application.OpenForms["frmMantTipoCambio"] != null)
            //{
            //    Application.OpenForms["frmMantTipoCambio"].Activate();
            //}
            //else
            //{
            //    frmMantTipoCambio frm = new frmMantTipoCambio();
            //    frm.ShowDialog();
            //}
            new HNG.SistemasHNG().Unit.Forms.OpenNormalMantTipoCambio("Tipo de Cambio");
        }

        private void btnTipoGastoCosto_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Application.OpenForms["frmMantTipoGastoCosto"] != null)
            {
                Application.OpenForms["frmMantTipoGastoCosto"].Activate();
            }
            else
            {
                frmMantTipoGastoCosto frm = new frmMantTipoGastoCosto();
                frm.ShowDialog();
            }
        }

        private void btnResgistroTrabajador_ItemClick(object sender, ItemClickEventArgs e)
        {
            formName = "Trabajador";
            if (Application.OpenForms["frmMantTrabajador"] != null)
            {
                Application.OpenForms["frmMantTrabajador"].Activate();
            }
            else
            {
                frmMantTrabajador frm = new frmMantTrabajador();
                frm.MiAccion = Trabajador.Nuevo;
                frm.ShowDialog();
            }
        }

        private void btnListadoDocumentacion_ItemClick(object sender, ItemClickEventArgs e)
        {
           // sadasd
            formName = "MisDocumentos";
            if (Application.OpenForms["MisDocumentos"] != null)
            {
                Application.OpenForms["MisDocumentos"].Activate();
            }
            else
            {
                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Renderizando plantilla", "Cargando...");

                var frm = new frmFormatoMD_Vinculo();
                frm.MdiParent = this;
                frm.Show();
                SplashScreenManager.CloseForm();
            }
        }

        private void btnConfiguracionDocumento_ItemClick(object sender, ItemClickEventArgs e)
        {
           // asdasdsad
           
        }

        private void btnListadoTrabajador_ItemClick(object sender, ItemClickEventArgs e)
        {
            formName = "ListadoTrabajador";
            if (Application.OpenForms["frmListadoTrabajador"] != null)
            {
                Application.OpenForms["frmListadoTrabajador"].Activate();
            }
            else
            {
                frmListadoTrabajador frm = new frmListadoTrabajador();
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnEliminarExportados_ItemClick(object sender, ItemClickEventArgs e)
        {
            //OBTENEMOS LA RUTA DONDE ESTAN LOS ARCHIVOS DESCARGADOS
            var carpeta = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString());
            DirectoryInfo source = new DirectoryInfo(carpeta);
            FileInfo[] filesToCopy = source.GetFiles();
            foreach (FileInfo oFile in filesToCopy)
            {
                oFile.Delete();
            }
            MessageBox.Show("Se procedió a eliminar los archivos exportados del sistema", "Eliminar documentos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnMaestroDeDocumentos_ItemClick(object sender, ItemClickEventArgs e)
        {
            formName = "MaestroDocumento";
            if (Application.OpenForms["frmMaestroDocumento"] != null)
            {
                Application.OpenForms["frmMaestroDocumento"].Activate();
            }
            else
            {
                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Renderizando plantilla", "Cargando...");
               
                var frm = new Formularios.Documento.frmFormatoMD_General();
                frm.MdiParent = this;
                frm.Show();

                SplashScreenManager.CloseForm();

            }
        }

        private void btnImpresionDocumentos_ItemClick(object sender, ItemClickEventArgs e)
        {
            formName = "ImpresionDocumento";
            if (Application.OpenForms["frmDocumentosImpresion"] != null)
            {
                Application.OpenForms["frmDocumentosImpresion"].Activate();
            }
            else
            {
                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Renderizando plantilla", "Cargando...");

                var frm = new Formularios.Documento.frmFormatoMD_Impresion(TipoImpresion.Principal);
                frm.CargarTipoImpresion();
                frm.MdiParent = this;
                frm.Show();

                SplashScreenManager.CloseForm();

            }
        }

        private void btnCorrespondencia_ItemClick(object sender, ItemClickEventArgs e)
        {
            formName = "Correspondencia_BoletaPago";
            if (Application.OpenForms["frmCorrespondencia_BoletaPago"] != null)
            {
                Application.OpenForms["frmCorrespondencia_BoletaPago"].Activate();
            }
            else
            {
                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Renderizando plantilla", "Cargando...");
                var frm = new frmCorrespondencia_BoletaPago();
                frm.MdiParent = this;
                frm.Show();
                SplashScreenManager.CloseForm();
            }
        }

        private void btnControlVacaciones_ItemClick(object sender, ItemClickEventArgs e)
        {
        //    formName = "Vacaciones_Listado";
        //    if (Application.OpenForms["frmVacaciones_Listado"] != null)
        //    {
        //        Application.OpenForms["frmVacaciones_Listado"].Activate();
        //    }
        //    else
        //    {
        //        unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Renderizando plantilla", "Cargando...");
        //        var frm = new Formularios.Personal.Vacaciones_Permisos.frmVacaciones_Listado();
        //        frm.MdiParent = this;
        //        frm.Show();
        //        SplashScreenManager.CloseForm();
        //    }
        }

        private void btnSoluciones_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnAusencias_ItemClick(object sender, ItemClickEventArgs e)
        {
        //    formName = "Ausencia_Listado";
        //    if (Application.OpenForms["frmAusencia_Listado"] != null)
        //    {
        //        Application.OpenForms["frmAusencia_Listado"].Activate();
        //    }
        //    else
        //    {
        //        unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Renderizando plantilla", "Cargando...");
        //        var frm = new Formularios.Personal.Vacaciones_Permisos.frmAusencia_Listado();
        //        frm.MdiParent = this;
        //        frm.Show();
        //        SplashScreenManager.CloseForm();
        //    }
        }

        private void btnSeguimientoFormatos_ItemClick(object sender, ItemClickEventArgs e)
        {
            

            formName = "FormatoMD_Seguimiento";
            if (Application.OpenForms["frmFormatoMD_Seguimiento"] != null)
            {
                Application.OpenForms["frmFormatoMD_Seguimiento"].Activate();
            }
            else
            {
                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Renderizando plantilla", "Cargando...");
                var frm = new frmFormatoMD_Seguimiento();
                frm.MdiParent = this;
                frm.Show();
                SplashScreenManager.CloseForm();
            }
        }

        private void btnListadoTrabajadorContabilidad_ItemClick(object sender, ItemClickEventArgs e)
        {
            formName = "frmListadoContabilidad";
            if (Application.OpenForms["frmListadoContabilidad"] != null)
            {
                Application.OpenForms["frmListadoContabilidad"].Activate();
            }
            else
            {
                frmListadoContabilidad frm = new frmListadoContabilidad();
               // frm.MdiParent = this;
                frm.Show();
            }
        }

      

        private void btnAsignacionSubmodulos_ItemClick(object sender, ItemClickEventArgs e)
        {
            formName = "frmAsignacionSubmodulos";
            if (Application.OpenForms["frmAsignacionSubmodulos"] != null)
            {
                Application.OpenForms["frmAsignacionSubmodulos"].Activate();
            }
            else
            {
                frmAsignacionSubmodulos frm = new frmAsignacionSubmodulos();
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnHistorialVersiones_ItemClick(object sender, ItemClickEventArgs e)
        {
            //formName = "HistorialVersiones";
            //if (Application.OpenForms["frmHistorialVersiones"] != null)
            //{
            //    Application.OpenForms["frmHistorialVersiones"].Activate();
            //}
            //else
            //{
            //    frmHistorialVersiones frm = new frmHistorialVersiones();
            //    frm.ShowDialog();
            //}
            new HNG.SistemasHNG().Unit.Forms.OpenNormalHistorialVersiones("Histoial de Versiones");
        }
        private void ribbon_Merge(object sender, DevExpress.XtraBars.Ribbon.RibbonMergeEventArgs e)
        {
            e.MergeOwner.SelectedPage = e.MergeOwner.MergedPages.GetPageByName(e.MergedChild.SelectedPage.Name);
        }

        private void btnCambiarContraseña_ItemClick(object sender, ItemClickEventArgs e)
        {
            //frmCambiarContraseña frm = new frmCambiarContraseña();
            //frm.Show();
            new HNG.SistemasHNG().Unit.Forms.OpenNormalCambiarContraseña("Cambiar Contraseña");
        }

        private void btnAcercaDeSistema_ItemClick(object sender, ItemClickEventArgs e)
        {
            //frmAcercaSistema frm = new frmAcercaSistema();
            //frm.ShowDialog();
            new HNG.SistemasHNG().Unit.Forms.OpenNormalAcercaSistema("Acerca del Sistema");
        }
    }
}