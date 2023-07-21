using DevExpress.XtraBars;
using DevExpress.XtraTreeList;
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
    public partial class frmListadoVacaciones : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frmListadoVacaciones()
        {
            InitializeComponent();
        }

     
        private void refreshTreeView()
        {

            treeFiltroVacaciones.OptionsView.RootCheckBoxStyle = DevExpress.XtraTreeList.NodeCheckBoxStyle.Radio;
            for (int i = 0; i < treeFiltroVacaciones.Nodes.Count; i++)
            {
                treeFiltroVacaciones.Nodes[i].ChildrenCheckBoxStyle = NodeCheckBoxStyle.Check;
                for (int j = 0; j < treeFiltroVacaciones.Nodes[i].Nodes.Count(); j++)
                {
                    treeFiltroVacaciones.Nodes[i].Nodes[j].ChildrenCheckBoxStyle = NodeCheckBoxStyle.Check;
                }
            }
            treeFiltroVacaciones.UncheckAll();
            treeFiltroVacaciones.Nodes[0].Checked = true;
            treeFiltroVacaciones.Nodes[0].Nodes[0].Checked = true;
            treeFiltroVacaciones.Nodes[0].Nodes[0].Nodes.ToList().ForEach((ch) => ch.Checked = true);
            //treeFiltroVacaciones.Nodes[0].Nodes[0].Checked = true;
            treeFiltroVacaciones.CollapseAll();
            treeFiltroVacaciones.Nodes[0].ExpandAll();
            treeFiltroVacaciones.Refresh();
        }

        private void navBarControl1_Click(object sender, EventArgs e)
        {
            refreshTreeView();
        }

        private void btnRegistroVac_ItemClick(object sender, ItemClickEventArgs e)
        {
            MessageBox.Show("hola0");
        }

       
    }
}