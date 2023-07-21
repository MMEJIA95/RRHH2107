using BE_GestionRRHH;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_GestionRRHH.Formularios.Personal
{
    internal enum Area
    {
        Nuevo = 0,
        Editar = 1,
    }
    public partial class frmAreaEmpresa : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;

        internal Area MiAccion = Area.Nuevo;
        public string Actualizarcombo = "NO";
        public eTrabajador eTrab = new eTrabajador();
        List<eTrabajador.eArea> ListTrabajdor = new List<eTrabajador.eArea>();
        public bool ActualizarListado = false;
        public string cod_trabajador = "", cod_empresa = "", cod_sede_empresa, empresa, area, cod_area = "";
        public frmAreaEmpresa()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            configurar_formulario();
            btneliminar.Visible = false;
            
        }



        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lkpempresa.EditValue == null) { MessageBox.Show("Debe seleccionar una empresa.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpempresa.Focus(); return; }
                if (lkpsede_empresa.EditValue == null) { MessageBox.Show("Debe seleccionar una sede.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpsede_empresa.Focus(); return; }
                if (txtCargo.Text.Trim() == "") { MessageBox.Show("Debe ingresar un Area", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCargo.Focus(); return; }
                if (btnGuardar.Text == "GUARDAR")
                {
                    eTrabajador.eArea eTrab = AsignarValores(remplazar: "NO");
                    eTrab = unit.Trabajador.Insertar_Actualizar_AreaEmpresa<eTrabajador.eArea>(eTrab);

                    if (eTrab == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (eTrab != null)
                    {
                        MiAccion = Area.Editar;
                        ActualizarListado = true;
                        MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Inicializar();
                    }
                }
                else if (btnGuardar.Text == "MODIFICAR")
                {
                    eTrabajador.eArea eTrab = AsignarValores(remplazar: "SI");
                    eTrab = unit.Trabajador.Insertar_Actualizar_AreaEmpresa<eTrabajador.eArea>(eTrab);

                    if (eTrab == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (eTrab != null)
                    {
                        MiAccion = Area.Editar;
                        ActualizarListado = true;
                        MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Inicializar();
                    }
                }
                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Actualizarcombo = "SI";
        }

        private void lkpempresa_EditValueChanged(object sender, EventArgs e)
        {
           
            if (lkpempresa.EditValue != null)
            {
                unit.Trabajador.CargaCombosLookUp("SedesEmpresa", lkpsede_empresa, "cod_sede_empresa", "dsc_sede_empresa", "", valorDefecto: true, lkpempresa.EditValue.ToString());
                lkpsede_empresa.ItemIndex = 0;
                listar_Area();
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }

           // listar_Area();
        }

        private void gvListadoArea_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvListadoArea_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void CargarLookUpEdit()
        {

            try
            {
                unit.Trabajador.CargaCombosLookUp("Empresa", lkpempresa, "cod_empresa", "dsc_empresa", "", valorDefecto: true);
                // blTrab.CargaCombosLookUp("flg_activos", lkpestado, "cod_flg_activo", "dsc_flg_activo", "", valorDefecto: true);
                //lkpempresa.EditValue = Program.Sesion.EmpresaList[0].cod_empresa;
                //cod_empresa = lkpempresa.EditValue.ToString();

                if (cod_empresa == null)
                {
                    //Se cambiar por la  empresa asignado al usuario; cargado en sesión.
                    //List<eProveedor_Empresas> listEmpresasUsuario = blProv.ListarEmpresasProveedor<eProveedor_Empresas>(11, "", Program.Sesion.Usuario.cod_usuario);
                    if (Program.Sesion.EmpresaList.Count == 1)
                    {
                        lkpempresa.EditValue = Program.Sesion.EmpresaList[0].cod_empresa;
                        cod_empresa = Program.Sesion.EmpresaList[0].cod_empresa;
                    }
                    else
                    {
                        lkpempresa.EditValue = empresa; cod_empresa = empresa;
                        
                    }
                }
                else
                {
                    lkpempresa.EditValue = cod_empresa;
                    lkpsede_empresa.ItemIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        public void mostrardatos()
        {
            eTrabajador.eArea obj = gvListadoArea.GetFocusedRow() as eTrabajador.eArea;
            obj = gvListadoArea.GetFocusedRow() as eTrabajador.eArea;
            if (obj == null) return;
            lkpempresa.EditValue = obj.cod_empresa;
            lkpsede_empresa.EditValue = obj.cod_sede_empresa;
            txtCargo.Text = obj.dsc_area;
            cod_area = obj.cod_area;
        }
        private void gvListadoArea_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0) gvListadoArea_FocusedRowChanged(gvListadoArea, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, e.RowHandle));
            mostrardatos();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
           // CargarLookUpEdit();
            txtCargo.Text = "";
            btnGuardar.Enabled = true;
            btnGuardar.Text = "GUARDAR";
            // gvListadoArea.RefreshData();
        }

        private void gvListadoArea_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
               
                mostrardatos();
            Cargosregistrados();
        }

        private void lkpsede_empresa_EditValueChanged(object sender, EventArgs e)
        {
            listar_Area();
            Cargosregistrados();
        }

        private void gcListadoArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {

                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Obteniendo listado", "Cargando...");
                listar_Area();
                //listTrabajador = unit.Trabajador.ListarTrabajadoresvista<eTrabajador>(1, "", "ALL", ""); // LISTA GENERAL
                //                                                                                         //CargarListado();
                //bsListaTrabajador.DataSource = listTrabajador; gvListadoTrabajador.RefreshData();
                SplashScreenManager.CloseForm();

            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("¿Esta seguro de eliminar el Area?" + Environment.NewLine + "Esta acción es irreversible.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string result = "";
                    result = unit.Trabajador.EliminarInactivar_DatosTrabajador(8, cod_empresa: lkpempresa.EditValue.ToString(), cod_sede_empresa: lkpsede_empresa.EditValue == null ? null : lkpsede_empresa.EditValue.ToString(),cod_area:cod_area);
                    if (result != "OK") { MessageBox.Show("Error al eliminar area", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (result == "OK") MessageBox.Show("Se procedió a eliminar area de manera satisfactoria.", "Eliminar Cargo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //mostrardatos();
                    listar_Area();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private eTrabajador.eArea AsignarValores(string remplazar="")
        {
            eTrabajador.eArea obj = new eTrabajador.eArea();
            if (remplazar == "NO")
            {
                obj.cod_empresa = lkpempresa.EditValue == null ? null : lkpempresa.EditValue.ToString();
                obj.cod_sede_empresa = lkpsede_empresa.EditValue == null ? null : lkpsede_empresa.EditValue.ToString();
                obj.dsc_area = txtCargo.Text;
                obj.flg_activo = "SI";
                obj.remplazo = "NO";
                

            }
            else if (remplazar == "SI")
            {
                obj.cod_empresa = lkpempresa.EditValue == null ? null : lkpempresa.EditValue.ToString();
                obj.cod_sede_empresa = lkpsede_empresa.EditValue == null ? null : lkpsede_empresa.EditValue.ToString();
                obj.cod_area = cod_area;
                obj.dsc_area = txtCargo.Text;
                obj.flg_activo = "SI";
                obj.remplazo = "SI";
            }
                return obj;

        }
        private void listar_Area()
        {

            ListTrabajdor = unit.Trabajador.ListarArea<eTrabajador.eArea>(7, lkpempresa.EditValue == null ? null : lkpempresa.EditValue.ToString(), lkpsede_empresa.EditValue == null ? null : lkpsede_empresa.EditValue.ToString());
            bsListaArea.DataSource = ListTrabajdor; gvListadoArea.RefreshData();
            //gvListadoArea.RefreshData();
        }
       
        private void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;
            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoArea, gvListadoArea);

        }
        private void Inicializar()
        {
            try
            {
                switch (MiAccion)
                {
                    case Area.Nuevo:
                        txtCargo.Text = "";
                        CargarLookUpEdit();
                        btnGuardar.Text = "GUARDAR";
                        break;
                    case Area.Editar:
                        listar_Area();
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
            
        }
        private void frmAreaEmpresa_Load(object sender, EventArgs e)
        {
            lkpsede_empresa.ItemIndex = 0;
            Inicializar();
            Cargosregistrados();



        }

        private void Cargosregistrados()
        {

            eTrabajador.eArea obj1 = gvListadoArea.GetFocusedRow() as eTrabajador.eArea;
            obj1 = gvListadoArea.GetFocusedRow() as eTrabajador.eArea;

            obj1 = unit.Trabajador.Obtener_cod_trabajador<eTrabajador.eArea>(94, cod_empresa: lkpempresa.EditValue.ToString(), cod_sede_empresa: lkpsede_empresa.EditValue == null ? null : lkpsede_empresa.EditValue.ToString(),cod_area:cod_area);
            if (obj1 == null) return;
            if (obj1.registros != null) { btnGuardar.Text = "GUARDAR"; txtCargo.ReadOnly = true; 
                btnGuardar.Enabled = false; 
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never; }
            else { btnGuardar.Enabled = true; 
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always; 
                btnGuardar.Text = "MODIFICAR"; txtCargo.ReadOnly = false; }

        }

    }
}