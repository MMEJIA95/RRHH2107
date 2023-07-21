using BE_GestionRRHH;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraSplashScreen;
using iTextSharp.text.pdf.codec.wmf;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static BE_GestionRRHH.eTrabajador;

namespace UI_GestionRRHH.Formularios.Personal
{
    public partial class frmAdjuntarArchivo : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;
        List<eTrabajador.eInfoFamiliar_Trabajador> listTrabajador;
        public string empresa = "", cod_trabajador="";
        private Microsoft.Graph.GraphServiceClient GraphClient { get; set; }
        AuthenticationResult authResult = null;
        string[] scopes = new string[] { "Files.ReadWrite.All" };
        string varPathOrigen = "";
        string varNombreArchivo = "", varNombreArchivoSinExtension = "";
        public frmAdjuntarArchivo()
        {
            unit = new UnitOfWork();
            InitializeComponent();
            configurar_formulario();
       
        }

        private void frmAdjuntarArchivo_Load(object sender, EventArgs e)
        {
            listado();
        }

        private void listado()
        {
            listTrabajador = unit.Trabajador.ListarTrabajadoresvista<eTrabajador.eInfoFamiliar_Trabajador>(opcion: 10, cod_trabajador: cod_trabajador, cod_empresa: empresa);
            bshijos.DataSource = listTrabajador; gvhijos.RefreshData();

           
        }

     

