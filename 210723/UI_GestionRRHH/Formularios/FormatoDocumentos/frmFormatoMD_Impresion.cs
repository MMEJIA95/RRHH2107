using BE_GestionRRHH;
using BE_GestionRRHH.FormatoMD;
using BL_GestionRRHH;
using DevExpress.XtraBars;
using DevExpress.XtraRichEdit;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_GestionRRHH.Tools;

namespace UI_GestionRRHH.Formularios.Documento
{
    public enum TipoImpresion { Principal, Trabajador}
    public partial class frmFormatoMD_Impresion : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly UnitOfWork unit;


        List<eFormatoMD_Parametro> parametroList;
        FormatoMDHelper _helper;
        List<eTrabajador_EmpAreaCargo_Vista> trabajadorList;
        List<eFormatoMD_Vinculo_Filtro> formatoList;
        TipoImpresion _tipoImpresion;
        private string _cod_usuario_multiple;
        private string _cod_empresa_ext;
        public frmFormatoMD_Impresion(TipoImpresion tipoImpresion)
        {
            InitializeComponent();
            unit = new UnitOfWork();

            _helper = new FormatoMDHelper();
            _tipoImpresion = tipoImpresion;

            //custom.
            txtTrabajador.Controls.Add(new Label() { Dock = DockStyle.Bottom, Height = 2, AutoSize = false, Text = "", BackColor = Program.Sesion.Colores.Verde });
            txDocumento.Controls.Add(new Label() { Dock = DockStyle.Bottom, Height = 2, AutoSize = false, Text = "", BackColor = Program.Sesion.Colores.Verde });
            lkpEmpresa.Controls.Add(new Label() { Dock = DockStyle.Bottom, Height = 2, AutoSize = false, Text = "", BackColor = Program.Sesion.Colores.Verde });

            lblNotaDocumento.Text = "<b>Nota: </b> Doble click sobre el documento para visualizar la plantilla";
            lblNotaDocumento.AllowHtmlString = true;
            lblNotaDocumento.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            lblNotaDocumento.Appearance.Options.UseTextOptions = true;
            lblTitulo.ForeColor = Program.Sesion.Colores.Verde;
            _cod_usuario_multiple = string.Empty;
            _cod_empresa_ext = string.Empty;
        }

        private void InicializarDatos()
        {
            CargarParametro();
            CargarComboEmpresa();
            CargarListadoFormato();
            CargarTrabajador();
            recPlantilla.Tag = null;
            recPlantilla.WordMLText = "";
        }
        public void CargarTipoImpresion(string trabajador="", string cod_trabajador = "", string cod_empresa="")
        {
            switch (_tipoImpresion)
            {
                case TipoImpresion.Principal:
                    lciTrabajador.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lciEmpresa.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lciNombreTrabajador.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    break;
                case TipoImpresion.Trabajador:
                    lciTrabajador.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lciEmpresa.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lciNombreTrabajador.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    break;
            }
            _cod_usuario_multiple = cod_trabajador;
            _cod_empresa_ext = cod_empresa;
            lblNombreTrabajador.Text = trabajador;
        }
        private void CargarParametro()
        {
            var paramList = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eFormatoMD_Parametro>(
                  new pQFormatoMD() { Opcion = 6, });

            parametroList = new List<eFormatoMD_Parametro>();
            if (paramList != null || paramList.Count() > 0)
            {
                parametroList = paramList;
            }
        }
        private void CargarComboEmpresa()
        {
            unit.Factura.CargaCombosLookUp("EmpresasUsuarios", lkpEmpresa, "cod_empresa", "dsc_empresa", "", valorDefecto: true, cod_usuario: Program.Sesion.Usuario.cod_usuario);
            //List<eFacturaProveedor> list = blProv.ListarEmpresasProveedor<eFacturaProveedor>(11, "", user.cod_usuario);
            //if (list.Count >= 1)
            lkpEmpresa.EditValue = Program.Sesion.EmpresaList[0].cod_empresa;// "00001";// list[0].cod_empresa;
            //mostrar empresa asociado al usuario.
        }
        private void CargarTrabajador()
        {
            string codEmpresa = lkpEmpresa.EditValue.ToString();
            if (string.IsNullOrWhiteSpace(codEmpresa))
            {
                unit.Globales.Mensaje(false, "No se ha seleccionado ninguna empresa", "Cargar Trabajadores");
                return;
            }
            trabajadorList = new List<eTrabajador_EmpAreaCargo_Vista>();
            trabajadorList = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eTrabajador_EmpAreaCargo_Vista>(
                new pQFormatoMD() { Opcion = 11, Cod_empresaSplit = codEmpresa });

            //Inicializar TreeView con datos del documento.
            FiltrarTrabajadores(txtTrabajador.Text.Trim().ToLower());
        }
        private void CargarListadoFormato()
        {
            formatoList = new List<eFormatoMD_Vinculo_Filtro>();
            string codEmpresa = lkpEmpresa.EditValue.ToString();
            formatoList = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eFormatoMD_Vinculo_Filtro>(
                new pQFormatoMD() { Opcion = 9, Cod_empresaSplit = codEmpresa });

            FiltrarDocumentos(txDocumento.Text.Trim().ToLower());
        }

