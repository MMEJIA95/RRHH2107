using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_GestionRRHH
{
    public class eFiltros
    {
        public string cod_empresa { get; set; }
        public string dsc_empresa { get; set; }
    }
    public class eFltEmpresaSede: eFiltros
    {
        public string cod_sede_empresa { get; set; }
        public string dsc_sede_empresa { get; set; }
    }
    public class eFltEmpresaSedeArea: eFltEmpresaSede
    {
        public string cod_area { get; set; }
        public string dsc_area { get; set; }
    } 
}
