﻿using BE_GestionRRHH;
using DevExpress.XtraBars;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_GestionRRHH.Formularios.Shared;

namespace UI_GestionRRHH.Formularios.Sistema.Configuracion_del_Sistema
{
    public partial class frmMantUsuario : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;

        internal enum Usuario
        {
            Nuevo = 0,
            Editar = 1
        }

        frmListadoUsuario frmHandler = new frmListadoUsuario();
        //public eUsuario user = new eUsuario();
        public eTrabajador eTrab = new eTrabajador();
        internal Usuario MiAccion = Usuario.Nuevo;
        //public int[] colorVerde, colorPlomo, colorEventRow, colorFocus;
        List<eUsuario_Empresas> ListEmpresasUsuarios = new List<eUsuario_Empresas>();
        List<eSolucionUsuario_Consulta> ListSolucionUsuario = new List<eSolucionUsuario_Consulta>();
        public string cod_usuario = "";
        public string ActualizarListado = "NO";

        //public string GrupoSeleccionado = "";
        //public string ItemSeleccionado = "";
        internal string EstadoSeleccionado = "";
        internal string SolucionSeleccionado = "";
        internal string PerfilSeleccionado = "";
        public frmMantUsuario()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            configurar_formulario();
        }
        public frmMantUsuario(frmListadoUsuario frm)
        {
            InitializeComponent();
            frmHandler = frm;
            unit = new UnitOfWork();
            configurar_formulario();
        }
        private void configurar_formulario()
        {
            //unit.Globales.ConfigurarGridView_ClasicStyle(gc: gcListadoSolucion, gv: gvListadoSolucion);
            //gvListadoSolucion.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;

            unit.Globales.ConfigurarGridView_ClasicStyle(gc: gcPerfilAsignado, gv: gvPerfilAsignado);
            gvPerfilAsignado.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;

            unit.Globales.ConfigurarGridView_ClasicStyle(gc: gcPerfilDisponible, gv: gvPerfilDisponible);
            gvPerfilDisponible.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;

            this.TitleBackColor = Program.Sesion.Colores.Verde;
        }
        private void Inicializar()
        {
            switch (MiAccion)
            {
                case Usuario.Nuevo:
                    CargarCombos();
                    Nuevo();
                    ObtenerListadoEmpresas();
                    ObtenerListadoSoluciones();
                    CargarComboboxSolucionesAsignadas();
                    break;
                case Usuario.Editar:
                    CargarCombos();
                    Editar();
                    ObtenerListadoEmpresas();
                    ObtenerListadoSoluciones();
                    CargarComboboxSolucionesAsignadas();
                    break;
            }
        }
        private void CargarComboboxSolucionesAsignadas()
        {
            List<eSolucionUsuario_Consulta> usuarioSolucion = unit.Sistema.ListarSolucion<eSolucionUsuario_Consulta>(opcion: 1, cod_usuario: cod_usuario);
            lkpSolucion.Properties.DataSource = null;
            lkpSolucion.Properties.DataSource = usuarioSolucion.Where((a) => a.flg_acceso).ToList();
            lkpSolucion.Properties.DisplayMember = "dsc_solucion";
            lkpSolucion.Properties.ValueMember = "cod_solucion";

            var obj = usuarioSolucion.FirstOrDefault((c) => c.dsc_solucion.Equals(SolucionSeleccionado));
            if (obj != null)
                lkpSolucion.EditValue = obj.cod_solucion;
            else
                lkpSolucion.ItemIndex = 0;
        }
        private void frmMantUsuario_Load(object sender, EventArgs e)
        {
            Inicializar();
            groupControl4.AppearanceCaption.ForeColor = Program.Sesion.Colores.Verde; //Color.FromArgb(colorVerde[0], colorVerde[1], colorVerde[2]);
            groupControl2.AppearanceCaption.ForeColor = Program.Sesion.Colores.Verde; //Color.FromArgb(colorVerde[0], colorVerde[1], colorVerde[2]);
            groupControl3.AppearanceCaption.ForeColor = Program.Sesion.Colores.Verde; //Color.FromArgb(colorVerde[0], colorVerde[1], colorVerde[2]);
        }


        public void CargarCombos()
        {
            unit.Usuario.CargaCombosLookUp("DominioCorreo", lkpDominioCorreo, "codvar", "desvar1", "0");
        }

        private void Editar()
        {
            eUsuario eUsu = new eUsuario();
            eUsu = unit.Usuario.ObtenerUsuario<eUsuario>(2, cod_usuario);
            txtCodigoUsuario.Text = eUsu.cod_usuario;
            chkActivo.CheckState = eUsu.flg_activo == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkAdministrador.CheckState = eUsu.flg_administrador == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkTrabajador.CheckState = eUsu.flg_trabajador == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkNoExpira.CheckState = eUsu.flg_noexpira == "SI" ? CheckState.Checked : CheckState.Unchecked;
            chkGuardarAuditoria.CheckState = eUsu.flg_audita_login == "SI" ? CheckState.Checked : CheckState.Unchecked;
            txtClave.Text = eUsu.dsc_clave;
            txtNombreUsuario.Text = eUsu.dsc_usuario;
            txtCodTrabajador.Text = eUsu.cod_trabajador;
            txtTrabajador.Text = eUsu.dsc_trabajador;
            txtCorreo.Text = "";
            lkpDominioCorreo.EditValue = null;
            lkpDominioCorreo.ItemIndex = -1;
            txtClaveCorreo.Text = eUsu.dsc_contraseña;
            txtNumDiasCaduca.Text = eUsu.num_dias_cambio_contraseña.ToString();

            if (Convert.ToDateTime(eUsu.fch_cambioclave).Year == 1) { dtFechaCambioClave.EditValue = null; } else { dtFechaCambioClave.EditValue = Convert.ToDateTime(eUsu.fch_cambioclave); }
            if (Convert.ToDateTime(eUsu.fch_cambio).Year == 1) { dtFechaModificacion.EditValue = null; } else { dtFechaModificacion.EditValue = Convert.ToDateTime(eUsu.fch_cambio); }
            if (Convert.ToDateTime(eUsu.fch_registro).Year == 1) { dtFechaRegistro.EditValue = null; } else { dtFechaRegistro.EditValue = Convert.ToDateTime(eUsu.fch_registro); }

            txtUsuarioRegistro.Text = eUsu.dsc_usuario_registro;
            txtUsuarioCambio.Text = eUsu.dsc_usuario_cambio;

            if (eUsu.dsc_correo != null)
            {
                if (eUsu.dsc_correo != "")
                {
                    string[] words = eUsu.dsc_correo.Split('@');
                    txtCorreo.Text = words[0];
                    lkpDominioCorreo.EditValue = words[1];// == "refriperu.com.pe" ? "1" : "2";
                }
            }





            chkActivo.Enabled = true;
            txtCodigoUsuario.ReadOnly = true;
            picAnteriorUsuario.Enabled = true;
            picSiguienteUsuario.Enabled = true;

            CargarPerfilDisponible();
            CargarPerfilAsignados();
        }

        private void ObtenerListadoSoluciones()
        {
            var solucion = unit.Sistema.ListarSolucion<eSolucion>(opcion: 4);
            foreach (var sol in solucion)
            {
                ListSolucionUsuario.Add(new eSolucionUsuario_Consulta()
                {
                    cod_solucion = sol.cod_solucion,
                    dsc_solucion = sol.dsc_solucion,
                    dsc_texto = sol.dsc_texto,
                    dsc_ruta_solucion = sol.dsc_ruta_solucion,
                    dsc_icono = sol.dsc_icono,
                    dsc_version = sol.dsc_version,
                    dsc_key_encrypted = sol.dsc_key_encrypted,
                    num_orden = 0,
                    dsc_rgb_btnIconColor = sol.dsc_ruta_solucion,
                    flg_acceso = false
                });
            }

            bsListadoSolucion.DataSource = null; bsListadoSolucion.DataSource = ListSolucionUsuario.ToList();
            if (MiAccion == Usuario.Editar)
            {
                List<eSolucionUsuario_Consulta> usuarioSolucion = unit.Sistema.ListarSolucion<eSolucionUsuario_Consulta>(opcion: 1, cod_usuario: cod_usuario);
                foreach (var solu in ListSolucionUsuario)
                {
                    foreach (var flag in usuarioSolucion)
                    {
                        if (flag.cod_solucion.Equals(solu.cod_solucion) && flag.flg_acceso)
                        {
                            solu.flg_acceso = true; break;
                        }
                    }
                }
                //foreach (var sol in usuarioSolucion)
                //{
                //    eSolucionUsuario_Consulta oSol = ListSolucionUsuario.Find((x) => x.cod_solucion.Equals(sol.cod_solucion));
                //    if (oSol != null) oSol.flg_acceso = oSol.flg_acceso;
                //}
            }
            gvListadoSolucion.RefreshData();
        }

        private void ObtenerListadoEmpresas()
        {
            ListEmpresasUsuarios = unit.Usuario.ListarEmpresas<eUsuario_Empresas>(5, cod_usuario);
            bsEmpresasVinculadas.DataSource = null; bsEmpresasVinculadas.DataSource = ListEmpresasUsuarios;

            if (MiAccion == Usuario.Editar)
            {
                List<eUsuario_Empresas> lista = unit.Usuario.ListarEmpresas<eUsuario_Empresas>(4, cod_usuario);
                foreach (eUsuario_Empresas obj in lista)
                {
                    eUsuario_Empresas oUserEmp = ListEmpresasUsuarios.Find(x => x.cod_empresa == obj.cod_empresa);
                    if (oUserEmp != null) oUserEmp.Seleccionado = true;
                }
            }
            gvEmpresasVinculadas.RefreshData();
        }

        public void CargarPerfilDisponible()
        {

            List<ePerfil> ListadoPerfilesDisponible = new List<ePerfil>();
            ListadoPerfilesDisponible = unit.Usuario.ListarPerfiles<ePerfil>(3, cod_usuario: cod_usuario, dsc_solucion: SolucionSeleccionado);
            bsPerfilesDisponible.DataSource = null; bsPerfilesDisponible.DataSource = ListadoPerfilesDisponible;
        }

        public void CargarPerfilAsignados()
        {

            List<ePerfil> ListadoPerfilesAsignados = new List<ePerfil>();
            ListadoPerfilesAsignados = unit.Usuario.ListarPerfiles<ePerfil>(2, cod_usuario: cod_usuario, dsc_solucion: SolucionSeleccionado);
            bsPerfilesAsignados.DataSource = null; bsPerfilesAsignados.DataSource = ListadoPerfilesAsignados;
        }
        private void Nuevo()
        {
            LimpiarCamposUsuario();

        }

        private void picSiguienteUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                int tRow = frmHandler.gvListaUsuario.RowCount - 1;
                int nRow = frmHandler.gvListaUsuario.FocusedRowHandle;
                frmHandler.gvListaUsuario.FocusedRowHandle = nRow == tRow ? 0 : nRow + 1;

                eUsuario obj = frmHandler.gvListaUsuario.GetFocusedRow() as eUsuario;
                cod_usuario = obj.cod_usuario;
                MiAccion = Usuario.Editar;
                Editar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void picAnteriorUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                int tRow = frmHandler.gvListaUsuario.RowCount - 1;
                int nRow = frmHandler.gvListaUsuario.FocusedRowHandle;
                frmHandler.gvListaUsuario.FocusedRowHandle = nRow == 0 ? tRow : nRow - 1;

                eUsuario obj = frmHandler.gvListaUsuario.GetFocusedRow() as eUsuario;
                cod_usuario = obj.cod_usuario;
                MiAccion = Usuario.Editar;
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
                this.layoutEstado.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                this.layoutEstado.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }



        public void GrabarPerfiles()
        {

            string codperfiles = "";
            for (int x = 0; x <= gvPerfilAsignado.RowCount - 1; x++)
            {
                ePerfil obj = gvPerfilAsignado.GetRow(x) as ePerfil;
                codperfiles = codperfiles + obj.cod_perfil.ToString() + ',';
            }

            unit.Usuario.Guardar_PerfilUsuario<eUsuario>(1, cod_usuario, codperfiles, Program.Sesion.Usuario.cod_usuario, SolucionSeleccionado);
        }

        private void GrabarEmpresasVinculadas()
        {
            gvEmpresasVinculadas.RefreshData();

            for (int x = 0; x <= gvEmpresasVinculadas.RowCount - 1; x++)
            {
                eUsuario_Empresas eUserEmp = new eUsuario_Empresas();
                eUsuario_Empresas obj = gvEmpresasVinculadas.GetRow(x) as eUsuario_Empresas;
                if (obj.Seleccionado)
                {
                    obj.cod_usuario = cod_usuario;
                    eUserEmp = unit.Usuario.Guardar_Eliminar_UsuarioEmpresas<eUsuario_Empresas>(obj);
                    if (eUserEmp == null) { MessageBox.Show("Error al vincular empresa", "Vincular empresa", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                }
            }
        }
        private void GrabarSolucionesVinculadas()
        {
            gvListadoSolucion.RefreshData();

            for (int x = 0; x <= gvListadoSolucion.RowCount - 1; x++)
            {
                eSolucionUsuario_Consulta eSol = new eSolucionUsuario_Consulta();
                eSolucionUsuario_Consulta obj = gvListadoSolucion.GetRow(x) as eSolucionUsuario_Consulta;
                if (obj.flg_acceso)
                {
                    //obj.cod_usuario = cod_usuario;
                    var result = unit.Sistema.Guardar_Eliminar_UsuarioSolucion<eSolucionUsuario>(new eSolucionUsuario()
                    {
                        cod_usuario = cod_usuario,
                        cod_solucion = obj.cod_solucion,
                        cod_usuario_registro = Program.Sesion.Usuario.cod_usuario
                    }, obj.flg_acceso);
                    if (result == null) { MessageBox.Show("Error al vincular solución", "Vincular solución", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                }
            }

           
        }

        private string Guardar()
        {
            string result = "";
            eUsuario eUsu = AsignarValoresUsuario();
            eUsu = unit.Usuario.Guardar_Actualizar_Usuario<eUsuario>(eUsu, "Nuevo", Program.Sesion.Usuario.cod_usuario);
            if (eUsu != null)
            {
                if (eUsu.cod_usuario == "@CodRepetido")
                {
                    result = "REPETIDO";
                }
                else
                {
                    cod_usuario = eUsu.cod_usuario;
                    txtCodigoUsuario.Text = cod_usuario;
                    result = "OK";
                }

            }

            return result;
        }

        private string Modificar()
        {
            string result = "";
            eUsuario eUsu = AsignarValoresUsuario();
            eUsu = unit.Usuario.Guardar_Actualizar_Usuario<eUsuario>(eUsu, "Actualizar", Program.Sesion.Usuario.cod_usuario);

            if (eUsu != null)
            {
                cod_usuario = eUsu.cod_usuario;
                result = "OK";
            }

            return result;
        }


        private eUsuario AsignarValoresUsuario()
        {
            eUsuario eUsu = new eUsuario();
            eUsu.cod_usuario = txtCodigoUsuario.Text;
            eUsu.dsc_usuario = txtNombreUsuario.Text;
            eUsu.dsc_clave = txtClave.Text;
            eUsu.flg_activo = chkActivo.CheckState == CheckState.Checked ? "SI" : "NO";
            eUsu.flg_noexpira = chkNoExpira.CheckState == CheckState.Checked ? "SI" : "NO"; ;
            eUsu.flg_administrador = chkAdministrador.CheckState == CheckState.Checked ? "SI" : "NO";
            eUsu.flg_trabajador = chkTrabajador.CheckState == CheckState.Checked ? "SI" : "NO";
            eUsu.flg_audita_login = chkGuardarAuditoria.CheckState == CheckState.Checked ? "SI" : "NO";

            eUsu.cod_usuariobd = "userclient";
            eUsu.dsc_clavebd = "";
            eUsu.cod_trabajador = txtCodTrabajador.Text;
            eUsu.dsc_ruta_firma = "";

            if (txtCorreo.Text != "")
            {
                eUsu.dsc_correo = txtCorreo.Text + "@" + lkpDominioCorreo.EditValue.ToString();
            }
            else
            {
                eUsu.dsc_correo = "";
            }
            eUsu.dsc_contraseña = txtClaveCorreo.Text;
            eUsu.num_dias_cambio_contraseña = Convert.ToInt16(txtNumDiasCaduca.Text);
            eUsu.fch_cambioclave = Convert.ToDateTime(dtFechaCambioClave.EditValue);

            return eUsu;
        }

        private void btnNuevo_ItemClick(object sender, ItemClickEventArgs e)
        {
            MiAccion = Usuario.Nuevo;
            LimpiarCamposUsuario();
        }

        private void LimpiarCamposUsuario()
        {
            MiAccion = Usuario.Nuevo;
            cod_usuario = "";
            txtCodigoUsuario.Text = "";
            chkActivo.CheckState = CheckState.Checked;
            chkAdministrador.CheckState = CheckState.Unchecked;
            chkTrabajador.CheckState = CheckState.Unchecked;
            chkNoExpira.CheckState = CheckState.Unchecked;


            txtClave.Text = "";
            txtCorreo.Text = "";
            lkpDominioCorreo.EditValue = null;
            lkpDominioCorreo.ItemIndex = -1;
            txtNombreUsuario.Text = "";

            txtCodTrabajador.Text = "";


            chkGuardarAuditoria.Checked = true;

            txtUsuarioRegistro.Text = "";
            txtUsuarioCambio.Text = "";
            txtNumDiasCaduca.Text = "60";
            dtFechaCambioClave.EditValue = DateTime.Today.AddDays(Convert.ToInt32(txtNumDiasCaduca.Text));
            txtClaveCorreo.Text = "";
            // dtFechaCambioClave.EditValue = null;
            dtFechaRegistro.EditValue = null;
            dtFechaModificacion.EditValue = null;

            chkActivo.Enabled = false;
            txtCodigoUsuario.ReadOnly = false;
            this.layoutEstado.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            picAnteriorUsuario.Enabled = false;
            picSiguienteUsuario.Enabled = false;

            bsPerfilesAsignados.DataSource = null;
            List<ePerfil> ListadoPerfilesDisponible = new List<ePerfil>();
            ListadoPerfilesDisponible = unit.Usuario.ListarPerfiles<ePerfil>(1, cod_usuario: "", dsc_solucion: SolucionSeleccionado);
            bsPerfilesDisponible.DataSource = null; bsPerfilesDisponible.DataSource = ListadoPerfilesDisponible;

            ListEmpresasUsuarios.Clear();
            ListEmpresasUsuarios = unit.Usuario.ListarEmpresas<eUsuario_Empresas>(5, cod_usuario);
            bsEmpresasVinculadas.DataSource = null; bsEmpresasVinculadas.DataSource = ListEmpresasUsuarios;
        }

        private void btnAsignarPerfil_Click(object sender, EventArgs e)
        {
            int valor = 0;
            foreach (int nRow in gvPerfilDisponible.GetSelectedRows())
            {

                ePerfil obj = gvPerfilDisponible.GetRow(nRow - valor) as ePerfil;
                bsPerfilesAsignados.Add(obj);
                bsPerfilesDisponible.Remove(obj);
                valor = valor + 1;
            }



        }

        private void btnQuitarPerfil_Click(object sender, EventArgs e)
        {
            int valor = 0;
            foreach (int nRow in gvPerfilAsignado.GetSelectedRows())
            {

                ePerfil obj = gvPerfilAsignado.GetRow(nRow - valor) as ePerfil;
                bsPerfilesDisponible.Add(obj);
                bsPerfilesAsignados.Remove(obj);
                valor = valor + 1;
            }
        }

        private void btnBuscarTrabajador_Click(object sender, EventArgs e)
        {
            Busqueda("", "Trabajador");

            //frmBusquedaTrabajador frm = new frmBusquedaTrabajador();
            //frm.opcion = 1;
            //eTrab  = null;
            //frm.ShowDialog();
            //if (frm.eTrab.cod_trabajador != null) { 
            //    eTrab = frm.eTrab;
            //    txtCodTrabajador.Text = eTrab.cod_trabajador;
            //    if (txtNombreUsuario.Text == "") { txtNombreUsuario.Text = eTrab.dsc_nombres_completos;  } 
            //    if (txtCodTrabajador.Text == "") { chkTrabajador.Checked = false; } else { chkTrabajador.Checked = true; }
            //}
        }
        public void Busqueda(string dato, string tipo)
        {
            frmBusquedas frm = new frmBusquedas();
            //frm.user = user;
            frm.filtro = dato;
            //frm.colorVerde = colorVerde;
            //frm.colorPlomo = colorPlomo;
            //frm.colorEventRow = colorEventRow;
            //frm.colorFocus = colorFocus;
            switch (tipo)
            {
                case "Trabajador":
                    frm.entidad = frmBusquedas.MiEntidad.Trabajador;
                    frm.filtro = dato;
                    break;
            }
            frm.ShowDialog();
            if (frm.codigo == "" || frm.codigo == null) { return; }
            switch (tipo)
            {
                case "Trabajador":
                    txtCodTrabajador.Text = frm.codigo;
                    txtTrabajador.Text = frm.descripcion;
                    //eTrabajador.eInfoLaboral_Trabajador obj = new eTrabajador.eInfoLaboral_Trabajador();
                    //obj = blTrab.Obtener_Trabajador<eTrabajador.eInfoLaboral_Trabajador>(5, frm.codigo);
                    //txtUbicacion.Text = obj.dsc_empresa + " - " + obj.dsc_sede_empresa;
                    //txtUbicacion.Tag = obj.cod_sede_empresa;
                    //cod_empresa = obj.cod_empresa; cod_sede_empresa = obj.cod_sede_empresa;
                    break;
            }
        }

        private void chkTrabajador_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTrabajador.Checked == false)
            {
                txtCodTrabajador.Text = "";
            }
        }

        private void gvEmpresasVinculadas_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (MiAccion == Usuario.Nuevo) return;
                eUsuario_Empresas eUserEmp = new eUsuario_Empresas();
                if (e.Column.FieldName == "Seleccionado" && e.RowHandle >= 0)
                {
                    eUsuario_Empresas obj = gvEmpresasVinculadas.GetRow(e.RowHandle) as eUsuario_Empresas;
                    //if (!obj.Seleccionado)
                    //{
                    //    List<eUsuario_Empresas> lista = unit.Usuario.ListarEmpresas<eUsuario_Empresas>(6, cod_usuario, obj.cod_empresa);
                    //    if (lista.Count > 0) { MessageBox.Show("La empresa se encuentra vinculada a uno o más proveedores.", "Desvincular empresa", MessageBoxButtons.OK, MessageBoxIcon.Error); ObtenerListadoEmpresas(); return; }
                    //}
                    obj.cod_usuario = cod_usuario;
                    eUserEmp = unit.Usuario.Guardar_Eliminar_UsuarioEmpresas<eUsuario_Empresas>(obj);
                    if (eUserEmp == null) { MessageBox.Show("Error al vincular empresa", "Vincular empresa", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void rchkSeleccionado_CheckStateChanged(object sender, EventArgs e)
        {
            gvEmpresasVinculadas.PostEditor();
        }

        private void gvEmpresasVinculadas_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvEmpresasVinculadas_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void txtNumDiasCaduca_EditValueChanged(object sender, EventArgs e)
        {
            dtFechaCambioClave.EditValue = DateTime.Today.AddDays(Convert.ToInt32(txtNumDiasCaduca.Text));
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtCodigoUsuario.Text == "") { MessageBox.Show("Debe ingresar un código de usuario", "Guardar Usuario", MessageBoxButtons.OK, MessageBoxIcon.Error); txtCodigoUsuario.Focus(); return; }
                if (txtClave.Text == "") { MessageBox.Show("Debe ingresar una clave de usuario", "Guardar Usuario", MessageBoxButtons.OK, MessageBoxIcon.Error); txtClave.Focus(); return; }
                if (txtNombreUsuario.Text == "") { MessageBox.Show("Debe ingresar un nombre de usuario", "Guardar Usuario", MessageBoxButtons.OK, MessageBoxIcon.Error); txtNombreUsuario.Focus(); return; }
                if (txtCorreo.Text != "" && lkpDominioCorreo.EditValue == null) { MessageBox.Show("Debe seleccionar un dominio para el correo", "Guardar Usuario", MessageBoxButtons.OK, MessageBoxIcon.Error); lkpDominioCorreo.Focus(); return; }
                if (txtNumDiasCaduca.Text == "0" && chkNoExpira.Checked == false) { MessageBox.Show("El numero de dias a caducar debe ser mayor a 0", "Guardar Usuario", MessageBoxButtons.OK, MessageBoxIcon.Error); txtNumDiasCaduca.Focus(); return; }
                if (dtFechaCambioClave.EditValue == null) { MessageBox.Show("La fecha a caducar debe tener una fecha valida, escribir la cantidad de dias a caducar", "Guardar Usuario", MessageBoxButtons.OK, MessageBoxIcon.Error); txtNumDiasCaduca.Focus(); return; }
                DateTime Fecha1 = Convert.ToDateTime(dtFechaCambioClave.EditValue).Date;
                DateTime FechA2 = DateTime.Now.Date;
                if (Fecha1 <= FechA2 && chkNoExpira.Checked == false) // Si la fecha indicada es menor o igual a la fecha actual
                {
                    MessageBox.Show("La contraseña a caducado, escribir la cantidad de dias a caducar", "Guardar Usuario", MessageBoxButtons.OK, MessageBoxIcon.Error); txtNumDiasCaduca.Focus(); return;
                }

                string result = "";
                string Mensaje = "";
                switch (MiAccion)
                {
                    case Usuario.Nuevo: result = Guardar(); Mensaje = "Se creo el usuario de manera satisfactoria"; break;
                    case Usuario.Editar: result = Modificar(); Mensaje = "Se guardó el usuario de manera satisfactoria"; break;
                }

                if (result == "OK")
                {
                    GrabarPerfiles();
                    if (MiAccion == Usuario.Nuevo) { GrabarEmpresasVinculadas(); GrabarSolucionesVinculadas(); }
                    MessageBox.Show(Mensaje, "Guardar Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ActualizarListado = "SI";

                    int nRow = 0;
                    if (MiAccion == Usuario.Nuevo)
                    {
                        if (SolucionSeleccionado != "")
                        {
                            frmHandler.CargarListado(EstadoSeleccionado, SolucionSeleccionado, PerfilSeleccionado);
                            for (int x = 0; x <= frmHandler.gvListaUsuario.RowCount - 1; x++)
                            {
                                eUsuario obj = frmHandler.gvListaUsuario.GetRow(x) as eUsuario;
                                if (obj != null && obj.cod_usuario == cod_usuario) { nRow = x; }
                            }
                            frmHandler.gvListaUsuario.FocusedRowHandle = nRow;
                        }
                        MiAccion = Usuario.Editar;
                        chkActivo.Enabled = true;
                        picAnteriorUsuario.Enabled = true;
                        picSiguienteUsuario.Enabled = true;
                        txtUsuarioCambio.Text = Program.Sesion.Usuario.dsc_usuario;
                        txtUsuarioRegistro.Text = Program.Sesion.Usuario.dsc_usuario;
                        dtFechaModificacion.EditValue = DateTime.Now;
                        dtFechaRegistro.EditValue = DateTime.Now;

                    }
                    else
                    {
                        if (SolucionSeleccionado != "")
                        {
                            nRow = frmHandler.gvListaUsuario.FocusedRowHandle;
                            frmHandler.CargarListado(EstadoSeleccionado, SolucionSeleccionado, PerfilSeleccionado);
                            frmHandler.gvListaUsuario.FocusedRowHandle = nRow;
                        }

                        txtUsuarioCambio.Text = Program.Sesion.Usuario.dsc_usuario;
                        dtFechaModificacion.EditValue = DateTime.Now;


                    }
                }

                if (result == "REPETIDO")
                {
                    MessageBox.Show("Codigo de usuario ya existente", "Guardar Usuario", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MiAccion = Usuario.Nuevo;
            LimpiarCamposUsuario();
        }

        private void gvListadoSolucion_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvListadoSolucion_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvListadoSolucion_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (MiAccion == Usuario.Nuevo) return;
                // eUsuario_Empresas eUserEmp = new eUsuario_Empresas();
                if (e.Column.FieldName == "flg_acceso" && e.RowHandle >= 0)
                {
                    if (!(gvListadoSolucion.GetRow(e.RowHandle) is eSolucionUsuario_Consulta obj)) return;
                    var result = unit.Sistema.Guardar_Eliminar_UsuarioSolucion<eSolucionUsuario>(new eSolucionUsuario()
                    {
                        cod_usuario = cod_usuario,
                        cod_solucion = obj.cod_solucion,
                        cod_usuario_registro = Program.Sesion.Usuario.cod_usuario
                    }, obj.flg_acceso);
                    if (result != null) { CargarComboboxSolucionesAsignadas(); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void rchkSeleccionado_solucion_CheckStateChanged(object sender, EventArgs e)
        {
            gvListadoSolucion.PostEditor();
        }

        private void lkpSolucion_EditValueChanged(object sender, EventArgs e)
        {
            SolucionSeleccionado = lkpSolucion.Text.ToString();
            CargarPerfilDisponible();
            CargarPerfilAsignados();
        }
    }
}