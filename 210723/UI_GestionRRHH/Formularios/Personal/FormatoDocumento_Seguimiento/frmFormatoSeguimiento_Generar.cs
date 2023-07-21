using BE_GestionRRHH;
using BE_GestionRRHH.FormatoMD;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_GestionRRHH.Formularios.Documento;

namespace UI_GestionRRHH.Formularios.Personal.FormatoDocumento_Seguimiento
{
    public enum TipoSeguimientoGenerar { General, Asignado }
    public partial class frmFormatoSeguimiento_Generar : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;
        private string codEmpresa;
        private string codTrabajadorMultiple;
        TipoSeguimientoGenerar _tipo;
        public frmFormatoSeguimiento_Generar(TipoSeguimientoGenerar tipo)
        {
            InitializeComponent();
            unit = new UnitOfWork();
            configurar_formulario();
            _tipo = tipo;
            codEmpresa = string.Empty;
            codTrabajadorMultiple = string.Empty;
        }
        private void configurar_formulario()
        {
            unit.Globales.ConfigurarGridView_TreeStyle(gcListadoFormatos, gvListadoFormatos);
            unit.Globales.ConfigurarGridView_TreeStyle(gcListadoFormatoAsignado, gvListadoFormatoAsignado);
            gvListadoFormatoAsignado.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            gvListadoFormatoAsignado.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;

            gvListadoFormatos.OptionsView.EnableAppearanceEvenRow = false;
            gvListadoFormatoAsignado.OptionsView.EnableAppearanceEvenRow = false;
            this.TitleBackColor = Program.Sesion.Colores.Verde;

        }
        internal void CargarFormatoDocumentos(string cod_empresa, string cod_trabajador_multiple)
        {
            codEmpresa = cod_empresa;
            codTrabajadorMultiple = cod_trabajador_multiple;

            switch (_tipo)
            {
                case TipoSeguimientoGenerar.General:
                    {
                        layout_gridFormato.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        var objFormato = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eFormatoMD_Vinculo_Filtro>(
                            new pQFormatoMD() { Opcion = 9, Cod_empresaSplit = cod_empresa });

                        bsListadoFormatos.DataSource = null;
                        if (objFormato != null && objFormato.Count > 0)
                        {
                            var newList = objFormato
                               .Where(w => w.flg_estado.Equals("SI") && w.flg_publicado.Equals("SI"))
                               .OrderBy(o => o.num_jerarquia).ToList();
                            bsListadoFormatos.DataSource = newList;
                            gvListadoFormatos.RefreshData();
                            gvListadoFormatos.ExpandAllGroups();
                        }
                        break;
                    }
                case TipoSeguimientoGenerar.Asignado:
                    {
                        layout_gridAsignado.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        var objFormato = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eTrabajador_InfoFormatoMD_Vista>(
                            new pQFormatoMD() { Opcion = 18, Cod_empresaSplit = cod_empresa, Cod_trabajadorSplit = codTrabajadorMultiple });

                        bsListadoFormatoAsignado.DataSource = null;
                        if (objFormato != null && objFormato.Count > 0)
                        {
                            var newList = objFormato.Where((f) => f.flg_publicado.Equals("SI")).ToList(); //  ver si  le sumamos la gerarquía.
                            bsListadoFormatoAsignado.DataSource = newList;
                            gvListadoFormatoAsignado.RefreshData();
                            gvListadoFormatoAsignado.ExpandAllGroups();
                        }
                        break;
                    }
            }

        }
        private void AsignarFormatoDocumentos()
        {
            //Preparar Impresora //Numero de copias a imprimir.
            string numCopy = "0";
            if (chkImprimirFormato.Checked) { numCopy = PrepararImpresora(); }

            //Opcional: insertar multiples:
            var objData = new eFormatoMD_Seguimiento();
            objData.cod_formatoMD_seguimiento = "new";
            objData.fch_inicio_firma = detInicio.DateTime;
            objData.fch_vence_firma = detFinal.DateTime;
            objData.dsc_observacion = txtObservacion.Text;
            objData.cod_usuario_cambio = Program.Sesion.Usuario.cod_usuario;

            eSqlMessage result = new eSqlMessage();
            foreach (string tra in codTrabajadorMultiple.Split(','))
            {
                objData.cod_trabajador = tra;
                objData.cod_empresa = codEmpresa;

                //string formato_duplicado = "";
                var formList = new List<eFormatoMD_Vinculo_Filtro>();
                foreach (var rowFr in gvListadoFormatos.GetSelectedRows())
                {
                    var obj = gvListadoFormatos.GetRow(rowFr) as eFormatoMD_Vinculo_Filtro;
                    //if (obj.cod_formatoMD_vinculo.Equals(formato_duplicado))
                    //{
                    //    formato_duplicado = "";
                    //    continue;
                    //}
                    //formato_duplicado = obj.cod_formatoMD_vinculo;
                    formList.Add(obj);
                    //objData.cod_formatoMD_vinculo = obj.cod_formatoMD_vinculo;
                    //result = unit.FormatoMDocumento.InsertarActualizar_FormatoMDSeguimiento<eSqlMessage>(1, objData);
                }

                var formDistinct = formList.Distinct().ToList();
                foreach (var rf in formDistinct)
                {
                    objData.cod_formatoMD_vinculo = rf.cod_formatoMD_vinculo;
                    result = unit.FormatoMDocumento.InsertarActualizar_FormatoMDSeguimiento<eSqlMessage>(1, objData);
                    Imprimir(rf.dsc_wordMLText, numCopy);
                }
            }

            if (result.IsSuccess)
            {
                //Se ha registrado;
                unit.Globales.Mensaje(true, result.Outmessage, "Emisión de Formatos");
            }
        }
        private string PrepararImpresora()
        {
            var frm = new frmImpresora();
            frm.CargarImpresoras();

            frm.ShowDialog();
            if (frm.Result == DialogResult.OK)
                return frm.txtCopia.Text;
            return "0";
        }
        private void Imprimir(string wordPlantilla, string numCopy)
        {
            if (numCopy.Equals("0")) return;
            new FormatoMDHelper().ImpresionSimple(getWordParamsValues(wordPlantilla), numCopy);
        }

