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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_GestionRRHH.Formularios.Correspondencia
{
    public partial class frmEmailing : HNG_Tools.SimpleModalForm
    {
        private frmCorrespondencia_BoletaPago frmHandler;
        private List<eSistema> _credencialesList;
        private List<eSistema> _credencialesGrupoHNG;
        private eEmailFormato _formatoEmail;
        private eTrabajador_EmpEmail_Vista _trabajadorEmail;
        internal bool CancelarProceso;
        private bool Enviado;
        public frmEmailing(frmCorrespondencia_BoletaPago frm)
        {
            InitializeComponent();
            frmHandler = frm;
            CancelarProceso = false;
            Enviado = false;
        }
        internal void CargarDatos(eTrabajador_EmpEmail_Vista trabajadorEmail, List<eSistema> credencialesList, 
            List<eSistema> credencialesGrupoHNG, eEmailFormato formatoEmail)
        {
            _trabajadorEmail = trabajadorEmail; 
            _credencialesList = credencialesList;
            _credencialesGrupoHNG = credencialesGrupoHNG;
            _formatoEmail = formatoEmail;

            lblEmail.Text = $"Enviando a : {trabajadorEmail.dsc_email.ToLower()}";
            lblAdjunto.Text = $"Adjunto : {Path.GetFileName(trabajadorEmail.dsc_pathfile)}";

           
            //Enviar();
        }


        private void Enviar()
        {
            Enviado = false;           
            if (ValidateEmail(_trabajadorEmail.dsc_email.Trim()))
            {
                string newCuerpo = _formatoEmail.dsc_cuerpo.Replace("{nombre}", _trabajadorEmail.dsc_trabajador);
                newCuerpo = newCuerpo.Replace("{periodo}", frmHandler.obtenerPeriodo(_trabajadorEmail.dsc_pathfile));
                string newAsunto = _formatoEmail.dsc_asunto.Replace("{empresa}", _trabajadorEmail.dsc_empresa);

                //Obtener Credencial asignado a cada empresa
                var credencial = _credencialesList.Where(c => c.cod_clave == _trabajadorEmail.cod_empresa).ToList();
                //var __credencial = (credencial != null && credencial.Count > 0) ?
                //(credencial[0].dsc_clave != null && credencial[0].dsc_valor != null) ? credencial
                //: credencialGrupoHNG : credencialGrupoHNG; // Si la empresa no tiene credenciales, toma el credencial del grupo HNG
                //                                           //El resultado sirve para indicar si se ha enviado o no el correo a un destinatario.
                //                                           //OjO, enviado: quiere decir que si salió el correo. aún no se valida si el correo ha llegado. 
                Enviado = frmHandler.unit.Globales.EnviarCorreoElectronico_SMTP(
                   _trabajadorEmail.dsc_email,
                   newAsunto,
                   newCuerpo,
                   credencial,//credencialGrupoHNG,//__credencial,
                   Path.GetDirectoryName(_trabajadorEmail.dsc_pathfile),//Ruta
                   _trabajadorEmail.dsc_pathfile//archivo a enviar.
               );
                //MessageBox.Show(__credencial[0].dsc_valor.ToString());
                // arreglar   probar el doWorker
            }

            this.DialogResult = Enviado ? this.DialogResult = DialogResult.OK: this.DialogResult = DialogResult.Cancel;
           
        }

        bool ValidateEmail(string email) { return new EmailAddressAttribute().IsValid(email); }

        private void frmEmailing_Load(object sender, EventArgs e)
        {
      
            //MessageBox.Show("Locad");
        }

        private void frmEmailing_Validated(object sender, EventArgs e)
        {
          
        }

        private void frmEmailing_Shown(object sender, EventArgs e)
        {
            //MessageBox.Show("shown");
            StartProcess();
            
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Enviar();
            if (worker.CancellationPending == true)
            {
                e.Cancel = true;
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true) { }
            else if (e.Error != null) { /*new Tools.Message().MessageBox(e.Error.Message);*/ }
            else
            {
                this.Tag = "Ok";
                CancelProcess();
                //this.Close();
                //MessageBox.Show("Completado");
                this.Close();
            }
        }
        private void StartProcess() { if (worker.IsBusy != true) { worker.RunWorkerAsync(); } }
        private void CancelProcess() { worker.CancelAsync(); }

        private void btnCancelarProceso_Click(object sender, EventArgs e)
        {
            var x = frmHandler.unit.Globales.Mensaje(BL_GestionRRHH.blGlobales.TipoMensaje.YesNo, "Esta acción cancelará los envíos programados.\n¿Desea continuar?", "Cancelar Proceso");
            if(x== DialogResult.Yes)
            {
                this.DialogResult = Enviado ? this.DialogResult = DialogResult.OK : this.DialogResult = DialogResult.Cancel;
                this.CancelarProceso = true;
                this.Close();
            }
        }
    }
}