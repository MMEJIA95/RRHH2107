﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_GestionRRHH;
using DA_GestionRRHH;
using System.Data;
using DevExpress.XtraEditors;
//using System.Windows.Forms;

namespace BL_GestionRRHH
{

    public class blSistema
    {
        private readonly daSQL sql;
        public blSistema(daSQL sql) { this.sql = sql; }


        public T Guardar_Eliminar_UsuarioSolucion<T>(eSolucionUsuario eSol, bool flg_acceso) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_usuario", eSol.cod_usuario },
                { "cod_solucion", eSol.cod_solucion },
                { "cod_usuario_registro", eSol.cod_usuario_registro },
                { "flg_acceso", flg_acceso },
            };

            obj = sql.ConsultarEntidad<T>("Usp_SCF_Insertar_Eliminar_UsuarioSolucion", dictionary);
            return obj;
        }

        public List<T> ListarSolucion<T>(int opcion, string cod_usuario = "",
            string dsc_token_sesion = "", string dsc_solucion = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> dictionary = new Dictionary<string, object>() {
                {"opcion", opcion },
                {"cod_usuario", cod_usuario },
                {"dsc_token_sesion", dsc_token_sesion },
                {"dsc_solucion", dsc_solucion },
            };
            myList = sql.ListaconSP<T>("Usp_SCF_ConsultasVarias_Solucion", dictionary);
            return myList;
        }

        public List<T> ListarPerfiles<T>(int opcion, string cod_usuario, string flg_activo = "", string dsc_solucion="") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> dictionary = new Dictionary<string, object>() {
                { "opcion", opcion},
                { "cod_usuario", cod_usuario},
                { "flg_activo", flg_activo},
                { "dsc_solucion", dsc_solucion}
            };
            myList = sql.ListaconSP<T>("usp_Consulta_ListarPerfiles", dictionary);
            return myList;

        }

        public List<T> ListarVentanas<T>(int opcion, int cod_perfil = 0, string flg_activo = "",string dsc_solucion="") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> dictionary = new Dictionary<string, object>() {
                { "opcion", opcion},
                { "cod_perfil", cod_perfil},
                { "flg_activo", flg_activo},
                { "dsc_solucion", dsc_solucion}
            };
            myList = sql.ListaconSP<T>("usp_Consulta_ListarVentanas", dictionary);
            return myList;

        }

        public T ObtenerVentana<T>(int opcion, int cod_ventana, string dsc_solucion="") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>() {
                { "opcion", opcion},
                { "cod_ventana", cod_ventana},
                { "dsc_solucion", dsc_solucion},
            };

            obj = sql.ConsultarEntidad<T>("usp_Consulta_ListarVentanas", dictionary);
            return obj;
        }

        public T ObtenerPerfil<T>(int opcion, int cod_perfil, string dsc_solucion="") where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>() {
                { "opcion", opcion},
                { "cod_perfil", cod_perfil},
                { "dsc_solucion", dsc_solucion}
            };

            obj = sql.ConsultarEntidad<T>("usp_Consulta_ListarPerfiles", dictionary);
            return obj;
        }

        public void CargaCombosLookUp(string nCombo, LookUpEdit combo, string campoValueMember, string campoDispleyMember, string campoSelectedValue = "", string dsc_solucion = "")
        {
            combo.Text = "";
            string procedure = "usp_ConsultasVarias_Sistema";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            DataTable tabla = new DataTable();

            try
            {
                switch (nCombo)
                {
                    //case "Soluciones":
                    //    dictionary.Add("opcion", 1);
                    //    tabla = sql.ListaDatatable(procedure, dictionary);
                    //    break;
                    case "Modulos":
                        dictionary.Add("opcion", 1);
                        dictionary.Add("dsc_solucion", dsc_solucion);
                        tabla = sql.ListaDatatable(procedure, dictionary);
                        break;
                }

                combo.Properties.DataSource = tabla;
                combo.Properties.ValueMember = campoValueMember;
                combo.Properties.DisplayMember = campoDispleyMember;
                if (campoSelectedValue == "") { combo.ItemIndex = -1; } else { combo.EditValue = campoSelectedValue; }
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        public T Guardar_Actualizar_Ventana<T>(int opcion, eVentana eVen, string MiAccion, string coduser) where T : class, new()
        {
            T obj = new T();
            string procedure = "";
            procedure = MiAccion == "Nuevo" ? "usp_Insertar_Ventana" : "usp_Actualizar_Ventana";
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_ventana", eVen.cod_ventana },
                { "dsc_ventana", eVen.dsc_ventana },
                { "dsc_solucion", eVen.dsc_solucion },
                { "cod_grupo", eVen.cod_grupo },
                { "dsc_menu", eVen.dsc_menu },
                { "dsc_formulario", eVen.dsc_formulario },
                { "cod_usuario_registro", coduser },
                { "flg_activo", eVen.flg_activo },
                { "num_orden", eVen.num_orden }

            };

            obj = sql.ConsultarEntidad<T>(procedure, dictionary);
            return obj;
        }

        public T Guardar_Actualizar_Perfil<T>(int opcion, ePerfil ePer, string MiAccion, string coduser) where T : class, new()
        {
            T obj = new T();
            string procedure = "";
            procedure = MiAccion == "Nuevo" ? "usp_Insertar_Perfil" : "usp_Actualizar_Perfil";
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_perfil", ePer.cod_perfil },
                { "dsc_perfil", ePer.dsc_perfil },
                { "cod_usuario_registro", coduser },
                { "flg_activo", ePer.flg_activo },
                { "dsc_solucion", ePer.dsc_solucion },

            };


            obj = sql.ConsultarEntidad<T>(procedure, dictionary);
            return obj;
        }

        public List<T> ListarOpcionesMenuVentana<T>(int opcion) where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                {"opcion", opcion }
            };

            myList = sql.ListaconSP<T>("usp_ConsultasVarias_Sistema", oDictionary);
            return myList;
        }

        public List<T> ListarOpcionesMenuPerfil<T>(int opcion) where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                {"opcion", opcion }
            };

            myList = sql.ListaconSP<T>("usp_ConsultasVarias_Sistema", oDictionary);
            return myList;
        }
        public string Guardar_Perfil_Ventana(eVentana eVen, string coduser = "")
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            dictionary.Add("cod_item", eVen.cod_item);
            dictionary.Add("cod_ventana", eVen.cod_ventana);
            dictionary.Add("cod_perfil", eVen.cod_perfil);
            dictionary.Add("flg_lectura", eVen.flg_lectura);
            dictionary.Add("flg_escritura", eVen.flg_escritura);
            dictionary.Add("cod_usuario_registro", coduser);
            dictionary.Add("dsc_solucion", eVen.dsc_solucion);

            string result;
            result = sql.ExecuteScalarWithParams("usp_Insertar_PerfilVentanas", dictionary);
            return result;
        }


        public T Eliminar_Perfil<T>(int opcion, int cod_perfil, string dsc_solucion="") where T : class, new()
        {
            T obj = new T();
            string procedure = "";
            procedure = "usp_Eliminar_Perfil";
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_perfil", cod_perfil },
                { "dsc_solucion", dsc_solucion },

            };


            obj = sql.ConsultarEntidad<T>(procedure, dictionary);
            return obj;
        }

        public T Eliminar_Ventana<T>(int opcion, int cod_ventana, string dsc_solucion="") where T : class, new()
        {
            T obj = new T();
            string procedure = "";
            procedure = "usp_Eliminar_Ventana";
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_ventana", cod_ventana },
                { "dsc_solucion", dsc_solucion },
            };


            obj = sql.ConsultarEntidad<T>(procedure, dictionary);
            return obj;
        }

        public List<T> ListarEntidad<T>(int opcion, string cod_condicion1 = "", string cod_condicion2 = "", string flg_transportista = "", string cod_empresa = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                {"opcion", opcion },
                {"cod_condicion1", cod_condicion1 },
                {"cod_condicion2", cod_condicion2 },
                {"flg_transportista", flg_transportista },
                {"cod_empresa", cod_empresa }
            };

            myList = sql.ListaconSP<T>("usp_Consulta_BusquedaEntidad", oDictionary);
            return myList;
        }


        public List<T> Obtener_ParamterosSistema<T>(int opcion, string cod_empresa = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> dictionary = new Dictionary<string, object>() {
                { "opcion", opcion},
                { "cod_empresa", cod_empresa}
            };
            myList = sql.ListaconSP<T>("usp_ConsultasVarias_Login", dictionary);
            return myList;
        }

        public List<T> ListarMenuxUsuario<T>(string cod_usuario, string dsc_formulario, string dsc_solucion) where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("opcion", 3);
            dictionary.Add("cod_usuario", cod_usuario);
            dictionary.Add("dsc_formulario", dsc_formulario);
            dictionary.Add("dsc_solucion", dsc_solucion);

            myList = sql.ListaconSP<T>("usp_ConsultasVarias_Sistema", dictionary);
            return myList;
        }


        public List<T> ListarPerfilesUsuario<T>(int opcion, string cod_usuario, string dsc_solucion) where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("opcion", opcion);
            dictionary.Add("dsc_solucion", dsc_solucion);
            dictionary.Add("cod_usuario", cod_usuario);

            myList = sql.ListaconSP<T>("usp_ConsultasVarias_Sistema", dictionary);
            return myList;
        }

        public List<T> Obtener_DatosProductos<T>(int opcion, string cod_empresa = "") where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("opcion", opcion);
            dictionary.Add("cod_empresa", cod_empresa);

            myList = sql.ListaconSP<T>("usp_Consulta_ListarFacturasProveedor", dictionary);
            return myList;
        }
        public List<T> Listar_SubModulos<T>(int opcion, string cod_empresa = "" , string cod_solucion="" , int cod_submodulo = 1) where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("opcion", opcion);
            dictionary.Add("dsc_empresa", cod_empresa);
            dictionary.Add("dsc_solucion", cod_solucion);
            dictionary.Add("cod_submodulo", cod_submodulo);

            myList = sql.ListaconSP<T>("usp_Consulta_ListarSubModulos", dictionary);
            return myList;
        }
        public string GuardarActualizar_asignacion(eSubModulo.eCampos eVen, int opcion,string coduser = "")
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            dictionary.Add("opcion", opcion);
            dictionary.Add("cod_empresa", eVen.cod_empresa);
            dictionary.Add("flg_bloqueo", eVen.flg_bloqueo);
            dictionary.Add("flg_obligatorio", eVen.flg_obligatorio);
            dictionary.Add("flg_altaplacar", eVen.flg_altaplacar);
            dictionary.Add("flg_tregistro", eVen.flg_tregistro);
            dictionary.Add("cod_campoxempresa", eVen.cod_campoxempresa);

            string result;
            result = sql.ExecuteScalarWithParams("usp_Consulta_ListarSubModulos", dictionary);
            return result;
        }

    }
}