        private void FiltrarDocumentos(string filtro = "")
        {
            trlDocumento.ClearNodes();
            if (formatoList.Count() > 0 && formatoList != null)
            {
                var newList = formatoList
                    .Where(w => w.flg_estado.Equals("SI")
                    && (w.dsc_formatoMD_grupo.ToLower().Contains(filtro)
                    || w.dsc_formatoMD_vinculo.ToLower().Contains(filtro)))
                    .OrderBy(o => o.num_jerarquia).ToList();

                //new Herramienta.TreeViewFiltro(trlDocumento)
                //{
                //    Cod_Nodo1Column = "cod_formatoMD_grupo",
                //    Dsc_Nodo1Column = "dsc_formatoMD_grupo",
                //    Cod_Nodo2Column = "cod_formatoMD_vinculo",
                //    Dsc_Nodo2Column = "dsc_formatoMD_vinculo",
                //    CheckView = Herramienta.TreeCheck.Nodo_Ck2
                //}.With_2Nodes<eFormatoMD_Vinculo_Filtro>(newList);

                var tree = new TreeListHelper(trlDocumento);
                tree.TreeViewParaDosNodos<eFormatoMD_Vinculo_Filtro>(
                    newList,
                    ColumnaCod_Padre: "cod_formatoMD_grupo",
                    ColumnaDsc_Padre: "dsc_formatoMD_grupo",
                    ColumnaCod_Hijo: "cod_formatoMD_vinculo",
                    ColumnaDsc_Hijo: "dsc_formatoMD_vinculo");
                tree.CheckUltimoNodoDeDos();
                trlDocumento.Refresh();

            }
        }

        private void FiltrarTrabajadores(string filter = "")
        {
            trlTrabajador.ClearNodes();
            if (trabajadorList.Count() > 0 && trabajadorList != null)
            {
                var newList = trabajadorList.Where(x => x.dsc_trabajador.ToLower().Contains(filter)
                    || x.dsc_area.ToLower().Contains(filter) || x.dsc_cargo.ToLower().Contains(filter))
                    .OrderBy(o => o.dsc_area).ThenBy(o => o.dsc_cargo).ToList();

                //new Herramienta.TreeViewFiltro(trlTrabajador)
                //{
                //    Cod_Nodo1Column = "cod_area",
                //    Dsc_Nodo1Column = "dsc_area",
                //    Cod_Nodo2Column = "cod_cargo",
                //    Dsc_Nodo2Column = "dsc_cargo",
                //    Cod_Nodo3Column = "cod_trabajador",
                //    Dsc_Nodo3Column = "dsc_trabajador",
                //    CheckView = Herramienta.TreeCheck.Nodo_Ck123
                //}.With_3Nodes<eTrabajador_EmpAreaCargo_Vista>(newList);

                var tree = new TreeListHelper(trlTrabajador);
                tree.TreeViewParaTresNodos<eTrabajador_EmpAreaCargo_Vista>(
                    newList,
                    ColumnaCod_Abuelo: "cod_area",
                    ColumnaDsc_Abuelo: "dsc_area",
                    ColumnaCod_Padre: "cod_cargo",
                    ColumnaDsc_Padre: "dsc_cargo",
                    ColumnaCod_Hijo: "cod_trabajador",
                    ColumnaDsc_Hijo: "dsc_trabajador");
                tree.CheckTodosLosNodos();
                //trlTrabajador.CheckAll();
                trlTrabajador.Refresh();

            }
        }

