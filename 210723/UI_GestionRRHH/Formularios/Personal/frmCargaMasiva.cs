using DevExpress.XtraBars;
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
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using System.Collections;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using DevExpress.Spreadsheet.Export;
using DevExpress.XtraVerticalGrid;
using DevExpress.Spreadsheet;
using UI_GestionRRHH.Tools;
using System.Globalization;
using System.Threading;
using DevExpress.Charts.Native;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;
using System.Net;
using DevExpress.Utils.Extensions;

namespace UI_GestionRRHH.Formularios.Personal
{
    public partial class frmCargaMasiva : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly UnitOfWork unit;
        internal Area MiAccion = Area.Nuevo;
        List<eTrabajador> ListadoTrabajador = new List<eTrabajador>();
        List<eTrabajador> ListDepartamentos = new List<eTrabajador>();
        List<eTrabajador> ListProvincias = new List<eTrabajador>();
        List<eTrabajador> ListDistritos = new List<eTrabajador>();
        List<eTrabajador> ListEmpresa = new List<eTrabajador>();
        private static IEnumerable<eTrabajador>lstipodocumento,lssexo, lstvia, lstzona,lstProvincia, lstdepartamento, lstpais, lstnacionalidad, lstestadocivil, lstDistrito, lstTipovia, lstDep, lstProv, lstDist, lstdni, 
            lstSede,lstarea,lstcargo,lstempresa,lstipotrab,lsttipocontrato, lstcalpsto,lstcattrab,lstceco,lstsino,lsttipopago,lstperiodicidad, lsttipocuentacts,
            lstocupacional,lstbanco,lsttipocuenta,lstmoneda,lstregimenaseguramientosalud,lstregpension, lstreglaboral,lstsctrsalud, lstsctrsaludpension,lsttipotrabsalud,lstsittrabsalud,
            lstsegurosalud,lstsituacioninfolab,lscomisionmixta, lssegurovidaley;
        public eTrabajador eTipovia = new eTrabajador();
        public eTrabajador eTipozona = new eTrabajador();
        public eTrabajador eDepartamento = new eTrabajador();
        public eTrabajador eDistrito = new eTrabajador();
        public eTrabajador eProvincia = new eTrabajador();
        public eTrabajador eTrab = new eTrabajador();
        Brush ConCriterios = Brushes.Green;
        Brush SinCriterios = Brushes.Red;
        Brush NAplCriterio = Brushes.Orange;
        bool validacion = false;
        string cod_pais = "", cod_departamento = "", cod_provincia = "", cod_distrito = "", dsc_departamento = "", dsc_provincia = "", dsc_distrito = "", cod_empresa = "", cod_tipo_via = "",cod_trabajador="", validardni="";
        public frmCargaMasiva()
        {

            InitializeComponent();
            unit = new UnitOfWork();

            GrbDatos.Columns["colcod_trabajador"].Visible = false;
            GrbDatos.Columns["coldsc_nombres_completos"].Visible = false;
        }

