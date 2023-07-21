using BE_GestionRRHH;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraSplashScreen;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace UI_GestionRRHH.Formularios.Personal
{
    public partial class frmCargoArea : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;
        internal Area MiAccion = Area.Nuevo;
        public eTrabajador eTrab = new eTrabajador();
        List<eTrabajador.eCargos> ListCargo = new List<eTrabajador.eCargos>();
        public string ActualizarListado = "NO";
        public string Actualizarcombo = "NO";
        public string cod_trabajador = "", cod_empresa = "", cod_sede_empresa, empresa, cod_cargo = "",bloqueo;
        private static IEnumerable<eSubModulo.eCampos> listadocampos;
        public frmCargoArea()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            configurar_formulario();
        }
        private void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;
            unit.Globales.ConfigurarGridView_ClasicStyle(gcCargo, gvCargo);

        }
        private void ObtenerNombresControles()
        {
            List<string> names = new List<string>();
            RecorrerControles(layoutControlGroup1, names);

            // Aquí puedes hacer lo que necesites con los nombres de los controles de texto
            foreach (string name in names)
            {
                var enable = "";
                if (listadocampos != null && listadocampos.Any(e => e.cod_campo == name))
                {
                    // Agregar el nombre al TextBox llamado 'textBoxResultados'                    
                    textBoxResultados.AppendText("Se encontró el nombre: " + name + Environment.NewLine);

                    eSubModulo.eCampos trabajador = listadocampos.FirstOrDefault(e => e.cod_campo == name);
                    if (trabajador != null && trabajador.flg_bloqueo)
                    {
                       
                    }

                }

            }
        }

        private Control FindControlByName(Control container, string name)
        {
            foreach (Control control in container.Controls)
            {
                if (control.Name == name)
                {
                    return control;
                }
                else if (control.Controls.Count > 0)
                {
                    Control foundControl = FindControlByName(control, name);
                    if (foundControl != null)
                    {
                        return foundControl;
                    }
                }
            }
            return null;
        }





        private void listarcampos()
        {
            listadocampos = unit.Sistema.Listar_SubModulos<eSubModulo.eCampos>(2, cod_empresa, "", 0);
            
            

        }

        private void CargarLookUpEdit()
        {
            try
            {
                unit.Trabajador.CargaCombosLookUp("EmpresaUsuario", lkpempresa, "cod_empresa", "dsc_empresa", "", valorDefecto: true,cod_usuario:Program.Sesion.Usuario.cod_usuario);

            

                if (cod_empresa == null)
                {
                    //Cambiado por empresas asignadas a un usuario; regisradas en sesión
                    //List<eProveedor_Empresas> listEmpresasUsuario = blProv.ListarEmpresasProveedor<eProveedor_Empresas>(11, "", user.cod_usuario);
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
                    lkpsede.ItemIndex = 0;
                    lkp_area.ItemIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Inicializar()
        {
            try
            {
                switch (MiAccion)
                {
                    case Area.Nuevo:
                        break;
                    case Area.Editar:
                        //ObtenerDatos_Area();

                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        
            CargarLookUpEdit();
            CargarListado();
            //Listar_Cargo();



        }
  
        public void mostrardatos()
        {
            eTrabajador.eCargos obj = gvCargo.GetFocusedRow() as eTrabajador.eCargos;
            obj = gvCargo.GetFocusedRow() as eTrabajador.eCargos;
            if (obj == null) return;
            cod_cargo = obj.cod_cargo;
            lkpempresa.EditValue = obj.cod_empresa;
            lkpsede.EditValue = obj.cod_sede_empresa;
            lkp_area.EditValue = obj.cod_area;
            txtCargo.Text = obj.dsc_cargo;
        }


        private void CargarListado()
        {
           
           
                ListCargo = unit.Trabajador.ListarAreas<eTrabajador.eCargos>(84, lkpempresa.EditValue.ToString(), lkpsede.EditValue == null ? null : lkpsede.EditValue.ToString(), lkp_area.EditValue == null ? null : lkp_area.EditValue.ToString());
                bsCargos.DataSource = ListCargo; gvCargo.RefreshData();
                
            
        }

        
        private eTrabajador.eCargos AsignarValores(string remplazar="")
        {
            eTrabajador.eCargos obj = new eTrabajador.eCargos();
            if (remplazar == "NO") { 
            
           // obj.cod_cargo = cod_cargo;
            obj.cod_empresa = lkpempresa.EditValue.ToString();
            obj.cod_sede_empresa = lkpsede.EditValue.ToString();
            obj.cod_area = lkp_area.EditValue.ToString();
            obj.dsc_cargo = txtCargo.Text;
            obj.flg_activo = "SI";
            obj.flg_firma_documento = "NO";

            }else if (remplazar == "SI")
            {
               
                obj.cod_cargo = cod_cargo;
                obj.cod_empresa = lkpempresa.EditValue.ToString();
                obj.cod_sede_empresa = lkpsede.EditValue.ToString();
                obj.cod_area = lkp_area.EditValue.ToString();
                obj.dsc_cargo = txtCargo.Text;
                obj.flg_activo = "SI";
                obj.flg_firma_documento = "NO";
                obj.remplazo = "SI";
               
            }
            return obj;
        }

        private void frmCargoArea_Load(object sender, EventArgs e)
        {
            lkp_area.ItemIndex = 0;
            lkpsede.ItemIndex = 0;
            Inicializar();
            
            btnGuardar.Text = "GUARDAR";
            Cargosregistrados();
            listarcampos();
            ObtenerNombresControles();
            
        }

        private void lkpempresa_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpempresa.EditValue != null)
            {
                unit.Trabajador.CargaCombosLookUp("SedesEmpresa", lkpsede, "cod_sede_empresa", "dsc_sede_empresa", "", valorDefecto: true, lkpempresa.EditValue.ToString());
                lkpsede.ItemIndex = 0;
                unit.Trabajador.CargaCombosLookUp("AreaEmpresa", lkp_area, "cod_area", "dsc_area", "", valorDefecto: true, lkpempresa.EditValue.ToString(), lkpsede.EditValue.ToString());
                lkp_area.Refresh();
                lkp_area.ItemIndex = 0;
                CargarListado();
                layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
           
        }

        private void lkpsede_EditValueChanged(object sender, EventArgs e)
        {
                lkpsede.Refresh();
                if (lkpsede.EditValue != null) unit.Trabajador.CargaCombosLookUp("AreaEmpresa", lkp_area, "cod_area", "dsc_area", "", valorDefecto: true, lkpempresa.EditValue.ToString(), lkpsede.EditValue.ToString());
                if (lkpsede.EditValue == null) { lkp_area.EditValue = null; lkp_area.Properties.DataSource = null; }
                lkp_area.ItemIndex = 0;
                // lkp_area.EditValue = null;
                //txtCargo.Text = "";
                 CargarListado();
            
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lkpempresa.EditValue == null) { MessageBox.Show("Debe seleccionar una empresa.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpempresa.Focus(); return; }
                if (lkpsede.EditValue == null) { MessageBox.Show("Debe seleccionar una sede.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpsede.Focus(); return; }
                if (lkp_area.EditValue == null) { MessageBox.Show("Debe ingresar un Area.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCargo.Focus(); return; }

                if (txtCargo.Text.Trim() == "") { MessageBox.Show("Debe ingresar un Cargo.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtCargo.Focus(); return; }

                if (btnGuardar.Text == "GUARDAR")
                {
                    eTrabajador.eCargos eTrab = AsignarValores(remplazar: "NO");
                    eTrab = unit.Trabajador.Insertar_Actualizar_Cargos<eTrabajador.eCargos>(eTrab);

                    if (eTrab == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (eTrab != null)
                    {
                        MiAccion = Area.Editar;

                        MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        CargarListado();
                        // Inicializar();
                    }
                }
                else if (btnGuardar.Text == "MODIFICAR")
                {
                    eTrabajador.eCargos eTrab = AsignarValores(remplazar: "SI");
                    eTrab = unit.Trabajador.Insertar_Actualizar_Cargos<eTrabajador.eCargos>(eTrab);

                    if (eTrab == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (eTrab != null)
                    {
                        //MiAccion = Area.Editar;

                        MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        CargarListado();
                        // Inicializar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Actualizarcombo = "SI";
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            //CargarLookUpEdit();
            txtCargo.ReadOnly = false;
            txtCargo.Text = "";
            cod_cargo = "0";
            btnGuardar.Text = "GUARDAR";
        }

        internal void gcCargo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {

                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Obteniendo listado", "Cargando...");
                CargarListado();

                SplashScreenManager.CloseForm();

            }
        }

        private void lkp_area_EditValueChanged(object sender, EventArgs e)
        {
           
            CargarListado();
            Cargosregistrados();
        }

        private void gvCargo_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("¿Esta seguro de eliminar el Cargo?" + Environment.NewLine + "Esta acción es irreversible.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string result = "";
                    result = unit.Trabajador.EliminarInactivar_DatosTrabajador(7, cod_empresa: lkpempresa.EditValue.ToString(), cod_sede_empresa: lkpsede.EditValue == null ? null : lkpsede.EditValue.ToString(), cod_area: lkp_area.EditValue == null ? null : lkp_area.EditValue.ToString(), cod_cargo: cod_cargo == null ? null : cod_cargo);
                    if (result != "OK") { MessageBox.Show("Error al eliminar cargo", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (result == "OK") MessageBox.Show("Se procedió a eliminar cargo de manera satisfactoria.", "Eliminar Cargo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //mostrardatos();
                    CargarListado();
                    txtCargo.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gvCargo_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)    mostrardatos(); txtCargo.ReadOnly = true;
        }

        private void gvCargo_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0) gvCargo_FocusedRowChanged(gvCargo, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, e.RowHandle));
            mostrardatos();
            Cargosregistrados();
        }

        private void gvCargo_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
        }

        private void Cargosregistrados()
        {

            eTrabajador.eCargos obj1 = gvCargo.GetFocusedRow() as eTrabajador.eCargos;
            obj1 = gvCargo.GetFocusedRow() as eTrabajador.eCargos;
           
            obj1 = unit.Trabajador.Obtener_cod_trabajador<eTrabajador.eCargos>(93, cod_empresa:lkpempresa.EditValue.ToString(), cod_sede_empresa:lkpsede.EditValue == null ? null : lkpsede.EditValue.ToString(), cod_area:lkp_area.EditValue == null ? null : lkp_area.EditValue.ToString(), cod_cargo:cod_cargo == null ? null : cod_cargo);
            if (obj1 == null) return;
            if (obj1.registros != null) { btnGuardar.Text = "GUARDAR"; btnGuardar.Enabled = false; txtCargo.ReadOnly=true; btnEliminar.Visible = true; layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never; } else { btnGuardar.Enabled = true; layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always; btnGuardar.Text = "MODIFICAR"; txtCargo.ReadOnly = false; }
            
        }
        public void RecorrerControles(DevExpress.XtraLayout.LayoutControlGroup container, List<string> names)
        {
            foreach (BaseLayoutItem item in container.Items)
            {
                if (item is LayoutControlItem layoutControlItem)
                {
                    if (layoutControlItem.Control is DevExpress.XtraEditors.TextEdit textEdit)
                    {
                        names.Add(textEdit.Name);
                        eSubModulo.eCampos trabajador = listadocampos.FirstOrDefault(e => e.cod_campo == textEdit.Name);
                        if (trabajador != null && trabajador.flg_bloqueo)
                        {
                            textEdit.Enabled = Convert.ToBoolean(bloqueo);
                        }
                    }
                }
                else if (item is DevExpress.XtraLayout.LayoutControlGroup group)
                {
                    RecorrerControles(group, names);
                }
            }
        }


    }
}