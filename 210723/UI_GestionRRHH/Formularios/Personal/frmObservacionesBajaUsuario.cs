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
    internal enum Cese
    {
        Nuevo = 0,
        Editar = 1,
        Vista = 2
    }
    public partial class frmObservacionesBajaUsuario : HNG_Tools.ModalForm


    //DevExpress.XtraEditors.XtraForm
    {
        private readonly UnitOfWork unit;
        eTrabajador trab = new eTrabajador();
        internal Cese MiAccion = Cese.Nuevo;
        public string empresa, trabajador, estado;
        List<eTrabajador> listTrabajador = new List<eTrabajador>();
        public string ActualizarListado = "NO";
        public frmObservacionesBajaUsuario()
        {
            InitializeComponent();
            unit = new UnitOfWork();
        }

        public frmObservacionesBajaUsuario(frmObservacionesBajaUsuario frm)
        {
            InitializeComponent();
            unit = new UnitOfWork();

        }
        private void validacion()
        {
            if (dtFechaBaja == null)
            {
                emptySpaceItem5.TextVisible = true;
            }
            if (lkpMotivo == null)
            {
                emptySpaceItem8.TextVisible = true;
            }
            if (mmObservaciones == null)
            {
                // emptySpaceItem9.TextVisible = true;
            }
        }


        private void CargarLookUpEdit()
        {
            try
            {
                unit.Trabajador.CargaCombosLookUp("MotivosDespido", lkpMotivo, "cod_parametro", "dsc_parametro", "", valorDefecto: true);
                unit.Trabajador.CargaCombosLookUp("MotivosDespidointerno", lkpMotivointerno, "cod_motivointerno", "dsc_motivointerno", "", valorDefecto: true);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void dtFechaBaja_EditValueChanged(object sender, EventArgs e)
        {
            if (dtFechaBaja == null)
            {
                emptySpaceItem5.TextVisible = true;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (btnGuardar.Text == "GUARDAR")
            {

           
            string result = "";
            try
            {

                if (lkpMotivo.EditValue== null) { MessageBox.Show("Debe ingresar un MOTIVO", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpMotivo.Focus(); return; }
            eTrabajador trab = new eTrabajador();
             trab = AsignarValor();
            result = unit.Trabajador.ActualizarBaja(2, Fechabaja: trab.Fechabaja, motivo_baja: trab.motivo_baja, observaciones: trab.observaciones, cod_empresa: empresa, cod_trabajador: trabajador, flg_activo:estado, trab.motivointerno, trab.blacklist);
            result = unit.Trabajador.ActualizarBaja(4, Fechabaja: trab.Fechabaja, motivo_baja: trab.motivo_baja, observaciones: trab.observaciones, cod_empresa: empresa, cod_trabajador: trabajador, flg_activo: "SI", trab.motivointerno, trab.blacklist);
            if (result != "OK") { MessageBox.Show("Error al Inactivar registro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (result == "OK") MessageBox.Show("Se procedió a Inactivar el registro de manera satisfactoria.", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ActualizarListado = "SI";

            this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            }else if (btnGuardar.Text == "ACEPTAR")
            {
                this.Close();
            }

        }

        private void frmObservacionesBajaUsuario_Load(object sender, EventArgs e)
        {
            Inicializar();
           
            //validacion();
        }

        public eTrabajador AsignarValor()
        {
            eTrabajador obj = new eTrabajador();
            obj.Fechabaja = Convert.ToDateTime(dtFechaBaja.EditValue.ToString());
            obj.motivo_baja = lkpMotivo.EditValue.ToString();
            obj.observaciones = mmObservaciones.Text;
            obj.cod_trabajador = trabajador;
            obj.cod_empresa = empresa;
            obj.motivointerno=lkpMotivointerno.EditValue.ToString();
            obj.blacklist= chkblacklist.CheckState == CheckState.Checked ? "SI" : "NO";


            return obj;
        }
        private void Nuevo()
        {
            
            dtFechaBaja.EditValue = DateTime.Today;
            CargarLookUpEdit();
        }
        private void Editar()
        {
            if (estado == "SI")
            {
                string result = "";
                eTrabajador trab = new eTrabajador();

                result = unit.Trabajador.ActualizarBaja(3, Fechabaja: trab.Fechabaja, motivo_baja: trab.motivo_baja, observaciones: trab.observaciones, cod_empresa: empresa, cod_trabajador: trabajador, flg_activo: estado); 
                if (result != "OK") { MessageBox.Show("Error al Activar registro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (result == "OK") { MessageBox.Show("Se procedió a Activar Trabajador", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information); this.Close(); }
            }
            CargarLookUpEdit();
            ObtenerDatos_Trabajador();
        }
        private void Inicializar()
        {
            try
            {
                switch (MiAccion)
                {
                    case Cese.Nuevo:
                        Nuevo();
                        break;
                    case Cese.Editar:
                        Editar();
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void ObtenerDatos_Trabajador()
        {
            trab = unit.Trabajador.Obtener_Trabajador<eTrabajador>(2, trabajador, empresa);
            lkpMotivo.EditValue = trab.motivo_baja.ToString();
            lkpMotivo.ReadOnly = true;
            dtFechaBaja.EditValue = trab.Fechabaja;
            dtFechaBaja.ReadOnly = true;
            mmObservaciones.Text = trab.observaciones;
            mmObservaciones.ReadOnly = true;
            btnGuardar.Text = "ACEPTAR";
        }

        private void chkblacklist_CheckedChanged(object sender, EventArgs e)
        {
            if (chkblacklist.Checked == true) { chkblacklist.Enabled = true; HNG.MessageWarning("¿Seguro que desea pasar al trabajador a la lista Negra? , esta acción no es reversible", "LISTA NEGRA"); } else if (chkblacklist.Checked == false) { chkblacklist.EditValue = null; }

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            string respuesta = "";
            if (keyData == Keys.Enter)
            {

                guardarcese();

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void guardarcese()
        {
            if (btnGuardar.Text == "GUARDAR")
            {


                string result = "";
                try
                {

                    if (lkpMotivo.EditValue == null) { MessageBox.Show("Debe ingresar un MOTIVO", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning); lkpMotivo.Focus(); return; }
                    eTrabajador trab = new eTrabajador();
                    trab = AsignarValor();
                    result = unit.Trabajador.ActualizarBaja(2, Fechabaja: trab.Fechabaja, motivo_baja: trab.motivo_baja, observaciones: trab.observaciones, cod_empresa: empresa, cod_trabajador: trabajador, flg_activo: estado,trab.motivointerno,trab.blacklist);
                    result = unit.Trabajador.ActualizarBaja(4, Fechabaja: trab.Fechabaja, motivo_baja: trab.motivo_baja, observaciones: trab.observaciones, cod_empresa: empresa, cod_trabajador: trabajador, flg_activo: "SI");
                    if (result != "OK") { MessageBox.Show("Error al Inactivar registro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    if (result == "OK") MessageBox.Show("Se procedió a Inactivar el registro de manera satisfactoria.", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ActualizarListado = "SI";

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else if (btnGuardar.Text == "ACEPTAR")
            {
                this.Close();
            }
        }
    }
}
