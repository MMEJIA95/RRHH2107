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
using DevExpress.Utils.Menu;
using System.Web.UI.WebControls;

namespace UI_GestionRRHH.Formularios.Personal
{
    internal enum Documento
    {
        Nuevo = 0,
        Editar = 1,
    }
    public partial class frmRegistroDocumentos : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;
        internal Documento MiAccion = Documento.Nuevo;
        public string Actualizarcombo = "NO";
        public eTrabajador eTrab = new eTrabajador();
        public eTrabajador.eEMO eTrabemo = new eTrabajador.eEMO();
        public eTrabajador.eDocumento_Trabajador eTrabdoc = new eTrabajador.eDocumento_Trabajador();
        public bool ActualizarListado = false;
        List<eTrabajador.eDocumento_Trabajador> ListTrabajdor = new List<eTrabajador.eDocumento_Trabajador>();
        public frmRegistroDocumentos()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            configurar_formulario();
        }
        private void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;
            unit.Globales.ConfigurarGridView_ClasicStyle(gcDocumentos, gvDocumentos);

        }

        private void Inicializar()
        {
            try
            {
                switch (MiAccion)
                {
                    case Documento.Nuevo:
                        txtnombre.Text = "";
                        btnGuardar.Text = "GUARDAR";
                        CargarLookUpEdit();
                        break;
                    case Documento.Editar:
                        listarDocumento();
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void listarDocumento()
        {

            ListTrabajdor = unit.Trabajador.ListarArea<eTrabajador.eDocumento_Trabajador>(109, lkpEmpresa.EditValue.ToString());
            bsDocumentos.DataSource = ListTrabajdor; gvDocumentos.RefreshData();
        
        }

        private void frmRegistroDocumentos_Load(object sender, EventArgs e)
        {
            Inicializar();
            CargarLookUpEdit();
      
        }

        private void gvDocumentos_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)

                mostrardatos();
        }

        public void mostrardatos()
        {
            eTrabajador.eDocumento_Trabajador obj = gvDocumentos.GetFocusedRow() as eTrabajador.eDocumento_Trabajador;
            obj = gvDocumentos.GetFocusedRow() as eTrabajador.eDocumento_Trabajador;
            if (obj == null) return;
            txtnombre.Text = obj.dsc_documento;
            rgTipoRegistro.SelectedIndex = obj.flg_varios == "NO" ? 0 : 1;
            toggle_estado.EditValue = obj.flg_activo == "NO" ? false : true;
            txtabreviatura.Text = obj.dsc_abreviatura;
            lkpEmpresa.EditValue = obj.cod_empresa;

            if (obj.flg_varios == "SI")
            {
                eTrabemo = unit.Trabajador.Obtener_emo<eTrabajador.eEMO>(111, cod_documento: obj.cod_documento);
                if (eTrabemo != null) { rgTipoRegistro.Enabled = false; txtabreviatura.Enabled = false;lkpEmpresa.Enabled = false;txtnombre.Enabled = false; btneliminar.Visible = false; layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never; btnGuardar.Text = "GUARDAR"; } else if (eTrabemo == null) { rgTipoRegistro.Enabled = true; txtnombre.Enabled = true; txtabreviatura.Enabled = true; layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always; btnGuardar.Text = "MODIFICAR"; }
            }else if (obj.flg_varios =="NO")
            {

                eTrabdoc = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(110, cod_documento: obj.cod_documento, cod_empresa: lkpEmpresa.EditValue.ToString());
                if (eTrabdoc != null) { rgTipoRegistro.Enabled = false; txtnombre.Enabled = false; txtabreviatura.Enabled = false; lkpEmpresa.Enabled = false; btneliminar.Visible = false; layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never; btnGuardar.Text = "GUARDAR"; } else if (eTrabdoc == null) { rgTipoRegistro.Enabled = true; txtnombre.Enabled = true; txtabreviatura.Enabled = true; layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always; btnGuardar.Text = "MODIFICAR"; }

            }

            

        }

        private void gvDocumentos_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0)
                //gvDocumentos_FocusedRowChanged(gvDocumentos, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, e.RowHandle));

               
            mostrardatos();
        }

   

        private void btnnuevo_Click(object sender, EventArgs e)
        {
            int iddoc = 0;

            txtnombre.Text = "";
            rgTipoRegistro.SelectedIndex = 0;
            btnGuardar.Text = "GUARDAR";
            toggle_estado.EditValue = true;
            txtabreviatura.Text = "";
            txtabreviatura.Enabled = true;
            lkpEmpresa.Enabled = true;
            rgTipoRegistro.Enabled = true;
            txtnombre.Enabled = true;


        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtnombre.Text.Trim() == "") { MessageBox.Show("Debe ingresar un Area", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtnombre.Focus(); return; }
                if (btnGuardar.Text == "GUARDAR")
                {
                    eTrabajador.eDocumento_Trabajador eTrab = AsignarValores(remplazar: "NO");
                    eTrab = unit.Trabajador.Insertar_ActualizarDocumentos<eTrabajador.eDocumento_Trabajador>(eTrab);

                    if (eTrab == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (eTrab != null)
                    {
                        MiAccion = Documento.Editar;
                        ActualizarListado = true;
                        MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Inicializar();
                    }
                }
                else if (btnGuardar.Text == "MODIFICAR")
                {
                    eTrabajador.eDocumento_Trabajador eTrab = AsignarValores(remplazar: "SI");
                    eTrab = unit.Trabajador.Insertar_ActualizarDocumentos<eTrabajador.eDocumento_Trabajador>(eTrab);

                    if (eTrab == null) { MessageBox.Show("Error al guardar los datos.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (eTrab != null)
                    {
                        MiAccion = Documento.Editar;
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

        private eTrabajador.eDocumento_Trabajador AsignarValores(string remplazar = "")
        {
            bool val = Convert.ToBoolean(toggle_estado.EditValue);
            string activ = "";
            eTrabajador.eDocumento_Trabajador obj = new eTrabajador.eDocumento_Trabajador();
            val = false ? false : true;

            if (remplazar == "NO")
            {
                obj.flg_varios = rgTipoRegistro.SelectedIndex == 0 ? "NO" : "SI";
                obj.dsc_documento = txtnombre.Text;
                activ = toggle_estado.EditValue.ToString();
                obj.flg_activo = activ == "False" ? "NO" : "SI";
                obj.remplazo = "NO";
                obj.dsc_abreviatura = txtabreviatura.Text;
                obj.cod_empresa = lkpEmpresa.EditValue.ToString();



            }
            else if (remplazar == "SI")
            {
                eTrabajador.eDocumento_Trabajador obj1 = gvDocumentos.GetFocusedRow() as eTrabajador.eDocumento_Trabajador;
                obj.iddoc = obj1.iddoc;
                obj.flg_varios = rgTipoRegistro.SelectedIndex == 0 ? "NO" : "SI";
                obj.dsc_documento = txtnombre.Text;
                obj.remplazo = "SI";
                activ = toggle_estado.EditValue.ToString();
                obj.flg_activo = activ == "False" ? "NO" : "SI";
                obj.dsc_abreviatura = txtabreviatura.Text;
                obj.cod_empresa = lkpEmpresa.EditValue.ToString();
            }
            return obj;

        }

        private void gvDocumentos_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void toggleSwitch1_Toggled(object sender, EventArgs e)
        {

        }

        private void CargarLookUpEdit()
        {
            try
            {
                unit.Trabajador.CargaCombosLookUp("EmpresaUsuario", lkpEmpresa, "cod_empresa", "dsc_empresa", "", valorDefecto: true, cod_usuario: Program.Sesion.Usuario.cod_usuario);
                lkpEmpresa.ItemIndex = 0;
                if (Program.Sesion.EmpresaList.Count >= 1) lkpEmpresa.EditValue = Program.Sesion.EmpresaList[0].cod_empresa;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lkpEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpEmpresa.EditValue != null)
            {
                listarDocumento();

            }
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            eTrabajador.eDocumento_Trabajador obj = gvDocumentos.GetFocusedRow() as eTrabajador.eDocumento_Trabajador;
            string result = "";
            if (obj.flg_varios == "NO")
            {
              
                result = unit.Trabajador.EliminarInactivarEMOtrabajador(10, cod_documento: obj.cod_documento, cod_empresa: lkpEmpresa.EditValue.ToString());
            }else if (obj.flg_varios == "SI"){
                result = unit.Trabajador.EliminarInactivarEMOtrabajador(10, cod_documento: obj.cod_documento, cod_empresa: lkpEmpresa.EditValue.ToString());
            }

            if (result=="OK")
            {
                MiAccion = Documento.Editar;
                ActualizarListado = true;
                MessageBox.Show("Se elimino el domunto.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listarDocumento();
                gvDocumentos.RefreshData();
            }
        }
    }
}