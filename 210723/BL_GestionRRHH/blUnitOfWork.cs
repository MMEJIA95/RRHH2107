using DA_GestionRRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_GestionRRHH
{
    public class blUnitOfWork : IDisposable
    {
        daSQL _sql;
        public blUnitOfWork(string key)
        {
            _sql = new daSQL(key);

            Encripta = new blEncrypta(key);
            ConsultaSunat = new blConsultaSunat(key);
            Globales = new blGlobales(_sql, key);


            Clientes = new blClientes(_sql);
            Generales = new blGenerales(_sql);
            EmailingBoleta = new blEmailingBoleta(_sql);
            Factura = new blFactura(_sql);
            FormatoMDocumento = new blFormatoMDocumento(_sql);
            Proveedores = new blProveedores(_sql);
            Sistema = new blSistema(_sql);
            Trabajador = new blTrabajador(_sql);
            Usuario = new blUsuario(_sql);
            Vacaciones = new blVacaciones(_sql);
            VariableGeneral = new blVariableGeneral(_sql);
            Version = new blVersion(_sql);
            Ausencia = new blAusencia(_sql);
        }
        // Solo Llave
        public blEncrypta Encripta { get; private set; }
        public blConsultaSunat ConsultaSunat { get; private set; }

        // Llave y SQL
        public blGlobales Globales { get; private set; }


        // SQL
        public blGenerales Generales { get; private set; }
        public blClientes Clientes { get; private set; }
        public blEmailingBoleta EmailingBoleta { get; private set; }
        public blFactura Factura { get; private set; }
        public blFormatoMDocumento FormatoMDocumento { get; private set; }

        public blProveedores Proveedores { get; private set; }
        public blSistema Sistema { get; private set; }
        public blTrabajador Trabajador { get; private set; }
        public blUsuario Usuario { get; private set; }
        public blVacaciones Vacaciones { get; private set; }
        public blVariableGeneral VariableGeneral { get; private set; }
        public blVersion Version { get; private set; }
        public blAusencia Ausencia { get; private set; }

        public void Dispose()
        {
            // destruir instancia
        }
    }
}
