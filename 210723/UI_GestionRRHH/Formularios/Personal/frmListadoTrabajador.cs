using DevExpress.XtraBars;
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
using BL_GestionRRHH;
using DevExpress.Images;
using DevExpress.XtraNavBar;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraGrid.Views.Grid;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using DevExpress.XtraTreeList;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using System.Reflection;
using DevExpress.Spreadsheet;
using BE_GestionRRHH.FormatoMD;
using UI_GestionRRHH.Formularios.Personal.FormatoDocumento_Seguimiento;
using Microsoft.Identity.Client;
using Excel = Microsoft.Office.Interop.Excel;
using DevExpress.XtraVerticalGrid;

namespace UI_GestionRRHH.Formularios.Personal
{
    public partial class frmListadoTrabajador : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly UnitOfWork unit;
        private List<eTrabajador> trabajadorLista;
        public string formName, resultadotregistro="SI";
        frmListadoTrabajador frmHandler;
        //llklkkkk
        List<eTrabajador> listTrabajador = new List<eTrabajador>();
        List<eTrabajador.eFormatos> listaFormatos = new List<eTrabajador.eFormatos>();
        List<eTrabajador> listaraltaplacar = new List<eTrabajador>();
        List<eTrabajador> ListarContrato= new List<eTrabajador>();
        List<eTrabajador.eContactoEmergencia_Trabajador> ListContactos = new List<eTrabajador.eContactoEmergencia_Trabajador>();
        List<eTrabajador.eInfoLaboral_Trabajador> ListInfoLaboral = new List<eTrabajador.eInfoLaboral_Trabajador>();
        List<eFormatoMD_General.eFormatoDocumento> listaDocumetnos = new List<eFormatoMD_General.eFormatoDocumento>();
        public List<eTrabajador.eEMO> listadoemo;
        BindingSource Formato34 = new BindingSource();
        BindingSource Altaconcar = new BindingSource();

        public virtual bool Checked { get; set; }
        public bool AllowGrayed { get; set; }


        Image ImgCumple = Properties.Resources.birthday_cakex24;
        bool Buscar = false;
        Brush ConCriterios = Brushes.Green;
        Brush SinCriterios = Brushes.Red;
        Brush NAplCriterio = Brushes.Orange;
        int markWidth = 16;
        public string cod_trabajador = "", cod_empresa = "", txtformato = "", cod_usuario = "", fecha_baja = "", motivo_baja = "", observaciones_baja = "";
        XtraReport report = new XtraReport();
        DetailBand detailBand = new DetailBand();

        //OneDrive
        private Microsoft.Graph.GraphServiceClient GraphClient { get; set; }
        AuthenticationResult authResult = null;
        string[] scopes = new string[] { "Files.ReadWrite.All" };
        string varPathOrigen = "";
        string varNombreArchivo = "", varNombreArchivoSinExtension = "";



