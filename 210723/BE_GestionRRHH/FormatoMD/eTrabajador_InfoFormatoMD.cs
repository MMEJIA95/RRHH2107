using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_GestionRRHH.FormatoMD
{
    public class eTrabajador_InfoFormatoMD
    {
        public string cod_empresa { get; set; }
        public string cod_trabajador { get; set; }
        public string cod_formatoMD_vinculo { get; set; }
        public string flg_activo { get; set; }
        public DateTime fch_registro { get; set; }
        public string cod_usuario_registro { get; set; }
        public DateTime fch_cambio { get; set; }
        public string cod_usuario_cambio { get; set; }
    }

    public class eTrabajador_InfoFormatoMD_Vista
    {
        public string cod_empresa { get; set; }
        public string cod_trabajador { get; set; }
        public string cod_formatoMD_vinculo { get; set; }
        public string flg_activo { get; set; }
        public string dsc_formatoMD_vinculo { get; set; }
        public string dsc_version { get; set; }
        public string flg_obligatorio { get; set; }
        public string flg_publicado { get; set; }
        public string dsc_wordMLText { get; set; }
        public string dsc_formatoMD_grupo { get; set; }
    }
    public class eTrabajador_InfoFormatoMD_Resumen
    {
        public string cod_formatoMD_seguimiento { get; set; }
        public string dsc_formatoMD_vinculo { get; set; }
        public DateTime fch_inicio_firma { get; set; }
        public DateTime fch_vence_firma { get; set; }
        public string cod_estado { get; set; }
        public string dsc_observacion { get; set; }
    }
}
