using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_GestionRRHH.FormatoMD
{
    public class eFormatoMD_Vinculo
    {
        public string cod_empresa { get; set; }
        public string cod_formatoMD_general { get; set; }
        public string cod_formatoMD_vinculo { get; set; }
        public string dsc_formatoMD_vinculo { get; set; }
        public string dsc_observacion { get; set; }
        public string dsc_wordMLText { get; set; }
        public string flg_publicado { get; set; }
        public string flg_cambio_maestro { get; set; }
        public string flg_obligatorio { get; set; }
        public string flg_seguimiento { get; set; }
        public string cod_cargo_firma { get; set; }
        public string dsc_version { get; set; }
        public string flg_estado { get; set; }
        public DateTime fch_registro { get; set; }
        public string cod_usuario_registro { get; set; }
        public DateTime fch_cambio { get; set; }
        public string cod_usuario_cambio { get; set; }
    }
    public class eFormatoMD_Vinculo_Filtro
    {
        public string cod_formatoMD_grupo { get; set; }
        public string dsc_formatoMD_grupo { get; set; }
        public string cod_formatoMD_vinculo { get; set; }
        public string dsc_formatoMD_vinculo { get; set; }
        public string flg_publicado { get; set; }
        public string flg_cambio_maestro { get; set; }
        public int num_jerarquia { get; set; }
        public string flg_obligatorio { get; set; }
        public string flg_seguimiento { get; set; }
        public string dsc_version { get; set; }
        public string flg_estado { get; set; }
        public DateTime fch_cambio { get; set; }
        public string dsc_wordMLText { get; set; }
    }
}