        private bool Validacion(List<TreeListNode> nodoTrabajador, List<TreeListNode> nodoDocumento)
        {
            if (nodoTrabajador == null || nodoTrabajador.Count == 0)
            {
                unit.Globales.Mensaje(false, "Debes seleccionar un trabajador para visualizar la información.", "Visualizar información");
                btnVistaPrevia.Down = false;
                return false;
            }

            if (nodoDocumento == null)
            {
                if (recPlantilla.Tag == null)
                {
                    unit.Globales.Mensaje(false, "No se ha seleccionado ninguna plantilla, vuelve a intentarlo.", "Visualizar información");
                    btnVistaPrevia.Down = false;
                    return false;
                }
            }
            else
            {
                if (nodoDocumento.Count == 0)
                {
                    unit.Globales.Mensaje(false, "No se ha seleccionado ninguna plantilla, vuelve a intentarlo.", "Visualizar información");
                    btnVistaPrevia.Down = false;
                    return false;
                }
            }

            return true;
        }
        private void VistaPremilinar()
        {
            string codEmpresa = "";
            var trabChecked = trlTrabajador.GetAllCheckedNodes();
            string split_trabajador = "";
            if (string.IsNullOrWhiteSpace(_cod_usuario_multiple))
            {
                if (!Validacion(trabChecked, null)) return;
                codEmpresa = lkpEmpresa.EditValue.ToString();
                split_trabajador = _helper.ObtenerCodigoConcatenadoDeNodo(trabChecked);
            }
            else
            {
                split_trabajador = _cod_usuario_multiple;
                codEmpresa = _cod_empresa_ext;
            }
             
            string originalTemplate = recPlantilla.WordMLText.ToString();
            var objTrabajador = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eTrabajadorDocumentoInfo>(
                new pQFormatoMD() { Opcion = 12, Cod_empresaSplit = codEmpresa, Cod_trabajadorSplit = split_trabajador });


            if (objTrabajador.Count > 0 && objTrabajador != null)
            {
                originalTemplate = _helper.ObtenerWordParametroValor(originalTemplate, parametroList, objTrabajador.FirstOrDefault());
                recPlantilla.WordMLText = originalTemplate;
            }
        }
        private void PrepararPlantillaParaMostrar(out string titulo, out string cod_document)
        {
            titulo = cod_document = "";
            if (trlDocumento.Focused)
            {
                var childFocus = trlDocumento.FocusedNode;
                if (!childFocus.HasChildren)
                {
                    trlDocumento.UncheckAll();
                    childFocus.Checked = true;
                    var parent = childFocus.ParentNode;

                    //var dd = childFocus.GetType();
                    var childName = childFocus.GetValue("Descripcion");
                    var codigo = childFocus.GetValue("Codigo");
                    var parentName = parent.GetValue("Descripcion");
                    var parentCodigo = parent.GetValue("Codigo");

                    titulo = $"{parentName.ToString()}: {childName.ToString()}";
                    cod_document = codigo.ToString();
                    //MostrarDocumento(titulo, codigo.ToString());
                }
                //  VerificarParametrosAsignados();
            }
        }

