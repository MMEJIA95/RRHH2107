using BE_GestionRRHH;
using DevExpress.XtraBars;
using DevExpress.XtraTreeList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_GestionRRHH.Formularios.Personal.Vacaciones_Permisos
{
    public partial class frmAusencia_Listado : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly UnitOfWork unit;
        public frmAusencia_Listado()
        {
            InitializeComponent();
            unit = new UnitOfWork();

            InicializarDatos();
        }
        private void InicializarDatos()
        {
            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoTrabajador, gvListadoTrabajador);
            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoDetalle, gvListadoDetalle);
            CargarFiltroTreeList();

            grpEmpresa.BringToFront();
            //groupControl1.Dock = DockStyle.Fill;
            //navEmpresaSedeArea.ControlContainer.Controls.Add(groupControl1);


            CargarListadoGeneral();
        }
        private void CargarListadoGeneral()
        {
            var tool = new Tools.TreeListHelper(treeFiltroVacaciones);
            var codEmpresa = tool.ObtenerCodigoConcatenadoDeNodoIndex(0);
            var codSede = tool.ObtenerCodigoConcatenadoDeNodoIndex(1);
            var codArea = tool.ObtenerCodigoConcatenadoDeNodoIndex(2);

            var objList = unit.Ausencia.ConsultaVarias_Ausencias<eAusenciaListado>(
                new pAusencia()
                {
                    Opcion = 1,
                    cod_empresaMultiple = codEmpresa,
                    cod_areaMultiple = codArea,
                    cod_sedeMultiple = codSede,
                });
            bsListadoAusencia.DataSource = null;
            if (objList.Count > 0 && objList != null)
            {
                bsListadoAusencia.DataSource = objList.ToList();
                gvListadoTrabajador.RefreshData();
                gvListadoTrabajador.ExpandAllGroups();
            };
        }
        private void CargarListadoAusenciaDetalle()
        {
            var obj = gvListadoTrabajador.GetFocusedRow() as eVacaciones_Listado;
            if (obj == null) return;

            lblNombres.Text = obj.dsc_trabajador.Split(',')[1];
            lblApellidos.Text = obj.dsc_trabajador.Split(',')[0];
            lblCargo.Text = "";

            btnEmpresa.Tag = obj.dsc_empresa;
            btnSede.Tag = obj.dsc_sede_empresa;
            btnArea.Tag = obj.dsc_area;
            txtTipoContrato.Text = "";
            txtFechaIngreso.Text = obj.fch_ingreso.ToString("yyyy-MM-dd");
            //txtVacacionesAcumuladas.Text = obj.num_total_factor30.ToString();
            txtVacacionesProgramadas.Text = "";
            txtPersonaAReportar.Text = "";

            //var objList = unit.Vacaciones.ConsultaVarias_Vacaciones<eVacacionesDetalleListado>(
            //    new pVacaciones() { Opcion = 3, cod_vacacionGeneral = obj.cod_vacacionGeneral });
            //bsListadoVacacionDetalle.DataSource = null;
            //if (objList.Count > 0 && objList != null)
            //{
            //    bsListadoVacacionDetalle.DataSource = objList.ToList();
            //    gvListadoDetalle.RefreshData();
            //    //gvListadoDetalle.ExpandAllGroups();
            //};
        }
        private void CargarFiltroTreeList()
        {
            var empresasMultiples = new Tools.TreeListHelper().ObtenerValoresConcatenadoDeLista<eProveedor_Empresas>(Program.Sesion.EmpresaList, "cod_empresa");
            var filtro = unit.Vacaciones.ConsultaVarias_Vacaciones<eFltEmpresaSedeArea>(
                new pVacaciones() { Opcion = 1, Cod_empresa_multiple = empresasMultiples });
            if (filtro != null && filtro.Count > 0)
            {
                var lst = filtro;
                //.OrderBy(e => e.dsc_sede_empresa).ThenBy(s => s.dsc_sede_empresa).ThenBy(a => a.dsc_area).ToList();
                var tree = new Tools.TreeListHelper(treeFiltroVacaciones);
                tree.TreeViewParaTresNodos<eFltEmpresaSedeArea>(lst,
                      ColumnaCod_Abuelo: "cod_empresa",
                      ColumnaDsc_Abuelo: "dsc_empresa",
                      ColumnaCod_Padre: "cod_sede_empresa",
                      ColumnaDsc_Padre: "dsc_sede_empresa",
                      ColumnaCod_Hijo: "cod_area",
                      ColumnaDsc_Hijo: "dsc_area"
                    );
                //tree.CheckSubNodos();
                refreshTreeView();



                //_treeList.OptionsView.RootCheckBoxStyle = NodeCheckBoxStyle.Check;
            }
        }

        private void refreshTreeView()
        {
            //navBarControl1.OptionsNavPane.ExpandedWidth = 260;
            //navBarControl1.OptionsNavPane.CollapsedWidth = 50;

            //navBarControl1.OptionsNavPane.NavPaneState = NavPaneState.Collapsed; 
            //navBarControl1.OptionsNavPane.NavPaneState = NavPaneState.Expanded;
            //navBarControl1.OptionsNavPane.NavPaneState = NavPaneState.Collapsed;
            //navBarControl1.OptionsNavPane.NavPaneState = NavPaneState.Expanded;


            treeFiltroVacaciones.OptionsView.RootCheckBoxStyle = DevExpress.XtraTreeList.NodeCheckBoxStyle.Radio;
            for (int i = 0; i < treeFiltroVacaciones.Nodes.Count; i++)
            {
                treeFiltroVacaciones.Nodes[i].ChildrenCheckBoxStyle = NodeCheckBoxStyle.Check;
                for (int j = 0; j < treeFiltroVacaciones.Nodes[i].Nodes.Count(); j++)
                {
                    treeFiltroVacaciones.Nodes[i].Nodes[j].ChildrenCheckBoxStyle = NodeCheckBoxStyle.Check;
                }
            }
            treeFiltroVacaciones.UncheckAll();
            treeFiltroVacaciones.Nodes[0].Checked = true;
            treeFiltroVacaciones.Nodes[0].Nodes[0].Checked = true;
            treeFiltroVacaciones.Nodes[0].Nodes[0].Nodes.ToList().ForEach((ch) => ch.Checked = true);
            //treeFiltroVacaciones.Nodes[0].Nodes[0].Checked = true;
            treeFiltroVacaciones.CollapseAll();
            treeFiltroVacaciones.Nodes[0].ExpandAll();
            treeFiltroVacaciones.Refresh();
        }
        private void ControlarPanel()
        {
            var registro = btnRegistroAusencia.Down;
            var dashboard = btnDashboardAusencia.Down;
            rpgRegistro_EBasica.Visible = registro;
            rpgRegistro_Acciones.Visible = registro;
            rpgRegistro_PVista.Visible = registro;
            rpgRegistro_Reporte.Visible = registro;

            rpgDashboardAcciones.Visible = dashboard;
            layoutDashboard.Visibility = dashboard ? DevExpress.XtraLayout.Utils.LayoutVisibility.Always : DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutRegistro.Visibility = registro ? DevExpress.XtraLayout.Utils.LayoutVisibility.Always : DevExpress.XtraLayout.Utils.LayoutVisibility.Never; ;

        }
        private void btnDashboardAusencia_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnRegistroAusencia.Down = !btnDashboardAusencia.Down;
            ControlarPanel();
        }

        private void btnRegistroAusencia_ItemClick(object sender, ItemClickEventArgs e)
        {
            btnDashboardAusencia.Down = !btnRegistroAusencia.Down;
            ControlarPanel();
        }

        private void gvListadoTrabajador_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                //if (e.Clicks == 2 && e.RowHandle >= 0)
                //{
                //    eTrabajador obj = gvListadoTrabajador.GetFocusedRow() as eTrabajador;
                //    if (obj == null) return;
                //    if (Application.OpenForms["frmMantTrabajador"] != null)
                //    {
                //        Application.OpenForms["frmMantTrabajador"].Activate();
                //    }
                //    else
                //    {
                //        frmMantTrabajador frm = new frmMantTrabajador(this);
                //        frm.MiAccion = Trabajador.Editar;
                //        frm.cod_trabajador = obj.cod_trabajador;
                //        frm.cod_empresa = obj.cod_empresa;
                //        frm.ShowDialog();
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            CargarListadoAusenciaDetalle();
        }

        private void gvListadoTrabajador_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvListadoTrabajador_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvListadoTrabajador_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            CargarListadoAusenciaDetalle();
        }

        private void gvListadoDetalle_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvListadoDetalle_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void btnVista_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}