        private void Inicializar()
        {
            try
            {
                switch (MiAccion)
                {
                    case Area.Nuevo:
                        tipodocumento();
                        CargarLookUpEdit();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }


        private void CargarLookUpEdit()
        {
            try
            {
                lkpTipoDocumento.DataSource = lstipodocumento; lkpTipoDocumento.DisplayMember = "dsc_tipo_documento"; lkpTipoDocumento.ValueMember = "cod_tipo_documento";
                lkpEmpresa.DataSource = lstempresa; lkpEmpresa.DisplayMember = "dsc_empresa"; lkpEmpresa.ValueMember = "cod_empresa";
                lkpSexo.DataSource = lssexo; lkpSexo.DisplayMember = "dsc_sexo"; lkpSexo.ValueMember = "cod_sexo";
                lkpestadocivil.DataSource = lstestadocivil;  lkpestadocivil.DisplayMember = "dsc_estado_civil"; lkpestadocivil.ValueMember = "cod_estado_civil";
                lkpsituacioninfolab.DataSource = lstsituacioninfolab; lkpsituacioninfolab.DisplayMember = "dsc_parametro"; lkpsituacioninfolab.ValueMember = "cod_parametro";
                lkpNacionalidad.DataSource = lstnacionalidad; lkpNacionalidad.DisplayMember = "dsc_parametro"; lkpNacionalidad.ValueMember = "cod_parametro";
                lkptipozona.DataSource = lstzona; lkpestadocivil.DisplayMember = "dsc_tipo_zona"; lkpestadocivil.ValueMember = "cod_tipo_zona";
                lkptipovia.DataSource = lstvia; lkptipovia.DisplayMember = "dsc_tipo_via"; lkptipovia.ValueMember = "cod_tipo_via";
                lkpDepartamento.DataSource = lstdepartamento; lkpDepartamento.DisplayMember = "dsc_departamento"; lkpDepartamento.ValueMember = "cod_departamento";
                lkpProvincia.DataSource = lstProv; lkpProvincia.DisplayMember = "dsc_provincia"; lkpProvincia.ValueMember = "cod_provincia";
                lkpDistrito.DataSource = lstDist; lkpDistrito.DisplayMember = "dsc_distrito"; lkpDistrito.ValueMember = "cod_distrito";
                lkpSedeEmpresa.DataSource = lstSede; lkpSedeEmpresa.DisplayMember = "dsc_sede_empresa"; lkpSedeEmpresa.ValueMember = "cod_sede_empresa";
                lkparea.DataSource = lstarea; lkparea.DisplayMember = "dsc_area"; lkparea.ValueMember = "cod_area";
                lkpcargo.DataSource = lstcargo; lkpcargo.DisplayMember = "dsc_cargo"; lkpcargo.ValueMember = "cod_cargo";
                lkptipotrabajadorsalud.DataSource= lstipotrab; lkptipotrabajadorsalud.DisplayMember = "dsc_parametro"; lkptipotrabajadorsalud.ValueMember = "cod_parametro";
                lkpregpension.DataSource = lstregpension; lkpregpension.DisplayMember = "dsc_APF"; lkpregpension.ValueMember = "cod_APF";
                lkptipocomision.DataSource = lkptipocomision; lkptipocomision.DisplayMember = "dsc_parametro"; lkptipocomision.ValueMember = "cod_parametro";
                lkptipocontrato.DataSource = lsttipocontrato; lkptipocontrato.DisplayMember = "dsc_tipoContrato"; lkptipocontrato.ValueMember = "cod_tipoContrato";
                lkpregimenlaboral.DataSource = lstreglaboral; lkpregimenlaboral.DisplayMember = "dsc_parametro"; lkpregimenlaboral.ValueMember = "cod_parametro";
                lkpperiodicidad.DataSource = lstperiodicidad; lkpperiodicidad.DisplayMember = "dsc_parametro"; lkpperiodicidad.ValueMember = "cod_parametro";
                lkpmoneda.DataSource = lstmoneda; lkpmoneda.DisplayMember = "dsc_moneda"; lkpmoneda.ValueMember = "cod_moneda";
                lkpSino.DataSource = lstsino; lkpSino.DisplayMember = "dsc_sino"; lkpSino.ValueMember = "cod_sino";
                lkptipopago.DataSource = lsttipopago; lkptipopago.DisplayMember = "dsc_parametro"; lkptipopago.ValueMember = "cod_parametro";
                lkpBanco.DataSource = lstbanco; lkpBanco.DisplayMember = "dsc_banco"; lkpBanco.ValueMember = "cod_banco";
                lkpTipoCuenta.DataSource = lsttipocuenta; lkpTipoCuenta.DisplayMember = "dsc_parametro"; lkpTipoCuenta.ValueMember = "cod_parametro";
                lkpsegurosalud.DataSource = lstregimenaseguramientosalud; lkpsegurosalud.DisplayMember = "dsc_parametro"; lkpsegurosalud.ValueMember = "cod_parametro";
                lkpsituaciontrabajadorsalud.DataSource = lstsittrabsalud; lkpsituaciontrabajadorsalud.DisplayMember = "dsc_parametro"; lkpsituaciontrabajadorsalud.ValueMember = "cod_parametro";
                lkptipotrabajador.DataSource = lstipotrab; lkptipotrabajador.DisplayMember = "dsc_parametro"; lkptipotrabajador.ValueMember = "cod_parametro";
                lkpcatocupacional.DataSource = lstocupacional; lkpcatocupacional.DisplayMember = "dsc_parametro"; lkpcatocupacional.ValueMember = "cod_parametro";
                lkpCategoriaTrab.DataSource= lstcattrab; lkpCategoriaTrab.DisplayMember = "dsc_parametro"; lkpCategoriaTrab.ValueMember = "cod_parametro";
                //lkpcalificacionpuesto.DataSource = lstcalpsto; lkpcalificacionpuesto.DisplayMember = "dsc_parametro"; lkpcalificacionpuesto.ValueMember = "cod_parametro";
                lkpsctrsalud.DataSource = lstsctrsalud; lkpsctrsalud.DisplayMember = "dsc_parametro"; lkpsctrsalud.ValueMember = "cod_parametro";
                lkpsctrsaludpension.DataSource = lstsctrsaludpension; lkpsctrsaludpension.DisplayMember = "dsc_parametro"; lkpsctrsaludpension.ValueMember = "cod_parametro";
                lkptipocuentacts.DataSource = lsttipocuentacts; lkptipocuentacts.DisplayMember = "dsc_parametro"; lkptipocuentacts.ValueMember = "cod_parametro";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }


        private void lkpTipoDocumento_EditValueChanged(object sender, EventArgs e)
        {
            gvTrabajador.PostEditor();
        }

        private void lkpSexo_EditValueChanged(object sender, EventArgs e)
        {
            gvTrabajador.PostEditor();
        }

        private void lkpEstadoCivil_EditValueChanged(object sender, EventArgs e)
        {
            gvTrabajador.PostEditor();
        }

        private void lkpEmpresa_EditValueChanged(object sender, EventArgs e)
        {


            gvTrabajador.PostEditor();
            eTrabajador objFact = gvTrabajador.GetFocusedRow() as eTrabajador;
            foreach (GridColumn col in gvTrabajador.Columns)
            {
                if (col.FieldName == "cod_empresa")
                {

                    objFact.cod_empresa = null;
                    ListEmpresa = unit.Trabajador.CombosEnGridControl<eTrabajador>("EmpresaUsuario", cod_usuario: Program.Sesion.Usuario.cod_usuario);
                }

            }
            if (objFact.cod_empresa != null) cod_empresa = ListEmpresa.Find(x => x.cod_empresa == objFact.cod_empresa).cod_empresa;
        }
        private void lkpSedeEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            gvTrabajador.PostEditor();
        }

        private void btnImportarTrabajadores_ItemClick(object sender, ItemClickEventArgs e)
        {
            CargarExcel();
        }

       
        private void CargarExcel()
        {
            try
            {
                List<eTrabajadorMasivo> infogeneralist = new List<eTrabajadorMasivo>();

                var excel = new ReadExcel();

                string ruta = new ReadExcel().ObtenerExcel();

                if (String.IsNullOrEmpty(ruta)) { return; }

                using (var frmExcel = new ExcelSplash(ruta))
                {
                    if (frmExcel.ShowDialog() == DialogResult.OK)
                    {
                        var dt = frmExcel.DtExcel;
                        if (dt != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string cod_sunat_empresa = dt.Rows[i][0].ToString();
                                string cod_sunat = dt.Rows[i][4].ToString();
                                string cod_sunat_sexo = dt.Rows[i][6].ToString();
                                string cod_sunat_estadocivil = dt.Rows[i][10].ToString();
                                string cod_sunat_situacion_trabajador_2 = dt.Rows[i][12].ToString();
                                string cod_sunat_nacionalidad = dt.Rows[i][13].ToString();
                                string cod_sunat_catergoria_trabajador = dt.Rows[i][14].ToString();
                                string cod_sunat_tipo_zona = dt.Rows[i][27].ToString();
                                string cod_sunat_tipo_via = dt.Rows[i][17].ToString();
                                string cod_sunat_departamento = dt.Rows[i][29].ToString();
                                string cod_sunat_provincia = cod_sunat_departamento + dt.Rows[i][30].ToString();
                                string cod_sunat_distrito = cod_sunat_provincia + dt.Rows[i][31].ToString();
                                string cod_sunat_sedeempresa = dt.Rows[i][32].ToString();
                                string cod_area_empresa = cod_sunat_empresa + cod_sunat_sedeempresa;
                                string cod_sunat_area = dt.Rows[i][33].ToString();
                                string cod_cargo_empresa = cod_area_empresa + cod_sunat_area;
                                string cod_sunat_cargo = dt.Rows[i][34].ToString();
                                string cod_sunat_tipotrabajador_salud = dt.Rows[i][35].ToString();
                                string cod_sunat_sist_pension = dt.Rows[i][36].ToString();
                                string cod_sunat_tipo_comision_pension = dt.Rows[i][38].ToString();
                                string cod_sunat_tipo_contrato = dt.Rows[i][39].ToString();
                                string cod_sunat_regimen_laboral = dt.Rows[i][40].ToString();
                                string cod_sunat_periodicidad_pagos = dt.Rows[i][41].ToString();
                                string cod_sunat_moneda = dt.Rows[i][42].ToString();
                                string cod_sunat_flg_asignacionfamiliar = dt.Rows[i][46].ToString();
                                string cod_sunat_tipo_pago = dt.Rows[i][47].ToString();
                                string cod_sunat_banco = dt.Rows[i][48].ToString();
                                string cod_sunat_tipo_cuenta = dt.Rows[i][49].ToString();
                                string cod_sunatsctraseguramiento = dt.Rows[i][52].ToString();
                                string cod_sunat_situacion_trabajador_salud = dt.Rows[i][53].ToString();
                                string cod_sunat_tipotrabajador = dt.Rows[i][54].ToString();
                                string cod_sunat_ocupacional = dt.Rows[i][55].ToString();
                                string cod_sunat_situacion_especial = dt.Rows[i][56].ToString();
                                string cod_sunat_sctrsalud = dt.Rows[i][58].ToString();
                                string cod_sunat_sctrpensionsalud = dt.Rows[i][59].ToString();
                                string cod_sunat_tipo_cuenta_CTS = dt.Rows[i][65].ToString();
                                string cod_sunat_flg_afectoSCTR = dt.Rows[i][68].ToString();
                                string cod_sunat_flg_AfectoVidaLey = dt.Rows[i][69].ToString();
                                string cod_sunat_flg_horario_nocturno = dt.Rows[i][70].ToString();
                                string cod_sunat_flg_horas_extras = dt.Rows[i][71].ToString();
                                string cod_sunat_seguroley = dt.Rows[i][72].ToString();


                                


                                infogeneralist.Add(new eTrabajadorMasivo()
                                {
                                    dsc_empresa = lstempresa.FirstOrDefault(td => td.cod_empresa == cod_sunat_empresa)?.cod_empresa,
                                    dsc_nombres = dt.Rows[i][1].ToString(),
                                    dsc_apellido_paterno = dt.Rows[i][2].ToString(),
                                    dsc_apellido_materno = dt.Rows[i][3].ToString(),
                                    cod_tipo_documento = lstipodocumento.FirstOrDefault(td => td.cod_sunat == cod_sunat)?.cod_tipo_documento,
                                    dsc_documento = dt.Rows[i][5].ToString(),
                                    flg_sexo = lssexo.FirstOrDefault(td => td.cod_sunat == cod_sunat_sexo)?.cod_sexo,
                                    fch_nacimiento = dt.Rows[i][7].ToString(),
                                    dsc_celular = dt.Rows[i][8].ToString(),
                                    dsc_telefono = dt.Rows[i][9].ToString(),
                                    cod_estado_civil = lstestadocivil.FirstOrDefault(td => td.cod_sunat == cod_sunat_estadocivil)?.cod_estado_civil,
                                    dsc_mail_1 = dt.Rows[i][11].ToString(),
                                    cod_situacion_trabajador_2 = lstsituacioninfolab.FirstOrDefault(td => td.cod_sunat == cod_sunat_situacion_trabajador_2)?.cod_parametro,
                                    cod_nacionalidad = lstnacionalidad.FirstOrDefault(td => td.cod_sunat == cod_sunat_nacionalidad)?.cod_parametro,
                                    cod_catergoria_trabajador = lstcattrab.FirstOrDefault(td => td.cod_sunat == cod_sunat_catergoria_trabajador)?.cod_parametro,
                                    fch_ingreso = dt.Rows[i][15].ToString(),
                                    cod_tipozona = lstzona.FirstOrDefault(td => td.cod_sunat == cod_sunat_tipo_zona)?.cod_tipo_zona,
                                    dsc_nombre_zona = dt.Rows[i][17].ToString(),
                                    cod_tipo_via = lstvia.FirstOrDefault(td => td.cod_sunat == cod_sunat_tipo_via)?.cod_tipo_via,
                                    dsc_tipo_via = dt.Rows[i][19].ToString(),
                                    dsc_nro_vivienda = dt.Rows[i][20].ToString(),
                                    dsc_departamento_dir = dt.Rows[i][21].ToString(),
                                    dsc_interior = dt.Rows[i][22].ToString(),
                                    dsc_manzana = dt.Rows[i][23].ToString(),
                                    cod_lote = dt.Rows[i][24].ToString(),
                                    dsc_km = dt.Rows[i][25].ToString(),
                                    dsc_etapa = dt.Rows[i][26].ToString(),
                                    dsc_block = dt.Rows[i][27].ToString(),
                                    cod_departamento = lstdepartamento.FirstOrDefault(td => td.cod_sunat == cod_sunat_departamento)?.cod_departamento,
                                    cod_provincia = lstProv.FirstOrDefault(td => td.cod_sunat == cod_sunat_provincia)?.cod_provincia,
                                    cod_distrito = lstDistrito.FirstOrDefault(td => td.cod_sunat == cod_sunat_distrito)?.cod_distrito,
                                    cod_sede_empresa = lstSede.FirstOrDefault(td => td.cod_empresa == cod_sunat_empresa && td.cod_sede_empresa == cod_sunat_sedeempresa)?.cod_sede_empresa,
                                    cod_area = lstarea.FirstOrDefault(td => td.cod_empresa == cod_area_empresa && td.cod_area == cod_sunat_area)?.cod_area,
                                    cod_cargo = lstcargo.FirstOrDefault(td => td.cod_empresa == cod_cargo_empresa && td.cod_cargo == cod_sunat_cargo)?.cod_cargo,
                                    cod_tipo_trabajador_salud = lsttipotrabsalud.FirstOrDefault(td => td.cod_sunat == cod_sunat_tipotrabajador_salud)?.cod_parametro,
                                    dsc_pref_ceco = dt.Rows[i][36].ToString(),
                                    cod_sist_pension = lstregpension.FirstOrDefault(td => td.cod_sunat == cod_sunat_sist_pension)?.cod_APF,
                                    cod_CUSPP = dt.Rows[i][38].ToString(),
                                    comision_AFP = lscomisionmixta.FirstOrDefault(td => td.cod_sunat == cod_sunat_tipo_comision_pension)?.cod_parametro,
                                    cod_tipo_contrato = lsttipocontrato.FirstOrDefault(td => td.cod_sunat == cod_sunat_tipo_contrato)?.cod_tipoContrato,
                                    cod_regimen_laboral = lstreglaboral.FirstOrDefault(td => td.cod_sunat == cod_sunat_regimen_laboral)?.cod_parametro,
                                    cod_periodicidad_pagos = lstperiodicidad.FirstOrDefault(td => td.cod_sunat == cod_sunat_regimen_laboral)?.cod_parametro,
                                    cod_moneda = lstmoneda.FirstOrDefault(td => td.cod_sunat == cod_sunat_moneda)?.cod_moneda,
                                    imp_sueldo_base = dt.Rows[i][44].ToString(),
                                    dsc_porcentajequincena = dt.Rows[i][45].ToString(),
                                    dsc_porcentajecomision = dt.Rows[i][46].ToString(),
                                    flg_asignacionfamiliar = lstsino.FirstOrDefault(td => td.cod_sino == cod_sunat_flg_asignacionfamiliar)?.cod_sino,
                                    cod_tipo_pago = lsttipopago.FirstOrDefault(td=> td.cod_sino == cod_sunat_tipo_pago)?.cod_parametro,
                                    cod_banco = lstbanco.FirstOrDefault(td => td.cod_sunat == cod_sunat_banco)?.cod_parametro,
                                    cod_tipo_cuenta = lsttipocuenta.FirstOrDefault(td => td.cod_sunat == cod_sunat_tipo_cuenta)?.cod_banco,
                                    dsc_cta_bancaria = dt.Rows[i][51].ToString(),
                                    dsc_cta_interbancaria = dt.Rows[i][52].ToString(),
                                    cod_regimenasegsalud = lstregimenaseguramientosalud.FirstOrDefault(td => td.cod_sunat == cod_sunatsctraseguramiento)?.cod_parametro,
                                    cod_situacion_trabajador_salud = lstsittrabsalud.FirstOrDefault(td => td.cod_sunat == cod_sunat_situacion_trabajador_salud)?.cod_parametro,
                                    cod_tipo_trabajador = lstipotrab.FirstOrDefault(td => td.cod_sunat == cod_sunat_tipotrabajador)?.cod_parametro,
                                    cod_ocupacional = lstocupacional.FirstOrDefault(td => td.cod_sunat == cod_sunat_ocupacional)?.cod_parametro,
                                    cod_situacion_especial = lstcalpsto.FirstOrDefault(td => td.cod_sunat == cod_sunat_situacion_especial)?.cod_parametro,
                                    cod_sctr_salud = lstsctrsalud.FirstOrDefault(td => td.cod_sunat == cod_sunat_sctrsalud)?.cod_parametro,
                                    cod_sctr_salud_pension = lstsctrsaludpension.FirstOrDefault(td => td.cod_sunat == cod_sunat_sctrpensionsalud)?.cod_parametro,
                                    fch_vencimiento = dt.Rows[i][62].ToString(),
                                    cod_moneda_CTS = lstmoneda.FirstOrDefault(td => td.cod_sunat == cod_sunat_moneda)?.cod_moneda,
                                    cod_banco_CTS = lstbanco.FirstOrDefault(td => td.cod_sunat == cod_sunat_banco)?.cod_parametro,
                                    cod_tipo_cuenta_CTS = lsttipocuentacts.FirstOrDefault(td => td.cod_sunat == cod_sunat_tipo_cuenta_CTS)?.cod_parametro,
                                    dsc_cta_bancaria_CTS = dt.Rows[i][67].ToString(),
                                    dsc_cta_interbancaria_CTS = dt.Rows[i][68].ToString(),
                                    flg_afectoSCTR = lstsino.FirstOrDefault(td => td.cod_sino == cod_sunat_flg_afectoSCTR)?.cod_sino,
                                    flg_AfectoVidaLey = lstsino.FirstOrDefault(td => td.cod_sino == cod_sunat_flg_AfectoVidaLey)?.cod_sino,
                                    flg_horario_nocturno = lstsino.FirstOrDefault(td => td.cod_sino == cod_sunat_flg_horario_nocturno)?.cod_sino,
                                    flg_horas_extras = lstsino.FirstOrDefault(td => td.cod_sino == cod_sunat_flg_horas_extras)?.cod_sino,
                                    codsunat_seguroley = lssegurovidaley.FirstOrDefault(td => td.cod_sunat == cod_sunat_seguroley)?.cod_parametro,


                                });
                                //eTrabajadorMasivo a = new eTrabajadorMasivo();
                                //var dnidsc = infogeneralist.Find(x => x.dsc_documento == a.dsc_documento).dsc_documento;

                                bsTrabajador.DataSource = infogeneralist;
                                if(infogeneralist.Count > 0) { btnfilaPredeterminado.Enabled = true; }
                                //gcExcel.DataSource = prosList.ToList();
                                gvTrabajador.RefreshData();

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //SplashScreenManager.CloseForm();
                MessageBox.Show(ex.Message);
            }
        }

        private void lkpBanco_EditValueChanged(object sender, EventArgs e)
        {
            gvTrabajador.PostEditor();
        }

        private void lkpTipoCuenta_EditValueChanged(object sender, EventArgs e)
        {
            gvTrabajador.PostEditor();
        }



        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (chkInfogeneral.Checked == false) { GrbInfogen.Visible = false; }
            else { GrbInfogen.Visible = true; }


        }

        private void chkInfolaboral_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (chkInfolaboral.Checked == false) GrbInfoLaboral.Visible = false;
            else { GrbInfoLaboral.Visible = true; }
        }
        private void chkInfobancaria_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (chkInfobancaria.Checked == false) GrbInfoBancaria.Visible = false;
            else { GrbInfoBancaria.Visible = true; }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            validacion = true;
            gvTrabajador.GetRowCellValue(0, coldsc_empresa);
            string estado = "SI";
            var x = bsTrabajador.DataSource as List<eTrabajadorMasivo>;

            foreach (var item in x)
            {
                var y = item.dsc_empresa;
                item.dsc_usuario = Program.Sesion.Usuario.cod_usuario;

                var result = unit.Trabajador.InsertarActualizarMasivo_Trabajador<eTrabajadorMasivo>(item);
            }
            //objFact = unit.Trabajador.InsertarActualizar_Trabajador<eTrabajador>(objFact);
        }

        private void gvTrabajador_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gcTrabajadores_ProcessGridKey(object sender, KeyEventArgs e)
        {
            ColumnView view = (sender as GridControl).FocusedView as ColumnView;
            if (view == null) return;
            if (e.KeyCode == Keys.Delete && view.Editable && view.SelectedRowsCount > 0) { view.DeleteSelectedRows(); }
            if (e.KeyCode == Keys.F2)
            {
                if (view.ActiveEditor != null) return; //Prevent record deletion when an in-place editor is invoked
                e.Handled = true;
                // if (DevExpress.XtraEditors.XtraMessageBox.Show("Record deletion", "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                view.CopyToClipboard();
                //string data = "";
                ArrayList rows = new ArrayList();

                // Add the selected rows to the list.
                Int32[] selectedRowHandles = gvTrabajador.GetSelectedRows();
                for (int i = 0; i < selectedRowHandles.Length; i++)
                {
                    int selectedRowHandle = selectedRowHandles[i];
                    if (selectedRowHandle >= 0)
                        rows.Add(gvTrabajador.FocusedValue.ToString());
                }
                try
                {
                    gvTrabajador.BeginUpdate();
                    for (int i = 0; i < rows.Count; i++)
                    {
                        DataRow row = rows[i] as DataRow;
                        // Change the field value.
                        row["dsc_nombre"] = true;
                    }
                }
                finally
                {
                    gvTrabajador.EndUpdate();
                }
                {
                }


            }
        }



        private void barCheckItem1_CheckedChanged_1(object sender, ItemClickEventArgs e)
        {

            if (barCheckItem1.Checked == false)
            {
                GrbDatos.Columns["coldsc_empresa"].Visible = false;
                GrbDatos.Columns["colcod_tipo_documento"].Visible = false;
                GrbDatos.Columns["colcod_trabajador"].Visible = true;
                GrbDatos.Columns["coldsc_documento"].Visible = true;
                GrbDatos.Columns["coldsc_nombres_completos"].Visible = true;

            }
            if (barCheckItem1.Checked == true)
            {
                GrbDatos.Columns["coldsc_empresa"].Visible = true;
                GrbDatos.Columns["colcod_tipo_documento"].Visible = true;
                GrbDatos.Columns["colcod_trabajador"].Visible = true;
                GrbDatos.Columns["coldsc_documento"].Visible = true;
                GrbDatos.Columns["coldsc_nombres_completos"].Visible = true;
            }



        }

        private void gvTrabajador_CustomDrawColumnHeader_1(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvTrabajador_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvTrabajador_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            int a = 0; int b = 0;

            try
            {
                eTrabajador objFact = gvTrabajador.GetRow(e.RowHandle) as eTrabajador;
                if (e.RowHandle >= 0)
                {

                    //eTrabajador obj = gvTrabajador.GetRow(e.RowHandle) as eTrabajador;
                    if (objFact == null) return;
                    //if (validacion == true)
                    {
                        if (e.Column.FieldName == "cod_tipo_documento")
                        {
                           // if (string.IsNullOrWhiteSpace(objFact.cod_tipo_documento)) { e.Appearance.BackColor = Color.Red; a = 0; }
                           
                         //if (string.IsNullOrWhiteSpace(objFact.cod_tipo_documento)) { e.Appearance.BackColor = Color.Red; }                              

                        }
                        if (e.Column.FieldName == "dsc_documento") 
                        {
                            if (string.IsNullOrWhiteSpace(objFact.dsc_documento)) { e.Appearance.BackColor = Color.Red;b=1; }
                            
                        }
                        
                    }
                    
                   

                 

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewTextBoxEditingControl text = (DataGridViewTextBoxEditingControl)e.Control;
            text.KeyPress -= new KeyPressEventHandler(textbox_KeyPress);
            text.KeyPress += new KeyPressEventHandler(textbox_KeyPress);
        }


        private void textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            gvTrabajador.AddNewRow();
            string empresa = Program.Sesion.EmpresaList[0].cod_empresa;
            string tipodocumento = "DI001";
            string estadocivil = "01"; string nacionalidad = "00001"; string departamento = "00015"; string provincia = "00128";
            eTrabajador objFact = gvTrabajador.GetFocusedRow() as eTrabajador;

            gvTrabajador.SetRowCellValue(GridControl.NewItemRowHandle, gvTrabajador.Columns["dsc_empresa"], empresa);
            gvTrabajador.SetRowCellValue(GridControl.NewItemRowHandle, gvTrabajador.Columns["cod_tipo_documento"], tipodocumento);
            gvTrabajador.SetRowCellValue(GridControl.NewItemRowHandle, gvTrabajador.Columns["cod_estadocivil"], estadocivil);
            gvTrabajador.SetRowCellValue(GridControl.NewItemRowHandle, gvTrabajador.Columns["cod_departamento"], departamento);
            gvTrabajador.SetRowCellValue(GridControl.NewItemRowHandle, gvTrabajador.Columns["cod_provincia"], provincia);
            gvTrabajador.SetRowCellValue(GridControl.NewItemRowHandle, gvTrabajador.Columns["cod_nacionalidad"], nacionalidad);



        }

        private void gvTrabajador_ClipboardRowCopying(object sender, DevExpress.XtraGrid.Views.Grid.ClipboardRowCopyingEventArgs e)
        {

        }



        private void lkpDepartamento_EditValueChanged(object sender, EventArgs e)
        {

            gvTrabajador.PostEditor();

        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void gvTrabajador_ShownEditor(object sender, EventArgs e)
        {
            ColumnView view = (ColumnView)sender;
            if (view.FocusedColumn.FieldName == "cod_provincia")
            {
                LookUpEdit editor = (LookUpEdit)view.ActiveEditor;
                string cod_departamento = Convert.ToString(view.GetFocusedRowCellValue("cod_departamento"));
                editor.Properties.DataSource = obtenerProvincia(cod_departamento);
            }
            if (view.FocusedColumn.FieldName == "cod_distrito")
            {
                LookUpEdit editor = (LookUpEdit)view.ActiveEditor;
                string cod_departamento = Convert.ToString(view.GetFocusedRowCellValue("cod_departamento"));
                string cod_provincia = Convert.ToString(view.GetFocusedRowCellValue("cod_provincia"));
                editor.Properties.DataSource = ObtenerDistrito(cod_departamento, cod_provincia);
            }
            if (view.FocusedColumn.FieldName == "cod_sede_empresa")
            {
                LookUpEdit editor = (LookUpEdit)view.ActiveEditor;
                string cod_empresa = Convert.ToString(view.GetFocusedRowCellValue("dsc_empresa"));
                editor.Properties.DataSource = obtenersede(cod_empresa);
            }

            if (view.FocusedColumn.FieldName == "cod_area")
            {
                LookUpEdit editor = (LookUpEdit)view.ActiveEditor;
                string cod_empresa = Convert.ToString(view.GetFocusedRowCellValue("dsc_empresa"));
                string cod_sede_empresa = Convert.ToString(view.GetFocusedRowCellValue("cod_sede_empresa"));
                editor.Properties.DataSource = obtenerarea(cod_empresa,cod_sede_empresa);
            }
            if (view.FocusedColumn.FieldName == "cod_cargo")
            {
                LookUpEdit editor = (LookUpEdit)view.ActiveEditor;
                string cod_empresa = Convert.ToString(view.GetFocusedRowCellValue("dsc_empresa"));
                string cod_sede_empresa = Convert.ToString(view.GetFocusedRowCellValue("cod_sede_empresa"));
                string cod_area = Convert.ToString(view.GetFocusedRowCellValue("cod_area"));
                editor.Properties.DataSource = obtenercargo(cod_empresa, cod_sede_empresa,cod_area);
            }
            if (view.FocusedColumn.FieldName == "dsc_pref_ceco")
            {
                LookUpEdit editor = (LookUpEdit)view.ActiveEditor;
                string cod_empresa = Convert.ToString(view.GetFocusedRowCellValue("dsc_empresa"));
                editor.Properties.DataSource = obtenerceco(cod_empresa);
            }


            //obtenerceco
        }

        private void btnTrabajadores_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Application.OpenForms["frmTrabajadores_masivo"] != null)
            {
                Application.OpenForms["frmTrabajadores_masivo"].Activate();
            }
            else
            {
                frmTrabajadores_masivo frm = new frmTrabajadores_masivo();
                frm.cod_empresa = "00001";
                frm.cod_sede_empresa = "00001";
                frm.ShowDialog();

                string cod_tran = "";
                eTrabajador obj = new eTrabajador();
                ListadoTrabajador = frm.ListaSeleccionada;
                for (int i = 0; i < ListadoTrabajador.Count; i++)
                {
                    
                    gvTrabajador.SetRowCellValue(GridControl.NewItemRowHandle, gvTrabajador.Columns["cod_trabajador"], ListadoTrabajador[i].cod_trabajador);
                    gvTrabajador.AddNewRow();
                   
                }
                
                

            }
        }

        private void gvTrabajador_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }

        private void gvTrabajador_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            GridView view = sender as GridView;
            GridColumn empresa = view.Columns["dsc_empresa"];
            string cod_empresa = (string)view.GetRowCellValue(e.RowHandle, empresa);
            if (cod_empresa == null) { e.Valid = false; view.SetColumnError(empresa, "Tiene que Seleccionar una empresa"); }

            GridColumn tipo_doc = view.Columns["cod_tipo_documento"];
            string cod_tipo_documento = (string)view.GetRowCellValue(e.RowHandle, tipo_doc);
            if (cod_tipo_documento == null) { e.Valid = false; view.SetColumnError(tipo_doc, "Tiene que ingresar un Tipo de Documento"); }

            GridColumn documento = view.Columns["dsc_documento"];
            string dsc_documento = (string)view.GetRowCellValue(e.RowHandle, documento);
            if (dsc_documento == null) { e.Valid = false; view.SetColumnError(documento, "Tiene que ingresar un número de Documento"); }

            GridColumn civil = view.Columns["cod_estado_civil"];
            string dsc_estadocivil = (string)view.GetRowCellValue(e.RowHandle, civil);
            if (dsc_estadocivil == null) { e.Valid = false; view.SetColumnError(civil, "Tiene que ingresar Estado Civil"); }

            GridColumn nombre = view.Columns["dsc_nombres"];
            string dsc_nombre = (string)view.GetRowCellValue(e.RowHandle, nombre);
            if (dsc_nombre == null) { e.Valid = false; view.SetColumnError(nombre, "Tiene que ingresar un nombre"); }

            GridColumn apellidop = view.Columns["dsc_apellido_paterno"];
            string dsc_apellidop = (string)view.GetRowCellValue(e.RowHandle, apellidop);
            if (dsc_apellidop == null) { e.Valid = false; view.SetColumnError(apellidop, "Tiene que ingresar un Apellido Paterno"); }

            GridColumn apellidom = view.Columns["dsc_apellido_materno"];
            string dsc_apellidom = (string)view.GetRowCellValue(e.RowHandle, apellidom);
            if (dsc_apellidom == null) { e.Valid = false; view.SetColumnError(apellidom, "Tiene que ingresar un Apellido Materno"); }

            GridColumn fchnac = view.Columns["fch_nacimiento"];
            DateTime dsc_fchnaci = (DateTime)view.GetRowCellValue(e.RowHandle, fchnac);
            string fecha = Convert.ToString(dsc_fchnaci);
            if (fecha == "01/01/0001 0:00:00") { e.Valid = false; view.SetColumnError(fchnac, "Debe Ingresar una fecha valida de nacimiento"); }

            GridColumn celular = view.Columns["dsc_celular"];
            string dsc_celular = (string)view.GetRowCellValue(e.RowHandle, celular);
            if (dsc_celular == null) { e.Valid = false; view.SetColumnError(celular, "Debe ingresar número de celular"); }

            GridColumn sexo = view.Columns["flg_sexo"];
            string dsc_sexo = (string)view.GetRowCellValue(e.RowHandle, sexo);
            if (dsc_sexo == null) { e.Valid = false; view.SetColumnError(sexo, "Debe ingresar un sexo"); }

            GridColumn departamento = view.Columns["cod_departamento"];
            string cod_departamento = (string)view.GetRowCellValue(e.RowHandle, departamento);
            if (cod_departamento == null) { e.Valid = false; view.SetColumnError(departamento, "Debe ingresar un Departamento"); }

            GridColumn provincia = view.Columns["cod_provincia"];
            string cod_provincia = (string)view.GetRowCellValue(e.RowHandle, provincia);
            if (cod_provincia == null) { e.Valid = false; view.SetColumnError(provincia, "Debe ingresar una Provincia"); }

            GridColumn distrito = view.Columns["cod_distrito"];
            string cod_distrito = (string)view.GetRowCellValue(e.RowHandle, distrito);
            if (cod_distrito == null) { e.Valid = false; view.SetColumnError(distrito, "Debe ingresar una Distrito"); }

            GridColumn nacionalidad = view.Columns["cod_nacionalidad"];
            string cod_nacionalidad = (string)view.GetRowCellValue(e.RowHandle, nacionalidad);
            if (cod_nacionalidad == null) { e.Valid = false; view.SetColumnError(nacionalidad, "Debe ingresar una Nacionalidad"); }

            GridColumn direccion = view.Columns["dsc_direccion"];
            string dsc_direccion = (string)view.GetRowCellValue(e.RowHandle, direccion);
            if (dsc_direccion == null) { e.Valid = false; view.SetColumnError(direccion, "Debe ingresar Dirección"); }


            if (e.Valid) view.ClearColumnErrors();
        }

