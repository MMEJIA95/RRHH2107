using BE_GestionRRHH;
using DevExpress.CodeParser;
using DevExpress.Office.Utils;
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
using static UI_GestionRRHH.Formularios.Personal.Vacaciones_Permisos.frmVacaciones_Listado;

namespace UI_GestionRRHH.Formularios.Personal.Vacaciones_Permisos
{
    public partial class frmVacaciones_Registrar : HNG_Tools.SimpleModalForm
    {
        private readonly UnitOfWork unit;
        frmVacaciones_Listado Handler;
        private string cod_empresa, cod_periodo, cod_trabajador; //, cod_vacaciones, cod_detalle;        
        public frmVacaciones_Registrar(frmVacaciones_Listado frmHandler)
        {
            InitializeComponent();
            Handler = frmHandler;
            this.unit = Handler.unit;
            configurar_formulario();
        }
        private void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;
            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoVacacionesSolicitadas, gvListadoVacacionesSolicitadas);
            gvListadoVacacionesSolicitadas.OptionsView.ShowAutoFilterRow = false;

            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoVacacionesAsignadas, gvListadoVacacionesAsignadas);
            gvListadoVacacionesAsignadas.OptionsView.ShowAutoFilterRow = false;

            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoVacacionesAcumuladas, gvListadoVacacionesAcumuladas);
            gvListadoVacacionesAcumuladas.OptionsView.ShowAutoFilterRow = false;
        }
        private void CargarLookUp(string modulo = "")
        {
            List<eItems> registro = new List<eItems>();
            List<eItems> estado = new List<eItems>();
            switch (modulo)
            {
                case "control":
                    {
                        registro.Add(new eItems() { Key = "GOCE", Value = "Vacaciones Gozadas" });
                        registro.Add(new eItems() { Key = "VENT", Value = "Vacaciones Vendidas" });
                        lkpRegistro.Properties.DropDownRows = 2;

                        estado.Add(new eItems() { Key = "GOZ", Value = "Gozadas" });
                        estado.Add(new eItems() { Key = "PRG", Value = "Programada" });
                        lkpEstado.Properties.DropDownRows = 2;
                        break;
                    }
                case "solicitud":
                    {
                        registro.Add(new eItems() { Key = "GOCE", Value = "Vacaciones Gozadas" });
                        lkpRegistro.Properties.DropDownRows = 1;

                        estado.Add(new eItems() { Key = "PEN", Value = "Pendiente" });
                        lkpEstado.Properties.DropDownRows = 1;
                        break;
                    }
                case "revision":
                    {
                        registro.Add(new eItems() { Key = "GOCE", Value = "Vacaciones Gozadas" });
                        lkpRegistro.Properties.DropDownRows = 1;

                        estado.Add(new eItems() { Key = "PRG", Value = "Programada" });
                        lkpEstado.Properties.DropDownRows = 1;
                        break;
                    }
            }

            lkpRegistro.Properties.DataSource = registro.ToList();
            lkpRegistro.Properties.DisplayMember = "Value";
            lkpRegistro.Properties.ValueMember = "Key";
            lkpRegistro.EditValue = "GOCE";

            lkpEstado.Properties.DataSource = estado.ToList();
            lkpEstado.Properties.DisplayMember = "Value";
            lkpEstado.Properties.ValueMember = "Key";
            lkpEstado.EditValue = modulo == "control" ? "PRG" : modulo == "solicitud" ? "PEN" : "PRG";
        }
        internal void CargarDatos(string cod_empresa, string cod_periodo, string cod_trabajador, string modulo = "", string nombre = "")//, string cod_vacaciones, string cod_detalle)
        {
            this.cod_empresa = cod_empresa;
            this.cod_periodo = cod_periodo;
            this.cod_trabajador = cod_trabajador;
            this.lblTitulo.Text = nombre;

            // this.cod_vacaciones  = cod_vacaciones; // this.cod_detalle     = cod_detalle;
            CargarListados();
            CargarLookUp(modulo);
        }

        private void CargarListados()
        {

            var objAcumulados = unit.Vacaciones.ConsultaVarias_Vacaciones<eVacaciones_Acumuladas>(
                new pVacaciones()
                {
                    Opcion = 4,
                    Cod_empresa_multiple = cod_empresa,
                    //Cod_periodo = cod_periodo,
                    Cod_trabajador = cod_trabajador,
                });
            if (objAcumulados != null && objAcumulados.Count > 0)
            {
                bsListadoVacacionesAcumuladas.DataSource = objAcumulados;
                gvListadoVacacionesAcumuladas.RefreshData();
            }
        }

        private void MostrarVacacionesSolicitadas()
        {
            var objList = unit.Vacaciones.ConsultaVarias_Vacaciones<eVacaciones_Solicitadas>(
               new pVacaciones()
               {
                   Opcion = 6,
                   Cod_periodo = cod_periodo,
                   Cod_empresa_multiple = cod_empresa,
                   Cod_trabajador = cod_trabajador,
               });
            bsListadoVacacionesSolicitadas.DataSource = null;
            if (objList != null && objList.Count > 0)
            {
                bsListadoVacacionesSolicitadas.DataSource = objList.ToList();
                gvListadoVacacionesSolicitadas.RefreshData();
            }
        }

        private void ccnCalendario_MouseUp(object sender, MouseEventArgs e)
        {
            ObtenerVacacionesSolicitadasPorRangoFechas(start, end);
        }

        private void CalcularDiasDeGoce(DateTime inicio, DateTime fin)
        {
            var days = (fin - inicio).Days;
            txtCantidad.Text = days.ToString();
        }


        private void frmVacaciones_Solicitud_Load(object sender, EventArgs e)
        {
            gvListadoVacacionesAcumuladas.SelectRow(0);

        }
        DateTime start = new DateTime(), end = new DateTime();

        private void lkpEstado_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void gvListadoVacacionesAcumuladas_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (!(gvListadoVacacionesAcumuladas.GetFocusedRow() is eVacaciones_Acumuladas obj)) return;

            //Prop.Cod_trabajador = obj.cod_trabajador;
            //Prop.Cod_vacacion = obj.cod_vacacionGeneral;
            cod_periodo = obj.cod_periodo;
            //Prop.Cod_empresa = obj.cod_empresa;

            MostrarVacacionesSolicitadas();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarSeleccionPeriodo()) return;
            GuardarCambios();
        }

        private bool ValidarSeleccionPeriodo()
        {
            if (!(gvListadoVacacionesAcumuladas.GetFocusedRow() is eVacaciones_Acumuladas obj)) return false;
            var objList = (bsListadoVacacionesAcumuladas.DataSource as List<eVacaciones_Acumuladas>)
                .Where(p => p.num_dias_pendientes > 0)
                .OrderBy(o => o.cod_periodo).Take(1).ToList();

            //Se debe seleccionar el periodo más antigüo que tenga num días pendientes para vacaciones.
            if (objList == null) { OutMessage("No se ha seleccionado el Periodo."); return false; }
            if (objList[0].cod_periodo != obj.cod_periodo) { OutMessage("Se ha encontrado días pendientes en el Periodo anterior."); return false; }

            //verificar que no existan solicitudes pendientes de aprobación.
            var objSolicitud = (bsListadoVacacionesSolicitadas.DataSource as List<eVacaciones_Solicitadas>)
                .Where(p => p.flg_aprobado.Equals("PEN")).ToList();
            if (objSolicitud.Count() > 0) { OutMessage("Hay solicitudes pendientes sin aprobar."); return false; }

            //validar si la cantidad solicitada está dentro del periodo seleccionado.
            if (objList[0].num_dias_pendientes < decimal.Parse(txtCantidad.Text)) { OutMessage("Los días solicitados no debe ser mayor a la cantidad de días pendientes."); return false; }

            return true;
        }
        void OutMessage(string message, bool value = true)
        {
            HNG.MessageWarning(message, "Registro de Vacaciones");
        }

        private void GuardarCambios()
        {
            eVacacionesDetalle obj = ObtenerValores();
            if (obj == null) return;

            eSqlMessage result = unit.Vacaciones.InsertarActualizar_VacacionesDetalle<eSqlMessage>(obj);
            if (result.IsSuccess)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                HNG.MessageWarning(result.Outmessage, "Registro de Vacaciones");
            }
        }
        private eVacacionesDetalle ObtenerValores()
        {
            var id_cabecera = unit.Vacaciones.ConsultaVarias_Vacaciones<eVacaciones>(
                new pVacaciones()
                {
                    Opcion = 5,
                    Cod_empresa_multiple = cod_empresa,
                    Cod_trabajador = cod_trabajador,
                    Cod_periodo = cod_periodo
                });
            if (id_cabecera == null) return null;

            return new eVacacionesDetalle()
            {
                cod_vacacionDetalle = "",//this.cod_vacacionDetalle,
                cod_empresa = this.cod_empresa,
                cod_vacacionGeneral = id_cabecera[0].cod_vacacionGeneral,
                cod_tipo_vacacion = lkpRegistro.EditValue.ToString(),
                fch_solicitud = DateTime.Now,// analizar si se va  a colocar un DatePicker
                flg_aprobado = lkpEstado.EditValue.ToString(),
                fch_inicio_vacacion = start,
                fch_fin_vacacion = end,
                num_dias_goce = int.Parse(txtCantidad.Text),
                dsc_observacion = txtObservaciones.Text,
                cod_usuario_cambio = Program.Sesion.Usuario.cod_usuario,
            };
        }

        private void ccnCalendario_SelectionChanged(object sender, EventArgs e)
        {
            this.lbcRangoFechas.BeginUpdate();
            try
            {
                this.lbcRangoFechas.Items.Clear();
                foreach (DevExpress.XtraEditors.Controls.DateRange range in this.ccnCalendario.SelectedRanges)
                {
                    this.lbcRangoFechas.Items.Add(range.StartDate.ToShortDateString() + " - " + range.EndDate.ToShortDateString());
                    start = range.StartDate;
                    end = range.EndDate;
                }
                CalcularDiasDeGoce(start, end);
            }
            finally
            {
                this.lbcRangoFechas.EndUpdate();
            }
        }

        private void ObtenerVacacionesSolicitadasPorRangoFechas(DateTime desde, DateTime hasta)
        {
            var objList = unit.Vacaciones.ConsultaVarias_Vacaciones<eVacaciones_Solicitadas>(
                new pVacaciones()
                {
                    Opcion = 6,
                    Fch_rango_inicial = desde.ToString("yyyyMMdd"),
                    Fch_rango_final = hasta.ToString("yyyyMMdd"),
                });
            bsListadoVacacionesAsignadas.DataSource = null;
            if (objList != null && objList.Count() > 0)
            {
                bsListadoVacacionesAsignadas.DataSource = objList.ToList();
                gvListadoVacacionesAsignadas.RefreshData();
            }
        }
    }
}