        private string getWordParamsValues(string wordPlantilla)
        {
            var lsParams = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eFormatoMD_Parametro>(
                  new pQFormatoMD() { Opcion = 6, });


            var format = new FormatoMDHelper();
            return format.ObtenerWord_ConParametrosDescritos(codEmpresa, codTrabajadorMultiple, wordPlantilla, lsParams);
        }

        private void VistaPreliminar()
        {
            string strDscFormato = "", strWordTemplate = "";
            bool stop = true;
            switch (_tipo)
            {
                case TipoSeguimientoGenerar.General:
                    {
                        var obj = gvListadoFormatos.GetFocusedRow() as eFormatoMD_Vinculo_Filtro;
                        if (obj == null) return;
                        stop = false;

                        strDscFormato = obj.dsc_formatoMD_vinculo;
                        strWordTemplate = obj.dsc_wordMLText;
                        break;
                    }
                case TipoSeguimientoGenerar.Asignado:
                    {
                        var obj = gvListadoFormatoAsignado.GetFocusedRow() as eTrabajador_InfoFormatoMD_Vista;
                        if (obj == null) return;
                        stop = false;

                        strDscFormato = obj.dsc_formatoMD_vinculo;
                        strWordTemplate = obj.dsc_wordMLText;
                        break;
                    }
            }
            if (stop) return;

            var objTrabajador = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eTrabajador_InfoFormatoMD_Vista>(
                new pQFormatoMD() { Opcion = 18, Cod_trabajadorSplit = codTrabajadorMultiple, Cod_empresaSplit = codEmpresa });

            //var docList = unit.FormatoMDocumento.ConsultaVarias_FormatoMDocumento<eFormatoMD_Vinculo>(
            //    new pQFormatoMD() { Opcion = 8, Cod_empresaSplit = codEmpresa, Cod_formatoMD_vinculoSplit = obj.cod_formatoMD_vinculo });

            //if (docList == null) return;
            if (objTrabajador.Count == 0 || objTrabajador == null) return;

            var frm = new frmFormato_VistaPreliminar
            {
                Text = $"Vista Preliminar : {strDscFormato}",
                //WindowState = FormWindowState.Maximized
            };
            // var formato = docList.FirstOrDefault((f)=>f.cod_formatoMD_vinculo.Equals(obj.cod_formatoMD_vinculo));
            frm.CargarData(objTrabajador.FirstOrDefault(), strWordTemplate);
            frm.ShowDialog();
        }
        private void frmFormatoSeguimiento_Generar_Load(object sender, EventArgs e)
        {
            detInicio.DateTime = DateTime.Now;
            detFinal.DateTime = DateTime.Now;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            AsignarFormatoDocumentos();
        }
        private void btnOpcional_Click(object sender, EventArgs e)
        {
            VistaPreliminar();
        }
    }
}