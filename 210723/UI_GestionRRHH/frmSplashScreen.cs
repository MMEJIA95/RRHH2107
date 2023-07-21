using BE_GestionRRHH;
using BL_GestionRRHH;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace UI_GestionRRHH
{
    public partial class frmSplashScreen : DevExpress.XtraEditors.XtraForm
    {
        //private readonly blEncrypta blEncryp;
        //private readonly UnitOfWork unit;
        public frmSplashScreen()
        {
            InitializeComponent();
            //unit = new UnitOfWork();
            // blEncryp = new blEncrypta(Program.Sesion.Acceso.Key);
        }

        //private int[] IntColor(string color)
        //{ return blEncryp.Desencrypta(ConfigurationManager.AppSettings[color]).ToString().Split(',').Select(n => Convert.ToInt32(n)).ToArray(); }

        private void frmSplashScreen_Load(object sender, EventArgs e)
        {
            /*  Cargar valores del entorno. */
            // Asignar_VariablesGlobales();
            /*  Cargar colores en la sesión.*/
            //colorVerde = ConfigurationManager.AppSettings["colorVerde"].Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            //colorPlomo = ConfigurationManager.AppSettings["colorPlomo"].Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            //colorEventRow = ConfigurationManager.AppSettings["colorEventRow"].Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            //colorFocus = ConfigurationManager.AppSettings["colorFocus"].Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            // Program.Sesion.Colores = new eColor(IntColor(blEncryp.Encrypta("colorVerde")), IntColor(blEncryp.Encrypta("colorPlomo")), IntColor(blEncryp.Encrypta("colorEventRow")), IntColor(blEncryp.Encrypta("colorFocus")));
            panel2.BackColor = Program.Sesion.Colores.Verde;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel2.Width += 5;
            if (panel2.Width >= 700)
            {
                timer1.Stop();
                ValidarVersion();
            }
        }

        private void ValidarVersion()
        {
            this.Hide();
            if (Program.Sesion.Global.VersionApp == Program.Sesion.Version)
                new frmPrincipal().ShowDialog();
            else
            {
                frmAlertaVersion frmversion = new frmAlertaVersion();
                frmversion.lblVersion.Text = "Versión " + Program.Sesion.Version;
                frmversion.ShowDialog();
            }
        }
    }
}