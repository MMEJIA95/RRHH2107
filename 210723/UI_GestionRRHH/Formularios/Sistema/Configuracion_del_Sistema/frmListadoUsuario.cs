﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using BE_GestionRRHH;
using BL_GestionRRHH;
using DevExpress.Images;
using DevExpress.XtraNavBar;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils.Drawing;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using iTextSharp.text.pdf.codec.wmf;

namespace UI_GestionRRHH.Formularios.Sistema.Configuracion_del_Sistema
{
    public partial class frmListadoUsuario : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly UnitOfWork unit;
        //private string _estado;
        //private string _solucion;
        //private string _perfil;

        public frmListadoUsuario()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            configurar_formulario();
        }
        private void configurar_formulario()
        {
            btnBuscador.Appearance.BackColor = Program.Sesion.Colores.Verde;
        }

        private void frmListadoUsuario_Load(object sender, EventArgs e)
        {
            lblTitulo.ForeColor = Program.Sesion.Colores.Verde;//Color.FromArgb(colorVerde[0], colorVerde[1], colorVerde[2]);
            CargarOpcionesMenu();
            CargarListado("SI", Program.Sesion.Global.Solucion, "ALL");
            //navBarControl1.Groups[0].SelectedLinkIndex =1;
            //lblTitulo.Text = navBarControl1.SelectedLink.Group.Caption + ": " + "ACTIVOS";
            //picTitulo.Image = navBarControl1.SelectedLink.Group.ImageOptions.LargeImage;  
        }

        public void CargarListado(string estado, string solucion, string perfil)//(string NombreGrupo, string Codigo)
        {
            try
            {
                string flg_activo = estado;// "SI";
                string cod_perfil = perfil;

                int opcion = 1;
                //switch (NombreGrupo)
                //{
                //    case "Por Estado": opcion = 1; flg_activo = Codigo; break;
                //    case "Por Perfil": opcion = 3; cod_perfil = Codigo; break;
                //}

                if (cod_perfil == "ALL") { opcion = 1; } else { opcion = 3; }



                //if (opcion == 3 && Codigo == "ALL") { opcion = 1; }
                List<eUsuario> ListUsuario = new List<eUsuario>();
                ListUsuario = unit.Usuario.ListarUsuarios<eUsuario>(opcion: opcion, flg_activo: flg_activo, cod_perfil: cod_perfil, dsc_solucion:solucion=="TODOS"?"ALL":solucion);
                bsListadoUsuario.DataSource = null; bsListadoUsuario.DataSource = ListUsuario;
                gvListaUsuario.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void CargarOpcionesMenu()
        {
            var solucion = unit.Sistema.ListarOpcionesMenuVentana<eSolucion>(5);
            //foreach (var item in solucion)
            //{
            //    clbSolucion.Items.Add(item.dsc_solucion);
            //}
            solucion.Add(new eSolucion() { cod_solucion = "ALL", dsc_solucion = "TODOS" });
            lkpSolucion.Properties.DataSource = solucion.ToList();
            lkpSolucion.Properties.DisplayMember = "dsc_solucion";
            lkpSolucion.Properties.ValueMember = "cod_solucion";
            lkpSolucion.EditValue = "ALL";
            
            List<eUsuario> ListEstados = new List<eUsuario>();
            ListEstados = unit.Usuario.ListarOpcionesMenu<eUsuario>(1);
            //Image imgEstadoLarge = ImageResourceCache.Default.GetImage("images/programming/forcetesting_32x32.png");
            //Image imgEstadoSmall = ImageResourceCache.Default.GetImage("images/programming/forcetesting_16x16.png");

            lkpEstado.Properties.DataSource = ListEstados.ToList();
            lkpEstado.Properties.DisplayMember = "dsc_menu";
            lkpEstado.Properties.ValueMember = "cod_menu";
            lkpEstado.EditValue = "SI";
            //lkpEstado.ItemIndex=0;
            //NavBarGroup NavEstado = navBarControl1.Groups.Add();
            //NavEstado.Caption = "Por Estado"; NavEstado.Expanded = true; NavEstado.SelectedLinkIndex = 0;
            //NavEstado.GroupCaptionUseImage = NavBarImage.Large; NavEstado.GroupStyle = NavBarGroupStyle.SmallIconsText;
            //NavEstado.ImageOptions.LargeImage = imgEstadoLarge; NavEstado.ImageOptions.SmallImage = imgEstadoSmall;


            //NavTipoProv.ItemChanged += NavCabecera_LinkClicked;

            // foreach (eUsuario obj in ListEstados)
            {
                //NavBarItem NavDetalle = navBarControl1.Items.Add();
                //NavDetalle.Tag = obj.cod_menu; NavDetalle.Name = obj.cod_menu;
                //NavDetalle.Caption = obj.dsc_menu; NavDetalle.LinkClicked += NavDetalle_LinkClicked;

                //NavEstado.ItemLinks.Add(NavDetalle);
                //cbeEstado.Properties.Items.Add(obj.dsc_menu);
            }

            CagarPerfil("ALL");// lkpSolucion.Text.ToString());
            //clbPerfil.CheckMember = "ALL";
            //clbPerfil.Items[0].CheckState = CheckState.Checked;
            //clbPerfil.CheckedIndices;
            //if (ListPerfil.Count > 0)
            //    clbPerfil.Items[0].CheckState = CheckState.Checked;




            //NavBarGroup NavPerfil = navBarControl1.Groups.Add();
            //NavPerfil.Caption = "Por Perfil"; NavPerfil.Expanded = false; NavPerfil.SelectedLinkIndex = 0;
            //NavPerfil.GroupCaptionUseImage = NavBarImage.Large; NavPerfil.GroupStyle = NavBarGroupStyle.SmallIconsText;
            //NavPerfil.ImageOptions.LargeImage = imgPerfilLarge; NavPerfil.ImageOptions.SmallImage = imgPerfilSmall;


            //foreach (eUsuario obj in ListPerfil)
            //{
            //NavBarItem NavDetalle = navBarControl1.Items.Add();
            //NavDetalle.Tag = obj.cod_menu; NavDetalle.Name = obj.cod_menu;
            //NavDetalle.Caption = obj.dsc_menu; NavDetalle.LinkClicked += NavDetalle_LinkClicked;

            //NavPerfil.ItemLinks.Add(NavDetalle);
            //}


        }

        private void CagarPerfil(string dsc_solucion)
        {
            List<eUsuario> ListPerfil = new List<eUsuario>();
            dsc_solucion= dsc_solucion.ToLower().Contains("[vacío]")?null: dsc_solucion.Equals("TODOS") ?"ALL": dsc_solucion;
            ListPerfil = unit.Usuario.ListarOpcionesMenu<eUsuario>(3, dsc_solucion: dsc_solucion);

            Image imgPerfilLarge = ImageResourceCache.Default.GetImage("images/business%20objects/boresume_32x32.png");
            Image imgPerfilSmall = ImageResourceCache.Default.GetImage("images/business%20objects/boresume_16x16.png");

            clbPerfil.DataSource = ListPerfil.ToList();
            clbPerfil.ValueMember = "cod_menu";
            clbPerfil.DisplayMember = "dsc_menu";
            clbPerfil.SelectedValue = "ALL";
            //clbPerfil.UnCheckSelectedItems();
            clbPerfil.CheckSelectedItems();
        }

        private void navBarControl1_ActiveGroupChanged(object sender, NavBarGroupEventArgs e)
        {
            //e.Group.SelectedLinkIndex = 0;
            //if (e.Group.SelectedLink != null) ActiveGroupChanged(e.Group.Caption + ": " + e.Group.SelectedLink.Item.Caption, e.Group.ImageOptions.LargeImage);
            //if (e.Group.SelectedLink != null)
            //{
                
            //}
        }

        void ActiveGroupChanged(string caption, Image imagen)
        {
            lblTitulo.Text = caption; picTitulo.Image = imagen;
        }

        private void NavDetalle_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            //lblTitulo.Text = e.Link.Group.Caption + ": " + e.Link.Caption; picTitulo.Image = e.Link.Group.ImageOptions.LargeImage;
            //unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Obteniendo listado", "Cargando...");
            //CargarListado(e.Link.Group.Caption, e.Link.Item.Tag.ToString());
            //SplashScreenManager.CloseForm();
        }

        private void navBarControl1_SelectedLinkChanged(object sender, DevExpress.XtraNavBar.ViewInfo.NavBarSelectedLinkChangedEventArgs e)
        {
           // e.Group.SelectedLinkIndex = 0;
        }

        internal void frmListadoUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                EjecutarBuscador();
                //unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Obteniendo listado", "Cargando...");
                //NavBarGroup navGrupo = navBarControl1.SelectedLink.Group as NavBarGroup;
                //CargarListado(navGrupo.Caption, navGrupo.SelectedLink.Item.Tag.ToString());
                //SplashScreenManager.CloseForm();
            }
        }

        private void gvListaUsuario_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    GridView view = sender as GridView;
                    string campo = e.Column.FieldName;
                    if (view.GetRowCellValue(e.RowHandle, "flg_activo").ToString() == "NO") e.Appearance.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void gvListaUsuario_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void ExportarExcel()
        {
            try
            {
                string carpeta = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString());
                string archivo = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()) + "\\ReporteUsuarios" + DateTime.Now.ToString().Replace("/", "-").Replace(":", "") + ".xlsx";
                
                //string ruta = ConfigurationManager.AppSettings["RutaArchivosLocalExportar"].ToString() + "\\ReporteUsuarios" + DateTime.Now.ToString().Replace("/", "-").Replace(":", "") + ".xlsx";
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

                gvListaUsuario.OptionsPrint.AutoWidth = AutoSize;
                gvListaUsuario.OptionsPrint.ShowPrintExportProgress = true;
                gvListaUsuario.OptionsPrint.AllowCancelPrintExport = true;

                XlsxExportOptions options = new XlsxExportOptions();
                options.TextExportMode = TextExportMode.Text;
                options.ExportMode = XlsxExportMode.SingleFile;

                ExportSettings.DefaultExportType = ExportType.WYSIWYG;
                gvListaUsuario.ExportToXlsx(archivo);

                if (MessageBox.Show("Excel exportado en la ruta " + archivo + Environment.NewLine + "¿Desea abrir el archivo?", "Exportar Excel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Process.Start(archivo);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExportarExcel_ItemClick(object sender, ItemClickEventArgs e)
        {
            ExportarExcel();
        }

        private void btnImprimir_ItemClick(object sender, ItemClickEventArgs e)
        {
            gvListaUsuario.ShowPrintPreview();
        }

        private void gvListaUsuario_RowClick(object sender, RowClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2 && e.RowHandle >= 0)
                {
                    eUsuario obj = gvListaUsuario.GetFocusedRow() as eUsuario;

                    frmMantUsuario frm = new frmMantUsuario(this)
                    {
                        Text = "Mantenimiento de Usuarios",
                        SolucionSeleccionado = lkpSolucion.Text,
                        EstadoSeleccionado = lkpEstado.EditValue.ToString(),
                        PerfilSeleccionado = clbPerfil.SelectedValue.ToString()
                    };
                    //frm.colorVerde = colorVerde;
                    //frm.colorPlomo = colorPlomo;
                    //frm.colorEventRow = colorEventRow;
                    //frm.colorFocus = colorFocus;
                    //frm.user = user;
                    frm.cod_usuario = obj.cod_usuario;
                    frm.MiAccion = frmMantUsuario.Usuario.Editar;
                    //NavBarGroup navGrupo = navBarControl1.SelectedLink.Group as NavBarGroup;
                    //frm.GrupoSeleccionado = navGrupo.Caption;
                    //frm.ItemSeleccionado = navGrupo.SelectedLink.Item.Tag.ToString();
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnNuevo_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                frmMantUsuario frm = new frmMantUsuario(this) 
                { Text="Mantenimiento de Usuarios", 
                    SolucionSeleccionado=lkpSolucion.Text,
                    EstadoSeleccionado=lkpEstado.EditValue.ToString(),
                    PerfilSeleccionado=clbPerfil.SelectedValue.ToString()
                };
                //frm.colorVerde = colorVerde;
                //frm.colorPlomo = colorPlomo;
                //frm.colorEventRow = colorEventRow;
                //frm.colorFocus = colorFocus;
                //frm.user = user;
                frm.MiAccion = frmMantUsuario.Usuario.Nuevo;
                //NavBarGroup navGrupo = navBarControl1.SelectedLink.Group as NavBarGroup;
                //frm.GrupoSeleccionado = navGrupo.Caption;
                //frm.ItemSeleccionado = navGrupo.SelectedLink.Item.Tag.ToString();
                frm.ShowDialog();
                //CargarListado(navGrupo.Caption, navGrupo.SelectedLink.Item.Tag.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnActivar_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                eUsuario eUsu = gvListaUsuario.GetFocusedRow() as eUsuario;
                if (eUsu != null)
                {
                    DialogResult msgresult = MessageBox.Show("¿Está seguro de Activar el usuario " + eUsu.dsc_usuario + "?", "Activar Usuario", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (msgresult == DialogResult.Yes)
                    {
                        eUsu.flg_activo = "SI";
                        unit.Usuario.Guardar_Actualizar_Usuario<eUsuario>(eUsu, "Actualizar",Program.Sesion.Usuario.cod_usuario);
                        EjecutarBuscador();

                        // NavBarGroup navGrupo = navBarControl1.SelectedLink.Group as NavBarGroup;
                        //if (navGrupo.SelectedLink.Item.Tag.ToString() == "ALL")
                        //{
                        //    gvListaUsuario.SetRowCellValue(gvListaUsuario.FocusedRowHandle, "flg_activo", "SI");
                        //}
                        //else
                        //{
                        //    CargarListado(navGrupo.Caption, navGrupo.SelectedLink.Item.Tag.ToString());
                        //}

                    }
                } else
                {
                    MessageBox.Show("No hay usuario seleccionado", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gvListaUsuario_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void btnInactivar_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                eUsuario eUsu = gvListaUsuario.GetFocusedRow() as eUsuario;
                if (eUsu != null)
                {
                    DialogResult msgresult = MessageBox.Show("¿Está seguro de Inactivar el usuario " + eUsu.dsc_usuario + "?", "Inactivar Usuario", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (msgresult == DialogResult.Yes)
                    {
                        eUsu.flg_activo = "NO";
                        unit.Usuario.Guardar_Actualizar_Usuario<eUsuario>(eUsu, "Actualizar", Program.Sesion.Usuario.cod_usuario);
                        EjecutarBuscador();
                        // NavBarGroup navGrupo = navBarControl1.SelectedLink.Group as NavBarGroup;
                        //if (navGrupo.SelectedLink.Item.Tag.ToString() == "ALL")
                        //{
                        //    gvListaUsuario.SetRowCellValue(gvListaUsuario.FocusedRowHandle, "flg_activo", "NO");
                        //}
                        //else
                        //{
                        //    CargarListado(navGrupo.Caption, navGrupo.SelectedLink.Item.Tag.ToString());
                        //}
                    }
                }
                else
                {
                    MessageBox.Show("No hay usuario seleccionado", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEliminar_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                eUsuario eUsu = gvListaUsuario.GetFocusedRow() as eUsuario;
                if (eUsu != null)
                {
                    //DialogResult msgresult = MessageBox.Show("¿Está seguro de eliminar el usuario "+ eUsu.dsc_usuario+ "?", "Eliminar Usuario", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    //if (msgresult == DialogResult.Yes)
                    //{
                    //    blUsu.Eliminar_Usuario<eUsuario>(1, eUsu.cod_usuario);
                    //    gvListaUsuario.DeleteRow(gvListaUsuario.FocusedRowHandle);
                    //}
                    DialogResult msgresult = MessageBox.Show("¿Está seguro de Inactivar el usuario " + eUsu.dsc_usuario + "?", "Inactivar Usuario", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (msgresult == DialogResult.Yes)
                    {
                        eUsu.flg_activo = "NO";
                        unit.Usuario.Guardar_Actualizar_Usuario<eUsuario>(eUsu, "Actualizar", Program.Sesion.Usuario.cod_usuario);

                        EjecutarBuscador();
                        //NavBarGroup navGrupo = navBarControl1.SelectedLink.Group as NavBarGroup;
                        //if (navGrupo.SelectedLink.Item.Tag.ToString() == "ALL")
                        //{
                        //    gvListaUsuario.SetRowCellValue(gvListaUsuario.FocusedRowHandle, "flg_activo", "NO");
                        //}
                        //else
                        //{
                        //    CargarListado(navGrupo.Caption, navGrupo.SelectedLink.Item.Tag.ToString());
                        //}
                    }
                }
                else
                {
                    MessageBox.Show("No hay usuario seleccionado", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void rptFecha_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            if (e.DisplayText.Contains("0001")) { e.DisplayText = ""; }
        }

        private void btnBuscador_Click(object sender, EventArgs e)
        {
            EjecutarBuscador();
        }
        private void EjecutarBuscador()
        {
            var stdo = lkpEstado.EditValue.ToString();
            var sol = lkpSolucion.Text.ToString();
            var pefl=clbPerfil.SelectedValue.ToString();
            lblTitulo.Text = $"Estado: {lkpEstado.Text}, Solución: {lkpSolucion.Text}";//   e.Link.Group.Caption + ": " + e.Link.Caption; picTitulo.Image = e.Link.Group.ImageOptions.LargeImage;
            //lblTitulo.Text = e.Link.Group.Caption + ": " + e.Link.Caption; picTitulo.Image = e.Link.Group.ImageOptions.LargeImage;
            unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Obteniendo listado", "Cargando...");
            CargarListado(estado: stdo, solucion:sol,perfil: pefl);
            SplashScreenManager.CloseForm();
        }

        private void lkpSolucion_EditValueChanged(object sender, EventArgs e)
        {
            CagarPerfil(lkpSolucion.Text.ToString());
        }
    }
}