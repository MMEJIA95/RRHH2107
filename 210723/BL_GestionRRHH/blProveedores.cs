using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA_GestionRRHH;
using System.Data;
using DevExpress.XtraEditors;
using BE_GestionRRHH;

namespace BL_GestionRRHH
{
    public class blProveedores
    {
        daSQL _sql;
        public blProveedores(daSQL sql) { _sql = sql; }

        public List<T> ListarOpcionesMenu<T>(int opcion) where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                {"opcion", opcion }
            };

            myList = _sql.ListaconSP<T>("usp_ConsultasVarias_Proveedor", oDictionary);
            return myList;
        }

        public T Validar_NroDocumento<T>(int opcion, string num_documento, string cod_tipo_documento = "") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                {"opcion", opcion }, {"num_documento", num_documento },
                {"cod_tipo_documento", cod_tipo_documento }
            };

            obj = _sql.ConsultarEntidad<T>("usp_ConsultasVarias_Proveedor", dictionary);
            return obj;
        }

        public void CargaCombosLookUp(string nCombo, LookUpEdit combo, string campoValueMember, string campoDispleyMember, string campoSelectedValue = "", string cod_cliente = "", string cod_nivel = "", int num_linea = 0, int num_nivel = 0, bool valorDefecto = false)
        {
            combo.Text = "";
            string procedure = "usp_ConsultasVarias_Proveedor";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            DataTable tabla = new DataTable();

            try
            {
                switch (nCombo)
                {
                    case "Moneda":
                        dictionary.Add("opcion", 7);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoCuentaBancaria":
                        dictionary.Add("opcion", 8);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoProveedor":
                        dictionary.Add("opcion", 9);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoCompra":
                        dictionary.Add("opcion", 10);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "Frecuencia":
                        dictionary.Add("opcion", 13);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "FormaPago":
                        dictionary.Add("opcion", 14);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                }

                combo.Properties.DataSource = tabla;
                combo.Properties.ValueMember = campoValueMember;
                combo.Properties.DisplayMember = campoDispleyMember;
                if (campoSelectedValue == "") { combo.ItemIndex = -1; } else { combo.EditValue = campoSelectedValue; }
                if (tabla.Columns["flg_default"] != null) if (valorDefecto) combo.EditValue = tabla.Select("flg_default = 'SI'").Length == 0 ? null : (tabla.Select("flg_default = 'SI'"))[0].ItemArray[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> ListarEmpresasProveedor<T>(int opcion, string cod_proveedor="", string cod_usuario = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                {"opcion", opcion },
                { "cod_proveedor", cod_proveedor},
                { "cod_usuario", cod_usuario}
            };

            myList = _sql.ListaconSP<T>("usp_Consulta_ListarProveedores", oDictionary);
            return myList;
        }


    }
}
