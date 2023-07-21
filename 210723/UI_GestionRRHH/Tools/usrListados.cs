using BE_GestionRRHH;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Rectangle = System.Drawing.Rectangle;

namespace UI_GestionRRHH.UserControls
{
    public partial class usrListados : DevExpress.XtraEditors.XtraUserControl
    {
        private readonly UnitOfWork unit;
        List<eTrabajador> listTrabajador = new List<eTrabajador>();
        List<eEmpresa.eSedeEmpresa> ListSedes = new List<eEmpresa.eSedeEmpresa>();
        Image ImgCumple = Properties.Resources.birthday_cakex24;
        Brush ConCriterios = Brushes.Green;
        Brush SinCriterios = Brushes.Red;
        Brush NAplCriterio = Brushes.Orange;
        int markWidth = 16;
        string cod_empresa = "", cod_sede_empresa = "";

        public usrListados()
        {
            InitializeComponent();
            unit = new UnitOfWork();
        }

        private void ucListados_Load(object sender, EventArgs e)
        {
            //unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Cargando Dashboard", "Cargando...");
            List<eFacturaProveedor> list = unit.Proveedores.ListarEmpresasProveedor<eFacturaProveedor>(11, "", Program.Sesion.Usuario.cod_usuario);
            if (list.Count >= 1) cod_empresa = list[0].cod_empresa;
            ListSedes = unit.Clientes.ListarOpcionesMenu<eEmpresa.eSedeEmpresa>(6, cod_empresa);
            cod_sede_empresa = string.Join(",", ListSedes.Select(t => t.cod_sede_empresa));
            CargarListadoTrabajador(cod_empresa);
            CargarListadoCumpleanhos(cod_empresa);
            //SplashScreenManager.CloseForm();
        }

        private void CargarListadoTrabajador(string cod_empresa)
        {
            listTrabajador.Clear();
            listTrabajador = unit.Trabajador.ListarTrabajadoresvista<eTrabajador>(opcion: 7, cod_trabajador: "", cod_empresa: cod_empresa, cod_empresa_multiple: cod_empresa, cod_sede_multiple: cod_sede_empresa, flg_activo: "SI");
            bsListaTrabajador.DataSource = listTrabajador; gvListadoTrabajador.RefreshData();
        }

        private void CargarListadoCumpleanhos(string cod_empresa)
        {
            //List<eTrabajador> listTrabajadorCumpleMes = listTrabajador.FindAll(x => x.fch_nacimiento.Month == DateTime.Today.Month);
            List<eTrabajador> listTrabajadorCumpleMes = unit.Trabajador.ListarTrabajadoresvista<eTrabajador>(opcion: 8, cod_trabajador: "", cod_empresa: cod_empresa, cod_empresa_multiple: cod_empresa, cod_sede_multiple: cod_sede_empresa, flg_activo: "SI");
            bsListaCumpleanhosMes.DataSource = listTrabajadorCumpleMes; gvListadoCumpleanhosMes.RefreshData();

            //List<eTrabajador> listTrabajadorCumpleHoy = listTrabajadorCumpleMes.FindAll(x => x.fch_nacimiento.Day == DateTime.Today.Day && x.fch_nacimiento.Month == DateTime.Today.Month);
            List<eTrabajador> listTrabajadorCumpleHoy = listTrabajadorCumpleMes.FindAll(x => x.fch_nacimiento.Day == DateTime.Today.Day);
            bsListaCumpleanhosHoy.DataSource = listTrabajadorCumpleHoy; gvListadoCumpleanhosHoy.RefreshData();
        }

