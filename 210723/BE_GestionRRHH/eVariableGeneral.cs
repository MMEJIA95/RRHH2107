using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_GestionRRHH
{
    public  class eVariableGeneral
    {
        public string cod_variable { get; set; }
        public string cod_correlativo { get; set; }
        public string cod_referencia { get; set; }
        public string dsc_variable { get; set; }
        public string dsc_valor { get; set; }
        public int num_valor { get; set; }
        public decimal dec_valor { get; set; }
        public string flg_valor { get; set; }
        public DateTime fch_valor { get; set; }
    }
    public class eTablaGeneral_List
    {
        public string cod_tabla { get; set; }
        public string dsc_descripcion { get; set; }
        public string dsc_abreviado { get; set; }
        public string cod_referencia { get; set; }
    }
}
