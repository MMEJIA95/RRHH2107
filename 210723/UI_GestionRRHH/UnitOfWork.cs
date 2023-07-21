using BL_GestionRRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI_GestionRRHH
{
    internal class UnitOfWork : blUnitOfWork { public UnitOfWork() : base(Program.Sesion.Acceso.Key) { } }
}