        private void gvListadoTrabajador_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    eTrabajador obj = gvListadoTrabajador.GetRow(e.RowHandle) as eTrabajador;
                    if (obj == null) return;
                    if (e.Column.FieldName == "fch_ingreso" && obj.fch_ingreso.ToString().Contains("0001")) e.DisplayText = "";
                    if (e.Column.FieldName == "fch_vencimiento" && obj.fch_vencimiento.ToString().Contains("0001")) e.DisplayText = "";
                    if (e.Column.FieldName == "fch_entrega_uniforme" && obj.fch_entrega_uniforme.ToString().Contains("0001")) e.DisplayText = "";
                    if (e.Column.FieldName == "dsc_diasvencimiento") e.DisplayText = "";
                    if (obj.flg_activo == "NO") e.Appearance.ForeColor = Color.Red;
                    if (e.Column.FieldName == "fch_nacimiento" && (obj.fch_nacimiento.Day == DateTime.Today.Day && obj.fch_nacimiento.Month == DateTime.Today.Month))
                    {
                        e.Graphics.DrawImage(ImgCumple, new Rectangle(e.Bounds.X + 14, e.Bounds.Y + (e.Bounds.Height / 2) - 8, 16, 16));
                    }

                    if (e.Column.FieldName == "dsc_diasvencimiento")
                    {
                        Brush b; e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        int cellValue = Convert.ToInt32(e.CellValue);
                        b = cellValue > 30 ? ConCriterios : cellValue > 5 && cellValue <= 30 ? NAplCriterio : SinCriterios;
                        e.Graphics.FillEllipse(b, new Rectangle(e.Bounds.X + 12, e.Bounds.Y + 1, markWidth, markWidth));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gvListadoCumpleanhos_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    eTrabajador obj = gvListadoCumpleanhosMes.GetRow(e.RowHandle) as eTrabajador;
                    if (obj == null) return;
                    if (e.Column.FieldName == "dsc_cumpleanhos")
                    {
                        e.Graphics.DrawImage(ImgCumple, new Rectangle(e.Bounds.X + 14, e.Bounds.Y + (e.Bounds.Height / 2) - 8, 16, 16));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gvListadoCumpleanhosHoy_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    eTrabajador obj = gvListadoCumpleanhosHoy.GetRow(e.RowHandle) as eTrabajador;
                    if (obj == null) return;
                    if (e.Column.FieldName == "dsc_cumpleanhos")
                    {
                        e.Graphics.DrawImage(ImgCumple, new Rectangle(e.Bounds.X + 14, e.Bounds.Y + (e.Bounds.Height / 2) - 8, 16, 16));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gvListadoTrabajador_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.CellValue == null) return;
            eTrabajador obj = gvListadoTrabajador.GetFocusedRow() as eTrabajador;
            if (obj == null) return;

            if (e.RowHandle >= 0)
            {
                if (e.RowHandle != GridControl.NewItemRowHandle && e.Column.FieldName == "flg_sexo" && e.CellValue.ToString() == "F")
                {
                    e.RepositoryItem = repositoryItemTextEdit4;
                }
                else if (e.RowHandle != GridControl.NewItemRowHandle && e.Column.FieldName == "flg_sexo" && e.CellValue.ToString() == "M")
                {
                    e.RepositoryItem = repositoryItemTextEdit3;
                }
            }
        }

        private void gvListadoCumpleanhos_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.CellValue == null) return;
            eTrabajador obj = gvListadoCumpleanhosMes.GetFocusedRow() as eTrabajador;
            if (obj == null) return;

            if (e.RowHandle >= 0)
            {
                if (e.RowHandle != GridControl.NewItemRowHandle && e.Column.FieldName == "flg_sexo" && e.CellValue.ToString() == "F")
                {
                    e.RepositoryItem = repositoryItemTextEdit2;
                }
                else if (e.RowHandle != GridControl.NewItemRowHandle && e.Column.FieldName == "flg_sexo" && e.CellValue.ToString() == "M")
                {
                    e.RepositoryItem = repositoryItemTextEdit6;
                }
            }
        }

