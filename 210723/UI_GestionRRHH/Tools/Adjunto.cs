using BE_GestionRRHH;
using DevExpress.XtraSplashScreen;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UI_GestionRRHH.Tools
{
    internal class Adjunto
    {
        private readonly UnitOfWork unit;
        private static string ClientId = "";
        private static string TenantId = "";
        private static string Instance = "https://login.microsoftonline.com/";
        private Microsoft.Graph.GraphServiceClient GraphClient { get; set; }
        AuthenticationResult authResult = null;
        string[] scopes = new string[] { "Files.ReadWrite.All" };
        private static IPublicClientApplication _clientApp;
        private static IPublicClientApplication PublicClientApp { get { return _clientApp; } }
        public string[] PathOrigin;
        private string _nombreCarpeta;
        private string Extension;
        private string[] SafeFileName;
        private string NombreArchivo;
        internal class AdjuntoResumen { public string CodEmpresa { get; set; } public string CodTrabajador { get; set; } public string IdCarpetaTrabajador { get; set; } public string IdArchivoPDF { get; set; } }
        public Adjunto(UnitOfWork unit) { this.unit = unit; PathOrigin = null; Extension = ""; SafeFileName = null; NombreArchivo = ""; }

        static void Appl()
        {
            _clientApp = PublicClientApplicationBuilder.Create(ClientId)
                .WithAuthority($"{Instance}{TenantId}")
                .WithDefaultRedirectUri()
                .Build();
            TokenCacheHelper.EnableSerialization(_clientApp.UserTokenCache);
        }
        public bool CrearRutaPDF(bool multiselect)
        {
            bool flag = false;
            using (var o = new System.Windows.Forms.OpenFileDialog()
            {
                InitialDirectory = "C:\\",
                Multiselect = multiselect,
                Filter = "Archivos (*.pdf)|; *.pdf",
                Title = "Abrir formatos",
                CheckFileExists = false, //??
            })
            {
                if (o.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (o.FileName != "")
                    {
                        var TamañoDoc = new FileInfo(o.FileName).Length / 1024;
                        if (TamañoDoc < 4000)
                        {
                            PathOrigin = o.FileNames;
                            Extension = Path.GetExtension(o.SafeFileName);
                            SafeFileName = o.SafeFileNames;
                            flag = true;
                        }
                        else
                        {
                            unit.Globales.Mensaje(false, "Solo puede subir archivos hasta 4MB de tamaño", "Información");
                        }
                    }
                }
            }
            return flag;
        }
        public async Task<List<AdjuntoResumen>> AdjuntarDocumentosVarios(eTrabajador trabajador, string nombreDoc, string nombreDocAdicional = "")
        {
            var returnList = new List<AdjuntoResumen>();
            try
            {
                DateTime FechaRegistro = DateTime.Today;
                string nombreCarpeta = "";
                if (PathOrigin != null)
                {
                    string IdCarpetaTrabajador = "";
                    var idArchivoPDF = "";
                    NombreArchivo = nombreDoc + "-" + FechaRegistro.Year.ToString() + "." + FechaRegistro.ToString("MM") + "." + FechaRegistro.ToString("dd") + (nombreDocAdicional != "" ? " " + nombreDocAdicional : "") + Extension;// Path.GetExtension(myFileDialog.SafeFileName);

                    unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Por favor espere...", "Cargando...");
                    eEmpresa eEmp = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa>(12, trabajador.cod_empresa);
                    if (eEmp.ClientIdOnedrive == null || eEmp.ClientIdOnedrive == "")
                    { unit.Globales.Mensaje(false, "Debe configurar los datos del Onedrive de la empresa asignada", "Onedrive"); return null; }

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
                    eDatos = unit.Trabajador.ObtenerDatosOneDrive<eEmpresa.eOnedrive_Empresa>(13, trabajador.cod_empresa, dsc_Carpeta: "Personal");
                    var targetItemFolderId = eDatos.idCarpeta;

                    nombreCarpeta = trabajador.dsc_documento + " - " + trabajador.dsc_nombres_completos;
                    eTrabajador objCarpeta = unit.Trabajador.ObtenerDatosOneDrive<eTrabajador>(14, cod_trabajador: trabajador.cod_trabajador);
                    if (objCarpeta.idCarpeta_Trabajador == null || objCarpeta.idCarpeta_Trabajador == "") //Si no existe folder lo crea
                    {
                        var driveItem = new Microsoft.Graph.DriveItem
                        {
                            Name = nombreCarpeta,
                            Folder = new Microsoft.Graph.Folder { },
                            AdditionalData = new Dictionary<string, object>() { { "@microsoft.graph.conflictBehavior", "rename" } }
                        };

                        var driveItemInfo = await GraphClient.Me.Drive.Items[targetItemFolderId].Children.Request().AddAsync(driveItem);
                        IdCarpetaTrabajador = driveItemInfo.Id;
                    }
                    else //Si existe folder obtener id
                    {
                        IdCarpetaTrabajador = objCarpeta.idCarpeta_Trabajador;
                    }

                    //crea archivo en el OneDrive
                    foreach (string path in PathOrigin)
                    {
                        byte[] data = System.IO.File.ReadAllBytes(path);
                        using (Stream stream = new MemoryStream(data))
                        {
                            var DriveItem = await GraphClient.Me.Drive.Items[IdCarpetaTrabajador].ItemWithPath(NombreArchivo).Content.Request().PutAsync<Microsoft.Graph.DriveItem>(stream);
                            idArchivoPDF = DriveItem.Id;

                            returnList.Add(new AdjuntoResumen()
                            {
                                CodEmpresa = trabajador.cod_empresa,
                                CodTrabajador = trabajador.cod_trabajador,
                                IdCarpetaTrabajador = IdCarpetaTrabajador,
                                IdArchivoPDF = idArchivoPDF
                            });
                            if (!string.IsNullOrWhiteSpace(idArchivoPDF)) { unit.Globales.Mensaje(true, "Se registró el documento satisfactoriamente", "Información"); }
                            else { unit.Globales.Mensaje(false, "Hubieron problemas al registrar el documento", "Información"); }
                        }
                    }
                    SplashScreenManager.CloseForm();
                }
            }
            catch (Exception ex)
            {
                unit.Globales.Mensaje(false, ex.ToString(), "");
            }
            return returnList;
        }
    }
}
