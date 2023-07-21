using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_GestionRRHH.FormatoMD
{
    public class eTrabajadorDocumentoInfo
    {
        public string cod_trabajador { get; set; }
        public string dsc_empresa { get; set; }
        public string dsc_apellido_paterno { get; set; }
        public string dsc_apellido_materno { get; set; }
        public string dsc_nombres { get; set; }
        public string dsc_nombreCompleto { get; set; }
        public string fch_nacimiento { get; set; }
        public string dsc_direccion { get; set; }
        public string dsc_referencia { get; set; }
        public string dsc_distrito { get; set; }
        public string dsc_cargo { get; set; }
        public string dsc_email { get; set; }
        public string dsc_telefono { get; set; }
        public string dsc_celular { get; set; }
        public string dsc_documento { get; set; }
        public string dsc_fechaActual { get; set; }
        public string dsc_horaActual { get; set; }
        public string dsc_fecha_horaActual { get; set; }
    }
}