        private void gvListadoCumpleanhosHoy_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.CellValue == null) return;
            eTrabajador obj = gvListadoCumpleanhosHoy.GetFocusedRow() as eTrabajador;
            if (obj == null) return;

            if (e.RowHandle >= 0)
            {
                if (e.RowHandle != GridControl.NewItemRowHandle && e.Column.FieldName == "flg_sexo" && e.CellValue.ToString() == "F")
                {
                    e.RepositoryItem = repositoryItemTextEdit9;
                }
                else if (e.RowHandle != GridControl.NewItemRowHandle && e.Column.FieldName == "flg_sexo" && e.CellValue.ToString() == "M")
                {
                    e.RepositoryItem = repositoryItemTextEdit10;
                }
            }
        }

        private void gvListadoTrabajador_RowClick(object sender, RowClickEventArgs e)
        {
            //try
            //{
            //    if (e.Clicks == 2 && e.RowHandle >= 0)
            //    {
            //        eTrabajador obj = gvListadoTrabajador.GetFocusedRow() as eTrabajador;
            //        if (obj == null) return;

            //        frmMantTrabajador frm = new frmMantTrabajador();
            //        frm.MiAccion = Trabajador.Vista;
            //        frm.cod_trabajador = obj.cod_trabajador;
            //        frm.cod_empresa = obj.cod_empresa;
            //        frm.ShowDialog();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
        }
        private void gvListadoCumpleanhos_RowClick(object sender, RowClickEventArgs e)
        {
            //try
            //{
            //    if (e.Clicks == 2 && e.RowHandle >= 0)
            //    {
            //        eTrabajador obj = gvListadoCumpleanhosMes.GetFocusedRow() as eTrabajador;
            //        if (obj == null) return;

            //        frmMantTrabajador frm = new frmMantTrabajador();
            //        frm.MiAccion = Trabajador.Vista;
            //        frm.cod_trabajador = obj.cod_trabajador;
            //        frm.cod_empresa = obj.cod_empresa;
            //        frm.ShowDialog();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
        }

        private void gvListadoCumpleanhosHoy_RowClick(object sender, RowClickEventArgs e)
        {
            //try
            //{
            //    if (e.Clicks == 2 && e.RowHandle >= 0)
            //    {
            //        eTrabajador obj = gvListadoCumpleanhosHoy.GetFocusedRow() as eTrabajador;
            //        if (obj == null) return;

            //        frmMantTrabajador frm = new frmMantTrabajador();
            //        frm.MiAccion = Trabajador.Vista;
            //        frm.cod_trabajador = obj.cod_trabajador;
            //        frm.cod_empresa = obj.cod_empresa;
            //        frm.ShowDialog();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
        }

        private void gvListadoTrabajador_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
            GridView view = sender as GridView;
            if (view.Columns["flg_activo"] != null)
            {
                string estado = view.GetRowCellDisplayText(e.RowHandle, view.Columns["flg_activo"]);
                if (estado == "NO") e.Appearance.ForeColor = Color.Red;
            }
        }

        private void gvListadoCumpleanhos_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
            GridView view = sender as GridView;
            if (view.Columns["flg_activo"] != null)
            {
                string estado = view.GetRowCellDisplayText(e.RowHandle, view.Columns["flg_activo"]);
                if (estado == "NO") e.Appearance.ForeColor = Color.Red;
            }
        }

        private void gvListadoCumpleanhosHoy_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
            GridView view = sender as GridView;
            if (view.Columns["flg_activo"] != null)
            {
                string estado = view.GetRowCellDisplayText(e.RowHandle, view.Columns["flg_activo"]);
                if (estado == "NO") e.Appearance.ForeColor = Color.Red;
            }
        }

        private void gcListadoTrabajador_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5) CargarListadoTrabajador(cod_empresa);
        }

        private void gcListadoCumpleanhos_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5) CargarListadoCumpleanhos(cod_empresa);
        }

        private void gcListadoCumpleanhosHoy_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5) CargarListadoCumpleanhos(cod_empresa);
        }

        private void gvListadoTrabajador_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvListadoCumpleanhosMes_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvListadoCumpleanhosHoy_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }
    }
}
