using BE_GestionRRHH;
using DevExpress.XtraBars;
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

namespace UI_GestionRRHH.Formularios.Sistema.Configuracion_del_Sistema
{
    public partial class frmMantPerfil : HNG_Tools.ModalForm
    {
        internal enum Perfil
        {
            Nuevo = 0,
            Editar = 1
        }
        private readonly UnitOfWork unit;

        frmAsignacionPermiso frmHandler = new frmAsignacionPermiso();
        internal Perfil MiAccion = Perfil.Nuevo;
        public int cod_perfil = 0;
        //internal string GrupoSeleccionado = "";
        //internal string ItemSeleccionado = "";
        internal string EstadoSeleccionado = "";
        internal string SolucionSeleccionado = "";
        public frmMantPerfil()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            this.TitleBackColor = Program.Sesion.Colores.Verde;
        }

        public frmMantPerfil(frmAsignacionPermiso frm)
        {
            InitializeComponent();
            unit = new UnitOfWork();
            frmHandler = frm;
            this.TitleBackColor = Program.Sesion.Colores.Verde;
        }

        private void Inicializar()
        {
            switch (MiAccion)
            {
                case Perfil.Nuevo:

                    Nuevo();
                    break;
                case Perfil.Editar:

                    Editar();
                    break;
            }

            txtSolucion.Text = SolucionSeleccionado;
        }
        private void frmMantPerfil_Load(object sender, EventArgs e)
        {
            Inicializar();
        }
        public void Nuevo()
        {

            LimpiarCampos();

        }


        public void LimpiarCampos()
        {
            MiAccion = Perfil.Nuevo;
            cod_perfil = 0;
            txtNombrePerfil.Text = "";
            chkActivo.Checked = true;
            chkActivo.Enabled = false;

            picPerfilAnterior.Enabled = false;
            picPerfilSiguiente.Enabled = false;

        }
        public void Editar()
        {
            ePerfil ePer = new ePerfil();
            ePer = unit.Sistema.ObtenerPerfil<ePerfil>(opcion: 4, cod_perfil: cod_perfil, dsc_solucion: SolucionSeleccionado);

            txtNombrePerfil.Text = ePer.dsc_perfil;
            chkActivo.CheckState = ePer.flg_activo == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkActivo.Enabled = true;

            picPerfilAnterior.Enabled = true;
            picPerfilSiguiente.Enabled = true;
        }


        private void picPerfilAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                int tRow = frmHandler.gvPerfiles.RowCount - 1;
                int nRow = frmHandler.gvPerfiles.FocusedRowHandle;
                frmHandler.gvPerfiles.FocusedRowHandle = nRow == tRow ? 0 : nRow - 1;

                ePerfil obj = frmHandler.gvPerfiles.GetFocusedRow() as ePerfil;
                cod_perfil = obj.cod_perfil;
                MiAccion = Perfil.Editar;
                Editar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void picPerfilSiguiente_Click(object sender, EventArgs e)
        {
            try
            {
                int tRow = frmHandler.gvPerfiles.RowCount - 1;
                int nRow = frmHandler.gvPerfiles.FocusedRowHandle;
                frmHandler.gvPerfiles.FocusedRowHandle = nRow == tRow ? 0 : nRow + 1;

                ePerfil obj = frmHandler.gvPerfiles.GetFocusedRow() as ePerfil;
                cod_perfil = obj.cod_perfil;
                MiAccion = Perfil.Editar;
                Editar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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


        private string Guardar()
        {
            string result = "";
            //var ePer = new ePerfil();
            ePerfil ePer = AsignarValoresPerfil();
            ePer = unit.Sistema.Guardar_Actualizar_Perfil<ePerfil>( opcion:1, ePer: ePer, MiAccion: "Nuevo", coduser: "ADMINISTRADOR");
            if (ePer != null)
            {
                cod_perfil = ePer.cod_perfil;
                SolucionSeleccionado = ePer.dsc_solucion;
                result = "OK";
            }

            return result;
        }

        private string Modificar()
        {
            string result = "";
            //var ePer = new ePerfil();
            ePerfil ePer = AsignarValoresPerfil();
            ePer = unit.Sistema.Guardar_Actualizar_Perfil<ePerfil>(1, ePer, "Actualizar", Program.Sesion.Usuario.cod_usuario);

            if (ePer != null)
            {
                cod_perfil = ePer.cod_perfil;
                SolucionSeleccionado = ePer.dsc_solucion;
                result = "OK";
            }

            return result;
        }


        private ePerfil AsignarValoresPerfil()
        {
            ePerfil Eper = new ePerfil();
            Eper.cod_perfil = cod_perfil;
            Eper.dsc_perfil = txtNombrePerfil.Text;
            Eper.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
            Eper.flg_activo = chkActivo.CheckState == CheckState.Checked ? "SI" : "NO";
            Eper.dsc_solucion = txtSolucion.Text.Trim();

            return Eper;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Nuevo();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string Mensaje = "";
                if (txtNombrePerfil.Text == "") { MessageBox.Show("Debe ingresar un nombre para el perfil", "Guardar Perfil", MessageBoxButtons.OK, MessageBoxIcon.Error); txtNombrePerfil.Focus(); return; }



                string result = "";
                switch (MiAccion)
                {
                    case Perfil.Nuevo: result = Guardar(); Mensaje = "Se creo el perfil de manera satisfactoria"; break;
                    case Perfil.Editar: result = Modificar(); Mensaje = "Se actualizo el perfil de manera satisfactoria"; break;
                }

                if (result == "OK")
                {

                    MessageBox.Show(Mensaje, "Guardar Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    int nRow = 0;
                    if (MiAccion == Perfil.Nuevo)
                    {
                        if (SolucionSeleccionado != "")
                        {
                            frmHandler.CargarPerfiles(EstadoSeleccionado, SolucionSeleccionado);
                            for (int x = 0; x <= frmHandler.gvPerfiles.RowCount - 1; x++)
                            {
                                ePerfil obj = frmHandler.gvPerfiles.GetRow(x) as ePerfil;
                                if (obj != null && obj.cod_perfil == cod_perfil) { nRow = x; }
                            }
                            frmHandler.gvPerfiles.FocusedRowHandle = nRow;
                        }

                        MiAccion = Perfil.Editar;
                        chkActivo.Enabled = true;
                        picPerfilAnterior.Enabled = true;
                        picPerfilSiguiente.Enabled = true;
                    }
                    else
                    {
                        if (SolucionSeleccionado != "")
                        {
                            nRow = frmHandler.gvPerfiles.FocusedRowHandle;
                            frmHandler.CargarPerfiles(EstadoSeleccionado, SolucionSeleccionado);
                            frmHandler.gvPerfiles.FocusedRowHandle = nRow;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}