using BE_GestionRRHH;
using DevExpress.XtraBars;
using DevExpress.XtraTreeList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI_GestionRRHH.Formularios.Personal.Vacaciones_Permisos
{
    public partial class frmVacaciones_Listado : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        internal readonly UnitOfWork unit;
        private PROP Prop;
        private class PROP
        {
            public string Cod_empresa { get; set; }
            public string Cod_area { get; set; }
            public string Cod_vacacion { get; set; }
            public string Cod_detalle { get; set; }
            public string Cod_trabajador { get; set; }
            public string Periodo { get; set; }
        }
        public frmVacaciones_Listado()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            Prop = new PROP();
            configurar_formulario();

            InicializarDatos();
        }


        private void configurar_formulario()
        {
            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoVacaciones, gvListadoVacaciones, editable: false);

            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoVacacionesSolicitadas, gvListadoVacacionesSolicitadas, showAutoFilterRow: false);
            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoVacacionesAcumuladas, gvListadoVacacionesAcumuladas, showAutoFilterRow: false);
            btnBuscador.Appearance.BackColor = Program.Sesion.Colores.Verde;
        }
        private class Periodo { public string Key { get; set; } public string Value { get; set; } }
        private void InicializarDatos()
        {
            /*-----*Crear Periodos*-----*/
            /*
            int year = 2016;//Cambiar por año existente en la DB | o mostrar un rando 5 periodos abajo.
            List<eItems> periodos = new List<eItems>();
            //List<(string, string)> periodos = new List<(string, string)>();
            for (int i = year; i <= DateTime.Now.Year; i++)
                periodos.Add(new eItems() { Key = $"{i}_{i + 1}", Value = $"P:  {i} - {i + 1}" });

            //{
            //periodos.Add(($"{i}_{i+1}", $"{i} - {i+1}"));
            //  periodos.Add(new eItems() { Key = $"{i}_{i + 1}", Value = $"Periodo: {i} - {i + 1}" });
            //}
            periodos = periodos.OrderByDescending(k => k.Key).ToList();
            lkpPeriodo.Properties.DataSource = periodos;
            lkpPeriodo.Properties.ValueMember = "Key"; lkpPeriodo.Properties.DisplayMember = "Value";
            lkpPeriodo.EditValue = periodos[0].Key;
            */
            Cargar_Tree_Empresas();
            CargarListadoVacaciones();
        }

        private void CargarListadoVacaciones()
        {
            var tool = new Tools.TreeListHelper(treeEmpresas);
            Prop.Cod_empresa = tool.ObtenerCodigoConcatenadoDeNodoIndex(0);

            var objListado = unit.Vacaciones.ConsultaVarias_Vacaciones<eVacaciones_Resumen>(new pVacaciones()
            {
                Opcion = 7,//2,
                Cod_empresa_multiple = Prop.Cod_empresa,
                //Cod_periodo = Prop.Periodo,
                //Cod_sede_multiple = "",
                //Cod_area_multiple = "",
            });
            if (objListado != null && objListado.Count() > 0)
            {
                bsListadoVacaciones.DataSource = objListado.ToList();
                gvListadoVacaciones.RefreshData();
            }
        }
        private void Cargar_Tree_Empresas()
        {
            var ListEmp = Program.Sesion.EmpresaList;
            var emp_sedeList = new List<eFltEmpresaSede>();
            foreach (var obj in ListEmp)
            {
                List<eEmpresa.eSedeEmpresa> ListSedes = unit.Clientes.ListarOpcionesMenu<eEmpresa.eSedeEmpresa>(6, obj.cod_empresa);
                foreach (eEmpresa.eSedeEmpresa objSede in ListSedes)
                {
                    emp_sedeList.Add(new eFltEmpresaSede()
                    {
                        cod_empresa = obj.cod_empresa,
                        dsc_empresa = obj.dsc_empresa,
                        cod_sede_empresa = objSede.cod_sede_empresa,
                        dsc_sede_empresa = objSede.dsc_sede_empresa
                    });
                }
            }

            if (emp_sedeList != null && emp_sedeList.Count > 0)
            {
                var lst = emp_sedeList;
                var tree = new Tools.TreeListHelper(treeEmpresas);
                tree.TreeViewParaDosNodos<eFltEmpresaSede>(emp_sedeList,
                      ColumnaCod_Padre: "cod_empresa",
                      ColumnaDsc_Padre: "dsc_empresa",
                      ColumnaCod_Hijo: "cod_sede_empresa",
                      ColumnaDsc_Hijo: "dsc_sede_empresa"
                    );
                refreshTreeView();
            }
        }

        private void refreshTreeView()
        {
            treeEmpresas.OptionsView.RootCheckBoxStyle = DevExpress.XtraTreeList.NodeCheckBoxStyle.Radio;
            for (int i = 0; i < treeEmpresas.Nodes.Count; i++)
            {
                treeEmpresas.Nodes[i].ChildrenCheckBoxStyle = NodeCheckBoxStyle.Check;
                for (int j = 0; j < treeEmpresas.Nodes[i].Nodes.Count(); j++)
                {
                    treeEmpresas.Nodes[i].Nodes[j].ChildrenCheckBoxStyle = NodeCheckBoxStyle.Check;
                }
            }
            treeEmpresas.UncheckAll();
            treeEmpresas.Nodes[0].Checked = true;
            treeEmpresas.Nodes[0].Nodes[0].Checked = true;
            treeEmpresas.Nodes[0].Nodes[0].Nodes.ToList().ForEach((ch) => ch.Checked = true);
            //treeFiltroVacaciones.Nodes[0].Nodes[0].Checked = true;
            treeEmpresas.CollapseAll();
            treeEmpresas.Nodes[0].ExpandAll();
            treeEmpresas.Refresh();
        }

        private void navBarControl1_Click(object sender, EventArgs e)
        {
            refreshTreeView();
        }
        private void btnNuevo_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(gvListadoVacaciones.GetFocusedRow() is eVacaciones_Resumen obj)) return;

         //   var obj = gvListadoVacaciones.GetFocusedRow() as eVacaciones_Resumen ;

            var frm = new frmVacaciones_Registrar(this);
            frm.Text = "Nueva Solicitud de Vacaciones";
            frm.CargarDatos(cod_empresa: Prop.Cod_empresa, cod_periodo: Prop.Periodo, cod_trabajador: Prop.Cod_trabajador, "control", string.Concat(" ", obj.dsc_apellidos_nombres)); //, cod_vacaciones: Prop.Cod_vacacion, cod_detalle: ""
            if (frm.ShowDialog() == DialogResult.OK)
            {
                btnBuscador_Click(btnBuscador, new EventArgs());
            }
        }

        //private void lkpPeriodo_EditValueChanged(object sender, EventArgs e) { Prop.Periodo = lkpPeriodo.EditValue.ToString(); }

        private void btnBuscador_Click(object sender, EventArgs e)
        {
            CargarListadoVacaciones();
        }

        private void btnNuevaSolicitud_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private void gvListadoVacaciones_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (!(gvListadoVacaciones.GetFocusedRow() is eVacaciones_Resumen obj)) return;

            Prop.Cod_trabajador = obj.cod_trabajador;
            //Prop.Cod_vacacion = obj.cod_vacacionGeneral;
            //Prop.Periodo = obj.cod_periodo;
            Prop.Cod_empresa = obj.cod_empresa;

            //Mostrar  Solicitudes y Acumulados

            MostrarVacacionesAcumuladas();
        }

        private void MostrarVacacionesAcumuladas()
        {
            var objAcumulados = unit.Vacaciones.ConsultaVarias_Vacaciones<eVacaciones_Acumuladas>(
                 new pVacaciones()
                 {
                     Opcion = 4,
                     Cod_empresa_multiple = Prop.Cod_empresa,
                     Cod_trabajador = Prop.Cod_trabajador,
                 });
            bsListadoVacacionesAcumuladas.DataSource = null;
            if (objAcumulados != null && objAcumulados.Count > 0)
            {
                bsListadoVacacionesAcumuladas.DataSource = objAcumulados;
                gvListadoVacacionesAcumuladas.RefreshData();
            }
        }

        private void MostrarVacacionesSolicitadas()
        {
            var objList = unit.Vacaciones.ConsultaVarias_Vacaciones<eVacaciones_Solicitadas>(
               new pVacaciones()
               {
                   Opcion = 6,
                   Cod_periodo = Prop.Periodo,
                   Cod_empresa_multiple = Prop.Cod_empresa,
                   Cod_trabajador = Prop.Cod_trabajador,
               });
            bsListadoVacacionesSolicitadas.DataSource = null;
            if (objList != null && objList.Count > 0)
            {
                bsListadoVacacionesSolicitadas.DataSource = objList.ToList();
                gvListadoVacacionesSolicitadas.RefreshData();
            }
        }

        private void gvListadoVacaciones_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {

                if (e.Column.FieldName == "vac_total_pendientes")
                { e.Appearance.BackColor = Color.FromArgb(221, 235, 247); }
                if (e.Column.FieldName == "vac_total_truncas")
                { e.Appearance.BackColor = Color.FromArgb(221, 235, 247); }

                if (e.Column.FieldName == "vac_total_gozadas")
                { e.Appearance.BackColor = Color.FromArgb(226, 239, 218); }
                if (e.Column.FieldName == "vac_total_programadas")
                { e.Appearance.BackColor = Color.FromArgb(226, 239, 218); }
                if (e.Column.FieldName == "vac_total_vendidas")
                { e.Appearance.BackColor = Color.FromArgb(226, 239, 218); }

                if (e.Column.FieldName == "saldo_total_dias")
                { e.Appearance.BackColor = Color.FromArgb(255, 242, 204); }
                if (e.Column.FieldName == "saldo_total_pendientes")
                { e.Appearance.BackColor = Color.FromArgb(252, 228, 214); }
                e.DefaultDraw();
            }
        }

        private void frmVacaciones_Listado_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5) btnBuscador_Click(btnBuscador, new EventArgs());
        }

        private void btnAprobarSolicitud_ItemClick(object sender, ItemClickEventArgs e)
        {
    
        }

        private void gvListadoVacacionesAcumuladas_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (!(gvListadoVacacionesAcumuladas.GetFocusedRow() is eVacaciones_Acumuladas obj)) return;

            //Prop.Cod_trabajador = obj.cod_trabajador;
            //Prop.Cod_vacacion = obj.cod_vacacionGeneral;
            Prop.Periodo = obj.cod_periodo;
            //Prop.Cod_empresa = obj.cod_empresa;

            MostrarVacacionesSolicitadas();
        }

        private void frmVacaciones_Listado_Load(object sender, EventArgs e)
        {
            gvListadoVacaciones.SelectRow(0);
            gvListadoVacacionesAcumuladas.SelectRow(0);
            
        }
    }
}