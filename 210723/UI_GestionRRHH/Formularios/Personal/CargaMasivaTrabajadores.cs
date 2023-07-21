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
using System.Data.OleDb;
using UI_GestionRRHH.Tools;

namespace UI_GestionRRHH.Formularios.Personal
{
    public partial class CargaMasivaTrabajadores : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly UnitOfWork unit;
        frmListadoTrabajador frmHandler;

        public string cod_empresa = "";
        public string ActualizarListado = "NO";
        public string tipoIngreso = "";
        public string GrupoSeleccionado = "";
        public string ItemSeleccionado = "";

        string strPath = "";
        DataTable oDatosExcel = new DataTable();

        public CargaMasivaTrabajadores()
        {
            InitializeComponent();
            unit = new UnitOfWork();
        }

        internal CargaMasivaTrabajadores(frmListadoTrabajador frm)
        {
            InitializeComponent();
            frmHandler = frm;
            unit = new UnitOfWork();
        }

        private void Inicializar()
        {
            CargarComboExcel();
        }

        public string TipoExtension_Excel(string fileExtension, string strPath)
        {
            string conexion = "";

            switch (fileExtension)
            {
                case ".xls":
                    conexion = $@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={strPath};" + "Extended Properties=\"Excel 8.0; HDR=Yes; IMEX=1\"";
                    break;
                case ".xlsx":
                    conexion = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={strPath};" + "Extended Properties=\"Excel 12.0; HDR=Yes; IMEX=1\"";
                    break;
            }
            return conexion;
        }
        public class Informaciongeneral
        {
            public string cod_trabajador { get; set; }

