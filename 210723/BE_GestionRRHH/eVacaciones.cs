using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_GestionRRHH
{
    public class eVacaciones   // SI VA
    {
        public string cod_vacacionGeneral { get; set; }
        public string cod_empresa { get; set; }
        public string cod_trabajador { get; set; }
        public DateTime fch_inicio_periodo { get; set; }
        public DateTime fch_fin_periodo { get; set; }
        public int num_dias_si_laborados { get; set; }
        public int num_dias_no_laborados { get; set; }
        public int num_dias_contable { get; set; }
        public decimal num_meses { get; set; }
        public string dsc_observacion { get; set; }
        public string flg_gozado { get; set; }
        public string flg_cerrado { get; set; }
        public string flg_activo { get; set; }
        public DateTime fch_registro { get; set; }
        public string cod_usuario_registro { get; set; }
        public DateTime fch_cambio { get; set; }
        public string cod_usuario_cambio { get; set; }
    }
    public class eVacacionesDetalle  // SI VA
    {
        public string cod_vacacionDetalle { get; set; }
        public string cod_empresa { get; set; }
        public string cod_vacacionGeneral { get; set; }
        public string cod_tipo_vacacion { get; set; }
        public DateTime fch_solicitud { get; set; }
        public DateTime fch_aprobacion { get; set; }
        public string flg_aprobado { get; set; }
        public string cod_usuario_aprobado { get; set; }
        public DateTime fch_inicio_vacacion { get; set; }
        public DateTime fch_fin_vacacion { get; set; }
        public int num_dias_goce { get; set; }
        public string dsc_observacion { get; set; }
        public DateTime fch_registro { get; set; }
        public string cod_usuario_registro { get; set; }
        public DateTime fch_cambio { get; set; }
        public string cod_usuario_cambio { get; set; }
    }

    public class eVacacionesGosadas
    {
        public string cod_vacacionDetalle { get; set; }
        public string cod_vacacionGeneral { get; set; }
        public DateTime fch_solicitud { get; set; }
        public DateTime fch_aprobacion { get; set; }
        public string flg_aprobado { get; set; }
        public string cod_usuario_aprobado { get; set; }
        public DateTime fch_inicio_vacacion { get; set; }
        public DateTime fch_fin_vacacion { get; set; }
        public int num_dias_goce { get; set; }
        public string dsc_observacion { get; set; }
        public DateTime fch_registro { get; set; }
        public string cod_usuario_registro { get; set; }
        public DateTime fch_cambio { get; set; }
        public string cod_usuario_cambio { get; set; }
    }
    public class eVacaciones_Listado : eVacaciones
    {
        public string dsc_trabajador { get; set; }
        public string dsc_empresa { get; set; }
        public string dsc_sede_empresa { get; set; }
        public string dsc_area { get; set; }
        public DateTime fch_ingreso { get; set; }
        public decimal num_total_30 { get; set; }
        public decimal num_total_22 { get; set; }
        public decimal num_total_15 { get; set; }
        public string cod_periodo { get; set; }
    }
    public class eVacacionesDetalleListado : eVacacionesGosadas
    {

    }
   

    public class eVacacionesAcumulado // eliminar
    {
        public string cod_vacacionGeneral { get; set; }
        public string cod_trabajador { get; set; }
        public DateTime fch_inicio { get; set; }
        public DateTime fch_final { get; set; }
        public decimal num_acumulado { get; set; }
            
    }

    public class eVacaciones_Solicitadas  // SI VA
    {
        public string cod_trabajador { get; set; }
        public string dsc_nombres { get; set; }
        public DateTime fch_inicio_vacacion { get; set; }
        public DateTime fch_fin_vacacion { get; set; }
        public int num_dias_goce { get; set; }
        public string flg_aprobado { get; set; }
        public string cod_periodo { get; set; }
        public string dsc_observacion { get; set; }
    }

    public class eVacaciones_Acumuladas // SI VA
    {
        public string cod_empresa { get; set; }
        public string cod_trabajador { get; set; }
        public string cod_periodo { get; set; }
        public int num_dias_gozadas { get; set; }
        public int num_dias_vendidas { get; set; }
        public int num_dias_program { get; set; }
        public decimal num_dias_pendientes { get; set; }
    }

    public class eVacaciones_Resumen // SI VA
    {
        public string cod_empresa { get; set; }
        public string cod_trabajador { get; set; }
        public string dsc_apellidos_nombres { get; set; }
        public DateTime fch_ingreso { get; set; }
        public decimal num_anhos { get; set; }
        public decimal vac_total_dias { get; set; }
        public int vac_total_pendientes { get; set; }
        public decimal vac_total_truncas { get; set; }
        public int vac_total_gozadas { get; set; }
        public int vac_total_programadas { get; set; }
        public int vac_total_vendidas { get; set; }
        public decimal saldo_total_dias { get; set; }
        public decimal saldo_total_pendientes { get; set; }
        public int factor { get; set; }
    }

    public class pVacaciones: pConsultaBase
    {
        //public pVacaciones()
        //{
        //    _fch_rango_inicial= DateTime.Now.Date;
        //    _fch_rango_final = DateTime.Now.Date;
        //}
        //private DateTime _fch_rango_inicial;
        //private DateTime _fch_rango_final;

        public string Cod_trabajador { get; set; }
        public string Cod_vacacionGeneral { get; set; }
        public string Cod_periodo { get; set; }
        public string Cod_empresa_multiple { get; set; }
        public string Cod_sede_multiple { get; set; }
        public string Cod_area_multiple { get; set; }
        public string Fch_rango_inicial { get; set; }
        public string Fch_rango_final { get; set; }

        //public DateTime Fch_rango_inicial { get { return _fch_rango_inicial; } set { _fch_rango_inicial = value; } }
        //public DateTime Fch_rango_final { get { return _fch_rango_final; } set { _fch_rango_final = value; } }
    }
    
}