        public frmListadoTrabajador()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            //this.chkFormatoE4 = new DevExpress.XtraBars.BarCheckItem();
            //this.chkFormatoE5 = new DevExpress.XtraBars.BarCheckItem();
        }

        private void HabilitarBotones()
        {
            List<eVentana> listPermisos = unit.Sistema.ListarMenuxUsuario<eVentana>(Program.Sesion.Usuario.cod_usuario, this.Name, Program.Sesion.Global.Solucion);

            if (listPermisos.Count > 0)
            {
                grupoEdicion.Enabled = listPermisos[0].flg_escritura;
                //ribbonPageGroup1.Enabled = listPermisos[0].flg_escritura;
                ribbonPageGroup3.Enabled = listPermisos[0].flg_escritura;
                btnCargaMasivaEMO.Enabled = listPermisos[0].flg_escritura;
            }
            List<eVentana> listPerfil = unit.Sistema.ListarPerfilesUsuario<eVentana>(4, Program.Sesion.Usuario.cod_usuario, Program.Sesion.Global.Solucion);
            eVentana oPerfil = listPerfil.Find(x => x.cod_perfil == 1 || x.cod_perfil == 8);
            btnAreaEmpresa.Enabled = oPerfil != null ? true : false;
            btnCargoEmpresa.Enabled = oPerfil != null ? true : false;
        }
        private void BloqueoControles(bool Enabled, bool ReadOnly, bool Editable)
        {

        }
        private void CargarFiltroTreeList()
        {

            //List<eEmpresa> ListEmp = unit.Clientes.ListarOpcionesMenu<eEmpresa>(2);
            var ListEmp = Program.Sesion.EmpresaList;
            ctd_empresas = ListEmp.Count;
         //   ctd_empresas = ListEmp.Count;

            var emp_sedeList = new List<eFltEmpresaSede>();
            //_options.Add(new Option() { ParentID = "0", ID = "1", Name = "EMPRESA", Checked = true });
            foreach (var  obj in ListEmp)
            {
                //_options.Add(new Option() { ParentID = "1", ID = obj.cod_empresa, Name = obj.dsc_empresa, Checked = true });

                List<eEmpresa.eSedeEmpresa> ListSedes = unit.Clientes.ListarOpcionesMenu<eEmpresa.eSedeEmpresa>(6, obj.cod_empresa);
                foreach (eEmpresa.eSedeEmpresa objSede in ListSedes)
                {

                    //_options.Add(new Option() { ParentID = obj.cod_empresa, ID = obj.cod_empresa + "-" + objSede.cod_sede_empresa, Name = objSede.dsc_sede_empresa, Checked = true });

                    emp_sedeList.Add(new eFltEmpresaSede()
                    {
                        cod_empresa = obj.cod_empresa,
                        dsc_empresa = obj.dsc_empresa,
                        cod_sede_empresa = objSede.cod_sede_empresa,
                        dsc_sede_empresa = objSede.dsc_sede_empresa
                    });
                }
            }


            //var filtro = unit.Vacaciones.ConsultaVarias_Vacaciones<eFltEmpresaSedeArea>(
            //    new pVacaciones() { Opcion = 1 });
            if (emp_sedeList != null && emp_sedeList.Count > 0)
            {
                var lst = emp_sedeList;
                //.OrderBy(e => e.dsc_sede_empresa).ThenBy(s => s.dsc_sede_empresa).ThenBy(a => a.dsc_area).ToList();
                var tree = new Tools.TreeListHelper(treeFiltroEmpresa);
                tree.TreeViewParaDosNodos<eFltEmpresaSede>(emp_sedeList,
                      ColumnaCod_Padre: "cod_empresa",
                      ColumnaDsc_Padre: "dsc_empresa",
                      ColumnaCod_Hijo: "cod_sede_empresa",
                      ColumnaDsc_Hijo: "dsc_sede_empresa"
                    );
                //tree.CheckSubNodos();
                refreshTreeView();

                //_treeList.OptionsView.RootCheckBoxStyle = NodeCheckBoxStyle.Check;
            }
        }
        private void refreshTreeView()
        {

           
            /////////
            treeFiltroEmpresa.OptionsView.RootCheckBoxStyle = DevExpress.XtraTreeList.NodeCheckBoxStyle.Radio;
            for (int i = 0; i < treeFiltroEmpresa.Nodes.Count; i++)
            {
                treeFiltroEmpresa.Nodes[i].ChildrenCheckBoxStyle = NodeCheckBoxStyle.Check;
                for (int j = 0; j < treeFiltroEmpresa.Nodes[i].Nodes.Count(); j++)
                {
                    treeFiltroEmpresa.Nodes[i].Nodes[j].ChildrenCheckBoxStyle = NodeCheckBoxStyle.Check;
                }
            }
            //treeFiltroEmpresa.UncheckAll();
            treeFiltroEmpresa.Nodes[0].Checked = true;
            treeFiltroEmpresa.Nodes[0].Nodes[0].Checked = true;
            treeFiltroEmpresa.Nodes[0].Nodes[0].Nodes.ToList().ForEach((ch) => ch.Checked = true);
            //  treeFiltroVacaciones.Nodes[0].Nodes[0].Checked = true;
            treeFiltroEmpresa.CollapseAll();
            treeFiltroEmpresa.Nodes[0].ExpandAll();
            treeFiltroEmpresa.Refresh();
            treeFiltroEmpresa.CheckAll();


        }


        private void formatos(string nombrearchivo, int opcion, string extencion)
        {
            string lista = "";
            string empresa = "";
            string trabajador = "";
            string ruc = "";
            //string nombreformato = "";


            eTrabajador obj2 = new eTrabajador();
            
            foreach (int nRow in gvListadoTrabajador.GetSelectedRows())
            {
                
                eTrabajador obj = gvListadoTrabajador.GetRow(nRow) as eTrabajador;
                obj2 = unit.Trabajador.Obtener_Trabajador<eTrabajador>(22, cod_usuario: Program.Sesion.Usuario.cod_usuario, cod_empresa: obj.cod_empresa);
                empresa = obj.cod_empresa;

                ruc = obj2.dsc_ruc;
                trabajador = obj.cod_trabajador + "," + trabajador;
            }
            listaFormatos.Clear();
            listaFormatos = unit.Trabajador.ListarFormatos<eTrabajador.eFormatos>(opcion, cod_empresa: empresa, cod_trabajador_multiple: trabajador);

            Formato34.DataSource = listaFormatos;

            string texto = "";

            foreach (eTrabajador.eFormatos obj in listaFormatos)
            {

                texto = texto + (texto == "" ? "" : Environment.NewLine) + Convert.ToString(Formato34.DataSource = obj.FORMATOS);
                if (texto == "") { HNG.MessageError("SE REQUIERE MAS DATOS DEL TRABAJADOR", "ERROR AL EXPORTAR"); resultadotregistro="NO"; return; }
            }
            XtraReport report = new XtraReport()
            {
                Name = nombrearchivo+ruc,
                Bands = {
                    new DetailBand() {
                                Controls = {
                                        new XRLabel() {
                                            Multiline = true,
                                            Text = texto
                                        }
                                    }
                                }
                            }
            };
            report.CreateDocument();

            string carpeta = obtenerCarpetaArchivosExportados();
            string textExportFile = $@"{carpeta}\" + report.Name + extencion;

            report.PrintingSystem.ExportToText(textExportFile);

            //C:\IMPERIUM-Software\Archivos exportados
        }

        private void mostrarCarpetaPLAME(string mensaje)
        {
            // NOTA  Colocar la ruta que corresponde a descargas (ver si está en el APP config)
            MessageBox.Show(mensaje);
            //Process.Start(@"C:\IMPERIUM-Software\" + @"Archivos exportados\"); //para abrir la carpeta
            Process.Start(obtenerCarpetaArchivosExportados());
        }
        private string obtenerCarpetaArchivosExportados()
        {
            var unit = new UnitOfWork();
            var pathDirectory = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")]).ToString();


            if (!Directory.Exists(pathDirectory))
                Directory.CreateDirectory(pathDirectory);


            return pathDirectory;


        }
        private string eliminararchivodecarpeta()
        {
            var unit = new UnitOfWork();
            var pathDirectory = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")]).ToString();

            Directory.Delete(pathDirectory, true);


            return pathDirectory;


        }





        //if (n.GetValue("ID").ToString().Length == 5 ) 
        //                empresas = n.GetValue("ID").ToString() + "," + empresas;
        //            else sedes = n.GetValue("ID").ToString() + "," + sedes;

        public class AlertData
        {
            public AlertData(string trabajador, string hijo) {
                Trabajador = trabajador;
                Hijo = hijo;
            }

            public string Trabajador { get; set; }
            public string Hijo { get; set; }
        }

        private  void mayoredad()
        {
            string a = "";
            foreach (var item in listTrabajador)
            {
                if (item.estado_edad == "SI")
                {
                    AlertData adata = new AlertData(item.dsc_nombres_completos,"");
                    alertControl1.Show(adata, this);
                    alertControl1.HtmlElementMouseClick += (s, e) =>
                    {
                        if (e.ElementId == "btn_cancelar") e.HtmlPopup.Close();
                        if (e.ElementId == "btn_adjuntar")
                        {
                            a = "si";
                            e.HtmlPopup.Close();
                        }
                        if (a == "si")
                        {
                            frmAdjuntarArchivo adjuntar = new frmAdjuntarArchivo();
                            adjuntar.lbltrabajador.Text = item.dsc_nombres_completos;
                            adjuntar.empresa = item.cod_empresa;
                            adjuntar.cod_trabajador = item.cod_trabajador;
                            adjuntar.Show(this);
                        }
                        
                    };
                }
            }

           
        }

        private void frmListadoTrabajador_Load(object sender, EventArgs e)
        {
           

            CargarFiltroTreeList();
            HabilitarBotones();
            //mayoredad();
            //Inicializar();
            //listTrabajador = unit.Trabajador.ListarTrabajadoresvista<eTrabajador>(1, "", "ALL", ""); // LISTA GENERAL
            ////CargarListado();
            //bsListaTrabajador.DataSource = listTrabajador; gvListadoTrabajador.RefreshData();

            RepositoryItemTextEdit textEdit = new RepositoryItemTextEdit();
            //textEdit.ContextImageOptions.Image = Image.FromFile("../Resources/bocontact2_16x16.png");
            gvListadoTrabajador.Columns["flg_sexo"].ColumnEdit = textEdit;
            gcListadoTrabajador.RepositoryItems.Add(textEdit);
            splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel1;


            btnBuscar_Click(btnBuscar, new EventArgs());

            if (layoutFiltro.ContentVisible == false)
            {
                layoutFiltro.ContentVisible = true;
                layoutFiltro.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;


                return;
            }
        }

        private void Inicializar()
        {
            //CargarOpcionesMenu();
            ////CargarListado("TODOS", "");
            ////lblTitulo.ForeColor = Color.FromArgb(colorVerde[0], colorVerde[1], colorVerde[2]);
            ////lblTitulo.Text = navBarControl1.SelectedLink.Group.Caption + ": " + navBarControl1.SelectedLink.Item.Caption;
            ////picTitulo.Image = navBarControl1.SelectedLink.Group.ImageOptions.LargeImage;
            //navFilter.Groups[0].SelectedLinkIndex = 0;
            //Buscar = true;
            //navFilter.SelectedLink = navFilter.Groups[0].ItemLinks[0];
            //NavBarGroup navGrupo = navFilter.SelectedLink.Group as NavBarGroup;
            //CargarListado(navGrupo.Caption, navGrupo.SelectedLink.Item.Tag.ToString());
        }

        internal void CargarActivos()
        {
            List<eTrabajador> listActivos = unit.Proveedores.ListarOpcionesMenu<eTrabajador>(20);
            NavBarGroup NavEmpresa = navFilter.Groups.Add();

            NavEmpresa.Name = "Estado";
            NavEmpresa.Caption = "Estado"; NavEmpresa.Expanded = true; NavEmpresa.SelectedLinkIndex = 0;
            NavEmpresa.GroupCaptionUseImage = NavBarImage.Large; NavEmpresa.GroupStyle = NavBarGroupStyle.ControlContainer;



        }

        internal void CargarOpcionesMenu()
        {
            List<eProveedor_Empresas> listEmpresas = unit.Proveedores.ListarOpcionesMenu<eProveedor_Empresas>(12);
            Image imgEmpresaLarge = ImageResourceCache.Default.GetImage("images/navigation/home_32x32.png");
            Image imgEmpresaSmall = ImageResourceCache.Default.GetImage("images/navigation/home_16x16.png");

            NavBarGroup NavEmpresa = navFilter.Groups.Add();
            NavEmpresa.Name = "Por Empresa";
            NavEmpresa.Caption = "Por Empresa"; NavEmpresa.Expanded = true; NavEmpresa.SelectedLinkIndex = 0;
            NavEmpresa.GroupCaptionUseImage = NavBarImage.Large; NavEmpresa.GroupStyle = NavBarGroupStyle.ControlContainer;

            NavEmpresa.ImageOptions.LargeImage = imgEmpresaLarge; NavEmpresa.ImageOptions.SmallImage = imgEmpresaSmall;

            List<eProveedor_Empresas> listEmpresasUsuario = unit.Proveedores.ListarEmpresasProveedor<eProveedor_Empresas>(11, "", Program.Sesion.Usuario.cod_usuario);
            if (listEmpresasUsuario.Count == 0) { MessageBox.Show("Debe tener una empresa asignada para visualizar los datos", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }
            List<eProveedor_Empresas> listadoEmp = new List<eProveedor_Empresas>();
            eProveedor_Empresas objEmp = new eProveedor_Empresas();
            //objEmp = listEmpresas.Find(x => x.cod_empresa == "ALL");
            //listadoEmp.Add(objEmp);
            if (listEmpresas.Count > 0)
            {
                foreach (eProveedor_Empresas obj2 in listEmpresasUsuario)
                {
                    objEmp = listEmpresas.Find(x => x.cod_empresa == obj2.cod_empresa);
                    if (objEmp != null) listadoEmp.Add(objEmp);
                }
            }

            foreach (eProveedor_Empresas obj in listadoEmp)
            {
                NavBarItem NavDetalle = navFilter.Items.Add();
                NavDetalle.Tag = obj.cod_empresa; NavDetalle.Name = obj.cod_empresa;
                NavDetalle.Caption = obj.dsc_empresa; NavDetalle.LinkClicked += NavDetalle_LinkClicked;

                NavEmpresa.ItemLinks.Add(NavDetalle);
            }
        }

        private void NavDetalle_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            //lblTitulo.Text = e.Link.Group.Caption + ": " + e.Link.Caption; picTitulo.Image = e.Link.Group.ImageOptions.LargeImage;
            //unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Obteniendo listado", "Cargando...");
            //CargarListado(e.Link.Group.Caption, e.Link.Item.Tag.ToString());
            //SplashScreenManager.CloseForm();
        }

        public void CargarListado()
        {
            try
            {
                string cod_empresa = "";
                string empresas = "";
                string sedes = "";
                string estado = "SI";
                //foreach (DevExpress.XtraTreeList.Nodes.TreeListNode n in TreeList_empresas.GetAllCheckedNodes())
                //{
                //    if (n.GetValue("ID").ToString().Length == 5)
                //        empresas = n.GetValue("ID").ToString() + "," + empresas;
                //    else sedes = n.GetValue("ID").ToString() + "," + sedes;
                //}
                var tool = new Tools.TreeListHelper(treeFiltroEmpresa);
                cod_empresa = tool.ObtenerCodigoConcatenadoDeNodoIndex(0);
                empresas = tool.ObtenerCodigoConcatenadoDeNodoIndex(0);
                sedes = tool.ObtenerCodigoConcatenadoDeNodoIndex(1);

                if (chckEstadoActivo.CheckState == CheckState.Unchecked && chckEstadoInacactivo.CheckState == CheckState.Unchecked)
                {
                    chckEstadoActivo.EditValue = true;
                    estado = "SI";
                    listTrabajador.Clear();
                    listTrabajador = unit.Trabajador.ListarTrabajadoresvista<eTrabajador>(opcion: 1, cod_trabajador: "", cod_empresa: cod_empresa, cod_empresa_multiple: empresas, cod_sede_multiple: sedes, flg_activo: estado);
                    bsListaTrabajador.DataSource = listTrabajador; gvListadoTrabajador.RefreshData();
                    Image img = Properties.Resources.no;
                    btnDardebaja.ImageOptions.LargeImage = img;
                    btnDardebaja.Caption = "Cesar Trabajador";
                    mayoredad();

                }
                if (chckEstadoActivo.CheckState == CheckState.Checked && chckEstadoInacactivo.CheckState == CheckState.Checked) {
                    listTrabajador.Clear();
                    
                    listTrabajador = unit.Trabajador.ListarTrabajadoresvista<eTrabajador>(opcion: 6, cod_trabajador: "", cod_empresa: cod_empresa, cod_empresa_multiple: empresas, cod_sede_multiple: sedes);
                    bsListaTrabajador.DataSource = listTrabajador; gvListadoTrabajador.RefreshData();
                    mayoredad();
                }
                else if (chckEstadoActivo.CheckState == CheckState.Checked)
                {
                    estado = "SI";
                    listTrabajador.Clear();
                    listTrabajador = unit.Trabajador.ListarTrabajadoresvista<eTrabajador>(opcion: 1, cod_trabajador: "", cod_empresa: cod_empresa, cod_empresa_multiple: empresas, cod_sede_multiple: sedes, flg_activo: estado);
                    bsListaTrabajador.DataSource = listTrabajador; gvListadoTrabajador.RefreshData();
                    Image img = Properties.Resources.no;
                    btnDardebaja.ImageOptions.LargeImage = img;
                    btnDardebaja.Caption = "Cesar Trabajador";
                    mayoredad();
                }
                else if (chckEstadoActivo.CheckState == CheckState.Unchecked)
                {

                    estado = "NO";
                    listTrabajador.Clear();
                    listTrabajador = unit.Trabajador.ListarTrabajadoresvista<eTrabajador>(opcion: 1, cod_trabajador: "", cod_empresa: cod_empresa, cod_empresa_multiple: empresas, cod_sede_multiple: sedes, flg_activo: estado);
                    bsListaTrabajador.DataSource = listTrabajador; gvListadoTrabajador.RefreshData();
                    Image imagenbaja = Properties.Resources.apply_32x32;
                    btnDardebaja.ImageOptions.LargeImage = imagenbaja;
                    btnDardebaja.Caption="Activar Trabajador";
                    mayoredad();
                }

                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void navBarControl1_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
            e.Group.SelectedLinkIndex = 0;
            navBarControl1_SelectedLinkChanged(navFilter, new DevExpress.XtraNavBar.ViewInfo.NavBarSelectedLinkChangedEventArgs(e.Group, e.Group.SelectedLink));
        }
        void ActiveGroupChanged(string caption, Image imagen)
        {
            /// lblTitulo.Text = caption; picTitulo.Image = imagen;
        }

        private void navBarControl1_SelectedLinkChanged(object sender, DevExpress.XtraNavBar.ViewInfo.NavBarSelectedLinkChangedEventArgs e)
        {
            //e.Group.SelectedLinkIndex = 0;
            //if (!Buscar) e.Group.SelectedLinkIndex = 0;
            //if (e.Group.SelectedLink != null && Buscar)
            //{
            //    ActiveGroupChanged(e.Group.Caption + ": " + e.Group.SelectedLink.Item.Caption, e.Group.ImageOptions.LargeImage);
            //    //unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Obteniendo listado", "Cargando...");
            //    CargarListado(e.Group.Caption, e.Group.SelectedLink.Item.Tag.ToString());
            //    //SplashScreenManager.CloseForm();
            //}
        }

        internal void frmListadoTrabajador_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
              
                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Obteniendo listado", "Cargando...");
                CargarListado();
                //listTrabajador = unit.Trabajador.ListarTrabajadoresvista<eTrabajador>(1, "", "ALL", ""); // LISTA GENERAL
                //                                                                                         //CargarListado();
                //bsListaTrabajador.DataSource = listTrabajador; gvListadoTrabajador.RefreshData();
                SplashScreenManager.CloseForm();

            }
        }



        private void gvListadoTrabajador_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2 && e.RowHandle >= 0)
                {
                    eTrabajador obj = gvListadoTrabajador.GetFocusedRow() as eTrabajador;
                    if (obj == null) return;
                    if (Application.OpenForms["frmMantTrabajador"] != null)
                    {
                        Application.OpenForms["frmMantTrabajador"].Activate();
                    }
                    else
                    {
                        frmMantTrabajador frm = new frmMantTrabajador(this);
                        frm.MiAccion = Trabajador.Editar;
                        frm.cod_trabajador = obj.cod_trabajador;
                        frm.cod_empresa = obj.cod_empresa;
                        frm.ShowDialog();
                        if (frm.ActualizarListado == "SI") frmListadoTrabajador_KeyDown(null, new KeyEventArgs(Keys.F5));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            mostrardatos();
            //ObtenerDatos_InfoLaboral();



            txtdescripcion.Text = "Seleccione una opción";

        }

        private void btnNuevo_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnExportarExcel_ItemClick(object sender, ItemClickEventArgs e)
        {
            ExportarExcel();
        }
        private void ExportarExcel()
        {
            try
            {
                string carpeta = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString());
                string archivo = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()) + "\\Personal" + DateTime.Now.ToString().Replace("/", "-").Replace(":", "") + ".xlsx";
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
                gvListadoTrabajador.ExportToXlsx(archivo);
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

        private void ExportarExcelAltaplacar()
        {
            string lista = "";
            string empresa = "";
            string trabajador = "";
            string ruc = "";
            unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Exportando Reporte", "Cargando...");
            string ListSeparator = "";

            string entorno = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("conexion")].ToString());
            string server = unit.Encripta.Desencrypta(entorno == "LOCAL" ? ConfigurationManager.AppSettings[unit.Encripta.Encrypta("ServidorLOCAL")].ToString() : ConfigurationManager.AppSettings[unit.Encripta.Encrypta("ServidorREMOTO")].ToString());
            string bd = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("BBDD")].ToString());
            string user = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("UserID")].ToString());
            string pass = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("Password")].ToString());
            string AppName = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("AppName")].ToString());

            string cnxl = "ODBC;DRIVER=SQL Server;SERVER=" + server + ";UID=" + user + ";PWD=" + pass + ";APP=SGI_Excel;DATABASE=" + bd + "";
            string procedure = "";

            ListSeparator = ConfigurationManager.AppSettings["ListSeparator"];
            Excel.Application objExcel = new Excel.Application();
            objExcel.Workbooks.Add();
            //objExcel.Visible = true;
            var workbook = objExcel.ActiveWorkbook;
            var sheet = workbook.Sheets["Hoja1"];

            try
            {
                string carpeta = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString());
                string archivo = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()) + "\\Personal" + DateTime.Now.ToString().Replace("/", "-").Replace(":", "") + ".xlsx";
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

                objExcel.Sheets.Add();
                var worksheet = workbook.ActiveSheet;
              //  objExcel.ActiveWindow.DisplayGridlines = false;

                objExcel.Range["A3:BZ100"].Font.Name = "Calibri"; //objExcel.Range["A4:AB4"].Font.Size = 12;
                                                                  //objExcel.Range["A5:ZZ1000"].Font.Size = 10;

                //objExcel.Range["B1"].Font.Size = 8; objExcel.Range["B1"].Font.Bold = true;
                //objExcel.Range["B1"].Font.Color = System.Drawing.ColorTranslator.FromHtml("#E8194F");
                //objExcel.Range["B1"].Value = "REPORTE DE BOLETOS"; objExcel.Range["B2"].Font.Size = 18; objExcel.Range["B2"].Font.Bold = true; objExcel.Range["B2"].Font.Color = System.Drawing.Color.Blue;
                Excel.Range range = worksheet.Range["A3:BZ10"];
                range.Clear();
                eTrabajador obj2 = new eTrabajador();
                int fila = 3;
                foreach (int nRow in gvListadoTrabajador.GetSelectedRows())
                {

                    eTrabajador obj = gvListadoTrabajador.GetRow(nRow) as eTrabajador;
                    obj2 = unit.Trabajador.Obtener_Trabajador<eTrabajador>(22, cod_usuario: Program.Sesion.Usuario.cod_usuario, cod_empresa: obj.cod_empresa);
                    empresa = obj.cod_empresa;

                    ruc = obj2.dsc_ruc;
                    trabajador = obj.cod_trabajador + "," + trabajador;

                }
              

                fila = fila + 1;
                procedure = "usp_rhu_Consulta_ListarFormatosTrabajador @opcion = '15', @cod_trabajador_multiple = '" + trabajador +
                                                "', @cod_empresa = '" + empresa + "'";
                unit.Trabajador.pDatosAExcel(cnxl, objExcel, procedure, "Consulta", "A" + fila, true);
                if (fila > 1) objExcel.Rows[fila].Delete();
                fila = objExcel.Cells.Find("*", System.Reflection.Missing.Value,
                System.Reflection.Missing.Value, System.Reflection.Missing.Value, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious, false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

                

                worksheet.Rows(1).Delete();
                objExcel.Range["A3"].Select();
                fila = objExcel.Cells.Find("*", System.Reflection.Missing.Value,
                System.Reflection.Missing.Value, System.Reflection.Missing.Value, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious, false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;
                worksheet.Rows(1).Insert();
                worksheet.Rows(1).Insert();
                fila = fila + 1;
                //int fila = nInLastRow;

                objExcel.Range["A3"].Value = "Codigo";
                objExcel.Range["A3:A3"].Merge();
                objExcel.Range["A3"].Cells.ColumnWidth = 12;
                objExcel.Range["B3"].Value = "Nombre";
                objExcel.Range["B3:B3"].Merge();
                objExcel.Range["B3"].Cells.ColumnWidth = 17.57;
                objExcel.Range["C3"].Value = "Apellido Paterno";
                objExcel.Range["C3:C3"].Merge();
                objExcel.Range["C3"].Cells.ColumnWidth = 15.43;
                objExcel.Range["D3"].Value = "Apellido Materno";
                objExcel.Range["D3:D3"].Merge();
                objExcel.Range["D3"].Cells.ColumnWidth = 16;
                objExcel.Range["E3"].Value = "Tipo Documento";
                objExcel.Range["E3:E3"].Merge();
                objExcel.Range["E3"].Cells.ColumnWidth = 20.86;
                objExcel.Range["F3"].Value = "Nro Documento";
                objExcel.Range["F3:F3"].Merge();
                objExcel.Range["F3"].Cells.ColumnWidth = 14.86;
                objExcel.Range["G3"].Value = "Sexo";
                objExcel.Range["G3:G3"].Merge();
                objExcel.Range["G3"].Cells.ColumnWidth = 14.29;
                objExcel.Range["H3"].Value = "Fecha de Nacimiento";
                objExcel.Range["H3:H3"].Merge();
                objExcel.Range["H3"].Cells.ColumnWidth =19;
                objExcel.Range["I3"].Value = "Telefono Movil";
                objExcel.Range["I3:I3"].Merge();
                objExcel.Range["I3"].Cells.ColumnWidth = 13.71;
                objExcel.Range["J3"].Value = "Telefono Fijo";
                objExcel.Range["J3:J3"].Merge();
                objExcel.Range["G3"].Cells.ColumnWidth = 12;
                objExcel.Range["K3"].Value = "Estado Civil";
                objExcel.Range["K3:K3"].Merge();
                objExcel.Range["K3"].Cells.ColumnWidth = 11;
                objExcel.Range["L3"].Value = "Correo Personal";
                objExcel.Range["L3:L3"].Merge();
                objExcel.Range["L3"].Cells.ColumnWidth = 19.14;
                objExcel.Range["M3"].Value = "Situacion";
                objExcel.Range["M3:M3"].Merge();
                objExcel.Range["M3"].Cells.ColumnWidth = 20.71;
                objExcel.Range["N3"].Value = "Nacionalidad";
                objExcel.Range["N3:N3"].Merge();
                objExcel.Range["N3"].Cells.ColumnWidth = 11.86;
                objExcel.Range["O3"].Value = "Cat Trabajador";
                objExcel.Range["O3:O3"].Merge();
                objExcel.Range["O3"].Cells.ColumnWidth = 21.14;
                objExcel.Range["P3"].Value = "Nivel Educativo";
                objExcel.Range["P3:P3"].Merge();
                objExcel.Range["P3"].Cells.ColumnWidth = 22.14;
                objExcel.Range["Q3"].Value = "Fecha de Ingreso";
                objExcel.Range["Q3:Q3"].Merge();
                objExcel.Range["Q3"].Cells.ColumnWidth = 15.14;
                objExcel.Range["R3"].Value = "Direccion";
                objExcel.Range["R3:AF3"].Merge();
                objExcel.Range["R3:AF3"].WrapText = true;
                objExcel.Range["R4"].Value = "Tipo Via";
                objExcel.Range["R4"].Cells.ColumnWidth = 14.57;
                objExcel.Range["S4"].Value = "Nombre Via";
                objExcel.Range["S4"].Cells.ColumnWidth = 12;
                objExcel.Range["T4"].Value = "Nro";
                objExcel.Range["T4"].Cells.ColumnWidth = 11;
                objExcel.Range["U4"].Value = "Dep";
                objExcel.Range["U4"].Cells.ColumnWidth = 11;
                objExcel.Range["V4"].Value = "Interior";
                objExcel.Range["V4"].Cells.ColumnWidth = 11;
                objExcel.Range["W4"].Value = "Manzana";
                objExcel.Range["W4"].Cells.ColumnWidth = 11;
                objExcel.Range["X4"].Value = "Lote";
                objExcel.Range["X4"].Cells.ColumnWidth = 11;
                objExcel.Range["Y4"].Value = "KM";
                objExcel.Range["Y4"].Cells.ColumnWidth = 11;
                objExcel.Range["Z4"].Value = "Etapa";
                objExcel.Range["Z4"].Cells.ColumnWidth = 11;
                objExcel.Range["AA4"].Value = "Block";
                objExcel.Range["AA4"].Cells.ColumnWidth = 11;
                objExcel.Range["AB4"].Value = "Tipo Zona";
                objExcel.Range["AB4"].Cells.ColumnWidth = 15.57;
                objExcel.Range["AC4"].Value = "Nombre zona";
                objExcel.Range["AC4"].Cells.ColumnWidth = 12.14;
                objExcel.Range["AD4"].Value = "Departamento";
                objExcel.Range["AD4"].Cells.ColumnWidth = 13.14;
                objExcel.Range["AD4"].Columns.NumberFormat = "@";
                objExcel.Range["AD4:AD4000"].Columns.NumberFormat = "@";
                objExcel.Range["AE4"].Value = "Provincia";
                objExcel.Range["AE4"].Cells.ColumnWidth = 11;
                objExcel.Range["AF4"].Value = "Distrito";
                objExcel.Range["AF4"].Cells.ColumnWidth = 11;

                objExcel.Range["AG3"].Value = "Agencia";
                objExcel.Range["AG3:AG3"].Merge();
                objExcel.Range["AG3"].Cells.ColumnWidth = 11;
                objExcel.Range["AH3"].Value = "Seccion";
                objExcel.Range["AH3:AH3"].Merge();
                objExcel.Range["AH3"].Cells.ColumnWidth = 11;
                objExcel.Range["AI3"].Value = "Cargo";
                objExcel.Range["AI3:AI3"].Merge();
                objExcel.Range["AI3"].Cells.ColumnWidth = 11;
                objExcel.Range["AJ3"].Cells.ColumnWidth = 19.43;
                objExcel.Range["AJ3"].Value = "Tipo Trabajador Salud";
                objExcel.Range["AJ3:AJ3"].Merge();
                objExcel.Range["AK3"].Cells.ColumnWidth = 11.71;
                objExcel.Range["AK3"].Value = "Centro Costo";
                objExcel.Range["AK3:AK3"].Merge();
                objExcel.Range["AL3"].Cells.ColumnWidth = 26.71;
                objExcel.Range["AL3"].Value = "Nombre AFP";
                objExcel.Range["AL3:AL3"].Merge();
                objExcel.Range["AM3"].Cells.ColumnWidth = 26.71;
                objExcel.Range["AM3"].Value = "CUSPP";
                objExcel.Range["AM3:AM3"].Merge();
                objExcel.Range["AN3"].Value = "Comision AFP";
                objExcel.Range["AN3"].Cells.ColumnWidth = 13.43;
                objExcel.Range["AN4"].Value = "Comision Flujo /Mixta";
                objExcel.Range["AO3"].Value = "Tipo Contrato";
                objExcel.Range["AO3:AO3"].Merge();
                objExcel.Range["AO3:AO3"].WrapText = true;
                objExcel.Range["AO3"].Cells.ColumnWidth = 19.71;
                objExcel.Range["AP3"].Value = "Regimen Laboral";
                objExcel.Range["AP3:AP3"].Merge();
                objExcel.Range["AP3:AP3"].WrapText = true;
                objExcel.Range["AP3"].Cells.ColumnWidth = 21.86;
                objExcel.Range["AQ3"].Value = "Periodicidad Pago";
                objExcel.Range["AQ3:AQ3"].Merge();
                objExcel.Range["AQ3:AQ3"].WrapText = true;
                objExcel.Range["AQ3"].Cells.ColumnWidth = 16.29;
                objExcel.Range["AR3"].Value = "Moneda";
                objExcel.Range["AR3:AR3"].Merge();
                objExcel.Range["AR3"].Cells.ColumnWidth = 11;
                objExcel.Range["AS3"].Value = "Sueldo";
                objExcel.Range["AS3:AS3"].Merge();
                objExcel.Range["AS3"].Cells.ColumnWidth = 10.14;
                objExcel.Range["AT3"].Value = "Porcentaje Quincena";
                objExcel.Range["AT3:AT3"].Merge();
                objExcel.Range["AT3:AT3"].WrapText = true;
                objExcel.Range["AT3"].Cells.ColumnWidth = 10.14;
                objExcel.Range["AU3"].Value = "Porcentaje Comisión";
                objExcel.Range["AU3:AU3"].Merge();
                objExcel.Range["AU3:AU3"].WrapText = true;
                objExcel.Range["AU3"].Cells.ColumnWidth = 10.14;
                objExcel.Range["AV3"].Value = "Asignacion Familiar";
                objExcel.Range["AV3:AV3"].Merge();
                objExcel.Range["AV3:AV3"].WrapText = true;
                objExcel.Range["AV3"].Cells.ColumnWidth = 10.14;
                objExcel.Range["AW3"].Value = "Tipo Pago";
                objExcel.Range["AW3:AW3"].Merge();
                objExcel.Range["AW3"].Cells.ColumnWidth = 18;
                objExcel.Range["AX3"].Value = "Pago Remuneraciones";
                objExcel.Range["AX3:BA3"].Merge();
                objExcel.Range["AX3:BA3"].WrapText = true;
                objExcel.Range["AX4"].Value = "Entidad Financiera";
                objExcel.Range["AX4"].Cells.ColumnWidth = 16.71;
                objExcel.Range["AY4"].Value = "Tipo Cuenta";
                objExcel.Range["AY4"].Cells.ColumnWidth = 11.29;
                objExcel.Range["AZ4"].Value = "Nro Cuenta";
                objExcel.Range["AZ4"].Cells.ColumnWidth = 12;
                objExcel.Range["BA4"].Value = "Nro. Cuenta Interbancaria";
                objExcel.Range["BA4"].WrapText = true;
                objExcel.Range["BA4"].Cells.ColumnWidth = 12;
                objExcel.Range["BB3"].Value = "Regimen de Aseguramiento Salud";
                objExcel.Range["BB3:BB3"].Merge();
                objExcel.Range["BB3"].Cells.ColumnWidth = 35.29;
                objExcel.Range["BB3:BB3"].WrapText = true;
                objExcel.Range["BC3"].Value = "Situacion del Trabajador";
                objExcel.Range["BC3:BC3"].Merge();
                objExcel.Range["BC3:BC3"].WrapText = true;
                objExcel.Range["BC3"].Cells.ColumnWidth = 21.86;
                objExcel.Range["BD3"].Value = "Tipo Trabajador";
                objExcel.Range["BD3:BD3"].Merge();
                objExcel.Range["BD3:BD3"].WrapText = true;
                objExcel.Range["BD3"].Cells.ColumnWidth = 27.43;
                objExcel.Range["BE3"].Value = "Categoria Ocupacional";
                objExcel.Range["BE3:BE3"].Merge();
                objExcel.Range["BE3:BE3"].WrapText = true;
                objExcel.Range["BE3"].Cells.ColumnWidth = 27.43;
                objExcel.Range["BF3"].Value = "Calificación de Puesto";
                objExcel.Range["BF3:BF3"].Merge();
                objExcel.Range["BF3:BF3"].WrapText = true;
                objExcel.Range["BF3"].Cells.ColumnWidth = 23;
                objExcel.Range["BG3"].Value = "Fecha de Asignación de Calificación de Puesto";
                objExcel.Range["BG3:BG3"].Merge();
                objExcel.Range["BG3:BG3"].WrapText = true;
                objExcel.Range["BG3"].Cells.ColumnWidth = 23;
                objExcel.Range["BH3"].Value = "SCTR Salud";
                objExcel.Range["BH3:BH3"].Merge();
                objExcel.Range["BH3:BH3"].WrapText = true;
                objExcel.Range["BH3"].Cells.ColumnWidth = 11;
                objExcel.Range["BI3"].Value = "SCTR Pension";
                objExcel.Range["BI3:BI3"].Merge();
                objExcel.Range["BI3:BI3"].WrapText = true;
                objExcel.Range["BI3"].Cells.ColumnWidth = 12.14;
                objExcel.Range["BJ3"].Value = "Fecha Inicio Contrato";
                objExcel.Range["BJ3:BJ3"].Merge();
                objExcel.Range["BJ3:BJ3"].WrapText = true;
                objExcel.Range["BJ3"].Cells.ColumnWidth = 13.71;
                objExcel.Range["BK3"].Value = "Fecha Fin Contrato";
                objExcel.Range["BK3:BK3"].Merge();
                objExcel.Range["BK3:BK3"].WrapText = true;
                objExcel.Range["BK3"].Cells.ColumnWidth = 13.57;
                objExcel.Range["BL3"].Value = "Correo Laboral";
                objExcel.Range["BL3:BL3"].Merge();
                objExcel.Range["BL3:BL3"].WrapText = true;
                objExcel.Range["BL3"].Cells.ColumnWidth = 16.71;
                objExcel.Range["BM3"].Value = "Pago CTS";
                objExcel.Range["BM3:BQ3"].Merge();
                objExcel.Range["BM3:BQ3"].WrapText = true;
                objExcel.Range["BM4"].Value = "Moneda";
                objExcel.Range["BM4"].Cells.ColumnWidth = 11;
                objExcel.Range["BN4"].Value = "Entidad Financiera";
                objExcel.Range["BN4"].Cells.ColumnWidth = 16.71;
                objExcel.Range["BO4"].Value = "Tipo Cuenta";
                objExcel.Range["BO4"].Cells.ColumnWidth = 13.57;
                objExcel.Range["BP4"].Value = "Nro Cuenta";
                objExcel.Range["BP4"].Cells.ColumnWidth = 13.57;
                objExcel.Range["BQ4"].Value = "Nro. Cuenta Interbancaria";
                objExcel.Range["BQ4"].WrapText = true;
                objExcel.Range["BQ4"].Cells.ColumnWidth = 13.57;
                objExcel.Range["BR3"].Value = "File Number";
                objExcel.Range["BR3:BR3"].Merge();
                objExcel.Range["BR3:BR3"].WrapText = true;
                objExcel.Range["BR4"].Cells.ColumnWidth = 16.71;
                objExcel.Range["BS3"].Value = "CENTRO DE RIESGO";
                objExcel.Range["BS3:BS3"].Merge();
                objExcel.Range["BS3:BS3"].WrapText = true;
                objExcel.Range["BS4"].Cells.ColumnWidth = 13.57;
                objExcel.Range["BT3"].Value = "AFECTO SCTR";
                objExcel.Range["BT3:BT3"].Merge();
                objExcel.Range["BT3:BT3"].WrapText = true;
                objExcel.Range["BT3"].Cells.ColumnWidth = 13.57;
                objExcel.Range["BU3"].Value = "AFECTO VIDA LEY";
                objExcel.Range["BU3:BU3"].Merge();
                objExcel.Range["BU3:BU3"].WrapText = true;
                objExcel.Range["BU3"].Cells.ColumnWidth = 13.57;
                objExcel.Range["BV3"].Value = "HORARIO NOCTURNO ?";
                objExcel.Range["BV3:BV3"].Merge();
                objExcel.Range["BV3:BV3"].WrapText = true;
                objExcel.Range["BV3"].Cells.ColumnWidth = 13.57;
                objExcel.Range["BW3"].Value = "HORAS EXTRAS (AUTORIZACIÓN)";
                objExcel.Range["BW3:BW3"].Merge();
                objExcel.Range["BW3:BW3"].WrapText = true;
                objExcel.Range["BW3"].Cells.ColumnWidth = 15.29;
                objExcel.Range["BX3"].Value = "CIA SEGURO VIDA LEY";
                objExcel.Range["BX3:BX3"].Merge();
                objExcel.Range["BX3:BX3"].WrapText = true;
                objExcel.Range["BX3"].Cells.ColumnWidth = 13.57;
                objExcel.Range["BY3"].Value = "CÓDIGO DE EPS";
                objExcel.Range["BY3:BY3"].Merge();
                objExcel.Range["BY3:BY3"].WrapText = true;
                objExcel.Range["BY3"].Cells.ColumnWidth = 13.57;
                objExcel.Range["BZ3"].Value = "RUC DE LA EPS";
                objExcel.Range["BZ3:BZ3"].Merge();
                objExcel.Range["BZ3:BZ3"].WrapText = true;
                objExcel.Range["BZ3"].Cells.ColumnWidth = 13.57;
                ////objExcel.Range["R1:R2"].Value = "Telefono Fijo";
                ////objExcel.Range["S1:S2"].Value = "Telefono Fijo";
                ////objExcel.Range["T1:T2"].Value = "Telefono Fijo";
                ////objExcel.Range["U1:U2"].Value = "Telefono Fijo";
                ////objExcel.Range["V1:V2"].Value = "Telefono Fijo";
                ////objExcel.Range["W1:W2"].Value = "Telefono Fijo";
                ////objExcel.Range["X1:X2"].Value = "Telefono Fijo";
                ////objExcel.Range["Y1:Y2"].Value = "Telefono Fijo";
                ////objExcel.Range["Z1:Z2"].Value = "Telefono Fijo";
                ////objExcel.Range["AA1:AA2"].Value = "Telefono Fijo";

                objExcel.Range["A3:BZ3"].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                objExcel.Range["A3:BZ3"].Borders.Color = System.Drawing.Color.FromArgb(0, 0, 0);
                                 
                objExcel.Range["A3"].RowHeight = 30;
                objExcel.Range["A4"].RowHeight = 30;
                objExcel.Range["A3:BZ" + fila].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                objExcel.Range["A3:BZ" + fila].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                objExcel.Range["A3:BZ3" + fila].WrapText = true;

                objExcel.Range["A4:BZ" + fila].Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlDot;
                objExcel.Range["A4:BZ" + fila].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
                objExcel.Range["A4:BZ" + fila].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlDot;
                objExcel.Range["A4:BZ" + (fila + 1)].Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlDot;
                objExcel.Range["A4:BZ" + fila].Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlDot;
                objExcel.Range["A4:BZ" + fila].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDot;
                objExcel.Range["A4:BZ5"].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                objExcel.Range["A4:BZ5"].Borders.Color = System.Drawing.Color.FromArgb(0, 0, 0);


                objExcel.Range["A4:BZ" + fila].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                objExcel.Range["A4:BZ" + fila].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                objExcel.Range["A4:BZ2" + fila].WrapText = true;

                objExcel.Range["A4:BZ4"].Select();
                //objExcel.Selection.Borders.Color = System.Drawing.Color.FromArgb(0, 0, 0);
                //objExcel.Selection.Font.Bold = true;
                //objExcel.Selection.Font.Color = System.Drawing.Color.White;
                //objExcel.Selection.Interior.Color = System.Drawing.ColorTranslator.FromHtml("#598B7D");

                //objExcel.Range["A1:BX"].Select();
                //objExcel.Selection.Borders.Color = System.Drawing.Color.FromArgb(0, 0, 0);
                //objExcel.Selection.Font.Bold = true;
                //objExcel.Selection.Font.Color = System.Drawing.Color.Black;
                //objExcel.Selection.Interior.Color = System.Drawing.ColorTranslator.FromHtml("#FFC000");
                //objExcel.Range["R1:AF1" + fila].Value = "Direccion";
                //objExcel.Range["A1:AR" + fila].Font.Size = 10;
                //objExcel.Range["A1:BX1"].AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                //objExcel.Range["A1"].Select();
                //Excel.Worksheet worksheet1 = workbook.Worksheets[1];
                
                sheet.Delete();
                objExcel.WindowState = Excel.XlWindowState.xlMaximized;
                objExcel.Visible = true;
                objExcel = null/* TODO Change to default(_) if this is not a reference type */;
                SplashScreenManager.CloseForm();

            }
            catch (Exception ex)
            {
                System.Threading.Thread.CurrentThread.Abort();
                objExcel.ActiveWorkbook.Saved = true;
                objExcel.ActiveWorkbook.Close();
                objExcel = null/* TODO Change to default(_) if this is not a reference type */;
                objExcel.Quit();
                SplashScreenManager.CloseForm();
                MessageBox.Show(ex.Message.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }

        private void btnCargoEmpresa_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Application.OpenForms["frmCargoArea"] != null)
            {
                Application.OpenForms["frmCargoArea"].Activate();
            }
            else
            {
                var tool = new Tools.TreeListHelper(treeFiltroEmpresa);
                frmCargoArea frm = new frmCargoArea();
                frm.MiAccion = Area.Nuevo;
                frm.cod_empresa = tool.ObtenerCodigoConcatenadoDeNodoIndex(0);
                frm.ShowDialog();
            }
        }

        private void gvListadoTrabajador_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {


            try
            {
                if (e.RowHandle >= 0)
                {
                    eTrabajador obj = gvListadoTrabajador.GetRow(e.RowHandle) as eTrabajador;
                    if (obj == null) return;
                    if (e.Column.FieldName == "fch_ingreso" && obj.fch_ingreso.ToString().Contains("0001")) e.DisplayText = "";
                    if (e.Column.FieldName == "fch_vencimiento" && obj.fch_vencimiento.ToString().Contains("0001")) e.DisplayText = "";
                    if (e.Column.FieldName == "fch_entrega_uniforme" && obj.fch_entrega_uniforme.ToString().Contains("0001")) e.DisplayText = "";
                    if (e.Column.FieldName == "Fechabaja" && obj.Fechabaja.ToString().Contains("0001")) e.DisplayText = "";
                    if (e.Column.FieldName == "fch_firma" && obj.fch_firma.ToString().Contains("0001")) e.DisplayText = "";
                    if (e.Column.FieldName == "dsc_diasvencimiento") e.DisplayText = "";
                    if (e.Column.FieldName == "flg_sexo") e.DisplayText = "";
                    // if (e.Column.FieldName == "flg_sexo" && obj.flg_sexo.ToString().Contains("F")) e.
                    if (obj.flg_activo == "NO") e.Appearance.ForeColor = Color.Red;
                    //DateTime hoy = DateTime.Today;
                    //DateTime Fecha = Convert.ToDateTime(obj.fch_nacimiento);
                    //int dia = Fecha.Day;
                    //int mesn = Fecha.Month;
                    //int ano = Fecha.Year;
                    if (e.Column.FieldName == "fch_nacimiento" && (obj.fch_nacimiento.Day == DateTime.Today.Day && obj.fch_nacimiento.Month == DateTime.Today.Month))
                    {
                        /*e.Handled = true; */
                        e.Graphics.DrawImage(ImgCumple, new Rectangle(e.Bounds.X + 14, e.Bounds.Y + (e.Bounds.Height / 2) - 8, 16, 16));
                    }

                    if (e.Column.FieldName == "dsc_diasvencimiento")
                    {
                        Brush b; e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        int cellValue = Convert.ToInt32(e.CellValue);
                        b = cellValue > 30 ? ConCriterios : cellValue > 5 && cellValue <= 30 ? NAplCriterio : SinCriterios;
                        e.Graphics.FillEllipse(b, new Rectangle(e.Bounds.X + 12, e.Bounds.Y + 1, markWidth, markWidth));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        public void mostrardatos()
        {
            eTrabajador obj = gvListadoTrabajador.GetFocusedRow() as eTrabajador;
            if (obj == null) return;
            txtNombres.Text = obj.dsc_nombres;
            txtApellidos.Text = obj.dsc_apellido_materno + " " + obj.dsc_apellido_paterno;
            //txtcodigo.Text = obj.cod_trabajador;
            txtdireccion.Text = obj.dsc_direccion;
            txtprovincia.Text = obj.dsc_provincia;
            txtdistrito.Text = obj.dsc_distrito;
            txtnumero.Text = obj.dsc_documento;
            txttipo.Text = obj.dsc_tipo_documento;
            txtSistPenAFP.Text = obj.cod_sist_pension;
            txtCts.Text = obj.dsc_banco_CTS;
            txtbancosispafp.Text = obj.dsc_APF;
            txtSispensionario.Text = obj.dsc_banco;
            //  txtCargo.Text = obj.dsc_cargo;

            if (obj.cod_TipoPersonal == "DESTACADO")
            {
                txtestacado.Text = "SI";
                // txtFchUniforme.Text = Convert.ToDateTime(obj.fch_entrega_uniforme);
            }
            else if (obj.cod_TipoPersonal == "OFICINA")
            {
                txtestacado.Text = "NO";
            }
            //string codigo = "";
            //eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, cod_empresa);
            eTrabajador etra = unit.Trabajador.ObtenerUltimoCargo<eTrabajador>(2, obj.cod_trabajador, obj.cod_empresa, "");
            //codigo = etra.cod_cargo;
            //eTrabajador cod = unit.Trabajador.ObtenerUltimoCargo<eTrabajador>(3, "", obj.cod_empresa, etra == null ? "" : etra.cod_cargo); //codigo);
            txtCargo.Text = etra == null ? "SIN CARGO ASIGNADO" : etra.dsc_cargo;
            ListContactos = unit.Trabajador.ListarTrabajadores<eTrabajador.eContactoEmergencia_Trabajador>(3, obj.cod_trabajador, obj.cod_empresa);
            bsListaContactoEme.DataSource = ListContactos; gvPersonal.RefreshData();

            ListInfoLaboral = unit.Trabajador.ListarEMO<eTrabajador.eInfoLaboral_Trabajador>(4, obj.cod_trabajador, obj.cod_empresa);
            bsInformacionLaboral.DataSource = ListInfoLaboral; gvInfoLaboral.RefreshData();

            listaDocumetnos = unit.Trabajador.ListaDocuemtos<eFormatoMD_General.eFormatoDocumento>(4, obj.cod_empresa, obj.cod_trabajador);
            bsDocumentos.DataSource = listaDocumetnos;

            if (obj.flg_sexo == "F")
            {

                Image imgEmpresaLarge = Properties.Resources.female64;
                pct_foto.EditValue = imgEmpresaLarge;

            }
            else if (obj.flg_sexo == "M")
            {
                Image imgEmpresaLarge = Properties.Resources.Male64;
                pct_foto.EditValue = imgEmpresaLarge;
            }




        }



        private void gvListadoTrabajador_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvListadoTrabajador_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
            GridView view = sender as GridView;
            if (view.Columns["flg_activo"] != null)
            {
                string estado = view.GetRowCellDisplayText(e.RowHandle, view.Columns["flg_activo"]);
                if (estado == "NO") e.Appearance.ForeColor = Color.Red;
            }
        }

        void OnNodeCellStyle(object sender, GetCustomNodeCellStyleEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                e.Appearance.FontSizeDelta += 1;
                e.Appearance.FontStyleDelta = FontStyle.Bold;
            }
            if (e.Node.Level == 1 && e.Node.Nodes.Count > 0)
                e.Appearance.FontStyleDelta = FontStyle.Bold;
        }
        void OnBeforeFocusNode(object sender, BeforeFocusNodeEventArgs e)
        {
            e.CanFocus = false;
        }




        private void btnAreaEmpresa_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Application.OpenForms["frmAreaEmpresa"] != null)
            {
                Application.OpenForms["frmAreaEmpresa"].Activate();
            }
            else
            {
                var tool = new Tools.TreeListHelper(treeFiltroEmpresa);
                frmAreaEmpresa frm = new frmAreaEmpresa();
                frm.MiAccion = Area.Nuevo;
                frm.cod_empresa = tool.ObtenerCodigoConcatenadoDeNodoIndex(0);
                frm.ShowDialog();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            CargarListado();
        }


        private void gvListadoTrabajador_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.CellValue == null) return;
            //DateTime hoy = DateTime.Today;
            eTrabajador obj = gvListadoTrabajador.GetFocusedRow() as eTrabajador;
            if (obj == null) return;
            //DateTime Fecha = Convert.ToDateTime(obj.fch_nacimiento);

            //int dia = Fecha.Day;
            //int mesn = Fecha.Month;
            //int ano = Fecha.Year;

            if (e.RowHandle >= 0)
            {
                if (e.RowHandle != GridControl.NewItemRowHandle && e.Column.FieldName == "flg_sexo" && e.CellValue.ToString() == "F" )
                {
                    e.RepositoryItem = repositoryItemTextEdit1;
                    e.Column.Name = "";


                }
                else if (e.RowHandle != GridControl.NewItemRowHandle && e.Column.FieldName == "flg_sexo" && e.CellValue.ToString() == "M")
                {
                    e.RepositoryItem = repositoryItemTextEdit3;
                    e.Column.Name = "";

                }

                //if (e.RowHandle != GridControl.NewItemRowHandle && e.Column.FieldName == "fch_nacimiento" && dia == hoy.Day && mesn == hoy.Month)
                //{
                //    e.RepositoryItem = repositoryItemTextEdit2;
                //}
            }


        }
        //repositoryItemTextEdit1
        int ctd_empresas = 0;




        bool ValidarAtributos<T>(List<T> objList)
        {
            bool flag = true;
            objList.ForEach((obj) =>
            {
                var properties = obj.GetType().GetTypeInfo().GetProperties().ToList();
                properties.ForEach((k) =>
                {
                    var value = k.GetValue(obj);
                    if (value == null)
                    {
                        // hacer algo
                        MessageBox.Show("esta nulo");
                        flag = false;
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        // hacer algo
                        MessageBox.Show("esta nulo");
                        flag = false;
                        return;
                    }
                });
            });
            return flag;

        }

        private void officeNavigationBar1_QueryPeekFormContent(object sender, DevExpress.XtraBars.Navigation.QueryPeekFormContentEventArgs e)
        {
            // if (e.Item == navigationBarItem1) e.Control = new XtraUserControl1();
        }

        private void xtraUserControl1_Load(object sender, EventArgs e)
        {

        }

        private void TabGeneral_Click(object sender, EventArgs e)
        {

        }

        private void gvPersonal_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void btnCorreo_Click(object sender, EventArgs e)
        {
            eTrabajador obj = gvListadoTrabajador.GetFocusedRow() as eTrabajador;

            if (obj.dsc_mail_1 != null)
            {
                txtdescripcion.Text = obj.dsc_mail_1 + "-" + obj.dsc_mail_2;
            }
            else
            {
                txtdescripcion.Text = obj.dsc_mail_1;
            }
            // txtdescripcion.Text = "";
        }

        private void btncelular_Click(object sender, EventArgs e)
        {
            eTrabajador obj = gvListadoTrabajador.GetFocusedRow() as eTrabajador;

            if (obj.dsc_telefono != null)
            {
                txtdescripcion.Text = obj.dsc_celular;
            }
            else
            {
                txtdescripcion.Text = obj.dsc_telefono + "/" + obj.dsc_celular;
            }
            // txtdescripcion.Text = "";
        }

        private void btnEmpresa_Click(object sender, EventArgs e)
        {
            eTrabajador obj = gvListadoTrabajador.GetFocusedRow() as eTrabajador;

            if (obj.dsc_sede_empresa != null)
            {
                txtdescripcion.Text = obj.dsc_empresa;
            }
            else
            {
                txtdescripcion.Text = obj.dsc_empresa + "-" + obj.dsc_sede_empresa;
            }
        }

        private void btnDireccion_Click(object sender, EventArgs e)
        {
            eTrabajador obj = gvListadoTrabajador.GetFocusedRow() as eTrabajador;
            txtdescripcion.Text = obj.dsc_direccion + "-" + obj.cod_provincia + "-" + obj.cod_distrito;
        }

        private void btnCumpleanos_Click(object sender, EventArgs e)
        {
            eTrabajador obj = gvListadoTrabajador.GetFocusedRow() as eTrabajador;
            DateTime Fecha = Convert.ToDateTime(obj.fch_nacimiento);

            int dia = Fecha.Day;
            int mesn = Fecha.Month;
            int ano = Fecha.Year;


            string mes = "";
            switch (mesn)
            {
                case (1):
                    mes = "Enero";
                    break;
                case (2):
                    mes = "Febrero";
                    break;
                case (3):
                    mes = "Marzo";
                    break;
                case (4):
                    mes = "Abril";
                    break;
                case (5):
                    mes = "Mayo";
                    break;
                case (6):
                    mes = "Junio";
                    break;
                case (7):
                    mes = "Julio";
                    break;
                case (8):
                    mes = "Agosto";
                    break;
                case (9):
                    mes = "Setiembre";
                    break;
                case (10):
                    mes = "Octubre";
                    break;
                case (11):
                    mes = "Noviembre";
                    break;
                case (12):
                    mes = "Diciembre";
                    break;
            }


            txtdescripcion.Text = dia + " de " + mes + " del " + ano;


            DateTime hoy = DateTime.Today;
            if (dia == hoy.Day && mesn == hoy.Month)
            {

                txtdescripcion.Text = "Hoy es su cumpleaños";
            }
            else
            {
                txtdescripcion.Text = dia + " de " + mes + " del " + ano;
            }



        }

        private void gvListadoTrabajador_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                mostrardatos(); //ObtenerDatos_InfoLaboral();
                txtdescripcion.Text = "Seleccione una opción";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void gvListadoTrabajador_RowClick_1(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 1 && e.RowHandle >= 0) gvListadoTrabajador_FocusedRowChanged(gvListadoTrabajador, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(gvListadoTrabajador.FocusedRowHandle - 1, gvListadoTrabajador.FocusedRowHandle));

                if (e.Clicks == 2 && e.RowHandle >= 0)
                {
                    eTrabajador obj = gvListadoTrabajador.GetFocusedRow() as eTrabajador;
                    if (obj == null) return;
                    frmMantTrabajador frm = new frmMantTrabajador(this);
                    frm.MiAccion = Trabajador.Editar;
                    frm.cod_trabajador = obj.cod_trabajador;
                    frm.cod_empresa = obj.cod_empresa;
                    frm.fch_scrt = obj.fch_firma;
                    frm.ShowDialog();
                    if (frm.ActualizarListado == "SI") frmListadoTrabajador_KeyDown(null, new KeyEventArgs(Keys.F5));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        public class frmVistaTrabajor : XtraUserControl
        {

        }

        private void officeNavigationBar1_SelectedItemChanged(object sender, DevExpress.XtraBars.Navigation.NavigationBarItemEventArgs e)
        {
            try
            {
                switch (e.Item.Name)
                {
              
                    case "navigationBarItem2":
                        navigationFrame1.SelectedPage = NavPersonal;

                        break;
                    case "navigationBarItem1":
                        navigationFrame1.SelectedPage = NavLaboral;

                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvInfoLaboral_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void gvInfoLaboral_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

        }

        private void gvInfoLaboral_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);

        }

        private void chkSeleccionMultiple_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnOcultarFiltro_ItemClick(object sender, ItemClickEventArgs e)
        {
            //navFilter.OptionsNavPane.NavPaneState = NavPaneState.Expanded;
            //navFilter.OptionsNavPane.NavPaneState = NavPaneState.Collapsed;
            //navFilter.OptionsNavPane.CollapsedWidth = 50;
            //navFilter.OptionsNavPane.ExpandedWidth = 200;

            if (layoutFiltro.ContentVisible == true)
            {
                layoutFiltro.ContentVisible = false;
                layoutFiltro.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                Image img = ImageResourceCache.Default.GetImage("images/snap/quickfilter_32x32.png");
                btnOcultarFiltro.ImageOptions.LargeImage = img;
                btnOcultarFiltro.Caption = "Mostrar Filtro";

                return;
            }
            if (layoutFiltro.ContentVisible == false)
            {
                layoutFiltro.ContentVisible = true;
                layoutFiltro.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                Image img = ImageResourceCache.Default.GetImage("images/filter/ignoremasterfilter_32x32.png");
                btnOcultarFiltro.ImageOptions.LargeImage = img;
                btnOcultarFiltro.Caption = "Ocultar Filtro";
                NavBarEstado.Width = 180;
                return;
            }



        }

        private void btnVistaDetallada_ItemClick(object sender, ItemClickEventArgs e)
        {


            if (splitContainerControl1.Panel2.Visible == false)
            {
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
                gvListadoTrabajador.OptionsSelection.MultiSelect = false; return;
                return;
            }
            else
            {
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel1;
                gvListadoTrabajador.OptionsSelection.MultiSelect = false; return;
                return;
            }
        }

        private void gcListadoTrabajador_Click(object sender, EventArgs e)
        {

        }


        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {



        }
        private void marcartodo()
        {
            BarCheckItem chec = new BarCheckItem();

            switch (chec.Checked)
            {
                case true:
                    chkFormatoE4.Checked = true;
                    break;
                case false:
                    chkFormatoE4.Checked = false;
                    break;
            }
        }





       
        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (Application.OpenForms["frmVistaUsuario"] != null)
            {
                Application.OpenForms["frmVistaUsuario"].Activate();
            }
            else
            {
                frmVistaUsuario frm = new frmVistaUsuario();
                frm.cod_empresa = "00001";
                //frm.MiAccion = Trabajador.Nuevo;
                //frm.user = user;
                //frm.colorVerde = colorVerde;
                //frm.colorPlomo = colorPlomo;
                //frm.colorEventRow = colorEventRow;
                //frm.colorFocus = colorFocus;
                frm.ShowDialog();
                if (frm.ActualizarListado == "SI") { frmListadoTrabajador_KeyDown(null, new KeyEventArgs(Keys.F5)); }
            }
        }


        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (splitContainerControl1.Panel2.Visible == false)
            {
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
                gvListadoTrabajador.OptionsSelection.MultiSelect = false; return;
            }
            else
            {
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel1;
                gvListadoTrabajador.OptionsSelection.MultiSelect = false; return;
            }
        }

        private void navFilter_MouseClick(object sender, MouseEventArgs e)
        {
            refreshTreeView();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarListado();
        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //if (Application.OpenForms["frmMantTrabajador"] != null)
            //{
            //    Application.OpenForms["frmMantTrabajador"].Activate();
            //}
            //else
            {
                frmMantTrabajador frm = new frmMantTrabajador(this);
                frm.cod_usuario = cod_usuario.ToString();
                frm.MiAccion = Trabajador.Nuevo;
                frm.ShowDialog();
                if (frm.ActualizarListado == "SI") frmListadoTrabajador_KeyDown(null, new KeyEventArgs(Keys.F5));
                //s
            }
        }

       




        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Application.OpenForms["frmRegistroRapido"] != null)
            {
                Application.OpenForms["frmRegistroRapido"].Activate();
            }
            else
            {
                frmRegistroRapido frm = new frmRegistroRapido();
                // frm.MiAccion = Area.Nuevo;
                frm.ShowDialog();
                if (frm.ActualizarListado == "SI") frmListadoTrabajador_KeyDown(null, new KeyEventArgs(Keys.F5));
            }
        }



        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Application.OpenForms["frmMantTrabajador"] != null)
            {
                Application.OpenForms["frmMantTrabajador"].Activate();
            }
            else
            {
                frmMantTrabajador frm = new frmMantTrabajador();
                // frm.MiAccion = Area.Nuevo;
                frm.ShowDialog();
                if (frm.ActualizarListado == "SI") frmListadoTrabajador_KeyDown(null, new KeyEventArgs(Keys.F5));
            }
        }

        private void btnDisplayArchivoPlame_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void navBarGroupControlContainer2_Click(object sender, EventArgs e)
        {

        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnbusca_Click(object sender, EventArgs e)
        {
        
            CargarListado();
        }

        private void Mostrar_Formatos(TipoSeguimientoGenerar tipo)
        {
            string codTrabajadorMultiple = "delete";
            string codEmpresaMultiple = "";
            foreach (var nRow in gvListadoTrabajador.GetSelectedRows())
            {
                var objTrab = gvListadoTrabajador.GetRow(nRow) as eTrabajador;
                codTrabajadorMultiple += $",{objTrab.cod_trabajador}";
                codEmpresaMultiple = objTrab.cod_empresa;
            }

            codTrabajadorMultiple = codTrabajadorMultiple.Replace("delete,", "").Trim();
            codTrabajadorMultiple = codTrabajadorMultiple.Replace("delete", "").Trim();
            //AbrirDoumentoFormatoParaImprimir(dsc_trabajador: "Selección Múltiple", cod_trabajador: split, codEmpresa);



            var frm = new FormatoDocumento_Seguimiento.frmFormatoSeguimiento_Generar(tipo);
            frm.Text = "Generar Seguimiento de Formatos";
            frm.CargarFormatoDocumentos(codEmpresaMultiple, codTrabajadorMultiple);
            frm.ShowDialog();
        }
        private void btnGestionarFormatoSeguimiento_ItemClick(object sender, ItemClickEventArgs e)
        {
            Mostrar_Formatos(TipoSeguimientoGenerar.General);
        }



        private void btnGenerarFormatoAlta_ItemClick(object sender, ItemClickEventArgs e)
        {
            Mostrar_Formatos(TipoSeguimientoGenerar.Asignado);
        }



        private void btnAdjuntarFormatoPDF_ItemClick(object sender, ItemClickEventArgs e)
        {
            var obj = gvListadoTrabajador.GetFocusedRow() as eTrabajador;
            if (obj == null) return;



            var frm = new FormatoDocumento_Seguimiento.frmFormato_Adjuntar();
            frm.Text = "Adjuntar Formatos";
            if (frm.CargarDocumentos(obj.dsc_documento) == DialogResult.OK) { frm.ShowDialog(); }
            else { frm.Dispose(); }
        }

        private  void btnCargaMasivaEMO_ItemClick(object sender, ItemClickEventArgs e)
        {
            string empresas = "";
            string sedes = "";
            // await AdjuntarDocumentosVarios(8, "EMO");
            frmCargaMasivaEMO frmemo = new frmCargaMasivaEMO();
            var tool = new Tools.TreeListHelper(treeFiltroEmpresa);
             frmemo.cod_empresa = tool.ObtenerCodigoConcatenadoDeNodoIndex(0);
            frmemo.empresas = tool.ObtenerCodigoConcatenadoDeNodoIndex(0);
            frmemo.sedes = tool.ObtenerCodigoConcatenadoDeNodoIndex(1);

            frmemo.ShowDialog();


        }

       
        static void Appl()
        {
            _clientApp = PublicClientApplicationBuilder.Create(ClientId)
                .WithAuthority($"{Instance}{TenantId}")
                .WithDefaultRedirectUri()
                .Build();
            TokenCacheHelper.EnableSerialization(_clientApp.UserTokenCache);
        }

 

        private static string ClientId = "";
        private static string TenantId = "";
        private static string Instance = "https://login.microsoftonline.com/";
        public static IPublicClientApplication _clientApp;
        public static IPublicClientApplication PublicClientApp { get { return _clientApp; } }

        private void btnNuevoTrabajador_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Application.OpenForms["frmMantTrabajador"] != null)
            {
                Application.OpenForms["frmMantTrabajador"].Activate();
            }
            else
            {
                frmMantTrabajador frm = new frmMantTrabajador();
                eTrabajador obj = gvListadoTrabajador.GetFocusedRow() as eTrabajador;
                frm.MiAccion = Trabajador.Nuevo;
                frm.cod_empresa = obj.cod_empresa;
                frm.ShowDialog();
                if (frm.ActualizarListado == "SI") frmListadoTrabajador_KeyDown(null, new KeyEventArgs(Keys.F5));
            }
        }

        private void gvInfoLaboral_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    eTrabajador obj = gvInfoLaboral.GetRow(e.RowHandle) as eTrabajador;
                    if (obj == null) return;
                    if (e.Column.FieldName == "Fechabaja" && obj.Fechabaja.ToString().Contains("0001")) e.DisplayText = "";
                    if (e.Column.FieldName == "fch_firma" && obj.fch_firma.ToString().Contains("0001")) e.DisplayText = "";
                    if (e.Column.FieldName == "fch_vencimiento" && obj.fch_vencimiento.ToString().Contains("0001")) e.DisplayText = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnImprimir_ItemClick(object sender, ItemClickEventArgs e)
        {
            gvListadoTrabajador.ShowRibbonPrintPreview();
        }

        private void btnVigenciaContrato_ItemClick(object sender, ItemClickEventArgs e)
        {

            string empresa = "";
            string trabajador = "";
            string nombrecompleto = "";

            int emo = 0;

            foreach (int nRow in gvListadoTrabajador.GetSelectedRows())
            {

                eTrabajador obj = gvListadoTrabajador.GetRow(nRow) as eTrabajador;
                empresa = obj.cod_empresa;
                nombrecompleto = obj.dsc_nombres_completos;
                trabajador = obj.cod_trabajador + "," + trabajador;
            }
            //ListarContrato.Add(new eTrabajador() { cod_trabajador=obj});

            frmVigenciaContrato frm = new frmVigenciaContrato();
            string result = "";
            eTrabajador trab = new eTrabajador();

                frm.estado = "NO"; frm.MiAccion = Cese.Nuevo;
                frm.empresa = empresa;
                frm.trabajador = trabajador;
                frm.ShowDialog();
                frm.bsTrabajador.Clear();
                if (frm.ActualizarListado == "SI") frmListadoTrabajador_KeyDown(null, new KeyEventArgs(Keys.F5));

        }

        private void btnFormatoContable_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmListadoContabilidad frm = new frmListadoContabilidad();
            frm.Show();
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            //formName = "Solicitud de Vacaciones";
            //if (Application.OpenForms["frmSolicitudUsuario"] != null)
            //{
            //    Application.OpenForms["frmSolicitudUsuario"].Activate();
            //}
            //else
            //{
            //    Vacaciones_Permisos.frmSolicitudUsuario frm = new Vacaciones_Permisos.frmSolicitudUsuario();
            //    frm.Show();
            //}
        }

        private void barButtonItem14_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            formName = "Carga Masiva";
            if (Application.OpenForms["frmCargaMasiva"] != null)
            {
                Application.OpenForms["frmCargaMasiva"].Activate();
            }
            else
            {
                frmCargaMasiva frm = new frmCargaMasiva();
                frm.Show();
            }
            

        }

        private async Task AdjuntarDocumentosVarios(int opcionDoc, string nombreDoc, string nombreDocAdicional = "")
        {
            string cod_empresa = "";
            string empresas = "";
            string sedes = "";
            string estado = "SI";

            var tool = new Tools.TreeListHelper(treeFiltroEmpresa);
            cod_empresa = tool.ObtenerCodigoConcatenadoDeNodoIndex(0);
            empresas = tool.ObtenerCodigoConcatenadoDeNodoIndex(0);
            sedes = tool.ObtenerCodigoConcatenadoDeNodoIndex(1);
            try
            {
                DateTime FechaRegistro = DateTime.Today;
                string nombreCarpeta = "";

                //eTrabajador resultado = unit.Trabajador.Obtener_Trabajador<eTrabajador>(2, cod_trabajador, cod_empresa);
                //if (resultado == null) { MessageBox.Show("Antes de adjuntar los docuentos debe crear al trabajador.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
               
                OpenFileDialog myFileDialog = new OpenFileDialog();
                myFileDialog.Filter = "Archivos (*.pdf)|; *.pdf";
                myFileDialog.FilterIndex = 1;
                myFileDialog.InitialDirectory = "C:\\";
                myFileDialog.Title = "Abrir archivo";
                myFileDialog.CheckFileExists = false;
                myFileDialog.Multiselect = true;

                
                frmCargaMasivaEMO frmemo = new frmCargaMasivaEMO();
                if (myFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string IdCarpetaTrabajador = "", Extension = "";
                    var idArchivoPDF = "";
                     var TamañoDoc = new FileInfo(myFileDialog.FileName).Length / 1024;
                    System.IO.StreamReader sr = new
                    System.IO.StreamReader(myFileDialog.FileName);
                    int count = 0;
                    foreach (String cadena in myFileDialog.SafeFileNames)
                    {

                        FileInfo file = new FileInfo(cadena);

                       
                        eTrabajador.eEMO obj = new eTrabajador.eEMO();
                        obj.dsc_descripcion = file.ToString();
                        obj.estado = "SI";
                        frmemo.cod_empresa = cod_empresa;
                        frmemo.cod_empresa_multiple = empresas;
                        frmemo.cod_sede_multiple = sedes;
                        frmemo.flg_activo = estado;
                        frmemo.mostrararchivos(obj.dsc_descripcion);
                        obj.estado = frmemo.respuesta;
                        frmemo.bsArchivos.Add(obj);
                        // MessageBox.Show("Archivo: " + file.ToString() + " Directorio: " + myFileDialog.FileNames[count]);

                        //VALIDAR LISTA 
                        trabajadorLista = unit.Trabajador.ListarTrabajadoresvista<eTrabajador>(opcion: 1, cod_trabajador: "", cod_empresa: cod_empresa, cod_empresa_multiple: empresas, cod_sede_multiple: sedes, flg_activo: estado);
                        foreach (var item in trabajadorLista)
                        {
                            if (obj.dsc_descripcion.ToLower().Contains(item.dsc_documento.ToLower()))
                            {

                            }

                        }
                            //

                            count++;
                    }
                    frmemo.ShowDialog();
                     
                    sr.Close();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

      

        private void gvInfoLaboral_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0) ;

        }

       

      

        private void btnGenerarArchivoPLAME_Click(object sender, EventArgs e)
        {
            //eliminararchivodecarpeta();
            string respuestamultiple = " (";
            bool selected = false;

            if (chkFormatoE4.Checked == true) { selected = true; string nombre = "RP_"; respuestamultiple += $" {nombre } "; int opcion = 1; string extencion = ".ide"; formatos(nombrearchivo: nombre, opcion: opcion, extencion: extencion); }
            if (chkFormatoE5.Checked == true) { selected = true; string nombre = "RP_"; respuestamultiple += $" {nombre } "; int opcion = 2; string extencion = ".tra"; formatos(nombrearchivo: nombre, opcion: opcion, extencion: extencion); }
            if (chkFormatoE11.Checked == true) { selected = true; string nombre = "RP_"; respuestamultiple += $" {nombre } "; int opcion = 3; string extencion = ".per"; formatos(nombrearchivo: nombre, opcion: opcion, extencion: extencion); }
            if (chkFormatoE17.Checked == true) { selected = true; string nombre = "RP_"; respuestamultiple += $" {nombre } "; int opcion = 4; string extencion = ".est"; formatos(nombrearchivo: nombre, opcion: opcion, extencion: extencion); }
            if (chkFormatoE29.Checked == true)
                foreach (int nRow in gvListadoTrabajador.GetSelectedRows())
                {
                    eTrabajador obje = gvListadoTrabajador.GetRow(nRow) as eTrabajador;
                    eTrabajador.eInfoAcademica_Trabajador obj = unit.Trabajador.Obtener_Trabajador<eTrabajador.eInfoAcademica_Trabajador>(9, obje.cod_trabajador, obje.cod_empresa);
                    if (obj == null) { HNG.MessageWarning("El usuario no califica para abrir este reporte, es necesario agregar Nivel academico", ""); return; }
                    if (obj.cod_nivelacademico == null) { HNG.MessageWarning("El usuario" + obj.dsc_nombres_completos + "no califica para abrir este reporte", ""); }
                    else if (obj.cod_nivelacademico == "UNIVERSITARIA")
                    {
                        selected = true; string nombre = "RP_"; respuestamultiple += $" {nombre} "; int opcion = 5; string extencion = ".edu"; formatos(nombrearchivo: nombre, opcion: opcion, extencion: extencion);
                    }
                    else
                    {
                        HNG.MessageWarning("El usuario" + obj.dsc_nombres_completos + "no califica para abrir este reporte", "");
                    }
                }
            if (chkFormatoE30.Checked == true)
            {

                foreach (int nRow in gvListadoTrabajador.GetSelectedRows())
                {
                    eTrabajador obje = gvListadoTrabajador.GetRow(nRow) as eTrabajador;
                    eTrabajador.eInfoBancaria_Trabajador obj = unit.Trabajador.Obtener_Trabajador<eTrabajador.eInfoBancaria_Trabajador>(14,obje. cod_trabajador, obje.cod_empresa);
                    if(obj.cod_tipo_pago!= "00002") { HNG.MessageWarning("El usuario" + obj.dsc_nombres_completos + "no califica para abrir este reporte", ""); }
                    else {
                        selected = true; string nombre = "RP_"; respuestamultiple += $" {nombre} "; int opcion = 16; string extencion = ".cta"; formatos(nombrearchivo: nombre, opcion: opcion, extencion: extencion);
                    }
                   
                }

            }

            if (chkAltaplacar.Checked == true) { ExportarExcelAltaplacar(); }
            if (selected && resultadotregistro != "NO") mostrarCarpetaPLAME($"Se descargó los siguientes archivos PLAME:{respuestamultiple})");
        }

 

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            string empresa = "";
            string trabajador = "";

            int emo = 0;

            foreach (int nRow in gvListadoTrabajador.GetSelectedRows())
            {

                eTrabajador obj = gvListadoTrabajador.GetRow(nRow) as eTrabajador;
                if (obj.blacklist == "SI") { HNG.MessageWarning("El trabajador " + obj.dsc_nombres_completos + " " + "se encuentra en la lista negra", "LISTA NEGRA"); return; }
                empresa = obj.cod_empresa;
                trabajador = obj.cod_trabajador + "," + trabajador;
            }

            frmObservacionesBajaUsuario frm = new frmObservacionesBajaUsuario();
            string result = "";
            eTrabajador trab = new eTrabajador();
            if (btnDardebaja.Caption== "Activar Trabajador") {
                
                string estado = "SI"; frm.MiAccion = Cese.Editar;
                result = unit.Trabajador.ActualizarBaja(3, Fechabaja: trab.Fechabaja, motivo_baja: trab.motivo_baja, observaciones: trab.observaciones, cod_empresa: empresa, cod_trabajador: trabajador, flg_activo: estado);
                if (result != "OK") { MessageBox.Show("Error al Activar registro.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                if (result == "OK") { MessageBox.Show("Se procedió a Activar Trabajador", "Eliminar registro", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                frmListadoTrabajador_KeyDown(null, new KeyEventArgs(Keys.F5));
            }
            else { frm.estado = "NO"; frm.MiAccion = Cese.Nuevo;  
            frm.empresa = empresa;
            frm.trabajador = trabajador;
            frm.ShowDialog();
                if (frm.ActualizarListado == "SI") frmListadoTrabajador_KeyDown(null, new KeyEventArgs(Keys.F5));
            }
            


        }



        private void btnCheckMultiple_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gvListadoTrabajador.OptionsSelection.MultiSelect == true)
            {
                gvListadoTrabajador.OptionsSelection.MultiSelect = false;
                return;
            }
            if (gvListadoTrabajador.OptionsSelection.MultiSelect == false)
            {
                gvListadoTrabajador.OptionsSelection.MultiSelect = true;
                splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel1;
                return;
            }
        }




       
       
    }
}