using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BL_GestionRRHH;
using BE_GestionRRHH;
using DevExpress.Utils.Drawing;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraGrid.Views.Grid;

namespace UI_GestionRRHH.Formularios.Shared
{
    public partial class frmBusquedas : DevExpress.XtraEditors.XtraForm
    {
        private readonly UnitOfWork unit;

        //public eUsuario user = new eUsuario();
        public int BotonAgregarVisible = 0;  //0 se hace visible la botonera de agregar; 1 se visualiza
        //public int[] colorVerde, colorPlomo, colorEventRow, colorFocus;
        internal enum MiEntidad
        {
            Trabajador = 1
        }

        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string cod_condicion1 { get; set; }
        public string dsc_condicion1 { get; set; }
        public string cod_condicion2 { get; set; }
        public string dsc_condicion2 { get; set; }
        public string ruc { get; set; }
        public DateTime fch_generica { get; set; }

        internal MiEntidad entidad = MiEntidad.Trabajador;
        public string filtro = "";
        public string cod_cliente = "";
        public string cod_proveedor = "";
        public string filtroRUC = "NO";
        public string cod_empresa = "";
        public string cod_sede_empresa = "";
        public string cod_almacen = "";
        public string cod_proyecto = "";
        public string cod_tipo_servicio = "";
        public string flg_transportista = "";

        List<eTrabajador> ListTrabajador = new List<eTrabajador>();

        public frmBusquedas()
        {
            InitializeComponent();
            unit = new UnitOfWork();
        }
        private void frmBusquedas_Load(object sender, EventArgs e)
        {
            Inicializar();
        }

        public void Inicializar()
        {
            LlenarDataGrid();
        }

        public void LlenarDataGrid()
        {

            try
            {
                switch (entidad)
                {
                    case MiEntidad.Trabajador:
                        layoutControlItem2.Visibility = LayoutVisibility.Never;
                        layoutAgregar.Visibility = LayoutVisibility.Never;
                        ListTrabajador = unit.Trabajador.ListarTrabajadores<eTrabajador>(1, "", cod_empresa);
                        gcAyuda.DataSource = ListTrabajador;

                        this.Text = "Busqueda de Trabajadores";
                         
                        foreach (GridColumn col in gvAyuda.Columns)
                        {
                            col.Visible = false;
                            if (col.FieldName == "cod_trabajador" || col.FieldName == "dsc_nombres_completos") { col.Visible = true; }
                        }
                        gvAyuda.Columns["cod_trabajador"].Width = 50;
                        gvAyuda.Columns["dsc_nombres_completos"].Width = 200;
                        gvAyuda.Columns["dsc_empresa"].Width = 100;
                        
                        gvAyuda.Columns["cod_trabajador"].VisibleIndex = 0;
                        gvAyuda.Columns["dsc_nombres_completos"].VisibleIndex = 1;
                        gvAyuda.Columns["dsc_empresa"].VisibleIndex = 2;

                        gvAyuda.Columns["cod_trabajador"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                        gvAyuda.Columns["cod_trabajador"].Caption = "Código";
                        gvAyuda.Columns["dsc_nombres_completos"].Caption = "Nombre Completo";
                        gvAyuda.Columns["dsc_empresa"].Caption = "Empresa";

                        //focus en el campo autofilter
                        gcAyuda.Select();
                        gcAyuda.ForceInitialize();
                        gvAyuda.FocusedRowHandle = GridControl.AutoFilterRowHandle;
                        gvAyuda.FocusedColumn = gvAyuda.Columns["dsc_nombres_completos"];
                        gvAyuda.SetRowCellValue(GridControl.AutoFilterRowHandle, gvAyuda.Columns["dsc_nombres_completos"], filtro);

                        gvAyuda.ShowEditor();
                        break;
                }

            }
            catch
            {
            }
        }

        public void PasarDatos()
        {
            //DataGridViewRow row = (DataGridViewRow)gvAyuda.GetFocusedRow();
            switch (entidad)
            {
                case MiEntidad.Trabajador:
                    eTrabajador eTrab = gvAyuda.GetFocusedRow() as eTrabajador;
                    descripcion = eTrab.dsc_nombres_completos;
                    codigo = eTrab.cod_trabajador;
                    break;
            }
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //switch (entidad)
            //{
            //    case MiEntidad.ContactoxCliente:
            //        frmMantContactoDireccionCliente frm = new frmMantContactoDireccionCliente();
            //        frm.user = user;
            //        frm.MiAccion = frmMantContactoDireccionCliente.Cliente.NuevoContactoDesdeIncidente;
            //        frm.ShowDialog();
            //        codigo = frm.codigo;
            //        cod_condicion1 = frm.cod_condicion1;
            //        dsc_condicion1 = frm.dsc_condicion1;
            //        cod_condicion2 = frm.cod_condicion2;
            //        this.Close();
            //        break;
            //    case MiEntidad.Servicios:
            //        eProveedor_Servicios eProvServ = new eProveedor_Servicios();
            //        eProveedor_Servicios obj = gvAyuda.GetFocusedRow() as eProveedor_Servicios;
            //        eProvServ.cod_tipo_servicio = obj.cod_tipo_servicio; eProvServ.cod_proveedor = cod_proveedor; eProvServ.flg_activo = "SI" ; 
            //        eProvServ.cod_usuario_registro = user.cod_usuario;
            //        eProvServ = blProv.Guardar_Actualizar_ProveedorServicio<eProveedor_Servicios>(eProvServ);
            //        if (eProvServ == null) { MessageBox.Show("Error al vincular servicio al proveedor", "Vincular servicio", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            //        codigo = eProvServ.cod_tipo_servicio;
            //        this.Close();
            //        break;
            //    case MiEntidad.ProductosProyecto:
            //        blGlobal.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Agregando productos", "Cargando...");
            //        foreach (int nRow in gvAyuda.GetSelectedRows())
            //        {
            //            eProyecto.eProyecto_Producto objProduct = gvAyuda.GetRow(nRow) as eProyecto.eProyecto_Producto;
            //            eProyecto.eProyecto_Producto eProduct = new eProyecto.eProyecto_Producto();
            //            eProduct.cod_empresa = cod_empresa; eProduct.cod_proyecto = cod_proyecto; eProduct.flg_activo = "SI";
            //            eProduct.cod_tipo_servicio = objProduct.cod_tipo_servicio; eProduct.cod_subtipo_servicio = objProduct.cod_subtipo_servicio;
            //            eProduct.cod_producto = objProduct.cod_producto; eProduct.cod_usuario_registro = user.cod_usuario;
            //            eProduct = blFac.Insertar_Actualizar_ProyectoProductos<eProyecto.eProyecto_Producto>(eProduct);
            //            if (eProduct == null) { MessageBox.Show("Error al vincular productos", "Vincular producto", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            //            codigo = "-";
            //        }
            //        gvAyuda.RefreshData();
            //        SplashScreenManager.CloseForm();
            //        this.Close();
            //        break;
            //    case MiEntidad.ProveedorMultiple:
            //        foreach (int nRow in gvAyuda.GetSelectedRows())
            //        {
            //            eProveedor objP = gvAyuda.GetRow(nRow) as eProveedor;
            //            if (objP != null) ListProv.Add(objP);
            //        }
            //        this.Close();
            //        break;
            //    case MiEntidad.ProveedorTipoServicio:
            //        foreach (int nRow in gvAyuda.GetSelectedRows())
            //        {
            //            eProveedor objP = gvAyuda.GetRow(nRow) as eProveedor;
            //            if (objP != null) ListProv.Add(objP);
            //        }
            //        this.Close();
            //        break;
            //    case MiEntidad.Productos:
            //        foreach (int nRow in gvAyuda.GetSelectedRows())
            //        {
            //            eProyecto.eProyecto_Producto objPR = gvAyuda.GetRow(nRow) as eProyecto.eProyecto_Producto;
            //            if (objPR != null) ListProd.Add(objPR);
            //        }
            //        this.Close();
            //        break;
            //}
        }

        private void gvAyuda_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (gvAyuda.OptionsSelection.MultiSelectMode != DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect)
            {
                if (e.Clicks == 2 && e.RowHandle >= 0)
                {
                    PasarDatos();
                    this.Close();
                }
            }
        }

