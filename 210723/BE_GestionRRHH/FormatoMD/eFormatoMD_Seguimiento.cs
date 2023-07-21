using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_GestionRRHH.FormatoMD
{
    public class eFormatoMD_Seguimiento
    {
        public string cod_formatoMD_seguimiento { get; set; }
        public string cod_empresa { get; set; }
        public string cod_trabajador { get; set; }
        public string cod_formatoMD_vinculo { get; set; }
        public DateTime fch_inicio_firma { get; set; }
        public DateTime fch_vence_firma { get; set; }
        public string dsc_observacion { get; set; }
        public string cod_estado { get; set; }
        public string flg_activo { get; set; }
        public DateTime fch_registro { get; set; }
        public string cod_usuario_registro { get; set; }
        public DateTime fch_cambio { get; set; }
        public string cod_usuario_cambio { get; set; }
    }
    public class eFormatoMD_SeguimientoDetalle
    {
        public string cod_detalle_seguimiento { get; set; }
        public string cod_formatoMD_seguimiento { get; set; }
        public string cod_empresa { get; set; }
        public string cod_firma { get; set; }
        public DateTime fch_firma { get; set; }
        public string cod_estado { get; set; }
        public string dsc_observacion { get; set; }
        public string flg_activo { get; set; }
        public DateTime fch_registro { get; set; }
        public string cod_usuario_registro { get; set; }
        public DateTime fch_cambio { get; set; }
        public string cod_usuario_cambio { get; set; }
    }
    public class eFormatoMD_SeguimientoGroupVista
    {
        public string cod_empresa { get; set; }
        public string dsc_empresa { get; set; }
        public string cod_formatoMD_vinculo { get; set; }
        public string dsc_formatoMD_vinculo { get; set; }
        public string dsc_estado { get; set; }
        public string cod_estado { get; set; }
        public int num_documentos { get; set; }
    }
    public class eFormatoMD_SeguimientoResumenVista
    {
        public string cod_detalle_seguimiento { get; set; }
        public string cod_formatoMD_seguimiento { get; set; }
        public string dsc_nombre_trabajador { get; set; }
        public string cod_estado { get; set; }
        public DateTime fch_inicio_firma { get; set; }
        public DateTime fch_vence_firma { get; set; }
        public string dsc_fch_firma { get; set; }
        public string dsc_cargo { get; set; }
        public string dsc_observacion { get; set; }
    }




}
