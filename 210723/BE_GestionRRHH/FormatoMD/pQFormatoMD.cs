using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_GestionRRHH.FormatoMD
{
    public class pQFormatoMD
    {
        public pQFormatoMD()
        {
            _opcion = 0;
            _cod_usuario = string.Empty;
            _cod_estado = string.Empty;
            _cod_empresaSplit = string.Empty;
            _dsc_formatoMD_general = string.Empty;
            _cod_formatoMD_generalSplit = string.Empty;
            _cod_formatoMD_vinculoSplit = string.Empty;
            _cod_trabajadorSplit = string.Empty;
            _cod_formatoMD_seguimiento = string.Empty;
        }

        private int _opcion;
        private string _cod_usuario;
        private string _cod_estado;
        private string _cod_formatoMD_seguimiento;
        private string _cod_empresaSplit;
        private string _dsc_formatoMD_general;
        private string _cod_formatoMD_generalSplit;
        private string _cod_formatoMD_vinculoSplit;
        private string _cod_trabajadorSplit;

        public int Opcion { get => _opcion; set => _opcion = value; }
        public string Cod_usuario { get => _cod_usuario; set => _cod_usuario = value; }
        public string Cod_estado { get => _cod_estado; set => _cod_estado = value; }
        public string Cod_empresaSplit { get => _cod_empresaSplit; set => _cod_empresaSplit = value; }
        public string Dsc_formatoMD_general { get => _dsc_formatoMD_general; set => _dsc_formatoMD_general = value; }
        public string Cod_formatoMD_generalSplit { get => _cod_formatoMD_generalSplit; set => _cod_formatoMD_generalSplit = value; }
        public string Cod_formatoMD_vinculoSplit { get => _cod_formatoMD_vinculoSplit; set => _cod_formatoMD_vinculoSplit = value; }
        public string Cod_trabajadorSplit { get => _cod_trabajadorSplit; set => _cod_trabajadorSplit = value; }
        public string Cod_formatoMD_seguimiento { get => _cod_formatoMD_seguimiento; set => _cod_formatoMD_seguimiento = value; }
    }
}
