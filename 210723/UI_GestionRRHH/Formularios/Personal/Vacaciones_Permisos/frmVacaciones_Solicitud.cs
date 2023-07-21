using BE_GestionRRHH;
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

namespace UI_GestionRRHH.Formularios.Personal.Vacaciones_Permisos
{
    public partial class frmVacaciones_Solicitud : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frmVacaciones_Solicitud()
        {
            InitializeComponent();
        }

        private void btnNuevo_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (!(gvListadoVacaciones.GetFocusedRow() is eVacaciones_Resumen obj)) return;

            //var frm = new frmVacaciones_Registrar(this);
            //frm.Text = "Nueva Solicitud de Vacaciones";
            //frm.CargarDatos(cod_empresa: Prop.Cod_empresa, cod_periodo: Prop.Periodo, cod_trabajador: Prop.Cod_trabajador, "control", string.Concat(" ", obj.dsc_apellidos_nombres)); //, cod_vacaciones: Prop.Cod_vacacion, cod_detalle: ""
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    btnBuscador_Click(btnBuscador, new EventArgs());
            //}
        }
    }
}