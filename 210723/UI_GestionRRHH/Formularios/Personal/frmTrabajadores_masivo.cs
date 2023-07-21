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
namespace UI_GestionRRHH.Formularios.Personal
{
    public partial class frmTrabajadores_masivo : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;
        public string cod_empresa, cod_sede_empresa;
        List<eTrabajador> ListTrabajadores = new List<eTrabajador>();
        public List<eTrabajador> ListaSeleccionada = new List<eTrabajador>();
        

        public frmTrabajadores_masivo()
        {
            InitializeComponent();
            configurar_formulario();
            unit = new UnitOfWork();
            CargarLookUpEdit();
            ObtenerTrabajadores();
        }

        private void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;
          //  unit.Globales.ConfigurarGridView_ClasicStyle(gcTrabajador, gvTrabajador);

        }

        private void CargarLookUpEdit()
        {
            try
            {
                unit.Trabajador.CargaCombosLookUp("EmpresaUsuario", lkpEmpresa, "cod_empresa", "dsc_empresa", "", valorDefecto: true, cod_usuario: Program.Sesion.Usuario.cod_usuario);
                lkpEmpresa.EditValue = "00001";
                if (Program.Sesion.EmpresaList.Count == 1) lkpEmpresa.EditValue = Program.Sesion.EmpresaList[0].cod_empresa;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void lkpEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            ObtenerTrabajadores();
        }

        private void gvTrabajador_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvTrabajador_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);

        }

        private void gvTrabajador_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle >= 0) gvTrabajador_FocusedRowChanged(gvTrabajador, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, e.RowHandle));

        }

        private void gvTrabajador_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0) { }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            eTrabajador obj = gvTrabajador.GetFocusedRow() as eTrabajador;
            string cod_trabajador = "";
            for (int i = 0; i < gvTrabajador.SelectedRowsCount; i++)
            {
                ListaSeleccionada.Add(new eTrabajador() { cod_trabajador=obj.cod_trabajador});

            }
            Close();
            
        }

        private void ObtenerTrabajadores()
        {
            ListTrabajadores.Clear();
            ListTrabajadores = unit.Trabajador.ListarTrabajadoresvista<eTrabajador>(opcion: 9, cod_trabajador: "", cod_empresa: lkpEmpresa.EditValue.ToString());
            bsEmpresa.DataSource = ListTrabajadores; gvTrabajador.RefreshData();

        }

    }
}