using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_GestionRRHH
{
    public class pConsultaBase
    {
        public pConsultaBase()
        {
            _opcion = 0;
            _cod_empresa = string.Empty; 
        }

        private int _opcion;
        private string _cod_empresa;

        public int Opcion { get => _opcion; set => _opcion = value; }
        public string Cod_empresa { get => _cod_empresa; set => _cod_empresa = value; }
    }
}
