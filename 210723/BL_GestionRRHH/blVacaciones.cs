using BE_GestionRRHH;
using DA_GestionRRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_GestionRRHH
{
    public class blVacaciones
    {
        daSQL _sql;
        public blVacaciones(daSQL sql) { _sql = sql; }

        public List<T> ConsultaVarias_Vacaciones<T>(pVacaciones param) where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                { "opcion", param.Opcion },
                { "cod_trabajador", param.Cod_trabajador},
                { "cod_vacacionGeneral", param.Cod_vacacionGeneral},
                { "cod_periodo", param.Cod_periodo},
                { "cod_empresa_multiple", param.Cod_empresa_multiple},
                { "cod_sede_multiple", param.Cod_sede_multiple },
                { "cod_area_multiple", param.Cod_area_multiple },
                { "fch_rango_inicial", param.Fch_rango_inicial},
                { "fch_rango_final", param.Fch_rango_final},
            };

            myList = _sql.ListaconSP<T>("Usp_RHU_ConsultasVarias_Vacaciones", oDictionary);
            return myList;
        }
        public T InsertarActualizar_VacacionesDetalle<T>(eVacacionesDetalle objEntidad) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_vacacionDetalle", objEntidad.cod_vacacionDetalle },
                { "cod_empresa", objEntidad.cod_empresa },
                { "cod_vacacionGeneral", objEntidad.cod_vacacionGeneral },
                { "cod_tipo_vacacion", objEntidad.cod_tipo_vacacion },
                { "fch_solicitud", objEntidad.fch_solicitud },
                { "flg_aprobado", objEntidad.flg_aprobado },
                { "fch_inicio_vacacion", objEntidad.fch_inicio_vacacion },
                { "fch_fin_vacacion", objEntidad.fch_fin_vacacion },
                { "num_dias_goce", objEntidad.num_dias_goce},
                { "dsc_observacion", objEntidad.dsc_observacion },
                { "cod_usuario_cambio", objEntidad.cod_usuario_cambio},
            };

            obj = _sql.ConsultarEntidad<T>("Usp_RHU_Insertar_Actualizar_VacacionesDetalle", dictionary);
            return obj;
        }
    }
}