        public static string Obtenerdocumento(string dsc_documento)
        {

            var b = lstdni.FirstOrDefault(c => c.dsc_documento == dsc_documento);
            if (b == null) { return Convert.ToString(b = null); }
            else
            {
                return lstdni.Where(c => c.dsc_documento == dsc_documento).ToList().First().dsc_documento;
            }


        }


        public static List<eTrabajador> obtenerProvincia(string cod_departamento)
        {

            return lstProvincia.Where(c => c.cod_departamento == cod_departamento).ToList();
        }

        public static List<eTrabajador> ObtenerDistrito(string cod_departamento, string cod_provincia)
        {

            return lstDistrito.Where(c => c.cod_departamento == cod_departamento + cod_provincia).ToList();
        }

        public static string OBTENERTIPOVIA(string cod_tipo_via)
        {

            return lstTipovia.Where(c => c.cod_tipo_via == cod_tipo_via).ToList().First().dsc_tipo_via;

        }
        public static string OBTENERDEP(string cod_departamento)
        {

            return lstDep.Where(c => c.cod_departamento == cod_departamento).ToList().First().dsc_departamento;
        }
        public static string OBTENERPROV(string cod_provincia)
        {

            return lstProv.Where(c => c.cod_provincia == cod_provincia).ToList().First().dsc_provincia;
        }
        public static string OBTENERDIS(string cod_distrito)
        {

            return lstDist.Where(c => c.cod_distrito == cod_distrito).ToList().First().dsc_distrito;
        }