        private void MostrarDocumento()
        {
            lblTitulo.Text = "";
            PrepararPlantillaParaMostrar(out string titulo, out string cod_document);


            unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Renderizando plantilla", "Cargando...");
            btnVistaPrevia.Down = false;

            string codEmpresa = lkpEmpresa.EditValue.ToString();
            var docList = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eFormatoMD_Vinculo>(
                new pQFormatoMD() { Opcion = 8, Cod_empresaSplit = codEmpresa, Cod_formatoMD_vinculoSplit = cod_document });

            if (docList.Count() > 0 && docList != null)
            {
                var temp = docList.FirstOrDefault();

                var doc = recPlantilla.Document;
                doc.WordMLText = temp.dsc_wordMLText;
                recPlantilla.WordMLText = doc.WordMLText;
                recPlantilla.Tag = doc.WordMLText;
                lblTitulo.Text = titulo;
            }
            SplashScreenManager.CloseForm();
        }

        private void ImprimirEnBloque()
        {
            //Obtenemos los códigos de {parámetro y trabajador} de los nodos.
            var trabajadorCheck = trlTrabajador.GetAllCheckedNodes();
            var documentoCheck = trlDocumento.GetAllCheckedNodes();
            if (!Validacion(trabajadorCheck, documentoCheck)) return;

            var frm = new frmImpresora();
            frm.CargarImpresoras();
            frm.ShowDialog();
            if (frm.Result == DialogResult.OK)
            {
                MatrizEnvioImpresion("imprimir", trabajadorCheck, documentoCheck);
            }
        }
        private void EmailingEnBloque()
        {
            //Obtenemos los códigos de {parámetro y trabajador} de los nodos.
            var trabajadorCheck = trlTrabajador.GetAllCheckedNodes();
            var documentoCheck = trlDocumento.GetAllCheckedNodes();
            if (!Validacion(trabajadorCheck, documentoCheck)) return;

            MatrizEnvioImpresion("email", trabajadorCheck, documentoCheck);
        }

