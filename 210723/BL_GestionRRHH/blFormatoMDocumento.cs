using BE_GestionRRHH.FormatoMD;
using DA_GestionRRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_GestionRRHH
{
    public class blFormatoMDocumento
    {
        daSQL _sql;
        public blFormatoMDocumento(daSQL sql) { _sql = sql; }

        /// <summary>
        /// Retorna Consulta Varias del módulo Formato de documentos.
        /// </summary>
        /// <typeparam name="T">Clase genérica: asignar de acuerdo a la opción.</typeparam>
        /// <param name="param">Clase de parámetros.</param>
        /// <returns></returns>
        public List<T> ConsultaVarias_FormatoMDocumento<T>(pQFormatoMD param) where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                { "opcion", param.Opcion },
                { "cod_usuario", param.Cod_usuario},
                { "cod_estado", param.Cod_estado},
                { "cod_formatoMD_seguimiento", param.Cod_formatoMD_seguimiento},
                { "cod_empresaSplit", param.Cod_empresaSplit},
                { "dsc_formatoMD_general", param.Dsc_formatoMD_general},
                { "cod_formatoMD_generalSplit", param.Cod_formatoMD_generalSplit},
                { "cod_formatoMD_vinculoSplit", param.Cod_formatoMD_vinculoSplit},
                { "cod_trabajadorSplit", param.Cod_trabajadorSplit},
            };

            myList = _sql.ListaconSP<T>("Usp_RHU_ConsultasVarias_FormatoMDocumento", oDictionary);
            return myList;
        }
        public T InsertarActualizar_FormatoMDGeneral<T>(int opcion, eFormatoMD_General eDoc) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_formatoMD_general", eDoc.cod_formatoMD_general },
                { "cod_formatoMD_grupo", eDoc.cod_formatoMD_grupo },
                { "cod_tipo_formato", eDoc.cod_tipo_formato },
                { "dsc_formatoMD_general", eDoc.dsc_formatoMD_general },
                { "num_modelo", eDoc.num_modelo },
                { "dsc_observacion", eDoc.dsc_observacion},
                { "dsc_wordMLText", eDoc.dsc_wordMLText },
                { "flg_obligatorio", eDoc.flg_obligatorio},
                { "cod_usuario_cambio", eDoc.cod_usuario_cambio },
            };

            obj = _sql.ConsultarEntidad<T>("Usp_RHU_InsertarActualizar_FormatoMD_General", dictionary);
            return obj;
        }

        public T InsertarActualizar_FormatoMDVinculo<T>(int opcion, eFormatoMD_Vinculo eDoc) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_empresa", eDoc.cod_empresa },
                { "cod_formatoMD_general", eDoc.cod_formatoMD_general },
                { "cod_formatoMD_vinculo", eDoc.cod_formatoMD_vinculo },
                { "dsc_formatoMD_vinculo", eDoc.dsc_formatoMD_vinculo },
                { "dsc_observacion", eDoc.dsc_observacion },
                { "flg_obligatorio", eDoc.flg_obligatorio },
                { "flg_seguimiento", eDoc.flg_seguimiento },
                { "cod_cargo_firma", eDoc.cod_cargo_firma },
                { "dsc_version", eDoc.dsc_version },
                { "flg_estado", eDoc.flg_estado },
                { "dsc_wordMLText", eDoc.dsc_wordMLText },
                { "cod_usuario_cambio", eDoc.cod_usuario_cambio },
            };

            obj = _sql.ConsultarEntidad<T>("Usp_RHU_InsertarActualizar_FormatoMD_Vinculo", dictionary);
            return obj;
        }
        public T InsertarActualizar_FormatoMDGrupo<T>(eFormatoMD_Grupo eGrupo) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_formatoMD_grupo", eGrupo.cod_formatoMD_grupo },
                { "dsc_formatoMD_grupo", eGrupo.dsc_formatoMD_grupo },
                { "num_jerarquia", eGrupo.num_jerarquia }
            };

            obj = _sql.ConsultarEntidad<T>("Usp_RHU_InsertarActualizar_FormatoMD_Grupo", dictionary);
            return obj;
        }
        public T InsertarActualizar_FormatoMDSeguimiento<T>(int opcion,eFormatoMD_Seguimiento eDoc) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_formatoMD_seguimiento", eDoc.cod_formatoMD_seguimiento },
                { "cod_empresa", eDoc.cod_empresa },
                { "cod_trabajador", eDoc.cod_trabajador },
                { "cod_formatoMD_vinculo", eDoc.cod_formatoMD_vinculo },
                { "fch_inicio_firma", eDoc.fch_inicio_firma },
                { "fch_vence_firma", eDoc.fch_vence_firma },
                { "dsc_observacion", eDoc.dsc_observacion },
                { "cod_usuario_cambio", eDoc.cod_usuario_cambio },
            };

            obj = _sql.ConsultarEntidad<T>("Usp_RHU_InsertarActualizar_FormatoMD_Seguimiento", dictionary);
            return obj;
        }

        public T InsertarActualizar_FormatoMDSeguimientoDetalle<T>(int opcion,eFormatoMD_SeguimientoDetalle eDoc) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "opcion", opcion },
                { "cod_detalle_seguimiento", eDoc.cod_detalle_seguimiento },
                { "cod_formatoMD_seguimiento", eDoc.cod_formatoMD_seguimiento },
                { "cod_empresa", eDoc.cod_empresa },
                { "cod_firma", eDoc.cod_firma },
                { "cod_estado", eDoc.cod_estado },
                { "dsc_observacion", eDoc.dsc_observacion },
                { "flg_activo", eDoc.flg_activo },
                { "cod_usuario_cambio", eDoc.cod_usuario_cambio },
            };

            obj = _sql.ConsultarEntidad<T>("Usp_RHU_InsertarActualizar_FormatoMD_SeguimientoDetalle", dictionary);
            return obj;
        }
    }
}
