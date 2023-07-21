using BE_GestionRRHH;
using DevExpress.XtraEditors;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_GestionRRHH.Formularios.Sistema.Configuracion_del_Sistema
{
    public partial class frmMantVentana : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;
        internal enum Ventana
        {
            Nuevo = 0,
            Editar = 1
        }

        readonly frmOpcionesSistema frmHandler;// = new frmOpcionesSistema();
        internal Ventana MiAccion = Ventana.Nuevo;
        public int cod_ventana = 0;
        //internal string GrupoSeleccionado = "";
        //internal string ItemSeleccionado = "";
        internal string EstadoSeleccionado = "";
        internal string SolucionSeleccionado = "";
        public frmMantVentana()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            this.TitleBackColor = Program.Sesion.Colores.Verde;
        }
        public frmMantVentana(frmOpcionesSistema frm)
        {
            InitializeComponent();
            frmHandler = frm;
            unit = new UnitOfWork();
            this.TitleBackColor = Program.Sesion.Colores.Verde;
        }
        private void Inicializar()
        {
            switch (MiAccion)
            {
                case Ventana.Nuevo:
                    CargarCombos();
                    Nuevo();
                    break;
                case Ventana.Editar:
                    CargarCombos();
                    Editar();
                    break;
            }

            txtSolucion.Text = SolucionSeleccionado;
        }

        private void frmMantVentana_Load(object sender, EventArgs e)
        {

            Inicializar();
        }
        public void CargarCombos()
        {
            unit.Sistema.CargaCombosLookUp("Modulos", lkpModulo, "codvar", "desvar1", dsc_solucion: SolucionSeleccionado);
            lkpModulo.ItemIndex = -1;
            lkpModulo.EditValue = null;
        }


        public void Nuevo()
        {

            LimpiarCampos();

        }

        public void Editar()
        {

            eVentana eVen = new eVentana();
            eVen = unit.Sistema.ObtenerVentana<eVentana>(2, cod_ventana, SolucionSeleccionado);
            txtNombreVentana.Text = eVen.dsc_ventana;
            chkActivo.CheckState = eVen.flg_activo == "SI" ? CheckState.Checked : CheckState.Unchecked;
            txtMenu.Text = eVen.dsc_menu;
            lkpModulo.EditValue = eVen.cod_grupo;
            txtFormulario.Text = eVen.dsc_formulario;
            txtNumOrden.Text = eVen.num_orden.ToString();
            //chkActivo.Enabled = true;
            chkActivo.Enabled = eVen.flg_activo == "SI" ? true : false;

            picAnteriorVentana.Enabled = true;
            picSiguienteVentana.Enabled = true;
        }
        public void LimpiarCampos()
        {
            MiAccion = Ventana.Nuevo;
            cod_ventana = 0;
            txtNombreVentana.Text = "";
            chkActivo.Checked = true;
            lkpModulo.ItemIndex = -1;
            lkpModulo.EditValue = null;
            txtMenu.Text = "";
            txtFormulario.Text = "";
            txtNumOrden.Text = "0";
            chkActivo.Enabled = false;

            picAnteriorVentana.Enabled = false;
            picSiguienteVentana.Enabled = false;

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string Mensaje = "";
                if (txtNombreVentana.Text == "") { MessageBox.Show("Debe ingresar un nombre para la ventana", "Guardar Ventana", MessageBoxButtons.OK, MessageBoxIcon.Error); txtNombreVentana.Focus(); return; }
                if (lkpModulo.EditValue.ToString() == "") { MessageBox.Show("Debe seleccionar un módulo", "Guardar Ventana", MessageBoxButtons.OK, MessageBoxIcon.Error); lkpModulo.Focus(); return; }
                if (txtFormulario.Text == "") { MessageBox.Show("Debe ingresar el formulario al que pertenece", "Guardar Ventana", MessageBoxButtons.OK, MessageBoxIcon.Error); txtFormulario.Focus(); return; }
                if (txtMenu.Text == "") { MessageBox.Show("Debe ingresar ela opción del menú al que pertenece", "Guardar Ventana", MessageBoxButtons.OK, MessageBoxIcon.Error); txtMenu.Focus(); return; }

                string result = "";
                switch (MiAccion)
                {
                    case Ventana.Nuevo: result = Guardar(); Mensaje = "Se creo la ventana de manera satisfactoria"; break;
                    case Ventana.Editar: result = Modificar(); Mensaje = "Se actualizo la ventana de manera satisfactoria"; break;
                }

                if (result == "OK")
                {

                    MessageBox.Show(Mensaje, "Guardar Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    int nRow = 0;
                    if (MiAccion == Ventana.Nuevo)
                    {
                        //if (GrupoSeleccionado != "")
                        if (SolucionSeleccionado != "")
                        {
                            frmHandler.CargarListadoVentanas(EstadoSeleccionado, SolucionSeleccionado);
                            for (int x = 0; x <= frmHandler.gvVentana.RowCount - 1; x++)
                            {
                                eVentana obj = frmHandler.gvVentana.GetRow(x) as eVentana;
                                if (obj != null && obj.cod_ventana == cod_ventana) { nRow = x; }
                            }
                            frmHandler.gvVentana.FocusedRowHandle = nRow;
                        }

                        MiAccion = Ventana.Editar;
                        chkActivo.Enabled = true;
                        picAnteriorVentana.Enabled = true;
                        picSiguienteVentana.Enabled = true;
                    }
                    else
                    {
                        if (SolucionSeleccionado != "")
                        {
                            nRow = frmHandler.gvVentana.FocusedRowHandle;
                            frmHandler.CargarListadoVentanas(EstadoSeleccionado, SolucionSeleccionado);
                            frmHandler.gvVentana.FocusedRowHandle = nRow;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string Guardar()
        {
            string result = "";
            //var eVen = new eVentana();
            eVentana eVen = AsignarValoresVentana();
            eVen = unit.Sistema.Guardar_Actualizar_Ventana<eVentana>(1, eVen, "Nuevo", Program.Sesion.Usuario.cod_usuario);
            if (eVen != null)
            {
                cod_ventana = eVen.cod_ventana;
                SolucionSeleccionado = eVen.dsc_solucion;
                result = "OK";
            }

            return result;
        }

        private string Modificar()
        {
            string result = "";
           // var eVen = new eVentana();
            eVentana eVen = AsignarValoresVentana();
            eVen = unit.Sistema.Guardar_Actualizar_Ventana<eVentana>(1, eVen, "Actualizar", Program.Sesion.Usuario.cod_usuario);

            if (eVen != null)
            {
                cod_ventana = eVen.cod_ventana;
                SolucionSeleccionado = eVen.dsc_solucion;
                result = "OK";
            }

            return result;
        }

        private eVentana AsignarValoresVentana()
        {
            eVentana eVen = new eVentana();
            eVen.cod_ventana = cod_ventana;
            eVen.dsc_solucion = txtSolucion.Text.Trim();
            eVen.dsc_ventana = txtNombreVentana.Text;
            eVen.dsc_menu = txtMenu.Text;
            eVen.cod_grupo = Convert.ToInt32(lkpModulo.EditValue);
            eVen.dsc_formulario = txtFormulario.Text;
            eVen.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            eVen.flg_activo = chkActivo.CheckState == CheckState.Checked ? "SI" : "NO";
            eVen.num_orden = Convert.ToInt32(txtNumOrden.Text);

            return eVen;
        }

        private void chkActivo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkActivo.Checked == false)
            {
                this.layoutActivo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                this.layoutActivo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Nuevo();
        }

        private void picAnteriorVentana_Click(object sender, EventArgs e)
        {
            try
            {
                int tRow = frmHandler.gvVentana.RowCount - 1;
                int nRow = frmHandler.gvVentana.FocusedRowHandle;
                frmHandler.gvVentana.FocusedRowHandle = nRow == tRow ? 0 : nRow - 1;

                eVentana obj = frmHandler.gvVentana.GetFocusedRow() as eVentana;
                cod_ventana = obj.cod_ventana;
                MiAccion = Ventana.Editar;
                Editar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void picSiguienteVentana_Click(object sender, EventArgs e)
        {
            try
            {
                int tRow = frmHandler.gvVentana.RowCount - 1;
                int nRow = frmHandler.gvVentana.FocusedRowHandle;
                frmHandler.gvVentana.FocusedRowHandle = nRow == tRow ? 0 : nRow + 1;

                eVentana obj = frmHandler.gvVentana.GetFocusedRow() as eVentana;
                cod_ventana = obj.cod_ventana;
                MiAccion = Ventana.Editar;
                Editar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}