        public static List<eTrabajador> obtenersede(string cod_empresa)
        {

            return lstSede.Where(c => c.cod_empresa == cod_empresa).ToList();
        }

        public static List<eTrabajador> obtenerceco(string cod_empresa)
        {

            return lstceco.Where(c => c.cod_empresa == cod_empresa).ToList();
        }

        public static List<eTrabajador> obtenerarea(string cod_empresa,string cod_sede_empresa)
        {

            return lstarea.Where(c => c.cod_empresa == cod_empresa+cod_sede_empresa).ToList();
        }

        public static List<eTrabajador> obtenercargo(string cod_empresa, string cod_sede_empresa, string cod_area)
        {

            return lstcargo.Where(c => c.cod_empresa == cod_empresa + cod_sede_empresa+cod_area).ToList();
        }

        private void chkInfoSalud_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (chkInfoSalud.Checked == false) GrbInfoSalud.Visible = false;
            else { GrbInfoSalud.Visible = true; }
        }

        private void chkInfotallas_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (chkInfotallas.Checked == false) GrbInfoCaracFisicas.Visible = false;
            else { GrbInfoCaracFisicas.Visible = true; }
        }

        private void chkInfocaracfisicas_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (chkInfocaracfisicas.Checked == false) GrbInfoCaracFisicas.Visible = false;
            else { GrbInfoCaracFisicas.Visible = true; }
        }

        private void frmCargaMasiva_Load(object sender, EventArgs e)
        {
            
            RepositoryItemTextEdit textEdit = new RepositoryItemTextEdit();
            gvTrabajador.Columns["estado"].ColumnEdit = textEdit;
            Inicializar();
            Thread carga = new Thread(tipodocumento);
            carga.Start();
            




        }

        private void gvTrabajador_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            int x = 0;
            int total = 0;
            int y = Convert.ToInt32(txterrores.Text);
            ColumnView view = (ColumnView)sender;
            for (int i = 0; i < gvTrabajador.RowCount; i++)
            {
                x = gvTrabajador.RowCount + 1;

                eTrabajadorMasivo objFact = gvTrabajador.GetFocusedRow() as eTrabajadorMasivo;
                if(objFact == null) { return; }
                if (objFact.cod_tipo_documento == null) { total = y + 1; } else if (total>1) { total = y-1; } else { total = y; }
                txterrores.Text = Convert.ToString(total);
                
                foreach (GridColumn col in gvTrabajador.Columns)
                {
                    string apellido = ""; string apellidom = ""; string nombres = "";

                    if (col.FieldName == "dsc_documento")
                    {
                        var dsc_documento = "";

                        string dni = Convert.ToString(view.GetFocusedRowCellValue("dsc_documento"));
                        if (dni == null || dni == "") { objFact.estado = "ERROR"; }
                        else
                        {
                            dsc_documento = Obtenerdocumento(dni);
                            if (dsc_documento == null || dsc_documento == "") { objFact.estado = "NO"; } else { objFact.estado = "SI"; }
                        }
                    }

                   
                }

            }

        }

        

        private void tipodocumento()
        {
            lstempresa = unit.Trabajador.ListarUbigedo<eTrabajador>(82, cod_usuario: Program.Sesion.Usuario.cod_usuario);
            lstipodocumento = unit.Trabajador.ListarUbigedo<eTrabajador>(125); 
            lstestadocivil = unit.Trabajador.ListarUbigedo<eTrabajador>(37);
            lstnacionalidad = unit.Trabajador.ListarUbigedo<eTrabajador>(38);
            lssexo = unit.Trabajador.ListarUbigedo<eTrabajador>(39);
            lstpais = unit.Trabajador.ListarUbigedo<eTrabajador>(40);
            lstdepartamento= unit.Trabajador.ListarUbigedo<eTrabajador>(41);
            lstvia = unit.Trabajador.ListarUbigedo<eTrabajador>(44);
            lstzona = unit.Trabajador.ListarUbigedo<eTrabajador>(45);
            lstProvincia = unit.Trabajador.ListarUbigedo<eTrabajador>(94);
            lstDistrito = unit.Trabajador.ListarUbigedo<eTrabajador>(95);
            lstProv = unit.Trabajador.ListarUbigedo<eTrabajador>(99);
            lstDist = unit.Trabajador.ListarUbigedo<eTrabajador>(100);
            lstdni = unit.Trabajador.ListarUbigedo<eTrabajador>(101);
            lstSede = unit.Trabajador.ListarUbigedo<eTrabajador>(102);
            lstarea = unit.Trabajador.ListarUbigedo<eTrabajador>(103);
            lstcargo = unit.Trabajador.ListarUbigedo<eTrabajador>(104);
            lstipotrab = unit.Trabajador.ListarUbigedo<eTrabajador>(69);
            lsttipocontrato = unit.Trabajador.ListarUbigedo<eTrabajador>(9);
            lstcalpsto = unit.Trabajador.ListarUbigedo<eTrabajador>(64);
            lstcattrab = unit.Trabajador.ListarUbigedo<eTrabajador>(68);
            lstsino = unit.Trabajador.ListarUbigedo<eTrabajador>(63);
            lsttipopago = unit.Trabajador.ListarUbigedo<eTrabajador>(65);
            lstperiodicidad = unit.Trabajador.ListarUbigedo<eTrabajador>(61);
            lstocupacional = unit.Trabajador.ListarUbigedo<eTrabajador>(66);
            lstbanco = unit.Trabajador.ListarUbigedo<eTrabajador>(52);
            lsttipocuenta = unit.Trabajador.ListarUbigedo<eTrabajador>(53);
            lstmoneda = unit.Trabajador.ListarUbigedo<eTrabajador>(58);
            lstregimenaseguramientosalud = unit.Trabajador.ListarUbigedo<eTrabajador>(72);
            lstregpension = unit.Trabajador.ListarUbigedo<eTrabajador>(5);
            lstsctrsalud = unit.Trabajador.ListarUbigedo<eTrabajador>(47);
            lstsctrsaludpension = unit.Trabajador.ListarUbigedo<eTrabajador>(49);
            lsttipotrabsalud = unit.Trabajador.ListarUbigedo<eTrabajador>(48);
            lstsittrabsalud = unit.Trabajador.ListarUbigedo<eTrabajador>(51);
            lsttipocuentacts = unit.Trabajador.ListarUbigedo<eTrabajador>(71);
            lstsegurosalud = unit.Trabajador.ListarUbigedo<eTrabajador>(72);
            lstsituacioninfolab = unit.Trabajador.ListarUbigedo<eTrabajador>(73);
            lstreglaboral = unit.Trabajador.ListarUbigedo<eTrabajador>(70);
            lscomisionmixta = unit.Trabajador.ListarUbigedo<eTrabajador>(54);
            lssegurovidaley = unit.Trabajador.ListarUbigedo<eTrabajador>(90);
        }
    }
}