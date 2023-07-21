using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_GestionRRHH
{
    public class eSubModulo
    {

        public int cod_submodulo { get; set; }
        public string dsc_submodulo { get; set; }
        public string dsc_descripcion { get; set; }


        public class eCampos : eSubModulo
        {
            public int cod_campoxempresa { get; set; }
            public int cod_submodulo { get; set; }
            public string cod_campo { get; set; }
            public string dsc_descripcion { get; set; }
            public bool flg_bloqueo { get; set; }
            public bool flg_obligatorio { get; set; }
            public string cod_solucion { get; set; }
            public string cod_empresa { get; set; }
            public DateTime fch_registro { get; set; }
            public string cod_usuario_registro { get; set; }
            public DateTime fch_cambio { get; set; }
            public string cod_usuario_cambio { get; set; }
            public bool flg_altaplacar { get; set; }
            public bool flg_tregistro { get; set; }
            public string dsc_empresa { get; set; }
        }

    }
}
