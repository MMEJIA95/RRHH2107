using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_GestionRRHH.FormatoMD
{
    public class eFormatoMD_General
    {
        public string cod_formatoMD_general { get; set; }
        public string cod_formatoMD_grupo { get; set; }
        public string cod_tipo_formato { get; set; }
        public string dsc_formatoMD_general { get; set; }
        public int num_modelo { get; set; }
        public string dsc_observacion { get; set; }
        public string dsc_wordMLText { get; set; }
        public string flg_obligatorio { get; set; }
        public string flg_editado { get; set; }
        public string flg_publicado { get; set; }
        public DateTime fch_registro { get; set; }
        public string cod_usuario_registro { get; set; }
        public DateTime fch_cambio { get; set; }
        public string cod_usuario_cambio { get; set; }
        public DateTime fch_publicacion { get; set; }
        public string cod_usuario_publicacion { get; set; }

        public class eFormatoMD_General_Vista : eFormatoMD_General
        {
            public string dsc_formatoMD_grupo { get; set; }
            public string dsc_modelo { get; set; }
        }
        public class eFormatoDocumento : eFormatoMD_General
        {
            public string cod_formatoMD_seguimiento { get; set; }
            public string cod_detalle_seguimiento { get; set; }
            public string cod_formatoMD_general { get; set; }
            public string dsc_formatoMD_general { get; set; }
            public int cod_formatoMD_vinculo { get; set; }
            public string dsc_formatoMD_vinculo { get; set; }
            public string cod_estado { get; set; }
            public DateTime fch_firma { get; set; }
            public string CABECERA { get; set; }
        }
    }
    public class eEmpresaUsuaioVista
    {
        public string cod_empresa { get; set; }
        public string dsc_empresa { get; set; }
    }
    public class eFormatoMDGeneral_Tree
    {
        public string cod_formatoMD_grupo { get; set; }
        public string dsc_formatoMD_grupo { get; set; }
        public string cod_formatoMD_general { get; set; }
        public string dsc_formatoMD_general { get; set; }
        public int num_jerarquia { get; set; }
        public string flg_obligatorio { get; set; }
        public string num_modelo { get; set; }
        public string flg_publicado { get; set; }
    }
}
