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
    internal enum Trabajador_vista
    {
        Nuevo = 0,
        Editar = 1,
        Vista = 2
    }
    public partial class frmVista : DevExpress.XtraEditors.XtraForm
    {
        private readonly UnitOfWork unit;
        public eTrabajador eTrab = new eTrabajador();
        public eTrabajador eTrabV = new eTrabajador();
        public eTrabajador.eInfoSalud_Trabajador eTrabsalud = new eTrabajador.eInfoSalud_Trabajador();
        public eTrabajador.eInfoBancaria_Trabajador etrabBanco = new eTrabajador.eInfoBancaria_Trabajador();
        public string cod_trabajador = "", cod_empresa = "", sede_empresa = "", dsc_documento = "";
        public int ctd_digitos = 0;
        public string ActualizarListado = "NO";
        internal Trabajador_vista MiAccion = Trabajador_vista.Nuevo;
        private string _cadena;
        private int cantidad;

//ddddd
        public frmVista()
        {
            InitializeComponent();
            unit = new UnitOfWork();
        }

        private void Inicializar()
        {
            try
            {
                switch (MiAccion)
                {
                    case Trabajador_vista.Nuevo:
                        Nuevo();
                        break;
                    case Trabajador_vista.Editar:
                        Editar();
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            CargarLookUpEdit();
            glkpTipoDocumento.EditValue = "DI001";
            //navigationFrame1.SelectedPage = navigationPage4;
            lkpmoneda.EditValue = null;
            lkpmonedacts.EditValue = null;
            lkpEntidadFinanciera.EditValue = null;
            lkpEntidadFinaciera2.EditValue = null;
            lkpTipoCuenta.EditValue = null;
            lkpTipoCuentaCTS.EditValue = null;
            lkpRegimenPensión.EditValue = null;
            lkpTipoComisión.EditValue = null;


        }

        
        private void Nuevo()
        {
            CargarLookUpEdit();
            glkpTipoDocumento.EditValue = "DI001";
            lkpEstadoCivil.EditValue = "01";
            lkpNacionalidad.EditValue = "PERUANO";
            lkpPais.EditValue = "00001"; lkpDepartamento.EditValue = "00015"; lkpProvincia.EditValue = "00128"; lkpNacionalidad.EditValue = "00001";
            lkpmoneda.EditValue = null;
            lkpmonedacts.EditValue = null;
            lkpEntidadFinanciera.EditValue = null;
            lkpEntidadFinaciera2.EditValue = null;
            lkpTipoCuenta.EditValue = null;
            lkpTipoCuentaCTS.EditValue = null;
            lkpRegimenPensión.EditValue = null;
            lkpTipoComisión.EditValue = null;



        }
        private void Editar()
        {
            CargarLookUpEdit();
            ActualizarListado = "SI";

        }

        private void CargarLookUpEdit()
        {
            try
            {
                CargarCombosGridLookup("TipoDocumentoVista", glkpTipoDocumento, "cod_tipo_documento" + "", "dsc_tipo_documento", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("EstadoCivil", lkpEstadoCivil, "cod_estado_civil", "dsc_estado_civil", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Nacionalidad", lkpNacionalidad, "cod_nacionalidad", "dsc_nacionalidad", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Sexo", lkpSexo, "cod_sexo", "dsc_sexo", "");
                unit.Trabajador.CargaCombosLookUp("Pais", lkpPais, "cod_pais", "dsc_pais", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Tipovia", lkpTipoVia, "cod_tipo_via", "dsc_tipo_via", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("TipoZona", lkpTipoZona, "cod_tipo_zona", "dsc_tipo_zona", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("NivAcademico", lkpNivelAcademico, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("SCRTPension", lkpScrtPension, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("TrabajadorSalud", lkpTipoTrabajadorSalud, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("SCRTSALUD", lkpscrtSalud, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("GrupoSanguineo", lkpGrupoSanguineo, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("SituacionTrabajador_salud", lkpSituaciontrabajador, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("EntidadFinanciera", lkpEntidadFinanciera, "cod_banco", "dsc_banco", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("EntidadFinanciera", lkpEntidadFinaciera2, "cod_banco", "dsc_banco", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("TipoCuenta", lkpTipoCuenta, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("TipoCuenta", lkpTipoCuentaCTS, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("TipoComision", lkpTipoComisión, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("RegimenPension", lkpRegimenPensión, "cod_APF", "dsc_APF", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Moneda", lkpmonedacts, "cod_moneda", "dsc_moneda", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Moneda", lkpmoneda, "cod_moneda", "dsc_moneda", "", valorDefecto: true);

                //blCli.CargaCombosLookUp("TipoDepartamento", lkpDepartamento, "cod_departamento", "dsc_departamento", "");
                //blCli.CargaCombosLookUp("TipoProvincia", lkpProvincia, "cod_provincia", "dsc_provincia", "");
                //CargarCombosGridLookup("TipoDistrito", glkpDistrito, "cod_distrito", "dsc_distrito", "");
                //blCli.CargaCombosLookUp("TipoPais", lkpNacionalidad, "cod_pais", "dsc_pais", "");
                //List<eProveedor_Empresas> listEmpresasUsuario = blProv.ListarEmpresasProveedor<eProveedor_Empresas>(11, "", user.cod_usuario);
                //lkpEmpresa.EditValue = listEmpresasUsuario[0].cod_empresa;
                //lkpDepartamento.EditValue = "00015"; lkpProvincia.EditValue = "00128";
                //if (MiAccion == Trabajador.Nuevo)
                //{
                //    picAnteriorTrabajador.Enabled = false; picSiguienteTrabajador.Enabled = false; btnNuevo.Enabled = false;
                //}
                //else
                //{
                //    picAnteriorTrabajador.Enabled = true; picSiguienteTrabajador.Enabled = true; btnNuevo.Enabled = true;
                //}
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

        private eTrabajador AsignarInfoGeneral()
        {

            eTrabajador obj = new eTrabajador();
            eTrab = unit.Trabajador.Obtener_cod_trabajador<eTrabajador>(57, cod_empresa);
            cod_trabajador = eTrab.cod_trabajador;
            obj.cod_tipo_documento = glkpTipoDocumento.EditValue == null ? null : glkpTipoDocumento.EditValue.ToString();
            obj.dsc_documento = txtNroDocumento.Text;
            obj.dsc_nombres = txtnombre.Text;
            obj.dsc_apellido_paterno = txtApellidoPaterno.Text;
            obj.dsc_apellido_materno = txtApellidoMaterno.Text;
            obj.fch_nacimiento = Convert.ToDateTime(dtFecNacimiento.Text.ToString());
            obj.dsc_edad = Convert.ToInt32(txtedad.Text.ToString());
            obj.flg_sexo = lkpSexo.EditValue.ToString();
            obj.cod_estadocivil = lkpEstadoCivil.EditValue == null ? null : lkpEstadoCivil.EditValue.ToString();
            obj.cod_nacionalidad = lkpNacionalidad.EditValue == null ? null : lkpNacionalidad.EditValue.ToString();
            obj.dsc_celular = txtCelular.Text;
            obj.dsc_telefono = txtTelefono.Text;
            obj.dsc_mail_1 = txtCorreo.Text;
            obj.cod_pais = lkpPais.EditValue == null ? null : lkpPais.EditValue.ToString();
            obj.cod_departamento = lkpDepartamento.EditValue == null ? null : lkpDepartamento.EditValue.ToString();
            obj.cod_provincia = lkpProvincia.EditValue == null ? null : lkpProvincia.EditValue.ToString();
            obj.cod_distrito = lkpDistrito.EditValue == null ? null : lkpDistrito.EditValue.ToString();
            obj.cod_lote = txtlote.Text;
            obj.cod_tipo_via = lkpTipoVia.EditValue == null ? null : lkpTipoVia.EditValue.ToString();
            obj.dsc_tipo_via = txtNombrevia.Text;
            obj.dsc_nro_vivienda = txtnro.Text;
            obj.dsc_departamento_dir = txtdep.Text;
            obj.dsc_interior = txtinterior.Text;
            obj.dsc_manzana = txtmz.Text;
            obj.dsc_etapa = txtEtapa.Text;
            obj.dsc_block = txtblock.Text;
            obj.dsc_tipo_zona = lkpTipoZona.EditValue == null ? null : lkpTipoZona.EditValue.ToString();
            obj.dsc_nombre_zona = txtNombrezona.Text;
            obj.dsc_direccion = txtReferencia.Text;
            obj.dsc_nivel_educativo = lkpNivelAcademico.EditValue == null ? null : lkpNivelAcademico.EditValue.ToString();
            obj.cod_empresa = cod_empresa;
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            return obj;
        }

        private eTrabajador.eInfoSalud_Trabajador AsignarInfoSalud(string cod_trabajador)
        {

            eTrabajador.eInfoSalud_Trabajador obj = new eTrabajador.eInfoSalud_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_tipo_trabajador_salud = lkpTipoTrabajadorSalud.EditValue == null ? null : lkpTipoTrabajadorSalud.EditValue.ToString();
            obj.cod_sctr_salud = lkpscrtSalud.EditValue == null ? null : lkpscrtSalud.EditValue.ToString();
            obj.cod_sctr_salud_pension = lkpScrtPension.EditValue == null ? null : lkpScrtPension.EditValue.ToString();
            obj.cod_situacion_trabajador_salud = lkpSituaciontrabajador.EditValue == null ? null : lkpSituaciontrabajador.EditValue.ToString();
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
            obj.dsc_gruposanguineo = lkpGrupoSanguineo.EditValue == null ? null : lkpGrupoSanguineo.EditValue.ToString();
            obj.cod_empresa = cod_empresa;
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            return obj;
        }

        private eTrabajador.eInfoBancaria_Trabajador AsignarInfo_Banco(string cod_trabajador)
        {

            eTrabajador.eInfoBancaria_Trabajador obj = new eTrabajador.eInfoBancaria_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_banco = lkpEntidadFinanciera.EditValue == null ? null : lkpEntidadFinanciera.EditValue.ToString();
            obj.cod_tipo_cuenta = lkpTipoCuenta.EditValue == null ? null : lkpTipoCuenta.EditValue.ToString();
            obj.cod_moneda = lkpmonedacts.EditValue == null ? null : lkpmonedacts.EditValue.ToString();
            obj.dsc_cta_bancaria = txtNroCuentaRem.Text;
            obj.dsc_cta_interbancaria = txtCciRem.Text;
            obj.cod_moneda_CTS = lkpmonedacts.EditValue == null ? null : lkpmonedacts.EditValue.ToString();
            obj.cod_banco_CTS = lkpEntidadFinaciera2.EditValue == null ? null : lkpEntidadFinaciera2.EditValue.ToString();
            obj.cod_tipo_cuenta_CTS = lkpTipoCuentaCTS.EditValue == null ? null : lkpTipoCuentaCTS.EditValue.ToString();
            obj.dsc_cta_bancaria_CTS = txtNroCuentaCTS.Text;
            obj.dsc_cta_interbancaria_CTS = txtCciCTS.Text;
            obj.cod_CUSPP = txtCUSPP.Text;
            obj.dsc_sist_pension = lkpRegimenPensión.EditValue == null ? null : lkpRegimenPensión.EditValue.ToString();
            obj.cod_tipo_comision_pension = lkpTipoComisión.EditValue == null ? null : lkpTipoComisión.EditValue.ToString();
            obj.cod_empresa = cod_empresa;
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            return obj;
        }


        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            if (splitContainerControl1.SplitterPosition == 104)
            {
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
                splitContainerControl1.SplitterPosition = 244;



                return;
            }
            else
            {
                splitContainerControl1.SplitterPosition = 104;
               // layoutControlGroup14.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
               // layoutControl15.Visible = false;             

            }
        }



        public void SelectNextPage()
        {
            navigationPage2.Show();

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (navigationFrame1.SelectedPage != navigationPage2)
            {
                navigationFrame1.SelectedPage = navigationPage2;
                txtlabelboton.Text = "Información Salud";
                btnguardar.Enabled = false;
            }
            //SelectNextPage();
        }

        private void btnInfoGeneral_Click(object sender, EventArgs e)
        {
            if (navigationFrame1.SelectedPage != navigationPage1)
            {
                navigationFrame1.SelectedPage = navigationPage1;
                btnInfoGeneral.BackColor = Color.Red;
                txtlabelboton.Text = "Información General";
                btnguardar.Enabled = false;
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (navigationFrame1.SelectedPage != navigationPage3)
            {
                navigationFrame1.SelectedPage = navigationPage3;
                txtlabelboton.Text = "Información Bancaria";
                btnguardar.Enabled = true;
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            
            if (navigationFrame1.SelectedPage == navigationPage1)
            {
                navigationFrame1.SelectedPage = navigationPage2;
                btnguardar.Enabled = false;
            }
            else if (navigationFrame1.SelectedPage == navigationPage2)
            {
                navigationFrame1.SelectedPage = navigationPage3;
                btnguardar.Enabled = true;
            }
            else if (navigationFrame1.SelectedPage == navigationPage3)
            {
                navigationFrame1.SelectedPage = navigationPage1;
                //  navigationFrame1.SelectedPage = navigationPage4;
                btnguardar.Enabled = false;
            }

        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (navigationFrame1.SelectedPage == navigationPage1)
            {
                navigationFrame1.SelectedPage = navigationPage3;
                btnguardar.Enabled = true;
            }
            else if (navigationFrame1.SelectedPage == navigationPage3)
            {
                navigationFrame1.SelectedPage = navigationPage2;
                btnguardar.Enabled = false;
            }
            else if (navigationFrame1.SelectedPage == navigationPage2)
            {
                navigationFrame1.SelectedPage = navigationPage1;
                btnguardar.Enabled = false;
            }
            else if (navigationFrame1.SelectedPage == navigationPage1)
            {
                navigationFrame1.SelectedPage = navigationPage3;
                btnguardar.Enabled = false;


            }
        }
        private void lkpPais_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpPais.EditValue != null)
            {
                unit.Trabajador.CargaCombosLookUp("Departamento", lkpDepartamento, "cod_departamento", "dsc_departamento", "", valorDefecto: true, cod_pais: lkpPais.EditValue.ToString());
                lkpDepartamento.EditValue = null;
            }


        }

        private void lkpDepartamento_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpDepartamento.EditValue != null)
            {
                unit.Trabajador.CargaCombosLookUp("Provincia", lkpProvincia, "cod_provincia", "dsc_provincia", "", valorDefecto: true, cod_pais: lkpPais.EditValue.ToString(), cod_departamento: lkpDepartamento.EditValue.ToString());
                lkpProvincia.EditValue = null;
            }
            if (lkpDepartamento.EditValue == null)
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (lkpDepartamento.EditValue != null)
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void lkpProvincia_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpProvincia.EditValue != null)
            {
                unit.Trabajador.CargaCombosLookUp("Distrito", lkpDistrito, "cod_distrito", "dsc_distrito", "", valorDefecto: true, cod_pais: lkpPais.EditValue.ToString(), cod_departamento: lkpDepartamento.EditValue.ToString(), cod_provincia: lkpProvincia.EditValue.ToString());
                lkpDistrito.EditValue = null;
            }
            if (lkpProvincia.EditValue == null)
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (lkpProvincia.EditValue != null)
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            //if (navigationFrame1.SelectedPage != navigationPage4)
            //{
            //    navigationFrame1.SelectedPage = navigationPage4;
            //    txtlabelboton.Text = "Home";
            //    btnguardar.Enabled = false;
            //}
        }

        private void frmVista_Load(object sender, EventArgs e)
        {
            splitContainerControl1.SplitterPosition = 244;
            Inicializar();
           // validaciones();

        }

        private void txtNroDocumento_Validating(object sender, CancelEventArgs e)
        {

            //string mensaje_error = "Solo se Ingresa Números de tamaño";
            //string mensaje_guardado = "Se registraron los datos de manera satisfactoria";
            //frmCuadroRegistroVista cuadromensaje = new frmCuadroRegistroVista();

            //eTrabajador trab = new eTrabajador();

            //string numerodocumento = txtNroDocumento.Text;
            //int length = numerodocumento.Length;
            //int resultado = 0;
            //eTrabajador ctd_digitos = unit.Trabajador.Obtener_cantidad_doc<eTrabajador>(59, glkpTipoDocumento.EditValue.ToString());
           

            //if (numerodocumento.All(char.IsDigit))
            //{
               
            //}
            //else
            //{
            //    cuadromensaje.mensaje = mensaje_error;
            //    cuadromensaje.Show();
            //}
        }

        private void layoutControlItem6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Close();
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

        private void dtFecNacimiento_EditValueChanged(object sender, EventArgs e)
        {
            DateTime fechaNacimiento = dtFecNacimiento.DateTime;
            DateTime fechaActual = DateTime.Today;
            int edad = fechaActual.Year - fechaNacimiento.Year;
            txtedad.Text = Convert.ToString(edad).ToString();
        }

        private void lkpTipoZona_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) lkpTipoZona.EditValue = null;
        }

        private void lkpTipoVia_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) lkpTipoVia.EditValue = null;
        }

        private void lkpTipoZona_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpTipoZona.EditValue == null)
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (lkpTipoZona.EditValue != null)
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtNombrezona_EditValueChanged(object sender, EventArgs e)
        {
            if (txtNombrezona.Text == "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtNombrezona.Text != "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void lkpTipoVia_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpTipoVia.EditValue == null)
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (lkpTipoVia.EditValue != null)
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtNombrevia_EditValueChanged(object sender, EventArgs e)
        {

            if (txtNombrevia.Text == "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtNombrevia.Text != "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtnro_EditValueChanged(object sender, EventArgs e)
        {
            if (txtnro.Text == "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtnro.Text != "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtdep_EditValueChanged(object sender, EventArgs e)
        {
            if (txtdep.Text == "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtdep.Text != "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtinterior_EditValueChanged(object sender, EventArgs e)
        {
            if (txtinterior.Text == "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtinterior.Text != "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtmz_EditValueChanged(object sender, EventArgs e)
        {
            if (txtmz.Text == "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtmz.Text != "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtlote_EditValueChanged(object sender, EventArgs e)
        {
            if (txtlote.Text == "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtlote.Text != "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtkm_EditValueChanged(object sender, EventArgs e)
        {
            if (txtkm.Text == "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtkm.Text != "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtEtapa_EditValueChanged(object sender, EventArgs e)
        {
            if (txtEtapa.Text == "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtEtapa.Text != "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void txtblock_EditValueChanged(object sender, EventArgs e)
        {
            if (txtblock.Text == "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (txtblock.Text != "")
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
        }

        private void lkpDistrito_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpProvincia.EditValue == null)
            {
                Direccion(lkpDistrito.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);
            }
            else if (lkpDistrito.EditValue != null)
            {
                Direccion(lkpTipoZona.Text == null ? null : lkpTipoZona.Text.ToString(),
                    txtNombrezona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
            txtnro.Text, txtdep.Text, txtinterior.Text, txtmz.Text, txtlote.Text, txtkm.Text, txtEtapa.Text, txtblock.Text,
            lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
            , lkpDistrito.Text == null ? null : lkpDistrito.Text.ToString(), txtReferencia.Text);

            }
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

        private void txtCelular_KeyPress(object sender, KeyPressEventArgs e)
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

        private void layoutControlItem82_Click(object sender, EventArgs e)
        {

        }

        private void lkpSexo_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpSexo.EditValue.ToString() == "F")
            {
                Image imgEmpresaLarge = Properties.Resources.female64;
                pictureEdit1.EditValue = imgEmpresaLarge;
            }
            else if (lkpSexo.EditValue.ToString() == "M")
            {
                Image imgEmpresaLarge = Properties.Resources.Male64;
                pictureEdit1.EditValue = imgEmpresaLarge;
            }
        }

        private void txtnombre_EditValueChanged(object sender, EventArgs e)
        {
            
            if (txtnombre.Text == null || txtnombre.Text=="") { simpleLabelItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always; }
            if (txtnombre.Text != null) { simpleLabelItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never; }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            //string mensaje_error = "ERROR AL GUARDAR LOS DATOS";
            //string mensaje_guardado = "SE REGISTRARON LOS DATOS DE MANERA CORRECTA";
            //frmMensaje mensaje = new frmMensaje();
            
            try
            {
                if (dtFecNacimiento.EditValue == null) { MessageBox.Show("Debe ingresar la fecha de nacimiento", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtFecNacimiento.Focus(); return; }

                if (txtNroDocumento.Text != "")
                {
                    eProveedor obj = new eProveedor();
                    obj = unit.Proveedores.Validar_NroDocumento<eProveedor>(19, "", glkpTipoDocumento.EditValue.ToString());
                    int nrodocumento = Convert.ToInt32(txtNroDocumento.Text.Length);
                    if (nrodocumento < obj.ctd_digitos) { MessageBox.Show("Los digitos del documento debe ser igual a " + obj.ctd_digitos, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCelular.Focus(); return; }
                    txtNroDocumento.Properties.MaxLength = obj.ctd_digitos;
                }
                eTrabV = unit.Trabajador.Obtener_cod_trabajador<eTrabajador>(79, "",txtNroDocumento.Text.ToString());
                //dsc_documento = eTrabV.cod_trabajador;
                

                eTrab = AsignarInfoGeneral();
                eTrab = unit.Trabajador.InsertarActualizar_Trabajador<eTrabajador>(eTrab);
                
               // eTrabsalud.cod_trabajador = eTrab.cod_trabajador;
                eTrabsalud = AsignarInfoSalud(cod_trabajador);
                eTrabsalud = unit.Trabajador.InsertarActualizar_InfoSaludTrabajador<eTrabajador.eInfoSalud_Trabajador>(eTrabsalud);

                etrabBanco = AsignarInfo_Banco(cod_trabajador);
                etrabBanco = unit.Trabajador.InsertarActualizar_InfoBancariaTrabajador<eTrabajador.eInfoBancaria_Trabajador>(etrabBanco);

                //if (eTrab == null || eTrabsalud == null || etrabBanco == null)
                //{
                //    cuadromensaje.mensaje = mensaje_error;
                //    cuadromensaje.Show();
                //}
                    //MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; 
                //cod_trabajador = eTrab.cod_trabajador; cod_empresa = eTrab.cod_empresa; 

                if (eTrab != null || eTrabsalud!=null || etrabBanco != null)
                {
                    

                    MiAccion = Trabajador_vista.Editar;
                    ActualizarListado = "SI";
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

        private void Direccion(string tipo_zona = "", string nombre_zona = "", string tipo_via = "", string nombre_via = "",
           string nro = "", string dep = "", string interior = "", string mz = "", string lote = "", string km = "", string etapa = "",
           string block = "", string departamento = "", string provincia = "", string distrito = "", string referencia = "")
        {
            txtReferencia.Text = "";

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

            txtReferencia.Text = tipo_zona + nombre_zona + tipo_via + nombre_via + nrodi + depdi +
            intdir + mzdir + lotedir + kmdir + etapdir + blockdir + departamentodir +
            provinciadir + distritodir + "\r\n" + referenciadir;
        }





    }

}
       
            
        

