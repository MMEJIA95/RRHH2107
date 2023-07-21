using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_GestionRRHH;
using DA_GestionRRHH;
using DevExpress.XtraEditors;
using System.Data;

namespace BL_GestionRRHH
{
    public class blClientes
    {
        daSQL _sql;
        public blClientes(daSQL sql) { _sql = sql; }

        public void CargaCombosLookUp(string nCombo, LookUpEdit combo, string campoValueMember, string campoDispleyMember, string campoSelectedValue = "", string cod_cliente = "", string cod_nivel = "", int num_linea = 0, int num_nivel = 0, string cod_condicion = "", bool valorDefecto = false)
        {
            combo.Text = "";
            string procedure = "usp_ConsultasVarias_Cliente";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            DataTable tabla = new DataTable();

            try
            {
                switch (nCombo)
                {
                    case "TipoEstadoCivil":
                        dictionary.Add("opcion", 6);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoSexo":
                        dictionary.Add("opcion", 7);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoPais":
                        dictionary.Add("opcion", 13);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoDepartamento":
                        dictionary.Add("cod_condicion", cod_condicion);
                        dictionary.Add("opcion", 14);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoProvincia":
                        dictionary.Add("cod_condicion", cod_condicion);
                        dictionary.Add("opcion", 15);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoDistrito":
                        dictionary.Add("cod_condicion", cod_condicion);
                        dictionary.Add("opcion", 16);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoDocumento":
                        dictionary.Add("opcion", 17);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "TipoCliente":
                        dictionary.Add("opcion", 18);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "NivelUbicacion":
                        dictionary.Add("opcion", 22);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "NivelSuperiorUbicacion":
                        dictionary.Add("opcion", 23);
                        dictionary.Add("cod_cliente", cod_cliente);
                        dictionary.Add("cod_nivel", cod_nivel);
                        dictionary.Add("num_linea", num_linea);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "ResponsableUbicacion":
                        dictionary.Add("opcion", 24);
                        dictionary.Add("cod_cliente", cod_cliente);
                        dictionary.Add("num_linea", num_linea);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "NivelSuperiorCentroResponsabilidad":
                        dictionary.Add("opcion", 25);
                        dictionary.Add("cod_cliente", cod_cliente);
                        dictionary.Add("num_nivel", num_nivel);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "ResponsableCentroResponsabilidad":
                        dictionary.Add("opcion", 26);
                        dictionary.Add("cod_cliente", cod_cliente);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "ResponsableTrabajador":
                        dictionary.Add("opcion", 34);
                        tabla = _sql.ListaDatatable(procedure, dictionary);
                        break;
                    case "NivelSuperiorUbicacionInterna":
                        dictionary.Add("opcion", 35);
                        dictionary.Add("cod_nivel", cod_nivel);
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
        public List<T> ListarOpcionesMenu<T>(int opcion, string cod_empresa = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                {"opcion", opcion }, {"cod_empresa", cod_empresa }
            };

            myList = _sql.ListaconSP<T>("usp_ConsultasVarias_Trabajador", oDictionary);
            return myList;
        }

        public List<T> ListarOpcionesVariasCliente<T>(int opcion, string cod_cliente = "", string cod_condicion = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                {"opcion", opcion },
                {"cod_cliente", cod_cliente },
                {"cod_condicion", cod_condicion }
            };

            myList = _sql.ListaconSP<T>("usp_ConsultasVarias_Cliente", oDictionary);
            return myList;
        }
    }
}