        private void MatrizEnvioImpresion(string accion, List<TreeListNode> chksTrabajador, List<TreeListNode> chksDocumento)
        {
            List<BE_GestionRRHH.eSistema> eSist = unit.Sistema.Obtener_ParamterosSistema<BE_GestionRRHH.eSistema>(11);

            unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), accion == "imprimir" ? "Procesando impresión" : "Enviando documentos por correo electrónico", "Cargando...");

            //Obtenemos información de los trabajadores según selección de nodos.
            var trabajadores = GetInfoTrabajador(chksTrabajador);

            chksDocumento.ForEach((doc) =>
            {
                if (!doc.HasChildren)
                {
                    var cod_documento = doc.GetValue("Codigo").ToString();
                    var temps = GetFormatoWord(cod_documento);
                    var tmpOriginal = temps;

                    chksTrabajador.ForEach((tr) =>
                    {
                        if (!tr.HasChildren)
                        {
                            var cod_trabajador = tr.GetValue("Codigo").ToString();
                            var trabajador = trabajadores.FirstOrDefault(o => o.cod_trabajador.Equals(cod_trabajador));
                            temps = _helper.ObtenerWordParametroValor(temps, parametroList, trabajador);

                            switch (accion)
                            {
                                case "email":
                                    {
                                        //En construcción: envío por correo.
                                        //MessageBox.Show(trabajador.dsc_email.Trim());
                                        unit.Globales.EnviarCorreoElectronico_SMTP(
                                            trabajador.dsc_email.Trim(),
                                            $"Formato: {doc.GetValue("Descripcion").ToString()}",
                                            "Por favor, completar los datos... al concluir, favor de reenviar la documentación al correo abc@mail.com",
                                            eSist, @"C:\TS"//ruta del adjunto. //C:\\TS\\Test.docx
                                            );
                                        break;
                                    }
                                case "imprimir":
                                    {
                                        using (var wordProcessor = new RichEditDocumentServer())
                                        {
                                            wordProcessor.WordMLText = temps;
                                            PrinterSettings printerSettings = new PrinterSettings();
                                            printerSettings.Copies = 1;

                                            wordProcessor.Print(printerSettings);
                                        }
                                        break;
                                    }
                            }
                            temps = tmpOriginal;
                        }
                    });
                }
            });

            SplashScreenManager.CloseForm();
        }

        private List<eTrabajadorDocumentoInfo> GetInfoTrabajador(List<TreeListNode> listNodes)
        {
            string split_trabajador = string.Empty;
            switch (_tipoImpresion)
            {
                case TipoImpresion.Principal:
                    split_trabajador = _helper.ObtenerCodigoConcatenadoDeNodo(listNodes);
                    break;
                case TipoImpresion.Trabajador:
                    split_trabajador = _cod_usuario_multiple;
                    break;
            }
            

            string cod_empresa = lkpEmpresa.EditValue.ToString();
            var obj = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eTrabajadorDocumentoInfo>(
                                        new pQFormatoMD()
                                        {
                                            Opcion = 5,
                                            Cod_empresaSplit = cod_empresa,
                                            Cod_trabajadorSplit = split_trabajador
                                        });
            return obj.ToList();
        }
        private string GetFormatoWord(string cod_document)
        {
            string codEmpresa = lkpEmpresa.EditValue.ToString();
            var docList = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eFormatoMD_Vinculo>(
                new pQFormatoMD() { Opcion = 1, Cod_empresaSplit = codEmpresa, Cod_formatoMD_vinculoSplit = cod_document });

            if (docList.Count() > 0 && docList != null)
            {
                var temp = docList.FirstOrDefault();
                return temp.dsc_wordMLText;
            }
            else return null;
        }

        private void frmDocumentosImpresion_Load(object sender, EventArgs e)
        {
            InicializarDatos();
        }

        private void lkpEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            CargarListadoFormato();
            CargarTrabajador();
        }

        private void btnVistaPrevia_ItemClick(object sender, ItemClickEventArgs e)
        {
            // var value = btnVistaPrevia.Down;
            // if (!value)
            // MostrarPlantilla();
            //else
            //  {
            MostrarDocumento();
            VistaPremilinar();
            // }
        }

        private void btnInicializar_ItemClick(object sender, ItemClickEventArgs e)
        {
            InicializarDatos();
        }

        private void trlDocumento_DoubleClick(object sender, EventArgs e)
        {
            trlDocumento.UncheckAll();
            var f = trlDocumento.FocusedNode;
            f.Checked = true;
            MostrarDocumento();
        }

        private void btnImprimirGrupo_ItemClick(object sender, ItemClickEventArgs e)
        {
            ImprimirEnBloque();
        }

        private void btnEnviarMail_ItemClick(object sender, ItemClickEventArgs e)
        {
            EmailingEnBloque();
        }

        private void txtTrabajador_TextChanged(object sender, EventArgs e)
        {
            FiltrarTrabajadores(txtTrabajador.Text.Trim().ToLower());
        }

        private void txDocumento_TextChanged(object sender, EventArgs e)
        {
            FiltrarDocumentos(txDocumento.Text.Trim().ToLower());
        }

        private void trlTrabajador_MouseClick(object sender, MouseEventArgs e)
        {
            //var f = trlTrabajador.FocusedNode;
            //f.Checked = !f.Checked;
        }

        private void trlDocumento_MouseClick(object sender, MouseEventArgs e)
        {
            //trlDocumento.UncheckAll();
            //var f = trlDocumento.FocusedNode;
            //f.Checked = true;// !f.Checked;
        }

        private void trlDocumento_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            //var dd = trlDocumento.GetAllCheckedNodes();

            //var dsd = trlDocumento.GetFocusedRow() as TreeViewOption;
            //MessageBox.Show(dsd.Name);
        }
    }
}