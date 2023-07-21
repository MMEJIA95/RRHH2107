using BE_GestionRRHH;
using DA_GestionRRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_GestionRRHH
{
    public class blAusencia
    {
        daSQL _sql;
        public blAusencia(daSQL sql) { _sql = sql; }

        public List<T> ConsultaVarias_Ausencias<T>(pAusencia param) where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                { "opcion", param.Opcion },
                { "cod_ausenciaGeneral", param.cod_ausenciaGeneral},
                { "cod_empresa", param.Cod_empresa},
                { "cod_empresaMultiple", param.cod_empresaMultiple},
                { "cod_sedeMultiple", param.cod_sedeMultiple},
                { "cod_areaMultiple", param.cod_areaMultiple},
            };

            myList = _sql.ListaconSP<T>("Usp_RHU_ConsultasVarias_Ausencias", oDictionary);
            return myList;
        }
    }
}
