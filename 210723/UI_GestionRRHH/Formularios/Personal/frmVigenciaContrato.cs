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
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System.ComponentModel.DataAnnotations;

namespace UI_GestionRRHH.Formularios.Personal
{
    internal enum Vigencia
    {
        Nuevo = 0,
        Editar = 1,
        Vista = 2
    }
    public partial class frmVigenciaContrato : HNG_Tools.SimpleModalForm
    {
        private readonly UnitOfWork unit;
        eTrabajador trab = new eTrabajador();
        internal Cese MiAccion = Cese.Nuevo;
        public string empresa, trabajador, estado;
        public List<eTrabajador> listTrabajador;
        List<eTrabajador.eInfoLaboral_Trabajador> ListInfoLaboral = new List<eTrabajador.eInfoLaboral_Trabajador>();
        private static IEnumerable<eTrabajador> stSede, lstcargo, lstsino;
        private static IEnumerable<eTrabajador.eInfoLaboral_Trabajador> lstarea;
        public string ActualizarListado = "NO";

        private void frmVigenciaContrato_Load(object sender, EventArgs e)
        {
            gvTrabajador.RefreshData();
            infolaboral();
            
            rslkpsedeempresa.DataSource = unit.Trabajador.CombosEnGridControl<eTrabajador.eEMO>("sedeempregrid", cod_empresa: empresa);
            rsLkpsino.DataSource = unit.Trabajador.CombosEnGridControl<eTrabajador>("Sino");


            //dtFchEvaluacion.EditValue = Convert.ToDateTime(DateTime.Today);
        }
        private void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;

        }
        private void infolaboral()
        {

            ListInfoLaboral.Clear();

            ListInfoLaboral = unit.Trabajador.ListarTrabajadorMasivo<eTrabajador.eInfoLaboral_Trabajador>(117, cod_trabajador_multiple: trabajador, cod_empresa: empresa);
            bsTrabajador.DataSource = ListInfoLaboral; gvTrabajador.RefreshData();

        }

        public frmVigenciaContrato()
        {
            InitializeComponent();
            unit = new UnitOfWork();

            lstarea = unit.Trabajador.ListarUbigedo<eTrabajador.eInfoLaboral_Trabajador>(103);
            rslkparea.DataSource = lstarea;
            lstcargo = unit.Trabajador.ListarUbigedo<eTrabajador>(104);
            rslkpcargo.DataSource = lstcargo;
            lstsino = unit.Trabajador.ListarUbigedo<eTrabajador>(63);
            rsLkpsino.DataSource = lstsino;
        }

        private void gvTrabajador_ShownEditor(object sender, EventArgs e)
        {
            ColumnView view = (ColumnView)sender;

            if (view.FocusedColumn.FieldName == "cod_area")
            {
                LookUpEdit editor = (LookUpEdit)view.ActiveEditor;
                string cod_empresa = Convert.ToString(view.GetFocusedRowCellValue("cod_empresa"));
                string cod_sede_empresa = Convert.ToString(view.GetFocusedRowCellValue("cod_sede_empresa"));
                editor.Properties.DataSource = obtenerarea(empresa, cod_sede_empresa);
            }
            if (view.FocusedColumn.FieldName == "cod_cargo")
            {
                LookUpEdit editor = (LookUpEdit)view.ActiveEditor;
                string cod_empresa = Convert.ToString(view.GetFocusedRowCellValue("dsc_empresa"));
                string cod_sede_empresa = Convert.ToString(view.GetFocusedRowCellValue("cod_sede_empresa"));
                string cod_area = Convert.ToString(view.GetFocusedRowCellValue("cod_area"));
                editor.Properties.DataSource = obtenercargo(empresa, cod_sede_empresa, cod_area);
            }
            //if (view.FocusedColumn.FieldName== "flg_asignacionfamiliar")
            //{
            //    string flg = Convert.ToString(view.GetFocusedRowCellValue("flg_asignacionfamiliar"));
            //    decimal imp = Convert.ToDecimal(view.GetFocusedRowCellValue("imp_asig_familiar"));

                
                
            //}
        }

