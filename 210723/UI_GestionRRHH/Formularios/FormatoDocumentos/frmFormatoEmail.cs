using BE_GestionRRHH;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BL_GestionRRHH.blGlobales;

namespace UI_GestionRRHH.Formularios.FormatoDocumentos
{
    public partial class frmFormatoEmail : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;
        string cod_empresa;
        string dsc_trabajador;
        string dsc_asunto;
        string dsc_cuerpo;
        public frmFormatoEmail()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            configurar_formulario();
        }

        private void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;
            //unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoAcumulado, gvListadoAcumulado);
            txtAsunto.Controls.Add(new Label() { Dock = DockStyle.Bottom, Height = 2, Text = "", BackColor = Program.Sesion.Colores.Verde });
            pnlPara.Controls.Add(new Label() { Dock = DockStyle.Bottom, Height = 2, Text = "", BackColor = Program.Sesion.Colores.Verde });
            pnlCopia.Controls.Add(new Label() { Dock = DockStyle.Bottom, Height = 2, Text = "", BackColor = Program.Sesion.Colores.Verde });

        }

        internal void CargarInfo(string mailSplit, string adjuntoSplit, string dscTrabajador, string dscDocumento, string codEmpresa)
        {
            CrearEtiquetaDestinatario(mailSplit);
            CrearEtiquetaAdjunto(adjuntoSplit);
            cod_empresa = codEmpresa;
            dsc_trabajador = dscTrabajador;
            txtAsunto.Text = $"DOCUMENTO {dscDocumento}";
            txtCuerpo.Text = $"Estimado(a): {dsc_trabajador}\n\n" +
                $"Por este medio le hacemos llegar el documento que corresponde a ...\n\n" +
                $"Atentamente.\n" +
                $"Gerencia Administrativa...";
        }

        private void CrearEtiquetaDestinatario(string mailSplit)
        {
            string[] array = mailSplit.Split(',');
            int i = pnlPara.Controls.Count;
            foreach (string email in array)
            {
                bool continuar = true;
                foreach (Label ex in pnlPara.Controls.OfType<Label>().ToList())
                    if (ex.Text.Equals(email.Trim())) { continuar = false; break; }

                if (!continuar) continue;

                i++;
                string id = $"_to{i}";
                var para = new Label()
                {
                    Name = id,
                    Text = email.Trim(),
                    Dock = DockStyle.Left,
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Cursor = Cursors.Hand,

                };

                para.DoubleClick += (sender, args) => DeleteToMail(id);
                pnlPara.Controls.Add(para);
                para.BringToFront();
            }
        }
        private void CrearEtiquetaConCopia(string copiaSplit)
        {
            string[] array = copiaSplit.Split(',');
            int i = pnlCopia.Controls.Count;
            foreach (string email in array)
            {
                bool continuar = true;
                foreach (Label ex in pnlCopia.Controls.OfType<Label>().ToList())
                    if (ex.Text.Equals(email.Trim())) { continuar = false; break; }

                if (!continuar) continue;

                i++;
                string id = $"_cc{i}";
                var copia = new Label()
                {
                    Name = id,
                    Text = email,
                    Dock = DockStyle.Left,
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Cursor = Cursors.Hand,
                };

                copia.DoubleClick += (sender, args) => DeleteToCopy(id);
                pnlCopia.Controls.Add(copia);
                copia.BringToFront();
            }
        }

        private void EnviarMail()
        {
            //Traer credenciales de todas las empresas
            var formatoEnvio = unit.Globales.Mensaje(TipoMensaje.YesNo,
                "¿Seguro de continuar con el envío?", "Formato envío masivo de Boletas");
            if (formatoEnvio == DialogResult.No)
            {
                //Se cnacela el proceso.
                //flagEnviado = false;
                //lblProceso.Text = $"Status: enviados <b>(0)</b> de <b>({numSeleccionadosPorEnviar})</b>  boletas.";
                return;
            }

            bool enviado = false;
           // foreach (var item in pnlPara.Controls.OfType<Label>().ToList())
            {
              //  if (item.Name.Contains("_to"))
                {
                    string email = GetEmailDestinoSplit();// item.Text.ToLower().Trim();
                    //if (new EmailAddressAttribute().IsValid(email))
                    {
                        //Obtener Credencial asignado a cada empresa
                        // ver si las credenciales trae  valores vacíos.
                        var credencial = GetCredenciales().Where((e) => e.cod_clave.Equals(cod_empresa)).ToList();

                        enviado = unit.Globales.EnviarCorreoElectronico_SMTP(
                           sDestinatario: email, sCopia: GetEmailCopiaSplit(), sAsunto: dsc_asunto, sCuerpo: dsc_cuerpo, credenciales: credencial,
                           RutasAdjunto: GetDirectorioAjunto(), ArchivosAdjunto: GetArrayArchivoAdjunto());
                        //MessageBox.Show(__credencial[0].dsc_valor.ToString());
                        // arreglar   probar el doWorker
                    }
                }
            }

            if (enviado)
            {
                MessageBox.Show("Enviado");
            }
        }
        private string GetEmailDestinoSplit()
        {
            string copia = "delete";
            foreach (var item in pnlPara.Controls.OfType<Label>().ToList())
            {
                if (item.Name.Contains("_to"))
                {
                    if(new EmailAddressAttribute().IsValid(item))
                    {
                        copia += $",{item.Text.ToLower().Trim()}";
                    }
                }
            }
            return (copia.Replace("delete,", "")).Replace("delete", "");
        }
        private string GetEmailCopiaSplit()
        {
            string copia = "delete";
            foreach (var item in pnlCopia.Controls.OfType<Label>().ToList())
            {
                if (item.Name.Contains("_cc"))
                {
                    copia += $",{item.Text.ToLower().Trim()}";
                }
            }
            return (copia.Replace("delete,", "")).Replace("delete", "");
        }
        private string GetDirectorioAjunto()
        {
            string directory = "";
            foreach (var item in pnlAdjunto.Controls.OfType<Label>().ToList())
            {
                if (item.Name.Contains("_att"))
                {
                    directory = Path.GetDirectoryName(item.Tag.ToString().Trim());
                    break;
                }
            }
            return directory;
        }
        private string[] GetArrayArchivoAdjunto()
        {
            string[] path = new string[pnlAdjunto.Controls.Count/* - 1*/];
            int i = -1;
            foreach (var item in pnlAdjunto.Controls.OfType<Label>().ToList())
            {
                i++;
                if (item.Name.Contains("_att"))
                {
                    path[i] = item.Text.ToString().Trim();
                    break;
                }
            }
            return path;
        }
        private List<eSistema> GetCredenciales()
        {
            var empresaParaAsuntoMail = unit.EmailingBoleta.ConsultaVarias_EmailingBoletas<eEmpresaEmail>
               (new pEmailingBoleta() { Opcion = 7 });

            // Obtener Listado de Credenciales para el correo electrónico.
            return empresaParaAsuntoMail.Select((obj) => new eSistema()
            {
                cod_clave = obj.cod_empresa,//"CC000", //Esto viene de la tabla: scfma_empresa
                dsc_clave = obj.dsc_UsuarioEmailBoletas,
                dsc_valor = obj.dsc_ClaveEmailBoletas,
            }).ToList();
        }
        private void CrearEtiquetaAdjunto(string adjuntoSplit)
        {
            string[] array = adjuntoSplit.Split(',');
            int i = pnlAdjunto.Controls.Count;
            foreach (string adjunto in array)
            {
                var fname = Path.GetFileName(adjunto);
                bool continuar = true;
                foreach (Label ex in pnlAdjunto.Controls.OfType<Label>().ToList())
                    if (ex.Text.Equals(fname.Trim())) { continuar = false; break; }

                if (!continuar) continue;
                i++;
                string id = $"_att{i}";
                var file = new Label()
                {
                    Name = id,
                    Text = fname,
                    Tag = adjunto,
                    AutoSize = true,
                    Dock = DockStyle.Left,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Cursor = Cursors.Hand,
                };
                file.DoubleClick += (sender, args) => DeleteAttachment(id);
                pnlAdjunto.Controls.Add(file);
                file.BringToFront();
            }
        }

        private void DeleteToMail(string id)
        {
            pnlPara.Controls.OfType<Label>().ToList().ForEach((l) => { if (l.Name.Equals(id)) { pnlPara.Controls.Remove(l); } });
        }
        private void DeleteToCopy(string id)
        {
            pnlCopia.Controls.OfType<Label>().ToList().ForEach((l) => { if (l.Name.Equals(id)) { pnlCopia.Controls.Remove(l); } });
        }

        private void DeleteAttachment(string id)
        {
            pnlAdjunto.Controls.OfType<Label>().ToList().ForEach((l) => { if (l.Name.Equals(id)) { pnlAdjunto.Controls.Remove(l); } });
        }

        private void frmFormatoEmail_Load(object sender, EventArgs e)
        {

        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            dsc_asunto = txtAsunto.Text;
            dsc_cuerpo = txtCuerpo.Text;
            EnviarMail();
        }

        private void btnPara_Click(object sender, EventArgs e)
        {
            XtraInputBoxArgs boxArgs = new XtraInputBoxArgs();
            boxArgs.Caption = "Correo destinatario";
            boxArgs.Prompt = "Email";
            boxArgs.DefaultButtonIndex = 0;

            TextEdit editor = new TextEdit();
            boxArgs.Editor = editor;

            var result = XtraInputBox.Show(boxArgs);
            if (result != null)
            {
                if (new EmailAddressAttribute().IsValid(result))
                    CrearEtiquetaDestinatario(result.ToString());
            }
        }

        private void btnCopia_Click(object sender, EventArgs e)
        {
            XtraInputBoxArgs boxArgs = new XtraInputBoxArgs();
            boxArgs.Caption = "Correo con copia";
            boxArgs.Prompt = "Email";
            boxArgs.DefaultButtonIndex = 0;

            TextEdit editor = new TextEdit();
            boxArgs.Editor = editor;
            var result = XtraInputBox.Show(boxArgs);
            if (result != null)
            {
                if (new EmailAddressAttribute().IsValid(result))
                    CrearEtiquetaConCopia(result.ToString());
            }
        }

        private void btnAdjunto_Click(object sender, EventArgs e)
        {
            //XtraInputBoxArgs boxArgs = new XtraInputBoxArgs();
            //boxArgs.Caption = "Archivo Adjunto";
            //boxArgs.Prompt = "Email";
            //boxArgs.DefaultButtonIndex = 0;

            //TextEdit editor = new TextEdit();
            ////FileAccess fileAccess
            //boxArgs.Editor = editor;
            //var result = XtraInputBox.Show(boxArgs);
            //if (result != null)
            //{
            //    CrearEtiquetaDestinatario(result.ToString());
            //}
        }
    }
}