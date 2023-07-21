using BE_GestionRRHH;
using BL_GestionRRHH;
using DevExpress.XtraBars;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_GestionRRHH.Formularios.Correspondencia
{
    public partial class frmCorrespondencia_BoletaPago : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        internal readonly UnitOfWork unit;
        private string extension = "pdf";
        private List<eSistema> CredencialesList;
        private List<eSistema> CredencialesGrupoHNG;
        private List<eEmailingBoleta> EmailsEnviados;
        private List<eEmailFormato> FormatoEmailList;
        private Label progressText;
        private int iEnviados = 0;
        private int iProgressMax = 0;
        private int selectedRows = 0;
        private string _codEmpresa = string.Empty;
        public frmCorrespondencia_BoletaPago()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            configurar_formulario();
            this.teeExplorador.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.teeExplorador_NodeMouseClick);
            PopulateTreeView();
            InicializarDatos();
        }

        private void configurar_formulario()
        {
            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoTrabajadores, gvListadoTrabajadores);
            gvListadoTrabajadores.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            gvListadoTrabajadores.OptionsView.ShowIndicator = false;
            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoEmailing, gvListadoEmailing);
            gvListadoEmailing.OptionsView.ShowIndicator = false;

            CrearProgressBar();
        }
        private void CrearProgressBar()
        {
            /*-----*Crear progress bar*-----*/
            this.iEnviados = 0;
            this.iProgressMax = progressBar.Width;
            progressBar.Controls.Clear();
            progressText = new Label()
            {
                Dock = DockStyle.Left,
                Width = iEnviados,
                BackColor = Color.FromArgb(167, 25, 192),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Tahoma", 10, FontStyle.Regular),
                Text = ""
            };
            progressBar.Controls.Add(progressText);
            progressBar.BringToFront();
        }

        private void InicializarDatos()
        {
            CargarDatosParaEnvios();
        }

        private void PopulateTreeView()
        {
            string[] paths = new string[] { "C:", "D:", "E:", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) };
            /*------**------*/
            TreeNode rootNode;
            foreach (string item in paths)
            {
                DirectoryInfo info = new DirectoryInfo($"{item}\\");
                if (info.Exists)
                {
                    rootNode = new TreeNode(info.Name);
                    rootNode.Tag = info;
                    GetDirectories(info.GetDirectories(), rootNode);
                    teeExplorador.Nodes.Add(rootNode);

                    var imgList = new ImageList();
                    imgList.Images.Add(Properties.Resources.open_16x16);
                    teeExplorador.ImageList = imgList;
                }
            }
            if (teeExplorador.Nodes.Count > 0)
            { teeExplorador.Nodes[teeExplorador.Nodes.Count - 1].Expand(); }
        }

        private void GetDirectories(DirectoryInfo[] subDirs,
            TreeNode nodeToAddTo)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                if (aNode.ToString().Contains("$")) continue;
                if (aNode.ToString().ToLower().Contains("adobe")) continue;
                if (aNode.ToString().ToLower().Contains("files")) continue;
                if (aNode.ToString().ToLower().Contains("x86")) continue;
                if (aNode.ToString().ToLower().Contains("x64")) continue;
                if (aNode.ToString().ToLower().Contains("program")) continue;
                if (aNode.ToString().ToLower().Contains("windows")) continue;
                if (aNode.ToString().ToLower().Contains("temp")) continue;
                if (aNode.ToString().ToLower().Contains("app")) continue;
                if (aNode.ToString().ToLower().Contains("intel")) continue;
                if (aNode.ToString().ToLower().Contains("eset")) continue;
                if (aNode.ToString().ToLower().Contains("updat")) continue;
                if (aNode.ToString().ToLower().Contains("log")) continue;
                if (aNode.ToString().ToLower().Contains("sett")) continue;
                if (aNode.ToString().ToLower().Contains("drive")) continue;
                if (aNode.ToString().ToLower().Contains("config")) continue;
                if (aNode.ToString().ToLower().Contains("fuent")) continue;
                if (aNode.ToString().ToLower().Contains("git")) continue;
                if (aNode.ToString().ToLower().Contains("ico")) continue;
                if (aNode.ToString().ToLower().Contains("bin")) continue;
                if (aNode.ToString().ToLower().Contains("debug")) continue;
                if (aNode.ToString().ToLower().Contains("obj")) continue;
                if (aNode.ToString().ToLower().Contains("proper")) continue;
                if (aNode.ToString().ToLower().Contains("pp")) continue;
                if (aNode.ToString().ToLower().Contains("fuente")) continue;
                if (aNode.ToString().ToLower().Contains("pub")) continue;
                if (aNode.ToString().ToLower().Contains("del")) continue;
                if (aNode.ToString().ToLower().Contains("data")) continue;
                if (aNode.ToString().ToLower().Contains("usua")) continue;
                if (aNode.ToString().ToLower().Contains("user")) continue;
                if (aNode.ToString().ToLower().Contains(".")) continue;
                if (aNode.ToString().ToLower().Contains("source")) continue;
                if (aNode.ToString().ToLower().Contains("contact")) continue;
                if (aNode.ToString().ToLower().Contains("favorit")) continue;
                if (aNode.ToString().ToLower().Contains("video")) continue;
                if (aNode.ToString().ToLower().Contains("link")) continue;
                if (aNode.ToString().ToLower().Contains("music")) continue;
                if (aNode.ToString().ToLower().Contains("games")) continue;
                if (aNode.ToString().ToLower().Contains("search")) continue;
                if (aNode.ToString().ToLower().Contains("busqued")) continue;
                if (aNode.ToString().ToLower().Contains("tracin")) continue;
                //if (aNode.ToString().ToLower().Contains("de")) continue;
                //if (aNode.ToString().ToLower().Contains("es")) continue;
                //if (aNode.ToString().ToLower().Contains("ja")) continue;
                //if (aNode.ToString().ToLower().Contains("js")) continue;
                //if (aNode.ToString().ToLower().Contains("ru")) continue;
                if (aNode.ToString().ToLower().Contains("~")) continue;

                aNode.Tag = subDir;
                aNode.ImageKey = "folder";
                try { subSubDirs = subDir.GetDirectories(); } catch { continue; }
                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, aNode);
                }
                nodeToAddTo.Nodes.Add(aNode);
            }
        }


        private TreeNode __node;
        private int __x, __y;
        void teeExplorador_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            __node = e.Node;
            __x = e.X;
            __y = e.Y;
            listDocumentos.Items.Clear();
            DirectoryInfo nodeDirInfo = (DirectoryInfo)newSelected.Tag;
            //ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;

            /*foreach (DirectoryInfo dir in nodeDirInfo.GetDirectories())
            {
                item = new ListViewItem(dir.Name, 0);
                subItems = new ListViewItem.ListViewSubItem[]
                    {new ListViewItem.ListViewSubItem(item, "Directory"),
             new ListViewItem.ListViewSubItem(item,
                dir.LastAccessTime.ToShortDateString())};
                item.SubItems.AddRange(subItems);
                listDocumentos.Items.Add(item);
            }*/
            foreach (FileInfo file in nodeDirInfo.GetFiles())
            {
                if (!file.Name.ToLower().Contains($".{extension}")) continue;
                item = new ListViewItem(file.Name, 1);
                //subItems = new ListViewItem.ListViewSubItem[]
                //{ new ListViewItem.ListViewSubItem(item, _documento),new ListViewItem.ListViewSubItem(item, _periodo)};
                //{ new ListViewItem.ListViewSubItem(item, "File"), new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString())};
                //item.SubItems.AddRange(subItems);
                listDocumentos.Items.Add(item);
            }

            cbxSelectAll.Text = $"Seleccionar ({listDocumentos.Items.Count} items)";
            lblRuta.Caption = $"Ruta: {nodeDirInfo.FullName}\\";
            lblRuta.Tag = $"{nodeDirInfo.FullName}\\";
            listDocumentos.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            layout_Encontrado.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            cbxSelectAll.Checked = false;
            cbxSelectAllNoRegistrado.Checked = false;
            bsListadoEmailing.DataSource = null;
            bsListadoTrabajadores.DataSource = null;
            gvListadoEmailing.RefreshData();
            gvListadoTrabajadores.RefreshData();
            btnVerBoletasEncontradas.Visibility = BarItemVisibility.Never;
            btnVerBoletasNoRegistradas.Visibility = BarItemVisibility.Never;
        }
        void establecerExtension()
        {
            extension = chkPDF.Checked ? "pdf" :
                chkEXCEL.Checked ? "xlsx" :
                chkWORD.Checked ? "docx" :
                chkXML.Checked ? "xml" : "*";

            //btnBuscarDocumento.Enabled = chkPDF.Checked ? true :
            //    chkEXCEL.Checked ? true :
            //    chkWORD.Checked ? true :
            //    chkXML.Checked ? true : false;

            grpArchivoImportado.Text = $"Documentos {extension.ToUpper()} Encontrados";
            teeExplorador_NodeMouseClick(teeExplorador, new TreeNodeMouseClickEventArgs(__node, MouseButtons.Left, 1, __x, __y));
            //btnBuscar_Click(btnBuscar, new EventArgs());
        }


        ///*-----*Configuramos el nombre del archivo, extraer valores*-----*/
        ///*-----*Formato: LEGUI_0142805995202207.PDF*-----*/
        //string doc = (file.Name.Split('.')[0]).Split('_')[1];
        //string _documento = doc.Substring(2, doc.Length - 8);
        //string per = file.Name.Split('.')[0];
        //string _periodo = per.Substring(per.Length - 5, 4);

        #region /*---------*Espacio para Gestionar Listado de Documentos y Trabajadores*---------*/
        string ObtenerDniMultiple_deListView()
        {
            string ret = "delete";
            try
            {
                /*-----*Obtener documentos checkados del ListView*-----*/
                foreach (ListViewItem item in listDocumentos.Items)
                {
                    if (item.Checked)
                    {
                        /*-----*Formato: LEGUI_0142805995202207.PDF*-----*/
                        string _nombre = (item.Text.Split('.')[0]).Split('_')[1];
                        string _documento = _nombre.Substring(2, _nombre.Length - 8);
                        ret += $",{_documento}";
                    }
                }
            }
            catch (Exception ex)
            {
                unit.Globales.Mensaje(false, "Error al leer la descripción de las boletas, asegurese que la carpeta contenga solo los documentos a tratar. " + ex.Message, "Extraer caracteres");
            }
            return (ret.Replace("delete,", "")).Replace("delete", "");
        }

        //private T[] removeDuplicates<T>(T[] array)
        //{
        //    HashSet<T> set = new HashSet<T>(array);
        //    T[] result = new T[set.Count];
        //    set.CopyTo(result);
        //    return result;
        //}
        //string[] distinto = removeDuplicates(string.Concat(documentoSplit, ",", doc_multiple_desdeSQL).Split(',')); //

        /*-----*Formato: LEGUI_0142805995202207.PDF*-----*/
        string extraerDocumento(string value) { string _nombre = (value.Split('.')[0]).Split('_')[1]; return (_nombre.Substring(2, _nombre.Length - 8)); }
        void CargarTrabajadores()
        {
            unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Comprobando registros", "Procesando...");

            /*-----*Obtenemos Listado de Trabajadores*-----*/
            var documentoSplit = ObtenerDniMultiple_deListView();
            var trabajadorList = unit.EmailingBoleta.ConsultaVarias_EmailingBoletas<eTrabajador_EmpEmail_Vista>(
                new pEmailingBoleta { Opcion = 1, Dsc_documento_trabajadorSplit = documentoSplit });

            if (trabajadorList.Count > 0 && trabajadorList != null)
            {
                /*-----*Insertar en GridView nombre de la Boleta+Ruta, enlazada al trabajador*-----*/
                foreach (ListViewItem docs in listDocumentos.Items)
                {
                    if (docs.Checked)
                    {
                        trabajadorList.Where(p => p.dsc_documento.Equals(extraerDocumento(docs.Text))).ToList()
                            .ForEach((d) => d.dsc_pathfile = String.Concat(lblRuta.Tag.ToString(), docs.Text));
                    }
                }

                /*-----*Remover documentos exitentes*-----*/
                var doc_multiple_desdeSQL = new Tools.TreeListHelper().ObtenerValoresConcatenadoDeLista(trabajadorList, "dsc_documento");
                var distinto = documentoSplit.Split(',').Except(doc_multiple_desdeSQL.Split(',')); // string con_coma = String.Join(",", distinto);
                if (distinto != null && distinto.Count() > 0)
                {
                    /*-----*Insertar en ListView Items No Encontrados en DB*-----*/
                    listNoRegitrados.Items.Clear();
                    ListViewItem item = null;
                    foreach (var file in distinto)
                    {
                        foreach (ListViewItem docs in listDocumentos.Items)
                        {
                            if (docs.Checked)
                            {
                                /*-----*Formato: LEGUI_0142805995202207.PDF*-----*/
                                string _nombre = (docs.Text.Split('.')[0]).Split('_')[1];
                                string _documento = _nombre.Substring(2, _nombre.Length - 8);
                                if (file.ToString().Equals(_documento))
                                {
                                    item = new ListViewItem(docs.Text, 1);
                                    listNoRegitrados.Items.Add(item);
                                }
                            }
                        }

                    }
                    cbxSelectAllNoRegistrado.Text = $"Seleccionar ({listNoRegitrados.Items.Count} items)";
                    layout_NoRegistrados.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    btnExportarNoRegistrados.Visibility = BarItemVisibility.Always;
                }
                /*-----*Cagar Grilla con los datos del Trabajador*-----*/
                bsListadoTrabajadores.DataSource = trabajadorList;
                gvListadoTrabajadores.RefreshData();
                gvListadoTrabajadores.ExpandAllGroups();

            }
            SplashScreenManager.CloseForm();
        }
        #endregion /*---------*Espacio para Gestionar Listado de Documentos y Trabajadores*---------*/

        #region /*---------*Espacio para Gestionar Envío de Correos*---------*/
        private void CargarDatosParaEnvios()
        {
            /*---------*Obtener Listado de Credenciales y Credencial del Grupo HNG; por si una empresa no tiene correo de envíos*---------*/
            var empresaParaAsuntoMail = unit.EmailingBoleta.ConsultaVarias_EmailingBoletas<eEmpresaEmail>
                (new pEmailingBoleta() { Opcion = 7 });
            CredencialesList = new List<eSistema>();
            CredencialesList = empresaParaAsuntoMail.Select((obj) => new eSistema()
            {
                cod_clave = obj.cod_empresa,//"CC000", //Esto viene de la tabla: scfma_empresa
                dsc_clave = obj.dsc_UsuarioEmailBoletas,
                dsc_valor = obj.dsc_ClaveEmailBoletas,
            }).ToList();
            CredencialesGrupoHNG = new List<eSistema>();
            CredencialesGrupoHNG = CredencialesList.Where(c => c.cod_clave.Equals("00001")).ToList();//ver si se va  a utilizar por default.

            /*---------*Obtener Formato-Texto de Asunto y Cuerpo para el Correo*---------*/
            FormatoEmailList = new List<eEmailFormato>();
            FormatoEmailList = unit.EmailingBoleta.ConsultaVarias_EmailingBoletas<eEmailFormato>(new pEmailingBoleta() { Opcion = 6, Cod_empresaSplit = "ALL", Cod_tipo_formato = "RRHH_BOLETA_PAGO" });


        }
        private void EnviarCorreos()
        {
            lblAlerta.Text = " ";
            /*---------*Validar los paámetros para descripción del Email*---------*/
            var FormatoEmail = new eEmailFormato();
            if (FormatoEmailList != null && FormatoEmailList.Count > 0) { FormatoEmail = FormatoEmailList.FirstOrDefault((f) => f.cod_empresa.Equals(_codEmpresa)); }

            if (FormatoEmail == null) return;
            if (!FormatoEmail.dsc_cuerpo.ToLower().Contains("{nombre}")) return;
            if (!FormatoEmail.dsc_cuerpo.ToLower().Contains("{periodo}")) return;
            if (!FormatoEmail.dsc_asunto.ToLower().Contains("{empresa}")) return;
            if (!(gvListadoTrabajadores.GetFocusedRow() is eTrabajador_EmpEmail_Vista)) return;

            var rows = gvListadoTrabajadores.GetSelectedRows();
            selectedRows = rows.Where((i) => i >= 0).Count();
            if (selectedRows == 0) { return; }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            PintarProgressBar(selectedRows, stopwatch.Elapsed);
            CrearProgressBar();
            EmailsEnviados = new List<eEmailingBoleta>();
            bsListadoEmailing.DataSource = null;
            pnlAvance.Visible = true;
            int flagMensaje = 0;
            foreach (var nRow in (rows.Where((i) => i >= 0)))
            {
                //StartProcess();
                flagMensaje++;
                var objTrabajador = gvListadoTrabajadores.GetRow(nRow) as eTrabajador_EmpEmail_Vista;
                /*---------*Verificar Formato para envíos.*---------*/
                //var cuerpo = FormatoEmail.dsc_cuerpo;
                string newCuerpo = FormatoEmail.dsc_cuerpo.Replace("{nombre}", objTrabajador.dsc_trabajador);
                newCuerpo = newCuerpo.Replace("{periodo}", obtenerPeriodo(objTrabajador.dsc_pathfile));
                string newAsunto = FormatoEmail.dsc_asunto.Replace("{empresa}", objTrabajador.dsc_empresa);
                if (flagMensaje == 1)
                {
                    var formatoEnvio = unit.Globales.Mensaje(blGlobales.TipoMensaje.YesNo,
                        $"Asunto: {newAsunto}\nMensaje:\n{newCuerpo}\n" +
                        $"----------------------------------------------------------\n" +
                        "¿Seguro de continuar con el envío?", "Formato envío masivo de Boletas");
                    if (formatoEnvio == DialogResult.No)
                    {
                        //Se cnacela el proceso.
                        //flagEnviado = false;
                        //lblProceso.Text = $"Status: enviados <b>(0)</b> de <b>({numSeleccionadosPorEnviar})</b>  boletas.";
                        pnlAvance.Visible = false;
                        progressBar.Controls.Clear();
                        return;
                    }
                }

                /*---------*Generar Parámetros para en envío de correos*---------*/
                bool enviado = false;
                bool cancelarProceso = false;
                var frm = new frmEmailing(this) { Text = "Emailing" };
                frm.CargarDatos(objTrabajador, CredencialesList, CredencialesGrupoHNG, FormatoEmail);
                if (frm.ShowDialog() == DialogResult.OK) { enviado = true; }
                cancelarProceso = frm.CancelarProceso;
                frm.Dispose();
                iEnviados++;
                /*---------*Mostrar Avance*---------*/
                PintarProgressBar(selectedRows, stopwatch.Elapsed);

                /*---------*Mostrar Avance en Grid*---------*/
                EmailsEnviados.Add(new eEmailingBoleta()
                {
                    cod_empresa = objTrabajador.cod_empresa,
                    dsc_emailingBoletas = newAsunto,
                    dsc_documento_trabajador = objTrabajador.dsc_documento,
                    dsc_email_trabajador = objTrabajador.dsc_email,
                    dsc_archivo_enviado = Path.GetFileName(objTrabajador.dsc_pathfile),
                    flg_enviado = enviado ? "SI" : "NO"
                });
                bsListadoEmailing.DataSource = EmailsEnviados.ToList();
                gvListadoEmailing.RefreshData();

                /*---------*Cancelar Envíos programados:: flag enviada desde el formulario frmEmailing*---------*/
                if (cancelarProceso) { lblAlerta.Text = "¡Poceso Cancelado!"; break; }
            }


            stopwatch.Stop();

            /*---------*Insertar Resultado de Envíos a la DB*---------*/
            var values = ObtenerValoresConcatenados(EmailsEnviados);
            InsertarRegistrosEnviados(values);
        }


        private void PintarProgressBar(int total, TimeSpan tiempoTranscurrido)
        {
            if (total <= 0 || iEnviados <= 0) { return; }

            var width = Math.Ceiling((iProgressMax * 1.0m) / (total * 1.0m));
            //MessageBox.Show($"{width}, {(iProgressMax * 1.0m) / (total * 1.0m)}");
            //var current = progressText.Width;
            //180
            //
            //txtAvanceEnviados.Text = width.ToString();
            //txtAvanceTiempoEnvio.Text = iProgressMax.ToString();
            //txtAvancTiempoAproximado.Text= iEnviados.ToString();

            progressText.Width += int.Parse(width.ToString());
            progressText.Text = $"{Math.Round((((iEnviados * 1.0) / (total * 1.0)) * 100), 2)}%";

            /*---------*Resumen en TextBox*---------*/
            var a = iEnviados; var b = tiempoTranscurrido.TotalSeconds; var c = total;
            var aproximado = (b * c) / a;
            var tiempoEstimado = DateTime.Now.AddSeconds(aproximado).Subtract(DateTime.Now);
            var tiempoRestante = tiempoEstimado.Subtract(tiempoTranscurrido);
            txtAvanceEnviados.Text = $"{iEnviados}/{total}";
            txtAvanceTiempoEnvio.Text = $"{tiempoTranscurrido.ToString(@"hh\:mm\:ss")}/{tiempoEstimado.ToString(@"hh\:mm\:ss")}";
            txtAvancTiempoAproximado.Text = $"{tiempoRestante.ToString(@"hh\:mm\:ss")}";
        }
        internal string obtenerPeriodo(string text)
        {
            //Se extrae el año y mes del nombre del archivo.
            var fileName = text.Replace(lblRuta.Tag.ToString(), "");
            string file = (fileName.Split('.')[0]);//Nombre sin extención y sin ruta.
            string a_m = file.Substring(file.Length - 6, 6);//Año y mes 202001
            string anho = a_m.Substring(0, 4);//Año
            string mes = a_m.Substring(a_m.Length - 2, 2);//Mes

            // cambiar por DateTimeValues // ver formato idioma.
            switch (mes)
            {
                case "01":
                    mes = "ENERO";
                    break;
                case "02":
                    mes = "FEBRERO";
                    break;
                case "03":
                    mes = "MARZO";
                    break;
                case "04":
                    mes = "ABRIL";
                    break;
                case "05":
                    mes = "MAYO";
                    break;
                case "06":
                    mes = "JUNIO";
                    break;
                case "07":
                    mes = "JULIO";
                    break;
                case "08":
                    mes = "AGOSTO";
                    break;
                case "09":
                    mes = "SETIEMBRE";
                    break;
                case "10":
                    mes = "OCTUBRE";
                    break;
                case "11":
                    mes = "NOVIEMBRE";
                    break;
                case "12":
                    mes = "DICIEMBRE";
                    break;
            }
            return string.Concat(" ", mes, " ", anho);
        }
        #endregion /*---------*Espacio para Gestionar Envío de Correos*---------*/


        #region /*---------*Espacio para Guardar Resultado de Envíos en DB*---------*/
        /// <summary>
        /// Método para insertar en SQL registros enviados.
        /// </summary>
        /// <param name="concats"> valores concatenados, para insesión múltiple.</param>
        private void InsertarRegistrosEnviados(string[] concats)
        {
            if (concats == null) return;

            var insertado = unit.EmailingBoleta.ConsultaVarias_EmailingBoletas<eEmailingBoleta>(
                new pEmailingBoleta()
                {
                    Opcion = 3,
                    Cod_empresaSplit = concats[5],
                    Dsc_emailingBoletaSplit = concats[4],// "BOLETAS BLAH BLAH BLAH",
                    Cod_usuario_enviado = Program.Sesion.Usuario.cod_usuario,//"ADMINISTRADOR",
                    Dsc_email_trabajadorSplit = concats[0],
                    Dsc_archivo_enviadoSplit = concats[1],
                    Flg_enviadoSplit = concats[2],
                    Dsc_documento_trabajadorSplit = concats[3]
                });
            if (insertado != null && insertado.Count() > 0)
            {
                bsListadoEmailing.DataSource = insertado.ToList();
            }
        }
        /// <summary>
        /// Generar valores concatenados para insesión múltiple.
        /// </summary>
        /// <param name="mailList">Listado de correos enviados.</param>
        /// <returns></returns>
        private string[] ObtenerValoresConcatenados(List<eEmailingBoleta> mailList)
        {
            /*[0]->Email,[1]->Archivo,[2]->FLag,[3]->Documento,[3]->Descripcion del envio, [5-Cod_Empresa]*/
            string[] values = new string[6];
            values[0] = "delete";
            values[1] = "delete";
            values[2] = "delete";
            values[3] = "delete";
            values[4] = "delete";
            values[5] = "delete";
            mailList.ForEach((obj) =>
            {
                values[0] += $",{obj.dsc_email_trabajador.ToString()}";
                values[1] += $",{obj.dsc_archivo_enviado.ToString()}";
                values[2] += $",{obj.flg_enviado.ToString()}";
                values[3] += $",{obj.dsc_documento_trabajador.ToString()}";
                values[4] += $",{obj.dsc_emailingBoletas.ToString()}";
                values[5] += $",{obj.cod_empresa.ToString()}";
            });
            values[0] = values[0].Replace("delete,", "").Trim();
            values[1] = values[1].Replace("delete,", "").Trim();
            values[2] = values[2].Replace("delete,", "").Trim();
            values[3] = values[3].Replace("delete,", "").Trim();
            values[4] = values[4].Replace("delete,", "").Trim();
            values[5] = values[5].Replace("delete,", "").Trim();
            return values;
        }
        #endregion/*---------*Espacio para Guardar Resultado de Envíos en DB*---------*/

        private void ActualizarCorreoIndividual()
        {
            if (!(gvListadoTrabajadores.GetFocusedRow() is eTrabajador_EmpEmail_Vista obj)) return;
            var frm = new frmCorrespondencia_EmailEdit
            {
                Text = "Actualizar Correos"
            };
            frm.CargarDatos(obj.dsc_documento);
            if (frm.ShowDialog() == DialogResult.OK) { CargarDatosParaEnvios(); }
            frm.Dispose();
            //new Tools.ToolHelper.Forms().ShowDialog(frm);
        }
        void CargarCorreoMasivo()
        {
            var frm = new frmCorrespondencia_EmailEditMasivo
            {
                Text = "Actualizar Correos Masivos"
            };
            if (frm.ShowDialog() == DialogResult.OK) { CargarDatosParaEnvios(); }
            frm.Dispose();
            //new Tools.ToolHelper.Forms().ShowDialog(frm);
        }
        private void ConfigurarFormatoEmail(string codEmpresa)
        {
            var frm = new frmCorrespondencia_FormatoParaEnvio
            {
                Text = "Configurar Formato para Envíos"
            };
            frm.CargarDatos(codEmpresa, "RRHH_BOLETA_PAGO");
            if (frm.ShowDialog() == DialogResult.OK) { CargarDatosParaEnvios(); }
            frm.Dispose();
            //new Tools.ToolHelper.Forms().ShowDialog(frm);
        }
        private void frmCorrespondencia_BoletaPago_Load(object sender, EventArgs e)
        {
        }

        private void chkPDF_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPDF.Checked)
            {
                chkWORD.Checked = false;
                chkEXCEL.Checked = false;
                chkXML.Checked = false;
            }
            establecerExtension();
        }

        private void chkWORD_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWORD.Checked)
            {
                chkEXCEL.Checked = false;
                chkPDF.Checked = false;
                chkXML.Checked = false;
            }
            establecerExtension();
        }

        private void chkEXCEL_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEXCEL.Checked)
            {
                chkWORD.Checked = false;
                chkPDF.Checked = false;
                chkXML.Checked = false;
            }
            establecerExtension();
        }

        private void chkXML_CheckedChanged(object sender, EventArgs e)
        {
            if (chkXML.Checked)
            {
                chkWORD.Checked = false;
                chkPDF.Checked = false;
                chkEXCEL.Checked = false;
            }
            establecerExtension();
        }

        private void btnProcesarTrabajador_ItemClick(object sender, ItemClickEventArgs e)
        {
            layout_NoRegistrados.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            _codEmpresa = string.Empty;
            CargarTrabajadores();

            //MessageBox.Show(ObtenerDniMultiple_deListView());
            //CargarTrabajadores();
        }

        private void cbxSelectAllNoRegistrado_CheckedChanged(object sender, EventArgs e)
        {
            listNoRegitrados.BeginUpdate();
            foreach (ListViewItem item in listNoRegitrados.Items)
                item.Checked = cbxSelectAllNoRegistrado.Checked;
            listNoRegitrados.EndUpdate();
        }

        private void btnVerExplorador_ItemClick(object sender, ItemClickEventArgs e)
        {
            var value = btnVerExplorador.Down;
            if (!value)
            {
                btnVerExplorador.Caption = "Ocultar Explorador de Archivos";
                layout_Explorador.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                btnVerExplorador.ImageOptions.LargeImage = Properties.Resources.hide_32x32;
            }
            else
            {
                btnVerExplorador.Caption = "Mostrar Explorador de Archivos";
                layout_Explorador.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                btnVerExplorador.ImageOptions.LargeImage = Properties.Resources.show_32x32;
            }
        }
        private void btnEsconderDocumentoEncontrado_Click(object sender, EventArgs e)
        {
            layout_Encontrado.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            btnVerBoletasEncontradas.Visibility = BarItemVisibility.Always;
        }

        private void btnEsconderNoRegistrado_Click(object sender, EventArgs e)
        {
            layout_NoRegistrados.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            btnVerBoletasNoRegistradas.Visibility = BarItemVisibility.Always;
            btnExportarNoRegistrados.Visibility = BarItemVisibility.Never;
        }

        private void btnVerBoletasEncontradas_ItemClick(object sender, ItemClickEventArgs e)
        {
            layout_Encontrado.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            btnVerBoletasEncontradas.Visibility = BarItemVisibility.Never;
        }

        private void btnVerBoletasNoRegistradas_ItemClick(object sender, ItemClickEventArgs e)
        {
            layout_NoRegistrados.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            btnVerBoletasNoRegistradas.Visibility = BarItemVisibility.Never;
            btnExportarNoRegistrados.Visibility = BarItemVisibility.Always;
        }

        private void btnActualizarIndividualCorreo_ItemClick(object sender, ItemClickEventArgs e)
        {
            ActualizarCorreoIndividual();
        }

        private void btnActualizarCorreoMasivo_ItemClick(object sender, ItemClickEventArgs e)
        {
            CargarCorreoMasivo();
        }

        private void btnFormatoEnvios_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(gvListadoTrabajadores.GetFocusedRow() is eTrabajador_EmpEmail_Vista obj)) return;
            _codEmpresa = obj.cod_empresa;
            ConfigurarFormatoEmail(obj.cod_empresa);
        }

        private void btnEnviarCorreos_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(gvListadoTrabajadores.GetFocusedRow() is eTrabajador_EmpEmail_Vista obj)) return;
            _codEmpresa = obj.cod_empresa;

            EnviarCorreos();
        }

        private void btnDescargarResultadoEnvios_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (gvListadoEmailing.GetFocusedRow() is List<eEmailingBoleta> obj && obj.Count > 0)
            if (EmailsEnviados != null && EmailsEnviados.Count > 0)
                HNG.Excel.GenerateExcel_fromGridControl(gcListadoEmailing, $"{DateTime.Now.Ticks}_Boletas_Enviadas");
        }

        private void btnCloseAvance_Click(object sender, EventArgs e)
        {
            pnlAvance.Visible = false;
            progressBar.Controls.Clear();
        }

        private void gvListadoEmailing_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.Column.FieldName == "flg_enviado")
                {
                    e.Appearance.ForeColor = e.Appearance.BackColor;
                    var obj = gvListadoEmailing.GetRow(e.RowHandle) as eEmailingBoleta;
                    e.DefaultDraw();
                    if (obj.flg_enviado.Equals("SI"))
                    {
                        e.Cache.DrawImage(Properties.Resources.ok_18px, e.Bounds.X + 15, e.Bounds.Y);
                    }
                    else
                    {
                        e.Cache.DrawImage(Properties.Resources.unchecked_radio_button_18px, e.Bounds.X + 15, e.Bounds.Y);
                    }
                }
            }
        }


        private class ExpDoc { public string Documento { get; set; } public string Archivo { get; set; } }
        private void btnExportarNoRegistrados_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (listNoRegitrados.Items.Count > 0)
            {
                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Preparando para exportar", "Procesando...");
                //StringBuilder sb = new StringBuilder();
                //sb.AppendLine("Documento,Archivo");
                //foreach (ListViewItem docs in listNoRegitrados.Items)
                //{
                //    if (docs.Checked)
                //    {
                //        /*-----*Formato: LEGUI_0142805995202207.PDF*-----*/
                //        string _nombre = (docs.Text.Split('.')[0]).Split('_')[1];
                //        string _documento = _nombre.Substring(2, _nombre.Length - 8);
                //        sb.Append($"{_documento},{docs.Text}");
                //        sb.AppendLine();
                //    }
                //}
                //ToExcel();
                var list = new List<ExpDoc>();
                foreach (ListViewItem docs in listNoRegitrados.Items)
                {
                    if (docs.Checked)
                    {
                        /*-----*Formato: LEGUI_0142805995202207.PDF*-----*/
                        string _nombre = (docs.Text.Split('.')[0]).Split('_')[1];
                        string _documento = _nombre.Substring(2, _nombre.Length - 8);
                        list.Add(new ExpDoc() { Documento = String.Concat("D ", _documento), Archivo = docs.Text });
                    }
                }
                /*-----*Exportar*-----*/
                HNG.Excel.GenerateExcel_fromList<ExpDoc>(list, $"{DateTime.Now.Ticks}_Boletas_no_registradas");

                SplashScreenManager.CloseForm();
            }

        }

        private void listDocumentos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                System.Text.StringBuilder copy_buffer = new System.Text.StringBuilder();
                foreach (ListViewItem item in listDocumentos.SelectedItems)
                    copy_buffer.AppendLine(item.Text.ToString());
                if (copy_buffer.Length > 0)
                    Clipboard.SetText(copy_buffer.ToString());
            }
        }

        private void listNoRegitrados_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                System.Text.StringBuilder copy_buffer = new System.Text.StringBuilder();
                foreach (ListViewItem item in listNoRegitrados.SelectedItems)
                    copy_buffer.AppendLine(item.Text.ToString());
                if (copy_buffer.Length > 0)
                    Clipboard.SetText(copy_buffer.ToString());
            }
        }

        private void bnRecargarExplorador_ItemClick(object sender, ItemClickEventArgs e)
        {
            unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Cargando Explorador", "Procesando...");
            teeExplorador.Controls.Clear();
            teeExplorador.Nodes.Clear();
            this.teeExplorador.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.teeExplorador_NodeMouseClick);
            PopulateTreeView();
            SplashScreenManager.CloseForm();
        }

        private void btnExportarPDF_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (EmailsEnviados != null && EmailsEnviados.Count > 0)
                HNG.Excel.GeneratePdf_fromGridControlII(gcListadoEmailing, $"{DateTime.Now.Ticks}_Boletas_Enviadas");
        }

        //private void ToExcel()
        //{
        //    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
        //    app.Visible = true;
        //    Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(1);
        //    Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];
        //    int i = 1;
        //    int i2 = 1;
        //    foreach (ListViewItem lvi in listNoRegitrados.Items)
        //    {
        //        i = 1;
        //        foreach (ListViewItem.ListViewSubItem lvs in lvi.SubItems)
        //        {
        //            ws.Cells[i2, i] = lvs.Text;
        //            i++;
        //        }
        //        i2++;
        //    }
        //    //ws
        //}
        private void cbxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            listDocumentos.BeginUpdate();
            foreach (ListViewItem item in listDocumentos.Items)
                item.Checked = cbxSelectAll.Checked;
            listDocumentos.EndUpdate();
        }
    }
}