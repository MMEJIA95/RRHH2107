using DA_GestionRRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_GestionRRHH
{
    public enum TyVariableGeneral
    {        
        MailBoletaPago=0
    }
    public class blVariableGeneral
    {
        daSQL _sql;
        public blVariableGeneral(daSQL sql) { _sql = sql; }

        /// <summary>
        /// Retorna Consulta Varias del módulo Formato de documentos.
        /// </summary>
        /// <typeparam name="T">Clase genérica: asignar de acuerdo a la opción.</typeparam>
        /// <param name="param">Clase de parámetros.</param>
        /// <returns></returns>
        public List<T> ConsultaVarias_VariableGeneral<T>(int opcion,TyVariableGeneral tyVariable) where T : class, new()
        {
            string referencia = "";
            switch (tyVariable)
            {
                case TyVariableGeneral.MailBoletaPago:
                    referencia = "001";
                    break;
            }

            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_referencia", referencia}
            };

            myList = _sql.ListaconSP<T>("Usp_RHU_ConsultasVarias_VariableGeneral", oDictionary);
            return myList;
        }

        public T Actualizar_VariableGeneral<T>(int opcion, string cod_variable, string dsc_valor) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_variable", cod_variable },
                { "dsc_valor", dsc_valor},
            };

            obj = _sql.ConsultarEntidad<T>("Usp_RHU_Actualizar_VariableGeneral", dictionary);
            return obj;
        }
    }
}
