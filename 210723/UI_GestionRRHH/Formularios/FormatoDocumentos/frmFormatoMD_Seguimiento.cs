using BE_GestionRRHH;
using BE_GestionRRHH.FormatoMD;
using DevExpress.XtraBars;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraPivotGrid.ViewInfo;
using DevExpress.XtraTreeList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_GestionRRHH.Formularios.FormatoDocumentos
{
    public partial class frmFormatoMD_Seguimiento : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        readonly UnitOfWork unit;
        public frmFormatoMD_Seguimiento()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            ConfigurarFormulario();

            InicializarDatos();
        }
        private void CargarFiltroTreeList()
        {
            var filtro = new List<eFiltroBase>();
            Program.Sesion.EmpresaList.ForEach((em) =>
            {
                filtro.Add(new eFiltroBase()
                {
                    cod_todos = "E0001",
                    dsc_todos = "POR EMPRESA",
                    cod_empresa = em.cod_empresa,
                    dsc_empresa = em.dsc_empresa
                });
            });
            filtro.Add(new eFiltroBase()
            {
                cod_todos = "E0000",
                dsc_todos = "TODOS",
                cod_empresa = "",
                dsc_empresa = "TODOS"
            });

            if (filtro != null && filtro.Count > 0)
            {
                var lst = filtro;
                //.OrderBy(e => e.dsc_sede_empresa).ThenBy(s => s.dsc_sede_empresa).ThenBy(a => a.dsc_area).ToList();
                var tree = new Tools.TreeListHelper(treeFiltroVacaciones);
                tree.TreeViewParaDosNodos<eFiltroBase>(lst,
                      ColumnaCod_Padre: "cod_todos",
                      ColumnaDsc_Padre: "dsc_todos",
                      ColumnaCod_Hijo: "cod_empresa",
                      ColumnaDsc_Hijo: "dsc_empresa"
                    );
                //tree.CheckSubNodos();
                refreshTreeView();

                //_treeList.OptionsView.RootCheckBoxStyle = NodeCheckBoxStyle.Check;
            }
        }
        private void refreshTreeView()
        {
            treeFiltroVacaciones.OptionsView.RootCheckBoxStyle = DevExpress.XtraTreeList.NodeCheckBoxStyle.Radio;
            for (int i = 0; i < treeFiltroVacaciones.Nodes.Count; i++)
            {
                treeFiltroVacaciones.Nodes[i].ChildrenCheckBoxStyle = NodeCheckBoxStyle.Check;
                //for (int j = 0; j < treeFiltroVacaciones.Nodes[i].Nodes.Count(); j++)
                //{
                //    treeFiltroVacaciones.Nodes[i].Nodes[j].ChildrenCheckBoxStyle = NodeCheckBoxStyle.Check;
                //}
            }
            treeFiltroVacaciones.UncheckAll();
            treeFiltroVacaciones.Nodes[0].Checked = true;
            treeFiltroVacaciones.Nodes[0].Nodes[0].Checked = true;
            treeFiltroVacaciones.Nodes[0].Nodes[0].Nodes.ToList().ForEach((ch) => ch.Checked = true);
            //treeFiltroVacaciones.Nodes[0].Nodes[0].Checked = true;
            treeFiltroVacaciones.CollapseAll();
            treeFiltroVacaciones.Nodes[0].ExpandAll();
            treeFiltroVacaciones.ExpandAll();
            treeFiltroVacaciones.Refresh();
        }
        private void ConfigurarFormulario()
        {
            unit.Globales.ConfigurarGridView_ClasicStyle(gcbsListadoFormatoTrabajador, gvbsListadoFormatoTrabajador);
            gvbsListadoFormatoTrabajador.OptionsView.EnableAppearanceEvenRow = false;


            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoDetalladoDocumentoMultiple, gvListadoDetalladoDocumentoMultiple);
            gvListadoDetalladoDocumentoMultiple.OptionsView.EnableAppearanceEvenRow = false;
            gvListadoDetalladoDocumentoMultiple.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;

            //unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoDetalleDocumento, gvListadoDetalleDocumento);
            //unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoPendientes, gvListadoPendientes);
            //gvListadoTrabajadores.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            //gvListadoFormatos.OptionsView.EnableAppearanceEvenRow = false;
            //gvListadoTrabajadores.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
        }
        private void InicializarDatos()
        {
            CargarListadoFormatoTrabajadores();
            CargarFiltroTreeList();


        }

        private void CargarListadoFormatoTrabajadores()
        {
            var tool = new Tools.TreeListHelper(treeFiltroVacaciones);
            var codEmpresa = tool.ObtenerCodigoConcatenadoDeNodoIndex(1);

            //var objTrabajadores = unit.Trabajador.ListarTrabajadoresvista<eTrabajador>(1, "", "", codEmpresa, "");//1, "", codEmpresa,"" );
            var objTrabajadores = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eFormatoMD_SeguimientoGroupVista>(
                new pQFormatoMD() { Opcion = 14, Cod_empresaSplit = codEmpresa });
            bsListadoFormatoTrabajador.DataSource = null;
            if (objTrabajadores != null && objTrabajadores.Count > 0)
            {
                bsListadoFormatoTrabajador.DataSource = objTrabajadores.ToList();
                gvbsListadoFormatoTrabajador.RefreshData();
                gvbsListadoFormatoTrabajador.ExpandAllGroups();

            }

            //btnBuscar.PerformClick();
        }

        List<eFormatoMD_SeguimientoResumenVista> _seguimientos; 
        private void CargarDetalleFormatos(string cod_empresa, string cod_formato, string cod_estado)
        {
            _seguimientos = new List<eFormatoMD_SeguimientoResumenVista>();
            _seguimientos = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eFormatoMD_SeguimientoResumenVista>(
                new pQFormatoMD() { Opcion = 15, Cod_empresaSplit = cod_empresa, Cod_formatoMD_vinculoSplit = cod_formato, Cod_estado = cod_estado });

            bsListadoDetalleDocumento.DataSource = null;
            if (_seguimientos != null && _seguimientos.Count > 0)
            {
                bsListadoDetalleDocumento.DataSource = _seguimientos.ToList();
                gvListadoDetalladoDocumentoMultiple.RefreshData();
                gvListadoDetalladoDocumentoMultiple.ExpandAllGroups();
            }
        }

        private void AdministrarFirmas(string value)
        {
            eSqlMessage result = null;
            int tabIndex = tabDocumentacion.SelectedPageIndex;
            switch (tabIndex)
            {
                case 0: //Registro de firmas: individual
                    {
                        var cod_detalle = lblSeleccion.Tag ?? "";
                        result = unit.FormatoMDocumento.InsertarActualizar_FormatoMDSeguimientoDetalle<eSqlMessage>(
                            2, new eFormatoMD_SeguimientoDetalle() { cod_estado = value, cod_usuario_cambio = Program.Sesion.Usuario.cod_usuario, cod_detalle_seguimiento = cod_detalle.ToString() });
                        break;
                    }
                case 1: //Registro de firmas: múltiple
                    {
                        foreach (var item in gvListadoDetalladoDocumentoMultiple.GetSelectedRows())
                        {
                            var obj = gvListadoDetalladoDocumentoMultiple.GetRow(item) as eFormatoMD_SeguimientoResumenVista;
                            result = unit.FormatoMDocumento.InsertarActualizar_FormatoMDSeguimientoDetalle<eSqlMessage>(
                             2, new eFormatoMD_SeguimientoDetalle() { cod_estado = value, cod_usuario_cambio = Program.Sesion.Usuario.cod_usuario, cod_detalle_seguimiento = obj.cod_detalle_seguimiento });
                        }
                        break;
                    }
            }
            if (result.IsSuccess)
            {
                lblSeleccion.Text = "Item seleccionado: {0}";
                lblSeleccion.Tag = "";

                //bsListadoDetalleDocumento.DataSource = null;
                //bsListadoFormatoTrabajador.DataSource = null;

                //CargarListadoFormatoTrabajadores();
                //btnBuscar_Click(btnBuscar, new EventArgs());
                gvListadoTrabajadores_SelectionChanged(gvbsListadoFormatoTrabajador, new DevExpress.Data.SelectionChangedEventArgs());
            }
        }
        private void AbrirFormularioRegistroSeguimiento(string titulo)
        {
            //var frm = new frmFormatoMD_ModalSeguimiento();
            //frm.Text = titulo;
            //var result = frm.ShowDialog();
            //if (result == DialogResult.OK)
            //{

            //}
            //frm.Dispose();
        }

        private void gvListadoTrabajadores_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvListadoTrabajadores_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void lkpEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            CargarListadoFormatoTrabajadores();
        }

        private void gvListadoTrabajadores_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            var obj = gvbsListadoFormatoTrabajador.GetFocusedRow() as eFormatoMD_SeguimientoGroupVista;
            if (obj != null)
            {
                //lblTrabajador_d1.Text = obj.dsc_nombre_trabajador.ToString();
                //lblTrabajador_d2.Text = obj.dsc_nombre_trabajador.ToString();
                // sexo:imagen /male/female
                CargarDetalleFormatos(obj.cod_empresa, obj.cod_formatoMD_vinculo, obj.cod_estado);

                // CargarDetalleFormatos("", "");
            }
        }

        private void btnNuevo_ItemClick(object sender, ItemClickEventArgs e)
        {
            AbrirFormularioRegistroSeguimiento("Nuevo Seguimiento");
        }
        private void tabDocumentacion_SelectedPageChanged(object sender, DevExpress.XtraBars.Navigation.SelectedPageChangedEventArgs e)
        {
            var index = tabDocumentacion.SelectedPageIndex;
            lblTitulo.Text = index == 0 ? "VISTA GENERAL SEGUIMIENTO DE DOCUMENTOS" :
                index == 1 ? "VISTA SELECCIÓN MULTIPLE DE DOCUMENTOS" : "";
            //btnAprobar.Enabled = index == 1;
            //btnRechazar.Enabled = index == 1;

        }

        private void navBarControl1_MouseClick(object sender, MouseEventArgs e)
        {
            refreshTreeView();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarListadoFormatoTrabajadores();
        }

        private void gvbsListadoFormatoTrabajador_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //if (e.RowHandle >= 0)
            //{
            //    if (e.Column.FieldName == "cod_estado")
            //    {
            //        e.Appearance.ForeColor = e.Appearance.BackColor;
            //        var obj = gvbsListadoFormatoTrabajador.GetRow(e.RowHandle) as eFormatoMD_SeguimientoGroupVista;
            //        e.DefaultDraw();
            //        if (obj.cod_estado.Equals("PEN"))
            //        { e.Cache.DrawImage(Properties.Resources.unchecked_radio_button_18px, e.Bounds.X + (e.Column.Width / 2), e.Bounds.Y); }
            //        else { e.Cache.DrawImage(Properties.Resources.Ok_icon20, e.Bounds.X + (e.Column.Width / 2), e.Bounds.Y); }
            //    }
            //}
        }

        private void gvListadoDetalleDocumento_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvListadoDetalleDocumento_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void pivotResumenDocumentos_CustomDrawCell(object sender, DevExpress.XtraPivotGrid.PivotCustomDrawCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.DataField.FieldName == "cod_estado")
                {
                    e.Appearance.ForeColor = e.Appearance.BackColor;
                    e.DefaultDraw();

                    var value = e.GetFieldValue(e.DataField);
                    if (value != null)
                    {
                        if (value.ToString().ToLower().Trim().Equals("pen"))
                        {
                            e.GraphicsCache.DrawImage(Properties.Resources.unchecked_radio_button_18px, e.Bounds.X + (15), e.Bounds.Y);
                        }
                        else
                            e.GraphicsCache.DrawImage(Properties.Resources.Checkmark_18px, e.Bounds.X + (15), e.Bounds.Y);
                    }
                }
                if (e.DataField.FieldName == "cod_detalle_seguimiento")
                {
                    e.Appearance.ForeColor = e.Appearance.BackColor;
                    e.DefaultDraw();
                    var value = e.GetFieldValue(e.DataField);
                    if (value != null)
                    {
                        e.GraphicsCache.DrawImage(Properties.Resources.hand_cursor_20px, e.Bounds.X, e.Bounds.Y);
                    }
                }
            }
        }

        private void pivotResumenDocumentos_CellClick(object sender, DevExpress.XtraPivotGrid.PivotCellEventArgs e)
        {
            if (e.DataField.FieldName == "cod_detalle_seguimiento")
            {
                var colIndex = e.ColumnIndex;
                var rowIndex = e.RowIndex;
                var celValue = pivotResumenDocumentos.GetCellValue(colIndex, rowIndex);

                var da = _seguimientos.FirstOrDefault((c) => c.cod_detalle_seguimiento.Equals(celValue));
                lblSeleccion.Text = $"Item seleccionado: {da.dsc_cargo}, {da.dsc_nombre_trabajador}";
                lblSeleccion.Tag = celValue;

                //var oss = 
                //var gas = e.DataField.GetAvailableValues();

                //foreach (var item in gas)
                //{
                //    MessageBox.Show(item.ToString());
                //}
            }
        }
        PivotFieldsAreaViewInfo FindChildViewInfo(PivotGridViewInfo pivot, bool isColumn)
        {
            for (int i = 0; i < pivot.ChildCount; i++)
            {
                PivotFieldsAreaViewInfo childViewInfo = pivot.GetChild(i) as PivotFieldsAreaViewInfo;
                if (childViewInfo != null && childViewInfo.Area == ((isColumn) ? PivotArea.ColumnArea : PivotArea.RowArea))
                    return (PivotFieldsAreaViewInfo)childViewInfo;
            }
            return null;
        }
        private List<CustomFieldCellInfo> GetBounds(PivotFieldsAreaViewInfo fieldViewInfo)
        {
            List<CustomFieldCellInfo> result = new List<CustomFieldCellInfo>(fieldViewInfo.ChildCount);
            for (int i = 0; i < fieldViewInfo.ChildCount; i++)
            {
                PivotFieldsAreaCellViewInfo cellViewInfo = (PivotFieldsAreaCellViewInfo)fieldViewInfo.GetChild(i);
                result.Add(new CustomFieldCellInfo { Value = cellViewInfo.Value, Bounds = cellViewInfo.Bounds });
            }

            return result;
        }
        private PivotGridViewInfo GetViewInfo(PivotGridControl pivotGridControl)
        {
            return (PivotGridViewInfo)typeof(PivotGridControl)
                .GetProperty("ViewInfo", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(pivotGridControl);
        }
        class CustomFieldCellInfo
        {
            public object Value { get; set; }
            public Rectangle Bounds { get; set; }
        }



        private void frmFormatoMD_Seguimiento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnBuscar_Click(btnBuscar, new EventArgs());
            }
        }

        private void frmFormatoMD_Seguimiento_Load(object sender, EventArgs e)
        {
            btnBuscar_Click(btnBuscar, new EventArgs());
            gvListadoTrabajadores_SelectionChanged(gvbsListadoFormatoTrabajador, new DevExpress.Data.SelectionChangedEventArgs());
        }

        private void pivotResumenDocumentos_CustomCellValue(object sender, PivotCellValueEventArgs e)
        {
            //if (e.DataField == pivotGridField10 && e.Value is decimal)
            //{

            //    e.Value = (decimal)e.Value - (decimal)e.GetCellValue(fieldMar);
            //}
        }

        private void btnCheckSign_Click(object sender, EventArgs e)
        {
            AdministrarFirmas("COM");
        }

        private void btnUncheckSign_Click(object sender, EventArgs e)
        {
            AdministrarFirmas("PEN");
        }

        private void btnRegistrarFirma_ItemClick(object sender, ItemClickEventArgs e)
        {
            AdministrarFirmas("COM");
        }

        private void btnRetirarFirma_ItemClick(object sender, ItemClickEventArgs e)
        {
            AdministrarFirmas("PEN");
        }

        private void gvListadoDetalladoDocumentoMultiple_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvListadoDetalladoDocumentoMultiple_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvListadoDetalladoDocumentoMultiple_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Column.FieldName == "cod_estado")
                {
                    e.Appearance.ForeColor = e.Appearance.BackColor;
                    var obj = gvListadoDetalladoDocumentoMultiple.GetRow(e.RowHandle) as eFormatoMD_SeguimientoResumenVista;
                    e.DefaultDraw();
                    if (obj.cod_estado.Equals("PEN"))
                    { e.Cache.DrawImage(Properties.Resources.unchecked_radio_button_18px, e.Bounds.X + (e.Column.Width / 2), e.Bounds.Y); }
                    else { e.Cache.DrawImage(Properties.Resources.Ok_icon20, e.Bounds.X + (e.Column.Width / 2), e.Bounds.Y); }
                }
            }
        }
    }
}