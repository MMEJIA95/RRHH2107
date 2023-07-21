using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_GestionRRHH
{
    public class eAusencia
    {
        public string cod_ausenciaGeneral { get; set; }
        public string cod_empresa { get; set; }
        public string cod_trabajador { get; set; }
        public string cod_tipo_ausencia { get; set; }
        public DateTime fch_inicio { get; set; }
        public DateTime fch_final { get; set; }
        public int num_dias { get; set; }
        public string cod_tipo_pago { get; set; }
        public string flg_estado { get; set; }
        public string flg_justificado { get; set; }
        public string flg_activo { get; set; }
        public DateTime fch_registro { get; set; }
        public string cod_usuario_registro { get; set; }
        public DateTime fch_cambio { get; set; }
        public string cod_usuario_cambio { get; set; }
    }

    public class eAusenciaListado : eAusencia
    {
        public string dsc_empresa { get; set; }
        public string dsc_sede_empresa { get; set; }
        public string dsc_area { get; set; }
        public string dsc_tipo_ausencia { get; set; }
        public string dsc_trabajador { get; set; }
        public string dsc_tipo_pago { get; set; }
    }
    public class pAusencia : pConsultaBase
    {
        public string cod_ausenciaGeneral { get; set; }
        public string cod_empresaMultiple { get; set; }
        public string cod_sedeMultiple { get; set; }
        public string cod_areaMultiple { get; set; }
    }
}