        private void gvhijos_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.CellValue == null) return;
       
            eTrabajador.eInfoFamiliar_Trabajador obj = gvhijos.GetFocusedRow() as eTrabajador.eInfoFamiliar_Trabajador;
            if (obj == null) return;
            
            if (e.RowHandle >= 0)
            {
                if (obj.flg_DNI_documentofam == "SI" && obj.flg_CERTIFICADOESTUDIOS_documentofam == "SI")
                {
                    obj.flg_DNI_documentofam = "VER DOC";
                    obj.flg_CERTIFICADOESTUDIOS_documentofam = "VER DOC";
                    obj.dsc_notificación = "SI";
                    if (e.RowHandle != GridControl.NewItemRowHandle && e.Column.FieldName == "dsc_notificacion" && e.CellValue.ToString() == "SI")
                    {
                        e.RepositoryItem = repositoryItemTextEdit2;
                        e.Column.Name = "";


                    }
                }
                else if (obj.flg_DNI_documentofam == "SI" && obj.flg_CERTIFICADOESTUDIOS_documentofam == null)
                {
                    obj.flg_CERTIFICADOESTUDIOS_documentofam = "ADJUNTAR DOC";
                    obj.flg_DNI_documentofam = "VER DOC";
                    obj.dsc_notificación = "NO";
                    if (e.RowHandle != GridControl.NewItemRowHandle && e.Column.FieldName == "dsc_notificacion" && e.CellValue.ToString() == "NO")
                    {
                        e.RepositoryItem = repositoryItemTextEdit1;
                        e.Column.Name = "";

                    }
                }
                else if (obj.flg_DNI_documentofam == null && obj.flg_CERTIFICADOESTUDIOS_documentofam == "SI")
                {
                    obj.flg_CERTIFICADOESTUDIOS_documentofam = "VER DOC";
                    obj.flg_DNI_documentofam = "ADJUNTAR DOC";
                    obj.dsc_notificación = "NO";
                    if (e.RowHandle != GridControl.NewItemRowHandle && e.Column.FieldName == "dsc_notificacion" && e.CellValue.ToString() == "NO")
                    {
                        e.RepositoryItem = repositoryItemTextEdit1;
                        e.Column.Name = "";

                    }
                }
                else if (obj.flg_DNI_documentofam == null && obj.flg_CERTIFICADOESTUDIOS_documentofam == null)
                {
                    obj.dsc_notificación = "NO";
                    obj.flg_CERTIFICADOESTUDIOS_documentofam = "ADJUNTAR DOC";
                    obj.flg_DNI_documentofam = "ADJUNTAR DOC";
                    if (e.RowHandle != GridControl.NewItemRowHandle && e.Column.FieldName == "dsc_notificacion" && e.CellValue.ToString() == "NO")
                    {
                        e.RepositoryItem = repositoryItemTextEdit1;
                        e.Column.Name = "";

                    }
                }

            }
        }

        private async void gvhijos_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                eTrabajador.eInfoFamiliar_Trabajador objh = gvhijos.GetFocusedRow() as eTrabajador.eInfoFamiliar_Trabajador;
                eTrabajador.eInfoFamiliar_Trabajador obj = new eTrabajador.eInfoFamiliar_Trabajador();

                if (e.Clicks == 1 && e.Column.FieldName == "flg_CERTIFICADOESTUDIOS_documentofam" && objh.flg_CERTIFICADOESTUDIOS_documentofam== "ADJUNTAR DOC")
                {
                    if (Convert.ToInt32(objh.cod_infofamiliar) != 0)
                    {
                        await AdjuntarDocumentoFamiliar(2, "Cert. Estudio", nombreDocAdicional: objh.dsc_nombrescompletos_hijo,objh.cod_infofamiliar);

                    }

                }
                if (e.Clicks == 1 && e.Column.FieldName == "flg_CERTIFICADOESTUDIOS_documentofam" && objh.flg_CERTIFICADOESTUDIOS_documentofam == "VER DOC")
                {
                    if (Convert.ToInt32(objh.cod_infofamiliar) != 0)
                    {
                        VerDocumentoEMO(1, "Cert. Estudio",objh.dsc_nombrescompletos_hijo);

                    }

                }
                if (e.Clicks == 1 && e.Column.FieldName == "flg_DNI_documentofam" && objh.flg_DNI_documentofam == "ADJUNTAR DOC")
                {
                    if (Convert.ToInt32(objh.cod_infofamiliar) != 0)
                    {
                        await AdjuntarDocumentoFamiliar(1, "Doc. Identidad", nombreDocAdicional: objh.dsc_nombrescompletos_hijo, objh.cod_infofamiliar);

                    }

                }
                if (e.Clicks == 1 && e.Column.FieldName == "flg_DNI_documentofam" && objh.flg_DNI_documentofam == "VER DOC")
                {
                    if (Convert.ToInt32(objh.cod_infofamiliar) != 0)
                    {
                        VerDocumentoEMO(2, "Doc. Identidad", objh.dsc_nombrescompletos_hijo);

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;
            unit.Globales.ConfigurarGridView_ClasicStyle(gchijos, gvhijos);

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

        private void gvhijos_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0) ; 
        }

        private void gvhijos_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private async Task AdjuntarDocumentoFamiliar(int opcionDoc, string nombreDoc, string nombreDocAdicional = "",int codinfolab=0)
        {
            try
            {
                DateTime FechaRegistro = DateTime.Today;
                string nombreCarpeta = "";

                eTrabajador.eInfoLaboral_Trabajador resultado = unit.Trabajador.Obtener_Familiar<eTrabajador.eInfoLaboral_Trabajador>(2, cod_trabajador, empresa, cod_infofamiliar: Convert.ToInt32(codinfolab));
                if (resultado == null) { MessageBox.Show("Antes de adjuntar los docuentos debe crear al familiar.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                OpenFileDialog myFileDialog = new OpenFileDialog();
                myFileDialog.Filter = "Archivos (*.pdf)|; *.pdf";
                myFileDialog.FilterIndex = 1;
                myFileDialog.InitialDirectory = "C:\\";
                myFileDialog.Title = "Abrir archivo";
                myFileDialog.CheckFileExists = false;
                myFileDialog.Multiselect = false;

                DialogResult result = myFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string IdCarpetaTrabajador = "", Extension = "";
                    var idArchivoPDF = "";
                    var TamañoDoc = new FileInfo(myFileDialog.FileName).Length / 1024;
                    if (TamañoDoc < 4000)
                    {
                        varPathOrigen = myFileDialog.FileName;
                        //varNombreArchivo = nombreDoc + "-" + FechaRegistro.ToString("yyyyMMdd") + "."+ Path.GetExtension(myFileDialog.SafeFileName);
                        varNombreArchivo = nombreDoc + "-" + FechaRegistro.Year.ToString() + "." + FechaRegistro.ToString("MM") + "." + FechaRegistro.ToString("dd") + (nombreDocAdicional != "" ? " " + nombreDocAdicional : "") + Path.GetExtension(myFileDialog.SafeFileName);
                        //varNombreArchivoSinExtension = nombreDoc + "-" + FechaRegistro.ToString("yyyyMMdd");
                        Extension = Path.GetExtension(myFileDialog.SafeFileName);
                    }
                    else
                    {
                        MessageBox.Show("Solo puede subir archivos hasta 4MB de tamaño", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Por favor espere...", "Cargando...");
                    eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, empresa);
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
                    eDatos = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa.eOnedrive_Empresa>(13, empresa, dsc_Carpeta: "Personal");
                    var targetItemFolderId = eDatos.idCarpeta;
                    nombreCarpeta = resultado.dsc_documento + " - " + resultado.dsc_nombres_completos;
                    eTrabajador objCarpeta = unit.Trabajador.ObtenerDatosOneDrive<eTrabajador>(14, cod_trabajador: cod_trabajador);
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

                        eTrabajador.eInfoFamiliar_Trabajador objTrab = new eTrabajador.eInfoFamiliar_Trabajador();
                        objTrab.cod_trabajador = cod_trabajador;
                        objTrab.cod_empresa = empresa;
                        objTrab.cod_infofamiliar = Convert.ToInt32(codinfolab);
                        objTrab.idCarpeta_Trabajador = IdCarpetaTrabajador;
                        objTrab.id_DNI_documentofam = opcionDoc == 1 ? idArchivoPDF : null;
                        objTrab.id_CERTIFICADOESTUDIOS_documentofam = opcionDoc == 2 ? idArchivoPDF : null;



                        if (opcionDoc <= 3) res = unit.Trabajador.ActualizarInformacionDocumentosFamilia("SI", opcionDoc, objTrab);
                        if (res == "OK")
                        {
                            MessageBox.Show("Se registró el documento satisfactoriamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Hubieron problemas al registrar el documento", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    SplashScreenManager.CloseForm();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private async void VerDocumentoEMO(int opcionDoc, string nombreDoc, string hijo)
        {
            string id_certificado = "";
            eTrabajador.eInfoFamiliar_Trabajador obj = new eTrabajador.eInfoFamiliar_Trabajador();
            obj = gvhijos.GetFocusedRow() as eTrabajador.eInfoFamiliar_Trabajador;
            string cod_emo = Convert.ToString(obj.cod_infofamiliar);
            if (obj == null) return;
            switch (opcionDoc)
            {
                case 1: if (obj.id_CERTIFICADOESTUDIOS_documentofam == null || obj.id_CERTIFICADOESTUDIOS_documentofam == "") return; else { id_certificado = obj.id_CERTIFICADOESTUDIOS_documentofam; } break;
                case 2: if (obj.id_DNI_documentofam == null || obj.id_DNI_documentofam == "") return; else { id_certificado = obj.id_DNI_documentofam; } break;
            }
            eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, empresa);
            if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
            { MessageBox.Show("Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            //var app = App.PublicClientApp;
            ClientId = eEmp.ClientIdOnedrive;
            TenantId = eEmp.TenantOnedrive;
            Appl();
            var app = PublicClientApp;

            try
            {
                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Abriendo documento", "Cargando...");
                //eEmpresa eEmp = unit.Factura.ObtenerDatosEmpresa<eEmpresa>(12, obj.cod_empresa);
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

                string IdOneDriveDoc = id_certificado;
                string Extension = ".pdf";

                var fileContent = await GraphClient.Me.Drive.Items[IdOneDriveDoc].Content.Request().GetAsync();
                string ruta = unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()) + @"\" + (nombreDoc +hijo+ Extension);
                if (!System.IO.File.Exists(ruta))
                {
                    using (var fileStream = new FileStream(ruta, FileMode.Create, System.IO.FileAccess.Write))
                        fileContent.CopyTo(fileStream);
                }

                if (!System.IO.Directory.Exists(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()))) System.IO.Directory.CreateDirectory(unit.Encripta.Desencrypta(ConfigurationManager.AppSettings[unit.Encripta.Encrypta("RutaArchivosLocalExportar")].ToString()));
                System.Diagnostics.Process.Start(ruta);
                SplashScreenManager.CloseForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubieron problemas al autenticar las credenciales", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lblResultado.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                return;
            }
        }
    }    
}