        private void gvInfolaboral_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

   
        private void btnguardar_Click(object sender, EventArgs e)
        {
            DateTime FechaRegistro = DateTime.Today;
            string cod_doc = ""; string result = "";
            //for (int nRow = 0; nRow <= gvTrabajador.RowCount - 1; nRow++)
            foreach (eTrabajador.eInfoLaboral_Trabajador obj in ListInfoLaboral)
            {
                eTrabajador.eInfoLaboral_Trabajador eInfoLab = new eTrabajador.eInfoLaboral_Trabajador();
                eInfoLab=AsignarValores_InfoLaboral(obj);
                eInfoLab = unit.Trabajador.InsertarActualizar_InfoLaboralTrabajador<eTrabajador.eInfoLaboral_Trabajador>(eInfoLab);
                gvTrabajador.RefreshData();
            }
            MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void gvTrabajador_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {

            if (e.RowHandle >= 0 && e.Column.FieldName == "cod_sede_empresa")
            {
                eTrabajador.eInfoLaboral_Trabajador obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eInfoLaboral_Trabajador obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eInfoLaboral_Trabajador;
                    obj3.cod_sede_empresa = obj2.cod_sede_empresa;
                }
                gvTrabajador.RefreshData();

            }
            if (e.RowHandle >= 0 &&  e.Column.FieldName == "cod_area")
            {
                eTrabajador.eInfoLaboral_Trabajador obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eInfoLaboral_Trabajador obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eInfoLaboral_Trabajador;
                    obj3.cod_area = obj2.cod_area;
                }
                gvTrabajador.RefreshData();

            }
            if (e.RowHandle >= 0 && e.Column.FieldName == "cod_cargo")
            {
                eTrabajador.eInfoLaboral_Trabajador obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eInfoLaboral_Trabajador obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eInfoLaboral_Trabajador;
                    obj3.cod_cargo = obj2.cod_cargo;
                }
                gvTrabajador.RefreshData();

            }
            if (e.RowHandle >= 0 && e.Column.FieldName == "flg_afectoSCTR")
            {
                eTrabajador.eInfoLaboral_Trabajador obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                if (obj2 == null) return;
               
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eInfoLaboral_Trabajador obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eInfoLaboral_Trabajador;
                    obj3.flg_afectoSCTR = obj2.flg_afectoSCTR;
                }
                gvTrabajador.RefreshData();

            }
            if (e.RowHandle >= 0 && e.Column.FieldName == "flg_AfectoVidaLey")
            {
                eTrabajador.eInfoLaboral_Trabajador obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eInfoLaboral_Trabajador obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eInfoLaboral_Trabajador;
                    obj3.flg_AfectoVidaLey = obj2.flg_AfectoVidaLey;
                }
                gvTrabajador.RefreshData();

            }
            if (e.RowHandle >= 0 && e.Column.FieldName == "fch_firma")
            {
                eTrabajador.eInfoLaboral_Trabajador obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eInfoLaboral_Trabajador obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eInfoLaboral_Trabajador;
                    obj3.fch_firma = obj2.fch_firma;
                }
                gvTrabajador.RefreshData();

            }
            if (e.RowHandle >= 0 && e.Column.FieldName == "fch_vencimiento")
            {
                eTrabajador.eInfoLaboral_Trabajador obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eInfoLaboral_Trabajador obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eInfoLaboral_Trabajador;
                    obj3.fch_vencimiento = obj2.fch_vencimiento;
                }
                gvTrabajador.RefreshData();

            }
            if (e.RowHandle >= 0 && e.Column.FieldName == "imp_sueldo_base")
            {
                eTrabajador.eInfoLaboral_Trabajador obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eInfoLaboral_Trabajador obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eInfoLaboral_Trabajador;
                    obj3.imp_sueldo_base = obj2.imp_sueldo_base;
                }
                gvTrabajador.RefreshData();

            }
            if (e.RowHandle >= 0 && e.Column.FieldName == "imp_movilidad")
            {
                eTrabajador.eInfoLaboral_Trabajador obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eInfoLaboral_Trabajador obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eInfoLaboral_Trabajador;
                    obj3.imp_movilidad = obj2.imp_movilidad;
                }
                gvTrabajador.RefreshData();

            }
            if (e.RowHandle >= 0 && e.Column.FieldName == "imp_alimentacion")
            {
                eTrabajador.eInfoLaboral_Trabajador obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eInfoLaboral_Trabajador obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eInfoLaboral_Trabajador;
                    obj3.imp_alimentacion = obj2.imp_alimentacion;
                }
                gvTrabajador.RefreshData();

            }
            if (e.RowHandle >= 0 && e.Column.FieldName == "imp_escolaridad")
            {
                eTrabajador.eInfoLaboral_Trabajador obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eInfoLaboral_Trabajador obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eInfoLaboral_Trabajador;
                    obj3.imp_escolaridad = obj2.imp_escolaridad;
                }
                gvTrabajador.RefreshData();

            }
            if (e.RowHandle >= 0 && e.Column.FieldName == "flg_asignacionfamiliar")
            {
                eTrabajador.eInfoLaboral_Trabajador obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eInfoLaboral_Trabajador obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eInfoLaboral_Trabajador;
                    obj3.flg_asignacionfamiliar = obj2.flg_asignacionfamiliar;
                    
                }
                gvTrabajador.RefreshData();

            }
            if (e.RowHandle >= 0 && e.Column.FieldName == "imp_asig_familiar")
            {
                eTrabajador.eInfoLaboral_Trabajador obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eInfoLaboral_Trabajador obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eInfoLaboral_Trabajador;
                    obj3.imp_asig_familiar = obj2.imp_asig_familiar;
                }
                gvTrabajador.RefreshData();

            }
        }

        private void gvTrabajador_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvTrabajador_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvTrabajador_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            eTrabajador etr = new eTrabajador();
            etr = unit.Trabajador.Obtener_cod_trabajador<eTrabajador>(118);
            try
            {
                if (e.RowHandle >= 0)
                {
                    eTrabajador.eInfoLaboral_Trabajador obj = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eInfoLaboral_Trabajador;
                    if (obj == null) return;
                    if (obj.flg_afectoSCTR == null) e.Appearance.ForeColor = Color.Red;
                    if (obj.flg_AfectoVidaLey == null) e.Appearance.ForeColor = Color.Red;
                    if (obj.correo_laboral == null) e.Appearance.ForeColor = Color.Red;
                    if (obj.imp_sueldo_base == 0) e.Appearance.ForeColor = Color.Red;
                    if (e.Column.FieldName == "flg_asignacionfamiliar")
                    {
                        if (obj.flg_asignacionfamiliar == "SI") { obj.imp_asig_familiar = etr.total_rem; }
                        else if(obj.flg_asignacionfamiliar == "NO") { obj.imp_asig_familiar = Convert.ToDecimal(0.00); }
                    }
                    ActualizarListado = "SI";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }
      

        public static List<eTrabajador.eInfoLaboral_Trabajador> obtenerarea(string cod_empresa, string cod_sede_empresa)
        {

            return lstarea.Where(c => c.cod_empresa == cod_empresa + cod_sede_empresa).ToList();
        }

        public static List<eTrabajador> obtenercargo(string cod_empresa, string cod_sede_empresa, string cod_area)
        {

            return lstcargo.Where(c => c.cod_empresa == cod_empresa + cod_sede_empresa + cod_area).ToList();
        }


        private void gcTrabajador_Click(object sender, EventArgs e)
        {

        }




        private eTrabajador.eInfoLaboral_Trabajador AsignarValores_InfoLaboral(eTrabajador.eInfoLaboral_Trabajador obj2)
        {
            eTrabajador.eInfoLaboral_Trabajador obj = new eTrabajador.eInfoLaboral_Trabajador();

            obj.cod_trabajador = obj2.cod_trabajador;
            obj.cod_empresa = empresa;
            obj.cod_infolab = 0;
            obj.fch_ingreso = obj2.fch_ingreso;
            obj.cod_area = obj2.cod_area;
            obj.cod_cargo = obj2.cod_cargo;
            obj.dsc_pref_ceco = obj2.dsc_pref_ceco;
            obj.cod_tipo_contrato = obj2.cod_tipo_contrato;
            obj.fch_firma = obj2.fch_firma;
            obj.fch_vencimiento = obj2.fch_vencimiento;
            obj.cod_modalidad = obj2.cod_modalidad;
            obj.imp_sueldo_base = obj2.imp_sueldo_base;
            obj.imp_asig_familiar = obj2.imp_asig_familiar;
            obj.imp_movilidad = obj2.imp_movilidad;
            obj.imp_alimentacion = obj2.imp_alimentacion;
            obj.imp_escolaridad = obj2.imp_escolaridad;
            obj.imp_bono = obj2.imp_bono;
            obj.cod_sede_empresa = obj2.cod_sede_empresa;
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            obj.flg_asignacionfamiliar = obj2.flg_asignacionfamiliar;
            obj.flg_Regimen_pension = obj2.flg_Regimen_pension;
            obj.flg_regimen_atipico = obj2.flg_regimen_atipico;
            obj.flg_jornada_maxima = obj2.flg_jornada_maxima;
            obj.flg_horario_nocturno = obj2.flg_horario_nocturno;
            obj.codsunat_scrtcentroriesgo = obj2.codsunat_scrtcentroriesgo;
            obj.flg_afectoSCTR = obj2.flg_afectoSCTR;
            obj.flg_AfectoVidaLey = obj2.flg_AfectoVidaLey;
            obj.codsunat_seguroley = obj2.codsunat_seguroley;
            obj.flg_horas_extras = obj2.flg_horas_extras;
            obj.flg_sindicato = obj2.flg_sindicato;
            obj.cod_tipo_trabajador = obj.cod_tipo_trabajador;
            obj.cod_categoria_trabajador = obj2.cod_categoria_trabajador;
            obj.dsc_calificacion_puesto = obj2.dsc_calificacion_puesto;
            obj.cod_situacion_trabajador_2 = obj2.cod_situacion_trabajador_2;
            obj.cod_exoneracion_5ta = obj2.cod_exoneracion_5ta;
            obj.cod_ocupacional = obj2.cod_ocupacional;
            obj.cod_conveniotributacion = obj2.cod_conveniotributacion;
            obj.cod_situacion_especial = obj2.cod_situacion_especial;
            obj.Fechabaja = obj2.Fechabaja;
            obj.motivo_baja = obj2.motivo_baja;
            obj.observaciones = obj2.observaciones;
            obj.cod_regimen_laboral = obj2.cod_regimen_laboral;
            obj.dsc_porcentajecomision = obj2.dsc_porcentajecomision;
            obj.dsc_porcentajequincena = obj2.dsc_porcentajequincena;
            obj.correo_laboral = obj2.correo_laboral;

            obj.flg_activo = "SI";


            return obj;
        }
    }
}