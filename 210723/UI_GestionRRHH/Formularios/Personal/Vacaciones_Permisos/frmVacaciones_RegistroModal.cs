using BE_GestionRRHH;
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

namespace UI_GestionRRHH.Formularios.Personal.Vacaciones_Permisos
{
    public partial class frmVacaciones_RegistroModal : HNG_Tools.ModalForm
    {
        private readonly UnitOfWork unit;
        private string cod_trabajador;
        private string cod_vacaionGeneral;
        private string cod_vacacionDetalle;
        public frmVacaciones_RegistroModal()
        {
            InitializeComponent();
            unit = new UnitOfWork();
            cod_trabajador = string.Empty;
            cod_vacaionGeneral = string.Empty;
            cod_vacacionDetalle = string.Empty;
            configurar_formulario();
            InicializarDatos();
        }

        private void configurar_formulario()
        {
            this.TitleBackColor = Program.Sesion.Colores.Verde;
            unit.Globales.ConfigurarGridView_ClasicStyle(gcListadoAcumulado, gvListadoAcumulado);

        }
        private void InicializarDatos()
        {
            detFechaSolicitud.EditValue = DateTime.Now;
            detFechaInicioVacaciones.EditValue = DateTime.Now;
            detFechaFinalVacaciones.EditValue = DateTime.Now;
        }

        

        private void CalcularDiasDeGoce(int numero)
        {
            var newDate = detFechaInicioVacaciones.DateTime.AddDays(numero);
            detFechaFinalVacaciones.EditValue = newDate;
            //MessageBox.Show(newDate.ToString());
        }
        private bool Validar()
        {
            if (txtNumeroDias.Text.Length == 0) txtNumeroDias.Text = "1";

            if (decimal.Parse(txtNumeroDias.Text) > decimal.Parse(txtNumerosPendientes.Text))
            {
                unit.Globales.Mensaje(false, "El número de días solicitadas no debe ser mayor a la cantidad de días disponibles", "Guardar Solicitud de Vacaciones");
                txtNumeroDias.Focus();
                return false;
            }
            if (detFechaInicioVacaciones.DateTime < DateTime.Now)
            {
                unit.Globales.Mensaje(false, "La fecha de inicio de vacaciones no debe ser una fecha anterior", "Guardar Solicitud de Vacaciones");
                detFechaInicioVacaciones.Focus();
                return false;
            }
            if (detFechaFinalVacaciones.DateTime < detFechaInicioVacaciones.DateTime)
            {
                unit.Globales.Mensaje(false, "La fecha final de vacaciones no debe ser menor a la fecha inicial", "Guardar Solicitud de Vacaciones");
                detFechaFinalVacaciones.Focus();
                return false;
            }

            return true;
        }
        private void Guardar()
        {
            //if (Validar())
            //{
            //    var result = unit.Vacaciones.InsertarActualizar_VacacionesDetalle<eSqlMessage>(ObtenerValores());
            //    if (result.IsSuccess)
            //    {
            //        this.DialogResult = DialogResult.OK;
            //        this.Close();
            //    }
            //    else
            //    {
            //        unit.Globales.Mensaje(false, result.Outmessage, "Registro de Vacaciones");
            //    }
            //}
        }

        private eVacacionesGosadas ObtenerValores()
        {
            return new eVacacionesGosadas()
            {
                cod_vacacionDetalle = this.cod_vacacionDetalle,
                cod_vacacionGeneral = this.cod_vacaionGeneral,
                fch_solicitud = detFechaSolicitud.DateTime,
                fch_inicio_vacacion = detFechaInicioVacaciones.DateTime,
                fch_fin_vacacion = detFechaFinalVacaciones.DateTime,
                num_dias_goce = int.Parse(txtNumeroDias.Text),
                dsc_observacion = txtObservacion.Text,
                cod_usuario_cambio = Program.Sesion.Usuario.cod_trabajador,
            };
        }
        private void gvListadoAcumulado_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0) unit.Globales.Pintar_EstiloGrilla(sender, e);
        }

        private void gvListadoAcumulado_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            unit.Globales.Pintar_CabeceraColumnas(e);
        }

        private void txtNumeroDias_MouseLeave(object sender, EventArgs e)
        {
            if (txtNumeroDias.Text.Length == 0) txtNumeroDias.Text = "1";
        }

        private void txtNumeroDias_KeyPress(object sender, KeyPressEventArgs e)
        {
            unit.Globales.keyPressOnlyNumber(e);
        }

        private void txtNumeroDias_EditValueChanged(object sender, EventArgs e)
        {
            int.TryParse(txtNumeroDias.Text, out int x);
            CalcularDiasDeGoce(x);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
        }
    }
}