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
using System.ComponentModel.DataAnnotations;

namespace UI_GestionRRHH.Formularios.Personal
{
    public partial class frmRegistroRapido : HNG_Tools.SimpleModalForm
    {

        private readonly UnitOfWork unit;
        frmListadoTrabajador frmHandler;
        List<eTrabajador> listTrabajador = new List<eTrabajador>();
        public eUsuario user = new eUsuario();
        public eTrabajador eTrab = new eTrabajador();
        public eTrabajador eTrabV = new eTrabajador();
        public eTrabajador.eInfoLaboral_Trabajador eInfoLab = new eTrabajador.eInfoLaboral_Trabajador();
        public eTrabajador.eInfoSalud_Trabajador obj = new eTrabajador.eInfoSalud_Trabajador();
        public eTrabajador.eInfoBancaria_Trabajador eBanc = new eTrabajador.eInfoBancaria_Trabajador();
        public string ActualizarListado = "NO";
        public string cod_trabajador = "", cod_empresa = "", sede_empresa = "", dsc_documento = "";

        private void lkpProvincia_EditValueChanged(object sender, EventArgs e)
        {
            lkpdistrito.Properties.DataSource = null;
            //unit.Clientes.CargaCombosLookUp("TipoDistrito", glkpDistrito, "cod_distrito", "dsc_distrito", "", cod_condicion: lkpProvincia.EditValue.ToString());
            unit.Trabajador.CargaCombosLookUp("Distrito", lkpdistrito, "cod_distrito", "dsc_distrito", "", cod_pais: lkpPais.EditValue == null ? "" : lkpPais.EditValue.ToString(), cod_departamento: lkpDepartamento.EditValue == null ? "" : lkpDepartamento.EditValue.ToString(),cod_provincia: lkpProvincia.EditValue == null ? "" : lkpProvincia.EditValue.ToString());
           

            if (lkpProvincia.EditValue == null)
            {
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());
            }
            else if (lkpProvincia.EditValue != null)

