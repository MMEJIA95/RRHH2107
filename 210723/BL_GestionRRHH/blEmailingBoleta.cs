using BE_GestionRRHH;
using DA_GestionRRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_GestionRRHH
{
    public class blEmailingBoleta
    {
        daSQL _sql;
        public blEmailingBoleta(daSQL sql) { _sql = sql; }


        public List<T> ConsultaVarias_EmailingBoletas<T>(pEmailingBoleta param) where T : class, new()
        {
            List<T> myList = new List<T>();
            Dictionary<string, object> oDictionary = new Dictionary<string, object>()
            {
                { "opcion", param.Opcion },
                { "cod_empresaSplit", param.Cod_empresaSplit},
                { "dsc_emailingBoletaSplit", param.Dsc_emailingBoletaSplit},
                { "cod_usuario_enviado", param.Cod_usuario_enviado},
                { "dsc_email_trabajadorSplit", param.Dsc_email_trabajadorSplit},
                { "dsc_archivo_enviadoSplit", param.Dsc_archivo_enviadoSplit},
                { "flg_enviadoSplit", param.Flg_enviadoSplit},
                { "dsc_documento_trabajadorSplit", param.Dsc_documento_trabajadorSplit},
                { "cod_emailingFormato", param.Cod_emailingFormato},
                { "cod_tipo_formato", param.Cod_tipo_formato},
            };

            myList = _sql.ListaconSP<T>("Usp_RHU_ConsultasVarias_EmailingBoletas", oDictionary);
            return myList;
        }

        public T Actualizar_FormatoEmail<T>(eEmailFormato eEmail) where T : class, new()
        {
            T obj = new T();
            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "cod_emailingFormato", eEmail.cod_emailingFormato },
                { "cod_empresa", eEmail.cod_empresa },
                { "cod_tipo_formato", eEmail.cod_tipo_formato },
                { "dsc_emailingFormato", eEmail.dsc_emailingFormato },
                { "dsc_asunto", eEmail.dsc_asunto },
                { "dsc_cuerpo", eEmail.dsc_cuerpo },
                { "dsc_rutaAdjunto", eEmail.dsc_rutaAdjunto },
            };

            obj = _sql.ConsultarEntidad<T>("Usp_RHU_InsertarActualizar_EmailingFormato", dictionary);
            return obj;
        }
    }
}
