using BE_GestionRRHH;
using BL_GestionRRHH;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_GestionRRHH.Formularios.Correspondencia
{
    public partial class frmCorrespondencia_EmailEditMasivo : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;
        string _docTrabajador = string.Empty;

        SimpleButton smpBtn;
        Label sliPrgs;
        bool estaValidado = false;
        public frmCorrespondencia_EmailEditMasivo()
        {
            InitializeComponent();
            unit = new UnitOfWork();

            this.smpBtn = new SimpleButton();
            this.smpBtn.ImageOptions.Image = global::UI_GestionRRHH.Properties.Resources.search_folder_20px;
            this.smpBtn.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.smpBtn.Location = new System.Drawing.Point(399, 294);
            this.smpBtn.Name = "smpBtn";
            this.smpBtn.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.smpBtn.Size = new System.Drawing.Size(36, 36);
            this.smpBtn.Dock = DockStyle.Right;
            this.smpBtn.Cursor = Cursors.Hand;
            this.smpBtn.Click += new EventHandler(ExtraerExcel_Click);
            txtPlantilla.Controls.Add(smpBtn);
            this.smpBtn.BringToFront();

            this.sliPrgs = new Label();
            this.sliPrgs.BackColor = Color.FromArgb(244, 130, 37);
            this.sliPrgs.ForeColor = Color.White;
            this.sliPrgs.Size = new Size(1, 26);
            this.sliPrgs.Location = new Point(0, 0);
            this.sliPrgs.TextAlign = ContentAlignment.TopCenter;
            lblProgress.Controls.Add(sliPrgs);
            this.sliPrgs.SendToBack();

            configurar_formulario();
        }
        private void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;
            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoPlantilla, gvListadoPlantilla);
        }
        class Emails { public string Documento { get; set; } public string Nombres { get; set; } public string Email { get; set; } }
        List<Emails> excelEmails;
        void CargarPlantilla()
        {
            
            //Traer información del Excel.
            estaValidado = false;
            excelEmails = new List<Emails>();
            excelEmails =  new Tools.ReadExcel().ListarExcel<Emails>(out string path);
            if (excelEmails != null && excelEmails.Count > 0)
            {
                unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Importando Excel", "Procesando...");

                txtPlantilla.Text = path;
                //Cargar Listado para DataSource
                var objList = new List<eTrabajadorMatch>();
                excelEmails.ForEach((exc) =>
                {
                    objList.Add(new eTrabajadorMatch()
                    {
                        dsc_documento = exc.Documento,
                        dsc_nombres = exc.Nombres,
                        dsc_mail_2 = exc.Email, // Al extraer, se guarda en columna email_2, porque en email_1 viene la de SQL.
                        flg_resultado = "NO", // Guardar con NO, para luego hacer el match
                        flg_existe="NO"//Guardar con NO, match con registros de SQL
                    });
                });
                bsListadoPlantilla.DataSource = objList;
                SplashScreenManager.CloseForm();
            }
           
        }

        bool ValidateEmail(string email) { return new EmailAddressAttribute().IsValid(email); }
        void ValidarAtributos()
        {
            unit.Globales.Abrir_SplashScreenManager(typeof(Formularios.Shared.FrmSplashCarga), "Comprobando existencia", "Procesando...");
            sliPrgs.Width = 1;
            var objList = bsListadoPlantilla.DataSource as List<eTrabajadorMatch>;
            if (objList != null && objList.Count > 0)
            {
                //Traermos los DNI concatenados, separados por (,) para el Split.
                var docs = new Tools.TreeListHelper().
                    ObtenerValoresConcatenadoDeLista<eTrabajadorMatch>
                    (objList, "dsc_documento");

                var listaDesdeSQL = unit.EmailingBoleta.ConsultaVarias_EmailingBoletas<eTrabajadorMatch>(
                    new pEmailingBoleta() { Opcion = 10, Dsc_documento_trabajadorSplit = docs });
                if (listaDesdeSQL.Count > 0 && listaDesdeSQL != null)
                {
                    objList.ForEach((obj) =>
                    {
                        listaDesdeSQL.ForEach((match) =>
                        {
                            if (obj.dsc_documento.Trim().Equals(match.dsc_documento.Trim()))
                            {
                                obj.dsc_mail_1 = match.dsc_mail_1;
                                obj.flg_existe = "SI";
                            }
                        });
                    });


                    //Validar que el correo esté bien escrito; solo para los que existen en SQL.
                    objList.Where(v => v.flg_existe.Equals("SI"))
                         .ToList().ForEach((mail) =>
                         {
                             var b = ValidateEmail(mail.dsc_mail_2.Trim());//solo validamos:dsc_mail_2, ya que es el nuevo correo.
                             mail.flg_resultado = b ? "SI" : "NO";
                         });
                }
                DibujarBarraDeProgreso();
                bsListadoPlantilla.DataSource = objList;
                gvListadoPlantilla.RefreshData();
            }
            SplashScreenManager.CloseForm();
        }

        //private bool DibujarBarraDeProgreso()
        //{
        //    sliPrgs.Width += 5;
        //    if (sliPrgs.Width > lblProgress.Width)
        //        return true;
        //    return false;
        //}
        private void DibujarBarraDeProgreso()
        {
            sliPrgs.Width += 5;
            while (sliPrgs.Width <= lblProgress.Width)
            {
                sliPrgs.Width += 5;
                sliPrgs.Text = $"{(lblProgress.Width / sliPrgs.Width * 100)}% Procesando.";
            }

            sliPrgs.Text = "Validación completada";
            estaValidado = true;
        }
        void Actualizar()
        {
            var objList = bsListadoPlantilla.DataSource as List<eTrabajadorMatch>;
            if (objList != null && objList.Count > 0)
            {
                var t = new Tools.TreeListHelper();
                var documentoConcatenado = t.ObtenerValoresConcatenadoDeLista<eTrabajadorMatch>(
                    objList.Where(o => o.flg_resultado.Equals("SI")).ToList(), "dsc_documento");
                var emailConcatenado = t.ObtenerValoresConcatenadoDeLista<eTrabajadorMatch>(
                    objList.Where(o => o.flg_resultado.Equals("SI")).ToList(), "dsc_mail_2");// dsc_mail_2 -> es elnuevo correo. 

                var result = unit.EmailingBoleta.ConsultaVarias_EmailingBoletas<eSqlMessage>(
                    new pEmailingBoleta()
                    {
                        Opcion = 11,
                        Dsc_documento_trabajadorSplit = documentoConcatenado,
                        Dsc_email_trabajadorSplit = emailConcatenado,
                    });
                if (result != null)
                {
                    unit.Globales.Mensaje(result[0].IsSuccess, result[0].Outmessage, "Actualización de correos");
                    if (result[0].IsSuccess)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
        }
        void ExtraerExcel_Click(object sender, EventArgs e)
        {
            CargarPlantilla();
        }
        private void frmCorrespondencia_EmailEditMasivo_Load(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (estaValidado)
                Actualizar();
            else
                unit.Globales.Mensaje(blGlobales.TipoMensaje.Alerta, "Antes de proceder con la actualización " +
                    "se debe validar la información cargada.", "Actualizar correos");
        }

        private void txtPlantilla_MouseClick(object sender, MouseEventArgs e)
        {
            CargarPlantilla();
        }

        private void btnValidacion_Click(object sender, EventArgs e)
        {
            ValidarAtributos();
        }

        private void gvListadoPlantilla_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                var obj = gvListadoPlantilla.GetRow(e.RowHandle) as eTrabajadorMatch;
                if (e.Column.FieldName == "flg_resultado")
                {
                    e.Appearance.ForeColor = e.Appearance.BackColor;
                   
                    e.DefaultDraw();
                    if (obj.flg_resultado.Equals("SI")) { e.Cache.DrawImage(Properties.Resources.ok_18px, e.Bounds.X + 15, e.Bounds.Y); }
                    else { e.Cache.DrawImage(Properties.Resources.cancel_18px, e.Bounds.X + 15, e.Bounds.Y); }
                }
                if (e.Column.FieldName == "flg_existe")
                {
                    e.Appearance.ForeColor = e.Appearance.BackColor;
                    e.DefaultDraw();
                    if (obj.flg_existe.Equals("SI")) { e.Cache.DrawImage(Properties.Resources.ok_18px, e.Bounds.X + 15, e.Bounds.Y); }
                    else { e.Cache.DrawImage(Properties.Resources.cancel_18px, e.Bounds.X + 15, e.Bounds.Y); }
                }
            }
        }

        private void btnExportarNoRegistrados_Click(object sender, EventArgs e)
        {
            var objList =bsListadoPlantilla.DataSource as List<eTrabajadorMatch>;
            if(objList!=null && objList.Count > 0)
            {
                HNG.Excel.GenerateExcel_fromList(objList.Where((s) => s.flg_existe.Equals("NO")).ToList(), $"{DateTime.Now.Ticks}_TrabajadoresNoRegistrados");
            }
        }

        private void btnCoreoErrado_Click(object sender, EventArgs e)
        {
            if (!(bsListadoPlantilla.DataSource is List<eTrabajadorMatch> objList)) return;

            HNG.Excel.GenerateExcel_fromList(objList.Where((s) => s.flg_resultado.Equals("NO")).ToList(), $"{DateTime.Now.Ticks}_CorreosErrados");
        }

        private void btnCloseAvance_Click(object sender, EventArgs e)
        {
            pnlAyudaPlantilla.Visible = false;
        }

        private void btnPlantillaImportar_Click(object sender, EventArgs e)
        {
            pnlAyudaPlantilla.Visible = true;
        }
    }
}