        private void gvAyuda_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }
        private void frmBusquedas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void gvAyuda_ShownEditor(object sender, EventArgs e)
        {
            if (gvAyuda.FocusedRowHandle == GridControl.AutoFilterRowHandle)
            {
                var editor = (TextEdit)gvAyuda.ActiveEditor;
                editor.SelectionLength = 0;
                editor.SelectionStart = editor.Text.Length;
            }
        }

        private void gvAyuda_KeyDown(object sender, KeyEventArgs e)
        {
            if (gvAyuda.FocusedRowHandle >= 0 && e.KeyCode == Keys.Enter)
            {
                PasarDatos();
                this.Close();
            }
        }

        private void gvAyuda_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void btnNuevoProveedor_Click(object sender, EventArgs e)
        {
            //switch (entidad)
            //{
            //    case MiEntidad.ClienteEmpresa:
            //        frmMantCliente frmCliente = new frmMantCliente();
            //        frmCliente.MiAccion = Cliente.Nuevo;
            //        frmCliente.colorVerde = colorVerde;
            //        frmCliente.colorPlomo = colorPlomo;
            //        frmCliente.colorEventRow = colorEventRow;
            //        frmCliente.colorFocus = colorFocus;
            //        frmCliente.user = user;
            //        frmCliente.ShowDialog();
            //        Inicializar();
            //        break;
            //    case MiEntidad.Proveedor:
            //        frmMantProveedor frm = new frmMantProveedor();
            //        frm.user = user;
            //        frm.colorVerde = colorVerde;
            //        frm.colorPlomo = colorPlomo;
            //        frm.colorEventRow = colorEventRow;
            //        frm.colorFocus = colorFocus;
            //        frm.ShowDialog();
            //        Inicializar();
            //        break;
            //}
        }
    }
}