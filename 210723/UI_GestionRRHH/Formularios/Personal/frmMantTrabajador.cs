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
using DevExpress.XtraLayout.Utils;
using Microsoft.Identity.Client;
using System.IO;
using DevExpress.XtraSplashScreen;
using System.Security;
using System.Net.Http.Headers;
using System.Configuration;
using BE_GestionRRHH.FormatoMD;
using UI_GestionRRHH.Formularios.FormatoDocumentos;
using System.Diagnostics;
using DevExpress.XtraPrinting;
using UI_GestionRRHH.Formularios.Documento;
using DevExpress.XtraLayout;
using DevExpress.XtraGrid;
using System.Net;
using Tesseract;
using DevExpress.Images;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors.Repository;
using IMPERIUM_Sistema.BE_Sistema;
using System.Threading;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.TextEditController;
using DevExpress.Utils.Extensions;
//using Microsoft.Office.Interop.Excel;

namespace UI_GestionRRHH.Formularios.Personal
{
    internal enum Trabajador
    {
        Nuevo = 0,
        Editar = 1,
        Vista = 2
    }
    public partial class frmMantTrabajador : DevExpress.XtraEditors.XtraForm
    {
        private readonly UnitOfWork unit;

        frmListadoTrabajador frmHandler;
        //  public eUsuario user = new eUsuario();

        internal Trabajador MiAccion = Trabajador.Nuevo;

        public eTrabajador eTrab = new eTrabajador();

        public eTrabajador.eDocumentosFamiliar docfa = new eTrabajador.eDocumentosFamiliar();
        List<eTrabajador.eInfoLaboral_Trabajador> ListHistInfoLaboral = new List<eTrabajador.eInfoLaboral_Trabajador>();
        List<eTrabajador.eContactoEmergencia_Trabajador> ListContactos = new List<eTrabajador.eContactoEmergencia_Trabajador>();
        List<eTrabajador.eCertificadoEMO_Trabajador> ListInfoEMO = new List<eTrabajador.eCertificadoEMO_Trabajador>();
        List<eTrabajador.eInfoLaboral_Trabajador> ListInfoLaboral = new List<eTrabajador.eInfoLaboral_Trabajador>();
        List<eTrabajador.eInfoFamiliar_Trabajador> ListInfoFamiliar = new List<eTrabajador.eInfoFamiliar_Trabajador>();
        List<eTrabajador.eInfoEconomica_Trabajador> ListInfoEconomica = new List<eTrabajador.eInfoEconomica_Trabajador>();
        List<eTrabajador.eInfoAcademica_Trabajador> ListInfoAcademica = new List<eTrabajador.eInfoAcademica_Trabajador>();
        List<eTrabajador.eInfoProfesional_Trabajador> ListInfoProfesional = new List<eTrabajador.eInfoProfesional_Trabajador>();
        List<eTrabajador.eDocumentosFamiliar> ListDocumentosFam = new List<eTrabajador.eDocumentosFamiliar>();
        eTrabajador.eDocumento_Trabajador eTrabdoc = new eTrabajador.eDocumento_Trabajador();
        eTrabajador.eDocumento_Trabajador etrabdocue = new eTrabajador.eDocumento_Trabajador();
        eTrabajador.eEMO etrabemo = new eTrabajador.eEMO();
        List<eTrabajador.eDocumento_Trabajador> ListTrabajdor = new List<eTrabajador.eDocumento_Trabajador>();
        private static IEnumerable<eSubModulo.eCampos> listadocampos;
        List<eSubModulo.eCampos> listabloqueadoscampos = new List<eSubModulo.eCampos>();

        List<eTrabajador.eEMO> ListInfoLaboralEMO = new List<eTrabajador.eEMO>();
        public string ActualizarListado = "NO";
        public string cod_trabajador = "", cod_empresa = "", cod_usuario = "", dsc_documento = "", motivobaja = "", observacionbaja = "", cod_sede_empresa = "",
            flg_documentor = "", id_documentor = "", dsc_archivor = "", accionr = "", nombrearchivo = "", remplazo = "NO",cod_tipopago="";
        DateTime? fechabaja = null;
        public int cod_infofamiliar;
        public DateTime fch_scrt;
        int ContactEmergencia = 0, InfoLaboral = 0, InfoBancaria = 0, DatosAdicionales = 0, InfoFamiliar = 0, InfoEconomica = 0, InfoAcademica = 0, ExpProfesional = 0, InfoSalud = 0, infoemo = 0, InfoVivienda = 0, cod_EMO = 0, InfoDocumentacion = 0;
        Image ImgPDF = DevExpress.Images.ImageResourceCache.Default.GetImage("images/export/exporttopdf_16x16.png");

        //OneDrive
        private Microsoft.Graph.GraphServiceClient GraphClient { get; set; }
        AuthenticationResult authResult = null;
        string[] scopes = new string[] { "Files.ReadWrite.All" };
        string varPathOrigen = "";
        string varNombreArchivo = "", varNombreArchivoSinExtension = "";

        public bool EnableAutoTabOrder { get; set; }

        public frmMantTrabajador()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            ConfigurarGridFormatos();
        }
        public frmMantTrabajador(frmListadoTrabajador frm)
        {
            InitializeComponent();
            unit = new UnitOfWork();
            frmHandler = frm;
            ConfigurarGridFormatos();
        }
        private void frmMantTrabajador_Load(object sender, EventArgs e)
        {
            Inicializar();
            CargarListadoDeFormatos();
            List<string> textEditNames = new List<string>();
            Thread carga = new Thread(listarcampos);
            carga.Start();
            ObtenerNombresControles();



        }
      
        private void ObtenerNombresControles()
        {
            List<string> textEditNames = new List<string>();
            RecorrerControles(this, textEditNames);
            TextEdit textEdit = new TextEdit();
            foreach (string name in textEditNames)
            {
                eSubModulo.eCampos campos = listabloqueadoscampos.Find(x => x.cod_campo == name && x.flg_bloqueo == true);

                if (campos != null)
                {
                    textEdit.Name = campos.cod_campo;
                    textEdit.Enabled = true; // Bloquear el control si el campo es encontrado.
                    break; // Si ya encontró un campo bloqueado, no es necesario seguir buscando.
                }
                else
                {
                    textEdit.ReadOnly = false; // Permitir la edición si el campo no es encontrado.
                }
            }

        }


        private void HabilitarBotones()
        {
            List<eVentana> listPermisos = unit.Sistema.ListarMenuxUsuario<eVentana>(Program.Sesion.Usuario.cod_usuario, frmHandler != null ? frmHandler.Name : "", Program.Sesion.Global.Solucion);

            if (listPermisos.Count > 0)
            {
                if (listPermisos[0].flg_escritura == false) BloqueoControles(false, true, false);
            }
        }
        private void Inicializar()
        {
            try
            {
                switch (MiAccion)
                {
                    case Trabajador.Nuevo:
                        Nuevo();
                        break;
                    case Trabajador.Editar:
                        Editar();
                        HabilitarBotones();
                        break;
                    case Trabajador.Vista:
                        Editar();
                        BloqueoControles(false, true, false);
                        break;
                }

                List<eVentana> listPerfil = unit.Sistema.ListarPerfilesUsuario<eVentana>(4, Program.Sesion.Usuario.cod_usuario, Program.Sesion.Global.Solucion);
                eVentana oPerfil = listPerfil.Find(x => x.cod_perfil == 9 || x.cod_perfil == 8);//adminstrador de planillas-administrador de sistemas
                if (oPerfil == null)
                {
                    txtMontoSueldoBaseInfoLaboral.Properties.UseSystemPasswordChar = true;
                    txtMontoMovilidadInfoLaboral.Properties.UseSystemPasswordChar = true;
                    txtMontoAlimentacionInfoLaboral.Properties.UseSystemPasswordChar = true;
                    txtMontoEscolaridadInfoLaboral.Properties.UseSystemPasswordChar = true;
                    txtMontoBonoInfoLaboral.Properties.UseSystemPasswordChar = true;
                    txtMontoAsigFamiliarInfoLaboral.Properties.UseSystemPasswordChar = true;
                    rtxtMonto.Properties.UseSystemPasswordChar = true;
                    txtMontoSueldoBaseInfoLaboral.ReadOnly = true;
                    txtMontoMovilidadInfoLaboral.ReadOnly = true;
                    txtMontoAlimentacionInfoLaboral.ReadOnly = true;
                    txtMontoEscolaridadInfoLaboral.ReadOnly = true;
                    txtMontoBonoInfoLaboral.ReadOnly = true;
                    txtMontoAsigFamiliarInfoLaboral.ReadOnly = true;
                    chkasignacionFamiliar.ReadOnly = true;
                }
                eVentana Perfilemo = listPerfil.Find(x => x.cod_perfil == 13 || x.cod_perfil == 14);

                if (Perfilemo == null)
                {
                    eTrabajador.eEMO er = gvEMO.GetFocusedRow() as eTrabajador.eEMO;
                    dtFchEvaluacion.Properties.UseSystemPasswordChar = true;
                    dtFchEvaluacionObs.Properties.UseSystemPasswordChar = true;
                    memObservacion.Properties.UseSystemPasswordChar = true;
                    txtArchivoEmo.Properties.UseSystemPasswordChar = true;
                    // btnNuevoEMO.Enabled = false;
                    btnAdjuntarEMO.Enabled = false; btnGuardarEMO.Enabled = false;

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }



        private void Nuevo()
        {
            lblfechacese.Visibility = LayoutVisibility.Never;
            btnActivar.Enabled = false; layoutControlItem215.Visibility = LayoutVisibility.Never;
            lblMotivo.Visible = false; simpleLabelItem76.Visibility = LayoutVisibility.Never;
            lblfechabaja.Visibility = LayoutVisibility.Never; lblfechabaja.Visibility = LayoutVisibility.Never;
            simpleButton2.Visible = false; layoutControlItem260.Visibility = LayoutVisibility.Never;
            lblEstadoTrabajador.Text = "PENDIENTE A REGISTRO";
            string resultado = "";
            string opcion = "1";
            frmMensaje mensaje = new frmMensaje();
            mensaje.txtmensaje.Text = "INGRESE EL NÚMERO DE DNI";
            mensaje.txtdni.Visible = true;
            mensaje.empresa = cod_empresa;
            mensaje.opcion = "1";
            mensaje.btnGuardar.Text = "VALIDAR";
            mensaje.ShowDialog();
            resultado = mensaje.resultado;



            if (resultado == "SI" || resultado == "")
            {
                this.Close();
            }
            else
            {
                CargarLookUpEdit();
                btnNuevo.Enabled = false;
                //acctlMenu.Enabled = false;
                txtCodTrabajador.Text = "";

                glkpTipoDocumento.ItemIndex = 0;
                lkpTipodoc.ItemIndex = 0;
                lkpEstadoCivil.EditValue = "01";
                lkpPais.EditValue = "00001"; lkpDepartamento.EditValue = "00015"; lkpProvincia.EditValue = "00128"; lkpNacionalidad.EditValue = "00001";
                //    dtFecNacimiento.EditValue = DateTime.Today;
                txtNroDocumento.Text = mensaje.dni;


                //dtFecVctoDocumento.EditValue = DateTime.Today;
                lkpSistPensionarioInfoBancaria.EditValue = "AFP";
            }
            txtApellPaterno.Focus(); return;
        }



        private void Editar()
        {
            ContactEmergencia = 0; InfoLaboral = 0; InfoBancaria = 0; DatosAdicionales = 0; InfoFamiliar = 0; InfoEconomica = 0;
            InfoAcademica = 0; ExpProfesional = 0; InfoSalud = 0; InfoVivienda = 0;
            CargarLookUpEdit();
            ObtenerDatos_Trabajador();
            sbtnVerDocumentos.Enabled = true;
            acctlMenu.Enabled = true;
            eTrab = unit.Trabajador.Obtener_Trabajador<eTrabajador>(2, cod_trabajador, cod_empresa);
            if (eTrab.flg_activo == "NO")
            {
                lblEstadoTrabajador.Text = "CESADO"; layoutControlItem315.AppearanceItemCaption.BorderColor = Color.Red;
                lblEstadoTrabajador.AppearanceItemCaption.ForeColor = Color.Red;
                btnEliminarContacto.Enabled = false;
                btnGuardarContacto.Enabled = false;
                btnNuevoContacto.Enabled = false;
                btnGuardar.Enabled = false;
                btnNuevaInfoLaboral.Enabled = false;
                btnGuardarInfoLaboral.Enabled = false;
                btn_clonar.Enabled = false;
                btnEliminarInfoLaboral.Enabled = false;
                btnGuardarInfoBancaria.Enabled = false;
                btnGuardarTallaUnif.Enabled = false;
                btnNuevaInfoFamiliar.Enabled = false;
                btnGuardarInfoFamiliar.Enabled = false;
                btnEliminarInfoFamiliar.Enabled = false;
                btnNuevaInfoEconomica.Enabled = false;
                btnGuardarInfoEconomica.Enabled = false;
                btnEliminarInfoEconomica.Enabled = false;
                btnNuevaFormAcademic.Enabled = false;
                btnGuardarFormAcademic.Enabled = false;
                btnEliminarFormAcademic.Enabled = false;
                btnNuevaExpProfesional.Enabled = false;
                btnGuardarExpProfesional.Enabled = false;
                btnEliminarExpProfesional.Enabled = false;
                btnGuardarInfoVivienda.Enabled = false;
                btnNuevoEMO.Enabled = false;
                btnGuardarEMO.Enabled = false;
                btnAdjuntarEMO.Enabled = false;
                //simpleButton1.Enabled = false;
                btnAdjuntarEMO.Enabled = false;
                chkDNIFAMILIAR.Enabled = false;
                chkCERTIFICADOESTUDIOS.Enabled = false;
                btnGuardarInfoSalud.Enabled = false;
                // actelDocumentos.Enabled = false;
                btnActivar.Caption = "Activar";
                lblfechacese.Visibility = LayoutVisibility.Always;
                lblMotivo.Visible = true; simpleLabelItem76.Visibility = LayoutVisibility.Always; layoutControlItem215.Visibility = LayoutVisibility.Always;
                lblfechabaja.Visibility = LayoutVisibility.Always; lblfechabaja.Visibility = LayoutVisibility.Always;
                lblfechacese.Visibility = LayoutVisibility.Always;
                simpleButton2.Visible = true; layoutControlItem260.Visibility = LayoutVisibility.Always;
                Image imagenbaja = Properties.Resources.apply_32x32;
                btnActivar.ImageOptions.Image = imagenbaja;
                layoutControlItem315.Size = new Size(329, 130);
                emptySpaceItem26.Size = new Size(329, 5);


            }
            else
            {
                lblEstadoTrabajador.Text = "ACTIVADO";
                btnActivar.Enabled = true;
                lblfechacese.Visibility = LayoutVisibility.Never;
                lblMotivo.Visible = false; simpleLabelItem76.Visibility = LayoutVisibility.Never; layoutControlItem215.Visibility = LayoutVisibility.Never;
                lblfechabaja.Visibility = LayoutVisibility.Never; lblfechabaja.Visibility = LayoutVisibility.Never;
                simpleButton2.Visible = false; lblfechacese.Visibility = LayoutVisibility.Never;
                btnEliminarContacto.Enabled = true;
                btnGuardarContacto.Enabled = true;
                btnNuevoContacto.Enabled = true;
                btnGuardar.Enabled = true;
                btnNuevaInfoLaboral.Enabled = true;
                btnGuardarInfoLaboral.Enabled = true;
                btn_clonar.Enabled = true;
                btnEliminarInfoLaboral.Enabled = true;
                btnGuardarInfoBancaria.Enabled = true;
                btnGuardarTallaUnif.Enabled = true;
                btnNuevaInfoFamiliar.Enabled = true;
                btnGuardarInfoFamiliar.Enabled = true;
                btnEliminarInfoFamiliar.Enabled = true;
                btnNuevaInfoEconomica.Enabled = true;
                btnGuardarInfoEconomica.Enabled = true;
                btnEliminarInfoEconomica.Enabled = true;
                btnNuevaFormAcademic.Enabled = true;
                btnGuardarFormAcademic.Enabled = true;
                btnEliminarFormAcademic.Enabled = true;
                btnNuevaExpProfesional.Enabled = true;
                btnGuardarExpProfesional.Enabled = true;
                btnEliminarExpProfesional.Enabled = true;
                btnGuardarInfoVivienda.Enabled = true;
                btnNuevoEMO.Enabled = true;
                btnGuardarEMO.Enabled = true;
                btnAdjuntarEMO.Enabled = true;
                // simpleButton1.Enabled = true;
                btnAdjuntarEMO.Enabled = true;
                chkDNIFAMILIAR.Enabled = true;
                chkCERTIFICADOESTUDIOS.Enabled = true;
                btnGuardarInfoSalud.Enabled = true;
                layoutControlItem315.Size = new Size(329, 60);
                emptySpaceItem26.Size = new Size(329, 50);
                //emptySpaceItem133.Size = new Size(120, 26);
                // actelDocumentos.Enabled = true;
                layoutControlItem260.Visibility = LayoutVisibility.Never;
                Image img = Properties.Resources.no;
                btnActivar.ImageOptions.Image = img;
                btnActivar.Caption = "Cesar Trabajador";
                lkptipozona.EditValue = eTrab.dsc_tipo_zona;
                lkptipovia.EditValue = eTrab.cod_tipo_via;
                txtnombrezona.Text = eTrab.dsc_nombre_zona;
                txtnombrevia.Text = eTrab.dsc_tipo_via;
                txtnro.Text = eTrab.dsc_nro_vivienda;
                txtdep.Text = eTrab.dsc_departamento_dir;
                txtinterior.Text = eTrab.dsc_interior;
                txtmz.Text = eTrab.dsc_manzana;
                txtlote.Text = eTrab.cod_lote;
                txtkm.Text = Convert.ToString(eTrab.dsc_km);
                txtetapa.Text = eTrab.dsc_etapa;
                txtblock.Text = eTrab.dsc_block;
                lkpPais.EditValue = eTrab.cod_pais;
                lkpDepartamento.EditValue = eTrab.cod_departamento;
                lkpProvincia.EditValue = eTrab.cod_provincia;
                lkpDistrito.EditValue = eTrab.cod_distrito;
            }

        }

        private void BloqueoControles(bool Enabled, bool ReadOnly, bool Editable)
        {
            layoutControlItem63.Enabled = Enabled;
            btnNuevo.Enabled = Enabled;
            btnGuardar.Enabled = Enabled;
            sbtnVerDocumentos.Enabled = Enabled;
            btnAreaEmpresa.Enabled = Enabled;
            btnCargoEmpresa.Enabled = Enabled;
            btnActivar.Enabled = Enabled;
        }
        private void CargarLookUpEdit()
        {
            try
            {
                unit.Trabajador.CargaCombosLookUp("TipoDocumentoTrabajador", glkpTipoDocumento, "cod_tipo_documento", "dsc_tipo_documento", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("EmpresaUsuario", lkpEmpresa, "cod_empresa", "dsc_empresa", "", valorDefecto: true, cod_usuario: Program.Sesion.Usuario.cod_usuario);
                unit.Trabajador.CargaCombosLookUp("EstadoCivil", lkpEstadoCivil, "cod_estado_civil", "dsc_estado_civil", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Tipovia", lkptipovia, "cod_tipo_via", "dsc_tipo_via", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Pais", lkpPais, "cod_pais", "dsc_pais", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Departamento", lkpDepartamento, "cod_departamento", "dsc_departamento", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Provincia", lkpProvincia, "cod_provincia", "dsc_provincia", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Distrito", lkpDistrito, "cod_distrito", "dsc_distrito", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Nacionalidad", lkpNacionalidad, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Sexo", lkpSexo, "cod_sexo", "dsc_sexo", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("TipoZona", lkptipozona, "cod_tipo_zona", "dsc_tipo_zona", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("TipoDocumentor", lkpTipoDocumentoingresos, "cod_documento", "dsc_documento", "", valorDefecto: true, flg_varios: "NO", cod_trabajador: cod_trabajador, cod_empresa: cod_empresa);
                unit.Trabajador.CargaCombosLookUp("TipoDocumento", lkpTipodoc, "cod_documento", "dsc_documento", "", valorDefecto: true, flg_varios: "SI", cod_empresa: cod_empresa);
                

                //Se cambia por el listado de empresas cargados asignados al usuario en la sesión.
                //List<eProveedor_Empresas> listEmpresasUsuario = unit.Proveedores.ListarEmpresasProveedor<eProveedor_Empresas>(11, "", Program.Sesion.Usuario.cod_usuario);
                //lkpEmpresa.EditValue = listEmpresasUsuario[0].cod_empresa;
                lkpEmpresa.EditValue = null;
                if (Program.Sesion.EmpresaList.Count == 1) lkpEmpresa.EditValue = Program.Sesion.EmpresaList[0].cod_empresa;

                lkpPais.EditValue = "00001"; lkpDepartamento.EditValue = "00015"; lkpProvincia.EditValue = "00128";
                if (MiAccion == Trabajador.Nuevo)
                {
                    picAnteriorTrabajador.Enabled = false; picSiguienteTrabajador.Enabled = false; btnNuevo.Enabled = false;
                }
                else
                {
                    picAnteriorTrabajador.Enabled = true; picSiguienteTrabajador.Enabled = true; btnNuevo.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void CargarCombosGridLookup(string nCombo, GridLookUpEdit combo, string campoValueMember, string campoDispleyMember, string campoSelectedValue = "", string cod_condicion = "", bool valorDefecto = false)
        {
            DataTable tabla = new DataTable();
            tabla = unit.Trabajador.ObtenerListadoGridLookup(nCombo, cod_condicion);

            combo.Properties.DataSource = tabla;
            combo.Properties.ValueMember = campoValueMember;
            combo.Properties.DisplayMember = campoDispleyMember;
            if (campoSelectedValue == "") { combo.EditValue = null; } else { combo.EditValue = campoSelectedValue; }
            if (tabla.Columns["flg_default"] != null) if (valorDefecto) combo.EditValue = tabla.Select("flg_default = 'SI'").Length == 0 ? null : (tabla.Select("flg_default = 'SI'"))[0].ItemArray[0];
        }

        //SECCION GENERAL 
        //TablaInfolaboral
        private void ObtenerDatos_Trabajador()
        {


            eTrab = unit.Trabajador.Obtener_Trabajador<eTrabajador>(2, cod_trabajador, cod_empresa);
            lblNombreTrabajador.Text = eTrab.dsc_nombres_completos;
            lblNombreTrabajador.Visibility = LayoutVisibility.Always;
            cod_trabajador = eTrab.cod_trabajador;
            txtCodTrabajador.Text = eTrab.cod_trabajador;
            txtApellPaterno.Text = eTrab.dsc_apellido_paterno;
            txtApellMaterno.Text = eTrab.dsc_apellido_materno;
            txtNombre.Text = eTrab.dsc_nombres;
            dtFecNacimiento.EditValue = eTrab.fch_nacimiento;
            if (eTrab.fch_nacimiento.ToString().Contains("0001")) { dtFecNacimiento.EditValue = null; }
            dtEntregaUnif.EditValue = eTrab.fch_entrega_uniforme;
            if (eTrab.fch_entrega_uniforme.ToString().Contains("0001")) { dtEntregaUnif.EditValue = null; chkfchentregauniforme.Checked = false; } else { chkfchentregauniforme.Checked = true; }
            dtRenovacionUnif.EditValue = eTrab.fch_renovacion_uniforme;
            if (eTrab.fch_renovacion_uniforme.ToString().Contains("0001")) { dtRenovacionUnif.EditValue = null; chkfchRenovacionuniforme.Checked = false; } else { chkfchRenovacionuniforme.Checked = true; }
            lkpEstadoCivil.EditValue = eTrab.cod_estadocivil;
            lkpEmpresa.EditValue = eTrab.cod_empresa;
            glkpTipoDocumento.EditValue = eTrab.cod_tipo_documento;
            txtNroDocumento.Text = eTrab.dsc_documento;
            dtFecVctoDocumento.EditValue = eTrab.fch_vcto_documento;
            //txtNroUbigeoDocumento.Text = eTrab.nro_ubigeo_documento;
            lkpNacionalidad.EditValue = eTrab.cod_nacionalidad;
            chkFlgDNI.CheckState = eTrab.flg_DNI == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkFlgCV.CheckState = eTrab.flg_CV == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkFlgAntPoliciales.CheckState = eTrab.flg_AntPolicial == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkFlgAntPenales.CheckState = eTrab.flg_AntPenal == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkFlgVerifDomiciliaria.CheckState = eTrab.flg_VerifDomiciliaria == "SI" ? CheckState.Checked : CheckState.Unchecked;
            btnVerDocIdentidad.Enabled = eTrab.flg_DNI == "SI" ? true : false;
            btnVerCV.Enabled = eTrab.flg_CV == "SI" ? true : false;
            btnVerAntcPoliciales.Enabled = eTrab.flg_AntPolicial == "SI" ? true : false;
            btnVerAntcPenales.Enabled = eTrab.flg_AntPenal == "SI" ? true : false;
            btnVerVerifDomiciliaria.Enabled = eTrab.flg_VerifDomiciliaria == "SI" ? true : false;
            lkpSistPensionarioInfoBancaria.EditValue = eTrab.cod_sist_pension;
            lkpNombreAFPInfoBancaria.EditValue = eTrab.cod_APF;
            txtNroCUSPPInfoBancaria.Text = eTrab.cod_CUSPP;
            txtDireccion.Text = eTrab.dsc_direccion;
            txtReferencia.Text = eTrab.dsc_referencia;
            lkpPais.EditValue = eTrab.cod_pais;
            lkpDepartamento.EditValue = eTrab.cod_departamento;
            lkpProvincia.EditValue = eTrab.cod_provincia;
            lkpDistrito.EditValue = eTrab.cod_distrito;
            txtTelefono.Text = eTrab.dsc_telefono;
            txtCelular.Text = eTrab.dsc_celular;
            txtEmail1.Text = eTrab.dsc_mail_1;
            txtEmail2.Text = eTrab.dsc_mail_2;
            if (grdbTipoPersonal.SelectedIndex == 0) { eTrab.cod_TipoPersonal = "OFICINA"; } else if (grdbTipoPersonal.SelectedIndex == 1) { eTrab.cod_TipoPersonal = "DESTACADO"; } else { eTrab.cod_TipoPersonal = "CRITICO"; }
            lkptipovia.EditValue = eTrab.cod_tipo_via;
            txtnombrevia.Text = eTrab.dsc_tipo_via;
            txtnro.Text = eTrab.dsc_nro_vivienda;
            txtdep.Text = eTrab.dsc_departamento_dir;
            txtinterior.Text = eTrab.dsc_interior;
            txtmz.Text = eTrab.dsc_manzana;
            txtlote.Text = eTrab.cod_lote;
            txtkm.Text = Convert.ToString(eTrab.dsc_km);
            txtetapa.Text = eTrab.dsc_etapa;
            txtblock.Text = eTrab.dsc_block;
            lkptipozona.EditValue = eTrab.dsc_tipo_zona;
            txtnombrezona.Text = eTrab.dsc_nombre_zona;
            lkpSexo.EditValue = eTrab.flg_sexo;
            if (eTrab.flg_activo == "SI")
            {
                lblMotivo.Text = "ACTIVADO"; lblEstadoTrabajador.AppearanceItemCaption.ForeColor = Color.Green;
                layoutControlItem215.Visibility = LayoutVisibility.Never;
                layoutControlItem260.Visibility = LayoutVisibility.Never; lblfechabaja.Visibility = LayoutVisibility.Never; lblfechabaja.Visibility = LayoutVisibility.Always;
                lblfechacese.Visibility = LayoutVisibility.Never;

            }
            lblMotivo.Text = eTrab.motivo_baja;
            lblfechabaja.Text = Convert.ToString(eTrab.Fechabaja).ToString();

            if (eTrab.flg_sexo == "F")
            {
                Image imgEmpresaLarge = Properties.Resources.female64;
                picTrabajador.EditValue = imgEmpresaLarge;
            }
            else if (eTrab.flg_sexo == "M")
            {
                Image imgEmpresaLarge = Properties.Resources.Male64;
                picTrabajador.EditValue = imgEmpresaLarge;
            }

            ObtenerDatos_HistoricoInfoLaboral();
        }

        private void ObtenerDatos_HistoricoInfoLaboral()
        {
            ListHistInfoLaboral = unit.Trabajador.ListarTrabajadores<eTrabajador.eInfoLaboral_Trabajador>(4, cod_trabajador, lkpEmpresa.EditValue.ToString());
            bsHistorialInfoLaboral.DataSource = ListHistInfoLaboral; gvInfoLaboral.RefreshData();

        }


        //SECCION INFORMACION SALUD

        private void ObtenerDatos_InfoSalud()
        {
            eTrabajador.eInfoSalud_Trabajador obj = new eTrabajador.eInfoSalud_Trabajador();
            obj = unit.Trabajador.Obtener_Trabajador<eTrabajador.eInfoSalud_Trabajador>(6, cod_trabajador, cod_empresa);
            if (obj == null) return;
            chkflgAlergiasInfoSalud.CheckState = obj.flg_alergias == "SI" ? CheckState.Checked : CheckState.Unchecked;
            mmAlergias.Text = obj.dsc_alergias;
            chkflgOperacionesInfoSalud.CheckState = obj.flg_operaciones == "SI" ? CheckState.Checked : CheckState.Unchecked;
            mmOperaciones.Text = obj.dsc_operaciones;
            chkflgEnfPrexistenteInfoSalud.CheckState = obj.flg_enfprexistente == "SI" ? CheckState.Checked : CheckState.Unchecked;
            mmEnfPrexistente.Text = obj.dsc_enfprexistente;
            chkflgTratamientoInfoSalud.CheckState = obj.flg_tratprexistente == "SI" ? CheckState.Checked : CheckState.Unchecked;
            mmTratamiento.Text = obj.dsc_tratprexistente;
            chkflgEnfActualInfoSalud.CheckState = obj.flg_enfactual == "SI" ? CheckState.Checked : CheckState.Unchecked;
            mmEnfActualidad.Text = obj.dsc_enfactual;
            chkflgTratActualInfoSalud.CheckState = obj.flg_tratactual == "SI" ? CheckState.Checked : CheckState.Unchecked;
            mmTratActual.Text = obj.dsc_tratactual;
            chkflgDiscapacidadInfoSalud.CheckState = obj.flg_discapacidad == "SI" ? CheckState.Checked : CheckState.Unchecked;
            mmDiscapacidad.Text = obj.dsc_discapacidad;
            chkflgOtrosInfoSalud.CheckState = obj.flg_otros == "SI" ? CheckState.Checked : CheckState.Unchecked;
            mmOtros.Text = obj.dsc_otros;
            lkpGrupoSanguineoInfoSalud.EditValue = obj.dsc_gruposanguineo;
            lkpEstadoSaludInfoSalud.EditValue = obj.dsc_estadosalud;
            lkpSeguroSaludInfoSalud.EditValue = obj.dsc_segurosalud;
            lkpTipoTrabajadorsalud.EditValue = obj.cod_tipo_trabajador_salud;
            lkpSCRTpension.EditValue = obj.cod_sctr_salud_pension;
            lkpSituacionTrabajador_salud.EditValue = obj.cod_situacion_trabajador_salud;
            lkpregimensalud.EditValue = obj.cod_regimen_salud;
            lkpSCRTsalud.EditValue = obj.cod_sctr_salud;
            dtFchInicscrtSalud.EditValue = obj.fch_scrtsalud;
            dtFchInicscrtPension.EditValue = obj.fch_scrtpension;
            lkpentiedadeps.EditValue = obj.cod_entidadeps;
            cod_usuario = obj.cod_usuario_registro;
        }

        private eTrabajador.eInfoSalud_Trabajador AsignarValores_InfoSalud()
        {
            eTrabajador.eInfoSalud_Trabajador obj = new eTrabajador.eInfoSalud_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = lkpEmpresa.EditValue.ToString();
            obj.flg_alergias = chkflgAlergiasInfoSalud.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.dsc_alergias = mmAlergias.Text;
            obj.flg_operaciones = chkflgOperacionesInfoSalud.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.dsc_operaciones = mmOperaciones.Text;
            obj.flg_enfprexistente = chkflgEnfPrexistenteInfoSalud.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.dsc_enfprexistente = mmEnfPrexistente.Text;
            obj.flg_tratprexistente = chkflgTratamientoInfoSalud.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.dsc_tratprexistente = mmTratamiento.Text;
            obj.flg_enfactual = chkflgEnfActualInfoSalud.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.dsc_enfactual = mmEnfActualidad.Text;
            obj.flg_tratactual = chkflgTratActualInfoSalud.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.dsc_tratactual = mmTratActual.Text;
            obj.flg_discapacidad = chkflgDiscapacidadInfoSalud.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.dsc_discapacidad = mmDiscapacidad.Text;
            obj.flg_otros = chkflgOtrosInfoSalud.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.dsc_otros = mmOtros.Text;
            obj.dsc_gruposanguineo = lkpGrupoSanguineoInfoSalud.EditValue == null ? null : lkpGrupoSanguineoInfoSalud.EditValue.ToString();
            obj.dsc_estadosalud = lkpEstadoSaludInfoSalud.EditValue == null ? null : lkpEstadoSaludInfoSalud.EditValue.ToString();
            obj.dsc_segurosalud = lkpSeguroSaludInfoSalud.EditValue == null ? null : lkpSeguroSaludInfoSalud.EditValue.ToString();
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            obj.cod_sctr_salud = lkpSCRTsalud.EditValue == null ? null : lkpSCRTsalud.EditValue.ToString();
            obj.cod_sctr_salud_pension = lkpSCRTpension.EditValue == null ? null : lkpSCRTpension.EditValue.ToString();
            obj.cod_tipo_trabajador_salud = lkpTipoTrabajadorsalud.EditValue == null ? null : lkpTipoTrabajadorsalud.EditValue.ToString();
            obj.cod_situacion_trabajador_salud = lkpSituacionTrabajador_salud.EditValue == null ? null : lkpSituacionTrabajador_salud.EditValue.ToString();
            if (lkpregimensalud.EditValue.ToString() == "00002") { obj.cod_entidadeps = lkpentiedadeps.EditValue == null ? null : lkpentiedadeps.EditValue.ToString(); }
            else { obj.cod_entidadeps = null; }
            obj.cod_regimen_salud = lkpregimensalud.EditValue == null ? null : lkpregimensalud.EditValue.ToString();
            if (lkpSCRTsalud.EditValue.ToString() != "00001") { obj.fch_scrtsalud = Convert.ToDateTime(dtFchInicscrtSalud.EditValue); }
            else { obj.fch_scrtsalud = Convert.ToDateTime(null); }
            if (lkpSCRTpension.EditValue.ToString() != "00001") { obj.fch_scrtpension = Convert.ToDateTime(dtFchInicscrtPension.EditValue); }
            else { obj.fch_scrtpension = Convert.ToDateTime(null); }
            return obj;
        }
        //SECCION EMO

        private eTrabajador.eEMO AsignarValor_EMO()

        {

            string nombrearchivorem = txtArchivoEmo.Text;
            DateTime FechaRegistro = DateTime.Today;

            eTrabajador.eEMO obj = new eTrabajador.eEMO();
            eTrabajador.eEMO er = gvEMO.GetFocusedRow() as eTrabajador.eEMO;
            if (btnGuardarEMO.Text == "GUARDAR")
            {
                obj.cod_EMO = 0; obj.dsc_descripcion = txtArchivoEmo.Text; obj.flg_certificado = flg_documentor; obj.id_certificado = id_documentor;
            }
            else if (btnGuardarEMO.Text == "EDITAR")
                if (remplazo == "SI") { obj.dsc_descripcion = dsc_archivor; obj.flg_certificado = flg_documentor; obj.id_certificado = id_documentor; }
                else if (remplazo == "NO")
                { obj.cod_EMO = er.cod_EMO; obj.dsc_descripcion = er.dsc_descripcion; obj.flg_certificado = er.flg_certificado; obj.id_certificado = er.id_certificado; }
            // obj.cod_EMO = er.cod_EMO;
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = cod_empresa;
            obj.fch_evaluacion = Convert.ToDateTime(dtFchEvaluacion.EditValue);
            obj.dsc_observacion = memObservacion.Text == null ? null : memObservacion.Text;
            obj.fch_evaluacion_obs = Convert.ToDateTime(dtFchEvaluacionObs.EditValue);
            obj.fch_registro = Convert.ToDateTime(DateTime.Today);
            obj.cod_documento = lkpTipodoc.EditValue.ToString();
            obj.dsc_anho = FechaRegistro.Year;
            if (grdbFlgObservado.SelectedIndex == 0) { obj.flg_observacion = "APTO"; } else if (grdbFlgObservado.SelectedIndex == 1) { obj.flg_observacion = "NO APTO"; } else { obj.flg_observacion = "APTO CON RESTRICCIONES"; }
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            obj.nombre_archivoonedrive = nombrearchivorem;

            return obj;
        }

        //SECCION CONTACTO EMERGENCIA



        private void ObtenerDatos_ContactosEmergencia()
        {
            ListContactos = unit.Trabajador.ListarTrabajadores<eTrabajador.eContactoEmergencia_Trabajador>(3, cod_trabajador, cod_empresa);
            bsListaContactos.DataSource = ListContactos; gvListadoContactos.RefreshData();
        }



        private void Cargarlistado_infolaboral()
        {
            eTrabajador.eInfoLaboral_Trabajador obj = new eTrabajador.eInfoLaboral_Trabajador();
            obj = unit.Trabajador.Obtener_Trabajador<eTrabajador.eInfoLaboral_Trabajador>(4, cod_trabajador, lkpEmpresa.EditValue.ToString());
            if (obj == null)
            {

                txtCodInfoLaboral.Text = "0";
                bsListaInfoLaboral.Clear();
                dtFecIngresoInfoLaboral.Enabled = true;
            }
            else
            {
                txtCodInfoLaboral.Text = obj.cod_infolab.ToString();
                ListInfoLaboral = unit.Trabajador.ListarTrabajadores<eTrabajador.eInfoLaboral_Trabajador>(4, cod_trabajador, lkpEmpresa.EditValue.ToString());
                bsListaInfoLaboral.DataSource = ListInfoLaboral; gvListadoInfoLaboral.RefreshData();

            }
        }

        private void ObtenerDatos_EMO()
        {
            ListInfoLaboralEMO = unit.Trabajador.ListarEMO<eTrabajador.eEMO>(17, cod_trabajador, cod_empresa);
            bsListadoInfoEMO.DataSource = ListInfoLaboralEMO; gvEMO.RefreshData();
        }



        private void Obtenervalor_EMO()
        {
            eTrabajador.eEMO obj = new eTrabajador.eEMO();
            obj = gvEMO.GetFocusedRow() as eTrabajador.eEMO;
            if (obj == null) return;
            if (obj.dsc_documento == "CERTIFICADO EMO") { lkpTipodoc.ReadOnly = false; }
            else
            {
                lkpTipodoc.ReadOnly = false;
            }
            if (obj.flg_observacion == "APTO") { grdbFlgObservado.SelectedIndex = 0; }
            if (obj.flg_observacion == "NO APTO") { grdbFlgObservado.SelectedIndex = 1; }
            if (obj.flg_observacion == "APTO CON RESTRICCIONES") { grdbFlgObservado.SelectedIndex = 2; }
            dtFchEvaluacionObs.EditValue = obj.fch_evaluacion_obs;
            dtFchEvaluacion.EditValue = obj.fch_evaluacion;
            memObservacion.Text = obj.dsc_observacion;
            txtArchivoEmo.Text = obj.nombre_archivoonedrive;
            btnGuardarEMO.Text = "EDITAR";
            lkpTipodoc.EditValue = obj.cod_documento;
            if (obj.dsc_descripcion == "") { txtArchivoEmo.Text = "FALTA INGRESAR DOCUMENTO"; txtArchivoEmo.ForeColor = Color.Red; lkpTipodoc.ReadOnly = false; }
            else
            {
                txtArchivoEmo.Text = obj.dsc_descripcion + ".pdf"; txtArchivoEmo.ForeColor = Color.Black;
            }
        }

        private void ObtenerDatos_CaracteristicasTallas()
        {
            eTrabajador.eCaractFisicas_Trabajador objC = new eTrabajador.eCaractFisicas_Trabajador();
            objC = unit.Trabajador.Obtener_Trabajador<eTrabajador.eCaractFisicas_Trabajador>(12, cod_trabajador, cod_empresa);
            if (objC == null) return;
            txtEstaturaCaractFisica.EditValue = objC.dsc_estatura;
            txtPesoCaractFisica.EditValue = objC.dsc_peso;
            txtIMCCaractFisica.EditValue = objC.dsc_IMC;
            chkflgLentesCaractFisica.CheckState = objC.flg_lentes == "SI" ? CheckState.Checked : CheckState.Unchecked;
            eTrabajador.eTallaUniforme_Trabajador objT = new eTrabajador.eTallaUniforme_Trabajador();
            objT = unit.Trabajador.Obtener_Trabajador<eTrabajador.eTallaUniforme_Trabajador>(13, cod_trabajador, cod_empresa);
            if (objT == null) return;
            lkpPoloTallaUnif.EditValue = objT.cod_talla_polo;
            lkpCamisaTallaUnif.EditValue = objT.cod_talla_camisa;
            lkpPantalonTallaUnif.EditValue = objT.cod_talla_pantalon;
            lkpCasacaTallaUnif.EditValue = objT.cod_talla_casaca;
            lkpMamelucoTallaUnif.EditValue = objT.cod_talla_mameluco;
            lkpChalecoTallaUnif.EditValue = objT.cod_talla_chaleco;
            txtCalzadoTallaUnif.EditValue = objT.cod_talla_calzado;
            lkpCascoTallaUnif.EditValue = objT.cod_talla_casco;
            lkpFajaTallaUnif.EditValue = objT.cod_talla_faja;
            txtCasilleroTallaUnif.EditValue = objT.dsc_casillero;
            chkflgLentesTallaUnif.CheckState = objT.flg_lentes == "SI" ? CheckState.Checked : CheckState.Unchecked;
        }

        //private void ObtenerDatos_HistorialEMO()
        //{
        //    ListInfoEMO.Clear();
        //    ListInfoEMO = unit.Trabajador.ListarTrabajadores<eTrabajador.eCertificadoEMO_Trabajador>(15, cod_trabajador, cod_empresa);
        //    bsListadoInfoEMO.DataSource = ListInfoEMO; gvEMO.RefreshData();
        //}

        private void ObtenerDatos_InfoAcademica()
        {
            ListInfoAcademica.Clear();
            ListInfoAcademica = unit.Trabajador.ListarTrabajadores<eTrabajador.eInfoAcademica_Trabajador>(9, cod_trabajador, cod_empresa);
            bsListaInfoAcademica.DataSource = ListInfoAcademica; gvListadoFormAcademica.RefreshData();
        }

        private void ObtenerDatos_InfoProfesional()
        {
            ListInfoProfesional.Clear();
            ListInfoProfesional = unit.Trabajador.ListarTrabajadores<eTrabajador.eInfoProfesional_Trabajador>(10, cod_trabajador, cod_empresa);
            bsListaInfoProfesional.DataSource = ListInfoProfesional; gvListadoExpProfesional.RefreshData();
        }

        private void ObtenerDatos_InfoFamiliar()
        {
            ListInfoFamiliar.Clear();
            ListInfoFamiliar = unit.Trabajador.ListarTrabajadores<eTrabajador.eInfoFamiliar_Trabajador>(7, cod_trabajador, cod_empresa);
            bsListaInfoFamiliar.DataSource = ListInfoFamiliar; gvListadoInfoFamiliar.RefreshData();
        }

        private void ObtenerDatos_InfoEconomica()
        {
            ListInfoEconomica.Clear();
            ListInfoEconomica = unit.Trabajador.ListarTrabajadores<eTrabajador.eInfoEconomica_Trabajador>(8, cod_trabajador, cod_empresa);
            bsListadoInfoEconomica.DataSource = ListInfoEconomica; gvListadoInfoEconomica.RefreshData();
        }

        private void ObtenerDatos_InfoVivienda()
        {
            eTrabajador.eInfoVivienda_Trabajador obj = new eTrabajador.eInfoVivienda_Trabajador();
            obj = unit.Trabajador.Obtener_Trabajador<eTrabajador.eInfoVivienda_Trabajador>(11, cod_trabajador, cod_empresa);
            if (obj == null) return;
            lkpViviendaInfoVivienda.EditValue = obj.cod_tipovivienda;
            lkpComodidadInfoVivienda.EditValue = obj.cod_tipocomodidad;
            chkflgPuertasInfoVivienda.CheckState = obj.flg_puertas == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkflgVentanasInfoVivienda.CheckState = obj.flg_ventanas == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkflgTechoInfoVivienda.CheckState = obj.flg_techo == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkflgTelefonoInfoVivienda.CheckState = obj.flg_telefono == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkCelularesInfoVivienda.CheckState = obj.flg_celulares == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkflgInternetComunicacionInfoVivienda.CheckState = obj.flg_internet_comunicacion == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkflgLuzInfoVivienda.CheckState = obj.flg_luz == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkflgAguaInfoVivienda.CheckState = obj.flg_agua == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkflgDesagueInfoVivienda.CheckState = obj.flg_desague == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkflgGasInfoVivienda.CheckState = obj.flg_gas == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkflgCableInfoVivienda.CheckState = obj.flg_cable == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkflgInternetServicioInfoVivienda.CheckState = obj.flg_internet_servicio == "SI" ? CheckState.Checked : CheckState.Unchecked;
            mmViasAccesoInfoVivienda.Text = obj.dsc_viaacceso;
            mmIluminacionInfoVivienda.Text = obj.dsc_iluminacion;
            mmEntornoInfoVivienda.Text = obj.dsc_entorno;
            mmPuestoPolicialInfoVivienda.Text = obj.dsc_puestopolicial;
            txtNombreFamiliarInfoVivienda.Text = obj.dsc_nombre_familiar;
            txtHorasCasaInfoVivienda.Text = obj.dsc_horasencasa;
            lkpParentescoInfoVivienda.EditValue = obj.cod_parentesco;
            txtCelularInfoVivienda.Text = obj.dsc_celular;
            txtEmailInfoVivienda.Text = obj.dsc_mail;
        }

        private void ObtenerDatos_InfoBancaria()
        {
            eTrabajador.eInfoBancaria_Trabajador obj = new eTrabajador.eInfoBancaria_Trabajador();
            obj = unit.Trabajador.Obtener_Trabajador<eTrabajador.eInfoBancaria_Trabajador>(14, cod_trabajador, cod_empresa);
            if (obj == null) return;
            glkpBancoInfoBancaria.EditValue = obj.cod_banco;
            lkpTipoMonedaInfoBancaria.EditValue = obj.cod_moneda;
            lkpTipoCuentaInfoBancaria.EditValue = obj.cod_tipo_cuenta;
            txtNroCuentaBancariaInfoBancaria.Text = obj.dsc_cta_bancaria;
            txtNroCuentaCCIInfoBancaria.Text = obj.dsc_cta_interbancaria;
            glkpBancoCTSInfoBancaria.EditValue = obj.cod_banco_CTS;
            lkpTipoMonedaCTSInfoBancaria.EditValue = obj.cod_moneda_CTS;
            txtNroCuentaBancariaCTSInfoBancaria.Text = obj.dsc_cta_bancaria_CTS;
            txtNroCuentaCCICTSInfoBancaria.Text = obj.dsc_cta_interbancaria_CTS;
            lkpSistPensionarioInfoBancaria.EditValue = obj.cod_sist_pension;
            lkpNombreAFPInfoBancaria.EditValue = obj.cod_APF;
            txtNroCUSPPInfoBancaria.Text = obj.cod_CUSPP;
            lkpTipoPago.EditValue = obj.cod_tipo_pago;
            lkpPeriodicidadPagos.EditValue = obj.cod_periodicidad_pagos;
            lkpTipoComision.EditValue = obj.cod_tipo_comision_pension;
            lkpTipoCuentaCTS.EditValue = obj.cod_tipo_cuenta_CTS;

       
        }

        private void acctlMenu_SelectedElementChanged(object sender, DevExpress.XtraBars.Navigation.SelectedElementChangedEventArgs e)
        {
            try
            {
                eTrabajador resultado = unit.Trabajador.Obtener_Trabajador<eTrabajador>(2, cod_trabajador, cod_empresa);
                if (resultado == null) { MessageBox.Show("Debe crear al trabajador.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                else
                {
                    switch (e.Element.Name)
                    {
                        case "actelGeneral":
                            navframeTrabajador.SelectedPage = navpageGemeral;
                            break;
                        case "actelContactoEmergencia":

                            navframeTrabajador.SelectedPage = navpageContactoEmergencia;
                            if (ContactEmergencia == 0)
                            {
                                unit.Trabajador.CargaCombosLookUp("Parentesco", lkpParentescoContacto, "cod_parentesco", "dsc_parentesco", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TipoDocumentoTrabajador", glkpTipoDocumentoContacto, "cod_tipo_documento", "dsc_tipo_documento", "", valorDefecto: true);
                                //glkpTipoDocumentoContacto.EditValue = "DI001";
                                //dtFecNacimientoContacto.EditValue = DateTime.Today;
                                ObtenerDatos_ContactosEmergencia();
                                gvListadoContactos_FocusedRowChanged(gvListadoContactos, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
                                ContactEmergencia = 1;


                            }

                            break;

                        case "actelInfoLaboral":

                            navframeTrabajador.SelectedPage = navpageInfoLaboral;



                            if (InfoLaboral == 0)
                            {
                                dtFecIngresoInfoLaboral.Enabled = true;
                                unit.Trabajador.CargaCombosLookUp("TipoContrato", lkpTipoContratoInfoLaboral, "cod_tipoContrato", "dsc_tipoContrato", "", valorDefecto: true);
                                //tipo_contrato();
                                // unit.Trabajador.CargaCombosLookUp("PeriodoPrueba", lkpTiempoPeriodoInfoLaboral, "cod_periodoprueba", "dsc_periodoprueba", "", valorDefecto: true);
                                unit.Factura.CargaCombosLookUp("DistribucionCECO", lkpPrefCECOInfoLaboral, "cod_CECO", "dsc_CECO", "", valorDefecto: true, cod_empresa: cod_empresa);
                                unit.Trabajador.CargaCombosLookUp("EmpresaUsuario", lkpEmpresaInfoLaboral, "cod_empresa", "dsc_empresa", "", valorDefecto: true, cod_usuario: Program.Sesion.Usuario.cod_usuario);
                                unit.Trabajador.CargaCombosLookUp("SituacionTrabajador2", lkpSituacionTrabajador_2, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("Sino", lkpexonerado5, "cod_sino", "dsc_sino", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("Sino", lkpHorarioNocturno, "cod_sino", "dsc_sino", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("Sino", cbxJornadaMaxima, "cod_sino", "dsc_sino", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("Sino", cbxRegimenAtipico, "cod_sino", "dsc_sino", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("Sino", cbxSindicato, "cod_sino", "dsc_sino", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("Sino", lkpafectosctr, "cod_sino", "dsc_sino", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("Sino", lkphorasextras, "cod_sino", "dsc_sino", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("Sino", lkpAfectovidaLey, "cod_sino", "dsc_sino", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("SituacionEspecial", lkpsituacionespecial, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("Ocupacional", lkpOcupacional, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("ConvenioTributacion", lkpConvenioTributacion, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("Categoria", lkpCategoria, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TipoTrabajo", lkpTipoTrabajador, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("CIASegurovidaley", lkpCIAsegurovidaley, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("CentroRiesgo", lkpCentroRiesgo, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("RegimenLaboral", lkpRegimenLaboral, "cod_parametro", "dsc_parametro", "", valorDefecto: true);


                                lkpEmpresaInfoLaboral.EditValue = lkpEmpresa.EditValue;
                                dtFecIngresoInfoLaboral.EditValue = DateTime.Today;
                                dtFecFirmaInfoLaboral.EditValue = DateTime.Today;
                                dtFecVctoInfoLaboral.EditValue = DateTime.Today;
                                lkpexonerado5.EditValue = "NO";
                                lkphorasextras.EditValue = "NO";
                                lkpafectosctr.EditValue = "NO";
                                lkpAfectovidaLey.EditValue = "NO";


                                if (ListInfoFamiliar.Count == 0)
                                {
                                    ListInfoFamiliar = unit.Trabajador.ListarTrabajadores<eTrabajador.eInfoFamiliar_Trabajador>(7, cod_trabajador, cod_empresa);
                                }
                                if (ListInfoFamiliar.Count > 0)
                                {
                                    eTrabajador.eInfoFamiliar_Trabajador objF = ListInfoFamiliar.Find(x => x.cod_parentesco == "PR006");
                                    // txtMontoAsigFamiliarInfoLaboral.ReadOnly = objF != null ? false : true;
                                }

                                Cargarlistado_infolaboral();
                                gvListadoInfoLaboral_FocusedRowChanged(gvListadoInfoLaboral, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));

                                InfoLaboral = 1;

                            }


                            break;
                        case "actelInfoBancaria":
                            navframeTrabajador.SelectedPage = navpageInfoBancaria;
                            if (InfoBancaria == 0)
                            {
                                unit.Trabajador.CargaCombosLookUp("EntidadFinanciera", glkpBancoInfoBancaria, "cod_banco", "dsc_banco", "", valorDefecto: true);
                                //CargarCombosGridLookup("Banco", glkpBancoInfoBancaria, "cod_banco", "dsc_banco", "");
                                unit.Proveedores.CargaCombosLookUp("Moneda", lkpTipoMonedaInfoBancaria, "cod_moneda", "dsc_moneda", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TipoCuenta", lkpTipoCuentaInfoBancaria, "cod_parametro", "dsc_parametro", "");
                                unit.Trabajador.CargaCombosLookUp("tipocuentacts", lkpTipoCuentaCTS, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("EntidadFinanciera", glkpBancoCTSInfoBancaria, "cod_banco", "dsc_banco", "", valorDefecto: true);
                                //CargarCombosGridLookup("Banco", glkpBancoCTSInfoBancaria, "cod_banco", "dsc_banco", "");
                                unit.Proveedores.CargaCombosLookUp("Moneda", lkpTipoMonedaCTSInfoBancaria, "cod_moneda", "dsc_moneda", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("SistPension", lkpSistPensionarioInfoBancaria, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("NombreAFP", lkpNombreAFPInfoBancaria, "cod_APF", "dsc_APF", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("PeriodicidadPago", lkpPeriodicidadPagos, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TipoPago", lkpTipoPago, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TipoComision", lkpTipoComision, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                //lkpTipoPago
                                ObtenerDatos_InfoBancaria();
                                InfoBancaria = 1;
                            }
                            break;
                        case "actelDatosAdicionales":
                            navframeTrabajador.SelectedPage = navpageDatosAdicionales;
                            if (DatosAdicionales == 0)
                            {
                                unit.Trabajador.CargaCombosLookUp("TallasUniforme", lkpPoloTallaUnif, "cod_tallauniforme", "dsc_tallauniforme", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TallasUniforme", lkpCasacaTallaUnif, "cod_tallauniforme", "dsc_tallauniforme", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TallasUniforme", lkpCamisaTallaUnif, "cod_tallauniforme", "dsc_tallauniforme", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TallasUniforme", lkpMamelucoTallaUnif, "cod_tallauniforme", "dsc_tallauniforme", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TallasUniforme", lkpPantalonTallaUnif, "cod_tallauniforme", "dsc_tallauniforme", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TallasUniforme", lkpChalecoTallaUnif, "cod_tallauniforme", "dsc_tallauniforme", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TallasUniforme", lkpCascoTallaUnif, "cod_tallauniforme", "dsc_tallauniforme", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TallasUniforme", lkpFajaTallaUnif, "cod_tallauniforme", "dsc_tallauniforme", "", valorDefecto: true);
                                ObtenerDatos_CaracteristicasTallas();
                                DatosAdicionales = 1;
                            }
                            break;
                        case "actelInfoFamiliar":
                            navframeTrabajador.SelectedPage = navpageInfoFamiliar;
                            if (InfoFamiliar == 0)
                            {
                                unit.Trabajador.CargaCombosLookUp("Parentesco", lkpParentescoInfoFamiliar, "cod_parentesco", "dsc_parentesco", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TipoDocumentoTrabajador", glkpTipoDocumentoInfoFamiliar, "cod_tipo_documento", "dsc_tipo_documento", "", valorDefecto: true);
                                unit.Clientes.CargaCombosLookUp("TipoPais", lkpPaisInfoFamiliar, "cod_pais", "dsc_pais", "");
                                lkpPaisInfoFamiliar.EditValue = "00001";
                                lkpDepartamentoInfoFamiliar.EditValue = "00015";
                                lkpProvinciaInfoFamiliar.EditValue = "00128";


                                //unit.Clientes.CargaCombosLookUp("TipoDepartamento", lkpDepartamentoInfoFamiliar, "cod_departamento", "dsc_departamento", "");
                                //unit.Clientes.CargaCombosLookUp("TipoProvincia", lkpProvinciaInfoFamiliar, "cod_provincia", "dsc_provincia", "");
                                //CargarCombosGridLookup("TipoDistrito", glkpDistritoInfoFamiliar, "cod_distrito", "dsc_distrito", "");
                                //dtFecNacimientoInfoFamiliar.EditValue = DateTime.Today;
                                ObtenerDatos_InfoFamiliar();
                                gvListadoInfoFamiliar_FocusedRowChanged(gvListadoInfoFamiliar, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
                                InfoFamiliar = 1;
                            }
                            break;
                        case "actelInfoEconomica":
                            navframeTrabajador.SelectedPage = navpageInfoEconomica;
                            if (InfoEconomica == 0)
                            {
                                unit.Trabajador.CargaCombosLookUp("TipoPropiedad", lkpViviendaInfoEconomica, "cod_tipopropiedad", "dsc_tipopropiedad", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TipoVivienda", lkpTipoViviendaInfoEconomica, "cod_tipovivienda", "dsc_tipovivienda", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TipoPropiedad", lkpVehiculoInfoEconomica, "cod_tipopropiedad", "dsc_tipopropiedad", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TipoDocumentoEconomico", glkpTipoDocumentoeconomica, "cod_tipo_documento", "dsc_tipo_documento", "", valorDefecto: true);
                                unit.Clientes.CargaCombosLookUp("TipoPais", lkpPaisViviendaInfoEconomica, "cod_pais", "dsc_pais", "");
                                unit.Clientes.CargaCombosLookUp("TipoDepartamento", lkpDepartamentoViviendaInfoEconomica, "cod_departamento", "dsc_departamento", "");
                                unit.Clientes.CargaCombosLookUp("TipoProvincia", lkpProvinciaViviendaInfoEconomica, "cod_provincia", "dsc_provincia", "");
                                CargarCombosGridLookup("TipoDistrito", glkpDistritoViviendaInfoEconomica, "cod_distrito", "dsc_distrito", "");
                                unit.Clientes.CargaCombosLookUp("TipoPais", lkpPaisEmpresaInfoEconomica, "cod_pais", "dsc_pais", "");
                                unit.Clientes.CargaCombosLookUp("TipoDepartamento", lkpDepartamentoEmpresaInfoEconomica, "cod_departamento", "dsc_departamento", "");
                                unit.Clientes.CargaCombosLookUp("TipoProvincia", lkpProvinciaEmpresaInfoEconomica, "cod_provincia", "dsc_provincia", "");
                                CargarCombosGridLookup("TipoDistrito", glkpDistritoEmpresaInfoEconomica, "cod_distrito", "dsc_distrito", "");
                                unit.Proveedores.CargaCombosLookUp("Moneda", lkpTipoMonedaViviendaInfoEconomica, "cod_moneda", "dsc_moneda", "", valorDefecto: true);
                                unit.Proveedores.CargaCombosLookUp("Moneda", lkpTipoMonedaVehiculoInfoEconomica, "cod_moneda", "dsc_moneda", "", valorDefecto: true);
                                ObtenerDatos_InfoEconomica();
                                glkpTipoDocumentoeconomica.EditValue = "DI004";
                                InfoEconomica = 1;

                                lkpPaisEmpresaInfoEconomica.EditValue = "00001";
                                lkpDepartamentoEmpresaInfoEconomica.EditValue = "00015";
                                lkpProvinciaEmpresaInfoEconomica.EditValue = "00128";

                                lkpPaisViviendaInfoEconomica.EditValue = "00001";
                                lkpDepartamentoViviendaInfoEconomica.EditValue = "00015";
                                lkpProvinciaViviendaInfoEconomica.EditValue = "00128";
                            }
                            break;
                        case "actelInfoAcademica":
                            navframeTrabajador.SelectedPage = navpageInfoAcademica;
                            if (InfoAcademica == 0)
                            {
                                unit.Trabajador.CargaCombosLookUp("NivelAcademico", lkpNivelAcademicoFormAcademic, "cod_nivelacademico", "dsc_nivelacademico", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TipoRegimenAcademico", lkpRegimenAcademico, "cod_tiporegimenacademico", "dsc_tiporegimenacademico", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("Pais", lkpPaisAcademico, "cod_pais", "dsc_pais", "", valorDefecto: true);
                                dtAnhoInicioFormAcademic.EditValue = DateTime.Today;
                                dtAnhoFinFormAcademic.EditValue = DateTime.Today;
                                ObtenerDatos_InfoAcademica();
                                gvListadoFormAcademica_FocusedRowChanged(gvListadoFormAcademica, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
                                InfoAcademica = 1;
                            }
                            break;
                        case "actelExpProfesional":
                            navframeTrabajador.SelectedPage = navpageExpProfesional;
                            if (ExpProfesional == 0)
                            {
                                dtFecInicioExpProfesional.EditValue = DateTime.Today;
                                dtFecFinExpProfesional.EditValue = DateTime.Today;
                                ObtenerDatos_InfoProfesional();
                                gvListadoExpProfesional_FocusedRowChanged(gvListadoExpProfesional, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
                                ExpProfesional = 1;
                            }
                            break;
                        case "actelInfoSalud":
                            navframeTrabajador.SelectedPage = navpageInfoSalud;
                            if (InfoSalud == 0)
                            {
                                unit.Trabajador.CargaCombosLookUp("GrupoSanguineo", lkpGrupoSanguineoInfoSalud, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("EstadoSalud", lkpEstadoSaludInfoSalud, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TipoSeguro", lkpSeguroSaludInfoSalud, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("SCRTPension", lkpSCRTpension, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TrabajadorSalud", lkpTipoTrabajadorsalud, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("SituacionTrabajador_salud", lkpSituacionTrabajador_salud, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("SCRTSALUD", lkpSCRTsalud, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("regimensegurosalud", lkpregimensalud, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                                dtFchEvaluacion.EditValue = Convert.ToDateTime(DateTime.Today);
                                dtFchEvaluacionObs.EditValue = Convert.ToDateTime(DateTime.Today);
                                ObtenerDatos_InfoSalud();
                                //  ObtenerDatos_HistorialEMO();
                                InfoSalud = 1;
                            }
                            break;

                        case "actelEMO":
                            navframeTrabajador.SelectedPage = navpageEMO;
                            if (infoemo == 0)
                            {

                                dtFchEvaluacion.EditValue = Convert.ToDateTime(DateTime.Today);
                                dtFchEvaluacionObs.EditValue = Convert.ToDateTime(DateTime.Today);
                                lkpTipoDocumentoingresos.ItemIndex = 0;
                                lkpTipodoc.ItemIndex = 0;
                                ObtenerDatos_EMO();
                                listarDocumentos(107);
                                if (gvDocIngreso.RowCount == 0)
                                {
                                    unit.Trabajador.CargaCombosLookUp("TipoDocumentoTIPO", lkpTipoDocumentoingresos, "cod_documento", "dsc_documento", "", valorDefecto: true, flg_varios: "NO", cod_trabajador: cod_trabajador, cod_empresa: cod_empresa);
                                    lkpTipoDocumentoingresos.ItemIndex = 0;
                                }

                                gvDocIngreso.OptionsSelection.MultiSelect = false;
                                gvEMO.OptionsSelection.MultiSelect = false;

                                infoemo = 1;

                            }
                            break;

                        case "actelInfoVivienda":
                            navframeTrabajador.SelectedPage = navpageInfoVivienda;
                            if (InfoVivienda == 0)
                            {
                                unit.Trabajador.CargaCombosLookUp("TipoPropiedad", lkpViviendaInfoVivienda, "cod_tipopropiedad", "dsc_tipopropiedad", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("TipoComodidad", lkpComodidadInfoVivienda, "cod_comodidad", "dsc_comodidad", "", valorDefecto: true);
                                unit.Trabajador.CargaCombosLookUp("Parentesco", lkpParentescoInfoVivienda, "cod_parentesco", "dsc_parentesco", "", valorDefecto: true);
                                ObtenerDatos_InfoVivienda();
                                //gvEMO_FocusedRowChanged(gvEMO, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
                                InfoVivienda = 1;
                            }
                            break;
                        case "actelDocumentos":
                            navframeTrabajador.SelectedPage = navpageDocumentacion;
                            if (InfoDocumentacion == 0)
                            {
                                //unit.Trabajador.CargaCombosLookUp("TipoPropiedad", lkpViviendaInfoVivienda, "cod_tipopropiedad", "dsc_tipopropiedad", "", valorDefecto: true);
                                //unit.Trabajador.CargaCombosLookUp("TipoComodidad", lkpComodidadInfoVivienda, "cod_comodidad", "dsc_comodidad", "", valorDefecto: true);
                                //unit.Trabajador.CargaCombosLookUp("Parentesco", lkpParentescoInfoVivienda, "cod_parentesco", "dsc_parentesco", "", valorDefecto: true);
                                //ObtenerDatos_InfoVivienda();
                                //gvEMO_FocusedRowChanged(gvEMO, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
                                InfoDocumentacion = 1;
                            }
                            break;
                    }
                    //ObtenerDatosConfiguracion(e.Element.Name.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void acctlMenu_StateChanged(object sender, EventArgs e)
        {
            try
            {
                if (acctlMenu.OptionsMinimizing.State == DevExpress.XtraBars.Navigation.AccordionControlState.Minimized)
                {
                    layoutControlItem1.Size = new Size(45, 559);
                    layoutControlItem62.Size = new Size(45, 559);
                }
                else
                {
                    layoutControlItem1.Size = new Size(182, 596);
                    layoutControlItem62.Size = new Size(182, 596);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void lkpSistPensionario_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpSistPensionarioInfoBancaria.EditValue == null) return;
            if (lkpSistPensionarioInfoBancaria.EditValue.ToString() != "AFP")
            {
                lkpNombreAFPInfoBancaria.EditValue = null; txtNroCUSPPInfoBancaria.Text = "";
                layoutControlItem18.Enabled = false;
                layoutControlItem20.Enabled = false;
            }
            else
            {
                layoutControlItem18.Enabled = true;
                layoutControlItem20.Enabled = true;
            }
        }

        private void glkpTipoDocumento_EditValueChanged(object sender, EventArgs e)
        {
            if (glkpTipoDocumento.EditValue != null)
            {
                eProveedor obj = new eProveedor();
                obj = unit.Proveedores.Validar_NroDocumento<eProveedor>(19, "", glkpTipoDocumento.EditValue.ToString());
                txtNroDocumento.Properties.MaxLength = obj.ctd_digitos;
            }
        }

        private void lkpPais_EditValueChanged(object sender, EventArgs e)
        {
            lkpDistrito.Properties.DataSource = null;
            lkpProvincia.Properties.DataSource = null;
            lkpDepartamento.Properties.DataSource = null;
            unit.Trabajador.CargaCombosLookUp("Departamento", lkpDepartamento, "cod_departamento", "dsc_departamento", "", cod_pais: lkpPais.EditValue == null ? "" : lkpPais.EditValue.ToString());
        }
        private void lkpProvincia_EditValueChanged(object sender, EventArgs e)
        {
            lkpDistrito.Properties.DataSource = null;
            //unit.Clientes.CargaCombosLookUp("TipoDistrito", glkpDistrito, "cod_distrito", "dsc_distrito", "", cod_condicion: lkpProvincia.EditValue.ToString());
            unit.Trabajador.CargaCombosLookUp("Distrito", lkpDistrito, "cod_distrito", "dsc_distrito", "", cod_pais: lkpPais.EditValue == null ? "" : lkpPais.EditValue.ToString(), cod_departamento: lkpDepartamento.EditValue == null ? "" : lkpDepartamento.EditValue.ToString(), cod_provincia: lkpProvincia.EditValue == null ? "" : lkpProvincia.EditValue.ToString());

            if (lkpDistrito.EditValue == null)
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                   txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
                    txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
                    lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
                    , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString());
            }
            else if (lkpDistrito.EditValue != null)

            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                  txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
          txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
          lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
          , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString());

            }


        }
        private void lkpDepartamento_EditValueChanged(object sender, EventArgs e)
        {
            lkpDistrito.Properties.DataSource = null;
            lkpProvincia.Properties.DataSource = null;
            unit.Trabajador.CargaCombosLookUp("Provincia", lkpProvincia, "cod_provincia", "dsc_provincia", "", cod_pais: lkpPais.EditValue == null ? "" : lkpPais.EditValue.ToString(), cod_departamento: lkpDepartamento.EditValue == null ? "" : lkpDepartamento.EditValue.ToString());
            if (lkpDepartamento.EditValue == null)
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                   txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
                    txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
                    lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
                    , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString());
            }
            else if (lkpDepartamento.EditValue != null)

            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                  txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
          txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
          lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
          , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString());

            }
        }



        private void btnNuevoContacto_Click(object sender, EventArgs e)
        {
            txtCodContacto.Text = "0";
            txtApellPaternoContacto.Text = "";
            txtApellMaternoContacto.Text = "";
            txtNombreContacto.Text = "";
            lkpParentescoContacto.EditValue = null;
            glkpTipoDocumentoContacto.EditValue = null;
            txtNroDocumentoContacto.Text = "";
            dtFecNacimientoContacto.EditValue = null;
            txtTelefonoContacto.Text = "";
            txtCelularContacto.Text = "";
            txtApellPaternoContacto.Select();
            sbtnVerDocumentos.Enabled = false;
        }

        private void btnGuardarContacto_Click(object sender, EventArgs e)
        {
            try
            {
                txtApellPaternoContacto.Select();
                if (txtApellPaternoContacto.Text.Trim() == "") { MessageBox.Show("Debe ingresar un apellido paterno.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtApellPaternoContacto.Focus(); return; }
                //if (txtApellMaternoContacto.Text.Trim() == "") { MessageBox.Show("Debe ingresar un apellido materno.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtApellMaternoContacto.Focus(); return; }
                if (txtNombreContacto.Text.Trim() == "") { MessageBox.Show("Debe ingresar un nombre.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNombreContacto.Focus(); return; }
                if (lkpParentescoContacto.EditValue == null) { MessageBox.Show("Debe seleccionar el parentesco.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpParentescoContacto.Focus(); return; }
                //if (glkpTipoDocumentoContacto.EditValue == null) { MessageBox.Show("Debe seleccionar un tipo de documento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); glkpTipoDocumentoContacto.Focus(); return; }
                //if (txtNroDocumentoContacto.Text.Trim() == "") { MessageBox.Show("Debe ingresar un número de documento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNroDocumentoContacto.Focus(); return; }
                //if (dtFecNacimientoContacto.EditValue == null) { MessageBox.Show("Debe ingresar la fecha de nacimiento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtFecNacimientoContacto.Focus(); return; }
                if (txtTelefonoContacto.Text.Trim() == "" && txtCelularContacto.Text.Trim() == "") { MessageBox.Show("Debe ingresar el telefono o celular.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCelularContacto.Focus(); return; }
                if (txtNroDocumentoContacto.Text != "")
                {
                    eProveedor obj = new eProveedor();
                    obj = unit.Proveedores.Validar_NroDocumento<eProveedor>(19, "", glkpTipoDocumentoContacto.EditValue.ToString());
                    int nrodocumento = Convert.ToInt32(txtNroDocumentoContacto.Text.Length);
                    if (nrodocumento < obj.ctd_digitos) { MessageBox.Show("Los digitos del documento debe ser igual a " + obj.ctd_digitos, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNroDocumentoContacto.Focus(); return; }
                    txtNroDocumentoContacto.Properties.MaxLength = obj.ctd_digitos;
                }
                eTrabajador.eContactoEmergencia_Trabajador eContact = new eTrabajador.eContactoEmergencia_Trabajador();
                eContact = AsignarValores_Contatos();
                eContact = unit.Trabajador.InsertarActualizar_ContactoTrabajador<eTrabajador.eContactoEmergencia_Trabajador>(eContact);
                if (eContact == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (eContact != null)
                {
                    ActualizarListado = "SI";
                    MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ObtenerDatos_ContactosEmergencia();
                    gvListadoContactos_FocusedRowChanged(gvListadoContactos, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private eTrabajador.eContactoEmergencia_Trabajador AsignarValores_Contatos()
        {
            eTrabajador.eContactoEmergencia_Trabajador obj = new eTrabajador.eContactoEmergencia_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = lkpEmpresa.EditValue.ToString();
            obj.cod_contactemerg = Convert.ToInt32(txtCodContacto.Text);
            obj.dsc_apellido_paterno = txtApellPaternoContacto.Text.Trim();
            obj.dsc_apellido_materno = txtApellMaternoContacto.Text.Trim();
            obj.dsc_nombres = txtNombreContacto.Text.Trim();
            obj.cod_parentesco = lkpParentescoContacto.EditValue == null ? null : lkpParentescoContacto.EditValue.ToString();
            obj.cod_tipo_documento = glkpTipoDocumentoContacto.EditValue == null ? null : glkpTipoDocumentoContacto.EditValue.ToString(); ;
            obj.dsc_documento = txtNroDocumentoContacto.Text;
            obj.fch_nacimiento = Convert.ToDateTime(dtFecNacimientoContacto.EditValue);
            obj.dsc_telefono = txtTelefonoContacto.Text;
            obj.dsc_celular = txtCelularContacto.Text;
            obj.cod_usuario_registro = cod_usuario;
            //user.cod_usuario;
            obj.flg_activo = "SI";


            return obj;
        }

        private void btnNuevaInfoLaboral_Click(object sender, EventArgs e)
        {


            txtCodInfoLaboral.Text = "0";
            dtFecIngresoInfoLaboral.Enabled = true;
            lkpAreaInfoLaboral.EditValue = null;
            lkpCargoInfoLaboral.EditValue = null;
            lkpPrefCECOInfoLaboral.EditValue = null;
            lkpTipoContratoInfoLaboral.EditValue = null;
            dtFecFirmaInfoLaboral.EditValue = DateTime.Today;
            dtFecVctoInfoLaboral.EditValue = DateTime.Today;
            lkpModalidadInfoLaboral.EditValue = null;
            txtMontoSueldoBaseInfoLaboral.EditValue = 0;
            txtMontoAsigFamiliarInfoLaboral.EditValue = 0;
            txtMontoMovilidadInfoLaboral.EditValue = 0;
            txtMontoAlimentacionInfoLaboral.EditValue = 0;
            txtMontoEscolaridadInfoLaboral.EditValue = 0;
            txtMontoBonoInfoLaboral.EditValue = 0;
            lkpSedeEmpresaInfoLaboral.EditValue = null;
            glkpBancoInfoBancaria.EditValue = null;
            lkpTipoMonedaInfoBancaria.EditValue = null;
            lkpTipoCuentaInfoBancaria.EditValue = null;
            lkpTipoCuentaCTS.EditValue = null;
            txtNroCuentaBancariaInfoBancaria.Text = "";
            txtNroCuentaCCIInfoBancaria.Text = "";
            cbxJornadaMaxima.EditValue = "NO";
            cbxSindicato.EditValue = "NO";
            cbxRegimenAtipico.EditValue = "NO";
            lkpHorarioNocturno.EditValue = "NO";
            lkphorasextras.EditValue = "NO";
            lkpafectosctr.EditValue = "NO";
            lkpAfectovidaLey.EditValue = "NO";
            lkphorasextras.EditValue = "NO";
            lkpexonerado5.EditValue = "NO";
            lkphorasextras.EditValue = "NO";
            lkpAfectovidaLey.EditValue = "NO";
            lkpEmpresaInfoLaboral.Enabled = true;
            lkpTipoContratoInfoLaboral.Enabled = true;
            lkpSedeEmpresaInfoLaboral.Enabled = true;
            lkpAreaInfoLaboral.Enabled = true;
            lkpTipoTrabajador.Enabled = true;
            lkpSituacionTrabajador_2.Enabled = true;
            lkpsituacionespecial.Enabled = true;
            lkpCargoInfoLaboral.Enabled = true;
            lkpPrefCECOInfoLaboral.Enabled = true;
            lkpTipoTrabajador.EditValue = "";
            lkpsituacionespecial.EditValue = "";
            lkpOcupacional.EditValue = "";
            lkpCategoria.EditValue = "";
            chkasignacionFamiliar.Checked = false;
            motivobaja = "";
            fechabaja = null;
            observacionbaja = "";
            chckAdjuntarContrato.Checked = false;

        }

        private void btnGuardarInfoLaboral_Click(object sender, EventArgs e)
        {
            string ingreso = Convert.ToString(dtFecIngresoInfoLaboral.EditValue.ToString());
            string fincontrato = Convert.ToString(dtFecVctoInfoLaboral.EditValue == null ? null : dtFecVctoInfoLaboral.EditValue.ToString());
            try
            {
                dtFecIngresoInfoLaboral.Select();
                if (dtFecIngresoInfoLaboral.EditValue == null) { MessageBox.Show("Debe ingresar la fecha de ingreso.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtFecIngresoInfoLaboral.Focus(); return; }
                if (lkpSedeEmpresaInfoLaboral.EditValue == null) { MessageBox.Show("Debe seleccionar una sede de la empresa.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpSedeEmpresaInfoLaboral.Focus(); return; }
                if (lkpAreaInfoLaboral.EditValue == null) { MessageBox.Show("Debe seleccionar el área.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpAreaInfoLaboral.Focus(); return; }
                if (lkpCargoInfoLaboral.EditValue == null) { MessageBox.Show("Debe seleccionar el cargo.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpCargoInfoLaboral.Focus(); return; }
                if (lkpTipoTrabajador.EditValue == null) { MessageBox.Show("Debe seleccionar un Tipo de Trabajador.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipoTrabajador.Focus(); return; }
                if (lkpSituacionTrabajador_2.EditValue == null) { MessageBox.Show("Debe seleccionar la Situación del Trabajador.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpSituacionTrabajador_2.Focus(); return; }
                if (lkpsituacionespecial.EditValue == null) { MessageBox.Show("Debe seleccionar la situacion Especial.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpsituacionespecial.Focus(); return; }
                if (lkpRegimenLaboral.EditValue == null) { MessageBox.Show("Debe seleccionar un Regimen Laboral.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpRegimenLaboral.Focus(); return; }
                if (lkpOcupacional.EditValue == null) { MessageBox.Show("Debe seleccionar Cargo Ocupacional.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpOcupacional.Focus(); return; }
                if (lkpCategoria.EditValue == null) { MessageBox.Show("Debe seleccionar una Categoria Trabajador.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpCategoria.Focus(); return; }
                if (lkpTipoContratoInfoLaboral.EditValue == null) { MessageBox.Show("Debe seleccionar el tipo de contrato.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipoContratoInfoLaboral.Focus(); return; }
                if (lkpCentroRiesgo.EditValue == null) { MessageBox.Show("Debe seleccionar Centro de Riesgo.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpCentroRiesgo.Focus(); return; }
              //  if (txtcorreolaboral.EditValue == null) { MessageBox.Show("Debe ingresar un correo laboral.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtcorreolaboral.Focus(); return; }
                if (lkpCIAsegurovidaley.EditValue == null) { MessageBox.Show("Debe seleccion un CIA seguro vida Ley.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpCIAsegurovidaley.Focus(); return; }

                if (lkpPrefCECOInfoLaboral.EditValue == null) { MessageBox.Show("Debe seleccionar el prefijo CECO.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpPrefCECOInfoLaboral.Focus(); return; }
                if (dtFecFirmaInfoLaboral.EditValue == null) { MessageBox.Show("Debe ingresar la fecha de Inicio de contrato.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtFecFirmaInfoLaboral.Focus(); return; }
                if (Convert.ToDecimal(txtMontoSueldoBaseInfoLaboral.EditValue) == 0) { MessageBox.Show("Debe ingresar el sueldo base.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtMontoSueldoBaseInfoLaboral.Focus(); return; }
                if (cbxRegimenAtipico.EditValue == null)  { MessageBox.Show("Debe ingresar el Regimen Atipico.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); cbxRegimenAtipico.Focus(); return; }
                if (cbxJornadaMaxima.EditValue == null) { MessageBox.Show("Debe ingresar la Jornada Maxima.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); cbxJornadaMaxima.Focus(); return; }
                if (cbxSindicato.EditValue == null) { MessageBox.Show("Debe ingresar si pertenece a un Sindicato.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); cbxSindicato.Focus(); return; }
                if (lkpexonerado5.EditValue == null) { MessageBox.Show("Debe ingresar si pertenece a Exonerado Renta de 5ta.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpexonerado5.Focus(); return; }

                //        if (ingreso == fincontrato) { MessageBox.Show("La fecha de ingreso no puede ser igual a la fecha fin de contrato.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtFecVctoInfoLaboral.Focus(); return; }


                eTrabajador.eInfoLaboral_Trabajador eInfoLab = new eTrabajador.eInfoLaboral_Trabajador();
                eInfoLab = AsignarValores_InfoLaboral();
                eInfoLab = unit.Trabajador.InsertarActualizar_InfoLaboralTrabajador<eTrabajador.eInfoLaboral_Trabajador>(eInfoLab);
                //eTrabajador.eDetalleTrabajador eInfoLab = new eTrabajador.eDetalleTrabajador();
                //eInfoLab = AsignarValores_InfoLaboral_detalle();
                //eInfoLab = unit.Trabajado.InsertarActualizar_InfoLaboralTrabajador_Detalle<eTrabajador.eDetalleTrabajador>(eInfoLab);
                if (eInfoLab == null) { MessageBox.Show("Error al registrar documento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (eInfoLab != null)
                {
                    ActualizarListado = "SI";
                    MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Cargarlistado_infolaboral();

                    gvListadoInfoLaboral_FocusedRowChanged(gvListadoInfoLaboral, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private eTrabajador.eInfoLaboral_Trabajador AsignarValores_InfoLaboral()
        {
            eTrabajador.eInfoLaboral_Trabajador obj = new eTrabajador.eInfoLaboral_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = lkpEmpresaInfoLaboral.EditValue.ToString();
            obj.cod_infolab = Convert.ToInt32(txtCodInfoLaboral.Text);
            obj.fch_ingreso = Convert.ToDateTime(dtFecIngresoInfoLaboral.EditValue);
            obj.cod_area = lkpAreaInfoLaboral.EditValue == null ? null : lkpAreaInfoLaboral.EditValue.ToString();
            obj.cod_cargo = lkpCargoInfoLaboral.EditValue == null ? null : lkpCargoInfoLaboral.EditValue.ToString();
            obj.dsc_pref_ceco = lkpPrefCECOInfoLaboral == null ? null : lkpPrefCECOInfoLaboral.EditValue.ToString();
            obj.cod_tipo_contrato = lkpTipoContratoInfoLaboral == null ? null : lkpTipoContratoInfoLaboral.EditValue.ToString();
            obj.fch_firma = Convert.ToDateTime(dtFecFirmaInfoLaboral.EditValue);
            obj.fch_vencimiento = Convert.ToDateTime(dtFecVctoInfoLaboral.EditValue);
            obj.cod_modalidad = lkpModalidadInfoLaboral.EditValue == null ? null : lkpModalidadInfoLaboral.EditValue.ToString();
            obj.imp_sueldo_base = Convert.ToDecimal(txtMontoSueldoBaseInfoLaboral.Text);
            obj.imp_asig_familiar = Convert.ToDecimal(txtMontoAsigFamiliarInfoLaboral.Text);
            obj.imp_movilidad = Convert.ToDecimal(txtMontoMovilidadInfoLaboral.Text);
            obj.imp_alimentacion = Convert.ToDecimal(txtMontoAlimentacionInfoLaboral.Text);
            obj.imp_escolaridad = Convert.ToDecimal(txtMontoEscolaridadInfoLaboral.Text);
            obj.imp_bono = Convert.ToDecimal(txtMontoBonoInfoLaboral.Text);
            obj.cod_sede_empresa = lkpSedeEmpresaInfoLaboral.EditValue.ToString();
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            obj.flg_asignacionfamiliar = chkasignacionFamiliar.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_Regimen_pension = chkflgRegimenPension1.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_regimen_atipico = cbxRegimenAtipico.EditValue == null ? "" : cbxRegimenAtipico.EditValue.ToString();
            obj.flg_jornada_maxima = cbxJornadaMaxima.EditValue == null ? "" : cbxJornadaMaxima.EditValue.ToString();
            obj.flg_horario_nocturno = lkpHorarioNocturno.EditValue == null ? "" : lkpHorarioNocturno.EditValue.ToString();
            obj.codsunat_scrtcentroriesgo = lkpCentroRiesgo.EditValue == null ? "" : lkpCentroRiesgo.EditValue.ToString();
            obj.flg_afectoSCTR = lkpafectosctr.EditValue == null ? "" : lkpafectosctr.EditValue.ToString();
            obj.flg_AfectoVidaLey = lkpAfectovidaLey.EditValue == null ? "" : lkpAfectovidaLey.EditValue.ToString();
            obj.codsunat_seguroley = lkpCIAsegurovidaley.EditValue == null ? "" : lkpCIAsegurovidaley.EditValue.ToString();
            obj.flg_horas_extras = lkphorasextras.EditValue == null ? "" : lkphorasextras.EditValue.ToString();
            obj.flg_sindicato = cbxSindicato.EditValue == null ? "" : cbxSindicato.EditValue.ToString();
            obj.cod_tipo_trabajador = lkpTipoTrabajador.EditValue == null ? null : lkpTipoTrabajador.EditValue.ToString();
            obj.cod_categoria_trabajador = lkpCategoria.EditValue == null ? null : lkpCategoria.EditValue.ToString();
            obj.dsc_calificacion_puesto = lkpsituacionespecial.EditValue == null ? null : lkpsituacionespecial.EditValue.ToString();
            obj.cod_situacion_trabajador_2 = lkpSituacionTrabajador_2.EditValue == null ? null : lkpSituacionTrabajador_2.EditValue.ToString();
            obj.cod_exoneracion_5ta = lkpexonerado5.EditValue == null ? null : lkpexonerado5.EditValue.ToString();
            obj.cod_ocupacional = lkpOcupacional.EditValue == null ? null : lkpOcupacional.EditValue.ToString();
            obj.cod_conveniotributacion = lkpConvenioTributacion.EditValue == null ? null : lkpConvenioTributacion.EditValue.ToString();
            obj.cod_situacion_especial = lkpsituacionespecial.EditValue == null ? null : lkpsituacionespecial.EditValue.ToString();
            obj.Fechabaja = Convert.ToDateTime(fechabaja);
            obj.motivo_baja = motivobaja;
            obj.observaciones = observacionbaja;
            obj.cod_regimen_laboral = lkpRegimenLaboral.EditValue == null ? null : lkpRegimenLaboral.EditValue.ToString();
            obj.dsc_porcentajecomision = txtprctcomision.Text == null ? null : txtprctcomision.Text;
            obj.dsc_porcentajequincena = txtprctquincena.Text == null ? null : txtprctquincena.Text;
            obj.correo_laboral = txtcorreolaboral.Text == null ? null : txtcorreolaboral.Text;
            obj.cod_ocupaciones= lkpOcpaciones.EditValue == null ? null : lkpOcpaciones.EditValue.ToString();
            obj.flg_activo = "SI";


            return obj;
        }

        private eTrabajador.eDetalleTrabajador AsignarValores_InfoLaboral_detalle()
        {
            eTrabajador.eDetalleTrabajador obj = new eTrabajador.eDetalleTrabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = lkpEmpresa.EditValue.ToString();
            obj.cod_sede_empresa = lkpSedeEmpresaInfoLaboral.EditValue.ToString();
            obj.cod_infolab = Convert.ToInt32(txtCodInfoLaboral.Text);
            obj.fch_ingreso = Convert.ToDateTime(dtFecIngresoInfoLaboral.EditValue);
            obj.cod_area = lkpAreaInfoLaboral.EditValue.ToString();
            obj.cod_cargo = lkpCargoInfoLaboral.EditValue.ToString();
            obj.fch_firma = Convert.ToDateTime(dtFecFirmaInfoLaboral.EditValue);
            obj.fch_vencimiento = Convert.ToDateTime(dtFecVctoInfoLaboral.EditValue);
            obj.imp_sueldo_base = Convert.ToDecimal(txtMontoSueldoBaseInfoLaboral.Text);
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            obj.cod_usuario_cambio = Program.Sesion.Usuario.cod_usuario;

            return obj;
        }



        private void gvListadoContactos_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0) gvListadoContactos_FocusedRowChanged(gvListadoContactos, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, e.RowHandle));
        }

        private void gvListadoContactos_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvListadoContactos_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvListadoContactos_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0) Obtener_ContactoEmergencia();
        }

        private void Obtener_ContactoEmergencia()
        {
            eTrabajador.eContactoEmergencia_Trabajador obj = new eTrabajador.eContactoEmergencia_Trabajador();
            obj = gvListadoContactos.GetFocusedRow() as eTrabajador.eContactoEmergencia_Trabajador;
            if (obj == null) return;
            txtCodContacto.Text = obj.cod_contactemerg.ToString();
            txtApellPaternoContacto.Text = obj.dsc_apellido_paterno;
            txtApellMaternoContacto.Text = obj.dsc_apellido_materno;
            txtNombreContacto.Text = obj.dsc_nombres;
            lkpParentescoContacto.EditValue = obj.cod_parentesco;
            glkpTipoDocumentoContacto.EditValue = obj.cod_tipo_documento;
            txtNroDocumentoContacto.Text = obj.dsc_documento;
            dtFecNacimientoContacto.EditValue = obj.fch_nacimiento;
            if (obj.fch_nacimiento.ToString().Contains("0001")) { dtFecNacimientoContacto.EditValue = null; }
            txtTelefonoContacto.Text = obj.dsc_telefono;
            txtCelularContacto.Text = obj.dsc_celular;
        }

        private void gvListadoInfoLaboral_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0) gvListadoInfoLaboral_FocusedRowChanged(gvListadoInfoLaboral, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, e.RowHandle));
            Obtener_InfoLaboral();

        }

        private void gvListadoInfoLaboral_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvListadoInfoLaboral_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvListadoInfoLaboral_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                Obtener_InfoLaboral();
            }


        }

        private void Obtener_InfoLaboral()
        {
            eTrabajador.eInfoLaboral_Trabajador obj = new eTrabajador.eInfoLaboral_Trabajador();
            obj = gvListadoInfoLaboral.GetFocusedRow() as eTrabajador.eInfoLaboral_Trabajador;
            lkpEmpresaInfoLaboral.Enabled = true;
            lkpSedeEmpresaInfoLaboral.Enabled = true;
            lkpAreaInfoLaboral.Enabled = true;
            lkpTipoTrabajador.Enabled = true;
            lkpSituacionTrabajador_2.Enabled = true;
            lkpsituacionespecial.Enabled = true;
            lkpCargoInfoLaboral.Enabled = true;
            lkpPrefCECOInfoLaboral.Enabled = true;
            lkpTipoContratoInfoLaboral.Enabled = true;

            chckAdjuntarContrato.Checked = false;
            chkasignacionFamiliar.Checked = false;
            chkflgRegimenPension1.Checked = false;
            //obj = unit.Trabajador.Obtener_Trabajador<eTrabajador.eInfoLaboral_Trabajador>(4, cod_trabajador, lkpEmpresa.EditValue.ToString());
            if (obj == null) { return; chckAdjuntarContrato.Enabled = false; return; }
            txtCodInfoLaboral.Text = obj.cod_infolab.ToString();
            dtFecIngresoInfoLaboral.EditValue = obj.fch_ingreso;
            lkpEmpresa.EditValue = obj.cod_empresa;
            lkpEmpresaInfoLaboral.EditValue = obj.cod_empresa;
            lkpSedeEmpresaInfoLaboral.EditValue = obj.cod_sede_empresa;
            lkpAreaInfoLaboral.EditValue = obj.cod_area;
            lkpCargoInfoLaboral.EditValue = obj.cod_cargo;
            lkpPrefCECOInfoLaboral.EditValue = obj.dsc_pref_ceco;
            lkpPrefCECOInfoLaboral.ToolTip = lkpPrefCECOInfoLaboral.Text;
            lkpTipoContratoInfoLaboral.EditValue = obj.cod_tipo_contrato;
            dtFecFirmaInfoLaboral.EditValue = obj.fch_firma;
            dtFecVctoInfoLaboral.EditValue = obj.fch_vencimiento;
            lkpModalidadInfoLaboral.EditValue = obj.cod_modalidad;
            txtMontoSueldoBaseInfoLaboral.EditValue = obj.imp_sueldo_base;
            txtMontoAsigFamiliarInfoLaboral.EditValue = obj.imp_asig_familiar;
            txtMontoMovilidadInfoLaboral.EditValue = obj.imp_movilidad;
            txtMontoAlimentacionInfoLaboral.EditValue = obj.imp_alimentacion;
            txtMontoEscolaridadInfoLaboral.EditValue = obj.imp_escolaridad;
            txtMontoBonoInfoLaboral.EditValue = obj.imp_bono;
            lkpexonerado5.EditValue = obj.cod_exoneracion_5ta;
            cbxJornadaMaxima.EditValue = obj.flg_jornada_maxima;
            cbxSindicato.EditValue = obj.flg_sindicato;
            cbxRegimenAtipico.EditValue = obj.flg_regimen_atipico;
            lkpHorarioNocturno.EditValue = obj.flg_horario_nocturno;
            lkpsituacionespecial.EditValue = obj.cod_situacion_especial;
            lkpOcupacional.EditValue = obj.cod_ocupacional;
            lkpTipoTrabajador.EditValue = obj.cod_tipo_trabajador;
            lkpCategoria.EditValue = obj.cod_categoria_trabajador;
            lkpSituacionTrabajador_2.EditValue = obj.cod_situacion_trabajador_2;
            lkpConvenioTributacion.EditValue = obj.cod_conveniotributacion;
            lkphorasextras.EditValue = obj.flg_horas_extras;
            lkpAfectovidaLey.EditValue = obj.flg_AfectoVidaLey;
            lkpafectosctr.EditValue = obj.flg_afectoSCTR;
            lkpCIAsegurovidaley.EditValue = obj.codsunat_seguroley;
            lkpCentroRiesgo.EditValue = obj.codsunat_scrtcentroriesgo;
            chkflgRegimenPension1.CheckState = obj.flg_Regimen_pension == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkasignacionFamiliar.CheckState = obj.flg_asignacionfamiliar == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chckAdjuntarContrato.CheckState = obj.flg_contrato == "SI" ? CheckState.Checked : CheckState.Unchecked;
            txtcorreolaboral.Text = obj.correo_laboral;
            lkpRegimenLaboral.EditValue = obj.cod_regimen_laboral;
            txtprctcomision.Text = obj.dsc_porcentajecomision;
            txtprctquincena.Text = obj.dsc_porcentajequincena;
            if (obj.cod_ocupaciones == null) { lkpOcpaciones.EditValue = "999001"; }
            else { lkpOcpaciones.EditValue = obj.cod_ocupaciones; }


            //    if (obj.motivo_baja !=null) { btnGuardarInfoLaboral.Enabled = false; }
            //else { btnGuardarInfoLaboral.Enabled = true; }


        }


        private void gvListadoInfoFamiliar_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvListadoInfoFamiliar_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvListadoInfoFamiliar_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0) gvListadoInfoFamiliar_FocusedRowChanged(gvListadoInfoFamiliar, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, e.RowHandle));
        }

        private void gvListadoInfoFamiliar_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0) Obtener_InfoFamiliar();

        }

        private void Obtener_InfoFamiliar()
        {
            eTrabajador.eInfoFamiliar_Trabajador obj = new eTrabajador.eInfoFamiliar_Trabajador();
            obj = gvListadoInfoFamiliar.GetFocusedRow() as eTrabajador.eInfoFamiliar_Trabajador;
            if (obj == null) return;
            txtCodInfoFamiliar.Text = obj.cod_infofamiliar.ToString();
            lkpParentescoInfoFamiliar.EditValue = obj.cod_parentesco;
            txtApellPaternoInfoFamiliar.Text = obj.dsc_apellido_paterno;
            txtApellMaternoInfoFamiliar.Text = obj.dsc_apellido_materno;
            txtNombreInfoFamiliar.Text = obj.dsc_nombres;
            dtFecNacimientoInfoFamiliar.EditValue = obj.fch_nacimiento;
            if (obj.fch_nacimiento.ToString().Contains("0001")) { dtFecNacimientoInfoFamiliar.EditValue = null; }
            chkflgVivoInfoFamiliar.CheckState = obj.flg_vivo == "SI" ? CheckState.Checked : CheckState.Unchecked;
            glkpTipoDocumentoInfoFamiliar.EditValue = obj.cod_tipo_documento;
            txtNroDocumentoInfoFamiliar.Text = obj.dsc_documento;
            txtEmailInfoFamiliar.Text = obj.dsc_mail;
            txtTelefonoInfoFamiliar.Text = obj.dsc_telefono;
            txtCelularInfoFamiliar.Text = obj.dsc_celular;
            txtProfesionInfoFamiliar.Text = obj.dsc_profesion;
            txtCentroLaboralInfoFamiliar.Text = obj.dsc_centrolaboral;
            txtGradoInstruccionInfoFamiliar.Text = obj.dsc_gradoinstruccion;
            txtOcupacionInfoFamiliar.Text = obj.dsc_ocupacion;
            txtDiscapacidadInfoFamiliar.Text = obj.dsc_discapacidad;
            txtDireccionInfoFamiliar.Text = obj.dsc_direccion;
            lkpPaisInfoFamiliar.EditValue = obj.cod_pais;
            lkpDepartamentoInfoFamiliar.EditValue = obj.cod_departamento;
            lkpProvinciaInfoFamiliar.EditValue = obj.cod_provincia;
            glkpDistritoInfoFamiliar.EditValue = obj.cod_distrito;
            chkflgEnfermedadInfoFamiliar.CheckState = obj.flg_enfermedad == "SI" ? CheckState.Checked : CheckState.Unchecked;
            txtEnfermedadInfoFamiliar.Text = obj.dsc_enfermedad;
            chkflgAdiccionInfoFamiliar.CheckState = obj.flg_adiccion == "SI" ? CheckState.Checked : CheckState.Unchecked;
            txtAdiccionInfoFamiliar.Text = obj.dsc_adiccion;
            chkflgDependeEconomiaInfoFamiliar.CheckState = obj.flg_dependenciaeconomica == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkDNIFAMILIAR.CheckState = obj.flg_DNI_documentofam == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkCERTIFICADOESTUDIOS.CheckState = obj.flg_CERTIFICADOESTUDIOS_documentofam == "SI" ? CheckState.Checked : CheckState.Unchecked;
            if (obj.dsc_direccion != "" || obj.dsc_direccion != null) { chckdireccionfamiliar.Checked = true; } else { chckdireccionfamiliar.Checked = false; txtDireccion.Enabled = false; }
        }

        private void gvListadoFormAcademica_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvListadoFormAcademica_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvListadoFormAcademica_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0) gvListadoFormAcademica_FocusedRowChanged(gvListadoFormAcademica, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, e.RowHandle));
        }

        private void gvListadoFormAcademica_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0) Obtener_FormAcademica();
        }

        private void Obtener_FormAcademica()
        {
            eTrabajador.eInfoAcademica_Trabajador obj = new eTrabajador.eInfoAcademica_Trabajador();
            obj = unit.Trabajador.Obtener_Trabajador_academico<eTrabajador.eInfoAcademica_Trabajador>(9, cod_trabajador: cod_trabajador, cod_empresa: cod_empresa);
            if (obj == null) { hlinkAdjuntarCertificadoInfoAcademica.Enabled = false; return; }
            txtCodFormAcademic.Text = obj.cod_infoacademica.ToString();
            lkpRegimenAcademico.EditValue = obj.cod_tiporegimenacademico;
            lkpTipoInstitucion.EditValue = obj.cod_tipoinstitucion;
            lkpCentroEstudiosFormAcademic.EditValue = obj.cod_centroestudios;
            lkpNivelAcademicoFormAcademic.EditValue = obj.cod_nivelacademico;
            txtLugarFormAcademic.Text = obj.dsc_lugar;
            lkpCarreraCursoFormAcademic.EditValue = obj.cod_carrera_profesional;
            txtGradoTituloFormAcademic.Text = obj.dsc_grado;
            dtAnhoInicioFormAcademic.EditValue = new DateTime(obj.anho_inicio, 01, 01);
            dtAnhoFinFormAcademic.EditValue = new DateTime(obj.anho_fin, 01, 01);
            txtPromedioAcademicoFormAcademic.Text = obj.imp_promedio.ToString();
            lkpPaisAcademico.EditValue = obj.cod_pais.ToString();
            hlinkAdjuntarCertificadoInfoAcademica.Enabled = true;
            chkflgEducComplPeru.CheckState = obj.flg_EducComplPeru == "SI" ? CheckState.Checked : CheckState.Unchecked;

        }




        private void gvListadoExpProfesional_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvListadoExpProfesional_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvListadoExpProfesional_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0) gvListadoExpProfesional_FocusedRowChanged(gvListadoExpProfesional, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, e.RowHandle));
        }

        private void gvListadoExpProfesional_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0) Obtener_ExpProfesional();
        }

        private void Obtener_ExpProfesional()
        {
            eTrabajador.eInfoProfesional_Trabajador obj = new eTrabajador.eInfoProfesional_Trabajador();
            obj = gvListadoExpProfesional.GetFocusedRow() as eTrabajador.eInfoProfesional_Trabajador;
            if (obj == null) { hlinkAdjuntarCertificadoInfoProfesional.Enabled = false; return; }
            txtCodExpProfesional.Text = obj.cod_infoprofesional.ToString();
            txtJefeInmediatoExpProfesional.Text = obj.dsc_nombre_jefe;
            txtRazonSocialExpProfesional.Text = obj.dsc_razon_social;
            txtCargoJefeExpProfesional.Text = obj.dsc_cargo_jefe;
            txtCargoExpProfesional.Text = obj.dsc_cargo;
            txtMotivoSalidaExpProfesional.Text = obj.dsc_motivo_salida;
            dtFecInicioExpProfesional.EditValue = obj.fch_inicio;
            if (obj.fch_inicio.ToString().Contains("0001")) { dtFecInicioExpProfesional.EditValue = null; }
            dtFecFinExpProfesional.EditValue = obj.fch_fin;
            if (obj.fch_fin.ToString().Contains("0001")) { dtFecFinExpProfesional.EditValue = null; }
            txtCelularExpProfesional.Text = obj.dsc_celular;
            mmComentariosExpProfesional.Text = obj.dsc_comentarios;
            hlinkAdjuntarCertificadoInfoProfesional.Enabled = true;
        }

        private void btnGuardarTallaUnif_Click(object sender, EventArgs e)
        {
            try
            {
                txtEstaturaCaractFisica.Select();
                if (Convert.ToDecimal(txtEstaturaCaractFisica.EditValue) == 0) { MessageBox.Show("Debe ingresar la estatura.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtEstaturaCaractFisica.Focus(); return; }
                if (Convert.ToDecimal(txtPesoCaractFisica.EditValue) == 0) { MessageBox.Show("Debe ingresar el peso.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPesoCaractFisica.Focus(); return; }
                eTrabajador.eCaractFisicas_Trabajador eCaractF = new eTrabajador.eCaractFisicas_Trabajador();
                eCaractF = AsignarValores_CaractFisicas();
                eCaractF = unit.Trabajador.InsertarActualizar_CaractFisicasTrabajador<eTrabajador.eCaractFisicas_Trabajador>(eCaractF);
                if (eCaractF == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                eTrabajador.eTallaUniforme_Trabajador eTallaU = new eTrabajador.eTallaUniforme_Trabajador();
                eTallaU = AsignarValores_TallaUniforme();
                eTallaU = unit.Trabajador.InsertarActualizar_TallaUniformesTrabajador<eTrabajador.eTallaUniforme_Trabajador>(eTallaU);
                if (eTallaU == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (eCaractF != null && eTallaU != null)
                {
                    ActualizarListado = "SI";
                    MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Cargarlistado_infolaboral();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private eTrabajador.eCaractFisicas_Trabajador AsignarValores_CaractFisicas()
        {
            eTrabajador.eCaractFisicas_Trabajador obj = new eTrabajador.eCaractFisicas_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = lkpEmpresa.EditValue.ToString();
            obj.dsc_estatura = Convert.ToDecimal(txtEstaturaCaractFisica.Text);
            obj.dsc_peso = Convert.ToDecimal(txtPesoCaractFisica.Text);
            obj.dsc_IMC = Convert.ToDecimal(txtIMCCaractFisica.Text);
            obj.flg_lentes = chkflgLentesCaractFisica.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;

            return obj;
        }

        private eTrabajador.eTallaUniforme_Trabajador AsignarValores_TallaUniforme()
        {
            eTrabajador.eTallaUniforme_Trabajador obj = new eTrabajador.eTallaUniforme_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = lkpEmpresa.EditValue.ToString();
            obj.cod_talla_polo = lkpPoloTallaUnif.EditValue == null ? null : lkpPoloTallaUnif.EditValue.ToString();
            obj.cod_talla_camisa = lkpCamisaTallaUnif.EditValue == null ? null : lkpCamisaTallaUnif.EditValue.ToString();
            obj.cod_talla_pantalon = lkpPantalonTallaUnif.EditValue == null ? null : lkpPantalonTallaUnif.EditValue.ToString();
            obj.cod_talla_casaca = lkpCasacaTallaUnif.EditValue == null ? null : lkpCasacaTallaUnif.EditValue.ToString();
            obj.cod_talla_mameluco = lkpMamelucoTallaUnif.EditValue == null ? null : lkpMamelucoTallaUnif.EditValue.ToString();
            obj.cod_talla_chaleco = lkpChalecoTallaUnif.EditValue == null ? null : lkpChalecoTallaUnif.EditValue.ToString();
            obj.cod_talla_calzado = txtCalzadoTallaUnif.EditValue.ToString();
            obj.cod_talla_casco = lkpCasacaTallaUnif.EditValue == null ? null : lkpCasacaTallaUnif.EditValue.ToString();
            obj.cod_talla_faja = lkpMamelucoTallaUnif.EditValue == null ? null : lkpMamelucoTallaUnif.EditValue.ToString();
            obj.dsc_casillero = Convert.ToInt32(txtCasilleroTallaUnif.Text);
            obj.flg_lentes = chkflgLentesTallaUnif.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;

            return obj;
        }

        private void btnNuevaInfoEconomica_Click(object sender, EventArgs e)
        {
            nuevoinfoeconomica();
        }
        private void nuevoinfoeconomica()
        {
            txtCodInfoEconomica.Text = "0";
            lkpViviendaInfoEconomica.EditValue = null;
            lkpTipoViviendaInfoEconomica.EditValue = null;
            txtValorRentaInfoEconomica.EditValue = 0;
            lkpTipoMonedaViviendaInfoEconomica.EditValue = "SOL";
            txtDireccionViviendaInfoEconomica.Text = "";
            txtReferenciaViviendaInfoEconomica.Text = "";
            lkpPaisEmpresaInfoEconomica.EditValue = "00001";
            lkpDepartamentoEmpresaInfoEconomica.EditValue = "00015";
            lkpProvinciaEmpresaInfoEconomica.EditValue = "00128";

            lkpPaisViviendaInfoEconomica.EditValue = "00001";
            lkpDepartamentoViviendaInfoEconomica.EditValue = "00015";
            lkpProvinciaViviendaInfoEconomica.EditValue = "00128";
            glkpDistritoViviendaInfoEconomica.EditValue = null;
            lkpVehiculoInfoEconomica.EditValue = null;
            txtMarcaInfoEconomica.Text = "";
            txtModeloInfoEconomica.Text = "";
            txtPlacaVehiculoInfoEconomica.Text = "";
            txtValorComercialInfoEconomica.EditValue = 0;
            lkpTipoMonedaVehiculoInfoEconomica.EditValue = "SOL";
            glkpTipoDocumentoeconomica.EditValue = "DI004";
            txtParticipacionInfoEconomica.Text = "";
            txtRUCEmpresaInfoEconomica.Text = "";
            txtGiroEmpresaInfoEconomica.Text = "";
            txtDireccionEmpresaInfoEconomica.Text = "";
            txtEmpresaInfoEconomica.Text = "";
            txtEmpresaInfoEconomica.Enabled = true;
            txtDireccionEmpresaInfoEconomica.Enabled = true;

            glkpDistritoEmpresaInfoEconomica.EditValue = null;
        }

        private void btnGuardarInfoEconomica_Click(object sender, EventArgs e)
        {
            try
            {
                txtIngresoMensualInfoEconomica.Select();
                eTrabajador.eInfoEconomica_Trabajador eInfoEco = new eTrabajador.eInfoEconomica_Trabajador();

                if (txtRUCEmpresaInfoEconomica.Text != "")
                {
                    eProveedor objw = new eProveedor();
                    objw = unit.Proveedores.Validar_NroDocumento<eProveedor>(19, "", glkpTipoDocumentoeconomica.EditValue.ToString());
                    int nrodocumento = Convert.ToInt32(txtRUCEmpresaInfoEconomica.Text.Length);
                    if (nrodocumento < objw.ctd_digitos) { MessageBox.Show("Los digitos del documento debe ser igual a " + objw.ctd_digitos, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtRUCEmpresaInfoEconomica.Focus(); return; }
                    txtRUCEmpresaInfoEconomica.Properties.MaxLength = objw.ctd_digitos;
                }
                eInfoEco = AsignarValores_InfoEconomica();
                eInfoEco = unit.Trabajador.InsertarActualizar_InfoEconomicaTrabajador<eTrabajador.eInfoEconomica_Trabajador>(eInfoEco);
                if (eInfoEco == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                if (eInfoEco != null)
                {
                    MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ObtenerDatos_InfoEconomica();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private eTrabajador.eInfoEconomica_Trabajador AsignarValores_InfoEconomica()
        {
            eTrabajador.eInfoEconomica_Trabajador obj = new eTrabajador.eInfoEconomica_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = lkpEmpresa.EditValue.ToString();
            obj.cod_infoeconomica = Convert.ToInt32(txtCodInfoEconomica.Text);
            obj.imp_ingresomensual = Convert.ToDecimal(txtIngresoMensualInfoEconomica.EditValue);
            obj.imp_gastomensual = Convert.ToDecimal(txtGastoMensualInfoEconomica.EditValue);
            obj.imp_totalactivos = Convert.ToDecimal(txtValorActivoInfoEconomica.EditValue);
            obj.imp_totaldeudas = Convert.ToDecimal(txtValorDeudaInfoEconomica.EditValue);
            obj.cod_vivienda = lkpViviendaInfoEconomica.EditValue == null ? null : lkpViviendaInfoEconomica.EditValue.ToString();
            obj.cod_tipovivienda = lkpTipoViviendaInfoEconomica.EditValue == null ? null : lkpTipoViviendaInfoEconomica.EditValue.ToString();
            obj.imp_valorvivienda = txtValorRentaInfoEconomica.EditValue.ToString();
            obj.cod_monedavivienda = lkpTipoMonedaViviendaInfoEconomica.EditValue.ToString();
            obj.dsc_direccion_vivienda = txtDireccionViviendaInfoEconomica.Text;
            obj.dsc_referencia_vivienda = txtReferenciaViviendaInfoEconomica.Text;
            obj.cod_pais_vivienda = lkpPaisViviendaInfoEconomica.EditValue == null ? null : lkpPaisViviendaInfoEconomica.EditValue.ToString();
            obj.cod_departamento_vivienda = lkpDepartamentoViviendaInfoEconomica.EditValue == null ? null : lkpDepartamentoViviendaInfoEconomica.EditValue.ToString();
            obj.cod_provincia_vivienda = lkpProvinciaViviendaInfoEconomica.EditValue == null ? null : lkpProvinciaViviendaInfoEconomica.EditValue.ToString();
            obj.cod_distrito_vivienda = glkpDistritoViviendaInfoEconomica.EditValue == null ? null : glkpDistritoViviendaInfoEconomica.EditValue.ToString();
            obj.cod_tipovehiculo = lkpVehiculoInfoEconomica.EditValue == null ? null : lkpVehiculoInfoEconomica.EditValue.ToString();
            obj.dsc_marcavehiculo = txtMarcaInfoEconomica.Text;
            obj.dsc_modelovehiculo = txtModeloInfoEconomica.Text;
            obj.dsc_placavehiculo = txtPlacaVehiculoInfoEconomica.Text;
            obj.imp_valorvehiculo = txtValorComercialInfoEconomica.EditValue.ToString();
            obj.cod_monedavehiculo = lkpTipoMonedaVehiculoInfoEconomica.EditValue.ToString();
            obj.cod_tipoempresa = glkpTipoDocumentoeconomica.EditValue == null ? null : glkpTipoDocumentoeconomica.EditValue.ToString();
            obj.dsc_participacion_empresa = txtParticipacionInfoEconomica.Text;
            obj.dsc_RUC_empresa = txtRUCEmpresaInfoEconomica.Text;
            obj.dsc_giro_empresa = txtGiroEmpresaInfoEconomica.Text;
            obj.dsc_direccion_empresa = txtDireccionEmpresaInfoEconomica.Text;
            obj.dsc_referencia_empresa = "";
            obj.cod_pais_empresa = lkpPaisEmpresaInfoEconomica.EditValue == null ? null : lkpPaisEmpresaInfoEconomica.EditValue.ToString();
            obj.cod_departamento_empresa = lkpDepartamentoEmpresaInfoEconomica.EditValue == null ? null : lkpDepartamentoEmpresaInfoEconomica.EditValue.ToString();
            obj.cod_provincia_empresa = lkpProvinciaEmpresaInfoEconomica.EditValue == null ? null : lkpProvinciaEmpresaInfoEconomica.EditValue.ToString();
            obj.cod_distrito_empresa = glkpDistritoEmpresaInfoEconomica.EditValue == null ? null : glkpDistritoEmpresaInfoEconomica.EditValue.ToString();
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;

            return obj;
        }

        private async void chkFlgDNI_Click(object sender, EventArgs e)
        {
            await AdjuntarDocumentosVarios(1, "Doc. Identidad");
        }

        private async void chkFlgCV_Click(object sender, EventArgs e)
        {
            await AdjuntarDocumentosVarios(2, "CV");
        }

        private async void chkFlgAntPoliciales_Click(object sender, EventArgs e)
        {
            await AdjuntarDocumentosVarios(3, "Antc. Policial");
        }

        private async void chkFlgAntPenales_Click(object sender, EventArgs e)
        {
            await AdjuntarDocumentosVarios(4, "Antc. Penal");
        }

        private async void chkFlgVerifDomiciliaria_Click(object sender, EventArgs e)
        {
            await AdjuntarDocumentosVarios(5, "Verif. Domiciliaria");
        }

        private async void hlinkAdjuntarCertificadoInfoAcademica_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtCodFormAcademic.Text) != 0)
            {
                await AdjuntarDocumentosVarios(6, "Cert. Academico", lkpCarreraCursoFormAcademic.Text + " " + txtCodFormAcademic.Text);
                ObtenerDatos_InfoAcademica();
                gvListadoFormAcademica_FocusedRowChanged(gvListadoFormAcademica, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
            }
        }

        private async void hlinkAdjuntarCertificadoInfoProfesional_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtCodExpProfesional.Text) != 0)
            {
                await AdjuntarDocumentosVarios(7, "Exp. Laboral", txtRazonSocialExpProfesional.Text + " " + txtCargoExpProfesional.Text);
                ObtenerDatos_InfoProfesional();
                gvListadoExpProfesional_FocusedRowChanged(gvListadoExpProfesional, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
            }
        }



        static void Appl()
        {
            _clientApp = PublicClientApplicationBuilder.Create(ClientId)
                .WithAuthority($"{Instance}{TenantId}")
                .WithDefaultRedirectUri()
                .Build();
            TokenCacheHelper.EnableSerialization(_clientApp.UserTokenCache);
        }
        private static string ClientId = "";
        private static string TenantId = "";
        private static string Instance = "https://login.microsoftonline.com/";
        public static IPublicClientApplication _clientApp;
        public static IPublicClientApplication PublicClientApp { get { return _clientApp; } }

        private async Task AdjuntarDocumentosVarios(int opcionDoc, string nombreDoc, string nombreDocAdicional = "", int cod = 0)
        {
            int cod_emoof = 0;
            try
            {
                if (btnGuardarEMO.Text == "EDITAR") { cod_emoof = cod; }
                else
                {
                    etrabemo = unit.Trabajador.Obtener_emo<eTrabajador.eEMO>(113, cod_trabajador: cod_trabajador, cod_empresa: cod_empresa, cod_documento: nombreDoc);
                    cod_emoof = etrabemo.cod_EMO;
                }

                DateTime FechaRegistro = DateTime.Today;
                string nombreCarpeta = "";

                eTrabajador resultado = unit.Trabajador.Obtener_Trabajador<eTrabajador>(2, cod_trabajador, cod_empresa);
                if (resultado == null) { MessageBox.Show("Antes de adjuntar los docuentos debe crear al trabajador.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                OpenFileDialog myFileDialog = new OpenFileDialog();
                myFileDialog.Filter = "Archivos (*.pdf)|; *.pdf";
                myFileDialog.FilterIndex = 1;
                myFileDialog.InitialDirectory = "C:\\";
                myFileDialog.Title = "Abrir archivo";
                myFileDialog.CheckFileExists = false;
                myFileDialog.Multiselect = false;

                DialogResult result = myFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string IdCarpetaTrabajador = "", Extension = "";
                    var idArchivoPDF = "";
                    var TamañoDoc = new FileInfo(myFileDialog.FileName).Length / 1024;
                    if (opcionDoc == 8 || opcionDoc == 10)
                    {
                        if (TamañoDoc < 4000)
                        {
                            varPathOrigen = myFileDialog.FileName;

                            //varNombreArchivo = nombreDoc + "-" + FechaRegistro.ToString("yyyyMMdd") + "."+ Path.GetExtension(myFileDialog.SafeFileName);
                            varNombreArchivo = nombreDocAdicional + "-" + FechaRegistro.Year.ToString() + "." + FechaRegistro.ToString("MM") + "." + FechaRegistro.ToString("dd") + (nombreDocAdicional != "" ? " " + nombreDocAdicional : "") + "_" + cod_emoof + Path.GetExtension(myFileDialog.SafeFileName);
                            //varNombreArchivoSinExtension = nombreDoc + "-" + FechaRegistro.ToString("yyyyMMdd");
                            Extension = Path.GetExtension(myFileDialog.SafeFileName);
                        }
                        else
                        {
                            MessageBox.Show("Solo puede subir archivos hasta 4MB de tamaño", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        if (TamañoDoc < 4000)
                        {
                            varPathOrigen = myFileDialog.FileName;

                            //varNombreArchivo = nombreDoc + "-" + FechaRegistro.ToString("yyyyMMdd") + "."+ Path.GetExtension(myFileDialog.SafeFileName);
                            varNombreArchivo = nombreDoc + "-" + FechaRegistro.Year.ToString() + "." + FechaRegistro.ToString("MM") + "." + FechaRegistro.ToString("dd") + (nombreDocAdicional != "" ? " " + nombreDocAdicional : "") + Path.GetExtension(myFileDialog.SafeFileName);
                            //varNombreArchivoSinExtension = nombreDoc + "-" + FechaRegistro.ToString("yyyyMMdd");
                            Extension = Path.GetExtension(myFileDialog.SafeFileName);
                        }
                        else
                        {
                            MessageBox.Show("Solo puede subir archivos hasta 4MB de tamaño", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                    unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Por favor espere...", "Cargando...");
                    eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, cod_empresa);
                    if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
                    { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                    ClientId = eEmp.ClientIdOnedrive;
                    TenantId = eEmp.TenantOnedrive;
                    Appl();
                    var app = PublicClientApp;
                    string correo = eEmp.UsuarioOnedrivePersonal;
                    string password = eEmp.ClaveOnedrivePersonal;

                    var securePassword = new SecureString();
                    foreach (char c in password)
                        securePassword.AppendChar(c);

                    authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

                    GraphClient = new Microsoft.Graph.GraphServiceClient(
                      new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                      {
                          requestMessage
                              .Headers
                              .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                          return Task.FromResult(0);
                      }));

                    eEmpresa.eOnedrive_Empresa eDatos = new eEmpresa.eOnedrive_Empresa();
                    eDatos = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa.eOnedrive_Empresa>(13, cod_empresa, dsc_Carpeta: "Personal");
                    var targetItemFolderId = eDatos.idCarpeta;

                    nombreCarpeta = resultado.dsc_documento + " - " + resultado.dsc_nombres_completos;

                    eTrabajador objCarpeta = unit.Trabajador.ObtenerDatosOneDrive<eTrabajador>(14, cod_trabajador: cod_trabajador);
                    if (objCarpeta.idCarpeta_Trabajador == null || objCarpeta.idCarpeta_Trabajador == "") //Si no existe folder lo crea
                    {
                        var driveItem = new Microsoft.Graph.DriveItem
                        {
                            Name = nombreCarpeta,
                            Folder = new Microsoft.Graph.Folder
                            {
                            },
                            AdditionalData = new Dictionary<string, object>()
                            {
                            {"@microsoft.graph.conflictBehavior", "rename"}
                            }
                        };

                        var driveItemInfo = await GraphClient.Me.Drive.Items[targetItemFolderId].Children.Request().AddAsync(driveItem);
                        IdCarpetaTrabajador = driveItemInfo.Id;
                    }
                    else //Si existe folder obtener id
                    {
                        IdCarpetaTrabajador = resultado.idCarpeta_Trabajador;
                    }

                    //crea archivo en el OneDrive
                    byte[] data = System.IO.File.ReadAllBytes(varPathOrigen);
                    using (Stream stream = new MemoryStream(data))
                    {
                        string res = "";
                        var DriveItem = await GraphClient.Me.Drive.Items[IdCarpetaTrabajador].ItemWithPath(varNombreArchivo).Content.Request().PutAsync<Microsoft.Graph.DriveItem>(stream);
                        idArchivoPDF = DriveItem.Id;

                        eTrabajador objTrab = new eTrabajador();
                        objTrab.cod_trabajador = cod_trabajador;
                        objTrab.cod_empresa = cod_empresa;
                        objTrab.idCarpeta_Trabajador = IdCarpetaTrabajador;
                        objTrab.id_DNI = opcionDoc == 1 ? idArchivoPDF : null;
                        objTrab.id_CV = opcionDoc == 2 ? idArchivoPDF : null;
                        objTrab.id_AntPolicial = opcionDoc == 3 ? idArchivoPDF : null;
                        objTrab.id_AntPenal = opcionDoc == 4 ? idArchivoPDF : null;
                        objTrab.id_VerifDomiciliaria = opcionDoc == 5 ? idArchivoPDF : null;

                        chkFlgDNI.CheckState = opcionDoc == 1 ? CheckState.Checked : chkFlgDNI.CheckState;
                        chkFlgCV.CheckState = opcionDoc == 2 ? CheckState.Checked : chkFlgCV.CheckState;
                        chkFlgAntPoliciales.CheckState = opcionDoc == 3 ? CheckState.Checked : chkFlgAntPoliciales.CheckState;
                        chkFlgAntPenales.CheckState = opcionDoc == 4 ? CheckState.Checked : chkFlgAntPenales.CheckState;
                        chkFlgVerifDomiciliaria.CheckState = opcionDoc == 5 ? CheckState.Checked : chkFlgVerifDomiciliaria.CheckState;

                        btnVerDocIdentidad.Enabled = opcionDoc == 1 ? true : btnVerDocIdentidad.Enabled;
                        btnVerCV.Enabled = opcionDoc == 2 ? true : btnVerCV.Enabled;
                        btnVerAntcPoliciales.Enabled = opcionDoc == 3 ? true : btnVerAntcPoliciales.Enabled;
                        btnVerAntcPenales.Enabled = opcionDoc == 4 ? true : btnVerAntcPenales.Enabled;
                        btnVerVerifDomiciliaria.Enabled = opcionDoc == 5 ? true : btnVerVerifDomiciliaria.Enabled;

                        if (opcionDoc <= 5) res = unit.Trabajador.ActualizarInformacionDocumentos("SI", opcionDoc, objTrab);

                        if (opcionDoc == 6)
                        {
                            eTrabajador.eInfoAcademica_Trabajador eAcad = new eTrabajador.eInfoAcademica_Trabajador();
                            if (objCarpeta.idCarpeta_Trabajador == null || objCarpeta.idCarpeta_Trabajador == "") { res = unit.Trabajador.ActualizarInformacionDocumentos("SI", opcionDoc, objTrab); }
                            eAcad.cod_trabajador = cod_trabajador; eAcad.cod_empresa = cod_empresa;
                            eAcad.cod_infoacademica = Convert.ToInt32(txtCodFormAcademic.Text);
                            eAcad.id_certificado = idArchivoPDF;
                            eAcad = unit.Trabajador.InsertarActualizar_InfoAcademicaTrabajador<eTrabajador.eInfoAcademica_Trabajador>(eAcad, "SI");
                            res = eAcad != null ? "OK" : "ERROR";
                        }
                        if (opcionDoc == 7)
                        {
                            eTrabajador.eInfoProfesional_Trabajador eProf = new eTrabajador.eInfoProfesional_Trabajador();
                            if (objCarpeta.idCarpeta_Trabajador == null || objCarpeta.idCarpeta_Trabajador == "") { res = unit.Trabajador.ActualizarInformacionDocumentos("SI", opcionDoc, objTrab); }
                            eProf.cod_trabajador = cod_trabajador; eProf.cod_empresa = cod_empresa;
                            eProf.cod_infoprofesional = Convert.ToInt32(txtCodExpProfesional.Text);
                            eProf.id_certificado = idArchivoPDF;
                            eProf = unit.Trabajador.InsertarActualizar_InfoProfesionalTrabajador<eTrabajador.eInfoProfesional_Trabajador>(eProf, "SI");
                            res = eProf != null ? "OK" : "ERROR";
                        }
                        if (opcionDoc == 8)
                        {
                            string resulto = ""; string cod_doc = "";
                            eTrabajador.eDocumento_Trabajador objInfolab = new eTrabajador.eDocumento_Trabajador();
                            eTrabajador.eEMO eEMO = new eTrabajador.eEMO();
                            eTrabajador.eEMO er = gvEMO.GetFocusedRow() as eTrabajador.eEMO;

                            cod_doc = lkpTipodoc.EditValue.ToString();

                            objInfolab.dsc_archivo = resultado.dsc_documento + "_" + FechaRegistro.Year.ToString() + "." + FechaRegistro.ToString("MM") + "." + FechaRegistro.ToString("dd") + "-" + eTrabdoc.dsc_abreviatura + "_" + cod_emoof;


                            if (objCarpeta.idCarpeta_Trabajador == null || objCarpeta.idCarpeta_Trabajador == "") { res = unit.Trabajador.ActualizarInformacionDocumentos("SI", opcionDoc, objTrab); }
                            eEMO.cod_trabajador = cod_trabajador;
                            eEMO.cod_empresa = cod_empresa;
                            eEMO.fch_registro = FechaRegistro;
                            eEMO.dsc_descripcion = objInfolab.dsc_archivo;
                            eEMO.dsc_anho = FechaRegistro.Year; eEMO.flg_certificado = "SI"; eEMO.id_certificado = idArchivoPDF;
                            eEMO.fch_evaluacion = dtFchEvaluacion.EditValue == null ? new DateTime() : Convert.ToDateTime(dtFchEvaluacion.EditValue);
                            eEMO.fch_evaluacion_obs = dtFchEvaluacionObs.EditValue == null ? new DateTime() : Convert.ToDateTime(dtFchEvaluacionObs.EditValue);
                            if (grdbFlgObservado.SelectedIndex == 0) { eEMO.flg_observacion = "APTO"; } else if (grdbFlgObservado.SelectedIndex == 1) { eEMO.flg_observacion = "NO APTO"; } else { eEMO.flg_observacion = "APTO CON RESTRICCIONES"; }
                            eEMO.dsc_observacion = memObservacion.Text;
                            eEMO.dsc_anho = FechaRegistro.Year; eEMO.flg_certificado = "SI"; eEMO.id_certificado = idArchivoPDF;

                            flg_documentor = eEMO.flg_certificado;
                            id_documentor = eEMO.id_certificado;
                            dsc_archivor = eEMO.dsc_descripcion;
                            txtArchivoEmo.Text = dsc_archivor;
                            res = "PDF";
                            remplazo = "SI";
                            gvEMO.RefreshData();
                        }
                        if (opcionDoc == 9)
                        {
                            string resulto = "";
                            eTrabajador.eInfoLaboral_Trabajador objInfolab = new eTrabajador.eInfoLaboral_Trabajador();
                            if (objCarpeta.idCarpeta_Trabajador == null || objCarpeta.idCarpeta_Trabajador == "") { res = unit.Trabajador.ActualizarInformacionDocumentos("SI", opcionDoc, objTrab); }
                            objInfolab.cod_trabajador = cod_trabajador;
                            objInfolab.cod_empresa = cod_empresa;
                            objInfolab.cod_infolab = Convert.ToInt32(txtCodInfoLaboral.Text);
                            objInfolab.id_contrato = idArchivoPDF;
                            resulto = unit.Trabajador.ActualizarDocumentoInfolab(opcion: 5, id_contrato: objInfolab.id_contrato, cod_infolab: objInfolab.cod_infolab, cod_empresa: objInfolab.cod_empresa,
                                cod_trabajador: objInfolab.cod_trabajador, flg_contrato: "SI");
                            chckAdjuntarContrato.CheckState = objInfolab.flg_contrato == "SI" ? CheckState.Checked : chkFlgDNI.CheckState;
                            res = objInfolab != null ? "OK" : "ERROR";

                        }
                        if (opcionDoc == 10)
                        {
                            string resulto = ""; string cod_doc = "";

                            eTrabajador.eDocumento_Trabajador objInfolab = new eTrabajador.eDocumento_Trabajador();
                            objInfolab.flg_documento = "SI";
                            objInfolab.id_documento = idArchivoPDF;

                            cod_doc = lkpTipoDocumentoingresos.EditValue.ToString();
                            eTrabdoc = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(119, cod_documento: cod_doc);


                            objInfolab.dsc_archivo = resultado.dsc_documento + "_" + FechaRegistro.Year.ToString() + "." + FechaRegistro.ToString("MM") + "." + FechaRegistro.ToString("dd") + "-" + eTrabdoc.dsc_abreviatura;


                            flg_documentor = objInfolab.flg_documento;
                            id_documentor = objInfolab.id_documento;
                            dsc_archivor = objInfolab.dsc_archivo;


                            txtArchivo.Text = dsc_archivor;
                            res = "PDF";


                        }

                        SplashScreenManager.CloseForm();

                        if (res == "OK")
                        {
                            MessageBox.Show("Se registró el documento satisfactoriamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            if (res == "PDF") { }
                            else
                            {
                                MessageBox.Show("Hubieron problemas al registrar el documento", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dtFecNacimientoInfoFamiliar_EditValueChanged(object sender, EventArgs e)
        {
            DateTime fechaNacimiento = dtFecNacimientoInfoFamiliar.DateTime;
            DateTime fechaActual = DateTime.Today;
            int edad = fechaActual.Year - fechaNacimiento.Year;
            txtEdadInfoFamiliar.Text = Convert.ToString(edad).ToString();


        }

        private void btnVerDocIdentidad_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            VerDocumentos(1, "Doc. Identidad");
        }

        private void btnVerCV_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            VerDocumentos(2, "CV");
        }

        private void btnVerAntcPoliciales_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            VerDocumentos(3, "Antc. Policial");
        }

        private void btnVerAntcPenales_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            VerDocumentos(4, "Antc. Penal");
        }

        private void btnVerVerifDomiciliaria_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            VerDocumentos(5, "Verif. Domiciliaria");
        }

        private void txtEstaturaCaractFisica_EditValueChanged(object sender, EventArgs e)
        {
            decimal peso = 0, estatura = 0;

            estatura = Convert.ToDecimal(Math.Pow(Convert.ToDouble(txtEstaturaCaractFisica.EditValue), 2));
            peso = Convert.ToDecimal(txtPesoCaractFisica.EditValue);
            if (estatura > 0) txtIMCCaractFisica.EditValue = Math.Round((peso / estatura), 2);
        }

        private void btnGuardarInfoSalud_Click(object sender, EventArgs e)
        {
            if (chkflgAlergiasInfoSalud.Checked == true && mmAlergias.Text == "") { MessageBox.Show("Ingrese Alergias.", "ADVERTENCIA ,FALTA LLENAR CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Warning); mmAlergias.Focus(); return; }
            if (chkflgOperacionesInfoSalud.Checked == true && mmOperaciones.Text == "") { MessageBox.Show("Ingrese Operaciones.", "ADVERTENCIA ,FALTA LLENAR CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Warning); mmAlergias.Focus(); return; }
            if (chkflgEnfPrexistenteInfoSalud.Checked == true && mmEnfPrexistente.Text == "") { MessageBox.Show("Ingrese Enfermedad Prexistente.", "ADVERTENCIA ,FALTA LLENAR CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Warning); mmEnfPrexistente.Focus(); return; }
            if (chkflgTratamientoInfoSalud.Checked == true && mmTratamiento.Text == "") { MessageBox.Show("Ingrese Tratamiento.", "ADVERTENCIA ,FALTA LLENAR CAMPOSFALTA LLENAR CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Warning); mmTratamiento.Focus(); return; }
            if (chkflgOtrosInfoSalud.Checked == true && mmOtros.Text == "") { MessageBox.Show("Ingrese Registro.", "ADVERTENCIA ,FALTA LLENAR CAMPOSFALTA LLENAR CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Warning); mmOtros.Focus(); return; }
            if (chkflgEnfActualInfoSalud.Checked == true && mmEnfActualidad.Text == "") { MessageBox.Show("Ingrese Enfermedades Actuales.", "ADVERTENCIA ,FALTA LLENAR CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Warning); mmEnfActualidad.Focus(); return; }
            if (lkpSituacionTrabajador_salud.EditValue==null) { MessageBox.Show("Ingrese Situacion Trabajador.", "ADVERTENCIA ,FALTA LLENAR CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpSituacionTrabajador_salud.Focus(); return; }
            if (lkpSeguroSaludInfoSalud.EditValue == null) { MessageBox.Show("Ingrese Seguro Salud.", "ADVERTENCIA ,FALTA LLENAR CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpSeguroSaludInfoSalud.Focus(); return; }
            if (lkpregimensalud.EditValue == null) { MessageBox.Show("Ingrese Regimen Salud.", "ADVERTENCIA ,FALTA LLENAR CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpregimensalud.Focus(); return; }
            if (lkpTipoTrabajadorsalud.EditValue == null) { MessageBox.Show("Ingrese Tipo Trabajador.", "ADVERTENCIA ,FALTA LLENAR CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipoTrabajadorsalud.Focus(); return; }
            if (lkpSCRTsalud.EditValue == null) { MessageBox.Show("Ingrese SCRT Salud.", "ADVERTENCIA ,FALTA LLENAR CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpSCRTsalud.Focus(); return; }
            if (lkpSCRTpension.EditValue == null) { MessageBox.Show("Ingrese SCRT Pension.", "ADVERTENCIA ,FALTA LLENAR CAMPOS", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpSCRTpension.Focus(); return; }

            eTrabajador.eInfoSalud_Trabajador obj = AsignarValores_InfoSalud();
            obj = unit.Trabajador.InsertarActualizar_InfoSaludTrabajador<eTrabajador.eInfoSalud_Trabajador>(obj);
            if (obj == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (obj != null)
            {
                ActualizarListado = "SI";
                MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ObtenerDatos_InfoSalud();
            }
        }




        private void btnGuardarInfoFamiliar_Click(object sender, EventArgs e)
        {
            txtApellPaternoInfoFamiliar.Select();
            if (lkpParentescoInfoFamiliar.EditValue == null) { MessageBox.Show("Debe seleccionar el parentesco.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpParentescoInfoFamiliar.Focus(); return; }
            if (dtFecNacimientoInfoFamiliar.EditValue == null) { MessageBox.Show("Debe ingresar la fecha de nacimiento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtFecNacimientoInfoFamiliar.Focus(); return; }
            if (txtApellPaternoInfoFamiliar.Text.Trim() == "") { MessageBox.Show("Debe ingresar el apellido paterno.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtApellPaternoInfoFamiliar.Focus(); return; }
            if (txtApellMaternoInfoFamiliar.Text.Trim() == "") { MessageBox.Show("Debe ingresar el apellido materno.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtApellMaternoInfoFamiliar.Focus(); return; }
            if (txtNombreInfoFamiliar.Text.Trim() == "") { MessageBox.Show("Debe ingresar el nombre.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNombreInfoFamiliar.Focus(); return; }
            //if (glkpTipoDocumentoInfoFamiliar.EditValue == null) { MessageBox.Show("Debe seleccionar el tipo de documento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); glkpTipoDocumentoInfoFamiliar.Focus(); return; }
            //if (txtNroDocumentoInfoFamiliar.Text.Trim() == "") { MessageBox.Show("Debe ingresar el número de documento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNroDocumentoInfoFamiliar.Focus(); return; }
            if (txtNroDocumentoContacto.Text != "")
            {
                eProveedor objw = new eProveedor();
                objw = unit.Proveedores.Validar_NroDocumento<eProveedor>(19, "", glkpTipoDocumentoInfoFamiliar.EditValue.ToString());
                int nrodocumento = Convert.ToInt32(txtNroDocumentoInfoFamiliar.Text.Length);
                if (nrodocumento < objw.ctd_digitos) { MessageBox.Show("Los digitos del documento debe ser igual a " + objw.ctd_digitos, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNroDocumentoInfoFamiliar.Focus(); return; }
                txtNroDocumentoInfoFamiliar.Properties.MaxLength = objw.ctd_digitos;
            }
            eTrabajador.eInfoFamiliar_Trabajador obj = AsignarValores_InfoFamiliar();
            obj = unit.Trabajador.InsertarActualizar_InfoFamiliarTrabajador<eTrabajador.eInfoFamiliar_Trabajador>(obj);
            if (obj == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (obj != null)
            {
                ActualizarListado = "SI";
                MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ObtenerDatos_InfoFamiliar();
                gvListadoInfoFamiliar_FocusedRowChanged(gvListadoInfoFamiliar, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
            }
        }

        private eTrabajador.eInfoFamiliar_Trabajador AsignarValores_InfoFamiliar()
        {
            eTrabajador.eInfoFamiliar_Trabajador obj = new eTrabajador.eInfoFamiliar_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = lkpEmpresa.EditValue.ToString();
            obj.cod_infofamiliar = Convert.ToInt32(txtCodInfoFamiliar.Text);
            obj.cod_parentesco = lkpParentescoInfoFamiliar.EditValue.ToString();
            obj.dsc_apellido_paterno = txtApellPaternoInfoFamiliar.Text.Trim();
            obj.dsc_apellido_materno = txtApellMaternoInfoFamiliar.Text.Trim();
            obj.dsc_nombres = txtNombreInfoFamiliar.Text.Trim();
            obj.fch_nacimiento = dtFecNacimientoInfoFamiliar.EditValue == null ? new DateTime() : Convert.ToDateTime(dtFecNacimientoInfoFamiliar.EditValue);
            obj.flg_vivo = chkflgVivoInfoFamiliar.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.cod_tipo_documento = glkpTipoDocumentoInfoFamiliar.EditValue == null ? null : glkpTipoDocumentoInfoFamiliar.EditValue.ToString();
            obj.dsc_documento = txtNroDocumentoInfoFamiliar.Text;
            obj.dsc_mail = txtEmailInfoFamiliar.Text;
            obj.dsc_telefono = txtTelefonoInfoFamiliar.Text;
            obj.dsc_celular = txtCelularInfoFamiliar.Text;
            obj.dsc_profesion = txtProfesionInfoFamiliar.Text;
            obj.dsc_centrolaboral = txtCentroLaboralInfoFamiliar.Text;
            obj.dsc_gradoinstruccion = txtGradoInstruccionInfoFamiliar.Text;
            obj.dsc_ocupacion = txtOcupacionInfoFamiliar.Text;
            obj.dsc_discapacidad = txtDiscapacidadInfoFamiliar.Text;
            obj.dsc_direccion = txtDireccionInfoFamiliar.Text;
            obj.dsc_referencia = "";
            obj.cod_pais = lkpPaisInfoFamiliar.EditValue == null ? null : lkpPaisInfoFamiliar.EditValue.ToString();
            obj.cod_departamento = lkpDepartamentoInfoFamiliar.EditValue == null ? null : lkpDepartamentoInfoFamiliar.EditValue.ToString();
            obj.cod_provincia = lkpProvinciaInfoFamiliar.EditValue == null ? null : lkpProvinciaInfoFamiliar.EditValue.ToString();
            obj.cod_distrito = glkpDistritoInfoFamiliar.EditValue == null ? null : glkpDistritoInfoFamiliar.EditValue.ToString();
            obj.flg_enfermedad = chkflgEnfermedadInfoFamiliar.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.dsc_enfermedad = txtEnfermedadInfoFamiliar.Text;
            obj.flg_adiccion = chkflgAdiccionInfoFamiliar.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.dsc_adiccion = txtAdiccionInfoFamiliar.Text;
            obj.flg_dependenciaeconomica = chkflgDependeEconomiaInfoFamiliar.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_trabajador;

            return obj;
        }

        private void btnNuevaInfoFamiliar_Click(object sender, EventArgs e)
        {
            txtCodInfoFamiliar.Text = "0";
            lkpParentescoInfoFamiliar.Text = null;
            txtApellPaternoInfoFamiliar.Text = "";
            txtApellMaternoInfoFamiliar.Text = "";
            txtNombreInfoFamiliar.Text = "";
            dtFecNacimientoInfoFamiliar.EditValue = null;
            chkflgVivoInfoFamiliar.CheckState = CheckState.Checked;
            glkpTipoDocumentoInfoFamiliar.EditValue = null;
            txtNroDocumentoInfoFamiliar.Text = "";
            txtEmailInfoFamiliar.Text = "";
            txtTelefonoInfoFamiliar.Text = "";
            txtCelularInfoFamiliar.Text = "";
            txtProfesionInfoFamiliar.Text = "";
            txtCentroLaboralInfoFamiliar.Text = "";
            txtGradoInstruccionInfoFamiliar.Text = "";
            txtOcupacionInfoFamiliar.Text = "";
            txtDiscapacidadInfoFamiliar.Text = "";
            txtDireccionInfoFamiliar.Text = "";
            lkpPaisInfoFamiliar.EditValue = "00001";
            lkpDepartamentoInfoFamiliar.EditValue = "00015";
            lkpProvinciaInfoFamiliar.EditValue = "00128";
            glkpDistritoInfoFamiliar.EditValue = "01251";
            chkflgEnfermedadInfoFamiliar.CheckState = CheckState.Unchecked;
            txtEnfermedadInfoFamiliar.Text = "";
            chkflgAdiccionInfoFamiliar.CheckState = CheckState.Unchecked;
            txtAdiccionInfoFamiliar.Text = "";
            chkflgDependeEconomiaInfoFamiliar.CheckState = CheckState.Unchecked;
            chckdireccionfamiliar.CheckState = CheckState.Unchecked;
            txtDireccionInfoFamiliar.Enabled = true;
            lkpParentescoInfoFamiliar.EditValue = null;
        }

        private void btnGuardarFormAcademic_Click(object sender, EventArgs e)
        {
            if (lkpNivelAcademicoFormAcademic.EditValue == null) { MessageBox.Show("Debe seleccionar el nivel académico.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpNivelAcademicoFormAcademic.Focus(); return; }
            if (lkpCentroEstudiosFormAcademic.EditValue == null) { MessageBox.Show("Debe ingresar el centro de estudios.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpCentroEstudiosFormAcademic.Focus(); return; }
            if (lkpCarreraCursoFormAcademic.EditValue == null) { MessageBox.Show("Debe ingresar el nombre de la carrera o curso.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpCarreraCursoFormAcademic.Focus(); return; }
            if (dtAnhoInicioFormAcademic.EditValue == null) { MessageBox.Show("Debe seleccionar el año de inicio.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtAnhoInicioFormAcademic.Focus(); return; }
            if (dtAnhoFinFormAcademic.EditValue == null) { MessageBox.Show("Debe seleccionar el año de fin.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtAnhoFinFormAcademic.Focus(); return; }
            if (lkpPaisAcademico.EditValue == null) { MessageBox.Show("Debe seleccionar Pais.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpPaisAcademico.Focus(); return; }


            eTrabajador.eInfoAcademica_Trabajador obj = AsignarValores_InfoAcademica();
            obj = unit.Trabajador.InsertarActualizar_InfoAcademicaTrabajador<eTrabajador.eInfoAcademica_Trabajador>(obj);
            if (obj == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (obj != null)
            {
                ActualizarListado = "SI";
                MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ObtenerDatos_InfoAcademica();
                gvListadoFormAcademica_FocusedRowChanged(gvListadoFormAcademica, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
            }
        }

        private eTrabajador.eInfoAcademica_Trabajador AsignarValores_InfoAcademica()
        {
            eTrabajador.eInfoAcademica_Trabajador obj = new eTrabajador.eInfoAcademica_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = lkpEmpresa.EditValue.ToString();
            obj.cod_infoacademica = Convert.ToInt32(txtCodFormAcademic.Text);
            obj.cod_nivelacademico = lkpNivelAcademicoFormAcademic.EditValue == null ? null : lkpNivelAcademicoFormAcademic.EditValue.ToString();
            obj.dsc_centroestudios = lkpCentroEstudiosFormAcademic.Text == null ? null : lkpCentroEstudiosFormAcademic.Text.ToString();
            obj.dsc_lugar = txtLugarFormAcademic.Text;
            obj.dsc_carrera_curso = lkpCarreraCursoFormAcademic.Text == null ? null : lkpCarreraCursoFormAcademic.Text.ToString();
            obj.dsc_grado = txtGradoTituloFormAcademic.Text;
            obj.anho_inicio = Convert.ToDateTime(dtAnhoInicioFormAcademic.EditValue).Year;
            obj.anho_fin = Convert.ToDateTime(dtAnhoFinFormAcademic.EditValue).Year;
            obj.imp_promedio = Convert.ToDecimal(txtPromedioAcademicoFormAcademic.Text);
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            obj.cod_centroestudios = lkpCentroEstudiosFormAcademic.EditValue == null ? null : lkpCentroEstudiosFormAcademic.EditValue.ToString();
            obj.cod_carrera_profesional = lkpCarreraCursoFormAcademic.EditValue == null ? null : lkpCarreraCursoFormAcademic.EditValue.ToString();
            obj.cod_pais = lkpPaisAcademico.EditValue == null ? null : lkpPaisAcademico.EditValue.ToString();
            obj.flg_EducComplPeru = chkflgEducComplPeru.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.cod_tiporegimenacademico = lkpRegimenAcademico.EditValue == null ? null : lkpRegimenAcademico.EditValue.ToString(); 
            obj.cod_tipoinstitucion = lkpTipoInstitucion.EditValue == null ? null : lkpTipoInstitucion.EditValue.ToString();
            return obj;
        }

        private void btnNuevaFormAcademic_Click(object sender, EventArgs e)
        {
            txtCodFormAcademic.Text = "0";
            hlinkAdjuntarCertificadoInfoAcademica.Enabled = false;
            lkpNivelAcademicoFormAcademic.EditValue = null;
            lkpCentroEstudiosFormAcademic.EditValue = null;
            txtLugarFormAcademic.Text = "";
            lkpCarreraCursoFormAcademic.EditValue = null;
            txtGradoTituloFormAcademic.Text = "";
            dtAnhoInicioFormAcademic.EditValue = DateTime.Today;
            dtAnhoFinFormAcademic.EditValue = DateTime.Today;
            txtPromedioAcademicoFormAcademic.Text = "";
        }

        private void btnGuardarExpProfesional_Click(object sender, EventArgs e)
        {
            if (txtRazonSocialExpProfesional.Text.Trim() == "") { MessageBox.Show("Debe ingresar la razón social.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtRazonSocialExpProfesional.Focus(); return; }
            if (txtCargoExpProfesional.Text.Trim() == "") { MessageBox.Show("Debe ingresar el cargo.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCargoExpProfesional.Focus(); return; }
            if (txtJefeInmediatoExpProfesional.Text.Trim() == "") { MessageBox.Show("Debe ingresar el nombre del jefe.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtJefeInmediatoExpProfesional.Focus(); return; }
            if (txtCargoJefeExpProfesional.Text.Trim() == "") { MessageBox.Show("Debe ingresar el cargo del jefe.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCargoJefeExpProfesional.Focus(); return; }
            if (txtMotivoSalidaExpProfesional.Text.Trim() == "") { MessageBox.Show("Debe ingresar el motivo de salida.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtMotivoSalidaExpProfesional.Focus(); return; }
            if (dtFecInicioExpProfesional.EditValue == null) { MessageBox.Show("Debe seleccionar la fecha de inicio.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtFecInicioExpProfesional.Focus(); return; }
            if (dtFecFinExpProfesional.EditValue == null) { MessageBox.Show("Debe seleccionar la fecha de inicio.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtFecFinExpProfesional.Focus(); return; }

            eTrabajador.eInfoProfesional_Trabajador obj = AsignarValores_InfoProfesional();
            obj = unit.Trabajador.InsertarActualizar_InfoProfesionalTrabajador<eTrabajador.eInfoProfesional_Trabajador>(obj, "NO");
            if (obj == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (obj != null)
            {
                ActualizarListado = "SI";
                MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ObtenerDatos_InfoProfesional();
                gvListadoExpProfesional_FocusedRowChanged(gvListadoExpProfesional, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
            }
        }

        private eTrabajador.eInfoProfesional_Trabajador AsignarValores_InfoProfesional()
        {
            eTrabajador.eInfoProfesional_Trabajador obj = new eTrabajador.eInfoProfesional_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = lkpEmpresa.EditValue.ToString();
            obj.cod_infoprofesional = Convert.ToInt32(txtCodExpProfesional.Text);
            obj.dsc_nombre_jefe = txtJefeInmediatoExpProfesional.Text;
            obj.dsc_razon_social = txtRazonSocialExpProfesional.Text;
            obj.dsc_cargo_jefe = txtCargoJefeExpProfesional.Text;
            obj.dsc_cargo = txtCargoExpProfesional.Text;
            obj.dsc_motivo_salida = txtMotivoSalidaExpProfesional.Text;
            obj.fch_inicio = Convert.ToDateTime(dtFecInicioExpProfesional.EditValue);
            obj.fch_fin = Convert.ToDateTime(dtFecFinExpProfesional.EditValue);
            obj.dsc_celular = txtCelularExpProfesional.Text;
            obj.dsc_comentarios = mmComentariosExpProfesional.Text;
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;

            return obj;
        }

        private void btnNuevaExpProfesional_Click(object sender, EventArgs e)
        {
            nuevoinfoprofesional();

        }

        private void nuevoinfoprofesional()
        {
            txtCodExpProfesional.Text = "0";
            hlinkAdjuntarCertificadoInfoProfesional.Enabled = false;
            txtJefeInmediatoExpProfesional.Text = "";
            txtRazonSocialExpProfesional.Text = "";
            txtCargoJefeExpProfesional.Text = "";
            txtCargoExpProfesional.Text = "";
            txtMotivoSalidaExpProfesional.Text = "";
            dtFecInicioExpProfesional.EditValue = DateTime.Today;
            dtFecFinExpProfesional.EditValue = DateTime.Today;
            txtCelularExpProfesional.Text = "";
            mmComentariosExpProfesional.Text = "";
        }

        private void btnGuardarInfoVivienda_Click(object sender, EventArgs e)
        {
            if (lkpViviendaInfoVivienda.EditValue == null) { MessageBox.Show("Debe seleccionar el tipo de vivienda.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpViviendaInfoVivienda.Focus(); return; }
            if (lkpComodidadInfoVivienda.EditValue == null) { MessageBox.Show("Debe seleccionar el tipo de comodidad.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpComodidadInfoVivienda.Focus(); return; }

            eTrabajador.eInfoVivienda_Trabajador obj = AsignarValores_InfoVivienda();
            obj = unit.Trabajador.InsertarActualizar_InfoViviendaTrabajador<eTrabajador.eInfoVivienda_Trabajador>(obj);
            if (obj == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (obj != null)
            {
                ActualizarListado = "SI";
                MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ObtenerDatos_InfoVivienda();
            }
        }

        private eTrabajador.eInfoVivienda_Trabajador AsignarValores_InfoVivienda()
        {
            eTrabajador.eInfoVivienda_Trabajador obj = new eTrabajador.eInfoVivienda_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = lkpEmpresa.EditValue.ToString();
            obj.cod_tipovivienda = lkpViviendaInfoVivienda.EditValue == null ? null : lkpViviendaInfoVivienda.EditValue.ToString();
            obj.cod_tipocomodidad = lkpComodidadInfoVivienda.EditValue == null ? null : lkpComodidadInfoVivienda.EditValue.ToString();
            obj.flg_puertas = chkflgPuertasInfoVivienda.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_ventanas = chkflgVentanasInfoVivienda.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_techo = chkflgTechoInfoVivienda.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_telefono = chkflgTelefonoInfoVivienda.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_celulares = chkCelularesInfoVivienda.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_internet_comunicacion = chkflgInternetComunicacionInfoVivienda.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_luz = chkflgLuzInfoVivienda.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_agua = chkflgAguaInfoVivienda.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_desague = chkflgDesagueInfoVivienda.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_gas = chkflgGasInfoVivienda.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_cable = chkflgCableInfoVivienda.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_internet_servicio = chkflgInternetServicioInfoVivienda.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.dsc_viaacceso = mmViasAccesoInfoVivienda.Text;
            obj.dsc_iluminacion = mmIluminacionInfoVivienda.Text;
            obj.dsc_entorno = mmEntornoInfoVivienda.Text;
            obj.dsc_puestopolicial = mmPuestoPolicialInfoVivienda.Text;
            obj.dsc_nombre_familiar = txtNombreFamiliarInfoVivienda.Text;
            obj.dsc_horasencasa = txtHorasCasaInfoVivienda.Text;
            obj.cod_parentesco = lkpParentescoInfoVivienda.EditValue == null ? null : lkpParentescoInfoVivienda.EditValue.ToString();
            obj.dsc_celular = txtCelularInfoVivienda.Text;
            obj.dsc_mail = txtEmailInfoVivienda.Text;
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;

            return obj;
        }

        private void btnEliminarContacto_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("¿Esta seguro de eliminar el Registro?" + Environment.NewLine + "Esta acción es irreversible.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {//user.cod_usuario
                    string result = "";
                    result = unit.Trabajador.EliminarInactivar_DatosTrabajador(1, cod_trabajador, cod_usuario, cod_contactemerg: Convert.ToInt32(txtCodContacto.Text), cod_empresa: cod_empresa);
                    if (result != "OK") { MessageBox.Show("Error al eliminar registro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (result == "OK") MessageBox.Show("Se procedió a eliminar el registro de manera satisfactoria.", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCodContacto.Text = "0";
                    txtApellPaternoContacto.Text = "";
                    txtApellMaternoContacto.Text = "";
                    txtNombreContacto.Text = "";
                    lkpParentescoContacto.EditValue = null;
                    glkpTipoDocumentoContacto.EditValue = "DI001";
                    txtNroDocumentoContacto.Text = "";
                    dtFecNacimientoContacto.EditValue = null;
                    txtTelefonoContacto.Text = "";
                    txtCelularContacto.Text = "";
                    txtApellPaternoContacto.Select();
                    sbtnVerDocumentos.Enabled = false;
                    ObtenerDatos_ContactosEmergencia();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEliminarInfoLaboral_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("¿Esta seguro de eliminar el Registro?" + Environment.NewLine + "Esta acción es irreversible.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string result = "";
                    result = unit.Trabajador.EliminarInactivar_DatosTrabajador(2, cod_trabajador, cod_usuario_registro: cod_usuario, cod_infolab: Convert.ToInt32(txtCodInfoLaboral.Text), cod_empresa: lkpEmpresaInfoLaboral.EditValue.ToString());
                    if (result != "OK") { MessageBox.Show("Error al eliminar registro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (result == "OK") MessageBox.Show("Se procedió a eliminar el registro de manera satisfactoria.", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCodInfoLaboral.Text = "0";
                    lkpAreaInfoLaboral.EditValue = null;
                    lkpCargoInfoLaboral.EditValue = null;
                    lkpPrefCECOInfoLaboral.EditValue = null;
                    lkpTipoContratoInfoLaboral.EditValue = null;
                    dtFecFirmaInfoLaboral.EditValue = DateTime.Today;
                    dtFecVctoInfoLaboral.EditValue = DateTime.Today;
                    lkpModalidadInfoLaboral.EditValue = null;
                    txtMontoSueldoBaseInfoLaboral.EditValue = 0;
                    txtMontoAsigFamiliarInfoLaboral.EditValue = 0;
                    txtMontoMovilidadInfoLaboral.EditValue = 0;
                    txtMontoAlimentacionInfoLaboral.EditValue = 0;
                    txtMontoEscolaridadInfoLaboral.EditValue = 0;
                    txtMontoBonoInfoLaboral.EditValue = 0;
                    lkpSedeEmpresaInfoLaboral.EditValue = null;
                    glkpBancoInfoBancaria.EditValue = null;
                    lkpTipoMonedaInfoBancaria.EditValue = null;
                    lkpTipoCuentaInfoBancaria.EditValue = null;
                    txtNroCuentaBancariaInfoBancaria.Text = "";
                    txtNroCuentaCCIInfoBancaria.Text = "";
                    cbxJornadaMaxima.EditValue = "NO";
                    cbxSindicato.EditValue = "NO";
                    cbxRegimenAtipico.EditValue = "NO";
                    lkpHorarioNocturno.EditValue = "NO";
                    lkpexonerado5.EditValue = "NO";
                    lkpAfectovidaLey.EditValue = "NO";
                    lkpafectosctr.EditValue = "NO";
                    lkphorasextras.EditValue = "NO";
                    lkpSedeEmpresaInfoLaboral.Enabled = true;
                    lkpAreaInfoLaboral.Enabled = true;
                    lkpTipoTrabajador.Enabled = true;
                    lkpSituacionTrabajador_2.Enabled = true;
                    lkpsituacionespecial.Enabled = true;
                    lkpCargoInfoLaboral.Enabled = true;
                    lkpPrefCECOInfoLaboral.Enabled = true;
                    lkpTipoTrabajador.EditValue = "";
                    lkpsituacionespecial.EditValue = "";
                    lkpOcupacional.EditValue = "";
                    lkpCategoria.EditValue = "";
                    chkasignacionFamiliar.Checked = false;

                    Cargarlistado_infolaboral();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEliminarInfoFamiliar_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("¿Esta seguro de eliminar el Registro?" + Environment.NewLine + "Esta acción es irreversible.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string result = "";
                    result = unit.Trabajador.EliminarInactivar_DatosTrabajador(3, cod_trabajador, Program.Sesion.Usuario.cod_usuario, cod_infofamiliar: Convert.ToInt32(txtCodInfoFamiliar.Text), cod_empresa: cod_empresa);
                    if (result != "OK") { MessageBox.Show("Error al eliminar registro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (result == "OK")
                    {

                        MessageBox.Show("Se procedió a eliminar el registro de manera satisfactoria.", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCodInfoFamiliar.Text = "0";
                        lkpParentescoInfoFamiliar.Text = null;
                        txtApellPaternoInfoFamiliar.Text = "";
                        txtApellMaternoInfoFamiliar.Text = "";
                        txtNombreInfoFamiliar.Text = "";
                        dtFecNacimientoInfoFamiliar.EditValue = null;
                        chkflgVivoInfoFamiliar.CheckState = CheckState.Checked;
                        glkpTipoDocumentoInfoFamiliar.EditValue = null;
                        txtNroDocumentoInfoFamiliar.Text = "";
                        txtEmailInfoFamiliar.Text = "";
                        txtTelefonoInfoFamiliar.Text = "";
                        txtCelularInfoFamiliar.Text = "";
                        txtProfesionInfoFamiliar.Text = "";
                        txtCentroLaboralInfoFamiliar.Text = "";
                        txtGradoInstruccionInfoFamiliar.Text = "";
                        txtOcupacionInfoFamiliar.Text = "";
                        txtDiscapacidadInfoFamiliar.Text = "";
                        txtDireccionInfoFamiliar.Text = "";
                        lkpPaisInfoFamiliar.EditValue = "00001";
                        lkpDepartamentoInfoFamiliar.EditValue = "00015";
                        lkpProvinciaInfoFamiliar.EditValue = "00128";
                        glkpDistritoInfoFamiliar.EditValue = "01251";
                        chkflgEnfermedadInfoFamiliar.CheckState = CheckState.Unchecked;
                        txtEnfermedadInfoFamiliar.Text = "";
                        chkflgAdiccionInfoFamiliar.CheckState = CheckState.Unchecked;
                        txtAdiccionInfoFamiliar.Text = "";
                        chkflgDependeEconomiaInfoFamiliar.CheckState = CheckState.Unchecked;
                        chckdireccionfamiliar.CheckState = CheckState.Unchecked;
                        txtDireccionInfoFamiliar.Enabled = true;
                        lkpParentescoInfoFamiliar.EditValue = null;
                        ObtenerDatos_InfoFamiliar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEliminarFormAcademic_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("¿Esta seguro de eliminar el Registro?" + Environment.NewLine + "Esta acción es irreversible.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string result = "";
                    result = unit.Trabajador.EliminarInactivar_DatosTrabajador(4, cod_trabajador, Program.Sesion.Usuario.cod_usuario, cod_infoacademica: Convert.ToInt32(txtCodFormAcademic.Text), cod_empresa: cod_empresa);
                    if (result != "OK") { MessageBox.Show("Error al eliminar registro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (result == "OK") MessageBox.Show("Se procedió a eliminar el registro de manera satisfactoria.", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCodFormAcademic.Text = "0";
                    hlinkAdjuntarCertificadoInfoAcademica.Enabled = false;
                    lkpNivelAcademicoFormAcademic.EditValue = null;
                    lkpCentroEstudiosFormAcademic.EditValue = null;
                    txtLugarFormAcademic.Text = "";
                    lkpCarreraCursoFormAcademic.EditValue = null;
                    txtGradoTituloFormAcademic.Text = "";
                    dtAnhoInicioFormAcademic.EditValue = DateTime.Today;
                    dtAnhoFinFormAcademic.EditValue = DateTime.Today;
                    txtPromedioAcademicoFormAcademic.Text = "";
                    ObtenerDatos_InfoAcademica();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEliminarExpProfesional_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("¿Esta seguro de eliminar el Registro?" + Environment.NewLine + "Esta acción es irreversible.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string result = "";
                    result = unit.Trabajador.EliminarInactivar_DatosTrabajador(5, cod_trabajador, Program.Sesion.Usuario.cod_usuario, cod_infoprofesional: Convert.ToInt32(txtCodExpProfesional.Text), cod_empresa: cod_empresa);
                    if (result != "OK") { MessageBox.Show("Error al eliminar registro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (result == "OK") MessageBox.Show("Se procedió a eliminar el registro de manera satisfactoria.", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nuevoinfoprofesional();
                    ObtenerDatos_InfoProfesional();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEliminarInfoEconomica_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("¿Esta seguro de eliminar el Registro?" + Environment.NewLine + "Esta acción es irreversible.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string result = "";
                    result = unit.Trabajador.EliminarInactivar_DatosTrabajador(6, cod_trabajador, Program.Sesion.Usuario.cod_usuario, cod_infoeconomica: Convert.ToInt32(txtCodInfoEconomica.Text), cod_empresa: cod_empresa);
                    if (result != "OK") { MessageBox.Show("Error al eliminar registro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (result == "OK") MessageBox.Show("Se procedió a eliminar el registro de manera satisfactoria.", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nuevoinfoeconomica();
                    ObtenerDatos_InfoEconomica();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnGuardarInfoBancaria_Click(object sender, EventArgs e)
        {
            if (lkpTipoPago.EditValue == null) { MessageBox.Show("Debe ingresar el tipo de Pago.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipoPago.Focus(); return; }
            if (lkpPeriodicidadPagos.Text.Trim() == "") { MessageBox.Show("Debe ingresar la Periodicidad de Pago.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpPeriodicidadPagos.Focus(); return; }
            if (glkpBancoInfoBancaria.EditValue == null) { MessageBox.Show("Debe seleccionar un banco.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); glkpBancoInfoBancaria.Focus(); return; }
            if (lkpTipoMonedaInfoBancaria.EditValue == null) { MessageBox.Show("Debe seleccionar un tipo de moneda.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipoMonedaInfoBancaria.Focus(); return; }
            if (lkpTipoCuentaInfoBancaria.EditValue == null) { MessageBox.Show("Debe seleccionar un tipo de cuenta.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipoCuentaInfoBancaria.Focus(); return; }
            if (txtNroCuentaBancariaInfoBancaria.Text.Trim() == "") { MessageBox.Show("Debe ingresar una cuenta bancaria.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNroCuentaBancariaInfoBancaria.Focus(); return; }
            if (txtNroCuentaCCIInfoBancaria.Text.Trim() == "") { MessageBox.Show("Debe ingresar una cuenta interbancaria.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNroCuentaCCIInfoBancaria.Focus(); return; }

            if (glkpBancoCTSInfoBancaria.EditValue == null) { MessageBox.Show("Debe seleccionar un banco.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); glkpBancoCTSInfoBancaria.Focus(); return; }
            if (lkpTipoMonedaCTSInfoBancaria.EditValue == null) { MessageBox.Show("Debe seleccionar un tipo de moneda.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipoMonedaCTSInfoBancaria.Focus(); return; }
            if (lkpTipoCuentaCTS.EditValue == null) { MessageBox.Show("Debe seleccionar un tipo de cuenta.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipoCuentaCTS.Focus(); return; }
            if (txtNroCuentaCCIInfoBancaria.Text.Trim() == "") { MessageBox.Show("Debe ingresar una cuenta bancaria.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNroCuentaCCIInfoBancaria.Focus(); return; }
            if (txtNroCuentaCCICTSInfoBancaria.Text.Trim() == "") { MessageBox.Show("Debe ingresar una cuenta interbancaria.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNroCuentaCCICTSInfoBancaria.Focus(); return; }
            if (lkpSistPensionarioInfoBancaria.EditValue == null) { MessageBox.Show("Debe seleccionar el Sistema Pensionario.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpSistPensionarioInfoBancaria.Focus(); return; }
            if (lkpNombreAFPInfoBancaria.Text.Trim() == "") { MessageBox.Show("Debe seleccionar un nombre AFP.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpNombreAFPInfoBancaria.Focus(); return; }
            if (lkpTipoComision.Text.Trim() == "") { MessageBox.Show("Debe seleccionar el tipo de Comisión de AFP.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipoComision.Focus(); return; }
            if (txtNroCUSPPInfoBancaria.Text.Trim() == "") { MessageBox.Show("Debe ingresar el CUSPP.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNroCUSPPInfoBancaria.Focus(); return; }


            eTrabajador.eInfoBancaria_Trabajador eBanc = new eTrabajador.eInfoBancaria_Trabajador();
            eBanc = AsignarValores_InfoBancaria();
            eBanc = unit.Trabajador.InsertarActualizar_InfoBancariaTrabajador<eTrabajador.eInfoBancaria_Trabajador>(eBanc);
            if (eBanc == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (eBanc != null)
            {
                ActualizarListado = "SI";
                MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ObtenerDatos_InfoBancaria();
            }
        }

        private eTrabajador.eInfoBancaria_Trabajador AsignarValores_InfoBancaria()
        {
            eTrabajador.eInfoBancaria_Trabajador obj = new eTrabajador.eInfoBancaria_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = lkpEmpresa.EditValue.ToString();
            obj.cod_banco = glkpBancoInfoBancaria.EditValue == null ? null : glkpBancoInfoBancaria.EditValue.ToString();
            obj.cod_moneda = lkpTipoMonedaInfoBancaria.EditValue == null ? null : lkpTipoMonedaInfoBancaria.EditValue.ToString();
            obj.cod_tipo_cuenta = lkpTipoCuentaInfoBancaria.EditValue == null ? null : lkpTipoCuentaInfoBancaria.EditValue.ToString();
            obj.dsc_cta_bancaria = txtNroCuentaBancariaInfoBancaria.Text;
            obj.dsc_cta_interbancaria = txtNroCuentaCCIInfoBancaria.Text;
            obj.cod_banco_CTS = glkpBancoCTSInfoBancaria.EditValue == null ? null : glkpBancoCTSInfoBancaria.EditValue.ToString();
            obj.cod_moneda_CTS = lkpTipoMonedaCTSInfoBancaria.EditValue == null ? null : lkpTipoMonedaCTSInfoBancaria.EditValue.ToString();
            obj.dsc_cta_bancaria_CTS = txtNroCuentaBancariaCTSInfoBancaria.Text;
            obj.dsc_cta_interbancaria_CTS = txtNroCuentaCCICTSInfoBancaria.Text;
            obj.cod_sist_pension = lkpSistPensionarioInfoBancaria.EditValue == null ? null : lkpSistPensionarioInfoBancaria.EditValue.ToString();
            obj.cod_APF = lkpNombreAFPInfoBancaria.EditValue == null ? null : lkpNombreAFPInfoBancaria.EditValue.ToString();
            obj.cod_CUSPP = lkpNombreAFPInfoBancaria.EditValue == null ? null : txtNroCUSPPInfoBancaria.Text;
            obj.cod_periodicidad_pagos = lkpPeriodicidadPagos.EditValue == null ? null : lkpPeriodicidadPagos.EditValue.ToString();
            obj.cod_tipo_pago = lkpTipoPago.EditValue == null ? null : lkpTipoPago.EditValue.ToString();
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            obj.cod_tipo_comision_pension = lkpTipoComision.EditValue == null ? null : lkpTipoComision.EditValue.ToString();
            obj.cod_tipo_cuenta_CTS = lkpTipoCuentaCTS.EditValue == null ? null : lkpTipoCuentaCTS.EditValue.ToString();


            return obj;
        }

        private void picAnteriorTrabajador_Click(object sender, EventArgs e)
        {
            try
            {
                int tRow = frmHandler.gvListadoTrabajador.RowCount - 1;
                int nRow = frmHandler.gvListadoTrabajador.FocusedRowHandle;
                frmHandler.gvListadoTrabajador.FocusedRowHandle = nRow == 0 ? tRow : nRow - 1;

                btnNuevo_ItemClick(btnNuevo, new DevExpress.XtraBars.ItemClickEventArgs(null, null));
                eTrabajador obj = frmHandler.gvListadoTrabajador.GetFocusedRow() as eTrabajador;
                cod_trabajador = obj.cod_trabajador;
                MiAccion = Trabajador.Editar;
                Editar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void picSiguienteTrabajador_Click(object sender, EventArgs e)
        {
            try
            {
                int tRow = frmHandler.gvListadoTrabajador.RowCount - 1;
                int nRow = frmHandler.gvListadoTrabajador.FocusedRowHandle;
                frmHandler.gvListadoTrabajador.FocusedRowHandle = nRow == tRow ? 0 : nRow + 1;

                btnNuevo_ItemClick(btnNuevo, new DevExpress.XtraBars.ItemClickEventArgs(null, null));
                eTrabajador obj = frmHandler.gvListadoTrabajador.GetFocusedRow() as eTrabajador;
                cod_trabajador = obj.cod_trabajador;
                MiAccion = Trabajador.Editar;
                Editar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkflgPeriodoPruebaInfoLaboral_CheckedChanged(object sender, EventArgs e)
        {
            //lkpTiempoPeriodoInfoLaboral.ReadOnly = chkflgPeriodoPruebaInfoLaboral.CheckState == CheckState.Checked ? false : true;
        }

        private void gvListadoFormAcademica_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    eTrabajador.eInfoAcademica_Trabajador obj = gvListadoFormAcademica.GetRow(e.RowHandle) as eTrabajador.eInfoAcademica_Trabajador;
                    if (e.Column.FieldName == "flg_certificado" && obj.flg_certificado == "SI")
                    {
                        e.Handled = true; e.Graphics.DrawImage(ImgPDF, new Rectangle(e.Bounds.X + (e.Bounds.Width / 2) - 8, e.Bounds.Y + (e.Bounds.Height / 2) - 8, 16, 16));
                    }
                    if (e.Column.FieldName == "flg_certificado" && obj.flg_certificado == "NO") e.DisplayText = "";
                    e.DefaultDraw();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gvListadoExpProfesional_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    eTrabajador.eInfoProfesional_Trabajador obj = gvListadoExpProfesional.GetRow(e.RowHandle) as eTrabajador.eInfoProfesional_Trabajador;
                    if (e.Column.FieldName == "flg_certificado" && obj.flg_certificado == "SI")
                    {
                        e.Handled = true; e.Graphics.DrawImage(ImgPDF, new Rectangle(e.Bounds.X + (e.Bounds.Width / 2) - 8, e.Bounds.Y + (e.Bounds.Height / 2) - 8, 16, 16));
                    }
                    if (e.Column.FieldName == "flg_certificado" && obj.flg_certificado == "NO") e.DisplayText = "";
                    e.DefaultDraw();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void gvListadoFormAcademica_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                eTrabajador.eInfoAcademica_Trabajador obj = new eTrabajador.eInfoAcademica_Trabajador();

                if (e.Clicks == 2 && e.Column.FieldName == "flg_certificado")
                {
                    obj = gvListadoFormAcademica.GetFocusedRow() as eTrabajador.eInfoAcademica_Trabajador;
                    if (obj == null) { return; }

                    if (obj.flg_certificado == "NO")
                    {
                        MessageBox.Show("No se cargado ningún PDF", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    //else
                    //{
                    eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, cod_empresa);
                    if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
                    { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    //var app = App.PublicClientApp;
                    ClientId = eEmp.ClientIdOnedrive;
                    TenantId = eEmp.TenantOnedrive;
                    Appl();
                    var app = PublicClientApp;

                    try
                    {
                        unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Abriendo documento", "Cargando...");
                        //eEmpresa eEmp = unit.Factura.ObtenerDatosEmpresa<eEmpresa>(12, obj.cod_empresa);
                        string correo = eEmp.UsuarioOnedrivePersonal;
                        string password = eEmp.ClaveOnedrivePersonal;

                        var securePassword = new SecureString();
                        foreach (char c in password)
                            securePassword.AppendChar(c);

                        authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

                        GraphClient = new Microsoft.Graph.GraphServiceClient(
                        new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                        {
                            requestMessage
                                .Headers
                                .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                            return Task.FromResult(0);
                        }));

                        string IdOneDriveDoc = obj.id_certificado;
                        string Extension = ".pdf";

                        var fileContent = await GraphClient.Me.Drive.Items[IdOneDriveDoc].Content.Request().GetAsync();
                        string ruta = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()) + @"\" + ("Cert. Academico_" + txtNroDocumento.Text + txtCodFormAcademic.Text);
                        if (!System.IO.File.Exists(ruta))
                        {
                            using (var fileStream = new FileStream(ruta, FileMode.Create, System.IO.FileAccess.Write))
                                fileContent.CopyTo(fileStream);
                        }

                        if (!System.IO.Directory.Exists(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()))) System.IO.Directory.CreateDirectory(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()));
                        System.Diagnostics.Process.Start(ruta);
                        SplashScreenManager.CloseForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hubieron problemas al autenticar las credenciales", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //lblResultado.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                        return;
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void gvListadoExpProfesional_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                eTrabajador.eInfoProfesional_Trabajador obj = new eTrabajador.eInfoProfesional_Trabajador();

                if (e.Clicks == 2 && e.Column.FieldName == "flg_certificado")
                {
                    obj = gvListadoExpProfesional.GetFocusedRow() as eTrabajador.eInfoProfesional_Trabajador;
                    if (obj == null) { return; }

                    if (obj.flg_certificado == "NO")
                    {
                        MessageBox.Show("No se cargado ningún PDF", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    //else
                    //{
                    eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, cod_empresa);
                    if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
                    { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    //var app = App.PublicClientApp;
                    ClientId = eEmp.ClientIdOnedrive;
                    TenantId = eEmp.TenantOnedrive;
                    Appl();
                    var app = PublicClientApp;

                    try
                    {
                        unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Abriendo documento", "Cargando...");
                        //eEmpresa eEmp = unit.Factura.ObtenerDatosEmpresa<eEmpresa>(12, obj.cod_empresa);
                        string correo = eEmp.UsuarioOnedrivePersonal;
                        string password = eEmp.ClaveOnedrivePersonal;

                        var securePassword = new SecureString();
                        foreach (char c in password)
                            securePassword.AppendChar(c);

                        authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

                        GraphClient = new Microsoft.Graph.GraphServiceClient(
                        new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                        {
                            requestMessage
                                .Headers
                                .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                            return Task.FromResult(0);
                        }));

                        string IdOneDriveDoc = obj.id_certificado;
                        string Extension = ".pdf";

                        var fileContent = await GraphClient.Me.Drive.Items[IdOneDriveDoc].Content.Request().GetAsync();
                        string ruta = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()) + @"\" + ("Cert. Profesional_" + txtNroDocumento.Text + txtCodExpProfesional.Text + Extension);
                        if (!System.IO.File.Exists(ruta))
                        {
                            using (var fileStream = new FileStream(ruta, FileMode.Create, System.IO.FileAccess.Write))
                                fileContent.CopyTo(fileStream);
                        }

                        if (!System.IO.Directory.Exists(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()))) System.IO.Directory.CreateDirectory(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()));
                        System.Diagnostics.Process.Start(ruta);
                        SplashScreenManager.CloseForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hubieron problemas al autenticar las credenciales", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //lblResultado.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                        return;
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvListadoInfoEconomica_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvListadoInfoEconomica_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvListadoInfoEconomica_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0) gvListadoInfoEconomica_FocusedRowChanged(gvListadoInfoEconomica, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, e.RowHandle));
        }

        private void gvListadoInfoEconomica_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0) Obtener_InfoEconomica();
        }

        private void Obtener_InfoEconomica()
        {
            eTrabajador.eInfoEconomica_Trabajador obj = new eTrabajador.eInfoEconomica_Trabajador();
            obj = gvListadoInfoEconomica.GetFocusedRow() as eTrabajador.eInfoEconomica_Trabajador;
            if (obj == null) return;
            txtCodInfoEconomica.Text = obj.cod_infoeconomica.ToString(); ;
            txtIngresoMensualInfoEconomica.EditValue = obj.imp_ingresomensual;
            txtGastoMensualInfoEconomica.EditValue = obj.imp_gastomensual;
            txtValorActivoInfoEconomica.EditValue = obj.imp_totalactivos;
            txtValorDeudaInfoEconomica.EditValue = obj.imp_totaldeudas;
            lkpViviendaInfoEconomica.EditValue = obj.cod_vivienda;
            lkpTipoViviendaInfoEconomica.EditValue = obj.cod_tipovivienda;
            txtValorRentaInfoEconomica.EditValue = obj.imp_valorvivienda;
            lkpTipoMonedaViviendaInfoEconomica.EditValue = obj.cod_monedavivienda;
            txtDireccionViviendaInfoEconomica.Text = obj.dsc_direccion_vivienda;
            txtReferenciaViviendaInfoEconomica.Text = obj.dsc_referencia_vivienda;
            lkpPaisViviendaInfoEconomica.EditValue = obj.cod_pais_vivienda;
            lkpDepartamentoViviendaInfoEconomica.EditValue = obj.cod_departamento_vivienda;
            lkpProvinciaViviendaInfoEconomica.EditValue = obj.cod_provincia_vivienda;
            glkpDistritoViviendaInfoEconomica.EditValue = obj.cod_distrito_vivienda;
            lkpVehiculoInfoEconomica.EditValue = obj.cod_tipovehiculo;
            txtMarcaInfoEconomica.Text = obj.dsc_marcavehiculo;
            txtModeloInfoEconomica.Text = obj.dsc_modelovehiculo;
            txtPlacaVehiculoInfoEconomica.Text = obj.dsc_placavehiculo;
            txtValorComercialInfoEconomica.EditValue = obj.imp_valorvehiculo;
            lkpTipoMonedaVehiculoInfoEconomica.EditValue = obj.cod_monedavehiculo;
            glkpTipoDocumentoeconomica.EditValue = obj.cod_tipoempresa;
            txtParticipacionInfoEconomica.Text = obj.dsc_participacion_empresa;
            txtRUCEmpresaInfoEconomica.Text = obj.dsc_RUC_empresa;
            txtGiroEmpresaInfoEconomica.Text = obj.dsc_giro_empresa;
            txtDireccionEmpresaInfoEconomica.Text = obj.dsc_direccion_empresa;
            lkpPaisEmpresaInfoEconomica.EditValue = obj.cod_pais_empresa;
            lkpDepartamentoEmpresaInfoEconomica.EditValue = obj.cod_departamento_empresa;
            lkpProvinciaEmpresaInfoEconomica.EditValue = obj.cod_provincia_empresa;
            glkpDistritoEmpresaInfoEconomica.EditValue = obj.cod_distrito_empresa;
        }

        private void gvHistoriaEMO_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvHistoriaEMO_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }



        private async void gvHistoriaEMO_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //    try
            //    {
            //        eTrabajador.eCertificadoEMO_Trabajador obj = new eTrabajador.eCertificadoEMO_Trabajador();
            //        if (e.Clicks == 2 && e.Column.FieldName == "flg_certificado")
            //        {
            //            obj = gvHistoriaEMO.GetFocusedRow() as eTrabajador.eCertificadoEMO_Trabajador;
            //            if (obj == null) { return; }

            //            if (obj.flg_certificado == "NO")
            //            {
            //                MessageBox.Show("No se cargado ningún PDF", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //                return;
            //            }
            //            //else
            //            //{
            //            eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, cod_empresa);
            //            if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
            //            { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            //            //var app = App.PublicClientApp;
            //            ClientId = eEmp.ClientIdOnedrive;
            //            TenantId = eEmp.TenantOnedrive;
            //            Appl();
            //            var app = PublicClientApp;

            //            try
            //            {
            //                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Abriendo documento", "Cargando...");
            //                //eEmpresa eEmp = unit.Factura.ObtenerDatosEmpresa<eEmpresa>(12, obj.cod_empresa);
            //                string correo = eEmp.UsuarioOnedrivePersonal;
            //                string password = eEmp.ClaveOnedrivePersonal;

            //                var securePassword = new SecureString();
            //                foreach (char c in password)
            //                    securePassword.AppendChar(c);

            //                authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

            //                GraphClient = new Microsoft.Graph.GraphServiceClient(
            //                new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
            //                {
            //                    requestMessage
            //                        .Headers
            //                        .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
            //                    return Task.FromResult(0);
            //                }));

            //                string IdOneDriveDoc = obj.id_certificado;
            //                string Extension = ".pdf";

            //                var fileContent = await GraphClient.Me.Drive.Items[IdOneDriveDoc].Content.Request().GetAsync();
            //                string ruta = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()) + @"\" + ("EMO_" + obj.fch_registro.Year.ToString() + "." + obj.fch_registro.ToString("MM") + "." + obj.fch_registro.ToString("dd") + Extension);
            //                if (!System.IO.File.Exists(ruta))
            //                {
            //                    using (var fileStream = new FileStream(ruta, FileMode.Create, System.IO.FileAccess.Write))
            //                        fileContent.CopyTo(fileStream);
            //                }

            //                if (!System.IO.Directory.Exists(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()))) System.IO.Directory.CreateDirectory(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()));
            //                System.Diagnostics.Process.Start(ruta);
            //                SplashScreenManager.CloseForm();
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show("Hubieron problemas al autenticar las credenciales", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                //lblResultado.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
            //                return;
            //            }
            //            //}
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
        }

        private void chkflgEnfermedadInfoFamiliar_CheckStateChanged(object sender, EventArgs e)
        {
            layoutControlItem298.Enabled = chkflgEnfermedadInfoFamiliar.CheckState == CheckState.Checked ? true : false;
        }

        private void lkpPrefCECOInfoLaboral_EditValueChanged(object sender, EventArgs e)
        {
            lkpPrefCECOInfoLaboral.ToolTip = lkpPrefCECOInfoLaboral.EditValue == null ? "" : lkpPrefCECOInfoLaboral.Text;
        }

        private void gvEMO_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {

                if (e.RowHandle >= 0)
                {
                    eTrabajador.eEMO obj1 = gvEMO.GetRow(e.RowHandle) as eTrabajador.eEMO;
                    if (e.Column.FieldName == "flg_certificado" && obj1.flg_certificado == "SI")
                    {
                        e.Handled = true; e.Graphics.DrawImage(ImgPDF, new Rectangle(e.Bounds.X + (e.Bounds.Width / 2) - 8, e.Bounds.Y + (e.Bounds.Height / 2) - 8, 16, 16));
                    }
                    if (e.Column.FieldName == "flg_certificado" && obj1.flg_certificado == "NO") e.DisplayText = "";
                    e.DefaultDraw();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void gvEMO_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private async void gvEMO_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                eTrabajador.eEMO obj = new eTrabajador.eEMO();
                eTrabajador.eEMO er = gvEMO.GetFocusedRow() as eTrabajador.eEMO;

                if (e.Clicks == 2 && e.Column.FieldName == "flg_certificado")
                {
                    obj = gvEMO.GetFocusedRow() as eTrabajador.eEMO;
                    if (obj == null) { return; }

                    if (obj.flg_certificado == "NO")
                    {
                        MessageBox.Show("No se cargado ningún PDF", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    //else
                    //{
                    eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, cod_empresa);
                    if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
                    { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    //var app = App.PublicClientApp;
                    ClientId = eEmp.ClientIdOnedrive;
                    TenantId = eEmp.TenantOnedrive;
                    Appl();
                    var app = PublicClientApp;

                    try
                    {
                        unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Abriendo documento", "Cargando...");
                        //eEmpresa eEmp = unit.Factura.ObtenerDatosEmpresa<eEmpresa>(12, obj.cod_empresa);
                        string correo = eEmp.UsuarioOnedrivePersonal;
                        string password = eEmp.ClaveOnedrivePersonal;

                        var securePassword = new SecureString();
                        foreach (char c in password)
                            securePassword.AppendChar(c);

                        authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

                        GraphClient = new Microsoft.Graph.GraphServiceClient(
                        new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                        {
                            requestMessage
                                .Headers
                                .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                            return Task.FromResult(0);
                        }));

                        string IdOneDriveDoc = obj.id_certificado;
                        string Extension = ".pdf";

                        var fileContent = await GraphClient.Me.Drive.Items[IdOneDriveDoc].Content.Request().GetAsync();
                        string ruta = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()) + @"\" + ("Certificado" + txtNroDocumento.Text + "_" + er.cod_EMO, Extension);
                        if (!System.IO.File.Exists(ruta))
                        {
                            using (var fileStream = new FileStream(ruta, FileMode.Create, System.IO.FileAccess.Write))
                                fileContent.CopyTo(fileStream);
                        }

                        if (!System.IO.Directory.Exists(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()))) System.IO.Directory.CreateDirectory(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()));
                        System.Diagnostics.Process.Start(ruta);
                        SplashScreenManager.CloseForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hubieron problemas al autenticar las credenciales", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //lblResultado.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                        return;
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void btnAdjuntarEMO_Click(object sender, EventArgs e)
        {
            List<eVentana> listPerfil = unit.Sistema.ListarPerfilesUsuario<eVentana>(4, Program.Sesion.Usuario.cod_usuario, Program.Sesion.Global.Solucion);
            eVentana oPerfil = listPerfil.Find(x => x.cod_perfil == 13 || x.cod_perfil == 14);
            string tipo_doc = lkpTipodoc.Text.ToString();
            Text = btnGuardarEMO.Text;
            if (oPerfil == null)
            {
                if (tipo_doc== "CERTIFICADO EMO") { MessageBox.Show("Acceso denegado para registrar EMO", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipodoc.Focus(); return; }
                else
                {
                    GUARDARADJUNTARDOCVARIOS(Text);
                }
            }
            else
            {
                GUARDARADJUNTARDOCVARIOS(Text);
            }

        }

        private async void GUARDARADJUNTARDOCVARIOS(string nombreboton)
        {

            if (nombreboton == "GUARDAR")
            {

                string tipo_doc = lkpTipodoc.EditValue.ToString();

                eTrabdoc = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(109, cod_documento: tipo_doc);
                await AdjuntarDocumentosVarios(8, nombreDoc: tipo_doc, nombreDocAdicional: eTrabdoc.dsc_abreviatura);

                eTrabajador.eEMO obj = AsignarValor_EMO();
                obj = unit.Trabajador.InsertarActualizar_EMOTrabajador<eTrabajador.eEMO>(obj, opcion: 1); ObtenerDatos_EMO();
                lkpTipodoc.ReadOnly = true;
            }
            else if (nombreboton == "EDITAR")
            {

                try
                {
                    eTrabajador.eEMO er = gvEMO.GetFocusedRow() as eTrabajador.eEMO;
                    if (er.dsc_descripcion == "")
                    {
                        string result = "";
                        string tipo_doc = lkpTipodoc.EditValue.ToString();
                        result = unit.Trabajador.EliminarInactivarEMOtrabajador(9, cod_trabajador: cod_trabajador, cod_empresa: cod_empresa, cod_EMO: er.cod_EMO, cod_documento: er.cod_documento);
                        gvEMO.RefreshData();
                        eTrabdoc = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(109, cod_documento: tipo_doc);
                        await AdjuntarDocumentosVarios(8, nombreDoc: tipo_doc, nombreDocAdicional: eTrabdoc.dsc_abreviatura, cod: er.cod_EMO);
                        eTrabajador.eEMO obj = AsignarValor_EMO();
                        obj = unit.Trabajador.InsertarActualizar_EMOTrabajador<eTrabajador.eEMO>(obj, opcion: 1);
                        if (obj != null)
                        {
                            MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ObtenerDatos_EMO(); lkpTipodoc.ReadOnly = true;
                        }
                    }
                    else if (MessageBox.Show("¿Desea remplazar Documento?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string result = "";
                        await Mover_Eliminar_ArchivoOneDrive();
                        result = unit.Trabajador.EliminarInactivarEMOtrabajador(9, cod_trabajador: cod_trabajador, cod_empresa: cod_empresa, cod_EMO: er.cod_EMO, cod_documento: er.cod_documento);
                        gvEMO.RefreshData();
                        string tipo_doc = lkpTipodoc.EditValue.ToString();
                        eTrabdoc = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(109, cod_documento: tipo_doc);
                        await AdjuntarDocumentosVarios(8, nombreDoc: tipo_doc, nombreDocAdicional: eTrabdoc.dsc_abreviatura, cod: er.cod_EMO);
                        eTrabajador.eEMO obj = AsignarValor_EMO();
                        obj = unit.Trabajador.InsertarActualizar_EMOTrabajador<eTrabajador.eEMO>(obj, opcion: 1);
                        ObtenerDatos_EMO(); gvEMO.RefreshData();

                        if (obj != null)
                        {
                            MessageBox.Show("Se procedió a remplazar el archivo PDF con EXITO", "Remplazo Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }


        }

        private void btnNuevo_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string resultado = "";
            string opcion = "1";
            frmMensaje mensaje = new frmMensaje();
            mensaje.mensaje = "INGRESE EL NÚMERO DE";
            mensaje.txtdni.Visible = true;
            mensaje.opcion = "1";
            mensaje.btnGuardar.Text = "VALIDAR";
            mensaje.ShowDialog();
            resultado = mensaje.resultado;


            if (resultado == "SI" || resultado == "")
            {
                //this.Close();
            }
            else
            {

                try
                {
                    MiAccion = Trabajador.Nuevo;
                    btnNuevo.Enabled = false;
                    //acctlMenu.Enabled = false;
                    cod_trabajador = "";
                    txtCodTrabajador.Text = "";
                    txtApellPaterno.Text = "";
                    txtApellMaterno.Text = "";
                    txtNombre.Text = "";
                    dtFecNacimiento.EditValue = null;
                    lkpEstadoCivil.EditValue = "01";
                    lkpEmpresa.EditValue = null;
                    glkpTipoDocumento.EditValue = mensaje.tipo_documento;
                    txtNroDocumento.Text = mensaje.dni;
                    //txtNroDocumento.Text = "";
                    dtFecVctoDocumento.EditValue = null;
                    txtNroUbigeoDocumento.Text = "";
                    chkFlgDNI.CheckState = CheckState.Unchecked;
                    chkFlgCV.CheckState = CheckState.Unchecked;
                    chkFlgVerifDomiciliaria.CheckState = CheckState.Unchecked;
                    chkFlgAntPoliciales.CheckState = CheckState.Unchecked;
                    chkFlgAntPenales.CheckState = CheckState.Unchecked;
                    lkpSistPensionarioInfoBancaria.EditValue = null;
                    lkpNombreAFPInfoBancaria.EditValue = null;
                    txtNroCUSPPInfoBancaria.Text = "";
                    txtDireccion.Text = "";
                    txtReferencia.Text = "";
                    lkpPais.EditValue = "00001"; lkpDepartamento.EditValue = "00015"; lkpProvincia.EditValue = "00128"; lkpNacionalidad.EditValue = "00001";
                    lkpDistrito.EditValue = "";
                    txtTelefono.Text = "";
                    txtCelular.Text = "";
                    txtEmail1.Text = "";
                    txtEmail2.Text = "";
                    ListHistInfoLaboral.Clear(); ListContactos.Clear(); ListInfoLaboral.Clear();
                    ListInfoFamiliar.Clear(); ListInfoAcademica.Clear(); ListInfoProfesional.Clear();
                    gvInfoLaboral.RefreshData(); gvListadoContactos.RefreshData(); gvListadoInfoLaboral.RefreshData();
                    gvListadoInfoFamiliar.RefreshData(); gvListadoFormAcademica.RefreshData(); gvListadoExpProfesional.RefreshData();
                    btnNuevoContacto_Click(null, new EventArgs());
                    btnNuevaInfoLaboral_Click(null, new EventArgs());
                    btnNuevaInfoFamiliar_Click(null, new EventArgs());
                    btnNuevaFormAcademic_Click(null, new EventArgs());
                    btnNuevaInfoEconomica_Click(null, new EventArgs());
                    btnNuevaExpProfesional_Click(null, new EventArgs());
                    chkflgAlergiasInfoSalud.CheckState = CheckState.Unchecked;
                    mmAlergias.Text = "";
                    chkflgOperacionesInfoSalud.CheckState = CheckState.Unchecked;
                    mmOperaciones.Text = "";
                    chkflgEnfPrexistenteInfoSalud.CheckState = CheckState.Unchecked;
                    mmEnfPrexistente.Text = "";
                    chkflgTratamientoInfoSalud.CheckState = CheckState.Unchecked;
                    mmTratamiento.Text = "";
                    chkflgEnfActualInfoSalud.CheckState = CheckState.Unchecked;
                    mmEnfActualidad.Text = "";
                    chkflgTratActualInfoSalud.CheckState = CheckState.Unchecked;
                    mmTratActual.Text = "";
                    chkflgDiscapacidadInfoSalud.CheckState = CheckState.Unchecked;
                    mmDiscapacidad.Text = "";
                    chkflgOtrosInfoSalud.CheckState = CheckState.Unchecked;
                    mmOtros.Text = "";
                    lkpGrupoSanguineoInfoSalud.EditValue = null;
                    lkpEstadoSaludInfoSalud.EditValue = null;
                    lkpSeguroSaludInfoSalud.EditValue = null;
                    glkpBancoInfoBancaria.EditValue = null;
                    lkpTipoMonedaInfoBancaria.EditValue = null;
                    lkpTipoCuentaInfoBancaria.EditValue = null;
                    txtNroCuentaBancariaInfoBancaria.Text = "";
                    txtNroCuentaCCIInfoBancaria.Text = "";
                    glkpBancoCTSInfoBancaria.EditValue = null;
                    lkpTipoMonedaCTSInfoBancaria.EditValue = null;
                    txtNroCuentaBancariaCTSInfoBancaria.Text = "";
                    txtNroCuentaCCICTSInfoBancaria.Text = "";
                    lkpSistPensionarioInfoBancaria.EditValue = null;
                    lkpNombreAFPInfoBancaria.EditValue = null;
                    txtNroCUSPPInfoBancaria.Text = "";
                    txtEstaturaCaractFisica.EditValue = 0;
                    txtPesoCaractFisica.EditValue = 0;
                    txtIMCCaractFisica.EditValue = 0;
                    chkflgLentesCaractFisica.CheckState = CheckState.Unchecked;
                    lkpPoloTallaUnif.EditValue = null;
                    lkpCamisaTallaUnif.EditValue = null;
                    lkpPantalonTallaUnif.EditValue = null;
                    lkpCasacaTallaUnif.EditValue = null;
                    lkpMamelucoTallaUnif.EditValue = null;
                    lkpChalecoTallaUnif.EditValue = null;
                    txtCalzadoTallaUnif.EditValue = null;
                    txtIngresoMensualInfoEconomica.EditValue = 0;
                    txtGastoMensualInfoEconomica.EditValue = 0;
                    txtValorActivoInfoEconomica.EditValue = 0;
                    txtValorDeudaInfoEconomica.EditValue = 0;
                    lkpViviendaInfoEconomica.EditValue = null;
                    lkpTipoViviendaInfoEconomica.EditValue = null;
                    txtValorRentaInfoEconomica.EditValue = 0;
                    txtDireccionViviendaInfoEconomica.Text = "";
                    txtReferenciaViviendaInfoEconomica.Text = "";
                    lkpPaisViviendaInfoEconomica.EditValue = null;
                    lkpDepartamentoViviendaInfoEconomica.EditValue = null;
                    lkpProvinciaViviendaInfoEconomica.EditValue = null;
                    glkpDistritoViviendaInfoEconomica.EditValue = null;
                    lkpVehiculoInfoEconomica.EditValue = null;
                    txtMarcaInfoEconomica.Text = "";
                    txtModeloInfoEconomica.Text = "";
                    txtPlacaVehiculoInfoEconomica.Text = "";
                    txtValorComercialInfoEconomica.EditValue = 0;
                    glkpTipoDocumentoeconomica.EditValue = null;
                    txtParticipacionInfoEconomica.Text = "";
                    txtRUCEmpresaInfoEconomica.Text = "";
                    txtGiroEmpresaInfoEconomica.Text = "";
                    txtDireccionEmpresaInfoEconomica.Text = "";
                    lkpPaisEmpresaInfoEconomica.EditValue = null;
                    lkpDepartamentoEmpresaInfoEconomica.EditValue = null;
                    lkpProvinciaEmpresaInfoEconomica.EditValue = null;
                    glkpDistritoEmpresaInfoEconomica.EditValue = null;
                    lkpViviendaInfoVivienda.EditValue = null;
                    lkpComodidadInfoVivienda.EditValue = null;
                    chkflgPuertasInfoVivienda.CheckState = CheckState.Unchecked;
                    chkflgVentanasInfoVivienda.CheckState = CheckState.Unchecked;
                    chkflgTechoInfoVivienda.CheckState = CheckState.Unchecked;
                    chkflgTelefonoInfoVivienda.CheckState = CheckState.Unchecked;
                    chkCelularesInfoVivienda.CheckState = CheckState.Unchecked;
                    chkflgInternetComunicacionInfoVivienda.CheckState = CheckState.Unchecked;
                    chkflgLuzInfoVivienda.CheckState = CheckState.Unchecked;
                    chkflgAguaInfoVivienda.CheckState = CheckState.Unchecked;
                    chkflgDesagueInfoVivienda.CheckState = CheckState.Unchecked;
                    chkflgGasInfoVivienda.CheckState = CheckState.Unchecked;
                    chkflgCableInfoVivienda.CheckState = CheckState.Unchecked;
                    chkflgInternetServicioInfoVivienda.CheckState = CheckState.Unchecked;
                    mmViasAccesoInfoVivienda.Text = "";
                    mmIluminacionInfoVivienda.Text = "";
                    mmEntornoInfoVivienda.Text = "";
                    mmPuestoPolicialInfoVivienda.Text = "";
                    txtNombreFamiliarInfoVivienda.Text = "";
                    txtHorasCasaInfoVivienda.Text = "";
                    lkpParentescoInfoVivienda.EditValue = null;
                    txtCelularInfoVivienda.Text = "";
                    txtEmailInfoVivienda.Text = "";
                    lkpPais.EditValue = "00001";
                    lkpDepartamento.EditValue = "00015";
                    lkpProvincia.EditValue = "00128";
                    lkpDistrito.EditValue = "";
                    lkptipovia.EditValue = "";
                    txtnombrevia.Text = "";
                    txtnro.Text = "";
                    txtdep.Text = "";
                    txtinterior.Text = "";
                    txtmz.Text = "";
                    txtlote.Text = "";
                    txtkm.Text = "";
                    txtetapa.Text = "";
                    txtblock.Text = "";
                    lkptipozona.EditValue = "";
                    txtnombrezona.Text = "";
                    txtDireccion.Text = "";


                    navframeTrabajador.SelectedPage = navpageGemeral;
                    txtApellPaterno.Select();
                    if (MiAccion != Trabajador.Editar)
                    {
                        picAnteriorTrabajador.Enabled = false; picSiguienteTrabajador.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void btnGuardar_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if(txtnro.Text=="" && txtdep.Text == "" && txtinterior.Text == "" && txtmz.Text == "" && txtlote.Text == "" && txtkm.Text == "" && txtetapa.Text == "" && txtblock.Text == "")
                { MessageBox.Show("Debe ingresar alguno de estos datos:" + "\r\n" + "- Nro de dirección" + "\r\n" + "- departamento" + "\r\n" + "- interior" + "\r\n" + "- Manzana" + "\r\n" + "- Lote" + "\r\n" + "- KM" + "\r\n" + "- etapa" + "\r\n" + "- Block", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtnro.Focus();  return; }
                txtApellPaterno.Select();
                if (lkpEmpresa.EditValue == null) { MessageBox.Show("Debe seleccionar una empresa.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpEmpresa.Focus(); return; }
                if (txtApellPaterno.Text.Trim() == "") { MessageBox.Show("Debe ingresar un apellido paterno.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtApellPaterno.Focus(); return; }
                if (txtApellMaterno.Text.Trim() == "") { MessageBox.Show("Debe ingresar un apellido materno.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtApellMaterno.Focus(); return; }
                if (txtNombre.Text.Trim() == "") { MessageBox.Show("Debe ingresar un nombre.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNombre.Focus(); return; }

                if (!ValidarFechaDeNacimiento()) return;

                if (dtFecNacimiento.EditValue == null) { MessageBox.Show("Debe ingresar la fecha de nacimiento", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtFecNacimiento.Focus(); return; }
                if (txtNroDocumento.Text.Trim() == "") { MessageBox.Show("Debe ingresar un número de documento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNroDocumento.Focus(); return; }
                if (lkpSexo.Text.Trim() == "") { MessageBox.Show("Debe ingresar Sexo.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpSexo.Focus(); return; }
                if (txtDireccion.Text.Trim() == "") { MessageBox.Show("Debe ingresar una dirección.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtDireccion.Focus(); return; }
                if (lkpPais.EditValue == null) { MessageBox.Show("Debe seleccionar un país.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpPais.Focus(); return; }
                if (lkpDepartamento.EditValue == null) { MessageBox.Show("Debe seleccionar un departamento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpDepartamento.Focus(); return; }
                if (lkpProvincia.EditValue == null) { MessageBox.Show("Debe seleccionar una provincia.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpProvincia.Focus(); return; }
                if (lkpDistrito.EditValue == null) { MessageBox.Show("Debe seleccionar un distrito.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpDistrito.Focus(); return; }
                if (lkpNacionalidad.EditValue == null) { MessageBox.Show("Debe seleccionar la nacionalidad.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpNacionalidad.Focus(); return; }
                if (txtEmail1.EditValue == null) { MessageBox.Show("Debe ingresar un Correo Electronico para el envio de Planillas.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtEmail1.Focus(); return; }
                if (txtCelular.Text.Trim() == "") { MessageBox.Show("Debe ingresar número de celular.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCelular.Focus(); return; }
                if (dtRenovacionUnif.EditValue == null && chkfchRenovacionuniforme.Checked == true) { MessageBox.Show("Debe ingresar Fecha de Renovación de Uniforme.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); chkfchRenovacionuniforme.Focus(); return; }
                if (dtEntregaUnif.EditValue == null && chkfchentregauniforme.Checked == true) { MessageBox.Show("Debe ingresar Fecha de Entrega de Uniforme.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); chkfchentregauniforme.Focus(); return; }
                
                if (txtNroDocumento.EditValue != null)
                {
                    eProveedor obj = new eProveedor();
                    obj = unit.Proveedores.Validar_NroDocumento<eProveedor>(19, "", glkpTipoDocumento.EditValue.ToString());
                    int nrodocumento = Convert.ToInt32(txtNroDocumento.Text.Length);
                    if (nrodocumento < obj.ctd_digitos) { MessageBox.Show("Los digitos del documento debe ser igual a " + obj.ctd_digitos, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCelular.Focus(); return; }
                    txtNroDocumento.Properties.MaxLength = obj.ctd_digitos;
                }
                eTrab = AsignarValores_Trabajador();
                eTrab = unit.Trabajador.InsertarActualizar_Trabajador<eTrabajador>(eTrab);
                if (eTrab == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                cod_trabajador = eTrab.cod_trabajador; cod_empresa = eTrab.cod_empresa;

                if (eTrab != null)
                {
                    ActualizarListado = "SI";
                    if (frmHandler != null)
                    {
                        int nRow = frmHandler.gvListadoTrabajador.FocusedRowHandle;
                        frmHandler.frmListadoTrabajador_KeyDown(frmHandler, new KeyEventArgs(Keys.F5));
                        frmHandler.gvListadoTrabajador.FocusedRowHandle = nRow;
                        //frmHandler.CargarOpcionesMenu();
                    }

                    MiAccion = Trabajador.Editar;
                    MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Inicializar();
                    //frmHandler.frmListadoTrabajador_KeyDown(null, new KeyEventArgs(Keys.F5));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void chkApto_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit edit = sender as CheckEdit;

        }

        private void grdbFlgObservado_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime FechaRegistro = DateTime.Today;
            int opcion = 0;
            eTrabajador.eEMO obj = new eTrabajador.eEMO();
            opcion = grdbFlgObservado.SelectedIndex;
            switch (opcion)
            {
                case 0:
                    obj.flg_observacion = "APTO";
                    dtFchEvaluacionObs.Enabled = false;
                    memObservacion.Enabled = false;
                    break;
                case 1:
                    obj.flg_observacion = "NO APTO";
                    dtFchEvaluacionObs.Enabled = false;
                    memObservacion.Enabled = false;
                    break;
                case 2:
                    obj.flg_observacion = "APTO CON RESTRICCIONES";
                    dtFchEvaluacionObs.Enabled = true;
                    memObservacion.Enabled = true;
                    dtFchEvaluacionObs.EditValue = FechaRegistro;
                    break;
            }
        }



        private async void btnGuardarEMO_Click(object sender, EventArgs e)
        {
            List<eVentana> listPerfil = unit.Sistema.ListarPerfilesUsuario<eVentana>(4, Program.Sesion.Usuario.cod_usuario, Program.Sesion.Global.Solucion);
            eVentana oPerfil = listPerfil.Find(x => x.cod_perfil == 13 || x.cod_perfil == 14);
            string tipo_doc = lkpTipodoc.Text.ToString();
            Text = btnGuardarEMO.Text;
            if (oPerfil == null)
            {
                if (tipo_doc == "CERTIFICADO EMO") { MessageBox.Show("Acceso denegado para registrar EMO", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipodoc.Focus(); return; }
                else
                {
                    GuardarDocVarios(Text);
                }
            }
            else
            {
                GuardarDocVarios(Text);
            }


        }
        private async void GuardarDocVarios(string nombreboton)
        {
            string result = "";
            DateTime FechaRegistro = DateTime.Today;
            eTrabajador.eEMO obj = new eTrabajador.eEMO();
            if (nombreboton == "EDITAR")
            {
                eTrabajador.eEMO er = gvEMO.GetFocusedRow() as eTrabajador.eEMO;
                if (er.dsc_documento == "POR SELECCIONAR")
                {
                    eTrabajador.eEMO objEMO = new eTrabajador.eEMO();
                    obj.dsc_descripcion = er.dsc_documento + "_" + FechaRegistro.Year.ToString() + "." + FechaRegistro.ToString("MM") + "." + FechaRegistro.ToString("dd") + "-" + eTrabdoc.dsc_abreviatura + "_" + obj.cod_EMO;
                    obj.cod_documento = lkpTipodoc.EditValue.ToString();
                    obj.cod_empresa = cod_empresa;
                    obj.id_certificado = er.id_certificado;
                    obj.flg_certificado = er.flg_certificado;
                    await Renombrar_ArchivoOneDrive(obj);
                    result = unit.Trabajador.EliminarInactivarEMOtrabajador(1, cod_trabajador: cod_trabajador, cod_empresa: cod_empresa, cod_EMO: er.cod_EMO, cod_documento: er.cod_documento);
                    remplazo = "SI";
                    obj.dsc_documento = dsc_archivor;
                    obj.nombre_archivoonedrive = dsc_archivor;
                    obj = AsignarValor_EMO();
                    obj = unit.Trabajador.InsertarActualizar_EMOTrabajador<eTrabajador.eEMO>(obj, opcion: 1);

                    gvEMO.RefreshData();

                }
                else
                {
                    remplazo = "NO";
                    obj = AsignarValor_EMO();
                    obj = unit.Trabajador.InsertarActualizar_EMOTrabajador<eTrabajador.eEMO>(obj, opcion: 1);
                }

            }
            else if (nombreboton == "GUARDAR")
            {
                remplazo = "SI";
                obj = AsignarValor_EMO();
                obj = unit.Trabajador.InsertarActualizar_EMOTrabajador<eTrabajador.eEMO>(obj, opcion: 1);
            }

            if (obj == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); nombrearchivo = ""; return; }
            if (obj != null)
            {
                MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ObtenerDatos_EMO();

                gvEMO_FocusedRowChanged(gvEMO, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
            }
        }

        private void gvEMO_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            List<eVentana> listPerfil = unit.Sistema.ListarPerfilesUsuario<eVentana>(4, Program.Sesion.Usuario.cod_usuario, Program.Sesion.Global.Solucion);
            eVentana oPerfil = listPerfil.Find(x => x.cod_perfil == 13 || x.cod_perfil == 14);
            eTrabajador.eEMO obj = new eTrabajador.eEMO();
            obj = gvEMO.GetFocusedRow() as eTrabajador.eEMO;

            if(obj.dsc_documento=="CERTIFICADO EMO" && oPerfil == null)
            {
                dtFchEvaluacion.Properties.UseSystemPasswordChar = true;
                dtFchEvaluacionObs.Properties.UseSystemPasswordChar = true;
                memObservacion.Properties.UseSystemPasswordChar = true;
                txtArchivoEmo.Properties.UseSystemPasswordChar = true;
                lkpTipodoc.ReadOnly = true;
                btnAdjuntarEMO.Enabled = false; btnGuardarEMO.Enabled = false;
            }else if(obj.dsc_documento == "CERTIFICADO EMO" && oPerfil != null)
            {
                dtFchEvaluacion.Properties.UseSystemPasswordChar = false;
                dtFchEvaluacionObs.Properties.UseSystemPasswordChar = false;
                memObservacion.Properties.UseSystemPasswordChar = false;
                txtArchivoEmo.Properties.UseSystemPasswordChar = false;
                lkpTipodoc.ReadOnly = false;
                btnAdjuntarEMO.Enabled = true; btnGuardarEMO.Enabled = true;
            }
                  
                   Obtenervalor_EMO();
            


        }

        private void btn_clonar_Click(object sender, EventArgs e)
        {
            eTrabajador.eInfoLaboral_Trabajador obj = new eTrabajador.eInfoLaboral_Trabajador();
            obj = gvListadoInfoLaboral.GetFocusedRow() as eTrabajador.eInfoLaboral_Trabajador;
            if (obj == null) return;
            txtCodInfoLaboral.Text = "0";
            if (lkpTipoContratoInfoLaboral.EditValue == null)
            {
                MessageBox.Show("Seleccione  Información Laboral", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }

            else if (obj.cod_tipo_contrato == "CT008")
            {
                eTrab = unit.Trabajador.Obtener_cod_trabajador<eTrabajador>(83, cod_empresa, "", cod_trabajador: cod_trabajador);
                dtFecIngresoInfoLaboral.EditValue = eTrab.fch_ingreso;

                lkpEmpresaInfoLaboral.Enabled = false;
                lkpSedeEmpresaInfoLaboral.Enabled = false;
                lkpAreaInfoLaboral.Enabled = false;
                lkpTipoTrabajador.Enabled = false;
                lkpSituacionTrabajador_2.Enabled = false;
                lkpsituacionespecial.Enabled = false;
                lkpCargoInfoLaboral.Enabled = false;
                lkpPrefCECOInfoLaboral.Enabled = false;
                dtFecIngresoInfoLaboral.Enabled = false;
                lkpOcupacional.EditValue = "";
                lkpCategoria.EditValue = "";
                lkpTipoContratoInfoLaboral.Enabled = false;
                //lkpModalidadInfoLaboral.EditValue = null;
                //lkpConvenioTributacion.EditValue=null;
                //cbxJornadaMaxima.EditValue = "NO";
                //cbxSindicato.EditValue = "NO";
                //cbxRegimenAtipico.EditValue = "NO";
                //cbxHorarioNocturno.EditValue = "NO";
                //lkpexonerado5.EditValue = "NO";
                chckAdjuntarContrato.Checked = false;
                chkasignacionFamiliar.Checked = false;
                chkflgRegimenPension1.Checked = false;

            }
            else
            {

                lkpEmpresaInfoLaboral.Enabled = true;
                lkpSedeEmpresaInfoLaboral.Enabled = true;
                lkpAreaInfoLaboral.Enabled = true;
                lkpTipoTrabajador.Enabled = true;
                lkpSituacionTrabajador_2.Enabled = true;
                lkpsituacionespecial.Enabled = true;
                lkpCargoInfoLaboral.Enabled = true;
                lkpPrefCECOInfoLaboral.Enabled = true;
                lkpTipoContratoInfoLaboral.Enabled = true;
                lkpEmpresa.EditValue = obj.cod_empresa;
                dtFecIngresoInfoLaboral.EditValue = obj.fch_ingreso;
                lkpSedeEmpresaInfoLaboral.EditValue = obj.cod_sede_empresa;
                lkpAreaInfoLaboral.EditValue = obj.cod_area;
                lkpCargoInfoLaboral.EditValue = obj.cod_cargo;
                lkpPrefCECOInfoLaboral.EditValue = obj.dsc_pref_ceco;
                lkpPrefCECOInfoLaboral.ToolTip = lkpPrefCECOInfoLaboral.Text;
                //validar adendas
                lkpTipoContratoInfoLaboral.EditValue = obj.cod_tipo_contrato;
                dtFecFirmaInfoLaboral.EditValue = obj.fch_firma;
                dtFecVctoInfoLaboral.EditValue = obj.fch_vencimiento;
                lkpModalidadInfoLaboral.EditValue = obj.cod_modalidad;
                txtMontoSueldoBaseInfoLaboral.EditValue = obj.imp_sueldo_base;
                txtMontoAsigFamiliarInfoLaboral.EditValue = obj.imp_asig_familiar;
                txtMontoMovilidadInfoLaboral.EditValue = obj.imp_movilidad;
                txtMontoAlimentacionInfoLaboral.EditValue = obj.imp_alimentacion;
                txtMontoEscolaridadInfoLaboral.EditValue = obj.imp_escolaridad;
                txtMontoBonoInfoLaboral.EditValue = obj.imp_bono;
                txtNroCuentaBancariaInfoBancaria.Text = "";
                txtNroCuentaCCIInfoBancaria.Text = "";
                lkpexonerado5.EditValue = obj.cod_exoneracion_5ta;
                cbxJornadaMaxima.EditValue = obj.flg_jornada_maxima;
                cbxSindicato.EditValue = obj.flg_jornada_maxima;
                cbxRegimenAtipico.EditValue = obj.flg_regimen_atipico;
                lkpHorarioNocturno.EditValue = obj.flg_regimen_atipico;
                lkpConvenioTributacion.EditValue = obj.cod_conveniotributacion;
                lkpsituacionespecial.EditValue = obj.cod_situacion_especial;
                lkpOcpaciones.EditValue = obj.cod_ocupaciones;
                chckAdjuntarContrato.Checked = false;
                chkasignacionFamiliar.Checked = false;
                chkflgRegimenPension1.Checked = false;
            }
        }


        private void actelContactoEmergencia_Click(object sender, EventArgs e)
        {

        }

        private void acctlMenu_Click(object sender, EventArgs e)
        {

        }

        private void bsListaInfoLaboral_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void gvInfoLaboral_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0) ObtenerDatos_Trabajador();
            //Obtener_InfoLaboralgeneral();
            //tipo_contrato();
        }

        private void lkptipovia_EditValueChanged(object sender, EventArgs e)
        {
            txtnombrevia.Enabled = true;
            if (lkptipovia.EditValue == null)
            {

            }
            else if (lkptipovia.EditValue != null)
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                    txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text);

            }
        }



        private void lkptipozona_EditValueChanged(object sender, EventArgs e)
        {
            txtnombrezona.Enabled = true;

            if (lkptipozona.EditValue == null)
            {

            }
            else if (lkptipozona.EditValue != null)

            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                    txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text);

            }
        }

        private void actelEMO_Click(object sender, EventArgs e)
        {

        }

        private void lkpEmpresaInfoLaboral_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpEmpresaInfoLaboral.EditValue != null)
            {
                unit.Trabajador.CargaCombosLookUp("SedesEmpresa", lkpSedeEmpresaInfoLaboral, "cod_sede_empresa", "dsc_sede_empresa", "", valorDefecto: true, lkpEmpresaInfoLaboral.EditValue.ToString());
                lkpAreaInfoLaboral.Properties.DataSource = null; lkpCargoInfoLaboral.Properties.DataSource = null;
                //lkpSedeEmpresaInfoLaboral.EditValue = null;
                //lkpAreaInfoLaboral.EditValue = null; lkpCargoInfoLaboral.EditValue = null;
            }
        }

        private void chkflgAdiccionInfoFamiliar_CheckStateChanged(object sender, EventArgs e)
        {
            layoutControlItem104.Enabled = chkflgAdiccionInfoFamiliar.CheckState == CheckState.Checked ? true : false;
        }

        private void txtPesoCaractFisica_EditValueChanged(object sender, EventArgs e)
        {
            decimal peso = 0, estatura = 0;
            estatura = Convert.ToDecimal(Math.Pow(Convert.ToDouble(txtEstaturaCaractFisica.EditValue), 2));
            peso = Convert.ToDecimal(txtPesoCaractFisica.EditValue);
            if (estatura > 0) txtIMCCaractFisica.EditValue = Math.Round((peso / estatura), 2);
        }

        private async void VerDocumentos(int opcionDoc, string nombreDoc)
        {
            eTrabajador resultado = unit.Trabajador.Obtener_Trabajador<eTrabajador>(2, cod_trabajador, cod_empresa);
            if (resultado == null) return;
            switch (opcionDoc)
            {
                case 1: if (resultado.id_DNI == null || resultado.id_DNI == "") return; break;
                case 2: if (resultado.id_CV == null || resultado.id_CV == "") return; break;
                case 3: if (resultado.id_AntPolicial == null || resultado.id_AntPolicial == "") return; break;
                case 4: if (resultado.id_AntPenal == null || resultado.id_AntPenal == "") return; break;
                case 5: if (resultado.id_VerifDomiciliaria == null || resultado.id_VerifDomiciliaria == "") return; break;
            }

            eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, cod_empresa);
            if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
            { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            var app = App.PublicClientApp;
            ClientId = eEmp.ClientIdOnedrive;
            TenantId = eEmp.TenantOnedrive;
            Appl();
            // var app = PublicClientApp;

            try
            {
                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Abriendo documento", "Cargando...");
                //eEmpresa eEmp = unit.Factura.ObtenerDatosEmpresa<eEmpresa>(12, lkpEmpresaProveedor.EditValue.ToString());
                string correo = eEmp.UsuarioOnedrivePersonal;
                string password = eEmp.ClaveOnedrivePersonal;

                var securePassword = new SecureString();
                foreach (char c in password)
                    securePassword.AppendChar(c);

                authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

                GraphClient = new Microsoft.Graph.GraphServiceClient(
                new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                {
                    requestMessage
                        .Headers
                        .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                    return Task.FromResult(0);
                }));

                string IdPDF = opcionDoc == 1 ? resultado.id_DNI : opcionDoc == 2 ? resultado.id_CV : opcionDoc == 3 ? resultado.id_AntPolicial : opcionDoc == 4 ? resultado.id_AntPenal : opcionDoc == 5 ? resultado.id_VerifDomiciliaria : "";
                string IdOneDriveDoc = IdPDF;

                var fileContent = await GraphClient.Me.Drive.Items[IdOneDriveDoc].Content.Request().GetAsync();
                string ruta = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()) + @"\" + nombreDoc + "-" + resultado.dsc_documento + ".pdf";
                if (!System.IO.File.Exists(ruta))
                {
                    using (var fileStream = new FileStream(ruta, FileMode.Create, System.IO.FileAccess.Write))
                        fileContent.CopyTo(fileStream);
                }

                if (!System.IO.Directory.Exists(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()))) System.IO.Directory.CreateDirectory(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()));
                System.Diagnostics.Process.Start(ruta);
                SplashScreenManager.CloseForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubieron problemas al autenticar las credenciales", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lblResultado.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                return;
            }
        }


        private void gvInfoLaboral_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvInfoLaboral_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void glkpTipoDocumentoContacto_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void lkpEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            //if (lkpempresa.editvalue != null)
            //{
            //    unit.trabajador.cargacomboslookup("sedesempresa", lkpsedeempresainfolaboral, "cod_sede_empresa", "dsc_sede_empresa", "", valordefecto: true, lkpempresa.editvalue.tostring());
            //    lkpareainfolaboral.properties.datasource = null; lkpcargoinfolaboral.properties.datasource = null;
            //    lkpsedeempresainfolaboral.editvalue = null; lkpareainfolaboral.editvalue = null; lkpcargoinfolaboral.editvalue = null;
            //}
        }

        private void lkpSedeEmpresaInfoLaboral_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpSedeEmpresaInfoLaboral.EditValue != null)
            {
                unit.Trabajador.CargaCombosLookUp("Areas", lkpAreaInfoLaboral, "cod_area", "dsc_area", "", valorDefecto: true, lkpEmpresaInfoLaboral.EditValue == null ? "" : lkpEmpresaInfoLaboral.EditValue.ToString(), lkpSedeEmpresaInfoLaboral.EditValue == null ? "" : lkpSedeEmpresaInfoLaboral.EditValue.ToString());
                lkpAreaInfoLaboral.EditValue = null;


            }
        }

        private void lkpAreaInfoLaboral_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpAreaInfoLaboral.EditValue != null)
            {
                unit.Trabajador.CargaCombosLookUp("CargoEmpresa", lkpCargoInfoLaboral, "cod_cargo", "dsc_cargo", "", valorDefecto: true, lkpEmpresaInfoLaboral.EditValue == null ? "" : lkpEmpresaInfoLaboral.EditValue.ToString(), lkpSedeEmpresaInfoLaboral.EditValue == null ? "" : lkpSedeEmpresaInfoLaboral.EditValue.ToString(), lkpAreaInfoLaboral.EditValue == null ? "" : lkpAreaInfoLaboral.EditValue.ToString());
                lkpCargoInfoLaboral.EditValue = null;
            }
        }


        private void lkpTipoContratoInfoLaboral_EditValueChanged(object sender, EventArgs e)
        {
            eTrabajador.eInfoLaboral_Trabajador obj = new eTrabajador.eInfoLaboral_Trabajador();
            obj = gvListadoInfoLaboral.GetFocusedRow() as eTrabajador.eInfoLaboral_Trabajador;

            if (lkpTipoContratoInfoLaboral.EditValue != null)
            {
                unit.Trabajador.CargaCombosLookUp("Modalidad", lkpModalidadInfoLaboral, "cod_ModContrato", "dsc_ModContrato", "", valorDefecto: true, cod_tipoContrato: lkpTipoContratoInfoLaboral.EditValue.ToString());
                if (lkpTipoContratoInfoLaboral.EditValue.ToString() == "CT002") { dtFecVctoInfoLaboral.EditValue = null; }
            };

            if (lkpTipoContratoInfoLaboral.EditValue == null) { lkpModalidadInfoLaboral.EditValue = null; lkpModalidadInfoLaboral.Properties.DataSource = null; }
        }

        private void frmMantTrabajador_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && MiAccion == Trabajador.Editar) this.Close();
        }

        private void btnNuevo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                MiAccion = Trabajador.Nuevo;
                btnNuevo.Enabled = false;
                //acctlMenu.Enabled = false;
                cod_trabajador = "";
                txtCodTrabajador.Text = "";
                txtApellPaterno.Text = "";
                txtApellMaterno.Text = "";
                txtNombre.Text = "";
                dtFecNacimiento.EditValue = null;
                dtEntregaUnif.EditValue = null;
                dtRenovacionUnif.EditValue = null;
                dtFchEvaluacion.EditValue = null;
                dtFchEvaluacionObs.EditValue = null;
                lkpEstadoCivil.EditValue = "01";
                lkpEmpresa.EditValue = null;
                glkpTipoDocumento.EditValue = "DI001";
                txtNroDocumento.Text = "";
                dtFecVctoDocumento.EditValue = null;
                txtNroUbigeoDocumento.Text = "";
                chkFlgDNI.CheckState = CheckState.Unchecked;
                chkFlgCV.CheckState = CheckState.Unchecked;
                chkFlgVerifDomiciliaria.CheckState = CheckState.Unchecked;
                chkFlgAntPoliciales.CheckState = CheckState.Unchecked;
                chkFlgAntPenales.CheckState = CheckState.Unchecked;
                lkpSistPensionarioInfoBancaria.EditValue = null;
                lkpNombreAFPInfoBancaria.EditValue = null;
                txtNroCUSPPInfoBancaria.Text = "";
                txtDireccion.Text = "";
                txtReferencia.Text = "";
                lkpPais.EditValue = "00001"; lkpDepartamento.EditValue = "00015"; lkpProvincia.EditValue = "00128"; lkpNacionalidad.EditValue = "00001";
                lkpDistrito.EditValue = null;
                txtTelefono.Text = "";
                txtCelular.Text = "";
                txtEmail1.Text = "";
                txtEmail2.Text = "";
                ListHistInfoLaboral.Clear(); ListContactos.Clear(); ListInfoLaboral.Clear();
                ListInfoFamiliar.Clear(); ListInfoAcademica.Clear(); ListInfoProfesional.Clear();
                gvInfoLaboral.RefreshData(); gvListadoContactos.RefreshData(); gvListadoInfoLaboral.RefreshData(); gvEMO.RefreshData();
                gvListadoInfoFamiliar.RefreshData(); gvListadoFormAcademica.RefreshData(); gvListadoExpProfesional.RefreshData();
                btnNuevoContacto_Click(null, new EventArgs());
                btnNuevaInfoLaboral_Click(null, new EventArgs());
                btnNuevaInfoFamiliar_Click(null, new EventArgs());
                btnNuevaFormAcademic_Click(null, new EventArgs());
                btnNuevaInfoEconomica_Click(null, new EventArgs());
                btnNuevaExpProfesional_Click(null, new EventArgs());
                chkflgAlergiasInfoSalud.CheckState = CheckState.Unchecked;
                mmAlergias.Text = "";
                chkflgOperacionesInfoSalud.CheckState = CheckState.Unchecked;
                mmOperaciones.Text = "";
                chkflgEnfPrexistenteInfoSalud.CheckState = CheckState.Unchecked;
                mmEnfPrexistente.Text = "";
                chkflgTratamientoInfoSalud.CheckState = CheckState.Unchecked;
                mmTratamiento.Text = "";
                chkflgEnfActualInfoSalud.CheckState = CheckState.Unchecked;
                mmEnfActualidad.Text = "";
                chkflgTratActualInfoSalud.CheckState = CheckState.Unchecked;
                mmTratActual.Text = "";
                chkflgDiscapacidadInfoSalud.CheckState = CheckState.Unchecked;
                mmDiscapacidad.Text = "";
                chkflgOtrosInfoSalud.CheckState = CheckState.Unchecked;
                mmOtros.Text = "";
                lkpGrupoSanguineoInfoSalud.EditValue = null;
                lkpEstadoSaludInfoSalud.EditValue = null;
                lkpSeguroSaludInfoSalud.EditValue = null;
                glkpBancoInfoBancaria.EditValue = null;
                lkpTipoMonedaInfoBancaria.EditValue = null;
                lkpTipoCuentaInfoBancaria.EditValue = null;
                txtNroCuentaBancariaInfoBancaria.Text = "";
                txtNroCuentaCCIInfoBancaria.Text = "";
                glkpBancoCTSInfoBancaria.EditValue = null;
                lkpTipoMonedaCTSInfoBancaria.EditValue = null;
                txtNroCuentaBancariaCTSInfoBancaria.Text = "";
                txtNroCuentaCCICTSInfoBancaria.Text = "";
                lkpSistPensionarioInfoBancaria.EditValue = null;
                lkpNombreAFPInfoBancaria.EditValue = null;
                txtNroCUSPPInfoBancaria.Text = "";
                txtEstaturaCaractFisica.EditValue = 0;
                txtPesoCaractFisica.EditValue = 0;
                txtIMCCaractFisica.EditValue = 0;
                chkflgLentesCaractFisica.CheckState = CheckState.Unchecked;
                lkpPoloTallaUnif.EditValue = null;
                lkpCamisaTallaUnif.EditValue = null;
                lkpPantalonTallaUnif.EditValue = null;
                lkpCasacaTallaUnif.EditValue = null;
                lkpMamelucoTallaUnif.EditValue = null;
                lkpChalecoTallaUnif.EditValue = null;
                txtCalzadoTallaUnif.EditValue = null;
                txtIngresoMensualInfoEconomica.EditValue = 0;
                txtGastoMensualInfoEconomica.EditValue = 0;
                txtValorActivoInfoEconomica.EditValue = 0;
                txtValorDeudaInfoEconomica.EditValue = 0;
                lkpViviendaInfoEconomica.EditValue = null;
                lkpTipoViviendaInfoEconomica.EditValue = null;
                txtValorRentaInfoEconomica.EditValue = 0;
                txtDireccionViviendaInfoEconomica.Text = "";
                txtReferenciaViviendaInfoEconomica.Text = "";
                lkpPaisViviendaInfoEconomica.EditValue = null;
                lkpDepartamentoViviendaInfoEconomica.EditValue = null;
                lkpProvinciaViviendaInfoEconomica.EditValue = null;
                glkpDistritoViviendaInfoEconomica.EditValue = null;
                lkpVehiculoInfoEconomica.EditValue = null;
                txtMarcaInfoEconomica.Text = "";
                txtModeloInfoEconomica.Text = "";
                txtPlacaVehiculoInfoEconomica.Text = "";
                txtValorComercialInfoEconomica.EditValue = 0;
                glkpTipoDocumentoeconomica.EditValue = "DI004";
                txtParticipacionInfoEconomica.Text = "";
                txtRUCEmpresaInfoEconomica.Text = "";
                txtGiroEmpresaInfoEconomica.Text = "";
                txtDireccionEmpresaInfoEconomica.Text = "";
                lkpPaisEmpresaInfoEconomica.EditValue = null;
                lkpDepartamentoEmpresaInfoEconomica.EditValue = null;
                lkpProvinciaEmpresaInfoEconomica.EditValue = null;
                glkpDistritoEmpresaInfoEconomica.EditValue = null;
                lkpViviendaInfoVivienda.EditValue = null;
                lkpComodidadInfoVivienda.EditValue = null;
                chkflgPuertasInfoVivienda.CheckState = CheckState.Unchecked;
                chkflgVentanasInfoVivienda.CheckState = CheckState.Unchecked;
                chkflgTechoInfoVivienda.CheckState = CheckState.Unchecked;
                chkflgTelefonoInfoVivienda.CheckState = CheckState.Unchecked;
                chkCelularesInfoVivienda.CheckState = CheckState.Unchecked;
                chkflgInternetComunicacionInfoVivienda.CheckState = CheckState.Unchecked;
                chkflgLuzInfoVivienda.CheckState = CheckState.Unchecked;
                chkflgAguaInfoVivienda.CheckState = CheckState.Unchecked;
                chkflgDesagueInfoVivienda.CheckState = CheckState.Unchecked;
                chkflgGasInfoVivienda.CheckState = CheckState.Unchecked;
                chkflgCableInfoVivienda.CheckState = CheckState.Unchecked;
                chkflgInternetServicioInfoVivienda.CheckState = CheckState.Unchecked;
                mmViasAccesoInfoVivienda.Text = "";
                mmIluminacionInfoVivienda.Text = "";
                mmEntornoInfoVivienda.Text = "";
                mmPuestoPolicialInfoVivienda.Text = "";
                txtNombreFamiliarInfoVivienda.Text = "";
                txtHorasCasaInfoVivienda.Text = "";
                lkpParentescoInfoVivienda.EditValue = null;
                txtCelularInfoVivienda.Text = "";
                txtEmailInfoVivienda.Text = "";


                navframeTrabajador.SelectedPage = navpageGemeral;
                txtApellPaterno.Select();
                if (MiAccion != Trabajador.Editar)
                {
                    picAnteriorTrabajador.Enabled = false; picSiguienteTrabajador.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnGuardar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                txtApellPaterno.Select();
                if (txtApellPaterno.Text.Trim() == "") { MessageBox.Show("Debe ingresar un apellido paterno.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtApellPaterno.Focus(); return; }
                if (txtApellMaterno.Text.Trim() == "") { MessageBox.Show("Debe ingresar un apellido materno.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtApellMaterno.Focus(); return; }
                if (txtNombre.Text.Trim() == "") { MessageBox.Show("Debe ingresar un nombre.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNombre.Focus(); return; }
                if (dtFecNacimiento.EditValue == null) { MessageBox.Show("Debe ingresar la fecha de nacimiento", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtFecNacimiento.Focus(); return; }
                if (txtNroDocumento.Text.Trim() == "") { MessageBox.Show("Debe ingresar un número de documento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNroDocumento.Focus(); return; }
                //if (txtNroUbigeoDocumento.Text.Trim() == "") { MessageBox.Show("Debe ingresar un número de ubigeo de documento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNroUbigeoDocumento.Focus(); return; }
                if (txtDireccion.Text.Trim() == "") { MessageBox.Show("Debe ingresar una dirección.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtDireccion.Focus(); return; }
                if (lkpPais.EditValue == null) { MessageBox.Show("Debe seleccionar un país.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpPais.Focus(); return; }
                if (lkpDepartamento.EditValue == null) { MessageBox.Show("Debe seleccionar un departamento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpDepartamento.Focus(); return; }
                if (lkpProvincia.EditValue == null) { MessageBox.Show("Debe seleccionar una provincia.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpProvincia.Focus(); return; }
                if (lkpDistrito.EditValue == null) { MessageBox.Show("Debe seleccionar un distrito.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpDistrito.Focus(); return; }
                if (lkpNacionalidad.EditValue == null) { MessageBox.Show("Debe seleccionar la nacionalidad.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpNacionalidad.Focus(); return; }

                eTrab = AsignarValores_Trabajador();
                eTrab = unit.Trabajador.InsertarActualizar_Trabajador<eTrabajador>(eTrab);
                if (eTrab == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                cod_trabajador = eTrab.cod_trabajador; cod_empresa = eTrab.cod_empresa;

                if (eTrab != null)
                {
                    ActualizarListado = "SI";
                    if (frmHandler != null)
                    {
                        int nRow = frmHandler.gvListadoTrabajador.FocusedRowHandle;
                        frmHandler.frmListadoTrabajador_KeyDown(frmHandler, new KeyEventArgs(Keys.F5));
                        frmHandler.gvListadoTrabajador.FocusedRowHandle = nRow;
                        //frmHandler.CargarOpcionesMenu();
                    }

                    MiAccion = Trabajador.Editar;
                    MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Inicializar();
                    //frmHandler.frmListadoTrabajador_KeyDown(null, new KeyEventArgs(Keys.F5));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private eTrabajador AsignarValores_Trabajador()
        {
            eTrabajador obj = new eTrabajador();
            obj.cod_trabajador = txtCodTrabajador.Text;
            obj.dsc_apellido_paterno = txtApellPaterno.Text.Trim();
            obj.dsc_apellido_materno = txtApellMaterno.Text.Trim();
            obj.dsc_nombres = txtNombre.Text.Trim();
            obj.fch_nacimiento = Convert.ToDateTime(dtFecNacimiento.EditValue);
            obj.cod_estadocivil = lkpEstadoCivil.EditValue == null ? null : lkpEstadoCivil.EditValue.ToString();
            obj.cod_empresa = lkpEmpresa.EditValue == null ? null : lkpEmpresa.EditValue.ToString();
            obj.cod_tipo_documento = glkpTipoDocumento.EditValue.ToString();
            obj.dsc_documento = txtNroDocumento.Text;
            obj.fch_vcto_documento = Convert.ToDateTime(dtFecVctoDocumento.EditValue);
            obj.nro_ubigeo_documento = txtNroUbigeoDocumento.Text;
            obj.cod_nacionalidad = lkpNacionalidad.EditValue == null ? null : lkpNacionalidad.EditValue.ToString();
            obj.flg_DNI = chkFlgDNI.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_CV = chkFlgCV.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_VerifDomiciliaria = chkFlgVerifDomiciliaria.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_AntPolicial = chkFlgAntPoliciales.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.flg_AntPenal = chkFlgAntPenales.CheckState == CheckState.Checked ? "SI" : "NO";
            obj.dsc_direccion = txtDireccion.Text;
            obj.dsc_referencia = txtReferencia.Text;
            obj.cod_pais = lkpPais.EditValue == null ? null : lkpPais.EditValue.ToString();
            obj.cod_departamento = lkpDepartamento.EditValue == null ? null : lkpDepartamento.EditValue.ToString();
            obj.cod_provincia = lkpProvincia.EditValue == null ? null : lkpProvincia.EditValue.ToString();
            obj.cod_distrito = lkpDistrito.EditValue.ToString();
            if (txtTelefono.Text == "") obj.dsc_telefono = "-"; else { obj.dsc_telefono = txtTelefono.Text; }   
            obj.dsc_celular = txtCelular.Text;
            obj.dsc_mail_1 = txtEmail1.Text;
            obj.dsc_mail_2 = txtEmail2.Text;
            if (grdbTipoPersonal.SelectedIndex == 0) { obj.cod_TipoPersonal = "OFICINA"; } else if (grdbTipoPersonal.SelectedIndex == 1) { obj.cod_TipoPersonal = "DESTACADO"; } else { obj.cod_TipoPersonal = "CRITICO"; }
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            obj.flg_activo = "SI";
            obj.fch_entrega_uniforme = Convert.ToDateTime(dtEntregaUnif.EditValue);
            obj.fch_renovacion_uniforme = Convert.ToDateTime(dtRenovacionUnif.EditValue);
            obj.flg_sexo = lkpSexo.EditValue == null ? null : lkpSexo.EditValue.ToString();
            obj.cod_tipo_via = lkptipovia.EditValue == null ? null : lkptipovia.EditValue.ToString();
            obj.dsc_tipo_via = txtnombrevia.Text;
            obj.dsc_nro_vivienda = txtnro.Text;
            obj.dsc_departamento_dir = txtdep.Text;
            obj.dsc_interior = txtinterior.Text;
            obj.dsc_manzana = txtmz.Text;
            obj.cod_lote = txtlote.Text;
           // if (txtkm.Text == "") { obj.dsc_km = null; } else { obj.dsc_km = Convert.ToInt32(txtkm.Text); }
            obj.dsc_km = txtkm.Text;
            obj.dsc_etapa = txtetapa.Text;
            obj.dsc_block = txtblock.Text;
            obj.dsc_tipo_zona = lkptipozona.EditValue == null ? null : lkptipozona.EditValue.ToString();
            obj.dsc_nombre_zona = txtnombrezona.Text;


            return obj;
        }





        #region Espacio para formato de documentos()
        private void ConfigurarGridFormatos()
        {
            //unit.Globales.ConfigurarGridView_TreeStyle(gcListadoFormatos, gvListadoFormatos);
            //gvListadoFormatos.OptionsView.EnableAppearanceEvenRow = false;
            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoTrabajadorInfoFormato, gvListadoTrabajadorInfoFormato);
            //gvListadoTrabajadorInfoFormato.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            //gvListadoFormatos.OptionsView.EnableAppearanceEvenRow = true;
            //btnGuardarFormatoDucumento.Appearance.BackColor = Program.Sesion.Colores.Verde;
            btnGuardarSeguimiento.Appearance.BackColor = Program.Sesion.Colores.Verde;
            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoDocumentoSeguimiento, gvListadoDocumentoSeguimiento);
        }

        private void CargarFormatosAsignados()
        {
            var objFormato = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eTrabajador_InfoFormatoMD_Vista>(
                new pQFormatoMD() { Opcion = 18, Cod_empresaSplit = cod_empresa, Cod_trabajadorSplit = cod_trabajador });
            bsListadoTrabajadorInfoFormato.DataSource = null;
            if (objFormato != null && objFormato.Count > 0)
            {
                bsListadoTrabajadorInfoFormato.DataSource = objFormato.Where(o => o.flg_activo.Equals("SI")).ToList();
                gvListadoTrabajadorInfoFormato.RefreshData();
                gvListadoTrabajadorInfoFormato.ExpandAllGroups();
            }
        }
        List<eFormatoMD_Parametro> _listadoParametros;
        private void CargarListadoDeFormatos()
        {
           
            CargarFormatosAsignados();
            CargarFormatosEmitidos();

            //Cargar Parametros
            _listadoParametros = new List<eFormatoMD_Parametro>();
            _listadoParametros = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eFormatoMD_Parametro>(
                 new pQFormatoMD() { Opcion = 6, });

        }

        private void CargarFormatosEmitidos()
        {
            var objFormato = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eTrabajador_InfoFormatoMD_Resumen>(
                   new pQFormatoMD() { Opcion = 20, Cod_trabajadorSplit = cod_trabajador, Cod_empresaSplit = cod_empresa });
            if (objFormato != null && objFormato.Count > 0)
            {

                bsListadoDocumentoSeguimiento.DataSource = objFormato.ToList();
                gvListadoDocumentoSeguimiento.RefreshData();
                gvListadoDocumentoSeguimiento.ExpandAllGroups();
            }
        }


        private void GuardarSeguimiento()
        {
            var objData = new eFormatoMD_Seguimiento();
            objData.cod_formatoMD_seguimiento = "new";
            objData.fch_inicio_firma = dtInicioSeguimiento.DateTime;
            objData.fch_vence_firma = dtFinSeguimiento.DateTime;
            objData.dsc_observacion = txtObsSeguimiento.Text;
            objData.cod_usuario_cambio = Program.Sesion.Usuario.cod_usuario;
            objData.cod_trabajador = cod_trabajador;
            objData.cod_empresa = cod_empresa;
            var objC = lblEmitirFormato.Tag as eTrabajador_InfoFormatoMD_Vista;
            objData.cod_formatoMD_vinculo = objC.cod_formatoMD_vinculo;

            var result = unit.FormatoMDocumento.InsertarActualizar_FormatoMDSeguimiento<eSqlMessage>(1, objData);
            if (result.IsSuccess)
            {
                //Se ha registrado;
                unit.Globales.Mensaje(true, result.Outmessage, "Emisión de Formatos");
                lciEmitirDocumento.Visibility = LayoutVisibility.Never;
                lciVistaPrevia.Visibility = LayoutVisibility.Always;
                groupPlantilla.Visibility = LayoutVisibility.Never;
                txtObsSeguimiento.Text = "";
                txtObsSeguimiento.Tag = null;
                gcListadoTrabajadorInfoFormato.Enabled = true;
                CargarFormatosEmitidos();
                AdministrarButtonDocumentacion(false);
            }
        }

        private async void checkEdit1_Click(object sender, EventArgs e)
        {
            await AdjuntarDocumentoFamiliar(1, "Doc. Identidad", nombreDocAdicional: txtNombreInfoFamiliar.Text + " " + txtApellPaterno.Text);
        }

        private async void chkCERTIFICADOESTUDIOS_Click(object sender, EventArgs e)
        {
            await AdjuntarDocumentoFamiliar(2, "Cert. Estudio", nombreDocAdicional: txtNombreInfoFamiliar.Text + " " + txtApellPaterno.Text);
        }

        private void gvListadoTrabajadorInfoFormato_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Column.FieldName == "flg_publicado")
                {
                    e.Appearance.ForeColor = e.Appearance.BackColor;
                    var obj = gvListadoTrabajadorInfoFormato.GetRow(e.RowHandle) as eTrabajador_InfoFormatoMD_Vista;
                    e.DefaultDraw();
                    if (obj.flg_publicado.Equals("SI"))
                    {
                        e.Cache.DrawImage(Properties.Resources.ok_18px, e.Bounds.X + (e.Column.Width / 2), e.Bounds.Y);
                    }
                    else
                    {
                        e.Cache.DrawImage(Properties.Resources.unchecked_radio_button_18px, e.Bounds.X + (e.Column.Width / 2), e.Bounds.Y);
                    }
                }
            }
        }


        private void btnEmitirDocumento_Click(object sender, EventArgs e)
        {
            var obj = gvListadoTrabajadorInfoFormato.GetFocusedRow() as eTrabajador_InfoFormatoMD_Vista;
            if (obj == null) return;

            // Si no se ha publicado el fomato, no se podrá emitir/generar.
            if (obj.flg_publicado.Equals("NO")) return;

            lblEmitirFormato.Text = obj.dsc_formatoMD_vinculo.ToString();
            lblEmitirFormato.Tag = obj;

            //var obj = gvListadoTrabajadorInfoFormato.GetFocusedRow() as eTrabajador_InfoFormatoMD_Vista;
            //if (lblEmitirFormato.Tag == null) return;
            //var obj = lblEmitirFormato.Tag as eTrabajador_InfoFormatoMD_Vista;

            lciOcultarPlantilla.Visibility = LayoutVisibility.Never;
            lciVistaPrevia.Visibility = LayoutVisibility.Never;
            lciEmitirDocumento.Visibility = LayoutVisibility.Always;
            //lblEmitirFormato.Text = obj.dsc_formatoMD_vinculo;
            //lblEmitirFormato.Tag = obj;
            gcListadoTrabajadorInfoFormato.Enabled = false;

            AdministrarButtonDocumentacion(true);
            VisualizarDocumentoPlantilla();
        }



        private void bttnbusc_Click(object sender, EventArgs e)
        {
            if (txtNroDocumento.Text == "")
            {
                MessageBox.Show("DEBE INGRESAR UN NÚMERO DE DOCUMENTO");
            }
            else
            {
                eTrab = unit.Trabajador.Obtener_cod_trabajador<eTrabajador>(79, "", txtNroDocumento.Text.ToString());

                if (eTrab != null)

                {
                    dsc_documento = eTrab.cod_trabajador;
                    bloqueo();
                    frmMensaje mensaje = new frmMensaje();
                    mensaje.txtmensaje.Text = "EL TRABAJADOR YA SE ENCUENTRA REGISTRADO, CODIGO DE TRABAJADOR: " + dsc_documento;

                    mensaje.ShowDialog();


                }
                else
                {
                    string numero = "";
                    numero = txtNroDocumento.Text;
                    eTrabajador resultado = unit.Trabajador.Validardni<eTrabajador>(18, numero);
                    desbloqueo();
                    txtNombre.Text = "Ingrese Nombre";
                    //resultado.dsc_nombres;
                }
            }
        }



        private void lkpSexo_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpSexo.EditValue.ToString() == "F")
            {
                Image imgEmpresaLarge = Properties.Resources.female64;
                picTrabajador.EditValue = imgEmpresaLarge;
            }
            else if (lkpSexo.EditValue.ToString() == "M")
            {
                Image imgEmpresaLarge = Properties.Resources.Male64;
                picTrabajador.EditValue = imgEmpresaLarge;
            }
        }

        private void chckdireccionfamiliar_CheckedChanged(object sender, EventArgs e)
        {
            layoutControlItem94.Enabled = chckdireccionfamiliar.CheckState == CheckState.Checked ? true : false;
            if (chckdireccionfamiliar.CheckState == CheckState.Checked)
            {
                eTrab = unit.Trabajador.Obtener_Trabajador<eTrabajador>(2, cod_trabajador, cod_empresa);
                if (eTrab.dsc_direccion == null)
                {

                    chckdireccionfamiliar.CheckState = CheckState.Unchecked;
                    txtDireccionInfoFamiliar.Text = " ";
                    txtDireccionInfoFamiliar.Enabled = true;
                    lkpPaisInfoFamiliar.Enabled = true;
                    lkpDepartamentoInfoFamiliar.Enabled = true;
                    lkpProvinciaInfoFamiliar.Enabled = true;
                    glkpDistritoInfoFamiliar.Enabled = true;
                    txtDireccionInfoFamiliar.Focus(); return;
                    lkpPaisInfoFamiliar.EditValue = "00001";
                    lkpDepartamentoInfoFamiliar.EditValue = "00015";
                    lkpProvinciaInfoFamiliar.EditValue = "00128";
                    glkpDistritoInfoFamiliar.EditValue = "01251";

                }
                else
                {
                    txtDireccionInfoFamiliar.Enabled = false;
                    lkpPaisInfoFamiliar.Enabled = false;
                    lkpDepartamentoInfoFamiliar.Enabled = false;
                    lkpProvinciaInfoFamiliar.Enabled = false;
                    glkpDistritoInfoFamiliar.Enabled = false;
                    txtDireccionInfoFamiliar.Text = eTrab.dsc_direccion;
                    lkpPaisInfoFamiliar.EditValue = eTrab.cod_pais;
                    lkpDepartamentoInfoFamiliar.EditValue = eTrab.cod_departamento;
                    lkpProvinciaInfoFamiliar.EditValue = eTrab.cod_provincia;
                    glkpDistritoInfoFamiliar.EditValue = eTrab.cod_distrito;
                }

            }
            else
            {
                txtDireccionInfoFamiliar.Enabled = true;
                txtDireccionInfoFamiliar.Text = " ";
                lkpPaisInfoFamiliar.Enabled = true;
                lkpDepartamentoInfoFamiliar.Enabled = true;
                lkpProvinciaInfoFamiliar.Enabled = true;
                glkpDistritoInfoFamiliar.Enabled = true;
                lkpPaisInfoFamiliar.EditValue = "00001";
                lkpDepartamentoInfoFamiliar.EditValue = "00015";
                lkpProvinciaInfoFamiliar.EditValue = "00128";
                glkpDistritoInfoFamiliar.EditValue = "01251";
            }

        }

        private void lkpPaisInfoFamiliar_EditValueChanged(object sender, EventArgs e)
        {

            lkpDepartamentoInfoFamiliar.Properties.DataSource = null;
            unit.Clientes.CargaCombosLookUp("TipoDepartamento", lkpDepartamentoInfoFamiliar, "cod_departamento", "dsc_departamento", "", cod_condicion: lkpPaisInfoFamiliar.EditValue == null ? "" : lkpPaisInfoFamiliar.EditValue.ToString());
            lkpDepartamentoInfoFamiliar.EditValue = "00015";

        }

        private void lkpDepartamentoInfoFamiliar_EditValueChanged(object sender, EventArgs e)
        {

            unit.Clientes.CargaCombosLookUp("TipoProvincia", lkpProvinciaInfoFamiliar, "cod_provincia", "dsc_provincia", "", cod_condicion: lkpDepartamentoInfoFamiliar.EditValue == null ? "" : lkpDepartamentoInfoFamiliar.EditValue.ToString());
            lkpProvinciaInfoFamiliar.EditValue = "00128";

        }

        private void lkpProvinciaInfoFamiliar_EditValueChanged(object sender, EventArgs e)
        {

            //unit.Clientes.CargaCombosLookUp("TipoDistrito", glkpDistrito, "cod_distrito", "dsc_distrito", "", cod_condicion: lkpProvincia.EditValue.ToString());
            CargarCombosGridLookup("TipoDistrito", glkpDistritoInfoFamiliar, "cod_distrito", "dsc_distrito", "", cod_condicion: lkpProvinciaInfoFamiliar.EditValue == null ? "" : lkpProvinciaInfoFamiliar.EditValue.ToString());
            glkpDistritoInfoFamiliar.EditValue = "01251";
        }

        private void groupControl15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCancelarSeguimiento_Click(object sender, EventArgs e)
        {
            lciVistaPrevia.Visibility = LayoutVisibility.Always;
            lciEmitirDocumento.Visibility = LayoutVisibility.Never;
            groupPlantilla.Visibility = LayoutVisibility.Never;
            //groupSeguimiento.Visibility = LayoutVisibility.Always;
            txtObsSeguimiento.Text = "";
            txtObsSeguimiento.Tag = null;
            gcListadoTrabajadorInfoFormato.Enabled = true;
            AdministrarButtonDocumentacion(false);
        }



        private void txtnombrezona_EditValueChanged(object sender, EventArgs e)
        {
            if (txtnombrezona.Text == "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                    txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtnombrezona.Text != "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                   txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
           txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
           lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
           , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtnombrevia_EditValueChanged(object sender, EventArgs e)
        {
            if (txtnombrevia.Text == "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                     txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
             txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
             lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
             , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtnombrevia.Text != "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                     txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
             txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
             lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
             , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtnro_EditValueChanged(object sender, EventArgs e)
        {
            if (txtnro.Text == "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                   txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
           txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
           lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
           , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtnro.Text != "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                     txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
             txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
             lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
             , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtlote_EditValueChanged(object sender, EventArgs e)
        {
            if (txtlote.Text == "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                   txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
           txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
           lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
           , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtlote.Text != "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                   txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
           txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
           lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
           , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
        }

        private void txtdep_EditValueChanged(object sender, EventArgs e)
        {
            if (txtdep.Text == "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                    txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtdep.Text != "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                    txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtkm_EditValueChanged(object sender, EventArgs e)
        {
            if (txtkm.Text == "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                      txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
              txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
              lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
              , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtkm.Text != "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                    txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
        }

        private void txtinterior_EditValueChanged(object sender, EventArgs e)
        {
            if (txtinterior.Text == "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                   txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
           txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
           lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
           , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtinterior.Text != "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                   txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
           txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
           lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
           , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtetapa_EditValueChanged(object sender, EventArgs e)
        {
            if (txtetapa.Text == "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                    txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtetapa.Text != "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                   txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
           txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
           lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
           , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtmz_EditValueChanged(object sender, EventArgs e)
        {
            if (txtmz.Text == "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                   txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
           txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
           lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
           , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtmz.Text != "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                   txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
           txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
           lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
           , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtblock_EditValueChanged(object sender, EventArgs e)
        {
            if (txtblock.Text == "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                    txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtblock.Text != "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                    txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtReferencia_EditValueChanged(object sender, EventArgs e)
        {
            if (txtReferencia.Text == "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                    txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtReferencia.Text != "")
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                    txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Application.OpenForms["frmCargoArea"] != null)
            {
                Application.OpenForms["frmCargoArea"].Activate();
            }
            else
            {
                frmCargoArea frm = new frmCargoArea();
                frm.cod_empresa = lkpEmpresa.EditValue.ToString();
                frm.MiAccion = Area.Nuevo;
                frm.ShowDialog();
                if (frm.Actualizarcombo == "SI")
                {
                    unit.Trabajador.CargaCombosLookUp("CargoEmpresa", lkpCargoInfoLaboral, "cod_cargo", "dsc_cargo", "", valorDefecto: true, lkpEmpresaInfoLaboral.EditValue.ToString(), lkpSedeEmpresaInfoLaboral.EditValue.ToString(), lkpAreaInfoLaboral.EditValue.ToString());
                    lkpCargoInfoLaboral.EditValue = "00001";
                }
            }
        }

        private void btnAreaEmpresa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            frmAreaEmpresa frm = new frmAreaEmpresa();
            frm.cod_empresa = lkpEmpresa.EditValue.ToString();
            frm.MiAccion = Area.Nuevo;
            frm.ShowDialog();
            if (frm.Actualizarcombo == "SI")
            {
                unit.Trabajador.CargaCombosLookUp("AreaEmpresa", lkpAreaInfoLaboral, "cod_area", "dsc_area", "", valorDefecto: true, lkpEmpresaInfoLaboral.EditValue.ToString(), lkpSedeEmpresaInfoLaboral.EditValue.ToString());
                lkpCargoInfoLaboral.EditValue = "00001";
            }



        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

            navframeTrabajador.SelectedPage = navpageInfoFamiliar;
            if (InfoFamiliar == 0)
            {
                unit.Trabajador.CargaCombosLookUp("Parentesco", lkpParentescoInfoFamiliar, "cod_parentesco", "dsc_parentesco", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("TipoDocumentoTrabajador", glkpTipoDocumentoInfoFamiliar, "cod_tipo_documento", "dsc_tipo_documento", "", valorDefecto: true);
                unit.Clientes.CargaCombosLookUp("TipoPais", lkpPaisInfoFamiliar, "cod_pais", "dsc_pais", "");
                lkpPaisInfoFamiliar.EditValue = "00001";
                lkpDepartamentoInfoFamiliar.EditValue = "00015";
                lkpProvinciaInfoFamiliar.EditValue = "00128";
                glkpDistritoInfoFamiliar.EditValue = "01251";
                lkpParentescoInfoFamiliar.EditValue = "PR006";

                //unit.Clientes.CargaCombosLookUp("TipoDepartamento", lkpDepartamentoInfoFamiliar, "cod_departamento", "dsc_departamento", "");
                //unit.Clientes.CargaCombosLookUp("TipoProvincia", lkpProvinciaInfoFamiliar, "cod_provincia", "dsc_provincia", "");
                //CargarCombosGridLookup("TipoDistrito", glkpDistritoInfoFamiliar, "cod_distrito", "dsc_distrito", "");
                //dtFecNacimientoInfoFamiliar.EditValue = DateTime.Today;
                ObtenerDatos_InfoFamiliar();
                gvListadoInfoFamiliar_FocusedRowChanged(gvListadoInfoFamiliar, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
                InfoFamiliar = 1;
            }
        }

        private void chkflgAlergiasInfoSalud_CheckedChanged(object sender, EventArgs e)
        {
            layoutControlItem150.Enabled = chkflgAlergiasInfoSalud.CheckState == CheckState.Checked ? true : false;


        }

        private void chkflgOperacionesInfoSalud_CheckedChanged(object sender, EventArgs e)
        {
            layoutControlItem151.Enabled = chkflgOperacionesInfoSalud.CheckState == CheckState.Checked ? true : false;

        }

        private void chkflgDiscapacidadInfoSalud_CheckedChanged(object sender, EventArgs e)
        {
            layoutControlItem152.Enabled = chkflgDiscapacidadInfoSalud.CheckState == CheckState.Checked ? true : false;
        }

        private void chkflgEnfPrexistenteInfoSalud_CheckedChanged(object sender, EventArgs e)
        {
            layoutControlItem155.Enabled = chkflgEnfPrexistenteInfoSalud.CheckState == CheckState.Checked ? true : false;
        }

        private void chkflgTratamientoInfoSalud_CheckedChanged(object sender, EventArgs e)
        {
            layoutControlItem156.Enabled = chkflgTratamientoInfoSalud.CheckState == CheckState.Checked ? true : false;
        }

        private void chkflgOtrosInfoSalud_CheckedChanged(object sender, EventArgs e)
        {
            layoutControlItem157.Enabled = chkflgOtrosInfoSalud.CheckState == CheckState.Checked ? true : false;
        }

        private void chkflgEnfActualInfoSalud_CheckedChanged(object sender, EventArgs e)
        {
            layoutControlItem153.Enabled = chkflgEnfActualInfoSalud.CheckState == CheckState.Checked ? true : false;
        }

        private void chkasignacionFamiliar_CheckedChanged(object sender, EventArgs e)
        {
            eRemuneracionMinimaUtil obj = new eRemuneracionMinimaUtil();
            obj = unit.Trabajador.ObtenerRemuneracionminimavital<eRemuneracionMinimaUtil>(120);

            if (chkasignacionFamiliar.Checked == true)
            {
                decimal desc = obj.total_rem;
                txtMontoAsigFamiliarInfoLaboral.Text = Convert.ToString(desc); 
            }
            else if (chkasignacionFamiliar.Checked == false)
            {
                txtMontoAsigFamiliarInfoLaboral.Text = "0.00";
            }


        }

        private async void btnConsultarSunat_Click(object sender, EventArgs e)
        {
            try
            {
                if (glkpTipoDocumentoeconomica.EditValue != null && glkpTipoDocumentoeconomica.EditValue.ToString() == "DI004" && txtRUCEmpresaInfoEconomica.Text.Length != 11)
                {
                    MessageBox.Show("El RUC debe tener 11 digitos", "Validación RUC", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNroDocumento.Select();
                    return;
                }
                if (glkpTipoDocumentoeconomica.EditValue != null && glkpTipoDocumentoeconomica.EditValue.ToString() == "DI001" && txtRUCEmpresaInfoEconomica.Text.Length != 8)
                {
                    MessageBox.Show("El DNI debe tener 8 digitos", "Validación DNI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNroDocumento.Select();
                    return;
                }
                cerrarsplag();
                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Renderizando plantilla", "Cargando...");
                await ConsultaSUNAT5(txtRUCEmpresaInfoEconomica.Text.Trim());
                SplashScreenManager.CloseForm();
            }
            catch (Exception ex)
            {
                // MessageBox.Show("El DNI debe tener 8 digitos", "Validación DNI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }
        private void cerrarsplag()
        {
            foreach (Form splash in System.Windows.Forms.Application.OpenForms)
            {
                if (splash.Name.Equals("FrmSplashCarga"))
                {
                    //splash.Close();
                    SplashScreenManager.CloseForm();
                    break;
                }
            }
        }

        private async Task ConsultaSUNAT5(string nDocumento)
        {
            string endpoint = @"https://api.apis.net.pe/v1/ruc?numero=" + nDocumento;
            if (glkpTipoDocumentoeconomica.EditValue.ToString() == "DI004")
            {
                endpoint = @"https://api.apis.net.pe/v1/ruc?numero=" + nDocumento;
            }
            else
            {
                endpoint = @"https://api.apis.net.pe/v1/dni?numero=" + nDocumento;
            }
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            myWebRequest.CookieContainer = cokkie;

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebResponse myhttpWebResponse = (HttpWebResponse)myWebRequest.GetResponse(); // cae
            Stream myStream = myhttpWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myStream);

            string xDat = "";
            string validar = "";

            while (!myStreamReader.EndOfStream)
            {
                xDat = myStreamReader.ReadLine();
                if (xDat != "")
                {
                    string Datos = xDat;
                    char[] separadores = { ',' };
                    string[] palabras = Datos.Replace("\"", "").Replace("{", "").Replace("}", "").Split(separadores);

                    if (glkpTipoDocumentoeconomica.EditValue.ToString() == "DI004")
                    {
                        txtEmpresaInfoEconomica.Text = palabras[0].Substring(palabras[0].IndexOf(":") + 1, palabras[0].Length - palabras[0].IndexOf(":") - 1).ToUpper();
                        txtDireccionEmpresaInfoEconomica.Text = palabras[5].Substring(palabras[5].IndexOf(":") + 1, palabras[5].Length - palabras[5].IndexOf(":") - 1).ToUpper();
                        txtEmpresaInfoEconomica.Enabled = false;
                        txtDireccionEmpresaInfoEconomica.Enabled = false;
                        if (txtDireccionEmpresaInfoEconomica.Text.Trim() != "")
                        {
                            List<eCiudades> listDepartamento = new List<eCiudades>();
                            List<eCiudades> listProvincia = new List<eCiudades>();
                            List<eCiudades> listDistrito = new List<eCiudades>();
                            lkpPaisEmpresaInfoEconomica.EditValue = "00001";
                            listDepartamento = unit.Clientes.ListarOpcionesVariasCliente<eCiudades>(14, cod_condicion: lkpPais.EditValue.ToString());
                            eCiudades objDepart = listDepartamento.Find(x => x.dsc_departamento.ToUpper() == palabras[19].Substring(palabras[19].IndexOf(":") + 1, palabras[19].Length - palabras[19].IndexOf(":") - 1).ToUpper());
                            if (objDepart != null)
                            {
                                lkpDepartamentoEmpresaInfoEconomica.EditValue = objDepart.cod_departamento;
                                listProvincia = unit.Clientes.ListarOpcionesVariasCliente<eCiudades>(15, cod_condicion: objDepart.cod_departamento);
                                eCiudades objProv = listProvincia.Find(x => x.dsc_provincia.ToUpper() == palabras[18].Substring(palabras[18].IndexOf(":") + 1, palabras[18].Length - palabras[18].IndexOf(":") - 1).ToUpper());
                                if (objProv != null)
                                {
                                    glkpDistritoEmpresaInfoEconomica.EditValue = objProv.cod_provincia;
                                    listDistrito = unit.Clientes.ListarOpcionesVariasCliente<eCiudades>(16, cod_condicion: objProv.cod_provincia);
                                    eCiudades objDist = listDistrito.Find(x => x.dsc_distrito.ToUpper() == palabras[17].Substring(palabras[17].IndexOf(":") + 1, palabras[17].Length - palabras[17].IndexOf(":") - 1).ToUpper());
                                    if (objDist != null) glkpDistritoEmpresaInfoEconomica.EditValue = objDist.cod_distrito;
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }

                    validar = "OK";
                }
            }//TERMINA EL WHILE

            if (validar == "OK")
            {
            }
            else
            {
                MessageBox.Show("Error al traer datos del proveedor.", "Traer datos SUNAT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        public class EnvioJSONRUC
        {
            public string token { get; set; }
            public string ruc { get; set; }
        }
        public class EnvioJSONDNI
        {
            public string token { get; set; }
            public string dni { get; set; }
        }

        private void ConsultaSUNAT()
        {
            ObtenerCap();
            string endpoint = @"http://www.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias?accion=consPorRuc&nroRuc=" + txtNroDocumento.Text.Trim() + "&codigo= " + texto.ToUpper() + "&tipdoc=1";

            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            myWebRequest.CookieContainer = cokkie;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            HttpWebResponse myhttpWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
            Stream myStream = myhttpWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myStream);

            string xDat = "";
            int pos = 0;
            int pocision = 0;
            string dato = "";
            int posicionPersonaNatural = 0;
            string validar = "";

            while (!myStreamReader.EndOfStream)
            {
                xDat = myStreamReader.ReadLine();
                pos += 1;
                if (xDat.ToString() == "            <td width=\"18%\" colspan=1  class=\"bgn\">N&uacute;mero de RUC: </td>")
                {
                    dato = "Razon Social";
                    pocision = pos + 1;
                    validar = "OK";
                }
                else if (xDat.ToString() == "            <td class=\"bgn\" colspan=1>Tipo Contribuyente: </td>")
                {
                    dato = "Persona Natural";
                    pocision = pos + 1;
                }

                else if (xDat.ToString() == "            <td class=\"bgn\" colspan=1>Tipo de Documento: </td>")
                {
                    dato = "Tipo Documento";
                    pocision = pos + 1;
                }
                else if (xDat.ToString() == "\t              <td class=\"bgn\"colspan=1>Condici&oacute;n del Contribuyente:</td>")
                {
                    dato = "Condicion";   //habido y no habido
                    pocision = pos + 3;
                }
                else if (xDat.ToString() == "              <td class=\"bgn\" colspan=1 >Nombre Comercial: </td>")
                {
                    dato = "Nombre Comercial";
                    pocision = pos + 1;
                }
                else if (xDat.ToString() == "              <td class=\"bgn\" colspan=1>Direcci&oacute;n del Domicilio Fiscal:</td>")
                {
                    dato = "Direccion";
                    pocision = pos + 1;
                }

                if (posicionPersonaNatural == pos)
                {
                    string NombresaApellidos = xDat.Substring(16);
                    char[] separadores = { ' ', ',' };
                    string[] palabras1 = NombresaApellidos.Split(separadores);
                    string ApellidosPat = palabras1[0];
                    string ApellidosMat = palabras1[1];
                    string Nombres = NombresaApellidos.Replace(ApellidosPat + " " + ApellidosMat + ", ", "");

                    txtNombre.Text = Nombres;
                    txtApellPaterno.Text = ApellidosPat;
                    txtApellMaterno.Text = ApellidosMat;
                    glkpTipoDocumento.EditValue = "DI001";

                    txtEmpresaInfoEconomica.Text = "";
                    posicionPersonaNatural = 0;
                }

                if (pocision == pos)
                {
                    if (dato == "Razon Social")
                    {
                        string razon = getDatafromXML(xDat, 25);
                        txtEmpresaInfoEconomica.Text = razon.Substring(15);
                        glkpTipoDocumento.EditValue = "DI004";
                    }


                    else if (dato == "Tipo Documento")
                    {
                        string personanatural = getDatafromXML(xDat, 25).ToString();
                        char[] separadores = { ' ' };
                        string[] palabras = personanatural.Split(separadores);

                        string TipoDocumento = palabras[0];
                        string NumeroDocumento = palabras[1];

                        txtRUCEmpresaInfoEconomica.Text = NumeroDocumento;
                        glkpTipoDocumentoeconomica.Text = TipoDocumento;
                        posicionPersonaNatural = pos + 2;
                    }

                    else if (dato == "Direccion")
                    {
                        string direccion = getDatafromXML(xDat, 25);
                        txtDireccionEmpresaInfoEconomica.Text = direccion;

                        if (direccion != "-")
                        {
                            ObtenerUbigeo();
                            List<eCiudades> listDepartamento = new List<eCiudades>();
                            List<eCiudades> listProvincia = new List<eCiudades>();
                            List<eCiudades> listDistrito = new List<eCiudades>();
                            lkpPaisEmpresaInfoEconomica.EditValue = "00001";
                            listDepartamento = unit.Clientes.ListarOpcionesVariasCliente<eCiudades>(14, cod_condicion: lkpPaisEmpresaInfoEconomica.EditValue.ToString());
                            eCiudades objDepart = listDepartamento.Find(x => x.dsc_departamento.ToUpper() == SuDepartamento.ToUpper());
                            if (objDepart != null)
                            {
                                lkpDepartamentoEmpresaInfoEconomica.EditValue = objDepart.cod_departamento;
                                listProvincia = unit.Clientes.ListarOpcionesVariasCliente<eCiudades>(15, cod_condicion: objDepart.cod_departamento);
                                eCiudades objProv = listProvincia.Find(x => x.dsc_provincia.ToUpper() == SuProvincia.ToUpper());
                                if (objProv != null)
                                {
                                    lkpProvinciaEmpresaInfoEconomica.EditValue = objProv.cod_provincia;
                                    listDistrito = unit.Clientes.ListarOpcionesVariasCliente<eCiudades>(16, cod_condicion: objProv.cod_provincia);
                                    eCiudades objDist = listDistrito.Find(x => x.dsc_distrito.ToUpper() == SUDistrito.ToUpper());
                                    if (objDist != null) glkpDistritoEmpresaInfoEconomica.EditValue = objDist.cod_distrito;
                                }
                            }
                        }
                        pocision = 0;
                        dato = "";
                    }
                }
            }//TERMINA EL WHILE

            if (validar == "OK")
            {
            }
            else
            {
                MessageBox.Show("El RUC " + txtNroDocumento.Text + "  no existe", "Validación RUC", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        string captcha = "";
        CookieContainer cokkie = new CookieContainer();
        string[] nrosRuc = new string[] { };
        string texto = "";
        public void ObtenerCap()
        {

            try
            {
                ///////https://cors-anywhere.herokuapp.com/wmtechnology.org/Consultar-RUC/?modo=1&btnBuscar=Buscar&nruc=
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.sunat.gob.pe/cl-ti-itmrconsruc/captcha?accion=image");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                request.CookieContainer = cokkie;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();

                var image = new Bitmap(responseStream);
                //pictureLogo.EditValue = image;
                string ruta = "C:\\IMPERIUM-Software\\tessdata";
                var ocr = new TesseractEngine(ruta, "eng", EngineMode.Default);
                ocr.DefaultPageSegMode = PageSegMode.SingleBlock;
                Tesseract.Page p = ocr.Process(image);
                texto = p.GetText().Trim().ToUpper().Replace(" ", "");
            }
            catch (Exception ex)
            {
                //mensaje de error
            }
        }
        public string getDatafromXML(string lineRead, int len = 0)
        {

            try
            {
                lineRead = lineRead.Trim();
                lineRead = lineRead.Remove(0, len);
                lineRead = lineRead.Replace("</td>", "");
                while (lineRead.Contains("  "))
                {
                    lineRead = lineRead.Replace("  ", " ");
                }
                return lineRead;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        string SUDistrito;
        string SuProvincia;
        string SuDepartamento;
        public void ObtenerUbigeo()
        {
            string direccion = txtDireccionEmpresaInfoEconomica.Text;
            int index = 0;
            for (int i = 0; i < direccion.Length; i++)
            {
                if (direccion[i].ToString() == "-")
                {
                    index = i + 2;
                }
            }
            SUDistrito = direccion.Substring(index).ToLower();
            direccion = direccion.Replace(" - " + SUDistrito.ToUpper(), "");
            index = 0;

            for (int i = 0; i < direccion.Length; i++)
            {
                if (direccion[i].ToString() == "-")
                {
                    index = i + 2;
                }
            }
            SuProvincia = direccion.Substring(index).ToLower();
            direccion = direccion.Replace(" - " + SuProvincia.ToUpper(), "");
            index = 0;

            for (int i = 0; i < direccion.Length; i++)
            {
                if (direccion[i].ToString() == "-")
                {
                    index = i + 3;
                }
            }
            SuDepartamento = direccion.Substring(index).ToLower();
            if (index == 0) SuDepartamento = SuProvincia;
            direccion = direccion.Replace(" - " + SuDepartamento.ToUpper(), "");
            index = 0;
        }

        private void glkpTipoDocumentoInfoFamiliar_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void glkpTipoDocumentoeconomica_EditValueChanged(object sender, EventArgs e)
        {
            if (glkpTipoDocumentoeconomica.EditValue != null)
            {
                eProveedor obj = new eProveedor();
                obj = unit.Proveedores.Validar_NroDocumento<eProveedor>(19, "", glkpTipoDocumentoeconomica.EditValue.ToString());
                txtRUCEmpresaInfoEconomica.Properties.MaxLength = obj.ctd_digitos;
            }
        }

        private void lkptipozona_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) lkptipozona.EditValue = null;
        }

        private void lkptipovia_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) lkptipovia.EditValue = null;
        }





        private void txtNroDocumento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
               (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtNroDocumentoInfoFamiliar_KeyPress(object sender, KeyPressEventArgs e)
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

        private void rbtnflg_certificado_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            VerDocumentos(1, "Doc. Identidad");
        }

        private void chkflgTratActualInfoSalud_CheckedChanged(object sender, EventArgs e)
        {
            layoutControlItem154.Enabled = chkflgTratActualInfoSalud.CheckState == CheckState.Checked ? true : false;
        }



        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            VerDocumentoEMO(1, "Certificado EMO");
        }

        private void dtFecNacimiento_Leave(object sender, EventArgs e)
        {
            ValidarFechaDeNacimiento();
        }
        private bool ValidarFechaDeNacimiento()
        {
            bool ret = true;
            DateTime fechaNacimiento = dtFecNacimiento.DateTime;
            DateTime fechaActual = DateTime.Today;
            int edad = fechaActual.Year - fechaNacimiento.Year;
            if (edad <= 17) { MessageBox.Show("El Trabajador registrado debe ser mayor a 17 años ", "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtFecNacimiento.Focus(); ret = false; }
            return ret;
        }

        private void dtFecNacimiento_EditValueChanged(object sender, EventArgs e)
        {
            // Se ha  cambiado a  dtFecNacimiento_Leave:  validar antes de guardar
        }

        private void txtMontoSueldoBaseInfoLaboral_EditValueChanged(object sender, EventArgs e)
        {
            if (chkasignacionFamiliar.Checked == true)
            {
                double sueldobase = Convert.ToDouble(txtMontoSueldoBaseInfoLaboral.Text);
                double resultado = 0.00;

                resultado = sueldobase * 0.10;
                txtMontoAsigFamiliarInfoLaboral.Text = Convert.ToString(resultado);
            }

        }



        private void chkfchentregauniforme_CheckedChanged(object sender, EventArgs e)
        {
            if (chkfchentregauniforme.Checked == true) { dtEntregaUnif.Enabled = true; } else if (chkfchentregauniforme.Checked == false) { dtEntregaUnif.Enabled = false; dtEntregaUnif.EditValue = null; }
        }

        private void chkfchRenovaciónuniforme_CheckedChanged(object sender, EventArgs e)
        {
            if (chkfchRenovacionuniforme.Checked == true) { dtRenovacionUnif.Enabled = true; } else if (chkfchRenovacionuniforme.Checked == false) { dtRenovacionUnif.Enabled = false; dtRenovacionUnif.EditValue = null; }

        }

        private void lkpModalidadInfoLaboral_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpTipoContratoInfoLaboral.EditValue == null || lkpModalidadInfoLaboral.EditValue == null || lkpModalidadInfoLaboral.EditValue == "") return;
            eTrabajador.eInfoLaboral_Trabajador obj = new eTrabajador.eInfoLaboral_Trabajador();
            if (lkpTipoContratoInfoLaboral.EditValue.ToString() == "CT002" || lkpTipoContratoInfoLaboral.EditValue.ToString() == "CT003") { dtFecVctoInfoLaboral.EditValue = null; }
            else
            {
                obj = unit.Trabajador.Obtener_meses<eTrabajador.eInfoLaboral_Trabajador>(10, lkpTipoContratoInfoLaboral.EditValue.ToString(), lkpModalidadInfoLaboral.EditValue.ToString());
                DateTime resultado = dtFecFirmaInfoLaboral.DateTime.AddMonths(obj.ctd_meses);
                dtFecVctoInfoLaboral.DateTime = resultado;
            }
        }

        private void dtFecFirmaInfoLaboral_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpTipoContratoInfoLaboral.EditValue == null || lkpModalidadInfoLaboral.EditValue == null || lkpModalidadInfoLaboral.EditValue == "") return;
            eTrabajador.eInfoLaboral_Trabajador obj = new eTrabajador.eInfoLaboral_Trabajador();
            obj = unit.Trabajador.Obtener_meses<eTrabajador.eInfoLaboral_Trabajador>(10, lkpTipoContratoInfoLaboral.EditValue.ToString(), lkpModalidadInfoLaboral.EditValue.ToString());
            if (obj == null) return;
            if (obj.ctd_meses != 0)
            {
                DateTime resultado = dtFecFirmaInfoLaboral.DateTime.AddMonths(obj.ctd_meses);
                dtFecVctoInfoLaboral.DateTime = resultado;
            }
            else
            {
                dtFecVctoInfoLaboral.EditValue = null;
            }

        }

        private void gvListadoInfoLaboral_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {

                if (e.RowHandle >= 0)
                {
                    eTrabajador.eInfoLaboral_Trabajador obj1 = gvListadoInfoLaboral.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                    if (obj1 == null) return;
                    if (e.Column.FieldName == "flg_contrato" && obj1.flg_contrato == "SI")
                    {
                        e.Handled = true; e.Graphics.DrawImage(ImgPDF, new Rectangle(e.Bounds.X + (e.Bounds.Width / 2) - 8, e.Bounds.Y + (e.Bounds.Height / 2) - 8, 16, 16));
                    }
                    if (e.Column.FieldName == "flg_contrato" && obj1.flg_contrato == "NO") e.DisplayText = "";
                    if (e.Column.FieldName == "Fechabaja" && obj1.Fechabaja.ToString().Contains("0001")) e.DisplayText = "";
                    e.DefaultDraw();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void gvListadoInfoLaboral_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Clicks == 2 && e.Column.FieldName == "flg_contrato")
            {
                eTrabajador.eInfoLaboral_Trabajador resultado = unit.Trabajador.Obtener_Trabajador<eTrabajador.eInfoLaboral_Trabajador>(21, cod_trabajador, cod_empresa, cod_infolab: txtCodInfoLaboral.Text);
                if (resultado == null) return;
                if (resultado.flg_contrato == "NO")
                {
                    MessageBox.Show("No se cargado ningún PDF", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, cod_empresa);
                if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
                { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                var app = App.PublicClientApp;
                ClientId = eEmp.ClientIdOnedrive;
                TenantId = eEmp.TenantOnedrive;
                Appl();
                // var app = PublicClientApp;

                try
                {
                    unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Abriendo documento", "Cargando...");
                    //eEmpresa eEmp = unit.Factura.ObtenerDatosEmpresa<eEmpresa>(12, lkpEmpresaProveedor.EditValue.ToString());
                    string correo = eEmp.UsuarioOnedrivePersonal;
                    string password = eEmp.ClaveOnedrivePersonal;

                    var securePassword = new SecureString();
                    foreach (char c in password)
                        securePassword.AppendChar(c);

                    authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

                    GraphClient = new Microsoft.Graph.GraphServiceClient(
                    new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                    {
                        requestMessage
                            .Headers
                            .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                        return Task.FromResult(0);
                    }));

                    string IdPDF = resultado.id_contrato;
                    string IdOneDriveDoc = IdPDF;

                    var fileContent = await GraphClient.Me.Drive.Items[IdOneDriveDoc].Content.Request().GetAsync();
                    string ruta = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()) + @"\" + "Contrato_" + dsc_documento + txtCodInfoLaboral.Text + ".pdf";
                    if (!System.IO.File.Exists(ruta))
                    {
                        using (var fileStream = new FileStream(ruta, FileMode.Create, System.IO.FileAccess.Write))
                            fileContent.CopyTo(fileStream);
                    }

                    if (!System.IO.Directory.Exists(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()))) System.IO.Directory.CreateDirectory(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()));
                    System.Diagnostics.Process.Start(ruta);
                    SplashScreenManager.CloseForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hubieron problemas al autenticar las credenciales", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lblResultado.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                    return;
                }
            }
        }

        private void gcListadoFormAcademica_Click(object sender, EventArgs e)
        {

        }

        private void gcEMO_Click(object sender, EventArgs e)
        {

        }

        private void gvInfoLaboral_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {

                if (e.RowHandle >= 0)
                {
                    eTrabajador.eInfoLaboral_Trabajador obj1 = gvInfoLaboral.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                    if (obj1 == null) return;

                    if (e.Column.FieldName == "Fechabaja" && obj1.Fechabaja.ToString().Contains("0001")) e.DisplayText = "";
                    e.DefaultDraw();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lkpDistrito_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpDistrito.EditValue == null)
            {
                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                   txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
           txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
           lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
           , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (lkpDistrito.EditValue != null)


            {
                eTrabajador obj = new eTrabajador();
                obj = unit.Trabajador.ObtenerDatos<eTrabajador>(127, "", "",cod_pais: lkpPais.EditValue == null ? "" : lkpPais.EditValue.ToString(), cod_departamento: lkpDepartamento.EditValue == null ? "" : lkpDepartamento.EditValue.ToString(), cod_provincia: lkpProvincia.EditValue == null ? "" : lkpProvincia.EditValue.ToString(), cod_distrito: lkpDistrito.EditValue == null ? "" : lkpDistrito.EditValue.ToString());
                string ubigeo = obj.cod_reniec.ToString();
                txtNroUbigeoDocumento.Text = ubigeo;

                Direccion(lkptipozona.Text == null ? null : lkptipozona.Text.ToString(),
                   txtnombrezona.Text, lkptipovia.Text == null ? null : lkptipovia.Text.ToString(), txtnombrevia.Text,
           txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtetapa.Text, txtblock.Text,
           lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
           , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
        }

        private void groupControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnRegistroDoc_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["frmRegistroDocumentos"] != null)
            {
                Application.OpenForms["frmRegistroDocumentos"].Activate();
            }
            else
            {

                frmRegistroDocumentos frm = new frmRegistroDocumentos();

                frm.ShowDialog();

                if (frm.Actualizarcombo == "SI")
                {
                    unit.Trabajador.CargaCombosLookUp("TipoDocumentor", lkpTipoDocumentoingresos, "cod_documento", "dsc_documento", "", valorDefecto: true, flg_varios: "NO", cod_trabajador: cod_trabajador, cod_empresa: cod_empresa);
                    lkpTipoDocumentoingresos.ItemIndex = 0;
                    unit.Trabajador.CargaCombosLookUp("TipoDocumento", lkpTipodoc, "cod_documento", "dsc_documento", "", valorDefecto: true, flg_varios: "SI", cod_empresa: cod_empresa);
                    lkpTipodoc.ItemIndex = 0;
                }
            }
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private async void chckAdjuntarContrato_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtCodInfoLaboral.Text) != 0)
            {
                await AdjuntarDocumentosVarios(9, "Contrato", lkpTipoContratoInfoLaboral.Text + " " + txtCodInfoLaboral.Text);
                Cargarlistado_infolaboral();
                gvInfoLaboral_FocusedRowChanged(gvListadoInfoLaboral, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));

            }
            else { MessageBox.Show("Antes de adjuntar los docuentos debe seleccionar o crear un detalle laboral.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
        }


        private void rbtnCRUD_Click(object sender, EventArgs e)
        {
            eTrabajador.eDocumento_Trabajador obj = new eTrabajador.eDocumento_Trabajador();
            obj = gvDocIngreso.GetFocusedRow() as eTrabajador.eDocumento_Trabajador;
            string codtipo = lkpTipoDocumentoingresos.EditValue.ToString(); //ultimamod
            int cod_emoof = 0;
            eTrabdoc = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(119, cod_documento: codtipo);
            etrabdocue = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(115, cod_trabajador: cod_trabajador, cod_empresa: cod_empresa, cod_documento: obj.cod_documento);
            VerDocumentoDocReg(1, codtipo, cod_d: etrabdocue.dsc_archivo);
        }

        private void btnGuardarEditar_Click(object sender, EventArgs e)
        {

            if (lkpTipoDocumentoingresos.EditValue == null || lkpTipoDocumentoingresos.EditValue == "")
            {
                MessageBox.Show("Debe Ingresar un tipo de Documento", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }
            eTrabajador.eDocumento_Trabajador edoc = new eTrabajador.eDocumento_Trabajador();

            eTrabajador.eDocumento_Trabajador obj = AsignarValor_documentos();
            obj = unit.Trabajador.InsertarActualizar_DocumentoIngreso<eTrabajador.eDocumento_Trabajador>(obj);
            if (obj == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (obj != null)
            {
                MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listarDocumentos(107);
                gvDocIngreso.RefreshData();
                gvDocIngreso_FocusedRowChanged(gvDocIngreso, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));

            }


        }

        private eTrabajador.eDocumento_Trabajador AsignarValor_documentos()
        {
            eTrabajador.eDocumento_Trabajador edoc = new eTrabajador.eDocumento_Trabajador();
            eTrabajador.eDocumento_Trabajador er = gvDocIngreso.GetFocusedRow() as eTrabajador.eDocumento_Trabajador;
            if (btnGuardarDocIngresos.Text == "GUARDAR")
            {
                edoc.cod_item = 0; edoc.flg_documento = "";
                edoc.id_documento = "";
                edoc.dsc_archivo = "";
            }
            else if (btnGuardarDocIngresos.Text == "EDITAR")
            {
                edoc.cod_item = er.cod_item; edoc.flg_documento = flg_documentor;
                edoc.id_documento = id_documentor;
                edoc.dsc_archivo = dsc_archivor;
            }
            edoc.cod_trabajador = cod_trabajador;
            edoc.cod_empresa = cod_empresa;
            if (grdbFlgResultados.SelectedIndex == 0) { edoc.flg_resultados = "APTO"; } else if (grdbFlgResultados.SelectedIndex == 1) { edoc.flg_resultados = "NO APTO"; } else { edoc.flg_resultados = "NO APLICA"; }
            edoc.fch_registro = DateTime.Today;
            edoc.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            edoc.cod_documento = lkpTipoDocumentoingresos.EditValue.ToString();


            return edoc;
        }

        private void gvDocIngreso_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            eTrabajador.eDocumento_Trabajador es = gvDocIngreso.GetFocusedRow() as eTrabajador.eDocumento_Trabajador;
            if (e.FocusedRowHandle >= 0)
                lkpTipoDocumentoingresos.ReadOnly = true;
            listarDocumentos(107);

            unit.Trabajador.CargaCombosLookUp("TipoDocumentor", lkpTipoDocumentoingresos, "cod_documento", "dsc_documento", "", valorDefecto: true, flg_varios: "NO", cod_empresa: cod_empresa);
            Obtenervalor_DocumentoIngresos();
        }

        private async void btnAgregarPDF_Click(object sender, EventArgs e)
        {
            if (btnGuardarDocIngresos.Text == "GUARDAR")
            {
                string tipo = lkpTipoDocumentoingresos.EditValue.ToString();
                eTrabdoc = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(119, cod_documento: tipo);

                await AdjuntarDocumentosVarios(10, "DocumentosIngresos", nombreDocAdicional: eTrabdoc.dsc_abreviatura);
                listarDocumentos(107);
            }
            else if (btnGuardarDocIngresos.Text == "EDITAR")
            {
                try
                {
                    eTrabajador.eDocumento_Trabajador es = gvDocIngreso.GetFocusedRow() as eTrabajador.eDocumento_Trabajador;
                    if (es.dsc_archivo == "")
                    {
                        string tipo = lkpTipoDocumentoingresos.EditValue.ToString();
                        eTrabdoc = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(119, cod_documento: tipo);
                        await AdjuntarDocumentosVarios(10, "DocumentosIngresos", nombreDocAdicional: eTrabdoc.dsc_abreviatura);
                        eTrabajador.eDocumento_Trabajador obj = AsignarValor_documentos();
                        obj = unit.Trabajador.InsertarActualizar_DocumentoIngreso<eTrabajador.eDocumento_Trabajador>(obj); gvDocIngreso.RefreshData();
                        if (obj != null)
                        {
                            MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            listarDocumentos(107);
                        }
                    }
                    else if (MessageBox.Show("¿Desea remplazar Documento?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        string result = "";
                        await Mover_Eliminar_ArchivoOneDrive_Ingresos();
                        result = unit.Trabajador.EliminarInactivarEMOtrabajador(6, cod_trabajador: cod_trabajador, cod_empresa: cod_empresa, cod_documento: es.cod_documento);

                        string tipo = lkpTipoDocumentoingresos.Text;
                        await AdjuntarDocumentosVarios(10, "DocumentosIngresos", tipo);
                        listarDocumentos(107);
                    }
                    gvDocIngreso.RefreshData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        private void gcDocIngreso_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {

                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Obteniendo listado", "Cargando...");
                listarDocumentos(107);

                SplashScreenManager.CloseForm();

            }
        }

        private void gvDocIngreso_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }


        private void gvDocIngreso_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0) gvDocIngreso_FocusedRowChanged(gvDocIngreso, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, e.RowHandle));
            accionr = "EDITAR";
            Obtenervalor_DocumentoIngresos();
            listarDocumentos(107);
            lkpTipoDocumentoingresos.ReadOnly = true;
        }

        private void lkpTipoDocumentoingresos_EditValueChanged(object sender, EventArgs e)
        {
            //eTrabajador.eDocumento_Trabajador er = gvDocIngreso.GetFocusedRow() as eTrabajador.eDocumento_Trabajador;
            //string tipo = lkpTipoDocumentoingresos.EditValue.ToString();

            //if (tipo == er.cod_documento)
            //{
            //    MessageBox.Show("El tipo de documento se encuentra registrado , ¿Desea remplazar datos?.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); btnGuardarDocIngresos.Text = "EDITAR"; return;
            //}

            //else { grdbFlgResultados.EditValue = 0; txtArchivo.Text = ""; btnGuardarDocIngresos.Text = "GUARDAR"; }
        }

        private void btnNuevoDocingreso_Click(object sender, EventArgs e)
        {
            if (gvDocIngreso.RowCount == 0)
            {
                unit.Trabajador.CargaCombosLookUp("TipoDocumentoTIPO", lkpTipoDocumentoingresos, "cod_documento", "dsc_documento", "", valorDefecto: true, flg_varios: "NO", cod_trabajador: cod_trabajador, cod_empresa: cod_empresa);
                lkpTipoDocumentoingresos.ItemIndex = 0;
            }
            else
            {
                unit.Trabajador.CargaCombosLookUp("TipoDocumentor", lkpTipoDocumentoingresos, "cod_documento", "dsc_documento", "", valorDefecto: true, flg_varios: "NO", cod_trabajador: cod_trabajador, cod_empresa: cod_empresa);
            }
            lkpTipoDocumentoingresos.ItemIndex = 0;
            lkpTipoDocumentoingresos.Refresh();
            accionr = "NUEVO";

            txtArchivo.Text = "";
            grdbFlgResultados.SelectedIndex = 0;
            btnGuardarDocIngresos.Text = "GUARDAR";
            lkpTipoDocumentoingresos.ReadOnly = false;




        }


        private async void rbtnEliminardocingre_Click(object sender, EventArgs e)
        {


            try
            {
                if (MessageBox.Show("¿Esta seguro de eliminar el Documento?" + Environment.NewLine + "Esta acción es irreversible.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {//user.cod_usuario
                    eTrabajador.eDocumento_Trabajador es = gvDocIngreso.GetFocusedRow() as eTrabajador.eDocumento_Trabajador;
                    string result = "";
                    if (es.dsc_archivo == "FALTA INGRESAR DOCUMENTO")
                    {
                        result = unit.Trabajador.EliminarInactivarEMOtrabajador(6, cod_trabajador: cod_trabajador, cod_empresa: cod_empresa, cod_documento: es.cod_documento);
                        if (result != "OK") { MessageBox.Show("Error al eliminar registro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                        if (result == "OK") MessageBox.Show("Se procedió a eliminar el registro de manera satisfactoria.", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        await Mover_Eliminar_ArchivoOneDrive_Ingresos();
                        result = unit.Trabajador.EliminarInactivarEMOtrabajador(6, cod_trabajador: cod_trabajador, cod_empresa: cod_empresa, cod_documento: es.cod_documento);
                        if (result != "OK") { MessageBox.Show("Error al eliminar registro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                        if (result == "OK") MessageBox.Show("Se procedió a eliminar el registro de manera satisfactoria.", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    accionr = "NUEVO";
                    txtArchivo.Text = "";
                    grdbFlgResultados.SelectedIndex = 0;
                    unit.Trabajador.CargaCombosLookUp("TipoDocumentor", lkpTipoDocumentoingresos, "cod_documento", "dsc_documento", "", valorDefecto: true, flg_varios: "NO", cod_trabajador: cod_trabajador, cod_empresa: cod_empresa);
                    lkpTipoDocumentoingresos.ItemIndex = 0;
                    btnGuardarDocIngresos.Text = "GUARDAR";
                    lkpTipoDocumentoingresos.ReadOnly = false;
                    listarDocumentos(107);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private async void rbtnEliminar_Click(object sender, EventArgs e)
        {
            eTrabajador.eEMO obj = new eTrabajador.eEMO();
            obj = gvEMO.GetFocusedRow() as eTrabajador.eEMO;
        //    if (obj.dsc_descripcion == "") { MessageBox.Show("Se requiere Adjuntar documento", "Documento no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            List<eVentana> listPerfil = unit.Sistema.ListarPerfilesUsuario<eVentana>(4, Program.Sesion.Usuario.cod_usuario, Program.Sesion.Global.Solucion);
            eVentana oPerfil = listPerfil.Find(x => x.cod_perfil == 13 || x.cod_perfil == 14);

            if (oPerfil == null)
            {
                if (obj.dsc_documento == "CERTIFICADO EMO") { MessageBox.Show("Acceso denegado para Eliminar EMO", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipodoc.Focus(); return; }
                else
                {
                    eliminaremo();
                }
            }
            else
            {
                eliminaremo();
            }

        }
        private  async void eliminaremo()
        {
            eTrabajador.eEMO obj = new eTrabajador.eEMO();
            obj = gvEMO.GetFocusedRow() as eTrabajador.eEMO;
            if (obj.cod_EMO == 0)
            {
                MessageBox.Show("Debe seleccionar una fila", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                string result = "";
                if (obj.nombre_archivoonedrive == "")
                {
                    result = unit.Trabajador.EliminarInactivarEMOtrabajador(1, cod_trabajador: cod_trabajador, cod_empresa: cod_empresa, cod_EMO: obj.cod_EMO, cod_documento: obj.cod_documento);

                }
                else if (obj.nombre_archivoonedrive != "")
                {
                    await Mover_Eliminar_ArchivoOneDrive();
                    result = unit.Trabajador.EliminarInactivarEMOtrabajador(1, cod_trabajador: cod_trabajador, cod_empresa: cod_empresa, cod_EMO: obj.cod_EMO, cod_documento: obj.cod_documento);
                }
                if (result != "OK") { MessageBox.Show("Error al eliminar registro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (result == "OK") MessageBox.Show("Se procedió a eliminar el registro de manera satisfactoria.", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dtFchEvaluacion.EditValue = Convert.ToDateTime(DateTime.Today);
                dtFchEvaluacionObs.EditValue = Convert.ToDateTime(DateTime.Today);
                grdbFlgObservado.SelectedIndex = 0;
                memObservacion.Text = "";
                txtArchivoEmo.Text = "";
                btnGuardarEMO.Text = "GUARDAR";

                ObtenerDatos_EMO();
                gvEMO_FocusedRowChanged(gvEMO, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
            }
        }

        private void btnNuevoEMO_Click(object sender, EventArgs e)
        {

            eTrabajador.eEMO eEMO = new eTrabajador.eEMO();
            accionr = "NUEVO";
            dtFchEvaluacion.EditValue = Convert.ToDateTime(DateTime.Today);
            grdbFlgObservado.SelectedIndex = 0;
            lkpTipodoc.ItemIndex = 0;
            memObservacion.Text = "";
            txtArchivoEmo.Text = "";
            btnGuardarEMO.Text = "GUARDAR";
            lkpTipodoc.ReadOnly = false;
            eEMO.id_certificado = null;
            eEMO.flg_certificado = null;
            dtFchEvaluacion.Properties.UseSystemPasswordChar = false;
            dtFchEvaluacionObs.Properties.UseSystemPasswordChar = false;
            memObservacion.Properties.UseSystemPasswordChar = false;
            txtArchivoEmo.Properties.UseSystemPasswordChar = false;
            btnAdjuntarEMO.Enabled = true;
            btnGuardarEMO.Enabled = true;



        }

        private void gvEMO_CustomDrawColumnHeader_1(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);

        }


        private void gvEMO_RowStyle_1(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }


        private void gvEMO_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0) gvEMO_FocusedRowChanged(gvEMO, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, e.RowHandle));
            accionr = "EDITAR";
            eTrabajador.eEMO er = gvEMO.GetFocusedRow() as eTrabajador.eEMO;
            eTrabajador.eEMO obj = new eTrabajador.eEMO();
            List<eVentana> listPerfil = unit.Sistema.ListarPerfilesUsuario<eVentana>(4, Program.Sesion.Usuario.cod_usuario, Program.Sesion.Global.Solucion);
            eVentana oPerfil = listPerfil.Find(x => x.cod_perfil == 13 || x.cod_perfil == 14);

            if (oPerfil == null)
            {
                if (er.dsc_documento != "CERTIFICADO EMO")
                {
                    dtFchEvaluacion.Properties.UseSystemPasswordChar = false;
                    dtFchEvaluacionObs.Properties.UseSystemPasswordChar = false;
                    memObservacion.Properties.UseSystemPasswordChar = false;
                    txtArchivoEmo.Properties.UseSystemPasswordChar = false;
                    // btnNuevoEMO.Enabled = true;
                    dtFchEvaluacionObs.Enabled = false;
                    lkpTipodoc.ReadOnly = false;
                    btnAdjuntarEMO.Enabled = true; btnGuardarEMO.Enabled = true;
                }
                else if (er.dsc_documento == "CERTIFICADO EMO")
                {
                    dtFchEvaluacion.Properties.UseSystemPasswordChar = true;
                    dtFchEvaluacionObs.Properties.UseSystemPasswordChar = true;
                    memObservacion.Properties.UseSystemPasswordChar = true;
                    txtArchivoEmo.Properties.UseSystemPasswordChar = true;
                    // btnNuevoEMO.Enabled = true;
                    dtFchEvaluacionObs.Enabled = true;
                    lkpTipodoc.ReadOnly = true;
                    btnAdjuntarEMO.Enabled = false; btnGuardarEMO.Enabled = false;

                }
            }
            else
            {
                dtFchEvaluacion.Properties.UseSystemPasswordChar = false;
                dtFchEvaluacionObs.Properties.UseSystemPasswordChar = false;
                memObservacion.Properties.UseSystemPasswordChar = false;
                txtArchivoEmo.Properties.UseSystemPasswordChar = false;
                // btnNuevoEMO.Enabled = true;
                dtFchEvaluacionObs.Enabled = false;
                btnAdjuntarEMO.Enabled = true; btnGuardarEMO.Enabled = true;
            }
            Obtenervalor_EMO();
            ObtenerDatos_EMO();
        }

        private void rowcligvemo()
        {
           
            
        }

        private void lkpTipoDocumentoingresos_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {

        }

        private void btnCheckMultiple_Click(object sender, EventArgs e)
        {
            if (gvDocIngreso.OptionsSelection.MultiSelect == true || gvEMO.OptionsSelection.MultiSelect == true)
            {
                gvDocIngreso.OptionsSelection.MultiSelect = false;
                gvEMO.OptionsSelection.MultiSelect = false;


                return;
            }
            if (gvDocIngreso.OptionsSelection.MultiSelect == false || gvEMO.OptionsSelection.MultiSelect == false)
            {
                gvDocIngreso.OptionsSelection.MultiSelect = true;
                gvEMO.OptionsSelection.MultiSelect = true;
                //gvDocIngreso.Columns["gridColumn5"].Visible = true;
                //gvEMO.Columns["gridColumn6"].Visible = true;
                return;
            }

        }

        private void memObservacion_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void gcEMO_Click_1(object sender, EventArgs e)
        {

        }

        private void rbtnVerPDF_Click(object sender, EventArgs e)
        {
            eTrabajador.eEMO obj = new eTrabajador.eEMO();
            obj = gvEMO.GetFocusedRow() as eTrabajador.eEMO;
            List<eVentana> listPerfil = unit.Sistema.ListarPerfilesUsuario<eVentana>(4, Program.Sesion.Usuario.cod_usuario, Program.Sesion.Global.Solucion);
            eVentana oPerfil = listPerfil.Find(x => x.cod_perfil == 13 || x.cod_perfil == 14);
            
            if (oPerfil == null)
            {
                if (obj.dsc_documento == "CERTIFICADO EMO") { MessageBox.Show("Acceso denegado para visualizar EMO", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipodoc.Focus(); return; }
                else
                {
                    verpdfemo();
                }
            }
            else
            {
                verpdfemo();
            }

        }
        private void verpdfemo()
        {
            eTrabajador.eEMO obj = new eTrabajador.eEMO();
            obj = gvEMO.GetFocusedRow() as eTrabajador.eEMO;
            string codtipo = lkpTipodoc.EditValue.ToString();
            int cod_emoof = 0;
            eTrabdoc = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(109, cod_documento: codtipo);
            etrabemo = unit.Trabajador.Obtener_emo<eTrabajador.eEMO>(114, cod_trabajador: cod_trabajador, cod_empresa: cod_empresa, cod_documento: codtipo, cod_EMO: obj.cod_EMO);
            VerDocumentoDocReg(2, codtipo, cod_d: etrabemo.dsc_descripcion);
        }

        private void gcInfoLaboral_Click(object sender, EventArgs e)
        {

        }

        private void lkpregimensalud_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpregimensalud.EditValue.ToString() == "00002") { lytentidadeps.Visibility = LayoutVisibility.Always;
                unit.Trabajador.CargaCombosLookUp("entidadeps", lkpentiedadeps, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                lkpentiedadeps.ItemIndex = 0;
            }
            else { lytentidadeps.Visibility = LayoutVisibility.Never; }
        }

        private void lkpSCRTsalud_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpSCRTsalud.EditValue.ToString() != "00001") { layoutControlItem263.Visibility = LayoutVisibility.Always; dtFchInicscrtSalud.EditValue = Convert.ToDateTime(fch_scrt); }
            else { layoutControlItem263.Visibility = LayoutVisibility.Never; }
        }

        private void lkpSCRTpension_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpSCRTpension.EditValue.ToString() != "00001") { layoutControlItem272.Visibility = LayoutVisibility.Always; dtFchInicscrtPension.EditValue = Convert.ToDateTime(fch_scrt); }
            else { layoutControlItem272.Visibility = LayoutVisibility.Never; }
        }

        private void lkpRegimenAcademico_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpRegimenAcademico.EditValue.ToString() != null)
            {
                unit.Trabajador.CargaCombosLookUp("TipoInstitucion", lkpTipoInstitucion, "cod_tipoinstitucion", "dsc_tipoinstitucion", "", valorDefecto: true, cod_tiporegimenacademico: lkpRegimenAcademico.EditValue.ToString());
                lkpTipoInstitucion.ItemIndex = 0;
            }
        }

        private void lkpTipoInstitucion_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpTipoInstitucion.EditValue.ToString() != null)
            {
                unit.Trabajador.CargaCombosLookUp("CentrodeEstudios", lkpCentroEstudiosFormAcademic, "cod_centroestudios", "dsc_centroestuidos", "", valorDefecto: true, cod_tiporegimenacademico: lkpRegimenAcademico.EditValue.ToString(), cod_tipoinsitucion: lkpTipoInstitucion.EditValue.ToString());
                lkpCentroEstudiosFormAcademic.ItemIndex = 0;
            }
        }

        private void lkpCentroEstudiosFormAcademic_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpCentroEstudiosFormAcademic.EditValue.ToString() != null)
            {
                unit.Trabajador.CargaCombosLookUp("CarreraProfesionales", lkpCarreraCursoFormAcademic, "cod_carrera_profesional", "dsc_carrera_curso", "", valorDefecto: true, cod_tiporegimenacademico: lkpRegimenAcademico.EditValue == null ? null : lkpRegimenAcademico.EditValue.ToString(), cod_tipoinsitucion: lkpTipoInstitucion.EditValue == null ? null : lkpTipoInstitucion.EditValue.ToString(), cod_centroestudios: lkpCentroEstudiosFormAcademic.EditValue == null ? null : lkpCentroEstudiosFormAcademic.EditValue.ToString());
                lkpCarreraCursoFormAcademic.ItemIndex = 0;
            }
        }

        private void lkpOcupacional_EditValueChanged(object sender, EventArgs e)
        {


            eTrabajador.eInfoLaboral_Trabajador obj = new eTrabajador.eInfoLaboral_Trabajador();
            obj = gvListadoInfoLaboral.GetFocusedRow() as eTrabajador.eInfoLaboral_Trabajador;

            if (lkpOcupacional.EditValue.ToString() != null)
            {
                unit.Trabajador.CargaCombosLookUp("Ocupacionesejecutivas", lkpOcpaciones, "cod_ocupaciones", "dsc_ocupacion", "", valorDefecto: true, cod_ocupacional: lkpOcupacional.EditValue.ToString());
                lkpOcpaciones.ItemIndex = 0;
            }

        }

        private void btnActivar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (btnActivar.Caption == "Activar")
            {
                try
                {
                    txtApellPaterno.Select();
                    if (lkpEmpresa.EditValue == null) { MessageBox.Show("Debe seleccionar una empresa.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpEmpresa.Focus(); return; }
                    if (txtApellPaterno.Text.Trim() == "") { MessageBox.Show("Debe ingresar un apellido paterno.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtApellPaterno.Focus(); return; }
                    if (txtApellMaterno.Text.Trim() == "") { MessageBox.Show("Debe ingresar un apellido materno.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtApellMaterno.Focus(); return; }
                    if (txtNombre.Text.Trim() == "") { MessageBox.Show("Debe ingresar un nombre.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNombre.Focus(); return; }
                    if (dtFecNacimiento.EditValue == null) { MessageBox.Show("Debe ingresar la fecha de nacimiento", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtFecNacimiento.Focus(); return; }
                    if (txtNroDocumento.Text.Trim() == "") { MessageBox.Show("Debe ingresar un número de documento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNroDocumento.Focus(); return; }
                    if (lkpSexo.Text.Trim() == "") { MessageBox.Show("Debe ingresar Sexo.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpSexo.Focus(); return; }
                    if (txtDireccion.Text.Trim() == "") { MessageBox.Show("Debe ingresar una dirección.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtDireccion.Focus(); return; }
                    if (lkpPais.EditValue == null) { MessageBox.Show("Debe seleccionar un país.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpPais.Focus(); return; }
                    if (lkpDepartamento.EditValue == null) { MessageBox.Show("Debe seleccionar un departamento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpDepartamento.Focus(); return; }
                    if (lkpProvincia.EditValue == null) { MessageBox.Show("Debe seleccionar una provincia.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpProvincia.Focus(); return; }
                    if (lkpDistrito.EditValue == null) { MessageBox.Show("Debe seleccionar un distrito.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpDistrito.Focus(); return; }
                    if (lkpNacionalidad.EditValue == null) { MessageBox.Show("Debe seleccionar la nacionalidad.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpNacionalidad.Focus(); return; }
                    if (txtCelular.Text.Trim() == "") { MessageBox.Show("Debe ingresar número de celular.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCelular.Focus(); return; }
                    if (txtNroDocumento.EditValue != null)
                    {
                        eProveedor obj = new eProveedor();
                        obj = unit.Proveedores.Validar_NroDocumento<eProveedor>(19, "", glkpTipoDocumento.EditValue.ToString());
                        int nrodocumento = Convert.ToInt32(txtNroDocumento.Text.Length);
                        if (nrodocumento < obj.ctd_digitos) { MessageBox.Show("Los digitos del documento debe ser igual a " + obj.ctd_digitos, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCelular.Focus(); return; }
                        txtNroDocumento.Properties.MaxLength = obj.ctd_digitos;
                    }
                    eTrab = AsignarValores_Trabajador();
                    eTrab = unit.Trabajador.InsertarActualizar_Trabajador<eTrabajador>(eTrab);
                    if (eTrab == null) { MessageBox.Show("Error al activar Trabajador.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    cod_trabajador = eTrab.cod_trabajador; cod_empresa = eTrab.cod_empresa;

                    if (eTrab != null)
                    {
                        ActualizarListado = "SI";
                        if (frmHandler != null)
                        {
                            int nRow = frmHandler.gvListadoTrabajador.FocusedRowHandle;
                            frmHandler.frmListadoTrabajador_KeyDown(frmHandler, new KeyEventArgs(Keys.F5));
                            frmHandler.gvListadoTrabajador.FocusedRowHandle = nRow;
                            //frmHandler.CargarOpcionesMenu();
                        }

                        MiAccion = Trabajador.Editar;
                        MessageBox.Show("Se Activo Trabajador ", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //Inicializar();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else if (btnActivar.Caption == "Cesar Trabajador")
            {
                frmObservacionesBajaUsuario frm = new frmObservacionesBajaUsuario();
                string result = "";
                eTrabajador trab = new eTrabajador();
                frm.estado = "NO"; frm.MiAccion = Cese.Nuevo;
                frm.empresa = cod_empresa;
                frm.trabajador = cod_trabajador;
                frm.ShowDialog();

                ActualizarListado = "SI";
                MiAccion = Trabajador.Editar;
                if (frmHandler != null)
                {
                    int nRow = frmHandler.gvListadoTrabajador.FocusedRowHandle;
                    frmHandler.frmListadoTrabajador_KeyDown(frmHandler, new KeyEventArgs(Keys.F5));
                    frmHandler.gvListadoTrabajador.FocusedRowHandle = nRow;
                    //frmHandler.CargarOpcionesMenu();
                }
                Inicializar();

            }


        }

        private void gvDocIngreso_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void chkflgEducComplPeru_CheckStateChanged(object sender, EventArgs e)
        {
            
           // chkflgEducComplPeru.CheckState == CheckState.Checked ? true : false;
        }

        private void txtTelefono_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            if (Application.OpenForms["frmObservacionesBajaUsuario"] != null)
            {
                Application.OpenForms["frmObservacionesBajaUsuario"].Activate();
            }
            else
            {
                frmObservacionesBajaUsuario frm = new frmObservacionesBajaUsuario();
                frm.trabajador = cod_trabajador;
                frm.empresa = cod_empresa;
                frm.MiAccion = Cese.Editar;
                frm.ShowDialog();

            }
        }

        private void btnGuardarSeguimiento_Click(object sender, EventArgs e)
        {
            GuardarSeguimiento();
        }


        private void gvListadoTrabajadorInfoFormato_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            var obj = gvListadoTrabajadorInfoFormato.GetFocusedRow() as eTrabajador_InfoFormatoMD_Vista;
            if (obj == null) return;

            lblEmitirFormato.Text = obj.dsc_formatoMD_vinculo.ToString();
        }

        private void gvListadoDocumentoSeguimiento_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

        }
        private void btnVistaPreviaDocumento_Click(object sender, EventArgs e)
        {
            if (!(gvListadoTrabajadorInfoFormato.GetFocusedRow() is eTrabajador_InfoFormatoMD_Vista obj)) return;

            lblEmitirFormato.Text = obj.dsc_formatoMD_vinculo.ToString();
            lblEmitirFormato.Tag = obj;
            //var obj = gvListadoTrabajadorInfoFormato.GetFocusedRow() as eTrabajador_InfoFormatoMD_Vista;
            //if (lblEmitirFormato.Tag == null) return;

            lciOcultarPlantilla.Visibility = LayoutVisibility.Always;
            AdministrarButtonDocumentacion(true);
            VisualizarDocumentoPlantilla();
        }
        private void VisualizarDocumentoPlantilla()
        {
            if (!(lblEmitirFormato.Tag is eTrabajador_InfoFormatoMD_Vista obj)) return;

            unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Renderizando plantilla", "Cargando...");
            var help = new FormatoMDHelper(recPlantilla);
            help.MostrarDocumento_RichEditControl(obj.cod_formatoMD_vinculo, cod_empresa);
            help.MostrarDescripcion_Parametros(cod_empresa, cod_trabajador, _listadoParametros);

            SplashScreenManager.CloseForm();
        }
        private void AdministrarButtonDocumentacion(bool value)
        {
            groupPlantilla.Visibility = value ? LayoutVisibility.Always : LayoutVisibility.Never;
            //lciOcultarPlantilla.Visibility = value ? LayoutVisibility.Always : LayoutVisibility.Never;
            lciDescargaFormatoPDF.Visibility = value ? LayoutVisibility.Always : LayoutVisibility.Never;
            lciEnviarMail.Visibility = value ? LayoutVisibility.Always : LayoutVisibility.Never;
            lciImprimirDocumentos.Visibility = value ? LayoutVisibility.Always : LayoutVisibility.Never;
            // lciVistaPrevia.Visibility = !value ? LayoutVisibility.Always : LayoutVisibility.Never;
            groupSeguimiento.Visibility = !value ? LayoutVisibility.Always : LayoutVisibility.Never;
            recPlantilla.Text = "";
        }
        private void btnOcultarVistaPreviaDocumento_Click(object sender, EventArgs e)
        {
            lciOcultarPlantilla.Visibility = LayoutVisibility.Never;
            AdministrarButtonDocumentacion(false);
        }

        private void btnImprimirDocumentoEmitido_Click(object sender, EventArgs e)
        {
            if (lblEmitirFormato.Tag == null) return;

            var frm = new frmImpresora();
            frm.CargarImpresoras();


            frm.ShowDialog();
            unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Procesando impresión", "Cargando...");
            if (frm.Result == DialogResult.OK)
            {
                //   MatrizEnvioImpresion("imprimir", trabajadorCheck, documentoCheck);
                new FormatoMDHelper().ImpresionSimple(recPlantilla.WordMLText, frm.txtCopia.Text);
            }
            SplashScreenManager.CloseForm();
        }

        private void btnDescargarPDFDocumentoEmitido_Click(object sender, EventArgs e)
        {
            exportarFormatoDocuemento_PDF(true);
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


            if (!(lblEmitirFormato.Tag is eTrabajador_InfoFormatoMD_Vista obj)) return "";

            var folder = $@"{unit.Globales.GetAppVariableValor("RutaArchivosLocalExportar")}\";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            //Preguntar con que nombre se va a descargar el archivo.
            string file = $"{obj.cod_empresa}-{obj.cod_trabajador}-{obj.dsc_formatoMD_vinculo.ToLower()}";
            string path = $"{folder}{file}.pdf";
            recPlantilla.ExportToPdf(path, options);

            if (openFolder) Process.Start(folder);
            return path;
        }
        private void btnDocumentoEnviarMail_Click(object sender, EventArgs e)
        {
            if (!(lblEmitirFormato.Tag is eTrabajador_InfoFormatoMD_Vista obj)) return;

            string path = exportarFormatoDocuemento_PDF();
            var frm = new frmFormatoEmail();
            frm.Text = "Formato de Correos";
            frm.CargarInfo(mailSplit: $"{txtEmail1.Text.Trim()},{txtEmail2.Text.Trim()}",
                adjuntoSplit: path,
                dscTrabajador: lblNombreTrabajador.Text,
                dscDocumento: obj.dsc_formatoMD_vinculo,
                codEmpresa: cod_empresa);
            frm.ShowDialog();
        }
        #endregion Fin espacio para formato de documentos()

        private async Task AdjuntarDocumentoFamiliar(int opcionDoc, string nombreDoc, string nombreDocAdicional = "")
        {
            try
            {
                DateTime FechaRegistro = DateTime.Today;
                string nombreCarpeta = "";

                eTrabajador.eInfoLaboral_Trabajador resultado = unit.Trabajador.Obtener_Familiar<eTrabajador.eInfoLaboral_Trabajador>(2, cod_trabajador, cod_empresa, cod_infofamiliar: Convert.ToInt32(txtCodInfoFamiliar.Text));
                if (resultado == null) { MessageBox.Show("Antes de adjuntar los docuentos debe crear al familiar.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                OpenFileDialog myFileDialog = new OpenFileDialog();
                myFileDialog.Filter = "Archivos (*.pdf)|; *.pdf";
                myFileDialog.FilterIndex = 1;
                myFileDialog.InitialDirectory = "C:\\";
                myFileDialog.Title = "Abrir archivo";
                myFileDialog.CheckFileExists = false;
                myFileDialog.Multiselect = false;

                DialogResult result = myFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string IdCarpetaTrabajador = "", Extension = "";
                    var idArchivoPDF = "";
                    var TamañoDoc = new FileInfo(myFileDialog.FileName).Length / 1024;
                    if (TamañoDoc < 4000)
                    {
                        varPathOrigen = myFileDialog.FileName;
                        //varNombreArchivo = nombreDoc + "-" + FechaRegistro.ToString("yyyyMMdd") + "."+ Path.GetExtension(myFileDialog.SafeFileName);
                        varNombreArchivo = nombreDoc + "-" + FechaRegistro.Year.ToString() + "." + FechaRegistro.ToString("MM") + "." + FechaRegistro.ToString("dd") + (nombreDocAdicional != "" ? " " + nombreDocAdicional : "") + Path.GetExtension(myFileDialog.SafeFileName);
                        //varNombreArchivoSinExtension = nombreDoc + "-" + FechaRegistro.ToString("yyyyMMdd");
                        Extension = Path.GetExtension(myFileDialog.SafeFileName);
                    }
                    else
                    {
                        MessageBox.Show("Solo puede subir archivos hasta 4MB de tamaño", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Por favor espere...", "Cargando...");
                    eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, cod_empresa);
                    if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
                    { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                    ClientId = eEmp.ClientIdOnedrive;
                    TenantId = eEmp.TenantOnedrive;
                    Appl();
                    var app = PublicClientApp;
                    string correo = eEmp.UsuarioOnedrivePersonal;
                    string password = eEmp.ClaveOnedrivePersonal;

                    var securePassword = new SecureString();
                    foreach (char c in password)
                        securePassword.AppendChar(c);

                    authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

                    GraphClient = new Microsoft.Graph.GraphServiceClient(
                      new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                      {
                          requestMessage
                              .Headers
                              .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                          return Task.FromResult(0);
                      }));

                    eEmpresa.eOnedrive_Empresa eDatos = new eEmpresa.eOnedrive_Empresa();
                    eDatos = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa.eOnedrive_Empresa>(13, cod_empresa, dsc_Carpeta: "Personal");
                    var targetItemFolderId = eDatos.idCarpeta;
                    nombreCarpeta = resultado.dsc_documento + " - " + resultado.dsc_nombres_completos;
                    eTrabajador objCarpeta = unit.Trabajador.ObtenerDatosOneDrive<eTrabajador>(14, cod_trabajador: cod_trabajador);
                    if (objCarpeta.idCarpeta_Trabajador == null || objCarpeta.idCarpeta_Trabajador == "") //Si no existe folder lo crea
                    {
                        var driveItem = new Microsoft.Graph.DriveItem
                        {
                            Name = nombreCarpeta,
                            Folder = new Microsoft.Graph.Folder
                            {
                            },
                            AdditionalData = new Dictionary<string, object>()
                            {
                            {"@microsoft.graph.conflictBehavior", "rename"}
                            }
                        };

                        var driveItemInfo = await GraphClient.Me.Drive.Items[targetItemFolderId].Children.Request().AddAsync(driveItem);
                        IdCarpetaTrabajador = driveItemInfo.Id;
                    }
                    else //Si existe folder obtener id
                    {
                        IdCarpetaTrabajador = objCarpeta.idCarpeta_Trabajador;
                    }

                    //crea archivo en el OneDrive
                    byte[] data = System.IO.File.ReadAllBytes(varPathOrigen);
                    using (Stream stream = new MemoryStream(data))
                    {
                        string res = "";
                        var DriveItem = await GraphClient.Me.Drive.Items[IdCarpetaTrabajador].ItemWithPath(varNombreArchivo).Content.Request().PutAsync<Microsoft.Graph.DriveItem>(stream);
                        idArchivoPDF = DriveItem.Id;

                        eTrabajador.eInfoFamiliar_Trabajador objTrab = new eTrabajador.eInfoFamiliar_Trabajador();
                        objTrab.cod_trabajador = cod_trabajador;
                        objTrab.cod_empresa = cod_empresa;
                        objTrab.cod_infofamiliar = Convert.ToInt32(txtCodInfoFamiliar.Text);
                        objTrab.idCarpeta_Trabajador = IdCarpetaTrabajador;
                        objTrab.id_DNI_documentofam = opcionDoc == 1 ? idArchivoPDF : null;
                        objTrab.id_CERTIFICADOESTUDIOS_documentofam = opcionDoc == 2 ? idArchivoPDF : null;
                        if(objTrab.flg_CERTIFICADOESTUDIOS_documentofam !=null) objTrab.flg_CERTIFICADOESTUDIOS_documentofam = "SI";
                        if(objTrab.flg_DNI_documentofam != null) objTrab.flg_DNI_documentofam = "SI";


                        chkDNIFAMILIAR.CheckState = opcionDoc == 1 ? CheckState.Checked : chkFlgDNI.CheckState;
                        chkCERTIFICADOESTUDIOS.CheckState = opcionDoc == 2 ? CheckState.Checked : chkFlgCV.CheckState;

                        if (opcionDoc <= 3) res = unit.Trabajador.ActualizarInformacionDocumentosFamilia("SI", opcionDoc, objTrab);
                        if (res == "OK")
                        {
                            MessageBox.Show("Se registró el documento satisfactoriamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Hubieron problemas al registrar el documento", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    SplashScreenManager.CloseForm();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void bloqueo()
        {

            txtApellPaterno.Enabled = true;
            txtApellMaterno.Enabled = true;
            //txtNombre.Enabled = true;
            lkpEstadoCivil.Enabled = true;
            lkpSexo.Enabled = true;
            lkpEmpresa.Enabled = true;
            txtDireccion.Enabled = true;
            txtReferencia.Enabled = true;
            lkpPais.Enabled = true;
            dtFecNacimiento.Enabled = true;
            lkpDepartamento.Enabled = true;
            lkpProvincia.Enabled = true;
            lkpDistrito.Enabled = true;
            lkptipovia.Enabled = true;
            txtnombrevia.Enabled = true;
            txtnro.Enabled = true;
            txtdep.Enabled = true;
            txtinterior.Enabled = true;
            txtmz.Enabled = true;
            txtlote.Enabled = true;
            txtkm.Enabled = true;
            txtetapa.Enabled = true;
            txtblock.Enabled = true;
            lkptipozona.Enabled = true;
            txtnombrezona.Enabled = true;
            txtTelefono.Enabled = true;
            txtCelular.Enabled = true;
            txtEmail1.Enabled = true;
            txtEmail2.Enabled = true;
            dtFecVctoDocumento.Enabled = true;
            txtNroUbigeoDocumento.Enabled = true;
            lkpNacionalidad.Enabled = true;
            chkFlgDNI.Enabled = true;
            chkFlgCV.Enabled = true;
            chkFlgAntPenales.Enabled = true;
            chkFlgVerifDomiciliaria.Enabled = true;
            chkFlgAntPoliciales.Enabled = true;
            grdbTipoPersonal.Enabled = true;
            dtEntregaUnif.Enabled = true;
            dtRenovacionUnif.Enabled = true;
            acctlMenu.Enabled = true;



        }

        private void desbloqueo()
        {

            txtApellPaterno.Enabled = false;
            txtApellMaterno.Enabled = false;
            txtNombre.Enabled = false;
            dtFecNacimiento.Enabled = false;
            lkpEstadoCivil.Enabled = false;
            lkpSexo.Enabled = false;
            lkpEmpresa.Enabled = false;
            txtDireccion.Enabled = false;
            txtReferencia.Enabled = false;
            lkpPais.Enabled = false;
            lkpDepartamento.Enabled = false;
            lkpProvincia.Enabled = false;
            lkpDistrito.Enabled = false;
            lkptipovia.Enabled = false;
            txtnombrevia.Enabled = false;
            txtnro.Enabled = false;
            txtdep.Enabled = false;
            txtinterior.Enabled = false;
            txtmz.Enabled = false;
            txtlote.Enabled = false;
            txtkm.Enabled = false;
            txtetapa.Enabled = false;
            txtblock.Enabled = false;
            lkptipozona.Enabled = false;
            txtnombrezona.Enabled = false;
            txtTelefono.Enabled = false;
            txtCelular.Enabled = false;
            txtEmail1.Enabled = false;
            txtEmail2.Enabled = false;
            dtFecVctoDocumento.Enabled = false;
            txtNroUbigeoDocumento.Enabled = false;
            lkpNacionalidad.Enabled = false;
            chkFlgDNI.Enabled = false;
            chkFlgCV.Enabled = false;
            chkFlgAntPenales.Enabled = false;
            chkFlgVerifDomiciliaria.Enabled = false;
            chkFlgAntPoliciales.Enabled = false;
            grdbTipoPersonal.Enabled = false;
            dtEntregaUnif.Enabled = false;
            dtRenovacionUnif.Enabled = false;
            acctlMenu.Enabled = false;



        }

        private void Direccion(string tipo_zona = "", string nombre_zona = "", string tipo_via = "", string nombre_via = "",
            string nro = "", string dep = "", string interior = "", string mz = "", string lote = "", string km = "", string etapa = "",
            string block = "", string departamento = "", string provincia = "", string distrito = "", string referencia = "",string ubigeo="")
        {
            txtDireccion.Text = "";

            tipo_zona = tipo_zona != "" ? tipo_zona + " " : tipo_zona;
            nombre_zona = nombre_zona != "" ? nombre_zona + " " : nombre_zona;
            tipo_via = tipo_via != "" ? tipo_via + " " : tipo_via;

            string nrodi = " N°" + nro;
            if (nro == "") { nrodi = ""; }

            string depdi = " DPTO." + dep;
            if (dep == "") { depdi = ""; }
            string intdir = " INT." + interior;
            if (interior == "") { intdir = ""; }
            string mzdir = " MZ." + mz;
            if (mz == "") { mzdir = ""; }

            string lotedir = " LOTE." + lote;
            if (lote == "") { lotedir = ""; }

            string kmdir = " KM." + km;
            if (km == "") { kmdir = ""; }
            string etapdir = " ETAPA." + etapa;
            if (etapa == "") { etapdir = ""; }
            string blockdir = " BLOCK." + block;

            if (block == "") blockdir = "";

            string referenciadir = "REF:" + referencia;
            if (referencia == "") { referenciadir = ""; }
            string departamentodir = " - " + departamento;
            if (departamento == "") { departamentodir = ""; }
            string provinciadir = " - " + provincia;
            if (provincia == "") { provinciadir = ""; }
            string distritodir = " - " + distrito;
            if (distrito == "") { distritodir = ""; }

            txtDireccion.Text = tipo_zona + nombre_zona + tipo_via + nombre_via + nrodi + depdi +
            intdir + mzdir + lotedir + kmdir + etapdir + blockdir + departamentodir +
            provinciadir + distritodir + "\r\n" + referenciadir;

            
        }

        private async void VerDocumentoEMO(int opcionDoc, string nombreDoc)
        {
            eTrabajador.eEMO obj = new eTrabajador.eEMO();
            obj = gvEMO.GetFocusedRow() as eTrabajador.eEMO;
            string cod_emo = Convert.ToString(obj.cod_EMO);
            if (obj == null) return;
            switch (opcionDoc)
            {
                case 1: if (obj.id_certificado == null || obj.id_certificado == "") return; break;
            }
            eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, cod_empresa);
            if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
            { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            //var app = App.PublicClientApp;
            ClientId = eEmp.ClientIdOnedrive;
            TenantId = eEmp.TenantOnedrive;
            Appl();
            var app = PublicClientApp;

            try
            {
                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Abriendo documento", "Cargando...");
                //eEmpresa eEmp = unit.Factura.ObtenerDatosEmpresa<eEmpresa>(12, obj.cod_empresa);
                string correo = eEmp.UsuarioOnedrivePersonal;
                string password = eEmp.ClaveOnedrivePersonal;

                var securePassword = new SecureString();
                foreach (char c in password)
                    securePassword.AppendChar(c);

                authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

                GraphClient = new Microsoft.Graph.GraphServiceClient(
                new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                {
                    requestMessage
                        .Headers
                        .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                    return Task.FromResult(0);
                }));

                string IdOneDriveDoc = obj.id_certificado;
                string Extension = ".pdf";

                var fileContent = await GraphClient.Me.Drive.Items[IdOneDriveDoc].Content.Request().GetAsync();
                string ruta = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()) + @"\" + (nombreDoc + txtNroDocumento.Text + Extension);
                if (!System.IO.File.Exists(ruta))
                {
                    using (var fileStream = new FileStream(ruta, FileMode.Create, System.IO.FileAccess.Write))
                        fileContent.CopyTo(fileStream);
                }

                if (!System.IO.Directory.Exists(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()))) System.IO.Directory.CreateDirectory(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()));
                System.Diagnostics.Process.Start(ruta);
                SplashScreenManager.CloseForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubieron problemas al autenticar las credenciales", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lblResultado.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                return;
            }
        }

        private async void VerDocumentoDocReg(int opcionDoc, string nombreDoc, string cod_d = "")
        {
            if (cod_d == "") { MessageBox.Show("Se requiere Adjuntar documento", "Documento no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            if (opcionDoc == 1)
            {
                eTrabajador.eDocumento_Trabajador obj = new eTrabajador.eDocumento_Trabajador();
                obj = gvDocIngreso.GetFocusedRow() as eTrabajador.eDocumento_Trabajador;
                string cod_documento = Convert.ToString(obj.cod_documento);
                if (obj == null) return;
                switch (opcionDoc)
                {
                    case 1: if (obj.id_documento == null || obj.id_documento == "") return; break;
                }
                eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, cod_empresa);
                if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
                { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                //var app = App.PublicClientApp;
                ClientId = eEmp.ClientIdOnedrive;
                TenantId = eEmp.TenantOnedrive;
                Appl();
                var app = PublicClientApp;

                try
                {
                    unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Abriendo documento", "Cargando...");
                    //eEmpresa eEmp = unit.Factura.ObtenerDatosEmpresa<eEmpresa>(12, obj.cod_empresa);
                    string correo = eEmp.UsuarioOnedrivePersonal;
                    string password = eEmp.ClaveOnedrivePersonal;

                    var securePassword = new SecureString();
                    foreach (char c in password)
                        securePassword.AppendChar(c);

                    authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

                    GraphClient = new Microsoft.Graph.GraphServiceClient(
                    new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                    {
                        requestMessage
                            .Headers
                            .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                        return Task.FromResult(0);
                    }));

                    string IdOneDriveDoc = obj.id_documento;
                    string Extension = ".pdf";

                    var fileContent = await GraphClient.Me.Drive.Items[IdOneDriveDoc].Content.Request().GetAsync();
                    string ruta = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()) + @"\" + (cod_d) + Extension;
                    if (!System.IO.File.Exists(ruta))
                    {
                        using (var fileStream = new FileStream(ruta, FileMode.Create, System.IO.FileAccess.Write))
                            fileContent.CopyTo(fileStream);
                    }

                    if (!System.IO.Directory.Exists(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()))) System.IO.Directory.CreateDirectory(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()));
                    System.Diagnostics.Process.Start(ruta);
                    SplashScreenManager.CloseForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hubieron problemas al autenticar las credenciales", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lblResultado.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                    return;
                }
            }
            else if (opcionDoc == 2)
            {
                eTrabajador.eEMO obj = new eTrabajador.eEMO();
                obj = gvEMO.GetFocusedRow() as eTrabajador.eEMO;

                string cod_documento = Convert.ToString(obj.cod_documento);
                if (obj == null) return;
                switch (opcionDoc)
                {
                    case 2: if (obj.id_certificado == null || obj.id_certificado == "") return; break;
                }
                eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, cod_empresa);
                if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
                { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                //var app = App.PublicClientApp;
                ClientId = eEmp.ClientIdOnedrive;
                TenantId = eEmp.TenantOnedrive;
                Appl();
                var app = PublicClientApp;

                try
                {
                    unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Abriendo documento", "Cargando...");
                    //eEmpresa eEmp = unit.Factura.ObtenerDatosEmpresa<eEmpresa>(12, obj.cod_empresa);
                    string correo = eEmp.UsuarioOnedrivePersonal;
                    string password = eEmp.ClaveOnedrivePersonal;

                    var securePassword = new SecureString();
                    foreach (char c in password)
                        securePassword.AppendChar(c);

                    authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

                    GraphClient = new Microsoft.Graph.GraphServiceClient(
                    new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                    {
                        requestMessage
                            .Headers
                            .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                        return Task.FromResult(0);
                    }));

                    string IdOneDriveDoc = obj.id_certificado;
                    string Extension = ".pdf";

                    var fileContent = await GraphClient.Me.Drive.Items[IdOneDriveDoc].Content.Request().GetAsync();
                    string ruta = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()) + @"\" + (cod_d) + Extension;
                    if (!System.IO.File.Exists(ruta))
                    {
                        using (var fileStream = new FileStream(ruta, FileMode.Create, System.IO.FileAccess.Write))
                            fileContent.CopyTo(fileStream);
                    }

                    if (!System.IO.Directory.Exists(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()))) System.IO.Directory.CreateDirectory(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()));
                    System.Diagnostics.Process.Start(ruta);
                    SplashScreenManager.CloseForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hubieron problemas al autenticar las credenciales", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lblResultado.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                    return;
                }
            }
        }

        private void listarDocumentos(int opcion)
        {

            ListTrabajdor = unit.Trabajador.ListarArea<eTrabajador.eDocumento_Trabajador>(opcion, cod_empresa: cod_empresa, flg_varios: "NO");
            bsDocIngresos.DataSource = ListTrabajdor;
            gvDocIngreso.RefreshData();
        }

        private void Obtenervalor_DocumentoIngresos()
        {

            eTrabajador.eDocumento_Trabajador obj = new eTrabajador.eDocumento_Trabajador();
            obj = gvDocIngreso.GetFocusedRow() as eTrabajador.eDocumento_Trabajador;
            if (obj == null) return;
            else
            {
                if (obj.flg_resultados == "APTO") { grdbFlgResultados.SelectedIndex = 0; }
                if (obj.flg_resultados == "NO APTO") { grdbFlgResultados.SelectedIndex = 1; }
                if (obj.flg_resultados == "NO APLICA") { grdbFlgResultados.SelectedIndex = 2; }
                //dtFchEvaluacionObs.EditValue = obj.fch_evaluacion_obs;
                //dtFchEvaluacion.EditValue = obj.fch_evaluacion;
                if (obj.dsc_archivo == "") { txtArchivo.Text = "FALTA INGRESAR DOCUMENTO"; txtArchivo.ForeColor = Color.Red; }
                else
                {
                    txtArchivo.Text = obj.dsc_archivo + ".pdf"; txtArchivo.ForeColor = Color.Black;
                }
                lkpTipoDocumentoingresos.EditValue = obj.cod_documento;

                btnGuardarDocIngresos.Text = "EDITAR";
            }


        }


        private async Task Mover_Eliminar_ArchivoOneDrive()
        {
            DateTime FechaRegistro = DateTime.Today;
            try
            {


                eTrabajador.eEMO obj = new eTrabajador.eEMO();

                obj = gvEMO.GetFocusedRow() as eTrabajador.eEMO;
                string IdCarpetaTrabajador = "", Extension = "";
                if (obj == null) { return; }

                eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, cod_empresa);
                if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
                { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }


                ClientId = eEmp.ClientIdOnedrive;
                TenantId = eEmp.TenantOnedrive;
                Appl();
                var app = PublicClientApp;
                string correo = eEmp.UsuarioOnedrivePersonal;
                string password = eEmp.ClaveOnedrivePersonal;

                var securePassword = new SecureString();
                foreach (char c in password)
                    securePassword.AppendChar(c);

                authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

                GraphClient = new Microsoft.Graph.GraphServiceClient(
                  new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                  {
                      requestMessage
                          .Headers
                          .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                      return Task.FromResult(0);
                  }));

                eEmpresa.eOnedrive_Empresa eDatos = new eEmpresa.eOnedrive_Empresa();
                eDatos = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa.eOnedrive_Empresa>(13, cod_empresa, dsc_Carpeta: "Personal");
                var targetItemFolderId = eDatos.idCarpeta;
                var targetItemFolderIdLote = eDatos.idCarpetaAnho;
                eTrab = unit.Trabajador.Obtener_emo<eTrabajador.eEMO>(111, cod_trabajador, cod_empresa, cod_EMO: obj.cod_EMO, cod_documento: obj.cod_documento);
                string doc = lkpTipodoc.EditValue.ToString();
                etrabdocue = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(109, cod_empresa, cod_documento: doc);

                varNombreArchivoSinExtension = etrabdocue.dsc_abreviatura + FechaRegistro.Year.ToString() + FechaRegistro.ToString("MM") + FechaRegistro.ToString("dd");

                await GraphClient.Me.Drive.Items[obj.id_certificado].Request().DeleteAsync();


            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                throw;
            }
        }


        private async Task Mover_Eliminar_ArchivoOneDrive_Ingresos()
        {
            try
            {
                eTrabajador.eDocumento_Trabajador obj = new eTrabajador.eDocumento_Trabajador();

                obj = gvDocIngreso.GetFocusedRow() as eTrabajador.eDocumento_Trabajador;
                string IdCarpetaTrabajador = "", Extension = "";
                if (obj == null) { return; }

                eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, cod_empresa);
                if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
                { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }


                ClientId = eEmp.ClientIdOnedrive;
                TenantId = eEmp.TenantOnedrive;
                Appl();
                var app = PublicClientApp;
                string correo = eEmp.UsuarioOnedrivePersonal;
                string password = eEmp.ClaveOnedrivePersonal;

                var securePassword = new SecureString();
                foreach (char c in password)
                    securePassword.AppendChar(c);

                authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

                GraphClient = new Microsoft.Graph.GraphServiceClient(
                  new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                  {
                      requestMessage
                          .Headers
                          .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                      return Task.FromResult(0);
                  }));

                eEmpresa.eOnedrive_Empresa eDatos = new eEmpresa.eOnedrive_Empresa();
                eDatos = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa.eOnedrive_Empresa>(13, cod_empresa, dsc_Carpeta: "Personal");
                var targetItemFolderId = eDatos.idCarpeta;
                var targetItemFolderIdLote = eDatos.idCarpetaAnho;



                eTrab = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(111, cod_trabajador, cod_empresa, cod_item: obj.cod_item, cod_documento: obj.cod_documento);
                var driveItem = new Microsoft.Graph.DriveItem
                {

                    Name = obj.dsc_archivo,
                    Folder = new Microsoft.Graph.Folder
                    {
                    },
                    AdditionalData = new Dictionary<string, object>()
                        {
                        {"@microsoft.graph.conflictBehavior", "rename"}
                        }
                };

                var driveItemInfo = await GraphClient.Me.Drive.Items[targetItemFolderId].Children.Request().AddAsync(driveItem);
                IdCarpetaTrabajador = driveItemInfo.Id;

                await GraphClient.Me.Drive.Items[obj.id_documento].Request().DeleteAsync();

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                throw;
            }
        }

        private async Task Renombrar_ArchivoOneDrive(eTrabajador.eEMO obj)
        {
            try
            {
                eTrabajador.eEMO objw = gvEMO.GetFocusedRow() as eTrabajador.eEMO;
                etrabemo = unit.Trabajador.Obtener_emo<eTrabajador.eEMO>(113, cod_trabajador: obj.cod_trabajador, cod_empresa: obj.cod_empresa, cod_documento: obj.cod_documento);
                eTrabdoc = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(109, cod_documento: obj.cod_documento);
                DateTime FechaRegistro = DateTime.Today;

                varNombreArchivo = eTrabdoc.dsc_abreviatura + "-" + FechaRegistro.Year.ToString() + "." + FechaRegistro.ToString("MM") + "." + FechaRegistro.ToString("dd") + "_" + objw.cod_EMO;

                eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, obj.cod_empresa);

                if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
                { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                ClientId = eEmp.ClientIdOnedrive;
                TenantId = eEmp.TenantOnedrive;
                Appl();
                var app = PublicClientApp;
                ////var app = App.PublicClientApp;
                string correo = eEmp.UsuarioOnedrivePersonal;
                string password = eEmp.ClaveOnedrivePersonal;

                var securePassword = new SecureString();
                foreach (char c in password)
                    securePassword.AppendChar(c);

                authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

                GraphClient = new Microsoft.Graph.GraphServiceClient(
                new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                {
                    requestMessage
                    .Headers
                    .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                    return Task.FromResult(0);
                }));

                string extension = ".pdf";
                string idArchivo = obj.id_certificado;

                //////////////////////////////////////////////////////// RENOMBRAR DOCUMENTO DE ONEDRIVE ////////////////////////////////////////////////////////
                GraphClient = new Microsoft.Graph.GraphServiceClient(
                    new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                    {
                        requestMessage
                            .Headers
                            .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                        return Task.FromResult(0);
                    }));



                var driveItem = new Microsoft.Graph.DriveItem
                {
                    Name = varNombreArchivo + extension
                };



                await GraphClient.Me.Drive.Items[idArchivo]
                    .Request()
                    .UpdateAsync(driveItem);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                dsc_archivor = varNombreArchivo;
                dsc_documento = varNombreArchivo;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void listarcampos()
        {
            listadocampos = unit.Sistema.Listar_SubModulos<eSubModulo.eCampos>(4, cod_empresa,"005",0);
            listabloqueadoscampos.AddRange(listadocampos);

        }

        public void RecorrerControles(Control container, List<string> names)
        {
                foreach (Control control in container.Controls)
                {
                    if (control is TextEdit textEdit)
                    {
                        names.Add(textEdit.Name);
                    }
                    else if (control.Controls.Count > 0)
                    {
                        RecorrerControles(control, names);
                    }
                }
        }


        private void restrinccionescontroles(string name)
        {
            foreach (var item in listadocampos)
            {

            }
        }


    }
}