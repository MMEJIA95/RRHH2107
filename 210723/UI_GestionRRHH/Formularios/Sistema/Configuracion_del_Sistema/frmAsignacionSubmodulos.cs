using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_GestionRRHH;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using iTextSharp.text.pdf.codec.wmf;
using HNG_Tools;
using static UI_GestionRRHH.Formularios.Personal.frmListadoTrabajador;

namespace UI_GestionRRHH.Formularios.Sistema.Configuracion_del_Sistema
{
    public partial class frmAsignacionSubmodulos : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly UnitOfWork unit;
        string _empresa = Program.Sesion.EmpresaList[0].dsc_empresa.ToString();
        string _solucion = Program.Sesion.Global.Solucion;
        private static IEnumerable<eSubModulo.eCampos> lstdatosgenerales;
        public string flg_obligatorio;
        public frmAsignacionSubmodulos()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            configurar_formulario();
        }
        private void configurar_formulario()
        {
            btnBuscador.Appearance.BackColor = Program.Sesion.Colores.Verde;
            unit.Globales.ConfigurarGridView_ClasicStyle(gcSubMofulo, gvSubModulo);
        }
        private void CargarOpcionesMenu()
        {
            var solucion = unit.Sistema.ListarOpcionesMenuVentana<eSolucion>(5);
            foreach (var item in solucion)
            {
                clbSolucion.Items.Add(item.dsc_solucion);
            }
            if (solucion.Count > 0)
                clbSolucion.Items[0].CheckState = CheckState.Checked;

            lblsolucion.Text = _solucion;

        }
        private void CargarOpcionesMenuEmpresa()
        {
            var empresa = unit.Proveedores.ListarEmpresasProveedor<eProveedor_Empresas>(11, "", Program.Sesion.Usuario.cod_usuario);
            foreach (var item in empresa)
            {
                clbEmpresa.Items.Add(item.dsc_empresa);
            }
            if (empresa.Count > 0)
                clbEmpresa.Items[0].CheckState = CheckState.Checked;

            lblempresa.Text = _empresa;

        }

        private void CargarSubModulos()
        {
            List<eSubModulo> ListadoSubModulos = new List<eSubModulo>();
            ListadoSubModulos = unit.Sistema.Listar_SubModulos<eSubModulo>(1,"","",1);
            bsSubModulo.DataSource = ListadoSubModulos;
        }
        private void CargarCamposxEmpresa(string _empresa , string _solucion)
        {
            bsCampos.DataSource = null;
            eSubModulo obj = gvSubModulo.GetFocusedRow() as eSubModulo;
            if (obj != null)
            {
                var r = gvCampos.FocusedRowHandle;
                List<eSubModulo.eCampos> ListadoVentanas = new List<eSubModulo.eCampos>();
                ListadoVentanas = unit.Sistema.Listar_SubModulos<eSubModulo.eCampos>(2, cod_empresa:_empresa, cod_solucion: _solucion, cod_submodulo:obj.cod_submodulo);
                bsCampos.DataSource = ListadoVentanas;
                gvCampos.FocusedRowHandle = r;
            }
        }

        private void frmAsignacionSubmodulos_Load(object sender, EventArgs e)
        {
            CargarOpcionesMenu();
            CargarOpcionesMenuEmpresa();
        }

        private void EjecutarBuscador()
        {
            _solucion = clbSolucion.SelectedItem.ToString();
            _empresa = clbEmpresa.SelectedItem.ToString();
            //   unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Obteniendo listado", "Cargando...");
            lblempresa.Text = _empresa;
            lblsolucion.Text = _solucion;
            CargarSubModulos();
            CargarCamposxEmpresa(_empresa, _solucion);
           // SplashScreenManager.CloseForm();
        }

        private void btnBuscador_Click(object sender, EventArgs e)
        {
            EjecutarBuscador();
        }

        private void rchkbloqueo_CheckedChanged(object sender, EventArgs e)
        {
            gvCampos.PostEditor();
            eSubModulo.eCampos obj = gvCampos.GetRow(gvCampos.FocusedRowHandle) as eSubModulo.eCampos;
            if (obj.flg_obligatorio == true) {
                obj.flg_bloqueo =  false ;
                 
            }
            if (!Convert.ToBoolean(obj.flg_bloqueo))
            {
                gvCampos.SetRowCellValue(gvCampos.FocusedRowHandle, "flg_bloqueo", false);
            }

            eSubModulo.eCampos obj1 = gvCampos.GetFocusedRow() as eSubModulo.eCampos;

            obj.flg_obligatorio = obj1.flg_obligatorio;
            obj.flg_bloqueo = obj1.flg_bloqueo;
            obj.dsc_empresa = _empresa;
            unit.Sistema.GuardarActualizar_asignacion(obj, 3, Program.Sesion.Usuario.cod_usuario);

        }



        private void gvCampos_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvCampos_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvSubModulo_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

            EjecutarBuscador();
        }

        private void gvCampos_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            eSubModulo.eCampos obj = gvCampos.GetFocusedRow() as eSubModulo.eCampos;
        
        }

        private void gvCampos_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "flg_bloqueo" && (bool)e.Value == true)
            {
                bool isFirstCheckSelected = (bool)gvCampos.GetRowCellValue(e.RowHandle, "flg_obligatorio");

                if (isFirstCheckSelected)
                {
                    HNG.MessageWarning("No se puede bloquear este campo ya que es obligatorio", "Advertencia");
                    gvCampos.SetRowCellValue(e.RowHandle, "flg_bloqueo", false); // Cancela el cambio
                }

            }
        }

        private void rchktregistro_CheckedChanged(object sender, EventArgs e)
        {
            gvCampos.PostEditor();
            eSubModulo.eCampos obj = gvCampos.GetRow(gvCampos.FocusedRowHandle) as eSubModulo.eCampos;
            if (obj.flg_tregistro == true)
            {
                obj.flg_obligatorio = true;
                obj.flg_bloqueo = false;
               
            }
            if (!Convert.ToBoolean(obj.flg_bloqueo))
            {
                gvCampos.SetRowCellValue(gvCampos.FocusedRowHandle, "flg_bloqueo", false);
            }

            eSubModulo.eCampos obj1 = gvCampos.GetFocusedRow() as eSubModulo.eCampos;

            obj.flg_obligatorio = obj1.flg_obligatorio;
            obj.flg_bloqueo = obj1.flg_bloqueo;
            obj.dsc_empresa = _empresa;
            unit.Sistema.GuardarActualizar_asignacion(obj,3, Program.Sesion.Usuario.cod_usuario);
        }

        private void btnVistaPreliminar_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }
    }
}