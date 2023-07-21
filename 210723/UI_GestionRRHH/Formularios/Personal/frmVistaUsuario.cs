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
using BE_GestionRRHH;
using BL_GestionRRHH;

namespace UI_GestionRRHH.Formularios.Personal
{
    public partial class frmVistaUsuario : DevExpress.XtraEditors.XtraForm
    {
        private readonly UnitOfWork unit;
        public eTrabajador eTrab = new eTrabajador();
        public string cod_empresa = "00001", dsc_documento = "";
        public string ActualizarListado = "NO";
        public frmVistaUsuario()
        {
            InitializeComponent();
            unit = new UnitOfWork();
        }



        private void simpleButton6_Click(object sender, EventArgs e)
        {
            frmVista vista = new frmVista();
            vista.MiAccion = Trabajador_vista.Nuevo;
            vista.cod_empresa = cod_empresa;
            vista.sede_empresa = lkpSedeEmpresa.EditValue.ToString();

            if (txtdocumento_num == null)
            {
                MessageBox.Show("Debe ingresar el número de documento.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return;

            }
            else
            {
                
                vista.ShowDialog();
            }



        }

        private void CargarLookUpEdit()
        {
            try
            {
                unit.Trabajador.CargaCombosLookUp("TipoDocumentoVista", lkp_Tipo_documento, "cod_tipo_documento", "dsc_tipo_documento", "", valorDefecto: true);
                // unit.Trabajador.CargaCombosLookUp("Empresa", lkpEmpresa, "cod_empresa", "dsc_empresa", "", valorDefecto: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            frmVista vista = new frmVista();
            vista.MiAccion = Trabajador_vista.Nuevo;

            vista.cod_empresa = cod_empresa;
            vista.sede_empresa = lkpSedeEmpresa.EditValue.ToString();

            if (txtdocumento_num.Text == "")
            {
                MessageBox.Show("DEBE INGRESAR UN NÚMERO DE DOCUMENTO");

            }
            else
            {
                eTrab = unit.Trabajador.Obtener_cod_trabajador<eTrabajador>(79, "", txtdocumento_num.Text.ToString());

                if (eTrab != null)

                {
                    dsc_documento = eTrab.cod_trabajador;
                    MessageBox.Show("EL TRABAJADOR YA SE ENCUENTRA REGISTRADO, CODIGO DE TRABAJADOR: " + dsc_documento, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                    ActualizarListado = "SI";

                }
                else
                {
                    string numero = "";
                    numero = txtdocumento_num.Text;
                    vista.txtNroDocumento.Text = numero;
                    vista.Show();
                    ActualizarListado = "SI";
                    //resultado.dsc_nombres;
                }
            }

            

        }

        private void lkpEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            
        }


        private void frmVistaUsuario_Load(object sender, EventArgs e)
        {
            CargarLookUpEdit();
            lkp_Tipo_documento.EditValue = "DI001";

            if (cod_empresa != null)
            {
                unit.Trabajador.CargaCombosLookUp("SedesEmpresa", lkpSedeEmpresa, "cod_sede_empresa", "dsc_sede_empresa", "", valorDefecto: true, cod_empresa: cod_empresa);
                lkpSedeEmpresa.EditValue = "00001";
            }

        }
        private void CargarCombosGridLookup(string nCombo, GridLookUpEdit combo, string campoValueMember, string campoDispleyMember, string campoSelectedValue = "", string cod_condicion = "", bool valorDefecto = false)
        {
            DataTable tabla = new DataTable();
            tabla = unit.Trabajador.ObtenerListadoGridLookup(nCombo, cod_condicion);

            combo.Properties.DataSource = tabla;
            combo.Properties.ValueMember = campoValueMember;
            combo.Properties.DisplayMember = campoDispleyMember;
            if (campoSelectedValue == "") { combo.EditValue = null; } else { combo.EditValue = campoSelectedValue; }
            if (tabla.Columns["flg_default"] != null) if (valorDefecto) combo.EditValue = tabla.Select("flg_default = 'SI'").Length == 0 ? null : (tabla.Select("flg_default = 'SI'"))[0].ItemArray[0];
        }


        public void validardni()
        {
            eTrabajador obj = new eTrabajador();
            obj.cod_trabajador = txtdocumento_num.Text;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
           
           
        }

        private void txtdocumento_num_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
           (e.KeyChar != '.'))
            {
                e.Handled = true;
            }


            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void lkp_Tipo_documento_EditValueChanged(object sender, EventArgs e)
        {
            if (lkp_Tipo_documento.EditValue != null)
            {
                eProveedor obj = new eProveedor();
                obj = unit.Proveedores.Validar_NroDocumento<eProveedor>(19, "", lkp_Tipo_documento.EditValue.ToString());
                txtdocumento_num.Properties.MaxLength = obj.ctd_digitos;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
            }
            if (keyData == Keys.Enter)
            {
                frmVista vista = new frmVista();
                vista.MiAccion = Trabajador_vista.Nuevo;

                vista.cod_empresa = cod_empresa;
                vista.sede_empresa = lkpSedeEmpresa.EditValue.ToString();

                if (txtdocumento_num.Text == "")
                {
                    MessageBox.Show("DEBE INGRESAR UN NÚMERO DE DOCUMENTO");

                }
                else
                {
                    eTrab = unit.Trabajador.Obtener_cod_trabajador<eTrabajador>(79, "", txtdocumento_num.Text.ToString());

                    if (eTrab != null)

                    {
                        dsc_documento = eTrab.cod_trabajador;
              
                        MessageBox.Show("EL TRABAJADOR YA SE ENCUENTRA REGISTRADO, CODIGO DE TRABAJADOR: " + dsc_documento, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); 

                    }
                    else
                    {
                        string numero = "";
                        numero = txtdocumento_num.Text;
                        vista.dsc_documento = numero;
                        vista.Show();
                        if (vista.ActualizarListado == "SI") ;
                        //resultado.dsc_nombres;
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}