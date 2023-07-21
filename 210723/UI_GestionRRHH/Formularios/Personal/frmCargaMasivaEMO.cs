using BE_GestionRRHH;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraSplashScreen;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BE_GestionRRHH.eTrabajador;

namespace UI_GestionRRHH.Formularios.Personal
{
    public partial class frmCargaMasivaEMO : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;
        public eTrabajador eTrab = new eTrabajador();
        // public string NombreArchivo;

        private List<eTrabajador.eEMO> TrabajadorListaEmo;
        private List<eTrabajador> trabajadorLista;
        public string cod_trabajador = "", cod_empresa = "", cod_empresa_multiple = "",
            cod_sede_multiple = "", flg_activo = "", estado = "", respuesta = "SI", empresas = "", sedes = "",cod_emo;
        int cod_EMO = 0;
        List<eTrabajador.eEMO> ListInfoLaboralEMO = new List<eTrabajador.eEMO>();
        eTrabajador.eEMO etrabemo = new eTrabajador.eEMO();
        eTrabajador.eDocumento_Trabajador eTrabdoc = new eTrabajador.eDocumento_Trabajador();

        //OneDrive
        private Microsoft.Graph.GraphServiceClient GraphClient { get; set; }
        AuthenticationResult authResult = null;
        string[] scopes = new string[] { "Files.ReadWrite.All" };
        string varPathOrigen = "";
        string varNombreArchivo = "", varNombreArchivoSinExtension = "";