            public string dsc_nombres { get; set; }
            public string dsc_apellido_paterno { get; set; }
            public string dsc_apellido_materno { get; set; }
            public string cod_tipo_documento { get; set; }
            public string dsc_documento { get; set; }
            public string flg_sexo { get; set; }
            public string fch_nacimiento { get; set; }
            public string dsc_celular { get; set; }
            public string dsc_telefono { get; set; }
            public string cod_estadocivil { get; set; }
            public string dsc_mail_1 { get; set; }
            public string cod_situacion_trabajador_2 { get; set; }
            public string cod_nacionalidad { get; set; }
            public string cod_catergoria_trabajador { get; set; }
            public string cod_nivelacademico { get; set; }
            public string fch_ingreso { get; set; }
            public string cod_tipo_via { get; set; }
            public string dsc_tipo_via { get; set; }
            public string dsc_nro_vivienda { get; set; }
            public string dsc_departamento_dir { get; set; }
            public string dsc_interior { get; set; }
            public string dsc_manzana { get; set; }
            public string cod_lote { get; set; }
            public string dsc_kilometro_dir { get; set; }
            public string dsc_etapa { get; set; }
            public string dsc_block { get; set; }
            public string dsc_tipo_zona { get; set; }
            public string dsc_nombre_zona { get; set; }
            public string cod_departamento { get; set; }
            public string cod_provincia { get; set; }
            public string cod_distrito { get; set; }
            public string cod_tipo_trabajador_salud { get; set; }
            public string dsc_pref_ceco { get; set; }
            public string cod_APF { get; set; }
            public string cod_CUSPP { get; set; }
            public string cod_tipo_comision_pension { get; set; }
            public string cod_tipo_contrato { get; set; }
            public string cod_regimen_laboral { get; set; }
            public string cod_periodicidad_pagos { get; set; }
            public string cod_moneda { get; set; }
            public string imp_sueldo_base { get; set; }
            public string dsc_porcentajequincena { get; set; }
            public string dsc_porcentajecomision { get; set; }
            public string flg_asignacionfamiliar { get; set; }
            public string cod_tipo_pago { get; set; }
            public string cod_banco { get; set; }
            public string cod_tipo_cuenta { get; set; }
            public string dsc_cta_bancaria { get; set; }
            public string dsc_cta_interbancaria { get; set; }
            public string cod_regimenasegsalud { get; set; }
            public string cod_situacion_trabajador_salud { get; set; }
            public string cod_tipo_trabajador { get; set; }
            public string cod_ocupacional { get; set; }
            public string cod_situacion_especial { get; set; }
            public string fch_firma { get; set; }
            public string cod_sctr_salud { get; set; }
            public string cod_sctr_salud_pension { get; set; }
            public string fch_vencimiento { get; set; }
            public string correo_laboral { get; set; }
            public string cod_moneda_CTS { get; set; }
            public string cod_banco_CTS { get; set; }
            public string cod_tipo_cuenta_CTS { get; set; }
            public string dsc_cta_bancaria_CTS { get; set; }
            public string dsc_cta_interbancaria_CTS { get; set; }
            public string codsunat_scrtcentroriesgo { get; set; }
            public string flg_afectoSCTR { get; set; }
            public string flg_AfectoVidaLey { get; set; }
            public string flg_horario_nocturno { get; set; }
            public string flg_horas_extras { get; set; }
            public string cod_sunat_seguroley { get; set; }

        }
        private void CargarComboExcel()
        {
            //try
            //{



                //List<Informaciongeneral> infogeneralist = new List<Informaciongeneral>();

                //var excel = new ReadExcel();

                //var inforGeneral = excel.ListarExcel<Informaciongeneral>(out string ss, 5, 1, 71);

                //gcExcel.DataSource = inforGeneral.ToList();
                //gvExcel.RefreshData();

                //gvExcel.Columns["cod_distrito"].Visible = true;


                // var dddddl = ddd;

                //string ruta = new ReadExcel().ObtenerExcel();
                //if (String.IsNullOrEmpty(ruta)) { return; }
                // var frmExcel = new ExcelSplash(ruta);
                //if (frmExcel.ShowDialog() == DialogResult.OK)
                //{
                //var dt = frmExcel.DtExcel;
                //if (dt != null)
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        infogeneralist.Add(new Informaciongeneral()
                //        {
                //            cod_trabajador = dt.Rows[i][1].ToString(),
                //            cod_tipo_documento = dt.Rows[i][5].ToString(),
                //            dsc_documento = dt.Rows[i][6].ToString(),
                //            dsc_nombres = dt.Rows[i][2].ToString(),
                //            dsc_apellido_paterno = dt.Rows[i][3].ToString(),
                //            dsc_apellido_materno = dt.Rows[i][4].ToString()
                //        });
                //    }
                //    gcExcel.DataSource = infogeneralist.ToList();

                // gvExcel.Columns["flg_sexo"].Visible = false;
                // gvExcel.Columns["fch_nacimiento"].Visible = false;
                // gvExcel.Columns["dsc_celular"].Visible = false;
                // gvExcel.Columns["dsc_telefono"].Visible = false;
                // gvExcel.Columns["cod_estadocivil"].Visible = false;
                // gvExcel.Columns["cod_nacionalidad"].Visible = false;
                // gvExcel.Columns["cod_tipo_via"].Visible = false;
                // gvExcel.Columns["dsc_tipo_via"].Visible = false;
                // gvExcel.Columns["dsc_nro_vivienda"].Visible = false;
                // gvExcel.Columns["dsc_departamento_dir"].Visible = false;
                //// gvExcel.Columns["dsc_interior"].Visible = false;
                // gvExcel.Columns["dsc_manzana"].Visible = false;
                // gvExcel.Columns["cod_lote"].Visible = false;
                // gvExcel.Columns["dsc_kilometro_dir"].Visible = false;
                // gvExcel.Columns["dsc_etapa"].Visible = false;
                // gvExcel.Columns["dsc_block"].Visible = false;
                // gvExcel.Columns["dsc_tipo_zona"].Visible = false;
                // gvExcel.Columns["dsc_nombre_zona"].Visible = false;
                // gvExcel.Columns["cod_departamento"].Visible = false;
                // gvExcel.Columns["cod_provincia"].Visible = false;
                // //gvExcel.Columns["cod_distrito"].Visible = false;



                //        gvExcel.Columns["cod_trabajador"].Caption = "Cod. Trabajador";
                //        gvExcel.Columns["cod_trabajador"].Width = 85;
                //        gvExcel.Columns["cod_tipo_documento"].Caption = "Tipo Documento";
                //        gvExcel.Columns["cod_tipo_documento"].Width = 85;
                //        gvExcel.Columns["dsc_documento"].Caption = "Nro Documento";
                //        gvExcel.Columns["dsc_documento"].Width = 100;
                //        gvExcel.Columns["dsc_nombres"].Caption = "Nombres";
                //        gvExcel.Columns["dsc_nombres"].Width = 230;
                //        gvExcel.Columns["dsc_apellido_paterno"].Caption = "Apellidos Paternos";
                //        gvExcel.Columns["dsc_apellido_paterno"].Width = 230;
                //        gvExcel.Columns["dsc_apellido_materno"].Caption = "Apellidos Maternos";
                //        gvExcel.Columns["dsc_apellido_materno"].Width = 230;

                //        gvExcel.RefreshData();
                //    }
                //}
                // }
            //}
            //////catch (Exception ex)
            //{
            //    //SplashScreenManager.CloseForm();
            //    MessageBox.Show(ex.Message);
            //}
               
        }

        private void CargaMasivaTrabajadores_Load(object sender, EventArgs e)
        {
            //if (tipoImporte == "campanha")
            //{
            //    frmImportarProspecto.ActiveForm.Text = "Importar Campaña";
            //}
            //if (tipoImporte == "prospecto")
            //{
            //    frmImportarProspecto.ActiveForm.Text = "Importar Prospecto";
            //}

            //grcInformacion.AppearanceCaption.ForeColor = Program.Sesion.Colores.Verde;
            Inicializar();
            //HabilitarBotones();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            CargarComboExcel();
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {

        }
    }
}