            {

                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
        txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
        lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
        , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());

            }
        }

        private void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;

        }
        private void frmRegistroRapido_Load(object sender, EventArgs e)
        {
            dtfechaIngreso.EditValue = DateTime.Today;
            dtFinContrato.EditValue = DateTime.Today;
            dtIniContrato.EditValue = DateTime.Today;
            dtFchPuesto.EditValue = DateTime.Today;
            lkpTipoDocumento.EditValue = "DI001";

            configurar_formulario();
            bloquearcontroles();
            CargarLookUpEdit();
        }

        private void lkpEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            unit.Trabajador.CargaCombosLookUp("SedesEmpresa", lkpSedeEmpresa, "cod_sede_empresa", "dsc_sede_empresa", "", cod_empresa: lkpEmpresa.EditValue == null ? "" : lkpEmpresa.EditValue.ToString());
            unit.Factura.CargaCombosLookUp("DistribucionCECO", lkpCco, "cod_CECO", "dsc_CECO", "", valorDefecto: true, cod_empresa: lkpEmpresa.EditValue == null ? "" : lkpEmpresa.EditValue.ToString());
            unit.Trabajador.CargaCombosLookUp("scrtcentroriesgo", lkpCentroRiesgo, "cod_sunat", "dsc_scrtcentroriesgo", "", valorDefecto: true, cod_empresa: lkpEmpresa.EditValue == null ? "" : lkpEmpresa.EditValue.ToString());
            unit.Trabajador.CargaCombosLookUp("seguroley", lkpCIAsegurovidaley, "cod_sunat", "dsc_seguroley", "", valorDefecto: true, cod_empresa: lkpEmpresa.EditValue == null ? "" : lkpEmpresa.EditValue.ToString());
            lkpSedeEmpresa.EditValue = null;
            lkpArea.EditValue = null;
            lkpCargo.EditValue = null;
        }

        private void lkpSedeEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            unit.Trabajador.CargaCombosLookUp("AreaEmpresa", lkpArea, "cod_area", "dsc_area", "", cod_empresa: lkpEmpresa.EditValue == null ? "" : lkpEmpresa.EditValue.ToString(), cod_sede_empresa: lkpSedeEmpresa.EditValue == null ? "" : lkpSedeEmpresa.EditValue.ToString());
            lkpArea.EditValue = "00001";
        }

        private void lkpArea_EditValueChanged(object sender, EventArgs e)
        {
            unit.Trabajador.CargaCombosLookUp("CargoEmpresa", lkpCargo, "cod_cargo", "dsc_cargo", "", cod_empresa: lkpEmpresa.EditValue == null ? "" : lkpEmpresa.EditValue.ToString(), cod_sede_empresa: lkpSedeEmpresa.EditValue == null ? "" : lkpSedeEmpresa.EditValue.ToString(), cod_area: lkpArea.EditValue == null ? "" : lkpArea.EditValue.ToString());

        }

        private void lkpDepartamento_EditValueChanged(object sender, EventArgs e)
        {
            unit.Trabajador.CargaCombosLookUp("Provincia", lkpProvincia, "cod_provincia", "dsc_provincia", "", cod_pais: lkpPais.EditValue == null ? "" : lkpPais.EditValue.ToString(), cod_departamento: lkpDepartamento.EditValue == null ? "" : lkpDepartamento.EditValue.ToString());
            lkpProvincia.EditValue = "00128";
            
            if (lkpDepartamento.EditValue == null)
            {
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
        txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
        lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
        , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());
            }
            else if (lkpDepartamento.EditValue != null)

            {

                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());

            }
        }

        private void lkpPais_EditValueChanged(object sender, EventArgs e)
        {
            lkpProvincia.Properties.DataSource = null;
            lkpDepartamento.Properties.DataSource = null;
            unit.Trabajador.CargaCombosLookUp("Departamento", lkpDepartamento, "cod_departamento", "dsc_departamento", "", cod_pais: lkpPais.EditValue == null ? "" : lkpPais.EditValue.ToString());
            lkpDepartamento.EditValue = "00015";


        }

        public frmRegistroRapido()
        {
            InitializeComponent();
            unit = new UnitOfWork();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string mensaje_error = "Error al guardar los datos";
            string mensaje_guardado = "Se registraron los datos de manera satisfactoria";
            string ingreso = Convert.ToString(dtfechaIngreso.EditValue.ToString());
            string fincontrato = Convert.ToString(dtFinContrato.EditValue.ToString());

            try
            {

                if (txtApellidoPaterno.Text.Trim() == "") { MessageBox.Show("Debe ingresar un apellido paterno.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtApellidoMaterno.Focus(); return; }
                if (txtApellidoMaterno.Text.Trim() == "") { MessageBox.Show("Debe ingresar un apellido materno.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtApellidoPaterno.Focus(); return; }
                if (txtNombre.Text.Trim() == "") { MessageBox.Show("Debe ingresar un nombre.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNombre.Focus(); return; }
                if (dtFchNacimiento.EditValue == null) { MessageBox.Show("Debe ingresar la fecha de nacimiento", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtFchNacimiento.Focus(); return; }
                if (lkps.EditValue == null) { MessageBox.Show("Debe seleccionar Sexo", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkps.Focus(); return; }
                if (lkpEstadoCivil.EditValue == null) { MessageBox.Show("Debe seleccionar un Estado Civil", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpEstadoCivil.Focus(); return; }
                if (lkpTipoDocumento.EditValue == null) { MessageBox.Show("Debe ingresar el tipo de documento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipoDocumento.Focus(); return; }
                if (txtnumdocumento.Text.Trim() == "") { MessageBox.Show("Debe ingresar un número de documento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtnumdocumento.Focus(); return; }
                if (lkpNivelEducativo.Text.Trim() == "") { MessageBox.Show("Debe ingresar un Nivel Academico.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpNivelEducativo.Focus(); return; }
                if (txtCelular.Text.Trim() == "") { MessageBox.Show("Debe ingresar un número de celular.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCelular.Focus(); return; }
                if (lkpPais.EditValue == null) { MessageBox.Show("Debe seleccionar un país.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpPais.Focus(); return; }
                if (lkpDepartamento.EditValue == null) { MessageBox.Show("Debe seleccionar un departamento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpDepartamento.Focus(); return; }
                if (lkpProvincia.EditValue == null) { MessageBox.Show("Debe seleccionar una provincia.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpProvincia.Focus(); return; }
                if (lkpdistrito.EditValue == null) { MessageBox.Show("Debe seleccionar un distrito.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpdistrito.Focus(); return; }
                if (lkpnacionalidad.EditValue == null) { MessageBox.Show("Debe seleccionar la nacionalidad.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpnacionalidad.Focus(); return; }
                if (txtDireccion.Text.Trim() == "") { MessageBox.Show("Debe ingresar una dirección.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpdistrito.Focus(); return; }
                if (lkpEmpresa.EditValue == null) { MessageBox.Show("Debe seleccionar Empresa.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpEmpresa.Focus(); return; }
                if (lkpSedeEmpresa.EditValue == null) { MessageBox.Show("Debe seleccionar Sede.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpSedeEmpresa.Focus(); return; }
                if (lkpCategoriaTrab.EditValue == null) { MessageBox.Show("Debe seleccionar Categoria Trabajador.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpCategoriaTrab.Focus(); return; }
                if (lkpTipoTrabajador.EditValue == null) { MessageBox.Show("Debe seleccionar Tipo Trabajador.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipoTrabajador.Focus(); return; }
                if (lkpCatOcupacional.EditValue == null) { MessageBox.Show("Debe seleccionar una Ocupación.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpCatOcupacional.Focus(); return; }
                if (lkpArea.EditValue == null) { MessageBox.Show("Debe seleccionar la Area.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpArea.Focus(); return; }
                if (lkpCargo.EditValue == null) { MessageBox.Show("Debe seleccionar la Cargo.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpCargo.Focus(); return; }
                if (lkpCalificacionPuesto.EditValue == null) { MessageBox.Show("Debe seleccionar Calificacion de puesto.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpCalificacionPuesto.Focus(); return; }
                if (lkpCco.EditValue == null) { MessageBox.Show("Debe seleccionar CECO.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpCco.Focus(); return; }
                if (lkpCentroRiesgo.EditValue == null) { MessageBox.Show("Debe seleccionar Centro de Riesgo.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpCentroRiesgo.Focus(); return; }
                if (lkpCIAsegurovidaley.EditValue == null) { MessageBox.Show("Debe seleccionar CIA seguro vida Ley.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpCIAsegurovidaley.Focus(); return; }
                if (lkpTipoContrato.EditValue == null) { MessageBox.Show("Debe seleccionar Tipo contrato.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipoContrato.Focus(); return; }
                if (lkpRegimenLaboral.EditValue == null) { MessageBox.Show("Debe seleccionar un Regimen Laboral.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpRegimenLaboral.Focus(); return; }
                if (lkpTipoPago.EditValue == null) { MessageBox.Show("Debe seleccionar Tipo Pago.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipoPago.Focus(); return; }
                if (lkpPeriodicidadpago.EditValue == null) { MessageBox.Show("Debe seleccionar Periodicidad de Pago", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpPeriodicidadpago.Focus(); return; }
                if (lkpMoneda.EditValue == null) { MessageBox.Show("Debe seleccionar Tipo Moneda", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpMoneda.Focus(); return; }
                if (lkpBancoRem.EditValue == null) { MessageBox.Show("Debe seleccionar Banco", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpBancoRem.Focus(); return; }
                if (lkptipocuentarem.EditValue == null) { MessageBox.Show("Debe seleccionar Tipo de cuenta", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkptipocuentarem.Focus(); return; }
                if (txtNroCuentaRem.EditValue == null) { MessageBox.Show("Debe ingresar Nro de cuenta", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNroCuentaRem.Focus(); return; }
                if (txtCCIREM.EditValue == null) { MessageBox.Show("Debe ingresar Nro de cuenta Interbancaria", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCCIREM.Focus(); return; }
                if (lkpMonedaCTS.EditValue == null) { MessageBox.Show("Debe seleccionar Tipo Moneda", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpMonedaCTS.Focus(); return; }
                if (lkpBancoCTS.EditValue == null) { MessageBox.Show("Debe seleccionar Banco", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpBancoCTS.Focus(); return; }
                if (lkptipocuentacts.EditValue == null) { MessageBox.Show("Debe seleccionar Tipo de cuenta", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkptipocuentacts.Focus(); return; }
                if (txtNroCuentaCTS.EditValue == null) { MessageBox.Show("Debe ingresar Nro de cuenta", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtNroCuentaCTS.Focus(); return; }
                if (txtCCICTS.EditValue == null) { MessageBox.Show("Debe ingresar Nro de cuenta Interbancaria", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCCICTS.Focus(); return; }

                if (lkpregimensalud.EditValue == null) { MessageBox.Show("Debe seleccionar Regimen Salud", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpregimensalud.Focus(); return; }
                if (lkpscrtsalud1.EditValue == null) { MessageBox.Show("Debe seleccionar SCRT Salud", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpscrtsalud1.Focus(); return; }
                if (lkpscrtTrab.EditValue == null) { MessageBox.Show("Debe seleccionar SCRT Pension", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpscrtTrab.Focus(); return; }
                if (lkpSituacionsalud.EditValue == null) { MessageBox.Show("Debe seleccionar Situacion Salud", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpSituacionsalud.Focus(); return; }
                if (lkpTipotrabsalud.EditValue == null) { MessageBox.Show("Debe seleccionar Tipo Trabajador Salud", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpTipotrabsalud.Focus(); return; }



                if (ingreso == fincontrato) { MessageBox.Show("La fecha de ingreso no puede ser igual a la fecha de vencimiento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); dtFinContrato.Focus(); return; }
                if (!ValidateEmail(txtCorreoPersonal.Text)) { MessageBox.Show("Debe ingresar un Correo Electronico Valido", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCorreoPersonal.Focus(); return; }
                if (!ValidateEmail(txtCorreoLaboral.Text)) { MessageBox.Show("Debe ingresar un Correo Electronico Valido", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCorreoLaboral.Focus(); return; }

                if (txtnumdocumento.EditValue != null)
                {
                    eProveedor obj = new eProveedor();
                    obj = unit.Proveedores.Validar_NroDocumento<eProveedor>(19, "", lkpTipoDocumento.EditValue.ToString());
                    int nrodocumento = Convert.ToInt32(txtnumdocumento.Text.Length);
                    if (nrodocumento < obj.ctd_digitos) { MessageBox.Show("Los digitos del documento debe ser igual a " + obj.ctd_digitos, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCelular.Focus(); return; }
                    txtnumdocumento.Properties.MaxLength = obj.ctd_digitos;
                }
                eTrab = AsignarValores_Trabajador();
                eTrab = unit.Trabajador.InsertarActualizar_Trabajador<eTrabajador>(eTrab);

                eInfoLab = AsignarValores_InfoLaboral(cod_trabajador);
                eInfoLab = unit.Trabajador.InsertarActualizar_InfoLaboralTrabajador<eTrabajador.eInfoLaboral_Trabajador>(eInfoLab);

                eBanc = AsignarValores_InfoBancaria(cod_trabajador);
                eBanc = unit.Trabajador.InsertarActualizar_InfoBancariaTrabajador<eTrabajador.eInfoBancaria_Trabajador>(eBanc);

                obj = AsignarValores_InfoSalud(cod_trabajador);
                obj = unit.Trabajador.InsertarActualizar_InfoSaludTrabajador<eTrabajador.eInfoSalud_Trabajador>(obj);


               


                if (eTrab != null && eInfoLab != null && eBanc != null && obj != null)
                {


                    MessageBox.Show(mensaje_guardado + " " + cod_trabajador);
                    ActualizarListado = "SI";

                } else
                {
                    MessageBox.Show(mensaje_error);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            frmListadoTrabajador listado = new frmListadoTrabajador();
            listado.gvListadoTrabajador.RefreshData();
            this.Close();
            listTrabajador = unit.Trabajador.ListarTrabajadoresvista<eTrabajador>(1, "", "ALL", ""); // LISTA GENERAL
            listado.CargarListado();
            listado.bsListaTrabajador.DataSource = listTrabajador; listado.gvListadoTrabajador.RefreshData();
            listado.gvListadoTrabajador.OptionsFind.FindNullPrompt = null;
            this.Close();
        }

        bool ValidateEmail(string email) { return new EmailAddressAttribute().IsValid(email); }
        private eTrabajador AsignarValores_Trabajador()
        {
            eTrabajador obj = new eTrabajador();
            eTrab = unit.Trabajador.Obtener_cod_trabajador<eTrabajador>(57, lkpEmpresa.EditValue.ToString());
            cod_trabajador = eTrab.cod_trabajador;
            obj.cod_empresa = lkpEmpresa.EditValue == null ? null : lkpEmpresa.EditValue.ToString();
            obj.cod_tipo_documento = lkpTipoDocumento.EditValue == null ? null : lkpTipoDocumento.EditValue.ToString();
            obj.dsc_documento = txtnumdocumento.Text;
            obj.dsc_apellido_paterno = txtApellidoPaterno.Text.Trim();
            obj.dsc_apellido_materno = txtApellidoMaterno.Text.Trim();
            obj.dsc_nombres = txtNombre.Text.Trim();
            obj.fch_nacimiento = Convert.ToDateTime(dtFchNacimiento.EditValue);
            obj.flg_sexo = lkps.EditValue == null ? null : lkps.EditValue.ToString();
            obj.cod_estadocivil = lkpEstadoCivil.EditValue == null ? null : lkpEstadoCivil.EditValue.ToString();
            obj.cod_nacionalidad = lkpnacionalidad.EditValue == null ? null : lkpnacionalidad.EditValue.ToString();
            obj.dsc_telefono = txtFijo.Text;
            obj.dsc_celular = txtCelular.Text;
            obj.dsc_mail_1 = txtCorreoPersonal.Text;
            obj.cod_nivel_educativo = lkpNivelEducativo.EditValue == null ? null : lkpNivelEducativo.EditValue.ToString();
            obj.cod_pais = lkpPais.EditValue == null ? null : lkpPais.EditValue.ToString();
            obj.cod_departamento = lkpDepartamento.EditValue == null ? null : lkpDepartamento.EditValue.ToString();
            obj.cod_provincia = lkpProvincia.EditValue == null ? null : lkpProvincia.EditValue.ToString();
            obj.cod_distrito = lkpdistrito.EditValue.ToString();
            obj.cod_tipo_via = lkpTipoVia.EditValue == null ? null : lkpTipoVia.EditValue.ToString();
            obj.dsc_tipo_via = txtNombrevia.Text;
            obj.dsc_nro_vivienda = txtNro.Text;
            obj.dsc_departamento_dir = txtDep.Text;
            obj.dsc_interior = txtInterior.Text;
            obj.dsc_manzana = txtManzana.Text;
            obj.cod_lote = txtLote.Text;
            obj.dsc_km = txtKm.Text;
            obj.dsc_etapa = txtEtapa.Text;
            obj.dsc_block = txtBlock.Text;
            obj.dsc_tipo_zona = lkpTipozona.EditValue == null ? null : lkpTipozona.EditValue.ToString();
            obj.dsc_nombre_zona = txtNombreZona.Text;
            obj.dsc_direccion = txtDireccion.Text;
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            obj.dsc_mail_2 = txtCorreoLaboral.Text;
            obj.flg_activo = "SI";


            return obj;
        }

        private void lkpCco_EditValueChanged(object sender, EventArgs e)
        {


        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (txtnumdocumento.Text == "")
            {
                MessageBox.Show("DEBE INGRESAR UN NÚMERO DE DOCUMENTO");

            }
            else
            {
                eTrabV = unit.Trabajador.Obtener_cod_trabajador<eTrabajador>(79, "", txtnumdocumento.Text.ToString());

                if (eTrabV != null)

                {
                    dsc_documento = eTrabV.cod_trabajador;
                    bloquearcontroles();
                    MessageBox.Show("EL TRABAJADOR YA SE ENCUENTRA REGISTRADO, CODIGO DE TRABAJADOR: " + dsc_documento);


                }
                else
                {
                    string numero = "";
                    numero = txtnumdocumento.Text;
                    eTrabajador resultado = unit.Trabajador.Validardni<eTrabajador>(18, numero);
                    desbloqueocontroles();
                    txtNombre.Select();

                }
            }
        }

        private eTrabajador.eInfoLaboral_Trabajador AsignarValores_InfoLaboral(string cod_trabajador)
        {
            eTrabajador.eInfoLaboral_Trabajador obj = new eTrabajador.eInfoLaboral_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = lkpEmpresa.EditValue.ToString();
            obj.cod_sede_empresa = lkpSedeEmpresa.EditValue.ToString();
            obj.cod_categoria_trabajador = lkpCategoriaTrab.EditValue == null ? null : lkpCategoriaTrab.EditValue.ToString();
            obj.cod_tipo_trabajador = lkpTipoTrabajador.EditValue == null ? null : lkpTipoTrabajador.EditValue.ToString();
            obj.cod_ocupacional = lkpCatOcupacional.EditValue == null ? null : lkpCatOcupacional.EditValue.ToString();
            obj.cod_area = lkpArea.EditValue.ToString();
            obj.cod_cargo = lkpCargo.EditValue.ToString();
            obj.cod_situacion_especial = lkpCalificacionPuesto.EditValue == null ? null : lkpCalificacionPuesto.EditValue.ToString();
            obj.fch_inicio_contrato = Convert.ToDateTime(dtIniContrato.EditValue.ToString());
            obj.cod_tipo_contrato = lkpTipoContrato.EditValue == null ? null : lkpTipoContrato.EditValue.ToString();
            obj.fch_fin_contrato = Convert.ToDateTime(dtFinContrato.EditValue.ToString());
            obj.fch_firma = Convert.ToDateTime(dtIniContrato.EditValue.ToString());
            obj.dsc_pref_ceco = lkpCco.EditValue == null ? null : lkpCco.EditValue.ToString();

            obj.cod_infolab = 0;
            obj.fch_puesto = Convert.ToDateTime(dtFchPuesto.EditValue.ToString());
            obj.flg_afectoSCTR = lkpafectosctr.EditValue.ToString();
            obj.flg_AfectoVidaLey = lkpAfectoVidaLey.EditValue.ToString();
            obj.flg_horario_nocturno = lkpHorarioNoc.EditValue.ToString();
            obj.flg_horas_extras = lkpHorarioextra.EditValue.ToString();
            obj.dsc_cia_segurovida = lkpCIAsegurovidaley.EditValue == null ? null : lkpCIAsegurovidaley.EditValue.ToString();
            obj.cod_regimen_laboral = lkpRegimenLaboral.EditValue == null ? null : lkpRegimenLaboral.EditValue.ToString();
            obj.correo_laboral = txtCorreoLaboral.Text == null ? null : txtCorreoLaboral.Text;
            //banco
            decimal imp = 0.00m;
            obj.imp_sueldo_base = Convert.ToDecimal(txtsueldo.Text);
            obj.imp_asig_familiar = imp;
            obj.imp_movilidad = imp;
            obj.imp_alimentacion = imp;
            obj.imp_escolaridad = imp;
            obj.imp_bono = imp;
            obj.flg_horas_extras = lkpHorarioextra.EditValue == null ? null : lkpHorarioextra.EditValue.ToString();
            obj.flg_horario_nocturno = lkpHorarioNoc.EditValue == null ? null : lkpHorarioNoc.EditValue.ToString();
            obj.dsc_porcentajecomision = txtporcComision.Text;
            obj.dsc_porcentajequincena = txtporcquincena.Text;
            obj.fch_ingreso = Convert.ToDateTime(dtfechaIngreso.EditValue.ToString());
            obj.fch_vencimiento = Convert.ToDateTime(dtFinContrato.EditValue.ToString());
             obj.cod_situacion_trabajador_2 = "0001";
            obj.codsunat_scrtcentroriesgo = lkpCentroRiesgo.EditValue == null ? null : lkpCentroRiesgo.EditValue.ToString();
            obj.codsunat_seguroley = lkpCIAsegurovidaley.EditValue == null ? null : lkpCIAsegurovidaley.EditValue.ToString();


            //       obj.dsc_sctr_pension = lkpscrtTrab.EditValue.ToString();



            obj.flg_activo = "SI";
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;

            //tipo contrato
            //asig familiar
            // reg laboral
            //scrt trabajador

            return obj;
        }
        private eTrabajador.eInfoBancaria_Trabajador AsignarValores_InfoBancaria(string cod_trabajador)
        {
            eTrabajador.eInfoBancaria_Trabajador obj = new eTrabajador.eInfoBancaria_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = lkpEmpresa.EditValue.ToString();
            obj.cod_APF = lkpRegimenPension.EditValue == null ? null : lkpRegimenPension.EditValue.ToString();
            obj.cod_CUSPP = txtCUSP.EditValue == null ? null : txtCUSP.Text;
            obj.cod_tipo_comision_pension = lkpTipoComision.EditValue == null ? null : lkpTipoComision.EditValue.ToString();
            obj.cod_tipo_pago = lkpTipoPago.EditValue == null ? null : lkpTipoPago.EditValue.ToString();
            obj.cod_periodicidad_pagos = lkpPeriodicidadpago.EditValue == null ? null : lkpPeriodicidadpago.EditValue.ToString();

            obj.cod_moneda = lkpMoneda.EditValue == null ? null : lkpMoneda.EditValue.ToString();
            obj.cod_banco = lkpBancoRem.EditValue == null ? null : lkpBancoRem.EditValue.ToString();
            obj.cod_tipo_cuenta = lkptipocuentarem.EditValue == null ? null : lkptipocuentarem.EditValue.ToString();
            obj.dsc_cta_bancaria = txtNroCuentaRem.Text;
            obj.dsc_cta_interbancaria = txtCCIREM.Text;

            obj.cod_moneda = lkpMonedaCTS.EditValue == null ? null : lkpMonedaCTS.EditValue.ToString();
            obj.cod_banco_CTS = lkpBancoCTS.EditValue == null ? null : lkpBancoCTS.EditValue.ToString();
            obj.cod_tipo_cuenta_CTS = lkptipocuentacts == null ? null : lkptipocuentacts.EditValue.ToString();
            obj.dsc_cta_bancaria_CTS = txtNroCuentaCTS.Text;
            obj.dsc_cta_interbancaria_CTS = txtCCICTS.Text;
            obj.flg_activo = "SI";


            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;


            return obj;
        }
        private eTrabajador.eInfoSalud_Trabajador AsignarValores_InfoSalud(string cod_trabajador)
        {
            eTrabajador.eInfoSalud_Trabajador obj = new eTrabajador.eInfoSalud_Trabajador();
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = lkpEmpresa.EditValue.ToString();
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            obj.cod_sctr_salud = lkpscrtsalud1.EditValue == null ? null : lkpscrtsalud1.EditValue.ToString();
            obj.cod_tipo_trabajador_salud = lkpTipotrabsalud.EditValue == null ? null : lkpTipotrabsalud.EditValue.ToString();
            obj.cod_situacion_trabajador_salud = lkpSituacionsalud.EditValue == null ? null : lkpSituacionsalud.EditValue.ToString();
            obj.cod_regimen_salud = lkpregimensalud.EditValue == null ? null : lkpSituacionsalud.EditValue.ToString();


            return obj;
        }

        private void desbloqueocontroles()
        {
            lkpTipoDocumento.Enabled = true;
            txtNombre.Enabled = true;
            txtApellidoPaterno.Enabled = true;
            txtApellidoMaterno.Enabled = true;
            dtFchNacimiento.Enabled = true;
            lkps.Enabled = true;
            lkpEstadoCivil.Enabled = true;
            dtFchNacimiento.Enabled = true;
            dtfechaIngreso.Enabled = true;
            txtCelular.Enabled = true;
            txtFijo.Enabled = true;
            txtCorreoPersonal.Enabled = true;
            lkpNivelEducativo.Enabled = true;
            lkpPais.Enabled = true;
            lkpDepartamento.Enabled = true;
            lkpProvincia.Enabled = true;
            lkpdistrito.Enabled = true;
            lkpTipoVia.Enabled = true;
            txtNombrevia.Enabled = true;
            txtNro.Enabled = true;
            txtDep.Enabled = true;
            txtInterior.Enabled = true;
            txtManzana.Enabled = true;
            txtLote.Enabled = true;
            txtKm.Enabled = true;
            txtEtapa.Enabled = true;
            txtBlock.Enabled = true;
            lkpTipozona.Enabled = true;
            txtNombreZona.Enabled = true;
            txtDireccion.Enabled = true;
            lkpEmpresa.Enabled = true;
            lkpSedeEmpresa.Enabled = true;
            lkpCategoriaTrab.Enabled = true;
            lkpTipoTrabajador.Enabled = true;
            lkpCatOcupacional.Enabled = true;
            lkpArea.Enabled = true;
            lkpCargo.Enabled = true;
            lkpCalificacionPuesto.Enabled = true;
            lkpCco.Enabled = true;
            dtFinContrato.Enabled = true;
            txtCorreoLaboral.Enabled = true;
            lkpafectosctr.Enabled = true;
            lkpAfectoVidaLey.Enabled = true;
            lkpHorarioNoc.Enabled = true;
            lkpHorarioextra.Enabled = true;
            lkpAfectoVidaLey.Enabled = true;
            lkpRegimenPension.Enabled = true;
            txtCUSP.Enabled = true;
            lkpTipoComision.Enabled = true;
            lkpTipoContrato.Enabled = true;
            lkpAsigFami.Enabled = true;
            lkpTipoPago.Enabled = true;
            lkpPeriodicidadpago.Enabled = true;
            lkpRegimenLaboral.Enabled = true;
            lkpMoneda.Enabled = true;
            lkpscrtTrab.Enabled = true;
            txtsueldo.Enabled = true;
            txtporcquincena.Enabled = true;
            txtporcComision.Enabled = true;
            lkpBancoRem.Enabled = true;
            lkptipocuentarem.Enabled = true;
            txtNroCuentaRem.Enabled = true;
            txtCCIREM.Enabled = true;
            lkpMonedaCTS.Enabled = true;
            lkpBancoCTS.Enabled = true;
            lkptipocuentacts.Enabled = true;
            txtNroCuentaCTS.Enabled = true;
            txtCCICTS.Enabled = true;
            lkpregimensalud.Enabled = true;
            lkpSituacionsalud.Enabled = true;
            lkpscrtsalud1.Enabled = true;
            lkpTipotrabsalud.Enabled = true;
            lkpnacionalidad.Enabled = true;
            lkpCentroRiesgo.Enabled = true;
            lkpAfectoVidaLey.Enabled = true;
            lkpCIAsegurovidaley.Enabled = true;

        }

        private void bloquearcontroles()
        {

            txtNombre.Enabled = false;
            txtApellidoPaterno.Enabled = false;
            txtApellidoMaterno.Enabled = false;
            dtFchNacimiento.Enabled = false;
            lkps.Enabled = false;
            lkpEstadoCivil.Enabled = false;
            dtFchNacimiento.Enabled = false;
            dtfechaIngreso.Enabled = false;
            txtCelular.Enabled = false;
            txtFijo.Enabled = false;
            txtCorreoPersonal.Enabled = false;
            lkpNivelEducativo.Enabled = false;
            lkpPais.Enabled = false;
            lkpDepartamento.Enabled = false;
            lkpProvincia.Enabled = false;
            lkpdistrito.Enabled = false;
            lkpTipoVia.Enabled = false;
            txtNombrevia.Enabled = false;
            txtNro.Enabled = false;
            txtDep.Enabled = false;
            txtInterior.Enabled = false;
            txtManzana.Enabled = false;
            txtLote.Enabled = false;
            txtKm.Enabled = false;
            txtEtapa.Enabled = false;
            txtBlock.Enabled = false;
            lkpTipozona.Enabled = false;
            txtNombreZona.Enabled = false;
            txtDireccion.Enabled = false;
            lkpEmpresa.Enabled = false;
            lkpSedeEmpresa.Enabled = false;
            lkpCategoriaTrab.Enabled = false;
            lkpTipoTrabajador.Enabled = false;
            lkpCatOcupacional.Enabled = false;
            lkpArea.Enabled = false;
            lkpCargo.Enabled = false;
            lkpCalificacionPuesto.Enabled = false;
            lkpCco.Enabled = false;
            dtIniContrato.Enabled = false;
            dtFinContrato.Enabled = false;
            dtFchPuesto.Enabled = false;
            txtCorreoLaboral.Enabled = false;
            lkpafectosctr.Enabled = false;
            lkpAfectoVidaLey.Enabled = false;
            lkpHorarioNoc.Enabled = false;
            lkpHorarioextra.Enabled = false;
            lkpAfectoVidaLey.Enabled = false;
            lkpRegimenPension.Enabled = false;
            txtCUSP.Enabled = false;
            lkpTipoComision.Enabled = false;
            lkpTipoContrato.Enabled = false;
            lkpAsigFami.Enabled = false;
            lkpTipoPago.Enabled = false;
            lkpPeriodicidadpago.Enabled = false;
            lkpRegimenLaboral.Enabled = false;
            lkpMoneda.Enabled = false;
            lkpscrtTrab.Enabled = false;
            txtsueldo.Enabled = false;
            txtporcquincena.Enabled = false;
            txtporcComision.Enabled = false;
            lkpBancoRem.Enabled = false;
            lkptipocuentarem.Enabled = false;
            txtNroCuentaRem.Enabled = false;
            txtCCIREM.Enabled = false;
            lkpMonedaCTS.Enabled = false;
            lkpBancoCTS.Enabled = false;
            lkptipocuentacts.Enabled = false;
            txtNroCuentaCTS.Enabled = false;
            txtCCICTS.Enabled = false;
            lkpregimensalud.Enabled = false;
            lkpSituacionsalud.Enabled = false;
            lkpscrtsalud1.Enabled = false;
            lkpTipotrabsalud.Enabled = false;
            lkpnacionalidad.Enabled = false;
            lkpCentroRiesgo.Enabled = false;
            lkpAfectoVidaLey.Enabled = false;
            lkpCIAsegurovidaley.Enabled = false;
        }

        private void btn_nuevo_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void lkps_EditValueChanged(object sender, EventArgs e)
        {
            if (lkps.EditValue.ToString() == "F")
            {
                Image imgEmpresaLarge = Properties.Resources.female64;
                pictureEdit1.EditValue = imgEmpresaLarge;
            }
            else if (lkps.EditValue.ToString() == "M")
            {
                Image imgEmpresaLarge = Properties.Resources.Male64;
                pictureEdit1.EditValue = imgEmpresaLarge;
            }
        }

        private void dtIniContrato_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void dtfechaIngreso_EditValueChanged(object sender, EventArgs e)
        {
            
            string ingreso = Convert.ToString(dtfechaIngreso.EditValue.ToString());
            dtIniContrato.EditValue = ingreso;
            dtFchPuesto.EditValue = ingreso;
        }

        private void lkpTipoDocumento_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpTipoDocumento.EditValue != null)
            {
                eProveedor obj = new eProveedor();
                obj = unit.Proveedores.Validar_NroDocumento<eProveedor>(19, "", lkpTipoDocumento.EditValue.ToString());
                txtnumdocumento.Properties.MaxLength = obj.ctd_digitos;
            }
        }

        private void lkpTipozona_EditValueChanged(object sender, EventArgs e)
        {
            
            if (lkpTipozona.EditValue == null)
            {

            }
            else if (lkpTipozona.EditValue != null)

            {
                txtNombreZona.Enabled = true;
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                    txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
                txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text);

            }
        }

        private void CargarLookUpEdit()
        {
            try
            {
                CargarCombosGridLookup("TipoDocumento", lkpTipoDocumento, "cod_tipo_documento", "dsc_tipo_documento", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("EmpresaUsuario", lkpEmpresa, "cod_empresa", "dsc_empresa", "", valorDefecto: true,cod_usuario:Program.Sesion.Usuario.cod_usuario);
                unit.Trabajador.CargaCombosLookUp("EstadoCivil", lkpEstadoCivil, "cod_estado_civil", "dsc_estado_civil", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Tipovia", lkpTipoVia, "cod_tipo_via", "dsc_tipo_via", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Pais", lkpPais, "cod_pais", "dsc_pais", "");
                unit.Trabajador.CargaCombosLookUp("Sexo", lkps, "cod_sexo", "dsc_sexo", "");
                unit.Trabajador.CargaCombosLookUp("TipoZona", lkpTipozona, "cod_tipo_zona", "dsc_tipo_zona", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Tipovia", lkpTipoVia, "cod_tipo_via", "dsc_tipo_via", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Nacionalidad", lkpnacionalidad, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Categoria", lkpCategoriaTrab, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Ocupacional", lkpCatOcupacional, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Moneda", lkpMoneda, "cod_moneda", "dsc_moneda", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Moneda", lkpMonedaCTS, "cod_moneda", "dsc_moneda", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("EntidadFinanciera", lkpBancoRem, "cod_banco", "dsc_banco", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("EntidadFinanciera", lkpBancoCTS, "cod_banco", "dsc_banco", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("TipoCuenta", lkptipocuentarem, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("tipocuentacts", lkptipocuentacts, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("TipoComision", lkpTipoComision, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("TipoContrato", lkpTipoContrato, "cod_tipoContrato", "dsc_tipoContrato", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("PeriodicidadPago", lkpPeriodicidadpago, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("RegimenPension", lkpRegimenPension, "cod_APF", "dsc_APF", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("RegimenLaboral", lkpRegimenLaboral, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("regimensegurosalud", lkpregimensalud, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("SituacionTrabajador_salud", lkpSituacionsalud, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("SCRTSALUD", lkpscrtsalud1, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("SCRTPension", lkpscrtTrab, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("TrabajadorSalud", lkpTipotrabsalud, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("NivAcademico", lkpNivelEducativo, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("TipoPago", lkpTipoPago, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("SituacionEspecial", lkpCalificacionPuesto, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Sino", lkpAsigFami, "cod_sino", "dsc_sino", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("TipoTrabajo", lkpTipoTrabajador, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Sino", lkpafectosctr, "cod_sino", "dsc_sino", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Sino", lkpHorarioNoc, "cod_sino", "dsc_sino", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Sino", lkpHorarioextra, "cod_sino", "dsc_sino", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("Sino", lkpAfectoVidaLey, "cod_sino", "dsc_sino", "", valorDefecto: true);
    
                limpiar();
                //lkpTipoDocumento.EditValue = "DI001";
                //lkpCategoriaTrab.EditValue = "00001";
                //lkpCatOcupacional.EditValue = "00001";
                //lkpEstadoCivil.EditValue = "01";
                //lkpnacionalidad.EditValue = "01";
                //lkpDepartamento.EditValue = "00015";
                //lkpMoneda.EditValue = "SOL";
                //lkpMonedaCTS.EditValue = "SOL";
                //lkpBancoRem.EditValue = "BA006";
                //lkpBancoCTS.EditValue = "BA006";
                //lkptipocuentarem.EditValue = "00002";
                //lkpPeriodicidadpago.EditValue = "00001";
                //lkpRegimenPension.EditValue = "00001";
                //lkpCargo.EditValue = "00001";
                lkpafectosctr.EditValue = "NO";
                lkpHorarioNoc.EditValue = "NO";
                lkpHorarioextra.EditValue = "NO";
                lkpAfectoVidaLey.EditValue = "NO";
                lkpAsigFami.EditValue = "NO";
                //lkpCalificacionPuesto.EditValue = "00001";
                //lkps.EditValue = "01";
                //lkpTipoPago.EditValue = "00001";
                //lkptipocuentacts.EditValue = "00002";
                //lkpTipoTrabajador.EditValue = "00004";
              
                



                lkpEmpresa.EditValue = Program.Sesion.EmpresaList[0].cod_empresa;

                lkpPais.EditValue = "00001";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void txtNombreZona_EditValueChanged(object sender, EventArgs e)
        {
            if (txtNombreZona.Text == "")
            {
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());
            }
            else if (txtNombreZona.Text != "")

            {

                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());

            }
        }

        private void lkpTipoVia_EditValueChanged(object sender, EventArgs e)
        {

            if (lkpTipoVia.EditValue == null)
            {
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());
            }
            else if (lkpTipoVia.EditValue != null)

            {
                txtNombrevia.Enabled = true;
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                  txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
          txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
          lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
          , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());

            }
        }

        private void txtNombrevia_EditValueChanged(object sender, EventArgs e)
        {
            if (txtNombrevia.Text == "")
            {
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());
            }
            else if (txtNombrevia.Text != "")

            {

                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());

            }
        }

        private void txtNro_EditValueChanged(object sender, EventArgs e)
        {
            if (txtNro.Text == "")
            {
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());
            }
            else if (txtNro.Text != "")

            {

                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                  txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
          txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
          lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
          , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());

            }
        }

        private void txtDep_EditValueChanged(object sender, EventArgs e)
        {
            if (txtDep.Text == "")
            {
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());
            }
            else if (txtDep.Text != "")

            {

                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                   txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
           txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
           lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
           , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());

            }
        }

        private void txtInterior_EditValueChanged(object sender, EventArgs e)
        {
            if (txtInterior.Text == "")
            {
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());
            }
            else if (txtInterior.Text != "")

            {

                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());
            }
        }
        private void txtManzana_EditValueChanged(object sender, EventArgs e)
        {
            if (txtManzana.Text == "")
            {
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());
            }
            else if (txtManzana.Text != "")

            {

                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                  txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
          txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
          lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
          , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());

            }
        }

        private void txtLote_EditValueChanged(object sender, EventArgs e)
        {
            if (txtLote.Text == "")
            {
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());
            }
            else if (txtLote.Text != "")

            {

                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());

            }

        }

        private void txtKm_EditValueChanged(object sender, EventArgs e)
        {
            if (txtKm.Text == "")
            {
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());
            }
            else if (txtKm.Text != "")

            {

                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                  txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
          txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
          lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
          , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());

            }
        }

        private void txtEtapa_EditValueChanged(object sender, EventArgs e)
        {
            if (txtEtapa.Text == "")
            {
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());
            }
            else if (txtEtapa.Text != "")

            {

                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                  txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
          txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
          lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
          , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());

            }
        }

        private void txtBlock_EditValueChanged(object sender, EventArgs e)
        {
            if (txtBlock.Text == "")
            {
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());
            }
            else if (txtBlock.Text != "")

            {

                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                   txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
           txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
           lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
           , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());

            }
        }

        private void glkpdistrito_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpdistrito.EditValue == null)
            {
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                 txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
         txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
         lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
         , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());

            }
            else if (lkpdistrito.EditValue != null)

            {
                Direccion(lkpTipozona.Text == null ? null : lkpTipozona.Text.ToString(),
                   txtNombreZona.Text, lkpTipoVia.Text == null ? null : lkpTipoVia.Text.ToString(), txtNombrevia.Text,
           txtNro.Text, txtDep.Text, txtInterior.Text, txtManzana.Text, txtLote.Text, txtKm.Text, txtEtapa.Text, txtBlock.Text,
           lkpDepartamento.Text == null ? null : lkpDepartamento.Text.ToString(), lkpProvincia.Text == null ? null : lkpProvincia.Text.ToString()
           , lkpdistrito.Text == null ? null : lkpdistrito.Text.ToString());


            }


        }

        private void txtnumdocumento_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtCelular_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtFijo_KeyPress(object sender, KeyPressEventArgs e)
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

        private void limpiar()
        {
            txtNombre.Text = "";
            txtApellidoPaterno.Text = "";
            txtApellidoMaterno.Text = "";
            dtFchNacimiento.EditValue = "";
            lkps.EditValue = "";
            dtFchNacimiento.EditValue = "";
            txtCelular.Text = "";
            txtFijo.Text = "";
            txtCorreoPersonal.Text = "";
            lkpNivelEducativo.EditValue = "";
            lkpPais.EditValue = "00001";
            lkpDepartamento.EditValue = "00001";
            lkpProvincia.EditValue = "00128";
            lkpdistrito.EditValue = "";
            lkpTipoVia.EditValue = "";
            txtNombrevia.Text = "";
            txtNro.Text = "";
            txtDep.Text = "";
            txtInterior.Text = "";
            txtManzana.Text = "";
            txtLote.Text = "";
            txtKm.Text = "";
            txtEtapa.Text = "";
            txtBlock.Text = "";
            lkpTipozona.EditValue = "";
            txtNombreZona.Text = "";
            txtDireccion.Text = "";
            lkpEmpresa.EditValue = "00001";
            lkpSedeEmpresa.EditValue = "00001";
            lkpArea.EditValue = "";
            lkpCco.EditValue = "";
            txtCorreoLaboral.EditValue = "";
            txtCUSP.Text = "";
            lkpTipoComision.EditValue = "";
            lkpTipoContrato.EditValue = "";
            lkpRegimenLaboral.EditValue = "";
            lkpscrtTrab.EditValue = "";
            txtporcquincena.Text = "";
            txtporcComision.Text = "";
            txtNroCuentaRem.Text = "";
            txtCCIREM.Text = "";
            txtNroCuentaCTS.Text = "";
            txtCCICTS.Text = "";



            lkpTipoDocumento.EditValue = "DI001";
            lkpCategoriaTrab.EditValue = "";
            lkpCatOcupacional.EditValue = "";
            lkpEstadoCivil.EditValue = "01";
            lkpnacionalidad.EditValue = "01";
            lkpDepartamento.EditValue = "00015";




            lkpCargo.EditValue = "00001";
            lkpafectosctr.EditValue = "00";
            lkpHorarioNoc.EditValue = "00";
            lkpHorarioextra.EditValue = "00";
            lkpAfectoVidaLey.EditValue = "00";
            lkps.EditValue = "01";
            lkptipocuentacts.EditValue = "00002";
            lkpTipoTrabajador.EditValue = "";

            
            dtfechaIngreso.EditValue = DateTime.Today;
            dtFinContrato.EditValue = DateTime.Today;
            dtIniContrato.EditValue = DateTime.Today;
            dtFchPuesto.EditValue = DateTime.Today;
        }

        private void Direccion(string tipo_zona = "", string nombre_zona = "", string tipo_via = "", string nombre_via = "",
            string nro = "", string dep = "", string interior = "", string mz = "", string lote = "", string km = "", string etapa = "",
            string block = "", string departamento = "", string provincia = "", string distrito = "", string referencia = "")
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




    }
}