        private void frmCargaMasivaEMO_Load(object sender, EventArgs e)
        {
            RepositoryItemTextEdit textEdit = new RepositoryItemTextEdit();
            gvArchivo.Columns["estado"].ColumnEdit = textEdit;
            rlkpEstado.DataSource = unit.Trabajador.CombosEnGridControl<eTrabajador.eEMO>("EstadoEMO");
            lkptipodocum.DataSource = unit.Trabajador.CombosEnGridControl<eTrabajador.eEMO>("TipoDoc", flg_varios:"SI",cod_empresa: cod_empresa); ;
            dtFchEvaluacion.EditValue = Convert.ToDateTime(DateTime.Today);
            dtFchEvaluacionObs.EditValue = Convert.ToDateTime(DateTime.Today);
            lkptipodoc.ItemIndex = 0;


            // gvArchivo.RepositoryItems.Add(textEdit);
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


        private async Task AdjuntarDocumentosVarios(int opcionDoc, string nombreDoc, string nombreDocAdicional = "")
        {
            string estado = "SI";
            string resultado = "";
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

                DialogResult result = myFileDialog.ShowDialog();
               if (result == DialogResult.OK)
                {
                    bsArchivos.DataSource = null; bsTrabajadorArchivos.DataSource = null; ListInfoLaboralEMO.Clear();
                    gvTrabajador.RefreshData(); gvArchivo.RefreshData();
                    string IdCarpetaTrabajador = "", Extension = "";
                    var idArchivoPDF = "";
                    var TamañoDoc = new FileInfo(myFileDialog.FileName).Length / 1024;
                    System.IO.StreamReader sr = new
                    System.IO.StreamReader(myFileDialog.FileName);
                    int count = 0;

                     //cerrarsplash();
                    unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Por favor espere...", "Cargando...");
                    //VALIDAR LISTA 
                    trabajadorLista = unit.Trabajador.ListarTrabajadoresvista<eTrabajador>(opcion: 1, cod_trabajador: "", cod_empresa: cod_empresa, cod_empresa_multiple: empresas, cod_sede_multiple: sedes, flg_activo: estado);
                    foreach (String cadena in myFileDialog.SafeFileNames)
                    {

                        eTrabajador.eEMO obj = new eTrabajador.eEMO();
                        FileInfo file = new FileInfo(cadena);
                        //obj.dsc_descripcion = file.ToString();
                        resultado = Path.GetFileNameWithoutExtension(file.Name).ToLower();
                        
                        obj.estado = "SI";
                        cod_empresa_multiple = empresas;
                        cod_sede_multiple = sedes;
                        flg_activo = "SI";

                        
                        if (TamañoDoc < 4000)
                        {

                            varPathOrigen = myFileDialog.FileNames[count];
                            varNombreArchivo = nombreDoc + "-" + FechaRegistro.Year.ToString() + "." + FechaRegistro.ToString("MM") + "." + FechaRegistro.ToString("dd") + (nombreDocAdicional != "" ? " " + nombreDocAdicional : "") + Path.GetExtension(myFileDialog.SafeFileName);
                            Extension = Path.GetExtension(myFileDialog.SafeFileName);

                        }
                        else
                        {
                            MessageBox.Show("Solo puede subir archivos hasta 4MB de tamaño", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        count++;
                        //foreach (var item in trabajadorLista)
                        //{
                        if (trabajadorLista.Find(x => x.dsc_documento.ToLower() == resultado) != null)
                        //if (obj.dsc_descripcion.ToLower().Contains(item.dsc_documento.ToLower()))
                        {

                            eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, cod_empresa);
                            if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
                            { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                            ClientId = eEmp.ClientIdOnedrive;
                            TenantId = eEmp.TenantOnedrive;
                            Appl();
                            var app = PublicClientApp;
                            string correo = eEmp.UsuarioOnedrivePersonal;
                            string password = eEmp.ClaveOnedrivePersonal;

                            var securePassword = new SecureString();
                            foreach (char c in password)
                                securePassword.AppendChar(c);

                            authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

                            GraphClient = new Microsoft.Graph.GraphServiceClient(
                              new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                              {
                                  requestMessage
                                      .Headers
                                      .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                                  return Task.FromResult(0);
                              }));

                            eEmpresa.eOnedrive_Empresa eDatos = new eEmpresa.eOnedrive_Empresa();
                            eDatos = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa.eOnedrive_Empresa>(13, cod_empresa, dsc_Carpeta: "Personal");
                            var targetItemFolderId = eDatos.idCarpeta;
                            eTrabajador.eEMO emo = new eTrabajador.eEMO();
                            eTrab = unit.Trabajador.Obtener_cod_trabajador<eTrabajador>(56, cod_empresa: cod_empresa, dsc_documento: resultado);
                            emo.cod_trabajador = eTrab.cod_trabajador;
                            emo.dsc_documento = eTrab.dsc_documento;
                            emo.dsc_nombres_completos = eTrab.dsc_nombres_completos;



                            nombreCarpeta = (emo.dsc_documento + " - " + emo.dsc_nombres_completos).Trim();
                            eTrabajador objCarpeta = unit.Trabajador.ObtenerDatosOneDrive<eTrabajador>(14, cod_trabajador: emo.cod_trabajador);
                            if (objCarpeta.idCarpeta_Trabajador == null || objCarpeta.idCarpeta_Trabajador == "") //Si no existe folder lo crea
                            {
                                var driveItem = new Microsoft.Graph.DriveItem
                                {
                                    Name = nombreCarpeta,
                                    Folder = new Microsoft.Graph.Folder
                                    {
                                    },
                                    AdditionalData = new Dictionary<string, object>()
                                        {
                                        {"@microsoft.graph.conflictBehavior", "rename"}
                                        }
                                };

                                var driveItemInfo = await GraphClient.Me.Drive.Items[targetItemFolderId].Children.Request().AddAsync(driveItem);
                                IdCarpetaTrabajador = driveItemInfo.Id;
                            }
                            else //Si existe folder obtener id
                            {
                                IdCarpetaTrabajador = objCarpeta.idCarpeta_Trabajador;
                            }

                            //crea archivo en el OneDrive
                            byte[] data = System.IO.File.ReadAllBytes(varPathOrigen);
                            using (Stream stream = new MemoryStream(data))
                            {
                                string res = "";
                                var DriveItem = await GraphClient.Me.Drive.Items[IdCarpetaTrabajador].ItemWithPath(varNombreArchivo).Content.Request().PutAsync<Microsoft.Graph.DriveItem>(stream);
                                idArchivoPDF = DriveItem.Id;

                                if (opcionDoc == 8)
                                {
                                    string resulto = ""; string cod_doc = ""; string abre = "";
                                    eTrabajador.eDocumento_Trabajador objInfolab = new eTrabajador.eDocumento_Trabajador();


                                    cod_doc = "";
                                    eTrabdoc = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(109, cod_empresa:cod_empresa);
                                    eTrabajador objTrab = new eTrabajador();
                                    objTrab.cod_trabajador = emo.cod_trabajador;
                                    objTrab.cod_empresa = cod_empresa;
                                    objTrab.idCarpeta_Trabajador = IdCarpetaTrabajador;
                                    res = unit.Trabajador.ActualizarInformacionDocumentos("SI", opcionDoc, objTrab);

                                    eTrabajador.eEMO eEMO = new eTrabajador.eEMO();
                                    eEMO.cod_trabajador = emo.cod_trabajador;
                                    eEMO.cod_empresa = cod_empresa;
                                    eEMO.cod_EMO = emo.cod_EMO;
                                    eEMO.fch_registro = FechaRegistro;
                                    //if (grdbFlgObservado.SelectedIndex == 0) { eEMO.flg_observacion = "APTO"; } else if (grdbFlgObservado.SelectedIndex == 1) { eEMO.flg_observacion = "NO APTO"; } else { eEMO.flg_observacion = "APTO CON RESTRICCIONES"; }
                                    //eEMO.fch_evaluacion = dtFchEvaluacion.EditValue == null ? new DateTime() : Convert.ToDateTime(dtFchEvaluacion.EditValue);
                                    //eEMO.fch_evaluacion_obs = dtFchEvaluacionObs.EditValue == null ? new DateTime() : Convert.ToDateTime(dtFchEvaluacionObs.EditValue);
                                    //eEMO.dsc_observacion = memObservacion.Text;
                                    eEMO.flg_observacion = "APTO";
                                    eEMO.fch_evaluacion = FechaRegistro;
                                    eEMO.fch_evaluacion_obs = new DateTime();
                                    eEMO.dsc_descripcion = emo.dsc_documento + "_" + FechaRegistro.Year.ToString() + "." + FechaRegistro.ToString("MM") + "." + FechaRegistro.ToString("dd") + "-" + eTrabdoc.dsc_abreviatura;
                                    eEMO.dsc_observacion = "";
                                    eEMO.cod_documento = eTrabdoc.cod_documento;
                                    eEMO.dsc_nombres_completos = emo.dsc_nombres_completos;
                                    eEMO.dsc_anho = FechaRegistro.Year; eEMO.flg_certificado = "SI"; eEMO.id_certificado = idArchivoPDF;
                                    eEMO.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;
                                    eEMO.nombre_archivoonedrive = varNombreArchivo;

                                    eEMO = unit.Trabajador.InsertarActualizar_EMOTrabajador<eTrabajador.eEMO>(eEMO, opcion: 1);

                                    if (eEMO != null)
                                    {
                                        //eTrabajador.eEMO em = new eTrabajador.eEMO();
                                       // string tipodoc = lkptipodocum.EditValueChanged();
                                       eTrab = unit.Trabajador.InsertarActualizar_EMOTrabajador<eTrabajador.eEMO>(eEMO, opcion: 3);
                                        //em.cod_trabajador = eTrab.cod_trabajador;
                                        eEMO.dsc_documento = eTrab.dsc_documento;
                                         eEMO.dsc_nombres_completos = eTrab.dsc_nombres_completos;
                                        eEMO.dsc_tipo_documento = eTrabdoc.dsc_documento;
                                        //em.dsc_anho = FechaRegistro.Year;
                                        //em.fch_evaluacion_obs = eTrab.fch_evaluacion_obs;
                                        //em.flg_observacion = "APTO";
                                        //em.dsc_observacion = eTrab.dsc_observacion;
                                        //em.cod_EMO = eEMO.cod_EMO;
                                        //bsTrabajadorArchivos.Add(em);
                                        ListInfoLaboralEMO.Add(eEMO);
                                        
                                    }
                                    obj.dsc_descripcion = Convert.ToString(file);
                                    obj.estado = "SI";
                                    bsArchivos.Add(obj);
                                    res = eEMO != null ? "OK" : "ERROR";
                                }

                                //if (res == "OK")
                                //{
                                //    MessageBox.Show("Se registró el documento satisfactoriamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //}
                                //else
                                //{
                                //    MessageBox.Show("Hubieron problemas al registrar el documento", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //}
                            }
                            // count++;

                        }
                        else
                        {
                            obj.estado = "NO";
                            obj.dsc_descripcion = Convert.ToString(file);
                            bsArchivos.Add(obj);
                        }
                        bsTrabajadorArchivos.DataSource = ListInfoLaboralEMO;
                        gvTrabajador.RefreshData();
                        gvArchivo.RefreshData();
                        sr.Close();
                        //}
                    }

                    SplashScreenManager.CloseForm();
                    MessageBox.Show("Se registraron los documentos de manera satisfactoriamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void cerrarsplash()
        {
            foreach (Form splash in System.Windows.Forms.Application.OpenForms)
            {
                if (splash.Name.Equals("FrmSplashCarga"))
                {
                    //splash.Close();
                    SplashScreenManager.CloseForm();
                    break;
                }
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        private async void btnOpcional_Click(object sender, EventArgs e)
        {
            eTrabajador.eEMO obj = new eTrabajador.eEMO();
            //desbloqueo();


            await AdjuntarDocumentosVarios(8, "EMO");
        }
        private void desbloqueo()
        {
            dtFchEvaluacion.Enabled = true;
            memObservacion.Enabled = true;
            grdbFlgObservado.Enabled = true;
            dtFchEvaluacionObs.Enabled = true;
        }

        private void gcArchivo_Click(object sender, EventArgs e)
        {

        }

        private void gvTrabajador_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //mostrardatos();

        }

        private void gvArchivo_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (e.RowHandle != GridControl.NewItemRowHandle && e.Column.FieldName == "estado")
                {
                    if (e.CellValue == null) return;
                    e.RepositoryItem = e.CellValue.ToString() == "SI" ? repositoryItemTextEdit1 : repositoryItemTextEdit2;
                }
               
            }
        }

        private void gvArchivo_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0) ;
        }

        private void gvTrabajador_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                gvTrabajador_FocusedRowChanged(gvTrabajador, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, e.RowHandle));

            }
            else if (e.RowHandle < 0) { IsButtonAditional1 = false; }



        }

        private void gvTrabajador_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    eTrabajador.eEMO obj = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eEMO;
                    if (e.Column.FieldName == "fch_evaluacion_obs" && obj.fch_evaluacion_obs.ToString().Contains("1/01/0001")) e.DisplayText = "";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gvTrabajador_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            DateTime FechaRegistro = DateTime.Today;
            if (e.RowHandle >= 0)
            {
                if (e.RowHandle != GridControl.NewItemRowHandle && e.Column.FieldName == "estado")
                {
                    if (e.CellValue == null) return;
                    e.RepositoryItem = e.CellValue.ToString() == "SI" ? repositoryItemTextEdit1 : repositoryItemTextEdit2;
                }
             

            }

        }

        public frmCargaMasivaEMO()
        {
            InitializeComponent();

            unit = new UnitOfWork();
            configurar_formulario();
            CargarLookUpEdit();
            //layoutCancelar.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;




        }

        private void btnAdicional_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["frmRegistroDocumentos"] != null)
            {
                Application.OpenForms["frmRegistroDocumentos"].Activate();
            }
            else
            {

                frmRegistroDocumentos frm = new frmRegistroDocumentos();

                frm.ShowDialog();

                if (frm.Actualizarcombo == "SI")
                {
                    lkptipodocum.DataSource = unit.Trabajador.CombosEnGridControl<eTrabajador.eEMO>("TipoDoc", flg_varios: "SI", cod_empresa: cod_empresa); ;      
                }
            }
        }

        private void lkptipodocum_EditValueChanged(object sender, EventArgs e)
        {
            gvTrabajador.PostEditor();
        }

        private void gvTrabajador_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            DateTime FechaRegistro = DateTime.Today;
            string cod_doc = "";

            if (e.RowHandle >= 0 && e.Column.FieldName == "cod_documento")
            {
                eTrabajador.eEMO obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eEMO;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eEMO obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eEMO;
                    obj3.cod_documento = obj2.cod_documento;
                }
                gvTrabajador.RefreshData();

               
            }
            if (e.RowHandle >= 0 && e.Column.FieldName == "fch_evaluacion")
            {
                eTrabajador.eEMO obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eEMO;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eEMO obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eEMO;
                    obj3.fch_evaluacion = obj2.fch_evaluacion;
                }
                gvTrabajador.RefreshData();


            }
            if (e.RowHandle >= 0 && e.Column.FieldName == "flg_observacion")
            {
                eTrabajador.eEMO obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eEMO;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eEMO obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eEMO;
                    obj3.flg_observacion = obj2.flg_observacion;
                }
                gvTrabajador.RefreshData();


            }
            if (e.RowHandle >= 0 && e.Column.FieldName == "dsc_observacion")
            {
                eTrabajador.eEMO obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eEMO;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eEMO obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eEMO;
                    obj3.dsc_observacion = obj2.dsc_observacion;
                }
                gvTrabajador.RefreshData();


            }
            if (e.RowHandle >= 0 && e.Column.FieldName == "fch_evaluacion_obs")
            {
                eTrabajador.eEMO obj2 = gvTrabajador.GetRow(e.RowHandle) as eTrabajador.eEMO;
                if (obj2 == null) return;
                foreach (int nRow in gvTrabajador.GetSelectedRows())
                {
                    eTrabajador.eEMO obj3 = gvTrabajador.GetRow(nRow) as eTrabajador.eEMO;
                    obj3.fch_evaluacion_obs = obj2.fch_evaluacion_obs;
                }
                gvTrabajador.RefreshData();


            }
        }

        private void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;
            unit.Globales.ConfigurarGridView_ClasicStyle(gcArchivo, gvArchivo);
            unit.Globales.ConfigurarGridView_ClasicStyle(gcTrabajador, gvTrabajador, true, false, false);

        }

        private void grdbFlgObservado_SelectedIndexChanged(object sender, EventArgs e)
        {
            int opcion = 0;
            eTrabajador.eEMO obj = new eTrabajador.eEMO();
            opcion = grdbFlgObservado.SelectedIndex;
            switch (opcion)
            {
                case 0:
                    obj.flg_observacion = "APTO";
                    break;
                case 1:
                    obj.flg_observacion = "NO APTO";
                    break;
                case 2:
                    obj.flg_observacion = "APTO CON RESTRICCIONES";
                    break;
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            DateTime FechaRegistro = DateTime.Today;
            string cod_doc = ""; string result = "";
            //for (int nRow = 0; nRow <= gvTrabajador.RowCount - 1; nRow++)
            unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Espere, por favor...");
            foreach (eTrabajador.eEMO obj in ListInfoLaboralEMO)
            {
                
                cod_doc = obj.cod_documento;
                etrabemo = unit.Trabajador.Obtener_emo<eTrabajador.eEMO>(113, cod_trabajador: obj.cod_trabajador, cod_empresa: obj.cod_empresa, cod_documento: obj.cod_documento);
                eTrabdoc = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(109, cod_documento: obj.cod_documento);
                if (obj.cod_EMO == 1 && obj.dsc_tipo_documento == "CERTIFICADO EMO")
                {
                    result = unit.Trabajador.EliminarInactivarEMOtrabajador(9, cod_trabajador: obj.cod_trabajador, cod_empresa: obj.cod_empresa, cod_EMO: obj.cod_EMO, cod_documento: obj.cod_documento);
                }
                else 
                {
                    obj.dsc_descripcion = obj.dsc_documento + "_" + FechaRegistro.Year.ToString() + "." + FechaRegistro.ToString("MM") + "." + FechaRegistro.ToString("dd") + "-" + eTrabdoc.dsc_abreviatura + "_" + obj.cod_EMO;

                }
                eTrabajador.eEMO objEMO = new eTrabajador.eEMO();
                await Renombrar_ArchivoOneDrive(obj);
                obj.dsc_anho = FechaRegistro.Year;
                objEMO = unit.Trabajador.InsertarActualizar_EMOTrabajador<eTrabajador.eEMO>(obj, opcion: 2);
                obj.estado = objEMO != null ? "SI" : "NO";
                gvTrabajador.RefreshData();
            }
            SplashScreenManager.CloseForm();
            MessageBox.Show("Se registraron los datos de manera satisfactoria.", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

        private eTrabajador.eEMO AsignarValor_EMO()

        {
            eTrabajador.eEMO obj = new eTrabajador.eEMO();
            obj.cod_EMO = cod_EMO;
            obj.cod_trabajador = cod_trabajador;
            obj.cod_empresa = cod_empresa;
            obj.fch_evaluacion = Convert.ToDateTime(dtFchEvaluacion.EditValue);
            obj.dsc_observacion = memObservacion.Text == null ? null : memObservacion.Text;
            obj.fch_evaluacion_obs = Convert.ToDateTime(dtFchEvaluacionObs.EditValue);
            obj.fch_registro = Convert.ToDateTime(DateTime.Today);
            if (grdbFlgObservado.SelectedIndex == 0) { obj.flg_observacion = "APTO"; } else if (grdbFlgObservado.SelectedIndex == 1) { obj.flg_observacion = "NO APTO"; } else { obj.flg_observacion = "APTO CON RESTRICCIONES"; }
            obj.cod_usuario_registro = Program.Sesion.Usuario.cod_usuario;

            return obj;
        }

        private void cargartrabajador()
        {
            trabajadorLista = unit.Trabajador.ListarTrabajadoresvista<eTrabajador>(opcion: 1, cod_trabajador: "", cod_empresa: cod_empresa, cod_empresa_multiple: cod_empresa_multiple, cod_sede_multiple: cod_sede_multiple, flg_activo: flg_activo);

        }

        public void mostrararchivos(string dsc_descripcion)
        {
            eTrabajador.eEMO emo = new eTrabajador.eEMO();
            eTrabajador.eEMO EMO2 = new eTrabajador.eEMO();
            cargartrabajador();
            foreach (var item in trabajadorLista)
            {
                if (dsc_descripcion.ToLower().Contains(item.dsc_documento.ToLower()))
                {
                    respuesta = "SI";
                    break;
                }
                else
                {
                    respuesta = "NO";
                }
            }
        }

        public void mostrardatos()
        {
            eTrabajador.eEMO obj = gvTrabajador.GetFocusedRow() as eTrabajador.eEMO;
            cod_trabajador = obj.cod_trabajador;
            cod_EMO = obj.cod_EMO;
            if (obj == null) return;
            dtFchEvaluacion.Text = Convert.ToString(obj.fch_evaluacion);
            if (obj.flg_observacion == "APTO") { grdbFlgObservado.SelectedIndex = 0; }
            if (obj.flg_observacion == "NO APTO") { grdbFlgObservado.SelectedIndex = 1; }
            if (obj.flg_observacion == "APTO CON RESTRICCIONES") { grdbFlgObservado.SelectedIndex = 2; }
            memObservacion.Text = obj.dsc_observacion;
            dtFchEvaluacionObs.Text = Convert.ToString(obj.fch_evaluacion_obs);
            lkptipodoc.EditValue = obj.cod_documento;
        }

        private void ObtenerDatos_EMO()
        {
            ListInfoLaboralEMO = unit.Trabajador.ListarEMO<eTrabajador.eEMO>(23, cod_trabajador, cod_empresa);
            bsTrabajadorArchivos.DataSource = ListInfoLaboralEMO;
            gvTrabajador.RefreshData();
        }

        private void CargarLookUpEdit()
        {
            try
            {

                unit.Trabajador.CargaCombosLookUp("TipoDocumento", lkptipodoc, "cod_documento", "dsc_documento", "", valorDefecto: true, flg_varios: "SI", cod_trabajador: cod_trabajador,cod_empresa:cod_empresa);

                lkptipodoc.ItemIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        
            private async Task Renombrar_ArchivoOneDrive(eTrabajador.eEMO obj)
            {
            try
            {
                eTrabajador.eEMO objw = gvTrabajador.GetFocusedRow() as eTrabajador.eEMO;
                etrabemo = unit.Trabajador.Obtener_emo<eTrabajador.eEMO>(113,cod_trabajador:obj.cod_trabajador,cod_empresa:obj.cod_empresa, cod_documento: obj.cod_documento);
                eTrabdoc = unit.Trabajador.Obtener_emo<eTrabajador.eDocumento_Trabajador>(109, cod_documento: obj.cod_documento);
                DateTime FechaRegistro = DateTime.Today;

                varNombreArchivo = eTrabdoc.dsc_abreviatura + "-" + FechaRegistro.Year.ToString() + "." + FechaRegistro.ToString("MM") + "." + FechaRegistro.ToString("dd")+"_"+objw.cod_EMO;

                eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, obj.cod_empresa);
    
                if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
                { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                ClientId = eEmp.ClientIdOnedrive;
                TenantId = eEmp.TenantOnedrive;
                Appl();
                var app = PublicClientApp;
                ////var app = App.PublicClientApp;
                string correo = eEmp.UsuarioOnedrivePersonal;
                string password = eEmp.ClaveOnedrivePersonal;

                var securePassword = new SecureString();
                foreach (char c in password)
                    securePassword.AppendChar(c);

                authResult = await app.AcquireTokenByUsernamePassword(scopes, correo, securePassword).ExecuteAsync();

                GraphClient = new Microsoft.Graph.GraphServiceClient(
                new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                {
                    requestMessage
                    .Headers
                    .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                    return Task.FromResult(0);
                }));

                    string extension = ".pdf";
                string idArchivo = obj.id_certificado;

                    //////////////////////////////////////////////////////// RENOMBRAR DOCUMENTO DE ONEDRIVE ////////////////////////////////////////////////////////
                    GraphClient = new Microsoft.Graph.GraphServiceClient(
                        new Microsoft.Graph.DelegateAuthenticationProvider((requestMessage) =>
                        {
                            requestMessage
                                .Headers
                                .Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                            return Task.FromResult(0);
                        }));



                    var driveItem = new Microsoft.Graph.DriveItem
                    {
                        Name = varNombreArchivo + extension
                    };



                    await GraphClient.Me.Drive.Items[idArchivo]
                        .Request()
                        .UpdateAsync(driveItem);
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
               


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }



    }
}