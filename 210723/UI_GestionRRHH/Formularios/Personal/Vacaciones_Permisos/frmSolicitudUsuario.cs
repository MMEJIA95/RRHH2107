using DevExpress.Skins;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors.Repository;
using System.Windows.Forms;


namespace UI_GestionRRHH.Formularios.Personal.Vacaciones_Permisos
{
    public partial class frmSolicitudUsuario : HNG_Tools.SimpleModalForm
    {
        public frmSolicitudUsuario()
        {
            InitializeComponent();
        }

        private void calendarControl1_SelectionChanged(object sender, EventArgs e)
        {
            this.listBoxControl1.BeginUpdate();
            try
            {
                this.listBoxControl1.Items.Clear();
                foreach (DevExpress.XtraEditors.Controls.DateRange range in this.calendarControl1.SelectedRanges)
                {
                    this.listBoxControl1.Items.Add(range.StartDate.ToShortDateString() + " - " + range.EndDate.ToShortDateString());
                }
            }
            finally
            {
                this.listBoxControl1.EndUpdate();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage = navigationPage2;
        }
    }
}